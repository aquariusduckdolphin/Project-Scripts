using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghoul : MasterAI
{

    private MasterAI masterAI;

    public enum MentalState { Wander, Chase, Attack, CircleStrafe, Dead }

    public MentalState state;

    [Header("AI INFO")]

    public float csDirection = 5f;

    public float maxRange = 20f;

    public float maxHealth = 100f;

    public float maxAppear = 10f;

    #region Material Properties
    [Header("Material Properties")]
    private MaterialPropertyBlock propertyBlock;

    private SkinnedMeshRenderer render;

    private string disappear = "_DissolveValue";
    #endregion

    #region State Machine
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
                CircleStrafe(player, csDirection);
                break;

            case MentalState.Attack:
                sound.PlayOnce(sound.clip[0], sound.volume);
                break;

            case MentalState.Dead:
                break;

        }

    }
    #endregion

    #region Gatehering Variables
    void Awake()
    {

        masterAI = GetComponent<MasterAI>();

        sound = GetComponent<Audio>();

        source = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        wayPoint.transform.parent = null;

        render = GetComponentInChildren<SkinnedMeshRenderer>();

        propertyBlock = new MaterialPropertyBlock();

        SetMaterial(-1.65f);

        health = maxHealth;

    }
    #endregion

    void Update()
    {

        if(health > 0)
        {

            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > range && dist < maxRange)
            {
                
                StateMachine(player);

                state = MentalState.Chase;

                //go invisible
                SetMaterial(0.8f);

                animator.SetBool("Running", true);

                animator.SetBool("CanAttack", false);

            }
            else if( dist < range)
            {

                state = MentalState.Attack;

                animator.SetBool("Running", false);

                //Animation for attack
                animator.SetBool("CanAttack", true);

                Watch(player);

            }
            else if( dist > range && dist > maxRange)
            {

                StateMachine(player);

                state = MentalState.Wander;

                animator.SetBool("Idle", true);

                animator.SetBool("Running", false);

                animator.SetBool("CanAttack", false);

            }

            if(dist < maxAppear)
            {

                //come out of invisiblity
                SetMaterial(-1.65f);

            }

        }
        else
        {

            state = MentalState.Dead;

        }
        
    }

    #region Circle Strafe Function
    void CircleStrafe(Transform strafeTarget, float direction)
    {

        Watch(strafeTarget);

        Vector3 value = transform.position + (direction * transform.right);

        Vector3 behindPlayer = -1 * strafeTarget.position;

        agent.SetDestination(value);

    }
    #endregion

    #region Setting Material
    void SetMaterial(float value)
    {

        propertyBlock.SetFloat(disappear, value);

        render.SetPropertyBlock(propertyBlock);

    }
    #endregion

}
