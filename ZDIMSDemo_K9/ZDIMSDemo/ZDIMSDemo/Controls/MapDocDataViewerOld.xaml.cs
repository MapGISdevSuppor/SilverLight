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
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMS.Map;
using System.Windows.Data;
using System.Collections;

namespace ZDIMSDemo.Controls
{
    public partial class MapDocDataViewerOld : ChildWindow
    {
        private IMSCatalog m_catalog = null;//目录
        private CMapSelectParam _lastSelectParam = null;
        //private int _lastPageCount = 0;
        private int pageSize = 0;
        private VectorMapDoc activeMapDoc = null;
        public MapDocDataViewerOld(IMSCatalog catalog)
        {
            m_catalog = catalog;
            InitializeComponent();
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
                ploy.Dots = new Dot_2D[logPntArr.Count+1];
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
        private void Select(Object geoObj, SelectionType selType, int page = 0, string condition = "")
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
            if (activeMapDoc != null && !activeMapDoc.Equals(m_catalog.ActiveMapDoc))
            {//不是同一个图层，清除原来的结果
                tabControl1.Items.Clear();                
            }
            pageSize = m_catalog.ActiveMapDoc.GetPageSize();
            activeMapDoc = m_catalog.ActiveMapDoc;
            CMapSelectAndGetAtt selRlt = m_catalog.ActiveMapDoc.OnSelect(e);
            TabItem item;
            string name;
            DataGrid grid;
            for(int i=0;i<selRlt.Count[0].Length;i++)
            {
                if(selRlt.Count[0][i]>0)
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
                            Height = 221,
                            Width = 493,
                            AutoGenerateColumns = false,
                            IsReadOnly = true
                        };
                        item.Content = grid;
                        tabControl1.Items.Add(item);
                    }
                    else
                    {
                        item = this.FindName(name) as TabItem;
                        grid = item.Content as DataGrid;
                    }
                    string[][] arr = new string[/*selRlt.Count[0][i]*/selRlt.AttDS[0].attTables[i].Rows.Length + 1][];
                    arr[0] = selRlt.AttDS[0].attTables[i].Columns.FldName;
                    for (int j = 0; j < selRlt.AttDS[0].attTables[i].Rows.Length; j++)
                    {
                        arr[j + 1] = selRlt.AttDS[0].attTables[i].Rows[j].Values;
                    }
                    dataPager1.PageSize = (int)Math.Ceiling(Convert.ToDouble(pageSize * pageSize) / selRlt.Count[0][i]);
                    BindClass bingclass = new BindClass();
                    dataPager1.DataContext = new PagedCollectionView(bingclass.ColumnDisplay(grid, arr));
                    item.IsSelected = true;
                    dataPager1.PageIndexChanged += new EventHandler<EventArgs>(dataPager1_PageIndexChanged);
                }
            }
            this.Show();
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
                if (selRlt.Count[0][i] > 0)
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
                            Height = 221,
                            Width = 493,
                            AutoGenerateColumns = false,
                            IsReadOnly = true
                        };
                        item.Content = grid;
                        tabControl1.Items.Add(item);
                    }
                    else
                    {
                        item = this.FindName(name) as TabItem;
                        grid = item.Content as DataGrid;
                    }
                    string[][] arr = new string[/*selRlt.Count[0][i]*/selRlt.AttDS[0].attTables[i].Rows.Length + 1][];
                    arr[0] = selRlt.AttDS[0].attTables[i].Columns.FldName;
                    for (int j = 0; j < selRlt.AttDS[0].attTables[i].Rows.Length; j++)
                    {
                        arr[j + 1] = selRlt.AttDS[0].attTables[i].Rows[j].Values;
                    }
                    dataPager1.PageSize = (int)Math.Ceiling(Convert.ToDouble(pageSize * pageSize) / selRlt.Count[0][i]);
                    BindClass bingclass = new BindClass();
                    dataPager1.DataContext = new PagedCollectionView(bingclass.ColumnDisplay(grid, arr));
                    item.IsSelected = true;
                    dataPager1.PageIndexChanged += new EventHandler<EventArgs>(dataPager1_PageIndexChanged);
                }
            }
        }
    }
}

