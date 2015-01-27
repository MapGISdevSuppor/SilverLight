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
using ZDIMS.BaseLib;
using ZDIMS.Map;
using ZDIMS.Interface;
using System.Windows.Browser;
using ZDIMS.Util;

namespace SilverlightDemo.Demo
{
    public partial class BufferAnalyse : Page
    {
        string TileLayerAddress = "";
        string VecLayerAddress = "";
        private string bufferName;
        private VectorLayer _vLayer;
        private Boolean successFlag = true;
        private List<VectorLayer> BufferArr = new List<VectorLayer>();
        public BufferAnalyse()
        {
            InitializeComponent();
        }
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
        //缓冲分析
        private void BufferSubmit(object sender, RoutedEventArgs e)
        {
           
            if (this.successFlag == true)
            {
                bufferName = "";
                this.successFlag = false;
                //根据要素id及图层索引获取要素的各种信息功能的参数类
                CGetObjByID targetObj = new CGetObjByID();
                targetObj.MapDocIndex = 0;
                //要素id
                targetObj.FeatureID = 2316;
                //图层索引
                targetObj.LayerIndex = 2;
                //初始化缓冲分析类
                CBufferFeature BufferInfo = new CBufferFeature();
                //设置缓冲分析的空间信息所在的文档的信息
                BufferInfo.FeatureInfo = targetObj;
                //设置缓冲半径
                BufferInfo.BufferRadius = Convert.ToDouble(this.radius.Text);
                //设置Trace半径
                BufferInfo.TraceRadius = Convert.ToDouble(this.radius.Text) / 3;
                Random rad = new Random();
                //设置缓冲分析结果图层名
                BufferInfo.ResultLayerName = "BUF" + rad.NextDouble();
                bufferName = BufferInfo.ResultLayerName;
                //设置缓冲分析结果图层类型设置为简单要素类   
                BufferInfo.ResultLayerType = XClsType.SFeatureCls;
                //要素缓冲分析
                this.mapDoc.BufferFeature(BufferInfo, new UploadStringCompletedEventHandler(getBufferFeature));
                this.iMSMap1.SetErrorText("");
            }
            else {
                this.iMSMap1.SetErrorText("正在缓冲分析,请稍后...");
                return;
            }
        }
        //缓冲分析结果回调函数
        private void getBufferFeature(Object sender, UploadStringCompletedEventArgs e)
        {
            if (this.successFlag == false)
            {
                COperResult obj = this.mapDoc.OnBufferFeature(e);
                if (obj.OperResult)
                {
                    string svAddress = this.mapDoc.ServerAddress;
                    //显示缓冲分析后的图层
                    showBufferInfo(bufferName, XClsType.SFeatureCls, svAddress);
                }
            
            }

        }
        //缓冲分析后的图层显示函数
        private void showBufferInfo(string layerName, XClsType layerType, string svrAddr = "127.0.0.1", string gdbSvr = "MapGisLocal", string gdbName = "IMSWebGISGDB", string usr = "", string psw = "")
        {
            if (this.successFlag == false)
            {
                //图层访问信息类
                CLayerAccessInfo layerAccessInfo = new CLayerAccessInfo();
                //gdb信息类
                layerAccessInfo.GdbInfo = new CGdbInfo();
                layerAccessInfo.GdbInfo.GDBName = gdbName;
                layerAccessInfo.GdbInfo.GDBSvrName = gdbSvr;
                layerAccessInfo.GdbInfo.User = usr;
                layerAccessInfo.GdbInfo.Password = psw;
                //图层信息类
                CLayerInfo layerInfo = new CLayerInfo();
                layerInfo.LayerDataName = layerName;
                layerInfo.LayerType = layerType;
                layerInfo.GeomType = SFclsGeomType.Reg;
                layerAccessInfo.LayerInfoList = new CLayerInfo[1];
                layerAccessInfo.LayerInfoList[0] = layerInfo;
                //设置要叠加打开的图层信息
                COpenLayer openLayer = new COpenLayer();
                openLayer.LayerAccessInfo = new CLayerAccessInfo[1];
                openLayer.LayerAccessInfo[0] = layerAccessInfo;
                //实例化矢量图层
                this._vLayer = new VectorLayer();
                this.iMSMap1.AddChild(this._vLayer);
                this._vLayer.ServerAddress = svrAddr;
                this._vLayer.LayerObj = openLayer;
                //加载缓冲分析后的图层
                this._vLayer.LoadLayer(openLayer, new UploadStringCompletedEventHandler(onSingleLayerLoad));
            }
        }
        //加载图层回调函数
        private void onSingleLayerLoad(Object sender, UploadStringCompletedEventArgs e)
        {
            COperResult rlt = this._vLayer.OnLoadLayer(e);
            if (rlt.OperResult)
            {
                this.BufferArr.Add(this._vLayer);
                this.successFlag = true;
                this.iMSMap1.SetErrorText("");
            }

        }

        //清除缓冲分析的图层
        private void removeBufLayer(object sender, RoutedEventArgs e)
        {
            if (this.BufferArr != null)
            {
                if (this.BufferArr.Count > 0)
                {
                    for (int i = 0; i < BufferArr.Count; i++)
                    {
                        this.iMSMap1.RemoveChild(BufferArr[i]);
                    }
                    BufferArr = new List<VectorLayer>();
                    successFlag = true;
                    this.iMSMap1.SetErrorText("");

                }
            }
        }

    }
}
