using DG.Tweening;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] 
    private float pressDuration = 0.25f;
    [SerializeField]
    private float pressDownScale = 0.2f;
    [SerializeField]
    private MeshRenderer tileRenderer;
    public void Hop()
    {
        float currentScaleY = tileRenderer.transform.localScale.y;
        float pressedScaleY = currentScaleY * pressDownScale;
        tileRenderer.transform.localScale = new Vector3(tileRenderer.transform.localScale.x, pressedScaleY, tileRenderer.transform.localScale.z);
        tileRenderer.transform.DOScaleY(currentScaleY, pressDuration)
            .SetEase(Ease.OutBounce);
    }


    public void Land()
    {
        Debug.Log("Land"+ gameObject.name);
    }
}
