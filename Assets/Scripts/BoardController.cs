using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardController : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField]
    private Button _travelButton;
    [SerializeField] 
    private TextMeshProUGUI stepsCounterText;
    [SerializeField] 
    private TextMeshProUGUI _travelPointsValueText;
    
    [Header("Game settings")]
    [SerializeField] 
    private int travelPointRewardOnEmptyTile = 1000;
    [SerializeField] 
    private Player player;
    [FormerlySerializedAs("jumpSpeed")] [SerializeField]
    private float playerJumpSpeed = 0.3f;
    [SerializeField] 
    private BoardFactory boardFactory;
    
    [SerializeField]
    private GraphicRaycaster raycaster;
    [FormerlySerializedAs("maxSteps")]
    [SerializeField] 
    [Range(1,10)]
    private int randomMaxSteps = 10;
    [Header("Debug options")]
    
    private BoardTile[] tiles;
    private int currentTileIndex;
    private int _previousTravelPoints;
    private const string FlagQuizSceneName = "FlagQuiz";
    private const string PictureQuizSceneName = "PictureQuiz";
    
    private void Start()
    {
        _previousTravelPoints = PlayerManager.Instance.GetTravelPoints();
        _travelPointsValueText.text = _previousTravelPoints.ToString();
        tiles = boardFactory.GetTiles();
        var savedTilePosition = PlayerManager.Instance.GetCurrentPosition();
        currentTileIndex = savedTilePosition;
        SnapPlayerToTile(savedTilePosition);
    }

    public void OnTravelButtonPressed()
    {
        MoveSteps(Random.Range(1, randomMaxSteps + 1));
    }
    
    private void MoveSteps(int stepsToMove)
    {
        AnimateStepsCounter(stepsToMove);
    }
    
    private void AnimateStepsCounter(int stepsToMove)
    {
        string targetText = stepsToMove.ToString("00");
        stepsCounterText.transform.DOScaleY(-1f, 0.1f)
            .SetEase(Ease.Linear)  
            .SetLoops(10, LoopType.Yoyo)
            .OnUpdate(() => 
            {
                stepsCounterText.text = Random.Range(0, 100).ToString("00");
            })
            .OnKill(() =>
            {
                stepsCounterText.text = targetText;
                StartCoroutine(MovePlayerInSteps(currentTileIndex, stepsToMove));
            });
    }
    
    private IEnumerator MovePlayerInSteps(int startIndex, int stepsToMove)
    {
        yield return new WaitForSeconds(0.2f);
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
                        if (tiles[nextTileIndex].GetTileType() == TileType.Start && nextTileIndex == 0)
                        {
                            HopOverStartTile();
                        }
                    }
                    else
                    {
                        tiles[nextTileIndex].Land();
                    }
                });
        
            yield return new WaitForSeconds(playerJumpSpeed);
            currentTileIndex = nextTileIndex;
            PlayerManager.Instance.SetCurrentPosition(currentTileIndex);
        }
        // TODO: Replace once we have a transition animation to wait for it
        yield return new WaitForSeconds(0.5f);
        OnLanding();
        raycaster.enabled = true;
    }

    private void HopOverStartTile()
    {
        var currentBoardIndex = PlayerManager.Instance.GetCurrentBoardIndex();
        currentBoardIndex++;
        PlayerManager.Instance.SetCurrentBoardIndex(currentBoardIndex);
        boardFactory.LoadBoardLayout(currentBoardIndex++);
    }

    private void SnapPlayerToTile(int targetTileIndex)
    {
        float originalY = transform.position.y;
        Vector3 targetPosition = tiles[targetTileIndex].transform.position;
        targetPosition.y = originalY;
        player.transform.position = targetPosition;
    }

    private void OnLanding()
    {
        TileType landedTileType = tiles[currentTileIndex].GetTileType();
        switch (landedTileType)
        {
            case TileType.Default:
                player.PlayCoinEffect();
                player.PlayTravelPointsEffect(travelPointRewardOnEmptyTile);
                PlayerManager.Instance.AddTravelPoints(travelPointRewardOnEmptyTile);
                break;
            case TileType.Start:
                HopOverStartTile();
                break;
            case TileType.FlagQuiz:
                StartFlagQuiz();
                break;
            case TileType.PictureQuiz:
                StartPictureQuiz();
                break;
            default:
                Debug.LogWarning("Landed on unknown landing type");
                break;
        }
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
