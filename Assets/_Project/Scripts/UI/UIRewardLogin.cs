using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huy;
namespace Huy
{
	public class UIRewardLogin : BaseUI
	{
		[SerializeField] private List<SlotItem> lsSlotItems = new List<SlotItem>();
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            for (int i = 0; i < lsSlotItems.Count; i++)
            {
	            //Get config daily reward
	            ConfigDailyLoginData configDailyLoginData = Huy.ConfigDailyLogin.GetDailyLoginData(i);
	            //Debug.Log("Config: " + configDailyLoginData.coin + " " + configDailyLoginData.id);
	            Debug.Log("login: " + GameManager.Instance.GameSave.CurrentDay + " " 
	                      + GameManager.Instance.GameSave.CurrentDayOfWeekLogin);
	            int currentDayLogin = GameManager.Instance.GameSave.CurrentDayLogin;
	            int currentWeekLogin = GameManager.Instance.GameSave.CurrentDayOfWeekLogin;
	            int coin = configDailyLoginData.coin;

	            if (DateTime.Now.DayOfYear - currentWeekLogin == currentDayLogin)
	            {
		            //Get coin from config
		            lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, i == currentDayLogin);
	            }
	            else
	            {
		            if (DateTime.Now.DayOfYear - currentWeekLogin < currentDayLogin)
		            {
			            lsSlotItems[i].OnSetup(i,coin,i>=currentDayLogin,false);
		            }
		            else
		            {
			            lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, i == currentDayLogin);
		            }
	            }
            }
         }

         public override void OnCloseClick()
         {
	         base.OnCloseClick();
	         UIMainMenu uiMainMenu = (UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
	         uiMainMenu.UpdateTextCoin();
	         //Show Inter ads
         }
	}
}