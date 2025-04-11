using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using DG.Tweening;
using Huy_Core;
using UnityEngine.Networking;

public class AssetBundleManager : SingletonMono<AssetBundleManager>
{
    string LocalPath
    {
        get => "AssetBundles";
    }

    private bool isDownloading;
    public bool IsDownloading
    {
        get => isDownloading;
        set => isDownloading = value;
    }

    public event Action onStartDownload;
    
    public Dictionary<string,AssetBundleItem> songDatabase = new Dictionary<string, AssetBundleItem>();

    private void Start()
    {
        DownloadAsset_WWW(
            "https://github.com/truonghuy0103/RhythmGame/raw/refs/heads/Huy/Huy_18-08/AssetBundles/iOS/inst-ballistic",
            "/inst-ballistic",
            () =>
            {
                Debug.Log("Success download assetbundle from github");
                StartCoroutine(GetSongFromBundle("/inst-ballistic", "inst-ballistic"));
            },
            () => { Debug.Log("Failed to download github"); });
    }

    public void RemoteConfigUpdate()
    {
        //Get Config asset bundle into song database
    }

    private IEnumerator Download(string url, Action<bool, object> onResult)
    {
        isDownloading = true;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            AsyncOperation request = webRequest.SendWebRequest();
            while (!request.isDone)
            {
                //Show UI Loading
                yield return null;
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result ==
                UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("failed reason: " + webRequest.result);
                onResult?.Invoke(false, null);
            }
            else
            {
                onResult?.Invoke(true, webRequest.downloadHandler.data);
            }
        }
    }

    public void DownloadAsset_WWW(string url, string path, Action onSuccess, Action onFail, bool isSaveLocal = true)
    {
        path = path.ToLower().Trim();
        //Show UI Loading
        string filePath = FileHelper.GetWritablePath(LocalPath) + path;
        Debug.Log("Filepath: " + filePath);
        if (File.Exists(filePath))
        {
            onSuccess();
        }
        else
        {
            Action<bool, object> func = (success, data) =>
            {
                if (success)
                {
                    Debug.Log("AssetBundle download success");
                    if (isSaveLocal)
                    {
                        try
                        {
                            FileHelper.SaveFile((byte[])data, filePath, true);
                            Debug.Log("Save ok " + filePath);
                            onSuccess?.Invoke();
                        }
                        catch (Exception e)
                        {
                           Debug.LogError("Failed to download AssetBundle");
                        }
                    }
                }
                else
                {
                    onFail?.Invoke();
                }
            };

            StartCoroutine(Download(url, (suc, dt) =>
            {
                func.Invoke(suc, dt);
            }));
            
            onStartDownload?.Invoke();
        }
    }

    public void StartDownloadAssetBundle(string nameBundle, Action finish)
    {
        string bundleURL = songDatabase[nameBundle].androidURL;
#if UNITY_IOS
        bundleURL = songDatabase[nameBundle].iosURL;
#endif
        DownloadAsset_WWW(bundleURL, "/" + nameBundle, finish, () =>
        {
            Debug.LogError("Download AssetBundle failed");
        });
    }

    public IEnumerator GetSongFromBundle(string path, string objectNameToLoad)
    {
        string filePath = FileHelper.GetWritablePath(LocalPath) + path;
        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
        
        yield return assetBundleCreateRequest != null;
        
        var assetBundle = assetBundleCreateRequest.assetBundle;
        var asset = assetBundle.LoadAssetAsync<AudioClip>(objectNameToLoad);
        yield return asset != null;
        float value = 0;
        if (!IsDownloading)
        {
            DOTween.To(() => value, x => value = x, 1, 1).OnUpdate(() =>
            {
                //Show text UI loading
            });

            yield return new WaitForSeconds(1);
        }
        
        Huy_SoundManager.Instance.AddSoundBGM(asset.asset as AudioClip);
        Huy_SoundManager.Instance.StopAllSoundFX();
        Huy_SoundManager.Instance.PlaySoundBGM();
        assetBundle.Unload(false);
    }
}

[Serializable]
public class AssetBundleItem
{
    public string name;
    public string androidURL;
    public string iosURL;

    public AssetBundleItem(string name, string androidURL, string iosURL)
    {
        this.name = name;
        this.androidURL = androidURL;
        this.iosURL = iosURL;
    }
}
