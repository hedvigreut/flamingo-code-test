using System;
using System.Linq;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IBoardFactory
{
    public void AssignTileTypes();
}

public class BoardFactory : MonoBehaviour, IBoardFactory
{
    
    [SerializeField] 
    private BoardTile[] tiles;

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

    public void AssignTileTypes()
    {
        TileType previousType = new TileType();
        foreach (var tile in tiles)
        {
            var randomType = _allTileTypes[Random.Range(0, _allTileTypes.Length)];
            if (randomType == previousType)
            {
                randomType = GetNewRandomTileType(randomType);
            }
            tile.ChangeTileType(randomType);
            previousType = randomType;
        }
    }
    
    private TileType GetNewRandomTileType(TileType typeToAvoid)
    {
        TileType[] arrayWithoutTypeToAvoid = _allTileTypes.Except(typeToAvoid).ToArray();
        TileType newType = arrayWithoutTypeToAvoid[Random.Range(0, arrayWithoutTypeToAvoid.Length)];
        return newType;
    }
}