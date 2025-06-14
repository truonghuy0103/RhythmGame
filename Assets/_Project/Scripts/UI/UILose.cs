using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
namespace Huy
{
	public class UILose : BaseUI
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
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.GameOver);
			Timer.DelayedCall(2, () =>
			{
				//Show UI buttons
				goBtns.SetActive(true);

			}, this);
		}

		public void OnHome_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			SoundManager.Instance.StopSoundSFX(SoundFXIndex.GameOver);
			
			UIManager.Instance.HideUI(this);
			UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
			//Show inter ads
			
			AdsManager.Instance.ShowInterstitialAds(() =>
			{
				Debug.Log("Show Inter UI Lose");
			});
		}

		public void OnRestart_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			SoundManager.Instance.StopSoundSFX(SoundFXIndex.GameOver);
			
			UIManager.Instance.HideUI(this);
			//Game manager restart function
			GameManager.Instance.RestartGame();
		}
	}
}