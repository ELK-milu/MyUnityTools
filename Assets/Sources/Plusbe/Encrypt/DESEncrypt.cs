using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Plusbe.Encrypt
{
    public class DESEncrypt
    {
        private static byte[] Keys = new byte[]
		{
			18,
			52,
			86,
			120,
			144,
			171,
			205,
			239
		};

        public static string publicKeys = "ProductName";

        public static string EncryptDES(string encryptString)
        {
            return EncryptDES(encryptString, DESEncrypt.publicKeys);
        }

        public static string EncryptDES(string encryptString, string encryptKey)
        {
            string result;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] keys = DESEncrypt.Keys;
                byte[] bytes2 = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, keys), CryptoStreamMode.Write);
                cryptoStream.Write(bytes2, 0, bytes2.Length);
                cryptoStream.FlushFinalBlock();
                result = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch
            {
                result = encryptString;
            }
            return result;
        }

        public static string DecryptDES(string decryptString)
        {
            return DecryptDES(decryptString, DESEncrypt.publicKeys);
        }

        public static string DecryptDES(string decryptString, string decryptKey)
        {
            string result;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] keys = DESEncrypt.Keys;
                byte[] array = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, keys), CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
                result = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch
            {
                result = decryptString;
            }
            return result;
        }
    }
}
