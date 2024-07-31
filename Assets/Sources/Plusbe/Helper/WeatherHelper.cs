using Plusbe.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace PlusbeHelper
{
    public class WeatherHelper : MonoBehaviour
    {
        public string url = "http://api.map.baidu.com/telematics/v3/weather?location=%E6%9D%AD%E5%B7%9E&output=json&ak=Fto47rO8MGIXvFKwPr3GZkM0";

        public string message;

        public Text txtNowTemp;
        public Text txtTemp;
        public Text txtWeather;
        public Text txtWind;
        public Text txtHum;
        public Text txtPm25;
        public Text txtSport;
        public Text txtIll;
        public Text txtCloth;

        public Image picWeather;

        private string[] airs = { "优", "良好", "轻度污染", "中度污染", "重度污染", "严重污染" };

        private Sprite[] weatherSprits;

        private float lastTime;
        private int timeAll = 60 * 30;


        private string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        public Text txtTime;
        public Text txtDate;
        public Text txtWeek;

        void Start()
        {
            weatherSprits = Resources.LoadAll<Sprite>("天气图标");

            lastTime = Time.time;

            initWeather();
        }

        void Update()
        {
            if (!string.IsNullOrEmpty(message))
            {
                Debug.Log(message);
                try
                {
                    Newtonsoft.Json.Linq.JObject result = Newtonsoft.Json.Linq.JObject.Parse(message);
                    if (result["status"].ToString() == "success")
                    {
                        string nowTemp = result["results"][0]["weather_data"][0]["date"].ToString();
                        int index = nowTemp.IndexOf('：');
                        showData(txtNowTemp, nowTemp.Substring(index + 1).Replace(")", ""));
                        Debug.Log(nowTemp);
                        showData(txtTemp, result["results"][0]["weather_data"][0]["temperature"].ToString());
                        showData(txtWeather, result["results"][0]["weather_data"][0]["weather"].ToString());
                        showData(txtWind, result["results"][0]["weather_data"][0]["wind"].ToString());
                        showData(txtHum, getHum(result["results"][0]["weather_data"][0]["weather"].ToString()));
                        showData(txtPm25, getPm25(result["results"][0]["pm25"].ToString()));
                        showData(picWeather, getWeatherSprite(result["results"][0]["weather_data"][0]["weather"].ToString()));

                        //txtTemp.text = nowTemp.Substring(index + 1).Replace(")", "");
                        //txtWeather.text = result["results"][0]["weather_data"][0]["weather"].ToString();
                        //txtWind.text = result["results"][0]["weather_data"][0]["wind"].ToString();
                        //txtHum.text = getHum("");
                        //txtPm25.text = getPm25(result["results"][0]["pm25"].ToString());

                        //picWeather.sprite = getWeatherSprite(txtWeather.text);

                        string wSport = "一般";
                        string wIll = "少发";
                        string wCloth = "舒适";
                        int indexCount = result["results"][0]["index"].Count();
                        for (int i = 0; i < indexCount; i++)
                        {
                            //穿衣  洗车  感冒 运动 紫外线强度
                            string indexTitle = result["results"][0]["index"][i]["title"].ToString();
                            if (indexTitle == "感冒")
                            {
                                wIll = result["results"][0]["index"][i]["zs"].ToString();
                            }
                            else if (indexTitle == "运动")
                            {
                                wSport = result["results"][0]["index"][i]["zs"].ToString();
                            }
                            else if (indexTitle == "穿衣")
                            {
                                wCloth = result["results"][0]["index"][i]["zs"].ToString();
                            }
                        }

                        showData(txtSport, wSport);
                        showData(txtIll, wIll);
                        showData(txtCloth, wCloth);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                }

                message = "";
            }

            if (Time.time - lastTime > timeAll)
            {
                lastTime = Time.time;
                initWeather();
            }

            updateTime();
        }

        public void changePic()
        {

        }

        public void initWeather()
        {
            Thread thread = new Thread(threadGoWeather);
            thread.Start();
        }

        public void updateWeather()
        {
            message = "";
        }

        private void showData(Text txt, string content)
        {
            if (txt != null)
            {
                txt.text = content;
            }
        }

        private void showData(Image img, Sprite sp)
        {
            if (img != null)
            {
                img.overrideSprite = sp;
            }
        }

        private void updateTime()
        {
            DateTime dateTime = DateTime.Now;

            string date = dateTime.ToString("yyyy年MM月dd日");
            string week = weekdays[Convert.ToInt32(dateTime.DayOfWeek)];
            string time = dateTime.ToString("HH : mm");

            showData(txtTime, time);
            showData(txtDate, date);
            showData(txtWeek, week);
        }

        private void threadGoWeather()
        {
            message = HttpHelper.HtmlCode(url);
        }

        private Sprite getWeatherSprite(string weat)
        {
            int len = weatherSprits.Length;
            if (len >= 25)
            {
                if (weat.IndexOf("雪") != -1)
                {
                    return weatherSprits[21];
                }
                else if (weat.IndexOf("雷阵雨") != -1)
                {
                    return weatherSprits[9];
                }
                else if (weat.IndexOf("雷") != -1)
                {
                    return weatherSprits[21];
                }
                else if (weat.IndexOf("雨") != -1)
                {
                    return weatherSprits[6];
                }
                else if (weat.IndexOf("阴") != -1)
                {
                    return weatherSprits[18];
                }
                else if (weat.IndexOf("晴") != -1)
                {
                    return weatherSprits[24];
                }
            }

            return weatherSprits[0];
        }

        private string getPm25(string pm)
        {
            try
            {
                int pm25 = Convert.ToInt32(pm);
                if (pm25 <= 50)
                {
                    return airs[0];
                }
                else if (pm25 <= 100)
                {
                    return airs[1];
                }
                else if (pm25 <= 150)
                {
                    return airs[2];
                }
                else if (pm25 <= 200)
                {
                    return airs[3];
                }
                else if (pm25 <= 300)
                {
                    return airs[4];
                }
                else
                {
                    return airs[5];
                }
            }
            catch { }

            return airs[1];
        }

        private string getHum(string weather)
        {
            System.Random r = new System.Random();
            if (weather.IndexOf("雨") != -1 || weather.IndexOf("雪") != -1)
            {
                return r.Next(60, 80) + "%";
            }
            else if (weather.IndexOf("阴") != -1)
            {
                return r.Next(50, 60) + "%";
            } if (weather == "晴" || weather == "晴转多云")
            {
                return r.Next(20, 30) + "%";
            }
            else
            {
                return r.Next(35, 45) + "%";
            }
        }
    }
}
