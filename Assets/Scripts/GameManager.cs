using UnityEngine;
using UnityEngine.Splines;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public enum GameState
{
    None,
    Countdown,
    Race,
    Replay,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameUI GameUI {
        get 
        {
            if(_gameUI == null)
            {
                _gameUI = gameObject.AddComponent<GameUI>();
            }
            return _gameUI;
        } 
    }
    public GameState GameState => _state;
    public LeaderBoard leaderBoard;

    public float ElapsedTime => Time.time - _time;
    public float LastLapTime = 0f;

    GameUI _gameUI;
    GameState _state;
    SplineContainer _trackSpline;
    public CarController carController;
    public CarReplayRecorder replayRecorder;

    float _time;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        carController = FindAnyObjectByType<CarController>();
        _trackSpline = GameObject.Find("TrackSpline").GetComponent<SplineContainer>();
        _state = GameState.None;

        leaderBoard = GetComponent<LeaderBoard>();
        if(leaderBoard == null)
            leaderBoard = gameObject.AddComponent<LeaderBoard>();
        replayRecorder = GetComponent<CarReplayRecorder>();
        if(replayRecorder == null)
            replayRecorder = gameObject.AddComponent<CarReplayRecorder>();
    }

    void Start()
    {
        SetUpEnvironments();
        Init();
        GameUI.BeginCountdown(5, GameState.Race);
    }

    public void SetUpEnvironments()
    {
        if (GameSettings.SelectedWeather == "Clear")
        {
            RenderSettings.fogDensity = 0f;
            GameObject.Find("Rain").SetActive(false);
        }
        else if (GameSettings.SelectedWeather == "Fog")
        {
            RenderSettings.fogDensity = 0.1f;
            GameObject.Find("Rain").SetActive(false);
        }
        else if(GameSettings.SelectedWeather == "Rain")
        {
            RenderSettings.fogDensity = 0f;
            GameObject.Find("Rain").SetActive(true);
        }

    }

    public void Init()
    {
        Time.timeScale = 1f;
        float t = 0f;
        Vector3 pos = _trackSpline.EvaluatePosition(t);
        Vector3 forward = _trackSpline.EvaluateTangent(t);
        Quaternion rot = Quaternion.LookRotation(forward, Vector3.up);

        Car prev = FindAnyObjectByType<Car>();
        if(prev != null)
            Destroy(prev.gameObject);

        GameObject go = GameObject.Instantiate(Resources.Load($"Prefabs/Car/{GameSettings.SelectedVehicle}_{GameSettings.SelectedDriveType}"), pos, rot) as GameObject;
        CameraController cameraController = FindAnyObjectByType<CameraController>();
        cameraController.follow = go.transform;
        carController.car = go.GetComponent<Car>();
    }

    public void BeginRace()
    {
        _time = Time.time;
        _state = GameState.Race;
    }

    public void FinishRace()
    {
        LastLapTime = ElapsedTime;

        var replayPlayer = carController.car.GetComponent<CarReplayPlayer>();
        if (replayPlayer.IsPlaying)
        {
            replayPlayer.StopPlayback();
        }
        else
        {
            leaderBoard.Add(LastLapTime);
        }

        _gameUI.resultPopup.UpdateLeaderboard(leaderBoard.GetFormattedLeaderboard());
        _gameUI.resultPopup.gameObject.SetActive(true);
        _state = GameState.None;
        replayRecorder.StopRecording();
        _gameUI.lapTimeUI.SetText("00:00:00");
        Time.timeScale = 0f; 
    }
    public void StartReplay()
    {
        _time = Time.time;
        _state = GameState.Replay;
        carController.car.GetComponent<CarReplayPlayer>().StartPlayback(replayRecorder.recorded);
    }
}