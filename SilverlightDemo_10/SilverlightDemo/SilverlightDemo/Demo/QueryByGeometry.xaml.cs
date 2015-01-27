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
using ZDIMS.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMSDemo.Controls;
using ZDIMS.Util;
using ZDIMS.Event;
using ZDIMSDemo.Controls.Layer;
using ZdimsExtends.QueryControl;
using System.Windows.Browser;

namespace SilverlightDemo
{
    public partial class QueryByGeometry : Page
    {
        string VecLayerAddress = "";
        string TileLayerAddress = "";
        private LayerDataViewer layer_dataView = null;       //地图文档视图
        public QueryByGeometry()
        {
            InitializeComponent();
            //添加地图容器地图准备完毕事件
            this.iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
            this.vectorLayer.LayerOpenSuccEvent += new LayerOpenSuccEventHandler(vectorLayer_LayerOpenSuccEvent);
        }

        void vectorLayer_LayerOpenSuccEvent(IVector layer)
        {
            //设置图层状态
            CSetEnumLayerStatus setLayerStatus;
            //设置状态更新参数
            setLayerStatus = new CSetEnumLayerStatus();
            setLayerStatus.GdbIndex = 0;//图层所在gdb数据库索引号
            //设置索引号为0的图层状态：可查询
            setLayerStatus.LayerStatus = new EnumLayerStatus[1];
            setLayerStatus.LayerStatus[0] = EnumLayerStatus.Editable;
            //调用矢量图层的设置图层状态方法，设置图层状态
            this.vectorLayer.SetEnumLayerStatus(setLayerStatus, onSetSingleLayerStatus);
        }

