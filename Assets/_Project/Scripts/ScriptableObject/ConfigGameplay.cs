using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigGameplay", menuName = "Config/Config Gameplay",order = 4)]
public class ConfigGameplay : ScriptableObject
{
    public GameplayModeData[] data;
    private static ConfigGameplay Instance;

    public static GameplayModeData ConfigModeData(int index)
    {
        Instance = Resources.Load<ConfigGameplay>("Configs/ConfigGameplay");
        GameplayModeData result = null;

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

    public static GameplayWeekData ConfigWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<ConfigGameplay>("Configs/ConfigGameplay");
        GameplayWeekData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].gameplayWeekDatas.Count > indexWeek)
        {
            result = Instance.data[indexMode].gameplayWeekDatas[indexWeek];
        }
        
        return result;
    }
    
    public static GameplaySongData ConfigSongData(int indexMode, int indexWeek, int indexSong)
    {
        Instance = Resources.Load<ConfigGameplay>("Configs/ConfigGameplay");
        GameplaySongData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].gameplayWeekDatas.Count > indexWeek &&
            Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas.Count > indexSong)
        {
            result = Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas[indexSong];
        }
        
        return result;
    }

    public static int GetModeLength()
    {
        Instance=Resources.Load<ConfigGameplay>("Configs/ConfigGameplay");
        return Instance.data.Length;
    }

    public static int GetWeekLength(int indexMode)
    {
        Instance=Resources.Load<ConfigGameplay>("Configs/ConfigGameplay");
        return Instance.data[indexMode].gameplayWeekDatas.Count;
    }

    public static int GetSongLength(int indexMode, int indexWeek)
    {
        Instance=Resources.Load<ConfigGameplay>("Configs/ConfigGameplay");
        return Instance.data[indexMode].gameplayWeekDatas[indexWeek].gameplaySongDatas.Count;
    }

    public static int GetAllSongInMode(int indexMode)
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
public class GameplayModeData
{
    public string nameMode;
    public List<GameplayWeekData> gameplayWeekDatas = new List<GameplayWeekData>();
}

[Serializable]
public class GameplayWeekData
{
    public string nameWeek;
    public List<GameplaySongData> gameplaySongDatas = new List<GameplaySongData>();
}

[Serializable]
public class GameplaySongData
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