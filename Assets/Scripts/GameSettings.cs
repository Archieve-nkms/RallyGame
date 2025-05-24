using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static string SelectedVehicle;
    public static string SelectedDriveType;
    public static string SelectedTrack;
    public static string SelectedWeather;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
