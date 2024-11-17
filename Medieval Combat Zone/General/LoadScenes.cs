using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatZone.Scenes
{

    public class LoadScenes : MonoBehaviour
    {

        public void LoadNewScene(string sceneName)
        {

            SceneManager.LoadScene(sceneName);

        }

        public void QuitGame()
        {

            print("Left the Game!");

            Application.Quit();

        }

    }

}