using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{

    private TMP_Text finalScore;

    void Start()
    {

        finalScore = GetComponent<TMP_Text>();
        
    }

    void Update()
    {

        finalScore.text = "Score: " + Manager._instance.score.ToString();
        
    }
}
