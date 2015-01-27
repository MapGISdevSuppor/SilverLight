<%@ WebHandler Language="C#" Class="service" %>

using System;
using System.Web;
using System;
using System.Web;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Configuration;
using System.Xml.Schema;
using System.Collections;
public class service : IHttpHandler {
    private HttpWebResponse m_response = null; //响应
    private Stream m_streamResponse = null;    //响应流
    private string m_requeststr = "";          //请求参数
    private string m_hostUrl = "";
    public void ProcessRequest (HttpContext context) {
        string requestMethod = HttpContext.Current.Request.HttpMethod;
        m_hostUrl = "http://" + HttpContext.Current.Request.Url.Authority + "/" + HttpContext.Current.Request.Url.Segments[1];
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        
         string RequestSign = "";
        //判断请求类型， http Get或Post
         switch (requestMethod.ToLower())
         {
             case "post":
                 {//解析Post请求参数
                     Stream clientStream = context.Request.InputStream;
                     Encoding encode = Encoding.UTF8;
                     StreamReader reader = new StreamReader(clientStream, encode);
                     m_requeststr = reader.ReadToEnd();
                     reader.Close();
                     clientStream.Close();
                     if (!m_requeststr.ToUpper().Contains("REQUEST"))
                     {
                         context.Response.AppendHeader("Content-Type", "text/plain");
                         context.Response.Write("REQUEST参数不能为空，请输入REQUEST值，REQUEST必须为gettile、gettilebypoint、gettilemapinfo、GetTileMetaData或GetTileNameList之一。");
                         context.Response.End();
                     }

                     int startIndex = m_requeststr.ToUpper().IndexOf("REQUEST=");
                     int endIndex = m_requeststr.ToUpper().IndexOf("&", startIndex);
                     RequestSign = m_requeststr.Substring(startIndex + 8, endIndex - (startIndex + 8));
                     break;
                 }
             case "get":
                 //获取Get请求参数
                 if (context.Request["REQUEST"] == null)
                 {
                     context.Response.AppendHeader("Content-Type", "text/plain");
                     context.Response.Write("REQUEST参数不能为空，请输入REQUEST值，REQUEST必须为gettile、gettilebypoint、gettilemapinfo、GetTileMetaData或GetTileNameList之一。");
                     context.Response.End();
                    
                 }
                         RequestSign = context.Request["REQUEST"];
                m_requeststr = context.Request.QueryString.ToString();
                break; 
         }
         string twmsUrl = "";

         twmsUrl = m_hostUrl + "Service.asmx/SearchMarketBykey?" + m_requeststr;
         GetMetaData(context, twmsUrl);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private void GetMetaData(HttpContext context, string url)
    {
        StreamReader strReader = null;
        string result = "";
        try
        {
            Uri m_Uri = new Uri(url);
            HttpWebRequest m_request = (HttpWebRequest)WebRequest.Create(m_Uri);
            m_request.CookieContainer = new CookieContainer();
            m_request.Method = "Get";
            //  m_request.AllowAutoRedirect = true;
            m_request.ContentType = "text/xml; charset=utf-8";
            m_response = (HttpWebResponse)m_request.GetResponse();
            m_streamResponse = m_response.GetResponseStream();
            strReader = new StreamReader(m_streamResponse);
            string tempresult = strReader.ReadToEnd();
            tempresult = tempresult.Replace("xmlns=\"http://tempuri.org/\"", "");
            result = (string)DeserializeToObj(typeof(string), tempresult);
            //context.Response.ContentType = "application/xml";
            context.Response.Write(result);
        }
        catch (Exception ex)
        {
            context.Response.AppendHeader("Content-Type", "text/plain");
            context.Response.Write(ex.Message);
            context.Response.End();
        }
        finally
        {
            ReleaseResouce();
        }
        context.Response.End();
    }

    private object DeserializeToObj(Type tt, string classStr)
    {
        object obj = null;
        XmlSerializer xmlSeri = new XmlSerializer(tt);
        StringReader sr = new StringReader(classStr);
        obj = xmlSeri.Deserialize(sr);
        return obj;
    }
    private void ReleaseResouce()
    {
        if (m_streamResponse != null)
            m_streamResponse.Close();
        if (m_response != null)
            m_response.Close();
    }

}