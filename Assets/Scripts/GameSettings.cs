using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Flamingo/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [Header("Player settings:")] public float playerJumpSpeed = 0.3f;
    public int randomMaxSteps = 10;

    [Header("Reward settings:")] public int correctTravelPointReward = 4000;
    public int incorrectTravelPointReward = 1000;
    public int travelPointRewardOnEmptyTile = 1000;
}