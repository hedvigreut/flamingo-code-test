using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavedBoardLayouts", menuName = "Flamingo/SavedBoardLayouts", order = 1)]
public class SavedBoardLayouts : ScriptableObject
{
    public List<Board> boards;
}