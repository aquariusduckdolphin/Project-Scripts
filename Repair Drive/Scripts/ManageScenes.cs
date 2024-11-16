using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScenes : MonoBehaviour
{

    [Header("Atenna References")]
    public List<FixAtenna> antennas;

    [SerializeField] private float totalNumOfAntennas = 0f;

    [SerializeField] private float antennaCollected;

    private float antennaScore = 100f;
    [Space(10)]

    [Header("Text References")]
    private TMP_Text repairCount;

    private TMP_Text carText;

    private TMP_Text timeText;
    [Space(10)]

    [Header("Timer References")]
    public float timeRemaining = 10f;

    [HideInInspector] public bool timerRunning = false;

    #region Gather Info
    void Awake()
    {

        repairCount = GameObject.FindGameObjectWithTag("Count").GetComponent<TMP_Text>();

        carText = GameObject.FindGameObjectWithTag("Enter Text").GetComponent<TMP_Text>();

        timeText = GameObject.FindGameObjectWithTag("Timer").GetComponent<TMP_Text>();

        GameObject[] antenna = GameObject.FindGameObjectsWithTag("Antenna");

        foreach(GameObject go in antenna)
        {

            antennas.Add(go.GetComponent<FixAtenna>());

        }

        totalNumOfAntennas = antennas.Count;

        StartCoroutine(CheckCount());

    }
    #endregion

    void Update()
    {

        if(antennaCollected == totalNumOfAntennas || timeRemaining <= 0)
        {

            SceneManager.LoadScene("EndScreen");

        }

        if(timerRunning)
        {

            if (timeRemaining > 0)
            {

                timeRemaining -= Time.deltaTime;

            }
            else
            {

                print("End Game");

                timeRemaining = 0f;

                timerRunning = false;

            }

            DisplayTime(timeRemaining);

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();

        }

    }

    IEnumerator CheckCount()
    {

        for(int i = 0; i < antennas.Count; i++)
        {

            if (antennas[i].repaired)
            {

                antennas[i].collected = true;

                Manager._instance.score += antennaScore;

                antennas.Remove(antennas[i]);

                antennaCollected++;

            }

        }

        repairCount.text = "Antenna Repair: " + antennaCollected + "/5";

        yield return new WaitForSeconds(1f);

        StartCoroutine(CheckCount());

    }

    void DisplayTime(float timeToDisplay)
    {

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

}
