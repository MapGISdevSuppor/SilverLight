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
using ZDIMS.Map;
using System.Collections.Generic;
using ZDIMS.Interface;
using System.ComponentModel;
using System.Threading;
using ZDIMS.Util;
using System.Windows.Media.Imaging;


namespace SilverlightDemo.components
{
    /// <summary>
    /// 手动添加mark回调委托
    /// </summary>
    /// <param name="markLayer">标注图层对象</param>
    /// <param name="mark">添加标注的对象</param>
    /// <param name="logicPnt">标志的逻辑坐标位置</param>
    public delegate void ManuallyAddMarkDelegate(MarkLayer markLayer, ImyMark mark, Point logicPnt);
    /// <summary>
    /// 标注图层
    /// </summary>
    public class MarkLayer : CustomLayerBase
    {
        /// <summary>
        /// 手动添加标注完毕回调
        /// </summary>
        public ManuallyAddMarkDelegate ManuallyAddMarkOverCallback;
        #region 内部成员变量
        /// <summary>
        /// 放缩地图后使用的上下文管理（用于跨线程访问UI）
        /// </summary>
        protected SynchronizationContext m_syncContext1 = null;
        /// <summary>
        /// 移动地图结束时使用的上下文管理（用于跨线程访问UI）
        /// </summary>
        protected SynchronizationContext m_syncContext2 = null;
        /// <summary>
        /// 放缩修正标注线程ID
        /// </summary>
        protected int m_zThreadId = -1;
        /// <summary>
        /// 移动修正标注线程ID
        /// </summary>
        protected int m_mThreadId = -1;
        /// <summary>
        /// 图层中标注管理列表
        /// </summary>
        protected List<ImyMark> m_markArr = new List<ImyMark>();
        /// <summary>
        /// 同步修正标注的最大数，超过该值将开启线程异步修正，默认该值是2000
        /// </summary>
        protected int m_maxMarkNumSyncRevised = 2000;  //同步修正标注的最大数，超过该值将开启线程异步修正，默认该值是2000
        /// <summary>
        /// 最大显示比例
        /// </summary>
        protected double m_maxShowScale = double.MaxValue;//显示比例
        /// <summary>
        /// 最小显示比例
        /// </summary>
        protected double m_minShowScale = double.MinValue;
        /// <summary>
        /// 是否使用聚合mark标志位
        /// </summary>
        protected bool m_enablePolymericMark = false;//是否使用聚合mark
        /// <summary>
        /// 聚合标注管理列表
        /// </summary>
        protected List<ImyPolymericMark> m_polymericMarkList = new List<ImyPolymericMark>();
        /// <summary>
        /// 手动添加标注的对象
        /// </summary>
        protected ImyMark m_manuallyAddMarkObj = null;
        /// <summary>
        /// 步长
        /// author liutuoli
        /// </summary>
        protected double curGridstep = 100;
        /// <summary>
        /// 缓存数组，用于按网格存储标注
        /// </summary>
        protected List<List<List<ImyMark>>> temMarkArr;
        #endregion

