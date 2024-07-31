
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlusbeQuickPlugin.HttpService
{
    /// <summary>
    /// 根据键值数组返回对应的http内容
    /// </summary>
    /// <param name="keyValues"></param>
    /// <returns></returns>
    public delegate string GetHttpResponseByKeyValues(string[] keyValues);
    public delegate void ExecuteMethodByHttpKeyValues(string[] keyValues);

    /// <summary>
    /// 需要程序返回内容的http指令
    /// </summary>
    
    public class HttpResponseCommand
    {
        public string CommandName;//命令名称
        public bool Enable = true;//备用
        [NonSerialized]
        public GetHttpResponseByKeyValues getResponseFun;//返回值
        [NonSerialized]
        public GameObject DependencyObj;//依赖的对象，没有的话就不执行命令
        public string[] requiredKeys;//必要的键
        public string[] requiredValues;//对应的必要键值
        public string[] variableKeys;//值会发生变化的键
        [NonSerialized]
        public StandardHttpService parent;

        /// <summary>
        /// 检查是否需要做出回应
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="responseTxt"></param>
        /// <returns></returns>
        public bool CheckResponse(NameValueCollection datas, out string responseTxt)
        {
            responseTxt = "";

            if (DependencyObj == null)//如果依赖obj消失则移除此条命令
            {
                parent.RemoveResponseCommand(this);
                return false;
            }

            if (Enable == false) return false;
            if (requiredKeys.Length < 1) return false;


            for (int i = 0; i < requiredKeys.Length; i++)
            {
                string key = requiredKeys[i];
                if (key == "") continue;//如果为空则跳过该键进入下一个键的判断
                var value = datas[key];
                if (string.IsNullOrEmpty(value)) return false;
                if (value != requiredValues[i]) return false;
            }

            
            responseTxt = getResponseFun(GetURLKeyValues(datas, variableKeys));
            return true;
        }


        /// <summary>
        /// 获取对应的httpurl的键值数组
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] GetURLKeyValues(NameValueCollection datas, string[] keys)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < keys.Length; i++)
            {
                string value = datas[keys[i]] ?? "";
                value = UnityWebRequest.UnEscapeURL(value);//中文url解码
                list.Add(value);
            }
            return list.ToArray();
        }

    }

    /// <summary>
    /// 调用程序方法的http指令
    /// </summary>
    
    public class HttpCallCommand
    {
        
        public string CommandName;//命令名称
        public bool Enable = true;//备用
        [NonSerialized]
        public GameObject DependencyObj;//依赖的对象，没有的话就不执行命令
        public string[] requiredKeys;//必要的键
        public string[] requiredValues;//对应的必要键值
        public string[] variableKeys;//值会发生变化的键
        [NonSerialized]
        public ExecuteMethodByHttpKeyValues callFun;
        [NonSerialized]
        public StandardHttpService parent;

        public bool CheckCallFun(NameValueCollection datas)
        {
            if (DependencyObj == null)//如果依赖obj消失则移除此条命令
            {
                parent.RemoveCallCommand(this);
                return false;
            }
            if (Enable == false) return false;
            if (requiredKeys.Length < 1) return false;

            //匹配必须满足的键
            for (int i = 0; i < requiredKeys.Length; i++)
            {
                string key = requiredKeys[i];
                //Debug.Log("key: " + key);
                if (key == "") continue;//如果为空则跳过该键进入下一个键的判断
                var value = datas[key];
                if (string.IsNullOrEmpty(value)) return false;
                if (value != requiredValues[i]) return false;
            }

            Loom.QueueOnMainThread(() =>
            {
                callFun(GetURLKeyValues(datas, variableKeys));
            });

            return true;
        }

        /// <summary>
        /// 获取对应的httpurl的键值数组
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] GetURLKeyValues(NameValueCollection datas, string[] keys)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < keys.Length; i++)
            {
                string value = datas[keys[i]] ?? "";
                value = UnityWebRequest.UnEscapeURL(value);//中文url解码
                list.Add(value);
            }
            return list.ToArray();
        }

    }

    /// <summary>
    /// 默认标准http服务，接收执行url命令，返回指定值
    /// </summary>
    public class StandardHttpService:IHttpService
    {
        private List<HttpResponseCommand> responseList = new List<HttpResponseCommand>();//http响应命令列表
        private List<HttpCallCommand> callList { get; set; } = new List<HttpCallCommand>();//http调用命令列表

        public int ServicePort { get; set; }
        public string ServiceName { get { return "标准Http交互服务"; }}

        public StandardHttpService(int port)
        {
            ServicePort = port;

            //注册默认内置命令
            RegisterResponseCommand("获取API开放接口列表", "get", "apiList", "", ResponseAPIList);
        }

        //public void RegisterHideShowCommand()
        //{
        //    RegisterCallCommand("显示客户端", "act,states", "unity,max", "", CallShowClient);
        //    RegisterCallCommand("隐藏客户端", "act,states", "unity,min", "", CallHideClient);
        //}

        //private void CallHideClient(string[] keyValues)
        //{
        //    IntPtr hwnd = API.WinApiUtils.API_FindWindow(null, Application.productName);
        //    API.WinApiUtils.API_ShowWindow(hwnd, API.WinApiUtils.NCmdShow.SW_MINIMIZE);
        //}

        //private void CallShowClient(string[] keyValues)
        //{
        //    IntPtr hwnd = API.WinApiUtils.API_FindWindow(null, Application.productName);
        //    API.WinApiUtils.API_ShowWindow(hwnd, API.WinApiUtils.NCmdShow.SW_RESTORE);
        //}

        private string ResponseAPIList(string[] keyValues)
        {
            return GetCommandJson();
        }


        /// <summary>
        /// 处理http命令
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public string HandleHttpCommand(string rawUrl)
        {
            Debug.Log("rawUrl: " + rawUrl);
            NameValueCollection datas = ConvertToHttpNameValueCollection(rawUrl);
            //处理返回信息的http命令
            string response = "true";
            for (int i = responseList.Count - 1; i >= 0; i--)
            {
                var responseCommand = responseList[i];
                if (responseCommand.CheckResponse(datas, out string responseTxt))
                {
                    response = responseTxt;
                    if (responseTxt != "") break;
                }
            }

            //处理调用方法的http命令
            for (int i = callList.Count - 1; i >= 0; i--)
            {
                var callCommand = callList[i];
                callCommand.CheckCallFun(datas);
            }

            return response;
        }

        /// <summary>
        /// 转换一个http请求路径为NameValueCollection对象
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        NameValueCollection ConvertToHttpNameValueCollection(string url)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            int startIndex = url.IndexOf('?');
            string input = url.Substring(startIndex);
            string pattern = @"(\?|&)(\w+)=([^&]+)?";//正则表达式匹配key=value的形式,每个括号是一个分组1，2，3
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            MatchCollection matchCollection = regex.Matches(input);
            IEnumerator enumerator = matchCollection.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Match match = (Match)enumerator.Current;
                    //将分组2作为key，将分组3作为value
                    string key = match.Result("$2").ToLower();
                    string value = match.Result("$3");
                    nameValueCollection.Add(key, value);
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = enumerator as IDisposable) != null)
                {
                    disposable.Dispose();
                }
            }
            return nameValueCollection;
        }
        void AddResponseCommand(HttpResponseCommand command)
        {
            if (responseList.Contains(command) == false)
            {
                command.parent = this;
                responseList.Add(command);
                Debug.Log("http返回命令:" + command.CommandName + "注册成功");
            }
        }
        void AddCallCommand(HttpCallCommand command)
        {
            if (callList.Contains(command) == false)
            {
                command.parent = this;
                callList.Add(command);
                Debug.Log("http调用命令:" + command.CommandName + "注册成功");
            }
        }
        public void RemoveResponseCommand(HttpResponseCommand command)
        {
            if (responseList.Contains(command))
            {
                responseList.Remove(command);
            }
        }
        public void RemoveCallCommand(HttpCallCommand command)
        {
            if (callList.Contains(command))
            {
                callList.Remove(command);
            }
        }

        #region 注册http命令相关
        /// <summary>
        /// 注册返回内容的http指令
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="requiredKeys">必要的键数组ex:act,object,states</param>
        /// <param name="requiredValues">对应的值ex: unity,getVolume,1</param>
        /// <param name="variableKeys">会发生变化的key键，可以没有ex type,day,r</param>
        /// <param name="uac">返回内容的委托方法</param>
        /// <param name="dependencyObj">绑定的依赖对象，对象消失后http指令也会作废</param>
        public void RegisterResponseCommand(string taskName, string requiredKeys, string requiredValues, string variableKeys, GetHttpResponseByKeyValues uac, GameObject dependencyObj = null)
        {
            HttpResponseCommand command = new HttpResponseCommand
            {
                CommandName = taskName,
                requiredKeys = requiredKeys.Split(','),
                requiredValues = requiredValues.Split(','),
                variableKeys = variableKeys.Split(','),
                getResponseFun = uac,
                DependencyObj = dependencyObj ?? HttpServiceManager.HttpObject
            };
            AddResponseCommand(command);
        }

        /// <summary>
        /// 注册方法调用的http方法
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="requiredKeys">必要的键数组ex:act,object,states</param>
        /// <param name="requiredValues">对应的值ex: unity,getVolume,1</param>
        /// <param name="variableKeys">会发生变化的key键，可以没有ex type,day,r</param>
        /// <param name="uac">调用方法的委托方法</param>
        /// <param name="dependencyObj">绑定的依赖对象，对象消失后http指令也会作废</param>
        public void RegisterCallCommand(string taskName, string requiredKeys, string requiredValues, string variableKeys, ExecuteMethodByHttpKeyValues uac, GameObject dependencyObj = null)
        {
            HttpCallCommand command = new HttpCallCommand
            {
                CommandName = taskName,
                requiredKeys = requiredKeys.Split(','),
                requiredValues = requiredValues.Split(','),
                variableKeys = variableKeys.Split(','),
                callFun = uac,
                DependencyObj = dependencyObj ?? HttpServiceManager.HttpObject
            };
            AddCallCommand(command);
        }

        public delegate void ExecuteFunByStatesString(string states);
        /// <summary>
        /// !!!!!!新项目请使用RegisterCommand!!!!!!!!!注册act=unity,object=自定义,states作为参数的方法调用
        /// </summary>
        /// <param name="taskName">任务描述</param>
        /// <param name="obj">想侦听的object值</param>
        /// <param name="fun">调用的方法</param>
        /// <param name="dependencyObj">绑定依赖对象</param>
        /// <param name="act">默认侦听act=unity</param>
        public void RegisterObjFunCallCommand(string taskName, string obj, ExecuteFunByStatesString fun, GameObject dependencyObj = null, string act = "unity")
        {
            RegisterCallCommand(taskName, "act,object", $"{act},{obj}", "states", (string[] keyValues) => {
                fun(keyValues[0]);
            }, dependencyObj);
        }
        /// <summary>
        /// 注册规范后的http指令
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="states"></param>
        /// <param name="fun"></param>
        /// <param name="dependencyObj"></param>
        /// <param name="act"></param>
        public void RegisterCommand(string taskName, string states, ExecuteFunByStatesString fun, GameObject dependencyObj = null, string act = "unity")
        {
            RegisterCallCommand(taskName, "act,states", $"{act},{states}", "object", (string[] keyValues) => {
                fun(keyValues[0]);
            }, dependencyObj);
        }

        #endregion 注册http命令相关

        public string GetCommandJson()
        {
            Debug.Log(callList.Count);
            JObject jo = new JObject();
            jo.Add("callCommand", JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(callList)));
            jo.Add("responseCommand", JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(responseList)));
            return jo.ToString();
        }
    }
}

