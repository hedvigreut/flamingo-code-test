using System.Collections;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PictureQuizController : QuizController
{
    [SerializeField]
    private TextButton[] optionButtons;
    [SerializeField]
    private Image quizImage;
    
    private const string SceneName = "PictureQuiz";
    
    protected override void SetQuestionData()
    {
        base.SetQuestionData();
        CurrentQuestionIndex = PlayerDataManager.Instance.GetPictureQuestionIndex();
        if (CurrentQuestionIndex >= Questions.Length || Questions.Length == 0)
        {
            CurrentQuestionIndex = 0;
            PlayerDataManager.Instance.ResetPictureQuestionIndex();
        }
        CurrentQuestion = Questions[CurrentQuestionIndex];
        if (CurrentQuestion != null)
        {
            SetQuestionVisuals();
        }
        else
        {
            Debug.LogWarning("Failed to parse picture quiz data.");

        }
    }
    
    /// <summary>
    /// Sets the flag images on the buttons from the ids
    /// </summary>
    private void SetQuestionVisuals()
    {
        questionText.text = CurrentQuestion.Question;
        string imageID = CurrentQuestion.CustomImageID;
        Sprite quizPicture = GetSpriteByID(imageID,  true);
        quizImage.sprite = quizPicture;
        correctAnswerImage.sprite = quizPicture;
        correctAnswerText.text = CurrentQuestion.Answers[CurrentQuestion.CorrectAnswerIndex].Text;
        for (int i = 0; i < CurrentQuestion.Answers.Length && i < optionButtons.Length; i++)
        {
            optionButtons[i].GetText().text = CurrentQuestion.Answers[i].Text;
        }
    }
    
    /// <summary>
    /// When the button is clicked check if the button index corresponds to the
    /// correct answer
    /// </summary>
    /// <param name="index">Button Index</param>
    public void OnOptionButtonPressed(int index)
    {
        TextButton textButton = optionButtons[index];
        bool isCorrect = index == CurrentQuestion.CorrectAnswerIndex;
        textButton.ChangeColor(isCorrect);
        SetTravelPoints(isCorrect: isCorrect);
        CurrentQuestionIndex++;
        HapticPatterns.PlayPreset(isCorrect ? HapticPatterns.PresetType.Success : HapticPatterns.PresetType.Failure);
        PlayerDataManager.Instance.SetPictureQuestionIndex(CurrentQuestionIndex);
        StartCoroutine(WaitForButtonAnimation(textButton, isCorrect));
        }
    
    private IEnumerator WaitForButtonAnimation(TextButton textButton, bool correct)
    {
        raycaster.enabled = false;
        yield return new WaitUntil(() => textButton.IsAnimatorAnimationComplete());
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
