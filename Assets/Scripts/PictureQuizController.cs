using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PictureQuizController : QuizController
{
    [SerializeField]
    private Button[] optionButtons;
    [SerializeField]
    private Image quizImage;
    
    private const string SceneName = "PictureQuiz";

    protected override void SetQuestionVisuals()
    {
        base.SetQuestionVisuals();
        for (int i = 0; i < _currentQuestion.Answers.Length && i < optionButtons.Length; i++)
        {
            string imageID = _currentQuestion.CustomImageID;
            Sprite quizPicture = GetSpriteByID(imageID);
            if (quizPicture != null)
            {
                quizImage.sprite = quizPicture;
            }
            else
            {
                Debug.LogWarning($"No sprite found for ImageID: {imageID}");
            }
        }
    }
    
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
    
    protected override void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
