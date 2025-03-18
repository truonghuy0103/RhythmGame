using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Huy Config Mode", menuName = "Config/Huy Config Mode",order = 3)]
public class Huy_ConfigMode : ScriptableObject
{
    public Huy_ConfigModeData[] data;
    private static Huy_ConfigMode Instance;

    public static Huy_ConfigModeData ConfigModeData(int index)
    {
        Instance = Resources.Load<Huy_ConfigMode>("Configs/Huy Config Mode");
        Huy_ConfigModeData result = null;
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

    public static Huy_ConfigWeekData ConfigWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<Huy_ConfigMode>("Configs/Huy Config Mode");
        Huy_ConfigWeekData result = null;
        if (Instance.data.Length > indexMode && Instance.data[indexMode].configWeekDatas.Count > indexWeek)
        {
            result = Instance.data[indexMode].configWeekDatas[indexWeek];
        }
        else
        {
            result = Instance.data[0].configWeekDatas[0];
        }

        return result;
    }

    public static Huy_ConfigSongData ConfigSongData(int indexMode, int indexWeek, int indexSong)
    {
        Instance = Resources.Load<Huy_ConfigMode>("Configs/Huy Config Mode");
        Huy_ConfigSongData result = null;
        if (Instance.data.Length > indexMode && Instance.data[indexMode].configWeekDatas.Count > indexWeek &&
            Instance.data[indexMode].configWeekDatas[indexWeek].configSongData.Count > indexSong)
        {
            result = Instance.data[indexMode].configWeekDatas[indexWeek].configSongData[indexSong];
        }
        else
        {
            result = Instance.data[0].configWeekDatas[0].configSongData[0];
        }

        return result;
    }
}

[Serializable]
public class Huy_ConfigSongData
{
    public string nameSong;
    public string nameJson;
}

[Serializable]
public class Huy_ConfigWeekData
{
    public string name;
    public List<Huy_ConfigSongData> configSongData = new List<Huy_ConfigSongData>();
}

[Serializable]
public class Huy_ConfigModeData
{
    public string mameMode;
    public string nameAssetBundle;
    public List<Huy_ConfigWeekData> configWeekDatas = new List<Huy_ConfigWeekData>();
}
