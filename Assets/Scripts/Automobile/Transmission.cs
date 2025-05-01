using System;
using System.Collections.Generic;
using UnityEngine;

/* å��
 * ��� ���� ó�� (ShiftUp, ShiftDown)
 * ���� �� ���� ��ũ�� ��ȯ
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