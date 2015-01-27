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
using ZDIMS.Interface;
using ZDIMS.Map;
using System.Windows.Threading;

namespace SilverlightDemo.components
{
    /// <summary>
    /// 图形闪烁完毕回调委托
    /// </summary>
    /// <param name="g">闪烁的标注对象</param>
    public delegate void MarkFlickerOverDelegate(MarkBase g);
    /// <summary>
    /// 图形拖拽完毕回调委托
    /// </summary>
    /// <param name="g">标注对象</param>
    public delegate void MarkDragOverDelegate(MarkBase g);
    /// <summary>
    /// 开始图形拖拽回调委托
    /// </summary>
    /// <param name="g">标注对象</param>
    public delegate void MarkDragStartDelegate(MarkBase g);
    /// <summary>
    /// 在地图放缩后，修正标注位置完毕的回调委托
    /// </summary>
    /// <param name="g">标注对象</param>
    public delegate void MarkReviseOverDelegate(MarkBase g);

    /// <summary>
    /// 标注基类
    /// </summary>
    public abstract class MarkBase : ImyMark
    {
        /// <summary>
        /// 图形闪烁完毕回调
        /// </summary>
        public MarkFlickerOverDelegate FlickerOverCallback { get; set; }
        /// <summary>
        /// 图形拖拽完毕回调
        /// </summary>
        public MarkDragOverDelegate MarkDragOverCallback { get; set; }
        /// <summary>
        /// 图形拖拽完毕回调
        /// </summary>
        public MarkDragStartDelegate MarkDragStartCallback { get; set; }
        /// <summary>
        /// 图形拖拽完毕回调
        /// </summary>
        public MarkReviseOverDelegate MarkReviseOverCallback { get; set; }

        #region 内部成员变量
        private bool m_xLogicChange = false;//x,y修改标志
        private bool m_yLogicChange = false;
        /// <summary>
        /// 标注图层对象
        /// </summary>
        protected MarkLayer m_markLayer = null;
        /// <summary>
        /// 要显示的标注点控件对象
        /// </summary>
        protected FrameworkElement m_markControl = null;
        /// <summary>
        /// 标注名称
        /// </summary>
        protected string m_name = null;
        /// <summary>
        /// x坐标位置
        /// </summary>
        protected double m_x = 0;
        /// <summary>
        /// y坐标位置
        /// </summary>
        protected double m_y = 0;
        /// <summary>
        /// 地图容器
        /// </summary>
        protected MapContainerBase m_mapContainer = null;
        /// <summary>
        /// 绘图坐标类型
        /// </summary>
        protected CoordinateType m_coordinateType = CoordinateType.Screen;
        /// <summary>
        /// 闪烁图形计数器
        /// </summary>
        protected DispatcherTimer m_flickerTimer = null;
        /// <summary>
        /// 闪烁总次数
        /// </summary>
        protected int m_flickerNum = 0;
        /// <summary>
        /// 当前闪烁次数
        /// </summary>
        protected int m_curFlickerCount = 0;
        /// <summary>
        /// 是否允许在地图变化时修正标注（注意：只在坐标系为逻辑坐标系时生效，默认允许）
        /// </summary>
        protected bool m_enableRevisedPos = true;
        /// <summary>
        /// 是否允许拖拽标志
        /// </summary>
        protected bool m_enableDrag = false;
        /// <summary>
        /// 鼠标按下标志
        /// </summary>
        protected bool m_isMouseDown = false;
        /// <summary>
        /// 鼠标按下点坐标
        /// </summary>
        protected Point m_mouseDownPnt;
        /// <summary>
        /// 标注位置值
        /// </summary>
        protected double m_tmpLeft = 0;
        /// <summary>
        /// 标注位置值
        /// </summary>
        protected double m_tmpTop = 0;
        /// <summary>
        /// 开始拖拽回调标志位
        /// </summary>
        protected bool m_startDragFlg = false;
        /// <summary>
        /// 设置是否可见
        /// </summary>
        protected Visibility m_visibility = Visibility.Visible;
        #endregion

