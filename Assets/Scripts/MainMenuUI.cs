using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button bikeSelectionButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        InitialButtons();
    }

    private void InitialButtons()
    {
        playButton?.onClick.AddListener(OnPlayButtonClicked);
        bikeSelectionButton?.onClick.AddListener(OnBikeSelectionButtonClicked);
        quitButton?.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        LoadScene("LevelSelection");
    }
    private void OnBikeSelectionButtonClicked()
    {
        LoadScene("BikeSelection");
    }
    
    private void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Quit CLicked");
    }
}