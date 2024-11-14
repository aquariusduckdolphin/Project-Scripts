using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireInnerCheck : MonoBehaviour
{

    public bool innerCheck;

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.CompareTag("Player"))
        {

            innerCheck = true;

            ////Debug.Log("Inner Check: " + innerCheck.ToString());

        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.transform.CompareTag("Player"))
        {

            innerCheck = false;

            ////Debug.Log("Inner Check: " + innerCheck.ToString());

        }

    }


}
