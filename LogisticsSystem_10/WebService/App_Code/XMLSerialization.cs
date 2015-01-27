using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
/// <summary>
/// ConvertXml 的摘要说明
/// </summary>
public class XMLSerialization
{
	public XMLSerialization()
	{
        
		
	}

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
}
