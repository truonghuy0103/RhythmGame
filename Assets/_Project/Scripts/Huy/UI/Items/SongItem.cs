using System;
using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
using TMPro;
using UnityEngine.UI;
namespace Huy
{
	public class SongItem : MonoBehaviour
	{
		[SerializeField] private Image imgIcon;
		[SerializeField] private Image imgDifficult;
		private Image imgFrame;
		
		[SerializeField] private List<Sprite> lsSpriteDifficults = new List<Sprite>();
		[SerializeField] private List<Sprite> lsSpriteFrames = new List<Sprite>();

		[SerializeField] private TextMeshProUGUI txtScore;
		[SerializeField] private TextMeshProUGUI txtNameSong;

		[SerializeField] private GameObject goPlay;
		[SerializeField] private GameObject goBuySong;

		private int indexDifficult;
		private int indexSong;
		private int indexWeek;
		private int indexMode;

		private int price;

		private Huy_UIMainMenu parent;

		private void Awake()
		{
			imgFrame = GetComponent<Image>();
		}

		public void OnSetupSongItem(Huy_UIMainMenu parent, int indexMode, int indexWeek, int indexSong,
			Huy_GameplaySongData gameplaySongData,int score, bool isBought)
		{
			int indexFrame = indexSong % lsSpriteFrames.Count;
			imgFrame.sprite = lsSpriteFrames[indexFrame];

			this.parent = parent;
			this.indexMode = indexMode;
			this.indexWeek = indexWeek;
			this.indexSong = indexSong;
			this.price = gameplaySongData.price;

			txtNameSong.text = gameplaySongData.nameSong;
			txtScore.text = score.ToString();
			
			indexDifficult = 0;
			imgDifficult.sprite = lsSpriteDifficults[indexDifficult];
			imgDifficult.SetNativeSize();

			imgIcon.sprite = gameplaySongData.spriteCharacter;
			imgIcon.SetNativeSize();
			imgIcon.transform.localScale = new Vector3(0.6f, 0.6f, 1);

			if (isBought)
			{
				goBuySong.SetActive(false);
				goPlay.SetActive(true);
			}
			else
			{
				goBuySong.SetActive(true);
				goPlay.SetActive(false);
			}
		}

		public void OnDifficult_Clicked(int index)
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			indexDifficult += index;
			if (indexDifficult < 0)
			{
				indexDifficult = lsSpriteDifficults.Count - 1;
			}
			else
			{
				if (indexDifficult >= lsSpriteDifficults.Count)
				{
					indexDifficult = 0;
				}
			}
			
			imgDifficult.sprite = lsSpriteDifficults[indexDifficult];
			imgDifficult.SetNativeSize();
		}

		public void OnBuySongItem_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			if (Huy_GameManager.Instance.GameSave.Coin >= price)
			{
				Huy_GameManager.Instance.GameSave.Coin -= price;
				SaveManager.Instance.SaveGame();
				
				goBuySong.SetActive(false);
				goPlay.SetActive(true);
				
				//Save song data item to UI Main Menu
				parent.OnSaveSongData(indexMode, indexWeek, indexSong);
			}
		}

		public void OnPlay_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			if (Huy_GameManager.Instance.GameSave.isFirstOpen)
			{
				//Show inter ads
			}
			UIManager.Instance.HideUI(parent);
			Huy_GameManager.Instance.SetupGameplay(indexMode, indexWeek, indexSong, (Difficult)indexDifficult);
		}

		public void OnRewardAds_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			//Show reward ads
			if (indexMode == 0)
			{
				Timer.DelayedCall(0.5f, () =>
				{
					UIManager.Instance.HideUI(parent);
					Huy_GameManager.Instance.SetupGameplay(indexMode, indexWeek, indexSong, (Difficult)indexDifficult);
				},this);
			}
		}
	}
}