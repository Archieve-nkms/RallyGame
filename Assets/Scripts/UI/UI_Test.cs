using System;
using System.Linq;
using UnityEngine;

public class UI_Test : MonoBehaviour
{
    public Car car;
    public TMPro.TextMeshProUGUI gearText;
    public TMPro.TextMeshProUGUI rpmText;
    public TMPro.TextMeshProUGUI speedText;
    public TMPro.TextMeshProUGUI wheelRPMText;

    void Update()
    {
        wheelRPMText.text = $"{Math.Round(car.Drivetrain.Wheels.Where(x => x.hasDrivePower).Average(x => x.Collider.rpm))}";
        gearText.text = $"{car.Drivetrain.Transmission.CurrentGear + 1}";
        rpmText.text = $"{Math.Round(car.Drivetrain.Engine.CurrentRPM)}";
        speedText.text = $"{Math.Round(car.Velocity * 3.6f)}";
    }
}