using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Huy Config Gameplay", menuName = "Config/Huy Config Gameplay",order = 4)]
public class Huy_ConfigGameplay : ScriptableObject
{
    public Huy_GameplayModeData[] data;
    private static Huy_ConfigGameplay Instance;

    public static Huy_GameplayModeData GameplayModeData(int index)
    {
        Instance = Resources.Load<Huy_ConfigGameplay>("Configs/Huy Config Gameplay");
        Huy_GameplayModeData result = null;

        if (Instance.data.Length > index)
        {
            result = Instance.data[index];
        }

        if (result == null)
        {
            result = Instance.data[0];
        }
        
        return result;
    }

    public static Huy_GameplayWeekData GameplayWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<Huy_ConfigGameplay>("Configs/Huy Config Gameplay");
        Huy_GameplayWeekData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].gameplayWeekDatas.Count > indexWeek)
        {
            result = Instance.data[indexMode].gameplayWeekDatas[indexWeek];
        }

        if (result == null)
        {
            result = Instance.data[0].gameplayWeekDatas[0];
        }
        
        return result;
    }
    
    public static Huy_GameplaySongData GameplaySongData(int indexMode, int indexWeek, int indexSong)
    {
        Instance = Resources.Load<Huy_ConfigGameplay>("Configs/Huy Config Gameplay");
        Huy_GameplaySongData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].gameplayWeekDatas.Count > indexWeek &&
            Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas.Count > indexSong)
        {
            result = Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas[indexSong];
        }

        if (result == null)
        {
            result = Instance.data[0].gameplayWeekDatas[0].gameplaySongDatas[0];
        }
        
        return result;
    }

    public static int GetModeLength()
    {
        Instance=Resources.Load<Huy_ConfigGameplay>("Configs/Huy Config Gameplay");
        return Instance.data.Length;
    }

    public static int GetWeekLength(int indexMode)
    {
        Instance=Resources.Load<Huy_ConfigGameplay>("Configs/Huy Config Gameplay");
        return Instance.data[indexMode].gameplayWeekDatas.Count;
    }

    public static int GetSongLength(int indexMode, int indexWeek)
    {
        Instance=Resources.Load<Huy_ConfigGameplay>("Configs/Huy Config Gameplay");
        return Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas.Count;
    }

    public static int GetCountSongInMode(int indexMode)
    {
        int countSong = 0;
        for (int i = 0; i < GetWeekLength(indexMode); i++)
        {
            countSong += GetSongLength(indexMode, i);
        }
        
        return countSong;
    }
}

[Serializable]
public class Huy_GameplayModeData
{
    public string nameMode;
    public List<Huy_GameplayWeekData> gameplayWeekDatas = new List<Huy_GameplayWeekData>();
}

[Serializable]
public class Huy_GameplayWeekData
{
    public string nameWeek;
    public List<Huy_GameplaySongData> gameplaySongDatas = new List<Huy_GameplaySongData>();
}

[Serializable]
public class Huy_GameplaySongData
{
    public string nameSong;
    public Sprite spriteBG;
    public Sprite spriteSubBG;
    public RuntimeAnimatorController enemyAnimator;
    public Sprite spriteIcon;
    public Sprite spriteIconLose;
    public Sprite spriteCharacter;
    public int price = 200;
    public Vector3 localScaleBG = Vector3.one;
    public Vector3 localPositionBG = Vector3.zero;
    public Vector3 localScaleSubBG = Vector3.one;
    public Vector3 localPositionSubBG = Vector3.zero;
}