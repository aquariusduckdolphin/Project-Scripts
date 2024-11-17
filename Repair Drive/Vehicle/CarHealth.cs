using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHealth : MonoBehaviour, IDamagePC
{

    public float maxHealth = 100f;

    public float currentHealth = 0f;

    public float vehicleScore = 50f;

    private ManageScenes manage;

    #region Gather Info
    void Start()
    {

        manage = GameObject.Find("Scene Manager").GetComponent<ManageScenes>();

        currentHealth = maxHealth;
        
    }
    #endregion

    void Update()
    {

        if (manage.antennas.Count <= 0f || manage.timeRemaining <= 0f)
        {

            Manager._instance.score += vehicleScore;

        }

    }

    public void TakeDamage(float damage)
    {

        currentHealth -= damage;

        if(currentHealth <= 0f)
        {

            Manager._instance.score -= vehicleScore;

            currentHealth = 0f;

            Destroy(gameObject);

        }

    }

}
