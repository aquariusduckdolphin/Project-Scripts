using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{

    public float score;

    public const string endScreen = "EndScreen";

    #region Singleton
    public static Manager _instance { get; private set; }

    private void Awake()
    {
        
        if(_instance != null && _instance != this)
        {

            Destroy(gameObject);

        }
        else
        {

            _instance = this;

        }

        DontDestroyOnLoad(gameObject);

    }
    #endregion

    public void EndScreen()
    {

        SceneManager.LoadScene(endScreen);

    }

}
