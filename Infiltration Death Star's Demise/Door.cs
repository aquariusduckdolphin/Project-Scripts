using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public Animator[] doors;

    public bool firstSet = false;

    public bool secondSet = false;

    public bool hasPlans = false;

    public ButtonDown button;

    public void Start()
    {

        button = GameObject.FindGameObjectWithTag("Button").GetComponent<ButtonDown>();

    }

    public void Update()
    {

        if (button.plan == true)
        {

            hasPlans = true;

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if(firstSet && !hasPlans || secondSet && hasPlans || other.CompareTag("Red Team") || other.CompareTag("Droid"))
        {

            foreach (Animator door in doors)
            {

                door.SetBool("Open", true);

                door.SetBool("Close", false);

            }

        }

    }

    private void OnTriggerExit(Collider other)
    {

        foreach(Animator door in doors)
        {

            door.SetBool("Open", false);

            door.SetBool("Close", true);

        }
        
    }

}
