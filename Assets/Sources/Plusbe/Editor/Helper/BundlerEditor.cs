using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BundlerEditor
{
    [MenuItem("PlusbeTool/打包AB包", priority = 3000)]
    public static void BuildAB()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/../Apps/Datas/AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();
    }


    [MenuItem("PlusbeTool/导出程序", priority = 3000)]
    public static void BuildApp()
    {

        string sceneName = SceneManager.GetActiveScene().name;
        PlayerSettings.productName = sceneName;
        //BuildOptions.ShowBuiltPlayer();

        

        Debug.Log("输出程序：" + sceneName + Application.dataPath);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;


        buildPlayerOptions.locationPathName = Application.dataPath + "/../Apps/"+ sceneName+".exe";

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        Application.OpenURL(Application.dataPath + "/../Apps");

        //BuildSummary summary = report.summary;

        //if (summary.result == BuildResult.Succeeded)
        //{
        //    Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        //}

        //if (summary.result == BuildResult.Failed)
        //{
        //    Debug.Log("Build failed");
        //}

        //BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

        //AssetDatabase.Refresh();
    }

    //public Image imgHead;

    //public void ToLoadABSCube()
    //{
    //    AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/../ABS/testzerocube");
    //    GameObject go = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("zerocube"));
    //}

    //private Sprite[] sps;

    //public void ToLoadABS()
    //{
    //    AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/../ABS/testimages");
    //    sps = assetBundle.LoadAllAssets<Sprite>();
    //    //Sprite sp = assetBundle.LoadAsset<Sprite>("IMG_20140608_190052");
    //    //imgHead.sprite = sp;
    //    //GameObject go = GameObject.Instantiate(assetBundle.LoadAsset<Sprite>("IMG_20140608_193548"));
    //}

    //private float lastTime;
    //private int currIndex;

    //void Update()
    //{
    //    if (Time.time - lastTime > 0.1f && sps != null && sps.Length > 0)
    //    {
    //        lastTime = Time.time;
    //        imgHead.sprite = sps[++currIndex % sps.Length];
    //    }
    //}
}
