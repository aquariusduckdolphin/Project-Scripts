using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireOuterCheck : MonoBehaviour
{

    public bool outerCheck;

    private void OnTriggerEnter(Collider other)
    {

        //print("collider outer");

        if (other.transform.CompareTag("Player"))
        {

            outerCheck = !outerCheck;

            ////Debug.Log("Outer Check: " + outerCheck.ToString());

        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.transform.CompareTag("Player"))
        {

            outerCheck = !outerCheck;

            ////Debug.Log("Outer Check: " + outerCheck.ToString());

        }

    }

}
