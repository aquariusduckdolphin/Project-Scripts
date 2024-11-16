using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private static bool isPaused;

    public FirstPersonLook cameraMovement;

    public GameObject pauseMenu;

    private void Start()
    {

        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FirstPersonLook>();

        pauseMenu = GameObject.Find("Journal");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused == true)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;

            //Turn off the camera movement
            cameraMovement.enabled = false;

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

            pauseMenu.SetActive(true);

        }

        else
        {

            Unpause();

        }
    }

    public void Unpause()
    {

        Time.timeScale = 1;
        AudioListener.pause = false;

        //Turn on the camera movement
        cameraMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        pauseMenu.SetActive(false);

    }
}
