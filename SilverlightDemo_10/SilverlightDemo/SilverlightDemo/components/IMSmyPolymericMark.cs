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
using System.Collections.Generic;
using System.Windows.Threading;
using SilverlightDemo.components;
using ZDIMS.Map;
using ZDIMS.Util;
using ZDIMS.Interface;

namespace SilverlightDemo.components
{
    /// <summary>
    /// 多重聚合标注
    /// </summary>
    public class IMSmyPolymericMark : MarkBase, ImyPolymericMark
    {        
        /// <summary>
        /// 聚合mark个数
        /// </summary>
        protected static long PolymericMarkNum = 0;
        private string m_ellipseName = "";
        #region 内部成员变量
        /// <summary>
        /// 默认半径
        /// </summary>
        protected const double DefaultRadius = 40;
        /// <summary>
        /// 默认旋转计数器次数
        /// </summary>
        protected const int DefaultRepeatCount = 6;
        /// <summary>
        /// 聚合mark数组
        /// </summary>
        protected List<ImyMark> m_markList = new List<ImyMark>();
        /// <summary>
        /// 中心mark数字文本显示对象
        /// </summary>
        protected TextBlock m_textBlock = null;
        /// <summary>
        /// 旋转半径
        /// </summary>
        protected double m_radius = DefaultRadius;//旋转半径
        /// <summary>
        /// 旋转角度
        /// </summary>
        protected double m_angle = 30;//旋转角度
        /// <summary>
        /// 旋转计数器
        /// </summary>
        protected DispatcherTimer m_timer = null;
        /// <summary>
        /// 旋转计数器执行总次数
        /// </summary>
        protected int m_repeatCount = DefaultRepeatCount;
        /// <summary>
        /// 当前旋转计数器执行次数
        /// </summary>
        protected int m_currentCount = 0;
        /// <summary>
        /// 中心mark
        /// </summary>
        protected Ellipse m_ellipse = null;//中心
        /// <summary>
        /// 所有mark周长
        /// </summary>
        protected double m_len = 0;//所有mark周长
        /// <summary>
        /// 偏移速度
        /// </summary>
        protected double m_offsetSpeed = 0;//偏移速度
        /// <summary>
        /// 旋转起始时角度
        /// </summary>
        protected double[] m_startAngleArr = null;//起始角度
        /// <summary>
        /// 旋转结束时角度
        /// </summary>
        protected double[] m_endAngleArr = null;//起始角度
        /// <summary>
        /// 当前展开mark半径
        /// </summary>
        protected double m_currentRadius = 0;//当前半径
        /// <summary>
        /// mark展开或关闭标志
        /// </summary>
        protected bool m_isOpen = false;
        /// <summary>
        /// 打开mark时的连接线数组
        /// </summary>
        protected List<Line> m_lineArr = new List<Line>();
        /// <summary>
        /// 关闭mark的延时计数器
        /// </summary>
        protected DispatcherTimer m_closeTimer = null;
        /// <summary>
        /// 展开mark的延时计数器
        /// </summary>
        protected DispatcherTimer m_openTimer = null;
        /// <summary>
        /// 中心mark边框变化的动画控制 对象
        /// </summary>
        protected Storyboard m_storyboard = null;
        #endregion

        #region 属性
        /// <summary>
        /// 聚合标注列表
        /// </summary>
        public List<ImyMark> MarkList
        {
            get { return m_markList; }
        }
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="coordinateType">坐标类型（默认是屏幕坐标，如果是逻辑坐标，在绘图时会根据地图容器的LogicToScreen方法转换坐标）</param>
        /// <param name="mLayer">标注图层对象，为“null”时会内部自动获取</param>
        /// <remarks>
        /// 下载示例代码<br/>
        /// <a href="http://www.lbsmap.net/imsdownload/IMSPolymericMark_Demo.rar">IMSPolymericMark_Demo</a>
        /// </remarks>
        public IMSmyPolymericMark(CoordinateType coordinateType = CoordinateType.Screen, MarkLayer mLayer = null) :
            base(null, coordinateType, mLayer)
        {
            m_ellipseName = "CE_" + (++PolymericMarkNum).ToString();
            Canvas canvas = new Canvas() { Width = 20, Height = 20};//, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            m_ellipse = new Ellipse()
            {
                Name = m_ellipseName,//"CenEllipse",
                Fill = new SolidColorBrush(Colors.Yellow),
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(Colors.Black),
                Width = 20,
                Height = 20
            };
            canvas.Children.Add(m_ellipse);
            Canvas.SetZIndex(m_ellipse, 2);
            m_textBlock = new TextBlock()
            {
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 20,
                Height = 20,
                Text = "0",
                FontWeight = FontWeights.Bold
            };
            canvas.Children.Add(m_textBlock);
            Canvas.SetZIndex(m_textBlock, 3);
            Canvas.SetTop(m_textBlock, 2);

            m_markControl = canvas;
            if (this.m_markControl != null)
            {
                m_coordinateType = coordinateType;
                this.m_markControl.Tag = this;
                this.m_markControl.Loaded += new RoutedEventHandler(OnLoaded);
                m_markControl.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
                m_markControl.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseLeftButtonUp);
                m_markControl.Cursor = Cursors.Hand;
                m_markControl.MouseEnter += new MouseEventHandler(OnMouseEnter1);
                m_markControl.MouseLeave += new MouseEventHandler(OnMouseLeave1);
            }
            m_timer = new DispatcherTimer();
            m_timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            m_timer.Tick += new EventHandler(Rotation_Tick);
            m_closeTimer = new DispatcherTimer();
            m_closeTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            m_closeTimer.Tick += new EventHandler(Close_Tick);
            m_openTimer = new DispatcherTimer();
            m_openTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            m_openTimer.Tick += new EventHandler(Open_Tick);

