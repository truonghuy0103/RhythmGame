using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huy_Core
{
    public class MAXAds : IGameAds
    {
        private string sdkKey;

        private bool hasRewarded = false;
        
        private Action<string,double> rewardCallback;

        private Action customRewardCallback = null;

        private Action watchFailed = null;
        
        private Action openedCallback = null;
        
        private Action closedCallback = null;

        private double rewardAmount;

        private string rewardType;

        private string bannerAdUnitID = "";
        
        private string interstitialAdUnitID = "";
        
        private string rewardedAdUnitID = "";

        private int interstitialRetryAttempt;
        
        private int rewardedRetryAttempt;
        
        private MonoBehaviour target = null;

        public MAXAds(MonoBehaviour target, string sdkKey, string banneriOSAds, string bannerAndroidAds,
            string interiOSAds, string interAndroidAds, string rewardiOSAds, string rewardAndroidAds,
            Action<string, double> rewardCallback, Action openedCallback, Action closedCallback)
        {
            this.target = target;
            this.sdkKey = sdkKey;
            
#if UNITY_ANDROID
            bannerAdUnitID = bannerAndroidAds;
            interstitialAdUnitID = interAndroidAds;
            rewardedAdUnitID = rewardAndroidAds;
#else
            bannerAdUnitID = banneriOSAds;
            interstitialAdUnitID = interiOSAds;
            rewardedAdUnitID = rewardiOSAds;
#endif
            
            this.rewardCallback = rewardCallback;
            this.openedCallback = openedCallback;
            this.closedCallback = closedCallback;
        }
        
        public void Init()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += configuration =>
            {   
                //Init banner
                InitBanner();
                //Init inter
                InitInterstitial();
                //Init reward
                InitRewardedVideo();
            };
            
            MaxSdk.SetSdkKey(sdkKey);
            MaxSdk.InitializeSdk();
        }
        
        #region Init

        void InitBanner()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += BannerOnOnAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += BannerOnOnAdLoadFailedEvent;

            if (!string.IsNullOrEmpty(bannerAdUnitID))
            {
                MaxSdk.CreateBanner(bannerAdUnitID,MaxSdkBase.BannerPosition.BottomCenter);
                ShowBanner();
            }
        }

        void InitInterstitial()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += InterstitialOnOnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += InterstitialOnOnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += InterstitialOnOnAdClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += InterstitialOnOnAdDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += InterstitialOnOnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialOnOnAdDisplayFailedEvent;
            
            LoadInterstitial();
        }

        void InitRewardedVideo()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += RewardedOnOnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += RewardedOnOnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += RewardedOnOnAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += RewardedOnOnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += RewardedOnOnAdDisplayFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += RewardedOnOnAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += RewardedOnOnAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += RewardedOnOnAdRevenuePaidEvent;
            
            LoadRewardedVideo();
        }
        
        #endregion

        public void Update()
        {
            
        }

        #region Banner
        public void ShowBanner()
        {
            if (!string.IsNullOrEmpty(bannerAdUnitID))
            {
                MaxSdk.ShowBanner(bannerAdUnitID);
            }
        }

        public void HideBanner()
        {
            if (!string.IsNullOrEmpty(bannerAdUnitID))
            {
                MaxSdk.HideBanner(bannerAdUnitID);
            }
        }
        #endregion
        
        #region Interstitial
        
        public bool IsInterstitialReady()
        {
            return MaxSdk.IsInterstitialReady(interstitialAdUnitID);
        }

        public void LoadInterstitial()
        {
            if (!string.IsNullOrEmpty(interstitialAdUnitID))
            {
                MaxSdk.LoadInterstitial(interstitialAdUnitID);
            }
        }

        public void ShowInterstitial(Action finished)
        {
            if (!string.IsNullOrEmpty(interstitialAdUnitID))
            {
                MaxSdk.ShowInterstitial(interstitialAdUnitID);
            }
        }
        
        #endregion
        
        #region RewardedVideo
        
        public bool IsRewardedReady()
        {
            return MaxSdk.IsRewardedAdReady(rewardedAdUnitID);
        }

        public void LoadRewardedVideo()
        {
            if (!string.IsNullOrEmpty(rewardedAdUnitID))
            {
                MaxSdk.LoadRewardedAd(rewardedAdUnitID);
            }
        }

        public void ShowRewardedVideo(Action finished, Action watchFailed)
        {
            if (!string.IsNullOrEmpty(rewardedAdUnitID))
            {
                customRewardCallback = finished;
                this.watchFailed = watchFailed;
                if (IsRewardedReady())
                {
                    MaxSdk.ShowRewardedAd(rewardedAdUnitID);
                }
            }
        }
        
        #endregion
        
        #region Callback
        private void RewardedOnOnAdRevenuePaidEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            Debug.Log("RewardedOnOnAdRevenuePaidEvent");
        }

        private void RewardedOnOnAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
        {
            hasRewarded = true;
        }

        private void RewardedOnOnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (hasRewarded)
            {
                if (customRewardCallback != null)
                {
                    customRewardCallback();
                    customRewardCallback = null;
                }
                else
                {
                    rewardCallback(rewardType, rewardAmount);
                }
                
                hasRewarded = false;
            }
            else
            {
                watchFailed?.Invoke();
            }

            if (closedCallback != null)
            {
                closedCallback();
            }
            
            LoadRewardedVideo();
        }

        private void RewardedOnOnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
        {
            LoadRewardedVideo();
        }

        private void RewardedOnOnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (openedCallback != null)
            {
                openedCallback();
            }
        }

        private void RewardedOnOnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            
        }

        private void RewardedOnOnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            Debug.Log("RewardedOnOnAdLoadFailedEvent");
            rewardedRetryAttempt++;
            double retryDelay = Math.Pow(2,Math.Min(6, rewardedRetryAttempt));
            if (target != null)
            {
                target.Invoke("LoadRewardedVideo", (float)retryDelay);
            }
        }

        private void RewardedOnOnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            Debug.Log("RewardedOnOnAdLoadedEvent");
            rewardedRetryAttempt = 0;
        }

        private void InterstitialOnOnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
        {
            LoadInterstitial();
        }

        private void InterstitialOnOnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            LoadInterstitial();
            if (closedCallback != null)
            {
                closedCallback();
            }

            if (customRewardCallback != null)
            {
                customRewardCallback();
                customRewardCallback = null;
            }
        }

        private void InterstitialOnOnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (openedCallback != null)
            {
                openedCallback();
            }
        }

        private void InterstitialOnOnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            
        }

        private void InterstitialOnOnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
            if (target)
            {
                target.Invoke("LoadInterstitial", (float)retryDelay);
            }
        }

        private void InterstitialOnOnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            interstitialRetryAttempt = 0;
        }

        private void BannerOnOnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            Debug.LogError("BannerOnOnAdLoadFailedEvent: " + arg2.Message);
        }

        private void BannerOnOnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            Debug.Log("BannerOnOnAdLoadedEvent");
        }

        #endregion
        
    }
}



