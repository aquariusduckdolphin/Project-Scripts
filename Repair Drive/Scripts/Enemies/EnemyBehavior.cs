using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{

    public enum MentalState { Idle, Patrol, Chase, Attack }

    public MentalState currentState = MentalState.Idle;

    [Header("References")]

    public ManageScenes manager;

    public NavMeshAgent agent;

    public Transform player;

    public float maxDistanceBetweenPlayer = 20f;

    public float distanceBetweenPlayer = 10f;

    public float slowUpdateDelay = 1f;

    [Space(10)]
    [Header("Patrol Variables")]
    public Transform[] wayPoints;

    [Range(0.1f, 25f)]
    [SerializeField] private float pointRadius = 10f;

    [SerializeField] private int currentWayPoint = 0;

    [Header("Attack References")]
    public GameObject bullet;

    public GameObject bulletLocation;

    public float delayForBullets = 1f;

    [Header("Animation References")]
    public Animator anim;

    void StateMachine(Transform statemachineTarget)
    {

        switch(currentState)
        {

            case MentalState.Idle:
                break;

            case MentalState.Patrol:
                Patrol(statemachineTarget);
                break;

            case MentalState.Chase:
                Chase(statemachineTarget);
                break;

            case MentalState.Attack:
                StartCoroutine(Attack(delayForBullets));
                break;

        }

    }

    void Start()
    {

        manager = GameObject.Find("Scene Manager").GetComponent<ManageScenes>();

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").GetComponent<Transform>();

        StartCoroutine(SlowUpdate(slowUpdateDelay));

    }

    private void FixedUpdate()
    {
        
        if(currentState == MentalState.Attack)
        {

            LookAtPlayer();

        }

    }

    IEnumerator SlowUpdate(float timeDelay)
    {

        if (manager.timerRunning)
        {

            float playerDistance = Vector3.Distance(transform.position, player.position);

            if (playerDistance > maxDistanceBetweenPlayer && playerDistance > distanceBetweenPlayer)
            {

                currentState = MentalState.Idle;

                //StateMachine(wayPoints[currentWayPoint]);

            }
            else if (playerDistance < maxDistanceBetweenPlayer && playerDistance > distanceBetweenPlayer)
            {

                currentState = MentalState.Chase;

                StateMachine(player);

            }
            else if(playerDistance < maxDistanceBetweenPlayer && playerDistance < distanceBetweenPlayer)
            {


                currentState = MentalState.Attack;

                StateMachine(player);

            }

        }

        yield return new WaitForSeconds(timeDelay);

        StartCoroutine(SlowUpdate(timeDelay));

    }

    void Chase(Transform chaseTarget)
    {

        currentState = MentalState.Chase;

        //agent.SetDestination(target.position);

        agent.destination = chaseTarget.position;

    }

    void Patrol(Transform patrolTarget)
    {

        currentState = MentalState.Patrol;

        float distance = Vector3.Distance(transform.position, wayPoints[currentWayPoint].position);

        if (distance > pointRadius)
        {

            Chase(wayPoints[currentWayPoint]);

        }

        if (distance < pointRadius)
        {

            currentWayPoint++;

        }

        if (currentWayPoint >= wayPoints.Length)
        {

            currentWayPoint = 0;

        }

    }

    IEnumerator Attack(float time)
    {

        GameObject bullets = Instantiate(bullet, bulletLocation.transform.position, Quaternion.identity);

        BulletForward bulleting = bullets.GetComponent<BulletForward>();

        bulleting.bulletPoint = gameObject.transform.GetChild(1).gameObject;

        yield return new WaitForSeconds(time);

        StartCoroutine(Attack(time));

    }

    void LookAtPlayer()
    {

        transform.LookAt(player);

    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere(transform.position, maxDistanceBetweenPlayer);

        Gizmos.DrawWireSphere(transform.position, distanceBetweenPlayer);

    }

}
