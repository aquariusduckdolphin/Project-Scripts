using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Player
{

    public class Sliding : MonoBehaviour
    {

        #region Varables

        #region References
        [Header("References")]
        public Transform orientation;
        public Transform playerObj;
        private Rigidbody rb;
        private PlayerMovementAdvanced pm;
        #endregion

        #region Sliding Varables
        [Header("Sliding")]
        public float maxSlideTime;
        public float slideForce;
        private float slideTimer;
        public float slideYScale;
        #endregion

        private float startYScale;

        #region PlayerInputSystem
        [Header("Input")]
        private PlayerControls controls;
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Input Action System
        public void OnSlideMovement(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                StartSlide();
                print("Sliding");

            }

            if (context.canceled && pm.sliding)
            {

                StopSlide();
                print("Stopped Sliding");
            }

        }
        #endregion

        //////////////////////////////////////////////////////

        #region Start, Update, Etc.

        #region Start
        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            pm = GetComponent<PlayerMovementAdvanced>();

            startYScale = playerObj.localScale.y;
        }
        #endregion

        #region FixedUpdate
        private void FixedUpdate()
        {

            if (pm.sliding) { SlidingMovement(); }

        }
        #endregion

        #endregion

        #region Slide Functions

        #region StartSlide
        private void StartSlide()
        {

            pm.sliding = true;

            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);

            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            slideTimer = maxSlideTime;

        }
        #endregion

        #region SlidingMovement
        private void SlidingMovement()
        {
            Vector3 inputDirection = PlayerMovementAdvanced.testDirection;

            // sliding normal
            if (!pm.OnSlope() || rb.linearVelocity.y > -0.1f)
            {

                rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

                slideTimer -= Time.deltaTime;

            }
            // sliding down a slope
            else
            {

                rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);

            }

            if (slideTimer <= 0) { StopSlide(); }

        }
        #endregion

        #region StopSlide
        private void StopSlide()
        {

            pm.sliding = false;

            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);

        }
        #endregion

        #endregion

    }

}