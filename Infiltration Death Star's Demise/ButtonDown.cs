using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDown : MonoBehaviour
{

    public Animator button;

    public bool canPress;

    public bool plan = false;

    public Material buttonMat;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && canPress)
        {

            button.SetBool("Down", true);

            canPress = true;

            plan = true;

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Blue Team")
        {

            canPress = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        
        if(other.transform.tag == "Blue Team")
        {

            canPress = false;

        }

    }

}
