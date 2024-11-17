using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreMeltDown : MonoBehaviour
{

    public Material coreMat;

    public Color baseColor;

    public Color changeColor;

    public float baseIntensity;

    public float intensity;

    public bool inMeltDown;

    void Start()
    {

        coreMat.SetColor("_BaseColor", baseColor);

        coreMat.SetColor("_EmissiveColor", baseColor * baseIntensity);

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.transform.tag == "Blue Team")
        {

            inMeltDown = true;

            coreMat.SetColor("_BaseColor", changeColor);

            coreMat.SetColor("_EmissiveColor", changeColor * intensity);

        }

    }

}
