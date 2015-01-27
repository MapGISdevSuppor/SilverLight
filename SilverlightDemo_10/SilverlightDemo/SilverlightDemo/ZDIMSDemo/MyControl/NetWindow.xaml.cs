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
using ZDIMS.BaseLib;
using ZDIMS.Drawing;
using ZDIMS.Map;
using ZDIMS.Util;
using ZDIMS.Event;
using ZDIMS.Interface;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;

namespace ZDIMSDemo.MyControl
{
    public partial class NetWindow : BaseUserControl
    {
        public MarkLayer m_markLayer;
        public GraphicsLayer g_graphicLayer;
        //原始的路径点
        public List<Point> OgicPnts = new List<Point>();

        // 点的测试数据
        List<TrackPntInfo> trackTimePnts = new List<TrackPntInfo>();

        // 拆线轨迹点的LIST
        List<Point> trackPnts = new List<Point>();

        //用于存放整条轨迹的离散点
        List<SlopePnt> AllLineDispersePnts = new List<SlopePnt>();
        private Boolean showFlag = true;
        Image img;
        int count = 0;
        IMSMark Mark = null;
        private Boolean StartStopFlag=true;
        DispatcherTimer m_timer = new DispatcherTimer();
        private Boolean finshShowFlag=true;
        public NetWindow()
        {
            InitializeComponent();
            // this.m_markLayer = m_markLayer;
            //  this.g_graphicLayer = g_graphicLayer;
            // this.trackPnts = trackPnts;
            dialogPanel1.OnClose += new RoutedEventHandler(Close);  
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void showMoveFlash(object sender, RoutedEventArgs e)
        {
            if(this.finshShowFlag==true){
                if(this.showFlag==true){
                     initData();
                     DrawTrackLine();
                 }
            AddShip();
            finshShowFlag = false;
            StartStopFlag = false;
            m_timer.Start();
            }
        }
        private void AddShip()
        {
            img = new Image();
            img.Source = new BitmapImage(new Uri("/images/bus/stop.png", UriKind.Relative));
            img.Width = 28;
            img.Height = 24;
            //PlaneProjection Pg = new PlaneProjection();
            //Pg.RotationX = 30;
            Mark = new IMSMark(img, CoordinateType.Logic, this.m_markLayer);
            Mark.X = AllLineDispersePnts[0].Pnt.X;
            Mark.Y = AllLineDispersePnts[0].Pnt.Y;
            Mark.EnableAnimation = false;
            Mark.EnableRevisedPos = true;
            this.m_markLayer.AddMark(Mark);
            if(this.showFlag==true){
            m_timer.Interval = new TimeSpan(0, 0, 0, 0, 80);
            m_timer.Tick += new EventHandler(m_timer_Tick);
            this.showFlag = false;
            }

        }
        void m_timer_Tick(object sender, EventArgs e)
        {
            if (count <= AllLineDispersePnts.Count - 1)
            {
                // 绘制地图
                // 当前船舶对像
               // IMSCircle CurrentShipPnt = new IMSCircle();
                //CurrentShipPnt.CenX = AllLineDispersePnts[count].Pnt.X;
               // CurrentShipPnt.CenY = AllLineDispersePnts[count].Pnt.Y;
                // 设置坐标类型为逻辑坐标
              //  CurrentShipPnt.CoordinateType = CoordinateType.Logic;
                // 船舶点的半径（像素）           
               // CurrentShipPnt.RadiusEx = 3;
                // 线的厚度
              //  CurrentShipPnt.StrokeThickness = 0.2;
              //  this.g_graphicLayer.AddGraphics(CurrentShipPnt);
              //  CurrentShipPnt.Draw();

                Mark.X = AllLineDispersePnts[count].Pnt.X;
                Mark.Y = AllLineDispersePnts[count].Pnt.Y;
                //PlaneProjection f = new PlaneProjection();
                //f.RotationZ = AllLineDispersePnts[count].PntSlopeAngle;
                //Mark.OffsetX = -img.ActualWidth / 2;
                //Mark.OffsetY = -img.ActualHeight / 2;
                RotateTransform f = new RotateTransform();//图片旋转的角度
                f.CenterX = 14;
                f.CenterY = 12;
                f.Angle = AllLineDispersePnts[count].PntSlopeAngle;
                img.RenderTransform = f;
               
                count++;
                int FinshEnd =AllLineDispersePnts.Count - 1;
                if (count == FinshEnd)
                {
                     Mark.X = AllLineDispersePnts[0].Pnt.X;
                    Mark.Y = AllLineDispersePnts[0].Pnt.Y;
                    if(this.Mark!=null){
                        this.m_markLayer.RemoveMark(this.Mark);
                    }
                     count = 0;
                     this.finshShowFlag = true;
                    // this.StartStopFlag = true;
                     m_timer.Stop();
                }
            }
        }

        private void initData()
        {
            for (int i = 0; i < OgicPnts.Count; i++)
            {
               
                    TrackPntInfo tPntInfo = new TrackPntInfo();
                    if (i <=60)
                    {
                    tPntInfo.GetPntTime = new DateTime(2011, 4, 1, 0, 0, i);
                    }else if(i<=120&&i>60){
                        tPntInfo.GetPntTime = new DateTime(2011, 4, 1, 0, 1, i-60);
                    }
                    else if (i <=180 && i >120)
                    {
                        tPntInfo.GetPntTime = new DateTime(2011, 4, 1, 0,2, i-120);
                    }
                    else if (i <=240 && i > 180)
                    {
                        tPntInfo.GetPntTime = new DateTime(2011, 4, 1, 0, 3, i -180);
                    }
                    else if (i <= 300 && i > 240)
                    {
                        tPntInfo.GetPntTime = new DateTime(2011, 4, 1, 0, 3, i - 240);
                    }
                    else if (i <= 300 && i > 360)
                    {
                        tPntInfo.GetPntTime = new DateTime(2011, 4, 1, 0, 3, i - 300);
                    }
                    Point pnt = new Point();
                    pnt.X=OgicPnts[i].X;
                    pnt.Y=OgicPnts[i].Y;
                    tPntInfo.LocalPnt = pnt;
                    trackTimePnts.Add(tPntInfo);        
            }
            // 对trackPnt按时间进行排序,时间早的点排在前            
            trackTimePnts.Sort();

            GetAllPntsOnLine(trackTimePnts);

        }
        private void GetAllPntsOnLine(List<TrackPntInfo> trackPnt)
        {

            // 对拆线点进行遍历
            for (int i = 0; i < trackPnt.Count - 1; i++)
            {
                // 直线的起点
                Point startPnt = trackPnt[i].LocalPnt;
                // 直线的终点
                Point endPnt = trackPnt[i + 1].LocalPnt;
                #region 由step根据斜率算出X方向上的偏移量
                //按照直线的固定步长，算出在X方向上的偏移量 
                // 未完成由step根据斜率算出X方向上的偏移量
                // 定义点在直线上移动的步长
                double step = 0.01;
                // 定义在X方向上每取出一点的步长,直线上距离固定
                double offsetX;

                // 求两点之间距离
                double distance = Math.Sqrt(Math.Pow((startPnt.Y - endPnt.Y), 2) + Math.Pow((startPnt.X - endPnt.X), 2));
                double xDircettionDis = 0.0;
                if (endPnt.X < startPnt.X) { xDircettionDis = startPnt.X - endPnt.X; }
                else if (endPnt.X > startPnt.X) { xDircettionDis = endPnt.X - startPnt.X; }
                offsetX = step * xDircettionDis / distance;
                #endregion

                // 如果两点之间为竖直直线，定义在Y方向上每取出一点的步长,直线上距离固定
          

                // 求出两点之间直线的斜率
                double k = (endPnt.Y - startPnt.Y) / (endPnt.X - startPnt.X);


                ////////////////////
                double agle = angle(startPnt, endPnt);
                ////////////////////



                // 判断结束点在开始点的左边(步长为负)还是右边(步长为正)
                // 结束点在起始点的右方，步长应该为正
                if (endPnt.X > startPnt.X)
                {
                    //  offsetX=1.0*offsetX;
                    // 取得该段线段的起点
                    double tempX = startPnt.X;
                    // 按步长取出每点对应的Y坐标，并构造成点添加到LIST里
                    while (tempX < endPnt.X)
                    {
                        SlopePnt tempSlopePnt = new SlopePnt();
                        // 取得点的X坐标 // 根据直线方程取得点的Y坐标
                        Point pnt = new Point(tempX, k * tempX - k * startPnt.X + startPnt.Y);
                        tempSlopePnt.Pnt = pnt;
                        //tempSlopePnt.Pnt.X = tempX;
                        tempSlopePnt.PntSlopeAngle = agle;
                        //tempSlopePnt.Pnt.Y = k * tempX - k * startPnt.X + startPnt.Y;
                        // 将取得的点添加到全局的轨迹点LIST里
                        AllLineDispersePnts.Add(tempSlopePnt);

                        tempX = tempX + offsetX;

                    }

                }
                // 结束点在起始点的左方，步长应该为负
                else if (endPnt.X < startPnt.X)
                {

                    // 取得该段线段的起点
                    double tempX = startPnt.X;
                    // 按步长取出每点对应的Y坐标，并构造成点添加到LIST里
                    while (tempX > endPnt.X)
                    {
                        SlopePnt tempSlopePnt = new SlopePnt();
                       // Point tempPnt = new Point();
                        Point pnt = new Point(tempX, k * tempX - k * startPnt.X + startPnt.Y);
                        tempSlopePnt.Pnt = pnt;
                        //tempSlopePnt.Pnt.X = tempX;
                        tempSlopePnt.PntSlopeAngle = agle;
                        //tempSlopePnt.Pnt.Y = k * tempX - k * startPnt.X + startPnt.Y;
                        // 将取得的点添加到全局的轨迹点LIST里
                        AllLineDispersePnts.Add(tempSlopePnt);

                        tempX = tempX - offsetX;

                    }
                }
            }
        }
        private double angle(Point p1, Point p2)//换算角度
        {
            //向量线与X轴正切值
            double tanVl;
            //旋转的角度
            double Angle;
            double a = 0;
            VectorLine Line = new VectorLine(p1, p2);

            if (Line.ParaX > 0 && Line.ParaY != 0)//向量线在一、四象限
            {
                tanVl = Line.ParaY / Line.ParaX;
                Angle = Math.Atan(tanVl);
                a = -(Math.Round((Angle / Math.PI) * 180));
            }
            //向量线垂直于X轴，方向向上
            if (Line.ParaX == 0 && Line.ParaY > 0)
            {
                a = -90;
            }
            //向量线垂直于X轴，方向向下
            if (Line.ParaX == 0 && Line.ParaY < 0)
            {
                a = 90;
            }
            //二、三象限
            if (Line.ParaX < 0 && Line.ParaY != 0)
            {
                tanVl = Line.ParaY / Line.ParaX;
                Angle = Math.Atan(tanVl);
                a = 180 - (Math.Round((Angle / Math.PI) * 180));
            }
            //向量线平行于x轴，方向向左
            if (Line.ParaY == 0 && Line.ParaX < 0)
            {
                a = -180;
            }
            //向量线平行于x轴，方向向右
            if (Line.ParaY == 0 && Line.ParaX > 0)
            {
                a = 0;
            }
            return Angle = a;

        }
      
        private void DrawTrackLine()
        {

            for (int i = 0; i < trackTimePnts.Count; i++)
            {
                Point tempPnt = new Point();
                tempPnt.X = trackTimePnts[i].LocalPnt.X;
                tempPnt.Y = trackTimePnts[i].LocalPnt.Y;
                trackPnts.Add(tempPnt);

            }

           
        }

        private void stopMoveFlash(object sender, RoutedEventArgs e)
        {
            if (StartStopFlag == true)
            {
                m_timer.Start();
                this.StartStopFlag = false;
                
            }
            else {

                this.StartStopFlag = true;
                m_timer.Stop();
            }
        }

        public void removeFlash() {
            if (m_timer!=null)
            {
                m_timer.Stop();
            }
             if(this.Mark!=null&&m_markLayer!=null)
             {
                this.m_markLayer.RemoveMark(this.Mark);
             }
             finshShowFlag = true;
             this.showFlag = true;
         count = 0;
        }
        
        

    }
}
