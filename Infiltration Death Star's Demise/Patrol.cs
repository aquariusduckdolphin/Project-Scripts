using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{

    public Transform[] wayPoints;

    public int currentWayPoints = 0;

    public float radius = 1f;

    public float delayTime = 1f;

    public NavMeshAgent agent;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(PatrolPoints(delayTime));

    }

    private void Chase(Transform chaseTarget)
    {

        agent.destination = chaseTarget.position;

    }

    private IEnumerator PatrolPoints(float time)
    {

        yield return new WaitForSeconds(time);

        float distance = Vector3.Distance(transform.position, wayPoints[currentWayPoints].position);

        if(distance > radius)
        {

            Chase(wayPoints[currentWayPoints].transform);

        }
        
        if (distance < radius)
        {

            currentWayPoints++;

        }

        if (currentWayPoints >= wayPoints.Length)
        {

            currentWayPoints = 0;

        }

        yield return new WaitForSeconds(time);

        StartCoroutine(PatrolPoints(time));

    }

}
