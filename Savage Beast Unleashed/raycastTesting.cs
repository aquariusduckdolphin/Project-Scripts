using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 20f, Color.red);

    }
}
