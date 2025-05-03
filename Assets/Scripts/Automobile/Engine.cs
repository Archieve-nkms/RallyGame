using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public float idleRPM = 800f;
    public float maxRPM = 9000f;
    public float maxTorque = 450f;
    public AnimationCurve torqueCurve;
    public float engineInertia = 0.25f;
    public float brakeCoefficient = 0.0001f;
    public float icreasedRPMMultiplier = 0.1f;

    float _currentRPM = 0f;
    public float CurrentRPM => _currentRPM;

    void Awake()
    {
        torqueCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(1750f, maxTorque),
            new Keyframe(4500f, maxTorque),
            new Keyframe(5500f, maxTorque * 0.8f),
            new Keyframe(maxRPM, maxTorque * 0.5f)
        );
    }

    public void UpdateRPM(Car car, float throttle, float wheelRadius, float gearRatio, float finalDrive, float delta)
    {
        float engineTorque = torqueCurve.Evaluate(_currentRPM);
        float loadTorque = CalculateLoadTorque(car.Rb.mass, car.Velocity, gearRatio, finalDrive, wheelRadius);
        float netTorque = (engineTorque - loadTorque) * throttle;
        float deltaRPM = 0;

        if (car.Velocity < 0.5f || gearRatio <= 0.01f)
        {
            float drag = -brakeCoefficient * _currentRPM * 100f;
            deltaRPM = (60f / (2f * Mathf.PI)) * (drag / engineInertia) * delta;
        }
        else if (throttle == 0)
        {
            float engineBrakeTorque = -brakeCoefficient * _currentRPM * gearRatio * finalDrive * car.Velocity;
            deltaRPM = (60f / (2f * Mathf.PI)) * (engineBrakeTorque / engineInertia) * delta;
        }
        else
        {
            deltaRPM = (60f / (2f * Mathf.PI)) * (netTorque * gearRatio / engineInertia * wheelRadius) * icreasedRPMMultiplier * delta;
        }

        _currentRPM = Mathf.Clamp(_currentRPM + deltaRPM, idleRPM, maxRPM);
    }

    float CalculateLoadTorque(float carMass, float velocity, float gearRatio, float finalDrive, float wheelRadius)
    {
        float airDensity = 1.225f; // kg/m©ø
        float dragCoefficient = 0.35f;
        float frontalArea = 2.2f; // m©÷
        float dragForce = 0.5f * airDensity * dragCoefficient * frontalArea * velocity * velocity;
        float rollingResistanceCoeff = 0.015f;
        float rollingResistance = rollingResistanceCoeff * carMass * 9.81f;
        float totalResistanceForce = dragForce + rollingResistance;
        float wheelTorque = totalResistanceForce * wheelRadius;
        float drivelineRatio = gearRatio * finalDrive;
        float engineLoadTorque = wheelTorque / Mathf.Max(drivelineRatio, 0.01f);

        return engineLoadTorque;
    }

    public void UpdateRPM(float newRPM)
    {
        _currentRPM = newRPM;
    }

    public float CalculateTorque(float throttle)
    {
        float curveTorque = torqueCurve.Evaluate(_currentRPM);

        float torque = curveTorque * throttle;
        return torque;
    }
}