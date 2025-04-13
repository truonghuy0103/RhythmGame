using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Huy_Core;
using UnityEngine;
using Huy;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Huy
{
	public class UIMainMenu : BaseUI
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
		private GameSave gameSave;
		
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
			gameSave = GameManager.Instance.GameSave;
			
			//Check save Setting Vibrate
			bool VibrateOn = gameSave.VibrateOn;
			if (VibrateOn)
			{
				imgVibrate.sprite = spriteVibrateOn;
			}
			else
			{
				imgVibrate.sprite = spriteVibrateOff;
			}
			imgVibrate.SetNativeSize();
			
			/*if (!SoundManager.Instance.CheckSoundFXAvailable(SoundFXIndex.SoundMenu))
			{
				SoundManager.Instance.PlaySoundSFX(SoundFXIndex.SoundMenu);
			}*/
			//Select first mode

			Timer.DelayedCall(0.5f, () =>
			{
				imgBoy.SetNativeSize();
				imgGirl.SetNativeSize();
				//Get coin from Save
				UpdateTextCoin();
			}, this);
			
			OnSelectMode(0);

			if (gameSave.CurrentDay != DateTime.Now.DayOfYear)
			{
				gameSave.CurrentDay = DateTime.Now.DayOfYear;
				gameSave.HotNewSongs.Clear();
				int countSong = 0;
				while (countSong < NumberSongNewHot)
				{
					int randMode = Random.Range(1, ConfigGameplay.GetModeLength());
					int randWeek = Random.Range(0, ConfigGameplay.GetWeekLength(randMode));
					int randSong = Random.Range(0, ConfigGameplay.GetSongLength(randMode, randWeek));
					HotNewSong hotNewSong = new HotNewSong()
					{
						IndexMode = randMode,
						IndexWeek = randWeek,
						IndexSong = randSong
					};
					if (!CheckAvailableSong(hotNewSong))
					{
						gameSave.HotNewSongs.Add(hotNewSong);
						countSong++;
					}
				}
			}
			
			ChangeBoySkin();
			ChangeGirlSkin();
			
			AdsManager.Instance.ShowBanner();
		}

		public void OnOption_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UIOption);
		}

		public void OnRate_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UIRate);
		}
		
		public void OnSkin_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UISkin);
		}
		
		public void OnRewardLogin_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UIRewardLogin);
		}
		
		public void OnLuckyDraw_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			UIManager.Instance.ShowUI(UIIndex.UILuckyDraw);
		}
		
		public void OnVibrate_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			//Save vibrate setting
			gameSave.VibrateOn = !gameSave.VibrateOn;
			bool VibrateOn = gameSave.VibrateOn;
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
			int coin = GameManager.Instance.GameSave.Coin;
			txtCoin.text = coin.ToString();
		}

		public void OnSaveSongData(int indexMode, int indexWeek, int indexSong)
		{
			gameSave.ModeSaves[indexMode].WeekSaves[indexWeek].SongSaves[indexSong].IsBought = true;
			UpdateTextCoin();
		}

		private bool CheckAvailableSong(HotNewSong hotNewSong)
		{
			if (gameSave.HotNewSongs.Contains(hotNewSong))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public void OnNewHot_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			for (int i = 0; i < lsSongItems.Count; i++)
			{
				lsSongItems[i].gameObject.SetActive(i < NumberSongNewHot);
			}

			for (int i = 0; i < gameSave.HotNewSongs.Count; i++)
			{
				HotNewSong hotNewSong = gameSave.HotNewSongs[i];
				SongSave songSave = gameSave.ModeSaves[hotNewSong.IndexMode].WeekSaves[hotNewSong.IndexWeek]
					.SongSaves[hotNewSong.IndexSong];
				GameplaySongData gameplaySongData =
					ConfigGameplay.ConfigSongData(hotNewSong.IndexMode, hotNewSong.IndexWeek, hotNewSong.IndexSong);
				lsSongItems[i].OnSetupSongItem(this, hotNewSong.IndexMode, hotNewSong.IndexWeek, hotNewSong.IndexSong,
					gameplaySongData, songSave.Score, songSave.IsBought);
			}
		}

		public void OnSelectMode(int index)
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			int countSong = ConfigGameplay.GetAllSongInMode(index);
			for (int i = 0; i < lsSongItems.Count; i++)
			{
				lsSongItems[i].gameObject.SetActive(i < countSong);
			}

			int count = 0;
			for (int i = 0; i < ConfigGameplay.GetWeekLength(index); i++)
			{
				for (int j = 0; j < ConfigGameplay.GetSongLength(index,i); j++)
				{
					SongSave songSave = gameSave.ModeSaves[index].WeekSaves[i].SongSaves[j];
					GameplaySongData gameplaySongData = ConfigGameplay.ConfigSongData(index, i, j);

					lsSongItems[count].OnSetupSongItem(this, index, i, j, gameplaySongData, songSave.Score,
						songSave.IsBought);
					count++;
				}
			}
			
			ChangeUISelectModeBtn(index);
		}

		public void ChangeUISelectModeBtn(int indexMode)
		{
			if (goCurSelectMode != null)
			{
				goCurSelectMode.SetActive(false);
			}
			
			goCurSelectMode = lsGoSelectModes[indexMode];
			goCurSelectMode.SetActive(true);
		}

		public void ChangeBoySkin()
		{
			int curIndexSkin = gameSave.CurrentIndexBoy;
			boyAnimator.SetFloat("Index", curIndexSkin);
		}

		public void ChangeGirlSkin()
		{
			int curIndexSkin = gameSave.CurrentIndexGirl;
			girlAnimator.SetFloat("Index", curIndexSkin);
		}
	}
}