using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public Car car;
    
    PlayerInput _playerInput;
    InputSystem_Actions _actions;
    CarInputData _carInputData;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        if(_actions == null)
            _actions = new InputSystem_Actions();

        _actions.Car.Accelerate.performed += inputActions => _carInputData.throttle = inputActions.ReadValue<float>();
        _actions.Car.Accelerate.canceled += inputActions => _carInputData.throttle = 0;

        _actions.Car.Brake.performed += inputActions => _carInputData.brake = inputActions.ReadValue<float>();
        _actions.Car.Brake.canceled += inputActions => _carInputData.brake = 0;

        _actions.Car.SteerLeft.performed += inputActions => _carInputData.steerLeft = inputActions.ReadValue<float>();
        _actions.Car.SteerLeft.canceled += inputActions => _carInputData.steerLeft = 0;

        _actions.Car.SteerRight.performed += inputActions => _carInputData.steerRight = inputActions.ReadValue<float>();
        _actions.Car.SteerRight.canceled += inputActions => _carInputData.steerRight = 0;

        _actions.Car.Clutch.performed += inputActions => _carInputData.clutch = inputActions.ReadValueAsButton();
        _actions.Car.Clutch.canceled += inputActions => _carInputData.clutch = inputActions.ReadValueAsButton();

        _actions.Car.HandBrake.performed += inputActions => _carInputData.handBrake = inputActions.ReadValueAsButton();
        _actions.Car.HandBrake.canceled += inputActions => _carInputData.handBrake = inputActions.ReadValueAsButton();

        _actions.Car.ShiftUp.performed += inputActions => _carInputData.shiftUp = inputActions.ReadValueAsButton();
        _actions.Car.ShiftUp.canceled += inputActions => _carInputData.shiftUp = inputActions.ReadValueAsButton();

        _actions.Car.ShiftDown.performed += inputActions => _carInputData.shiftDown = inputActions.ReadValueAsButton();
        _actions.Car.ShiftDown.canceled += inputActions => _carInputData.shiftDown = inputActions.ReadValueAsButton();

        _actions.Enable();
    }

    void OnDisable()
    {
        _actions.Disable();
    }

    void Update()
    {
        if (GameManager.Instance.GameState != GameState.Race)
            return;

        GameManager.Instance.replayRecorder.RecordInput(GameManager.Instance.ElapsedTime, car, _carInputData);
        car.Drivetrain.SetInput(_carInputData);

        if (_carInputData.shiftUp) //юс╫ц
        {
            float prevSpeed = car.Velocity;
            float prevRPM = car.Drivetrain.Engine.CurrentRPM;
            float prevGearRatio = car.Drivetrain.Transmission.CurrentGearRatio;

            car.Drivetrain.Transmission.ShiftUp();
            float newGearRatio = car.Drivetrain.Transmission.CurrentGearRatio;
            if(newGearRatio != 0)
            {
                float newRPM = prevRPM * (newGearRatio / prevGearRatio);
                car.Drivetrain.Engine.UpdateRPM(newRPM);
            }
        }
        else if (_carInputData.shiftDown)
            car.Drivetrain.Transmission.ShiftDown();

        _carInputData.shiftUp = false;
        _carInputData.shiftDown = false;
    }
}