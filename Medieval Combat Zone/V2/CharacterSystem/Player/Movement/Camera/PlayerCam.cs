using CombatZone.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Player
{

    public class PlayerCam : MonoBehaviour
    {

        [SerializeField] private bool isDeathCam = false;
        
        #region Camera Sensitivity
        [Header("Camera Sensitivity")]
        [SerializeField] private float sensX;
        [SerializeField] private float sensY;
        #endregion

        #region Camera Move
        [Header("Camera Move")]
        [Tooltip("Only for reading the input")]
        [SerializeField] private Vector2 cameraMove;
        #endregion

        #region Orientation & Rotation
        [Header("Orientation & Rotation")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform minimapIcon;
        private float xRotation;
        private float yRotation;
        #endregion

        /**************** Input Actions ****************/

        #region Camera Movement
        public void OnCameraMovement(InputAction.CallbackContext context)
        { 
            cameraMove = context.ReadValue<Vector2>();
        }
        #endregion

        /**************** Start, Update, Etc. ****************/

        #region Start
        private void Start()
        {
            CursorLockState.StateLocked();
        }
        #endregion

        #region Update
        private void Update()
        {
            // get mouse input
            //X is left and right. Y is up and down.
            float mouseX = cameraMove.x * Time.deltaTime * sensX;
            float mouseY = cameraMove.y * Time.deltaTime * sensY;
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            //Update if its not the death camera
            if (!isDeathCam)
            {
                minimapIcon.localRotation = Quaternion.Euler(0f, 0f, -yRotation);
            }
        }
        #endregion

    }

}