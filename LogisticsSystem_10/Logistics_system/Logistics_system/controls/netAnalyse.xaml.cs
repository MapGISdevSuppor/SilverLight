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
using System.Windows.Threading;
using ZDIMS.BaseLib;
using ZDIMS.Util;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZDIMS.Map;
using ZDIMSDemo.Controls;
using System.Windows.Media.Imaging;
namespace Logistics_system.controls
{
    public partial class netAnalyse : BaseUserControl
    {
        trackplay track;
        bool playFlag = false;
        IMSPolyline m_road;
        List<IMSMark> m_stopList = new List<IMSMark>(); 
        SpacialAnalyse _spatial;
        public  string m_pathDots = "";
        string m_barrierDots = "";
        List<IMark> m_pathMark = new List<IMark>();
        List<IMark> m_barrierMark = new List<IMark>();
        public string GDBName { get; set; }
        public string GDBSvrName { get; set; }
        public string Password { get; set; }
        public string User { get; set; }
        public string NetLayerName { get; set; }
        public MarkLayer MarkLayer { get; set; }
        public GraphicsLayer GraphicsLayer { get; set; }
        private  VectorLayerBase VectorObj { get; set; }
        private IMSMap _mapContainor = null;
        private GraphicsLayer _graphicslayer = null;
        private MyProgressBar progressbar = new MyProgressBar();
        string StartX = "";//起始点X坐标
        string StartY = "";//起始点Y坐标
        string stopx = "";//终点X坐标
        string stopy = "";//终点Y坐标
        private Point[] pathD = new Point[20];
        private Point[] barrD = new Point[20];
        string Points = "";
        string VIAPoints = "";//路径点坐标
        public netAnalyse(  VectorLayerBase VectObj,  MarkLayer markLayer, GraphicsLayer graphicsLayer)
        {
            MarkLayer = markLayer;
            markLayer.EnablePolymericMark = false;
            GraphicsLayer = graphicsLayer;
            GDBName = "test201";
            GDBSvrName = "MapGisLocal";
            Password = "";
            User = "";
            NetLayerName = "道路网";
            InitializeComponent();
             dialogPanel1.OnClose += new RoutedEventHandler(Close);
             this.VectorObj =  VectObj;
        }

