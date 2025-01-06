using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PictureQuizController : QuizController
{
    [SerializeField]
    private Button[] optionButtons;
    [SerializeField]
    private Image quizImage;
    [SerializeField]
    private TextMeshProUGUI[] buttonTexts;
    
    private const string SceneName = "PictureQuiz";
    
    protected override void SetQuestionData()
    {
        base.SetQuestionData();
        _currentQuestionIndex = PlayerManager.Instance.GetPictureQuestionIndex();
        if (_currentQuestionIndex >= _questions.Length || _questions.Length == 0)
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
    
    protected override void SetQuestionVisuals()
    {
        base.SetQuestionVisuals();
        string imageID = _currentQuestion.CustomImageID;
        Sprite quizPicture = GetSpriteByID(imageID,  true);
        quizImage.sprite = quizPicture;
        correctAnswerImage.sprite = quizPicture;
        correctAnswerText.text = _currentQuestion.Answers[_currentQuestion.CorrectAnswerIndex].Text;
        for (int i = 0; i < _currentQuestion.Answers.Length && i < optionButtons.Length; i++)
        { 
            buttonTexts[i].text = _currentQuestion.Answers[i].Text;
        }
    }
    
    /// <summary>
    /// When the button is clicked check if the button index corresponds to the
    /// correct answer
    /// </summary>
    /// <param name="index">Button Index</param>
    public void OnOptionButtonPressed(int index)
    {
        Button button = optionButtons[index];
        bool isCorrect = index == _currentQuestion.CorrectAnswerIndex;
        // flagButton.ChangeColor(isCorrect);
        SetTravelPoints(isCorrect: isCorrect);
        _currentQuestionIndex++;
        PlayerManager.Instance.SetCurrentPictureQuestionIndex(_currentQuestionIndex);
        // StartCoroutine(WaitForButtonAnimation(button, isCorrect));
        SetAnswerVisuals(isCorrect);
    }
    
    protected override void UnloadScene()
    {
        DOTween.KillAll();
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
