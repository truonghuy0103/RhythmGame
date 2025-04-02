using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Huy_Core;
using UnityEngine;
using Huy;
using TMPro;
using UnityEngine.UI;

namespace Huy
{
	public class Huy_UIMainMenu : BaseUI
	{
		[SerializeField] private List<SongItem> lsSongItems = new List<SongItem>();
		[SerializeField] private TextMeshProUGUI txtCoin;

		[SerializeField] private Image imgBoy;
		[SerializeField] private Image imgGirl;
		
		[SerializeField] private Image imgVibrate;
		[SerializeField] private Sprite spriteVibrateOn;
		[SerializeField] private Sprite spriteVibrateOff;
		
		[SerializeField] private Animator boyAnimator;
		[SerializeField] private Animator girlAnimator;
		
		[SerializeField] private List<GameObject> lsGoSelectModes = new List<GameObject>();
		private GameObject goCurSelectMode;

		private const int NumberSongNewHot = 20;
		
		public override void OnInit()
		{
			base.OnInit();

			for (int i = 0; i < lsGoSelectModes.Count; i++)
			{
				lsGoSelectModes[i].SetActive(false);
			}
		}

		public override void OnSetup(UIParam param = null)
		{
			base.OnSetup(param);
			//Check save Setting Vibrate
			bool VibrateOn = true;
			if (VibrateOn)
			{
				imgVibrate.sprite = spriteVibrateOn;
			}
			else
			{
				imgVibrate.sprite = spriteVibrateOff;
			}
			imgVibrate.SetNativeSize();
			
			if (!Huy_SoundManager.Instance.CheckSoundFXAvailable(SoundFXIndex.SoundMenu))
			{
				Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.SoundMenu);
			}
			//Select first mode

			Timer.DelayedCall(0.5f, () =>
			{
				imgBoy.SetNativeSize();
				imgGirl.SetNativeSize();
				//Get coin from Save
			}, this);
		}

		public void OnOption_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UIOption);
		}

		public void OnRate_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UIRate);
		}
		
		public void OnSkin_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UISkin);
		}
		
		public void OnRewardLogin_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UIRewardLogin);
		}
		
		public void OnLuckyDraw_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UILuckyDraw);
		}
		
		public void OnVibrate_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			//Save vibrate setting
			bool VibrateOn = true;
			if (VibrateOn)
			{
				imgVibrate.sprite = spriteVibrateOn;
			}
			else
			{
				imgVibrate.sprite = spriteVibrateOff;
			}
			imgVibrate.SetNativeSize();
		}

		public void UpdateTextCoin()
		{
			//Set coin from save to txtCoin
			int coin = Huy_GameManager.Instance.GameSave.Coin;
			txtCoin.text = coin.ToString();
		}
	}
}