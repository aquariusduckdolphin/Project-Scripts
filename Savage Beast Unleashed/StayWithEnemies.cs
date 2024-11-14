using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayWithEnemies : MonoBehaviour
{

    public Transform modelLocation;

    public float delay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Follow(delay));
        
    }

    IEnumerator Follow(float time)
    {

        yield return new WaitForSeconds(time);

        transform.position = modelLocation.position;

        StartCoroutine(Follow(time));

    }

}
