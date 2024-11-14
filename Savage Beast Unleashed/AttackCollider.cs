using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{

    public GameObject trigger;

    public Transform spawnPosition;

    public Vector3 offset;

    private GameObject box;

    public float time = 1f;

    [Tooltip("Make sure this value is negative to work properly on the Main Character.")]
    public float triggerlocation = 10f;

    void SpawnTrigger()
    {

        box = Instantiate(trigger, spawnPosition.position + (spawnPosition.forward * triggerlocation), Quaternion.identity);

    }

    void DestroyCollider()
    {

        Destroy(box);

    }

}
