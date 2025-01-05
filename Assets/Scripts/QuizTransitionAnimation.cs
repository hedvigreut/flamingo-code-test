using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuizTransitionAnimation : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Attach the objects you want to animate and choose a type of animation")]
    private List<ObjectToAnimation> _objectsToAnimate;
    
    [Header("Animation Settings")]
    [SerializeField]
    private float _duration = 1f;
    [SerializeField] 
    private float _squishMultiplier = 0.5f;

    // Directional flags
    [SerializeField] private bool _headerFromTop = true;
    [SerializeField] private bool _questionFromTop = false;
    [SerializeField] private bool _flagFromTop = false;

    private void Start()
    {
        foreach (var objectToAnimate in _objectsToAnimate)
        {
            if (objectToAnimate.target != null)
            {
                ApplyAnimation(objectToAnimate);
            }
        }
    }
    
    private void ApplyAnimation(ObjectToAnimation objectToAnimate)
    {
        switch (objectToAnimate.type)
        {
            case AnimationType.Squish:
                SquishAndStretch(objectToAnimate.target);
                break;
            case AnimationType.FlyFromTop:
                FlyFromDirection(objectToAnimate.target, fromTop: true);
                break;
            case AnimationType.FlyFromBottom:
                FlyFromDirection(objectToAnimate.target, fromTop: false);
                break;
            case AnimationType.FadeIn:
                FadeIn(objectToAnimate.target);
                break;
            default:
                Debug.LogWarning($"Unsupported animation type: {objectToAnimate.type}");
                break;
        }
    }
    
    /// <summary>
    /// Squishes a GameObject in a specified direction (horizontal or vertical) and then stretches it back to its original size.
    /// </summary>
    private void SquishAndStretch(GameObject target)
    {
        Vector3 originalScale = target.transform.localScale;
        Vector3 squishedScale = new Vector3(
            _squishMultiplier * originalScale.x,
            originalScale.y,
            originalScale.z
        );

        target.transform.localScale = squishedScale;
        target.transform.DOScale(originalScale, _duration).SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// Animate a GameObject's position and transparency from the specified direction (top or bottom) to its target position.
    /// </summary>
    private void FlyFromDirection(GameObject target, bool fromTop)
    {
        Vector3 originalPosition = target.transform.position;
        float screenHeight = Screen.height;
        float startY = fromTop ? screenHeight : -screenHeight;

        Vector3 startPosition = new Vector3(originalPosition.x, startY, originalPosition.z);
        target.transform.position = startPosition;

        target.transform.DOMoveY(originalPosition.y, _duration).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Takes a CanvasGroup and animates from fully invisible to opaque
    /// </summary>
    private void FadeIn(GameObject target)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = target.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, _duration).SetEase(Ease.InQuad);
    }
}
