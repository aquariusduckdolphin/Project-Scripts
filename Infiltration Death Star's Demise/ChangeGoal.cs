using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGoal : MonoBehaviour
{

    public GameObject[] tacticalPosition;

    public int value;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Blue Team")
        {

            RedTeamObjectives.Instance.currentGoal = value;

            foreach(GameObject go in tacticalPosition)
            {

                Destroy(go);

            }


        }

    }

}
