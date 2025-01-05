using System;
using UnityEngine;

public class QuizReader : MonoBehaviour
{
    /// <summary>
    /// Parse a given text asset and return the quiz data as an array of QuestionData
    /// </summary>
    /// <param name="textData">A text asset</param>
    /// <returns>Array of QuestionData</returns>
    public QuestionData[] ParseQuizData(TextAsset textData)
    {
        try
        {
            // Deserialize JSON into a QuestionDataArray containing the questions array
            return JsonUtility.FromJson<QuestionDataArray>(textData.text).questions;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse quizdata: {e.Message}");
            return null;
        }
    }
}

/// <summary>
/// Helper wrapper as with more questions, the JSON has an array
/// </summary>
[Serializable]
public class QuestionDataArray
{
    public QuestionData[] questions;
}