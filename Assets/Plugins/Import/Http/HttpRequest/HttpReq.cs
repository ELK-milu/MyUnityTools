using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace PlusbeQuickPlugin.HttpRequest
{
    public class HttpReq : MonoBehaviour
    {
        static HttpReq singleton;
        static bool inited = false;
        static void Init()
        {
            if (inited) return;
            inited = true;

            GameObject go = new GameObject("HttpRequest");
            singleton = go.AddComponent<HttpReq>();
            DontDestroyOnLoad(go);
        }


        /// <summary>
        /// 获取Get接口内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ac"></param>
        public static void GetAPIResponse(string url, HandleResponseFunc ac)
        {
            Init();
            singleton.Get(url, ac);
        }

        /// <summary>
        /// 获取post接口内容-body->form
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <param name="ac"></param>
        public static void PostAPIResponse(string url, WWWForm form, HandleResponseFunc ac)
        {
            Init();
            singleton.Post(url, form, ac);
        }

        /// <summary>
        /// 获取post接口内容-body->raw
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyraw"></param>
        /// <param name="ac"></param>
        public static void PostAPIResponse(string url, string bodyraw, HandleResponseFunc ac)
        {
            Init();
            singleton.Post(url, bodyraw, ac);
        }

        void Post(string url, string bodyraw, HandleResponseFunc action)
        {
            dict.Add(count, action);
            StartCoroutine(PostURLRes(url, count, bodyraw));
            count++;
        }

        void Post(string url, WWWForm form, HandleResponseFunc action)
        {
            dict.Add(count, action);
            StartCoroutine(PostURLRes(url, count, form));
            count++;
        }

        IEnumerator PostURLRes(string url, int id, string bodyraw)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, bodyraw);
            uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyraw));
            uwr.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            uwr.timeout = 3;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                if (dict.TryGetValue(id, out HandleResponseFunc ac))
                {
                    print($"ID: {id} post请求失败 url: {url}");
                    ac.Invoke(false, uwr.downloadHandler.text);
                    dict.Remove(id);
                }
            }
            else
            {
                if (dict.TryGetValue(id, out HandleResponseFunc ac))
                {
                    print($"ID: {id} post请求成功 url: {url}");
                    ac.Invoke(true, uwr.downloadHandler.text);
                    dict.Remove(id);
                }
            }
        }

        IEnumerator PostURLRes(string url, int id, WWWForm form)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.timeout = 3;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                if (dict.TryGetValue(id, out HandleResponseFunc ac))
                {
                    print($"ID: {id} post请求失败 url: {url}");
                    ac.Invoke(false, uwr.downloadHandler.text);
                    dict.Remove(id);
                }
            }
            else
            {
                if (dict.TryGetValue(id, out HandleResponseFunc ac))
                {
                    print($"ID: {id} post请求成功 url: {url}");
                    ac.Invoke(true, uwr.downloadHandler.text);
                    dict.Remove(id);
                }
            }
        }


        public delegate void HandleResponseFunc(bool successed, string responseStr);
        int count = 0;
        Dictionary<int, HandleResponseFunc> dict = new Dictionary<int, HandleResponseFunc>();
        void Get(string url, HandleResponseFunc action)
        {
            dict.Add(count, action);
            StartCoroutine(GetURLRes(url, count));
            count++;
        }

        IEnumerator GetURLRes(string url, int id)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.timeout = 3;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                if (dict.TryGetValue(id, out HandleResponseFunc ac))
                {
                    print($"ID: {id} 请求失败 url: {url}");
                    string res = uwr.downloadHandler.text.Replace("\ufeff", "");
                    ac.Invoke(false, res);
                    dict.Remove(id);
                }
            }
            else
            {
                if (dict.TryGetValue(id, out HandleResponseFunc ac))
                {
                    print($"ID: {id} 请求成功 url: {url}");
                    string res = uwr.downloadHandler.text.Replace("\ufeff", "");
                    ac.Invoke(true, res);
                    dict.Remove(id);
                }
            }
        }

    }
}

