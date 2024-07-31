using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Plusbe.Net
{

    /// <summary>
    /// https://www.jianshu.com/p/3da9191f82a0
    /// </summary>
    public class UnityWebRequestHelper : MonoBehaviour
    {
        //[Obsolete("测试用例")]
        //public Coroutine Get(string url, Action<UnityWebRequest> actionResult)
        //{
        //    return StartCoroutine(GetData(url, actionResult));
        //}

        //[Obsolete("测试用例")]
        //public Coroutine GetTexture(string url, Action<Texture2D> actionResult)
        //{
        //    return StartCoroutine(GetTextureData(url, actionResult));
        //}

        /// <summary>
        /// 无返回
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IEnumerator GetData(string url)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();
            }
        }

        /// <summary>
        /// 通用返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static IEnumerator GetData(string url, Action<UnityWebRequest> actionResult)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();
                actionResult?.Invoke(uwr);
            }
        }

        /// <summary>
        /// 字符串数据返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionResult">data,url</param>
        /// <returns></returns>
        public static IEnumerator GetData(string url, Action<string, string> actionResult)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.isHttpError)
                {
                    actionResult?.Invoke("", url);
                }
                else
                {
                    actionResult?.Invoke(Encoding.UTF8.GetString(uwr.downloadHandler.data), url);
                }
            }
        }

        /// <summary>
        /// 图片返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static IEnumerator GetTextureData(string url, Action<Texture2D> actionResult)
        {
            yield return GetTextureData(url, actionResult, null);
        }

        /// <summary>
        /// 图片返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static IEnumerator GetTextureData(string url, Action<Texture2D, string> actionResult)
        {
            yield return GetTextureData(url, null, actionResult);
        }

        private static IEnumerator GetTextureData(string url, Action<Texture2D> action1, Action<Texture2D, string> action2)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;
            yield return uwr.SendWebRequest();
            Texture2D t = null;
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                t = downloadTexture.texture;
            }

            action1?.Invoke(t);

            action2?.Invoke(t, url);
        }

    }
}
