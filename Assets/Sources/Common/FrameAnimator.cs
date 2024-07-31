/****************************************************
    文件：FrameAnimator.cs
	作者：Plusbe_wyl
    邮箱: 1442625266@qq.com
    日期：2022/9/7 15:27:4
	功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameAnimator : MonoBehaviour
{
    /// <summary>
    /// 序列帧
    /// </summary>
    public Texture2D[] Frames
    {
        get => frames;
        set => frames = value;
    }
    [SerializeField] private Texture2D[] frames = null;


    /// <summary>
    /// 帧率，为正时正向播放，为负时反向播放
    /// </summary>
    public float Framerate
    {
        get => framerate;
        set => framerate = value;
    }
    [SerializeField] private float framerate = 20.0f;

    /// <summary>
    /// 是否忽略timeScale
    /// </summary>
    public bool IgnoreTimeScale
    {
        get => ignoreTimeScale;
        set => ignoreTimeScale = value;
    }
    [SerializeField] private bool ignoreTimeScale = true;

    /// <summary>
    /// 是否循环
    /// </summary>
    public bool Loop
    {
        get => loop;
        set => loop = value;
    }
    [SerializeField] private bool loop = true;

    //动画曲线
    [SerializeField]
    private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1, 0, 0), new Keyframe(1, 1, 0, 0));

    /// <summary>
    /// 结束事件
    /// 在每次播放完一个周期时触发
    /// 在循环模式下触发此事件时，当前帧不一定为结束帧
    /// </summary>
    public event Action FinishEvent;

    //目标Image组件
    private RawImage image;

    //目标SpriteRenderer组件
    private SpriteRenderer spriteRenderer;

    //当前帧索引
    private int currentFrameIndex = 0;

    //下一次更新时间
    private float timer = 0.0f;

    //当前帧率，通过曲线计算而来
    private float currentFramerate = 20.0f;

    /// <summary>
    /// 重设动画
    /// </summary>
    public void Reset()
    {
        currentFrameIndex = framerate < 0 ? frames.Length - 1 : 0;
    }

    /// <summary>
    /// 从停止的位置播放动画
    /// </summary>
    public void Play()
    {
        this.enabled = true;
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    public void Pause()
    {
        this.enabled = false;
    }

    /// <summary>
    /// 停止动画，将位置设为初始位置
    /// </summary>
    public void Stop()
    {
        Pause();
        Reset();
    }

    /// <summary>
    /// 重播动画
    /// </summary>
    public void Restart()
    {
        Stop();
        Play();
    }


    //自动开启动画
    void Start()
    {
        image = this.GetComponent<RawImage>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
#if UNITY_EDITOR
        if (image == null && spriteRenderer == null)
        {
            Debug.LogWarning("No available component found. 'Image' or 'SpriteRenderer' required.", this.gameObject);
        }
#endif
    }

    void Update()
    {
        //帧数据无效，禁用脚本
        if (frames == null || frames.Length == 0)
        {
            this.enabled = false;
        }
        else
        {
            //从曲线值计算当前帧率
            float curveValue = curve.Evaluate((float)currentFrameIndex / frames.Length);
            float curvedFramerate = curveValue * framerate;
            //帧率有效
            if (curvedFramerate != 0)
            {
                //获取当前时间
                float time = ignoreTimeScale ? Time.unscaledTime : Time.time;
                //计算帧间隔时间
                float interval = Mathf.Abs(1.0f / curvedFramerate);
                //满足更新条件，执行更新操作
                if (time - timer > interval)
                {
                    //执行更新操作
                    DoUpdate();
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("Framerate got '0' value, animation stopped.");
            }
#endif
        }
    }

    //具体更新操作
    private void DoUpdate()
    {
        //计算新的索引
        int nextIndex = currentFrameIndex + (int)Mathf.Sign(currentFramerate);
        //索引越界，表示已经到结束帧
        if (nextIndex < 0 || nextIndex >= frames.Length)
        {
            //广播事件
            FinishEvent?.Invoke();

            //非循环模式，禁用脚本
            if (loop == false)
            {
                if (frames == null) return;
                currentFrameIndex = Mathf.Clamp(currentFrameIndex, 0, frames.Length - 1);
                this.enabled = false;
                return;
            }
        }

        //钳制索引
        currentFrameIndex = nextIndex % frames.Length;
        //更新图片
        if (image != null)
        {
            image.texture = frames[currentFrameIndex];
        }
        else if (spriteRenderer != null)
        {
            // spriteRenderer.sprite = frames[currentFrameIndex];
        }

        //设置计时器为当前时间
        timer = ignoreTimeScale ? Time.unscaledTime : Time.time;
    }

}
