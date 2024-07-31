using RenderHeads.Media.AVProVideo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Video;

namespace Plusbe.Video
{
    /// <summary>
    /// 当部分视频播放不了的情况下，可以尝试设置 Options windows >>  video api >> direct show
    /// </summary>
    public class AVProVideoPlayer : MonoBehaviour, IVideoPlayer
    {

        public MediaPlayer _mediaPlayer;
        public MediaPlayer _mediaPlayerB;
        public DisplayUGUI _mediaDisplay;

        public float CurrentTimeMs { get; set; }
        public float DurationMs { get; set; }


        private DisplayUGUI displayUGUI;

        private MediaPlayer _loadingPlayer;

        private MediaPathType _location = MediaPathType.AbsolutePathOrURL;

        public bool autoPlay = true;
        public bool AutoPlay
        {
            get
            {
                return autoPlay;
            }
            set
            {
                autoPlay = value;
            }
        }

        public bool loop;
        public bool Loop
        {
            get
            {
                return loop;
            }

            set
            {
                loop = value;
            }
        }

        

        public void Init()
        {
            displayUGUI = gameObject.GetComponent<DisplayUGUI>();

            _loadingPlayer = _mediaPlayerB;
            if (PlayingPlayer)
            {
                PlayingPlayer.Events.AddListener(OnVideoEvent);
                PlayingPlayer.Loop = Loop;

                if (LoadingPlayer)
                {
                    LoadingPlayer.Events.AddListener(OnVideoEvent);
                    LoadingPlayer.Loop = Loop;
                }
            }
            //SetLoop(loop);
        }

        public void AddVideoEndEVent(Action onVideoEnd)
        {
            this.onVideoEnd = onVideoEnd;
        }

        void Update()
        {
            if (PlayingPlayer && PlayingPlayer.Info != null && PlayingPlayer.Info.GetDuration() > 0f)
            {
                //float time = PlayingPlayer.Control.GetCurrentTimeMs();
                //float duration = PlayingPlayer.Info.GetDurationMs();
                //float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);

                //进度展示控件
                // Debug.Log(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));
            }
        }


        public MediaPlayer PlayingPlayer
        {
            get
            {
                if (LoadingPlayer == _mediaPlayer)
                {
                    return _mediaPlayerB;
                }
                return _mediaPlayer;
            }
        }

        public MediaPlayer LoadingPlayer
        {
            get
            {
                return _loadingPlayer;
            }
        }

        

        private void SwapPlayers()
        {
            // Pause the previously playing video
            PlayingPlayer.Control.Pause();

            // Swap the videos
            if (LoadingPlayer == _mediaPlayer)
            {
                _loadingPlayer = _mediaPlayerB;
            }
            else
            {
                _loadingPlayer = _mediaPlayer;
            }

            _mediaDisplay.CurrentMediaPlayer = PlayingPlayer;
        }

        #region 事件

        private float lastChangePathTime;

        

        // Callback function to handle events
        public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
        {
            switch (et)
            {
                case MediaPlayerEvent.EventType.ReadyToPlay:
                    break;
                case MediaPlayerEvent.EventType.Started:
                    break;
                case MediaPlayerEvent.EventType.FirstFrameReady:
                    //SwapPlayers();
                    OnFirstFrameReadyEvent();
                    break;
                case MediaPlayerEvent.EventType.FinishedPlaying:                    
                    OnVideoEndEvent();
                    print("视频播放完成...");
                    break;
                case MediaPlayerEvent.EventType.FinishedSeeking:
                    OnFinishedSeekingEvent();
                    break;
            }

            Debug.Log("Event: " + mp.name + "," + et.ToString());
        }

        /// <summary>
        /// 常规事件分析
        /// 1.循环事件，当loop==true，不抛出视频结束事件
        /// 2.
        /// 
        /// xx，滑轨说明，1.切换视频不黑，2.能够完成切换完成后跳转指定位置不闪
        /// </summary>

        

        public Action onVideoEnd;
        public Action onVideoFinishSeeking;
        public Action onVideoFirstFrameReady;

        private void OnVideoEndEvent()
        {
            onVideoEnd?.Invoke();
        }

