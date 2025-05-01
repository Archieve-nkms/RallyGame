using System;
using System.Collections.Generic;
using UnityEngine;

/* 책임
 * 기어 변경 처리 (ShiftUp, ShiftDown)
 * 현재 기어에 따라 토크를 변환
 */

public class Transmission : MonoBehaviour
{
    [Header("Transmission Simulation")]
    public List<float> gearRatios;
    public float finalDrive;
    
    int _currentGear;

    public int CurrentGear => _currentGear;
    public float CurrentGearRatio => gearRatios[_currentGear];
    public float FinalDrive => finalDrive;

    public void ShiftUp()
    {
        if (_currentGear < gearRatios.Count - 1)
        {
            _currentGear++;
        }
    }

    public void ShiftDown()
    {
        if (_currentGear > 0)
        {
            _currentGear--;
        }
    }
}