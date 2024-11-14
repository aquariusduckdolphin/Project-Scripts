using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTING : MonoBehaviour
{

    public KeyCode test = KeyCode.Tab;

    public GameObject control;

    public bool flip = false;

    // Start is called before the first frame update
    void Awake()
    {

        control = GameObject.Find("Control Window");
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(test))
        { 

            control.SetActive(!flip);

            flip = !flip;
        
        }
        
    }
}
