using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBending : MonoBehaviour
{

    public bool canBridgeIncrease = false;

    public GameObject startPoint;

    public GameObject endPoint;

    // Start is called before the first frame update
    void Start()
    {

        endPoint.SetActive(false);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            canBridgeIncrease = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            canBridgeIncrease = false;

        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && canBridgeIncrease)
        {

            startPoint.SetActive(false);

            endPoint.SetActive(true);

        }

        if(Input.GetKeyDown(KeyCode.Q) && !canBridgeIncrease)
        {

            startPoint.SetActive(false);

            endPoint.SetActive(false);

        }

    }
}
