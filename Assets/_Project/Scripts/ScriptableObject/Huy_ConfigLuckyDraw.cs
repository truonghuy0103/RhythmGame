using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huy;
namespace Huy
{
	[CreateAssetMenu(fileName = "Huy Config Lucky Draw",menuName = "Config/Huy Config Lucky Draw",order = 0)]
	public class Huy_ConfigLuckyDraw : ScriptableObject
	{
		public Huy_ConfigLuckyDrawData[] data;
		private static Huy_ConfigLuckyDraw Instance;

		public static Huy_ConfigLuckyDrawData GetConfigLuckyDrawData(int index)
		{
			Instance = Resources.Load<Huy_ConfigLuckyDraw>("Configs/Huy Config Lucky Draw");
			Huy_ConfigLuckyDrawData result = null;
			foreach (var go in Instance.data)
			{
				if (go.id == index)
				{
					return go;
					break;
				}
			}

			if (result == null)
			{
				result = Instance.data[0];
			}
			
			return result;
		}
	}

	[Serializable]
	public class Huy_ConfigLuckyDrawData
	{
		public int id;
		public int coin;
	}
}