using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config Gameplay", menuName = "Config/Config Gameplay", order = 4)]
public class Hiep_ConfigGameplay : ScriptableObject
{
    public Hiep_GameplayModData[] data;
    static Hiep_ConfigGameplay Instance;
    public static Hiep_GameplayModData ConfigModData(int index)
    {
        Instance = Resources.Load<Hiep_ConfigGameplay>("Configs/Config Gameplay");

        Hiep_GameplayModData result = null;
        //foreach (var go in Instance.data)
        //{
        //    if (go.id == index)
        //    {
        //        result = go;
        //        break;
        //    }
        //}

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

    public static Hiep_GamePlaySongData ConfigSongData(int indexMod, int indexWeek, int indexSong)
    {
        Instance = Resources.Load<Hiep_ConfigGameplay>("Configs/Config Gameplay");

        Hiep_GamePlaySongData result = null;

        if (Instance.data.Length > indexMod && Instance.data[indexMod].gameplayWeekDatas.Count > indexWeek 
        && Instance.data[indexMod].gameplayWeekDatas[indexWeek].gamePlaySongDatas.Count > indexSong)
        {
            result = Instance.data[indexMod].gameplayWeekDatas[indexWeek].gamePlaySongDatas[indexSong];
        }

        return result;
    }

    public static Hiep_GameplayWeekData ConfigWeekData(int indexMod, int indexWeek)
    {
        Instance = Resources.Load<Hiep_ConfigGameplay>("Configs/Config Gameplay");

        Hiep_GameplayWeekData result = null;

        if (Instance.data.Length > indexMod && Instance.data[indexMod].gameplayWeekDatas.Count > indexWeek)
        {
            result = Instance.data[indexMod].gameplayWeekDatas[indexWeek];
        }

        return result;
    }

    public static int GetModeLength()
    {
        Instance = Resources.Load<Hiep_ConfigGameplay>("Configs/Config Gameplay");
        return Instance.data.Length;
    }

    public static int GetWeekLength(int indexMode)
    {
        Instance = Resources.Load<Hiep_ConfigGameplay>("Configs/Config Gameplay");
        return Instance.data[indexMode].gameplayWeekDatas.Count;
    }

    public static int GetSongLength(int indexMode, int indexWeek)
    {        
        Instance = Resources.Load<Hiep_ConfigGameplay>("Configs/Config Gameplay");
        return Instance.data[indexMode].gameplayWeekDatas[indexWeek].gamePlaySongDatas.Count;
    }

    public static int GetAllSongInMode(int indexMode)
    {
        int countSong = 0;
        for(int i = 0; i < GetWeekLength(indexMode); i++)
        {
            countSong += GetSongLength(indexMode, i);
        }
        return countSong;
    }

}

[Serializable]
public class Hiep_GameplayModData
{
    public string nameMod;
    public List<Hiep_GameplayWeekData> gameplayWeekDatas = new List<Hiep_GameplayWeekData>();

}

[Serializable]
public class Hiep_GameplayWeekData
{
    public string nameWeek;
    public List<Hiep_GamePlaySongData> gamePlaySongDatas = new List<Hiep_GamePlaySongData>();
}

[Serializable]
public class Hiep_GamePlaySongData
{
    public string nameSong;
    public Sprite spriteBG;
    public Sprite spriteSubBG;
    public RuntimeAnimatorController enemyAnimator;
    public Sprite spriteIcon;
    public Sprite spriteIconLose;
    public Sprite spriteCharacter;
    public int price;
    public Vector3 localPositionBG = Vector3.zero;
    public Vector3 localScaleBG = Vector3.one;
    public Vector3 localPositionSubBG = Vector3.zero;
    public Vector3 localScaleSubBG = Vector3.one;
}