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
using System.Windows.Controls.Primitives;
using ZDIMS.BaseLib;
using ZDIMS.Map;
using System.Windows.Data;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZDIMS.Util;
using System.Windows.Media.Imaging;
using System.Collections;

namespace ZDIMSDemo.Controls.MapDoc
{
    public partial class MapDocDataViewer : BaseUserControl
    {
        DataGrid m_lastSelDataGrid;
        BindClass m_lastSelItem;
        private IMSCatalog m_catalog = null;//目录
        private CMapSelectParam _lastSelectParam = null;
        private CMapSelectAndGetAtt _lastSelRlt;
        //private int _lastPageCount = 0;
        private int pageSize = 0;
        private SFeatureGeometry m_featureGeo = null;
        private VectorMapDoc activeMapDoc = null;
        private GraphicsLayer m_graphicsLayer = null;
        private GraphicsBase m_graphics = null;
        private BufferAnalyse m_bufferControl = null;//缓冲分析控件
        private Chart m_chartControl = null;//统计图控件
        private MapDocEditor m_mapDocEditor = null;

        public BufferAnalyse BufferControl
        {
            get { return m_bufferControl; }
            set { m_bufferControl = value; }
        }
        public Chart ChartControl
        {
            get { return m_chartControl; }
            set { m_chartControl = value; }
        }
        /// <summary>
        /// 树目录
        /// </summary>
        public IMSCatalog IMSCatalog
        {
            get { return m_catalog; }
            set { m_catalog = value; }
        }
        /// <summary>
        /// 绘图层
        /// </summary>
        public GraphicsLayer GraphicsLayer
        {
            get { return m_graphicsLayer; }
            set { m_graphicsLayer = value; }
        }
        /// <summary>
        /// 工具条
        /// </summary>
        public MapDocEditor MapDocEditorObj
        {
            get { return m_mapDocEditor; }
            set { m_mapDocEditor = value; }
        }
        public MapDocDataViewer()
        {
            InitializeComponent();
            myDialog.OnClose += new RoutedEventHandler(OnClose);            
        }


        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
        /// <summary>
        /// 点击查询回调
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        public void DotSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 0)
            {
                Dot_2D dot = new Dot_2D();
                dot.x = logPntArr[0].X;
                dot.y = logPntArr[0].Y;
                Select(dot, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 框查询
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        public void RectSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                ZDIMS.BaseLib.Rect rect = new ZDIMS.BaseLib.Rect();
                rect.xmin = Math.Min(logPntArr[0].X, logPntArr[1].X);
                rect.xmax = Math.Max(logPntArr[0].X, logPntArr[1].X);
                rect.ymin = Math.Min(logPntArr[0].Y, logPntArr[1].Y);
                rect.ymax = Math.Max(logPntArr[0].Y, logPntArr[1].Y);
                Select(rect, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 圆查询
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        public void CircleSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                Circle obj = new Circle();
                obj.Center = new Dot_2D()
                {
                    x = logPntArr[0].X,
                    y = logPntArr[0].Y
                };
                obj.Radius = Math.Sqrt(Math.Pow(logPntArr[0].X - logPntArr[1].X, 2) + Math.Pow(logPntArr[0].Y - logPntArr[1].Y, 2));
                Select(obj, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 线查询
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        public void LineSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                AnyLine line = new AnyLine();
                Arc arc = new Arc();
                arc.Dots = new Dot_2D[logPntArr.Count];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    arc.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                line.Arcs = new Arc[1] { arc };
                Select(line, SelectionType.SpatialRange);
            }
        }
        /// <summary>
        /// 多边形查询
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        public void PloySelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 2)
            {
                ZDIMS.BaseLib.Polygon ploy = new ZDIMS.BaseLib.Polygon();
                ploy.Dots = new Dot_2D[logPntArr.Count + 1];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    ploy.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                ploy.Dots[ploy.Dots.Length - 1] = ploy.Dots[0];
                Select(ploy, SelectionType.SpatialRange);
            }
        }


