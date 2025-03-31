using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huy;
using System;
namespace Huy
{
	public enum UIIndex
	{
		UIMainMenu,
		UIGameplay,
		UIWin,
		UILose,
		UILoading,
		UILuckyDraw,
		UISkin,
		UIReward,
		UIPause,
		UIRate,
		UINoInternet,
		UIOption,
		UIRewardLogin,
		COUNT
	}
	public class UIManager : SingletonMono<UIManager>
	{
		private Dictionary<UIIndex, BaseUI> dictUIs = new Dictionary<UIIndex, BaseUI>();
		public UIIndex currentUIIndex;
		public List<BaseUI> lsUIs = new List<BaseUI>();
		public List<BaseUI> lsUIShows = new List<BaseUI>();

		public void Init(Action callback)
		{
			for (int i = 0; i < lsUIs.Count; i++)
			{
				GameObject goUI = lsUIs[i].gameObject;
				goUI.transform.SetParent(transform,false);
				
				RectTransform rectTransform = goUI.transform.GetComponent<RectTransform>();
				rectTransform.offsetMax = rectTransform.offsetMin = Vector2.zero;
				
				BaseUI baseUI = goUI.GetComponent<BaseUI>();
				baseUI.OnInit();
				
				dictUIs.Add(baseUI.uiIndex, baseUI);
				
				goUI.SetActive(false);
			}

			if (callback != null)
			{
				callback();
			}
		}

		public void ShowUI(UIIndex uiIndex, UIParam param = null, Action callback = null)
		{
			BaseUI baseUI = dictUIs[uiIndex];
			if (!lsUIShows.Contains(baseUI))
			{
				baseUI.ShowUI(param);
				lsUIShows.Add(baseUI);
			}
		}

		public void HideUI(BaseUI baseUI, Action callback = null)
		{
			baseUI.HideUI(callback);
			lsUIShows.Remove(baseUI);
		}
		
		public void HideUI(UIIndex uiIndex, Action callback = null)
		{
			BaseUI baseUI = FindUIVisible(uiIndex);
			if (baseUI != null)
			{
				HideUI(baseUI);
			}
		}

		public void HideAllUI(Action callback = null)
		{
			for (int i = 0; i < lsUIShows.Count - 1; i++)
			{
				HideUI(lsUIShows[i], null);
			}

			if (lsUIShows.Count > 0)
			{
				HideUI(lsUIShows[lsUIShows.Count - 1], callback);
			}
			
			lsUIShows.Clear();
		}

		public BaseUI FindUIVisible(UIIndex uiIndex)
		{
			for (int i = 0; i < lsUIShows.Count; i++)
			{
				if (lsUIShows[i].uiIndex == uiIndex)
				{
					return lsUIShows[i];
				}
			}

			return null;
		}

		public BaseUI FindUI(UIIndex uiIndex)
		{
			return dictUIs[uiIndex];
		}
	}

	public class UIParam
	{
		
	}
}