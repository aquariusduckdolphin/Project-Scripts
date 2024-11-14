using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShootingBullets : MonoBehaviour
{

    public float attackDamage = 5f;

    public int shotsRemaining = 0;

    public int clipSize = 6;

    public float attackRange = 100f;

    public float triggerReactionSpeed = 0f;

    public float fireRate = 0.15f;

    public float shotDuration = 0.05f;

    public float reloadDuration = 1f;

    public RaycastHit[] hits;

    private Vector3 hitPosition;

    private float laserRange;

    public bool aimReady = false;

    public bool shotReady = true;

    public bool firing = false;

    public const string player = "Player";

    public GameObject bullet;

    void Start()
    {

        shotsRemaining = clipSize;
        
    }

    void Update()
    {

        Debug.DrawLine(transform.position, hitPosition);

        if (shotReady && !firing)
        {

            CheckRaycast();

        }

    }

    void CheckRaycast()
    {

        hits = Physics.RaycastAll(transform.position, transform.forward, attackRange);

        Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));

        if (hits.Length > 0)
        {

            if (hits[0].transform.CompareTag(player))
            {

                StartCoroutine(Shoot());

            }

        }

    }

    IEnumerator Shoot()
    {

        yield return new WaitForSeconds(triggerReactionSpeed);

        firing = true;

        shotReady = true;

        shotsRemaining--;

        hitPosition = transform.position;

        PlayerHealth targetHealth;

        foreach(RaycastHit hit in hits)
        {

            if (transform.CompareTag(player))
            {

                Instantiate(bullet, transform.position + Vector3.forward * 5f, Quaternion.identity);

                //targetHealth = hit.transform.GetComponent<PlayerHealth>();

                //if(targetHealth != null ) { targetHealth.TakeDamage(attackDamage); }

            }
            else
            {

                break;

            }

        }

        yield return new WaitForSeconds(shotDuration);

        firing = false;

        float cooldownRemaining = fireRate - shotDuration;

        if(cooldownRemaining < 0f ) { cooldownRemaining = 0f; }

        if(shotsRemaining <= 0)
        {

            yield return new WaitForSeconds(cooldownRemaining);

            shotsRemaining = clipSize;

        }

        shotReady = true;

    }

}
