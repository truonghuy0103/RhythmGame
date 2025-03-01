using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiep;
using Hiep_Core;

namespace Hiep
{
	public class Hiep_UIPause : BaseUI
	{
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            Time.timeScale = 0;
         }

         public void OnResume_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         // Resume game
	         Hiep_GameManager.Instance.ResumeGame();
	         // Show inter ads
	         UIManager.Instance.HideUI(this);
         }

         public void OnHome_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         // End game
	         Hiep_GameManager.Instance.GoToHome();
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
         }

         public void OnRestart_Clicked()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.HideUI(UIIndex.UIGameplay);
	         // Show inter ads
	         // Restart game
	         Hiep_GameManager.Instance.RestartGame();
         }
	}
}