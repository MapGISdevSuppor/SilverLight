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
using ZDIMSDemo.Controls;
using ZDIMSDemo.Controls.MapDoc;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using ZDIMS.Util;
using ZDIMSDemo.MyControl;
using System.Windows.Browser;
namespace SilverlightDemo.Demo
{
    public partial class NetAnalyse : Page
    {
        //标注图层
        string VecLayerAddress = "";
        string TileLayerAddress = "";
        private IMSMark mark1 = null;
        private IMSMark mark2 = null;
        private IMSMark mark3 = null;
        private String pathDots = 114.26244962792968 + "," + 30.55578913671875 + "," + 114.27868539904786 + "," + 30.55466705944824 + "," + 114.29711952563477 + "," + 30.546514824584957 + ",";
        public string GDBName = "world";
        public string GDBSvrName = "MapGisLocal";
        public string Password = "";
        public string User = "";
        public string NetLayerName = "武汉道路网";
        private SpacialAnalyse _spatial;
        IMSPolyline m_road;
        public GraphicsLayer GraphicsLayer;
        List<IMSMark> m_stopList = new List<IMSMark>();
        private NetWindow netWindow = null;
        private Boolean SuccessFlag = true;
        private List<Point> LinePnts;
        public NetAnalyse()
        {
            InitializeComponent();
            iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
        }
        private void iMSMap1_MapReady(IMSMapEvent e)
        {
            initMark();

        }
        private void initMark()
        {
            if (GraphicsLayer == null)
            {
                GraphicsLayer = new ZDIMS.Drawing.GraphicsLayer();
                this.iMSMap1.AddChild(this.GraphicsLayer);
            }
        
            //起始路径点

            Image img1 = new Image();
            img1.Source = new BitmapImage(new Uri("/images/NetImage/qidian.jpg", UriKind.Relative));
            mark1 = new IMSMark(img1, CoordinateType.Logic);
            mark1.X = 114.26244962792968;
            mark1.Y = 30.55578913671875;
            m_markLayer.AddMark(mark1);
            //路径点
            Image img2 = new Image();
            img2.Source = new BitmapImage(new Uri("/images/NetImage/v1.jpg", UriKind.Relative));
            mark2 = new IMSMark(img2, CoordinateType.Logic);
            mark2.X = 114.27868539904786;
            mark2.Y = 30.55466705944824;
            m_markLayer.AddMark(mark2);
            //目的点
            Image img3 = new Image();
            img3.Source = new BitmapImage(new Uri("/images/NetImage/zhongdian.jpg", UriKind.Relative));
            mark3 = new IMSMark(img3, CoordinateType.Logic);
            mark3.X = 114.29711952563477;
            mark3.Y = 30.546514824584957;
            m_markLayer.AddMark(mark3);
            

        }
        private void OnSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            LinePnts = new List<Point>();
            CPathAnalyzeResult obj = _spatial.OnNetAnalyse(e);
            if (obj == null || obj.Paths == null)
                return;
            CNetPath path = obj.Paths[0];
            int edgeNum = path.Edges.Length;
            Paragraph myParagraph = new Paragraph();
            myParagraph.Inlines.Add(new Run() { Text = "1.从起点出发" });
            Bold bold;
            IMSMark mark;
            StackPanel panel;
            for (int i = 0; i < edgeNum; i++)
            {
                CNetEdge edge = path.Edges[i];
                if (i != 0)
                    myParagraph.Inlines.Add(new Run() { Text = (i + 1).ToString() + "." });
                myParagraph.Inlines.Add(new Run() { Text = "经" });
                bold = new Bold() { Foreground = new SolidColorBrush(Colors.Red) };
                bold.Inlines.Add(edge.FieldValus[2]);
                myParagraph.Inlines.Add(bold);
                if (i != edgeNum - 1)
                {
                    myParagraph.Inlines.Add(new Run() { Text = "到达" });
                    bold = new Bold() { Foreground = new SolidColorBrush(Colors.Red) };
                    bold.Inlines.Add(path.Nodes[i + 1].FieldValus[0] + "\n");
                    myParagraph.Inlines.Add(bold);
                }
                else
                    myParagraph.Inlines.Add(new Run() { Text = "到达终点" });
                if (GraphicsLayer != null)
                {
                    if (m_road == null)
                    {
                        m_road = new IMSPolyline(CoordinateType.Logic);
                        m_road.Shape.Stroke = new SolidColorBrush(Colors.Red);
                        GraphicsLayer.AddGraphics(m_road);
                    }
                    for (int j = 0; j < edge.Dots.Length; j++)
                    {
                        m_road.Points.Add(new Point(edge.Dots[j].x, edge.Dots[j].y));
                        LinePnts.Add(new Point(edge.Dots[j].x, edge.Dots[j].y));
                    }
                    panel = new StackPanel() { Orientation = Orientation.Horizontal };
                    panel.Children.Add(new Image() { Source = new BitmapImage(new Uri("/images/bus/stop.png", UriKind.Relative)) });
                    panel.Children.Add(new TextBlock() { Text = edge.FieldValus[2] });
                    mark = new IMSMark(panel, CoordinateType.Logic) {EnableDrag = false,EnableAnimation=false};
                    mark.X = edge.Dots[edge.Dots.Length / 2].x;
                    mark.Y = edge.Dots[edge.Dots.Length / 2].y;
                    m_markLayer.AddMark(mark);
                    m_stopList.Add(mark);
                }
            }
            m_road.Draw();
            if (netWindow == null)
            {
                netWindow = new NetWindow();
            }
            if (netWindow != null)
            {
                this.netWindow.g_graphicLayer = this.GraphicsLayer;
                this.netWindow.m_markLayer = this.m_markLayer;
                this.netWindow.OgicPnts = this.LinePnts;
                netWindow.Show();
                this.netWindow.Margin = new Thickness(0,120,0,0);
                this.netWindow.HorizontalAlignment = HorizontalAlignment.Right;
                this.netWindow.roadReslut.Blocks.Add(myParagraph);
            }
            SuccessFlag = true;
        }

