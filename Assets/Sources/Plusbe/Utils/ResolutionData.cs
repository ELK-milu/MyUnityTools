using Plusbe.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Utils
{
    public class ResolutionData
    {
        public int max;
        public int x;
        public int y;
        public int w;
        public int h;
        public bool full;
        public bool hideCursor;
        public MaxType type;
        public bool autoHide;
        public bool topMost;

        private static ResolutionData resolutionData;

        private ResolutionData() { }

        public static ResolutionData Instance
        {
            get
            {
                if (resolutionData == null)
                {
                    resolutionData = new ResolutionData();
                    resolutionData.x = 0;
                    resolutionData.y = 0;
                    resolutionData.w = 1920;
                    resolutionData.h = 1080;
                    resolutionData.full = false;
                    resolutionData.hideCursor = true;
                    resolutionData.autoHide = false;
                    resolutionData.topMost = true;
                }

                return resolutionData;
            }
        }

        public void Init()
        {
            this.max = AppConfig.Instance.GetValueByKeyInt("Max"); 
            this.type = CheckMaxType(max);

            if(type == MaxType.Max)
            {
                Init(AppConfig.Instance.GetValueByKeyInt("Max"), 0, 0, Display.displays[0].systemWidth, Display.displays[0].systemHeight, false, AppConfig.Instance.GetValueByKeyBool("HideCursor"), AppConfig.Instance.GetValueByKeyBool("AutoHide"), true);
            }else if(type == MaxType.Custom || type == MaxType.CustomReset)
            {
                Init(AppConfig.Instance.GetValueByKeyInt("Max"), AppConfig.Instance.GetValueByKeyInt("Left"), AppConfig.Instance.GetValueByKeyInt("Top"), AppConfig.Instance.GetValueByKeyInt("Width"), AppConfig.Instance.GetValueByKeyInt("Height"), false, AppConfig.Instance.GetValueByKeyBool("HideCursor"), AppConfig.Instance.GetValueByKeyBool("AutoHide"), AppConfig.Instance.GetValueByKeyBool("TopMost"));
            }
        }

        /// <summary>
        /// 配置坐标基本信息
        /// </summary>
        public void Init(int max, int x, int y, int w, int h, bool full, bool hideCursor,bool autoHide,bool topMost)
        {
            this.max = max;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.full = full;
            this.hideCursor = hideCursor;
            this.autoHide = autoHide;
            this.topMost = topMost;
        }

        public MaxType CheckMaxType(int type)
        {
            //int type = AppConfig.Instance.getValueByKeyInt("Max");
            if (type == 0)
            {
                return MaxType.Normal;
            }
            else if (type == 1)
            {
                return MaxType.Max;
            }
            else if (type == 2)
            {
                return MaxType.Custom;
            }
            else if (type == 3)
            {
                return MaxType.CustomReset;
            }

            return MaxType.Normal;
        }
    }

    public enum MaxType
    {
        Normal,
        Max,
        Custom,
        CustomReset
    }
}
