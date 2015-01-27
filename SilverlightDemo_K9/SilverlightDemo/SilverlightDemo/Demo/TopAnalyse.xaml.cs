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
    public partial class TopAnalyse : Page
    {
        string TileLayerAddress = "";
        string VecLayerAddress = "";
        private GraphicsLayer m_gpLayer = null;
        private SpacialAnalyse _spatial;
        private SFeatureGeometry _firstFeature;
        private SFeatureGeometry _secondFeature;
        private IMSPolygon _firstPoly;
        private IMSPolygon _secondPoly;
        private long firstFeatID;
        private long secondFeatID;
        private int TFlag = 1;
        public TopAnalyse()
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
                this.iMSCatalog1.MapContainer = iMSMap1;

            }
        }


        private void TopAnalyseSubmit1(object sender, RoutedEventArgs e)
        {
            this.TFlag = 1;
            this.firstFeatID = 3095;
            this.secondFeatID = 3083;
            initTopAnalyse();

        }
        private void TopAnalyseSubmit2(object sender, RoutedEventArgs e)
        {
            this.TFlag = 2;
            this.firstFeatID = 3281;
            this.secondFeatID = 3282;
            initTopAnalyse();

        }
        private void TopAnalyseSubmit3(object sender, RoutedEventArgs e)
        {
            this.TFlag = 3;
            this.firstFeatID = 1886;
            this.secondFeatID = 559;
            initTopAnalyse();

        }
        private void initTopAnalyse()
        {

            if (this._firstPoly != null)
            {

                this._firstPoly.FlickerStop();
            }
            if (this._secondPoly != null)
            {

                this._secondPoly.FlickerStop();
            }
            if (m_gpLayer != null)
            {
                this.m_gpLayer.RemoveAll();
            }
            this.topResult.Content = "";

            string mapName = this.iMSCatalog1.ActiveMapDoc.MapDocName;
            if (mapName == null)
            {
                //设置水系为激活的状态
                if (this.m_gpLayer != null)
                {
                    this.iMSCatalog1.ActiveMapDoc = this.mapDoc;
                    this.mapDoc.ActiveLayerIndex = 2;

                }
            }
            else
            {
                if (this.iMSCatalog1.ActiveMapDoc.ActiveLayerIndex == -1)
                {
                    if (this.m_gpLayer != null)
                    {
                        //设置水系为激活的状态
                        this.iMSCatalog1.ActiveMapDoc = this.mapDoc;
                        this.mapDoc.ActiveLayerIndex = 2;
                    }
                }

            }
            this.mapDoc.UpdateAllLayerInfo(new UploadStringCompletedEventHandler(updateLayerStatus));

        }
        private void updateLayerStatus(object sender, UploadStringCompletedEventArgs e)
        {

            //实例化第一个拓扑分析的要素
            _firstFeature = new SFeatureGeometry();
            //实例化第二个拓扑分析的要素
            _secondFeature = new SFeatureGeometry();
            //实例化第一个拓扑分析要素的显示区的对象
            _firstPoly = new IMSPolygon();
            //实例化第二个拓扑分析要素的县市区的对象
            _secondPoly = new IMSPolygon();
            //根据要素id获取需要的数据
            CGetObjByID targetObj = new CGetObjByID();
            //设置第一个要素的id
            targetObj.FeatureID = firstFeatID;
            //设置第一个要素所在的图层号
            targetObj.LayerIndex = 2;
            //获取第一个要素空间信息 
            this.mapDoc.GetGeomByID(targetObj, FirstgetGemos);

        }
        public void FirstgetGemos(object sender, UploadStringCompletedEventArgs e)
        {
            SFeatureGeometry FirstResultFeature = this.mapDoc.OnGetGeomByID(e);
            if (FirstResultFeature != null)
            {
                this._firstFeature = FirstResultFeature;
                this._firstPoly = drawGetFeature(FirstResultFeature);
                //获取第二个要素的空间信息
                CGetObjByID targetObj2 = new CGetObjByID();
                //设置拓扑分析的第二个要素的id
                targetObj2.FeatureID = this.secondFeatID;
                //设置第二个要素所在的图层号
                targetObj2.LayerIndex = 2;
                //获取第二个要素空间信息 
                this.mapDoc.GetGeomByID(targetObj2, SelectgetGemos);
            }
            else
            {
                MessageBox.Show("获取第一个要素失败！");
                return;
            }

        }
        //获取第二个要素回调函数
        public void SelectgetGemos(object sender, UploadStringCompletedEventArgs e)
        {
            SFeatureGeometry SelectresutltGemo = this.mapDoc.OnGetGeomByID(e);
            if (SelectresutltGemo != null)
            {
                this._secondFeature = SelectresutltGemo;
                //显示拓扑分析的第二个要素
                this._secondPoly = drawGetFeature(SelectresutltGemo);

                //拓扑分析
                onTopSpanalyse();
            }
            else
            {
                MessageBox.Show("获取第二个要素失败！");
                return;
            }
        }

        private void onTopSpanalyse()
        {
            //闪烁第一个要素
            this._firstPoly.Flicker();
            //闪烁第二个要素
            this._secondPoly.Flicker();
            //设置拓扑分析类的参数
            CRegionRelationAnalyse obj = new CRegionRelationAnalyse();
            obj.Reg0 = this._firstFeature.RegGeom[0];
            obj.Reg1 = this._secondFeature.RegGeom[0];
            obj.NearDis = 0.002;
            //空间分析类
            this._spatial = new SpacialAnalyse(this.iMSCatalog1.ActiveMapDoc);
            //拓扑分析
            this._spatial.RegionRelationAnalyse(obj, onSubmit);
        }
        //拓扑分析结果
        private void onSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            string data = this._spatial.OnRegionRelationAnalyse(e).ToString();
            switch (data)
            {
                case "Unknown":
                    data = "未知类型";
                    break;
                case "Intersect":
                    data = "相交";
                    break;
                case "Disjoin":
                    data = "相离";
                    break;
                case "Include":
                    data = "包含";
                    break;
                case "Adjacent":
                    data = "相邻";
                    break;
                default:
                    data = "未知类型";
                    break;
            }
            //地图跳转到某个位置
            if (this.TFlag == 1)
            {
                this.iMSMap1.PanTo(114.24202324169921, 30.63291477441406);

            }
            else if (this.TFlag == 2)
            {
                this.iMSMap1.PanTo(114.2513662524414, 30.4964335390625);
            }
            else if (this.TFlag == 3)
            {
                this.iMSMap1.PanTo(114.25081666357421, 30.56174301611328);
            }
            _firstFeature = null;
            _secondFeature = null;
            this.topResult.Content = data;
        }

        public IMSPolygon drawGetFeature(SFeatureGeometry sfeatureTemp)
        {
            GRegion reg = sfeatureTemp.RegGeom[0];
            AnyLine arcLine = reg.Rings[0];
            Arc arc = arcLine.Arcs[0];
            //获取要素边界的点
            Dot_2D[] Arcpnts = arc.Dots;
            int arcpntNum = Arcpnts.Length;
            //绘制多边形
            IMSPolygon poly = new IMSPolygon(CoordinateType.Logic);
            m_gpLayer.AddGraphics(poly);
            for (int i = 0; i < arcpntNum; i++)
            {
                poly.Points.Add(new Point(arc.Dots[i].x, arc.Dots[i].y));
            }
            poly.Draw();

            return poly;
        }
        //清除闪烁的图形以及分析的结果
        private void ClearMap(object sender, RoutedEventArgs e)
        {
            if (this._firstPoly != null)
            {
                //要素1停止闪烁
                this._firstPoly.FlickerStop();
            }
            if (this._secondPoly != null)
            {
                //要素2停止闪烁
                this._secondPoly.FlickerStop();
            }
            if (m_gpLayer != null)
            {
                //清除拓扑分析要素的图形
                this.m_gpLayer.RemoveAll();
            }
            this.topResult.Content = "";
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
    }
}
