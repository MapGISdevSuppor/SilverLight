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
using System.ComponentModel;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZdimsExtends.style;
using ZdimsExtends.drawing;
using ZDIMS.Util;
using System.Windows.Browser;

namespace SilverlightDemo
{
    public partial class DrawGraphics : Page
    {
        //自由绘制对象
        public freeDrawing drawingCtl;
        string TileLayerAddress = "";
        //预定义点样式对象
        private IMSSimpleMarkerSymbol _markStyle;
        //预定义线样式对象
        private IMSPredefinedLineStyle _lineStyle;
        //预定义填充对象
        private IMSSimpleFillStyle _polygonStyle;
        //点样式类型参数
        private MarkSymbolStyle markType;
        //线样式参数
        private LineType lineType= LineType.DashDot;
        //区域填充样式参数
        private FillSymbol fillType;
        public DrawGraphics()
        {
            InitializeComponent();
            //添加地图准备完毕事件
            this.iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);
            this.iMSMap1.MapInitComplete += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapInitComplete);
        }

        void iMSMap1_MapInitComplete(ZDIMS.Event.IMSMapEvent e)
        {
            //添加地图容器右键菜单
            MenuItem item = new MenuItem();
            item.Header = "取消选择";
            this.iMSMap1.ContextMenu.Items.Add(item);
            item.Click += new RoutedEventHandler(item_Click);
        }
        /// <summary>
        /// 右键菜单“取消选择”，监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click(object sender, RoutedEventArgs e)
        {
            //取消当前选择的对象
            //this.drawingCtl.FreeDrawingType = FreeHanderType.None;
            //this.drawingCtl.DrawingOverCallback = null;
            this.graphicsLayer.DrawingOverCallback = null;
            this.graphicsLayer.DrawingType = DrawingType.None;
        }
        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                //初始化自由绘制对象
                this.drawingCtl = new freeDrawing(this.iMSMap1);
            }
        }
      /// <summary>
      /// 添加预定义样式点对象
      /// </summary>
      /// <param name="logPntArr"></param>
        private void mark(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            //初始化点样式对象
            _markStyle = new IMSSimpleMarkerSymbol();
            //设置点显示样式
            _markStyle.SymbolStyle = this.markType;
            _markStyle.Size = 20;//点大小
            //添加点样式到标注里面
            IMSMark mark = new IMSMark(_markStyle.control, ZDIMS.Interface.CoordinateType.Logic);
            //设置坐标
            mark.X = logPntArr[0].X;
            mark.Y = logPntArr[0].Y;
            mark.EnableAnimation = false;//不允许动态变换
            this.markLayer.AddMark(mark);//添加点样式
        }
        /// <summary>
        /// 绘制线下拉控件监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawLine_DropDownClosed(object sender, EventArgs e)
        {
            //根据选择对象，设置线样式
            if (this.DrawLine != null)
            {
                switch (((Image)(this.DrawLine.SelectionBoxItem)).Tag.ToString())
                {
                    case "Soild":
                        {
                            this.lineType = LineType.Solid;
                            break;
                        }
                    case "Dash":
                        {
                            lineType = LineType.Dash;
                            break;
                        }
                    case "DashDot":
                        {
                            lineType = LineType.DashDot;
                            break;
                        }
                    case "Dot":
                        {
                            lineType = LineType.Dot;
                            break;
                        }
                    case "DashDotDot":
                        {
                            lineType = LineType.DashDotDot;
                            break;
                        }
                }
                //设置绘制类型，添加绘制结束监听事件
                this.drawingCtl.FreeDrawingType = FreeHanderType.None;
                //this.drawingCtl.DrawingOverCallback = null;
                //初始化临时绘制线对象
                IMSPredefinedLineStyle lin = new IMSPredefinedLineStyle();
                lin.LineSymbol = this.lineType;//设置线样式
                this.graphicsLayer.DrawingType = DrawingType.Polyline;

                this.graphicsLayer.RemoveGraphics(this.graphicsLayer.DrawingObj);
              
                this.graphicsLayer.DrawingObj = lin;
                this.graphicsLayer.DrawingObj.Visibility = System.Windows.Visibility.Collapsed;
                this.graphicsLayer.DrawingOverCallback += new DrawingEventHandler(PrelineStyle);
                this.graphicsLayer.AddGraphics(lin);
                //this.drawingCtl.drawObj = lin;//将临时绘制对象赋给图形绘制控件
                //this.drawingCtl.DrawingOverCallback += new freeDrawing.DrawEventHanders(PrelineStyle);
            }
        }
        /// <summary>
        /// 绘制预定义线样式对象
        /// </summary>
        /// <param name="logPntArr"></param>
        //private void PrelineStyle(List<Point> logPntArr)//(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        //{
        private void PrelineStyle(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            //初始化线样式对象
            this._lineStyle = new IMSPredefinedLineStyle();        
            this.graphicsLayer.AddGraphics(this._lineStyle);
            this._lineStyle.LineSymbol = this.lineType;//设置线的显示样式
            this._lineStyle.Draw(logPntArr);//添加坐标，绘制线
        }
        /// <summary>
        /// 绘制预定义区填充样式
        /// </summary>
        /// <param name="logPntArr"></param>
        void PrePolygon(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            //初始化区样式对象
            this._polygonStyle = new IMSSimpleFillStyle();
            this.graphicsLayer.AddGraphics(this._polygonStyle);//添加区对象
            this._polygonStyle.FillSymbol = fillType;//设置填充样式
            //this._polygonStyle.StrokeColor = Colors.LightGray;//多边形边框颜色
            //this._polygonStyle.FillColor = Colors.Red;//填充颜色
            this._polygonStyle.Draw(logPntArr);//绘制多边形
        }
        /// <summary>
        /// 绘制自由线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawFreeLine_Click(object sender, RoutedEventArgs e)
        {
            this.graphicsLayer.DrawingType = DrawingType.None;
            this.graphicsLayer.DrawingOverCallback = null;
            //设置绘制类型
            this.drawingCtl.FreeDrawingType = FreeHanderType.FreeLine;
            //添加绘制结束回调事件
            //this.drawingCtl.DrawingOverCallback += new freeDrawing.DrawEventHanders(freeline);
        }
        IMSPredefinedLineStyle line;
        List<Point> lg = new List<Point>();
        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="logPntArr"></param>
        void freeline(List<Point> logPntArr)
        {
            line = new IMSPredefinedLineStyle();
            this.graphicsLayer.AddGraphicsAt(line,1000000);
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.Draw(logPntArr);
        }
        /// <summary>
        /// 绘制自由面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawFreePolygon_Click(object sender, RoutedEventArgs e)
        {
            this.graphicsLayer.DrawingType = DrawingType.None;
            this.graphicsLayer.DrawingOverCallback = null;
            //设置绘制类型
            this.drawingCtl.FreeDrawingType = FreeHanderType.FreePolygon;
            //添加绘制结束事件
          //  this.drawingCtl.DrawingOverCallback += new freeDrawing.DrawEventHanders(freepolygon);
        }
        void freepolygon(List<Point> logPntArr)
        {
            //显示绘制的自由面
            IMSPolygon polygon = new IMSPolygon(CoordinateType.Logic);
            polygon.Stroke = new SolidColorBrush(Colors.Red);//设置边框颜色
            polygon.Shape.Fill = new SolidColorBrush(Colors.Red);//填充颜色
            polygon.Shape.Fill.Opacity = 0.5;//填充区域透明度
            this.graphicsLayer.AddGraphics(polygon);//添加对象
            polygon.Draw(logPntArr);//绘制对象
        }
        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawRect_Click(object sender, RoutedEventArgs e)
        {
            this.drawingCtl.FreeDrawingType = FreeHanderType.None;
           // this.drawingCtl.DrawingOverCallback = null;
            //设置绘制类型，添加绘制结束事件
            this.graphicsLayer.DrawingType = DrawingType.Rectangle;
            this.graphicsLayer.DrawingOverCallback += new DrawingEventHandler(rect);
        }
        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        void rect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            //定义矩形对象，添加到绘图层
            IMSRectangle rect = new IMSRectangle(CoordinateType.Logic);
            rect.EnableEdit = false;
            this.graphicsLayer.AddGraphics(rect);
            rect.Draw(logPntArr);
        }
        /// <summary>
        /// 绘制圆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawCircle_Click(object sender, RoutedEventArgs e)
        {
            this.drawingCtl.FreeDrawingType = FreeHanderType.None;
            this.drawingCtl.DrawingOverCallback = null;
            //设置绘制类型，添加绘制结束事件
            this.graphicsLayer.DrawingType = DrawingType.Circle;
            this.graphicsLayer.DrawingOverCallback += new DrawingEventHandler(circle);
        }
        /// <summary>
        /// 绘制圆
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        void circle(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            //定义圆对象，添加到绘图层
            IMSCircle cir = new IMSCircle(CoordinateType.Logic);
            this.graphicsLayer.AddGraphics(cir);
            cir.Draw(logPntArr);
        }
        /// <summary>
        /// 清除绘制的对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, RoutedEventArgs e)       
        {
            //移除图形
            this.drawingCtl.RemoveAllGraphics();
            this.graphicsLayer.RemoveAll();
            this.markLayer.RemoveAll();
            this.iMSMap1.OperType = ZDIMS.Util.IMSOperType.Drag;
        }
        /// <summary>
        /// 添加点对象
        /// </summary>
        private void addPnt()
        {
            //根据选择的点样式，设置显示样式
            switch (((Image)(this.DrawPnt.SelectionBoxItem)).Tag.ToString())
            {
                case "圆":
                    {
                        markType = MarkSymbolStyle.Circle;
                        break;
                    }
                case "矩形":
                    {
                        markType = MarkSymbolStyle.Square;
                        break;
                    }
                case "三角形":
                    {
                        markType = MarkSymbolStyle.Triangle;
                        break;
                    }
                case "扇形":
                    {
                        markType = MarkSymbolStyle.Sector;
                        break;
                    }
                case "菱形":
                    {
                        markType = MarkSymbolStyle.Diamond;
                        break;
                    }
                case "五角星":
                    {
                        markType = MarkSymbolStyle.Star;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            //设置自由绘制对象绘图类型为None
            this.drawingCtl.FreeDrawingType = FreeHanderType.None;
            //设置绘图层绘图类型为画点
            this.graphicsLayer.DrawingType = DrawingType.Point;
            //添加绘制结束事件
            this.graphicsLayer.DrawingOverCallback += mark;
            //设置绘制类型，添加监听事件
        }
        /// <summary>
        /// 绘制点下拉控件监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawPnt_DropDownClosed(object sender, EventArgs e)
        {
            //添加点
            addPnt();
        }     
        /// <summary>
        /// 多边形绘制下拉控件监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawPolygon_DropDownClosed(object sender, EventArgs e)
        {
            //根据选择类型设置多边形填充样式
            if (this.DrawPolygon != null)
            {
                switch (((Image)(this.DrawPolygon.SelectionBoxItem)).Tag.ToString())
                {
                    case "颜色填充":
                        {
                            this.fillType = FillSymbol.Solid;
                            break;
                        }
                    case "斜线":
                        {
                            fillType = FillSymbol.Slash;
                            break;
                        }
                    case "反斜线":
                        {
                            fillType = FillSymbol.BackSlash;
                            break;
                        }
                    case "网格":
                        {
                            fillType = FillSymbol.Cross;
                            break;
                        }
                    case "横线":
                        {
                            fillType = FillSymbol.Horizontal;
                            break;
                        }
                    case "竖线":
                        {
                            fillType = FillSymbol.Vertical;
                            break;
                        }

                }
                this.drawingCtl.FreeDrawingType = FreeHanderType.None;
                //定义多边形填充样式对象
                IMSSimpleFillStyle fill = new IMSSimpleFillStyle();
                fill.FillSymbol = this.fillType;
                this.graphicsLayer.DrawingObj = fill;
                //设置绘制类型，监听绘制结束事件,绘制区
                this.graphicsLayer.DrawingType = DrawingType.Polygon;
                //移除默认的绘制多边形临时对象
                this.graphicsLayer.RemoveGraphics(this.graphicsLayer.DrawingObj);
                this.graphicsLayer.DrawingObj = fill;//设置自定义临时绘制对象
                //设置临时绘制对象初始不可见
                this.graphicsLayer.DrawingObj.Visibility = System.Windows.Visibility.Collapsed;
                //设置绘制结束事件
                this.graphicsLayer.DrawingOverCallback += new DrawingEventHandler(PrePolygon);
                //将临时绘制对象添加到绘图层
                this.graphicsLayer.AddGraphics(fill);
               

            }
        }
        //地图加载时获取访问地址
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            tileLayer1.ServerAddress = TileLayerAddress;
        }


    }
}

