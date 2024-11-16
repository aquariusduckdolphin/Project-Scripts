using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;

    public Gradient gradient;

    public Image fill;

    public void Start()
    {
        
        healthSlider = GetComponent<Slider>();

    }

    public void HealthDisplay(float health)
    {

        healthSlider.value = health;

        fill.color = gradient.Evaluate(healthSlider.normalizedValue);

    }

    public void SetHealthBar(float health)
    {

        healthSlider.maxValue = health;

        healthSlider.value = health;

        if(gradient != null)
        {

            fill.color = gradient.Evaluate(1f);

        }

    }

}
