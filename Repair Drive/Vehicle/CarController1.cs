using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController1 : MonoBehaviour
{

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private float horizontalInput;

    private float verticalInput;

    private float currentSteerAngle;

    private float currentBreakForce;

    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;


    [SerializeField] private WheelCollider frontWheel;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;

    [SerializeField] private Transform frontWheelTransform;
    [SerializeField] private Transform rearLeftTransform;
    [SerializeField] private Transform rearRightTransform;


    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GetInput();

        HandleMotor();

        HandleSteering();

        UpdateWheels();

    }

    private void GetInput()
    {
        
        horizontalInput = Input.GetAxis(Horizontal);

        verticalInput = Input.GetAxis(Vertical);

        isBreaking = Input.GetKey(KeyCode.Space);

    }

    private void HandleMotor()
    {

        frontWheel.motorTorque = verticalInput * motorForce;

        currentBreakForce = isBreaking ? breakForce : 0;

        if (isBreaking)
        {

            ApplyBreaking();

        }

    }

    private void ApplyBreaking()
    {

        frontWheel.brakeTorque = currentBreakForce;

        rearLeft.brakeTorque = currentBreakForce;

        rearRight.brakeTorque = currentBreakForce;

    }
    private void HandleSteering()
    {

        currentSteerAngle = maxSteerAngle * horizontalInput;

        frontWheel.steerAngle = currentSteerAngle;

    }


    private void UpdateWheels()
    {

        UpdateSingleWheel(frontWheel, frontWheelTransform);
        UpdateSingleWheel(rearLeft, rearLeftTransform);
        UpdateSingleWheel(rearRight, rearRightTransform);

    }

    private void UpdateSingleWheel(WheelCollider wheel, Transform wheelTransform)
    {

        Vector3 pos;

        Quaternion rot;

        wheel.GetWorldPose(out pos, out rot);
        
        wheelTransform.rotation = rot;

        wheelTransform.position = pos;

    }
}
