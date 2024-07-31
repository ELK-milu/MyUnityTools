using Plusbe.AppManager;
using Plusbe.Message;
using Plusbe.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AppCustom
{
    public class PlusbeCommandCenter : MonoBehaviour
    {
        private static PlusbeCommandCenter instance;




        #region 初始化

        public static void Init()
        {
            instance = ApplicationManager.Instance.gameObject.AddComponent<PlusbeCommandCenter>();

            instance.AddListener();
            instance.InitSendMessage();
        }

        private void AddListener()
        {
            //NotificationCenter.Instance.AddObserver(this, "SendMessageUnity");

            NotificationCenter.Instance.AddObserver(this, "UnityCommand");
        }

        private void RemoveListener()
        {
            NotificationCenter.Instance.RemoveObserver(this, "UnityCommand");
        }

        #endregion

        #region unity 消息发送

        public static void SendMessage(object data)
        {
            NotificationCenter.Instance.PostNotification(null, "SendMessageUnity", data);
        }

        private void InitSendMessage()
        {
            NotificationCenter.Instance.AddObserver(this, "SendMessageUnity");
        }

        public void SendMessageUnity(Notification notification)
        {
            if (notification != null)
                receiveMessage?.Invoke(notification.data);
        }
        public static PlusbeReceiveMessage receiveMessage;

        public delegate void PlusbeReceiveMessage(object data);

        #endregion
        #region 界面跳转逻辑

        /// <summary>
        /// 进入欢迎词
        /// </summary>
        private static void EnterWelcome()
        {
            //ApplicationStatusManager.EnterStatus<PlusbeWelcomeStatus>();
        }

        /// <summary>
        /// 进入视频点播
        /// </summary>
        private static void EnterVideo()
        {
            //ApplicationStatusManager.EnterStatus<PlusbeVideopStatus>();
        }

        #endregion

        public void UnityCommand(Notification notifycation)
        {
            ScreenManager.UpdateGameTime();

            if (notifycation.data != null)
            {
                NameValueCollection datas = MessageSerializerHelper.ParseHttpUrl(notifycation.data.ToString());

                if (datas["act"] == "welcome")
                {
                    AddWelcome(datas["act"], datas["t"], datas["id"], datas["title"], datas["size"]);
                }
                else if (datas["act"] == "movie" || datas["act"] == "unity")
                {
                    Add(datas["act"], datas["states"], datas["object"]);
                }
            }
        }

        /// <summary>
        /// 处理中心 
        /// </summary>
        /// <returns></returns>
        public static string Add(string act, string states, string obj, string param4, string param5)
        {
            FiveCallBack(act, states, obj, param4, param5);

            string result = "";

            //StandardReturnJsonInformation.Instance.VerifyBackMessage(act, obj, states, out result);

            if (act == "welcome")
            {
                //增删改查 列表
                //欢迎词
                //类型-t-states
                //id-obj
                //title-param4
                //size-param5
                switch (states)
                {
                    case "add": WelcomeAdd(param4, param5); break;
                    case "delete": WelcomeDelete(obj); break;
                    case "update": WelcomeUpdate(obj, param4, param5); break;
                    case "getxml": result = WelcomeJson(); break; //有返回值 需提前处理
                    case "play": WelcomePlay(obj); break;
                    case "back": WelcomeBack(); break;
                    default: break;
                }
            }
            else if (act == "movie")
            {
                switch (states)
                {
                    case "0": Stop(); break; //
                    case "1": Play(); break; //
                    case "2": Pause(); break; //
                    case "3": Replay(); break; //
                    case "4": break; //
                    case "5": break; //
                    case "6": break; //
                    case "7": PrevPage(); break; //
                    case "8": NextPage(); break; //
                    case "9": break; //
                    case "10": break; //
                    case "11": ShowMin(); break; //
                    case "12": ShowMax(); break; //
                    case "13": Open(obj); break; //
                    case "14": /*result = GetJson(obj);*/ break; //有返回值需要在顶级跳进进行处理
                    case "15": break; //
                    case "16": break; //
                    case "17": break; //
                    case "18": break; //
                    case "19": break; //
                    case "20": break; //
                    case "21": break; //
                    case "22": break; //
                    case "23": break; //
                    default: break;
                }
            }
            else if (act == "unity")
            {
                switch (states)
                {
                    case "0": Stop(); break; //
                    case "1": Play(); break; //
                    case "2": Pause(); break; //
                    case "3": Replay(); break; //
                    case "4": break; //
                    case "5": break; //
                    case "6": break; //
                    case "7": PrevPage(); break; //
                    case "8": NextPage(); break; //
                    case "9": break; //
                    case "10": break; //
                    case "11": ShowMin(); break; //
                    case "12": ShowMax(); break; //
                    case "13": Open(obj); break; //
                    case "14": result = GetJson(obj); break; //有返回值需要在顶级跳进进行处理
                    case "15": break; //
                    case "16": break; //
                    case "17": break; //
                    case "18": break; //
                    case "19": break; //
                    case "20": break; //
                    case "21": break; //
                    case "22": break; //
                    case "23": break; //
                    default: break;
                }
            }

            return result;
        }

        #region 5参数回调

        public static PlusbeCommandFiveCallBack s_five_callBack;

        private static void FiveCallBack(string act, string states, string obj, string param4, string param5)
        {
            s_five_callBack?.Invoke(act, states, obj, param4, param5);
        }

        #endregion

        #region 视频点播

        public static PlusbeCommandVoidCallBack s_stop; //停止 不可用
        public static PlusbeCommandVoidCallBack s_play; //播放
        public static PlusbeCommandVoidCallBack s_pause; //暂停
        public static PlusbeCommandVoidCallBack s_replay;// 重播

        public static PlusbeCommandVoidCallBack s_prev_page;//上一个 不可用
        public static PlusbeCommandVoidCallBack s_next_page;//下一个 不可用
        public static PlusbeCommandVoidCallBack s_show_min;//最小化
        public static PlusbeCommandVoidCallBack s_show_max;//最大化
        public static PlusbeCommandOneCallBack s_open;//点播

        public static PlusbeCommandStringCallBack s_get_json; //json数据获取 有返回值需要在顶级跳进进行处理

        private static void Stop()
        {
            Debug.Log("stop");
            if (s_stop != null) s_stop();
        }

        private static void Play()
        {
            if (s_play != null) s_play();
        }

        private static void Pause()
        {
            if (s_pause != null) s_pause();
        }

        private static void Replay()
        {
            if (s_replay != null) s_replay();
        }

        private static void PrevPage()
        {
            if (s_prev_page != null) s_prev_page();
        }

        private static void NextPage()
        {
            if (s_next_page != null) s_next_page();
        }

        private static void ShowMin()
        {
            if (s_show_min != null) s_show_min();
        }

        private static void ShowMax()
        {
            if (s_show_max != null) s_show_max();
        }

        private static void Open(string obj)
        {
            EnterVideo();
            if (s_open != null) s_open(obj);
        }

        private static string GetJson(string obj)
        {
            string result = "";
            if (s_get_json != null)
            {
                result = s_get_json(obj);
            }
            return result;
        }


        #endregion

        #region 欢迎词

        public static PlusbeCommandTweCallBack s_welcome_add;
        public static PlusbeCommandOneCallBack s_welcome_delete;
        public static PlusbeCommandThreeCallBack s_welcome_update;
        public static PlusbeCommandStringVoidCallBack s_welcome_json;
        public static PlusbeCommandOneCallBack s_welcome_play;
        public static PlusbeCommandOneCallBack s_welcome_mode_play;
        public static PlusbeCommandVoidCallBack s_welcome_back;

        public static string AddWelcome(string act, string t, string id, string title, string size)
        {
            return Add(act, t, id, title, size);
        }

        public static void WelcomeAdd(string title, string size)
        {
            if (s_welcome_add != null) s_welcome_add(title, size);
        }

        public static void WelcomeDelete(string id)
        {
            if (s_welcome_delete != null) s_welcome_delete(id);            
        }

        public static void WelcomeUpdate(string id, string title, string size)
        {
            if (s_welcome_update != null) s_welcome_update(id, title, size);
        }

        public static string WelcomeJson()
        {
            string result = "";
            if (s_welcome_json != null) result = s_welcome_json();
            return result;
        }

        public static void WelcomePlay(string id)
        {
            if (s_welcome_play != null) s_welcome_play(id);
        }

        public static void WelcomeModePlay(string index)
        {
            if (s_welcome_mode_play != null) s_welcome_mode_play(index);
        }

        public static void WelcomeBack()
        {
            EnterWelcome();

            if (s_welcome_back != null) s_welcome_back();
        }

        #endregion

        #region add

        public static string Add(string act, string states, string obj)
        {
            return Add(act, states, obj, "");
        }

        public static string Add(string act, string states, string obj, string param4)
        {
            return Add(act, states, obj, param4, "");
        }
        

        #endregion

    }

    public delegate string PlusbeCommandStringCallBack(string obj);
    public delegate string PlusbeCommandStringVoidCallBack();

    public delegate void PlusbeCommandFiveCallBack(string act, string states, string obj, string param4, string param5);
    public delegate void PlusbeCommandFourCallBack(string states, string obj, string param4, string param5);
    public delegate void PlusbeCommandThreeCallBack(string states, string obj, string param4);
    public delegate void PlusbeCommandTweCallBack(string states, string obj);
    public delegate void PlusbeCommandOneCallBack(string states);
    public delegate void PlusbeCommandVoidCallBack();
   
}
