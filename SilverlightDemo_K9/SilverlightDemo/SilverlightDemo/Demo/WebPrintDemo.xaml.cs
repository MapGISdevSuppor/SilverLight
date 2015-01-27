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
using System.Windows.Media.Imaging;
using System.IO;
using ZDIMS.Map;
using ZdimsExtends.WebPrint.Scale;
using ZdimsExtends.WebPrint.BaseControl;
using ZdimsExtends.WebPrint.Mark;
using ZdimsExtends.WebPrint.ImageControl;
using System.Windows.Printing;
using ZdimsExtends.WebPrint.Compass;
using ZdimsExtends.WebPrint.Text;
using System.Text.RegularExpressions;
using ZdimsExtends.style;
using ZDIMS.Drawing;
using System.ComponentModel;
using ZDIMS.Util;
using System.Windows.Browser;
namespace SilverlightDemo
{
    public partial class WebPrintDemo : Page
    {
        string TileLayerAddress = "";
        GraphicsBase shape;//图形对象
        private ChooseCompassDialog Comprassdialog;//指北针类型选择对象
        private IMSMarkerSymbol _markers;//点对象
        private BaseComprass Comprass;//指北针对象
        public WebPrintDemo()
        {
            InitializeComponent();
          
        }
        /// <summary>
        /// 地图准备完毕处理事件
        /// </summary>
        /// <param name="e"></param>
        void iMSMap1_MapInitComplete(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                //菜单项添加到地图容器的右键菜单
                MenuItem item = new MenuItem();
                item.Header = "取消选择";
                this.iMSMap1.ContextMenu.Items.Add(item);
                item.Click += new RoutedEventHandler(item_Click);
            }
        }
        /// <summary>
        /// 右键菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click(object sender, RoutedEventArgs e)
        {
            if (this.shape != null)
            {
                this.shape.EnableEdit = false;
                this.shape = null;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            FocusManager.GetFocusedElement();
            base.OnKeyDown(e);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            foreach (UIElement l in VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition(null), this))
            {
                if (l is Polyline || l is Polygon)
                {
                    return;
                }
                if (l is IMSMap || l is EditAlternationScaleBar || l is EditBaseContainer || l is EditContainer)
                {
                    this.iMSMap1.OperType = ZDIMS.Util.IMSOperType.None;
                    this.adorn.AdornedElement = l as FrameworkElement;
                    adorn.adorned_MouseLeftButtonDown(l, e);
                    break;
                }
            }
            base.OnMouseLeftButtonDown(e);
        }
        /// <summary>
        ///   添加比例尺控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddScale_Click(object sender, RoutedEventArgs e)
        {
                //添加比例尺控件
                EditAlternationScaleBar bar = new EditAlternationScaleBar();
                bar.MapContainer = this.iMSMap1;
                bar.Width = 112;
                bar.Height = 24;
                this.container.Children.Add(bar);
     
        }

