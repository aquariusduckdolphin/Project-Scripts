using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mutant : MasterAI
{

    //Private variable to store the masterai script
    private MasterAI masterai;

    //Enum for the various Mutant behaviors
    public enum MentalState { Wander, Chase, Attack, Dead }

    //Variable that will set the Mental State
    public MentalState state;

    //Boolean to make sure that the AI can move around the world
    public bool canRoam = true;

    //A max range
    public float maxRange = 20f;

    //A variable to go from idle to 
    private int count = 0;

    public float maxHealth = 100f;

    public HealthBar healthBar;

    public Text name;

    public GameObject healthIcon;

    public bool isBoss = false;

    public bool playSound = false;

    void StateMachine(Transform stateMachineTarget)
    {

        switch (state)
        {

            case MentalState.Wander:
                if (source.isPlaying)
                {

                    source.Stop();

                }
                Wander();
                break;

            case MentalState.Chase:
                if (source.isPlaying)
                {

                    source.Stop();

                }
                Chase(stateMachineTarget);
                break;

            case MentalState.Attack:
                if(!source.isPlaying)
                {

                    StartCoroutine(DelaySound(sound.clip[0], sound.volume, 0.5f));

                }
                break;

            case MentalState.Dead:
                break;

        }

    }

    // Start is called before the first frame update
    void Awake()
    {

        masterai = GetComponent<MasterAI>();

        sound = GetComponent<Audio>();

        source = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        wayPoint.transform.parent = null;

        health = maxHealth;

        if(isBoss == true)
        {

            healthBar = GameObject.Find("Boss Health Bar").GetComponent<HealthBar>();

            healthIcon = GameObject.Find("Boss Health Bar");

            healthBar.SetHealthBar(health);

        }

    }

    // Update is called once per frame
    void Update()
    {

        if(isBoss == true)
        {

            healthBar.HealthDisplay(health);

        }

        //Check to see if the health is above zero
        if (health > 0)
        {

            //Calculate the distance from the player and this game object
            float dist = Vector3.Distance(player.position, transform.position);

            /*if(dist <= source.maxDistance)
            {

                source.enabled = true;

            }
            else
            {

                source.enabled = false;

            }*/

            //Check to see if the distance is less than long range and distance is greater than range
            if (dist < maxRange && dist > range)
            {

                //Check if the count is zero
                if (count == 0)
                {

                    //Change teh boolean value
                    canRoam = !canRoam;

                    count++;

                }

            }

            //Check to see if the distance from the player is less than the range
            if (dist < range)
            {

                state = MentalState.Attack;

                Watch(player.transform);

                //Turn on the attack state
                animator.SetBool("CanAttack", true);

                //Turn off the walking state
                animator.SetBool("Running", false);

            }
            //Check if the distance is greater then the range
            else if (dist > range && !canRoam)
            {

                //Send the player transform info 
                StateMachine(player);

                //Set the mental state of the AI to chase the player
                state = MentalState.Chase;

            }
            //Check to see if the distance is greater than the range and they can roam
            else if (dist > range && canRoam)
            {

                //Set the animator idle state to off
                animator.SetBool("Idle", false);

                //Send the player position to the state machine
                StateMachine(player);

                //Set the state to wander
                state = MentalState.Wander;

            }

        }
        else
        {

            //Set the state to be Dead
            state = MentalState.Dead;

            if (healthIcon != null && name != null)
            {

                healthIcon.SetActive(false);

                name.enabled = false;

            }

            Destroy(wayPoint);

        }

    }

    public IEnumerator DelaySound(AudioClip effect, float loudness, float timeDelay)
    {

        source.PlayOneShot(effect, loudness);

        yield return new WaitForSeconds(timeDelay);

    }

}
