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
using System.ComponentModel;

using ZDIMS.Map;
using ZDIMS.Util;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMSDemo.Controls.MapDoc;
using ZDIMSDemo.Controls.Layer;

using ZDIMSDemo.Controls.FishEyeMenu;

namespace ZDIMSDemo.Controls.FishEyeToolMenu
{
    public partial class MapgisToolSubMenu : UserControl
    {
        #region 私有成员
        private FishEyeMenuH fishMenu = null;
        private IMSMap imsMap = null;
        private EnumMenuFun menuFun;
        private IMSEagleEye eagleEye = null;
        private Magnifier magnifier = null;
        private GraphicsLayer graphicsLayer = null;
        private MapDocDataViewer mapDocDataViewer = null;
        private IMSCatalog catalog = null;
        private LayerDataViewer layerDataViewer = null;
        private LayerConditionInput layerConditionInput = null;
        private ConditionInput mapDocConditionInput = null;
        private Project project = null;
        private PositionInfo positioninfo = null;
        private GPSSample gps = null;
        private BusAnalyse busAnalyseCtrl;
        private DisplaySet dispOption = null;
        private NetAnalyse netAnalyseCtrl;
        private ClipAnalyse clipAnalyse = null;
        private TopAnalyse topAnalyse = null;
        private Measure measureCtrl = null;
        private NavigationBar navigator = null;
        private LayerTopAnalyse LayertopAnalyse = null;
        private OverLayAnalyse overLayAnalyse = null;
        #endregion

        #region 公有属性

        [Browsable(false),DefaultValue(EnumMenuFun.Display)]
        public EnumMenuFun MenuFun
        {
            get { return menuFun; }
            set
            { 
                menuFun = value;
                this.ReCreate(value);
            }
        }

        public IMSEagleEye EagleEye
        {
            get { return eagleEye; }
            set { eagleEye = value; }
        }

        public IMSMap ImsMap
        {
            get { return this.imsMap; }
            set
            {
                this.imsMap = value;
                if (value != null && navigator != null && navigator.MapContainer == null)
                    navigator.MapContainer = value;
            }
        }

        /// <summary>
        /// 绘图层
        /// </summary>
        public GraphicsLayer GraphicsLayer
        {
            get { return graphicsLayer; }
            set
            {
                graphicsLayer = value;
                mapDocDataViewer.GraphicsLayer = value;
                layerDataViewer.GraphicsLayer = value;
                MapDocEditor.GraphicsLayer = graphicsLayer;
                LayerEditor.GraphicsLayer = graphicsLayer;
            }
        }

        public MarkLayer MarkLayer { get; set; }

         /// <summary>
        /// 编辑控件
        /// </summary>
        public MapDocEditor MapDocEditor { get; set; }

        /// <summary>
        /// 图层编辑控件
        /// </summary>
        public LayerEditor LayerEditor { get; set; }