        #region 属性
        /// <summary>
        /// 定位时需要偏移的x轴像素
        /// </summary>
        public virtual double OffsetX { get; set; }
        /// <summary>
        /// 定位时需要偏移的y轴像素
        /// </summary>
        public virtual double OffsetY { get; set; }
        /// <summary>
        /// 在相对于标注图层左距位置(标注隐藏时，该属性值无效)
        /// </summary>
        public double Left
        {
            get {
                double tmp = double.IsNaN(m_markControl.Width) ? 0 : m_markControl.Height / 2;//m_markControl.ActualWidth > 0 ? m_markControl.ActualWidth / 2 : 0;
                if (m_markControl != null)
                    return Canvas.GetLeft(m_markControl) + tmp;
                return double.NaN;
            }
        }
        /// <summary>
        /// 在相对于标注图层上距位置(标注隐藏时，该属性值无效)
        /// </summary>
        public double Top
        {
            get
            {
                double tmp = double.IsNaN(m_markControl.Height) ? 0 : m_markControl.Height / 2;//m_markControl.ActualHeight > 0 ? m_markControl.ActualHeight / 2 : 0;
                if (m_markControl != null)
                    return Canvas.GetTop(m_markControl) + tmp;
                return double.NaN;
            }
        }

        public virtual string markName {
            get { return m_name; }
            set { m_name = value; }
        }
        /// <summary>
        /// x轴坐标位置
        /// </summary>
        public virtual double X {
            get { return m_x; }
            set
            {
                m_x = value;
                m_xLogicChange = true;
                if (m_markControl != null && m_markLayer != null)
                {
                    if (m_coordinateType == CoordinateType.Logic)// && m_mapContainer != null)
                    {
                        if (m_yLogicChange && m_mapContainer != null)
                        {
                            RevisedPosition();
                            m_yLogicChange = false;
                            m_xLogicChange = false;
                        }
                    }
                    else
                        Canvas.SetLeft(m_markControl, m_x - (double.IsNaN(m_markControl.Width) ? 0 : m_markControl.Width / 2) + OffsetX);//m_markControl.ActualWidth / 2);
                }
            }
        }
        /// <summary>
        /// y轴坐标位置
        /// </summary>
        public virtual double Y {
            get { return m_y; }
            set
            {
                m_y = value;
                m_yLogicChange = true;
                if (m_markControl != null)
                {
                    if (m_coordinateType == CoordinateType.Logic)// && m_mapContainer != null)
                    {
                        if (m_xLogicChange && m_mapContainer != null)
                        {
                            RevisedPosition();
                            m_yLogicChange = false;
                            m_xLogicChange = false;
                        }
                    }
                    else
                        Canvas.SetTop(m_markControl, m_y - m_markControl.ActualHeight / 2 + OffsetY);//(double.IsNaN(m_markControl.Height) ? 0 : m_markControl.Height / 2));
                }
            }
        }
        /// <summary>
        /// 是否允许拖拽标注
        /// </summary>
        public bool EnableDrag {
            get { return m_enableDrag && (m_markControl.Parent is MarkLayer); }
            set {
                m_enableDrag = value;                
                if (m_markControl != null)
                {
                    if (m_enableDrag)
                    {
                        //m_markControl.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
                        //m_markControl.MouseMove += new MouseEventHandler(OnMouseMove);
                        //m_markControl.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseLeftButtonUp);
                        if (m_mapContainer != null)
                            m_mapContainer.MouseMove += new MouseEventHandler(OnMouseMove);
                    }
                    else
                    {
                        //m_markControl.MouseLeftButtonDown -= new MouseButtonEventHandler(OnMouseLeftButtonDown);
                        //m_markControl.MouseMove -= new MouseEventHandler(OnMouseMove);
                        //m_markControl.MouseLeftButtonUp -= new MouseButtonEventHandler(OnMouseLeftButtonUp);
                        if(m_mapContainer!=null)
                            m_mapContainer.MouseMove -= new MouseEventHandler(OnMouseMove);
                    }
                }
            }
        }
        /// <summary>
        ///  获取该标注是否在可视范围内
        /// </summary>
        public bool IsInMapViewBound
        {
            get {
                if (m_mapContainer != null)
                {
                    return m_mapContainer.WinViewBound.Contains(m_x, m_y);
                }
                return false;
            }
        }
        /// <summary>
        /// 标注点控件对象
        /// </summary>
        public FrameworkElement MarkControl { get { return m_markControl; } }
        /// <summary>
        /// 设置是否可见
        /// </summary>
        public Visibility Visibility {
            get
            {
                //if (m_markControl != null)
                //{
                //    if (m_markControl.Visibility == Visibility.Visible && m_visibility == Visibility.Visible)
                //        return Visibility.Visible;
                //}
                return m_visibility;//Visibility.Collapsed;
            }
            set {
                m_visibility = value;
                if (m_markControl != null)
                {
                    m_markControl.Visibility = value;
                    if (m_enableRevisedPos && m_visibility == Visibility.Visible)
                        RevisedPosition(true, m_markLayer.EnableMarkHiden);
                }
            }
        }
        /// <summary>
        /// 标注图层对象
        /// </summary>
        public MarkLayer MarkLayer
        {
            get { return m_markLayer; }
        }
        /// <summary>
        /// 是否允许在地图变化时修正标注（注意：只在坐标系为逻辑坐标系时生效，默认允许）
        /// </summary>
        public bool EnableRevisedPos { get { return m_enableRevisedPos; } set { m_enableRevisedPos = value; } }
        /// <summary>
        /// 设置坐标类型（默认是屏幕坐标，如果是逻辑坐标，在绘图时会根据地图容器的LogicToScreen方法转换坐标）
        /// </summary>
        public CoordinateType CoordinateType
        {
            get { return m_coordinateType; }
            set { m_coordinateType = value; }
        }
        /// <summary>
        /// 关联的目标标注（一般用于聚合标注）
        /// </summary>
        public ImyMark TargetMark { get; set; }
        /// <summary>
        /// 关联目标对象
        /// </summary>
        public object Tag { get; set; } 
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="markControl">要显示的标注点控件对象</param>
        /// <param name="coordinateType">坐标类型（默认是屏幕坐标，如果是逻辑坐标，在绘图时会根据地图容器的LogicToScreen方法转换坐标）</param>
        /// <param name="mLayer">标注图层对象，为“null”时会内部自动获取</param>
        public MarkBase(FrameworkElement markControl, CoordinateType coordinateType = CoordinateType.Screen, MarkLayer mLayer = null)
        {
            if (mLayer != null)
            {
                m_markLayer = mLayer;
                m_mapContainer = mLayer.MapContainer;
            }
            if (markControl != null)
            {
                this.m_markControl = markControl;
                m_coordinateType = coordinateType;
                this.m_markControl.Tag = this;
                this.m_markControl.Loaded += new RoutedEventHandler(OnLoaded);
                m_markControl.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
                m_markControl.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseLeftButtonUp);
            }
        }

