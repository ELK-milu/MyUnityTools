using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Plusbe.Develop;

namespace Plusbe.Develop
{
    /// <summary>
    /// 帧率计算器
    /// </summary>
    public class FPSCounterHelper
    {
        private static FPSCounterHelper instance;

        // 帧率计算频率
        private const float calcRate = 0.5f;
        // 本次计算频率下帧数
        private int frameCount = 0;
        // 频率时长
        private float rateDuration = 0f;
        // 显示帧率
        private int fps = 0;

        public static void Init()
        {
            if (instance == null)
            {
                instance = new FPSCounterHelper();
                instance.OnCreate();
            }
        }

        private void OnCreate()
        {
            this.frameCount = 0;
            this.rateDuration = 0f;
            this.fps = 0;

            GUIConsoleManager.onUpdateCallback += OnUpdate;
            GUIConsoleManager.onGUICallback += OnGUI;
        }

        void OnUpdate()
        {
            ++this.frameCount;
            this.rateDuration += Time.unscaledDeltaTime;
            if (this.rateDuration > calcRate)
            {
                // 计算帧率
                this.fps = (int)(this.frameCount / this.rateDuration);
                this.frameCount = 0;
                this.rateDuration = 0f;
            }
        }

        void OnGUI()
        {
            GUILayout.Label("FPS：" + fps.ToString());
        }
    }
}
