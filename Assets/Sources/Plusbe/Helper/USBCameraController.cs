using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using Plusbe.Core;
using PlusbeHelper;
using System;

public class USBCameraController : MonoBehaviour {

    public Sprite[] sprites;
    public Texture2D textureNoAlpha;

    private static USBCameraController intsance;

    private RawImage cameraView;
    private Image imgNum;

    private RawImage cameraView2;
    private Image imgNum2;
    private bool useSecondView;

    private string cameraName;
    private WebCamTexture cameraTexture;

    private int requestedWidth = 640;
    private int requestedHeight = 480;
    private int requestedFPS = 25;

    private bool isOpen;
    private bool isPlaying;
    private bool isAnimation;

    private ImageType saveType = ImageType.JPEG;
    private string saveDirectory = "Temps/";

    private Action<string,Texture2D> photoCallBack;

    private Texture2D textureCamera;

    private string takePath;

    #region 公共类

    public static USBCameraController Instance
    {
        get
        {
            if (intsance == null)
            {
                intsance = ApplicationManager.Instance.gameObject.GetComponent<USBCameraController>();
            }

            return intsance;
        }
    }

    public void Init(RawImage cameraView,Image imgNum)
    {
        Init(cameraView, imgNum, requestedWidth, requestedHeight, requestedFPS);
    }

    public void Init(RawImage cameraView, Image imgNum, int requestedWidth, int requestedHeight, int requestedFPS = 25)
    {
        Init(cameraView, imgNum, null, null, requestedWidth, requestedHeight, requestedFPS);
    }

    public void Init(RawImage cameraView, Image imgNum, RawImage cameraView2, Image imgNum2, int requestedWidth, int requestedHeight, int requestedFPS = 25)
    {
        if (cameraView2 != null && imgNum2 != null)
        {
            useSecondView = true;
            this.cameraView2 = cameraView2;
            this.imgNum2 = imgNum2;
        }

        this.cameraView = cameraView;
        this.imgNum = imgNum;
        this.requestedWidth = requestedWidth;
        this.requestedHeight = requestedHeight;
        this.requestedFPS = requestedFPS;
    }

    public void AddListener(Action<string,Texture2D> callBack)
    {
        photoCallBack = callBack;
    }

    public void RemoveListener()
    {
        photoCallBack = null;
    }

    public void OpenCamera()
    {
        if (!isOpen)
        {
            StartCoroutine(UserOpenCamera());
        }
        else
        {
            PlayCamera();
        }
    }

    public void StopCamera()
    {
        if (isOpen)
        {
            cameraTexture.Stop();
            cameraTexture = null;
            isOpen = isPlaying = false;
            ShowAlpha();
        }
    }

    public void PauseCamera()
    {
        if (isOpen)
        {
            cameraTexture.Pause();
            isPlaying = false;
        }
    }

    public void PlayCamera()
    {
        if (isOpen)
        {
            cameraTexture.Play();
            ShowCamera();
            isPlaying = true;
        }
    }

    public void TakePhoto()
    {
        if (isAnimation) return;
        OpenCamera();
        isAnimation = true;
        StartCoroutine(DoAnimation());
    }

    public void TakePhoto(string photoName)
    {
        takePath = photoName;
        TakePhoto();
    }

    //相对于datas的相对路径即可
    public void InitTempDirectory(string dir)
    {
        saveDirectory = dir;
    }

    //public void TakePhoto(string path)
    //{
    //    if (isAnimation) return;

    //}

    public Texture2D GetCameraTexture()
    {
        if (isOpen)
        {
            if (textureCamera == null) textureCamera = new Texture2D(requestedWidth, requestedHeight);
            textureCamera.SetPixels(cameraTexture.GetPixels());
            textureCamera.Apply();
            return textureCamera;
        }

        return null;
    }

    public Texture2D GetCameraTexture(int x, int y, int w, int h)
    {
        if (isOpen)
        {
            if (textureCamera == null) textureCamera = new Texture2D(w, h);
            textureCamera.SetPixels(0, 0, w, h, cameraTexture.GetPixels(x, y, w, h));
            textureCamera.Apply();
            return textureCamera;
        }

        return null;
    }

    public byte[] GetBytes()
    {
        Texture2D temp = GetCameraTexture();

        return temp == null ? null : temp.EncodeToJPG();
    }

