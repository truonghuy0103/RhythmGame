using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
namespace Huy
{
	public class UIPause : BaseUI
	{
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            
            Time.timeScale = 0;
         }

         public void OnResume_Clicked()
         {
	         SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         //Resume game
	         GameManager.Instance.ResumeGame();
	         //Show inter ads
	         UIManager.Instance.HideUI(this);
         }

         public void OnHome_Clicked()
         {
	         SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         //End game
	         GameManager.Instance.GoToHome();
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
	         
	         AdsManager.Instance.ShowInterstitialAds(() =>
	         {
	         });
         }
         
         public void OnRestart_Clicked()
         {
	         SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.HideUI(UIIndex.UIGameplay);
	         //Show inter ads
	         //Restart game
	         GameManager.Instance.RestartGame();
         }
	}
}