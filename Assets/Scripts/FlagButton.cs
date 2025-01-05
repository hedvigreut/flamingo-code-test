using UnityEngine;
using UnityEngine.UI;

public class FlagButton : MonoBehaviour
{
    [SerializeField] 
    private Image flagImage;
    [SerializeField] 
    private Image outline;
    [SerializeField]
    private Color correctColor;
    [SerializeField]
    private Color incorrectColor;

    public void ChangeColor(bool correct)
    {
        outline.color = correct ? correctColor : incorrectColor;
    }

    public void SetFlagImage(Sprite flag)
    {
        flagImage.sprite = flag;
    }
}
