using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Plusbe.Encrypt
{
    /// <summary>
    /// 本类进行目的//私钥加密 公钥解密
    /// 当前类为公钥加密 私钥解密
    /// </summary>
    public class RSAEncrypt
    {
        /// <summary>
        /// RSA密钥产生 私钥与公钥
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <param name="xmlPublicKey"></param>
        public static void RSAKey(out string xmlPrivateKey, out string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlPrivateKey = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
            //rsa.FromXmlString(
        }

        public static string EncryptRSA(string encryptString, string encryptKey)
        {
            string result;
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(encryptKey);
            PlainTextBArray = (new UnicodeEncoding()).GetBytes(encryptString);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            result = Convert.ToBase64String(CypherTextBArray);
            return result; 
        }

        public static string DecryptRSA(string decryptString, string decryptKey)
        {
            string result;
            byte[] PlainTextBArray; 
			byte[] DypherTextBArray; 
			System.Security.Cryptography.RSACryptoServiceProvider rsa=new RSACryptoServiceProvider(); 
			rsa.FromXmlString(decryptKey); 
			PlainTextBArray =Convert.FromBase64String(decryptString); 
			DypherTextBArray=rsa.Decrypt(PlainTextBArray, false); 
			result=(new UnicodeEncoding()).GetString(DypherTextBArray); 
			return result; 
        }
    }
}
