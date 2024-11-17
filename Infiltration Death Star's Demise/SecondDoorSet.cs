using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDoorSet : MonoBehaviour
{

    public Animator[] doors;

    public bool secondSet = true;

    public bool hasPlans = false;

    public ButtonDown button;

    private void Start()
    {

        button = GameObject.FindGameObjectWithTag("Button").GetComponent<ButtonDown>();

    }

    public void Update()
    {
        
        if(button.plan == true)
        {

            hasPlans = true;

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (secondSet && hasPlans || other.transform.tag == "Red Team")
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

        foreach (Animator door in doors)
        {

            door.SetBool("Open", false);

            door.SetBool("Close", true);

        }

    }

}
