using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class Drivetrain : MonoBehaviour
{
    Car _parentCar;
    Engine _engine;
    Transmission _transmission;
    [SerializeField]
    List<Wheel> _wheels;
    CarInputData _input;

    public float drivetrainLossFactor = 0.95f;

    public Engine Engine => _engine;
    public Transmission Transmission => _transmission;
    public IReadOnlyList<Wheel> Wheels => _wheels;

    void Awake()
    {
        _engine = GetComponent<Engine>();
        _transmission = GetComponent<Transmission>();
        _wheels = GetComponentsInChildren<Wheel>().ToList();
    }

    void FixedUpdate()
    {
        float fixedDelta = Time.fixedDeltaTime;
        float wheelRadius = 0.33f;
        float gearRatio = _transmission.CurrentGearRatio;
        float finalDrive = _transmission.FinalDrive;

        _engine.UpdateRPM(_parentCar, _input.throttle, wheelRadius, gearRatio, finalDrive, fixedDelta);

        float torque = 0f;
        float drivenEngineRPM = _wheels.Where(x=>x.hasDrivePower).Average(x=>x.Collider.rpm) * gearRatio * finalDrive;

        if(_engine.CurrentRPM >= drivenEngineRPM)
        {
            float rpmDifference = _engine.CurrentRPM - drivenEngineRPM;
            float rpmThreshold = 200f;

            float torqueFactor = Mathf.Clamp01(rpmDifference / rpmThreshold);
            torque = _engine.CalculateTorque(_input.throttle) * gearRatio * finalDrive * drivetrainLossFactor * torqueFactor;
        }

        foreach (Wheel wheel in _wheels)
        {
            if(wheel.canSteer)
                wheel.ApplySteering(_input.steerRight - _input.steerLeft);
            if (wheel.hasDrivePower)
            {
                wheel.ApplyTorque(torque/4f);
                wheel.ApplyBrake(_input.brake);
            }
            wheel.UpdateVisual();
        }
    }

    public void SetParentCar(Car car)
    {
        _parentCar = car;
    }

    public void SetInput(CarInputData input)
    {
        _input = input;
    }
}