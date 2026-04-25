using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Gameplay")]
    public float baseScrollSpeed = 5f;
    public float difficultyRamp = 0.03f;
    public float scoreMultiplier = 1f;

    [Header("References")]
    public PlayerController player;
    public ObstacleSpawner spawner;
    public UIManager uiManager;
    public HighScoreManager highScoreManager;
    public AchievementManager achievementManager;
    public DailyChallengeManager dailyChallengeManager;

    public bool IsRunning { get; private set; }
    public float DistanceScore { get; private set; }
    public float Difficulty { get; private set; }

    private float currentSpeed;
    private float elapsedTime;

    public event Action OnGameStarted;
    public event Action OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        elapsedTime += Time.deltaTime;
        Difficulty = 1f + elapsedTime * difficultyRamp;
        currentSpeed = baseScrollSpeed * Difficulty;
        DistanceScore += Time.deltaTime * scoreMultiplier * Difficulty * 10f;

        uiManager?.UpdateScore(Mathf.FloorToInt(DistanceScore));
        dailyChallengeManager?.UpdateDistance(Mathf.FloorToInt(DistanceScore));
        spawner?.UpdateDifficulty(Difficulty, currentSpeed);
    }

    public void StartGame()
    {
        ResetGame();
        IsRunning = true;
        player?.EnableControls(true);
        spawner?.EnableSpawning(true);
        uiManager?.ShowPlayState();
        dailyChallengeManager?.RegisterRun();
        OnGameStarted?.Invoke();
    }

    public void ResetGame()
    {
        IsRunning = false;
        DistanceScore = 0f;
        elapsedTime = 0f;
        Difficulty = 1f;
        currentSpeed = baseScrollSpeed;
        player?.ResetState();
        spawner?.ResetSpawner();
        uiManager?.ShowTitle();
    }

    public void GameOver()
    {
        if (!IsRunning)
            return;

        IsRunning = false;
        player?.EnableControls(false);
        spawner?.EnableSpawning(false);
        int finalScore = Mathf.FloorToInt(DistanceScore);
        uiManager?.ShowGameOver(finalScore);
        highScoreManager?.SubmitLocalScore(finalScore);
        highScoreManager?.SubmitGlobalScore(finalScore);
        achievementManager?.CheckForAchievements(finalScore, dailyChallengeManager);
        dailyChallengeManager?.CompleteRun(finalScore);
        uiManager?.RefreshLeaderboards();
        OnGameOver?.Invoke();
    }

    public float GetCurrentScrollSpeed()
    {
        return currentSpeed;
    }
}
