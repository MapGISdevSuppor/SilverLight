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
using ZDIMSDemo.Controls.Comm;
using ZDIMS.Map;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMS.Util;

namespace ZDIMSDemo.Controls.Layer
{
    public partial class LayerEditor : BaseUserControl
    {
        Object m_targetGeo;
        IMSCatalog m_catalog = null;
        GraphicsBase m_graphics = null;
        IFeatureStyleEditor m_style;
        CAttStruct m_attStruct = null;
        WebGraphicsInfo m_featureStyle;
        int m_featureID;
        private VectorLayer m_activeLayerObj = null;
        public VectorLayer ActiveLayerObj
        {
            set { m_activeLayerObj = value; }
            get
            {
                if (m_activeLayerObj == null && m_catalog != null)
                {
                    m_activeLayerObj = m_catalog.ActiveLayerObj;
                }
                return m_activeLayerObj;
            }
        }
        public IMSCatalog IMSCatalog { set { m_catalog = value; } }
        public GraphicsLayer GraphicsLayer { get; set; }
        public LayerEditor(IMSCatalog catalog = null)
        {
            m_catalog = catalog;
            InitializeComponent();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
        }
        public override void Show()
        {
            tabControl1.SelectedIndex = 0;
            base.Show();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            if (GraphicsLayer != null && m_graphics != null)
            {
                GraphicsLayer.RemoveGraphics(m_graphics);
                m_graphics = null;
            }
            this.Close();
        }
        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="gLayer">GraphicsLayer图层对象</param>
        /// <param name="graphics">绘图对象</param>
        /// <param name="logPntArr">绘图保存的逻辑坐标数组</param>
        public void AddDot(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (ActiveLayerObj != null && logPntArr.Count > 0)
            {
                if (ActiveLayerObj.ActiveGdbIndex < 0 || ActiveLayerObj.ActiveLayerIndex < 0)
                {
                    MessageBox.Show("请激活矢量图层的一个点图层", "提示", MessageBoxButton.OK);
                    return;
                }
                Dot_2D dot = new Dot_2D();
                dot.x = logPntArr[0].X;
                dot.y = logPntArr[0].Y;
                m_targetGeo = dot;
                CSetLayerIndex setIdx = new CSetLayerIndex();
                setIdx.GdbIndex = ActiveLayerObj.ActiveGdbIndex;
                setIdx.LayerIndex = ActiveLayerObj.ActiveLayerIndex;
                ActiveLayerObj.GetGeomType(setIdx, new UploadStringCompletedEventHandler(CheckPointType));
            }
        }
        /// <summary>
        /// 添加线
        /// </summary>
        /// <param name="gLayer">GraphicsLayer图层对象</param>
        /// <param name="graphics">绘图对象</param>
        /// <param name="logPntArr">绘图保存的逻辑坐标数组</param>
        public void AddLine(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (ActiveLayerObj != null && logPntArr.Count > 0)
            {
                if (ActiveLayerObj.ActiveGdbIndex < 0 || ActiveLayerObj.ActiveLayerIndex < 0)
                {
                    MessageBox.Show("请激活矢量图层的一个线图层", "提示", MessageBoxButton.OK);
                    return;
                }
                AnyLine line = new AnyLine();
                line.Arcs = new Arc[1];
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
                line.Arcs[0] = arc;
                m_targetGeo = line;
                CSetLayerIndex setIdx = new CSetLayerIndex();
                setIdx.GdbIndex = ActiveLayerObj.ActiveGdbIndex;
                setIdx.LayerIndex = ActiveLayerObj.ActiveLayerIndex;
                ActiveLayerObj.GetGeomType(setIdx, new UploadStringCompletedEventHandler(CheckLineType));
            }
        }
        /// <summary>
        /// 添加区
        /// </summary>
        /// <param name="gLayer">GraphicsLayer图层对象</param>
        /// <param name="graphics">绘图对象</param>
        /// <param name="logPntArr">绘图保存的逻辑坐标数组</param>
        public void AddArea(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (ActiveLayerObj != null && logPntArr.Count > 0)
            {
                if (ActiveLayerObj.ActiveGdbIndex < 0 || ActiveLayerObj.ActiveLayerIndex < 0)
                {
                    MessageBox.Show("请激活矢量图层的一个区图层", "提示", MessageBoxButton.OK);
                    return;
                }
                ZDIMS.BaseLib.Polygon polygon = new ZDIMS.BaseLib.Polygon();
                polygon.Dots = new Dot_2D[logPntArr.Count];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    polygon.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                m_targetGeo = polygon;
                CSetLayerIndex setIdx = new CSetLayerIndex();
                setIdx.GdbIndex = ActiveLayerObj.ActiveGdbIndex;
                setIdx.LayerIndex = ActiveLayerObj.ActiveLayerIndex;
                ActiveLayerObj.GetGeomType(setIdx, new UploadStringCompletedEventHandler(CheckPolyType));
            }
        }
        private void CheckPointType(object sender, UploadStringCompletedEventArgs e)
        {
            SFclsGeomType data = ActiveLayerObj.OnGetGeomType(e);
            if (data != SFclsGeomType.Pnt)
            {
                MessageBox.Show("当前图层不能添加该几何类型的要素", "提示", MessageBoxButton.OK);
                return;
            }
            AddFeature(SFclsGeomType.Pnt);
        }
        private void CheckLineType(object sender, UploadStringCompletedEventArgs e)
        {
            SFclsGeomType data = ActiveLayerObj.OnGetGeomType(e);
            if (data != SFclsGeomType.Lin)
            {
                MessageBox.Show("当前图层不能添加该几何类型的要素", "提示", MessageBoxButton.OK);
                return;
            }
            AddFeature(SFclsGeomType.Lin);
        }
        private void CheckPolyType(object sender, UploadStringCompletedEventArgs e)
        {
            SFclsGeomType data = ActiveLayerObj.OnGetGeomType(e);
            if (data != SFclsGeomType.Reg)
            {
                MessageBox.Show("当前图层不能添加该几何类型的要素", "提示", MessageBoxButton.OK);
                return;
            }
            AddFeature(SFclsGeomType.Reg);
        }

