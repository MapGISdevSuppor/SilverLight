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
using ZDIMSDemo.Controls;
using ZDIMS.Map;
using ZDIMS.BaseLib;
using ZDIMS.Interface;
using ZDIMS.Util;
using ZDIMS.Drawing;
using ZDIMSDemo.Controls.Layer;

namespace ZDIMSDemo.Controls.Layer
{
    public partial class LayerTopAnalyse : BaseUserControl
    {
        private SFeatureGeometry _firstFeature = null;
        private SFeatureGeometry _secondFeature = null;
        private SpacialAnalyse _spatial = null;
        private SFeatureGeometry geoObj = null;
        private IMSCatalog m_catalog = null;//目录
        private GraphicsLayer m_graphicsLayer = null;
        private GraphicsBase m_graphics = null;
        private object obj1 = null;

        private int set;

        public IMSCatalog IMSCatalog
        {
            get { return m_catalog; }
            set { m_catalog = value; }

        }
        public GraphicsLayer GraphicsLayer
        {
            get { return m_graphicsLayer; }
            set { m_graphicsLayer = value; }
        }
        public LayerTopAnalyse()
        {
            InitializeComponent();
            _firstFeature = null;
            _secondFeature = null;
            geoObj = null;
            this.radioButton1.Content = "获取第一个区要素(未选取)";
            this.radioButton2.Content = "获取第二个区要素(未选取)";
            this.myDialog.OnClose += close;
        }

        public void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 进行拓扑分析
        /// </summary>
        private void submit(object sender, RoutedEventArgs e)
        {
            this.topRlt.Content = "";
            if (_firstFeature == null)
            {
                MessageBox.Show(" 进行拓扑分析的要素还未选取完成！");//进行拓扑分析的第一个要素还未选取完成！
                return;
            }
            if (_secondFeature == null)
            {
                MessageBox.Show("进行拓扑分析的要素还未选取完成！");
                //进行拓扑分析的第一个要素还未选取完成！
                return;
            }
            CRegionRelationAnalyse obj = new CRegionRelationAnalyse();
            obj.Reg0 = this._firstFeature.RegGeom[0];
            obj.Reg1 = this._secondFeature.RegGeom[0];
            string s = this.buffer.Text;
            obj.NearDis = Convert.ToDouble(s);
            if (obj.NearDis.ToString() == "")
            {
                MessageBox.Show("输入正确的半径！");
                return;
            }
            this._spatial = new SpacialAnalyse(this.IMSCatalog.ActiveMapDoc);
            this._spatial.RegionRelationAnalyse(obj, new UploadStringCompletedEventHandler(onSubmit));

        }
        /// <summary>
        /// 获取拓扑分析结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            this.topRlt.Content = data;
            _firstFeature = null;
            _secondFeature = null;
            radioButton1.IsChecked = false;
            radioButton2.IsChecked = false;
            this.radioButton1.Content = "获取第一个区要素(未选取)";
            this.radioButton2.Content = "获取第二个区要素(未选取)";
        }

        public void setMouseFun(object sender, RoutedEventArgs e)
        {

            int GdbIndex = this.IMSCatalog.ActiveLayerObj.ActiveGdbIndex;


            if (GdbIndex<0)
            {
                MessageBox.Show("在矢量图层中没有激活的图层！");
                if (sender is RadioButton)
                    (sender as RadioButton).IsChecked = false;
                return;
            }
            else
            {
                if (this.IMSCatalog.ActiveLayerObj.ActiveLayerIndex == -1)
                {
                    MessageBox.Show("在矢量图层中没有激活的图层！");
                    if (sender is RadioButton)
                        (sender as RadioButton).IsChecked = false;
                    return;
                }
            }

            bool flag = false;
            if (flag = Convert.ToBoolean(this.radioButton1.IsChecked))
            {
                set = 0;
            }
            else
            {
                set = 1;

            }

            this.m_graphicsLayer.DrawingType = DrawingType.Point;
            this.m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(Drawpoint_callback);

        }
        private void Drawpoint_callback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            this.m_graphicsLayer.DrawingOverCallback = null;
            this.m_graphicsLayer.RemoveAll();

