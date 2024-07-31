using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
/*
AB包加载模型
王帮晖
2019.9.20
*/
//1. 

namespace WBH.AB
{

    public class AbHelper
    {

        /// <summary>
        /// AB包本地路径
        /// </summary>
        private static string path = Directory.GetCurrentDirectory();
        /// <summary>
        /// AB包清单路径
        /// </summary>
        public static string manifestFilePath = Directory.GetCurrentDirectory() + "/Apps/Datas/AssetBundles/AssetBundles.manifest";

        //通过包名存放AssetBundle包
        public static Dictionary<string, AssetBundle> assetBundleDic = new Dictionary<string, AssetBundle>();

        public static string Path { get => path; set => path = value; }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="BundleName">Bundle包名</param>
        /// <param name="FileName">需要的资源名</param>
        /// <returns></returns>
        public static GameObject LoadAsset(string BundleName, string FileName)
        {

            AssetBundle bundle;
            Object asset;
            string path = $"{Path}/" + BundleName;
            if (assetBundleDic.TryGetValue(BundleName, out bundle)) { }
            else
            {
                bundle = AssetBundle.LoadFromFile(path);
                if (bundle != null)
                {
                    assetBundleDic.Add(BundleName, bundle);
                }
            }
            asset = bundle.LoadAsset(FileName);
            LoadDependencies(BundleName);
            return GameObject.Instantiate(asset) as GameObject;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="BundleName">Bundle包名</param>
        /// <param name="FileName">需要的资源名</param>
        /// <returns></returns>
        public static void LoadScene(string BundleName, string scene)
        {

            AssetBundle bundle;
            string path = $"{Path}/" + BundleName;
            if (assetBundleDic.TryGetValue(BundleName, out bundle)) { }
            else
            {
                bundle = AssetBundle.LoadFromFile(path);
                if (bundle != null)
                {
                    assetBundleDic.Add(BundleName, bundle);
                }
            }
            SceneManager.LoadScene(scene);
        }
        /// <summary>
        /// 通过包名加载依赖文件,一般会在加载资源的时候自动加载
        /// </summary>
        /// <param name="bundle">包名</param>
        /// <returns></returns>
        public static void LoadDependencies(string BundleName)
        {
            AssetBundle assetBundle = null;
            if (!assetBundleDic.TryGetValue(manifestFilePath, out assetBundle))
            {
                assetBundle = AssetBundle.LoadFromFile(manifestFilePath);
                if (assetBundle != null)
                {
                    assetBundleDic.Add(manifestFilePath, assetBundle);
                }
            }

            AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //string[] bundles = manifest.GetAllAssetBundles();
            string[] depens = manifest.GetAllDependencies(BundleName);
            foreach (var item in depens)
            {
                AssetBundle depen=null;
                if (assetBundleDic.TryGetValue(item, out depen) == false)
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(Path + "/" + item);
                    assetBundleDic.Add(item, ab);
                }
            }

        }
        /// <summary>
        /// 将加载的Bundle包删除, 弱卸载，释放AssetBundle本身的内存
        /// </summary>
        /// <param name="BundleName">Bundle包名</param>
        /// <returns></returns>
        public static bool Unload(string BundleName)
        {
            AssetBundle bundle;
            if (assetBundleDic.TryGetValue(BundleName, out bundle))
            {
                bundle.Unload(false);
                assetBundleDic.Remove(BundleName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 强卸载（无视引用的卸载），释放AssetBundle本身的内存，同时回收从AssetBundle抽取的Asset
        /// </summary>
        /// <param name="BundleName">Bundle包名</param>
        /// <returns></returns>
        public static bool UnloadForce(string BundleName)
        {
            AssetBundle bundle;
            if (assetBundleDic.TryGetValue(BundleName, out bundle))
            {
                bundle.Unload(true);
                assetBundleDic.Remove(BundleName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 全局弱卸载，回收无引用Asset
        /// </summary>
        public static void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }




    }

}