        /// <summary>
        /// 地图文档查询
        /// </summary>
        /// <param name="geoObj">查询空间范围对象</param>
        /// <param name="selType">查询类型</param>
        /// <param name="page">结果记录页码</param>
        /// <param name="condition">查询条件</param>
        internal void Select(Object geoObj, SelectionType selType, int page = 0, string condition = "")
        {
            if (m_catalog.ActiveMapDoc == null)
            {
                MessageBox.Show("请选择一个活动矢量文档图层!", "提示", MessageBoxButton.OK);
                return;
            }
            dataPager1.PageIndexChanged -= new EventHandler<EventArgs>(dataPager1_PageIndexChanged);
            CMapSelectParam mapsel = new CMapSelectParam();
            CWebSelectParam websel = new CWebSelectParam();
            websel.CompareRectOnly = m_catalog.ActiveMapDoc.CompareRectOnly;
            websel.Geometry = geoObj;
            if (geoObj != null)
            {
                websel.GeomType = (geoObj as IWebGeometry).GetGeomType();
            }
            websel.MustInside = m_catalog.ActiveMapDoc.MustInside;
            websel.NearDistance = m_catalog.ActiveMapDoc.NearDistanse;
            websel.SelectionType = selType;
            websel.WhereClause = condition;
            mapsel.SelectParam = websel;
            mapsel.PageCount = page;
            this.SetLastSelectParam(mapsel);
            m_catalog.ActiveMapDoc.Select(mapsel, new UploadStringCompletedEventHandler(SelectCallback));
        }
        /// <summary>
        /// 查询完毕回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCallback(object sender, UploadStringCompletedEventArgs e)
        {
            //if (activeMapDoc != null && !activeMapDoc.Equals(m_catalog.ActiveMapDoc))
            //{//不是同一个图层，清除原来的结果
            //    tabControl1.Items.Clear();
            //}
            dataPager1.DataContext = null;
            tabControl1.Items.Clear();
            pageSize = m_catalog.ActiveMapDoc.GetPageSize();
            activeMapDoc = m_catalog.ActiveMapDoc;
            CMapSelectAndGetAtt selRlt = m_catalog.ActiveMapDoc.OnSelect(e);
            _lastSelRlt = selRlt;
            TabItem item;
            string name;
            DataGrid grid = null;
            int maxCount=0;
            for (int i = 0; i < selRlt.Count[0].Length; i++)
            {
                if (selRlt.Count[0][i] > 0 && selRlt.AttDS[0].attTables!=null&&selRlt.AttDS[0].attTables[i].Rows != null)
                {
                    name = activeMapDoc.GetLayerInfo(i).LayerDataName;
                    if (this.FindName(name) == null)
                    {
                        item = new TabItem()
                        {
                            Header = name,
                            Name = name
                        };
                        grid = new DataGrid()
                        {
                            Name = "datagrid" + name + "_" + i,
                            Height = 221,
                            Width = 493,
                            AutoGenerateColumns = false,
                            IsReadOnly = true,
                            Tag = new TmpInfo() { LayerIndex = i }
                        };
                        grid.MouseLeftButtonUp += new MouseButtonEventHandler(DataGrid_MouseLeftButtonUp);
                        grid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRow);
                        item.Content = grid;
                        if (ContextMenuService.GetContextMenu(grid) == null)
                        {
                            ContextMenu contextMenu = GetContexMenu(i);
                            ContextMenuService.SetContextMenu(grid, contextMenu);
                        }
                        tabControl1.Items.Add(item);
                    }
                    else
                    {
                        item = this.FindName(name) as TabItem;
                        grid = item.Content as DataGrid;
                        grid.Columns.Clear();
                        grid.ItemsSource = null;
                    }
                    List<string> addColumnlHeadArr = new List<string>();
                    addColumnlHeadArr.Add("FID");
                    string[][] arr = new string[/*selRlt.Count[0][i]*/selRlt.AttDS[0].attTables[i].Rows.Length + 1][];
                    arr[0] = selRlt.AttDS[0].attTables[i].Columns.FldName;
                    
                    List<string[]> addColumnlContentArr = new List<string[]>();
                    string[] fidArr = new string[selRlt.AttDS[0].attTables[i].Rows.Length];
                    for (int j = 0; j < selRlt.AttDS[0].attTables[i].Rows.Length; j++)
                    {
                        fidArr[j] = selRlt.AttDS[0].attTables[i].Rows[j].FID.ToString();
                        arr[j + 1] = selRlt.AttDS[0].attTables[i].Rows[j].Values;
                    }
                    addColumnlContentArr.Add(fidArr);
                    if (maxCount < selRlt.Count[0][i])
                        maxCount = selRlt.Count[0][i];                    
                    BindClass bingclass = new BindClass();
                    dataPager1.DataContext = new PagedCollectionView(bingclass.ColumnDisplay(grid, arr, addColumnlHeadArr, addColumnlContentArr));
                    item.IsSelected = true;                    
                }
            }
            if (maxCount > 0)
            {
                dataPager1.PageSize = (int)Math.Ceiling(Convert.ToDouble(pageSize * pageSize) / maxCount);
                dataPager1.PageIndexChanged += new EventHandler<EventArgs>(dataPager1_PageIndexChanged);
            }
            if (tabControl1.Items.Count <= 0) //&& grid != null && grid.ItemsSource == null)
                m_graphicsLayer.MapContainer.SetErrorText("矢量文档查询结果为空,请检查图层是否设置为可查询状态或者输入条件是否正确后重试!");
                //MessageBox.Show("没有查询到结果,请更换条件后重试!", "提示", MessageBoxButton.OK);
            else
                this.Show();
        }
        /// <summary>
        /// 设置右键
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
		private ContextMenu GetContexMenu(int layerIndex)
		{
			ContextMenu menu=new ContextMenu();
            MenuItem delItem = new MenuItem();
            delItem.Header = "删除";
            delItem.Click += new RoutedEventHandler(ContextMenuClick_Del);
            menu.Items.Add(delItem);
            MenuItem editItem = new MenuItem();
            editItem.Header = "编辑";
            editItem.Click += new RoutedEventHandler(ContextMenuClick_Edit);
            menu.Items.Add(editItem);
			if (activeMapDoc.ActiveLayerIndex == layerIndex)
			{
                MenuItem bufferItem = new MenuItem();
                bufferItem.Header = "缓冲分析";
                bufferItem.Click += new RoutedEventHandler(ContextMenuClick_bufferFeature);
                menu.Items.Add(bufferItem);
                //var delItem:ContextMenuItem=new ContextMenuItem("删除");
                //delItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, deleteFeature);
                //var editItem:ContextMenuItem=new ContextMenuItem("编辑");
                //editItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, editFeature);
                //var bufferItem:ContextMenuItem=new ContextMenuItem("缓冲分析");
                //bufferItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, bufferFeature);
                //menu.customItems.push(editItem, delItem, bufferItem);
			}
            MenuItem charItem = new MenuItem();
            charItem.Header = "统计图";
            charItem.Click += new RoutedEventHandler(ContextMenuClick_onChart);
            menu.Items.Add(charItem);
            //var charItem:ContextMenuItem=new ContextMenuItem("统计图");
            //charItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, onChart);
            //menu.customItems.push(charItem);
				
            //var selLayerItem:ContextMenuItem = new ContextMenuItem("显示/隐藏图层");
            //selLayerItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT,onSelLayer);
            //menu.customItems.push(selLayerItem);

            //var selFldName:ContextMenuItem=new ContextMenuItem("显示/隐藏属性字段");
            //selFldName.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, onSelFldName);
            //menu.customItems.push(selFldName);
				
            //menu.hideBuiltInItems();
			return menu;
		}
        /// <summary>
        /// 缓冲分析回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuClick_bufferFeature(object sender, RoutedEventArgs e)
        {
            if (this.m_bufferControl == null)
            {
                this.m_bufferControl = new BufferAnalyse();
            }
            this.m_bufferControl.IMSCatalog = m_catalog;
            this.m_bufferControl.VectorBaseObject = m_catalog.ActiveMapDoc;
            int tblIndex = Convert.ToInt32(((System.Windows.FrameworkElement)(((System.Windows.Controls.ContentControl)(tabControl1.SelectedItem)).Content)).Name.ToString().Split('_')[1]);
            m_catalog.ActiveMapDoc.GetMapLayerAttStruct(tblIndex, new UploadStringCompletedEventHandler(onGetMapAttStruct));
        }
        private void onGetMapAttStruct(object sender, UploadStringCompletedEventArgs e)
        {
            CAttStruct attStruct = m_catalog.ActiveMapDoc.OnGetMapLayerAttStruct(e);
            this.m_bufferControl.setTargetattStrct(attStruct);
            BindClass bindobj = ((System.Windows.Controls.DataGrid)(((System.Windows.Controls.ContentControl)(tabControl1.SelectedItem)).Content)).SelectedItem as BindClass;
            CGetObjByID getobj=new CGetObjByID();
            getobj.MapDocIndex = 0;
            getobj.LayerIndex = Convert.ToInt32(((System.Windows.FrameworkElement)(((System.Windows.Controls.ContentControl)(tabControl1.SelectedItem)).Content)).Name.ToString().Split('_')[1]); 
            getobj.FeatureID = Convert.ToInt64(bindobj.key0);
            m_catalog.ActiveMapDoc.GetFeatureByID(getobj, new UploadStringCompletedEventHandler(onGetFeatureByID));
        }
        private void onGetFeatureByID(object sender, UploadStringCompletedEventArgs e)
        {
            SFeature sf = m_catalog.ActiveMapDoc.OnGetFeatureByID(e);
            CAttDataRow[] drArr = new CAttDataRow[1];
            drArr[0] = new CAttDataRow();
            drArr[0].Values = sf.AttValue;
            this.m_bufferControl.setTargetattRows(drArr);
            SFeatureGeometry[] sfGeoArr = new SFeatureGeometry[1];
            sfGeoArr[0] = sf.fGeom;
            this.m_bufferControl.setTargetsfGeometry(sfGeoArr);
            this.m_bufferControl.Show();
        }
        /// <summary>
        /// 右键选中DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += (s, a) =>
            {
                (sender as DataGrid).SelectedIndex = (s as DataGridRow).GetIndex();
                (s as DataGridRow).Focus();
            };
        }
        /// <summary>
        /// 删除回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuClick_Del(object sender, RoutedEventArgs e)
        {
            if (m_catalog.ActiveMapDoc.ActiveLayerIndex < 0)
            {
                MessageBox.Show("请激活该编辑的图层", "提示", MessageBoxButton.OK);
                return;
            }
            TabItem item = tabControl1.SelectedItem as TabItem;
            if (item != null)
            {
                DataGrid datagrid = item.Content as DataGrid;
                if (datagrid != null)
                {
                    m_lastSelDataGrid = datagrid;
                    int tabIndex = Convert.ToInt32(((System.Windows.FrameworkElement)(((System.Windows.Controls.ContentControl)(tabControl1.SelectedItem)).Content)).Name.ToString().Split('_')[1]);
                    if (tabIndex >= 0 && tabIndex < _lastSelRlt.AttDS[0].attTables.Length)
                    {
                        if (MessageBox.Show("你真的要删除吗？执行后将不能撤消！", "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            BindClass bc = datagrid.SelectedItem as BindClass;
                            m_lastSelItem = bc;
                            CGetObjByID getGeo = new CGetObjByID();
                            getGeo.FeatureID = Convert.ToInt64(bc.key0);
                            getGeo.LayerIndex = tabIndex;
                            COpenMap openmap = new COpenMap();
                            openmap.MapName = new string[] { m_catalog.ActiveMapDoc.MapDocName };
                            getGeo.MapName = openmap;
                            m_catalog.ActiveMapDoc.DeleteFeature(getGeo, new UploadStringCompletedEventHandler(OnDeleteFeature));
                            
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 删除完毕回调
        /// </summary>
        private void OnDeleteFeature(object sender, UploadStringCompletedEventArgs e)
        {
            COperResult rlt = m_catalog.ActiveMapDoc.OnDeleteFeature(e);
            if (rlt.OperResult == true)
            {
                MessageBox.Show("删除成功", "提示", MessageBoxButton.OK);
                if (m_lastSelDataGrid != null && m_lastSelItem != null && m_lastSelDataGrid.ItemsSource is List<BindClass>)
                {
                    (m_lastSelDataGrid.ItemsSource as List<BindClass>).Remove(m_lastSelItem);
                    IEnumerable ie = m_lastSelDataGrid.ItemsSource;
                    m_lastSelDataGrid.ItemsSource = null;
                    m_lastSelDataGrid.ItemsSource =ie;
                }
                //m_catalog.ActiveMapDoc.Refresh();
                m_catalog.ActiveMapDoc.MapContainer.OperType = IMSOperType.Refresh;
            }
            else
                MessageBox.Show("删除失败，错误信息：" + rlt.ErrorInfo, "提示", MessageBoxButton.OK);
            m_lastSelDataGrid = null;
            m_lastSelItem = null;
        }
        /// <summary>
        /// 编辑回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuClick_Edit(object sender, RoutedEventArgs e)
        {
            if (m_catalog.ActiveMapDoc.ActiveLayerIndex < 0)
            {
                MessageBox.Show("请激活该编辑的图层", "提示", MessageBoxButton.OK);
                return;
            }
            TabItem item = tabControl1.SelectedItem as TabItem;
            if (item != null)
            {
                DataGrid datagrid = item.Content as DataGrid;
                if (datagrid != null)
                {
                    m_lastSelDataGrid = datagrid;
                    int tabIndex = Convert.ToInt32(((System.Windows.FrameworkElement)(((System.Windows.Controls.ContentControl)(tabControl1.SelectedItem)).Content)).Name.ToString().Split('_')[1]);
                    if (tabIndex >= 0 && tabIndex < _lastSelRlt.AttDS[0].attTables.Length)
                    {
                        m_lastSelItem = datagrid.SelectedItem as BindClass;
                        if (MapDocEditorObj == null)
                            MapDocEditorObj = new MapDocEditor() { ActiveMapDoc = m_catalog.ActiveMapDoc, GraphicsLayer = this.GraphicsLayer };
                        MapDocEditorObj.SetAttStruct(_lastSelRlt.AttDS[0].attTables[tabIndex].Columns, datagrid.SelectedItem as BindClass, this);
                    }
                }
            }
        }
        /// <summary>
        /// 统计图回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuClick_onChart(object sender, RoutedEventArgs e)
        {
            int tblIndex = Convert.ToInt32(((System.Windows.FrameworkElement)(((System.Windows.Controls.ContentControl)(tabControl1.SelectedItem)).Content)).Name.ToString().Split('_')[1]);
			CAttDataTable curRltTable=_lastSelRlt.AttDS[0].attTables[tblIndex];
            if (this.m_chartControl == null)
			{
                this.m_chartControl = new Chart();
			}
            this.m_chartControl.setDataProvider(curRltTable);
            this.m_chartControl.Show();
        }
        /// <summary>
        /// 设置上次查询参数对象
        /// </summary>
        /// <param name="param"></param>
        private void SetLastSelectParam(CMapSelectParam param)
        {
            this._lastSelectParam = param;
            //this.pageNum.text = "1";
            //this._lastPageCount = 0;
        }
        /// <summary>
        /// 页面切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataPager1_PageIndexChanged(object sender, EventArgs e)
        {
            if (_lastSelectParam != null)
            {
                this._lastSelectParam.PageCount = dataPager1.PageIndex;
                activeMapDoc.Select(_lastSelectParam, new UploadStringCompletedEventHandler(NextPageSelectCallback));
            }
        }
        /// <summary>
        /// 查询完毕回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPageSelectCallback(object sender, UploadStringCompletedEventArgs e)
        {
            CMapSelectAndGetAtt selRlt = m_catalog.ActiveMapDoc.OnSelect(e);
            TabItem item;
            string name;
            DataGrid grid;
            for (int i = 0; i < selRlt.Count[0].Length; i++)
            {
                if (selRlt.AttDS[0].attTables!=null&&selRlt.AttDS[0].attTables[i]!=null&&selRlt.AttDS[0].attTables[i].Rows != null)//selRlt.Count[0][i] > 0)
                {
                    name = activeMapDoc.GetLayerInfo(i).LayerDataName;
                    //if (this.FindName(name) == null)
                    //{
                    //    item = new TabItem()
                    //    {
                    //        Header = name,
                    //        Name = name
                    //    };
                    //    grid = new DataGrid()
                    //    {
                    //        Height = 221,
                    //        Width = 493,
                    //        AutoGenerateColumns = false,
                    //        IsReadOnly = true
                    //    };
                    //    item.Content = grid;
                    //    tabControl1.Items.Add(item);
                    //}
                    //else
                    {
                        item = this.FindName(name) as TabItem;
                        grid = item.Content as DataGrid;
                        grid.Columns.Clear();
                        grid.ItemsSource = null;
                    }
                    List<string> addColumnlHeadArr = new List<string>();
                    addColumnlHeadArr.Add("FID");
                    string[][] arr = new string[/*selRlt.Count[0][i]*/selRlt.AttDS[0].attTables[i].Rows.Length + 1][];
                    arr[0] = selRlt.AttDS[0].attTables[i].Columns.FldName;

                    List<string[]> addColumnlContentArr = new List<string[]>();
                    string[] fidArr = new string[selRlt.AttDS[0].attTables[i].Rows.Length];
                    for (int j = 0; j < selRlt.AttDS[0].attTables[i].Rows.Length; j++)
                    {
                        fidArr[j] = selRlt.AttDS[0].attTables[i].Rows[j].FID.ToString();
                        arr[j + 1] = selRlt.AttDS[0].attTables[i].Rows[j].Values;
                    }
                    addColumnlContentArr.Add(fidArr);
                    BindClass bingclass = new BindClass();
                    bingclass.ColumnDisplay(grid, arr, addColumnlHeadArr, addColumnlContentArr);
                }
            }
        }
        
        private DateTime m_timeLastLeftButtonUp = DateTime.Now;//鼠标左键弹起的时间,判断双击用     
        /// <summary>
        /// 鼠标在DataGrid弹起事件回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DateTime b = DateTime.Now;
            long c = CommFun.TimeDiff(b, m_timeLastLeftButtonUp);
            m_timeLastLeftButtonUp = b;
            if (c < 280)
            {//双击事件
                if (m_graphicsLayer != null && m_graphics != null)
                {
                    m_graphicsLayer.RemoveGraphics(m_graphics);
                    m_graphics = null;
                }
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItem is BindClass && grid.Tag is TmpInfo && m_graphicsLayer != null)
                {
                    CGetObjByID getGeo = new CGetObjByID();
                    getGeo.MapDocIndex = 0;
                    getGeo.LayerIndex = (grid.Tag as TmpInfo).LayerIndex;
                    getGeo.FeatureID = Convert.ToInt32((grid.SelectedItem as BindClass).key0);
                    COpenMap openmap = new COpenMap();
                    openmap.MapName = new string[1] { activeMapDoc.MapDocName };
                    getGeo.MapName = openmap;
                    activeMapDoc.GetGeomByID(getGeo, new UploadStringCompletedEventHandler(FlashFeature));
                }
            }
        }
        /// <summary>
        /// 闪烁图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlashFeature(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                m_featureGeo = activeMapDoc.OnGetGeomByID(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取该要素空间信息失败！" + ex.Message);
                return;
            }
            bool flg = false;
            if (m_featureGeo.LinGeom != null&&m_featureGeo.LinGeom.Length>0)
            {
                List<Point> pntArr = new List<Point>();
                for (int i = 0; i < m_featureGeo.LinGeom.Length; i++)
                {
                    for (int j = 0; j < m_featureGeo.LinGeom[i].Line.Arcs.Length; j++)
                        for (int k = 0; k < m_featureGeo.LinGeom[i].Line.Arcs[j].Dots.Length; k++)
                            pntArr.Add(new Point(m_featureGeo.LinGeom[i].Line.Arcs[j].Dots[k].x, m_featureGeo.LinGeom[i].Line.Arcs[j].Dots[k].y));
                }
                if (pntArr[0].X < activeMapDoc.MapContainer.WinViewBound.XMin || pntArr[0].X > activeMapDoc.MapContainer.WinViewBound.XMax ||
                    pntArr[0].Y < activeMapDoc.MapContainer.WinViewBound.YMin || pntArr[0].Y > activeMapDoc.MapContainer.WinViewBound.YMax)
                    activeMapDoc.MapContainer.PanTo(pntArr[0].X, pntArr[0].Y);
                m_graphics = new IMSPolyline(CoordinateType.Logic)
                {
                    Points = pntArr,
                    StrokeThickness = 4
                };
                flg = true;
            }
            if (m_featureGeo.PntGeom != null&&m_featureGeo.PntGeom.Length>0)
            {
                if (m_featureGeo.PntGeom[0].Dot.x < activeMapDoc.MapContainer.WinViewBound.XMin || m_featureGeo.PntGeom[0].Dot.x > activeMapDoc.MapContainer.WinViewBound.XMax ||
                    m_featureGeo.PntGeom[0].Dot.y < activeMapDoc.MapContainer.WinViewBound.YMin || m_featureGeo.PntGeom[0].Dot.y > activeMapDoc.MapContainer.WinViewBound.YMax)
                    activeMapDoc.MapContainer.PanTo(m_featureGeo.PntGeom[0].Dot.x, m_featureGeo.PntGeom[0].Dot.y);
                m_graphics = new IMSCircle(CoordinateType.Logic)
                {
                    CenX = m_featureGeo.PntGeom[0].Dot.x,
                    CenY = m_featureGeo.PntGeom[0].Dot.y,
                    RadiusEx = 6
                };
                flg = true;
            }
            if (m_featureGeo.RegGeom != null && m_featureGeo.RegGeom.Length > 0)
            {
                List<Point> pntArr = new List<Point>();
                for (int i = 0; i < m_featureGeo.RegGeom[0].Rings.Length; i++)
                    for (int j = 0; j < m_featureGeo.RegGeom[0].Rings[i].Arcs.Length; j++)
                        for (int k = 0; k < m_featureGeo.RegGeom[0].Rings[i].Arcs[j].Dots.Length; k++)
                            pntArr.Add(new Point(m_featureGeo.RegGeom[0].Rings[i].Arcs[j].Dots[k].x, m_featureGeo.RegGeom[0].Rings[i].Arcs[j].Dots[k].y));
                if (pntArr[0].X < activeMapDoc.MapContainer.WinViewBound.XMin || pntArr[0].X > activeMapDoc.MapContainer.WinViewBound.XMax ||
                    pntArr[0].Y < activeMapDoc.MapContainer.WinViewBound.YMin || pntArr[0].Y > activeMapDoc.MapContainer.WinViewBound.YMax)
                    activeMapDoc.MapContainer.PanTo(pntArr[0].X, pntArr[0].Y);
                m_graphics = new IMSPolygon(CoordinateType.Logic)
                {
                    Points = pntArr
                };
                flg = true;
            }
            if (flg)
            {
                m_graphicsLayer.AddGraphics(m_graphics);
                m_graphics.FlickerOverCallback = new GraphicsFlickerOverDelegate(FlickerOverCallback);
                m_graphics.Draw();
                m_graphics.Flicker();
            }
        }
        /// <summary>
        /// 闪烁完毕回调
        /// </summary>
        /// <param name="g"></param>
        private void FlickerOverCallback(GraphicsBase g)
        {
            if (m_graphicsLayer != null && m_graphics != null)
            {
                m_graphicsLayer.RemoveGraphics(m_graphics);
                m_graphics = null;
            }
        }
        /// <summary>
        /// 零时保存信息结构
        /// </summary>
        public class TmpInfo
        {
            public int LayerIndex { get; set; }
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        public void UpdateRecord(string[] AttValue)
        {
            if (m_lastSelDataGrid != null && m_lastSelItem != null && m_lastSelDataGrid.ItemsSource is List<BindClass>)
            {
                if (AttValue.Length == m_lastSelItem.ColumnCount - 1)
                {
                    for (int i = 0; i < AttValue.Length; i++)
                        m_lastSelItem.keyarr[i + 1] = AttValue[i];
                    m_lastSelItem.Refresh();
                }
                IEnumerable ie = m_lastSelDataGrid.ItemsSource;
                m_lastSelDataGrid.ItemsSource = null;
                m_lastSelDataGrid.ItemsSource = ie;
            }
            m_lastSelDataGrid = null;
            m_lastSelItem = null;
        }
    }     
}
