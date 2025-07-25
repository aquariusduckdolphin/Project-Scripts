using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Player
{

    public class PlayerMovement : MonoBehaviour
    {

        /**************** Variables ****************/

        #region State Machine
        [Header("State Machine")]
        public MovementState state;
        public enum MovementState { walking, sprinting, crouching, sliding, air }
        #endregion

        #region Movement
        [Header("Movement Settings")]
        private float moveSpeed;

        public float walkSpeed;
        public float sprintSpeed;
        public bool isSprinting = false;

        [Space]
        public float slideSpeed;
        public bool sliding;

        [Space]
        private float desiredMoveSpeed;
        private float lastDesiredMoveSpeed;

        [Space]
        public float speedIncreaseMultiplier;
        public float slopeIncreaseMultiplier;
        public float groundDrag;
        
        [Header("Jumping")]
        private bool readyToJump;

        [Header("Crouching")]
        public bool isCrouching = false;
        private float startYScale;

        [Header("Ground Check")]
        public bool grounded;

        [Header("Slope Handling")]
        private RaycastHit slopeHit;
        private bool exitingSlope;
        #endregion

        #region Settings
        [Header("Settings")]
        [SerializeField] private PlayerMovementSettings movementSettings;
        public Transform orientation;

        private Rigidbody rb;

        //Input System Handler
        private PlayerControls inputHandler;

        [SerializeField] private static Vector2 move;
        private Vector3 moveDirection;
        public static Vector3 testDirection;
        #endregion

        private Coroutine limitSpeed;

        /**************** Input Action System Functions ****************/

        #region OnMove
        //Works for controller but weird with keyboard
        public void OnMove(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                move = context.ReadValue<Vector2>();
            }

            if (context.canceled)
            {
                move = Vector2.zero;
                return;
            }

        }
        #endregion

        #region Jump Input Action
        public void OnJump(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                if (readyToJump && grounded)
                {
                    readyToJump = false;
                    Jump();
                    Invoke(nameof(ResetJump), movementSettings.jumpCooldownDuration);
                }

            }

        }
        #endregion

        #region Crouch Input Action
        public void OnCrouch(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                isCrouching = true;
                transform.localScale = new Vector3(transform.localScale.x, movementSettings.crouchHeightScale, transform.localScale.z);
                //rb.AddForce(Vector3.down * 1f, ForceMode.Impulse);
            }

            if (context.canceled)
            {
                isCrouching = false;
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }

        }
        #endregion

        #region  Spinting Action
        public void OnSprinting(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                isSprinting = true;
            }

            if (context.canceled)
            {
                isSprinting = false;
            }

        }
        #endregion

        /**************** Start,Update & Fixed Update Function ****************/

        #region Start Function
        private void Start()
        {

            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            readyToJump = true;
            startYScale = transform.localScale.y;

        }
        #endregion

        #region Update
        private void Update()
        {

            // ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, movementSettings.playerHeight * 0.5f + 0.2f, movementSettings.groundLayer);

            SpeedControl();
            StateHandler();

            // handle drag
            if (grounded)
            {
                rb.linearDamping = groundDrag;
            }
            else
            {
                rb.linearDamping = 0;
            }

        }
        #endregion

        #region FixedUpdate
        private void FixedUpdate()
        {
            Movement();
        }
        #endregion

        /**************** Handler Functions ****************/

        #region State Handler
        private void StateHandler()
        {
            // Mode - Sliding
            if (sliding)
            {
                state = MovementState.sliding;

                if (OnSlope() && rb.linearVelocity.y < 0.1f)
                {
                    desiredMoveSpeed = slideSpeed;
                }
                else
                {
                    desiredMoveSpeed = sprintSpeed;
                }

            }
            // Mode - Crouching
            else if (isCrouching)
            {
                state = MovementState.crouching;
                desiredMoveSpeed = movementSettings.crouchSpeed;
            }
            // Mode - Sprinting
            else if (grounded && isSprinting)
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
            }
            // Mode - Walking
            else if (grounded)
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;
            }
            // Mode - Air
            else
            {
                state = MovementState.air;
            }

            // check if desiredMoveSpeed has changed drastically
            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
            {

                if(limitSpeed != null)
                {
                    StopCoroutine(limitSpeed);
                    limitSpeed = null;
                }

                limitSpeed = StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }

            lastDesiredMoveSpeed = desiredMoveSpeed;

        }
        #endregion

        #region Movement & Speed Functions

        #region Lerping Speed
        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            // smoothly lerp movementSpeed to desired value
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (time < difference)
            {

                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

                if (OnSlope())
                {
                    float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                    time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
                }
                else
                {
                    time += Time.deltaTime * speedIncreaseMultiplier;
                }

                yield return null;

            }

            moveSpeed = desiredMoveSpeed;

        }
        #endregion

        #region Movement

        public void Movement()
        {

            // calculate movement direction
            moveDirection = orientation.forward * move.y + orientation.right * move.x;
            testDirection = moveDirection;

            // on slope
            if (OnSlope() && !exitingSlope)
            {

                rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

                if (rb.linearVelocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }

            }

            // on ground
            else if (grounded)
            {
                rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
            }

            // in air
            else if (!grounded)
            {
                rb.AddForce(moveDirection * moveSpeed * 10f * movementSettings.airMultiplier, ForceMode.Force);
            }

            // turn gravity off while on slope
            rb.useGravity = !OnSlope();

        }
        #endregion

        #region Speed Control
        private void SpeedControl()
        {

            // limiting speed on slope
            if (OnSlope() && !exitingSlope)
            {

                if (rb.linearVelocity.magnitude > moveSpeed)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
                }

            }

            // limiting speed on ground or in air
            else
            {

                Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

                // limit velocity if needed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
                }

            }

        }
        #endregion

        #endregion

        #region Jump Function

        #region Jump
        private void Jump()
        {
            exitingSlope = true;
            // reset y velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * movementSettings.jumpForce, ForceMode.Impulse);
        }
        #endregion

        #region ResetJump
        private void ResetJump()
        {
            readyToJump = true;
            exitingSlope = false;
        }
        #endregion

        #endregion

        #region Slope Function

        #region OnSlope
        public bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, movementSettings.playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < movementSettings.maxSlopeAngle && angle != 0;
            }

            return false;
        }
        #endregion

        #region GetSlopeMoveDirection
        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }

        #endregion

        #endregion

        #region RayCast For Grounded
        private void OnDrawGizmosSelected()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, movementSettings.playerHeight * 0.5f + 0.2f, movementSettings.groundLayer))
            {
                Debug.DrawLine(transform.position, hit.point, Color.blue);
            }

            else
            {
                Debug.DrawLine(transform.position, transform.position + Vector3.down * (movementSettings.playerHeight * 0.5f + 0.2f), Color.red);
            }
        }
        #endregion

        /**************** Status Effect Reset ****************/

        #region Reset Walk and Sprint
        public IEnumerator ResetWalkAndSprint(float delayAmount)
        {
            yield return new WaitForSeconds(delayAmount);
            walkSpeed = movementSettings.defaultWalkSpeed;
            sprintSpeed = movementSettings.defaultSprintSpeed;
        }
        #endregion

    }

}