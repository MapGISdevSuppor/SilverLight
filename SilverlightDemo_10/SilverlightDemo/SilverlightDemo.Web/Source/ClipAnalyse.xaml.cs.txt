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
using ZDIMS.Drawing;
using ZDIMS.Event;
using ZDIMS.Map;
using ZDIMS.Interface;
using ZDIMS.Util;
using ZDIMSDemo.Controls;
using ZDIMSDemo.Controls.MapDoc;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Browser;
namespace SilverlightDemo.Demo
{
    public partial class ClipAnalyse : Page
    {
        string TileLayerAddress = "";
        string VecLayerAddress = "";
        private GraphicsLayer m_gpLayer = null;
        private IMSCircle Clipcir = null;
        private Circle cirObj = null;
        private Dot_2D[] dot = null;
        private GRegion _clipRegion = null;
        private CLayerAccessInfo _clipLayerAcsInfo = null;
        private string _serverAddr = null;
        private SpacialAnalyse _spatial;
        private string _UID = "";
        private int timerCnt = 0;
        private VectorLayer _vLayer;
        private int flag = 0;
        private int successFlag = 0;

        // 时间控制属性
        private DispatcherTimer clipTimer;
        public ClipAnalyse()
        {
            InitializeComponent();
            iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
        }
        private void iMSMap1_MapReady(IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                //绘图图层
                m_gpLayer = new GraphicsLayer();
                iMSMap1.AddChild(m_gpLayer);
                m_gpLayer.EnableGPUMode = true;
                //添加圆形区域
                AddCircle();
                this.iMSCatalog1.MapContainer = iMSMap1;

            }
        }
        //将圆形区域添加到地图上
        private void AddCircle()
        {
            cirObj = new Circle();
            cirObj.Center = new Dot_2D();
            cirObj.Center.x = 114.3041267836914;
            cirObj.Center.y = 30.65155499682617;
            cirObj.Radius = 0.004484101072174819;

            //在地图上显示圆形区域
            Clipcir = new IMSCircle(CoordinateType.Logic);
            // cir.Stroke = new SolidColorBrush(Colors.Orange);
            Clipcir.StrokeThickness = 2;
            //设置圆心
            Clipcir.CenX = cirObj.Center.x;
            Clipcir.CenY = cirObj.Center.y;
            //设置半径
            Clipcir.Radius = cirObj.Radius;
            //绘制圆形
            Clipcir.Draw();
            m_gpLayer.AddGraphics(Clipcir);

        }

