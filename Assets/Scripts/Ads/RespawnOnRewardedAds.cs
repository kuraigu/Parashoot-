using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class RespawnOnRewardedAds : MonoBehaviour
{
    private RewardedAd _rewardedAd;
    private string _adUnitId = "your-rewarded-ad-unit-id-here";

    [SerializeField]
    private GameObject _toClose;

    private void Start()
    {
        if (AdManager.instance != null)
        {
            if (AdManager.instance.isTesting)
                _adUnitId = "ca-app-pub-3940256099942544/5224354917";
        }


        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        LoadRewardedAd();
    }

    private void OnDestroy()
    {
        if(this._rewardedAd != null)
        {
            _rewardedAd.Destroy();
        }
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                if (GameManager.instance != null)
                {
                    if(_toClose != null)
                        _toClose.gameObject.SetActive(false);
                    GameManager.instance.Retry();

                    LoadRewardedAd();
                }
            });
        }

        else 
        {
            if(FloatingMessageManager.instance != null)
            {
                FloatingMessageManager.instance.SpawnFloatingText("Ad is currently unavailable! Try again later.");
            }
        }
    }
}