        /// <summary>
        /// 地图加载就绪事件
        /// </summary>
        /// <param name="e"></param>
        void iMSMap1_MapReady(IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
               
                //初始化查询结果显示控件
                layer_dataView = new LayerDataViewer()
                {
                    activeLayer = this.vectorLayer,
                    GraphicsLayer = this.graphicsLayer
                };
                //地图容器右键菜单添加一项：取消鼠标状态
                this.iMSMap1.SetInfoText("                                             选择几何查询类型，然后查询；右键菜单“取消鼠标状态”", Colors.Black);
                MenuItem item = new MenuItem();
                item.Header = "取消鼠标状态";
                this.iMSMap1.ContextMenu.Items.Add(item);
                item.Click += new RoutedEventHandler(item_Click);
            }
        }
        void item_Click(object sender, RoutedEventArgs e)
        {
            this.graphicsLayer.DrawingType = DrawingType.None;
        }
        /// <summary>
        /// 更新图层返回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void onSetSingleLayerStatus(object sender, UploadStringCompletedEventArgs e)
        {
            this.iMSMap1.Refresh();
          
           // MessageBox.Show("图层状态更新成功！");
        }
        /// <summary>
        /// 控件加载完毕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //设置矢量、瓦片数据地址
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
            vectorLayer.ServerAddress = VecLayerAddress;
        }
        /// <summary>
        /// 几何查询类型设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selType != null)
            {
                CSetEnumLayerStatus setLayerStatus;
                setLayerStatus = new CSetEnumLayerStatus();
                setLayerStatus.GdbIndex = 0;
                setLayerStatus.LayerStatus = new EnumLayerStatus[1];
                setLayerStatus.LayerStatus[0] = EnumLayerStatus.Editable;
                this.vectorLayer.SetEnumLayerStatus(setLayerStatus, onSetSingleLayerStatus);

                switch (this.selType.SelectedIndex)
                {
                    case 0:
                        {//点查询
                            this.graphicsLayer.DrawingType = DrawingType.Point;
                            this.graphicsLayer.DrawingOverCallback += new DrawingEventHandler(DotSelect);

                            break;
                        }
                    case 1:
                        {//矩形查询
                            this.graphicsLayer.DrawingType = DrawingType.Rectangle;
                            this.graphicsLayer.DrawingOverCallback += RectSelect;
                            break;
                        }
                    case 2:
                        {//线查询
                            this.graphicsLayer.DrawingType = DrawingType.Polyline;
                            this.graphicsLayer.DrawingOverCallback += LineSelect;
                            break;
                        }
                    case 3:
                        {//圆查询
                            this.graphicsLayer.DrawingType = DrawingType.Circle;
                            this.graphicsLayer.DrawingOverCallback += CircleSelect;

                            break;
                        }
                    case 4:
                        {//圆查询
                            this.graphicsLayer.DrawingType = DrawingType.Polygon;
                            this.graphicsLayer.DrawingOverCallback += PolySelect;
                            break;
                        }
                }
            }

        }
        /// <summary>
        /// 点击查询回调
        /// </summary>
        private void DotSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 0)
            {
                Dot_2D dot = new Dot_2D();
                dot.x = logPntArr[0].X;
                dot.y = logPntArr[0].Y;
                this.select(dot, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 矩形查询回调
        /// </summary>
        private void RectSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                ZDIMS.BaseLib.Rect rect = new ZDIMS.BaseLib.Rect();
                rect.xmin = Math.Min(logPntArr[0].X, logPntArr[1].X);
                rect.xmax = Math.Max(logPntArr[0].X, logPntArr[1].X);
                rect.ymin = Math.Min(logPntArr[0].Y, logPntArr[1].Y);
                rect.ymax = Math.Max(logPntArr[0].Y, logPntArr[1].Y);
                this.select(rect, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 线查询回调
        /// </summary>
        private void LineSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            //获取线坐标
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
                this.select(line, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 圆形查询回调
        /// </summary>
        private void CircleSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            //获取圆的中心点、以及半径
            if (logPntArr.Count > 1)
            {
                Circle obj = new Circle();
                obj.Center = new Dot_2D()
                {
                    x = logPntArr[0].X,
                    y = logPntArr[0].Y
                };
                obj.Radius = Math.Sqrt(Math.Pow(logPntArr[0].X - logPntArr[1].X, 2) + Math.Pow(logPntArr[0].Y - logPntArr[1].Y, 2));
                this.select(obj, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 多边形查询回调
        /// </summary>
        private void PolySelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 2)
            {
                ZDIMS.BaseLib.Polygon poly = new ZDIMS.BaseLib.Polygon();
                poly.Dots = new Dot_2D[logPntArr.Count + 1];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    poly.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                poly.Dots[poly.Dots.Length - 1] = poly.Dots[0];
                this.select(poly, SelectionType.SpatialRange);
            }
        }

        private void select(Object geoObj, SelectionType selType)
        {
            //设置web查询参数
            CLayerSelectParam mapsel = new CLayerSelectParam();
            CWebSelectParam websel = new CWebSelectParam();
            websel.CompareRectOnly = this.vectorLayer.CompareRectOnly;
            websel.Geometry = geoObj;//几何查询范围
            if (geoObj != null)//几何查询图形类型
                websel.GeomType = (geoObj as IWebGeometry).GetGeomType();
            websel.MustInside = this.vectorLayer.MustInside;//查询要素是否在图形里面
            websel.NearDistance = 0;// this.vectorLayer.NearDistanse;//查询半径
            websel.SelectionType = selType;//查询类型：几何
            mapsel.WebSelectParam = websel;//web查询参数对象
            mapsel.PageCount = 0;//查询起始页数
            this.layer_dataView._lastSelectParam = mapsel;
            //调用矢量图层的查询方法查询
            this.vectorLayer.LayerSelectAndGetAtt(mapsel, this.layer_dataView.SelectCallback);
            if (this.HightLight.IsChecked == true)
            {//高亮显示：传入地图容器对象、查询参数、绘图层对象、矢量图层对象
                drawHighLight.AddhighLightFeatures(this.iMSMap1, mapsel, this.graphicsLayer1, this.vectorLayer);
                drawHighLight.reflash();
            }
        }
        /// <summary>
        /// 清除高亮显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.graphicsLayer1.RemoveAll();
            this.iMSMap1.Refresh();
        }

        private void CloseLayerView(object sender, RoutedEventArgs e)
        {
            if(graphicsLayer!=null){
                this.graphicsLayer.RemoveAll();
            }
            if (layer_dataView!=null)
            {
                this.layer_dataView.Visibility = Visibility.Collapsed;
            }
        }
        
    }
}
