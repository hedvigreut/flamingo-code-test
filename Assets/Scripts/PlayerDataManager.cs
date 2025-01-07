using UnityEngine;

public class PlayerDataManager
{
    private static PlayerDataManager _instance;
    public static PlayerDataManager Instance => _instance ??= new PlayerDataManager();
    private PlayerDataManager() { }
    
    private const string TravelPointsKey = "Travel Points";
    private const string CurrentFlagQuestionIndexKey = "FlagQuestionIndex";
    private const string CurrentPictureQuestionIndexKey = "PictureQuestionIndex";
    private const string CurrentBoardKey = "CurrentBoard";
    private const string CurrentPositionKey = "CurrentPosition";
    
    public int GetTravelPoints()
    {
        return PlayerPrefs.GetInt(TravelPointsKey, 0);
    }

    public void AddTravelPoints(int points)
    {
        int currentPoints = GetTravelPoints();
        PlayerPrefs.SetInt(TravelPointsKey, currentPoints + points);
        PlayerPrefs.Save();
    }

    public int GetFlagQuestionIndex()
    {
        return PlayerPrefs.GetInt(CurrentFlagQuestionIndexKey, 0);
    }

    public void SetFlagQuestionIndex(int questionIndex)
    {
        PlayerPrefs.SetInt(CurrentFlagQuestionIndexKey, questionIndex);
        PlayerPrefs.Save();
    }

    public void ResetFlagQuestionIndex()
    {
        PlayerPrefs.SetInt(CurrentFlagQuestionIndexKey, 0);
        PlayerPrefs.Save();
    }

    public int GetPictureQuestionIndex()
    {
        return PlayerPrefs.GetInt(CurrentPictureQuestionIndexKey, 0);
    }

    public void SetPictureQuestionIndex(int questionIndex)
    {
        PlayerPrefs.SetInt(CurrentPictureQuestionIndexKey, questionIndex);
        PlayerPrefs.Save();
    }

    public void ResetPictureQuestionIndex()
    {
        PlayerPrefs.SetInt(CurrentPictureQuestionIndexKey, 0);
        PlayerPrefs.Save();
    }

    public int GetCurrentBoardIndex()
    {
        return PlayerPrefs.GetInt(CurrentBoardKey, 0);
    }

    public void SetCurrentBoardIndex(int boardIndex)
    {
        PlayerPrefs.SetInt(CurrentBoardKey, boardIndex);
        PlayerPrefs.Save();
    }

    public int GetCurrentPosition()
    {
        return PlayerPrefs.GetInt(CurrentPositionKey, 0);
    }

    public void SetCurrentPosition(int position)
    {
        PlayerPrefs.SetInt(CurrentPositionKey, position);
        PlayerPrefs.Save();
    }
}
