using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAbility : MonoBehaviour
{

    public bool canUseAbility;

    public GameObject border;

    public ParticleSystem earthShatter;
    private void Start()
    {

        earthShatter.Stop();

        border.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            canUseAbility = true;

            border.SetActive(true);

        }

    }
    private void OnTriggerExit(Collider other)
    {

        canUseAbility = false;

        border.SetActive(false);

    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q) && canUseAbility)
        {
            
            earthShatter.Play();
            
        }
        else
        {

            earthShatter.Stop();

        }

    }

}
