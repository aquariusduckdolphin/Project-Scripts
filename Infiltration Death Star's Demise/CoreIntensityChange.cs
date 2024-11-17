using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreIntensityChange : MonoBehaviour
{

    public Material color;

    public CoreMeltDown core;

    public float intensity1;

    public float intensity2;

    public float time = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(core.inMeltDown)
        {

            StartCoroutine(ChangeIntensity(time));

        }

    }

    IEnumerator ChangeIntensity(float time)
    {

        color.SetFloat("_EmissiveIntensity", Mathf.Lerp(30, 150, 60) );

        //color.SetFloat("_EmissiveIntensity", Mathf.PingPong(Time.deltaTime, intensity2));

        yield return new WaitForSeconds(time);

        StartCoroutine(ChangeIntensity(time));

    }

}
