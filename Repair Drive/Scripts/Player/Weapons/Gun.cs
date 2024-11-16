using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{

    [Header("Weapon")]
    public float damage = 10f;

    public float range = 100f;

    public float fireRate = 15f;

    private float nextTimeToFire = 0f;

    public int maxAmmo = 20;

    [Header("UI Text")]
    public TMP_Text ammoCount;

    private int currentAmmo;

    public float reloadTime = 1f;

    private bool isReloading = false;

    [Header("References")]
    public Camera fpsCam;

    public ParticleSystem muzzleFlash;

    public Transform playerPosParticle;

    public GameObject muzzleLocation;

    [Header("Animation References")]
    private const string animIdle = "Reloading";

    void Start()
    {

        currentAmmo = maxAmmo;

    }

    void Update()
    {

        UiUpdate(ammoCount);

        if (isReloading) { return; }

        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {

            StartCoroutine(Reload(reloadTime));

            return;

        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {

            nextTimeToFire = Time.time + 1f / fireRate;

            Shoot();

        }
        
    }

    void Shoot()
    {

        Instantiate(muzzleFlash, muzzleLocation.transform.position, muzzleLocation.transform.rotation);

        currentAmmo--;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward * 50f, out hit, range))
        {

            Debug.DrawRay(transform.position, transform.forward * 1000f, Color.green);

            IDamagable hurtEnemy = hit.transform.GetComponent<IDamagable>();

            if( hurtEnemy != null )
            {

                hurtEnemy.TakeDamage(damage);

            }

            Debug.Log(hit.transform.name);

        }

    }

    IEnumerator Reload(float time)
    {

        isReloading = true;

        yield return new WaitForSeconds(time - .25f);

        Debug.Log("Reloading");

        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;

        isReloading = false;

    }

    void UiUpdate(TMP_Text word)
    {

        word.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();

    }

}
