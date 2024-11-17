using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamagePC
{

    public float maxHealth = 100f;

    public float currentHealth = 100f;

    public float playerScore = 150f;

    private ManageScenes manage;

    private const string endScreen = "EndScreen";

    #region Gather Info
    void Awake()
    {

        manage = GameObject.FindWithTag("Scene Manager").GetComponent<ManageScenes>();
        
    }
    #endregion

    void Update()
    {

        if (manage.antennas.Count <= 0 || manage.timeRemaining <= 0f)
        {

            print("working");

            Manager._instance.score += playerScore;

        }

        if(currentHealth <= 0f)
        {

            Manager._instance.EndScreen();

        }
        
    }

    public void TakeDamage(float amount)
    {

        currentHealth -= amount;

        if(currentHealth <= 0f)
        {

            Manager._instance.score -= playerScore;

            currentHealth = 0f;

            Destroy(gameObject.transform.parent);

        }

    }

}
