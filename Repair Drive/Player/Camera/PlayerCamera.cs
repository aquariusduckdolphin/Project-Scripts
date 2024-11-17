using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public PlayerInfo sens;

    private Transform orientation;

    private float xRotation;

    private float yRotation;

    void Start()
    {

        orientation = GameObject.Find("Orientation").transform;

    }

    void Update()
    {

        //Get the mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens.mouseSensitivity;

        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens.mouseSensitivity;

        Debug.Log(sens.mouseSensitivity);

        //Rotate the camera in the direction on the mouse input
        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        //rotate camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);

    }

}
