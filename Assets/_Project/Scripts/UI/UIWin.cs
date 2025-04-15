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
	
	public class UIWin : BaseUI
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
            valueCoinReward = winParam.coinReward;
            txtCoinReward.text = "+" + winParam.coinReward;
            txtCoin.text = GameManager.Instance.GameSave.Coin.ToString();
            
            SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Victory);
            
            btnX5.gameObject.SetActive(true);
            isWatchAds = false;
     
            AdsManager.Instance.ShowInterstitialAds(() =>
            {
	            
            });
         }

         public void OnHome_Clicked()
         {
	         SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         SoundManager.Instance.StopSoundSFX(SoundFXIndex.Victory);
	         //Show inter ads
	         
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
	         GameManager.Instance.GoToHome();
	         
	         //Add coin reward value to Save
	         GameManager.Instance.GameSave.Coin += valueCoinReward;
	         SaveManager.Instance.SaveGame();
         }
         
         public void OnX5_Clicked()
         {
	         SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         AdsManager.Instance.ShowRewardedAds(() =>
	         {
		         UIManager.Instance.HideUI(UIIndex.UIGameplay);
		         //UIManager.Instance.HideUI(this);
	         
		         //Show reward ads
		         isWatchAds = true;
		         valueCoinReward *= 5;
		         txtCoinReward.text = "+" + valueCoinReward;
		         //Add coin reward to Save
		         btnX5.gameObject.SetActive(false);
	         });
         }
	}
}