/*
*┌────────────────────────────────────────────────┐
*│　描    述：DirectoryChecked                                                    
*│　作    者：Kimi                                          
*│　版    本：1.0   
*│  UnityVersion：#UNITYVERSION#                                            
*│　创建时间：#DATE#                        
*└────────────────────────────────────────────────┘
*/

using Plusbe.Core;
using Plusbe.Drawing;
using PlusbeHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 主要功能
/// 1.文件检测读取变更，
/// 2.生成缩略图
/// 3.上传，保留功能
/// </summary>
public class DirectoryChecked : MonoBehaviour
{
    private string checkedPath;

    private float gameTime;

    private List<string> nikanPhotos;

    public void Init()
    {
        checkedPath = GlobalSetting.CheckedPath;

        nikanPhotos = new List<string>();
        StartCoroutine(DoUpdate());
    }

    private int mustCount;
    private string lastPhotoName;
    private int lastFileCount;

    private IEnumerator DoUpdate()
    {
        yield return new WaitForEndOfFrame();

        while(Application.isPlaying)
        {
            string[] files = PathHelper.GetFileImage(checkedPath);
            mustCount++;
            yield return new WaitForSeconds(1f);

            string[] historyFiles = null; // PathHelper.GetAllFilesAndChild(GlobalSetting.PhotoPath);

            if (files.Length > 0)
            {
                string last = files[files.Length - 1]; //获取最新的1张照片名字

                if (last != lastPhotoName  || files.Length != lastFileCount || mustCount > 10)
                {
                    lastFileCount = files.Length;
                    lastPhotoName = last;
                    mustCount = 0;


                    for (int i = 0; i < files.Length; i++)
                    {
                        string path = files[i];
                        string thumbPath = GlobalSetting.PhotoThumbPath + Path.GetFileName(path);

                        //FileInfo fileInfo2 = new FileInfo(path);
                        //Debug.Log(fileInfo2.LastWriteTime+","+fileInfo2.CreationTime+","+fileInfo2.LastAccessTime);

                        //不存在缩略图
                        if (File.Exists(path) && !File.Exists(thumbPath))
                        {
                            long fileLength = -1;
                            //0.3f*10次 检测照片是否已经结束传输
                            for (int j = 0; j < 10; j++)
                            {
                                FileInfo fileInfo = new FileInfo(path);

                                if (fileInfo.Length != fileLength)
                                {
                                    if (j == 0)
                                    {
                                        yield return new WaitForSeconds(0.2f);
                                    }
                                    else
                                    {
                                        yield return new WaitForSeconds(0.5f);
                                    }

                                    fileLength = fileInfo.Length;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            try
                            {
                                if (!PicHelper.Thumbnail(path, thumbPath, 192, 108))
                                {
                                    PathHelper.CreateDirectory(GlobalSetting.PhotoPath + GlobalSetting.DateDirectory);
                                    MoveFile(path, GlobalSetting.PhotoPath + GlobalSetting.DateDirectory + Path.GetFileName(path),historyFiles);

                                    Debug.Log("检测新增:" + path);
                                }
                                else
                                {
                                    //已经存在了
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.Log(ex.ToString() + ",检测新增失败:" + path);
                            }
                        }
                        else if (File.Exists(path) && File.Exists(thumbPath)) 
                        {
                            //图与缩略图同时存在
                            PathHelper.CreateDirectory(GlobalSetting.PhotoPath + GlobalSetting.DateDirectory);
                            MoveFile(path, GlobalSetting.PhotoPath + GlobalSetting.DateDirectory + Path.GetFileName(path), historyFiles);

                        }
                    }

                    Debug.Log("检测结束");
                }
            }
        }
    }

    
    private bool MoveFile(string source, string des,string[] historyFiles)
    {

        try
        {
            if (!ExistFile(source, historyFiles))
            {
                File.Copy(source, des);

                File.Delete(source);
            }
            else
            {
                File.Delete(source);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("move/delete file error;" + source + "," + ex.ToString());
        }

        return true;
    }

    private bool ExistFile(string source, string[] history)
    {
        string fileName = Path.GetFileName(source);
        for (int i = 0; i < history.Length; i++)
        {
            if (history[i].IndexOf(fileName) != -1)
                return true;
        }

        return false;
    }
}
