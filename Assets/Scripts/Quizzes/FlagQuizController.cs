using System.Collections;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quizzes
{
    public class FlagQuizController : QuizController
    {
        [SerializeField] private FlagButton[] flagButtons;

        private const string SceneName = "FlagQuiz";

        protected override void SetQuestionData()
        {
            base.SetQuestionData();
            CurrentQuestionIndex = PlayerDataManager.Instance.GetFlagQuestionIndex();
            if (CurrentQuestionIndex >= Questions.Length || Questions.Length == 0)
            {
                CurrentQuestionIndex = 0;
                PlayerDataManager.Instance.ResetFlagQuestionIndex();
            }

            CurrentQuestion = Questions[CurrentQuestionIndex];
            if (CurrentQuestion != null)
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
            questionText.text = CurrentQuestion.Question;
            Answer correctAnswer = CurrentQuestion.Answers[CurrentQuestion.CorrectAnswerIndex];
            Sprite correctPicture = GetSpriteByID(correctAnswer.ImageID, false);
            correctAnswerImage.sprite = correctPicture;
            correctAnswerText.text = CurrentQuestion.Answers[CurrentQuestion.CorrectAnswerIndex].Text;
            for (int i = 0; i < CurrentQuestion.Answers.Length && i < flagButtons.Length; i++)
            {
                string imageID = CurrentQuestion.Answers[i].ImageID;
                Sprite flagSprite = GetSpriteByID(imageID);
                if (flagSprite != null)
                {
                    flagButtons[i].SetFlagImage(flagSprite);
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
            FlagButton flagButton = flagButtons[index];
            bool isCorrect = index == CurrentQuestion.CorrectAnswerIndex;
            flagButton.ChangeColor(isCorrect);
            SetTravelPoints(isCorrect: isCorrect);
            CurrentQuestionIndex++;
            PlayerDataManager.Instance.SetFlagQuestionIndex(CurrentQuestionIndex);
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
}