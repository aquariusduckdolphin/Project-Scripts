using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    private float moveSpeed;

    public float walkSpeed = 7f;

    public float sprintSpeed = 10f;

    public float groundDrag = 4f;

    [Header("Jumping")]
    public float jumpForce = 12f;

    public float jumpCooldown = 0.25f;

    public float airMultiplier = 0.4f;

    private bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed = 5f;

    public float crouchYScale = 0.5f;

    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    public KeyCode sprintKey = KeyCode.LeftShift;

    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight = 2f;

    public LayerMask whatIsGround;

    bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 45f;

    private RaycastHit slopeHit;

    private bool exitingSlope;

    [Header("References")]
    public Transform orientation;

    private float horizontalInput;

    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    public MovementState state;

    public enum MovementState { Walking, Sprinting, Crouching, Air }

    private void Start()
    {

        orientation = transform.GetChild(1).gameObject.transform;

        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

    }

    private void Update()
    {

        GroundDetection();

        MyInput();

        SpeedControl();

        StateHandler();

        // handle drag
        if (isGrounded)

            rb.drag = groundDrag;

        else

            rb.drag = 0;

    }

    private void FixedUpdate()
    {

        MovePlayer();

    }

    private void MyInput()
    {

        horizontalInput = Input.GetAxisRaw("Horizontal");

        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {

            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {

            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {

            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        
        }

    }

    private void StateHandler()
    {

        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {

            state = MovementState.Crouching;

            moveSpeed = crouchSpeed;

        }

        // Mode - Sprinting
        else if (isGrounded && Input.GetKey(sprintKey))
        {

            state = MovementState.Sprinting;

            moveSpeed = sprintSpeed;

        }

        // Mode - Walking
        else if (isGrounded)
        {

            state = MovementState.Walking;

            moveSpeed = walkSpeed;

        }

        // Mode - Air
        else
        {

            state = MovementState.Air;

        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {

            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {

                rb.AddForce(Vector3.down * 80f, ForceMode.Force);

            }

        }
        // on ground
        else if (isGrounded)
        {

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }
        // in air
        else if (!isGrounded)
        {

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        }
        // turn gravity off while on slope
        rb.useGravity = !OnSlope();

    }

    private void SpeedControl()
    {

        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {

            if (rb.velocity.magnitude > moveSpeed)
            {

                rb.velocity = rb.velocity.normalized * moveSpeed;

            }

        }
        // limiting speed on ground or in air
        else
        {

            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {

               
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            
            }
        
        }
    
    }

    private void Jump()
    {

        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    
    }

    private void ResetJump()
    {

        readyToJump = true;

        exitingSlope = false;

    }

    private bool OnSlope()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            
            return angle < maxSlopeAngle && angle != 0;

        }

        return false;

    }

    private Vector3 GetSlopeMoveDirection()
    {

        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    
    }

    private bool GroundDetection()
    {

        // ground check
        return isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

    }

}
