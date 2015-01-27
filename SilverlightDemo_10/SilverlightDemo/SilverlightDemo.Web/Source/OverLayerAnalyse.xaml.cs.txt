using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using ZDIMS.Drawing;
using ZDIMS.Event;
using ZDIMS.Map;
using ZDIMSDemo.Controls;
using ZDIMSDemo.Controls.MapDoc;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using ZDIMS.Util;
using ZDIMSDemo.MyControl;
using ZDIMS.BaseLib;
using ZDIMS.Interface;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Json;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.ServiceModel;
using ZDIMSDemo.Controls.Layer;
using System.Windows.Browser;

namespace SilverlightDemo
{
    public partial class OverLayerAnalyse : Page
    {
        string TileLayerAddress = "";
        string VecLayerAddress = "";
        private IMSRectangle IMSRect = null;
        private IMSRectangle IMSRect2 = null;
        private ZDIMS.BaseLib.Rect rect1 = null;
        private ZDIMS.BaseLib.Rect rect2 = null;
        private GraphicsLayer g_GraphicsLayer = null;
        private Plugin plug;
        private ZDIMSDemo.Controls.Layer.LayerDataViewer layerDataViewer;
        private Boolean Overflag = true;
        private Boolean ISSuccess = true;

        public OverLayerAnalyse()
        {
            InitializeComponent();
            iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
        }
        private void iMSMap1_MapReady(IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                if (g_GraphicsLayer == null)
                {
                    g_GraphicsLayer = new GraphicsLayer();
                    this.iMSMap1.AddChild(this.g_GraphicsLayer);
                    g_GraphicsLayer.EnableGPUMode = true;
                }
                if (g_GraphicsLayer != null)
                {
                    //添加第一个矩形范围
                    IMSRect = new IMSRectangle(CoordinateType.Logic);
                    this.g_GraphicsLayer.AddGraphics(IMSRect);
                    //起始点 
                    IMSRect.StartPoint = new Point(114.30829449926757, 30.637357284423828);
                    //结束点
                    IMSRect.EndPoint = new Point(114.32125563671875, 30.64560111743164);
                    IMSRect.Draw();
                    //添加第二个矩形范围
                    IMSRect2 = new IMSRectangle(CoordinateType.Logic);
                    this.g_GraphicsLayer.AddGraphics(IMSRect2);
                    //起始点
                    IMSRect2.StartPoint = new Point(114.3189198840332, 30.63515892895508);
                    //结束点
                    IMSRect2.EndPoint = new Point(114.32991166137695, 30.64738728125);
                    IMSRect2.Draw();
                    //Rect对象存储第一个矩形的范围
                    rect1 = new ZDIMS.BaseLib.Rect();
                    rect1.xmin = 114.30829449926757;
                    rect1.xmax = 114.32125563671875;
                    rect1.ymin = 30.637357284423828;
                    rect1.ymax = 30.64560111743164;
                    //Rect对象存储第二个矩形范围
                    rect2 = new ZDIMS.BaseLib.Rect();
                    rect2.xmin = 114.3189198840332;
                    rect2.xmax = 114.32991166137695;
                    rect2.ymin = 30.63515892895508;
                    rect2.ymax = 30.64738728125;
                    this.iMSCatalog1.MapContainer = iMSMap1;
                }
            }

        }
        private void OverLayerAnalyseSubmit(object sender, RoutedEventArgs e)
        {

            if (ISSuccess == true)
            {
            //   if (layerDataViewer == null)
            //    {
           //         layerDataViewer = new LayerDataViewer();
           //         layerDataViewer.IMSCatalog = this.iMSCatalog1;
           //     }
            //    if (this.layerDataViewer == null)
              //  {
             //       MessageBox.Show("layerDataViewer属性为空！请创建LayerDataViewer控件！");
           //     }

                //叠加参数操作类
                OverlayBySelectParam param = new OverlayBySelectParam();
                //设置第一个查询参数类
                CWebSelectParam selectParam = new CWebSelectParam();
                //设置查询类型为矩形查询
                selectParam.GeomType = WebGeomType.Rect;
                //设置知查询的矩形范围
                MemoryStream ms1 = new MemoryStream();
                DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(ZDIMS.BaseLib.Rect));
                ser1.WriteObject(ms1, rect1);
                byte[] json1 = ms1.ToArray();
                ms1.Close();
                string jsonString1 = Encoding.UTF8.GetString(json1, 0, json1.Length);//序列化得到的字符串
                selectParam.Geometry = jsonString1;
                //设置搜索半径
                selectParam.NearDistance = 0.1;
                //设置查询类型为简单查询
                selectParam.SelectionType = SelectionType.SpatialRange;
                //设置查询对象是否必须完全包含在输入的空间的范围内
                selectParam.MustInside = true;
                //设置第二个CWebSelectParam查询参数类
                CWebSelectParam selectParam2 = new CWebSelectParam();
                //设置查询类型为矩形查询
                selectParam2.GeomType = WebGeomType.Rect;
                MemoryStream ms2 = new MemoryStream();
                DataContractJsonSerializer ser2 = new DataContractJsonSerializer(typeof(ZDIMS.BaseLib.Rect));
                ser1.WriteObject(ms2, rect2);
                byte[] json2 = ms2.ToArray();
                ms1.Close();
                string jsonString2 = Encoding.UTF8.GetString(json2, 0, json2.Length);//序列化得到的字符串
                //设置查询的矩形范围
                selectParam2.Geometry = jsonString2;
                //设置搜索半径
                selectParam2.NearDistance = 0.1;
                //设置查询类型为简单查询
                selectParam2.SelectionType = SelectionType.SpatialRange;
                //CGdbInfo gdb信息类 
                CGdbInfo gdbInfo = new CGdbInfo();
                gdbInfo.GDBSvrName = "mapgislocal";
                gdbInfo.GDBName = "world";
                //叠加的图层名称
                param.LayerName1 = "城市植被";
                //叠加分析第一个图层的查询参数
                param.SelectParam1 = selectParam;
                //被叠加的图层名称
                param.LayerName2 = "行政区.WP";
                //叠加分析第二个图层的查询参数
                param.SelectParam2 = selectParam2;
                param.Radius = 0.001;
                //设置叠加分析的类型   1为相交分析
                param.OverlayType = 1;
                //是否重算面积
                param.IsReCalculate = false;
                param.GdbInfo1 = gdbInfo;
                param.GdbInfo2 = gdbInfo;
                //设置图层类型为简单要素类 
                param.ClsType = XClsType.SFeatureCls;
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(OverlayBySelectParam));
                ser.WriteObject(ms, param);
                byte[] json = ms.ToArray();
                ms.Close();
                string jsonString = Encoding.UTF8.GetString(json, 0, json.Length);//序列化得到的字符串
                //var postData:String =JSON.encode(param) as String;
                //实例化插件对象
                plug = new Plugin();
                plug.ServerAddress = this.mapDoc.ServerAddress;
                ISSuccess = false;
                plug.CallPlugin("OverlayAnalysisExtend", "OverlayAnalysisClass", "OverlayBySelect", "inputFormat=JSON&outputFormat=JSON", jsonString, onPlug);

            }
        }


        private void onPlug(object sender, UploadStringCompletedEventArgs e)
        {
            //设置两个矩形不可见
            this.IMSRect.Visibility = Visibility.Collapsed;
            this.IMSRect2.Visibility = Visibility.Collapsed;
            //获取调用插件的请求结果，得到分析成功后的url地址
            String getOverLayerUrl = plug.OnGetPluginList(sender, e) as String;
            //判断是否成功

            if (getOverLayerUrl != "" && getOverLayerUrl != null)
            {
                if (this.layerDataViewer != null)
                {
                    this.layerDataViewer.removeLayer();
                    this.layerDataViewer.Close();
                    layerDataViewer = null;

                }
                    if (this.layerDataViewer == null)
                    {
                        layerDataViewer = new LayerDataViewer();
                        layerDataViewer.IMSCatalog = this.iMSCatalog1;

                    }

                int loc1 = getOverLayerUrl.IndexOf("sfcls") + 6;
                int loc3 = getOverLayerUrl.Length - 1;
                int loc4 = loc3 - loc1;
                //截取字符串，获取叠加分析成功后的图层名称
                String loc2 = getOverLayerUrl.Substring(loc1, loc4);
                XClsType layerType = XClsType.SFeatureCls;
                String SAddress = this.mapDoc.ServerAddress;
                this.layerDataViewer.IMSCatalog = this.iMSCatalog1;
                this.layerDataViewer.GraphicsLayer = g_GraphicsLayer;
                this.layerDataViewer.Show();
                this.layerDataViewer.ShowLayerAttRecord(loc2, layerType, SAddress);
                ISSuccess = true;
                Overflag = true;
                this.layerDataViewer.Margin = new Thickness(0,120,0,0);
                this.layerDataViewer.HorizontalAlignment = HorizontalAlignment.Right;
                this.layerDataViewer.Visibility = Visibility.Visible;

            }
            else
            {

                MessageBox.Show("分析失败，重新点击分析！");
                ISSuccess = true;
            };

        }

        private void ClearOverLayer(object sender, RoutedEventArgs e)
        {
            this.layerDataViewer.removeLayer();
            Overflag = false;
            this.layerDataViewer.Visibility = Visibility.Collapsed;
        }
        //页面加载时给瓦片地图和矢量地图的ServerAddress赋值
        private void onLoadMap(object sender, RoutedEventArgs e)
        {
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();
                VecLayerAddress = HtmlPage.Window.Eval("VecLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            tileLayer1.ServerAddress = TileLayerAddress;
            mapDoc.ServerAddress = VecLayerAddress;

        }

        private void Close_LayerView(object sender, RoutedEventArgs e)
        {
            if(layerDataViewer!=null){

                this.layerDataViewer.removeLayer();
                Overflag = false;
                this.layerDataViewer.Visibility = Visibility.Collapsed;
            }
           
        }

    }
}

