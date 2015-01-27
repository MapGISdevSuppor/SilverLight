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
using System.Windows.Media.Imaging;
using ZDIMS.Map;
using ZDIMS.BaseLib;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;
using ZDIMSDemo.Controls;

namespace ZDIMSDemo.Controls
{
    /// <summary>
    /// 矢量图层管理
    /// </summary>
    public class LayerStateManager
    {
        /// <summary>
        /// 状态图片数组
        /// </summary>
        public readonly static BitmapImage[] StateBitmapList = new BitmapImage[5] { 
            new BitmapImage(new Uri("/images/tree/v0.PNG",UriKind.Relative)),//不可见
            new BitmapImage(new Uri("/images/tree/v1.PNG",UriKind.Relative)),//可见
            new BitmapImage(new Uri("/images/tree/v3.PNG",UriKind.Relative)),//编辑
            new BitmapImage(new Uri("/images/tree/v2.PNG",UriKind.Relative)),//可查询
            new BitmapImage(new Uri("/images/tree/v4.PNG",UriKind.Relative))//激活
        };
        /// <summary>
        /// 计数器字典
        /// </summary>
        private static Dictionary<string, DispatcherTimer> dictionary = new Dictionary<string, DispatcherTimer>();
        private static MyProgressBar progressBar = new MyProgressBar();

