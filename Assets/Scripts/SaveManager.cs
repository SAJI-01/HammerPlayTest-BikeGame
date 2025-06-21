using UnityEngine;

public class SaveSystem : ISaveSystem
{
    private const string COINS_KEY = "Coins";
    private const string BIKE_UNLOCKED_KEY = "BikeUnlocked_";
    private const string SELECTED_BIKE_KEY = "SelectedBike_";
    
    public int Coins
    {
        get => PlayerPrefs.GetInt(COINS_KEY, 0);
        set => PlayerPrefs.SetInt(COINS_KEY, value);
    }
    
    public bool IsBikeUnlocked(int bikeIndex)
    {
        return PlayerPrefs.GetInt(BIKE_UNLOCKED_KEY + bikeIndex, bikeIndex == 0 ? 1 : 0) == 1;
    }
    
    public string GetSelectedBike()
    {
        return PlayerPrefs.GetString(SELECTED_BIKE_KEY, "DefaultBike");
    }
    public void SetSelectedBike(string bikeName)
    {
        PlayerPrefs.SetString(SELECTED_BIKE_KEY, bikeName);
        PlayerPrefs.Save();
    }
    
    
    public void UnlockBike(int bikeIndex)
    {
        PlayerPrefs.SetInt(BIKE_UNLOCKED_KEY + bikeIndex, 1);
        PlayerPrefs.Save();
    }
    
    public void AddCoins(int amount)
    {
        if (amount > 0)
        {
            Coins += amount;
            PlayerPrefs.Save();
        }
    }
    
    public bool SpendCoins(int amount)
    {
        if (amount > 0 && Coins >= amount)
        {
            Coins -= amount;
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}