using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerOn : MonoBehaviour
{

    public GameObject icon;

    public KnifeThrow script;

    public void OnTriggerEnter(Collider other)
    {
       
        if(other.transform.tag == "Player")
        {

            script.enabled = true;

            icon.SetActive(true);

            Destroy(this.gameObject);

        }

    }

}
