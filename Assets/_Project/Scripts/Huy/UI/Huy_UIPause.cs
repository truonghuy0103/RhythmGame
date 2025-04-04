using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
namespace Huy
{
	public class Huy_UIPause : BaseUI
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
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         //Resume game
	         Huy_GameManager.Instance.ResumeGame();
	         //Show inter ads
	         UIManager.Instance.HideUI(this);
         }

         public void OnHome_Clicked()
         {
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         //End game
	         Huy_GameManager.Instance.GoToHome();
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
         }
         
         public void OnRestart_Clicked()
         {
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         UIManager.Instance.HideUI(this);
	         UIManager.Instance.HideUI(UIIndex.UIGameplay);
	         //Show inter ads
	         //Restart game
	         Huy_GameManager.Instance.RestartGame();
         }
	}
}