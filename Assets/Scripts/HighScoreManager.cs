using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public string date;
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class HighScoreManager : MonoBehaviour
{
    public int topCount = 5;
    public List<int> localTopScores = new List<int>();
    public List<LeaderboardEntry> globalTopScores = new List<LeaderboardEntry>();

    private const string LocalKey = "StarHopperLocalScores";
    private const string GlobalKey = "StarHopperGlobalScores";

    private void Awake()
    {
        LoadLocalScores();
        LoadGlobalScores();
    }

    public void SubmitLocalScore(int score)
    {
        localTopScores.Add(score);
        localTopScores = localTopScores.OrderByDescending(x => x).Take(topCount).ToList();
        SaveLocalScores();
    }

    public void SubmitGlobalScore(int score)
    {
        LeaderboardEntry entry = new LeaderboardEntry
        {
            playerName = "PLAYER",
            score = score,
            date = DateTime.UtcNow.ToString("yyyy-MM-dd")
        };

        globalTopScores.Add(entry);
        globalTopScores = globalTopScores.OrderByDescending(x => x.score).Take(topCount).ToList();
        SaveGlobalScores();
    }

    public List<int> GetLocalTopScores()
    {
        return new List<int>(localTopScores);
    }

    public List<LeaderboardEntry> GetGlobalTopScores()
    {
        return new List<LeaderboardEntry>(globalTopScores);
    }

    private void SaveLocalScores()
    {
        string data = string.Join(",", localTopScores);
        PlayerPrefs.SetString(LocalKey, data);
        PlayerPrefs.Save();
    }

    private void LoadLocalScores()
    {
        localTopScores.Clear();
        string raw = PlayerPrefs.GetString(LocalKey, string.Empty);
        if (string.IsNullOrEmpty(raw))
            return;

        string[] values = raw.Split(',');
        foreach (string value in values)
        {
            if (int.TryParse(value, out int score))
            {
                localTopScores.Add(score);
            }
        }

        localTopScores = localTopScores.OrderByDescending(x => x).Take(topCount).ToList();
    }

    private void SaveGlobalScores()
    {
        LeaderboardData data = new LeaderboardData { entries = globalTopScores };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(GlobalKey, json);
        PlayerPrefs.Save();
    }

    private void LoadGlobalScores()
    {
        globalTopScores.Clear();
        string raw = PlayerPrefs.GetString(GlobalKey, string.Empty);
        if (string.IsNullOrEmpty(raw))
            return;

        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(raw);
        if (data != null && data.entries != null)
        {
            globalTopScores = data.entries.OrderByDescending(e => e.score).Take(topCount).ToList();
        }
    }
}
