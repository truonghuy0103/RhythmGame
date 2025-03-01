using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiep;
using Hiep_Core;
using TMPro;
using UnityEngine.UI;

namespace Hiep
{
	public class Hiep_UISkin : BaseUI
	{
		[SerializeField] private Image imgGirl;
		[SerializeField] private Image imgBoy;

		[SerializeField] private List<SkinItem> lsSkinBoyItems = new List<SkinItem>();
		[SerializeField] private List<SkinItem> lsSkinGirlItems = new List<SkinItem>();
		private SkinItem curBoySkinItem;
		private SkinItem curGirlSkinItem;
		
		[SerializeField]
		private GameObject goScrollRectBoy;

		[SerializeField] private GameObject goScrollRectGirl;

		[SerializeField] private Button btnAds;
		[SerializeField] private Button btnBoy;
		[SerializeField] private Button btnGirl;

		[SerializeField] private Sprite spriteBtnBoySelect;
		[SerializeField] private Sprite spriteBtnBoyDeselect;

		[SerializeField] private Sprite spriteBtnGirlSelect;
		[SerializeField] private Sprite spriteBtnGirlDeselect;

		[SerializeField] private TextMeshProUGUI txtCoin;

		[SerializeField] private Animator boyAnimator;
		[SerializeField] private Animator girlAnimator;
		
		
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            for (int i = 0; i < ConfigSkin.GetBoySkinDataLength(); i++)
            {
	            ConfigSkinData configSkinData = ConfigSkin.GetConfigSkinDataBoy(i);
	            bool isBought = Hiep_GameManager.Instance.GameSave.BoySkinBoughts.IndexOf(i) >= 0;
	            lsSkinBoyItems[i].OnSetup(this, configSkinData, isBought, false);
            }
            
            for (int i = 0; i < ConfigSkin.GetGirlSkinDataLength(); i++)
            {
	            ConfigSkinData configSkinData = ConfigSkin.GetConfigSkinDataGirl(i);
	            bool isBought = Hiep_GameManager.Instance.GameSave.GirlSkinBoughts.IndexOf(i) >= 0;
	            lsSkinGirlItems[i].OnSetup(this, configSkinData, isBought, true);
	            Debug.Log("girl: " + i + " " + configSkinData.id);
            }

            txtCoin.text = Hiep_GameManager.Instance.GameSave.Coin.ToString();
            lsSkinBoyItems[Hiep_GameManager.Instance.GameSave.CurrentIndexBoy].OnSkin_Clicked(false);
            lsSkinGirlItems[Hiep_GameManager.Instance.GameSave.CurrentIndexGirl].OnSkin_Clicked();
            // show Boy skin
            OnShowBoySkinClick();
         }

         public void OnChangeSkinGirl(Sprite spriteGirl, SkinItem skinItem)
         {
	         if (curGirlSkinItem != null && curGirlSkinItem != skinItem)
	         {
		         curGirlSkinItem.OnDeselect();
	         }

	         curGirlSkinItem = skinItem;
	         int indexSkin = lsSkinGirlItems.IndexOf(skinItem);
	         girlAnimator.SetFloat("Index", indexSkin);

	         imgGirl.sprite = spriteGirl;
	         imgGirl.SetNativeSize();

	         bool isBought = Hiep_GameManager.Instance.GameSave.GirlSkinBoughts.IndexOf(indexSkin) >= 0;
	         if (isBought)
	         {
		         Hiep_GameManager.Instance.GameSave.CurrentIndexGirl = indexSkin;
	         }
         }

         public void OnChangeSkinBoy(Sprite spriteBoy, SkinItem skinItem)
         {
	         if (curBoySkinItem != null && curBoySkinItem != skinItem)
	         {
		         curBoySkinItem.OnDeselect();
	         }

	         curBoySkinItem = skinItem;
	         int indexSkin = lsSkinBoyItems.IndexOf(skinItem);
	         boyAnimator.SetFloat("Index", indexSkin);

	         imgBoy.sprite = spriteBoy;
	         imgBoy.SetNativeSize();

	         bool isBought = Hiep_GameManager.Instance.GameSave.BoySkinBoughts.IndexOf(indexSkin) >= 0;
	         if (isBought)
	         {
		         Hiep_GameManager.Instance.GameSave.CurrentIndexBoy = indexSkin;
	         }
         }

         public void OnShowBoySkinClick()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);

	         btnBoy.enabled = false;
	         btnBoy.image.sprite = spriteBtnBoySelect;

	         btnGirl.enabled = true;
	         btnGirl.image.sprite = spriteBtnGirlDeselect;
	         
	         goScrollRectBoy.SetActive(true);
	         goScrollRectGirl.SetActive(false);
         }

         public void OnShowGirlSkinClick()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);

	         btnGirl.enabled = false;
	         btnGirl.image.sprite = spriteBtnGirlSelect;

	         btnBoy.enabled = true;
	         btnBoy.image.sprite = spriteBtnBoyDeselect;
	         
	         goScrollRectBoy.SetActive(false);
	         goScrollRectGirl.SetActive(true);
         }

         public void OnAdsClick()
         {
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         // Show Ads
	         AdsManager.Instance.ShowInterstitialAds(() =>
	         {
		         Hiep_GameManager.Instance.GameSave.Coin += 100;
		         txtCoin.text = Hiep_GameManager.Instance.GameSave.Coin.ToString();
	         });
	     
         }

         public void UpdateTextCoin()
         {
	         txtCoin.text = Hiep_GameManager.Instance.GameSave.Coin.ToString();
         }

         public override void OnCloseClick()
         {
	         base.OnCloseClick();
	         UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
	         Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
	         if (Hiep_GameManager.Instance.GameSave.isFirstOpen)
	         {
		         // Show Inter ads
	         }
         }
	}
}