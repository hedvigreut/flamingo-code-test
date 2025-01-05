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
    [SerializeField] 
    private Animator _animator;
    private const string FinalAnimationState = "Pressed";

    public void ChangeColor(bool isCorrect)
    {
        outline.color = isCorrect ? correctColor : incorrectColor;
    }

    public void SetFlagImage(Sprite flag)
    {
        flagImage.sprite = flag;
    }
    
    public bool IsAnimatorAnimationComplete()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(FinalAnimationState) && stateInfo.normalizedTime >= 1f;
    }
    
}