        private void NetSubmit(object sender, RoutedEventArgs e)
        {
            if (SuccessFlag == true)
            {

                if (netWindow != null)
                {
                    this.netWindow.removeFlash();
                    ClearRoad();
                    this.netWindow.roadReslut.Blocks.Clear();
                    this.netWindow.Close();
                    netWindow = null;
                }
                SuccessFlag = false;
                CGdbInfo gdb = new CGdbInfo();
                gdb.GDBName = this.GDBName;
                gdb.GDBSvrName = this.GDBSvrName;
                gdb.Password = this.Password;
                gdb.User = this.User;
                CNetAnalyse obj = new CNetAnalyse();
                obj.GdbInfo = gdb;
                obj.NetLayerName = this.NetLayerName;
                obj.RequestDots = this.pathDots.Substring(0, this.pathDots.Length - 1);
                obj.BarrierDots = "";
                obj.NearDis = 0.002;
                obj.FlgType = "line";
                obj.NetWeight = ",Weight1,Weight1";
                obj.AnalysTypeParam = AnalysType.UserMode.ToString();
                _spatial = new SpacialAnalyse(this.mapDoc);
             

                _spatial.NetAnalyse(obj, new UploadStringCompletedEventHandler(OnSubmit));
            }
        }

        private void clearNet(object sender, RoutedEventArgs e)
        {
            SuccessFlag = true;
            ClearRoad();
            if (netWindow != null)
            {
                this.netWindow.removeFlash();
                this.netWindow.roadReslut.Blocks.Clear();
                this.netWindow.Close();
                netWindow = null;
            }
        }

        private void ClearRoad()
        {
            if (m_road != null && GraphicsLayer != null)
            {
                GraphicsLayer.RemoveGraphics(m_road);
                m_road = null;
            }
            if (this.m_markLayer != null)
            {
                for (int i = 0; i < m_stopList.Count; i++)
                    m_markLayer.RemoveMark(m_stopList[i]);
                m_stopList.Clear();
            }
        }

        private void Close_NetWindow(object sender, RoutedEventArgs e)
        {
            if (netWindow != null)
            {
                this.netWindow.roadReslut.Blocks.Clear();
                this.netWindow.Close();
            }
        }
        // 页面加载的时候给瓦片地图和矢量地图的ServerAddress赋值
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
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

