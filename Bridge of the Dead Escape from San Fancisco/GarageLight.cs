using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GarageLight : MonoBehaviour
{

    public Material garageLights;

    public GameObject spark;

    public GameObject text;

    private bool onWires;

    void Start()
    {

        text.SetActive(false);

        onWires = false;

        garageLights.DisableKeyword("_EMISSION");
        
    }

    private void OnTriggerEnter(Collider other)
    {

        text.SetActive(true);

        onWires = true;

    }

    private void OnTriggerExit(Collider other)
    {

        text.SetActive(false);

        onWires = false;

    }

    void Update()
    {

        if (onWires == true && Input.GetKeyUp(KeyCode.E))
        {

            garageLights.EnableKeyword("_EMISSION");

        }
        
    }

}
