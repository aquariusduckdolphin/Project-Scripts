using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Player
{

    public class PlayerCam : MonoBehaviour
    {

        #region Varables

        #region Camera Sensitivity
        [Header("Camera Sensitivity")]
        [SerializeField] private float sensX;

        [SerializeField] private float sensY;
        #endregion

        #region Camera Move
        [Header("Camera Move")]
        public Vector2 cameraMove;
        #endregion

        #region Orientation & Rotation
        [Header("Orientation & Rotation")]
        [SerializeField] private Transform orientation;

        private float xRotation;

        private float yRotation;
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Input Action System
        public void OnCameraMovement(InputAction.CallbackContext context)
        {

            cameraMove = context.ReadValue<Vector2>();

            if (context.performed)
            {
                print("Camera is Moving");
            }

        }
        #endregion

        //////////////////////////////////////////////////////

        #region Start, Update, Etc.

        #region Start
        private void Start()
        {

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;

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

        }
        #endregion

        #endregion

    }

}