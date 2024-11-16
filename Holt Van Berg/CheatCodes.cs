using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{

    public bool invincibility = false;

    public bool turnOff = false;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.Y))
        {

            invincibility = true;

        }

        if (Input.GetKey(KeyCode.P))
        {

            turnOff = true; 

        }
        
    }
}
