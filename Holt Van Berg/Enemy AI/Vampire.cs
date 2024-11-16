using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vampire : MasterAI
{

    //Store the custom script
    public MasterAI masterAI;

    //Have the various AI states
    public enum MentalState { Wander, Chase, Flee, Attack, Death }

    //A variable to set the MentalState
    public MentalState state;

    public bool isIdle;

    public float maxHealth = 100f;

    public float maxRange = 32f;

    public bool idle = false;

    // projectile stuff
    public GameObject bloodAttack;
    private float timeBtwShots;
    public float startTimeBtwShots;

    //State Machine for the AIlogic
    void StateMachine(Transform stateMachineTarget)
    {

        //A switch statement to change the AI mental state
        switch (state)
        {

            case MentalState.Wander:
                Wander();
                break;

            //Case 1 - if the mental state is Chase, do this
            case MentalState.Chase:
                //Call the Chase Function and pass the state machine parameter
                Chase(stateMachineTarget);
                break;

            //Case 2 - if the mental state is Flee, do this
            case MentalState.Flee:
                //Call the Flee function and pass the state machine parameter
                Flee(stateMachineTarget);
                break;

            case MentalState.Attack:
                break;

        }

    }

    void Awake()
    {

        masterAI = GetComponent<MasterAI>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();

        //animator = GetComponent<Animator>();

        rb = transform.root.GetComponent<Rigidbody>();

        wayPoint.transform.parent = null;

        health = maxHealth;

        timeBtwShots = startTimeBtwShots;

    }

    void Update()
    {

        if (idle)
        {

            StateMachine(wayPoint.transform);

            state = MentalState.Chase;

        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (health > 0)
        {

            if(dist < range)
            {

                StateMachine(player);

                state = MentalState.Flee;

            }
            else if(dist > range && dist < maxRange)
            {
                Watch(player);

                if (timeBtwShots <-0)
                {
                    Instantiate(bloodAttack, transform.position, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }

                else
                {
                    timeBtwShots -= Time.deltaTime;
                }

                state = MentalState.Attack;

            }
            else if(dist < range && dist < maxRange)
            {

                StateMachine(this.transform);

                state = MentalState.Wander;

            }

        }
        else
        {

            state = MentalState.Death;

        }

    }

    //Custom Funtion - Makes the AI run in the opposite direction of the player
    public void Flee(Transform fleeTarget)
    {

        Watch(player);

        //Calculate the opposite direction from the player
        Vector3 fleeDir = -1f * (fleeTarget.position - transform.position);

        //Tell the nav mesh to move in the opposite direction of the player
        agent.SetDestination(transform.position + fleeDir);

    }

}
