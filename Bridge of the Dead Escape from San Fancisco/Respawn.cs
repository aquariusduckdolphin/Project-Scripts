using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    public Transform respawnPoint;

    public Transform player;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.transform.tag == "Player")
        {

            player.transform.position = respawnPoint.position;

        }

    }

}
