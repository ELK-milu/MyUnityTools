using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Plusbe.Serialization
{
    public class MessageSerializerHelper
    {
        /// <summary>
        /// Http命令参数序列号转换
        /// </summary>
        /// <param name="url"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static NameValueCollection ParseHttpUrl(string url)
        {
            //string baseUrl = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            NameValueCollection nameValueCollection = new NameValueCollection();
            try
            {
                int num = url.IndexOf('?');
                //if (num == -1)
                //{
                //    baseUrl = url;
                //}
                //else
                //{
                //    baseUrl = url.Substring(0, num);
                //}
                if (num == url.Length - 1)
                {
                    return null;
                }
                string input = url.Substring(num + 1);
                Regex regex = new Regex("(^|&)?(\\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
                MatchCollection matchCollection = regex.Matches(input);
                IEnumerator enumerator = matchCollection.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Match match = (Match)enumerator.Current;
                        nameValueCollection.Add(match.Result("$2").ToLower(), match.Result("$3"));
                    }
                }
                finally
                {
                    IDisposable disposable;
                    if ((disposable = (enumerator as IDisposable)) != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            catch
            {
            }
            return nameValueCollection;
        }
    }
}
