using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private Damaged player;

    public float GainedHealth = 100f;

    public GameObject health;

    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Damaged>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {

            player.health += GainedHealth;

            if (player.health >= player.maxHealth)
            {

                player.health = player.maxHealth;

            }

            Destroy(health);
        }
    }
}
