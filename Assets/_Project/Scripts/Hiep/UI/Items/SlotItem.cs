using System;
using System.Collections;
using System.Collections.Generic;
using Hiep;
using Hiep_Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCoin;

    [SerializeField] private GameObject goFade;
    
    private int valueCoin;
    private int indexLogin;

    private Button btnClick;

    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;
    
    // Start is called before the first frame update
    void Awake()
    {
        btnClick = GetComponent<Button>();
        btnClick.onClick.AddListener(() =>
        {
            OnLogin_Clicked();
        });
    }

    public void OnSetup(int index, int coin, bool getReward, bool isToday)
    {
        valueCoin = coin;
        indexLogin = index;
        txtCoin.text = valueCoin.ToString();
        GetComponent<Image>().sprite = spriteOff;

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
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLogin_Clicked()
    {
        Hiep_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
        DisableSlot();
        // Show UI Reward
        UIManager.Instance.ShowUI(UIIndex.UIReward, new RewardParam(){valueCoin = valueCoin});
        indexLogin++;
        if (indexLogin >= 7)
        {
            indexLogin = 0;
        }

        Hiep_GameManager.Instance.GameSave.CurrentDayLogin = indexLogin;

    }

    private void DisableSlot()
    {
        GetComponent<Image>().sprite = spriteOff;
        goFade.SetActive(true);
        btnClick.interactable = false;
    }
}
