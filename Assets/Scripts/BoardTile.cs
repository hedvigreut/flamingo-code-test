using System;
using DG.Tweening;
using UnityEngine;

public enum TileType
{
    Default,
    FlagQuiz,
    PictureQuiz
}

public class BoardTile : MonoBehaviour
{
    [SerializeField] 
    private float pressDuration = 0.25f;
    [SerializeField]
    private float pressDownScale = 0.2f;
    [SerializeField]
    private MeshRenderer tileRenderer;
    [SerializeField]
    private Color pressColor = Color.white;
    [SerializeField] 
    private TileType tileType;
    
    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;

    private const string ShaderBaseColor = "_BaseColor";

    private void Awake()
    {
        // Use a material property block to swap colors of the material of an instance of the tile effectively
        propertyBlock = new MaterialPropertyBlock();
        tileRenderer.GetPropertyBlock(propertyBlock);
        originalColor = propertyBlock.HasProperty(ShaderBaseColor) 
            ? propertyBlock.GetColor(ShaderBaseColor) 
            : tileRenderer.sharedMaterial.HasProperty(ShaderBaseColor) 
                ? tileRenderer.sharedMaterial.GetColor(ShaderBaseColor) 
                : Color.white;
    }

  public void Hop()
  {
      TileEffectAnimation();
  }
    private void TileEffectAnimation()
    {
        DOTween.To(
                () => GetCurrentColor(),
                x => SetTileColor(x),
                pressColor,
                pressDuration
            )
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        
        float currentScaleY = tileRenderer.transform.localScale.y;
        float pressedScaleY = currentScaleY * pressDownScale;
        
        tileRenderer.transform.localScale = new Vector3(tileRenderer.transform.localScale.x, pressedScaleY, tileRenderer.transform.localScale.z);
        tileRenderer.transform.DOScaleY(currentScaleY, pressDuration)
            .SetEase(Ease.OutBounce);
    }

    private Color GetCurrentColor()
    {
        tileRenderer.GetPropertyBlock(propertyBlock);
        return propertyBlock.HasProperty(ShaderBaseColor) 
            ? propertyBlock.GetColor(ShaderBaseColor) 
            : originalColor;
    }

    private void SetTileColor(Color color)
    {
        propertyBlock.SetColor(ShaderBaseColor, color);
        tileRenderer.SetPropertyBlock(propertyBlock);
    }

    public void Land()
    {
        Debug.Log($"Landed on {gameObject.name} with type {tileType})");
        TileEffectAnimation();
        switch (tileType)
        {
            case TileType.Default:
                break;
            case TileType.FlagQuiz:
                break;
            case TileType.PictureQuiz:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
