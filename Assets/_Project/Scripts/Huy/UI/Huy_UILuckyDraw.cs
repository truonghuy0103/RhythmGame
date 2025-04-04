using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Huy_Core;
using UnityEngine;
using Huy;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Huy
{
	public class Huy_UILuckyDraw : BaseUI
	{
		[SerializeField] private Image imgDraw;
		[SerializeField] private TextMeshProUGUI txtCountdown;
		[SerializeField] private List<TextMeshProUGUI> lsTextCoins = new List<TextMeshProUGUI>();

		[SerializeField] private Button btnFree;

		private double timerCountdown;
		private const double ValueTimerCountdown = 7199;
		private bool isShowCountdown = false;
		private bool isAds;
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            timerCountdown = Huy_GameManager.Instance.GameSave.CountdownLuckyDraw -
                              TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;

            if (timerCountdown > 0)
            {
	            isShowCountdown = true;
            }
            else
            {
	            isShowCountdown = false;
	            txtCountdown.text = "FREE";
            }
            
            btnFree.interactable = !isShowCountdown;
            
            imgDraw.transform.eulerAngles = new Vector3(0, 0, -30);
         }

         private void Spin()
         {
	         float randtimer = Random.Range(3.5f, 5f);
	         Transform transWheelCircle = imgDraw.transform;
	         transWheelCircle.eulerAngles = new Vector3(0, 0, -30);

	         float pieceAngle = 360 / lsTextCoins.Count;
	         float halfPieceAngle = pieceAngle / 2;
	         float halfPieceAngleWithPadding = halfPieceAngle - (halfPieceAngle / 4f);
	         
	         int randIndex = Random.Range(0, lsTextCoins.Count);
	         float angle = -(pieceAngle * randIndex);

	         /*float rightOffset = (angle - halfPieceAngleWithPadding) % 360;
	         float leftOffset = (angle + halfPieceAngleWithPadding) % 360;

	         float randomAngle = Random.Range(leftOffset, rightOffset);*/
	         float randomAngle = randIndex * 60 + 30;

	         Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * 5);

	         float prevAngle, curAngle;
	         prevAngle = curAngle = transWheelCircle.eulerAngles.z;

	         transWheelCircle.DORotate(targetRotation, randtimer, RotateMode.Fast).SetEase(Ease.InOutQuart).OnUpdate(
		         () =>
		         {
			         float diff = Mathf.Abs(prevAngle - curAngle);
			         if (diff >= halfPieceAngle)
			         {
				         prevAngle = curAngle;
			         }
			         
			         curAngle = transWheelCircle.eulerAngles.z;
		         }).OnComplete(
		         () =>
		         {
			         Timer.DelayedCall(1, () =>
			         {
				         int indexLuckyDraw = Mathf.Abs(Mathf.RoundToInt(transWheelCircle.eulerAngles.z / 60));
				         //Get config depend Index to coin
				         //Show reward

				         if (!isAds)
				         {
					         timerCountdown = ValueTimerCountdown;
					         isShowCountdown = true;
					         //Show timer
					         ShowTimer();
				         }
			         },this);
		         });
         }

         private void ShowTimer()
         {
	         int second = (int)(timerCountdown % 60);
	         int minutes = (int)(timerCountdown / 60) % 60;
	         int hours = (int)((timerCountdown / 60) / 60) % 60;
	         txtCountdown.text = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, second);
         }

         private void Update()
         {
	         if (isShowCountdown)
	         {
		         timerCountdown -= Time.deltaTime;
		         ShowTimer();
		         if (timerCountdown <= 0)
		         {
			         timerCountdown = 0;
			         isShowCountdown = false;
			         txtCountdown.text = "FREE";
			         btnFree.interactable = true;
		         }
	         }
         }

         public void OnFree_Clicked()
         {
	         isAds = false;
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         Huy_GameManager.Instance.GameSave.CountdownLuckyDraw =
		         TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds + ValueTimerCountdown;
	         btnFree.interactable = false;
	         Spin();
         }

         public void OnAds_Clicked()
         {
	         isAds = true;
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         //Show Reward Ads
	         Spin();
         }

         public override void OnCloseClick()
         {
	         Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
	         base.OnCloseClick();
	         //Show Inter ads
	         if (UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu) != null)
	         {
		         Huy_UIMainMenu uiMainMenu = (Huy_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
	         }
         }
	}
}