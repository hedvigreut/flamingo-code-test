using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quizzes
{
    public abstract class QuizController : MonoBehaviour
    {
        [Header("Quiz data:")] [SerializeField]
        protected TextAsset _quizData;

        [SerializeField] protected float maximumQuizAnswerTime = 15f;
        [SerializeField] protected GameSettings gameSettings;
        [SerializeField] [Space(10)] protected QuizReader quizReader;

        [Header("Button settings:")] [SerializeField]
        private QuizVisualsData[] _quizVisuals;

        [Header("UI Elements")] [SerializeField]
        protected TextMeshProUGUI questionText;

        [SerializeField] private GameObject rewardUI;
        [SerializeField] protected Image correctAnswerImage;
        [SerializeField] protected TextMeshProUGUI correctAnswerText;
        [SerializeField] private TextMeshProUGUI answerMessage;
        [SerializeField] protected TextMeshProUGUI rewardValue;
        [SerializeField] protected GraphicRaycaster raycaster;
        [SerializeField] protected TMP_Text _currentTimeText;
        [SerializeField] protected Slider _progressSlider;

        [Header("Animation:")] [SerializeField]
        protected Animator _rewardsAnimator;
        [SerializeField]
        private float maxNumberOfSecondsToWaitForTap = 5f;

        private const string animatorOpenState = "Open";
        private const string animatorCloseState = "Close";

        [Space(10)] private float _timeLeft;
        private bool _countingDown;

        protected QuestionData[] Questions;
        protected int CurrentQuestionIndex = 0;
        protected QuestionData CurrentQuestion;
        private const string incorrectAnswerMessage = "You'll get it right next time!";
        private const string correctAnswerMessage = "Well Done!";


        private void Awake()
        {
            _timeLeft = maximumQuizAnswerTime;
            _currentTimeText.text = _timeLeft.ToString();
            SetQuestionData();
        }

        private void Start()
        {
            _countingDown = true;
        }

        protected virtual void SetQuestionData()
        {
            Questions = quizReader.ParseQuizData(_quizData);
        }

        private void Update()
        {
            if (_countingDown && _timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                _currentTimeText.text = _timeLeft.ToString("F0") + "s";
                _progressSlider.value = _timeLeft / maximumQuizAnswerTime;
            }
            else
            {
                SetAnswerVisuals(isCorrect: false);
                SetTravelPoints(isCorrect: false);
                _countingDown = false;
            }
        }

        protected void SetAnswerVisuals(bool isCorrect)
        {
            answerMessage.text = isCorrect ? correctAnswerMessage : incorrectAnswerMessage;
            rewardValue.text = isCorrect
                ? gameSettings.correctTravelPointReward.ToString()
                : gameSettings.incorrectTravelPointReward.ToString();
            rewardUI.SetActive(true);
            AnimateRewardText();
        }

        private void AnimateRewardText()
        {
            float targetValue = float.Parse(rewardValue.text);
            rewardValue.text = "0";
            DOTween.Sequence()
                .Append(DOTween.To(() => 0f, x => rewardValue.text = Mathf.FloorToInt(x).ToString(), targetValue, 1f))
                .Join(rewardValue.transform.DOScale(Vector3.one * 1.2f, 1f))
                .Append(rewardValue.transform.DOScale(Vector3.one, 0.3f))
                .OnComplete(() => { WaitForTapToUnload(); });
        }

        protected void WaitForTapToUnload()
        {
            StartCoroutine(CheckForTap());
        }

        protected IEnumerator CheckForTap()
        {
            float timer = 0f;
            while (timer < maxNumberOfSecondsToWaitForTap)
            {
                if (Input.GetMouseButtonDown(0))
                    break;

                timer += Time.deltaTime;
                yield return null;
            }
            _rewardsAnimator.SetBool(animatorOpenState, false);
            while (!IsAnimatorDone(_rewardsAnimator, animatorCloseState))
                yield return null;
            UnloadScene();
        }

        protected bool IsAnimatorDone(Animator animator, string stateName)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1f;
        }

        protected abstract void UnloadScene();

        protected void SetTravelPoints(bool isCorrect)
        {
            PlayerDataManager.Instance.AddTravelPoints(isCorrect
                ? gameSettings.correctTravelPointReward
                : gameSettings.incorrectTravelPointReward);
        }

        protected Sprite GetSpriteByID(string imageID, bool customID = false)
        {
            foreach (var visualsData in _quizVisuals)
            {
                string compareID = customID ? visualsData.sprite.name : visualsData.ImageID;
                if (compareID == imageID)
                {
                    return visualsData.sprite;
                }
            }

            return null;
        }

        protected string GetCountryNameByID(string imageID)
        {
            foreach (var visualsData in _quizVisuals)
            {
                if (visualsData.ImageID == imageID)
                {
                    return visualsData.name;
                }
            }

            return null;
        }
    }
}