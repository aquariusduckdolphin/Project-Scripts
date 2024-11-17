using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    private GameObject settings;

    public bool settingsOpen = false;

    void Awake()
    {

        settings = GameObject.Find("Pause Menu");

        settings.SetActive(false);

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !settingsOpen)
        {

            settingsOpen = true;

            settings.SetActive(true);

            Time.timeScale = 0.0f;

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && settings)
        {

            settingsOpen = false;

            settings.SetActive(false);

            Time.timeScale = 1.0f;

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;

        }
    }

}
