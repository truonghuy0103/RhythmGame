using System;
using System.Collections;
using System.Collections.Generic;
using Hiep;
using UnityEngine;
using Huy;
namespace Huy
{
	[CreateAssetMenu(fileName = "Huy Config Daily Login Reward",menuName = "Config/Huy Config Daily Login",order = 1)]
	public class Huy_ConfigDailyLogin : ScriptableObject
	{
		public Huy_ConfigDailyLoginData[] data;
		private static Huy_ConfigDailyLogin Instance;

		public static Huy_ConfigDailyLoginData GetDailyLoginData(int index)
		{
			Instance = Resources.Load<Huy_ConfigDailyLogin>("Configs/Huy Config Daily Login Reward");
			Huy_ConfigDailyLoginData result = null;
			foreach (var go in Instance.data)
			{
				if (go.id == index)
				{
					result = go;
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
	public class Huy_ConfigDailyLoginData
	{
		public string day;
		public int id;
		public int coin;
	}
}