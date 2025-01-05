using System;
using UnityEngine;

public class QuizReader : MonoBehaviour
{
    public QuestionData ParseQuizData(TextAsset textData)
    {
        try
        {
            return JsonUtility.FromJson<QuestionData>(textData.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse quizdata : {e.Message}");
            return null;
        }
    }
}