                m_storyboard = new Storyboard();
                DoubleAnimation da = new DoubleAnimation();
                da.From = 2;
                da.To = 1;
                da.Duration = new TimeSpan(0, 0, 0, 0, 500);
                da.AutoReverse = true;
                da.RepeatBehavior = RepeatBehavior.Forever;
                Storyboard.SetTarget(da, m_ellipse);//"CenEllipse");
                Storyboard.SetTargetProperty(da, new PropertyPath("StrokeThickness"));
                m_storyboard.Children.Add(da);
                Application.Current.Resources.Add(m_ellipse.Name + "_SB", m_storyboard);
          
        }

        #region 公共方法
        /// <summary>
        /// 释放相关资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Application.Current.Resources.Remove(m_ellipseName + "_SB");
            if (m_closeTimer != null)
                m_closeTimer.Stop();
            if (m_timer != null)
                m_timer.Stop();
        }
        /// <summary>
        /// 添加要聚合标注
        /// </summary>
        public void AddMark(ImyMark mark)
        {
            if (mark != null && mark.MarkControl != null)
            {
                m_markList.Add(mark);
                if (m_markControl != null && m_markControl is Panel)
                {
                    (m_markControl as Panel).Children.Add(mark.MarkControl);
                    mark.TargetMark = this;
                    Canvas.SetLeft(mark.MarkControl, Canvas.GetLeft(m_ellipse));
                    Canvas.SetTop(mark.MarkControl, Canvas.GetTop(m_ellipse));
                    Canvas.SetZIndex(mark.MarkControl, 1);
                    //mark.Tag = mark.EnableDrag;
                    //mark.EnableDrag = false;
                    mark.MarkControl.Visibility = Visibility.Collapsed;
                    mark.MarkControl.MouseEnter += new MouseEventHandler(OnMouseEnter2);
                    mark.MarkControl.MouseLeave += new MouseEventHandler(OnMouseLeave2);
                    mark.MarkControl.Projection = new PlaneProjection() { CenterOfRotationX = 0.5, CenterOfRotationY = 0.5 };
                    m_textBlock.Text = m_markList.Count.ToString();
                    m_len = m_len + (mark.MarkControl.Width == double.NaN ? 16 : mark.MarkControl.Width) + (mark.MarkControl.Height == double.NaN ? 16 : mark.MarkControl.Height);
                    Line line = new Line()
                    {
                        X1 = 10,//m_markControl.ActualWidth > 0 ? m_markControl.ActualWidth / 2 : 0,//m_markControl.Width / 2,
                        Y1 = 10,//m_markControl.Width / 2,
                        X2 = 10,//m_markControl.Width / 2,
                        Y2 = 10,//m_markControl.Width / 2,
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.Black)//,
                        //Visibility = Visibility.Collapsed
                    };
                    (m_markControl as Panel).Children.Add(line);
                    m_lineArr.Add(line);
                    Init();
                }
            }
        }
        /// <summary>
        /// 删除要聚合标注
        /// </summary>
        public bool RemoveMark(ImyMark mark)
        {
            if (mark != null && mark.MarkControl != null)
            {
                //if (mark.Tag is bool)
                //    mark.EnableDrag = (bool)mark.Tag;
                m_markList.Remove(mark);
                m_textBlock.Text = m_markList.Count.ToString();
                if (m_markControl != null && m_markControl is Panel)
                {
                    mark.MarkControl.MouseEnter -= new MouseEventHandler(OnMouseEnter2);
                    mark.MarkControl.MouseLeave -= new MouseEventHandler(OnMouseLeave2);
                    if ((m_markControl as Panel).Children.Remove(mark.MarkControl))
                    {
                        mark.TargetMark = null;
                        mark.MarkControl.Projection = null;
                        if (mark.MarkControl.Visibility == Visibility.Collapsed)
                            mark.MarkControl.Visibility = Visibility.Visible;
                        m_len = m_len - (mark.MarkControl.Width == double.NaN ? 16 : mark.MarkControl.Width) - (mark.MarkControl.Height == double.NaN ? 16 : mark.MarkControl.Height);
                        Init();
                        (m_markControl as Panel).Children.Remove(m_lineArr[0]);
                        m_lineArr.RemoveAt(0);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 删除所有聚合标注
        /// </summary>
        public void RemoveAll()
        {
            for (int i = 0; i < m_markList.Count; i++)
            {
                //if (m_markList[i].Tag is bool)
                //    m_markList[i].EnableDrag = (bool)m_markList[i].Tag;
                m_markList[i].MarkControl.MouseEnter -= new MouseEventHandler(OnMouseEnter2);
                m_markList[i].MarkControl.MouseLeave -= new MouseEventHandler(OnMouseLeave2);
                m_markList[i].TargetMark = null;
            }
            m_markList.Clear();
            m_textBlock.Text = "0";
            if (m_markControl != null && m_markControl is Panel)
                (m_markControl as Panel).Children.Clear();
            m_lineArr.Clear();
            m_len = 0;
        }
        #endregion

        #region 事件回调
        /// <summary>
        /// 关闭标注计数器停止标志位
        /// </summary>
        protected bool m_closeStop = false;
        /// <summary>
        /// 鼠标从Mark上移开事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void OnMouseLeave1(object sender, MouseEventArgs e)
        {
            if (this.m_openTimer.IsEnabled)
                this.m_openTimer.Stop();
            if (m_storyboard != null)
                m_storyboard.Stop();
            if (m_closeTimer.IsEnabled)
                m_closeTimer.Stop();
            m_closeStop = false;
            if (m_isOpen)
                m_closeTimer.Start();
        }        
        /// <summary>
        /// 鼠标移上Mark事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void OnMouseEnter1(object sender, MouseEventArgs e)
        {
            if (m_mapContainer.MouseEventLockFlg)
                return;
            if (m_storyboard != null)
                m_storyboard.Begin();
            m_closeStop = true;
            if (m_closeTimer.IsEnabled)
                m_closeTimer.Stop();
            if (!m_isOpen)
            {
                if (!this.m_openTimer.IsEnabled)
                    this.m_openTimer.Start();
                //Open();
            }
        }
        /// <summary>
        /// 鼠标从线上移开事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void OnMouseLeave2(object sender, MouseEventArgs e)
        {
            if (m_closeTimer.IsEnabled)
                m_closeTimer.Stop();
            m_closeStop = false;
            if (m_isOpen)
                m_closeTimer.Start();
        }
        /// <summary>
        /// 鼠标移上线事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void OnMouseEnter2(object sender, MouseEventArgs e)
        {
            if (m_mapContainer.MouseEventLockFlg)
                return;
            m_closeStop = true;
            if (m_closeTimer.IsEnabled)
                m_closeTimer.Stop();
        }
        /// <summary>
        /// 打开聚合标注
        /// </summary>
        protected void Open()
        {
            if (m_markControl != null && !m_closeTimer.IsEnabled && !m_isOpen)
            {
                if (m_timer.IsEnabled)
                {
                    m_timer.Stop();
                    if (m_currentRadius <= 0)
                        m_repeatCount = DefaultRepeatCount;
                    else
                    {
                        int num = (int)Math.Round(m_currentRadius / m_offsetSpeed);
                        m_repeatCount = DefaultRepeatCount - num;
                    }
                }
                else
                {
                    m_repeatCount = DefaultRepeatCount;
                }
                m_currentCount = 0;
                m_isOpen = true;
                m_timer.Start();
                Rotation_Tick();
            }
        }
        /// <summary>
        /// 关闭聚合标注
        /// </summary>
        protected void Close()
        {
            if (m_markControl != null && m_isOpen)
            {
                m_closeTimer.Stop();
                if (m_timer.IsEnabled)
                {
                    m_timer.Stop();
                    if (m_currentRadius >= m_radius)
                        m_repeatCount = DefaultRepeatCount;
                    else
                    {
                        int num = (int)Math.Round(m_currentRadius / m_offsetSpeed);
                        m_repeatCount = num;
                    }
                }
                else
                    m_repeatCount = DefaultRepeatCount;
                m_isOpen = false;
                m_currentCount = 0;
                m_timer.Start();
                Rotation_Tick();
            }
        }
        /// <summary>
        /// 延时展开
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void Open_Tick(object sender, EventArgs e)
        {
            this.m_openTimer.Stop();
            Open();
        }
        /// <summary>
        /// 延时关闭计数器回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void Close_Tick(object sender , EventArgs e)
        {
            if (!m_timer.IsEnabled)
            {
                if (m_closeTimer != null)
                    m_closeTimer.Stop();
                if (!m_closeStop && m_isOpen)
                    Close();
            }
        }
        /// <summary>
        /// 旋转计数器响应事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void Rotation_Tick(object sender = null, EventArgs e = null)
        {
            //m_currentCount++;
            if (m_markList.Count > 0) //&& m_currentRadius >= 0 && m_currentRadius <= m_radius)
            {
                double a, globalOffsetX, globalOffsetY;//,w,h;
                PlaneProjection planeProjection;

                for (int i = 0; i < m_markList.Count; i++)
                {
                    if (m_markList[i].MarkControl.Visibility == Visibility.Collapsed)
                        m_markList[i].MarkControl.Visibility = Visibility.Visible;
                    if (m_isOpen)//打开
                    {
                        m_endAngleArr[i] = m_startAngleArr[i] + m_angle * m_currentCount;
                        a = m_endAngleArr[i] * Math.PI / 180.0;
                        m_currentRadius = m_offsetSpeed * m_currentCount;
                    }
                    else
                    {
                        m_startAngleArr[i] = m_endAngleArr[i] - m_angle * m_currentCount;
                        a = m_startAngleArr[i] * Math.PI / 180.0;
                        m_currentRadius = m_offsetSpeed * (m_repeatCount - m_currentCount);
                    }
                    
                    //w = m_markList[i].MarkControl.ActualWidth > 0 ? m_markList[i].MarkControl.ActualWidth / 2 : 0;
                    //h = m_markList[i].MarkControl.ActualHeight > 0 ? m_markList[i].MarkControl.ActualHeight / 2 : 0;
                    globalOffsetX = Math.Round(m_currentRadius * Math.Cos(a));
                    globalOffsetY = Math.Round(m_currentRadius * Math.Sin(a));
                    m_lineArr[i].X2 = globalOffsetX + 10;
                    m_lineArr[i].Y2 = globalOffsetY + 10;
                    planeProjection = m_markList[i].MarkControl.Projection as PlaneProjection;
                    if (planeProjection != null)
                    {
                        planeProjection.GlobalOffsetX = globalOffsetX;
                        planeProjection.GlobalOffsetY = globalOffsetY;
                    }
                }
            }
            if (m_currentCount++ >= m_repeatCount )//|| m_currentRadius < 0 || m_currentRadius > m_radius)
            {
                m_timer.Stop();
                if (!m_isOpen && m_currentRadius <= 0)
                {
                    double angle = 360.0 / m_markList.Count;
                    for (int i = 0; i < m_markList.Count; i++)
                    {
                        m_markList[i].MarkControl.Visibility = Visibility.Collapsed;
                        m_startAngleArr[i] = angle * i;
                        m_lineArr[i].X2 = m_lineArr[i].X1;
                        m_lineArr[i].Y2 = m_lineArr[i].Y1;
                    }
                }
            }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 复位状态
        /// </summary>
        protected void Init()
        {
            if (m_markList.Count > 0)
            {
                m_currentCount = 0;
                //double left = Canvas.GetLeft(m_ellipse);
                //double top = Canvas.GetTop(m_ellipse);
                double angle = 360.0 / m_markList.Count;
                m_startAngleArr = new double[m_markList.Count];
                m_endAngleArr = new double[m_markList.Count];
                for (int i = 0; i < m_markList.Count; i++)
                {
                    //Canvas.SetLeft(m_markList[i].MarkControl, left);
                    //Canvas.SetTop(m_markList[i].MarkControl, top);
                    m_startAngleArr[i] = angle * i;
                    //m_markList[i].MarkControl.Visibility = Visibility.Visible;  
                    //m_endAngleArr[i] = m_startAngleArr[i] + 180;
                }
                double tmp = m_len / 2 / Math.PI;
                m_radius = tmp < DefaultRadius ? DefaultRadius : tmp;
                m_offsetSpeed = m_radius / m_repeatCount;
            }
        }
        #endregion
    }
}
