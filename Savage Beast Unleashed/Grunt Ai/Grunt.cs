using System.Collections.Generic;
using TetraCreations.Attributes;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;

[RequireComponent(typeof(NavMeshAgent))]
public class Grunt : MonoBehaviour, IDamageable
{
    public enum MentalState { Default, Chase, Attack, Patrol, Death }

    public MentalState currentState = MentalState.Default;

    [Tooltip("Just for the IDamagable.")]
    public float damage = 5f;

    float IDamageable.damage => damage;

    public bool isDead = false;

    #region References Variables
    [Title("AI References", TitleColor.Aqua, TitleColor.Orange)]
    public float attackDist = 5f;

    private float dist;

    private NavMeshAgent agent;

    private Rigidbody rb;
    #endregion

    #region Watch Variables
    private Vector3 targetDirection;

    public float rotateSpeed = 5f;
    #endregion

    #region Info Variables
    [Title("Grunt Info", TitleColor.Aqua, TitleColor.Orange)]
    [Range(0, 1000)]
    public int maxHealth = 100;

    public float currentHealth;

    public float timeToDestroy = 2f;

    public float timeEnumerator = 0.5f;
    #endregion

    #region Animation Variables
    private Animator animator;

    public float timeForAnimation = 1f;

    private const string animAttack = "Attack";

    private const string animWalk = "Walk";

    private const string animIdle = "Idle";

    private const string animDeath = "Death";
    #endregion

    #region Data Varibles
    [Title("Player Data", TitleColor.Aqua, TitleColor.Orange)]
    private Transform player;
    #endregion

    #region Way Points Variables
    [Title("Way Points", TitleColor.Yellow, TitleColor.Orange)]
    [HideInInspector] public Transform[] wayPoints;

    [HideInInspector] private int currentWayPoint = 0;

    [HideInInspector] public float pointRadius = 2f;
    #endregion

    #region Material Properties
    public MaterialPropertyBlock glowMatBlock;

    public SkinnedMeshRenderer render;

    private const string matIntensity = "_Intensity";

    private const string matGlow = "_isGlowing";
    #endregion



    #region State Machine
    void StateMachine(Transform mentalStateTarget)
    {

        switch (currentState)
        {

            case MentalState.Default:
                break;

            case MentalState.Chase:
                animator.SetBool(animAttack, false);
                Chase(mentalStateTarget);
                break;

            case MentalState.Attack:
                Attack();
                break;

            case MentalState.Patrol:
                Patrol(mentalStateTarget);
                break;

            case MentalState.Death:
                StartCoroutine(Death(this.gameObject, timeToDestroy, timeForAnimation)); 
                break;

        }

    }
    #endregion

    #region Gather info
    void Start()
    {

        glowMatBlock = new MaterialPropertyBlock();

        //render = GetComponent<MeshRenderer>();

        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        //Keep the stopping distance the same as the dist for attack
        agent.stoppingDistance = attackDist;

        agent.updateRotation = false;

        StartCoroutine(SlowDown(timeEnumerator));

    }
    #endregion

    #region Update
    private void Update()
    {

        //Watch(player);

        //Debug.Log("Update");

        if (currentHealth <= 0)
        {

            currentState = MentalState.Death;

            StateMachine(this.gameObject.transform);

        }

    }
    #endregion

    #region Watch the Player
    private void FixedUpdate()
    {

        if (!isDead)
        {

            Watch(player);

        }

    }
    #endregion

    #region A slower Update
    //This handles all the taxing stuff
    IEnumerator SlowDown(float time)
    {

        //Wait for a couple of seconds
        yield return new WaitForSeconds(time);

        //Debug.Log("SLow Update");

        //Calculate the dist between the grunt & player
        dist = Vector3.Distance(transform.position, player.position);

        if(currentHealth > 0 )
        {

            //Check to see if dist is greater than attack dist
            if (dist > attackDist)
            {

                rb.isKinematic = false;

                //Set the current state to chase
                currentState = MentalState.Chase;

                //Pass the player in the state machine for chase
                StateMachine(player);

            }

            if (dist < attackDist)
            {

                //Go into the attack state
                Attack();

                StateMachine(player);

            }

            if(currentHealth <= 40f)
            {

                glowMatBlock.SetFloat(matIntensity, 1f);

                glowMatBlock.SetFloat(matGlow, 1f);

                render.SetPropertyBlock(glowMatBlock);

            }

        }

        //Calling itself
        StartCoroutine(SlowDown(time));

    }
    #endregion

    #region Chase position
    void Chase(Transform target)
    {

        if (!rb.isKinematic || !isDead)
        {

            currentState = MentalState.Chase;

            agent.SetDestination(target.position);

            SetAnimation(animWalk, animIdle, true);

        }

    }
    #endregion

    #region Attack State
    void Attack()
    {

        currentState = MentalState.Attack;

        rb.isKinematic = true;

        animator.SetBool(animAttack, true);

        SetAnimation(animAttack, animWalk, true);


        //Debug.Log("I have been hit.");

    }
    #endregion

    #region Patrol State
    // ***THIS IS A TESTING FEATURE ONLY***
    void Patrol(Transform target)
    {

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
    #endregion

    #region Damage State
    public void TakeDamage(float damage)
    {

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {

            currentState = MentalState.Death;

            StateMachine(this.gameObject.transform);
            
        }

    }
    #endregion

    #region Death State
    IEnumerator Death(GameObject obj, float timeForDestroy, float animationTimeDelay)
    {

        //Play Death Animation

        isDead = true;

        rb.isKinematic = true;

        glowMatBlock.SetFloat(matIntensity, 0f);

        glowMatBlock.SetFloat(matGlow, 0f);

        render.SetPropertyBlock(glowMatBlock);

        TurnOffAllAnimation(animAttack, animIdle, animWalk);

        animator.SetBool(animDeath, true);

        yield return new WaitForSeconds(animationTimeDelay);

        Destroy(obj, timeForDestroy);

    }
    #endregion

    #region Watch Target
    void Watch(Transform watchTarget)
    {

        targetDirection = watchTarget.position - transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0f));

    }
    #endregion

    #region Animation
    /// <summary>
    /// Sets the animations for the current action.
    /// </summary>
    /// <param name="animateName"> Animation that will play. </param>
    /// <param name="animateName2"> Animation that will not play. </param>
    /// <param name="setAnimation"> Set default to true. </param>
    void SetAnimation(string animateName, string animateName2, bool setAnimation)
    {

        animator.SetBool(animateName, setAnimation);

        animator.SetBool(animateName2, !setAnimation);

    }

    /// <summary>
    /// This will turn off all the animations. DEATH STATE ONLY.
    /// </summary>
    /// <param name="animateName"></param>
    /// <param name="animateName2"></param>
    /// <param name="animateName3"></param>
    void TurnOffAllAnimation(string animateName, string animateName2, string animateName3)
    {

        animator.SetBool(animateName, false);

        animator.SetBool(animateName2, false);

        animator.SetBool(animateName3, false);

    }
    #endregion

}