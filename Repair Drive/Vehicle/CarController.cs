using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public enum Axel
    {

        Front, Rear

    }

    [Serializable]
    public struct Wheel
    {

        public GameObject wheelModel;

        public WheelCollider wheelCollider;

        public Axel axel;

    }

    public float maxAcceleration = 30.0f;

    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;

    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    public float moveInput;

    public float steerInput;

    private Rigidbody carRB;

    public float interpolsteer = 0.6f;

    public float speed = 600f;

    // Start is called before the first frame update
    void Start()
    {

        carRB = GetComponent<Rigidbody>();

        carRB.centerOfMass = _centerOfMass;

    }

    // Update is called once per frame
    void Update()
    {

            GetInputs();

            //AnimateWheels();

            Brake();
        

    }

    void LateUpdate()
    {

        Move();
        Steer();

    }

    void GetInputs()
    {

        moveInput = -Input.GetAxis("Vertical");

        steerInput = Input.GetAxis("Horizontal");

    }

    void Move()
    {

        foreach(var wheel in wheels)
        {

            wheel.wheelCollider.motorTorque = moveInput * speed *  maxAcceleration * Time.deltaTime;

        }
        
    }

    void Steer()
    {

        foreach(var wheel in wheels)
        {

            if(wheel.axel == Axel.Front)
            {

                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;

                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, interpolsteer);

            }

        }

    }

    void Brake()
    {

        if (Input.GetKey(KeyCode.Space))
        {

            foreach(var wheel in wheels)
            {

                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;

            }

        }
        else
        {

            foreach(var wheel in wheels)
            {

                wheel.wheelCollider.brakeTorque = 0f;

            }

        }

    }

    void AnimateWheels()
    {

        foreach(var wheel in wheels)
        {

            Quaternion rot;

            Vector3 pos;

            wheel.wheelCollider.GetWorldPose(out pos, out rot);

            wheel.wheelModel.transform.position = pos;

            wheel.wheelModel.transform.rotation = rot;

        }

    }

}
