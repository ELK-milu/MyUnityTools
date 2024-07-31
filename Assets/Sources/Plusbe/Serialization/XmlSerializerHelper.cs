using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PlusbeSerialization
{
    public class XmlSerializerHelper
    {

        /// <summary>
        /// 文件化XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void SaveXml(object obj, string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter write = new StreamWriter(fs, Encoding.UTF8))
                    {
                        XmlSerializer serializer = new XmlSerializer(obj.GetType());
                        serializer.Serialize(write, obj);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            //FileStream fs = null;
            //try
            //{
            //    fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            //    XmlSerializer serializer = new XmlSerializer(obj.GetType());
            //    serializer.Serialize(fs, obj);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    if (fs != null) fs.Close();
            //}
        }

        /// <summary>
        /// 文件化XML反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            //try
            //{
            //    using (StringUTF8Writer writer = new StringUTF8Writer())
            //    {
            //        XmlSerializer serializer = new XmlSerializer(type,);
            //        serializer.
            //        return serializer.Deserialize(writer,);
            //    }
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}


            //FileStream fs = null;
            //try
            //{
            //    fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //    XmlSerializer serializer = new XmlSerializer(type);
            //    return serializer.Deserialize(fs);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    if (fs != null) fs.Close();
            //}
        }

        /// <summary>
        /// 文本化XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        public static string ToXml<T>(T obj)
        {
            string result;
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(obj.GetType()).Serialize(writer, obj);
                result = writer.ToString();
            }
            return result;
        }

        /// <summary>
        /// 文本化XML反序列化
        /// </summary>
        /// <param name="strInfo">字符串序列</param>
        public static T FromXml<T>(string strInfo)
        {
            T result;
            using (StringReader reader = new StringReader(strInfo))
            {
                result = (T)(new XmlSerializer(typeof(T)).Deserialize(reader));
            }
            return result;
        }
    }

    public class StringUTF8Writer : StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
