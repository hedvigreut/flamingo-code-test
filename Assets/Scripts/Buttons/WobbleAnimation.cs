using DG.Tweening;
using UnityEngine;

namespace Buttons
{
    public class WobbleAnimation : MonoBehaviour
    {
        [Header("Squish Settings")]
        [SerializeField]
        private float _minScale = 0.99f;
        [SerializeField]
        private float _maxScale = 1.01f;
        [SerializeField]
        private float _duration = 0.5f; 
        [SerializeField]
        private bool _playOnStart;

        private Vector3 originalScale;

        private void Start()
        {
            originalScale = transform.localScale;
            if (_playOnStart)
            {
                StartSquishLoop();
            }
        }

        private void StartSquishLoop()
        {
            transform.localScale = originalScale;
        
            Sequence squishSequence = DOTween.Sequence();
            squishSequence.Append(transform.DOScale(new Vector3(originalScale.x * _minScale, originalScale.y * _maxScale, originalScale.z), _duration / 2)
                .SetEase(Ease.InOutSine));
            squishSequence.Append(transform.DOScale(new Vector3(originalScale.x * _maxScale, originalScale.y * _minScale, originalScale.z), _duration / 2)
                .SetEase(Ease.InOutSine));
            squishSequence.SetLoops(-1, LoopType.Yoyo);
        }

        //TODO: Cleanup
        public void StopSquishLoop()
        {
            transform.DOKill();
            transform.localScale = originalScale;
        }
    }
}
