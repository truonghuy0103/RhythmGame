using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiep;
using Hiep_Core;
using TMPro;
using UnityEngine.UI;

namespace Hiep
{
	public class WinParam : UIParam
	{
		public int coinReward;
	}
	
	public class HIep_UIWin : BaseUI
	{
		[SerializeField] private TextMeshProUGUI txtCoinReward;
		[SerializeField] private TextMeshProUGUI txtCoin;

		[SerializeField] private Button btnX5;

		private bool isWatchAds;
		private int valueCoinReward;
		
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            // Get coin from Save
            WinParam winParam = (WinParam)param;
            txtCoinReward.text = "+" + winParam.coinReward;
            
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Victory);
            
			btnX5.gameObject.SetActive(true);
			isWatchAds = false;
			AdsManager.Instance.ShowInterstitialAds(() =>
			{
				
			});
         }

         public void OnHome_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         Hiep_SoundManager.Instance.StopSoundFX(SoundFXIndex.Victory);
	         // Show inter ads
	         
	         UIManager.Instance.HideUI(UIIndex.UIGameplay);
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
	         
	         // Disable Game Content
	         if (!isWatchAds)
	         {
		         // Add coin reward value to Save
	         }
         }

         public void OnX5_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         AdsManager.Instance.ShowInterstitialAds(() =>
	         {
		         UIManager.Instance.HideUI(UIIndex.UIGameplay);

		         // Show reward ads
		         isWatchAds = true;
		         valueCoinReward *= 5;
		         // Add coin reward to Save
		         btnX5.gameObject.SetActive(false);
	         });
         }
	}
}