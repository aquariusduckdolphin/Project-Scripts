using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{

    public Material mat;

    public GameObject headLights;

    [Range(0.01f,20)]
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {

        //mat = GetComponent<Material>();

        StartCoroutine(HeadLights());
        
    }

    IEnumerator HeadLights()
    {

        mat.DisableKeyword("_EMISSION");

        headLights.SetActive(false);

        yield return new WaitForSeconds(waitTime);

        mat.EnableKeyword("_EMISSION");

        headLights.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        StartCoroutine(HeadLights());

    }

}
