using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;


[System.Obsolete]
public class GenericBannerAd : MonoBehaviour
{
    private BannerView _bannerView;

    [SerializeField]
    private string _adUnitId = "your-ad-unit-id-here";

    private float _chance = 30.0f;

    void Start()
    {
        if(AdManager.instance != null)
        {
            if(AdManager.instance.isTesting)
                _adUnitId = "ca-app-pub-3940256099942544/6300978111";

            else 
                 _adUnitId = "ca-app-pub-2912355367336344/3484470317"; // banner id
        }

        
        //this.RequestBanner();
    }

    void OnDestroy()
    {
        if(_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }


    void OnEnable()
    {
        /**float indicator = UnityEngine.Random.Range(0.0f, 100f);

        if(indicator > _chance)
        {
            return;
        }

        if(this._bannerView != null)
        {
            this._bannerView.Show();
        }**/
    }

    void OnDisable()
    {
        /**if(this._bannerView != null)
        {
            this._bannerView.Hide();
        }**/
    }

    private void RequestBanner()
    {
      
        // Clean up banner ad before creating a new one.
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        _bannerView = new BannerView(_adUnitId, adaptiveSize, AdPosition.Bottom);

        AdRequest adRequest = new AdRequest.Builder().Build();

        // Load a banner ad.
        _bannerView.LoadAd(adRequest);
    }
}