using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huy_Core
{
    public interface IGameAds
    {
        void Init();
        
        void ShowBanner();
        
        void HideBanner();
        

        bool IsRewardedReady();

        void LoadRewardedVideo();

        void ShowRewardedVideo(Action finished, Action watchFailed);
        
        
        bool IsInterstitialReady();

        void LoadInterstitial();
        
        void ShowInterstitial(Action finished);
    }
}

