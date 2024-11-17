using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HordeMovement : MonoBehaviour
{

    private Animator animator;

    private NavMeshAgent agent;

    public GameObject move;

    //public ChaseZombie zombie;

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(animator != null && move != null)
        {

            agent.SetDestination(move.transform.position);

        }
        
    }
}
