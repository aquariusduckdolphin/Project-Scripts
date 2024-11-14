using System.Collections.Generic;
using TetraCreations.Attributes;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(NavMeshAgent))]
public class Vampire : MonoBehaviour
{

    public enum MentalState{ Default, Chase, Attack, Death }

    public MentalState currentState = MentalState.Default;

    [SerializeField] private float maxHealth = 100f;

    public float currentHealth = 0f;

    #region AI References
    [Title("AI", TitleColor.Aqua, TitleColor.Orange)]
    public float timeToDestroy = 0.5f;

    public float maxDistance = 30f;

    public float distanceToMove = -10;

    //Calculate the distance between it and the player
    private float dist;

    private Transform player;

    private NavMeshAgent agent;
    #endregion

    #region Teleportation Variables
    [Title("Teleport", TitleColor.Aqua, TitleColor.Orange)]
    public GameObject spit;

    public float teleportDist = -10f;

    public float slowUpdateTime = 0.5f;

    public float checkBeforeTeleport = 0f;

    [SerializeField] private Vector3 newPosition;

    private VampireInnerCheck inside;

    private VampireOuterCheck outer;
    #endregion

    #region Watch Variables
    public Vector3 targetDirection;

    public float rotationSpeed = 10f;
    #endregion

    #region Animation Variables
    public Animator anim;

    public float delayForDeath = 0.5f;

    private const string animIdle = "Idle";

    private const string animAttacking = "Attacking";

    private const string animDeath = "Death";
    #endregion



    #region State Machine
    void StateMachine(Transform stateMachineTarget)
    {

        switch(currentState)
        {

            case MentalState.Default:
                break;

            case MentalState.Chase:
                Chase(stateMachineTarget);
                break;

            case MentalState.Attack:
                StartCoroutine(CheckColliders(checkBeforeTeleport));
                Attack();
                break;

            case MentalState.Death:
                StartCoroutine(Death(stateMachineTarget, delayForDeath));
                break;

        }

    }
    #endregion
    
    #region Gather Info
    void Start()
    {

        currentHealth = maxHealth;

        inside = GetComponentInChildren<VampireInnerCheck>();

        outer = GetComponentInChildren<VampireOuterCheck>();

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.stoppingDistance = maxDistance;

        agent.updateRotation = false;

        StartCoroutine(SlowUpdate(slowUpdateTime));

    }
    #endregion

    #region Bot Look At Player
    void FixedUpdate()
    {

        Watch(player);

    }
    #endregion

    #region Slow Update
    IEnumerator SlowUpdate(float time)
    {

        yield return new WaitForSeconds(time);

        StateMachine(player);

        //Calculate the distance between this object and player
        dist = Vector3.Distance(transform.position, player.position);

        //Set the current state depending on the distance
        if(dist > maxDistance)
        {

            currentState = MentalState.Chase;

            StateMachine(player);

        }
        else if(dist <= maxDistance)
        {

            currentState = MentalState.Attack;

            StateMachine(player);

        }

        //Call itself
        StartCoroutine(SlowUpdate(time));

    }
    #endregion

    #region Chase Function
    //Make the AI go towards the target
    void Chase(Transform target)
    {

        agent.SetDestination(target.position);

    }
    #endregion

    #region Teleportation Function
    private IEnumerator CheckColliders(float time)
    {

        //Debug.Log("First Check");

        if (!inside.innerCheck && outer.outerCheck)
        {

            //Debug.Log("Why is it not chceking");

            yield return new WaitForSeconds(time);

            bool canTP = SecondCheck();

            if (!canTP)
            {

                Teleport(player);

            }

        }

        yield return new WaitForSeconds(time);

    }
    
    private bool SecondCheck()
    {

        //Debug.Log("Check Two");

        //Check to see if the player entered both triggers
        if (inside.innerCheck && outer.outerCheck)
        {

            return true;

        }

        return false;

    }

    //Make the vampire teleport
    void Teleport(Transform player)
    {

        newPosition = (-player.forward * Mathf.Abs(teleportDist)) + player.position;

        transform.position = newPosition;

        Chase(transform);

    }
    #endregion

    #region Attack Function
    void Attack()
    {
        
        currentState = MentalState.Attack;

        SetAnimation(anim, animAttacking, animIdle, true);

        //Spawn projectile
        Instantiate(spit, (transform.forward * 2f) + transform.position, Quaternion.identity);

        //Debug.Log("Attacking you!");

    }
    #endregion

    #region Watch Target
    void Watch(Transform watchTarget)
    {

        targetDirection = watchTarget.position - transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f));

    }
    #endregion

    #region Death State
    IEnumerator Death(Transform vampire, float time)
    {

        currentState = MentalState.Death;

        StateMachine(this.transform);

        TurnOffAnimations(anim, animIdle, animAttacking);

        anim.SetBool(animDeath, true);

        yield return new WaitForSeconds(time);

        Destroy(vampire.parent.gameObject);

    }
    #endregion


    /// <summary>
    /// 
    /// </summary>
    /// <param name="anime"> The animation controller </param>
    /// <param name="animation1"> First animation boolean. Will be set to true. </param>
    /// <param name="animation2"> Second animaiton boolean. Will be set to false. </param>
    /// <param name="setAnimation"> Boolean for  </param>
    void SetAnimation(Animator anime, string animation1, string animation2, bool setAnimation)
    {

        anime.SetBool(animation1, setAnimation);

        anime.SetBool(animation2, !setAnimation);

    }

    void TurnOffAnimations(Animator anime, string animation1, string animation2)
    {

        anime.SetBool(animation1, false);

        anime.SetBool(animation2, false);

    }

}
