using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{

    public Damaged playerKey;

    public Animator animator;

    public bool enterGate;

    private ZoneEnemies zone;

    // Start is called before the first frame update
    void Awake()
    {

        playerKey = GameObject.FindGameObjectWithTag("Player").GetComponent<Damaged>();

        zone = GetComponent<ZoneEnemies>();

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Player" && playerKey.hasKey == true && enterGate == true)
        {

            animator.SetBool("PlayerHasKey", true);

            animator.SetBool("FrontGateOpen", true);

            playerKey.hasKey = false;

            zone.EnemiesOn(true);

        }
        else if(other.transform.tag == "Player" && playerKey.hasKey == true && enterGate == false)
        {

            animator.SetBool("PlayerHasKeyExit", true);

            playerKey.hasKey = false;

        }

    }

}
