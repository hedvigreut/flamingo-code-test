using System;
using UnityEngine;

[Serializable]
public struct ObjectToAnimation
{
    public GameObject target;
    public AnimationType type;

}

public enum AnimationType
{
    Squish,
    FlyFromTop,
    FlyFromBottom,
    FadeIn
}
