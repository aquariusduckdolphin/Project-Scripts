using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    public float maxHealth = 100f;

    [Range(0f, 100f)]
    public float currentHealth = 0f;

    private PlayerMovement player;

    public Image bloodBorder;

    public float inherintatedDamage;

    public float time = 0.5f;

    float IDamageable.damage => inherintatedDamage;

    public void TakeDamage(float damage)
    {
        
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {

            currentHealth = 0f;

            print("Dead");

        }

    }

    void Start()
    {

        currentHealth = maxHealth;

        player = GetComponent<PlayerMovement>();

        bloodBorder = GameObject.Find("HurtBorder").GetComponent<Image>();

        StartCoroutine(SlowUdate(time));

        SetBorderTransparency(0);

    }

    void Update()
    {

        SettingBorderValue(currentHealth, 100f, 80f, 0f);

        SettingBorderValue(currentHealth, 80f, 60f, 0.1f);

        SettingBorderValue(currentHealth, 60f, 40f, 0.2f);

        SettingBorderValue(currentHealth, 40f, 20f, 0.5f);

        SettingBorderValue(currentHealth, 20f, 0f, 0.6f);

        if (currentHealth <= 0)
        {

            SetBorderTransparency(1f);

            SceneManager.LoadScene("Death Screen");

        }

        if(currentHealth > 100)
        {

            currentHealth = 100f;

        }

    }

    IEnumerator SlowUdate(float time)
    {

        yield return new WaitForSeconds(time);

        inherintatedDamage = player.damage;

        StartCoroutine(SlowUdate(time));

    }

    void SetBorderTransparency(float number)
    {

        Color tempColor = bloodBorder.color;

        tempColor.a = number;

        bloodBorder.color = tempColor;

    }

    void SettingBorderValue(float health, float maxVaule, float minValue, float transparencyVaue)
    {

        if(health <= maxVaule && health > minValue)
        {

            SetBorderTransparency(transparencyVaue);

        }

    }

}
