using UnityEngine;
using UnityEngine.Splines;

public enum GameState
{
    None,
    Countdown,
    Race,
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

    public float ElapsedTime => _state == GameState.Race ?  Time.time - _time : 0f;

    GameUI _gameUI;
    GameState _state;
    SplineContainer _trackSpline;
    float _time;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _trackSpline = GameObject.Find("TrackSpline").GetComponent<SplineContainer>();
        _state = GameState.None;
    }

    void Start()
    {
        GameUI.BeginCountdown(5);
    }

    public void StartRace()
    {
        _state = GameState.Race;
        _time = Time.time;
    }

    public void StopRace()
    {
        _state = GameState.None;
    }
}