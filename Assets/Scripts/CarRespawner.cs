using UnityEngine.Splines;
using UnityEngine;
using System.Collections;

public class CarRespawner : MonoBehaviour
{
    public SplineContainer spline; // Ʈ�� spline
    public float maxAllowedDistance = 5f; // �� �Ÿ� �̻� ����� ������
    public float checkInterval = 0.2f; // �˻� �ֱ� (��)

    private Rigidbody rb;
    private float timeSinceLastCheck = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spline = FindAnyObjectByType<SplineContainer>();
    }

    void Update()
    {
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            timeSinceLastCheck = 0f;
            CheckDistanceAndRespawn();
        }
    }

    void CheckDistanceAndRespawn()
    {
        Vector3 currentPosition = transform.position;

        // spline �󿡼� ���� ����� ���� ã��
        float closestT = 0f;
        float closestDistance = float.MaxValue;

        for (float t = 0f; t <= 1f; t += 0.01f)
        {
            Vector3 splinePos = spline.EvaluatePosition(t);
            float dist = Vector3.Distance(currentPosition, splinePos);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestT = t;
            }
        }

        if (closestDistance > maxAllowedDistance)
        {
            RespawnAtSpline(closestT);
        }
    }

    void RespawnAtSpline(float t)
    {
        Vector3 pos = spline.EvaluatePosition(t);
        Quaternion rot = Quaternion.LookRotation(spline.EvaluateTangent(t));

        transform.position = pos;
        transform.rotation = rot;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StartCoroutine(ResetRigidbody());
    }

    System.Collections.IEnumerator ResetRigidbody()
    {
        rb.isKinematic = true;
        yield return null;
        rb.isKinematic = false;
    }
}
