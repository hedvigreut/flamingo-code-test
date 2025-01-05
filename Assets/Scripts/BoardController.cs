using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
    private const string TextQuizSceneName = "TextQuiz";
    
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

    private void OnSceneUnloaded(Scene scene)
    {
        int currentTravelPoints = PlayerManager.Instance.GetTravelPoints();
        Debug.Log(currentTravelPoints);
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

    public void OnFlagQuizPressed()
    {
        StartFlagQuiz();
    }
    
    public void OnTextQuizPressed()
    {
        StartTextQuiz();
    }
    
    private async void StartFlagQuiz()
    {
        var loadOperation = SceneManager.LoadSceneAsync(FlagQuizSceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }

    private async void StartTextQuiz()
    {
        var loadOperation = SceneManager.LoadSceneAsync(TextQuizSceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
}
