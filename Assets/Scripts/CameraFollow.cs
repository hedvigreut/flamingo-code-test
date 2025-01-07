using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followDuration = 0.5f;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void Update()
    {
        Vector3 targetPosition =
            new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.z);
        transform.DOMove(targetPosition, followDuration).SetEase(Ease.InOutQuad);
    }
}