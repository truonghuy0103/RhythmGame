using System;
using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
using Unity.VisualScripting;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Huy
{
	public class SkinItem : MonoBehaviour
	{
		[SerializeField] private GameObject goPrice;
		[SerializeField] private Image imgSkin;

		private UISkin parent;
		
		//Config Skin data
		private ConfigSkinData configSkinData;
		
		private bool isBought;
		
		private bool isSkinGirl;

		[SerializeField] private Sprite spriteDeselect;
		[SerializeField] private Sprite spriteSelect;
		
		[SerializeField] private Sprite spriteGirlDeselect;

		private const int valueBoughtSkin = 1000;

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() => OnSkin_Clicked());
			
			goPrice.GetComponent<Button>().onClick.AddListener(() => OnBuy_Clicked());
		}

		public void OnSkin_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			if (!goPrice.activeSelf)
			{
				GetComponent<Image>().sprite = spriteSelect;
				if (isSkinGirl)
				{
					//Change skin for girl
					parent.OnChangeSkinGirl(configSkinData.spriteSkin,this);
				}
				else
				{
					//Change skin for boy
					parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);
				}
			}
		}

		public void OnSkin_Clicked(bool isGirl)
		{
			GetComponent<Image>().sprite = spriteSelect;
			if (isGirl)
			{
				//Change skin for girl
				parent.OnChangeSkinGirl(configSkinData.spriteSkin,this);
			}
			else
			{
				//Change skin for boy
				parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);
			}
		}

		public void OnDeselect()
		{
			GetComponent<Image>().sprite = spriteDeselect;
		}

		public void OnSetup(UISkin parent, ConfigSkinData configSkinData, bool isBought, bool isSkinGirl = true)
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

		public void OnBuy_Clicked()
		{
			SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
			if (GameManager.Instance.GameSave.Coin >= valueBoughtSkin && !isBought)
			{
				isBought = true;
				GameManager.Instance.GameSave.Coin -= valueBoughtSkin;
				goPrice.SetActive(false);
				if (isSkinGirl)
				{
					int idSkinGirl = configSkinData.id;
					//id get form config
					GameManager.Instance.GameSave.GirlSkinBoughts.Add(idSkinGirl);
				}
				else
				{
					int idSkinBoy = configSkinData.id;
					//id get form config
					GameManager.Instance.GameSave.BoySkinBoughts.Add(idSkinBoy);
				}
				//Update text coin
				parent.UpdateTextCoin();
				//Set sprite for imgSkin
				imgSkin.sprite = configSkinData.spriteSkin;
			}
			OnSkin_Clicked();
		}
	}
}