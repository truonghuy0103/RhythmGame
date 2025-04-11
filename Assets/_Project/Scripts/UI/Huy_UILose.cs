using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
namespace Huy
{
	public class Huy_UILose : BaseUI
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
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.GameOver);
			Timer.DelayedCall(2, () =>
			{
				//Show UI buttons
				goBtns.SetActive(true);

			}, this);
			
			AdsManager.Instance.ShowInterstitialAds(() =>
			{
				Debug.Log("Show Inter UI Lose");
			});
		}

		public void OnHome_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			Huy_SoundManager.Instance.StopSoundSFX(SoundFXIndex.GameOver);
			
			UIManager.Instance.HideUI(this);
			UIManager.Instance.ShowUI(UIIndex.UIGameplay);
			//Show inter ads
		}

		public void OnRestart_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			Huy_SoundManager.Instance.StopSoundSFX(SoundFXIndex.GameOver);
			
			UIManager.Instance.HideUI(this);
			//Game manager restart function
			Huy_GameManager.Instance.RestartGame();
		}
	}
}