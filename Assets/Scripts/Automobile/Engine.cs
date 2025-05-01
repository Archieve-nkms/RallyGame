using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public float idleRPM = 800f;
    public float maxRPM = 9000f;
    public float maxTorque = 400f;

    public AnimationCurve torqueCurve;

    float _currentRPM = 0f;
    public float CurrentRPM => _currentRPM;

    void Awake()
    {

    }

    public void UpdateRPM(float targetWheelRPM, float throttle, float gearRatio, float finalDrive, bool clutchEngaged, float delta)
    {
    }

    public float CalculateTorque(float throttle)
    {
        float curveTorque = torqueCurve.Evaluate(_currentRPM / maxRPM);

        float torque = maxTorque * curveTorque * throttle;
        return torque;
    }
}