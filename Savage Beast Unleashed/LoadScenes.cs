using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadScenes : MonoBehaviour
{

    public string sceneName;

    public float delay = 0.5f;

    public Image coolEffect;

    public void LoadInScene()
    {

        SceneManager.LoadScene(sceneName);

    }

    public void NextScene()
    {

        StartCoroutine(NextSceneToLoad(delay));

    }

    public void QuitTheGame()
    {

        Application.Quit();
        
    }

    IEnumerator NextSceneToLoad(float time)
    {

        SetBorderTransparency(1);

        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneName);

    }

    void SetBorderTransparency(float number)
    {

        Color tempColor = coolEffect.color;

        tempColor.a = number;

        coolEffect.color = tempColor;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            LoadInScene();

        }

    }

}
