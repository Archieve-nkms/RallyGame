using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Car : MonoBehaviour
{
    Drivetrain _drivetrain;
    Rigidbody _rigidBody;

    public Drivetrain Drivetrain => _drivetrain;

    void Awake()
    {
        _drivetrain ??= gameObject.AddComponent<Drivetrain>();
        _rigidBody = GetComponent<Rigidbody>();
    }
}