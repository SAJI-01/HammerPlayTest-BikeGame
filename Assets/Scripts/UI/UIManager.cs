using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IUIController
{
    [Header("UI References")]
    [SerializeField] private Text coinText;
    [SerializeField] private Text messageText;
    [SerializeField] private GameObject messagePanel;
    
    private ISaveSystem saveSystem;
    
    private void Awake()
    {
        saveSystem = new SaveSystem();
    }
    
    private void Start()
    {
        UpdateCoinsDisplay(saveSystem.Coins);
        HideMessage();
        

    }
    
    public void UpdateCoinsDisplay(int coins)
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {coins}";
        }
    }
    
    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
        
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(2f));
        }
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
    
    private System.Collections.IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideMessage();
    }
    
}