using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHealth : MonoBehaviour, IDamageable
{

    public float maxHealth = 100f;
    
    public float currentHealth;

    public float damage = 10f;

    public Ragdoll ragdoll;

    float IDamageable.damage => damage;

    #region Gather Info
    void Start()
    {

        ragdoll = GetComponent<Ragdoll>();

        currentHealth = maxHealth;

    }
    #endregion


    #region Damage Function
    public void TakeDamage(float amount)
    {
        
        currentHealth -= amount;

        if(currentHealth <= 0f)
        {

            Die();

        }

    }
    #endregion

    #region Die Function
    public void Die()
    {
        
        ragdoll.ActivateRagdoll();

    }

    void IDamageable.TakeDamage(float damage)
    {

        currentHealth -= damage;

        if( currentHealth <= 0f)
        {

            currentHealth = 0f;

        }

    }
    #endregion

}
