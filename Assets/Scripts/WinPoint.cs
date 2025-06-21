using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPoint : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int coinsWon = 10;
    [SerializeField] private float timeLimit = 45f;
    [Header("UI References")]
    [SerializeField] private GameObject winPointCanvas;
    [SerializeField] private Text completedTimeText;
    [SerializeField] private Text wonCoinsText;
    [SerializeField] private Text gameStatusText;
    [SerializeField] private Text timeCounterText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Scene Settings")]
    [SerializeField] private string nextLevelName;

    private bool levelCompleted = false;
    private const string PlayerTag = "Player";
    private const string MainMenuSceneName = "MainMenu";
    private ISaveSystem saveSystem;
    
    private void Awake()
    {
        saveSystem = new SaveSystem();
        if (winPointCanvas != null)
        {
            winPointCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!levelCompleted && collision.tag == PlayerTag)
        {
            float completionTime = Time.timeSinceLevelLoad;
            if (completionTime <= timeLimit)
            {
                levelCompleted = true;
                ShowWinUI(completionTime);
                Time.timeScale = 0f;

                nextLevelButton?.onClick.AddListener(LoadNextLevel);
                mainMenuButton?.onClick.AddListener(LoadMainMenu);
            }
            else
            {
                ShowFailureUI();
            }
        }
    }

    private void ShowWinUI(float completionTime)
    {
        winPointCanvas.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);
        completedTimeText.text = $"Completed in: {completionTime:F2} seconds";
        wonCoinsText.text = $"Coins Won: {coinsWon}";
        saveSystem.AddCoins(coinsWon);
        timeCounterText.text = $"Time: {completionTime:F2} seconds";
        gameStatusText.text = "Level Complete!";
    }

    private void ShowFailureUI()
    {
        winPointCanvas.SetActive(true);
        gameStatusText.text = "You Failed!";
        timeCounterText.text = $"Time: {timeLimit:F2} seconds";
        completedTimeText.text = "Try Again!";
        wonCoinsText.text = "No Coins Won";
        nextLevelButton.gameObject.SetActive(false);
    }

    private void LoadNextLevel()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuSceneName);
    }

    private void Update()
    {
        if (!levelCompleted)
        {
            float currentTime = Time.timeSinceLevelLoad;
            float timeRemaining = Mathf.Max(0f, timeLimit - currentTime);
            timeCounterText.text = $"Time Remaining: {timeRemaining:F2} seconds";

            if (timeRemaining <= 0f)
            {
                ShowFailureUI();
            }
        }
    }
}