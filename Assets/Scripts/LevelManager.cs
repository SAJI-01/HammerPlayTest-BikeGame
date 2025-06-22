using UnityEngine;
using UnityEngine.UI;

public class LevelManager: MonoBehaviour
{
    [SerializeField] private Button playResumeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button bikeSelectionButton;
    [SerializeField] private GameObject pauseMenuCanvas;
    
    
    private void Start()
    {
        InitializeButtons();
        TimeScale(1f);
    }
    private void InitializeButtons()
    {
        playResumeButton?.onClick.AddListener(OnPlayResumeButtonClicked);
        restartButton?.onClick.AddListener(OnRestartButtonClicked);
        mainMenuButton?.onClick.AddListener(OnMainMenuButtonClicked);
        bikeSelectionButton?.onClick.AddListener(OnBikeSelectionButtonClicked);
        pauseButton?.onClick.AddListener(OnPauseButtonClicked);
    }

    private void TimeScale(float scale)
    {
        Time.timeScale = scale;
    }
    private void OnPauseButtonClicked()
    {
        TimeScale(0f);
        pauseMenuCanvas.SetActive(true);
    }
    private void OnPlayResumeButtonClicked()
    {
        TimeScale(1f);
        pauseMenuCanvas.SetActive(false); 
    }
    private void OnRestartButtonClicked()
    {
        TimeScale(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    private void OnMainMenuButtonClicked()
    {
        TimeScale(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    private void OnBikeSelectionButtonClicked()
    {
        TimeScale(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("BikeSelection");
    }
    
}