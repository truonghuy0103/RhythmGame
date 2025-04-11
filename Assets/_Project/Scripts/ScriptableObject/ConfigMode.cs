using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigMode", menuName = "Config/Config Mode",order = 3)]
public class ConfigMode : ScriptableObject
{
    public ConfigModeData[] data;
    private static ConfigMode Instance;

    public static ConfigModeData ConfigModeData(int index)
    {
        Instance = Resources.Load<ConfigMode>("Configs/ConfigMode");
        ConfigModeData result = null;
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

    public static ConfigWeekData ConfigWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<ConfigMode>("Configs/ConfigMode");
        ConfigWeekData result = null;
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

    public static ConfigSongData ConfigSongData(int indexMode, int indexWeek, int indexSong)
    {
        Instance = Resources.Load<ConfigMode>("Configs/ConfigMode");
        ConfigSongData result = null;
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
public class ConfigSongData
{
    public string nameSong;
    public string nameJson;
}

[Serializable]
public class ConfigWeekData
{
    public string name;
    public List<ConfigSongData> configSongData = new List<ConfigSongData>();
}

[Serializable]
public class ConfigModeData
{
    public string mameMode;
    public string nameAssetBundle;
    public List<ConfigWeekData> configWeekDatas = new List<ConfigWeekData>();
}