        #region 公共接口方法
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            FlickerStop();
            if (m_mapContainer != null)
                m_mapContainer.MouseMove -= new MouseEventHandler(OnMouseMove);
            m_markLayer = null;
            m_mapContainer = null;
        }
        /// <summary>
        /// 闪烁图形
        /// </summary>
        /// <param name="timeSpan">时间间隔(毫秒单位)</param>
        /// <param name="num">闪烁次数(如果传入“-1”将一直闪烁，传入“0”停止闪烁)</param>
        public void Flicker(int timeSpan = 500, int num = 6)
        {
            if (m_markControl == null)
                return;
            if (num == 0 && m_flickerTimer != null)
            {
                m_flickerTimer.Stop();
                m_flickerTimer = null;
                m_markControl.Visibility = Visibility.Visible;
                return;
            }
            m_flickerNum = num * 2;
            m_curFlickerCount = 0;
            if (m_flickerTimer == null)
                m_flickerTimer = new DispatcherTimer();
            m_flickerTimer.Interval = new TimeSpan(0, 0, 0, 0, timeSpan);
            m_flickerTimer.Tick += new EventHandler(FlickerTimer_Tick);
            m_flickerTimer.Start();
            FlickerTimer_Tick();
        }
        /// <summary>
        /// 停止闪烁
        /// </summary>
        public void FlickerStop()
        {
            if (m_flickerTimer != null && m_flickerTimer.IsEnabled)
            {
                m_flickerTimer.Stop();                
                if (m_markControl.Visibility == Visibility.Collapsed)
                    m_markControl.Visibility = Visibility.Visible;
                if (FlickerOverCallback != null)
                    FlickerOverCallback(this);
                m_flickerTimer = null;
            }
        }
        /// <summary>
        /// 修正位置（只在坐标系是逻辑坐标时有效）
        /// </summary>
        /// <param name="isZoom">是否是放缩时修正位置</param>
        /// <param name="enableMarkHiden">是否允许判断并隐藏不在可视范围的标注</param>
        public virtual void RevisedPosition(bool isZoom = true, bool enableMarkHiden = true)
        {
            if (this.TargetMark == null && m_coordinateType == CoordinateType.Logic && m_markControl!=null &&
                m_mapContainer != null && m_markLayer != null && !m_mapContainer.IsMoveing)
            {
                if (enableMarkHiden)
                {
                    if(IsInMapViewBound)
                    {
                        if (isZoom || (m_markControl.Visibility == Visibility.Collapsed && !isZoom))
                        {
                            try
                            {
                                double ml_x = Canvas.GetLeft(this.m_markLayer);
                                double ml_y = Canvas.GetTop(this.m_markLayer);
                                Point pnt = m_mapContainer.LogicToScreen(m_x, m_y);
                                if (!double.IsNaN(pnt.X) && !double.IsNaN(pnt.Y))
                                {
                                    double w = double.IsNaN(m_markControl.Width) ? 0 : m_markControl.Width / 2; //m_markControl.ActualWidth / 2;//
                                    double h = double.IsNaN(m_markControl.Height) ? 0 : m_markControl.Height / 2; //m_markControl.ActualHeight / 2;//
                                    Canvas.SetLeft(m_markControl, pnt.X - w - ml_x + OffsetX);
                                    Canvas.SetTop(m_markControl, pnt.Y - h - ml_y + OffsetY);
                                    if (m_markControl.Visibility == Visibility.Collapsed)
                                        this.m_markControl.Visibility = Visibility.Visible;
                                }
                            }
                            catch { }
                        }
                    }
                    else if (m_markControl.Visibility == Visibility.Visible)
                        m_markControl.Visibility = Visibility.Collapsed;

                    if (MarkReviseOverCallback != null && isZoom)
                        MarkReviseOverCallback(this);
                }
                else
                {
                    if (isZoom)
                    {
                        try
                        {
                            double ml_x = Canvas.GetLeft(this.m_markLayer);
                            double ml_y = Canvas.GetTop(this.m_markLayer);
                            Point pnt = m_mapContainer.LogicToScreen(m_x, m_y);
                            if (!double.IsNaN(pnt.X) && !double.IsNaN(pnt.Y))
                            {
                                double w = double.IsNaN(m_markControl.Width) ? 0 : m_markControl.Width / 2; //m_markControl.ActualWidth / 2;//
                                double h = double.IsNaN(m_markControl.Height) ? 0 : m_markControl.Height / 2; //m_markControl.ActualHeight / 2;//
                                Canvas.SetLeft(m_markControl, pnt.X - w - ml_x + OffsetX);
                                Canvas.SetTop(m_markControl, pnt.Y - h - ml_y + OffsetY);
                                if (m_markControl.Visibility == Visibility.Collapsed)
                                    this.m_markControl.Visibility = Visibility.Visible;
                            }
                        }
                        catch { }

                        if (MarkReviseOverCallback != null)
                            MarkReviseOverCallback(this);
                    }
                }
            }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 控件加载完毕
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.m_markControl.Loaded -= new RoutedEventHandler(OnLoaded);
            if (m_markControl != null && m_markControl.Parent is MarkLayer)
            {
                m_markLayer = m_markControl.Parent as MarkLayer;
                m_mapContainer = m_markLayer.MapContainer;
                if (m_mapContainer != null)
                {
                    if (m_xLogicChange || m_yLogicChange)
                    {
                        double ml_x = Canvas.GetLeft(this.m_markLayer);
                        double ml_y = Canvas.GetTop(this.m_markLayer);
                        double w = double.IsNaN(m_markControl.Width) ? 0 : m_markControl.Width / 2; //m_markControl.ActualWidth / 2;//
                        double h = double.IsNaN(m_markControl.Height) ? 0 : m_markControl.Height / 2; //m_markControl.ActualHeight / 2;//
                        if (m_coordinateType == CoordinateType.Logic && m_mapContainer != null)
                        {
                            Point pnt = m_mapContainer.LogicToScreen(m_x, m_y);
                            if (!double.IsNaN(pnt.X) && !double.IsNaN(pnt.Y))
                            {
                                Canvas.SetLeft(m_markControl, pnt.X - w - ml_x);
                                Canvas.SetTop(m_markControl, pnt.Y - h - ml_y);
                            }
                        }
                        else
                        {
                            Canvas.SetLeft(m_markControl, m_x - w - ml_x);
                            Canvas.SetTop(m_markControl, m_y - h - ml_y);
                        }
                    }
                    if (m_enableDrag)
                        m_mapContainer.MouseMove += new MouseEventHandler(OnMouseMove);
                    else
                        m_mapContainer.MouseMove -= new MouseEventHandler(OnMouseMove);
                }
            }
            m_yLogicChange = false;
            m_xLogicChange = false;
        }
        /// <summary>
        /// 鼠标左键按下事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected virtual void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (m_mapContainer != null)
            {
                if (m_mapContainer.MouseEventLockFlg)
                    return;
            }
            if (EnableDrag)
            {
                e.Handled = true;
                m_isMouseDown = true;
                m_tmpLeft = Canvas.GetLeft(m_markControl);
                m_tmpTop = Canvas.GetTop(m_markControl);
                m_mouseDownPnt = e.GetPosition(m_mapContainer);

                m_startDragFlg = true;                
            }
        }
        /// <summary>
        /// 鼠标移动事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown && EnableDrag)
            {
                Point pnt = e.GetPosition(m_mapContainer);
                Canvas.SetLeft(m_markControl, m_tmpLeft + pnt.X - m_mouseDownPnt.X);
                Canvas.SetTop(m_markControl, m_tmpTop + pnt.Y - m_mouseDownPnt.Y);

                if (m_startDragFlg && MarkDragStartCallback != null)
                {
                    MarkDragStartCallback(this);
                    m_startDragFlg = false;
                }
            }
        }
        /// <summary>
        /// 鼠标左键弹起事件回调
        /// </summary>
        protected virtual void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (m_isMouseDown && EnableDrag)
            {
                if (m_coordinateType == CoordinateType.Logic && m_mapContainer != null && m_markLayer != null)
                {
                    Point pnt = m_mapContainer.ScreenToLogic(Canvas.GetLeft(m_markControl) + Canvas.GetLeft(m_markLayer), Canvas.GetTop(m_markControl) + Canvas.GetTop(m_markLayer));
                    m_x = pnt.X;
                    m_y = pnt.Y;
                }
                if (MarkDragOverCallback != null)
                    MarkDragOverCallback(this);
                e.Handled = true;
                m_isMouseDown = false;
                m_startDragFlg = false;
            }
        }
        /// <summary>
        /// 闪烁回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void FlickerTimer_Tick(object sender = null, EventArgs e = null)
        {
            if (m_markControl == null)
                return;
            if (m_markControl.Visibility == Visibility.Collapsed)
                m_markControl.Visibility = Visibility.Visible;
            else
                m_markControl.Visibility = Visibility.Collapsed;
            if (m_flickerNum > 0 && ++m_curFlickerCount >= m_flickerNum)
            {
                FlickerStop();
            }
        }
        #endregion
    }
}
