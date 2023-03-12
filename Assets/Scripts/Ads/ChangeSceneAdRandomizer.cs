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

    // Start is called before the first frame update
    void Start()
    {
        if (AdManager.instance != null)
        {
            if (AdManager.instance.isTesting)
            {
                _adUnitId = "ca-app-pub-3940256099942544/1033173712"; ;
            }
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


        if (DataManager.instance != null)
        {
            if (DataManager.instance.data.highScore > 0)
            {
                float random = UnityEngine.Random.Range(0.0f, 100.0f);

                if (random <= _showChance)
                {
                    LoadAd();
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (SceneManager.instance != null)
        {
            //SceneManager.instance.OnStartSceneChange -= OnStartSceneChange;
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

                StartCoroutine(ShowAd());
            }
        );
    }

    private IEnumerator ShowAd()
    {
        while (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            yield return null;


            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
            }

        }

    }

    private void OnStartSceneChange()
    {
        ShowAd();
    }
}
