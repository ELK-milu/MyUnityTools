using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class DownLoadAssetBundle : MonoBehaviour {


    private string mainAssetBundleURL = @"http://localhost/AssetBundles/AssetBundles";

    private string allAssetBundleURL = @"http://localhost/AssetBundles/";
    private string SaveUri=Application.streamingAssetsPath+"/AssetBundle";

    [System.Obsolete]
    void Start () {

        StartCoroutine(DownLoadMainAssetBundel());

    }

    [System.Obsolete]
    IEnumerator DownLoadMainAssetBundel()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(mainAssetBundleURL);
        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //Debug.Log("OK");
        AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] names = manifest.GetAllAssetBundles();
        for (int i = 0; i < names.Length; i++)
        {
            Debug.Log(allAssetBundleURL + names[i]);
            //StartCoroutine(DownLoadSingleAssetBundel(allAssetBundleURL + names[i]));
            StartCoroutine(DownLoadAssetBundelAbdSave(allAssetBundleURL + names[i]));
        }
    }
    /// <summary>
    /// 下载单个AB文件不保存
    /// </summary>
    IEnumerator DownLoadSingleAssetBundel(string url)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //Debug.Log("OK");

        string[] names = ab.GetAllAssetNames();
        for (int i = 0; i < names.Length; i++)
        {

            string tempName = Path.GetFileNameWithoutExtension(names[i]);
            //Debug.Log(tempName);

            GameObject gameObject = ab.LoadAsset<GameObject>(tempName);
            GameObject.Instantiate<GameObject>(gameObject);

        }


    }

    /// <summary>
    /// 下载AB文件并保存到本地
    /// </summary>
    /// <returns></returns>
    [System.Obsolete]
    IEnumerator DownLoadAssetBundelAbdSave(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if(www.isDone)
        {
            //表示资源下载完毕使用IO技术把www对象存储到本地
            SaveAssetBundle(Path.GetFileName(url), www.bytes, www.bytes.Length);
            
        }
    }
    /// <summary>
    /// 存储AB文件到本地
    /// </summary>
    private void SaveAssetBundle(string fileName, byte[] bytes, int count)
    {   
        if (!Directory.Exists(SaveUri))
        {   
            Directory.CreateDirectory(SaveUri);
        }
        FileInfo fileInfo = new FileInfo(SaveUri + "//" + fileName);
        FileStream fs = fileInfo.Create();
        fs.Write(bytes, 0, count);
        fs.Flush();
        fs.Close();
        fs.Dispose();
        Debug.Log(fileName + "下载并存储完成");
    }

}