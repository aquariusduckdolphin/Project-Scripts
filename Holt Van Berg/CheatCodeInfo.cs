using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodeInfo : MonoBehaviour
{

    [Header("BOOLEANS")]
    public bool isInvincibile = false;

    public bool enemiesOff = false;

    [Header("LIST")]
    public GameObject[] zombies;

    public GameObject[] vampire;

    public GameObject[] ghoul;

    [Header("SCRIPTS")]
    public Damaged player;

    public CheatCodes codes;

    [Header("VALUES")]
    public float currentHealth;

    public float health;

    void Update()
    {

        if (player != null)
        {

            StartCoroutine(Health());

        }
        else
        {

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Damaged>();

        }

        if (codes != null)
        {

            if (codes.invincibility == true)
            {

                isInvincibile = true;

            }
            
            if (codes.turnOff == true)
            {

                enemiesOff = true;

            }

        }
        else
        {

            codes = GameObject.FindGameObjectWithTag("Player").GetComponent<CheatCodes>();

        }

        if (enemiesOff)
        {

            zombies = GameObject.FindGameObjectsWithTag("Zombie");

            vampire = GameObject.FindGameObjectsWithTag("Vampire");

            ghoul = GameObject.FindGameObjectsWithTag("Ghoul");

            TurnOffEnemies();

        }

    }

    void TurnOffEnemies()
    {

        for (int i = 0; i < zombies.Length; i++)
        {

            zombies[i].SetActive(false);

        }

        for (int i = 0; i < vampire.Length; i++)
        {

            vampire[i].SetActive(false);

        }

        for (int i = 0; i < ghoul.Length; i++)
        {

            ghoul[i].SetActive(false);

        }

    }

    IEnumerator Health()
    {

        currentHealth = player.health;

        yield return new WaitForSeconds(0.5f);
        
        player.health = currentHealth;

    }

}
