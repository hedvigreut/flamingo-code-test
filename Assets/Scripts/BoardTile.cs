using System;
using DG.Tweening;
using UnityEngine;

public enum TileType
{
    Default,
    Start,
    FlagQuiz,
    PictureQuiz
}

public class BoardTile : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer tileRenderer;
    [SerializeField] 
    private TileType tileType;
    
    [Header("Color settings")]
    [SerializeField] 
    private Color defaultTileColor;
    [SerializeField] 
    private Color startColor;
    [SerializeField] 
    private Color flagTileColor;
    [SerializeField] 
    private Color pictureTileColor;
    [SerializeField]
    private float pressLightenFactor;
    
    [Header("Animation settings")]
    [SerializeField] 
    private float pressDuration = 0.25f;
    [SerializeField]
    private float pressDownScale = 0.2f;
    
    private MaterialPropertyBlock _propertyBlock;
    private Color _originalColor;
    private const string ShaderBaseColor = "_BaseColor";

    private void Awake()
    {
        // Use a material property block to swap colors of the material of an instance of the tile effectively
        _propertyBlock = new MaterialPropertyBlock();
        tileRenderer.GetPropertyBlock(_propertyBlock);
    }

    private void Start()
    {
        ChangeTileColor(tileType);
    }

    public TileType GetTileType()
    {
        return tileType;
    }

    /// <summary>
    /// Change the tile type
    /// </summary>
    /// <param name="newTileType">Tile type we want to change to</param>
    public void ChangeTileType(TileType newTileType)
    {
        tileType = newTileType;
        ChangeTileColor(newTileType);
    }
    private void ChangeTileColor(TileType newTileType)
    {
        switch (newTileType)
        {
            case TileType.Default:
                _propertyBlock.SetColor(ShaderBaseColor, defaultTileColor);
                break;
            case TileType.Start:
                _propertyBlock.SetColor(ShaderBaseColor, startColor);
                break;
            case TileType.FlagQuiz:
                _propertyBlock.SetColor(ShaderBaseColor, flagTileColor);
                break;
            case TileType.PictureQuiz:
                _propertyBlock.SetColor(ShaderBaseColor, pictureTileColor);
                break;
            default:
                Debug.LogWarning("Unknown tile type");
                break;
        }
        tileRenderer.SetPropertyBlock(_propertyBlock);
    }
    public void Hop()
  {
      _originalColor = _propertyBlock.HasProperty(ShaderBaseColor) 
          ? _propertyBlock.GetColor(ShaderBaseColor) 
          : tileRenderer.sharedMaterial.HasProperty(ShaderBaseColor) 
              ? tileRenderer.sharedMaterial.GetColor(ShaderBaseColor) 
              : Color.white;
      TileEffectAnimation();
  }
    private void TileEffectAnimation()
    {
        DOTween.To(
                () => GetCurrentColor(),
                x => SetTileColor(x),
                LightenColor(_originalColor, pressLightenFactor),
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
    
    public void Land()
    {
        Debug.Log($"Landed on {gameObject.name} with type {tileType})");
        TileEffectAnimation();
        switch (tileType)
        {
            case TileType.Default:
                break;
            case TileType.Start:
                break;
            case TileType.FlagQuiz:
                break;
            case TileType.PictureQuiz:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private Color GetCurrentColor()
    {
        tileRenderer.GetPropertyBlock(_propertyBlock);
        return _propertyBlock.HasProperty(ShaderBaseColor) 
            ? _propertyBlock.GetColor(ShaderBaseColor) 
            : _originalColor;
    }

    private void SetTileColor(Color color)
    {
        _propertyBlock.SetColor(ShaderBaseColor, color);
        tileRenderer.SetPropertyBlock(_propertyBlock);
    }
    
    private Color LightenColor(Color color, float factor)
    {
        return Color.Lerp(color, Color.white, factor);
    }
}
