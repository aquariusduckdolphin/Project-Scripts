using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieFighterExplosion : MonoBehaviour
{

    public Rigidbody[] parts;

    //public 

    public float force;

    public void TieFighterBlowUp(float force)
    {

        foreach (Rigidbody rigid in parts)
        {

            rigid.isKinematic = false;

        }

        parts[0].AddForce(-transform.right * force);

        parts[1].AddForce(-transform.right * force);

        parts[2].AddForce(transform.right * force);

        parts[3].AddForce(transform.right * force);

        parts[4].AddForce(transform.forward * force);

    }

}
