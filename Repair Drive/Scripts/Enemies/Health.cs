using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour, IDamagePC
{

    [Range(0f, 100f)]
    public float maxHealth = 100f;

    public float currentHealth;

    public Material mat;

    public float delay = 0.25f;

    #region Gather Info
    void Start()
    {

        currentHealth = maxHealth;

        mat = GetComponent<Renderer>().material;
        
    }
    #endregion

    public void TakeDamage(float damage)
    {

        mat.color = Color.red;

        currentHealth -= damage;

        StartCoroutine(ChangeMaterial(delay));

        if(currentHealth <= 0f)
        {

            Destroy(gameObject.transform.root.parent);

            SceneManager.LoadScene("EndScreen");

        }

    }

    IEnumerator ChangeMaterial(float time)
    {

        yield return new WaitForSeconds(time);

        mat.color = Color.white;

    }

}
