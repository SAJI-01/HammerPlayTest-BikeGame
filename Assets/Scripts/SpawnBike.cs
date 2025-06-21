using UnityEngine;

public class SpawnBike : MonoBehaviour
{
    [SerializeField] private GameObject[] bikes;
     private ISaveSystem saveSystem;

     private void Awake()
    {
        if (saveSystem == null)
        {
            saveSystem = new SaveSystem();
        }

        string selectedBikeName = saveSystem.GetSelectedBike();
        GameObject selectedBike = null;
        foreach (var bike in bikes)
        {
            if (bike.name == selectedBikeName)
            {
                selectedBike = bike;
                break;
            }
        }

        if (selectedBike != null)
        {
            var instantiatedBike = Instantiate(selectedBike, transform.position, transform.rotation);
            instantiatedBike.SetActive(true);
            Debug.Log($"Spawned bike: {selectedBikeName}");
        }
        else
        {
            Debug.LogWarning($"No bike found with name: {selectedBikeName}. Using default bike.");
            Instantiate(bikes[0], transform.position, transform.rotation);
        }
    }
    
}