        public IMSCatalog IMSCatalog
        {
            set
            {
                this.catalog = value;
                mapDocDataViewer.IMSCatalog = value;
                layerDataViewer.IMSCatalog = value;
                MapDocEditor.IMSCatalog = value;
                LayerEditor.IMSCatalog = value;
            }
            get { return this.catalog; }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MapgisToolSubMenu()
        {
            InitializeComponent();
            fishMenu = new FishEyeMenuH();
            this.ResizeControl();
            this.LayoutRoot.Children.Add(fishMenu);
            this.Loaded += new RoutedEventHandler(OnLoaded);
            MapDocEditor = new MapDocEditor(catalog) { IsPopup = true };
            mapDocDataViewer = new MapDocDataViewer() { IsPopup = true };//, MapDocEditorObj = MapDocEditor };
            mapDocConditionInput = new ConditionInput(mapDocDataViewer) { IsPopup = true };
            this.BorderThickness = new Thickness(0);

            LayerEditor = new LayerEditor(catalog) { IsPopup = true };
            layerDataViewer = new LayerDataViewer() { IsPopup = true };//, LayerEditorObj = LayerEditor };
            layerConditionInput = new LayerConditionInput(layerDataViewer) { IsPopup = true };
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.c1.Width = fishMenu.Width;
            this.c1.Height = fishMenu.Height;
        }

        private void ResizeControl()
        {
            this.c1.Width = fishMenu.Width;
            this.c1.Height = fishMenu.Height;
        }

        private void ReCreate(EnumMenuFun menuFun)
        {
            if (this.ImsMap != null)
            {
                switch (menuFun)
                {
                    case EnumMenuFun.Display:
                        this.PrepareDisplayPics();
                        break;
                    case EnumMenuFun.Select:
                        this.PrepareSelectPics();
                        break;
                    case EnumMenuFun.Editor:
                        this.PrepareEditorPics();
                        break;
                    case EnumMenuFun.Anlysis:
                        this.PrepareAnlysisPics();
                        break;
                    case EnumMenuFun.View:
                        this.PrepareViewPics();
                        break;
                    default:
                        break;
                }
                this.ResizeControl();
            }
        }

        private void PrepareDisplayPics()
        {
            String[] displayItemPics = { "images/widget/Display/btn_01.png", 
                     "images/widget/Display/btn_02.png", "images/widget/Display/btn_03.png",
                     "images/widget/Display/btn_04.png", "images/widget/Display/btn_05.png", 
                     "images/widget/Display/tool_6.PNG", "images/widget/Display/tool_7.gif", 
                     "images/widget/Display/p8.jpg" };
            String[] displayItemToolTips = { "放大", "缩小", "移动", "复位", "刷新", "放大镜", "鹰眼", "标注管理" };
            this.fishMenu.ImageUris = displayItemPics;
            this.fishMenu.ImageToolTip = displayItemToolTips;
            List<Image> pics = this.fishMenu.ImageItems;
            pics[0].MouseLeftButtonDown += new MouseButtonEventHandler(ZoomIn_Click);
            pics[1].MouseLeftButtonDown += new MouseButtonEventHandler(ZoomOut_Click);
            pics[2].MouseLeftButtonDown += new MouseButtonEventHandler(MapMove_Click);
            pics[3].MouseLeftButtonDown += new MouseButtonEventHandler(MapRestore_Click);
            pics[4].MouseLeftButtonDown += new MouseButtonEventHandler(MapUpdate_Click);
            pics[5].MouseLeftButtonDown += new MouseButtonEventHandler(EagleEye_Click);
            pics[6].MouseLeftButtonDown += new MouseButtonEventHandler(Magnifier_Click);
            pics[7].MouseLeftButtonDown += new MouseButtonEventHandler(Marker_Click);
        }

        private void PrepareSelectPics()
        {
            String[] selectItemPics = {"images/widget/Select/btn_06.png","images/widget/Select/btn_07.png",
                      "images/widget/Select/btn_08.png","images/widget/Select/btn_09.png","images/widget/Select/btn_10.png",
                       "images/widget/Select/btn_11.png","images/widget/Select/btn_12.png","images/widget/Select/btn_13.png",
                       "images/widget/Select/btn_14.png","images/widget/Select/btn_15.png","images/widget/Select/btn_16.png"};
            this.fishMenu.ImageUris = selectItemPics;
            List<Image> pics = this.fishMenu.ImageItems;
            pics[0].MouseLeftButtonDown += new MouseButtonEventHandler(DotSelect_Click);
            pics[1].MouseLeftButtonDown += new MouseButtonEventHandler(RectSelect_Click);
            pics[2].MouseLeftButtonDown += new MouseButtonEventHandler(CircleSelect_Click);
            pics[3].MouseLeftButtonDown += new MouseButtonEventHandler(LineSelect_Click);
            pics[4].MouseLeftButtonDown += new MouseButtonEventHandler(PolySelect_Click);
            pics[5].MouseLeftButtonDown += new MouseButtonEventHandler(AttSelect_Click);
            pics[6].MouseLeftButtonDown += new MouseButtonEventHandler(DotConSelect_Click);
            pics[7].MouseLeftButtonDown += new MouseButtonEventHandler(RectConSelect_Click);
            pics[8].MouseLeftButtonDown += new MouseButtonEventHandler(CircleConSelect_Click);
            pics[9].MouseLeftButtonDown += new MouseButtonEventHandler(LineConSelect_Click);
            pics[10].MouseLeftButtonDown += new MouseButtonEventHandler(PolyConSelect_Click);
        }

        private void PrepareEditorPics()
        {
            String[] editorItemPics = {"images/widget/Edit/tool_21.png","images/widget/Edit/tool_22.png",
                                          "images/widget/Edit/tool_23.png"};
            this.fishMenu.ImageUris = editorItemPics;
            List<Image> pics = this.fishMenu.ImageItems;
            pics[0].MouseLeftButtonDown += new MouseButtonEventHandler(AddDot_Click);
            pics[1].MouseLeftButtonDown += new MouseButtonEventHandler(AddLine_Click);
            pics[2].MouseLeftButtonDown += new MouseButtonEventHandler(AddArea_Click);
        }

        private void PrepareAnlysisPics()
        {
            String[] anlysisItemPics = {"images/widget/Anlysis/btn_22.png","images/widget/Anlysis/btn_23.png",
                    "images/widget/Anlysis/btn_17.png","images/widget/Anlysis/btn_18.png","images/widget/Anlysis/btn_19.png","images/widget/Anlysis/btn_20.png",
                    "images/widget/Anlysis/btn_21.png"};
            this.fishMenu.ImageUris = anlysisItemPics;
            List<Image> pics = this.fishMenu.ImageItems;
            pics[0].MouseLeftButtonDown += new MouseButtonEventHandler(BusAnalyse_Click);
            pics[1].MouseLeftButtonDown += new MouseButtonEventHandler(NetAnalyse_Click);
            pics[2].MouseLeftButtonDown += new MouseButtonEventHandler(circleClip_click);
            pics[3].MouseLeftButtonDown += new MouseButtonEventHandler(polygonClip_click);
            pics[4].MouseLeftButtonDown += new MouseButtonEventHandler(OverLayerAnalyse_click);
            pics[5].MouseLeftButtonDown += new MouseButtonEventHandler(topAnalyse_click);
            pics[6].MouseLeftButtonDown += new MouseButtonEventHandler(project_click);
        }

        private void PrepareViewPics()
        {
            String[] viewItemPics ={"images/widget/View/btn_24.png","images/widget/View/btn_32.png",
                        "images/widget/View/btn_33.png","images/widget/View/btn_28.png","images/widget/View/btn_21.png"};
            this.fishMenu.ImageUris = viewItemPics;
            List<Image> pics = this.fishMenu.ImageItems;
            pics[0].MouseLeftButtonDown += new MouseButtonEventHandler(Navigator_Click);
            pics[1].MouseLeftButtonDown += new MouseButtonEventHandler(dispSet_Click);
            pics[2].MouseLeftButtonDown += new MouseButtonEventHandler(measure_Click);
            pics[3].MouseLeftButtonDown += new MouseButtonEventHandler(position_Click);
            pics[4].MouseLeftButtonDown += new MouseButtonEventHandler(gps_Click);
        }

        #region 按钮功能函数

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ImsMap != null)
                ImsMap.OperType = IMSOperType.ZoomIn;
           // MarkLayer.ManuallyAddMarkObj = null;
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ImsMap != null)
                ImsMap.OperType = IMSOperType.ZoomOut;
           // MarkLayer.ManuallyAddMarkObj = null;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapMove_Click(object sender, RoutedEventArgs e)
        {
            if (ImsMap != null)
                ImsMap.OperType = IMSOperType.Drag;
           // MarkLayer.ManuallyAddMarkObj = null;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (ImsMap != null)
                ImsMap.OperType = IMSOperType.Refresh;
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapRestore_Click(object sender, RoutedEventArgs e)
        {
            if (ImsMap != null)
                ImsMap.OperType = IMSOperType.Restore;
        }

        /// <summary>
        /// 鹰眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EagleEye_Click(object sender, RoutedEventArgs e)
        {
            if (eagleEye != null)
                eagleEye.AnimationOpenOrClose();
        }

        /// <summary>
        /// 放大镜
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Magnifier_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            ImsMap.OperType = IMSOperType.None;
            if (this.magnifier == null)
            {
                this.magnifier = new Magnifier() { MapContainer = ImsMap };
                this.ImsMap.AddChildAt(this.magnifier, 1000);
            }
            else
            {
                if (this.ImsMap.Contains(this.magnifier))
                {
                    this.ImsMap.RemoveChild(this.magnifier);
                }
                else
                {
                    this.magnifier = new Magnifier();
                    this.ImsMap.AddChildAt(this.magnifier, 1000);
                }
            }
        }

