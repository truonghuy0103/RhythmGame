using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiep;
using Hiep_Core;
using TMPro;
using UnityEngine.UI;

namespace Hiep
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

        private Hiep_UIMainMenu parent;

        private void Awake()
        {
            imgFrame = GetComponent<Image>();
        }

        public void OnSetupSongItem(Hiep_UIMainMenu parent, int indexMode, int indexWeek, int indexSong,
            Hiep_GamePlaySongData gamePlaySongData, int score, bool isBoght)
        {
            int indexFrame = indexSong % lsSpriteFrames.Count;
            imgFrame.sprite = lsSpriteFrames[indexFrame];

            this.parent = parent;
            this.indexMode = indexMode;
            this.indexWeek = indexWeek;
            this.indexSong = indexSong;
            this.price = gamePlaySongData.price;

            txtNameSong.text = gamePlaySongData.nameSong;
            txtScore.text = score.ToString();

            indexDifficult = 0;
            imgDifficult.sprite = lsSpriteDifficults[indexDifficult];
            imgDifficult.SetNativeSize();

            imgIcon.sprite = gamePlaySongData.spriteCharacter;
            imgIcon.SetNativeSize();
            imgIcon.transform.localScale = new Vector3(0.6f, 0.6f, 1);

            if (isBoght)
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
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
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
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            if (Hiep_GameManager.Instance.GameSave.Coin >= price)
            {
                Hiep_GameManager.Instance.GameSave.Coin -= price;
                SaveManager.Instance.SaveGame();

                goBuySong.SetActive(false);
                goPlay.SetActive(true);

                // Save Song data item to UI Main menu
                parent.OnSaveSongData(indexMode, indexWeek, indexSong);
            }
        }

        public void OnPlay_Clicked()
        {
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            if (Hiep_GameManager.Instance.GameSave.isFirstOpen)
            {
                // Show inter ads
            }

            UIManager.Instance.HideUI(parent);
            Hiep_GameManager.Instance.SetupGameplay(indexMode, indexWeek, indexSong, (Difficult)indexDifficult);
        }

        public void OnRewardAds_Clicked()
        {
            Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            // Show reward ads
            Timer.DelayedCall(0.5f, () =>
            {
                UIManager.Instance.HideUI(parent);
                Hiep_GameManager.Instance.SetupGameplay(indexMode, indexWeek, indexSong,
                    (Difficult)indexDifficult);
            }, this);
        }
    }
}