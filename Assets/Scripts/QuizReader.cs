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
            // Try to parse as an array of questions
            var parsedData = JsonUtility.FromJson<QuestionDataArray>(textData.text);
            if (parsedData != null && parsedData.questions != null)
            {
                return parsedData.questions;
            }

            // If parsing as an array fails, try parsing as a single question
            var singleQuestion = JsonUtility.FromJson<QuestionData>(textData.text);
            if (singleQuestion != null)
            {
                return new[] { singleQuestion }; // Return as a single-element array
            }

            throw new Exception("Invalid JSON format");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse quizdata: {e.Message}");
            return null;
        }
    }
}

/// <summary>
/// Helper wrapper for multiple questions
/// </summary>
[Serializable]
public class QuestionDataArray
{
    public QuestionData[] questions;
}