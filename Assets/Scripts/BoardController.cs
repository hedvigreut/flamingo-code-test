using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField]
    private Button _travelButton;
    [SerializeField] 
    private TextMeshProUGUI _travelPointsValueText;
    
    [Header("Game settings")]
    [SerializeField] 
    private Player player;
    [FormerlySerializedAs("jumpSpeed")] [SerializeField]
    private float playerJumpSpeed = 0.3f;
    [SerializeField] 
    private BoardFactory boardFactory;
    [SerializeField] 
    private BoardTile[] tiles;
    [SerializeField]
    private GraphicRaycaster raycaster;
    
    private int currentTileIndex;
    private int _previousTravelPoints;
    private const string FlagQuizSceneName = "FlagQuiz";
    private const string PictureQuizSceneName = "PictureQuiz";
    
    private void Start()
    {
        _previousTravelPoints = PlayerManager.Instance.GetTravelPoints();
        _travelPointsValueText.text = _previousTravelPoints.ToString();
        tiles = boardFactory.GetTiles();
    }
    
    public void MoveSteps(int stepsToMove)
    {
        int stepCount = stepsToMove;
        int startIndex = currentTileIndex;
        StartCoroutine(MovePlayer(startIndex, stepCount));
    }
    
    private IEnumerator MovePlayer(int startIndex, int stepsToMove)
    {
        raycaster.enabled = false;
        int totalTiles = tiles.Length;
        float originalY = player.transform.position.y;
    
        for (int i = 1; i <= stepsToMove; i++)
        {
            // Using modulo to set index if needed to loop around the board
            int nextTileIndex = (startIndex + i) % totalTiles;
            Vector3 targetPosition = tiles[nextTileIndex].transform.position;
            targetPosition.y = originalY;
        
            player.transform.DOJump(targetPosition, jumpPower: 0.5f, numJumps: 1, duration: playerJumpSpeed)
                .SetEase(Ease.OutQuad)
                .OnKill(() =>
                {
                    if (i < stepsToMove)
                    {
                        tiles[nextTileIndex].Hop();
                    }
                    else
                    {
                        tiles[nextTileIndex].Land();
                    }
                });
        
            yield return new WaitForSeconds(playerJumpSpeed);
            currentTileIndex = nextTileIndex;
        }
        raycaster.enabled = true;
    }


    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    /// <summary>
    /// If another quiz scene unloads, let's update travel points if changed
    /// </summary>
    /// <param name="scene"></param>
    private void OnSceneUnloaded(Scene scene)
    {
        int currentTravelPoints = PlayerManager.Instance.GetTravelPoints();
        if (currentTravelPoints != _previousTravelPoints)
        {
            AnimateTravelPointsChange(_previousTravelPoints, currentTravelPoints);
            _previousTravelPoints = currentTravelPoints;
        }
    }
    
    private void AnimateTravelPointsChange(int fromValue, int toValue)
    {
        DOTween.To(() => (float)fromValue, 
                x => _travelPointsValueText.text = Mathf.FloorToInt(x).ToString(),
                toValue, 1f)
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// Debug button to start Flag quiz
    /// </summary>
    public void OnFlagQuizPressed()
    {
        StartFlagQuiz();
    }
    
    private async void StartFlagQuiz()
    {
        var loadOperation = SceneManager.LoadSceneAsync(FlagQuizSceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
    
    /// <summary>
    /// Debug button to start Text quiz
    /// </summary>
    public void OnPictureQuizPressed()
    {
        StartPictureQuiz();
    }

    private async void StartPictureQuiz()
    {
        var loadOperation = SceneManager.LoadSceneAsync(PictureQuizSceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
}
