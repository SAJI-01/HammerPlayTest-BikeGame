using UnityEngine;
using UnityEngine.UI;

public class BikeSelectionMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] bikes; 
    private int currentBikeIndex = 0;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Text bikeSelectConditionText;
    
    private void Start()
    {
        UpdateBikeSelection();
        selectButton.onClick.AddListener(OnSelectButtonClicked);
        previousButton.onClick.AddListener(PreviousBike);
        nextButton.onClick.AddListener(NextBike);
    }

    private void OnSelectButtonClicked()
    {
        Debug.Log("Selected Bike: " + bikes[currentBikeIndex].name);
    }

    void NextBike()
    {
        currentBikeIndex = (currentBikeIndex + 1) % bikes.Length;
        UpdateBikeSelection();
    }

    void PreviousBike()
    {
        currentBikeIndex = (currentBikeIndex - 1 + bikes.Length) % bikes.Length;
        UpdateBikeSelection();
    }

    void UpdateBikeSelection()
    {
        for (int i = 0; i < bikes.Length; i++)
        {
            bikes[i].SetActive(i == currentBikeIndex);
        }
    }
}