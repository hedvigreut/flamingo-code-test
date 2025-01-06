using System;
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
    
    public void AssignTileTypes()
    {
        TileType previousType = defaultTileType;
        for (int i = 0; i < tiles.Length; i++)
        {
            TileType currentType;
            if (previousType != defaultTileType)
            {
                currentType = defaultTileType;
            }
            else
            {
                currentType = _allTileTypes[Random.Range(0, _allTileTypes.Length)];
            }

            tiles[i].ChangeTileType(currentType);
            previousType = currentType;
        }
    }
}