using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SplashController : MonoBehaviour
{
    public void Awake()
    {
        StartSplashScreen();
    }
    private async void StartSplashScreen()
    {
        Debug.Log("Splash: Showing");

        // Simulation - TODO: Progress bar 
        await Task.Delay(5000);
        var loadOperation = SceneManager.LoadSceneAsync("Board", LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
        await UnloadSplashSceneAsync();
    }

    private async Task UnloadSplashSceneAsync()
    {
        // Unload the scene using Unity's SceneManager
        var unloadOperation = SceneManager.UnloadSceneAsync("Splash");

        // Wait until the scene has finished unloading
        while (!unloadOperation.isDone)
        {
            await Task.Yield();
        }
        Debug.Log("Splash: Unloaded");
    }
}