using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    private static SceneManager _instance;

    [SerializeField]
    private Image _crossFadeImage;

    [SerializeField]
    private bool _allowTransitionOnWake;

    [SerializeField]
    private bool _allowTransitionOnEnd;

    [SerializeField]
    private float _crossFadeDuration = 0.3f;

    public static SceneManager instance
    { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }


    public void LoadSceneAsync(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadSceneAsyncWithTransition(string sceneName)
    {
        StartCoroutine(ScreenTransition(sceneName));
    }

    private IEnumerator ScreenTransition(string sceneName)
    {
        float alpha;

        string previousSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Fade in the crossfade image
        _crossFadeImage.gameObject.SetActive(true);
        _crossFadeImage.canvasRenderer.SetAlpha(0f);

        alpha = 0f;
        // Wait for the new scene to load
        while (_crossFadeImage.canvasRenderer.GetAlpha() < 1f)
        {
            alpha += (1 / _crossFadeDuration) * Time.unscaledDeltaTime;
            _crossFadeImage.canvasRenderer.SetAlpha(alpha);
            yield return null;
        }

        NormalizeTimeScale();
        alpha = 1f;
        _crossFadeImage.canvasRenderer.SetAlpha(alpha);
        
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        //_crossFadeImage.gameObject.SetActive(false);
        while(asyncLoad.isDone && _crossFadeImage.canvasRenderer.GetAlpha() > 0f)
        {
            alpha -= (1 / _crossFadeDuration) * Time.unscaledDeltaTime;
            _crossFadeImage.canvasRenderer.SetAlpha(alpha);
            yield return null;
        }

    }

    public void NormalizeTimeScale()
    {
        Time.timeScale = 1.0f;
    }
}
