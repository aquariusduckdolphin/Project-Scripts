using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetraCreations.Attributes;
using UnityEngine.UI;

public class AbilitiesScript : MonoBehaviour
{

    #region References
    [Title("References", TitleColor.Aqua, TitleColor.Orange)]
    private Transform orientation;

    private Transform playerCam;

    private Rigidbody rb;

    private PlayerMovement movement;

    private Image dashIcon;

    private Image whirlWindIcon;

    [Header("Raycast Info")]
    RaycastHit hit;

    public float raycastLength = 10f;
    #endregion

    #region Settings
    [Title("Settings", TitleColor.Aqua, TitleColor.Orange)]
    public bool useCameraForward = true;

    public bool allowAllDirections = true;

    public bool disableGravity = false;

    public bool restVel = true;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.Alpha2;

    public KeyCode whirlWind = KeyCode.Alpha1;
    #endregion

    #region Dashing Variables
    [Title("Dashing References", TitleColor.Aqua, TitleColor.Orange)]
    public float dashForce;

    public float dashUpwardForce;

    public float maxDashYSpeed;

    [Title("Dashing Cool Down", TitleColor.Aqua, TitleColor.Orange)]
    public float dashDuration;

    public float dashCoolDown;

    public float dashCoolDownTimer;

    public float dashCoolDownTime = 5f;
    #endregion

    public bool killing = false;

    #region Whirl Wind Variables
    [Title("Whirl Wind References", TitleColor.Aqua, TitleColor.Orange)]
    public GameObject whirlWindEffect;

    public Vector3 offset;

    [Title("Whirl Wind Cool Down", TitleColor.Aqua, TitleColor.Orange)]
    public float whirlWindDuration;

    public float whirlWindCoolDownTime;

    public float whirlWindCoolDownTimer;
    #endregion

    #region Gather Info
    void Start()
    {

        rb = GetComponent<Rigidbody>();

        movement = GetComponent<PlayerMovement>();

        orientation = transform.GetChild(1);

        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        dashIcon = GameObject.Find("Dash Fill").GetComponent<Image>();

        whirlWindIcon = GameObject.Find("Whirl Wind Fill").GetComponent<Image>();

        //This makes the fill for dash work nicely
        dashCoolDownTime = dashCoolDown;

        dashCoolDownTimer = 0f;

        whirlWindCoolDownTimer = 0f;

    }
    #endregion

    #region Update
    void Update()
    {

        if (Input.GetKeyDown(dashKey))
        {

            if (!killing)
            {

                StartCoroutine(WaitBeforeDashing(0.5f));

            }

        }

        if (Input.GetKeyDown(whirlWind))
        {

            WhirlWind();

        }

        if (dashCoolDownTimer > 0)
        {

            dashCoolDownTimer -= Time.deltaTime;

            dashIcon.fillAmount = dashCoolDownTimer / dashCoolDownTime;

        }
        else
        {

            dashIcon.fillAmount = 1f;

        }

        if (whirlWindCoolDownTimer > 0)
        {

            whirlWindCoolDownTimer -= Time.deltaTime;

            whirlWindIcon.fillAmount = whirlWindCoolDownTimer / whirlWindCoolDownTime;

        }
        else
        {

            whirlWindIcon.fillAmount = 1f;

        }

    }
    #endregion

    #region Dash
    void Dash()
    {

        if (dashCoolDownTimer > 0) { return; }
        else { dashCoolDownTimer = dashCoolDown; }

        movement.dashing = true;

        movement.maxYSpeed = maxDashYSpeed;

        Transform forwardT;

        if (useCameraForward)
        {

            forwardT = playerCam;

        }
        else
        {

            forwardT = orientation;

        }

        Vector3 direction = GetDirection(forwardT);
        
        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
        {

            rb.useGravity = false;

        }

        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);

    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {

        if (restVel)
        {

            rb.velocity = Vector3.zero;

        }

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);

    }

    private void ResetDash()
    {

        movement.dashing = false;

        movement.maxYSpeed = 0f;

        if (disableGravity)
        {

            rb.useGravity = true;

        }

    }

    private Vector3 GetDirection(Transform forwardT)
    {

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
        {

            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;

        }
        else
        {

            direction = forwardT.forward;

        }

        if (verticalInput == 0f && horizontalInput == 0)
        {

            direction = forwardT.forward;

        }

        return direction.normalized;

    }
    #endregion

    #region Whirl Wind
    void WhirlWind()
    {

        if (whirlWindCoolDownTimer > 0) { return; }
        else { whirlWindCoolDownTimer = whirlWindCoolDownTime; }

        Invoke(nameof(SpawnWhirlWind), whirlWindDuration);

    }

    void SpawnWhirlWind()
    {

        Instantiate(whirlWindEffect, orientation.position + offset, Quaternion.identity);

    }
    #endregion

    IEnumerator WaitBeforeDashing(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);

        Dash();

    }

}