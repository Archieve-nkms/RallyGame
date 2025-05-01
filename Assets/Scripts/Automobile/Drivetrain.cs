using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

/* 책임
 * 엔진 → 변속기 → 디퍼렌셜 → 바퀴로의 동력 전달 흐름 제어
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