using UnityEngine;
using UnityEngine.UI;

public class PictureQuizController : QuizController
{
    [SerializeField]
    private Button[] optionButtons;

    [Header("Button settings:")]
    [SerializeField] 
    private FlagData[] _flagSpritesToIds;
    
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
}