        #region 属性
        /// <summary>
        /// 图层中标注管理列表
        /// </summary>
        [Browsable(false)]
        public List<ImyMark> MarkList
        {
            get
            {
                return m_markArr;
            }
        }
        /// <summary>
        /// 手动添加标注的对象
        /// </summary>
        [Browsable(false)]
        public ImyMark ManuallyAddMarkObj { 
            get { return m_manuallyAddMarkObj; } 
            set {
                if (value == null && m_manuallyAddMarkObj != null)
                    this.RemoveMark(m_manuallyAddMarkObj);
                m_manuallyAddMarkObj = value; 
                if (value != null) { 
                    //map.m_operType = IMSOperType.None;
                    if (!m_markArr.Contains(m_manuallyAddMarkObj))
                        this.AddMark(m_manuallyAddMarkObj);
                } 
            }
        }
        /// <summary>
        /// 聚合判断的半径，单位是像素单位，默认值是65
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("聚合判断的半径，单位是像素单位"), DefaultValue(65)]
        public double MergeRadius { get; set; }
        /// <summary>
        /// 是否使用聚合标注功能
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("是否使用聚合标注功能"), DefaultValue(false)]
        public bool EnablePolymericMark
        {
            get { return m_enablePolymericMark; }
            set
            {
                m_enablePolymericMark = value;
                if (!m_enablePolymericMark)
                {
                    cancelPolymeric();
                }
                else
                {
                    cancelPolymeric();
                    InitMergeMark();
                }
            }
        }
        /// <summary>
        /// 是否允许判断并隐藏不在可视范围的标注
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("是否允许判断并隐藏不在可视范围的标注"), DefaultValue(true)]
        public bool EnableMarkHiden { get; set; }
        /// <summary>
        /// 设置标注图层最大显示时的比例（注意：当存在瓦片图层时，显示比例等效于级数，当前级数小于等于该值时显示）
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("设置标注图层最大显示时的比例（注意：当存在瓦片图层时，显示比例等效于级数，当前级数小于等于该值时显示）"), DefaultValue(double.MaxValue)]
        public double MaxShowScale
        {
            get { return m_maxShowScale; }
            set { m_maxShowScale = value; }
        }
        /// <summary>
        /// 设置标注图层最小显示时的比例（注意：当存在瓦片图层时，显示比例等效于级数，当前级数大于等于该值时显示）
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("设置标注图层最小显示时的比例（注意：当存在瓦片图层时，显示比例等效于级数，当前级数大于等于该值时显示）"), DefaultValue(double.MinValue)]
        public double MinShowScale
        {
            get { return m_minShowScale; }
            set { m_minShowScale = value; }
        }
        /// <summary>
        /// 同步修正标注的最大数，超过该值将开启线程异步修正，默认该值是2000
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("同步修正标注的最大数，超过该值将开启线程异步修正"), DefaultValue(2000)]
        public int MaxMarkNumSyncRevised
        {
            get { return m_maxMarkNumSyncRevised; }
            set { m_maxMarkNumSyncRevised = value; }
        }
        /// <summary>
        /// 是否开启GPU加速模式
        /// 并且在页面要设置Silverlight插件参数允许开启GPU加速才能完全开启（param name="enableGPUAcceleration" value="true"）
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("GPU加速模式"), DefaultValue(false)]
        public override bool EnableGPUMode
        {
            get { return m_enableGPUMode; }
            set
            {
                m_enableGPUMode = value;
                if (value)
                {
                    for (int i = 0; i < m_markArr.Count; i++)
                        if (m_markArr[i].MarkControl.CacheMode == null)
                            m_markArr[i].MarkControl.CacheMode = new BitmapCache() { RenderAtScale = m_renderAtScale };
                }
                else
                {
                    for (int i = 0; i < m_markArr.Count; i++)
                        m_markArr[i].MarkControl.CacheMode = null;
                }
            }
        }
        /// <summary>
        /// 孩子节点属性（该属性已禁用!）
        /// </summary>
        [Obsolete("该属性已禁用!", true)]
        public new UIElementCollection Children
        {
            get { return base.Children; }
        }
        /// <summary>
        /// 标注总个数
        /// </summary>
        public override int ChildrenCount
        {
            get { return m_markArr.Count; }
        }        
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 下载示例代码<br/>
        /// <a href="http://www.lbsmap.net/imsdownload/MarkLayer_Demo.rar">MarkLayer_Demo</a>
        /// </remarks>
        public MarkLayer()
        {
            //MergeRadius = 65;
            EnableMarkHiden = true;
            EnableZoomAnimation = false;    
        }

        //protected class m_mark
        //{
        //    public List<ImyMark> mark1{get;set;}
        //}

        #region 公共方法
        /// <summary>
        /// 添加标注到图层
        /// </summary>
        /// <param name="mark">标注对象</param>
        public void AddMark(ImyMark mark)
        {
            if (mark != null && mark.MarkControl != null && !m_markArr.Contains(mark))
            {
                m_markArr.Add(mark);
                base.Children.Add(mark.MarkControl);
                if (this.EnableGPUMode)
                    mark.MarkControl.CacheMode = new BitmapCache() { RenderAtScale = m_renderAtScale };
                //if (mark.MarkControl.Visibility == Visibility.Collapsed)
                //    mark.MarkControl.Visibility = Visibility.Visible;
                if (mark.EnableRevisedPos && mark.Visibility == Visibility.Visible)
                    mark.RevisedPosition(true, EnableMarkHiden);
            }
        }
        /// <summary>
        /// 添加标注到图层
        /// </summary>
        /// <param name="mark">标注对象</param>
        /// <param name="zIndex">显示的顺序</param>
        public void AddMarkAt(ImyMark mark, int zIndex)
        {
            AddMark(mark);
            if (zIndex > -1 && mark != null && mark.MarkControl != null)
                Canvas.SetZIndex(mark.MarkControl, zIndex);
            ClusterMark(m_markArr,100);
        }
        /// <summary>
        /// 移除mark
        /// </summary>
        /// <param name="mark">标注对象</param>
        /// <returns>成功返回“true”，否则返回“false”</returns>
        public bool RemoveMark(ImyMark mark)
        {
            if (mark != null && mark.MarkControl != null)
            {
               // mark.Dispose();
                m_markArr.Remove(mark);
                return base.Children.Remove(mark.MarkControl);
            }
            return false;
        }
        /// <summary>
        /// 移除所有mark
        /// </summary>
        public void RemoveAll()
        {
            cancelPolymeric();//包含释放聚合列表的方法
            for (int i = 0; i < m_markArr.Count; i++)
            {
                m_markArr[i].Dispose();
            }
            base.Children.Clear();
            m_markArr.Clear();
        }
        /// <summary>
        /// 根据索引获取Mark对象
        /// </summary>
        /// <param name="index">在图层中的索引</param>
        /// <returns>Mark对象,没有返回null</returns>
        public ImyMark GetMarkByIndex(int index)
        {
            if (index > -1 && index < m_markArr.Count)
                return m_markArr[index];
            return null;
        }
        /// <summary>
        /// 是否包含特定mark对象
        /// </summary>
        /// <param name="mark">mark对象</param>
        /// <returns>包含返回“true”，否则返回“false”</returns>
        public bool Contains(ImyMark mark)
        {
            return base.Children.Contains(mark.MarkControl);
        }
        /// <summary>
        /// 拷贝一个本图层的副本
        /// </summary>
        /// <param name="enableCopySysUIData">是否拷贝图层数据</param>
        /// <param name="enableCopyUserCfgData">是否拷贝图层配置信息</param>
        /// <returns>该图层的副本对象</returns>
        public override FrameworkElement Clone(bool enableCopySysUIData = true, bool enableCopyUserCfgData = false)
        {
            return new MarkLayer();
        }    
        /// <summary>
        /// 鼠标左键按下 （该方法一般内部自动调用，一般不在外部通过对象调用 ） 
        /// </summary>
        /// <param name="e">事件的数据源</param>
        public override void MapMouseDown(EventArgs e)
        {
            base.MapMouseDown(e);
            if (/*map.m_operType == IMSOperType.None &&*/ m_manuallyAddMarkObj != null && m_manuallyAddMarkObj.MarkControl != null)
            {
                m_manuallyAddMarkObj.CoordinateType = CoordinateType.Logic;
                m_manuallyAddMarkObj.X = map.MouseDownLogicPnt.X;
                m_manuallyAddMarkObj.Y = map.MouseDownLogicPnt.Y;
                //IMark m = m_manuallyAddMarkObj;
                //m_manuallyAddMarkObj = null;
                if (ManuallyAddMarkOverCallback != null)
                    ManuallyAddMarkOverCallback(this, m_manuallyAddMarkObj, new Point(map.MouseDownLogicPnt.X, map.MouseDownLogicPnt.Y));
            }            
        }
        /// <summary>
        /// 鼠标移动（该方法一般内部自动调用，一般不在外部通过对象调用 ）
        /// </summary>
        /// <param name="e">事件的数据源</param>
        public override void MapMouseMove(EventArgs e)
         {
            base.MapMouseMove(e);
            if (/*map.m_operType == IMSOperType.None &&*/ m_manuallyAddMarkObj != null && m_manuallyAddMarkObj.MarkControl != null)
            {
                Canvas.SetLeft(m_manuallyAddMarkObj.MarkControl, map.MouseMoveScrPnt.X - Canvas.GetLeft(this) + 5);
                Canvas.SetTop(m_manuallyAddMarkObj.MarkControl, map.MouseMoveScrPnt.Y - Canvas.GetTop(this));
            }
            //else
            //    m_manuallyAddMarkObj = null;
        }
        /// <summary>
        /// 鼠标左键弹起（该方法一般内部自动调用，一般不在外部通过对象调用 ）
        /// </summary>
        /// <param name="e">事件的数据源</param>
        public override void MapMouseUp(EventArgs e)
        {//屏蔽父类的MapMouseUp操作
            if (map.OperType == IMSOperType.Drag && EnableMarkHiden)
            {
                //if (m_markArr.Count > m_maxMarkNumSyncRevised)
                if (base.Children.Count > m_maxMarkNumSyncRevised)
                {
                    m_syncContext2 = SynchronizationContext.Current;
                    Thread mThread = new Thread(new ParameterizedThreadStart(MRevisedMarkPosition));
                    m_mThreadId = mThread.ManagedThreadId;
                    mThread.Start(mThread.ManagedThreadId);
                }
                else
                {
                    m_mThreadId = 0;
                    MRevisedMarkPosition1(0);
                }
            }
        }
        /// <summary>
        /// 键盘按键弹起（该方法一般内部自动调用，一般不在外部通过对象调用 ）
        /// </summary>
        /// <param name="e">事件的数据源</param>
        public override void MapKeyUp(KeyEventArgs e)
        {//屏蔽父类的MapKeyUp操作
            if (e.PlatformKeyCode > 36 && e.PlatformKeyCode < 41)
            {
                if (!map.DirectionKey.IsLeftKeyDown &&
                    !map.DirectionKey.IsUpKeyDown &&
                    !map.DirectionKey.IsRightKeyDown &&
                    !map.DirectionKey.IsBottomKeyDown)
                {
                    MapMouseUp(null);
                }
            }
        }
        /// <summary>
        /// 刷新图层
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            if (this.Display && map!=null)
            {
                double scale = map.CurShowScale;
                if ((map.TileLayerLength > 0 && map.CurMapLevel <= m_maxShowScale && map.CurMapLevel >= m_minShowScale) ||
                    (map.TileLayerLength == 0 && map.VectorLayerLenth > 0 && scale <= m_maxShowScale && scale >= m_minShowScale))
                {
                    if (base.Children.Count > m_maxMarkNumSyncRevised)
                    {
                        m_syncContext1 = SynchronizationContext.Current;
                        Thread zThread = new Thread(new ParameterizedThreadStart(RevisedMarkPosition));
                        m_zThreadId = zThread.ManagedThreadId;
                        zThread.Start(zThread.ManagedThreadId);
                    }
                    else
                    {
                        m_zThreadId = 0;
                        RevisedMarkPosition1(0);
                    }
                    if (this.Visibility == Visibility.Collapsed)
                        this.Visibility = Visibility.Visible;
                }
                else if (this.Visibility == Visibility.Visible)
                    this.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region 内部回调方法
        private void RevisedMarkPosition(object stateInfo)
        {
            m_syncContext1.Post(new SendOrPostCallback(RevisedMarkPosition1), stateInfo);
        }
        private void MRevisedMarkPosition(object stateInfo)
        {
            m_syncContext2.Post(new SendOrPostCallback(MRevisedMarkPosition1), stateInfo);
        }
        private void RevisedMarkPosition1(object stateInfo)
        {
            try
            {
                int id = (int)stateInfo;
                ImyMark mark;
                for (int i = 0; i < base.Children.Count; i++)
                {
                    if (base.Children[i] is FrameworkElement && (base.Children[i] as FrameworkElement).Tag is ImyMark)
                    {
                        if (m_zThreadId != id)
                            return;
                        mark = (base.Children[i] as FrameworkElement).Tag as ImyMark;
                        if (mark!=null && mark.EnableRevisedPos && mark.Visibility == Visibility.Visible)
                            mark.RevisedPosition(true, EnableMarkHiden);
                    }
                }
                MergeMark();
            }
            catch (ThreadAbortException)
            {
            }
            //catch (Exception)
            //{
            //}
        }
        private void MRevisedMarkPosition1(object stateInfo)
        {
            try
            {
                int id = (int)stateInfo;
                //for (int i = 0; i < m_markArr.Count; i++)
                ImyMark mark;
                for (int i = 0; i < base.Children.Count; i++)
                {
                    if (base.Children[i] is FrameworkElement && (base.Children[i] as FrameworkElement).Tag is ImyMark)
                    {
                        if (m_mThreadId != id)
                            return;
                        mark = (base.Children[i] as FrameworkElement).Tag as ImyMark;
                        if (mark.EnableRevisedPos)
                            mark.RevisedPosition(false, EnableMarkHiden);
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            //catch (Exception)
            //{
            //}
        }
        /// <summary>
        /// 遍历所有标注，计算标注对应的网格
        /// </summary>
        /// <author>liuruoli</author>
        /// <param name="allMarks">所有标注对象</param>
        /// <param name="step">网格划分步长</param>
        protected void ClusterMark(List<ImyMark> allMarks,double step)
        {
            if (allMarks == null||allMarks.Count<=0)
                return;
            curGridstep = step;
            for (int i = 0; i < allMarks.Count; i++)
            {
                calGrid(map,allMarks[i],step);
            }
            CreateClusterMark(temMarkArr);
        }
        /// <summary>
        /// 按网格合并mark，并显示聚合标注
        /// </summary>
        /// <param name="markArr">网格数组，存储了网格范围内的所有mark</param>
        protected void CreateClusterMark(List<List<List<ImyMark>>> markArr)
        {
            ImyMark mark;
            ImyPolymericMark polymericmark;
            int m_index;
            if (markArr == null || markArr.Count == 0)
            {
                MessageBox.Show("mark数组为空");
                return;
            }
            for (int i = 0; i < markArr.Count; i++)
            {//第i行包含的标注
                if (markArr[i] == null || markArr[i].Count <= 0)
                    continue;
                for (int j = 0; j < markArr[i].Count; j++)
                { //第j列的标注
                    
                        if (markArr[i][j] == null || markArr[i][j].Count <= 0)
                            continue;
                        polymericmark = new IMSmyPolymericMark(CoordinateType.Logic, this);
                        //随机选择一个mark，获取其地图位置
                        Random ra = new Random();
                        m_index = new int();
                        m_index = (int)Math.Floor(ra.Next(markArr[i][j].Count-1));
                        for (int m = 0; m < markArr[i][j].Count; m++)
                        {
                            if (markArr[i][j][m].TargetMark == null && markArr[i][j][m].CoordinateType == CoordinateType.Logic && markArr[i][j][m].Visibility == Visibility.Visible)
                            {
                                mark = markArr[i][j][m];
                                this.RemoveMark(mark);
                                polymericmark.AddMark(mark);
                            }
                        }
                        if (polymericmark != null)
                        {
                            if (polymericmark.MarkList.Count == 0)
                                return;
                            if (polymericmark.MarkList.Count == 1)
                            {
                                mark = polymericmark.MarkList[0];
                                polymericmark.RemoveMark(mark);
                                this.AddMark(mark);
                                mark.EnableRevisedPos = true;
                                mark.RevisedPosition(true, true);
                            }
                            else if (polymericmark.MarkList.Count > 1)
                            {
                                base.Children.Add(polymericmark.MarkControl);
                                m_polymericMarkList.Add(polymericmark);
                                polymericmark.X = markArr[i][j][m_index].X;
                                polymericmark.Y = markArr[i][j][m_index].Y;
                                polymericmark.EnableRevisedPos = true;
                                polymericmark.RevisedPosition(true, true);

                            }
                        }
                    }
                polymericmark = null;
            }
        }
        /// <summary>
        /// 把一个mark按照网格的大小，分配到具体的网格中
        /// </summary>
        /// <param name="map">地图容器，需要该地图容器中已经装载基础图层</param>
        /// <param name="mark"></param>
        /// <param name="gridstep">单位为像素，网格大小（int）,网格设计为gridstep*gridstep</param>
        /// <returns></returns>
        protected bool calGrid(MapContainerBase map,ImyMark mark,double gridstep)
        {
            if (map == null)
            {
                MessageBox.Show("没有地图！");
                return false;
            }
            if (mark == null)
            {
                MessageBox.Show("没有标注！");
                return false;
            }
            if (mark.X < map.WinViewBound.XMin || mark.X > map.WinViewBound.XMax || mark.Y < map.WinViewBound.YMin || mark.Y > map.WinViewBound.YMax)
            {
                //MessageBox.Show("mark没在当前视图范围内");
                return false;
            }
            if (gridstep > map.CurShowScale * (map.WinViewBound.XMax - map.WinViewBound.XMin) || gridstep > map.CurShowScale * (map.WinViewBound.YMax - map.WinViewBound.YMin))
            {
                MessageBox.Show("网格步长太长");
                return false;
            }
            int col = new int();
            int row = new int();
            col =(int)Math.Floor((mark.X-map.WinViewBound.XMin)/(gridstep/map.CurShowScale));
            row = (int)Math.Floor((mark.Y-map.WinViewBound.YMin)/(gridstep/map.CurShowScale));
            if (temMarkArr[row][col] == null)
                temMarkArr[row][col] = new List<ImyMark>();
            temMarkArr[row][col].Add(mark);
            return true;
        }
        /// <summary>
        /// 检测是否需要合并mark
        /// </summary>
        protected void MergeMark()
        {
            if (m_enablePolymericMark)
            {
                double r = MergeRadius / map.CurShowScale;
                int dx = map.CurMapLevel - (map as IMSMap).OldLevel;
                if (dx < 0)//缩小
                {
                    ZoomOutMergeMark(r);
                }
                else if (dx > 0)//放大
                {
                    ZoomInMergeMark(r);
                }
                else if(m_polymericMarkList.Count==0)//初始化
                {
                    InitMergeMark();
                }
            }
        }
        /// <summary>
        /// 初始化时合并mark,计算网格大小，并将标注按网格存储到数组temMarkArr中
        /// </summary>
        /// <param name="r">合并判断的半径</param>
        protected void InitMergeMark()
        {
            curGridstep = 100;
            int a = (int)(Math.Ceiling(map.CurShowScale * (map.WinViewBound.YMax - map.WinViewBound.YMin) / curGridstep));
            int b = (int)(Math.Ceiling(map.CurShowScale * (map.WinViewBound.XMax - map.WinViewBound.XMin) / curGridstep));
            //初始化网格标注数组
            temMarkArr = new List<List<List<ImyMark>>>();
            for (int i = 0; i < a; i++)
            {
                temMarkArr.Add(new List<List<ImyMark>>());
                for (int j = 0; j < b; j++)
                {
                    temMarkArr[i].Add(new List<ImyMark>());
              
                }
               
            }
            ClusterMark(m_markArr,100);
        }

        /// <summary>
        /// 解除聚合标注
        /// </summary>
        protected void cancelPolymeric()
        {
            ImyMark mark;
            for (int i = 0; i < m_polymericMarkList.Count; i++)
            {
                for (int j = 0; j < m_polymericMarkList[i].MarkList.Count; j++)
                {
                    mark = m_polymericMarkList[i].MarkList[j];//单个聚合标注包含的标注
                    m_polymericMarkList[i].RemoveMark(mark);
                    mark.RevisedPosition(true,true);
                    this.AddMark(mark);
                    j--;
                }
                if (m_polymericMarkList[i].MarkList.Count <= 0)
                { //删除没有子mark的聚合mark
                    if (m_polymericMarkList[i].MarkControl != null)
                    {
                        base.Children.Remove(m_polymericMarkList[i].MarkControl);
                        m_polymericMarkList[i].Dispose();
                    }
                    m_polymericMarkList.Remove(m_polymericMarkList[i]);
                    i--;
                }
            }
        }
        /// <summary>
        /// 缩小时合并mark
        /// </summary>
        /// <param name="r">合并判断的半径</param>
        protected void ZoomOutMergeMark(double r)
        {
            cancelPolymeric();
            InitMergeMark();
        }
        /// <summary>
        /// 放大时合并mark
        /// </summary>
        /// <param name="r">合并判断的半径</param>
        protected void ZoomInMergeMark(double r)
        {
            cancelPolymeric();
            InitMergeMark();
        }
        #endregion
    }
}
