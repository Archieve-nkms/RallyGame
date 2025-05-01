using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

/* å��
 * ���� �� ���ӱ� �� ���۷��� �� �������� ���� ���� �帧 ����
 */

public class Drivetrain : MonoBehaviour
{
    Engine _engine;
    Transmission _transmission;
    List<Wheel> _wheels;
    CarInputData _input;

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
    }

    public void SetInput(CarInputData input)
    {
        _input = input;
    }
}