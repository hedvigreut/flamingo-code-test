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

    [SerializeField] 
    private int _correctReward = 5000;    
    [SerializeField] 
    private int _incorrectReward = 2000; 
    
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
    private FlagSpriteToID[] _flagSpritesToIds;
    [SerializeField] 
    private QuizReader _quizReader;
    private QuestionData _currentQuestion;
    private Answer _currentAnswer;

    private const string incorrectAnswerMessage = "You'll get it right next time!";
    private const string correctAnswerMessage = "Well Done!";
    
    private void Awake()
    {
        _timeLeft = _totalTime;
        _currentTimeText.text = _timeLeft.ToString();
        _currentQuestion = _quizReader.ParseQuizData(_quizData);
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
        StartCoroutine(WaitForButtonAnimation(flagButton, isCorrect));
    }

    private IEnumerator WaitForButtonAnimation(FlagButton flagButton, bool correct)
    {
        _raycaster.enabled = false;
        yield return new WaitUntil(() => flagButton.IsAnimatorAnimationComplete());
        yield return new WaitForSeconds(0.5f);
        SetAnswerVisuals(correct);
    }

    /// <summary>
    /// Sets the flag images on the buttons from the ids
    /// </summary>
    private void SetQuestionVisuals()
    {
        _flagQuestionText.text = _currentQuestion.Question;
        for (int i = 0; i < _currentQuestion.Answers.Count && i < _flagButtons.Length; i++)
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
        Debug.Log(_correctAnswerFlag.ToString());
        Debug.Log(GetSpriteByID(_currentAnswer.ImageID).ToString());
        _correctAnswerFlag.sprite = GetSpriteByID(_currentAnswer.ImageID);
        Debug.Log(_correctAnswerFlag.ToString());

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

        UnloadScene();
    }

    private void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("FlagQuiz");
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
