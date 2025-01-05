using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GeoguessrButtonPressAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button _button;
    private Color _originalColor;
    private Vector3 _originalPosition;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button.image == null)
        {
            Debug.LogError("GeoguessrButtonPressAnimation requires a Button with an Image component.");
            return;
        }
        _originalColor = _button.image.color;
        _originalPosition = _button.transform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button != null)
        {
            DOTween.Sequence()
                .Append(_button.transform.DOScale(0.95f, 0.1f))
                .Join(_button.transform.DOLocalMoveY(_originalPosition.y - 5f, 0.1f))
                .Join(_button.image.DOColor(Color.grey, 0.1f));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button != null)
        {
            DOTween.Sequence()
                .Append(_button.transform.DOScale(1f, 0.1f))
                .Join(_button.transform.DOLocalMoveY(_originalPosition.y, 0.1f))
                .Join(_button.image.DOColor(_originalColor, 0.1f))
                .Append(_button.transform.DOPunchScale(Vector3.one * 0.05f, 0.2f, 10, 0.5f));
        }
    }
}
