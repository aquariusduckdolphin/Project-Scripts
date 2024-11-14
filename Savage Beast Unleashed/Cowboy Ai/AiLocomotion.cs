using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{

    public NavMeshAgent agent;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (agent.hasPath)
        {

            animator.SetFloat("Speed", agent.velocity.magnitude);

        }
        else
        {

            animator.SetFloat("Speed", 0);

        }
        
    }
}
