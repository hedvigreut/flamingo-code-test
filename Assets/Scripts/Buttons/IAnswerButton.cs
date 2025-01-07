namespace Buttons
{
    public interface IAnswerButton
    {
        public void ChangeColor(bool isCorrect){}

        public bool IsAnimatorAnimationComplete();
    
    }
}
