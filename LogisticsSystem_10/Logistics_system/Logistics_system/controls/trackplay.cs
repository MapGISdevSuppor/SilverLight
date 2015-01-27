using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZDIMS.BaseLib;
using ZDIMS.Util;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
namespace Logistics_system.controls
{
    public delegate void PlayOverCallBack();
    public delegate void StopPlayEventHander();
    public class trackplay
    {
        //用于存放整条轨迹的离散点
        List<SlopePnt> AllLineDispersePnts = new List<SlopePnt>();
      public   DispatcherTimer m_timer = null;
        Image img;
        IMSMark Mark = null;
        IMSMark shipMark = null;//显示小车图标的标注对象
        int count = 0;

        public StopPlayEventHander stopPlayCallBack = null;
        public PlayOverCallBack playOverBackHander = null;
        public MarkLayer m_markLayer;


        #region 轨迹回放
        /// <summary>
        /// 轨迹点
        /// </summary>
        /// <param name="trackPnt"></param>
        public  void GetAllPntsOnLine(List<Point> trackPnt)
        {
            // 对拆线点进行遍历
            for (int i = 0; i < trackPnt.Count - 1; i++)
            {
                // 直线的起点
                Point startPnt = trackPnt[i];
                // 直线的终点
                Point endPnt = trackPnt[i + 1];
                #region 由step根据斜率算出X方向上的偏移量
                //按照直线的固定步长，算出在X方向上的偏移量 
                // 未完成由step根据斜率算出X方向上的偏移量
                // 定义点在直线上移动的步长
                double step = 10;
                // 定义在X方向上每取出一点的步长,直线上距离固定
                double offsetX;
                // 求两点之间距离
                double distance = Math.Sqrt(Math.Pow((startPnt.Y - endPnt.Y), 2) + Math.Pow((startPnt.X - endPnt.X), 2));
                double xDircettionDis = 0.0;
                if (endPnt.X < startPnt.X) { xDircettionDis = startPnt.X - endPnt.X; }
                else if (endPnt.X > startPnt.X) { xDircettionDis = endPnt.X - startPnt.X; }
                offsetX = step * xDircettionDis / distance;
                #endregion
                // 求出两点之间直线的斜率
                double k = (endPnt.Y - startPnt.Y) / (endPnt.X - startPnt.X);
                //计算两折线间的夹角
                double agle = angle(startPnt, endPnt);
                ////////////////////
                // 判断结束点在开始点的左边(步长为负)还是右边(步长为正)
                // 结束点在起始点的右方，步长应该为正
                if (endPnt.X > startPnt.X)
                {
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
                        // 取得点的X坐标 // 根据直线方程取得点的Y坐标
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
        /// <summary>
        /// 换算角度，小车图片水平向右时角度为0
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
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
        public void start()
        {
        }
        /// <summary>
        /// 播放按钮事件
        /// </summary>
        public void play()
        {
            Addcar();
            m_timer.Start();
        }
        public void Start()
        {
            m_timer.Start();
        }
        public void pause()
        {
            //this.m_markLayer.RemoveMark(Mark);

            m_timer.Stop();

        }
        public void stop()
        {
            Mark.X = AllLineDispersePnts[0].Pnt.X;
            Mark.Y = AllLineDispersePnts[0].Pnt.Y;

            count = 0;
            this.m_markLayer.RemoveMark(Mark);
            if (this.stopPlayCallBack != null)
            {
                this.stopPlayCallBack();
            }
            //  this.m_markLayer.MapContainer.Refresh();
            m_timer.Stop();
        }
        /// <summary>
        /// 设置速度
        /// </summary>
        private void Addcar()
        {
            //添加小车图片
            img = new Image();
            img.Source = new BitmapImage(new Uri("../images/car.PNG", UriKind.Relative));
            img.Width = 32;
            img.Height = 20;
            Mark = new IMSMark(img, CoordinateType.Logic, this.m_markLayer);
            Mark.X = AllLineDispersePnts[0].Pnt.X;
            Mark.Y = AllLineDispersePnts[0].Pnt.Y;
            Mark.EnableAnimation = false;
            Mark.EnableRevisedPos = true;
            this.m_markLayer.AddMarkAt(Mark,20000);
            m_timer = new DispatcherTimer();
            m_timer.Interval = new TimeSpan(0, 0, 0, 0, 1);//设置播放速度
            m_timer.Tick += new EventHandler(m_timer_Tick);//添加时间变化事件：小车标注变化
        }
        /// <summary>
        /// 时间变化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_timer_Tick(object sender, EventArgs e)
        {
            if (count <= AllLineDispersePnts.Count - 1)
            {

                //小车标注坐标、角度变化
                Mark.X = AllLineDispersePnts[count].Pnt.X;
                Mark.Y = AllLineDispersePnts[count].Pnt.Y;
                RotateTransform f = new RotateTransform();//图片旋转的角度
                f.CenterX = 14;
                f.CenterY = 12;
                f.Angle = AllLineDispersePnts[count].PntSlopeAngle;
                img.RenderTransform = f;
                count++;
            }
            else
            {
                this.stop();
                if (this.playOverBackHander != null)
                {
                    this.playOverBackHander();
                }
                this.m_markLayer.RemoveMark(this.Mark);
            }
        }
        #endregion
    }
}