    public byte[] GetBytes(int x, int y, int w, int h)
    {
        //Texture2D temp = GetCameraTexture();

        Texture2D temp = GetCameraTexture(x, y, w, h);

        return temp == null ? null : temp.EncodeToJPG();
    }


    #endregion

    //private void initTempTexture()
    //{
    //    textureTemp = new Texture2D(cameraW, cameraH);
    //    textureFace = new Texture2D(cameraW, cameraH);
    //    textureSprite = new Texture2D(cameraW, cameraH);
    //}

    //private Texture2D getTexture2D(WebCamTexture cameraTexture)
    //{
    //    textureTemp.SetPixels(cameraTexture.GetPixels());
    //    textureTemp.Apply();
    //    return textureTemp;
    //}


    //private Texture2D LoadFaceTexture(byte[] result)
    //{
    //    textureFace.LoadImage(result);
    //    textureFace.Apply();

    //    return textureFace;
    //}

    #region 内部处理方法
    void OnAapplicationQuit()
    {
        StopCamera();
    }


    private void ShowAlpha()
    { 
        cameraView.texture = textureNoAlpha;
        if (useSecondView) cameraView2.texture = textureNoAlpha;
    }

    private void ShowCamera()
    {
        cameraView.texture = cameraTexture;
        if (useSecondView) cameraView2.texture = cameraTexture;
    }

    private void ShowImgNum()
    {
        imgNum.gameObject.SetActive(true);
        if (useSecondView) imgNum2.gameObject.SetActive(true);
    }

    private void HideImgNum()
    {
        imgNum.gameObject.SetActive(false);
        if (useSecondView) imgNum2.gameObject.SetActive(false);
    }

    private IEnumerator DoAnimation()
    {
        yield return new WaitForEndOfFrame();

        ShowImgNum();

        float scale = 1.6f;
        float time = 0.8f;
        //
        //for (int i = sprites.Length - 1; i >= 0; i--)
        for (int i = sprites.Length - 1; i >= 0; i--)
        {
            imgNum.rectTransform.localScale = new Vector3(1, 1, 1);
            imgNum.overrideSprite = sprites[i];
            imgNum.rectTransform.DOScaleX(scale, time);
            imgNum.rectTransform.DOScaleY(scale, time);

            if (useSecondView)
            {
                imgNum2.rectTransform.localScale = new Vector3(1, 1, 1);
                imgNum2.overrideSprite = sprites[i];
                imgNum2.rectTransform.DOScaleX(scale, time);
                imgNum2.rectTransform.DOScaleY(scale, time);
            }

            yield return new WaitForSeconds(1f);
        }

        isAnimation = false;
        
        SavePhoto();

        HideImgNum();
    }

    private void SavePhoto()
    {
        if (isOpen && isPlaying)
        {
            //Color[] colors = cameraTexture.GetPixels();
            Texture2D t = new Texture2D(cameraTexture.width, cameraTexture.height);
            int y = 0;
            while (y < t.height)
            {
                int x = 0;
                while (x < t.width)
                {
                    Color color = cameraTexture.GetPixel(x, y);
                    t.SetPixel(x, y, color);
                    ++x;
                }
                ++y;
            }
            t.Apply();

            byte[] pngs;
            string path;
            saveType = ImageType.PNG;
            if (string.IsNullOrEmpty(takePath))
            {
                pngs = t.EncodeToPNG();
                path = GlobalSetting.DataPath + saveDirectory + takePath;
            }else if (saveType == ImageType.JPEG)
            {
                pngs = t.EncodeToJPG();
                path = GlobalSetting.DataPath + saveDirectory + PathHelper.getTempFileName(".jpg");
            }
            else
            {
                pngs = t.EncodeToPNG();
                path = GlobalSetting.DataPath + saveDirectory + PathHelper.getTempFileName();
            }

            File.WriteAllBytes(path, pngs);

            GlobalSetting.LastPhoto = path;

            if (photoCallBack != null) photoCallBack(path, t);
        }
    }

    private IEnumerator UserOpenCamera()
    { 
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            cameraName = devices[0].name;
            cameraTexture = new WebCamTexture(cameraName, requestedWidth, requestedHeight, requestedFPS);
            cameraTexture.Play();
            isOpen = true;
            isPlaying = true;

            ShowCamera();
        }
    }

    

    #endregion

    #region 图像类型
    private enum ImageType
    {
        JPEG,
        PNG
    }

    #endregion

}
