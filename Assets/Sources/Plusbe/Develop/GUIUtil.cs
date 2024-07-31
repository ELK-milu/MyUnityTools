using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Develop
{
    public class GUIUtil
    {
        static bool s_isInit = false;

        static int s_fontSize = 40;

        public static int FontSize
        {
            get { return GUIUtil.s_fontSize; }
            set { GUIUtil.s_fontSize = value; }
        }

        public static void SetGUIStyle()
        {
            if (!s_isInit)
            {
                s_isInit = true;

                //s_fontSize = (int)(Screen.dpi * 0.13f);

                //GUI.backgroundColor = Color.red;//设置背景颜色
                //GUI.color = Color.yellow;//设置颜色
                //GUI.contentColor = Color.black;

                GUI.skin.label.normal.textColor = Color.red;

                GUI.skin.label.fontSize = s_fontSize;
                //GUI.skin.button.fixedHeight = 0;
                GUI.skin.button.fontSize = s_fontSize;

                GUI.skin.verticalScrollbar.fixedWidth = s_fontSize;
                GUI.skin.verticalScrollbarUpButton.fixedWidth = s_fontSize;
                GUI.skin.verticalScrollbarThumb.fixedWidth = s_fontSize;

                GUI.skin.horizontalScrollbar.fixedHeight = s_fontSize;
                GUI.skin.horizontalScrollbarLeftButton.fixedHeight = s_fontSize;
                GUI.skin.horizontalScrollbarThumb.fixedHeight = s_fontSize;

                GUI.skin.toggle.fontSize = s_fontSize;
                GUI.skin.textField.fontSize = s_fontSize;
                GUI.skin.textField.wordWrap = true;
            }
        }
    }
}
