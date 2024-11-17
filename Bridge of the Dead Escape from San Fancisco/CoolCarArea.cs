using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolCarArea : MonoBehaviour
{

    public ParticleSystem earthShatter;

    public OpenandAbility hasAbility;

    public GameObject border;

    public bool inTrigger;
    void Start()
    {

        earthShatter.Stop();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            border.SetActive(true);

            inTrigger = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            border.SetActive(false);

            inTrigger = false;

        }

    }
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q) && inTrigger)
        {

            earthShatter.Play();

        }
        else
        {

            earthShatter.Stop();

        }
        
    }
}
