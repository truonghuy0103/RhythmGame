using System.Collections;
using System.Collections.Generic;
using Hiep;
using Hiep_Core;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    [SerializeField] private GameObject goPrice;

    [SerializeField] private Image imgSkin;

    private Hiep_UISkin parent;
    
    // Config Skin Data
    private ConfigSkinData configSkinData;
    
    private bool isBought;

    private bool isSkinGirl;

    [SerializeField] private Sprite spriteDeselect;
    [SerializeField] private Sprite spriteSelect;

    [SerializeField] private Sprite spriteGirlDeselect;

    private const int valueBoughtSkin = 1000;
    
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnSkin_Clicked());
        
        goPrice.GetComponent<Button>().onClick.AddListener(() => OnBuy_Clicked());
        
    }

    public void OnSkin_Clicked()
    {
        Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
        if (!goPrice.activeSelf)
        {
            GetComponent<Image>().sprite = spriteSelect;
            if (isSkinGirl)
            {
                // Change skin for girl
                parent.OnChangeSkinGirl(configSkinData.spriteSkin, this);
            }
            else
            {
                // Change skin for boy
                parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);
            }
        }
    }

    public void OnSkin_Clicked(bool isGirl)
    {
        GetComponent<Image>().sprite = spriteSelect;
        if (isGirl)
        {
            // Change skin for girl
            parent.OnChangeSkinGirl(configSkinData.spriteSkin, this);
        }
        else
        {
            // Change skin for boy
            parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);
        }
    }

    public void OnDeselect()
    {
        GetComponent<Image>().sprite = spriteDeselect;
    }

    public void OnSetup(Hiep_UISkin parent, ConfigSkinData configSkinData, bool isBought, bool isSkinGirl = true)
    {
        this.parent = parent;
        this.isBought = isBought;
        this.isSkinGirl = isSkinGirl;
        
        this.configSkinData = configSkinData;
        GetComponent<Image>().sprite = spriteDeselect;
        
        if (configSkinData.coin == 0 || isBought)
        {
            goPrice.SetActive(false);
            // Set sprite skin
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
        Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
        if (Hiep_GameManager.Instance.GameSave.Coin >= valueBoughtSkin && !isBought)
        {
            isBought = true;
            Hiep_GameManager.Instance.GameSave.Coin -= valueBoughtSkin;
            goPrice.SetActive(false);
            if (isSkinGirl)
            {
                // id get form config
                int idSkinGirl = configSkinData.id;
                Hiep_GameManager.Instance.GameSave.GirlSkinBoughts.Add(idSkinGirl);
            }
            else
            {
                // id get from config
                int idSkinboy = configSkinData.id;
                
                Hiep_GameManager.Instance.GameSave.BoySkinBoughts.Add(idSkinboy);
            }
            // Update text coin
            parent.UpdateTextCoin();
            // Set sprite for imgSkin
            imgSkin.sprite = configSkinData.spriteSkin;
        }
        OnSkin_Clicked();
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
