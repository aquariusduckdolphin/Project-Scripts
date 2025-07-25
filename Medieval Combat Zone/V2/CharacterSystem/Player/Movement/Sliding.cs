using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Player
{

    public class Sliding : MonoBehaviour
    {

        #region Varables

        #region References
        [Header("Sliding Settings")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform playerObj;
        private PlayerMovement movement;
        private Rigidbody rb;
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
            }

            if (context.canceled && movement.sliding)
            {
                StopSlide();
            }

        }
        #endregion

        //////////////////////////////////////////////////////

        #region Start, Update, Etc.

        #region Start
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            movement = GetComponent<PlayerMovement>();
            startYScale = playerObj.localScale.y;
        }
        #endregion

        #region FixedUpdate
        private void FixedUpdate()
        {
            if (movement.sliding)
            { 
                SlidingMovement(); 
            }
        }
        #endregion

        #endregion

        #region Slide Functions

        #region StartSlide
        private void StartSlide()
        {
            movement.sliding = true;
            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            slideTimer = maxSlideTime;
        }
        #endregion

        #region SlidingMovement
        private void SlidingMovement()
        {

            Vector3 inputDirection = PlayerMovement.testDirection;

            // sliding normal
            if (!movement.OnSlope() || rb.linearVelocity.y > -0.1f)
            {
                rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
                slideTimer -= Time.deltaTime;
            }
            // sliding down a slope
            else
            {
                rb.AddForce(movement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
            }

            if (slideTimer <= 0) 
            { 
                StopSlide(); 
            }

        }
        #endregion

        #region StopSlide
        private void StopSlide()
        {
            movement.sliding = false;
            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        }
        #endregion

        #endregion

    }

}