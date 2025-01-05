using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class QuizTransitionAnimation : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Attach the parent you want to fade in")]
    private CanvasGroup _fadeInCanvasGroup;

    [SerializeField] 
    private GameObject _headerArea;
    [SerializeField] 
    private GameObject _questionArea;
    [SerializeField] 
    private GameObject _flagArea;
    
    [Header("Animation Settings")]
    [SerializeField]
    private float _duration = 1f;
    [SerializeField] 
    private float _squishMultiplier = 0.5f;
    
    private void Start()
    {
        if (_fadeInCanvasGroup == null || _headerArea == null || _questionArea == null || _flagArea == null)
        {
            Debug.LogError("TransitionAnimation is missing object references");
            return;
        }
        AnimateFadeIn(_fadeInCanvasGroup);
        AnimateFromDirection(_flagArea, false);
        AnimateFromDirection(_headerArea, true);
        SquishAndStretch(_questionArea, true, false);
    }

    /// <summary>
    /// Takes a CanvasGroup and animates from fully invisible to opaque
    /// </summary>
    /// <param name="targetGroup"></param>
    private void AnimateFadeIn(CanvasGroup targetGroup)
    {
        targetGroup.alpha = 0f;
        targetGroup.DOFade(1f, _duration).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// Animate a GameObject's position and transparency from the specified direction (top or bottom) to its target position.
    /// </summary>
    /// <param name="target">The GameObject to animate.</param>
    /// <param name="fromTop">If true, the animation will start from the top; if false, it will start from the bottom.</param>
    private void AnimateFromDirection(GameObject target, bool fromTop)
    {
        Vector3 targetPosition = target.transform.position;
        float screenHeight = Screen.height;
        float startY = fromTop ? screenHeight : -screenHeight;
        Vector3 startPosition = new Vector3(targetPosition.x, startY, targetPosition.z);
        target.transform.position = startPosition;
        target.transform.DOMoveY(targetPosition.y, _duration).SetEase(Ease.OutBack);
    }
    
    /// <summary>
    /// Squishes a GameObject in a specified direction (horizontal or vertical) and then stretches it back to its original size.
    /// </summary>
    /// <param name="target">The GameObject to animate.</param>
    /// <param name="direction">The direction of the squish. True for horizontal, false for vertical.</param>
    private void SquishAndStretch(GameObject target, bool width, bool height)
    {
        Vector3 originalScale = target.transform.localScale;
        Vector3 squishedScale = new Vector3(
            width ? _squishMultiplier * originalScale.x : originalScale.x,
            height ? _squishMultiplier * originalScale.y : originalScale.y,
            originalScale.z
        );
        target.transform.localScale = squishedScale;
        target.transform.DOScale(originalScale, _duration).SetEase(Ease.OutQuad);
    }
}