using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Plusbe.Helper
{
    public class AnimationHelper
    {
        public static void DoScaleIn(Transform transform)
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.zero;
            //transform.localPosition = Vector3.zero;
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack);
        }

        public static void DoScaleIn(Transform transform, Vector3 postion)
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.zero;
            transform.localPosition = postion;
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack);
        }

        public static void DoScaleBigOut(Transform transform, Action callBack)
        {
            DOTween.Kill(transform);
            //Ease.InExpo
            transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (callBack != null) callBack();
            });
        }

        public static void DoScaleOut(Transform transform, Action callBack)
        {
            DOTween.Kill(transform);
            transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (callBack != null) callBack();
            });
        }

        public static void DoMoveOut(Transform transform, Action callBack)
        {
            DOTween.Kill(transform);
            transform.DOLocalMoveX(-1920, 0.5f).OnComplete(() =>
            {
                if (callBack != null) callBack();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <param name="from">0-1</param>
        /// <param name="to">0-1</param>
        /// <param name="time"></param>
        public static void UGUIAlpha(GameObject go, float? from, float to, float time)
        {
            Graphic graphics = go.GetComponent<Graphic>();
            Color fromColor = graphics.color;
            fromColor.a = from ?? fromColor.a;

            graphics.color = fromColor;

            Color toColor = fromColor;
            toColor.a = to;

            graphics.DOColor(toColor, time);
        }

        public static void UGUIAlphaAndChild(GameObject go, float? from, float to, float time, float delay = 0)
        {
            UGUIAlpha(go.GetComponent<Graphic>(), from, to, time, delay);

            Graphic[] graphicss = go.GetComponentsInChildren<Graphic>();
            for (int i = 0; i < graphicss.Length; i++)
            {
                UGUIAlpha(graphicss[i], from, to, time, delay);
            }
        }

        private static void UGUIAlpha(Graphic graphics, float? from, float to, float time, float delay = 0)
        {
            if (graphics != null)
            {
                Color fromColor = graphics.color;
                fromColor.a = from ?? fromColor.a;

                graphics.color = fromColor;

                Color toColor = fromColor;
                toColor.a = to;

                graphics.DOColor(toColor, time).SetDelay(delay);
            }
        }

        public static void DoMoveIn(Transform transform, int offX, int offY, int offZ, Vector3 endPosition, float time, float delay = 0)
        {
            Vector3 startPosition = new Vector3(endPosition.x + offX, endPosition.y + offY, endPosition.z + offZ);

            transform.localPosition = startPosition;

            transform.DOLocalMove(endPosition, time).SetDelay(delay);
        }

    }
}
