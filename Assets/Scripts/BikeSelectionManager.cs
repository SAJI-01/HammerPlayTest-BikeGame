using UnityEngine;
using UnityEngine.UI;

public class BikeSelectionManager : MonoBehaviour
{
    [Header("Bike Configuration")]
    [SerializeField] private BikeController[] availableBikes;
    
    [Header("UI References")]
    [SerializeField] private Button selectButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Text bikeNameText;
    [SerializeField] private Text bikePriceText;
    [SerializeField] private Text bikeStatusText;
    
    
    [SerializeField] private UIManager uiController;
    private ISaveSystem saveSystem;
    private int currentBikeIndex = 0;
    
    public BikeController CurrentBike => availableBikes[currentBikeIndex];
    
    private void Awake()
    {
        saveSystem = new SaveSystem();
    }
    
    private void Start()
    {
        if (ValidateComponents())
        {
            SetupEventListeners();
            UpdateBikeSelection();
        }
    }
    
    private bool ValidateComponents()
    {
        if (availableBikes == null || availableBikes.Length == 0)
        {
            Debug.LogError("bikes not assigned in BikeSelection Manager");
            return false;
        }
        
        return true;
    }
    
    private void SetupEventListeners()
    {
        selectButton?.onClick.AddListener(SelectCurrentBike);
        buyButton?.onClick.AddListener(BuyCurrentBike);
        nextButton?.onClick.AddListener(SelectNextBike);
        backButton?.onClick.AddListener(() => uiController?.ShowMainMenu());
    }

    private void SelectNextBike()
    {
        currentBikeIndex = (currentBikeIndex + 1) % availableBikes.Length;
        UpdateBikeSelection();
    }
    
    private void UpdateBikeSelection()
    {
        UpdateBikeVisibility();
        UpdateUI();
    }
    
    private void UpdateBikeVisibility()
    {
        for (int i = 0; i < availableBikes.Length; i++)
        {
            availableBikes[i].gameObject.SetActive(i == currentBikeIndex);
        }
    }
    
    private void UpdateUI()
    {
        var currentBike = CurrentBike;
        var bikeData = currentBike.BikeData;
        bool isUnlocked = saveSystem.IsBikeUnlocked(currentBikeIndex);
        
        // Update bike info text
        bikeNameText.text = bikeData.name;
        bikePriceText.text = $"${bikeData.price}";
        
        // Update button states
        selectButton.interactable = isUnlocked;
        buyButton.gameObject.SetActive(!isUnlocked);
        buyButton.interactable = saveSystem.Coins >= bikeData.price;
        
        // Update status text
        bikeStatusText.text = isUnlocked ? "UNLOCKED" : "LOCKED";
    }
    
    private void SelectCurrentBike()
    {
        if (saveSystem.IsBikeUnlocked(currentBikeIndex))
        {
            saveSystem.SetSelectedBike(CurrentBike.BikeData.name);
            uiController?.ShowMessage($"Selected: {CurrentBike.BikeData.name}");
        }
    }

    private void BuyCurrentBike()
    {
        var bikeData = CurrentBike.BikeData;
        
        if (saveSystem.SpendCoins(bikeData.price))
        {
            saveSystem.UnlockBike(currentBikeIndex);
            uiController?.UpdateCoinsDisplay(saveSystem.Coins);
            uiController?.ShowMessage($"Purchased: {bikeData.name}");
            UpdateBikeSelection();
        }
        else
        {
            uiController?.ShowMessage("Not enough coins!");
        }
    }
    
    private void OnDestroy()
    {
        selectButton?.onClick.RemoveAllListeners();
        buyButton?.onClick.RemoveAllListeners();
        nextButton?.onClick.RemoveAllListeners();
    }
}