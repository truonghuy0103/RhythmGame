using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hiep;
using Hiep_Core;
using TMPro;

namespace Hiep
{
	public class RewardParam : UIParam
	{
		public int valueCoin;
	}
	public class Hiep_UIReward : BaseUI
	{
		public TextMeshProUGUI txtCoin;
		
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            RewardParam rewardParam = (RewardParam)param;
            txtCoin.text = "+" + rewardParam.valueCoin;

            Hiep_GameManager.Instance.GameSave.Coin += rewardParam.valueCoin;
            SaveManager.Instance.SaveGame();

            Hiep_UIMainMenu uiMainMenu = (Hiep_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
            uiMainMenu.UpdateTextCoin();
         }
         
	}
}