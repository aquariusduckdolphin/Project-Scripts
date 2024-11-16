using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemies : MonoBehaviour
{

    public float killAmount = 100f;

    private void OnTriggerEnter(Collider other)
    {

        IDamagable kill = other.GetComponent<IDamagable>();

        if (kill != null)
        {

            kill.TakeDamage(killAmount);

        }

    }

}
