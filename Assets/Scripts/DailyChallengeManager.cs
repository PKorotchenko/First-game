using System;
using UnityEngine;

public enum DailyChallengeType
{
    Distance,
    AvoidAttraction,
    MeteoriteRush
}

[Serializable]
public class DailyChallenge
{
    public string id;
    public string title;
    public string description;
    public DailyChallengeType challengeType;
    public int targetValue;
    public int progress;
    public bool completed;
    public bool failed;
}

public class DailyChallengeManager : MonoBehaviour
{
    public DailyChallenge[] challenges;
    public DailyChallenge activeChallenge;

    public bool CurrentChallengeCompleted { get; private set; }
    public bool CurrentChallengeFailed => activeChallenge.failed;

    private const string ActiveChallengeKey = "StarHopperActiveChallenge";
    private const string ChallengeDateKey = "StarHopperChallengeDate";

    private void Awake()
    {
        LoadOrCreateChallenge();
    }

    public void RegisterRun()
    {
        if (activeChallenge == null || string.IsNullOrEmpty(activeChallenge.id))
        {
            PickNewChallenge();
        }

        activeChallenge.progress = 0;
        activeChallenge.completed = false;
        activeChallenge.failed = false;
        CurrentChallengeCompleted = false;
        SaveCurrentChallenge();
    }

    public void UpdateDistance(int score)
    {
        if (activeChallenge == null)
            return;

        if (activeChallenge.challengeType != DailyChallengeType.Distance)
            return;

        activeChallenge.progress = score;
        if (activeChallenge.progress >= activeChallenge.targetValue)
        {
            activeChallenge.completed = true;
            CurrentChallengeCompleted = true;
        }
    }

    public void RegisterAttractionEncounter()
    {
        if (activeChallenge == null)
            return;

        if (activeChallenge.challengeType == DailyChallengeType.AvoidAttraction)
        {
            activeChallenge.failed = true;
        }
    }

    public void RegisterMeteoriteEntry()
    {
        if (activeChallenge == null)
            return;

        if (activeChallenge.challengeType == DailyChallengeType.MeteoriteRush)
        {
            activeChallenge.progress = 1;
        }
    }

    public void CompleteRun(int finalScore)
    {
        if (activeChallenge == null)
            return;

        switch (activeChallenge.challengeType)
        {
            case DailyChallengeType.Distance:
                activeChallenge.completed = activeChallenge.progress >= activeChallenge.targetValue;
                break;
            case DailyChallengeType.AvoidAttraction:
                activeChallenge.completed = !activeChallenge.failed;
                break;
            case DailyChallengeType.MeteoriteRush:
                activeChallenge.completed = activeChallenge.progress > 0 && finalScore >= activeChallenge.targetValue;
                break;
        }

        CurrentChallengeCompleted = activeChallenge.completed;
        SaveCurrentChallenge();
    }

    public string GetChallengeStatusText()
    {
        if (activeChallenge == null)
            return "No challenge available.";

        string progressText = activeChallenge.challengeType switch
        {
            DailyChallengeType.Distance => $"{Mathf.Min(activeChallenge.progress, activeChallenge.targetValue)}/{activeChallenge.targetValue}",
            DailyChallengeType.AvoidAttraction => activeChallenge.failed ? "Failed" : "Avoid the black hole",
            DailyChallengeType.MeteoriteRush => activeChallenge.progress > 0 ? "Meteorite zone entered" : "Enter the meteorite zone",
            _ => ""
        };

        return $"Daily: {activeChallenge.title}\n{activeChallenge.description}\n{progressText}";
    }

    private void LoadOrCreateChallenge()
    {
        string savedDate = PlayerPrefs.GetString(ChallengeDateKey, string.Empty);
        string today = DateTime.UtcNow.ToString("yyyyMMdd");

        if (savedDate != today)
        {
            PickNewChallenge();
            PlayerPrefs.SetString(ChallengeDateKey, today);
            SaveCurrentChallenge();
            PlayerPrefs.Save();
            return;
        }

        string raw = PlayerPrefs.GetString(ActiveChallengeKey, string.Empty);
        if (string.IsNullOrEmpty(raw))
        {
            PickNewChallenge();
            SaveCurrentChallenge();
            return;
        }

        activeChallenge = JsonUtility.FromJson<DailyChallenge>(raw);
        if (activeChallenge == null || string.IsNullOrEmpty(activeChallenge.id))
        {
            PickNewChallenge();
            SaveCurrentChallenge();
        }
    }

    private void SaveCurrentChallenge()
    {
        if (activeChallenge == null)
            return;

        string json = JsonUtility.ToJson(activeChallenge);
        PlayerPrefs.SetString(ActiveChallengeKey, json);
        PlayerPrefs.Save();
    }

    private void PickNewChallenge()
    {
        if (challenges == null || challenges.Length == 0)
        {
            activeChallenge = new DailyChallenge
            {
                id = "none",
                title = "Free Run",
                description = "No challenge active.",
                challengeType = DailyChallengeType.Distance,
                targetValue = 100
            };
            return;
        }

        activeChallenge = challenges[UnityEngine.Random.Range(0, challenges.Length)];
        activeChallenge.progress = 0;
        activeChallenge.completed = false;
        activeChallenge.failed = false;
    }
}
