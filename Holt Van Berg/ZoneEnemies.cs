using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneEnemies : MonoBehaviour
{

    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {

        EnemiesOn(false);
        
    }

    public void EnemiesOn(bool isActive)
    {

        foreach(GameObject go in enemies)
        {

            go.SetActive(isActive);
        
        }

    }

}
