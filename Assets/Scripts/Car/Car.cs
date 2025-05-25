using Unity.VisualScripting;
using UnityEngine;


public class Car : MonoBehaviour
{
    Drivetrain _drivetrain;
    Rigidbody _rigidBody;
    float _lastTickVelocity = 0;
    float _acceleration = 0;

    public Drivetrain Drivetrain => _drivetrain;
    public Rigidbody Rb => _rigidBody;
    public float Velocity => _rigidBody.linearVelocity.magnitude; // m/s ±âÁØ, 3.6 °öÇÏ¸é km/h
    public float Acceleration => _acceleration;

    void Awake()
    {
        _drivetrain = GetComponent<Drivetrain>();
        _rigidBody = GetComponent<Rigidbody>();

        _drivetrain.SetParentCar(this);
    }

    void FixedUpdate()
    {
        _acceleration = (Velocity - _lastTickVelocity) / Time.fixedDeltaTime;
        _lastTickVelocity = Velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.FinishRace();
            if(GetComponent<CarReplayPlayer>().IsPlaying)
                GetComponent<CarReplayPlayer>().StopPlayback();
        }
    }
}