        /// <summary>
        /// 标注管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Marker_Click(object sender, RoutedEventArgs e)
        {
            MarkerEditer mark = new MarkerEditer();
            mark.MapContainer = this.ImsMap;
            mark.Show();
        }

        /// <summary>
        /// 点查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DotSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Point;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(OnDotSelect);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        private void OnDotSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (mapDocDataViewer != null && catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                mapDocDataViewer.DotSelect(gLayer, graphics, logPntArr);
            if (layerDataViewer != null && catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                layerDataViewer.DotSelect(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 拉框查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Rectangle;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(OnRectSelect);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        void OnRectSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (mapDocDataViewer != null && catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                mapDocDataViewer.RectSelect(gLayer, graphics, logPntArr);
            if (layerDataViewer != null && catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                layerDataViewer.RectSelect(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 画圆查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircleSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Circle;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(OnCircleSelect);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        void OnCircleSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (mapDocDataViewer != null && catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                mapDocDataViewer.CircleSelect(gLayer, graphics, logPntArr);
            if (layerDataViewer != null && catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                layerDataViewer.CircleSelect(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 线查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Polyline;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(OnLineSelect);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        void OnLineSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (mapDocDataViewer != null && catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                mapDocDataViewer.LineSelect(gLayer, graphics, logPntArr);
            if (layerDataViewer != null && catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                layerDataViewer.LineSelect(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 多边形查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolySelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Polygon;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(mapDocDataViewer.PloySelect);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        void OnPolySelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (mapDocDataViewer != null && catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                mapDocDataViewer.PloySelect(gLayer, graphics, logPntArr);
            if (mapDocDataViewer != null && catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                mapDocDataViewer.PloySelect(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 属性条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttSelect_Click(object sender, RoutedEventArgs e)
        {
            if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
            {
                mapDocConditionInput.SelectionType = SelectionType.Condition;
                mapDocConditionInput.QueryGeoObj = null;
                mapDocConditionInput.Show();
            }
            if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
            {
                mapDocConditionInput.SelectionType = SelectionType.Condition;
                mapDocConditionInput.QueryGeoObj = null;
                mapDocConditionInput.Show();
            }
        }

        /// <summary>
        /// 点击条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DotConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Point;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(DotConSelectCallback);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        private void DotConSelectCallback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 0)
            {
                Dot_2D dot = new Dot_2D();
                dot.x = logPntArr[0].X;
                dot.y = logPntArr[0].Y;
                if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                {
                    mapDocConditionInput.SelectionType = SelectionType.Both;
                    mapDocConditionInput.QueryGeoObj = dot;
                    mapDocConditionInput.Show();
                }
                if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                {
                    layerConditionInput.SelectionType = SelectionType.Both;
                    layerConditionInput.QueryGeoObj = dot;
                    layerConditionInput.Show();
                }
            }
        }

        /// <summary>
        /// 拉框条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Rectangle;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(RectConSelectCallback);
            }
            MarkLayer.ManuallyAddMarkObj = null;
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
                if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                {
                    mapDocConditionInput.SelectionType = SelectionType.Both;
                    mapDocConditionInput.QueryGeoObj = rect;
                    mapDocConditionInput.Show();
                }
                if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                {
                    layerConditionInput.SelectionType = SelectionType.Both;
                    layerConditionInput.QueryGeoObj = rect;
                    layerConditionInput.Show();
                }
            }
        }

        /// <summary>
        /// 画圆条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircleConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null && mapDocDataViewer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Circle;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(CircleConSelectCallback);
            }
            MarkLayer.ManuallyAddMarkObj = null;
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
                if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                {
                    mapDocConditionInput.SelectionType = SelectionType.Both;
                    mapDocConditionInput.QueryGeoObj = obj;
                    mapDocConditionInput.Show();
                }
                if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                {
                    layerConditionInput.SelectionType = SelectionType.Both;
                    layerConditionInput.QueryGeoObj = obj;
                    layerConditionInput.Show();
                }
            }
        }

        /// <summary>
        /// 画线条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Polyline;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(LineConSelectCallback);
            }
            MarkLayer.ManuallyAddMarkObj = null;
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
                if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                {
                    mapDocConditionInput.SelectionType = SelectionType.Both;
                    mapDocConditionInput.QueryGeoObj = line;
                    mapDocConditionInput.Show();
                }
                if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                {
                    layerConditionInput.SelectionType = SelectionType.Both;
                    layerConditionInput.QueryGeoObj = line;
                    layerConditionInput.Show();
                }
            }
        }

        /// <summary>
        /// 多边形条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolyConSelect_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Polygon;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(PolyConSelectCallback);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        private void PolyConSelectCallback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
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
                if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                {
                    mapDocConditionInput.SelectionType = SelectionType.Both;
                    mapDocConditionInput.QueryGeoObj = ploy;
                    mapDocConditionInput.Show();
                }
                if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                {
                    layerConditionInput.SelectionType = SelectionType.Both;
                    layerConditionInput.QueryGeoObj = ploy;
                    layerConditionInput.Show();
                }
            }
        }

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          private void AddDot_Click(object sender, RoutedEventArgs e)
          {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Point;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(OnAddDot);
            }
            MarkLayer.ManuallyAddMarkObj = null;
          }
          void OnAddDot(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
          {
            if (catalog.ActiveMapDoc.Display)
                MapDocEditor.AddDot(gLayer, graphics, logPntArr);
            if (catalog.ActiveLayerObj.Display)
                LayerEditor.AddDot(gLayer, graphics, logPntArr);
          }

        /// <summary>
        /// 添加线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Polyline;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(OnAddLine);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
         void OnAddLine(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (catalog.ActiveMapDoc.Display)
                MapDocEditor.AddLine(gLayer, graphics, logPntArr);
            if (catalog.ActiveLayerObj.Display)
                LayerEditor.AddLine(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 添加区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddArea_Click(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                graphicsLayer.DrawingType = DrawingType.Polygon;
                graphicsLayer.DrawingOverCallback = new DrawingEventHandler(OnAddArea);
            }
            MarkLayer.ManuallyAddMarkObj = null;
        }
        void OnAddArea(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (catalog.ActiveMapDoc.Display)
                MapDocEditor.AddArea(gLayer, graphics, logPntArr);
            if (catalog.ActiveLayerObj.Display)
                LayerEditor.AddArea(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 公交换乘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BusAnalyse_Click(object sender, RoutedEventArgs e)
        {
            graphicsLayer.DrawingType = DrawingType.None;
            if (busAnalyseCtrl == null)
                busAnalyseCtrl = new BusAnalyse(MarkLayer, graphicsLayer);
            if (catalog != null)
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
            if (this.ImsMap == null)
                return;
            if (overLayAnalyse == null)
            {
                overLayAnalyse = new OverLayAnalyse() { IsPopup = true };
            }
            //if (m_catalog.ActiveLayerObj != null && m_catalog.ActiveLayerObj.Display)
            //    m_overLayAnalyse.vectorObj = this.IMSCatalog.ActiveLayerObj;
            if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                overLayAnalyse.vectorObj = this.IMSCatalog.ActiveMapDoc;

            overLayAnalyse.IMSCatalog = this.IMSCatalog;
            overLayAnalyse.GraphicsLayerObj = GraphicsLayer;
            overLayAnalyse.Show();
        }

        /// <summary>
        /// 网络分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetAnalyse_Click(object sender, RoutedEventArgs e)
        {
            graphicsLayer.DrawingType = DrawingType.None;
            if (netAnalyseCtrl == null)
                netAnalyseCtrl = new NetAnalyse(MarkLayer, graphicsLayer);
            if (catalog != null)
            {
                if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
                    netAnalyseCtrl.VectorObj = catalog.ActiveLayerObj;
                if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                    netAnalyseCtrl.VectorObj = catalog.ActiveMapDoc;
                netAnalyseCtrl.Show();
            }
        }

        /// <summary>
        /// 圆裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void circleClip_click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (clipAnalyse == null)
            {
                clipAnalyse = new ClipAnalyse() { IsPopup = true };
            }
            clipAnalyse.IMSCatalog = this.IMSCatalog;
            clipAnalyse.GraphicsLayerObj = GraphicsLayer;
            this.graphicsLayer.DrawingType = DrawingType.Circle;
            this.graphicsLayer.DrawingOverCallback += new DrawingEventHandler(OnCircleClip);
        }
        private void OnCircleClip(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                clipAnalyse.CircleSelect(gLayer, graphics, logPntArr);
            //if (m_catalog.ActiveLayerObj != null && m_catalog.ActiveLayerObj.Display)
            //    m_clipAnalyse.CircleSelect(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 多边形裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void polygonClip_click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (clipAnalyse == null)
            {
                clipAnalyse = new ClipAnalyse() { IsPopup = true };
            }
            clipAnalyse.IMSCatalog = this.IMSCatalog;
            clipAnalyse.GraphicsLayerObj = GraphicsLayer;
            this.graphicsLayer.DrawingType = DrawingType.Polygon;
            this.graphicsLayer.DrawingOverCallback += new DrawingEventHandler(OnPolygonClip);
        }
        private void OnPolygonClip(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                clipAnalyse.PloySelect(gLayer, graphics, logPntArr);
            //if (m_catalog.ActiveLayerObj != null && m_catalog.ActiveLayerObj.Display)
            //    m_clipAnalyse.LayerPloySelect(gLayer, graphics, logPntArr);
        }

        /// <summary>
        /// 拓扑分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topAnalyse_click(object sender, RoutedEventArgs e)
        {
            if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
            {
                if (topAnalyse == null)
                {
                    topAnalyse = new TopAnalyse() { IsPopup = true };
                }
                topAnalyse.IMSCatalog = this.IMSCatalog;
                topAnalyse.GraphicsLayer = this.graphicsLayer;
                topAnalyse.Show();
            }
            if (catalog.ActiveLayerObj != null && catalog.ActiveLayerObj.Display)
            {
                if (LayertopAnalyse == null)
                {
                    LayertopAnalyse = new LayerTopAnalyse() { IsPopup = true };
                }
                LayertopAnalyse.IMSCatalog = this.IMSCatalog;
                LayertopAnalyse.GraphicsLayer = this.graphicsLayer;
                LayertopAnalyse.Show();
            }
        }

        /// <summary>
        /// 投影转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void project_click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (this.project == null)
            {
                project = new Project() { IsPopup = true };
            }
            project.IMSCatalog = this.IMSCatalog;
            //if (m_catalog.ActiveLayerObj != null && m_catalog.ActiveLayerObj.Display)
            //    m_project.vectorObj = this.IMSCatalog.ActiveLayerObj;
            if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
                project.vectorObj = this.IMSCatalog.ActiveMapDoc;
            project.Show();
        }

        /// <summary>
        /// 地图导航
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navigator_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (navigator == null)
            {
                navigator = new NavigationBar();
                navigator.MapContainer = this.ImsMap;
            }
            if (!this.ImsMap.Contains(this.navigator))
            {
                this.ImsMap.AddChild(this.navigator);
                Canvas.SetZIndex(this.navigator, 2000);
            }
            else
                this.ImsMap.RemoveChild(this.navigator);
        }

        /// <summary>
        /// 视图参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispSet_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (catalog.ActiveMapDoc != null && catalog.ActiveMapDoc.Display)
            {
                if (dispOption == null)
                {
                    dispOption = new DisplaySet();
                    dispOption.IMSCatalog = this.IMSCatalog;
                }
                dispOption.ShowAsModal();
            }
        }

        /// <summary>
        /// 测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void measure_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (measureCtrl == null)
            {
                measureCtrl = new Measure();
                measureCtrl.m_graphicsLayer = graphicsLayer;
            }
            measureCtrl.Show();
        }

        /// <summary>
        /// 位置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void position_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (this.positioninfo == null)
            {
                positioninfo = new PositionInfo(this.ImsMap) { IsPopup = true };
            }
            positioninfo.Show();
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gps_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImsMap == null)
                return;
            if (gps == null)
            {
                gps = new GPSSample();
                gps.MapContainer = this.ImsMap; 
            }
            gps.Show();
        }
        #endregion
    }
}
