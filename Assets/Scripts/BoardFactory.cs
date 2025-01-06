using UnityEngine;
public interface IBoardFactory
{
    public void GenerateBoard();
}


public class BoardFactory : MonoBehaviour, IBoardFactory
{
    
    [SerializeField] 
    private BoardTile[] tiles;

    public BoardTile[] GetTiles()
    {
        return tiles;
    }

    public void GenerateBoard()
    { }
}