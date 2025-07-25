using CombatZone.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.UI
{

    public class PauseMenu : MonoBehaviour
    {

        #region Varables

        [SerializeField] private PlayerInput playerInput;
        
        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseMenu;

        [SerializeField] private bool isPaused = false;

        #endregion

        /**************** Input Actions Execute Methods ****************/

        #region On Pause
        public void OnPause(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                isPaused = !isPaused;
                print("Preformed");
            }

        }
        #endregion

        /**************** Start, Update, Etc. ****************/

        #region Start
        void Start()
        { 
            pauseMenu.SetActive(false);
            playerInput.actions.FindActionMap("Pausing").Enable();
        }
        #endregion

        #region Update
        void Update()
        {
            if (isPaused) 
            { 
                Pause(0f, true);
                CursorLockState.StateUnlocked();
                DisableActions();
            }
            else 
            { 
                Pause(1f, false);
                CursorLockState.StateLocked();
                EnableActions();
            }
        }
        #endregion

        #region Pause
        private void Pause(float timeScale, bool value)
        {

            //Time.timeScale = timeScale;
            //AudioListener.pause = value;

            pauseMenu.SetActive(value);

        }
        #endregion

        /**************** Input Action States ****************/

        #region Enable Input Actions
        private void EnableActions()
        {
            playerInput.actions.actionMaps
                .Where(m => m.name != "Pausing")
                .ToList().ForEach(m => m.Enable());
        }
        #endregion

        #region Disable Input Actions
        private void DisableActions()
        {

            playerInput.actions.actionMaps
                .Where(m => m.name != "Pausing")
                .ToList().ForEach(m => m.Disable());
        }
        #endregion

        
    }

}