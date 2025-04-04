using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
using TMPro;
using UnityEngine.UI;

namespace Huy
{
	public class WinParam : UIParam
	{
		public int coinReward;
	}
	
	public class Huy_UIWin : BaseUI
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
            
            //Get coin from Save
            WinParam winParam = param as WinParam;
            txtCoinReward.text = "+" + winParam.coinReward;
            
            Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Victory);
            
            btnX5.gameObject.SetActive(true);
            isWatchAds = false;
         }

         public void OnHome_Clicked()
         {
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         Huy_SoundManager.Instance.StopSoundSFX(SoundFXIndex.Victory);
	         //Show inter ads
	         
	         Huy_GameManager.Instance.GoToHome();
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
	         
	         //Disable Game Content
	         if (!isWatchAds)
	         {
		         //Add coin reward value to Save
	         }
         }
         
         public void OnX5_Clicked()
         {
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         UIManager.Instance.HideUI(UIIndex.UIGameplay);
	         UIManager.Instance.HideUI(this);
	         
	         //Show reward ads
	         isWatchAds = true;
	         valueCoinReward *= 5;
	         //Add coin reward to Save
	         btnX5.gameObject.SetActive(false);
         }
	}
}