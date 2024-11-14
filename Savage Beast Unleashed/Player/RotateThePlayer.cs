using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThePlayer : MonoBehaviour
{

    public GameObject orientation;

    private Vector3 targetDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RotateCorrectly();
        
    }

    void RotateCorrectly()
    {

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);

    }

}
