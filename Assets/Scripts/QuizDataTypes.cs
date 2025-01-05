using UnityEngine;
using System;

[Serializable]
public struct QuizVisualsData
{
    public Sprite sprite;
    public string ImageID;
    public string name;
}

[Serializable]
public class Answer
{
    public string ImageID;
    public string Text;
}

[Serializable]
public class QuestionData
{
    public string ID;
    public int QuestionType;
    public string Question;
    public string CustomImageID;
    public Answer[] Answers;
    public int CorrectAnswerIndex;
}