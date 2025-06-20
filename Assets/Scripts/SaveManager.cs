using UnityEngine;

public static class SaveManager
{
    public static int Coins
    {
        get => PlayerPrefs.GetInt("Coins", 0);
        set => PlayerPrefs.SetInt("Coins", value);
    }

    public static bool IsBike2Unlocked
    {
        get => PlayerPrefs.GetInt("Bike2Unlocked", 0) == 1;
        set => PlayerPrefs.SetInt("Bike2Unlocked", value ? 1 : 0);
    }

    public static void AddCoins(int amount)
    {
        Coins += amount;
    }

    public static void UnlockBike2()
    {
        if (Coins >= 30 && !IsBike2Unlocked)
        {
            Coins -= 30;
            IsBike2Unlocked = true;
        }
    }
}
