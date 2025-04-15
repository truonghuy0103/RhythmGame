using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huy_Core
{
    public class AdsManager : SingletonMono<AdsManager>
    {
        /*[Header("Applovin Settings")]
        private IGameAds maxApplovin = null;
        public bool maxApplovinSupport;
        public string maxSDKKey;
        public string maxAndroidBannerID;
        public string maxiOSBannerID;
        public string maxAndroidInterID;
        public string maxiOSInterID;
        public string maxAndroidRewardID;
        public string maxiOSRewardID;*/
        
        [Header("AdMob Settings")]
        private IGameAds _adMob = null;
        public bool adMobSupport;
        public string adMobAndroidBannerID;
        public string adMobiOSBannerID;
        public string adMobAndroidInterID;
        public string adMobiOSInterID;
        public string adMobAndroidRewardID;
        public string adMobiOSRewardID;
        

        private void Awake()
        {
            /*if (maxApplovinSupport)
            {
                maxApplovin = new MAXAds(this, maxSDKKey, maxiOSBannerID, maxAndroidBannerID, maxiOSInterID,
                    maxAndroidInterID, maxiOSRewardID, maxAndroidRewardID, RewardCallback, OpenedCallback,
                    ClosedCallback);
                
                maxApplovin.Init();
                ShowBanner();
            }*/

            if (adMobSupport)
            {
                _adMob = new AdMobAds(this, adMobiOSBannerID, adMobAndroidBannerID, adMobiOSInterID,
                    adMobAndroidInterID, adMobiOSRewardID, adMobAndroidRewardID, RewardCallback, OpenedCallback,
                    ClosedCallback);
                
                _adMob.Init();
                _adMob.ShowBanner();
            }
        }

        private void ClosedCallback()
        {
            SoundManager.Instance.ResumeAll();
        }

        private void OpenedCallback()
        {
            SoundManager.Instance.PauseAll();
        }

        private void RewardCallback(string arg1, double arg2)
        {

        }

        public void ShowBanner()
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.ShowBanner();
            }
            */

            if (adMobSupport && _adMob != null)
            {
                _adMob.ShowBanner();
            }
        }

        public void HideBanner()
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.HideBanner();
            }*/
            
            if (adMobSupport && _adMob != null)
            {
                _adMob.HideBanner();
            }
        }

        public bool IsInterstitialReady()
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                return maxApplovin.IsInterstitialReady();
            }*/
            
            if (adMobSupport && _adMob != null)
            {
                return _adMob.IsInterstitialReady();
            }
            
            return Application.isEditor;
        }

        public void LoadInterstitialAds()
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.LoadInterstitial();
            }*/
            
            if (adMobSupport && _adMob != null)
            {
                _adMob.LoadInterstitial();
            }
        }

        public void ShowInterstitialAds(Action finished)
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                if (IsInterstitialReady())
                {
                    maxApplovin.ShowInterstitial(finished);
                }
                else
                {
                    if (finished != null)
                    {
                        finished();
                    }
                }
            }*/
            
            if (adMobSupport && _adMob != null)
            {
                if (IsInterstitialReady())
                {
                    _adMob.ShowInterstitial(finished);
                }
                else
                {
                    if (finished != null)
                    {
                        finished();
                    }
                }
            }
        }

        public bool IsRewardedReady()
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                return maxApplovin.IsRewardedReady();
            }*/

            if (adMobSupport && _adMob != null)
            {
                return _adMob.IsRewardedReady();
            }
            
            return Application.isEditor;
        }

        public void LoadRewardedAds()
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                maxApplovin.LoadRewardedVideo();
            }*/
            
            if (adMobSupport && _adMob != null)
            {
                _adMob.LoadRewardedVideo();
            }
        }

        public void ShowRewardedAds(Action finished, Action watchFailed = null)
        {
            /*if (maxApplovinSupport && maxApplovin != null)
            {
                if (IsRewardedReady())
                {
                    maxApplovin.ShowRewardedVideo(finished, watchFailed);
                }
                else
                {
                    //Show UI No Internet
                }
            }*/
            
            if (adMobSupport && _adMob != null)
            {
                if (IsRewardedReady())
                {
                    _adMob.ShowRewardedVideo(finished, watchFailed);
                }
                else
                {
                    //Show UI No Internet
                }
            }
        }
    }
}

