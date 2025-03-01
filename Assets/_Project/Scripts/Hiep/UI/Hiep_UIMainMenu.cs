using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiep;
using Hiep_Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Hiep
{
	public class Hiep_UIMainMenu : BaseUI
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
            gameSave = Hiep_GameManager.Instance.GameSave;
            
            // Check save Setting Vibrate
            bool VirbateOn = gameSave.VirbrateOn;
            if (VirbateOn)
            {
	            imgVibrate.sprite = spriteVibrateOn;
            }
            else
            {
	            imgVibrate.sprite = spriteVibrateOff;
            }
            imgVibrate.SetNativeSize();
            
            // if (!Hiep_SoundManager.Instance.CheckSoundFXAvailable(SoundFXIndex.SoundMenu))
            // {
	           //  Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.SoundMenu);
            // }
            // Select first mode
            
            Timer.DelayedCall(0.5f, () =>
            {
	            imgBoy.SetNativeSize();
	            imgGirl.SetNativeSize();
	            // Get coin from Save
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
		            int randMode = Random.Range(1, Hiep_ConfigGameplay.GetModeLength());
		            int randWeek = Random.Range(0, Hiep_ConfigGameplay.GetWeekLength(randMode));
		            int randSong = Random.Range(0, Hiep_ConfigGameplay.GetSongLength(randMode, randWeek));
		            HotNewSong hotNewSong = new HotNewSong
			            { IndexMode = randMode, IndexWeek = randWeek, IndexSong = randSong };
		            if (!CheckAvailableSong(hotNewSong))
		            {
			            gameSave.HotNewSongs.Add(hotNewSong);
			            countSong++;
		            }
	            }
            }
            
            AdsManager.Instance.ShowBanner();
         }

         public void OnOption_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         UIManager.Instance.ShowUI(UIIndex.UIOption);
         }

         public void OnRate_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         UIManager.Instance.ShowUI(UIIndex.UIRate);
         }

         public void OnSkin_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         UIManager.Instance.ShowUI(UIIndex.UISkin);
         }

         public void OnRewardLogin_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         UIManager.Instance.ShowUI(UIIndex.UIRewardLogin);
         }

         public void OnLuckyDraw_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         UIManager.Instance.ShowUI(UIIndex.UILuckyDraw);
         }

         public void OnVibrate_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         // Save vibrate setting
	         gameSave.VirbrateOn = !gameSave.VirbrateOn;
	         bool vibrateOn = gameSave.VirbrateOn;
	         if (vibrateOn)
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
	         // Set coin from save to TxtCoin
	         int coin = gameSave.Coin;
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
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         for (int i = 0; i < lsSongItems.Count; i++)
	         {
		         lsSongItems[i].gameObject.SetActive(i < NumberSongNewHot);
	         }

	         for (int i = 0; i < gameSave.HotNewSongs.Count; i++)
	         {
		         HotNewSong hotNewSong = gameSave.HotNewSongs[i];
		         
		         SongSave songSave = gameSave.ModeSaves[hotNewSong.IndexMode].WeekSaves[hotNewSong.IndexWeek]
			         .SongSaves[hotNewSong.IndexSong];
		         
		         Hiep_GamePlaySongData gamePlaySongData = Hiep_ConfigGameplay.ConfigSongData
			         (hotNewSong.IndexMode, hotNewSong.IndexWeek, hotNewSong.IndexSong);
		         
		         lsSongItems[i].OnSetupSongItem(this, hotNewSong.IndexMode, hotNewSong.IndexWeek,
			         hotNewSong.IndexSong,gamePlaySongData, songSave.Score, songSave.IsBought);
	         }
         }

         public void OnSelectMode(int index)
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         int countSong = Hiep_ConfigGameplay.GetAllSongInMode(index);
	         for (int i = 0; i < lsSongItems.Count; i++)
	         {
		         lsSongItems[i].gameObject.SetActive(i < countSong);
	         }

	         int count = 0;
	         for (int i = 0; i < Hiep_ConfigGameplay.GetWeekLength(index); i++)
	         {
		         for (int j = 0; j < Hiep_ConfigGameplay.GetSongLength(index,i); j++)
		         {
			         SongSave songSave = gameSave.ModeSaves[index].WeekSaves[i].SongSaves[j];
			         Hiep_GamePlaySongData gamePlaySongData = Hiep_ConfigGameplay.
				         ConfigSongData(index, i, j);
			         
			         lsSongItems[count].OnSetupSongItem(this, index, i, j, 
				         gamePlaySongData, songSave.Score, songSave.IsBought);
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
	}
}