using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public abstract class QuizController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    protected TextMeshProUGUI questionText;
    [SerializeField] 
    private GameObject rewardUI;
    [SerializeField] protected Image correctAnswerImage;
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
    private int _correctReward = 4000;    
    [SerializeField] 
    private int _incorrectReward = 1000; 
    
    [FormerlySerializedAs("_flagSpritesToIds")]
    [Header("Button settings:")]
    [SerializeField] 
    private QuizVisualsData[] _quizVisuals;
    
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
        SetQuestionData();
    }
    
    private void Start()
    {
        _countingDown = true;
    }

    protected virtual void SetQuestionData()
    {
        _questions = quizReader.ParseQuizData(_quizData);
    }
    
    private void Update()                                                    
    {                                                                        
        if(_countingDown && _timeLeft > 0)                                                 
        {                                                                            
            _timeLeft -= Time.deltaTime;                                     
            _currentTimeText.text = _timeLeft.ToString("F0") + "s";          
            _progressSlider.value = _timeLeft / _totalTime;                  
        }
        else            
        {
            SetAnswerVisuals(isCorrect:false);    
            SetTravelPoints(isCorrect: false);
            _countingDown = false;                    
        }                                             
    }                                                                        

    protected void SetAnswerVisuals(bool isCorrect)
    {
        answerMessage.text = isCorrect ? correctAnswerMessage : incorrectAnswerMessage;
        rewardValue.text = isCorrect ? _correctReward.ToString() : _incorrectReward.ToString();
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
            .OnComplete(() => {
                WaitForTapToUnload();
            });
    }
    
    protected void WaitForTapToUnload()
    {
        StartCoroutine(CheckForTap());
    }

    protected IEnumerator CheckForTap()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        _rewardsAnimator.SetBool(animatorOpenState, false);
        
        while (!IsAnimatorDone(_rewardsAnimator, animatorCloseState))
        {
            yield return null;
        }
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
        PlayerDataManager.Instance.AddTravelPoints(isCorrect ? _correctReward : _incorrectReward);
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