        private IMSCatalog m_catalog = null;//目录
        private bool m_mouseDown = false;
        private Point m_mouseDownCoordinate;
        private LayerState m_layerState = LayerState.Visible;
        private LayerState m_layerStateOld = LayerState.Visible;
        private IMap m_layerObj = null;
        private LayerTreeNodeType m_layerTreeNodeType = LayerTreeNodeType.ChildLayer;
        private TreeViewItem m_treeViewItem = null;
        private Image m_treeNodeImg = null;
        private LayerType m_layerType = LayerType.TileLayer;
        //bool m_flg = false;//立即执行状态切换标志
        bool m_isRefresh = false;
        EnumLayerStatus[] mapDocStatusArrOld = null;
        EnumLayerStatus[,] layerStatusArrOld = null;
        /// <summary>
        /// 图层状态(内部使用)
        /// </summary>
        internal LayerState LayerState1
        {
            set
            {
                m_layerStateOld = m_layerState;
                m_layerState = value;
                m_treeNodeImg.Source = StateBitmapList[(int)m_layerState];
                if (m_layerObj != null)
                {
                    if (m_layerTreeNodeType == LayerTreeNodeType.ChildLayer)
                    {
                        if (m_layerType == LayerType.VectorMapDoc)
                        {
                            VectorMapDoc vectorMapDoc = m_layerObj as VectorMapDoc;
                            if (vectorMapDoc != null)
                            {
                                int index = (m_treeViewItem.Parent as TreeViewItem).Items.IndexOf(m_treeViewItem);                                
                                if (index > -1 && index < vectorMapDoc.LoadMapResult.MapLayerInfo.Length)
                                {
                                    vectorMapDoc.LoadMapResult.MapLayerInfo[index].LayerStatus = GetLayerStatus();
                                }
                            }
                        }
                        else if (m_layerType == LayerType.VectorLayer)
                        {
                            VectorLayer vectorLayer = m_layerObj as VectorLayer;
                            if (vectorLayer != null)
                            {
                                int index = (m_treeViewItem.Parent as TreeViewItem).Items.IndexOf(m_treeViewItem);
                                if (m_layerTreeNodeType != LayerTreeNodeType.GDBLayer)
                                {
                                    int gdbIndex = ((m_treeViewItem.Parent as TreeViewItem).Parent as TreeViewItem).Items.IndexOf(m_treeViewItem.Parent as TreeViewItem);
                                    if (gdbIndex >= 0 && gdbIndex < vectorLayer.LayerObj.LayerAccessInfo.Length &&
                                        index >= 0 && index < vectorLayer.LayerObj.LayerAccessInfo[gdbIndex].LayerInfoList.Length)
                                    {
                                        vectorLayer.LayerStatus[gdbIndex][index] = GetLayerStatus();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 图层状态
        /// </summary>
        public LayerState LayerState
        {
            get { return m_layerState; }
            set
            {
                m_layerStateOld = m_layerState;
                m_layerState = value;
                m_treeNodeImg.Source = StateBitmapList[(int)m_layerState];
                if (m_layerObj != null)
                {
                    if (m_layerTreeNodeType == LayerTreeNodeType.ChildLayer)
                    {
                        if (m_layerType == LayerType.VectorMapDoc)
                        {
                            VectorMapDoc vectorMapDoc = m_layerObj as VectorMapDoc;
                            if (vectorMapDoc != null)
                            {
                                if (!dictionary.ContainsKey(vectorMapDoc.ClientUID))
                                {
                                    DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 1, 200) };
                                    timer.Tick += new EventHandler(OnTick);
                                    dictionary.Add(vectorMapDoc.ClientUID, timer);
                                }
                                int index = (m_treeViewItem.Parent as TreeViewItem).Items.IndexOf(m_treeViewItem);
                                if (index > -1 && index < vectorMapDoc.LoadMapResult.MapLayerInfo.Length)
                                {
                                    if (m_layerState != LayerState.Activation)
                                    {
                                        vectorMapDoc.LoadMapResult.MapLayerInfo[index].LayerStatus = GetLayerStatus();
                                        if (m_layerStateOld != m_layerState)
                                        {
                                            if (dictionary[vectorMapDoc.ClientUID].IsEnabled)
                                                dictionary[vectorMapDoc.ClientUID].Stop();
                                            dictionary[vectorMapDoc.ClientUID].Start();
                                        }
                                        if (m_treeNodeImg.Tag is RadioButton)
                                            (m_treeNodeImg.Tag as RadioButton).IsChecked = false;
                                    }
                                    else
                                    {
                                        //m_catalog.ActiveMapDoc = vectorMapDoc;
                                        vectorMapDoc.ActiveLayerIndex = index;
                                        if (m_treeNodeImg.Tag is RadioButton)
                                            (m_treeNodeImg.Tag as RadioButton).IsChecked = true;
                                        if (m_layerStateOld != LayerState.Edit)
                                        {
                                            vectorMapDoc.LoadMapResult.MapLayerInfo[index].LayerStatus = EnumLayerStatus.Editable;
                                            if (dictionary[vectorMapDoc.ClientUID].IsEnabled)
                                                dictionary[vectorMapDoc.ClientUID].Stop();
                                            dictionary[vectorMapDoc.ClientUID].Start();
                                        }
                                    }
                                }
                            }
                        }
                        else if (m_layerType == LayerType.VectorLayer)
                        {
                            VectorLayer vectorLayer = m_layerObj as VectorLayer;
                            if (vectorLayer != null)
                            {
                                if (!dictionary.ContainsKey(vectorLayer.ClientUID))
                                {
                                    DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 1, 200) };
                                    timer.Tick += new EventHandler(OnTick);
                                    dictionary.Add(vectorLayer.ClientUID, timer);
                                }
                                int index = (m_treeViewItem.Parent as TreeViewItem).Items.IndexOf(m_treeViewItem);
                                if (m_layerTreeNodeType == LayerTreeNodeType.GDBLayer)
                                {//GDB
                                    //if (index >= 0 && index < vectorLayer.LayerObj.LayerAccessInfo.Length)
                                    //{
                                    //}
                                }
                                else
                                {
                                    int gdbIndex = ((m_treeViewItem.Parent as TreeViewItem).Parent as TreeViewItem).Items.IndexOf(m_treeViewItem.Parent as TreeViewItem);
                                    if (gdbIndex >= 0 && gdbIndex < vectorLayer.LayerObj.LayerAccessInfo.Length &&
                                        index >= 0 && index < vectorLayer.LayerObj.LayerAccessInfo[gdbIndex].LayerInfoList.Length)
                                    {                                        
                                        if (m_layerState != LayerState.Activation)
                                        {
                                            vectorLayer.LayerStatus[gdbIndex][index] = GetLayerStatus();
                                            if (m_layerStateOld != m_layerState)
                                            {
                                                if (dictionary[vectorLayer.ClientUID].IsEnabled)
                                                    dictionary[vectorLayer.ClientUID].Stop();
                                                dictionary[vectorLayer.ClientUID].Start();
                                            }
                                            if (m_treeNodeImg.Tag is RadioButton)
                                                (m_treeNodeImg.Tag as RadioButton).IsChecked = false;
                                        }
                                        else
                                        {
                                            vectorLayer.ActiveGdbIndex = gdbIndex;
                                            vectorLayer.ActiveLayerIndex = index;
                                            if (m_treeNodeImg.Tag is RadioButton)
                                                (m_treeNodeImg.Tag as RadioButton).IsChecked = true;
                                            if (m_layerStateOld != LayerState.Edit)
                                            {
                                                vectorLayer.LayerStatus[gdbIndex][index] = EnumLayerStatus.Editable;
                                                if (dictionary[vectorLayer.ClientUID].IsEnabled)
                                                    dictionary[vectorLayer.ClientUID].Stop();
                                                dictionary[vectorLayer.ClientUID].Start();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m_layerType == LayerType.VectorMapDoc)
                        {
                            VectorMapDoc vectorMapDoc = m_layerObj as VectorMapDoc;
                            if (!dictionary.ContainsKey(vectorMapDoc.ClientUID))
                            {
                                DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 999) };
                                timer.Tick += new EventHandler(OnTick);
                                dictionary.Add(vectorMapDoc.ClientUID, timer);
                            }
                            if (dictionary[vectorMapDoc.ClientUID].IsEnabled)
                                dictionary[vectorMapDoc.ClientUID].Stop();
                            dictionary[vectorMapDoc.ClientUID].Start();
                        }
                        else if (m_layerType == LayerType.VectorLayer)
                        {
                            VectorLayer vectorLayer = m_layerObj as VectorLayer;
                            if (!dictionary.ContainsKey(vectorLayer.ClientUID))
                            {
                                DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 999) };
                                timer.Tick += new EventHandler(OnTick);
                                dictionary.Add(vectorLayer.ClientUID, timer);
                            }
                            if (dictionary[vectorLayer.ClientUID].IsEnabled)
                                dictionary[vectorLayer.ClientUID].Stop();
                            dictionary[vectorLayer.ClientUID].Start();
                        }
                    }
                }
                if (m_layerState != LayerState.Activation)
                    m_layerStateOld = m_layerState;
            }
        }
        /// <summary>
        /// 上一次图层状态
        /// </summary>
        public LayerState LayerStateOld
        {
            get { return m_layerStateOld; }
        }
        /// <summary>
        /// 图层对象
        /// </summary>
        public IMap LayerObj
        {
            get { return m_layerObj; }
        }
        /// <summary>
        /// 树节点image对象
        /// </summary>
        public Image TreeNodeImg
        {
            get { return m_treeNodeImg; }
        }
        /// <summary>
        /// 节点图层类型
        /// </summary>
        public LayerTreeNodeType LayerTreeNodeType
        {
            get { return m_layerTreeNodeType; }
        }
        /// <summary>
        /// 图层上RadioButton对象
        /// </summary>
        public RadioButton LayerRadioBtn { get; set; }

        public LayerStateManager(IMSCatalog catalog, IMap layerObj, TreeViewItem item, Image treeNodeImg,
            LayerTreeNodeType layerTreeNodeType = LayerTreeNodeType.ChildLayer, LayerState layerState = LayerState.Visible)
        {
            if (item != null && treeNodeImg != null)
            {
                this.m_layerState = layerState;
                this.m_layerStateOld = layerState;
                m_catalog = catalog;
                m_layerObj = layerObj;
                if (m_layerObj is VectorLayer)
                {
                    m_layerType = LayerType.VectorLayer;                    
                }
                else if (m_layerObj is VectorMapDoc)
                {
                    m_layerType = LayerType.VectorMapDoc;                    
                }
                m_treeViewItem = item;
                m_treeNodeImg = treeNodeImg;
                m_layerTreeNodeType = layerTreeNodeType;
                //m_treeNodeImg.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
                //m_treeNodeImg.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseLeftButtonUp);
                m_treeNodeImg.Source = StateBitmapList[(int)m_layerState];
                if (m_treeNodeImg.Tag is RadioButton && layerTreeNodeType == LayerTreeNodeType.ChildLayer)
                    (m_treeNodeImg.Tag as RadioButton).Click += new RoutedEventHandler(RadioButton_Click);
            }
        }
        public void ChangeState(LayerState layerState)
        {
            int s = (int)m_layerState;
            int e = (int)layerState;
            int count = 5;
            if (m_layerTreeNodeType == LayerTreeNodeType.ParentLayer)
                count = 2;
            else if (m_layerTreeNodeType == LayerTreeNodeType.GDBLayer)
                count = 4;
            int len = 0;
            if (e > s)
                len = e - s;
            else if (s > e)
                len = count + e - s;
            for (int i = 0; i < len; i++)
            {
                if (i == len - 1)
                    StateChange(true);//m_flg = true;
                else
                    StateChange();
            }            
        }


        /// <summary>
        /// 改变状态
        /// </summary>
        protected void StateChange(bool flg =false)
        {
            int i = (int)m_layerState;
            int j = (i+1) % 5;
            if (m_layerType == LayerType.TileLayer || m_layerTreeNodeType == LayerTreeNodeType.ParentLayer)
            {
                j = (i + 1) % 2;
                LayerState = (LayerState)j;
                if (m_layerState == LayerState.Visible)
                    m_layerObj.Display = true;
                else if (m_layerState == LayerState.Hidden)
                    m_layerObj.Display = false;
            }
            else
            {
                if (m_layerType == LayerType.VectorMapDoc)
                {
                    
                }
                else
                {
                    if (m_layerTreeNodeType == LayerTreeNodeType.GDBLayer)
                    {
                        j = (i + 1) % 4;
                    }
                    else
                    {
                    }
                }
                LayerState = (LayerState)j;
            }
            CancelOtherActivationState();
            UpdateParent(m_treeViewItem.Parent as TreeViewItem);
            UpdateChildren(m_treeViewItem.Items);
            if (flg && m_layerObj is VectorLayerBase && dictionary.ContainsKey((m_layerObj as VectorLayerBase).ClientUID) &&
                dictionary[(m_layerObj as VectorLayerBase).ClientUID].IsEnabled)
            {
                dictionary[(m_layerObj as VectorLayerBase).ClientUID].Stop();
                OnTick();
            }
        }
        private void CancelOtherActivationState()
        {
            if (this.m_layerState == LayerState.Activation && this.LayerTreeNodeType == LayerTreeNodeType.ChildLayer)
            {//取消其他图层激活状态
                LayerStateManager layerStateManager;
                if (m_layerType == LayerType.VectorMapDoc)
                {
                    TreeViewItem item = m_treeViewItem.Parent as TreeViewItem;                    
                    for (int n = 0; n < item.Items.Count; n++)
                    {
                        if (item.Items[n] is TreeViewItem && (item.Items[n] as TreeViewItem).Tag != null && (item.Items[n] as TreeViewItem).Tag is LayerStateManager)
                        {
                            layerStateManager = (item.Items[n] as TreeViewItem).Tag as LayerStateManager;
                            if (!layerStateManager.Equals(this) && layerStateManager.LayerState == LayerState.Activation)
                            {
                                layerStateManager.LayerState = layerStateManager.LayerStateOld;
                            }
                        }
                    }
                }
                else if (m_layerType == LayerType.VectorLayer)
                {
                    TreeViewItem pItem = (m_treeViewItem.Parent as TreeViewItem).Parent as TreeViewItem;
                    TreeViewItem gdbItem;
                    for (int n = 0; n < pItem.Items.Count; n++)
                    {
                        gdbItem=pItem.Items[n] as TreeViewItem;
                        for (int m = 0; m < gdbItem.Items.Count; m++)
                        {
                            if (gdbItem.Items[m] is TreeViewItem && (gdbItem.Items[m] as TreeViewItem).Tag != null && (gdbItem.Items[m] as TreeViewItem).Tag is LayerStateManager)
                            {
                                layerStateManager = (gdbItem.Items[m] as TreeViewItem).Tag as LayerStateManager;
                                if (!layerStateManager.Equals(this) && layerStateManager.LayerState == LayerState.Activation)
                                {
                                    layerStateManager.LayerState = layerStateManager.LayerStateOld;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void UpdateParent(TreeViewItem parent)
        {
            if (parent != null && parent.Tag != null && parent.Tag is LayerStateManager)
            {
                for (int i = 0; i < parent.Items.Count; i++)
                {
                    if (parent.Items[i] is TreeViewItem && (parent.Items[i] as TreeViewItem).Tag != null && (parent.Items[i] as TreeViewItem).Tag is LayerStateManager)
                    {
                        if (((parent.Items[i] as TreeViewItem).Tag as LayerStateManager).LayerState != this.LayerState)
                            return;
                    }
                    else
                        return;
                }
                LayerStateManager layerStateManager = parent.Tag as LayerStateManager;
                layerStateManager.LayerState = this.LayerState;
                UpdateParent(parent.Parent as TreeViewItem);
                if (layerStateManager.LayerTreeNodeType == LayerTreeNodeType.ParentLayer)
                {
                    if (layerStateManager.LayerState == LayerState.Visible)
                        layerStateManager.LayerObj.Display = true;
                    else if (layerStateManager.LayerState == LayerState.Hidden)
                        layerStateManager.LayerObj.Display = false;
                }
            }
        }
        private void UpdateChildren(ItemCollection children)
        {
            if (children != null && children.Count > 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] is TreeViewItem && (children[i] as TreeViewItem).Tag != null && (children[i] as TreeViewItem).Tag is LayerStateManager)
                    {
                        if (m_layerTreeNodeType == LayerTreeNodeType.ParentLayer)
                            ((children[i] as TreeViewItem).Tag as LayerStateManager).LayerState1 = this.LayerState;
                        else
                            ((children[i] as TreeViewItem).Tag as LayerStateManager).LayerState = this.LayerState; 
                        UpdateChildren((children[i] as TreeViewItem).Items);
                    }
                }                
            }
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.m_mouseDown = true;
            this.m_mouseDownCoordinate = e.GetPosition(m_treeNodeImg);
            e.Handled = true;
        }
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (m_mouseDown)
            {
                Point position = e.GetPosition(m_treeNodeImg);
                if (((position.X == this.m_mouseDownCoordinate.X) && (position.Y == this.m_mouseDownCoordinate.Y)))
                {
                    StateChange();
                    e.Handled = true;
                    this.m_mouseDownCoordinate = new Point(-1, -1);
                }
                m_mouseDown = false;
            }
        }
        private void OnTick(object sender = null, EventArgs e = null)
        {
            if (sender != null)
            {
                (sender as DispatcherTimer).Stop();
            }
            //m_flg = false;
            if (m_layerObj is VectorLayerBase && dictionary.ContainsKey((m_layerObj as VectorLayerBase).ClientUID) &&
                dictionary[(m_layerObj as VectorLayerBase).ClientUID].IsEnabled)
                dictionary[(m_layerObj as VectorLayerBase).ClientUID].Stop();
            if (m_layerObj != null) //&& m_layerTreeNodeType == LayerTreeNodeType.ChildLayer)
            {
                //progressBar. Show( );
                if (m_layerType == LayerType.VectorMapDoc)
                {
                    VectorMapDoc vectorMapDoc = m_layerObj as VectorMapDoc;
                    if (mapDocStatusArrOld == null || mapDocStatusArrOld.Length < vectorMapDoc.LoadMapResult.MapLayerInfo.Length)
                    {
                        mapDocStatusArrOld = new EnumLayerStatus[vectorMapDoc.LoadMapResult.MapLayerInfo.Length];
                        for (int i = 0; i < vectorMapDoc.LoadMapResult.MapLayerInfo.Length; i++)
                            mapDocStatusArrOld[i] = EnumLayerStatus.Visiable;
                    }
                    //保存发送之前的状态
                    for (int i = 0; i < vectorMapDoc.LoadMapResult.MapLayerInfo.Length; i++)
                    {
                        if ((mapDocStatusArrOld[i] == EnumLayerStatus.Invisiable && vectorMapDoc.LoadMapResult.MapLayerInfo[i].LayerStatus == EnumLayerStatus.Visiable) ||
                            (mapDocStatusArrOld[i] != EnumLayerStatus.Invisiable && vectorMapDoc.LoadMapResult.MapLayerInfo[i].LayerStatus == EnumLayerStatus.Invisiable))
                            m_isRefresh = true;
                        mapDocStatusArrOld[i] = vectorMapDoc.LoadMapResult.MapLayerInfo[i].LayerStatus;
                    }

                    vectorMapDoc.UpdateAllLayerInfo(new UploadStringCompletedEventHandler(OnTick_Callback));
                }
                else if (m_layerType == LayerType.VectorLayer)
                {
                    VectorLayer vectorLayer = m_layerObj as VectorLayer;
                    if (layerStatusArrOld == null || layerStatusArrOld.Length < vectorLayer.LayerStatus.Length * vectorLayer.LayerStatus[0].Count)
                    {
                        layerStatusArrOld = new EnumLayerStatus[vectorLayer.LayerStatus.Length, vectorLayer.LayerStatus[0].Count];
                        for (int i = 0; i < vectorLayer.LayerStatus.Length; i++)
                            for (int j = 0; j < vectorLayer.LayerStatus[0].Count; j++)
                                layerStatusArrOld[i, j] = EnumLayerStatus.Visiable;
                    }
                    //保存发送之前的状态
                    for (int i = 0; i < vectorLayer.LayerStatus.Length; i++)
                        for (int j = 0; j < vectorLayer.LayerStatus[0].Count; j++)
                        {
                            if ((layerStatusArrOld[i, j] == EnumLayerStatus.Invisiable && vectorLayer.LayerStatus[i][j] == EnumLayerStatus.Visiable) ||
                                layerStatusArrOld[i, j] != EnumLayerStatus.Invisiable && vectorLayer.LayerStatus[i][j] == EnumLayerStatus.Invisiable)
                                m_isRefresh = true;
                            layerStatusArrOld[i, j] = vectorLayer.LayerStatus[i][j];
                        }

                    CSetEnumLayerStatus setLayerStatus;
                    for (int i = 0; i < vectorLayer.LayerStatus.Length; i++)
                    {
                        setLayerStatus = new CSetEnumLayerStatus();
                        setLayerStatus.GdbIndex = i;
                        setLayerStatus.LayerStatus = new EnumLayerStatus[vectorLayer.LayerStatus[i].Count];
                        vectorLayer.LayerStatus[i].CopyTo(setLayerStatus.LayerStatus);
                        if (i == vectorLayer.LayerStatus.Length - 1)
                            vectorLayer.SetEnumLayerStatus(setLayerStatus, new UploadStringCompletedEventHandler(OnTick_Callback));
                        else
                            vectorLayer.SetEnumLayerStatus(setLayerStatus, null);
                    }
                }
            }            
        }

        private void OnTick_Callback(object sender, UploadStringCompletedEventArgs e)
        {
            progressBar.Close();
            if (m_layerObj != null && m_isRefresh)
                m_layerObj.MapContainer.Refresh();
            m_isRefresh = false;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn != null)
            {
                if (rbtn.IsChecked == true)
                {
                    LayerState = LayerState.Activation;
                    if (LayerRadioBtn!=null)
                    {
                        LayerRadioBtn.IsChecked = true;
                        m_catalog.OnChecked(LayerRadioBtn, null);
                    }
                }
                else if (rbtn.IsChecked == false)
                {
                    LayerState = this.LayerStateOld;
                }
                CancelOtherActivationState();
            }
        }
        /// <summary>
        /// 获取转换图层状态
        /// </summary>
        /// <returns></returns>
        public EnumLayerStatus GetLayerStatus()
        {
            switch (m_layerState)
            {
                case LayerState.Visible:
                    return EnumLayerStatus.Visiable;
                case LayerState.Hidden:
                    return EnumLayerStatus.Invisiable;
                case LayerState.Edit:
                    return EnumLayerStatus.Editable;
                case LayerState.Select:
                    return EnumLayerStatus.Selectable;
                default:
                    return EnumLayerStatus.Visiable;
            }
        }
    }
    /// <summary>
    /// 图层状态
    /// </summary>
    public enum LayerState
    {
        /// <summary>
        /// 不可见
        /// </summary>
        Hidden = 0,
        /// <summary>
        /// 可见
        /// </summary>
        Visible = 1,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 2,
        /// <summary>
        /// 查询
        /// </summary>
        Select = 3,
        /// <summary>
        /// 激活
        /// </summary>
        Activation = 4
    }
    /// <summary>
    /// 图层节点类型
    /// </summary>
    public enum LayerTreeNodeType
    {
        /// <summary>
        /// 父图层
        /// </summary>
        ParentLayer = 0,
        /// <summary>
        /// GDB图层
        /// </summary>
        GDBLayer = 1,
        /// <summary>
        /// 子图层
        /// </summary>
        ChildLayer = 2
    }
    /// <summary>
    /// 图层类型
    /// </summary>
    public enum LayerType
    {
        /// <summary>
        /// 瓦片
        /// </summary>
        TileLayer,
        /// <summary>
        /// 矢量图层
        /// </summary>
        VectorLayer,
        /// <summary>
        /// 矢量文档
        /// </summary>
        VectorMapDoc
    }
}
