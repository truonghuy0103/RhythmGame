using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hellmade.Sound;
using UnityEngine;

namespace Hiep_Core
{
    public class Hiep_SoundManager : SingletonMono<Hiep_SoundManager>
    {

        public bool isMute = false;
        [SerializeField]
        private List<Hiep_SoundItem> soundFxItems = new List<Hiep_SoundItem>();

        private Dictionary<SoundFXIndex, Hiep_SoundItem> dicSoundFxs = new Dictionary<SoundFXIndex, Hiep_SoundItem>();
        
        private List<AudioClip> lsBGMs = new List<AudioClip>();
        private AudioSource bgmSource;

        private void Awake()
        {
            dicSoundFxs.Clear();
            for (int i = 0; i < soundFxItems.Count; i++)
            {
                Debug.Log("dict: " + soundFxItems[i].soundFxIndex);
                dicSoundFxs.Add(soundFxItems[i].soundFxIndex, soundFxItems[i]);
            }

            bgmSource = GetComponent<AudioSource>();
        }

        #region BGM

        public void AddSoundBGM(AudioClip bgmClip)
        {
            if (isMute)
                return;
            lsBGMs.Clear();
            lsBGMs.Add(bgmClip);
            
            // Get length clip
        }

        public float GetLengthBGM()
        {
            if (lsBGMs.Count > 0)
            {
                return lsBGMs[0].length;
            }

            return 0;
        }

        public void PlaySoundBGM(float volume = 1, bool isLoop = false)
        {
            bgmSource.clip = lsBGMs[0];
            bgmSource.Play();
            bgmSource.volume = 0;
            bgmSource.DOFade(volume, 0.25f);
        }

        public void PauseSoundBGM()
        {
            bgmSource.Pause();
        }

        public void ResumeSoundBGM()
        {
            bgmSource.Play();
        }

        public void StopSoundBGM()
        {
            bgmSource.Stop();
        }

        public float GetCurrentTimeSoundBGM()
        {
            if (bgmSource.clip != null)
            {
                return bgmSource.time;
            }

            return 0;
        }

        #endregion

        #region SFX

        public void PlaySoundFX(SoundFXIndex soundIndex, bool isLoop = false)
        {
            if (isMute)
                return;

            EazySoundManager.PlaySound(dicSoundFxs[soundIndex].soundFxClip, isLoop);
        }

        public void StopSoundFX(SoundFXIndex soundIndex)
        {
            Audio audio = EazySoundManager.GetAudio(dicSoundFxs[soundIndex].soundFxClip);
            if (audio != null)
            {
                audio.Stop();
            }
        }

        public void StopAllSoundFX()
        {
            EazySoundManager.StopAllSounds();
        }

        public bool CheckSoundFXAvailable(SoundFXIndex soundIndex)
        {
            Audio audio = EazySoundManager.GetAudio(dicSoundFxs[soundIndex].soundFxClip);
            if (audio != null && audio.IsPlaying)
            {
                return true;
            }

            return false;
        }
        #endregion

        public void Mute()
        {
            isMute = true;
            StopSoundBGM();
            // Stop all FX
            StopAllSoundFX();
        }

        public void UnMute()
        {
            for (SoundFXIndex i = SoundFXIndex.Click; i < SoundFXIndex.COUNT; i++)
            {
                StopSoundFX(i);
            }

            isMute = false;
        }
    }

    public enum SoundFXIndex
    {
        Click = 0,
        One,
        Two,
        Three,
        Go,
        SoundMenu,
        GameOver,
        MissNote,
        ConfirmMenu,
        Victory,
        COUNT
    }
    [Serializable]
    public class Hiep_SoundItem
    {
        public SoundFXIndex soundFxIndex;
        public AudioClip soundFxClip;
    }
}

