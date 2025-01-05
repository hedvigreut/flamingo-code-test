using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct FlagSpriteToID
{
    public Sprite sprite;
    public string ImageID;
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
    public List<Answer> Answers;
    public int CorrectAnswerIndex;
}
