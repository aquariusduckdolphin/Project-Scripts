using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeLamp : MonoBehaviour
{

    public LightTest test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        test.TurnOnLamp();

        /*if (other.CompareTag("Lamp"))
        {

            foreach(GameObject go in other.gameObject.transform.GetComponentInChildren<LightTest>())
            {

                LightTest light = go.GetComponent<LightTest>();

                if (light != null)
                {

                    light.TurnOnLamp();

                }

            }

        }*/

    }

}
