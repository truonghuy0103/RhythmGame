using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Config Mode", menuName = "Config/Config Mode", order = 3)]
public class Hiep_ConfigMode : ScriptableObject
{
    public Hiep_ConfigModeData[] data;
    private static Hiep_ConfigMode Instance;

    public static Hiep_ConfigModeData ConfigModeData(int indexMode)
    {
        Instance = Resources.Load<Hiep_ConfigMode>("Configs/Config Mode");
        Hiep_ConfigModeData result = null;
        if (Instance.data.Length > indexMode)
        {
            result = Instance.data[indexMode];
        }

        if (result == null)
        {
            result = Instance.data[0];
        }

        return result;
    }

    public static Hiep_ConfigWeekData ConfigWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<Hiep_ConfigMode>("Configs/Config Mode");
        Hiep_ConfigWeekData result = null;
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

    public static Hiep_ConfigSongData ConfigSongData(int indexMode, int indexWeek, int indexSong)
    {
        Instance = Resources.Load<Hiep_ConfigMode>("Configs/Config Mode");
        Hiep_ConfigSongData result = null;
        if (Instance.data.Length > indexMode && Instance.data[indexMode].configWeekDatas.Count > indexWeek 
            && Instance.data[indexMode].configWeekDatas[indexWeek].configSongDatas.Count > indexSong)
        {
            result = Instance.data[indexMode].configWeekDatas[indexWeek].configSongDatas[indexSong];
        }
        else
        {
            result = Instance.data[0].configWeekDatas[0].configSongDatas[0];
        }

        return result;
    }
}

[Serializable]
public class Hiep_ConfigSongData
{
    public string nameSong;
    public string nameJson;
}

[Serializable]
public class Hiep_ConfigWeekData
{
    public string name;
    public List<Hiep_ConfigSongData> configSongDatas = new List<Hiep_ConfigSongData>();
}

[Serializable]
public class Hiep_ConfigModeData
{
    public string nameMode;
    public string nameAssetBundle;
    public List<Hiep_ConfigWeekData> configWeekDatas = new List<Hiep_ConfigWeekData>();
}
