using System.Collections;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagQuizController : QuizController
{
    [SerializeField]
    private FlagButton[] _flagButtons;
    
    private const string SceneName = "FlagQuiz";
    
    protected override void SetQuestionData()
    {
        base.SetQuestionData();
        _currentQuestionIndex = PlayerManager.Instance.GetFlagQuestionIndex();
        if (_currentQuestionIndex >= _questions.Length || _questions.Length == 0)
        {
            _currentQuestionIndex = 0;
            PlayerManager.Instance.ResetCurrentFlagQuestionIndex();
        }
        _currentQuestion = _questions[_currentQuestionIndex];
        _currentAnswer = _currentQuestion.Answers[_currentQuestion.CorrectAnswerIndex];
        if (_currentQuestion != null)
        {
            SetQuestionVisuals();
        }
        else
        {
            Debug.LogWarning("Failed to parse flag quiz data.");
        }
    }
    
    /// <summary>
    /// Sets the flag images on the buttons from the ids
    /// </summary>
    private void SetQuestionVisuals()
    {
        questionText.text = _currentQuestion.Question;
        Answer correctAnswer = _currentQuestion.Answers[_currentQuestion.CorrectAnswerIndex];
        Sprite correctPicture = GetSpriteByID(correctAnswer.ImageID, false);
        correctAnswerImage.sprite = correctPicture;
        correctAnswerText.text = _currentQuestion.Answers[_currentQuestion.CorrectAnswerIndex].Text;
        for (int i = 0; i < _currentQuestion.Answers.Length && i < _flagButtons.Length; i++)
        {
            string imageID = _currentQuestion.Answers[i].ImageID;
            Sprite flagSprite = GetSpriteByID(imageID);
            if (flagSprite != null)
            {
                _flagButtons[i].SetFlagImage(flagSprite);
            }
            else
            {
                Debug.LogWarning($"No sprite found for ImageID: {imageID}");
            }
        }
    }
    
    /// <summary>
    /// When the button is clicked check if the button index corresponds to the
    /// correct answer
    /// </summary>
    /// <param name="index">Button Index</param>
    public void OnFlagButtonPressed(int index)
    {
        FlagButton flagButton = _flagButtons[index];
        bool isCorrect = index == _currentQuestion.CorrectAnswerIndex;
        flagButton.ChangeColor(isCorrect);
        SetTravelPoints(isCorrect: isCorrect);
        _currentQuestionIndex++;
        PlayerManager.Instance.SetCurrentFlagQuestionIndex(_currentQuestionIndex);
        StartCoroutine(WaitForButtonAnimation(flagButton, isCorrect));
        HapticPatterns.PlayPreset(isCorrect ? HapticPatterns.PresetType.Success : HapticPatterns.PresetType.Failure);
    }

    private IEnumerator WaitForButtonAnimation(FlagButton flagButton, bool correct)
    {
        raycaster.enabled = false;
        yield return new WaitUntil(() => flagButton.IsAnimatorAnimationComplete());
        yield return new WaitForSeconds(0.5f);
        raycaster.enabled = true;
        SetAnswerVisuals(correct);
    }
    protected override void UnloadScene()
    {
        DOTween.KillAll();
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
