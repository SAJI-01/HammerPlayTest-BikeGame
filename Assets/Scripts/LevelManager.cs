using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevelCoins = 10;
    public float currentLevelTime = 30f;
    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);
}
