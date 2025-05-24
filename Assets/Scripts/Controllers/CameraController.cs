using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    float _followSpeed = 7f;
    [SerializeField]
    float _rotationSpeed = 2f;

    float _cameraCollisionOffset = 0.2f;
    float _minCollisionOffset = 0.2f;

    [SerializeField]
    Transform _follow;

    [SerializeField]
    LayerMask _collisionMask;

    Transform _pivot;
    Transform _camera;
    Vector3 _defaultPosition;


    void Awake()
    {
        _camera = GetComponentInChildren<Camera>().transform;
        _pivot = _camera.parent;
        _defaultPosition = _camera.localPosition;
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if(_follow != null)
            FollowTarget(delta);

        HandleCameraCollision(delta);
    }

    void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.Lerp(transform.position, _follow.position, delta * _followSpeed);
        Quaternion targetRotation = Quaternion.Lerp(transform.rotation, _follow.rotation, delta * _rotationSpeed);
        targetRotation = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    void HandleCameraCollision(float delta)
    {
        float targetPos = _defaultPosition.z;
        Vector3 dir = (_camera.position - _pivot.position).normalized;
        Debug.DrawRay(_pivot.position, dir * Mathf.Abs(_defaultPosition.z), Color.red, 1f);
        if (Physics.Raycast(_pivot.position, dir, out RaycastHit hit, Mathf.Abs(targetPos), _collisionMask))
        {
            float distance = Vector3.Distance(_pivot.position, hit.point);
            targetPos = -(distance - _cameraCollisionOffset);
        }
        if (Mathf.Abs(targetPos) < _minCollisionOffset)
        {
            targetPos = -_minCollisionOffset;
        }

        targetPos = Mathf.Lerp(_camera.localPosition.z, targetPos, delta / 0.2f);
        _camera.localPosition = new Vector3(0, _defaultPosition.y, targetPos);
    }

}