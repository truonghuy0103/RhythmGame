using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;
using Hiep;
using Hiep_Core;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Hiep
{
	public class Hiep_UILuckyDraw : BaseUI
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
            timerCountdown = (Hiep_GameManager.Instance.GameSave.CountdownLuckyDraw -
                              TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds);

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
            
            // Set text coin from config Lucky Draw
            for (int i = 0; i < lsTextCoins.Count; i++)
            {
	            ConfigLuckyDrawData luckyDrawData = ConfigLuckyDraw.GetConfigLuckyDrawData(i);
	            lsTextCoins[i].text = luckyDrawData.coin.ToString();
            }
            imgDraw.transform.eulerAngles = new Vector3(0, 0, -30);
         }

         private void Spin()
         {
	         float randTimer = Random.Range(3.5f, 5f);
	         Transform transWheelCircle = imgDraw.transform;
	         transWheelCircle.eulerAngles = new Vector3(0, 0, -30);
	         float pieceAngle = 360 / lsTextCoins.Count;
	         float halfPieceAngle = pieceAngle / 2f;
	         float halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);
	         int index = Random.Range(0, lsTextCoins.Count);
	         float angle = -(pieceAngle * index);

	         float randomAngle = index * 60 - 30;
	         Vector3 targetRotation = -Vector3.back * (randomAngle + (10 * 360));
	         //float prevAngle = wheelCircle.eulerAngles.z + halfPieceAngle ;
	         float prevAngle, curAngle;
	         prevAngle = curAngle = transWheelCircle.eulerAngles.z;


	         transWheelCircle
		         .DORotate(targetRotation, randTimer, RotateMode.FastBeyond360)
		         .SetEase(Ease.InOutQuart)
		         .OnUpdate(() =>
		         {
			         float diff = Mathf.Abs(prevAngle - curAngle);
			         if (diff >= halfPieceAngle)
			         {
				         prevAngle = curAngle;
			         }
			         curAngle = transWheelCircle.eulerAngles.z;
		         })
		         .OnComplete(() =>
		         {
					Timer.DelayedCall(1, () =>
					{
						int indexLuckyDraw = Mathf.Abs(Mathf.RoundToInt(transWheelCircle.eulerAngles.z / 60));
						//Debug.Log("index: " + indexLuckyDraw + " " + lsTextCoins[indexLuckyDraw].text);
						// Get config depend Index to coin
						ConfigLuckyDrawData luckyDrawData = ConfigLuckyDraw.GetConfigLuckyDrawData(index);
						// Show Reward 
						UIManager.Instance.ShowUI(UIIndex.UIReward, new RewardParam()
						{
							valueCoin = luckyDrawData.coin
						});
						if (!isAds)
						{
							timerCountdown = ValueTimerCountdown;
							isShowCountdown = true;
							// Show timer
							Showtimer();
						}
					}, this);
		         });
         }

         private void Showtimer()
         {
	         int second = (int)(timerCountdown % 60);
	         int minutes = (int)(timerCountdown / 60) % 60;
	         int hour = (int)((timerCountdown / 60) / 60) % 60;
	         txtCountdown.text = string.Format("{0:0}:{1:00}:{2:00}", hour, minutes, second);
         }

         private void Update()
         {
	         if (isShowCountdown)
	         {
		         timerCountdown -= Time.deltaTime;
		         Showtimer();
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
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         Hiep_GameManager.Instance.GameSave.CountdownLuckyDraw =
		         TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds + ValueTimerCountdown;
	         btnFree.interactable = false;
	         Spin();
         }

         public void OnAds_Clicked()
         {
	         
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         // Show Reward Ads
	         AdsManager.Instance.ShowRewardedAds(() =>
	         {
		         Spin();
		         isAds = true;
	         });
	         
         }

         public override void OnCloseClick()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         base.OnCloseClick();
	         // Show Inter ads
	         if (UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu) != null)
	         {
		         Hiep_UIMainMenu uiMainMenu = (Hiep_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
		         // Update Currency
	         }
         }
	}
}