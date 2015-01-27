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
using ZDIMS.Map;
using ZDIMS.Util;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZDIMS.BaseLib;

namespace ZDIMSDemo.Controls.MapDoc
{
    public partial class ToolbarByMapDoc : UserControl
    {
        private IMSMap m_mapContainer = null;//地图容器对象
        private IMSEagleEye m_eagleEye = null;
        private GraphicsLayer m_graphicsLayer = null;
        private MapDocDataViewer m_mapDocDataViewer = null;
        private  NavigationBar m_navigator = null;
        private DisplaySet m_dispOption = null;
        private Measure m_measureCtrl = null;
        private Magnifier m_magnifier = null;
        private NetAnalyse netAnalyseCtrl;
        private BusAnalyse busAnalyseCtrl;
        public MarkLayer MarkLayer { get; set; }
        private OverLayAnalyse m_overLayAnalyse = null;
        private Project m_project = null;
        private PositionInfo m_positioninfo = null;
        private TopAnalyse m_topAnalyse = null;
        private ClipAnalyse m_clipAnalyse = null;
        public Measure MeasureCtrl
        {
            get { return m_measureCtrl; }
            set { m_measureCtrl = value; }
        }
        private GPSSample m_gps = null;
        /// <summary>
        /// GPS示例控件
        /// </summary>
        public GPSSample Gps
        {
            get { return m_gps; }
            set { m_gps = value; }
        }
        private IMSCatalog m_catalog = null;
        /// <summary>
        /// 显示参数设置控件
        /// </summary>
        public DisplaySet DispOption
        {
            get { return m_dispOption; }
            set { m_dispOption = value; }
        }
        /// <summary>
        /// 地图导航控件
        /// </summary>
        public NavigationBar NavigationBar
        {
            get { return m_navigator; }
            set { m_navigator = value; }
        }
        private ConditionInput m_conditionInput = null;
        /// <summary>
        /// 编辑控件
        /// </summary>
        public MapDocEditor MapDocEditor { get; set; }
        /// <summary>
        /// 条件对话框
        /// </summary>
        public ConditionInput ConditionInput { get { return m_conditionInput; } }
        public ToolbarByMapDoc()
        {
            InitializeComponent();
            m_mapDocDataViewer = new MapDocDataViewer() { IsPopup = true, MapDocEditorObj = MapDocEditor };
            m_conditionInput = new ConditionInput(m_mapDocDataViewer) { IsPopup = true };
            this.BorderThickness = new Thickness(0);
            MapDocEditor = new MapDocEditor(m_catalog) { IsPopup = true };
        }
        /// <summary>
        /// 地图容器对象
        /// </summary>
        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set { m_mapContainer = value;
                if(value!=null&&m_navigator!=null&&m_navigator.MapContainer==null)
                    m_navigator.MapContainer = value;            
            }
        }
        /// <summary>
        /// 鹰眼
        /// </summary>
        public IMSEagleEye EagleEye
        {
            get { return m_eagleEye; }
            set { m_eagleEye = value; }
        }
        /// <summary>
        /// 绘图层
        /// </summary>
        public GraphicsLayer GraphicsLayer
        {
            get { return m_graphicsLayer; }
            set { 
                m_graphicsLayer = value;
                m_mapDocDataViewer.GraphicsLayer = value;
                MapDocEditor.GraphicsLayer = m_graphicsLayer;
            }
        }
        /// <summary>
        /// 目录
        /// </summary>
        public IMSCatalog IMSCatalog
        {
            set {
                this.m_catalog = value;
                m_mapDocDataViewer.IMSCatalog = value;
                MapDocEditor.ActiveMapDoc = m_catalog.ActiveMapDoc;
            }
            get { return this.m_catalog; }
        }
        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null)
                m_mapContainer.OperType = IMSOperType.ZoomIn;
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null)
                m_mapContainer.OperType = IMSOperType.ZoomOut;
        }
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapMove_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null)
                m_mapContainer.OperType = IMSOperType.Drag;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null)
                m_mapContainer.OperType = IMSOperType.Refresh;
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapRestore_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null)
                m_mapContainer.OperType = IMSOperType.Restore;
        }
        /// <summary>
        /// 鹰眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EagleEye_Click(object sender, RoutedEventArgs e)
        {
            if (m_eagleEye != null)
                m_eagleEye.AnimationOpenOrClose();
        }
        /// <summary>
        /// 点查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DotSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Point;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(m_mapDocDataViewer.DotSelect);
            }
        }
        /// <summary>
        /// 拉框查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Rectangle;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(m_mapDocDataViewer.RectSelect);
            }
        }
        /// <summary>
        /// 画圆查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircleSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Circle;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(m_mapDocDataViewer.CircleSelect);
            }
        }
        /// <summary>
        /// 线查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Polyline;
                m_graphicsLayer.DrawingOverCallback= new DrawingEventHandler(m_mapDocDataViewer.LineSelect);
            }
        }
        /// <summary>
        /// 多边形查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PloySelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Polygon;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(m_mapDocDataViewer.PloySelect);
            }
        }
        /// <summary>
        /// 属性条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttSelect_Click(object sender, RoutedEventArgs e)
        {
            m_conditionInput.SelectionType = SelectionType.Condition;
            m_conditionInput.QueryGeoObj = null;
            m_conditionInput.Show();
        }
        /// <summary>
        /// 点击条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DotConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Point;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(DotConSelectCallback);
            }
        }
        private void DotConSelectCallback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {            
            if (logPntArr.Count > 0)
            {
                Dot_2D dot = new Dot_2D();
                dot.x = logPntArr[0].X;
                dot.y = logPntArr[0].Y;
                m_conditionInput.SelectionType = SelectionType.Both;
                m_conditionInput.QueryGeoObj = dot;
                m_conditionInput.Show();
            }
        }
        /// <summary>
        /// 拉框条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Rectangle;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(RectConSelectCallback);
            }
        }
        private void RectConSelectCallback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                ZDIMS.BaseLib.Rect rect = new ZDIMS.BaseLib.Rect();
                rect.xmin = Math.Min(logPntArr[0].X, logPntArr[1].X);
                rect.xmax = Math.Max(logPntArr[0].X, logPntArr[1].X);
                rect.ymin = Math.Min(logPntArr[0].Y, logPntArr[1].Y);
                rect.ymax = Math.Max(logPntArr[0].Y, logPntArr[1].Y);
                m_conditionInput.SelectionType = SelectionType.Both;
                m_conditionInput.QueryGeoObj = rect;
                m_conditionInput.Show();
            }
        }
        /// <summary>
        /// 画圆条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircleConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Circle;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(CircleConSelectCallback);
            }
        }
        private void CircleConSelectCallback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                Circle obj = new Circle();
                obj.Center = new Dot_2D()
                {
                    x = logPntArr[0].X,
                    y = logPntArr[0].Y
                };
                obj.Radius = Math.Sqrt(Math.Pow(logPntArr[0].X - logPntArr[1].X, 2) + Math.Pow(logPntArr[0].Y - logPntArr[1].Y, 2));
                m_conditionInput.SelectionType = SelectionType.Both;
                m_conditionInput.QueryGeoObj = obj;
                m_conditionInput.Show();
            }
        }
        /// <summary>
        /// 画线条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Polyline;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(LineConSelectCallback);
            }
        }
        private void LineConSelectCallback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                AnyLine line = new AnyLine();
                Arc arc = new Arc();
                arc.Dots = new Dot_2D[logPntArr.Count];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    arc.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                line.Arcs = new Arc[1] { arc };
                m_conditionInput.SelectionType = SelectionType.Both;
                m_conditionInput.QueryGeoObj = line;
                m_conditionInput.Show();
            }
        }
        /// <summary>
        /// 多边形条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PloyConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null && m_mapDocDataViewer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Polygon;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(PloyConSelectCallback);
            }
        }
        private void PloyConSelectCallback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 2)
            {
                ZDIMS.BaseLib.Polygon ploy = new ZDIMS.BaseLib.Polygon();
                ploy.Dots = new Dot_2D[logPntArr.Count + 1];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    ploy.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                ploy.Dots[ploy.Dots.Length - 1] = ploy.Dots[0];
                m_conditionInput.SelectionType = SelectionType.Both;
                m_conditionInput.QueryGeoObj = ploy;
                m_conditionInput.Show();
            }
        }

        private void Navigator_Click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (m_navigator == null)
            {
                m_navigator = new NavigationBar();
                m_navigator.MapContainer = this.MapContainer;
            }
            if (!this.MapContainer.Contains(this.m_navigator))
            {
                this.MapContainer.AddChild(this.m_navigator);
                Canvas.SetZIndex(this.m_navigator,2000);
            }
            else
                this.MapContainer.RemoveChild(this.m_navigator);
        }

        private void dispSet_Click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (m_dispOption == null)
            {
                m_dispOption = new DisplaySet();
                m_dispOption.IMSCatalog = this.IMSCatalog;
            }
            m_dispOption.ShowAsModal();
        }

        private void gps_Click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (m_gps == null)
            {
                m_gps = new GPSSample();
                m_gps.MapContainer = this.MapContainer;
            }
            m_gps.Show();
        }
        private void measure_Click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (m_measureCtrl == null)
            {
                m_measureCtrl = new Measure();
                m_measureCtrl.m_graphicsLayer = m_graphicsLayer;
            }
            m_measureCtrl.Show();
        }

        private void Magnifier_Click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (this.m_magnifier == null)
            {
                this.m_magnifier = new Magnifier();
                this.MapContainer.AddChildAt(this.m_magnifier, 1000);
            }
            else
            {
                if (this.MapContainer.Contains(this.m_magnifier))
                {
                    this.MapContainer.RemoveChild(this.m_magnifier);
                }
                else
                {
                    this.m_magnifier = new Magnifier();
                    this.MapContainer.AddChildAt(this.m_magnifier, 1000);
                }
            }
        }

        private void Marker_Click(object sender, RoutedEventArgs e)
        {
            MarkerEditer mark = new MarkerEditer();
            mark.MapContainer = this.MapContainer;
            mark.Show();
        }

        private void AddDot_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Point;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(MapDocEditor.AddDot);
            }
        }

        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Polyline;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(MapDocEditor.AddLine);
            }
        }

        private void AddArea_Click(object sender, RoutedEventArgs e)
        {
            if (m_graphicsLayer != null)
            {
                m_graphicsLayer.DrawingType = DrawingType.Polygon;
                m_graphicsLayer.DrawingOverCallback = new DrawingEventHandler(MapDocEditor.AddArea);
            }
        }

        private void NetAnalyse_Click(object sender, RoutedEventArgs e)
        {
            if (netAnalyseCtrl == null)
                netAnalyseCtrl = new NetAnalyse(MarkLayer, m_graphicsLayer);
            if (m_catalog != null)
            {
                netAnalyseCtrl.VectorObj = m_catalog.ActiveMapDoc;
                netAnalyseCtrl.Show();
            }
        }

        private void BusAnalyse_Click(object sender, RoutedEventArgs e)
        {
            if(busAnalyseCtrl==null)
                busAnalyseCtrl = new BusAnalyse(MarkLayer, m_graphicsLayer);
            if (m_catalog != null)
            {
                busAnalyseCtrl.Show();
            }
        }
        /// <summary>
        /// 叠加分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverLayerAnalyse_click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (m_overLayAnalyse == null)
            {
                m_overLayAnalyse = new OverLayAnalyse() { IsPopup = true };
            }
            m_overLayAnalyse.vectorObj = this.IMSCatalog.ActiveMapDoc;
            m_overLayAnalyse.IMSCatalog = this.IMSCatalog;
            m_overLayAnalyse.GraphicsLayerObj = GraphicsLayer;
            m_overLayAnalyse.Show();
        }
        /// <summary>
        /// 投影转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void project_click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (this.m_project == null)
            {
                m_project = new Project() { IsPopup = true };
            }
           // m_project.IMSCatalog = this.IMSCatalog;
            m_project.vectorObj = this.IMSCatalog.ActiveMapDoc;
            m_project.Show();
        }
        /// <summary>
        /// 位置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void position_Click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (this.m_positioninfo == null)
            {
                m_positioninfo = new PositionInfo(this.MapContainer) { IsPopup = true };
            }
            m_positioninfo.Show();
        }
        /// <summary>
        /// 拓扑分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topAnalyse_click(object sender, RoutedEventArgs e)
        {
            if (m_topAnalyse == null)
            {
                m_topAnalyse = new TopAnalyse() { IsPopup = true };
            }
            m_topAnalyse.IMSCatalog = this.IMSCatalog;
            m_topAnalyse.GraphicsLayer = this.m_graphicsLayer;
            m_topAnalyse.Show();
        }
        /// <summary>
        /// 圆裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void circleClip_click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (m_clipAnalyse == null)
            {
                m_clipAnalyse = new ClipAnalyse() { IsPopup = true };
            }
            m_clipAnalyse.IMSCatalog = this.IMSCatalog;
            m_clipAnalyse.GraphicsLayerObj = GraphicsLayer;
            this.m_graphicsLayer.DrawingType = DrawingType.Circle;
            this.m_graphicsLayer.DrawingOverCallback += new DrawingEventHandler(m_clipAnalyse.CircleSelect);
        }
        /// <summary>
        /// 多边形裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void polygonClip_click(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            if (m_clipAnalyse == null)
            {
                m_clipAnalyse = new ClipAnalyse() { IsPopup = true };
            }
            m_clipAnalyse.IMSCatalog = this.IMSCatalog;
            m_clipAnalyse.GraphicsLayerObj = GraphicsLayer;
            this.m_graphicsLayer.DrawingType = DrawingType.Polygon;
            this.m_graphicsLayer.DrawingOverCallback += new DrawingEventHandler(m_clipAnalyse.PloySelect);
        }
    }
}
