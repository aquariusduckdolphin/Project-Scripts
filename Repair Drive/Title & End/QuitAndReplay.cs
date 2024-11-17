using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitAndReplay : MonoBehaviour
{

    public string sceneToLoad;

    public void QuitGame()
    {

        Application.Quit();

    }

    public void ReplayGame()
    {

        SceneManager.LoadScene(sceneToLoad);

    } 


}
