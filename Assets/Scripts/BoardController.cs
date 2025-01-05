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
    [SerializeField] private TextMeshProUGUI _travelPointsText;
    private int _previousTravelPoints;
    
    private void Start()
    {
        // TODO: Fix for problem with UI elements disappear after another additive scene unloading
        if (_travelPointsText == null)
        {
            _travelPointsText = FindObjectOfType<TextMeshProUGUI>();
        }
        if (_travelPointsText != null && PlayerManager.Instance != null)
        {
            _previousTravelPoints = PlayerManager.Instance.GetTravelPoints();
            _travelPointsText.text = _previousTravelPoints.ToString();
        }
        if (_travelButton == null)
        {
            _travelButton = FindObjectOfType<Button>();
        }
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
        Debug.Log("Additive scene unloaded: " + scene.name);
        int currentTravelPoints = PlayerManager.Instance.GetTravelPoints();

        if (currentTravelPoints != _previousTravelPoints)
        {
            AnimateTravelPointsChange(_previousTravelPoints, currentTravelPoints);
            _previousTravelPoints = currentTravelPoints;
        }
        else
        {
            Debug.Log("No change");
        }
    }
    private void AnimateTravelPointsChange(int fromValue, int toValue)
    {
        DOTween.To(() => (float)fromValue, 
                x => _travelPointsText.text = Mathf.FloorToInt(x).ToString(),
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
        Debug.Log("FlagQuiz: Showing");

        var loadOperation = SceneManager.LoadSceneAsync("FlagQuiz", LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }

    private async void StartTextQuiz()
    {
        Debug.Log("TextQuiz: Showing");
        var loadOperation = SceneManager.LoadSceneAsync("TextQuiz", LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
}
