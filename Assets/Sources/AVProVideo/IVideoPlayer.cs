using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Video
{
    public interface IVideoPlayer
    {

        void Init();

        /// <summary>
        /// 播放视频
        /// </summary>
        void OpenVideo(string path);

        /// <summary>
        /// 播放
        /// </summary>
        void Resume();

        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();

        /// <summary>
        /// 重播
        /// </summary>
        void Rewind();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 释放
        /// </summary>
        void Release();

        /// <summary>
        /// 设置视频进度
        /// </summary>
        /// <param name="timeMs"></param>
        void Seek(float timeMs);

        /// <summary>
        /// 设置播放速率
        /// </summary>
        void SetRate(float rate);

        /// <summary>
        /// 设置循环模式
        /// </summary>
        void SetLoop(bool loop);

        /// <summary>
        /// 视频当前进度，毫米
        /// </summary>
        float GetCurrentTimeMs();

        /// <summary>
        /// 视频当前进度，秒
        /// </summary>
        float GetCurrentTime();

        /// <summary>
        /// 视频长度，毫秒
        /// </summary>
        float GetDurationMs();

        /// <summary>
        /// 视频长度，秒
        /// </summary>
        float GetDuration();

        /// <summary>
        /// 是否自动播放
        /// </summary>
        bool AutoPlay { get; set; }
        bool Loop { get; set; }
    }
}
