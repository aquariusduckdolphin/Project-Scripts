using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TetraCreations.Attributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class CowboyAi : MonoBehaviour
{

    public bool isDead = false;

    public bool haveCover = false;

    public enum MentalState { Chase, Cover, Shooting, Death }

    public MentalState currentState;

    #region References
    [Title("References", TitleColor.Aqua, TitleColor.Orange)]
    public float maxDistance = 30f;

    public float slowUpdateTime = 0.5f;

    //This will calculate the distance between it and the target
    private float dist;

    private NavMeshAgent agent;

    [SerializeField] private Transform player;
    #endregion

    #region Shooting Variables
    [Title("Shooting", TitleColor.Aqua, TitleColor.Orange)]
    public Vector3 bulletOffsets;

    public GameObject bullet;

    public Transform bulletSpawnLocation;
    #endregion

    #region Watch Variables
    [Title("Watch Function", TitleColor.Aqua, TitleColor.Orange)]
    public float rotateSpeed = 10f;

    private Vector3 targetDir;
    #endregion

    #region Cover Variables
    [Title("Cover", TitleColor.Aqua, TitleColor.Orange)]
    public GameObject[] coverPositions;

    public Transform bestPosition;

    Transform cover;
    #endregion

    #region
    public HumanHealth health;

    public float kill = 40f;

    public MaterialPropertyBlock glowMaterialBlock;

    public SkinnedMeshRenderer render;

    public Material[] materials;

    private const string matIntensity = "_Intensity";

    private const string matGlow = "_isGlowing";
    #endregion

    public TwoBoneIKConstraint ikConstraint;

    public TwoBoneIKConstraint ikConstraint2;

    public Animator anim;

    #region State Machine
    void StateMachine(Transform stateMachineTarget)
    {

        switch(currentState)
        {

            case MentalState.Chase:
                Chase(stateMachineTarget);
                Watch(player);
                break;

            case MentalState.Cover:
                NearestCoverPoint(coverPositions);
                Watch(player);
                break;

            case MentalState.Shooting:
                Shooting();
                break;

            case MentalState.Death:
                Death();
                break;

        }

    }
    #endregion

    #region Gathering Info
    void Start()
    {

        glowMaterialBlock = new MaterialPropertyBlock();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        health = GetComponent<HumanHealth>();

        agent = GetComponent<NavMeshAgent>();

        coverPositions = GameObject.FindGameObjectsWithTag("Cover Point");

        StartCoroutine(SlowUpdate(slowUpdateTime));

    }
    #endregion

    #region Watch Function
    private void FixedUpdate()
    {

        if (!isDead)
        {

            Watch(player);

        }

    }
    #endregion

    #region Slow Update
    IEnumerator SlowUpdate(float time)
    {

        yield return new WaitForSeconds(time);

        //StateMachine(player);

        dist = Vector3.Distance(transform.position, player.position);

        if (dist > maxDistance)
        {

            if(coverPositions != null && !haveCover)
            {

                Transform bestPosition = NearestCoverPoint(coverPositions);

                /*currentState = MentalState.Chase;

                StateMachine(bestPosition);*/

                //Debug.Log(bestPosition.name);

                if (transform.position != bestPosition.position)
                {

                    currentState = MentalState.Chase;

                    StateMachine(bestPosition);

                }

            }

        }

        if (dist < maxDistance)
        {

            currentState = MentalState.Shooting;

            StateMachine(player);

        }

        if(health.currentHealth <= kill)
        {

            glowMaterialBlock.SetFloat(matIntensity, 1f);

            glowMaterialBlock.SetFloat(matGlow, 1f);

            render.SetPropertyBlock(glowMaterialBlock);

        }

        StartCoroutine(SlowUpdate(time));

    }
    #endregion

    #region Chase Function
    void Chase(Transform target)
    {

        currentState = MentalState.Chase;

        agent.SetDestination(target.position);

    }
    #endregion

    #region Shooting Function
    void Shooting()
    {

        currentState = MentalState.Shooting;

        Vector3 bulletLocation;

        bulletLocation.x = bulletSpawnLocation.position.x + Random.Range(0, 1f);

        bulletLocation.y = bulletSpawnLocation.position.y;

        bulletLocation.z = bulletSpawnLocation.position.z + Random.Range(0, 1f);

        Instantiate(bullet, bulletLocation, Quaternion.identity);

    }
    #endregion

    #region Cover Function
    Transform NearestCoverPoint(GameObject[] points)
    {

        float nearestCover = Mathf.Infinity;

        CoverPoint coverPoint;

        for(int i = 0; i < points.Length; i++)
        {

            float distance = Vector3.Distance(transform.position, points[i].transform.position);

            coverPoint = points[i].GetComponent<CoverPoint>();

            //Debug.Log(coverPoint);

            if (distance < nearestCover && !coverPoint.isFull)
            {

                bestPosition = points[i].transform;

                nearestCover = distance;

            }

        }

        return bestPosition;

    }
    #endregion

    #region Death State
    void Death()
    {

        ikConstraint.weight = 0f;

        ikConstraint2.weight = 0f;

        anim.SetBool("Death", true);

        currentState = MentalState.Death;

        Destroy(gameObject);

    }
    #endregion

    #region Look at the Target
    void Watch(Transform watchTarget)
    {

        targetDir = watchTarget.position - transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, rotateSpeed * Time.deltaTime, 0f));

    }
    #endregion

}
