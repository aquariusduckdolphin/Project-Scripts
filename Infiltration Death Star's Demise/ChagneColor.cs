using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChagneColor : MonoBehaviour
{

    public Material buttonMat;

    [Range(0,1)]
    public float transparency;

    public Color color;

    private void Awake()
    {

        buttonMat.color = new Color(1, 0, 0, transparency);

    }

    public void ChangeColor(Material material)
    {

        Color newColor = new Color(0, 1, 0, transparency);

        material.color = newColor;

    }

}
