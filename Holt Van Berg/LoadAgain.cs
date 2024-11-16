using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAgain : MonoBehaviour
{

    public SceneManageInfo sceneInfo;

    public void Start()
    {

        sceneInfo = GameObject.Find("Scene Info").GetComponent<SceneManageInfo>();

    }

    public void PreviousScene()
    {

        SceneManager.LoadScene(sceneInfo.previousScene);

    }

}
