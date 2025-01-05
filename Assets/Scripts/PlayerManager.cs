using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    private const string TravelPoints = "Travel Points";
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public int GetTravelPoints()
    {
        return PlayerPrefs.GetInt(TravelPoints, 0);
    }
    
    public void AddTravelPoints(int points)
    {
        int currentPoints = GetTravelPoints();
        int newTotal = currentPoints + points;
        PlayerPrefs.SetInt(TravelPoints, newTotal);
        PlayerPrefs.Save();
    }
}
