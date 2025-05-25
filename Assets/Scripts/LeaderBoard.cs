using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public List<float> leaderboard = new();

    public void Add(float laptime)
    {
        leaderboard.Add(laptime);
        leaderboard.Sort();
    }

    public List<string> GetFormattedLeaderboard()
    {
        List<string> result = new();

        foreach (float t in leaderboard) 
        {
            int minutes = Mathf.FloorToInt(t / 60f);
            int seconds = Mathf.FloorToInt(t % 60f);
            int milliseconds = Mathf.FloorToInt((t * 1000f) % 1000f);
            result.Add($"{minutes:00}:{seconds:00}.{milliseconds:000}");
        }

        return result;
    }
}
