using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    float _followSpeed = 7f;

    [SerializeField]
    Transform _follow;
    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if(_follow != null )
            FollowTarget(delta);
    }

    void FollowTarget(float delta)
    {
        float speed = delta * _followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, _follow.position, speed);
        Quaternion targetRotation = Quaternion.Lerp(transform.rotation, _follow.rotation, speed);
        targetRotation = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}