using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForward : MonoBehaviour
{

    private Rigidbody rb;

    public float speed = 100f;

    public GameObject bulletPoint;

    public float timeDelay = 10f;

    private void OnEnable()
    {
        
        rb = GetComponent<Rigidbody>();

    }

    void Start()
    {

        //bulletPoint = GameObject.Find("RocketLocation");

        Forward();
        
    }

    void Forward()
    {

        rb.velocity = Vector3.zero;

        rb.AddForce(bulletPoint.transform.forward * speed);

    }

    private void OnTriggerEnter(Collider other)
    {

        IDamagePC damage = other.GetComponent<IDamagePC>();

        if(damage !=  null )
        {

            damage.TakeDamage(10f);

        }

        Destroy(gameObject, timeDelay);

    }

}
