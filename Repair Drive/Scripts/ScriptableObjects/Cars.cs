using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Cars/ Small Car", order = 1)]
public class Cars : ScriptableObject
{

    public GameObject car;

    public float speed = 10f;

    public float rotationFront = 10f;
    
}
