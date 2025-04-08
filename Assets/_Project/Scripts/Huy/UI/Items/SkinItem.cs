using System;
using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace Huy
{
	public class SkinItem : MonoBehaviour
	{
		[SerializeField] private GameObject goPrice;
		[SerializeField] private Image imgSkin;

		private Huy_UISkin parent;
		
		//Config Skin data
		private Huy_ConfigSkinData configSkinData;
		
		private bool isBought;
		
		private bool isSkinGirl;

		[SerializeField] private Sprite spriteDeselect;
		[SerializeField] private Sprite spriteSelect;
		
		[SerializeField] private Sprite spriteGirlDeselect;

		private const int valueBoughtSkin = 1000;

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() => OnSkin_Clicked());
			
			goPrice = transform.Find("imgFrame").gameObject;
			goPrice.GetComponent<Button>().onClick.AddListener(() => OnBuy_clicked());
			
			imgSkin = transform.Find("imgGirl").GetComponent<Image>();
		}

		public void OnSkin_Clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			if (!goPrice.activeSelf)
			{
				GetComponent<Image>().sprite = spriteSelect;
				if (isSkinGirl)
				{
					//Change skin for girl
				}
				else
				{
					//Change skin for boy
				}
			}
		}

		public void OnSkin_Clicked(bool isGirl)
		{
			GetComponent<Image>().sprite = spriteSelect;
			if (isGirl)
			{
				//Change skin for girl
			}
			else
			{
				//Change skin for boy
			}
		}

		public void OnDeselect()
		{
			GetComponent<Image>().sprite = spriteDeselect;
		}

		public void OnSetup(Huy_UISkin parent, Huy_ConfigSkinData configSkinData, bool isBought, bool isSkinGirl = true)
		{
			this.parent = parent;
			this.isBought = isBought;
			this.isSkinGirl = isSkinGirl;
			this.configSkinData = configSkinData;
			GetComponent<Image>().sprite = spriteDeselect;
			if (configSkinData.coin == 0 || isBought)
			{
				goPrice.SetActive(false);
				//Set sprite skin
				imgSkin.sprite = configSkinData.spriteSkin;
			}
			else
			{
				imgSkin.sprite = spriteGirlDeselect;
				goPrice.SetActive(true);
			}
		}

		public void OnBuy_clicked()
		{
			Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			if (Huy_GameManager.Instance.GameSave.Coin >= valueBoughtSkin && !isBought)
			{
				isBought = true;
				Huy_GameManager.Instance.GameSave.Coin -= valueBoughtSkin;
				goPrice.SetActive(false);
				if (isSkinGirl)
				{
					int idSkinGirl = configSkinData.id;
					//id get form config
					Huy_GameManager.Instance.GameSave.GirlSkinBoughts.Add(idSkinGirl);
				}
				else
				{
					int idSkinBoy = configSkinData.id;
					//id get form config
					Huy_GameManager.Instance.GameSave.BoySkinBoughts.Add(idSkinBoy);
				}
				//Update text coin
				//Set sprite for imgSkin
				imgSkin.sprite = configSkinData.spriteSkin;
			}
			OnSkin_Clicked();
		}
	}
}