using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text scoreText;
    public Text finalScoreText;
    public Text localLeaderboardText;
    public Text globalLeaderboardText;
    public Text challengeText;
    public Text notificationText;
    public GameObject titlePanel;
    public GameObject gameOverPanel;

    [Header("References")]
    public HighScoreManager highScoreManager;
    public DailyChallengeManager dailyChallengeManager;

    private void Start()
    {
        RefreshLeaderboards();
        UpdateChallengePanel();
        ShowTitle();
    }

    public void UpdateScore(int currentScore)
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString("0000");
        }
    }

    public void ShowTitle()
    {
        if (titlePanel != null)
            titlePanel.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowPlayState()
    {
        if (titlePanel != null)
            titlePanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int finalScore)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = finalScore.ToString();
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        RefreshLeaderboards();
    }

    public void RefreshLeaderboards()
    {
        if (highScoreManager == null)
            return;

        if (localLeaderboardText != null)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Local High Scores");
            var local = highScoreManager.GetLocalTopScores();
            for (int i = 0; i < local.Count; i++)
            {
                builder.AppendLine($"{i + 1}. {local[i]}");
            }
            localLeaderboardText.text = builder.ToString();
        }

        if (globalLeaderboardText != null)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Global Scores");
            var global = highScoreManager.GetGlobalTopScores();
            for (int i = 0; i < global.Count; i++)
            {
                builder.AppendLine($"{i + 1}. {global[i].playerName}: {global[i].score}");
            }
            globalLeaderboardText.text = builder.ToString();
        }
    }

    public void ShowAchievementNotification(string title, string description)
    {
        if (notificationText == null)
            return;

        StopAllCoroutines();
        StartCoroutine(ShowNotificationRoutine(title, description));
    }

    private IEnumerator ShowNotificationRoutine(string title, string description)
    {
        notificationText.text = $"Achievement unlocked:\n{title}\n{description}";
        notificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        notificationText.gameObject.SetActive(false);
    }

    public void UpdateChallengePanel()
    {
        if (challengeText == null || dailyChallengeManager == null)
            return;

        challengeText.text = dailyChallengeManager.GetChallengeStatusText();
    }

    public void OnStartButton()
    {
        GameManager.Instance?.StartGame();
        UpdateChallengePanel();
    }

    public void OnRestartButton()
    {
        GameManager.Instance?.StartGame();
        UpdateChallengePanel();
    }
}
