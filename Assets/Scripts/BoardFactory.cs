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
    
    private TileType[] _allTileTypes;

    public BoardTile[] GetTiles()
    {
        return tiles;
    }

    public void Start()
    {
        _allTileTypes = (TileType[])Enum.GetValues(typeof(TileType));
        AssignTileTypes(); 
    }
    
    private void AssignTileTypes()
    {
        TileType previousType = defaultTileType;
        for (int i = 0; i < tiles.Length; i++)
        {
            TileType currentType;
            // Rule 1: After a non-default tile a default follows
            if (previousType != defaultTileType)
            {
                currentType = defaultTileType;
            }
            else
            {
                // Rule 2: Randomise the chance of default tile based upon probability setting
                currentType = Random.value < probabilityDefaultBetweenCustomTiles
                    ? defaultTileType
                    : GetRandomNonDefaultTileType();
            }
            tiles[i].ChangeTileType(currentType);
            previousType = currentType;
        }
    }
    
    private TileType GetRandomNonDefaultTileType()
    {
        TileType[] nonDefaultTileTypes = _allTileTypes.Except(defaultTileType).ToArray();
        TileType newNonDefaultTile = nonDefaultTileTypes[Random.Range(0, nonDefaultTileTypes.Length)];
        return newNonDefaultTile;
    }
}