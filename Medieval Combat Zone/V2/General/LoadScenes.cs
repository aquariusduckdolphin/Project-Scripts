using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CombatZone.Scenes
{

    [System.Serializable]
    public class Scenes
    {
        public string mapName;
        public List<string> sceneNames;
    }

    public class LoadScenes : MonoBehaviour
    {

        public List<Scenes> scene;

        private List<AsyncOperation> scenesToLaod = new List<AsyncOperation>();

        [SerializeField] private bool hasLoadingScreen = true;

        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject loadingBar;
        [SerializeField] private Image loadingProgressBar;

        /************************** Start, Update, Etc. **************************/

        #region Start
        private void Start()
        {
            if(loadingBar != null)
            {
                loadingBar.SetActive(false);
            }    
        }
        #endregion

        /************************** Load Scene **************************/

        #region Single Scene Loading
        public void LoadNewScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        #endregion

        /************************** Additvie Secene **************************/

        #region Additive Scene Loading
        private void SetLoadingScreen()
        {
            if (!hasLoadingScreen) { return; }
            loadingProgressBar.fillAmount = 0f;
            loadingBar.SetActive(true);
            menu.SetActive(false);
        }

        public void ImprovedAdditiveLoading(int sceneIndex)
        {
            for (int i = 0; i < scene[sceneIndex].sceneNames.Count; i++)
            {
                scenesToLaod.Add(SceneManager.LoadSceneAsync(scene[sceneIndex].sceneNames[i], LoadSceneMode.Additive)); 
            }
            StartCoroutine(Loading(sceneIndex));
        }

        private IEnumerator Loading(int sceneIndex)
        {
            SetLoadingScreen();
            float totalProgress = 0f;
            float currentProgress = 0f;
            for (int i = 0; i < scene[sceneIndex].sceneNames.Count; i++)
            {
                while (!scenesToLaod[i].isDone) 
                {
                    if (hasLoadingScreen)
                    {
                        totalProgress += scenesToLaod[i].progress;
                        currentProgress = totalProgress / scenesToLaod.Count;
                        loadingProgressBar.fillAmount = Mathf.Clamp(currentProgress, 0f, 0.9f);
                    }
                    yield return null; 
                }
            }
            yield return new WaitForSeconds(0.5f);
            loadingProgressBar.fillAmount = 1f;
            yield return new WaitForSeconds(0.5f);
            loadingBar.SetActive(false);
            SceneManager.UnloadSceneAsync("TitleScene");
        }
        #endregion

        /************************** Quit **************************/

        #region Quit Game
        public void QuitGame()
        {
            Debug.Log("Left the Game!");
            Application.Quit();
        }
        #endregion

    }

}