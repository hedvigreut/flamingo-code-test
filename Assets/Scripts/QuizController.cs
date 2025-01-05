using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class QuizController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI questionText;
    [SerializeField] 
    private GameObject rewardUI;
    [SerializeField] 
    private Image correctAnswerImage;
    [SerializeField] 
    protected TextMeshProUGUI correctAnswerText;
    [SerializeField] 
    private TextMeshProUGUI answerMessage;
    [SerializeField] 
    protected TextMeshProUGUI rewardValue;
    [SerializeField] 
    protected GraphicRaycaster raycaster;
    
    [Header("Animation:")]
    [SerializeField]
    protected Animator _rewardsAnimator;
    
    [Header("Timer:")]
    [SerializeField] 
    protected float _totalTime = 15f;
    [SerializeField] 
    protected TMP_Text _currentTimeText;
    [SerializeField] 
    protected Slider _progressSlider;
    protected float _timeLeft;
    protected bool _countingDown;
    
    [Header("Reward settings:")]
    [SerializeField] 
    private int _correctReward = 5000;    
    [SerializeField] 
    private int _incorrectReward = 2000; 
    
    [Header("Data Reading")]
    [SerializeField] 
    protected TextAsset _quizData;
    [SerializeField] 
    protected QuizReader quizReader;
    protected QuestionData[] _questions;
    protected int _currentQuestionIndex = 0;
    protected QuestionData _currentQuestion;
    protected Answer _currentAnswer;

    private const string incorrectAnswerMessage = "You'll get it right next time!";
    private const string correctAnswerMessage = "Well Done!";
    private const string animatorOpenState = "Open";
    private const string animatorCloseState = "Close";
    
    private void Awake()
    {
        _timeLeft = _totalTime;
        _currentTimeText.text = _timeLeft.ToString();
        _questions = quizReader.ParseQuizData(_quizData);
        _currentQuestionIndex = PlayerManager.Instance.GetPictureQuestionIndex();
        if (_currentQuestionIndex >= _questions.Length)
        {
            _currentQuestionIndex = 0;
            PlayerManager.Instance.ResetCurrentPictureQuestionIndex();
        }
        _currentQuestion = _questions[_currentQuestionIndex];
        _currentAnswer = _currentQuestion.Answers[_currentQuestion.CorrectAnswerIndex];
        if (_currentQuestion != null)
        {
            SetQuestionVisuals();
        }
        else
        {
            Debug.LogWarning("Failed to parse flag data.");
        }
    }

    protected void SetQuestionVisuals()
    {
        
    }
}
