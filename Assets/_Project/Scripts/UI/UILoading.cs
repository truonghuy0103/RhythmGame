using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace Huy
{
	public class UILoading : BaseUI
	{
		[SerializeField] private TextMeshProUGUI txtLoading;
		[SerializeField] private Image imgLoading;
		[SerializeField] private float timerLoading = 3;

		private float timer;
		private float valueSlider;
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
	         SoundManager.Instance.PlaySoundSFX(SoundFXIndex.SoundMenu,true);
	         valueSlider = 0;
	         imgLoading.fillAmount = 0;
	         DOTween.To(()=>imgLoading.fillAmount,x=>imgLoading.fillAmount=x,1,timerLoading).OnComplete(() =>
	         {
		         UIManager.Instance.HideUI(this);
		         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
	         });

	         StartCoroutine(ShowLoading());
            base.OnSetup(param);
         }

         IEnumerator ShowLoading()
         {
	         while (true)
	         {
		         txtLoading.text = "Loading.";
		         for (int i = 0; i < 3; i++)
		         {
			         yield return new WaitForSeconds(1f);
			         txtLoading.text = txtLoading.text + ".";
		         }
	         }
         }
	}
}