using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlagQuizController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI _flagQuestionText;
    [SerializeField] 
    private GameObject _rewardUI;
    [SerializeField] 
    private Image _correctAnswerFlag;
    [SerializeField] 
    private TextMeshProUGUI _correctAnswerText;
    [SerializeField] 
    private TextMeshProUGUI _answerMessage;
    [SerializeField] 
    private TextMeshProUGUI _rewardValue;
    [SerializeField] 
    private GraphicRaycaster _raycaster;
    [SerializeField]
    private FlagButton[] _flagButtons;

    [Header("Reward settings:")]
    [SerializeField] 
    private int _correctReward = 5000;    
    [SerializeField] 
    private int _incorrectReward = 2000; 
    
    [Header("Animation:")]
    [SerializeField]
    private Animator _rewardsAnimator;
    
    [Header("Timer:")]
    [SerializeField] 
    private float _totalTime = 15f;
    [SerializeField] 
    private TMP_Text _currentTimeText;
    [SerializeField] 
    private Slider _progressSlider;
    private float _timeLeft;
    private bool _countingDown;

    [Header("Data Reading")]
    [SerializeField] 
    private TextAsset _quizData;
    [SerializeField] 
    private QuizVisualsData[] _flagSpritesToIds;
    [SerializeField] 
    private QuizReader _quizReader;
    private QuestionData[] _questions;
    private int currentQuestionIndex = 0;
    private QuestionData _currentQuestion;
    private Answer _currentAnswer;

    private const string incorrectAnswerMessage = "You'll get it right next time!";
    private const string correctAnswerMessage = "Well Done!";
    private const string animatorOpenState = "Open";
    private const string animatorCloseState = "Close";
    
    private const string SceneName = "FlagQuiz";

    
    private void Awake()
    {
        _timeLeft = _totalTime;
        _currentTimeText.text = _timeLeft.ToString();
        _questions = _quizReader.ParseQuizData(_quizData);
        currentQuestionIndex = PlayerManager.Instance.GetFlagQuestionIndex();
        if (currentQuestionIndex >= _questions.Length)
        {
            currentQuestionIndex = 0;
            PlayerManager.Instance.ResetCurrentFlagQuestionIndex();
        }
        _currentQuestion = _questions[currentQuestionIndex];
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
    
    private void Start()
    {
        _countingDown = true;
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
        currentQuestionIndex++;
        PlayerManager.Instance.SetCurrentFlagQuestionIndex(currentQuestionIndex);
        StartCoroutine(WaitForButtonAnimation(flagButton, isCorrect));
    }

    private IEnumerator WaitForButtonAnimation(FlagButton flagButton, bool correct)
    {
        _raycaster.enabled = false;
        yield return new WaitUntil(() => flagButton.IsAnimatorAnimationComplete());
        yield return new WaitForSeconds(0.5f);
        _raycaster.enabled = true;
        SetAnswerVisuals(correct);
    }

    /// <summary>
    /// Sets the flag images on the buttons from the ids
    /// </summary>
    private void SetQuestionVisuals()
    {
        _flagQuestionText.text = _currentQuestion.Question;
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

    private void SetAnswerVisuals(bool isCorrect)
    {
        _correctAnswerText.text = GetCountryNameByID(_currentAnswer.ImageID);
        _correctAnswerFlag.sprite = GetSpriteByID(_currentAnswer.ImageID);
        _answerMessage.text = isCorrect ? correctAnswerMessage : incorrectAnswerMessage;
        _rewardValue.text = isCorrect ? _correctReward.ToString() : _incorrectReward.ToString();
        _rewardUI.SetActive(true);
        AnimateRewardText();
    }
    
    private void AnimateRewardText()
    {
        float targetValue = float.Parse(_rewardValue.text);
        _rewardValue.text = "0";
        DOTween.Sequence()
            .Append(DOTween.To(() => 0f, x => _rewardValue.text = Mathf.FloorToInt(x).ToString(), targetValue, 1f))
            .Join(_rewardValue.transform.DOScale(Vector3.one * 1.2f, 1f))
            .Append(_rewardValue.transform.DOScale(Vector3.one, 0.3f))
            .OnComplete(() => {
                WaitForTapToUnload();
            });
    }
    
    private void WaitForTapToUnload()
    {
        StartCoroutine(CheckForTap());
    }

    private IEnumerator CheckForTap()
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
    
    private bool IsAnimatorDone(Animator animator, string stateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1f;
    }
    
    private void UnloadScene()
    {
        DOTween.KillAll();
        SceneManager.UnloadSceneAsync(SceneName);
    }

    private void SetTravelPoints(bool isCorrect)
    {
        PlayerManager.Instance.AddTravelPoints(isCorrect ? _correctReward : _incorrectReward);
    }
    
    private Sprite GetSpriteByID(string imageID)
    {
        foreach (var flagSpriteToID in _flagSpritesToIds)
        {
            if (flagSpriteToID.ImageID == imageID)
            {
                return flagSpriteToID.sprite;
            }
        }
        return null;
    }
    
    private string GetCountryNameByID(string imageID)
    {
        foreach (var flagSpriteToID in _flagSpritesToIds)
        {
            if (flagSpriteToID.ImageID == imageID)
            {
                return flagSpriteToID.name;
            }
        }
        return null;
    }
}
