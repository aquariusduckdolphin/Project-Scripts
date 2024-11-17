using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlockPlayer : MonoBehaviour
{

    public Animator block;

    public Transform point;

    public NavMeshAgent agent;

    public bool zombieMoves = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            zombieMoves = true;

            block.SetBool("Move", true);

        }

    }

    void Update()
    {

        if (zombieMoves)
        {

            //float dist = Vector3.Distance(this.transform.position, point.position);

            agent.SetDestination(point.position);
            
            block.SetBool("Move", false);

        }
        
    }

}
