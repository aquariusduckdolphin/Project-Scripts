using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBodyParts : MonoBehaviour
{

    public float force = 5f;

    public Rigidbody[] rb;

    public float destroyTime = 2f;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigid in rb)
        {

            Vector3 randomDirection = Random.onUnitSphere * force;

            randomDirection.y = 0f;

            rigid.velocity = randomDirection;

            //rigid.AddRelativeForce(Random.onUnitSphere * force);

        }

    }

    private void Update()
    {
        
        time += Time.deltaTime;

        if(time > destroyTime)
        {

            Destroy(gameObject);

        }

    }

}
