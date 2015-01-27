using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.Json;
using ZDIMS.BaseLib;
using ZDIMS.Util;
namespace Logistics_system
{
    public class XMLSerialization
    {
        public XMLSerialization()
        { }
        /// <summary>
        /// XML序列化 对象到字符串
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string Serialize<T>(T obj)
        {
            try
            {
                XmlSerializer xmlSeria = null;
                StringWriter ms = new StringWriter();
                xmlSeria = new XmlSerializer(typeof(T));
                xmlSeria.Serialize(ms, obj);
                return ms.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("序列化失败,原因:" + ex.Message);
            }
        }
        public static string ConvertToxmlString<T>(T para)
        {
            string tmp = XMLSerialization.Serialize(para);
            tmp = tmp.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            tmp = tmp.Replace("&lt;", "<");
            tmp = tmp.Replace("&gt;", ">");
            tmp = tmp.Trim();
            return tmp;
        }
        /// <summary>
        /// 类似Dot_2D数组xml转换成java服务器端识别的xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string convertToArrxmlString<T>(T para)
        {
            string tmp = XMLSerialization.Serialize(para);
            tmp = tmp.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Trim();
            int s = tmp.IndexOf(">");
            int end = tmp.LastIndexOf("</");
            int length = tmp.Length;
            string temp = tmp.Substring(s + 1, length - (length - end + s + 1)).Trim();
            return temp;
        }
        /// <summary>
        /// Json序列化 对象到字符串
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <returns>序列化后的字符串</returns>
        //public static string SerializeJson<T>(T obj)
        //{
        //    try
        //    {
        //        string strJson = JsonConvert.SerializeObject(obj);
        //        return strJson;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("序列化失败,原因:" + ex.Message);
        //    }
        //}

        /// <summary>
        /// XML反序列化 字符串到对象
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <param name="str">要转换为对象的字符串</param>
        /// <returns>反序列化出来的对象</returns>
        public static T Desrialize<T>(T obj, string str)
        {


            try
            {
                obj = default(T);
                StringReader sr = new StringReader(str);
                XmlSerializer xmlSerial = new XmlSerializer(typeof(T));
                obj = (T)xmlSerial.Deserialize(sr);

            }
            catch (Exception ex)
            {
                throw new Exception("反序列化失败,原因:" + ex.Message);


            }
            return obj;
        }

        /// <summary>
        /// Json反序列化 字符串到对象
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <param name="str">要转换为对象的字符串</param>
        /// <returns>反序列化出来的对象</returns>
        //public static T DesrializeJson<T>(T obj, string str)
        //{
        //    try
        //    {
        //        obj = default(T);
        //        obj = (T)JsonConvert.DeserializeObject(str);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("反序列化失败,原因:" + ex.Message);
        //    }
        //    return obj;
        //}
        /// <summary>
        /// 序列化为服务器端识别的xml串
        /// </summary>
        /// <param name="para">序列化对象</param>
        /// <param name="className">序列化的类名</param>
        /// <returns></returns>

    }
}

