using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpMess : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        
        Destroy(other.gameObject);

        print(other.gameObject);

    }

}
