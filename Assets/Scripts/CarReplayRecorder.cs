using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarState
{
    public float time;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public CarInputData input;

    public CarState(float time, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, CarInputData input)
    {
        this.time = time;
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
        this.input = input;
    }
}

public class CarReplayRecorder : MonoBehaviour
{
    public List<CarState> recorded = new();

    private bool isRecording = false;

    public void StartRecording()
    {
        recorded.Clear();
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void RecordInput(float time, Car car, CarInputData input)
    {
        if (isRecording)
        {
            CarState state = new CarState(
                time,
                car.transform.position,
                car.transform.rotation,
                car.Rb.linearVelocity,
                car.Rb.angularVelocity,
                input
                );

            recorded.Add(state);
        }
    }
}