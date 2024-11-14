using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetraCreations.Attributes;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{

    public enum WolfState { Default, Human, Middle, Werewolf }

    public WolfState playerState = WolfState.Default;

    public enum MovementState { Walking, Sprinting, Dashing, Air }

    public MovementState movementState = MovementState.Walking;

    #region Universal stats
    [Title("Universal State", TitleColor.Aqua, TitleColor.Orange)]

    [Range(0, 100)]
    public float rageMeter = 0f;

    private Transform orientation;

    [Header("Private Variables")]
    private float horizontalInput;

    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    public GameObject meshCollider;

    public CapsuleCollider capsuleCollider;

    public GameObject humanPlayer;

    public GameObject werewolfPlayer;

    [Header("Ground Check")]
    public LayerMask whatIsGround;

    private float playerHeight = 2f;

    private bool isGrounded;

    public float groundDrag = 5f;

    [Header("Bindings")]
    public KeyCode jumpKey = KeyCode.Space;

    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Slope Handling")]
    public float maxSlopeAngle;

    private RaycastHit slopeHit;

    private bool exitingSlope;

    public float maxYSpeed;

    public float downForceSlope = 80f;

    [Header("Dashing")]
    public float dashSpeed;

    [HideInInspector] public bool dashing;

    public float dashSpeedChangeFactor;

    [Header("Different States")]
    private float desiredMoveSpeed;

    private float lastDesiredMoveSpeed;

    private MovementState lastState;

    private bool keepMomenturm;

    [Header("Lerp")]
    private float speedChangeFactor;

    public PlayerDashRaycast raycast;
    #endregion

    #region Default Variables
    [Title("Default State", TitleColor.Aqua, TitleColor.Orange)]

    [Header("Move")]
    public float walkSpeed = 7f;

    public float sprintSpeed = 10f;

    private float moveSpeed = 7f;

    [Header("Jump")]
    public float jumpForce = 12f;

    public float jumpCooldown = 0.25f;

    public float airMultiplier = 0.4f;

    public Animator baseAnimation;

    public float damage;

    [HideInInspector] public bool readyToJump;
    #endregion

    #region Human Variables
    [Title("Human State", TitleColor.Aqua, TitleColor.Orange)]

    public float humanWalkSpeed = 5f;

    public float humanSprintSpeed = 10f;

    public float humanJumpForce = 10f;

    public float HumanHeight = 5f;

    public Animator humanAnimation;

    public float humanDamage = 5f;

    #endregion

    #region Werewolf Variables
    [Title("Werewolf State", TitleColor.Aqua, TitleColor.Orange)]
    public float wolfWalkSpeed = 10f;

    public float wolfSprintSpeed = 15f;

    public float wolfJumpForce = 20f;

    public float wolfHeight = 10f;

    public Animator werewolfAnimation;

    public float werewolfDamage = 10f;
    #endregion

    #region Animation Variables
    public const string animVertical = "Vertical Running";

    public const string animHorizontal = "Horizontal Running";

    public const string animJump = "Jumping";

    public const string animAttack = "Punching";
    #endregion

    #region Collider Info
    [Header("Collider Info")]
    [Tooltip("The x is the radius of the collider. Y is the height of the collider.")]
    public Vector2 humanCapsuleCollider = new Vector2(0.4033203f, 1.828102f);

    [Tooltip("The x is the radius of the collier. Y is the height of the collider.")]
    public Vector2 wereworlfCapsuleCollider = new Vector2(0.5799789f, 2.686279f);
    #endregion

    public float time;

    public float time2;
    public float timeFrame = 15f;

    public float time2Frame = 5f;

    public PlayerHealth helath;

    #region Gathering Info
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        orientation = transform.GetChild(1);

        readyToJump = true;

        rageMeter = 0f;

    }
    #endregion

    #region Update 
    void Update()
    {

        //Shoot a raycast down to determine if there is a ground. 
        //Set the ground bool based off the result
        isGrounded = Physics.Raycast(transform.position,
                                     -Vector3.up,
                                     playerHeight * 0.5f + 0.2f, 
                                     whatIsGround);

        //Debug.DrawLine(transform.position, Vector3.down * playerHeight, Color.red);

        StateChange();

        PlayerStateChange();

        MyInput();

        SpeedControl();

        StateHandler();

        if (movementState == MovementState.Walking || movementState == MovementState.Sprinting )
        {

            //Add drag to the player controller
            //Stops from gliding on the floor
            rb.drag = groundDrag;

        }
        else
        {

            //This means the player is in the air so no drag is required.
            rb.drag = 0f;

        }

        time += Time.deltaTime;

        if(time > timeFrame)
        {

            rageMeter -= 5f;

            helath.currentHealth += 10f;

            time = 0f;

        }

        time2 += Time.deltaTime;

        if (time2 > time2Frame)
        {

            helath.currentHealth += 10f;

            time2 = 0f;

        }

        if (rageMeter  < 0f)
        {

            rageMeter = 0f;

        }

    }
    #endregion

    #region Physics
    private void FixedUpdate()
    {

        MovePlayer();

    }
    #endregion

    #region Input Function
    /// <summary>
    /// A function for player input
    /// </summary>
    /// <returns> Returns the input  </returns>
    private void MyInput()
    {

        //Get the S and W input = a value of -1 to 1
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //Get the A and D input = a value of -1 to 1
        verticalInput = Input.GetAxisRaw("Vertical");

        UpdateAnimation(horizontalInput, animHorizontal);

        UpdateAnimation(verticalInput, animVertical);

        //Allow the plyaer to jump up into the air
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {

            readyToJump = false;

            baseAnimation.SetBool(animJump, true);

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }

        if (!Input.GetKey(jumpKey))
        {

            baseAnimation.SetBool(animJump, false);

        }

        if (Input.GetMouseButton(0))
        {

            baseAnimation.SetBool(animAttack, true);

        }
        
        if(!Input.GetMouseButton(0))
        {

            baseAnimation.SetBool(animAttack, false);

        }

    }
    #endregion

    #region Different States
    /// <summary>
    /// Handles the various player movement states
    /// </summary>
    private void StateHandler()
    {

        //Mode - Dashing
        if (dashing)
        {

            movementState = MovementState.Dashing;

            desiredMoveSpeed = dashSpeed;

            speedChangeFactor = dashSpeedChangeFactor;

        }
        //Mode - Sprinting
        else if(isGrounded && Input.GetKey(sprintKey))
        {

            movementState = MovementState.Sprinting;

            desiredMoveSpeed = sprintSpeed;

        }
        //Mode - Walking
        else if (isGrounded)
        {

            movementState = MovementState.Walking;

            desiredMoveSpeed = walkSpeed;

        }
        //Mode - Air
        else
        {

            movementState = MovementState.Air;

            if(desiredMoveSpeed < sprintSpeed)
            {

                desiredMoveSpeed = sprintSpeed;

            }
            else
            {

                desiredMoveSpeed = sprintSpeed;

            }

        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if(lastState == MovementState.Dashing) { keepMomenturm = true; }

        if (desiredMoveSpeedHasChanged)
        {

            if (keepMomenturm)
            {

                StopAllCoroutines();


                StartCoroutine(SmoothlyLerpMoveSpeed());

            }
            else
            {


                StopAllCoroutines();

                moveSpeed = desiredMoveSpeed;

            }

        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        lastState = movementState;

    }
    #endregion

    #region Handles Player Physical Movement
    /// <summary>
    /// Move the player in the physical world.
    /// </summary>
    private void MovePlayer()
    {

        if(movementState == MovementState.Dashing) { return; }

        //Calculate movement direction
        moveDirection = orientation.forward * verticalInput + 
                        orientation.right * horizontalInput;

        //Debug.Log(OnSlope());

        //On slope
        if (OnSlope() && !exitingSlope)
        {

            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {

                rb.AddForce(Vector3.down * downForceSlope, ForceMode.Force);

            }

        }

        //On ground
        if (isGrounded)
        {

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }
        //in air
        else if(!isGrounded)
        {

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        }

        //Turn gravity off while on slope
        rb.useGravity = !OnSlope();

    }
    #endregion

    #region Speed on Slope/Ground
    /// <summary>
    /// Controls the speed of the controller depending on the situation.
    /// </summary>
    void SpeedControl()
    {

        //Limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {

            if(rb.velocity.magnitude > moveSpeed)
            {

                rb.velocity = rb.velocity.normalized * moveSpeed;

            }

        }
        //Limiting speed on ground or air
        else
        {

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit velocity if needed
            if (flatVelocity.magnitude > moveSpeed)
            {

                Vector3 limitedVelcotiy = flatVelocity.normalized * moveSpeed;

                rb.velocity = new Vector3(limitedVelcotiy.x, rb.velocity.y, limitedVelcotiy.z);

            }


        }

        //Limit y velocity
        if(maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {

            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);

        }

    }
    #endregion

    #region Jump Function
    /// <summary>
    /// A function that deals with Jumping on the character controller.
    /// </summary>
    void Jump()
    {

        exitingSlope = true;

        //Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }
    #endregion

    #region Rest Jump
    /// <summary>
    /// Resting jump to allow player to jump again.
    /// </summary>
    private void ResetJump()
    {

        readyToJump = true;

        exitingSlope = false;

    }
    #endregion

    #region Slope Function
    /// <summary>
    /// Test to see if the controller is on a slope.
    /// </summary>
    /// <returns> Return type bool </returns>
    private bool OnSlope()
    {

        if(Physics.Raycast(transform.position, 
                           Vector3.down, 
                           out slopeHit, 
                           playerHeight * 0.5f + 0.3f))
        {

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < maxSlopeAngle && angle != 0f;

        }

        return false;

    }

    /// <summary>
    /// Will fix the direction of the arrows when on a slope.
    /// </summary>
    /// <returns> Return type of Vector3 </returns>
    private Vector3 GetSlopeMoveDirection()
    {

        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;

    }
    #endregion

    #region Change Player State
    /// <summary>
    /// This is meant to change the player state
    /// </summary>
    private void PlayerStateChange()
    {

        switch (playerState)
        {

            case WolfState.Default:

                walkSpeed = 7f;

                sprintSpeed = 10f;

                moveSpeed = 7f;

                jumpForce = 12f;

                playerHeight = 2f;

                damage = 0f;

                break;
            
            case WolfState.Human:

                capsuleCollider.radius = humanCapsuleCollider.x;

                capsuleCollider.height = humanCapsuleCollider.y;

                walkSpeed = humanWalkSpeed;

                sprintSpeed = humanSprintSpeed;

                //moveSpeed = 7f;

                jumpForce = humanJumpForce;

                playerHeight = HumanHeight;

                baseAnimation = humanAnimation;

                damage = humanDamage;

                raycast.human = true;

                break;

            case WolfState.Middle:

                walkSpeed = CalculateAverage(humanWalkSpeed, wolfWalkSpeed);

                sprintSpeed = CalculateAverage(humanSprintSpeed, wolfSprintSpeed);

                //moveSpeed = 7f;

                jumpForce = CalculateAverage(humanJumpForce, wolfJumpForce);

                playerHeight = CalculateAverage(HumanHeight, wolfHeight);

                damage = CalculateAverage(humanDamage, werewolfDamage);

                raycast.human = true;

                break;

            case WolfState.Werewolf:

                capsuleCollider.radius = wereworlfCapsuleCollider.x;

                capsuleCollider.height = wereworlfCapsuleCollider.y;

                capsuleCollider.center = new Vector3(0f, 0f, 0f);

                walkSpeed = wolfWalkSpeed;

                sprintSpeed = wolfSprintSpeed;

                //moveSpeed = 7f;

                jumpForce = wolfJumpForce;

                playerHeight = wolfHeight;

                baseAnimation = werewolfAnimation;

                damage = werewolfDamage;

                raycast.human = false;

                break;

        }

    }
    #endregion

    #region Average Calculation
    /// <summary>
    /// Find the average between two values.
    /// </summary>
    /// <param name="a"> Pass in the first value </param>
    /// <param name="b"> Pass in the second value</param>
    /// <returns> A float value </returns>
    float CalculateAverage(float a, float b)
    {

        float average;

        return average = (a + b) / 2;

    }
    #endregion

    #region Handle Rage Meter
    /// <summary>
    /// The function will change the player state
    /// </summary>
    void StateChange()
    {

        if (rageMeter >= 75)
        {

            playerState = WolfState.Werewolf;

            humanPlayer.SetActive(false);

            werewolfPlayer.SetActive(true);

        }
        else if(rageMeter >= 50)
        {

            playerState = WolfState.Middle;

        }
        else if(rageMeter <= 50)
        {

            playerState = WolfState.Human;

            humanPlayer.SetActive(true);

            werewolfPlayer.SetActive(false);

        }

    }
    #endregion

    #region Lerp Function
    IEnumerator SmoothlyLerpMoveSpeed()
    {

        //smoothly lerp movementSpeed to desired value
        float time = 0f;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);

        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while(time < difference)
        {

            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference); ;

            time += Time.deltaTime * boostFactor;

            yield return null;

        }

        moveSpeed = desiredMoveSpeed;

        speedChangeFactor = 1f;

        keepMomenturm = false;

    }
    #endregion

    #region Animation Ccontroller
    /// <summary>
    /// This will update the animations to match with movement
    /// </summary>
    /// <param name="input"> The input </param>
    /// <param name="animationName"> The variable name in the animator </param>
    void UpdateAnimation(float input, string animationName)
    {

        if (input > 0)
        {

            baseAnimation.SetFloat(animationName, 1);

        }
        else if (input < 0)
        {

            baseAnimation.SetFloat(animationName, -1);

        }
        else
        {

            baseAnimation.SetFloat(animationName, 0);

        }

    }
    #endregion

}