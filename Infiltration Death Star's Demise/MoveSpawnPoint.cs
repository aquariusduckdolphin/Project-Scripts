using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpawnPoint : MonoBehaviour
{

    public GameObject spawnPoint;

    public Vector3 worldPosition;

    public Vector3 rotation;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Blue Team")
        {

            spawnPoint.transform.position = worldPosition;

            //spawnPoint.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

            spawnPoint.transform.eulerAngles = rotation;

        }

    }

}
