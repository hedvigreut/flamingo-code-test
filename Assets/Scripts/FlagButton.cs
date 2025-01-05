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
    public FlagAnimationState _lastAnimationState;

    public void ChangeColor(bool correct)
    {
        outline.color = correct ? correctColor : incorrectColor;
    }

    public void SetFlagImage(Sprite flag)
    {
        flagImage.sprite = flag;
    }
    
    public bool IsAnimatorAnimationComplete()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
        // Check if we're in the "Pressed" animation state and if its normalized time has completed
        return stateInfo.IsName(_lastAnimationState.ToString()) && stateInfo.normalizedTime >= 1f;
    }
    
    public enum FlagAnimationState
    {
        None,
        Normal,
        Pressed
    }
}
