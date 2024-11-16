using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MasterAI
{

    //Store the custom script
    private MasterAI masterAI;

    //A custom type of collection for the AI states
    public enum MentalState { Wander, Chase, CircleStrafe, Attack, Dead }

    //create a variable for the state
    public MentalState state;

    [Header("ZOMBIE AI INFO")]
    //A boolean for the zombie to move or not to move
    public bool canRoam = true;

    //A number for the raycast length
    public float raycastLength = 5f;

    //A number for how far the zombie will move away from other zombies
    public float csDirection = 5f;

    //A number outside of the zombies range
    public float maxRange = 20f;

    //A number to make the zombie chase forever
    public int count = 0;

    public LayerMask mask;

    //The various states that the Zombie can do
    void StateMachine(Transform stateMachineTarget)
    {

        switch (state)
        {

            case MentalState.Wander:
                Wander();
                break;

            case MentalState.Chase:
                Chase(stateMachineTarget);
                break;

            case MentalState.CircleStrafe:
                CircleStrafe(stateMachineTarget, csDirection);
                break;

            case MentalState.Attack:
                if (!source.isPlaying)
                {

                    source.clip = sound.clip[0];

                    sound.PlayOnce(sound.clip[0], sound.volume);

                }
                break;

            case MentalState.Dead:
                break;

        }

    }

    // Start is called before the first frame update
    private void Awake()
    {

        //Find the MasterAI script and store in the variable
        masterAI = GetComponent<MasterAI>();

        sound = GetComponent<Audio>();

        source = GetComponent<AudioSource>();

        //Find the player and store the transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Store the nav mesh agent component
        agent = GetComponent<NavMeshAgent>();

        //Store the animator component
        animator = GetComponent<Animator>();

        //Remove the way point from the zombie
        wayPoint.transform.parent = null;

        //Store the rigidbody
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        //Check to see if the health is above zero
        if (health > 0)
        {

            //Calculate the distance from the player and this game object
            float dist = Vector3.Distance(player.position, transform.position);

            /*if (dist <= source.maxDistance)
            {

                source.enabled = true;

            }
            else
            {

                source.enabled = false;

            }*/

            //Check to see if the distance from the player is less than the range
            if (dist < range)
            {

                //Set the mental state to attack
                state = MentalState.Attack;

                //Make the zombie watch the player
                Watch(player.transform);

                //Turn on the attack state
                animator.SetBool("CanAttack", true);

                //Turn off the walking state
                animator.SetBool("Running", false);

                animator.SetBool("LeftStrafe", false);

                animator.SetBool("RightStrafe", false);

            }

            //Check if the distance is greater then the range
            if (dist > range && !canRoam)
            {

                //Store the info of the hit object
                RaycastHit hit;

                //Check to see if there is a zombie in front of it 
                if (Physics.Raycast(hitBox.transform.position, transform.forward, out hit, raycastLength, mask))
                {

                    //Check to see if the parent is Zombie
                    if (hit.transform.root.tag == "Zombie")
                    {

                        //Send the player position into the state machine
                        StateMachine(player);

                        //Set the current state to circle strafe
                        state = MentalState.CircleStrafe;

                    }

                }

                //Send the player transform info 
                StateMachine(player);

                animator.SetBool("LeftStrafe", false);

                animator.SetBool("RightStrafe", false);

                if (wayPoint != null)
                {

                    Destroy(wayPoint);

                }

                //Set the mental state of the AI to chase the player
                state = MentalState.Chase;

            }
            //Check to see if the distance is greater than the range and they can roam
            else if (dist > range && canRoam)
            {

                //Set the animator idle state to off
                animator.SetBool("Idle", false);

                animator.SetBool("LeftStrafe", false);

                animator.SetBool("RightStrafe", false);

                //Send the player position to the state machine
                StateMachine(player);

                //Set the state to wander
                state = MentalState.Wander;

            }

            //Check to see if the distance is less than long range and distance is greater than range
            if (dist < maxRange && dist > range)
            {

                //Check if the count is zero
                if (count == 0)
                {

                    //Change teh boolean value
                    canRoam = !canRoam;

                    //Add one to count
                    count++;

                }

            }

        }
        else
        {

            //Set the state to be Dead
            state = MentalState.Dead;

        }
 
    }

    //Custom Funtion - move to the side
    private void CircleStrafe(Transform circleStrafeTarget, float direction)
    {

        //Call the custom function - watch
        Watch(circleStrafeTarget);

        //Get a random value and store it
        direction = Random.Range(-direction, direction);

        print("WHAT WHAT");

        if(Mathf.Sign(direction) > 0)
        {

            animator.SetBool("RightStrafe", true);

            animator.SetBool("CanAttack", false);

            //Turn off the walking state
            animator.SetBool("Running", false);

            animator.SetBool("Idle", false);

        }
        else if(Mathf.Sign(direction) < 0)
        {

            animator.SetBool("LeftStrafe", true);

            animator.SetBool("CanAttack", false);

            //Turn off the walking state
            animator.SetBool("Running", false);

            animator.SetBool("Idle", false);

        }

        //Mathematical function to move in a certain direction
        Vector3 value = transform.position + (direction * transform.right);

        if (!isDead)
        {

            //Tell the agent to move to the calculated value
            agent.SetDestination(value);

        }

    }

}