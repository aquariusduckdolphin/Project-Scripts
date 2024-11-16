using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{

    public Cars vehicle;

    public Vector3 spawnLocation;

    private GameObject card;

    private GameObject player;

    private ManageScenes manager;

    #region Gather Variables
    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player").gameObject;

        card = transform.parent.gameObject;

        manager = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<ManageScenes>();

    }

    private IEnumerator Start()
    {

        yield return new WaitForSeconds(2f);

        player.SetActive(false);
        
    }
    #endregion

    #region Spawn in a vehicle
    public void CreateVehicle()
    {

        Instantiate(vehicle.car, spawnLocation, Quaternion.identity);

        player.SetActive(true);

        card.SetActive(false);

        manager.timerRunning = true;

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

    }
    #endregion

}
