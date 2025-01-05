using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    private async void Awake()
    {
        await StartSplashScreen();
    }

    private async Task StartSplashScreen()
    {
        Debug.Log("Splash: Showing");

        // Simulate loading delay (e.g., a progress bar in the future)
        await Task.Delay(2000);
        SceneManager.LoadScene("Board", LoadSceneMode.Single);
        Debug.Log("Splash: Transitioning to BoardScene");
    }
}