using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZombie : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.transform.tag == "Bot")
        {

            Destroy(other.gameObject);

        }

    }

}
