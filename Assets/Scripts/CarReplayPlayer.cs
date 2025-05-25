using System.Collections.Generic;
using UnityEngine;

public class CarReplayPlayer : MonoBehaviour
{
    Car _car;
    List<CarState> records = new();
    int _currentIndex;
    CarState prev;

    public bool IsPlaying { get; private set; } = false;

    private void Awake()
    {
        _car = GetComponent<Car>();
    }

    public void StartPlayback(List<CarState> recorded)
    {
        _car.Rb.isKinematic = true;

        records = recorded;
        _currentIndex = 0;
        IsPlaying = true;
    }

    public void StopPlayback()
    {
        _car.Rb.isKinematic = false;

        IsPlaying = false;
    }

    void FixedUpdate()
    {
        if (_currentIndex >= records.Count || !IsPlaying) return;

        float elapsed = GameManager.Instance.ElapsedTime;
        prev = null;

        while (_currentIndex < records.Count && records[_currentIndex].time <= elapsed)
        {
            prev ??= records[_currentIndex];

            ApplyInput(records[_currentIndex], elapsed);
            prev = records[_currentIndex];
            _currentIndex++;
        }
    }

    void ApplyInput(CarState state, float elpased)
    {
        float t = Mathf.InverseLerp(prev.time, state.time, elpased);

        _car.Rb.MovePosition(Vector3.Lerp(prev.position, state.position, Time.fixedDeltaTime));
        _car.Rb.MoveRotation(Quaternion.Slerp(prev.rotation, state.rotation, Time.fixedDeltaTime));
        _car.Rb.linearVelocity = state.velocity;
        _car.Rb.angularVelocity = state.angularVelocity;

        CarInputData currentInput = state.input;

        _car.Drivetrain.SetInput(currentInput);

        if (currentInput.shiftUp) //юс╫ц
        {
            float prevSpeed = _car.Velocity;
            float prevRPM = _car.Drivetrain.Engine.CurrentRPM;
            float prevGearRatio = _car.Drivetrain.Transmission.CurrentGearRatio;

            _car.Drivetrain.Transmission.ShiftUp();
            float newGearRatio = _car.Drivetrain.Transmission.CurrentGearRatio;
            if (newGearRatio != 0)
            {
                float newRPM = prevRPM * (newGearRatio / prevGearRatio);
                _car.Drivetrain.Engine.UpdateRPM(newRPM);
            }
        }
        else if (currentInput.shiftDown)
            _car.Drivetrain.Transmission.ShiftDown();
    }
}