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
using ZDIMS.BaseLib;
using ZDIMS.Map;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using System.Windows.Data;
using ZDIMS.Util;
using System.Collections;

namespace ZDIMSDemo.Controls.Layer
{
    public partial class LayerDataViewer : BaseUserControl
    {
        DataGrid m_lastSelDataGrid;
        BindClass m_lastSelItem;
        CLayerSelectAndGetAtt _lastSelRlt;
        private IMSCatalog m_catalog = null;//目录
        private VectorLayer _vLayer = null;
        public CLayerSelectParam _lastSelectParam = null;
        //private int _lastPageCount = 0;
        private int pageSize = 0;
        private SFeatureGeometry m_featureGeo = null;
        public VectorLayer activeLayer = null;
        private GraphicsLayer m_graphicsLayer = null;
        private GraphicsBase m_graphics = null;
        private LayerEditor m_layerEditor = null;
        private BufferAnalyse m_bufferControl = null;//缓冲分析控件
        private Chart m_chartControl = null;//统计图控件

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
        public LayerDataViewer()
        {
            InitializeComponent();
            myDialog.OnClose += new RoutedEventHandler(OnClose);    
        }
        /// <summary>
        /// 工具条
        /// </summary>
        public LayerEditor LayerEditorObj 
        {
            get { return m_layerEditor; }
            set {
                    m_layerEditor = value;
            }
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
        /// 
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="layerType"></param>
        /// <param name="svrAddr"></param>
        /// <param name="gdbSvr"></param>
        /// <param name="gdbName"></param>
        /// <param name="usr"></param>
        /// <param name="psw"></param>
        public void ShowLayerAttRecord(string layerName,XClsType layerType,string svrAddr="127.0.0.1",string gdbSvr="MapGisLocal",string gdbName="IMSWebGISGDB",string usr="",string psw="")
        {
			CLayerAccessInfo layerAccessInfo=new CLayerAccessInfo();
            layerAccessInfo.GdbInfo = new CGdbInfo();
			layerAccessInfo.GdbInfo.GDBName=gdbName;
			layerAccessInfo.GdbInfo.GDBSvrName=gdbSvr;
			layerAccessInfo.GdbInfo.User=usr;
			layerAccessInfo.GdbInfo.Password=psw;
			CLayerInfo layerInfo=new CLayerInfo();
			layerInfo.LayerDataName=layerName;
			layerInfo.LayerType=layerType;
            layerInfo.GeomType = SFclsGeomType.Reg;
            layerAccessInfo.LayerInfoList = new CLayerInfo[1];
			layerAccessInfo.LayerInfoList[0]=layerInfo;
            COpenLayer openLayer = new COpenLayer();
            openLayer.LayerAccessInfo = new CLayerAccessInfo[1];
			openLayer.LayerAccessInfo[0]=layerAccessInfo;
            this._vLayer = new VectorLayer();
            this._vLayer.ServerAddress = svrAddr;
            this._vLayer.LayerObj = openLayer;
            m_catalog.MapContainer.AddChild(this._vLayer);
            // this._vLayer.AddedToMapContainerCallback += new AddedToMapContainerDelegate(AddToContainer);
            this._vLayer.LayerOpenSuccEvent += new LayerOpenSuccEventHandler(_vLayer_LayerOpenSuccEvent);


        }
        void _vLayer_LayerOpenSuccEvent(IVector layer)
        {
            m_catalog.Refresh();
            m_catalog.ActiveLayerObj = this._vLayer;
            this.activeLayer = this._vLayer;
            CSetAllEnumLayerStatus setAllStatus = new CSetAllEnumLayerStatus();
            setAllStatus.GdbIndex = 0;
            setAllStatus.LayerStatus = EnumLayerStatus.Selectable;
            this._vLayer.SetAllEnumLayerStatus(setAllStatus, new UploadStringCompletedEventHandler(OnSetAllEnumLayerStatusCallBack));
        }
        private void AddToContainer(IMap layer)
        {
            m_catalog.Refresh();
            m_catalog.ActiveLayerObj = this._vLayer;
            CSetAllEnumLayerStatus setAllStatus = new CSetAllEnumLayerStatus();
            setAllStatus.GdbIndex = 0;
            setAllStatus.LayerStatus = EnumLayerStatus.Selectable;
            this._vLayer.SetAllEnumLayerStatus(setAllStatus, new UploadStringCompletedEventHandler(OnSetAllEnumLayerStatusCallBack));
            
        }
        public void OnSetAllEnumLayerStatusCallBack(object sender, UploadStringCompletedEventArgs e)
        {
            COperResult operRlt = this._vLayer.OnSetAllEnumLayerStatus(e);
            if (operRlt.OperResult)
            {
                CLayerSelectParam layerSelectParam = new CLayerSelectParam();
                layerSelectParam.PageCount = 0;
                layerSelectParam.WebSelectParam = new CWebSelectParam();
                layerSelectParam.WebSelectParam.SelectionType = SelectionType.Condition;
                layerSelectParam.WebSelectParam.WhereClause = "";
                _lastSelectParam = layerSelectParam;
                this._vLayer.LayerSelectAndGetAtt(layerSelectParam, new UploadStringCompletedEventHandler(SelectCallback));
            }
            else
                MessageBox.Show(operRlt.ErrorInfo);
        }
        /// <summary>
        /// 图层查询
        /// </summary>
        /// <param name="geoObj">查询空间范围对象</param>
        /// <param name="selType">查询类型</param>
        /// <param name="page">结果记录页码</param>
        /// <param name="condition">查询条件</param>
        internal void Select(Object geoObj, SelectionType selType, int page = 0, string condition = "")
        {
            if (m_catalog.ActiveLayerObj == null)
            {
                MessageBox.Show("请选择一个活动的矢量图层!", "提示", MessageBoxButton.OK);
                return;
            }
            dataPager1.PageIndexChanged -= new EventHandler<EventArgs>(dataPager1_PageIndexChanged);
            CLayerSelectParam mapsel = new CLayerSelectParam();
            CWebSelectParam websel = new CWebSelectParam();
            websel.CompareRectOnly = m_catalog.ActiveLayerObj.CompareRectOnly;
            websel.Geometry = geoObj;
            if (geoObj != null)
            {
                websel.GeomType = (geoObj as IWebGeometry).GetGeomType();
            }
            websel.MustInside = m_catalog.ActiveLayerObj.MustInside;
            websel.NearDistance = m_catalog.ActiveLayerObj.NearDistanse;
            websel.SelectionType = selType;
            websel.WhereClause = condition;
            mapsel.WebSelectParam = websel;
            mapsel.PageCount = page;
            _lastSelectParam = mapsel;
            m_catalog.ActiveLayerObj.LayerSelectAndGetAtt(mapsel, new UploadStringCompletedEventHandler(SelectCallback));
        }
        /// <summary>
        /// 查询完毕回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectCallback(object sender, UploadStringCompletedEventArgs e)
        {
            //if (activeLayer != null && !activeLayer.Equals(m_catalog.ActiveLayerObj))
            //{//不是同一个图层，清除原来的结果
            //    tabControl1.Items.Clear();
            //}
            
            dataPager1.DataContext = null;
            tabControl1.Items.Clear();
            if (activeLayer != null)
            {
            }
                pageSize = activeLayer.GetPageSize();
                CLayerSelectAndGetAtt selRlt = this.activeLayer.OnLayerSelectAndGetAtt(e);
         
           
            _lastSelRlt = selRlt;
            TabItem item;
            string name;
            DataGrid grid = null;
            int maxCount = 0;
            if (selRlt.Count == null) return;
            for (int k = 0; k < selRlt.Count.Length; k++)//GDB
            {
                for (int i = 0; i < selRlt.Count[k].Length; i++)//层
                {
                    if (selRlt.Count[k][i] > 0 &&selRlt.AttDS[k].attTables!=null&& selRlt.AttDS[k].attTables[i].Rows != null)
                    {
                        name = activeLayer.LayerObj.LayerAccessInfo[k].LayerInfoList[i].LayerDataName;
                        if (this.FindName(name) == null)
                        {
                            item = new TabItem()
                            {
                                Header = name,
                                Name = name
                            };
                            grid = new DataGrid()
                            {
                                Name = "datagrid" + name + "_" + k + "_" + i,
                                Height = 221,
                                Width = 493,
                                AutoGenerateColumns = false,
                                IsReadOnly = true,
                                Tag = new TmpInfo() { LayerIndex = i, GDBIndex = k }
                            };
                            grid.MouseLeftButtonUp += new MouseButtonEventHandler(DataGrid_MouseLeftButtonUp);
                            grid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRow);
                            item.Content = grid;
                            if (ContextMenuService.GetContextMenu(grid) == null)
                            {
                                ContextMenu contextMenu = GetContexMenu(k,i);
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
                        string[][] arr = new string[/*selRlt.Count[0][i]*/selRlt.AttDS[k].attTables[i].Rows.Length + 1][];
                        arr[0] = selRlt.AttDS[k].attTables[i].Columns.FldName;

                        List<string[]> addColumnlContentArr = new List<string[]>();
                        string[] fidArr = new string[selRlt.AttDS[k].attTables[i].Rows.Length];
                        for (int j = 0; j < selRlt.AttDS[k].attTables[i].Rows.Length; j++)
                        {
                            fidArr[j] = selRlt.AttDS[k].attTables[i].Rows[j].FID.ToString();
                            arr[j + 1] = selRlt.AttDS[k].attTables[i].Rows[j].Values;
                        }
                        addColumnlContentArr.Add(fidArr);
                        if (maxCount < selRlt.Count[k][i])
                            maxCount = selRlt.Count[k][i];
                        BindClass bingclass = new BindClass();
                        dataPager1.DataContext = new PagedCollectionView(bingclass.ColumnDisplay(grid, arr, addColumnlHeadArr, addColumnlContentArr));
                        item.IsSelected = true;
                    }
                }
            }
            if (maxCount > 0)
            {
                dataPager1.PageSize = (int)Math.Ceiling(Convert.ToDouble(pageSize * pageSize) / maxCount);
                dataPager1.PageIndexChanged += new EventHandler<EventArgs>(dataPager1_PageIndexChanged);
            }
            if (tabControl1.Items.Count <= 0) //&& grid != null && grid.ItemsSource == null)
               // m_graphicsLayer.MapContainer.SetErrorText("没有符合条件的数据，请重新操作!");
            MessageBox.Show("没有查询到结果,请更换条件后重试!", "提示", MessageBoxButton.OK);
            else
                this.Show();
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
                activeLayer.LayerSelectAndGetAtt(_lastSelectParam, new UploadStringCompletedEventHandler(NextPageSelectCallback));
            }
        }
        /// <summary>
        /// 查询完毕回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPageSelectCallback(object sender, UploadStringCompletedEventArgs e)
        {
            CLayerSelectAndGetAtt selRlt = activeLayer.OnLayerSelectAndGetAtt(e);
            TabItem item;
            string name;
            DataGrid grid;
            for (int k = 0; k < selRlt.Count.Length; k++)
            {
                for (int i = 0; i < selRlt.Count[k].Length; i++)
                {
                    if (selRlt.Count[k][i] > 0 && selRlt.AttDS[k].attTables!=null&&selRlt.AttDS[k].attTables[i].Rows != null)
                    {
                        name = activeLayer.LayerObj.LayerAccessInfo[k].LayerInfoList[i].LayerDataName;
                        //if (this.FindName(name) == null)
                        //{
                        //    item = new TabItem()
                        //    {
                        //        Header = name,
                        //        Name = name
                        //    };
                        //    grid = new DataGrid()
                        //    {
                        //        Name = "datagrid" + name + "_" + k + "_" + i,
                        //        Height = 221,
                        //        Width = 493,
                        //        AutoGenerateColumns = false,
                        //        IsReadOnly = true,
                        //        Tag = new TmpInfo() { LayerIndex = i, GDBIndex = k }
                        //    };
                        //    grid.MouseLeftButtonUp += new MouseButtonEventHandler(DataGrid_MouseLeftButtonUp);
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
                        string[][] arr = new string[/*selRlt.Count[0][i]*/selRlt.AttDS[k].attTables[i].Rows.Length + 1][];
                        arr[0] = selRlt.AttDS[k].attTables[i].Columns.FldName;

                        List<string[]> addColumnlContentArr = new List<string[]>();
                        string[] fidArr = new string[selRlt.AttDS[k].attTables[i].Rows.Length];
                        for (int j = 0; j < selRlt.AttDS[k].attTables[i].Rows.Length; j++)
                        {
                            fidArr[j] = selRlt.AttDS[k].attTables[i].Rows[j].FID.ToString();
                            arr[j + 1] = selRlt.AttDS[k].attTables[i].Rows[j].Values;
                        }
                        addColumnlContentArr.Add(fidArr);
                        BindClass bingclass = new BindClass();
                        bingclass.ColumnDisplay(grid, arr, addColumnlHeadArr, addColumnlContentArr);
                    }
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
                    CLayerGetByID getGeo = new CLayerGetByID();
                    getGeo.GdbIndex = (grid.Tag as TmpInfo).GDBIndex;
                    getGeo.LayerIndex = (grid.Tag as TmpInfo).LayerIndex;
                    getGeo.FeatureID = Convert.ToInt32((grid.SelectedItem as BindClass).key0);
                    activeLayer.GetGeomByFID(getGeo, new UploadStringCompletedEventHandler(FlashFeature));
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
                m_featureGeo = activeLayer.OnGetGeomByFID(e);
            }
            catch(Exception ex)
            {
                MessageBox.Show("获取该要素空间信息失败！"+ex.Message);
                return;
            }
            bool flg = false;
            if (m_featureGeo.LinGeom != null && m_featureGeo.LinGeom.Length > 0)
            {
                List<Point> pntArr = new List<Point>();
                for (int i = 0; i < m_featureGeo.LinGeom.Length; i++)
                {
                    for (int j = 0; j < m_featureGeo.LinGeom[i].Line.Arcs.Length; j++)
                        for (int k = 0; k < m_featureGeo.LinGeom[i].Line.Arcs[j].Dots.Length; k++)
                            pntArr.Add(new Point(m_featureGeo.LinGeom[i].Line.Arcs[j].Dots[k].x, m_featureGeo.LinGeom[i].Line.Arcs[j].Dots[k].y));
                }
                if (pntArr[0].X < activeLayer.MapContainer.WinViewBound.XMin || pntArr[0].X > activeLayer.MapContainer.WinViewBound.XMax ||
                    pntArr[0].Y < activeLayer.MapContainer.WinViewBound.YMin || pntArr[0].Y > activeLayer.MapContainer.WinViewBound.YMax)
                    activeLayer.MapContainer.PanTo(pntArr[0].X, pntArr[0].Y);
                m_graphics = new IMSPolyline(CoordinateType.Logic)
                {
                    Points = pntArr,
                    StrokeThickness = 4
                };
                flg = true;
            }
            if (m_featureGeo.PntGeom != null && m_featureGeo.PntGeom.Length > 0)
            {
                if (m_featureGeo.PntGeom[0].Dot.x < activeLayer.MapContainer.WinViewBound.XMin || m_featureGeo.PntGeom[0].Dot.x > activeLayer.MapContainer.WinViewBound.XMax ||
                    m_featureGeo.PntGeom[0].Dot.y < activeLayer.MapContainer.WinViewBound.YMin || m_featureGeo.PntGeom[0].Dot.y > activeLayer.MapContainer.WinViewBound.YMax)
                    activeLayer.MapContainer.PanTo(m_featureGeo.PntGeom[0].Dot.x, m_featureGeo.PntGeom[0].Dot.y);
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
                if (pntArr[0].X < activeLayer.MapContainer.WinViewBound.XMin || pntArr[0].X > activeLayer.MapContainer.WinViewBound.XMax ||
                    pntArr[0].Y < activeLayer.MapContainer.WinViewBound.YMin || pntArr[0].Y > activeLayer.MapContainer.WinViewBound.YMax)
                    activeLayer.MapContainer.PanTo(pntArr[0].X, pntArr[0].Y);
                for (int n = 0; n < m_featureGeo.RegGeom.Length; n++)
                {
                    DrawPolygons(m_featureGeo.RegGeom[n]);
                }
                
            }
            if (flg)
            {
                m_graphicsLayer.AddGraphics(m_graphics);
                m_graphics.FlickerOverCallback = new GraphicsFlickerOverDelegate(FlickerOverCallback);
                m_graphics.Draw();
                m_graphics.Flicker();
            }
        }

        private  void DrawPolygons(GRegion regions)
        {
            GRegion _tmp = regions;
            List<Point> logPnts = new List<Point>();
            this.GraphicsLayer.MapContainer.Refresh();
            for (int i = 0; i < _tmp.Rings.Length; i++)
            {
                logPnts = GetLogPnts(_tmp.Rings[i]);
                IMSPolygon polygon = new IMSPolygon(ZDIMS.Interface.CoordinateType.Logic);
                this.GraphicsLayer.AddGraphics(polygon);
                this.GraphicsLayer.MapContainer.Refresh();
                polygon.StrokeThickness = 1;
              // polygon.Shape.Fill = new SolidColorBrush(Colors.Red);// new SolidColorBrush(t.ToColor("#ee0000"));
                polygon.Points = logPnts;
                polygon.Flicker();

                polygon.FlickerOverCallback += new GraphicsFlickerOverDelegate(fickerover);

            }
        }

        private void fickerover(GraphicsBase g)
    {
        this.GraphicsLayer.RemoveAll();
    }
        private static List<Point> GetLogPnts(AnyLine lines)
        {
            AnyLine _tmp = lines;
            List<Point> logPnts = new List<Point>();
            Point _p;
            Dot_2D _dot;
            for (int i = 0; i < _tmp.Arcs.Length; i++)
            {
                for (int j = 0; j < _tmp.Arcs[i].Dots.Length; j++)
                {
                    _dot = new Dot_2D();
                    _dot = _tmp.Arcs[i].Dots[j];
                    _p = new Point(_dot.x, _dot.y);
                    logPnts.Add(_p);
                }
            }
            return logPnts;
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
        /// 设置右键
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        private ContextMenu GetContexMenu(int gdbIndex,int layerIndex)
        {
            ContextMenu menu = new ContextMenu();
         //   MenuItem delItem = new MenuItem();
       //     delItem.Header = "删除";
        //    delItem.Click += new RoutedEventHandler(ContextMenuClick_Del);
         //   menu.Items.Add(delItem);
        //    MenuItem editItem = new MenuItem();
        //    editItem.Header = "编辑";
        //    editItem.Click += new RoutedEventHandler(ContextMenuClick_Edit);
        //    menu.Items.Add(editItem);
            if (activeLayer.ActiveGdbIndex == gdbIndex && activeLayer.ActiveLayerIndex == layerIndex)
            {
         //       MenuItem bufferItem = new MenuItem();
          //      bufferItem.Header = "缓冲分析";
          //      bufferItem.Click += new RoutedEventHandler(ContextMenuClick_bufferFeature);
          //      menu.Items.Add(bufferItem);
                //var delItem:ContextMenuItem=new ContextMenuItem("删除");
                //delItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, deleteFeature);
                //var editItem:ContextMenuItem=new ContextMenuItem("编辑");
                //editItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, editFeature);
                //var bufferItem:ContextMenuItem=new ContextMenuItem("缓冲分析");
                //bufferItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, bufferFeature);
                //menu.customItems.push(editItem, delItem, bufferItem);
            }
         //   MenuItem charItem = new MenuItem();
         //   charItem.Header = "统计图";
         //   charItem.Click += new RoutedEventHandler(ContextMenuClick_onChart);
          //  menu.Items.Add(charItem);
            return menu;
        }
        /// <summary>
        /// 统计图回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuClick_onChart(object sender, RoutedEventArgs e)
        {
            int tblIndex = Convert.ToInt32(((System.Windows.FrameworkElement)(((System.Windows.Controls.ContentControl)(tabControl1.SelectedItem)).Content)).Name.ToString().Split('_')[1]);
            CAttDataTable curRltTable = _lastSelRlt.AttDS[0].attTables[tblIndex];
            if (this.m_chartControl == null)
            {
                this.m_chartControl = new Chart();
            }
            this.m_chartControl.setDataProvider(curRltTable);
            this.m_chartControl.Show();
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
            CGetObjByID getobj = new CGetObjByID();
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
            if (m_catalog.ActiveLayerObj.ActiveLayerIndex < 0 || m_catalog.ActiveLayerObj.ActiveGdbIndex<0)
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
                            CLayerGetByID getGeo = new CLayerGetByID();
                            getGeo.FeatureID = Convert.ToInt64(bc.key0);
                            if (m_lastSelDataGrid.Tag is TmpInfo)
                            {
                                getGeo.GdbIndex = (m_lastSelDataGrid.Tag as TmpInfo).GDBIndex;
                                getGeo.LayerIndex = (m_lastSelDataGrid.Tag as TmpInfo).LayerIndex;
                                COpenMap openmap = new COpenMap();
                                m_catalog.ActiveLayerObj.DeleteFeature(getGeo, new UploadStringCompletedEventHandler(OnDeleteFeature));
                            }
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
            COperResult rlt = m_catalog.ActiveLayerObj.OnDeleteFeature(e);
            if (rlt.OperResult == true)
            {
                MessageBox.Show("删除成功", "提示", MessageBoxButton.OK);
                if (m_lastSelDataGrid != null && m_lastSelItem != null && m_lastSelDataGrid.ItemsSource is List<BindClass>)
                {
                    (m_lastSelDataGrid.ItemsSource as List<BindClass>).Remove(m_lastSelItem);
                    IEnumerable ie = m_lastSelDataGrid.ItemsSource;
                    m_lastSelDataGrid.ItemsSource = null;
                    m_lastSelDataGrid.ItemsSource = ie;
                }
                //m_catalog.ActiveMapDoc.Refresh();
                m_catalog.ActiveLayerObj.MapContainer.OperType = IMSOperType.Refresh;
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
            if (m_catalog.ActiveLayerObj.ActiveLayerIndex < 0 || m_catalog.ActiveLayerObj.ActiveGdbIndex<0)
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
                    m_lastSelItem = datagrid.SelectedItem as BindClass;
                    if (LayerEditorObj == null)
                        LayerEditorObj = new LayerEditor() { ActiveLayerObj = m_catalog.ActiveLayerObj, GraphicsLayer = this.GraphicsLayer };
                    if (datagrid.Tag is TmpInfo)
                    {
                        int gdbIndex = (datagrid.Tag as TmpInfo).GDBIndex;
                        int layerIndex = (datagrid.Tag as TmpInfo).LayerIndex;
                        LayerEditorObj.ActiveLayerObj = m_catalog.ActiveLayerObj;
                        LayerEditorObj.GraphicsLayer = GraphicsLayer;
                        LayerEditorObj.SetAttStruct(_lastSelRlt.AttDS[gdbIndex].attTables[layerIndex].Columns, datagrid.SelectedItem as BindClass, this);
                    }
                }
            }
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
        /// <summary>
        /// 零时保存信息结构
        /// </summary>
        public class TmpInfo
        {
            public int GDBIndex { get; set; }
            public int LayerIndex { get; set; }
        }

        public void removeLayer()
        {
            if (this._vLayer != null)
            {
                this.IMSCatalog.MapContainer.RemoveChild(this._vLayer);
            }

        }
    }

}
