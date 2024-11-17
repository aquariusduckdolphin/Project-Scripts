using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraIntoPosition : MonoBehaviour
{

    private Transform cameraPosition;

    private void Start()
    {

        cameraPosition = GameObject.Find("Camera Position").transform;

    }

    void Update()
    {

        transform.position = cameraPosition.position;
        
    }

}
