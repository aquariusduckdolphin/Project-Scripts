using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTo : MonoBehaviour
{

    public Transform snapTo;

    public bool snapPosition = true;

    public bool snapRotation = true;

    // Update is called once per frame
    void Update()
    {

        if(snapPosition) transform.position = snapTo.position;

        if(snapRotation) transform.rotation = snapTo.rotation;
        
    }
}
