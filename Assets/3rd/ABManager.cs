using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum AssetLoadType
{
    AssetBundle,
    Resources,
}
public class ABManager
{

    public static AssetLoadType LoadType;
    public static IAssetLoader Loader;
    #region  AB包路径
    /// <summary>
    /// AB包本地路径
    /// </summary>
    private static string path = Directory.GetCurrentDirectory();
    /// <summary>
    /// AB包清单路径
    /// </summary>
    public static string manifestFilePath = Directory.GetCurrentDirectory() + "/Apps/Datas/AssetBundles/AssetBundles";
#endregion

    public static void Init()
    {
        Loader = AssetLoaderFactory.CreateLoader(LoadType);
        LoadABInfo();
    }

    /// <summary>
    /// 加载所有的AB包清单
    /// </summary>
    private static void LoadABInfo()
    {
        AssetBundleManifest manifest = AssetBundle.LoadFromFile(manifestFilePath).LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] abs = manifest.GetAllAssetBundles();
        
        string[] abwvs = manifest.GetAllAssetBundlesWithVariant();

        string[] ads = manifest.GetAllDependencies(abs[1]);

        Debug.Log(ads);
    }

}
