using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextUI countdownUI;
    public TextUI lapTimeUI;
    public TextUI gearUI;
    public TextUI rpmUI;
    public TextUI velocityUI;
    public ResultPopup resultPopup;

    private void Awake()
    {
        countdownUI = GameObject.Find("Countdown").GetComponent<TextUI>();
        lapTimeUI = GameObject.Find("Lap Time").GetComponent<TextUI>();
        gearUI = GameObject.Find("Gear").GetComponent<TextUI>();
        rpmUI = GameObject.Find("RPM").GetComponent<TextUI>();
        velocityUI = GameObject.Find("Velocity").GetComponent<TextUI>();
        resultPopup = GameObject.Find("Result").GetComponent<ResultPopup>();

        resultPopup.gameObject.SetActive(false);
    }


    private void Update()
    {
        if(GameManager.Instance.carController.car != null)
        {
            Car car = GameManager.Instance.carController.car;

            string gear = car.Drivetrain.Transmission.CurrentGear == 0 ? "R" : 
                car.Drivetrain.Transmission.CurrentGear == 1 ? "N" : (car.Drivetrain.Transmission.CurrentGear - 1).ToString();
            gearUI.SetText($"{gear}");
            rpmUI.SetText($"{Mathf.Round(car.Drivetrain.Engine.CurrentRPM)}");
            velocityUI.SetText($"{Mathf.Round(car.Rb.linearVelocity.magnitude * 3.6f)}");
        }

        if(GameManager.Instance.GameState == GameState.Race)
        {
            float t = GameManager.Instance.ElapsedTime;
            int minutes = Mathf.FloorToInt(t / 60f);
            int seconds = Mathf.FloorToInt(t % 60f);
            int milliseconds = Mathf.FloorToInt((t * 1000f) % 1000f);

            lapTimeUI.SetText($"{minutes:00}:{seconds:00}.{milliseconds:000}");
        }
    }

    public void BeginCountdown(int n, GameState nextState)
    {
        countdownUI.gameObject.SetActive(true);
        StartCoroutine(CountDown(n, nextState));
    }

    IEnumerator CountDown(int n, GameState nextState)
    {
        for (int i = n; i > 0; i--)
        {
            countdownUI.SetText(i.ToString());
            yield return new WaitForSeconds(1f);
        }

        countdownUI.gameObject.SetActive(false);
        if(nextState == GameState.Race)
        {
            GameManager.Instance.BeginRace();
            GameManager.Instance.replayRecorder.StartRecording();
        }
        else if(nextState == GameState.Replay)
            GameManager.Instance.StartReplay();
            yield break;
    }
}