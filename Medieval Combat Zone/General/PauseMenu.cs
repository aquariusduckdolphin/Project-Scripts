using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.UI
{

    public class PauseMenu : MonoBehaviour
    {

        #region Varables
        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseMenu;

        [SerializeField] private static bool isPaused = false;
        #endregion

        /***************************************************************/

        #region Input Action System

        #region OnPause
        public void OnPause(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                isPaused = !isPaused;
                print("Pause");

            }

        }
        #endregion

        #endregion

        /***************************************************************/

        #region Start, Update, Etc

        #region Start
        void Start() { pauseMenu.SetActive(false); }
        #endregion

        #region Update
        void Update()
        {
            if (isPaused) { Pause(0f, true, CursorLockMode.None); }

            else { Pause(1f, false, CursorLockMode.Locked); }

        }
        #endregion

        #endregion

        #region Pause
        private void Pause(float timeScale, bool value, CursorLockMode lockMode)
        {

            Time.timeScale = timeScale;

            //AudioListener.pause = value;

            Cursor.lockState = lockMode;

            Cursor.visible = value;

            pauseMenu.SetActive(value);

        }
        #endregion

    }

}