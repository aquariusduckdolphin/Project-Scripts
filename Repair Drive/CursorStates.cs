using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStates : MonoBehaviour
{

    public bool lockState = false;

    void Start()
    {

        if (lockState)
        {

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = !lockState;

        }
        else
        {

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

        }

    }

}
