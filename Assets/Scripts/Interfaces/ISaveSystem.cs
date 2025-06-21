public interface ISaveSystem
{
    int Coins { get; set; }
    bool IsBikeUnlocked(int bikeIndex);
    void UnlockBike(int bikeIndex);
    string GetSelectedBike();
    void SetSelectedBike(string bikeName);
    void AddCoins(int amount);
    bool SpendCoins(int amount);
}