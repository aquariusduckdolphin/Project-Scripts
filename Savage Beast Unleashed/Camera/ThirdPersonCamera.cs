using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetraCreations.Attributes;
using Unity.VisualScripting;

public class ThirdPersonCamera : MonoBehaviour
{

    public enum CameraStyle { Basic, Combat }

    public CameraStyle style;

    #region References
    [Title("References", TitleColor.Aqua, TitleColor.Orange)]
    public float rotationSpeed = 2f;

    private Transform orientation;

    private Transform player;

    private Transform appearance;

    private Rigidbody rb;

    [Header("Input")]
    private float horizontalInput;

    private float verticalInput;

    public GameObject basicCam;

    public GameObject combatCam;

    private Transform combatLookAt;

    private GameObject cursorDot;
    #endregion

    #region Gather Info
    private void Awake()
    {

        cursorDot = GameObject.Find("Cursor");

        player = GameObject.Find("Player").transform;

        orientation = GameObject.Find("Orientation").transform;

        appearance = GameObject.Find("Mesh and Collider").transform;

        rb = GameObject.Find("Player").GetComponent<Rigidbody>();

        combatLookAt = GameObject.Find("CombatLookAt").transform; 

    }

    public void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        cursorDot.SetActive(false);

    }
    #endregion

    #region Update
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.M))
        {

            SwitchCameraStyle(CameraStyle.Basic);

        }

        if (Input.GetKeyDown(KeyCode.N))
        {

            SwitchCameraStyle(CameraStyle.Combat);
        
        }

        //Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, 
                                                        player.position.y, 
                                                        transform.position.z);

        orientation.forward = viewDir.normalized;

        //Rotate the player Object
        if(style == CameraStyle.Basic)
        {

            horizontalInput = Input.GetAxis("Horizontal");

            verticalInput = Input.GetAxis("Vertical");

            Vector3 inputDir = orientation.forward * verticalInput +
                               orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {

                appearance.forward = Vector3.Slerp(appearance.forward,
                                                   inputDir.normalized,
                                                   Time.deltaTime * rotationSpeed);

            }

        }
        else if(style == CameraStyle.Combat)
        {

            Vector3 dirToComabtLookAt = combatLookAt.position - new Vector3(transform.position.x,
                                                        combatLookAt.position.y,
                                                        transform.position.z);

            orientation.forward = dirToComabtLookAt.normalized;

            player.forward = dirToComabtLookAt.normalized;

        }

    }
    #endregion

    #region Change Camera
    void SwitchCameraStyle(CameraStyle newStyle)
    {

        combatCam.SetActive(false);

        basicCam.SetActive(false);

        cursorDot.SetActive(false);

        if(newStyle == CameraStyle.Basic)
        {

            basicCam.SetActive(true);

            cursorDot.SetActive(false);

        }
        else if(newStyle == CameraStyle.Combat)
        {

            combatCam.SetActive(true);

            cursorDot.SetActive(true);

        }

    }
    #endregion

}
