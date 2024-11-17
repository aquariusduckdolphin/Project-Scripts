using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChasePlayer : MonoBehaviour
{

    private Animator animator;

    private NavMeshAgent agent;

    public GameObject playerPos;

    public ZombieWakeUp zombie;

    public bool run = false;

    void Start()
    {

        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        playerPos = GameObject.FindGameObjectWithTag("Player");

        zombie = GameObject.FindGameObjectWithTag("Trigger").GetComponent<ZombieWakeUp>();

    }

    void Update()
    {

        if (animator != null && playerPos != null)
        {

            if (zombie.chase)
            {

                agent.SetDestination(playerPos.transform.position);

            }

        }

    }

}
