using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{

    public bool isFull = false;

    public float radius = 0.5f;

    public const string coverCharacterName = "Cowboy";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(coverCharacterName))
        {

            isFull = true;

            other.GetComponent<CowboyAi>().haveCover = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag(coverCharacterName))
        {

            isFull = false;

        }

    }

}
