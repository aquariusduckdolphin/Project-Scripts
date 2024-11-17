using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public GameObject[] allZones;
    public GameObject[] activeZonesOnStart;

    //Singleton
    private static ZoneManager _instance;
    public static ZoneManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        HideAllZones();
        ActivateZones(activeZonesOnStart);
    }

    public void HideAllZones()
    {
        foreach(GameObject zone in allZones)
        {
            zone.SetActive(false);
        }
    }

    public void ActivateZones(GameObject[] activeZones)
    {
        foreach (GameObject zone in activeZones)
        {
            zone.SetActive(true);
        }
    }
}