            if (logPntArr.Count > 0)
            {
                Dot_2D dot = new Dot_2D();
                dot.x = logPntArr[0].X;
                dot.y = logPntArr[0].Y;
                obj1 = dot;
            }
            int GdbIndex = this.IMSCatalog.ActiveLayerObj.ActiveGdbIndex;
            if (GdbIndex < 0)
            {
                MessageBox.Show("在矢量图层中没有激活的图层！");
                return;
            }
            else
            {
                if (this.IMSCatalog.ActiveLayerObj.ActiveLayerIndex == -1)
                {
                    MessageBox.Show("在矢量图层中没有激活的图层！");
                    return;
                }
            }
            selFeature();
        }
        /// <summary>
        /// 选择要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selFeature()
        {
            CLayerSelectParam mapsel = new CLayerSelectParam();
            CWebSelectParam websel = new CWebSelectParam();
            websel.Geometry = obj1;
            websel.CompareRectOnly = this.IMSCatalog.ActiveLayerObj.CompareRectOnly;
            websel.GeomType = (obj1 as IWebGeometry).GetGeomType();

            websel.NearDistance = this.IMSCatalog.ActiveLayerObj.NearDistanse;
            websel.SelectionType = SelectionType.SpatialRange;
            websel.MustInside = this.IMSCatalog.ActiveLayerObj.MustInside;
            mapsel.WebSelectParam = websel;
            mapsel.PageCount = 0;
            this.IMSCatalog.ActiveLayerObj.LayerSelectAndGetAtt(mapsel, setFeature);
        }
        private void setFeature(object sender, UploadStringCompletedEventArgs e)
        {
            CLayerSelectAndGetAtt obj = this.IMSCatalog.ActiveLayerObj.OnLayerSelectAndGetAtt(e);
            if (obj == null)
            {
                MessageBox.Show("未获取到要素");
                this.m_graphicsLayer.DrawingType = DrawingType.Point;
                this.m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(Drawpoint_callback);
                return;
            }
            CAttDataTable curRltTable = obj.AttDS[this.IMSCatalog.ActiveLayerObj.ActiveGdbIndex].attTables[this.IMSCatalog.ActiveLayerObj.ActiveLayerIndex];
            if (obj.AttDS[this.IMSCatalog.ActiveLayerObj.ActiveGdbIndex].attTables[this.IMSCatalog.ActiveLayerObj.ActiveLayerIndex] != null)
            {
                CAttDataRow[] curRltRows = obj.AttDS[this.IMSCatalog.ActiveLayerObj.ActiveGdbIndex].attTables[this.IMSCatalog.ActiveLayerObj.ActiveLayerIndex].Rows;
                if (curRltRows == null)
                {
                    MessageBox.Show("未获取到要素");
                    this.m_graphicsLayer.DrawingType = DrawingType.Point;
                    this.m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(Drawpoint_callback);
                    return;
                }
                CAttDataRow row = curRltTable.Rows[0] as CAttDataRow;
                CLayerGetByID getGeo = new CLayerGetByID();
                getGeo.FeatureID = row.FID;
                getGeo.GdbIndex = this.IMSCatalog.ActiveLayerObj.ActiveGdbIndex;
                getGeo.LayerIndex = this.IMSCatalog.ActiveLayerObj.ActiveLayerIndex;
                this.IMSCatalog.ActiveLayerObj.GetGeomByFID(getGeo, flashFeature);
            }
            else
            {
                MessageBox.Show("未获取到要素");
                this.m_graphicsLayer.DrawingType = DrawingType.Point;
                this.m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(Drawpoint_callback);
            }
        }
        /// <summary>
        /// 闪烁查询到要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flashFeature(object sender, UploadStringCompletedEventArgs e)
        {
            geoObj = this.IMSCatalog.ActiveLayerObj.OnGetGeomByFID(e);
            if (geoObj == null)
            {
                return;
            }
            List<Point> pntArr = new List<Point>();
            for (int i = 0; i < geoObj.RegGeom[0].Rings.Length; i++)
                for (int j = 0; j < geoObj.RegGeom[0].Rings[i].Arcs.Length; j++)
                    for (int k = 0; k < geoObj.RegGeom[0].Rings[i].Arcs[j].Dots.Length; k++)
                        pntArr.Add(new Point(geoObj.RegGeom[0].Rings[i].Arcs[j].Dots[k].x, geoObj.RegGeom[0].Rings[i].Arcs[j].Dots[k].y));
            if (pntArr[0].X < this.IMSCatalog.ActiveLayerObj.MapContainer.WinViewBound.XMin || pntArr[0].X > this.IMSCatalog.ActiveLayerObj.MapContainer.WinViewBound.XMax ||
                pntArr[0].Y < this.IMSCatalog.ActiveLayerObj.MapContainer.WinViewBound.YMin || pntArr[0].Y > this.IMSCatalog.ActiveLayerObj.MapContainer.WinViewBound.YMax)
                this.IMSCatalog.ActiveLayerObj.MapContainer.PanTo(pntArr[0].X, pntArr[0].Y);
            m_graphics = new IMSPolygon(CoordinateType.Logic)
            {
                Points = pntArr
            };
            m_graphicsLayer.AddGraphics(m_graphics);
            m_graphics.FlickerOverCallback = new GraphicsFlickerOverDelegate(FlickerOverCallback);
            m_graphics.Draw();
            m_graphics.Flicker();
        }
        /// <summary>
        /// 闪烁完毕回调
        /// </summary>
        /// <param name="g"></param>
        private void FlickerOverCallback(GraphicsBase g)
        {
            m_graphicsLayer.RemoveGraphics(m_graphics);
            m_graphics = null;
            if (MessageBox.Show("查询成功。是否添加该要素？", "查询并闪烁成功", MessageBoxButton.OKCancel) == MessageBoxResult.OK)//"提示",Alert.YES|Alert.NO , this , onAlert , null , Alert.NO);  
            {
                if (set == 0)
                {
                    this._firstFeature = geoObj;
                    this.radioButton1.Content = "获取第一个区要素(已获取)";
                }
                if (set == 1)
                {
                    this._secondFeature = geoObj;
                    this.radioButton2.Content = "获取第二个区要素(已获取)";
                }
            }
        }
    }
}
