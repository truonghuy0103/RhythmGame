using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiep;
using Hiep_Core;

namespace Hiep
{
    public class Hiep_UILose : BaseUI
    {
        [SerializeField] private GameObject goBtns;
        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            goBtns.SetActive(false);
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.GameOver);
            Timer.DelayedCall(2, () =>
            {
                // Show UI buttons
                goBtns.SetActive(true);
                
            }, this);
            AdsManager.Instance.ShowInterstitialAds(() =>
            {
				
            });
        }

        public void OnHome_Clicked()
        {
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            Hiep_SoundManager.Instance.StopSoundFX(SoundFXIndex.GameOver);
            
            UIManager.Instance.HideUI(this);
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
            // Show Inter ads
            
        }

        public void OnRestart_Clicked()
        {
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            Hiep_SoundManager.Instance.StopSoundFX(SoundFXIndex.GameOver);
            
            UIManager.Instance.HideUI(this);
            // Game Manager restart function
            Hiep_GameManager.Instance.RestartGame();
        }
    }
}