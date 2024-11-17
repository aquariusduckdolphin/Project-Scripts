using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenandAbility : MonoBehaviour
{

    public bool obtainAbility = false;

    private bool canGrab = false;

    public GameObject[] crates;

    public GameObject crateDestroy;

    public GameObject[] barrels;

    public GameObject barrelsDestroy;

    public GameObject pill;

    public GameObject text; 

    void Start()
    {

        obtainAbility = false;

        text.SetActive(false);
        
    }

    private void OnTriggerEnter(Collider other)
    {

        canGrab = true;

        text.SetActive(true);

    }

    private void OnTriggerExit(Collider other)
    {

        canGrab = false;

        text.SetActive(false);

    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.E))
        {

            foreach(GameObject crate in crates)
            {

                if(crate != null)
                {

                    Instantiate(crateDestroy, crate.transform.position, crate.transform.rotation);

                    Destroy(crate);

                }

            }

            foreach(GameObject barrel in barrels)
            {

                if(barrel != null)
                {

                    Instantiate(barrelsDestroy, barrel.transform.position, barrel.transform.rotation);

                    Destroy(barrel);

                }

            }

            obtainAbility = true;

            if(pill != null)
            {

                Destroy(pill);

            }
            
        }
        
    }

}
