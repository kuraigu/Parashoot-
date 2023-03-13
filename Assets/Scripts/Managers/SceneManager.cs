using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

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

    [SerializeField]
    private GameObject _loadingParent;
    [SerializeField]
    private GameObject _loadingHandle;

    [SerializeField]
    private ChangeSceneAdRandomizer changeSceneAd;

    public UnityAction OnStartSceneChange;

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
        OnStartSceneChange?.Invoke();

        float alpha;

        string previousSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (changeSceneAd != null)
        {
            if (changeSceneAd.CanShowAd())
            {
                changeSceneAd.ShowAd();

                float adTimeout = 0;

                while (!changeSceneAd.adClosed || !changeSceneAd.adFailed)
                {  
                    adTimeout += Time.unscaledDeltaTime;

                    if(adTimeout >= 30) break;
                    if (changeSceneAd.adClosed) break;
                    if (changeSceneAd.adFailed) break;
                    yield return null;
                }
            }
        }


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

        while (!asyncLoad.isDone)
        {
            if (_loadingParent != null)
            {
                if (!_loadingParent.activeSelf) _loadingParent.transform.gameObject.SetActive(true);
            }

            if (_loadingHandle != null)
            {
                Vector3 newScale = _loadingHandle.transform.localScale;

                newScale.x = Mathf.Clamp(asyncLoad.progress, 0f, 1f);

                _loadingHandle.transform.localScale = newScale;
            }
            yield return null;
        }

        //_crossFadeImage.gameObject.SetActive(false);
        while (asyncLoad.isDone && _crossFadeImage.canvasRenderer.GetAlpha() > 0f)
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
