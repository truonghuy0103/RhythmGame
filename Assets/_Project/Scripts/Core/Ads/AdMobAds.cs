using System;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobAds : IGameAds
{
    private BannerView _bannerAd;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

    private bool _hasRewarded = false;

    private Action<string, double> _rewardCallback;

    private Action _customRewardCallback = null;

    private Action _watchFailed = null;

    private Action _openedCallback = null;

    private Action _closedCallback = null;

    private double _rewardAmount;

    private string _rewardType;

    private string _bannerAdUnitID = "";

    private string _interstitialAdUnitID = "";

    private string _rewardedAdUnitID = "";

    private int _interstitialRetryAttempt;

    private int _rewardedRetryAttempt;

    private MonoBehaviour _target = null;

    public AdMobAds(MonoBehaviour target, string banneriOSAds, string bannerAndroidAds,
        string interiOSAds, string interAndroidAds, string rewardiOSAds, string rewardAndroidAds,
        Action<string, double> rewardCallback, Action openedCallback, Action closedCallback)
    {
        _target = target;

#if UNITY_ANDROID
        _bannerAdUnitID = bannerAndroidAds;
        _interstitialAdUnitID = interAndroidAds;
        _rewardedAdUnitID = rewardAndroidAds;
#else
        _bannerAdUnitID = banneriOSAds;
        _interstitialAdUnitID = interiOSAds;
        _rewardedAdUnitID = rewardiOSAds;
#endif

        _rewardCallback = rewardCallback;
        _openedCallback = openedCallback;
        _closedCallback = closedCallback;
    }

    public void Init()
    {
        // ✅ Set test device IDs (recommended during development)
        var requestConfiguration = new RequestConfiguration
        {
            TestDeviceIds = new List<string> { "CAD4CF379084C93DEC48AE6BCFC685CC" }
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);
        
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob Initialization success ");
            //Init banner
            InitBanner();
            //Init inter
            InitInterstitial();
            //Init reward
            InitRewardedVideo();
        });
    }

    #region Init

    void InitBanner()
    {
        if (!string.IsNullOrEmpty(_bannerAdUnitID))
        {
            if (_bannerAd != null)
            {
                _bannerAd.Destroy();
                _bannerAd = null;
            }
            _bannerAd = new BannerView(_bannerAdUnitID, AdSize.Banner, AdPosition.Bottom);
            
            AdRequest adRequest = new AdRequest();
            _bannerAd.LoadAd(adRequest);
            
            _bannerAd.OnBannerAdLoaded += BannerAdOnOnBannerAdLoaded;
            _bannerAd.OnBannerAdLoadFailed += BannerAdOnOnBannerAdLoadFailed;
            
            ShowBanner();
        }
    }
    
    void InitInterstitial()
    {
        _interstitialRetryAttempt = 0;
        
        LoadInterstitial();
    }

    void InitRewardedVideo()
    {
        _rewardedRetryAttempt = 0;
        
        LoadRewardedVideo();
    }

    #endregion

    #region Banner

    public void ShowBanner()
    {
        if (_bannerAd != null)
        {
            _bannerAd.Show();
        }
    }

    public void HideBanner()
    {
        if (_bannerAd != null)
        {
            _bannerAd.Hide();
        }
    }
    
    private void BannerAdOnOnBannerAdLoaded()
    {
        Debug.Log("AdMob BannerOnOnAdLoadedEvent");
    }

    private void BannerAdOnOnBannerAdLoadFailed(LoadAdError obj)
    {
        Debug.LogError("AdMob BannerAdOnOnBannerAdLoadFailed: " + obj.GetMessage());
    }

    #endregion

    #region Interstitial

    public bool IsInterstitialReady()
    {
        return _interstitialAd != null && _interstitialAd.CanShowAd();
    }

    public void LoadInterstitial()
    {
        if (!string.IsNullOrEmpty(_interstitialAdUnitID))
        {
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }
            
            AdRequest adRequest = new AdRequest();
            InterstitialAd.Load(_interstitialAdUnitID, adRequest, OnInterstitialLoaded);
        }
    }

    private void OnInterstitialLoaded(InterstitialAd ad, LoadAdError obj)
    {
        if (obj != null || ad == null)
        {
            _interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, _interstitialRetryAttempt));
            if (_target)
            {
                _target.Invoke("LoadInterstitial", (float)retryDelay);
            }
        }

        _interstitialAd = ad;

        InterstitialAdRegisterHandler();
    }

    private void InterstitialAdRegisterHandler()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.OnAdClicked -= InterstitialAdOnOnAdClicked;
            _interstitialAd.OnAdFullScreenContentOpened -= InterstitialAdOnOnAdFullScreenContentOpened;
            _interstitialAd.OnAdFullScreenContentClosed -= InterstitialAdOnOnAdFullScreenContentClosed;
            _interstitialAd.OnAdFullScreenContentFailed -= InterstitialAdOnOnAdFullScreenContentFailed;
            
            _interstitialAd.OnAdClicked += InterstitialAdOnOnAdClicked;
            _interstitialAd.OnAdFullScreenContentOpened += InterstitialAdOnOnAdFullScreenContentOpened;
            _interstitialAd.OnAdFullScreenContentClosed += InterstitialAdOnOnAdFullScreenContentClosed;
            _interstitialAd.OnAdFullScreenContentFailed += InterstitialAdOnOnAdFullScreenContentFailed;
        }
    }

    private void InterstitialAdOnOnAdFullScreenContentFailed(AdError obj)
    {
        LoadInterstitial();
    }

    private void InterstitialAdOnOnAdFullScreenContentClosed()
    {
        
        if (_closedCallback != null)
        {
            _closedCallback();
        }

        if (_customRewardCallback != null)
        {
            _customRewardCallback();
            _customRewardCallback = null;
        }
        
        LoadInterstitial();
    }

    private void InterstitialAdOnOnAdFullScreenContentOpened()
    {
        if (_openedCallback != null)
        {
            _openedCallback();
        }
    }

    private void InterstitialAdOnOnAdClicked()
    {
        
    }

    public void ShowInterstitial(Action finished)
    {
        if (!string.IsNullOrEmpty(_interstitialAdUnitID))
        {
            if (IsInterstitialReady())
            {
                _interstitialAd.Show();
            }
        }
    }

    #endregion

    #region RewardedVideo

    public bool IsRewardedReady()
    {
        return _rewardedAd != null && _rewardedAd.CanShowAd();
    }

    public void LoadRewardedVideo()
    {
        if (!string.IsNullOrEmpty(_rewardedAdUnitID))
        {
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
            
            AdRequest adRequest = new AdRequest();
            RewardedAd.Load(_rewardedAdUnitID, adRequest, OnRewardedLoaded);
        }
    }

    private void OnRewardedLoaded(RewardedAd ad,LoadAdError error)
    {
        if (error != null && ad == null)
        {
            _rewardedRetryAttempt++;
            double retryDelay = Math.Pow(2,Math.Min(6, _rewardedRetryAttempt));
            if (_target != null)
            {
                _target.Invoke("LoadRewardedVideo", (float)retryDelay);
            }
        }
        
        _rewardedAd = ad;

        RewardedAdRegisterHandler();
    }

    private void RewardedAdRegisterHandler()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.OnAdClicked -= RewardedAdOnOnAdClicked;
            _rewardedAd.OnAdFullScreenContentOpened -= RewardedAdOnOnAdFullScreenContentOpened;
            _rewardedAd.OnAdFullScreenContentClosed -= RewardedAdOnOnAdFullScreenContentClosed;
            _rewardedAd.OnAdFullScreenContentFailed -= RewardedAdOnOnAdFullScreenContentFailed;
            
            _rewardedAd.OnAdClicked += RewardedAdOnOnAdClicked;
            _rewardedAd.OnAdFullScreenContentOpened += RewardedAdOnOnAdFullScreenContentOpened;
            _rewardedAd.OnAdFullScreenContentClosed += RewardedAdOnOnAdFullScreenContentClosed;
            _rewardedAd.OnAdFullScreenContentFailed += RewardedAdOnOnAdFullScreenContentFailed;
        }
    }

    public void ShowRewardedVideo(Action finished, Action watchFailed)
    {
        if (!string.IsNullOrEmpty(_rewardedAdUnitID))
        {
            _customRewardCallback = finished;
            _watchFailed = watchFailed;
            if (IsRewardedReady())
            {
                _rewardedAd.Show(OnReceiveReward);
            }
        }
    }

    private void OnReceiveReward(Reward reward)
    {
        _hasRewarded = true;
    }
    
    private void RewardedAdOnOnAdFullScreenContentFailed(AdError obj)
    {
        LoadRewardedVideo();
    }

    private void RewardedAdOnOnAdFullScreenContentClosed()
    {
        Debug.Log("AdMob RewardedVideoOnOnAdFullScreenContentClosed");
        if (_hasRewarded)
        {
            if (_customRewardCallback != null)
            {
                _customRewardCallback();
                _customRewardCallback = null;
            }
            else
            {
                _rewardCallback(_rewardType, _rewardAmount);
            }
                
            _hasRewarded = false;
        }
        else
        {
            _watchFailed?.Invoke();
        }

        if (_closedCallback != null)
        {
            _closedCallback();
        }
            
        LoadRewardedVideo();
    }

    private void RewardedAdOnOnAdFullScreenContentOpened()
    {
        if (_openedCallback != null)
        {
            _openedCallback();
        }
    }

    private void RewardedAdOnOnAdClicked()
    {
        
    }

    #endregion
    

}
