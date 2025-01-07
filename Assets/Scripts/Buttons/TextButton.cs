using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buttons
{
    public class TextButton : MonoBehaviour, IAnswerButton
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private Image buttonImage;
        [SerializeField]
        private Color correctColor;
        [SerializeField]
        private Color incorrectColor;
        [SerializeField] 
        private Animator _animator;
        private const string FinalAnimationState = "Pressed";
    
        public TextMeshProUGUI GetText()
        {
            return text;
        }
    
        public void ChangeColor(bool isCorrect)
        {
            buttonImage.color = isCorrect ? correctColor : incorrectColor;
        }
    
        public bool IsAnimatorAnimationComplete()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(FinalAnimationState) && stateInfo.normalizedTime >= 1f;
        }
    }
}
