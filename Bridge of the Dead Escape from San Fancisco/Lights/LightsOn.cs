using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightsOn : MonoBehaviour
{

    public GameObject[] lights;

    public GameObject text;

    public GameObject sparks;

    private bool enterLightRange;


    public void Start()
    {

        sparks.SetActive(true);

        text.SetActive(false);
        
        enterLightRange = false;

        foreach(GameObject light in lights)
        {

            light.SetActive(false);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        enterLightRange = true;

        text.SetActive(true);

    }

    private void OnTriggerExit(Collider other)
    {
        
        enterLightRange = false;

        text.SetActive(false);

    }

    public void Update()
    {

        if (enterLightRange == true && Input.GetKeyDown(KeyCode.E))
        {

            foreach(GameObject light in lights)
            {

                light.SetActive(true);

                sparks.SetActive(false);

            }

        }


    }

}
