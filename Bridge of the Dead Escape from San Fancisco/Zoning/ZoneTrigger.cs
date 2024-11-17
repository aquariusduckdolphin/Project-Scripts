using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public GameObject[] activeZonesOnEnter;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            ZoneManager.Instance.HideAllZones();
            ZoneManager.Instance.ActivateZones(activeZonesOnEnter);
        }
    }
}
