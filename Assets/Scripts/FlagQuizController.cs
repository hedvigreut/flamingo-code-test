using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum AnswerState{
    Correct,
    Incorrect,
}
public class FlagQuizController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI _flagQuestionText;
    [SerializeField, 
     Tooltip("Assign the flag images in the buttons to be populated")]
    private Image[] flags;
    [SerializeField] private GameObject _answerCorrectUI;
    [SerializeField] private TextMeshProUGUI _answerCorrectReward;
    [SerializeField] private GameObject _answerIncorrectUI;
    [SerializeField] private TextMeshProUGUI _answerIncorrectReward;

    [SerializeField] private int _correctReward = 5000;    
    [SerializeField] private int _incorrectReward = 2000; 
    
    [Header("Timer:")]
    [SerializeField] private float _totalTime = 15f;
    [SerializeField] private TMP_Text _currentTimeText;
    [SerializeField] private Slider _progressSlider;
    private float _timeLeft;
    private bool _countingDown;

    [Header("Data Reading")]
    [SerializeField] private TextAsset _quizData;
    [SerializeField] private FlagSpriteToID[] _flagSpritesToIds;
    [SerializeField] private QuizReader _quizReader;
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
        if (index == _currentQuestion.CorrectAnswerIndex)
        {
            Debug.Log("CORRECT ANSWER");
            SetTravelPoints(_correctReward);
            SetAnswerVisuals(AnswerState.Correct);
        }
        else
        {
            Debug.Log("NOT CORRECT ANSWER");
            SetTravelPoints(_incorrectReward);
            SetAnswerVisuals(AnswerState.Incorrect);
        }
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
            SetAnswerVisuals(AnswerState.Incorrect);        
            _countingDown = false;                    
        }                                             
    }                                                                        
    
    private void SetQuestionVisuals()
    {
        _flagQuestionText.text = _currentQuestion.Question;
        for (int i = 0; i < _currentQuestion.Answers.Count && i < flags.Length; i++)
        {
            string imageID = _currentQuestion.Answers[i].ImageID;
            Sprite flagSprite = GetSpriteByID(imageID);
            if (flagSprite != null)
            {
                flags[i].sprite = flagSprite;
            }
            else
            {
                Debug.LogWarning($"No sprite found for ImageID: {imageID}");
            }
        }
    }

    private void SetAnswerVisuals(AnswerState state)
    {
        switch (state)
        {
            case AnswerState.Correct:
                _answerCorrectUI.SetActive(true);
                _answerCorrectReward.text = _correctReward.ToString();
                AnimateRewardText(_answerCorrectReward);
                break;
            case AnswerState.Incorrect:
                _answerIncorrectUI.SetActive(true);
                _answerIncorrectReward.text = _incorrectReward.ToString();
                AnimateRewardText(_answerIncorrectReward);
                break;
        }
    }
    
    private void AnimateRewardText(TextMeshProUGUI rewardText)
    {
        // Set initial value to 0
        float targetValue = float.Parse(rewardText.text);  // Get the target value (correct or incorrect reward)
        rewardText.text = "0";  // Start from 0

        // Animate the number from 0 to the target value
        DOTween.Sequence()
            // First part: Animate the number count-up
            .Append(DOTween.To(() => 0f, x => rewardText.text = Mathf.FloorToInt(x).ToString(), targetValue, 1f)) // Duration of 1 second
            // Second part: Animate the scaling (upsizing)
            .Join(rewardText.transform.DOScale(Vector3.one * 1.2f, 1f))  // Scale up to 1.2 times
            // After both animations, scale back down
            .Append(rewardText.transform.DOScale(Vector3.one, 0.3f))
            // OnComplete to handle actions after animation finishes
            .OnComplete(() => {
                WaitForTapToUnload(); // Wait for tap to unload the scene
            });
    }
    
    private void WaitForTapToUnload()
    {
        StartCoroutine(CheckForTap());
    }

    private IEnumerator CheckForTap()
    {
        // Wait for any tap (on mobile or mouse click)
        while (!Input.GetMouseButtonDown(0)) // This works for both mouse and touch input
        {
            yield return null;  // Wait until the next frame
        }

        UnloadScene();  // Call the function to unload the scene
    }

    private void UnloadScene()
    {
        // Here, you can unload the scene, for example:
        SceneManager.UnloadSceneAsync("FlagQuiz");
        Debug.Log("Scene unloaded!");
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
