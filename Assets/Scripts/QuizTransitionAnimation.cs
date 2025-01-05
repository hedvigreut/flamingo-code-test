using UnityEngine;
using DG.Tweening;

public class QuizTransitionAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField]
    private float _duration = 1f;
    
    [SerializeField] 
    [Tooltip("Attach the parent you want to fade in")]
    private CanvasGroup _fadeInCanvasGroup;

    [SerializeField] 
    private GameObject _flagArea;
    
    private void Start()
    {
        if (_fadeInCanvasGroup == null)
        {
            Debug.LogError("No canvas group to fade in");
            return;
        }
        AnimateFadeIn();
        AnimateFlagArea();
    }

    private void AnimateFadeIn()
    {
        _fadeInCanvasGroup.alpha = 0f;
        _fadeInCanvasGroup.DOFade(1f, _duration).SetEase(Ease.OutCubic);
    }

    private void AnimateFlagArea()
    {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float startY = screenHeight;

        Vector3 targetPosition = _flagArea.transform.position;
        _flagArea.transform.position = new Vector3(_flagArea.transform.position.x, startY, _flagArea.transform.position.z);
        _flagArea.transform.DOMoveY(targetPosition.y, _duration).SetEase(Ease.OutBack);
    }
}