        //public NetAnalyse(MarkLayer markLayer, GraphicsLayer graphicsLayer)
        //{
        //    MarkLayer = markLayer;
        //    markLayer.EnablePolymericMark = false;
        //    GraphicsLayer = graphicsLayer;
        //    GDBName = "world";
        //    GDBSvrName = "MapGisLocal";
        //    Password = "";
        //    User = "";
        //    NetLayerName = "武汉道路网";
        //    InitializeComponent();
        //    dialogPanel1.OnClose += new RoutedEventHandler(Close);            
        //}
        private void Clear()
        {
            m_pathDots = "";
            m_barrierDots = "";
            //if (MarkLayer != null)
            //    MarkLayer.RemoveAll();
            ClearPathDot();
            ClearBarrier();
            ClearRoad();
            roadReslut.Blocks.Clear();
        }
        void ClearRoad()
        {
            if (m_road != null && GraphicsLayer != null)
            {
                GraphicsLayer.RemoveGraphics(m_road);
                m_road = null;
            }
            if (MarkLayer != null)
            {
                for (int i = 0; i < m_stopList.Count; i++)
                    MarkLayer.RemoveMark(m_stopList[i]);
                m_stopList.Clear();
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            Clear();
            if (track != null)
            {
                track.stop();
            }
            this.Close();
        }
        private void radioButton1_Click(object sender, RoutedEventArgs e)
        {
            GraphicsLayer.DrawingType = DrawingType.None;
            if (MarkLayer != null)
            {
                MarkLayer.ManuallyAddMarkObj = null;
                IMSMark markPnt = new IMSMark(new Image() { Source = new BitmapImage(new Uri("../images/mark/v1.png", UriKind.Relative)) }) { EnableAnimation = false };
                MarkLayer.ManuallyAddMarkObj = markPnt;
                MarkLayer.ManuallyAddMarkOverCallback = new ManuallyAddMarkDelegate(StartPntAdd1);
            }
        }
        private void StartPntAdd1(MarkLayer markLayer, IMark mark, Point logicPnt)
        {
            m_pathMark.Add(mark);
            m_pathDots += logicPnt.X + "," + logicPnt.Y + ",";
            IMSMark markPnt = new IMSMark(new Image() { Source = new BitmapImage(new Uri("../images/mark/v1.png", UriKind.Relative)) }) { EnableAnimation = false };
            MarkLayer.ManuallyAddMarkObj = markPnt;
            MarkLayer.ManuallyAddMarkOverCallback = new ManuallyAddMarkDelegate(StartPntAdd1);
        }
        private void radioButton2_Click(object sender, RoutedEventArgs e)
        {
            GraphicsLayer.DrawingType = DrawingType.None;
            if (MarkLayer != null)
            {
                MarkLayer.ManuallyAddMarkObj = null;
                IMSMark markPnt = new IMSMark(new Image() { Source = new BitmapImage(new Uri("../images/mark/v0.png", UriKind.Relative)) }) { EnableAnimation = false };
                MarkLayer.ManuallyAddMarkObj = markPnt;
                MarkLayer.ManuallyAddMarkOverCallback = new ManuallyAddMarkDelegate(StartPntAdd2);
            }
        }
        private void StartPntAdd2(MarkLayer markLayer, IMark mark, Point logicPnt)
        {
            m_barrierMark.Add(mark);
            m_barrierDots += logicPnt.X + "," + logicPnt.Y + ",";
            IMSMark markPnt = new IMSMark(new Image() { Source = new BitmapImage(new Uri("../images/mark/v0.png", UriKind.Relative)) }) { EnableAnimation = false };
            MarkLayer.ManuallyAddMarkObj = markPnt;
            MarkLayer.ManuallyAddMarkOverCallback = new ManuallyAddMarkDelegate(StartPntAdd2);
        }

        private void ClearPathDot(object sender = null, RoutedEventArgs e = null)
        {
            if (MarkLayer != null)
            {
                for (int i = 0; i < m_pathMark.Count; i++)
                    MarkLayer.RemoveMark(m_pathMark[i]);
                m_pathMark.Clear();
                m_pathDots = "";
            }
        }

        private void ClearBarrier(object sender = null, RoutedEventArgs e = null)
        {
            if (MarkLayer != null)
            {
                for (int i = 0; i < m_barrierMark.Count; i++)
                    MarkLayer.RemoveMark(m_barrierMark[i]);
                m_barrierMark.Clear();
                m_barrierDots = "";
            }
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            GraphicsLayer.DrawingType = DrawingType.None;
            Clear();
            MarkLayer.ManuallyAddMarkObj = null;
            radioButton1.IsChecked = false;
            radioButton2.IsChecked = false;
        }

        public  void Submit()
        {
            
            if (CommFun.IsNumber(this.buffer.Text))
            {
                if (VectorObj == null)
                {
                    MessageBox.Show("绑定的矢量图对象为空", "提示", MessageBoxButton.OK);
                    return;
                }
                MarkLayer.ManuallyAddMarkObj = null;
                CGdbInfo gdb = new CGdbInfo();
                gdb.GDBName = this.GDBName;
                gdb.GDBSvrName = this.GDBSvrName;
                gdb.Password = this.Password;
                gdb.User = this.User;
                CNetAnalyse obj = new CNetAnalyse();
                obj.GdbInfo = gdb;
                obj.NetLayerName = this.NetLayerName;
                obj.RequestDots = this.m_pathDots.Length > 0 ? this.m_pathDots.Substring(0, this.m_pathDots.Length - 1) : this.m_pathDots;
                obj.BarrierDots = this.m_barrierDots.Length > 0 ? this.m_barrierDots.Substring(0, this.m_barrierDots.Length - 1) : this.m_barrierDots;
                obj.NearDis = 100; //Convert.ToSingle(this.buffer.Text);
                obj.FlgType = "line";
                obj.NetWeight = ",Weight1,Weight1";
                _spatial = new SpacialAnalyse(VectorObj);
                _spatial.NetAnalyse(obj, new UploadStringCompletedEventHandler(OnSubmit));
            }
            else
                MessageBox.Show("请输入合法的分析半径", "提示", MessageBoxButton.OK);
        }

        private void OnSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            ClearRoad();
            roadReslut.Blocks.Clear();
            if (e.Error != null)
            {
                MessageBox.Show("分析失败,错误原因为:"+e.Error.Message, "提示", MessageBoxButton.OK);
                return;
            }
            try
            {
                if (e.Result.IndexOf("COperResult") > -1)
                {
                    COperResult res=VectorObj.GetObject(e, typeof(COperResult)) as COperResult;
                    MessageBox.Show(res.ErrorInfo, "提示", MessageBoxButton.OK);
                    return;
                }
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
                        }
                        panel = new StackPanel() { Orientation = Orientation.Horizontal };
                        panel.Children.Add(new Image() { Source = new BitmapImage(new Uri("../images/bus/stop.png", UriKind.Relative)) });
                        panel.Children.Add(new TextBlock() { Text = edge.FieldValus[2] });
                        mark = new IMSMark(panel, CoordinateType.Logic, MarkLayer) { EnableAnimation = false, EnableDrag = false };
                        mark.X = edge.Dots[edge.Dots.Length / 2].x;
                        mark.Y = edge.Dots[edge.Dots.Length / 2].y;
                        MarkLayer.AddMark(mark);
                        m_stopList.Add(mark);
                    }
                }


                m_road.Draw();
                roadReslut.Blocks.Add(myParagraph);
                radioButton1.IsChecked = false;
                radioButton2.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK);
            }
        }

        private void play_pause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            img.Source = new BitmapImage(new Uri("../images/play/pause.png", UriKind.Relative));
        }

        private void play_pause_Click(object sender, RoutedEventArgs e)
        {
            if (track == null)
            {
                track = new trackplay();
                track.m_markLayer = this.MarkLayer;
                track.GetAllPntsOnLine(m_road.Points);
            }
            if (!playFlag)
            {
                
                playFlag = true;
                this.play.Source = new BitmapImage(new Uri("../images/play/pause.png", UriKind.Relative));
                this.stop.IsEnabled = true;
                track.play();

            }
            else
            {
                track.pause();
                this.play.Source = new BitmapImage(new Uri("../images/play/play.png", UriKind.Relative));
            }
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            playFlag = false;
            track.stop();
            this.play.Source = new BitmapImage(new Uri("../images/play/play.png", UriKind.Relative));
            this.stop.IsEnabled = false;
        }
      
    }
}
