using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{

    public float maxHealth = 100f;

    public float currentHealth = 0f;

    public float enemyScore = 50f;

    void Start()
    {

        currentHealth = maxHealth;
        
    }

    public void TakeDamage(float amount)
    {

        currentHealth -= amount;

        if(currentHealth <= 0f)
        {

            Manager._instance.score += enemyScore;

            currentHealth = 0f;

            Destroy(gameObject);

        }

    }

}
