using System;
using UnityEngine;
public interface IAssetLoader
{
    void LoadAsset(string path, Action<UnityEngine.Object> onComplete);
}

public class AssetBundleLoader : IAssetLoader
{
    public void LoadAsset(string path, Action<UnityEngine.Object> onComplete)
    {
        
    }
}
public class ResourcesLoader : IAssetLoader
{
    public void LoadAsset(string path, Action<UnityEngine.Object> onComplete)
    {
        UnityEngine.Object obj = Resources.Load(path);
        onComplete?.Invoke(obj);
    }
}