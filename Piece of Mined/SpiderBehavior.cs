using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderBehavior : MonoBehaviour
{

    public enum MentalState { Chase, Attack, Retreat, None }

    public MentalState mentalState = MentalState.None;

    public LayerMask mask;

    [Header("Ai Variables")]
    public float agentSpeed = 10f;
    
    private NavMeshAgent agent;

    [Header("References")]
    public Transform player;

    public Transform retreatPoint;

    [Range(0f, 20f)]
    public float sneakDelay = 10f;

    public float maxPlayerDist = 2f;

    public float maxRetreatDist = 10f;

    public bool isRetreating = false;

    [Header("Raycast")]
    public bool canAttack = true;

    public float attackStrength = 10f;

    public float minDistance = 10f;

    public float rotationSpeed = 10f;

    public Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        agent.speed = agentSpeed;

        mentalState = MentalState.Chase;

    }

    private void StateMachine(Transform stateMachineTarget)
    {

        switch (mentalState)
        {

            case MentalState.None:
                return;

            case MentalState.Chase:
                Chase(stateMachineTarget); 
                break;

            case MentalState.Attack:
                Attack();
                Watch();
                break;

            case MentalState.Retreat:
                Retreating(stateMachineTarget);
                break;

        }

    }

    void Update()
    {

        ChasePlayer();

        RetreatingToPoint();
        
    }

    private void ChasePlayer()
    {

        if (CalculateDistance(player, transform) > maxPlayerDist && !isRetreating)
        {

            mentalState = MentalState.Chase;

            StateMachine(player);

        }
        else if (CalculateDistance(player, transform) < maxPlayerDist && !isRetreating)
        {

            Chase(transform);

            mentalState = MentalState.Attack;

            StateMachine(player);

        }

    }

    private void RetreatingToPoint()
    {

        if (CalculateDistance(transform, retreatPoint) > maxRetreatDist && isRetreating)
        {

            mentalState = MentalState.Chase;

            StateMachine(retreatPoint);

        }
        else if (CalculateDistance(transform, retreatPoint) < maxRetreatDist && isRetreating)
        {

            mentalState = MentalState.Chase;

            StateMachine(transform);

            Invoke("DecideWhatToDo", sneakDelay);

        }

    }

    private void DecideWhatToDo()
    {

        isRetreating = false;

        mentalState = MentalState.Chase;

        StateMachine(player);

    }

    private void Chase(Transform chaseTarget)
    {

        agent.SetDestination(chaseTarget.position);

    }

    #region Attacking
    private void Attack()
    {

        RaycastHit hit;

        bool rayhit = Physics.Raycast(transform.position, transform.forward * minDistance, out hit, minDistance, mask);

        if (rayhit)
        {

            IDamagable damage = hit.transform.GetComponent<IDamagable>();

            if (damage != null && canAttack)
            {

                print("hitting");

                damage.TakeDamage(attackStrength);

                DelayAttack();

                Invoke("ResetAttack", 1f);

            }
            else
            {

                print("Failed");

            }

        }

    }

    private void ResetAttack()
    {

        canAttack = true;

    }

    private void DelayAttack()
    {

        canAttack = false;

    }
    #endregion

    private void Retreating(Transform retreatingTarget)
    {

        mentalState = MentalState.Retreat;

        Chase(retreatingTarget);

    }

    private float CalculateDistance(Transform firstTarget, Transform secondTarget)
    {

        return Vector3.Distance(firstTarget.position, secondTarget.position);

    }

    public void Watch()
    {

        Vector3 targetDir = player.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0f));

    }

}