        private void AddFeature(SFclsGeomType geoType)
        {
            if (geoType==SFclsGeomType.Unknown)
            {
                MessageBox.Show("当前激活图层几何类型未知", "提示", MessageBoxButton.OK);
                return;
            }
            CSetLayerIndex obj = new CSetLayerIndex()
            {
                GdbIndex = ActiveLayerObj.ActiveGdbIndex,
                LayerIndex = ActiveLayerObj.ActiveLayerIndex
            };
            ActiveLayerObj.GetLayerAttStruct(obj, new UploadStringCompletedEventHandler(GetAttStructCallBack));
        }
        private void GetAttStructCallBack(object sender, UploadStringCompletedEventArgs e)
        {
            CAttStruct attStru = ActiveLayerObj.OnGetLayerAttStruct(e);
            SetAttStruct(attStru);
        }
        LayerDataViewer m_layerDataViewer;
        public void SetAttStruct(CAttStruct attStruct, BindClass values = null, LayerDataViewer layerDataViewer = null)
        {
            m_layerDataViewer = layerDataViewer;
            m_attStruct = attStruct;
            m_featureStyle = null;
            Clear();
            Label label;
            TextBox txtbox;
            if (values != null && values.ColumnCount > 0)
                m_featureID = Convert.ToInt32(values.keyarr[0]);
            for (int i = 0; i < m_attStruct.FldNumber; i++)
            {
                grid1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                label = new Label() { Content = m_attStruct.FldName[i] + ":", Width = 60 };
                grid1.Children.Add(label);
                Grid.SetRow(label, i);
                txtbox = new TextBox() { Width = 120, Height = 23, Text = "0" };//, Name = "Fld_" + m_attStruct.FldName[i] };
                grid1.Children.Add(txtbox);
                m_textBoxArr.Add(txtbox);
                if (values != null && values.ColumnCount > i + 1)
                    txtbox.Text = values.keyarr[i + 1];
                Grid.SetRow(txtbox, i);
                Grid.SetColumn(txtbox, 1);
            }
            if (values == null)
            {
                switch ((m_targetGeo as IWebGeometry).GetGeomType())
                {
                    case WebGeomType.Point:
                        m_style = new PointStyle();
                        break;
                    case WebGeomType.Line:
                        m_style = new LineStyle();
                        break;
                    case WebGeomType.Polygon:
                        m_style = new PolygonStyle();
                        break;
                }
                grid2.Children.Add(m_style as UIElement);
            }
            else
            {
                CLayerGetByID feature = new CLayerGetByID();
                feature.FeatureID = this.m_featureID;
                feature.GdbIndex = ActiveLayerObj.ActiveGdbIndex;
                feature.LayerIndex = ActiveLayerObj.ActiveLayerIndex;
                ActiveLayerObj.GetGraphicInfo(feature, OnGetStyle);
            }
            Button btn = new Button() { Width = 70 };
            btn.Content = "提交";
            if (values == null)
                btn.Click += new RoutedEventHandler(SubmitForAdd);
            else
                btn.Click += new RoutedEventHandler(SubmitForEdit);
            grid3.Children.Add(btn);
            btn = new Button() { Width = 70 };
            btn.Content = "关闭";
            btn.Click += new RoutedEventHandler(Close);
            grid3.Children.Add(btn);
            Grid.SetColumn(btn, 1);
            if (values != null)
            {
                grid3.ColumnDefinitions.Add(new ColumnDefinition()); //{ Width = new GridLength(120) });
                btn = new Button() { Width = 90 };
                btn.Content = "调整要素位置";
                btn.Click += new RoutedEventHandler(EditPoint);
                grid3.Children.Add(btn);
                Grid.SetColumn(btn, 2);
            }
            this.Show();
        }
        private void SubmitForAdd(object sender, RoutedEventArgs e)
        {
            CSetLayerIndex setLayerIdx = new CSetLayerIndex() { GdbIndex = ActiveLayerObj.ActiveGdbIndex, LayerIndex = ActiveLayerObj.ActiveLayerIndex };
            ActiveLayerObj.GetGeomType(setLayerIdx, new UploadStringCompletedEventHandler(GetTypeForAdd));
        }
        private void GetTypeForAdd(object sender, UploadStringCompletedEventArgs e)
        {
            SFclsGeomType geoType = ActiveLayerObj.OnGetGeomType(e);
            CAppendFeature fIno = GetMapFeatureInfo(geoType);
            ActiveLayerObj.AppendFeature(fIno, new UploadStringCompletedEventHandler(GetAddOperResult));
        }
        private void GetAddOperResult(object sender, UploadStringCompletedEventArgs e)
        {
            COperResult rlt = ActiveLayerObj.OnAppendFeature(e);
            if (rlt.OperResult == true)
            {
                MessageBox.Show("添加成功", "提示", MessageBoxButton.OK);
                this.Close();
                ActiveLayerObj.MapContainer.OperType = IMSOperType.Refresh;
                //this.m_activeMapDoc.Refresh();
            }
            else
                MessageBox.Show("添加失败，错误信息：" + rlt.ErrorInfo, "提示", MessageBoxButton.OK);
        }
        private void SubmitForEdit(object sender, RoutedEventArgs e)
        {
            CSetLayerIndex setLayerIdx = new CSetLayerIndex() { GdbIndex = ActiveLayerObj.ActiveGdbIndex, LayerIndex = ActiveLayerObj.ActiveLayerIndex };
            ActiveLayerObj.GetGeomType(setLayerIdx, new UploadStringCompletedEventHandler(GetTypeForEdit));
        }
        private void GetTypeForEdit(object sender, UploadStringCompletedEventArgs e)
        {
            SFclsGeomType geoType = ActiveLayerObj.OnGetGeomType(e);
            if (GraphicsLayer != null && m_graphics != null)
            {
                SFeatureGeometry geoObj = m_targetGeo as SFeatureGeometry;
                int n = 0;
                if (geoObj != null)
                {
                    if (geoObj.LinGeom != null && geoObj.LinGeom.Length > 0)
                    {
                        List<Point> pntArr = new List<Point>();
                        for (int i = 0; i < geoObj.LinGeom.Length; i++)
                        {
                            for (int j = 0; j < geoObj.LinGeom[i].Line.Arcs.Length; j++)
                                for (int k = 0; k < geoObj.LinGeom[i].Line.Arcs[j].Dots.Length; k++)
                                {
                                    geoObj.LinGeom[i].Line.Arcs[j].Dots[k].x = m_graphics.Points[n].X;
                                    geoObj.LinGeom[i].Line.Arcs[j].Dots[k].y = m_graphics.Points[n++].Y;
                                }
                        }
                    }
                    if (geoObj.PntGeom != null && geoObj.PntGeom.Length > 0)
                    {
                        geoObj.PntGeom[0].Dot.x = m_graphics.Points[0].X;
                        geoObj.PntGeom[0].Dot.y = m_graphics.Points[0].Y;
                    }
                    if (geoObj.RegGeom != null && geoObj.RegGeom.Length > 0)
                    {
                        List<Point> pntArr = new List<Point>();
                        for (int i = 0; i < geoObj.RegGeom[0].Rings.Length; i++)
                            for (int j = 0; j < geoObj.RegGeom[0].Rings[i].Arcs.Length; j++)
                                for (int k = 0; k < geoObj.RegGeom[0].Rings[i].Arcs[j].Dots.Length; k++)
                                {
                                    geoObj.RegGeom[0].Rings[i].Arcs[j].Dots[k].x = m_graphics.Points[n].X;
                                    geoObj.RegGeom[0].Rings[i].Arcs[j].Dots[k].y = m_graphics.Points[n++].Y;
                                }

                    }
                }
                GraphicsLayer.RemoveGraphics(m_graphics);
                m_graphics = null;
            }
            CAppendFeature fIno = GetMapFeatureInfo(geoType);
            if (fIno == null)
                return;
            ActiveLayerObj.UpdateFeature(fIno, new UploadStringCompletedEventHandler((s, evt) =>
            {
                if (m_layerDataViewer != null)
                    m_layerDataViewer.UpdateRecord(fIno.FSet.AttValue);
                GetEditOperResult(s, evt);
            }));
        }
        private void GetEditOperResult(object sender, UploadStringCompletedEventArgs e)
        {
            if (m_layerDataViewer != null)
            {
                //COperResult rlt = ActiveMapDoc.OnAddFeature(e);
                if (ActiveLayerObj.OnUpdateFeature(e).OperResult == true)
                {
                    MessageBox.Show("编辑成功", "提示", MessageBoxButton.OK);
                    this.Close();
                    ActiveLayerObj.MapContainer.OperType = IMSOperType.Refresh;
                    //this.m_activeMapDoc.Refresh();
                }
                else
                    MessageBox.Show("编辑失败，错误信息：" + ActiveLayerObj.OnUpdateFeature(e).ErrorInfo, "提示", MessageBoxButton.OK);
            }
        }
        private void EditPoint(object sender, RoutedEventArgs e)
        {
            if (GraphicsLayer != null)
            {
                if (m_graphics != null)
                {
                    GraphicsLayer.RemoveGraphics(m_graphics);
                    m_graphics = null;
                }
                CLayerGetByID getGeo = new CLayerGetByID();
                getGeo.GdbIndex = ActiveLayerObj.ActiveGdbIndex;
                getGeo.LayerIndex = ActiveLayerObj.ActiveLayerIndex;
                getGeo.FeatureID = m_featureID;
                ActiveLayerObj.GetGeomByFID(getGeo, new UploadStringCompletedEventHandler(DrawFeature));
            }
        }
        private void DrawFeature(object sender, UploadStringCompletedEventArgs e)
        {
            SFeatureGeometry geoObj = ActiveLayerObj.OnGetGeomByFID(e);
            m_targetGeo = geoObj;
            bool flg = false;
            //RectBound rect = new RectBound(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);
            if (geoObj.LinGeom != null && geoObj.LinGeom.Length > 0)
            {
                List<Point> pntArr = new List<Point>();
                for (int i = 0; i < geoObj.LinGeom.Length; i++)
                {
                    for (int j = 0; j < geoObj.LinGeom[i].Line.Arcs.Length; j++)
                        for (int k = 0; k < geoObj.LinGeom[i].Line.Arcs[j].Dots.Length; k++)
                            pntArr.Add(new Point(geoObj.LinGeom[i].Line.Arcs[j].Dots[k].x, geoObj.LinGeom[i].Line.Arcs[j].Dots[k].y));
                }
                if (pntArr[0].X < ActiveLayerObj.MapContainer.WinViewBound.XMin || pntArr[0].X > ActiveLayerObj.MapContainer.WinViewBound.XMax ||
                    pntArr[0].Y < ActiveLayerObj.MapContainer.WinViewBound.YMin || pntArr[0].Y > ActiveLayerObj.MapContainer.WinViewBound.YMax)
                    ActiveLayerObj.MapContainer.PanTo(pntArr[0].X, pntArr[0].Y);
                //(ActiveLayerObj.MapContainer as IMSMap).JumpByRectBound(
                m_graphics = new IMSPolyline(CoordinateType.Logic)
                {
                    Points = pntArr,
                    StrokeThickness = 4
                };
                flg = true;
            }
            if (geoObj.PntGeom != null && geoObj.PntGeom.Length > 0)
            {
                if (geoObj.PntGeom[0].Dot.x < ActiveLayerObj.MapContainer.WinViewBound.XMin || geoObj.PntGeom[0].Dot.x > ActiveLayerObj.MapContainer.WinViewBound.XMax ||
                    geoObj.PntGeom[0].Dot.y < ActiveLayerObj.MapContainer.WinViewBound.YMin || geoObj.PntGeom[0].Dot.y > ActiveLayerObj.MapContainer.WinViewBound.YMax)
                    ActiveLayerObj.MapContainer.PanTo(geoObj.PntGeom[0].Dot.x, geoObj.PntGeom[0].Dot.y);
                m_graphics = new IMSCircle(CoordinateType.Logic)
                {
                    CenX = geoObj.PntGeom[0].Dot.x,
                    CenY = geoObj.PntGeom[0].Dot.y,
                    RadiusEx = 6
                };
                flg = true;
            }
            if (geoObj.RegGeom != null && geoObj.RegGeom.Length > 0)
            {
                List<Point> pntArr = new List<Point>();
                for (int i = 0; i < geoObj.RegGeom[0].Rings.Length; i++)
                    for (int j = 0; j < geoObj.RegGeom[0].Rings[i].Arcs.Length; j++)
                        for (int k = 0; k < geoObj.RegGeom[0].Rings[i].Arcs[j].Dots.Length; k++)
                            pntArr.Add(new Point(geoObj.RegGeom[0].Rings[i].Arcs[j].Dots[k].x, geoObj.RegGeom[0].Rings[i].Arcs[j].Dots[k].y));
                if (pntArr[0].X < ActiveLayerObj.MapContainer.WinViewBound.XMin || pntArr[0].X > ActiveLayerObj.MapContainer.WinViewBound.XMax ||
                    pntArr[0].Y < ActiveLayerObj.MapContainer.WinViewBound.YMin || pntArr[0].Y > ActiveLayerObj.MapContainer.WinViewBound.YMax)
                    ActiveLayerObj.MapContainer.PanTo(pntArr[0].X, pntArr[0].Y);
                m_graphics = new IMSPolygon(CoordinateType.Logic)
                {
                    Points = pntArr
                };
                flg = true;
            }
            if (flg)
            {
                GraphicsLayer.AddGraphics(m_graphics);
                m_graphics.EnableEdit = true;
                m_graphics.EnableEditMark2 = false;
                m_graphics.Draw();
            }
        }
        private void OnGetStyle(object sender, UploadStringCompletedEventArgs e)
        {
            WebGraphicsInfo gInfo = ActiveLayerObj.OnGetGraphicInfo(e);
            m_featureStyle = gInfo;
            switch (gInfo.InfoType)
            {
                case GInfoType.PntInfo:
                    m_style = new PointStyle(gInfo);
                    break;
                case GInfoType.LinInfo:
                    m_style = new LineStyle(gInfo);
                    break;
                case GInfoType.RegInfo:
                    m_style = new PolygonStyle(gInfo);
                    break;
            }
            grid2.Children.Add(m_style as UIElement);
        }
        List<TextBox> m_textBoxArr = new List<TextBox>();
        private void Clear()
        {
            if (GraphicsLayer != null && m_graphics != null)
            {
                GraphicsLayer.RemoveGraphics(m_graphics);
                m_graphics = null;
            }
            this.grid1.RowDefinitions.Clear();
            this.grid1.Children.Clear();
            this.grid2.Children.Clear();
            this.grid3.ColumnDefinitions.Clear();
            this.grid3.Children.Clear();
            this.grid3.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(75) });
            this.grid3.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(75) });
            m_textBoxArr.Clear();
        }
        /// <summary>
        /// 获取要素信息
        /// </summary>
        /// <returns></returns>
        private CAppendFeature GetMapFeatureInfo(SFclsGeomType geoType)
        {
            CAppendFeature fInfo = new CAppendFeature();
            SFeature sf = new SFeature();
            sf.AttValue = new string[m_attStruct.FldNumber];
            for (int i = 0; i < m_attStruct.FldNumber; i++)
            {
                sf.AttValue[i] = m_textBoxArr[i].Text;
                switch (this.m_attStruct.FldType[i])
                {
                    case "double":
                    case "integer":
                    case "long":
                    case "short":
                        if (!CommFun.IsNumber(sf.AttValue[i]))
                        {
                            MessageBox.Show("字段【Fld_" + this.m_attStruct.FldName[i] + "】输入的数据格式不正确。请重新输入！", "提示", MessageBoxButton.OK);
                            return null;
                        }
                        break;
                }
            }
            SFeatureGeometry sfGeo = null;
            SFclsGeomType curFGeoType;
            if (m_targetGeo != null)
            {
                sfGeo = m_targetGeo as SFeatureGeometry;
                if (sfGeo == null) //add feature
                {
                    sfGeo = new SFeatureGeometry();
                    switch ((m_targetGeo as IWebGeometry).GetGeomType())
                    {
                        case WebGeomType.Point:
                            sf.ftype = SFclsGeomType.Pnt;
                            GPoint pnt = new GPoint();
                            pnt.Dot = m_targetGeo as Dot_2D;
                            sfGeo.PntGeom = new GPoint[] { pnt };
                            break;
                        case WebGeomType.Line:
                            sf.ftype = SFclsGeomType.Lin;
                            GLine line = new GLine();
                            line.Line = m_targetGeo as AnyLine;
                            sfGeo.LinGeom = new GLine[] { line };
                            break;
                        case WebGeomType.Polygon:
                            sf.ftype = SFclsGeomType.Reg;
                            GRegion polygon = new GRegion();
                            AnyLine circle = new AnyLine();
                            circle.Arcs = new Arc[1];
                            circle.Arcs[0] = new Arc();
                            circle.Arcs[0].Dots = (m_targetGeo as ZDIMS.BaseLib.Polygon).Dots;
                            polygon.Rings = new AnyLine[] { circle };
                            sfGeo.RegGeom = new GRegion[] { polygon };
                            break;
                        default:
                            sfGeo = null;
                            break;
                    }
                }
            }
            curFGeoType = geoType;
            if (this.m_featureStyle == null)
                this.m_featureStyle = new WebGraphicsInfo();
            this.m_style.Update();
            switch (curFGeoType)
            {
                case SFclsGeomType.Pnt:
                    this.m_featureStyle.InfoType = GInfoType.PntInfo;
                    PointStyle newPntStyle = this.m_style as PointStyle;
                    this.m_featureStyle.PntInfo = new CPointInfo();
                    if (newPntStyle.patternAngle.Text == "")
                        this.m_featureStyle.PntInfo.Angle = 0.0;
                    else
                    this.m_featureStyle.PntInfo.Angle = Convert.ToDouble(newPntStyle.patternAngle.Text);
                    if (newPntStyle.patternColor._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.PntInfo.Color = 0;
                    else
                    this.m_featureStyle.PntInfo.Color = Convert.ToInt32(newPntStyle.patternColor._TextBoxInput.Text.Split(':')[0]);
                    if (newPntStyle.patternHeight.Text == "")
                        this.m_featureStyle.PntInfo.SymHeight = 0.0;
                    else
                    this.m_featureStyle.PntInfo.SymHeight = Convert.ToDouble(newPntStyle.patternHeight.Text);
                    if (newPntStyle.patternID._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.PntInfo.SymID = 0;
                    else
                    this.m_featureStyle.PntInfo.SymID = Convert.ToInt32(newPntStyle.patternID._TextBoxInput.Text.Split(':')[0]);
                    if (newPntStyle.patternWidth.Text == "")
                        this.m_featureStyle.PntInfo.SymWidth = 0.0;
                    else
                    this.m_featureStyle.PntInfo.SymWidth = Convert.ToDouble(newPntStyle.patternWidth.Text);
                    break;
                case SFclsGeomType.Lin:
                    this.m_featureStyle.InfoType = GInfoType.LinInfo;
                    LineStyle newLineStyle = this.m_style as LineStyle;
                    this.m_featureStyle.LinInfo = new CLineInfo();
                    if (newLineStyle.color._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.LinInfo.Color = 0;
                    else
                    this.m_featureStyle.LinInfo.Color = Convert.ToInt32(newLineStyle.color._TextBoxInput.Text.Split(':')[0]);
                    if (newLineStyle.patternID._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.LinInfo.LinStyleID = 0;
                    else
                    this.m_featureStyle.LinInfo.LinStyleID = Convert.ToInt32(newLineStyle.patternID._TextBoxInput.Text.Split(':')[0]);
                    if (newLineStyle.patternID2._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.LinInfo.LinStyleID2 = 0;
                    else
                    this.m_featureStyle.LinInfo.LinStyleID2 = Convert.ToInt32(newLineStyle.patternID2._TextBoxInput.Text.Split(':')[0]);
                    if (newLineStyle.penWidth.Text == "")
                        this.m_featureStyle.LinInfo.LinWidth = 0.0;
                    else
                        this.m_featureStyle.LinInfo.LinWidth = Convert.ToDouble(newLineStyle.penWidth.Text);
                    if (newLineStyle.lineScaleX.Text == "")
                        this.m_featureStyle.LinInfo.Xscale = 0.0;
                    else
                        this.m_featureStyle.LinInfo.Xscale = Convert.ToDouble(newLineStyle.lineScaleX.Text);
                    if (newLineStyle.lineScaleY.Text == "")
                        this.m_featureStyle.LinInfo.Yscale = 0.0;
                    else
                        this.m_featureStyle.LinInfo.Yscale = Convert.ToDouble(newLineStyle.lineScaleY.Text);
                    break;
                case SFclsGeomType.Reg:
                    this.m_featureStyle.InfoType = GInfoType.RegInfo;
                    PolygonStyle newRegStyle = this.m_style as PolygonStyle;
                    this.m_featureStyle.RegInfo = new CRegionInfo();
                    if (newRegStyle.fillcolor._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.RegInfo.FillColor = 0;
                    else
                    this.m_featureStyle.RegInfo.FillColor = Convert.ToInt32(newRegStyle.fillcolor._TextBoxInput.Text.Split(':')[0]);
                    if (newRegStyle.patternColor._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.RegInfo.PatColor = 0;
                    else
                    this.m_featureStyle.RegInfo.PatColor = Convert.ToInt32(newRegStyle.patternColor._TextBoxInput.Text.Split(':')[0]);
                    if (newRegStyle.patternHeight.Text == "")
                        this.m_featureStyle.RegInfo.PatHeight = 0.0;
                    else
                    this.m_featureStyle.RegInfo.PatHeight = Convert.ToDouble(newRegStyle.patternHeight.Text);
                    if (newRegStyle.patternID._TextBoxInput.Text.Split(':')[0] == "")
                        this.m_featureStyle.RegInfo.PatID = 0;
                    else
                    this.m_featureStyle.RegInfo.PatID = Convert.ToInt32(newRegStyle.patternID._TextBoxInput.Text.Split(':')[0]);
                    if (newRegStyle.patternPenWidth.Text == "")
                        this.m_featureStyle.RegInfo.OutPenWidth = 0.0;
                    else
                    this.m_featureStyle.RegInfo.OutPenWidth = Convert.ToDouble(newRegStyle.patternPenWidth.Text);
                    if (newRegStyle.patternWidth.Text == "")
                        this.m_featureStyle.RegInfo.PatWidth = 0.0;
                    else
                    this.m_featureStyle.RegInfo.PatWidth = Convert.ToDouble(newRegStyle.patternWidth.Text);
                    break;
            }
            sf.fGeom = sfGeo;
            sf.FID = m_featureID;
            fInfo.GInfo = this.m_featureStyle;
            fInfo.FSet = sf;
            fInfo.GdbIndex = ActiveLayerObj.ActiveGdbIndex;
            fInfo.LayerIndex = ActiveLayerObj.ActiveLayerIndex;
            return fInfo;
        }
    }
}
