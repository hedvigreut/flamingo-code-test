using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressBarText;
    [SerializeField] private float fakeLoadingScalar = 20f;

    private void Awake()
    {
        Application.targetFrameRate = 90;
    }

    private void Start()
    {
        StartCoroutine(LoadSceneRoutine("Board"));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        float fakeProgress = 0f;
        progressBar.value = 0f;
        progressBarText.text = "0%";

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (fakeProgress < 100f)
            {
                fakeProgress += Time.deltaTime * fakeLoadingScalar;
                progressBar.value = fakeProgress / 100f;
                progressBarText.text = Mathf.Clamp(fakeProgress, 0, 100).ToString("F0") + "%";
            }

            if (fakeProgress >= 90f && asyncOperation.progress >= 0.9f)
            {
                fakeProgress = 100f;
                progressBar.value = 1f;
                progressBarText.text = "100%";
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}