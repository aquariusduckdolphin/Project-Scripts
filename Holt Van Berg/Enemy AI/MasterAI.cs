using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MasterAI : MonoBehaviour
{

    [Header("AI NUMBER INFO")]
    //Store a value for the health
    public float health;

    //A variable for the range of the AI
    public float range;

    //A variable to easily change the wait timer
    private float delay = 5f;

    //A variable for the rotation of the AI
    public float rotateSpeed = 10f;

    public float hitOffDelay = 0.5f;

    [Header("BOOLEANS")]
    //A boolean for to tell if the AI is dead or alive
    public bool isDead = false;

    [Header("AI INFO")]
    //Store the nav mesh agent
    public NavMeshAgent agent;

    //Store the animator controller
    public Animator animator;

    //Store the player's transform component
    public Transform player;

    //Store the hit box AKA colliders
    public GameObject hitBox;

    //Store the Rigidbody of the enemy
    public Rigidbody rb;

    //Store a vector
    private Vector3 targetDir;

    [Header("WAYPOINT")]
    //A list to store various points for the AI to go towards
    public Transform[] patrolPoints;

    //Store the intial way point
    public GameObject wayPoint;

    //Set the current patrol point to zero
    public int currentPoint = 0;

    //A float for how close to the point the AI has to go
    public float pointRadius = 1f;

    public Vector3 goingTo;

    public Transform tracking;

    public float chaseTimeRun = 0f;

    public AudioSource source;

    public Audio sound;

    //Custom Funtion - Take in a integer and have the new health value
    public void RemoveHealth(float damage)
    {

        //Take the damage and subtract health, then set it to health
        health -= damage;

        //Set the animators hit boolean to be true
        animator.SetBool("Hit", true);

        //Call the custom function
        StartCoroutine(HitOff());

        //Check to see if the health is less than or equal to zero
        if (health <= 0)
        {

                //A custom function for the AI death
                StartCoroutine(Death(delay));

        }

    }

    //Custom Funtion --
    IEnumerator HitOff()
    {

        //Add a delay
        yield return new WaitForSeconds(hitOffDelay);

        //Set the animator hit variable to be false;
        animator.SetBool("Hit", false);

    }

    //Custom Function for the Death logic of the AI
    public IEnumerator Death(float time)
    {
        //Turn the walking state off
        animator.SetBool("Running", false);
        
        //Turn the attacking state off
        animator.SetBool("CanAttack", false);

        //Turn the is kinematic back on the enemies
        rb.isKinematic = true;

        //Turn on the death animation
        animator.SetBool("Death", true);

        //sound.PlayOnce(sound.clip[1], sound.volume);

        //Turn the bool to say is dead
        isDead = true;

        //Turn off the nav mesh agent so it does not move while on the ground
        agent.enabled = false;
        
        //Turn off the hit box of the AI so the player does not get caught on the body
        hitBox.SetActive(false);

        if (wayPoint != null)
        {

            Destroy(wayPoint);

        }

        //Waits for the time duration
        yield return new WaitForSeconds(time);

        animator.SetBool("Death", false);

        //Remove the dead AI body from the scene
        Destroy(gameObject);

    }

    //Custom Funtion - Get the AI to move to a position
    public void Chase(Transform targetPos)
    {

        tracking = targetPos;

        //Custom Function
        Watch(targetPos);

        goingTo = targetPos.position;

        //Check to see if the AI is alive
        if (!isDead)
        {

            //Make the nav mesh to move towards the targeted position
            agent.SetDestination(targetPos.position + Vector3.up);

            chaseTimeRun = Time.time;

        }
        
        //Turn the walking state on
        animator.SetBool("Running", true);

        //Turn off the attacking state
        animator.SetBool("CanAttack", false);

        animator.SetBool("Idle", false);


    }

    //Custom Function - Get the AI to move around the world
    public void Wander()
    {

        //Store the distance from this object to the current way point
        float dist = Vector3.Distance(transform.position, patrolPoints[currentPoint].position);

        if(dist > pointRadius)
        {

            //Calls the custom function - Chase
            Chase(patrolPoints[currentPoint]);

        }

        //Check if the distance is less than the radius
        if (dist < pointRadius)
        {

            //Set the animation for idle on
            animator.SetBool("Idle", true);

            //Set the animation for walking off
            animator.SetBool("Running", false);

            //Add one to the current point variable
            currentPoint++;

        }

        //Check if the current point is greater than or equal to the length of the array
        if (currentPoint >= patrolPoints.Length)
        {

            //Set the current point back to zero
            currentPoint = 0;

        }

    }

    //Custom Function to keep the AI looking towards the player at all times
    public void Watch(Transform watchTarget)
    {

        //Subtract the target's position and this objects position
        targetDir = watchTarget.transform.position - transform.position;

        //Calculate the rotation for this object
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, rotateSpeed * Time.deltaTime, 0f));

    }

}
