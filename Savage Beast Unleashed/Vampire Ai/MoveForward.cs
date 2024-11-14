using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

    public float speed;

    private Transform player;

    private Vector3 target;

    public float damage = 10f;

    public IDamageable canDamage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector3(player.position.x, player.position.y, player.position.z);

    }

    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y && transform.position.z == target.z)
        {

            DestroyProjectile();

        }

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            //Debug.Log("Hitting the player");

            if(canDamage == null)
            {

                //Debug.Log("Getting component");

                canDamage = other.GetComponentInParent<IDamageable>();

                //Debug.Log(canDamage);

            }

            if(canDamage != null)
            {

                canDamage.TakeDamage(damage);

                Destroy(this.gameObject);

            }

            //DestroyProjectile();

        }

        Destroy(gameObject, 6f);

    }

    void DestroyProjectile()
    {

        Destroy(gameObject);

    }

}
