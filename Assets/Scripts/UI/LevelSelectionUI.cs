using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour 
{
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Button backButton;
    [SerializeField] private string[] levelSceneNames;
    
    private void Start() 
    {
        InitializeButtons();
    }
    private void InitializeButtons() 
    {
        for (int i = 0; i < levelButtons.Length; i++) 
        {
            int index = i; 
            levelButtons[i].onClick.AddListener(() => LoadLevel(index));
        }

        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void LoadLevel(int index)
    {
        if (index < 0 || index >= levelSceneNames.Length) 
        {
            Debug.LogError("Invalid level index: " + index);
            return;
        }

        string sceneName = levelSceneNames[index];
        loadScene(sceneName);
    }
    private void OnBackButtonClicked() 
    {
        loadScene("MainMenu");
    }
    
    private void loadScene(string sceneName) 
    {
        if (!string.IsNullOrEmpty(sceneName)) 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        } 
    }
}