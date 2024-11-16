using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManageInfo : MonoBehaviour
{

    public int currentScene;

    public int previousScene;

    public Scene scene;

    private static SceneManageInfo _instance;
    public static SceneManageInfo Instance { get { return _instance; } }

    private void Awake()
    {
        
        if(_instance != null && _instance != this)
        {

            Destroy(this.gameObject);

        }
        else
        {

            _instance = this;

        }

    }

    void Start()
    {

        DontDestroyOnLoad(this);

    }

    private void Update()
    {

        scene = SceneManager.GetActiveScene();

        currentScene = Mathf.Abs(scene.buildIndex);

        if (currentScene == 1 || currentScene == 2 || currentScene == 3 || currentScene == 4)
        {

            previousScene = currentScene;

        }

    }

}
