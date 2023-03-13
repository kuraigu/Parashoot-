using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ChangeSceneAdRandomizer : MonoBehaviour
{
    [SerializeField]
    private string _adUnitId = "ad-unit";

    [SerializeField]
    [Range(0, 100)]
    private float _showChance = 30.0f;

    private InterstitialAd _interstitialAd;

    public InterstitialAd interstitialAd
    { get { return _interstitialAd; } }


    private bool _adClosed;
    private bool _adFailed;

    public bool adClosed
    { get { return _adClosed; } }

    public bool adFailed
    { get { return _adFailed; } }

    // Start is called before the first frame update
    void Start()
    {
        if (AdManager.instance != null)
        {
            if (AdManager.instance.isTesting)
            {
                _adUnitId = "ca-app-pub-3940256099942544/1033173712"; ;
            }

            else
                _adUnitId = "ca-app-pub-2912355367336344/6328041912";
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        if (SceneManager.instance != null)
        {
            //SceneManager.instance.OnStartSceneChange += OnStartSceneChange;
        }

        LoadAd();
    }

    private void OnDestroy()
    {
        if (SceneManager.instance != null)
        {
            //SceneManager.instance.OnStartSceneChange -= OnStartSceneChange;
        }

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
        }
    }

    private void LoadAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest.Builder().Build();

        InterstitialAd.Load
        (_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                _interstitialAd = ad;
                //StartCoroutine(ShowAd());

                RegisterEventHandlers(_interstitialAd);
            }
        );
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += HandleOnAdClosed;
        ad.OnAdFullScreenContentFailed += HandleOnAdFailedToLoad;
        ad.OnAdClicked += HandleOnAdClosed;
    }

    private void HandleOnAdClosed()
    {
        _adClosed = true;
        DebugHandler.Log("Ad Closed" + _adClosed);
    }

    private void HandleOnAdFailedToLoad(AdError error)
    {
        _adFailed = true;
    }

    public void ShowAd()
    {
        if (_interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
    }

    public bool CanShowAd()
    {
        if (DataManager.instance != null)
        {
            if (DataManager.instance.data.highScore > 0)
            {
                float random = UnityEngine.Random.Range(0.0f, 100.0f);

                if (random <= _showChance)
                {
                    return true;
                }

                return false;
            }
        }

        return false;
    }
}
