using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossOpenGate : MonoBehaviour
{
    public Mutant mutant;

    public Animator OpenDoor;

    private ZoneEnemies zone;

    void Start()
    {

        OpenDoor = GetComponent<Animator>();

        zone = GetComponent<ZoneEnemies>();

    }

    void Update()
    {

        if (mutant.isDead == true && mutant != null)
        {

            zone.EnemiesOn(true);

            OpenDoor.SetBool("OpenDoor", true);

        }

    }

}
