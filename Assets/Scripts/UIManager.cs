using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinText;
    public LevelManager levelManager;

    private void Start()
    {
        UpdateCoinsUI();
    }

    public void UpdateCoinsUI()
    {
        coinText.text = "Coins: " + SaveManager.Coins;
    }

    public void RestartLevel()
    {
        FindObjectOfType<LevelManager>().RestartLevel();
    }

    public void LevelEndReached()
    {
        Debug.Log("You Win!");
        SaveManager.AddCoins(levelManager.currentLevelCoins);
        
        
    }
}
