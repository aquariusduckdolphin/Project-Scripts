using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Demon : MasterAI
{

    private MasterAI masterAI;

    public enum MentalState { Chase, CloseRange, Invincibility, What, LongRange, Dead }

    public MentalState state;

    [Header("Demon AI INFO")]

    public bool move = true;

    public bool invincibility = false;

    public bool invincibilityCheck = false;

    public float maxRange = 20f;

    public float maxHealth = 100f;

    public int count = 0;

    public float time = 0.5f;

    public float dist;

    public float invincibiityTime = 5f;

    public Material bossMat;

    public HealthBar healthBar;

    public GameObject center;

    // projectile stuff
    public GameObject projectileAttack;
    private float timeBtwShots;
    public float startTimeBtwShots;

    void StateMachine(Transform stateMachineTarget)
    {

        switch (state)
        {

            case MentalState.Chase:
                Chase(stateMachineTarget);
                break;

            case MentalState.CloseRange:
                CloseRange();
                break;

            case MentalState.LongRange:
                LongRange();
                break;

            case MentalState.Invincibility:
                StartCoroutine(Invincibility(time));
                break;

            case MentalState.What:
                break;

            case MentalState.Dead:
                break;

        }

    }

    // Start is called before the first frame update
    void Awake()
    {

        sound = GetComponent<Audio>();

        source = GetComponent<AudioSource>();

        //Find the MasterAI script and store in the variable
        masterAI = GetComponent<MasterAI>();

        //Find the player and store the transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Store the nav mesh agent component
        agent = GetComponent<NavMeshAgent>();

        //Store the animator component
        animator = GetComponent<Animator>();

        rb = transform.root.GetComponent<Rigidbody>();

        health = maxHealth;

        healthBar = GameObject.Find("Boss Health Bar").GetComponent<HealthBar>();

        if(healthBar != null)
        {

            healthBar.SetHealthBar(health);

        }

        bossMat.SetFloat("_Invincibility", 132f);

        timeBtwShots = startTimeBtwShots;

    }

    // Update is called once per frame
    void Update()
    {

        if(healthBar != null)
        {

            healthBar.HealthDisplay(health);

        }

        if(health > 0)
        {
            
            if( health <= maxHealth / 2 && !invincibility && !invincibilityCheck)
            {

                state = MentalState.Invincibility;

            }

            dist = Vector3.Distance(player.position, transform.position);

            if(dist < range)
            {

                StateMachine(player);

                state = MentalState.CloseRange;

                move = true;

            }
            else if( dist < maxRange && dist > range)
            {

                StateMachine(player);

                state = MentalState.Chase;

                move = true;

            }
            else if (dist > maxRange)
            {

                if (move == true)
                {

                    StateMachine(player);

                    state = MentalState.Chase;

                    move = false;

                }

                StateMachine(player);

                state = MentalState.LongRange;

                print("Long Range");

            }
            else
            {

                StateMachine(player);

                state = MentalState.Chase;

            }

        }
        else
        {

            state = MentalState.Dead;

        }

    }

    void CloseRange()
    {

        Watch(player);

        animator.SetBool("CanAttack", true);

        if (!source.isPlaying)
        {

            source.clip = sound.clip[0];

            sound.PlayOnce(sound.clip[0], sound.volume);

        }

        animator.SetBool("Running", false);

    }

    void LongRange()
    {

        animator.SetBool("LongRange", true);

        animator.SetBool("CanAttack", false);

        animator.SetBool("Running", false);
        
        Watch(player);
        
        //Instantiate(effect, transform.position, transform.rotation);
        
        print("Chase Now");
        
        move = false;

        if (timeBtwShots < -0)
        {
            Instantiate(projectileAttack, center.transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }

        else
        {
            timeBtwShots -= Time.deltaTime;
        }

    }

    IEnumerator Invincibility(float time)
    {

        invincibility = true;

        animator.SetBool("Roar", true);

        animator.SetBool("Running", false);

        animator.SetBool("LongRange", false);

        animator.SetBool("CanAttack", false);

        bossMat.SetFloat("_Invincibility", 4f);

        agent.Stop();

        yield return new WaitForSeconds(time);

        agent.Resume();

        animator.SetBool("Roar", false);

        print("Invincibility");

        StartCoroutine(CanDamage(invincibiityTime));

    }

    public IEnumerator CanDamage(float time)
    {

        yield return new WaitForSeconds(time);

        bossMat.SetFloat("_Invincibility", 132f);

        invincibility = false;

        invincibilityCheck = true;


    }

}
