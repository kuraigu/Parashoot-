using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private static AdManager _instance;

    [SerializeField]
    private bool _isTesting;

    public static AdManager instance
    { get { return _instance; } }
    public bool isTesting
    { get { return _isTesting; } }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }
}
