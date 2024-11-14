using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWIndow : MonoBehaviour
{

    public float time;

    public bool firstTime = true;

    // Update is called once per frame
    void Update()
    {

        if(firstTime)
        {

            if (time > 0f)
            {

                time -= Time.deltaTime;

            }

            if (time < 0f)
            {

                firstTime = false;

                gameObject.SetActive(false);

            }

        }

    }
}
