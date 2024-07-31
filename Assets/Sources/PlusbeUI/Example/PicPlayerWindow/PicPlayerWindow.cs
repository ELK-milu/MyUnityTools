using UnityEngine;
using System.Collections;
using PlusbeHelper;
using Plusbe.Core;
using Plusbe.Drawing;
using UnityEngine.UI;
using Plusbe.Helper;
using System.IO;
using UnityEngine.Profiling;
using UnityEngine.Networking;
public class PicPlayerWindow : UIWindowBase 
{

    public RawImage[] rawImages;

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("btn_load", OnClickLoad);
        AddOnClickListener("btn_load2", OnClickLoad2);

        AddOnClickListener("btn_gc", OnClickGC);
        AddOnClickListener("btn_save", OnClickSave);
        AddOnClickListener("btn_save_thumb", OnClickSaveThumb);
        AddOnClickListener("btn_save_merge", OnClickSaveMerge);

        ApplicationManager.s_OnApplicationUpdate += OnUpdate;

    }

    private readonly static string TotalAllocMemroyFormation = "Alloc Memory : {0}M";
    private readonly static string TotalReservedMemoryFormation = "Reserved Memory : {0}M";
    private readonly static string TotalUnusedReservedMemoryFormation = "Unused Reserved: {0}M";
    //private readonly static string RuntimeMemorySizeFormation = "RuntimeMemorySize: {0}M";
    private readonly static string MonoHeapFormation = "Mono Heap : {0}M";
    private readonly static string MonoUsedFormation = "Mono Used : {0}M";
    // 字节到兆
    private float ByteToM = 0.000001f;

    public Text txtDebug;

    private void OnUpdate()
    {
        txtDebug.text = string.Format(TotalAllocMemroyFormation, Profiler.GetTotalAllocatedMemoryLong() * ByteToM) + "\r\n"
            + string.Format(TotalReservedMemoryFormation, Profiler.GetTotalReservedMemoryLong() * ByteToM) + "\r\n"
            + string.Format(TotalUnusedReservedMemoryFormation, Profiler.GetTotalUnusedReservedMemoryLong() * ByteToM) + "\r\n"
            + string.Format(MonoHeapFormation, Profiler.GetMonoHeapSizeLong() * ByteToM) + "\r\n"
            + string.Format(MonoUsedFormation, Profiler.GetMonoUsedSizeLong() * ByteToM) + "\r\n";
    }


    public override void OnClose()
    {
        ApplicationManager.s_OnApplicationUpdate -= OnUpdate;
    }

    public override void OnUIDestroy()
    {
        OnClose();

        base.OnUIDestroy();
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {
        
    }

    //UI的进入动画 调用 base.EnterAnim 表示进入动画播放完成
    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画 调用 base.ExitAnim 表示退出动画播放完成
    public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    private void OnClickLoad(InputUIOnClickEvent e)
    {
        StartCoroutine(ToLoadPic());
    }

    private void OnClickLoad2(InputUIOnClickEvent e)
    {
        ToLoadPic3();
        //StartCoroutine(ToLoadPic2());
    }

    private IEnumerator ToLoadPic()
    {
        yield return new WaitForEndOfFrame();
        string[] files = PathHelper.GetFileImage(GlobalSetting.DataPath + "UploadFiles");
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < files.Length; i++)
            {
                GetRaw().texture = PicHelper.LoadPicTexture(files[i]);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }
        }

        
    }

    private void ToLoadPic3()
    {
        string[] files = PathHelper.GetFileImage(GlobalSetting.DataPath + "UploadFiles");

        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < files.Length; i++)
            {
                StartCoroutine(PicHelper.LoadPicTexture(files[i],LoadPicCallBack));
            }
        }

        ResourceGCHelper.GC();
    }

    

    //private IEnumerator ToLoadPic2()
    //{
    //    yield return new WaitForEndOfFrame();
    //    string[] files = PathHelper.GetFileImage(GlobalSetting.DataPath + "UploadFiles");

    //    for (int j = 0; j < 10; j++)
    //    {
    //        for (int i = 0; i < files.Length; i++)
    //        {

    //            WWW www = new WWW(files[i]);

    //            yield return www;
    //            if (string.IsNullOrEmpty(www.error))
    //                GetRaw().texture = www.texture;

    //            yield return new WaitForEndOfFrame();
    //        }
    //    }

    //    ResourceGCHelper.GC();
    //}

    private void LoadPicCallBack(Texture2D texture)
    {
        GetRaw().texture = texture;
    }

    private void OnClickGC(InputUIOnClickEvent e)
    {
        //yield return new WaitForEndOfFrame();

        for (int i = 0; i < rawImages.Length; i++)
        {
            GetRaw().texture = null;
        }

        ResourceGCHelper.GC();
    }

    private void OnClickSave(InputUIOnClickEvent e)
    {
        StartCoroutine(ToSavePic());
    }

    private IEnumerator ToSavePic()
    {
        yield return new WaitForEndOfFrame();
        string[] files = PathHelper.GetFileImage(GlobalSetting.DataPath + "UploadFiles");
        PathHelper.CreateDirectory(GlobalSetting.DataPath + "Temps/");
        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < files.Length; i++)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(files[i]);
                yield return www;
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                GetRaw().texture = texture;
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(GlobalSetting.DataPath + "Temps/" + PathHelper.getTempFileName(".png"), bytes);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void OnClickSaveThumb(InputUIOnClickEvent e)
    {
        StartCoroutine(ToSavePicThumb());
    }

    private IEnumerator ToSavePicThumb()
    {
        yield return new WaitForEndOfFrame();
        string[] files = PathHelper.GetFileImage(GlobalSetting.DataPath + "UploadFiles");
        PathHelper.CreateDirectory(GlobalSetting.DataPath + "Temps/");
        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < files.Length; i++)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(files[i]);
                yield return www;
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                GetRaw().texture = texture;
                byte[] bytes = texture.EncodeToPNG();
                string path = GlobalSetting.DataPath + "Temps/" + PathHelper.getTempFileName("");
                File.WriteAllBytes(path+".jpg", bytes);
                PicHelper.Thumbnail(path + ".jpg", path + "_thumb.jpg", 500, 280);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void OnClickSaveMerge(InputUIOnClickEvent e)
    {
        StartCoroutine(ToSavePicMerge());
    }

    private IEnumerator ToSavePicMerge()
    {
        yield return new WaitForEndOfFrame();
        string[] files = PathHelper.GetFileImage(GlobalSetting.DataPath + "UploadFiles");
        PathHelper.CreateDirectory(GlobalSetting.DataPath + "Temps/");
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < files.Length; i++)
            {

                watch.Stop();
                watch.Start();
                Debug.Log("index【" + i + "】【开始游戏】" + watch.ElapsedMilliseconds);
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(files[i]);
                Debug.Log("index【" + i + "】【开始加载】" + watch.ElapsedMilliseconds);
                yield return www;
                Debug.Log("index【" + i + "】【完成加载】" + watch.ElapsedMilliseconds);
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Debug.Log("index【" + i + "】【获取贴图】" + watch.ElapsedMilliseconds);
                GetRaw().texture = texture;
                Debug.Log("index【" + i + "】【赋值贴图】" + watch.ElapsedMilliseconds);
                byte[] bytes = texture.EncodeToJPG();
                Debug.Log("index【" + i + "】【转化数据】" + watch.ElapsedMilliseconds);
                string path = GlobalSetting.DataPath + "Temps/" + PathHelper.getTempFileName("");
                File.WriteAllBytes(path + ".jpg", bytes);
                Debug.Log("index【" + i + "】【保存图片】" + watch.ElapsedMilliseconds);
                PicHelper.Thumbnail(path + ".jpg", path + "_thumb.jpg", 192, 108);
                Debug.Log("index【" + i + "】【裁剪缩略】" + watch.ElapsedMilliseconds);
                PicHelper.MergeImage(path + ".jpg", path + "_merge.jpg", 1610, 1080, path + "_thumb.jpg", 0, 0, 192, 108, 0, 0, 192, 108);
                Debug.Log("index【" + i + "】【合成图片】" + watch.ElapsedMilliseconds);
                PicHelper.MergeImageThread(path + ".jpg", path + "_merge_thread.jpg", 1610, 1080, path + "_thumb.jpg", 0, 0, 192, 108, 0, 0, 192, 108, MergeCallBack);
                Debug.Log("index【" + i + "】【线程合成】" + watch.ElapsedMilliseconds);
                

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void MergeCallBack(string path)
    {
        Debug.Log("MergeCallBack:"+path);   
    }


    private int showIndex;

    private RawImage GetRaw()
    {
        return rawImages[++showIndex % rawImages.Length];
    }
}