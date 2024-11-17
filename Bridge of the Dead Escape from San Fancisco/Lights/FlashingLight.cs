using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{

    private Light light;

    [Range(0.01f,20)]
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {

        light = GetComponent<Light>();

        StartCoroutine(FlashingLights());
        
    }

    IEnumerator FlashingLights()
    {

        light.enabled = true;

        yield return new WaitForSeconds(waitTime);

        light.enabled = false;

        yield return new WaitForSeconds(waitTime);

        StartCoroutine(FlashingLights());

    }

}
