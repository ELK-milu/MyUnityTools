using Plusbe.Core;
using Plusbe.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Encrypt
{
    public class KeyHelper
    {

        //硬件唯一码 
        //有效期
        //产品名
        //版本号
        //RSA加密
        private const string publicKey = "Plusbe2020";

        public static bool Active()
        {
            return DogActive() || SoftActive();
        }

        private static bool DogActive()
        {
            return false;
        }

        private static bool SoftActive()
        {
            CodeStatus codeStatus = ActiveCode();

            if (codeStatus.code == 0)
            {
                Debug.Log(codeStatus.message);
                return true;
            }
            else if (codeStatus.code == 1)
            {
                Debug.Log(codeStatus.message);
                return true;
            }
            else
            {
                Debug.Log(codeStatus.message);
                return false;
            }
        }

        public static string GetActiveCode(string date = "2088/08/08")
        {
            return GetCheckKeyCode(publicKey, GlobalSetting.UniqueKey, date, GlobalSetting.ProductName, GlobalSetting.ProductVersion);
        }

        private static CodeStatus ActiveCode()
        {
            string keyCode = GetCheckKeyCode(GlobalSetting.KeyPath);
            return ActiveCode(keyCode, publicKey, GlobalSetting.UniqueKey, DateTime.Now, GlobalSetting.ProductName, GlobalSetting.ProductVersion);
        }

        //private static void Active()
        //{
        //    if (!CheckKey(GlobalSetting.KeyPath, GlobalSetting.UniqueKey))
        //    {
        //        if (Application.isEditor)
        //        {
        //            Debug.Log("未激活设备:" + GlobalSetting.UniqueKey);
        //        }
        //        else
        //        {
        //            Debug.Log("未激活设备:");
        //        }
        //        //Application.Quit();
        //    }
        //}

        //public static bool CheckKey(string path, string publicKey, string uniqueName)
        //{
        //    if (File.Exists(path))
        //    {
        //        using (StreamReader sr = new StreamReader(path))
        //        {
        //            string result = sr.ReadToEnd();
        //            if (uniqueName == RSAHelper.DecryptString(result, publicKey))
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private static bool CheckKey(string path, string uniqueName)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string result = sr.ReadToEnd();
                    if (uniqueName == result)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private static string GetCheckKeyCode(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string result = sr.ReadToEnd();
                    return result;
                }
            }

            return "123456890";
        }

        private static string GetCheckKeyCode(string key, string uniqueCode, string date, string productName, int version)
        {
            return DESEncrypt.EncryptDES(uniqueCode + "_" + date + "_" + productName + "_" + version, key);
        }

        //硬件唯一码 
        //有效期
        //产品名
        //版本号

        private static CodeStatus ActiveCode(string keyCode, string key, string uniqueCode, DateTime date, string productName, int version)
        {

            string code = DESEncrypt.DecryptDES(keyCode, key); //解码
            string[] datas = code.Split('_'); //分隔

            if (datas.Length >= 4)
            {
                try
                {
                    string keyUniqueCode = datas[0];
                    DateTime keyDate = DateTime.Parse(datas[1]);
                    string keyProductName = datas[2];
                    int keyVersion = Convert.ToInt32(datas[3]);

                    if (keyProductName != productName)
                        return new CodeStatus(CodeCode.Error, "无效的产品号");

                    if (keyVersion < version)
                        return new CodeStatus(CodeCode.Error, "无效的版本号");

                    if (keyUniqueCode != uniqueCode)
                        return new CodeStatus(CodeCode.Error, "无效的硬件码");

                    int day = keyDate.Subtract(date.Date).Days;
                    if(day<0)
                        return new CodeStatus(CodeCode.Error, "已过期");

                    if(day<10)
                        return new CodeStatus(CodeCode.Warning, string.Format("激活成功,即将过期，剩余{0}天", day));

                    return new CodeStatus(CodeCode.Success, "激活成功", ""+day);

                }
                catch (Exception ex)
                {
                    return new CodeStatus(CodeCode.Error, "无效的验证码，无法分解", ex.ToString());
                }
            }


            return new CodeStatus(CodeCode.Error, "无效的验证码，无法解析");
        }
    }

    
}
