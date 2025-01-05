using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField]
    private Button _travelButton;
    [SerializeField] 
    private TextMeshProUGUI _travelPointsValueText;
    
    private int _previousTravelPoints;
    private const string FlagQuizSceneName = "FlagQuiz";
    private const string PictureQuizSceneName = "PictureQuiz";
    
    private void Start()
    {
        _previousTravelPoints = PlayerManager.Instance.GetTravelPoints();
        _travelPointsValueText.text = _previousTravelPoints.ToString();
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    /// <summary>
    /// If another quiz scene unloads, let's update travel points if changed
    /// </summary>
    /// <param name="scene"></param>
    private void OnSceneUnloaded(Scene scene)
    {
        int currentTravelPoints = PlayerManager.Instance.GetTravelPoints();
        if (currentTravelPoints != _previousTravelPoints)
        {
            AnimateTravelPointsChange(_previousTravelPoints, currentTravelPoints);
            _previousTravelPoints = currentTravelPoints;
        }
    }
    
    private void AnimateTravelPointsChange(int fromValue, int toValue)
    {
        DOTween.To(() => (float)fromValue, 
                x => _travelPointsValueText.text = Mathf.FloorToInt(x).ToString(),
                toValue, 1f)
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// Debug button to start Flag quiz
    /// </summary>
    public void OnFlagQuizPressed()
    {
        StartFlagQuiz();
    }
    
    private async void StartFlagQuiz()
    {
        var loadOperation = SceneManager.LoadSceneAsync(FlagQuizSceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
    
    /// <summary>
    /// Debug button to start Text quiz
    /// </summary>
    public void OnPictureQuizPressed()
    {
        StartPictureQuiz();
    }

    private async void StartPictureQuiz()
    {
        Debug.Log("Loading PIctures");
        var loadOperation = SceneManager.LoadSceneAsync(PictureQuizSceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
}
