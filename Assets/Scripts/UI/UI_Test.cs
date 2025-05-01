using System;
using UnityEngine;

public class UI_Test : MonoBehaviour
{
    public Car car;
    public TMPro.TextMeshProUGUI gearText;
    public TMPro.TextMeshProUGUI rpmText;
        
    void Update()
    {
        gearText.text = $"{car.Drivetrain.Transmission.CurrentGear + 1}";
        rpmText.text = $"{Math.Round(car.Drivetrain.Engine.CurrentRPM)}";
    }
}