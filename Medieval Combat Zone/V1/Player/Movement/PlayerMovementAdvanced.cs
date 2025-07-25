using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Player
{

    public class PlayerMovementAdvanced : MonoBehaviour
    {
        [Space]
        public int index = 0;
        [Space]

        //Input System Handler
        PlayerControls playerControls;

        #region Varables

        #region State Machine
        [Header("State Machine")]
        public MovementState state;

        public enum MovementState { walking, sprinting, crouching, sliding, air }
        #endregion

        #region Movement
        [Header("Movement")]
        private float moveSpeed;

        [Space]
        public float walkSpeed;

        [HideInInspector] public float defaultWalkSpeed;

        public float sprintSpeed;

        [HideInInspector] public float defaultSprintSpeed;

        [Space]
        public bool isSprinting = false;

        [Space]
        public float slideSpeed;

        [Space]
        public bool sliding;

        private float desiredMoveSpeed;

        private float lastDesiredMoveSpeed;

        [Space]
        public float speedIncreaseMultiplier;

        public float slopeIncreaseMultiplier;

        public float groundDrag;
        #endregion

        #region Jumping
        [Header("Jumping")]
        public float jumpForce;

        public float jumpCooldown;

        public float airMultiplier;

        bool readyToJump;
        #endregion

        #region Crouching
        [Header("Crouching")]
        public float crouchSpeed;

        public float crouchYScale;

        private float startYScale;

        public bool isCrouching = false;
        #endregion

        #region Ground Check
        [Header("Ground Check")]
        public float playerHeight;

        public LayerMask whatIsGround;

        public bool grounded;
        #endregion

        #region Slope
        [Header("Slope Handling")]
        public float maxSlopeAngle;

        private RaycastHit slopeHit;

        private bool exitingSlope;
        #endregion

        #region Other
        [Header("Other")]
        public Transform orientation;

        Rigidbody rb;

        public static Vector2 move;

        public Vector3 moveDirection;

        public static Vector3 testDirection;
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Input Action Systems Functions

        #region OnMove
        //Works for controller but weird with keyboard
        public void OnMove(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                move = context.ReadValue<Vector2>();

                print("Moving");

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

                    Invoke(nameof(ResetJump), jumpCooldown);

                }

                print("Jump");
            }

        }
        #endregion

        #region Crouch Input Action
        public void OnCrouch(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                isCrouching = true;

                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

                //rb.AddForce(Vector3.down * 1f, ForceMode.Impulse);

                print("Crouch");
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

                print("Sprinting");

            }

            if (context.canceled)
            {

                isSprinting = false;

            }

        }
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Start,Update & Fixed Update Function

        #region Start Function
        private void Start()
        {

            rb = GetComponent<Rigidbody>();

            rb.freezeRotation = true;

            readyToJump = true;

            startYScale = transform.localScale.y;

            defaultWalkSpeed = walkSpeed;

            defaultSprintSpeed = sprintSpeed;

        }
        #endregion

        #region Update
        private void Update()
        {

            // ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            SpeedControl();

            StateHandler();

            // handle drag
            if (grounded)

                rb.linearDamping = groundDrag;

            else

                rb.linearDamping = 0;

        }
        #endregion

        #region FixedUpdate
        private void FixedUpdate()
        {

            Movement();

        }
        #endregion

        #endregion

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

                desiredMoveSpeed = crouchSpeed;

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

                StopAllCoroutines();

                StartCoroutine(SmoothlyLerpMoveSpeed());

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

                rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);

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

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

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
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
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

            if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, whatIsGround))
            {
                Debug.DrawLine(transform.position, hit.point, Color.blue);
            }

            else
            {
                Debug.DrawLine(transform.position, transform.position + Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);
            }
        }
        #endregion

    }

}