        //裁剪分析按钮点击事件
        private void ClipSubmit(object sender, RoutedEventArgs e)
        {
            if (this.m_gpLayer != null)
            {
                this.iMSCatalog1.ActiveMapDoc = this.mapDoc;
                this.mapDoc.ActiveLayerIndex = 2;
                this.mapDoc.UpdateAllLayerInfo(new UploadStringCompletedEventHandler(updateLayerStatus));
            }

        }
        private void updateLayerStatus(object sender, UploadStringCompletedEventArgs e)
        {

            if (this._vLayer != null)
            {
                if (flag == 0 || flag.Equals(0))
                {
                    this.iMSMap1.RemoveChild(this._vLayer);

                }
            }
            //判断是第一次点击或者是分析成功之后点击
            if (successFlag == 1 || successFlag == 0)
            {
                successFlag = 2;
                this.iMSMap1.Refresh();
                //圆形区域上所有的边界点
                dot = cirObj.GetDots();
                this.CircleClip();
            }
            else
            {
                this.iMSMap1.SetErrorText("正在分析，请在分析成功之后再点击！");
            }

        }
        //设置裁剪分析的参数
        private void CircleClip()
        {
            if (this.iMSCatalog1.ActiveMapDoc != null)
            {
                //删除绘制的图形
                this.m_gpLayer.RemoveAll();
                //GRegion多区
                GRegion region = new GRegion();
                //AnyLine任意线
                region.Rings = new AnyLine[1];
                region.Rings[0] = new AnyLine();
                region.Rings[0].Arcs = new Arc[1];
                region.Rings[0].Arcs[0] = new Arc();
                region.Rings[0].Arcs[0].Dots = new Dot_2D[dot.Length];
                region.Rings[0].Arcs[0].Dots = dot;
                //将圆形区域上的所有的边界点赋值给多区的对象
                this._clipRegion = region;
                // 设置图层的访问信息
                this._clipLayerAcsInfo = new CLayerAccessInfo();
                this._clipLayerAcsInfo = this.iMSCatalog1.ActiveMapDoc.ActiveLayerAccessInfo;
                this._serverAddr = this.iMSCatalog1.ActiveMapDoc.ServerAddress;
                //设置裁剪分析参数
                CClipByPolygon clip = new CClipByPolygon();
                //设置要裁剪的区
                clip.Region = this._clipRegion;
                CLayerAccessInfo clipLayer = this._clipLayerAcsInfo;
                clip.GdbInfo = clipLayer.GdbInfo;
                clip.LayerName = clipLayer.LayerInfoList[0].LayerDataName;
                clip.XClsType = clipLayer.LayerInfoList[0].LayerType;
                clip.FClsNameRtn = "clip" + clip.LayerName;
                clip.Radius = 0.0001;
                clip.Username = "";
                clip.UserIP = "";
                clip.ClipFlg = 3;
                clip.IsReCalculate = false;
                //空间分析类
                this._spatial = new SpacialAnalyse(this.iMSCatalog1.ActiveMapDoc);
                //裁剪分析
                this._spatial.ClipByPolygon(clip, new UploadStringCompletedEventHandler(onSubmit));
            }
            else
            {
                MessageBox.Show("矢量文档中没有激活的图层!");
            }
        }
        private void onSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            this.iMSMap1.EnableProgressBar = true;
            string uid = this._spatial.OnClipByPolygon(e);//获取区裁剪结果
            if (uid != null && uid != "")
            {
                clipTimer = new DispatcherTimer();
                clipTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                clipTimer.Tick += new EventHandler(clipTimer_Tick);
                clipTimer.Start();
                _UID = uid;
            }
            else
            {
                cancle(null, null);
            }

        }
        //计数器响应事件
        private void clipTimer_Tick(object sender = null, EventArgs e = null)
        {
            timerCnt++;
            //获取裁剪分析进度情况
            this._spatial.GetAnalyseResult(_UID, new UploadStringCompletedEventHandler(getClipResult));
        }

        private void getClipResult(object sender, UploadStringCompletedEventArgs e)
        {
            //获取裁剪结果名称
            string rltName = this._spatial.OnGetAnalyseResult(e);
            if (rltName == null || rltName == "")
            {
                return;
            }
            if (rltName == "*#*")
            {
                timerCnt = 0;
                MessageBox.Show("裁剪失败");
                return;
            }
            else
            {
                cancle(null, null);
                this.iMSMap1.EnableProgressBar = false;
                //裁剪分析成功,叠加显示分析后的图层
                showClipInfo(rltName, this._clipLayerAcsInfo.LayerInfoList[0].LayerType, this._serverAddr);
            }
        }
        //将分析后的图层叠加显示到地图上
        private void showClipInfo(string layerName, XClsType layerType, string svrAddr = "192.168.17.78", string gdbSvr = "MapGisLocal", string gdbName = "IMSWebGISGDB", string usr = "", string psw = "")
        {
            this.iMSMap1.SetErrorText("");
            //设置图层访问信息
            CLayerAccessInfo layerAccessInfo = new CLayerAccessInfo();
            layerAccessInfo.GdbInfo = new CGdbInfo();
            layerAccessInfo.GdbInfo.GDBName = gdbName;
            layerAccessInfo.GdbInfo.GDBSvrName = gdbSvr;
            layerAccessInfo.GdbInfo.User = usr;
            layerAccessInfo.GdbInfo.Password = psw;
            //设置图层信息
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
            this._vLayer = new VectorLayer();
            this.iMSMap1.AddChild(this._vLayer);
            this._vLayer.ServerAddress = svrAddr;
            this._vLayer.LayerObj = openLayer;
            //加载裁剪分析后的图层
            this._vLayer.LoadLayer(openLayer, new UploadStringCompletedEventHandler(onSingleLayerLoad));

        }
        private void onSingleLayerLoad(Object sender, UploadStringCompletedEventArgs e)
        {
            COperResult rlt = this._vLayer.OnLoadLayer(e);
            if (rlt.OperResult)
            {
                this.flag = 0;
                this.iMSMap1.SetErrorText("");
                successFlag = 1;
            }

        }

        //取消分析
        private void cancle(object sender, RoutedEventArgs e)
        {
            if (this.clipTimer != null)
            {
                this.clipTimer.Stop();
                timerCnt = 0;
            }
        }
        //清除裁剪的图形
        private void removeClipLayer(object sender, RoutedEventArgs e)
        {
            cancle(null, null);
            if (this._vLayer != null)
            {
                if (successFlag == 1 || successFlag.Equals(1))
                {
                    flag = 1;
                    this.iMSMap1.RemoveChild(this._vLayer);
                    this._vLayer = null;
                    successFlag = 0;
                    this.iMSMap1.SetErrorText("");
                }
            }

        }
        //地图加载时获取访问地址
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

    }
}
