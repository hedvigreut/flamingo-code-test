using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct Board
{
    public string boardName;
    public List<TileType> tileTypes;
}

public interface IBoardFactory
{
    public BoardTile[] GetTiles();
}

public class BoardFactory : MonoBehaviour, IBoardFactory
{
    [SerializeField, Range(0f, 1f)] [Tooltip("How likely it is to get a default tile in between custom tiles")]
    private float probabilityDefaultBetweenCustomTiles = 0.5f;

    [SerializeField] private BoardTile[] tiles;
    [SerializeField] private TileType defaultTileType = TileType.Default;
    [SerializeField] private SavedBoardLayouts boardLayouts;

    private TileType[] _allQuizTileTypes;

    public BoardTile[] GetTiles()
    {
        return tiles;
    }

    public void Start()
    {
        var tileTypes = (TileType[])Enum.GetValues(typeof(TileType));
        _allQuizTileTypes = tileTypes.Where(t => t != TileType.Start).ToArray();
        LoadBoardLayout(PlayerDataManager.Instance.GetCurrentBoardIndex());
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
        TileType[] nonDefaultTileTypes = _allQuizTileTypes.Where(t => t != defaultTileType).ToArray();
        TileType newNonDefaultTile = nonDefaultTileTypes[Random.Range(0, nonDefaultTileTypes.Length)];
        return newNonDefaultTile;
    }

    public void Randomize()
    {
        var tileTypes = (TileType[])Enum.GetValues(typeof(TileType));
        _allQuizTileTypes = tileTypes.Where(t => t != TileType.Start).ToArray();
        AssignTileTypes();
    }

    public void Save()
    {
        var board = new Board();
        board.tileTypes = new List<TileType>();
        foreach (var tile in tiles)
        {
            board.tileTypes.Add(tile.GetTileType());
        }

        board.boardName = "Default Name";
        boardLayouts.boards.Add(board);
    }

    public void LoadBoardLayout(int boardIndex)
    {
        if (!boardLayouts.boards.Any())
        {
            Randomize();
            Save();
            return;
        }

        if (boardIndex >= boardLayouts.boards.Count)
        {
            Debug.LogWarning("Board Index is out of range, loading first board");
            PlayerDataManager.Instance.SetCurrentBoardIndex(0);
            boardIndex = 0;
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            var savedTileType = boardLayouts.boards[boardIndex].tileTypes[i];
            tiles[i].ChangeTileType(savedTileType);
            tiles[i].GrowAndWiggle();
        }

        Debug.Log($"Loaded a new board {boardLayouts.boards[boardIndex].boardName}");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BoardFactory))]
public class BoardFactoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);

        if (EditorApplication.isPlaying)
        {
            BoardFactory boardFactory = (BoardFactory)target;
            if (GUILayout.Button("Randomize"))
            {
                boardFactory.Randomize();
            }

            if (GUILayout.Button("Save"))
            {
                boardFactory.Save();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("To create new boards use buttons here in Play Mode.", MessageType.Info);
        }
    }
}
#endif