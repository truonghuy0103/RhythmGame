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
	public class SlotItem : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI txtCoin;
		[SerializeField] private GameObject goFade;

		private int valueCoin;
		private int indexLogin;

		private Button btnClick;

		[SerializeField] private Sprite spriteOn;
		[SerializeField] private Sprite spriteOff;

		void Awake()
		{
			btnClick = GetComponent<Button>();
			btnClick.onClick.AddListener(OnLogin_Clicked);
		}

		public void OnSetup(int index, int coin, bool getReward, bool isToday)
		{
			valueCoin = coin;
			indexLogin = index;
			txtCoin.text = valueCoin.ToString();
			GetComponent<Image>().sprite = spriteOn;

			if (!getReward)
			{
				DisableSlot();
			}
			else
			{
				btnClick.enabled = isToday;
				if (isToday)
				{
					GetComponent<Image>().sprite = spriteOn;
				}
			}
		}

		private void OnLogin_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			DisableSlot();
			//Show UI Reward
			indexLogin++;
			if (indexLogin >= 7)
			{
				indexLogin = 0;
			}
			
			Huy_GameManager.Instance.GameSave.CurrentDayLogin = indexLogin;
		}

		private void DisableSlot()
		{
			GetComponent<Image>().sprite = spriteOff;
			goFade.SetActive(true);
			btnClick.interactable = false;
		}
	}
}