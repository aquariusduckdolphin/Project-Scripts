using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnterExitCar : MonoBehaviour
{

    public bool inVehicle = false;

    private CarController vehicleScript;

    [Header("GameObject References")]
    private GameObject enterCarText;

    private GameObject player;

    private GameObject carCam;

    #region Gather Info
    private void Awake()
    {

        //Get the text and store it
        enterCarText = GameObject.FindWithTag("Enter Text");

        carCam = transform.root.GetChild(0).transform.gameObject;

        //Get the vehicle script
        vehicleScript = transform.root.GetComponent<CarController>();

    }

    void Start()
    {

        //Turn off the text from appearing
        enterCarText.SetActive(false);

        //Get the player and store it
        player = GameObject.FindWithTag("Player").gameObject.transform.root.gameObject;

        //Turn the car script off
        vehicleScript.enabled = false;

        //Turn the camera off
        carCam.SetActive(false);

    }
    #endregion

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player") && inVehicle == false)
        {

            enterCarText.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {

                enterCarText.SetActive(false);

                player.transform.parent = gameObject.transform;

                vehicleScript.enabled = true;

                player.SetActive(false);

                inVehicle = true;

                carCam.SetActive(true);

            }

        }

    }
    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            enterCarText.SetActive(false);

        }

    }

    void Update()
    {

        if (inVehicle == true && Input.GetKey(KeyCode.F))
        {

            vehicleScript.enabled = false;

            player.SetActive(true);

            player.transform.parent = null;

            inVehicle = false;

            carCam.SetActive(false);

        }

    }

}
