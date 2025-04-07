using System.Collections;
using System.Collections.Generic;
using Huy_Core;
using UnityEngine;
using Huy;
using TMPro;

namespace Huy
{
	public class RewardParam : UIParam
	{
		public int valueCoin;
	}
	public class Huy_UIReward : BaseUI
	{
		[SerializeField] private TextMeshProUGUI txtCoin;
	     public override void OnInit()
            {
                base.OnInit();
            }
        
         public override void OnSetup(UIParam param = null)
         {
            base.OnSetup(param);
            RewardParam rewardParam = (RewardParam)param;
            txtCoin.text = "+" + rewardParam.valueCoin.ToString();
            
            Huy_GameManager.Instance.GameSave.Coin += rewardParam.valueCoin;
            SaveManager.Instance.SaveGame();
         }
	}
}