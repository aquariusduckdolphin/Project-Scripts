using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{

    private Light light;

    [SerializeField] private float durationAmount = 10f;

    private float lightDuration = 0f;

    [SerializeField] private GameObject lightTrigger;

    // Start is called before the first frame update
    void Start()
    {

        light = GetComponent<Light>();

        lightDuration = durationAmount;

    }

    void Update()
    {

        if(lightDuration > 0f)
        {

            lightDuration -= Time.deltaTime;

            light.range -= Time.deltaTime;

        }

        if(lightDuration <= 0)
        {

            TurnOffLamp();

        }

    }

    public void TurnOffLamp()
    {

        Lamp(0, false);

    }

    private void Lamp(float durationAmount, bool isActive)
    {

        lightDuration = durationAmount;

        light.range = durationAmount;

        lightTrigger.SetActive(isActive);

        light.enabled = isActive;

    }

    public void TurnOnLamp()
    {

        Lamp(durationAmount, true);

    }

}
