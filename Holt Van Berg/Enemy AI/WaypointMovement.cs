using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{

    public Vector3 minRange;

    public Vector3 maxRange;

    public float initialDelay = 0.1f;

    public float delay = 10f;

    // Update is called once per frame
    void Start()
    {

        //Calls the custom function
        StartCoroutine(NewLocation(initialDelay));
        
    }

    //Custom Function - give the game object a new location
    IEnumerator NewLocation(float time)
    {

        //Set a delay
        yield return new WaitForSeconds(time);

        //Store the info of the hit object
        RaycastHit hit;

        //Check to see if the raycast hits anything with the tag ground
        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f) && hit.transform.tag == "Ground")
        {

            //Store a random value
            float xPos = Random.Range(minRange.x, maxRange.x);

            //Store a random value
            float zPos = Random.Range(minRange.z, maxRange.z);

            //Set this object to the new location
            transform.position = new Vector3(xPos, 1f, zPos);

            //call itself
            StartCoroutine(NewLocation(delay));

        }
        else
        {

            //Store a random value
            float xPos = Random.Range(minRange.x, maxRange.x);

            //Store a random value
            float zPos = Random.Range(minRange.z, maxRange.z);

            //Set this object to the new location
            transform.position = new Vector3(xPos, 1f, zPos);

            //call itself
            StartCoroutine(NewLocation(0.01f));

        }


    }

}
