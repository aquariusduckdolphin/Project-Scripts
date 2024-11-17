using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityObtained : MonoBehaviour
{

    public bool useAbility;

    public GameObject sphere;
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            useAbility = true;

            Destroy(sphere);

        }

    }

}
