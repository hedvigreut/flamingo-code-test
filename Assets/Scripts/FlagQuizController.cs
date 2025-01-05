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
    private FlagButton[] _flagButtons;
    [SerializeField] 
    private GameObject _answerCorrectUI;
    [SerializeField] 
    private TextMeshProUGUI _answerCorrectReward;
    [SerializeField] 
    private GameObject _answerIncorrectUI;
    [SerializeField] 
    private TextMeshProUGUI _answerIncorrectReward;
    [SerializeField] 
    private GraphicRaycaster _raycaster;


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
    
    private void Awake()
    {
        _timeLeft = _totalTime;
        _currentTimeText.text = _timeLeft.ToString();
        _currentQuestion = _quizReader.ParseQuizData(_quizData);
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

    /// <summary>
    /// When the button is clicked check if the button index corresponds to the
    /// correct answer
    /// </summary>
    /// <param name="index">Button Index</param>
    public void OnFlagButtonPressed(int index)
    {
        FlagButton flagButton = _flagButtons[index];
        bool isCorrect = index == _currentQuestion.CorrectAnswerIndex;
        
        if (isCorrect)
        {
            flagButton.ChangeColor(true);
            SetTravelPoints(_correctReward);
        }
        else
        {
            flagButton.ChangeColor(false);
            SetTravelPoints(_incorrectReward);
        }
        
        StartCoroutine(WaitForButtonAnimation(flagButton, isCorrect));
    }

    private IEnumerator WaitForButtonAnimation(FlagButton flagButton, bool correct)
    {
        _raycaster.enabled = false;
        yield return new WaitUntil(() => flagButton.IsAnimatorAnimationComplete());
        SetAnswerVisuals(correct);
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
            SetTravelPoints(_incorrectReward);
            SetAnswerVisuals(false);        
            _countingDown = false;                    
        }                                             
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
        if (isCorrect)
        {
            _answerCorrectUI.SetActive(true);
            _answerCorrectReward.text = _correctReward.ToString();
            AnimateRewardText(_answerCorrectReward);
        }
        else
        {
            _answerIncorrectUI.SetActive(true);
            _answerIncorrectReward.text = _incorrectReward.ToString();
            AnimateRewardText(_answerIncorrectReward);
        }
    }
    
    private void AnimateRewardText(TextMeshProUGUI rewardText)
    {
        float targetValue = float.Parse(rewardText.text);
        rewardText.text = "0";
        DOTween.Sequence()
            .Append(DOTween.To(() => 0f, x => rewardText.text = Mathf.FloorToInt(x).ToString(), targetValue, 1f))
            .Join(rewardText.transform.DOScale(Vector3.one * 1.2f, 1f))
            .Append(rewardText.transform.DOScale(Vector3.one, 0.3f))
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


    private void SetTravelPoints(int points)
    {
        PlayerManager.Instance.AddTravelPoints(points);
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
}
