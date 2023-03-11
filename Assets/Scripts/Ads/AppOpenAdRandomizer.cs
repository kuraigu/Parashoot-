using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AppOpenAdRandomizer : MonoBehaviour
{
    [SerializeField]
    private string _adUnitId = "ad-unit";

    [Range(0, 100)]
    private float _showChance = 30.0f;

    private AppOpenAd _appOpenAd;

    // Start is called before the first frame update
    void Start()
    {
        if (AdManager.instance != null)
        {
            if (AdManager.instance.isTesting)
            {
                _adUnitId = "ca-app-pub-3940256099942544/3419835294"; ;
            }
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        float random = UnityEngine.Random.Range(0.0f, 100.0f);

        if(random <= _showChance)
        {
            LoadAd();
        }
    }

    private void LoadAd()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        AdRequest adRequest = new AdRequest.Builder().Build();

        ScreenOrientation screenOrientation = ScreenOrientation.Portrait;

        AppOpenAd.Load(_adUnitId, screenOrientation, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                _appOpenAd = ad;
            }
        );


        ShowAd();
    }

    private void ShowAd()
    {
        if (_appOpenAd != null && _appOpenAd.CanShowAd())
        {
            _appOpenAd.Show();
        }
    }
}
