using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    public IDamageable canDamage;

    public IDamageable damageable;

    public PlayerMovement pMove;

    public AttackCollider attackCol;

    public float addToRage = 10f;

    public float gruntDamage = 10f;

    public Grunt grunt;

    public Vampire vampire;

    public CowboyAi cowboy;

    public float lifeTime = 2f;

    private float time;


    private void Start()
    {

        pMove = GameObject.Find("Player").GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        
        time += Time.deltaTime;

        if(time > lifeTime)
        {

            Destroy(gameObject);

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        canDamage = other.GetComponent<IDamageable>();

        damageable = other.transform.root.GetComponent<IDamageable>();

        grunt = other.GetComponent<Grunt>();

        vampire = other.GetComponent<Vampire>();

        cowboy = other.GetComponent<CowboyAi>();

        if(pMove != null && canDamage != null)
        {

            canDamage.TakeDamage(pMove.damage);

            pMove.rageMeter += addToRage;

        }

        HurtCheck();

    }

    void HurtCheck()
    {

        if (damageable != null)
        {

            try
            {

                damageable.TakeDamage(gruntDamage);

            }
            catch ( Exception e)
            {

                print("error");

            }

        }

    }

}
