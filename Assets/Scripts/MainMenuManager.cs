using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Dropdown vehicleDropdown;
    public TMP_Dropdown driveTypeDropdown;
    public TMP_Dropdown trackDropdown;
    public TMP_Dropdown weatherDropdown;

    public void OnStartButtonClicked()
    {
        GameSettings.SelectedVehicle = vehicleDropdown.options[vehicleDropdown.value].text;
        GameSettings.SelectedDriveType = driveTypeDropdown.options[driveTypeDropdown.value].text;
        GameSettings.SelectedTrack = trackDropdown.options[trackDropdown.value].text;
        GameSettings.SelectedWeather = weatherDropdown.options[weatherDropdown.value].text;

        SceneManager.LoadScene(GameSettings.SelectedTrack); // 트랙 이름과 씬 이름이 같아야 함
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
