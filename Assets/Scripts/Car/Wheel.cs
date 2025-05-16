using Unity.VisualScripting;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool hasDrivePower = false;
    public bool canSteer = false;

    public float maxSteerAngle = 30f;
    public float brakeTorqueFactor = 4500f;

    [SerializeField]
    WheelCollider _collider;

    public WheelCollider Collider => _collider;

    public void ApplyTorque(float torque)
    {
        _collider.motorTorque = torque;
    }

    public void ApplyBrake(float torque)
    {
        _collider.brakeTorque = torque * brakeTorqueFactor;
    }
    public void ApplySteering(float steerInput)
    {
        float steerSpeed = 3.0f;
        float returnSpeedFactor = 8f;

        if (steerInput != 0)
        {
            float targetAngle = Mathf.Clamp(_collider.steerAngle + steerInput, -maxSteerAngle, maxSteerAngle);
            _collider.steerAngle = Mathf.Lerp(_collider.steerAngle, targetAngle, steerSpeed);
        }
        else
        {
            _collider.steerAngle = Mathf.Lerp(_collider.steerAngle, 0f, steerSpeed * Time.deltaTime * returnSpeedFactor);
        }
    }

    public void UpdateVisual()
    {
        Vector3 pos;
        Quaternion rot;
        _collider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }
}