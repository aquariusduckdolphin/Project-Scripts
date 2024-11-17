using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapons : MonoBehaviour
{

    [Header("Weapon")]
    public float damage = 10f;

    public float range = 100f;

    public float fireRate = 15f;

    private float nextTimeToFire = 0f;

    public int maxAmmo = 20;
    [Space(10f)]

    [Header("UI Text")]
    public TMP_Text ammoCount;

    private int currentAmmo;

    public float reloadTime = 1f;

    private bool isReloading = false;
    [Space(10f)]

    [Header("References")]
    public ParticleSystem muzzleFlash;

    public GameObject muzzleLocation;
    [Space(10f)]

    [Header("Animation References")]
    public Animator anim;

    private const string animReload = "isReloading";

    void Start()
    {

        ammoCount = GameObject.Find("Ammo Counter").GetComponent<TMP_Text>();

        muzzleLocation = transform.GetChild(0).gameObject;

        anim = transform.parent.GetComponent<Animator>();

        currentAmmo = maxAmmo;

    }

    private void OnEnable()
    {

        isReloading = false;
        
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

        Debug.DrawRay(transform.position, transform.forward * 1000f, Color.green);


    }

    void Shoot()
    {

        //Instantiate(muzzleFlash, muzzleLocation.transform.position, muzzleLocation.transform.rotation);

        currentAmmo--;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, muzzleLocation.transform.forward * 50f, out hit, range))
        {
            
            IDamagable hurtEnemy = hit.transform.GetComponent<IDamagable>();

            if(hurtEnemy != null)
            {

                hurtEnemy.TakeDamage(damage);

            }

            Debug.Log(hit.transform.name);

        }

    }

    IEnumerator Reload(float time)
    {

        isReloading = true;

        anim.SetBool(animReload, true);

        yield return new WaitForSeconds(time - .25f);

        Debug.Log("Reloading");

        anim.SetBool(animReload, false);

        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;

        isReloading = false;

    }

    void UiUpdate(TMP_Text word)
    {

        word.text = "Ammo: " + currentAmmo.ToString() + "/" + maxAmmo.ToString();

    }
}
