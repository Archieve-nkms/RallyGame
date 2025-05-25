using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ResultPopup : BasePopupUI
{
    [SerializeField]
    TextUI _leaderboardUI;

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.Init();
        GameManager.Instance.GameUI.BeginCountdown(5, GameState.Race);
        gameObject.SetActive(false);
    }

    public void OnReplayButtonClicked()
    {
        GameManager.Instance.Init();
        GameManager.Instance.GameUI.BeginCountdown(5, GameState.Replay);
        gameObject.SetActive(false);
    }

    public void UpdateLeaderboard(List<string> leaderboard)
    {
        string leaderboardString = "";

        for (int i = 0; i < leaderboard.Count; i++)
        {
            leaderboardString += $"{i + 1}. {leaderboard[i]}\n";
        }
        _leaderboardUI.SetText(leaderboardString);
    }
}