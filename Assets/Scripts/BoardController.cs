using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour
{
    public void OnFlagQuizPressed()
    {
        StartFlagQuiz();
    }
    
    private async void StartFlagQuiz()
    {
        Debug.Log("FlagQuiz: Showing");

        var loadOperation = SceneManager.LoadSceneAsync("FlagQuiz", LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
}
