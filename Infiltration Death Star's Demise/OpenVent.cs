using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenVent : MonoBehaviour
{

    public Rigidbody vent;

    public bool ventOne;

    public bool ventTwo;

    public ButtonDown hasPlan;

    public float force;

    // Start is called before the first frame update
    void Start()
    {

        hasPlan = GameObject.FindGameObjectWithTag("Button").GetComponent<ButtonDown>();

        vent = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (hasPlan.plan && ventOne)
        {

            vent.isKinematic = false;

            vent.AddForce(transform.forward * force);

        }
        else if (hasPlan.plan && ventTwo)
        {

            vent.isKinematic = false;

            vent.AddForce(-transform.forward * force);

        }
        
    }
}
