using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huy;
namespace Huy
{
	public class Huy_UIRewardLogin : BaseUI
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
	            Huy_ConfigDailyLoginData configDailyLoginData = Huy.Huy_ConfigDailyLogin.GetDailyLoginData(i);
	            int currentDayLogin = Huy_GameManager.Instance.GameSave.CurrentDayLogin;
	            int currentWeekLogin = Huy_GameManager.Instance.GameSave.CurrentDayOfWeekLogin;
	            int coin = 0;

	            if (DateTime.Now.DayOfYear - currentWeekLogin == currentDayLogin)
	            {
		            //Get coin from config
		            coin = configDailyLoginData.coin;
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
	         Huy_UIMainMenu uiMainMenu = (Huy_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
	         uiMainMenu.UpdateTextCoin();
	         //Show Inter ads
         }
	}
}