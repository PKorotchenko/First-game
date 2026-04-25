using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementDefinition
{
    public string id;
    public string title;
    public string description;
    public int targetValue;
    public bool unlocked;
}

public class AchievementManager : MonoBehaviour
{
    public AchievementDefinition[] achievements;
    public UIManager uiManager;

    private const string AchievementKey = "StarHopperAchievement_";

    private void Awake()
    {
        LoadAchievements();
    }

    public void CheckForAchievements(int score, DailyChallengeManager challengeManager)
    {
        foreach (var achievement in achievements)
        {
            if (achievement.unlocked)
                continue;

            bool complete = achievement.id switch
            {
                "first_flight" => score >= 1,
                "distance_500" => score >= 500,
                "distance_1000" => score >= 1000,
                "challenge_master" => challengeManager != null && challengeManager.CurrentChallengeCompleted,
                _ => false
            };

            if (complete)
            {
                UnlockAchievement(achievement);
            }
        }
    }

    private void UnlockAchievement(AchievementDefinition achievement)
    {
        achievement.unlocked = true;
        SaveAchievement(achievement.id, true);
        uiManager?.ShowAchievementNotification(achievement.title, achievement.description);
    }

    private void SaveAchievement(string id, bool value)
    {
        PlayerPrefs.SetInt(AchievementKey + id, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        foreach (var achievement in achievements)
        {
            achievement.unlocked = PlayerPrefs.GetInt(AchievementKey + achievement.id, 0) == 1;
        }
    }
}
