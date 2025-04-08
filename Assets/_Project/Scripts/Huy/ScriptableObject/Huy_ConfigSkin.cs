using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huy;
namespace Huy
{
	[CreateAssetMenu(fileName = "Huy Config Skin", menuName = "Config/Huy Config Skin",order = 5)]
	public class Huy_ConfigSkin : ScriptableObject
	{
		public Huy_ConfigSkinData[] dataBoys;
		public Huy_ConfigSkinData[] dataGirls;
		
		private static Huy_ConfigSkin Instance;

		public static Huy_ConfigSkinData GetConfigSkinDataBoy(int index)
		{
			Instance = Resources.Load<Huy_ConfigSkin>("Configs/Huy Config Skin");

			Huy_ConfigSkinData result = null;
			foreach (var go in Instance.dataBoys)
			{
				if (go.id == index)
				{
					result = go;
					break;
				}
			}

			if (result == null)
			{
				result = Instance.dataBoys[0];
			}
			
			return result;
		}
		
		public static Huy_ConfigSkinData GetConfigSkinDataGirl(int index)
		{
			Instance = Resources.Load<Huy_ConfigSkin>("Configs/Huy Config Skin");

			Huy_ConfigSkinData result = null;
			foreach (var go in Instance.dataGirls)
			{
				if (go.id == index)
				{
					result = go;
					break;
				}
			}

			if (result == null)
			{
				result = Instance.dataGirls[0];
			}
			
			return result;
		}
	}

	[Serializable]
	public class Huy_ConfigSkinData
	{
		public int id;
		public RuntimeAnimatorController skinAnimator;
		public int coin;
		public Sprite spriteSkin;
	}
}