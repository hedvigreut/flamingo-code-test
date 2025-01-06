using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    private const string TravelPoints = "Travel Points";
    private const string CurrentFlagQuestionIndex = "FlagQuestionIndex";
    private const string CurrentPictureQuestionIndex = "PictureQuestionIndex";
    private const string CurrentBoard = "CurrentBoard";
    private const string CurrentPosition= "CurrentPosition";
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PlayerManager).ToString());
                    _instance = singletonObject.AddComponent<PlayerManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
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
    
    public int GetFlagQuestionIndex()
    {
        return PlayerPrefs.GetInt(CurrentFlagQuestionIndex, 0);
    }

    public void SetCurrentFlagQuestionIndex(int questionIndex)
    {
        PlayerPrefs.SetInt(CurrentFlagQuestionIndex, questionIndex);
        PlayerPrefs.Save();
    }

    public void ResetCurrentFlagQuestionIndex()
    {
        PlayerPrefs.SetInt(CurrentFlagQuestionIndex, 0);
        PlayerPrefs.Save();
    }
    
    public int GetPictureQuestionIndex()
    {
        return PlayerPrefs.GetInt(CurrentPictureQuestionIndex, 0);
    }

    public void SetCurrentPictureQuestionIndex(int questionIndex)
    {
        PlayerPrefs.SetInt(CurrentPictureQuestionIndex, questionIndex);
        PlayerPrefs.Save();
    }

    public void ResetCurrentPictureQuestionIndex()
    {
        PlayerPrefs.SetInt(CurrentPictureQuestionIndex, 0);
        PlayerPrefs.Save();
    }
    
    public int GetCurrentBoardIndex()
    {
        return PlayerPrefs.GetInt(CurrentBoard, 0);
    }
    
    public void SetCurrentBoardIndex(int questionIndex)
    {
        PlayerPrefs.SetInt(CurrentBoard, questionIndex);
        PlayerPrefs.Save();
    }
    
    public int GetCurrentPosition()
    {
        return PlayerPrefs.GetInt(CurrentPosition, 0);
    }
    
    public void SetCurrentPosition(int tileIndex)
    {
        PlayerPrefs.SetInt(CurrentPosition, tileIndex);
        PlayerPrefs.Save();
    }
}
