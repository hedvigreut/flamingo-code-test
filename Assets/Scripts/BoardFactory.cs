using System;
using System.Linq;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IBoardFactory
{
    public BoardTile[] GetTiles();
}

public class BoardFactory : MonoBehaviour, IBoardFactory
{
    [SerializeField, Range(0f, 1f)]
    [Tooltip("How likely it is to get a default tile inbetween custom tiles")]
    private float probabilityDefaultBetweenCustomTiles = 0.5f;
    [SerializeField] 
    private BoardTile[] tiles;
    [SerializeField]
    private TileType defaultTileType = TileType.Default;
    
    private TileType[] _allQuizTileTypes;

    public BoardTile[] GetTiles()
    {
        return tiles;
    }

    public void Start()
    {
        var tileTypes = (TileType[])Enum.GetValues(typeof(TileType));
        _allQuizTileTypes = tileTypes.Except(TileType.Start).ToArray();
        AssignTileTypes(); 
    }
    
    /// <summary>
    /// Assigns tiles so that non-default tiles are never after each other and there is a variable for
    /// more liklihood of default tiles inbetween quiz type tiles
    ///  Rule 1: After a non-default tile a default follows
    /// Rule 2: Randomise the chance of default tile based upon probability setting
    /// </summary>
    private void AssignTileTypes()
    {
        TileType previousType = defaultTileType;
        for (int i = 0; i < tiles.Length; i++)
        {
            TileType currentType;
            if (i == 0)
            {
                currentType = TileType.Start;
            }
            else
            {
                // After any non-default tile, a default follows
                currentType = previousType != defaultTileType
                    ? defaultTileType
                    : (Random.value < probabilityDefaultBetweenCustomTiles
                        ? defaultTileType
                        : GetRandomNonDefaultTileType());
            }

            tiles[i].ChangeTileType(currentType);
            previousType = currentType;
        }
    }
    
    private TileType GetRandomNonDefaultTileType()
    {
        TileType[] nonDefaultTileTypes = _allQuizTileTypes.Except(defaultTileType).ToArray();
        TileType newNonDefaultTile = nonDefaultTileTypes[Random.Range(0, nonDefaultTileTypes.Length)];
        return newNonDefaultTile;
    }
}