        private void OnFirstFrameReadyEvent()
        {
            if (onVideoFirstFrameReady != null)
            {
                onVideoFirstFrameReady();
            }
            else
            {
                SwapPlayers();
            }
        }

        private void OnFinishedSeekingEvent()
        {
            onVideoFinishSeeking?.Invoke();
        }

        private System.Collections.IEnumerator TrySwapPlayers()
        {
            yield return new WaitForEndOfFrame();
            SwapPlayers();
        }

        public void ChangePlayer()
        {
            StartCoroutine(TrySwapPlayers());
        }


        #endregion

        #region 播放器控制及参数

        public void OpenVideo(string path)
        {
            LoadingPlayer.m_VideoPath = path;

            if (string.IsNullOrEmpty(LoadingPlayer.m_VideoPath))
            {
                LoadingPlayer.CloseMedia();
            }
            else
            {
                LoadingPlayer.OpenMedia(_location, LoadingPlayer.m_VideoPath, AutoPlay);
            }
        }

        public void Stop()
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.CloseMedia();
            }

            if (LoadingPlayer)
            {
                LoadingPlayer.CloseMedia();
            }

            //displayUGUI._mediaPlayer = null;
        }

        public void Resume()
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.Play();
            }
        }

        public void Pause()
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.Pause();
            }
        }

        public void Rewind()
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.Rewind();
                PlayingPlayer.Control.Play();
            }
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        public void Seek(float timeMs)
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.Seek(timeMs);
            }
        }

        public void SetLoop(bool loop)
        {
            Loop = loop;
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.SetLooping(loop);
                PlayingPlayer.Loop = loop;
            }
            if (LoadingPlayer)
            {
                LoadingPlayer.Control.SetLooping(loop);
                LoadingPlayer.Loop = loop;
            }
        }

        public void SetRate(float rate)
        {
            if (PlayingPlayer)
            {
                PlayingPlayer.Control.SetPlaybackRate(rate);
                PlayingPlayer.PlaybackRate = rate;
            }
            if (LoadingPlayer)
            {
                LoadingPlayer.Control.SetPlaybackRate(rate);
                PlayingPlayer.PlaybackRate = rate;
            }

        }

        public float GetCurrentTimeMs()
        {
            if (PlayingPlayer && PlayingPlayer.Info != null)
            {
                return (float)PlayingPlayer.Control.GetCurrentTime();
            }

            return 0;
        }

        public float GetCurrentTime()
        {
            return GetCurrentTimeMs() * 0.001f;
        }

        public float GetDurationMs()
        {
            if (PlayingPlayer && PlayingPlayer.Info != null && PlayingPlayer.Info.GetDuration() > 0f)
            {
                return (float)PlayingPlayer.Info.GetDuration();
            }

            return 0;
        }

        public float GetDuration()
        {
            return GetDurationMs() * 0.001f;
        }

        #endregion

        #region 测试

        /// <summary>
        /// 预播放视频完成跳转指定位置
        /// </summary>
        /// <param name="timeMs"></param>
        public void SeekTemp(float timeMs)
        {
            if (LoadingPlayer)
            {
                LoadingPlayer.Control.Seek(timeMs);
            }
        }

        /// <summary>
        /// 播放与暂停切换
        /// </summary>
        public void ResumeOrPause()
        {
            if (PlayingPlayer)
            {
                if (PlayingPlayer.Control.IsPlaying())
                {
                    PlayingPlayer.Control.Pause();
                }
                else if (PlayingPlayer.Control.IsPaused())
                {
                    PlayingPlayer.Control.Play();
                }
            }
        }

        /// <summary>
        /// 打开或播放或暂停视频
        /// </summary>
        /// <param name="path"></param>
        public void OpenOrResumeOrPause(string path)
        {
            if (PlayingPlayer)
            {
                if (string.IsNullOrEmpty(PlayingPlayer.m_VideoPath) || path != PlayingPlayer.m_VideoPath)
                {
                    OpenVideo(path);
                } else
                {
                    ResumeOrPause();
                }
            }
        }

        

        #endregion

    }
}