        private void addMark(object sender, RoutedEventArgs e)
        {
            MapSite m = new MapSite();

            //EditMark m = new EditMark();
            //m.Width = m.Height = 40;
            this.container.Children.Add(m);
        }
        /// <summary>
        /// 添加指北针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCompass1(object sender, RoutedEventArgs e)
        {
            //添加指北针类型选择对话框
            if (this.Comprassdialog == null)
            {
                this.Comprassdialog = new ChooseCompassDialog();
                //监听选择类型处理事件
                this.Comprassdialog.ChooseOk += new ChooseCompassDialogEvent(Comprassdialog_ChooseOk);
            }
            this.Comprassdialog.Show();//显示对话框
        }
        /// <summary>
        /// 选择处理事件
        /// </summary>
        /// <param name="Type"></param>
        void Comprassdialog_ChooseOk(CompassType Type)
        {
            //根据选择的类型，添加对应得指北针类型
            switch (Type)
            {
                case CompassType.TragleCompass:
                    Comprass = new TragleCompass();
                    break;
                case CompassType.trangel2Comprass:
                    Comprass = new trangel2Comprass();
                    break;
                case CompassType.DoubleTragleComprass:
                    Comprass = new DoubleTragleComprass();
                    break;
                case CompassType.CrossComprass:
                    Comprass = new CrossComprass();
                    break;
                case CompassType.Compross6:
                    Comprass = new Compross6();
                    break;
                case CompassType.Compross5:
                    Comprass = new Compross5();
                    break;
                case CompassType.CircleComprass:
                    Comprass = new CircleComprass();
                    break;
                case CompassType.ArrossCompross:
                    Comprass = new ArrossCompross();
                    break;
            }
            this.container.Children.Add(Comprass);
        }
        /// <summary>
        /// 查看打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printview(object sender, RoutedEventArgs e)
        {
            WriteableBitmap g = new WriteableBitmap(this.container, null);
            PrintPreviewWindow h = new PrintPreviewWindow();
            h.bitmap = g;
            h.Show();
        }
        /// <summary>
        /// 添加文本控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addText_Click(object sender, RoutedEventArgs e)
        {
            EditText txt = new EditText();
            this.container.Children.Add(txt);
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            //添加图片控件
            EditImage img = new EditImage();
            img.Width = img.Height = 40;
            this.container.Children.Add(img);
        }
        /// <summary>
        /// 添加地图的经纬图框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addFrame_Click(object sender, RoutedEventArgs e)
        {
            //设置图框控件的属性
            Frame.MapContainer = this.iMSMap1;
            Frame.enableFrame = true;
        }
        /// <summary>
        /// 视图内容另存为图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveImg_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap bm = new WriteableBitmap(this.container, null);
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Image Files (*.png, *.jpg)|*.png;*.jpg";
            if (dlg.ShowDialog() == true)
            {
                using (var pngStream = GetPngStream(bm))
                using (var file = dlg.OpenFile())
                {
                    byte[] binaryData = new Byte[pngStream.Length];
                    long bytesRead = pngStream.Read(binaryData, 0, (int)pngStream.Length);
                    file.Write(binaryData, 0, (int)pngStream.Length);
                    file.Flush();
                    file.Close();
                }
            }
        }
        /// <summary>
        /// 获取视图内容的图片流
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private Stream GetPngStream(WriteableBitmap bmp)
        {
            EditableImage imageData = new EditableImage(bmp.PixelWidth, bmp.PixelHeight);
            for (int y = 0; y < bmp.PixelHeight; ++y)
            {
               for (int x = 0; x < bmp.PixelWidth; ++x)
                {
                    int pixel = bmp.Pixels[bmp.PixelWidth * y + x];
                    imageData.SetPixel(x, y,

                                (byte)((pixel >> 16) & 0xFF),

                                (byte)((pixel >> 8) & 0xFF),

                                (byte)(pixel & 0xFF),

                                (byte)((pixel >> 24) & 0xFF)

                                );
                }
            }
            return imageData.GetStream();
        }
        /// <summary>
        /// 删除地图的经纬图框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delFrame_Click(object sender, RoutedEventArgs e)
        {
            Frame.enableFrame = false;
        }
        /// <summary>
        /// 打印地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintClick_Click(object sender, RoutedEventArgs e)
        {
            //调用打印机制打印
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += (s, ee) =>
            {
                ee.PageVisual = this.container;
                ee.HasMorePages = false;
            };
            doc.Print("Map");
        }
        /// <summary>
        /// 设置视图框大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSize_Click(object sender, RoutedEventArgs e)
        {
            this.iMSMap1.Width = this.container.Width = Convert.ToDouble(this.W.Text);
            this.iMSMap1.Height = this.container.Height = Convert.ToDouble(this.H.Text);
        }
        /// <summary>
        /// 设置图框显示位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetPos_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(this.container, Convert.ToDouble(this.XPos.Text));
            Canvas.SetTop(this.container, Convert.ToDouble(this.YPos.Text));
        }
        /// <summary>
        /// 文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            //判断文本输入是否合法
            if ((sender as TextBox).Text != "")
            {
                if (!m_blnIsNUmber((sender as TextBox).Text))
                {
                    (sender as TextBox).Text = "";
                    MessageBox.Show("输入数字！");
                    return;
                }
            }
        }
        /// <summary>
        /// 判断输入的字符是否为数字
        /// </summary>
        /// <param name="p_strVaule"></param>
        /// <returns></returns>
        private bool m_blnIsNUmber(string p_strVaule)
        {
            Regex m_regex = new System.Text.RegularExpressions.Regex("^(-?[0-9]*[.]*[0-9]{0,3})$");
            return m_regex.IsMatch(p_strVaule);
        }
        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPnt_Click(object sender, RoutedEventArgs e)
        {
            //初始化点对象
            IMSSimpleMarkerSymbol markers = new IMSSimpleMarkerSymbol();
            markers.IsShowMenum = true;
            //监听对象选择事件
            markers.ChossedMarkerOverCallBack += new IMSMarkerSymbol.ChossedMarkerHander(markers_ChossedShapeOverCallBack);
            //定义标注对象
            IMSMark mark;
            //将点对象添加到标注中
            mark = new IMSMark(markers.control, ZDIMS.Interface.CoordinateType.Logic);
            mark.EnableDrag = true;
            //设置点位置
            mark.X = 6.99919517012924;
            mark.Y = 30.671057152220655;
            mark.EnableAnimation = false;
            this.markLayer.AddMark(mark);//添加到标注图层
        }
        /// <summary>
        /// 点选择回调事件
        /// </summary>
        /// <param name="sender"></param>
        void markers_ChossedShapeOverCallBack(IMSMarkerSymbol sender)
        {
            //清除上选择的对象
            if (this.shape != null)
            {
                this.shape.EnableEdit = false;
            }
            if (_markers != null)
            {
                _markers.EnableEdit = false;
            }
            //设置当前选择对象
            _markers = sender;
            _markers.EnableEdit = true;
        }
        /// <summary>
        /// 添加线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            //初始化线样式对象
            IMSPredefinedLineStyle lines;
            lines = new IMSPredefinedLineStyle();
            lines.IsShowMenum = true;//允许显示右键菜单
            lines.IsCursor = true;//鼠标形状为手形
            //监听对象选择事件
            lines.ChossedMarkerOverCallBack += new IMSPredefinedLineStyle.ChossedMarkerHander(chossedCallBack);
            this.graphicesLaye.AddGraphics(lines);//图形添加到绘图层对象
            //设置线的坐标
            lines.Points.Add(new Point(6.99919517012924, 30.671057152220655));
            lines.Points.Add(new Point(20.99919517012924, 10.671057152220655));
            lines.LineSymbol = LineType.DashDotDot;//设置线的显示样式为：_ .._ ..
            lines.Draw();//绘制对象
        }
        /// <summary>
        /// 线对象选择回调事件
        /// </summary>
        /// <param name="sender"></param>
        void chossedCallBack(IMSPredefinedLineStyle sender)
        {
            //清除上次选择的对象
            if (this.shape != null)
            {
                this.shape.EnableEdit = false;
            }
            if (_markers != null)
            {
                _markers.EnableEdit = false;
            }
            //设置当前选择的对象
            this.shape = sender;
            this.shape.EnableEdit = true;
        }
        /// <summary>
        /// 添加多边形事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPolygon_Click(object sender, RoutedEventArgs e)
        {
            //初始化多边形对象
            IMSSimpleFillStyle fill = new IMSSimpleFillStyle();
            fill.IsShowMenum = true;//容许显示多边形右键菜单
            fill.IsCursor = true;//鼠标状态为手形
            fill.ChossedMarkerOverCallBack += callback;//监听图形选择事件
            this.graphicesLaye.AddGraphics(fill);//图形添加到绘图层中
            fill.Points.Add(new Point(10.99919517012924, 29.671057152220655));//设置多边形的坐标
            fill.Points.Add(new Point(19.99919517012924, 10.671057152220655));
            fill.Points.Add(new Point(12.99919517012924, 30.671057152220655));
            fill.Draw();//绘制多边形
        }
        /// <summary>
        /// 多边形选择后回调事件
        /// </summary>
        /// <param name="sender"></param>
        public void callback(GraphicsBase sender)
        {
            //上次选择对象清除
            if (this.shape != null)
            {
                this.shape.EnableEdit = false;
            }
            if (_markers != null)
            {
                _markers.EnableEdit = false;
            }
            //设置当前选择的对象
            this.shape = sender;
            this.shape.EnableEdit = true;
        }
        /// <summary>
        /// 控件加载完毕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //从js文件获取瓦片数据地址
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;//设置地图容器操作状态：移动地图
            tileLayer1.ServerAddress = TileLayerAddress;
            
        }

        private void iMSMap1_Loaded(object sender, RoutedEventArgs e)
        {
            this.iMSMap1.MapInitComplete += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapInitComplete);
            
           
        }
    }
}

