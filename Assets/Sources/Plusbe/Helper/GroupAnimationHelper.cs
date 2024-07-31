using Plusbe.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AppCustom
{
    public class GroupAnimationHelper
    {
        public static void DoMoveIn(Transform transform, Vector3 endPosition, MoveFromType type, float time,float delay)
        {
            Vector3 startPosition = endPosition;

            if (type == MoveFromType.Left) startPosition.x -= 500;
            if (type == MoveFromType.Right) startPosition.x += 500;
            if (type == MoveFromType.Top) startPosition.y += 800;
            if (type == MoveFromType.Bottom) startPosition.y -= 800;

            transform.localPosition = startPosition;
            transform.DOLocalMove(endPosition, time).SetDelay(delay);

            AnimationHelper.UGUIAlphaAndChild(transform.gameObject, 0, 1, time, delay+0.1f);
        }

        public static void DoMoveInJiao(Transform transform, Vector3 endPosition, MoveFromType type, float time, float delay)
        {
            Vector3 startPosition = endPosition;

            if (type == MoveFromType.Left) startPosition.x -= 500;
            if (type == MoveFromType.Right) startPosition.x += 500;
            if (type == MoveFromType.Top) startPosition.y += 500;
            if (type == MoveFromType.Bottom) startPosition.y -= 500;

            transform.localPosition = startPosition;
            transform.DOLocalMove(endPosition, time).SetEase(Ease.OutQuint).SetDelay(delay);

            AnimationHelper.UGUIAlphaAndChild(transform.gameObject, 0, 1, time, delay);
        }
    }

    public enum MoveFromType
    {
        None,
        Left,
        Right,
        Top,
        Bottom
    }
}
