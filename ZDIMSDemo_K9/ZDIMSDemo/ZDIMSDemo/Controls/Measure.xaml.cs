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
using ZDIMS.Map;
using ZDIMS.Drawing;
using ZDIMS.Util;
using ZDIMS.Interface;
using ZDIMS.Event;
using System.ComponentModel;

namespace ZDIMSDemo.Controls
{
    public partial class Measure : BaseUserControl
    {
        private List<IMSCircle> measurePnts;
        private List<IMSMark> measureMarks;
        private List<IMSMark> measureCenterMarks;
        private IMSMark measureMark;
        private MarkLayer mymarklayer;
        private double measureleng = 0;
        private List<Point> measurePntsTmp;
        private List<IGraphics> measureGraphic;
        private IMSMap m_mapContainer = null;
        private bool IsDrawOver = true;
        [Browsable(true), Category("MapGisIMS"), Description("绘制图形所在的图层")]
        public GraphicsLayer m_graphicsLayer = null;

        [Browsable(true), Category("MapGisIMS"), Description("测量结果的单位长度代表的实际长度")]
        public double unitLength = 1;
        /// <summary>
        /// 关联的地图容器
        /// </summary>
        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set
            {
                m_mapContainer = value;
            }
        }
        public Measure()
        {
            InitializeComponent();
            this.measurePnts = new List<IMSCircle>();
            this.measureMarks = new List<IMSMark>();
            this.measureCenterMarks = new List<IMSMark>();
            this.measurePntsTmp = new List<Point>();
            this.measureGraphic = new List<IGraphics>();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
        }
        /**
        * 添加测量标记点
        */
        private void addMeasurePoint(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
          
            if (IsDrawOver)
                clearMeasurePnts(null, null, null);
            IMSCircle pnt = new IMSCircle(CoordinateType.Logic);
            pnt.RadiusEx = 4;
            pnt.CenX = logPntArr[logPntArr.Count - 1].X;
            pnt.CenY = logPntArr[logPntArr.Count - 1].Y;
            m_graphicsLayer.AddGraphics(pnt);
            pnt.Draw();
            this.measurePnts.Add(pnt);
            this.measurePntsTmp.Add(new Point(logPntArr[logPntArr.Count - 1].X, logPntArr[logPntArr.Count - 1].Y));

            if (this.m_graphicsLayer.DrawingType == DrawingType.Polyline)
            {
                Label currmeasureLabel = new Label();
                currmeasureLabel.Content = "";
                currmeasureLabel.FontSize = 12;
                IMSMark currmeasureMark = new IMSMark(currmeasureLabel, CoordinateType.Logic);
                currmeasureMark.EnableDrag = false;
                currmeasureMark.EnableAnimation = false;
                mymarklayer.AddMark(currmeasureMark);
                measureMarks.Add(currmeasureMark);
                //   (currmeasureMark.MarkControl as Label).Content = getLength(new Point(-1, -1)); ;
                string tollength = getCurrentLength(-1, -1, new Point(-1, -1));
                measureleng += Convert.ToDouble(tollength.Split(':')[1]);
                currmeasureMark.X = logPntArr[logPntArr.Count - 1].X + 0.0055;
                currmeasureMark.Y = logPntArr[logPntArr.Count - 1].Y - 0.0055;

                Label centermeasureLabel = new Label();
                centermeasureLabel.Content = "";
                centermeasureLabel.FontSize = 12;
                IMSMark centermeasureMark = new IMSMark(centermeasureLabel, CoordinateType.Logic);
                centermeasureMark.EnableDrag = false;
                centermeasureMark.EnableAnimation = false;
                mymarklayer.AddMark(centermeasureMark);
                measureCenterMarks.Add(centermeasureMark);
                (centermeasureMark.MarkControl as Label).RenderTransform = new RotateTransform();
                if ((bool)checkBox1.IsChecked)
                {

                    if (centermeasureMark != null)
                    {
                        //              centermeasureMark.Visibility = System.Windows.Visibility.Visible;
                    }

                }
                else
                {
                    if (centermeasureMark != null)
                    {
                        centermeasureMark.Visibility = System.Windows.Visibility.Collapsed;
                    }

                }

            }
            this.m_mapContainer.MouseMove -= new MouseEventHandler(m_graphicsLayer_MouseMove);
            this.m_mapContainer.MouseMove += new MouseEventHandler(m_graphicsLayer_MouseMove);
            IsDrawOver = false;
        }
        public string getLength(Point point)
        {
            if (this.measurePnts.Count < 1)
            {
                return "总长度:0";
            }
            if (point.X == -1 && point.Y == -1)
            {
                if (this.measurePnts.Count != 1)
                    point = new Point(this.measurePnts[this.measurePnts.Count - 2].CenX, this.measurePnts[this.measurePnts.Count - 2].CenY);
                else
                    point = new Point(this.measurePnts[this.measurePnts.Count - 1].CenX, this.measurePnts[this.measurePnts.Count - 1].CenY);
            }
            IMSCircle pnt = this.measurePnts[this.measurePnts.Count - 1];
            double length = Math.Sqrt(Math.Pow(pnt.CenX - point.X, 2) + Math.Pow(pnt.CenY - point.Y, 2));
            return "总长度:" + (Convert.ToDouble(measureleng + length * this.unitLength)).ToString();
        }
        /**
         * 获取长度
         */
        public string getCurrentLength(int currentIdx, int lastIdx, Point currPnt)
        {
            double length = 0;
            if (currPnt.X == -1 && currPnt.Y == -1)
            {
                if (this.measurePnts.Count < 2)
                {
                    return "线段长度:0";
                }
                if (currentIdx == -1 && lastIdx == -1)
                {
                    currentIdx = this.measurePnts.Count - 1;
                    lastIdx = this.measurePnts.Count - 2;
                }
                if (currentIdx == 0)
                    lastIdx = 0;
                IMSCircle pnt = this.measurePnts[currentIdx];
                IMSCircle lastPnt = this.measurePnts[lastIdx];
                length = Math.Sqrt(Math.Pow(pnt.CenX - lastPnt.CenX, 2) + Math.Pow(pnt.CenY - lastPnt.CenY, 2));
            }
            else
            {
                if (this.measurePnts.Count < 1)
                {
                    return "线段长度:0";
                }
                IMSCircle lastPnt = this.measurePnts[this.measurePnts.Count - 1];
                length = Math.Sqrt(Math.Pow(currPnt.X - lastPnt.CenX, 2) + Math.Pow(currPnt.Y - lastPnt.CenY, 2));
            }
            return "线段长度:" + (length * this.unitLength).ToString();
        }
        public string getArea(Point point)
        {
            if (this.measurePnts.Count < 2)
            {
                return "面积:0";
            }
            int dotNum;
            double area = 0;
            int j;
            double xjyi;
            double xiyj;
            double xydiff;
            if (point.X != -1 && point.Y != -1)
                measurePntsTmp.Add(point);
            dotNum = measurePntsTmp.Count - 1;
            j = dotNum;
            for (int i = 0; i <= dotNum; i++)
            {
                xiyj = measurePntsTmp[i].X * measurePntsTmp[j].Y;
                xjyi = measurePntsTmp[j].X * measurePntsTmp[i].Y;
                xydiff = (xiyj - xjyi);
                area += xydiff;
                j = i;
            }
            area = Math.Abs(area / 2) * this.unitLength;
            if (point.X != -1 && point.Y != -1)
                measurePntsTmp.Remove(point);
            return "面积:" + area.ToString();
        }
        private void clearMeasure_Click(object sender, RoutedEventArgs e)
        {
            clearMeasurePnts(null, null, null);
            if ((bool)this.Measurelen.IsChecked)
                Measurelen_Checked(null, null);
            else
                Measurearea_Checked(null, null);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            clearMeasurePnts(null, null, null);
            this.Close();
        }

        private void Measurelen_Checked(object sender, RoutedEventArgs e)
        {
            this.m_graphicsLayer.DrawingType = DrawingType.Polyline;
            setMeasureType();
        }

        private void Measurearea_Checked(object sender, RoutedEventArgs e)
        {
            this.m_graphicsLayer.DrawingType = DrawingType.Polygon;
            setMeasureType();
        }
        /**
         * 设置测量类型
         */
        private void setMeasureType()
        {
            if (this.m_graphicsLayer == null)
            {
                MessageBox.Show("m_graphicsLayer为空，请赋值!", "提示", MessageBoxButton.OK);
                return;
            }
            this.m_mapContainer = this.m_graphicsLayer.MapContainer as IMSMap;
            clearMeasurePnts(null, null, null);
            if (mymarklayer == null)
            {
                mymarklayer = new MarkLayer();
                this.m_mapContainer.AddChild(mymarklayer);
            }
            this.m_graphicsLayer.Drawing += new DrawingEventHandler(addMeasurePoint);
            this.m_graphicsLayer.DrawingTypeChange += new DrawingTypeChangeEventHandler(typeChange);
            this.m_graphicsLayer.DrawingOver += new DrawingEventHandler(reDrawMeasurePnts);
            this.m_mapContainer.MapOperTypeChange += new IMSMapEventHandler(onOperTypeChange);
        }
        private void reDrawMeasurePnts(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            IsDrawOver = true;
            this.m_graphicsLayer.RemoveGraphics(this.measurePnts[this.measurePnts.Count - 1]);
            this.measurePnts.RemoveAt(this.measurePnts.Count - 1);
            if (this.measureMarks != null && this.measureMarks.Count != 0)
            {
                this.mymarklayer.RemoveMark(this.measureMarks[this.measureMarks.Count - 1]);
                this.measureMarks.RemoveAt(this.measureMarks.Count - 1);
            }
            if (this.measureCenterMarks != null && this.measureCenterMarks.Count != 0)
            {
                this.mymarklayer.RemoveMark(this.measureCenterMarks[this.measureCenterMarks.Count - 1]);
                this.measureCenterMarks.RemoveAt(this.measureCenterMarks.Count - 1);

                this.mymarklayer.RemoveMark(this.measureCenterMarks[this.measureCenterMarks.Count - 1]);
                this.measureCenterMarks.RemoveAt(this.measureCenterMarks.Count - 1);
            }
            switch (graphics.DrawingType)
            {
                case DrawingType.Polyline:
                    IMSPolyline pl = new IMSPolyline(CoordinateType.Logic) { EnableEdit = true };
                    this.m_graphicsLayer.AddGraphics(pl);
                    for (int i = 0; i < logPntArr.Count; i++)
                    {
                        pl.Points.Add(logPntArr[i]);
                    }
                    pl.Draw();

                    pl.EditMark1DragInCallback += new EditMarkDelegate(markLengthMove);
                    pl.EditMark1DragOverCallback += new EditMarkDelegate(markOver);
                    pl.EditMark2DragInCallback += new EditMarkDelegate(mark2LengthMove);
                    pl.EditMark2DragOverCallback += new EditMarkDelegate(mark2LengthOver);
                    pl.EnableEdit = false;
                    measureGraphic.Add(pl);
                    break;
                case DrawingType.Polygon:
                    IMSPolygon p2 = new IMSPolygon(CoordinateType.Logic) { EnableEdit = true };
                    this.m_graphicsLayer.AddGraphics(p2);
                    for (int i = 0; i < logPntArr.Count; i++)
                    {
                        p2.Points.Add(logPntArr[i]);
                    }
                    p2.Draw();
                    p2.EditMark1DragInCallback += new EditMarkDelegate(markAreaMove);
                    p2.EditMark1DragOverCallback += new EditMarkDelegate(markOver);
                    p2.EnableEdit = false;
                    measureGraphic.Add(p2);
                    break;
            }
            //   if (this.measureMark != null)
            //   {
            //   (this.measureMark.MarkControl as Label).Content = "";
            //    (this.measureMark.MarkControl as Label).Visibility = System.Windows.Visibility.Collapsed;
            //   }
            //    if (this.measurePnts != null)
            //     {
            //        for (int i = 0; i < this.measurePnts.Count; i++)
            //        {
            //            this.m_graphicsLayer.RemoveGraphics(this.measurePnts[i]);
            //       }
            //   }
            this.m_mapContainer.MouseMove -= new MouseEventHandler(m_graphicsLayer_MouseMove);
        }
        private void markAreaMove(int curEditMarkIndex, Shape curEditMark, List<Shape> editMarkList, GraphicsBase g)
        {
            this.m_graphicsLayer.Drawing -= new DrawingEventHandler(addMeasurePoint);
            Point pnt = m_mapContainer.ScreenToLogic(Canvas.GetLeft(curEditMark), Canvas.GetTop(curEditMark));
            this.measurePntsTmp[curEditMarkIndex] = pnt;

            if (this.measureMark != null)
            {
                (this.measureMark.MarkControl as Label).Content = getArea(new Point(-1, -1));
                (this.measureMark.MarkControl as Label).Visibility = System.Windows.Visibility.Collapsed;
                this.measureMark.X = pnt.X + 0.0055;
                this.measureMark.Y = pnt.Y - 0.0055;
            }
        }
        private void mark2LengthMove(int curEditMarkIndex, Shape curEditMark, List<Shape> editMarkList, GraphicsBase g)
        {
            this.m_graphicsLayer.Drawing -= new DrawingEventHandler(addMeasurePoint);
            IMSMark centermeasureMark = this.measureCenterMarks[curEditMarkIndex];
            centermeasureMark.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void mark2LengthOver(int curEditMarkIndex, Shape curEditMark, List<Shape> editMarkList, GraphicsBase g)
        {
            this.m_graphicsLayer.Drawing += new DrawingEventHandler(addMeasurePoint);
            Point pnt = m_mapContainer.ScreenToLogic(Canvas.GetLeft(g.EditMark1List[curEditMarkIndex + 1]), Canvas.GetTop(g.EditMark1List[curEditMarkIndex + 1]));
            IMSCircle circle = new IMSCircle();
            circle.CenX = pnt.X;
            circle.CenY = pnt.Y;
            this.measurePnts.Insert(curEditMarkIndex + 1, circle);

            double currLength = 0;
            Label currmeasureLabel = new Label();
            currmeasureLabel.Content = "";
            currmeasureLabel.FontSize = 12;
            IMSMark currmeasureMark = new IMSMark(currmeasureLabel, CoordinateType.Logic);
            currmeasureMark.EnableDrag = false;
            mymarklayer.AddMark(currmeasureMark);
            measureMarks.Insert(curEditMarkIndex + 1, currmeasureMark);
            currmeasureMark.X = pnt.X + 0.0055;
            currmeasureMark.Y = pnt.Y - 0.0055;

            IMSMark lastmeasureMark = measureMarks[curEditMarkIndex];
            currLength += Convert.ToDouble((lastmeasureMark.MarkControl as Label).Content.ToString().Split(':')[1]);

            IMSMark centermeasureMark = this.measureCenterMarks[curEditMarkIndex];
            (centermeasureMark.MarkControl as Label).Content = getCurrentLength(curEditMarkIndex + 1, curEditMarkIndex, new Point(-1, -1));
            IMSCircle pntCircle = this.measurePnts[curEditMarkIndex];
            centermeasureMark.X = (pnt.X + 3 * pntCircle.CenX) / 4;
            centermeasureMark.Y = (pnt.Y + 3 * pntCircle.CenY) / 4;
            Point scrPnt = this.m_mapContainer.LogicToScreen(pntCircle.CenX, pntCircle.CenY);
            double angle = Math.Atan2(scrPnt.Y - Canvas.GetTop(g.EditMark1List[curEditMarkIndex + 1]), Canvas.GetLeft(g.EditMark1List[curEditMarkIndex + 1]) - scrPnt.X);
            ((centermeasureMark.MarkControl as Label).RenderTransform as RotateTransform).Angle = 360 - angle * 360 / (2 * Math.PI);

            if ((bool)checkBox1.IsChecked)
                centermeasureMark.Visibility = System.Windows.Visibility.Visible;
            else
                centermeasureMark.Visibility = System.Windows.Visibility.Collapsed;

            Label centermeasureLabel2 = new Label();
            centermeasureLabel2.Content = "";
            centermeasureLabel2.FontSize = 12;
            IMSMark centermeasureMark2 = new IMSMark(centermeasureLabel2, CoordinateType.Logic);
            centermeasureMark2.EnableDrag = false;
            centermeasureMark2.EnableAnimation = false;
            mymarklayer.AddMark(centermeasureMark2);
            measureCenterMarks.Insert(curEditMarkIndex + 1, centermeasureMark2);
            (centermeasureMark2.MarkControl as Label).RenderTransform = new RotateTransform();
            (centermeasureMark2.MarkControl as Label).Content = getCurrentLength(curEditMarkIndex + 2, curEditMarkIndex + 1, new Point(-1, -1));
            IMSCircle pntCircle2 = this.measurePnts[curEditMarkIndex + 2];
            centermeasureMark2.X = (3 * pnt.X + pntCircle2.CenX) / 4;
            centermeasureMark2.Y = (3 * pnt.Y + pntCircle2.CenY) / 4;
            Point scrPnt2 = this.m_mapContainer.LogicToScreen(pntCircle2.CenX, pntCircle2.CenY);
            double angle2 = Math.Atan2(Canvas.GetTop(g.EditMark1List[curEditMarkIndex + 1]) - scrPnt2.Y, scrPnt2.X - Canvas.GetLeft(g.EditMark1List[curEditMarkIndex + 1]));
            ((centermeasureMark2.MarkControl as Label).RenderTransform as RotateTransform).Angle = 360 - angle2 * 360 / (2 * Math.PI);
            if ((bool)checkBox1.IsChecked)
                centermeasureMark2.Visibility = System.Windows.Visibility.Visible;
            else
                centermeasureMark2.Visibility = System.Windows.Visibility.Collapsed;

            for (int i = curEditMarkIndex; i < this.measureMarks.Count - 1; i++)
            {
                IMSMark nextmeasureMark = measureMarks[i + 1];
                double nextLegth = Convert.ToDouble(getCurrentLength(i + 1, i, new Point(-1, -1)).Split(':')[1]);
                currLength += nextLegth;
                (nextmeasureMark.MarkControl as Label).Content = "总长度:" + currLength.ToString();
            }
        }
        private void markLengthMove(int curEditMarkIndex, Shape curEditMark, List<Shape> editMarkList, GraphicsBase g)
        {
            this.m_graphicsLayer.Drawing += new DrawingEventHandler(addMeasurePoint);
            this.m_graphicsLayer.Drawing -= new DrawingEventHandler(addMeasurePoint);
            Point pnt = m_mapContainer.ScreenToLogic(Canvas.GetLeft(curEditMark), Canvas.GetTop(curEditMark));
            if (measurePnts.Count >= curEditMarkIndex)
            {
                this.measurePnts[curEditMarkIndex].CenX = pnt.X;
                this.measurePnts[curEditMarkIndex].CenY = pnt.Y;
            }
            else
            {

                return;
            }

            IMSMark currmeasureMark = measureMarks[curEditMarkIndex];
            double currLength = Convert.ToDouble(getCurrentLength(curEditMarkIndex, curEditMarkIndex - 1, new Point(-1, -1)).Split(':')[1]);
            if (currLength != 0)
            {
                IMSMark lastmeasureMark = measureMarks[curEditMarkIndex - 1];
                currLength += Convert.ToDouble((lastmeasureMark.MarkControl as Label).Content.ToString().Split(':')[1]);
            }
            (currmeasureMark.MarkControl as Label).Content = "总长度:" + currLength.ToString();
            currmeasureMark.X = pnt.X + 0.0055;
            currmeasureMark.Y = pnt.Y - 0.0055;
            if (curEditMarkIndex != 0)
            {
                IMSMark centermeasureMark = this.measureCenterMarks[curEditMarkIndex - 1];
                (centermeasureMark.MarkControl as Label).Content = getCurrentLength(curEditMarkIndex, curEditMarkIndex - 1, new Point(-1, -1));
                IMSCircle pntCircle = this.measurePnts[curEditMarkIndex - 1];
                centermeasureMark.X = (pnt.X + 3 * pntCircle.CenX) / 4;
                centermeasureMark.Y = (pnt.Y + 3 * pntCircle.CenY) / 4;
                Point scrPnt = this.m_mapContainer.LogicToScreen(pntCircle.CenX, pntCircle.CenY);
                double angle = Math.Atan2(scrPnt.Y - Canvas.GetTop(curEditMark), Canvas.GetLeft(curEditMark) - scrPnt.X);
                ((centermeasureMark.MarkControl as Label).RenderTransform as RotateTransform).Angle = 360 - angle * 360 / (2 * Math.PI);
                if ((bool)checkBox1.IsChecked)
                    centermeasureMark.Visibility = System.Windows.Visibility.Visible;
                else
                    centermeasureMark.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (curEditMarkIndex < this.measureCenterMarks.Count)
            {
                IMSMark centermeasureMark = this.measureCenterMarks[curEditMarkIndex];
                (centermeasureMark.MarkControl as Label).Content = getCurrentLength(curEditMarkIndex + 1, curEditMarkIndex, new Point(-1, -1));
                IMSCircle pntCircle = this.measurePnts[curEditMarkIndex + 1];
                centermeasureMark.X = (3 * pnt.X + pntCircle.CenX) / 4;
                centermeasureMark.Y = (3 * pnt.Y + pntCircle.CenY) / 4;
                Point scrPnt = this.m_mapContainer.LogicToScreen(pntCircle.CenX, pntCircle.CenY);
                double angle = Math.Atan2(Canvas.GetTop(curEditMark) - scrPnt.Y, scrPnt.X - Canvas.GetLeft(curEditMark));
                ((centermeasureMark.MarkControl as Label).RenderTransform as RotateTransform).Angle = 360 - angle * 360 / (2 * Math.PI);
                if ((bool)checkBox1.IsChecked)
                {
                    if (centermeasureMark != null)
                    {
                        centermeasureMark.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                else
                {

                    if (centermeasureMark != null)
                    {
                        centermeasureMark.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }

            }
            for (int i = curEditMarkIndex; i < this.measureMarks.Count - 1; i++)
            {
                IMSMark nextmeasureMark = measureMarks[i + 1];
                double nextLegth = Convert.ToDouble(getCurrentLength(i + 1, i, new Point(-1, -1)).Split(':')[1]);
                currLength += nextLegth;
                (nextmeasureMark.MarkControl as Label).Content = "总长度:" + currLength.ToString();
            }
        }
        private void markOver(int curEditMarkIndex, Shape curEditMark, List<Shape> editMarkList, GraphicsBase g)
        {
            this.m_graphicsLayer.Drawing += new DrawingEventHandler(addMeasurePoint);
        }
        private void onOperTypeChange(IMSMapEvent e)
        {
            this.m_mapContainer.MouseMove -= new MouseEventHandler(m_graphicsLayer_MouseMove);
            this.m_mapContainer.MapOperTypeChange -= new IMSMapEventHandler(onOperTypeChange);
            clearMeasurePnts(null, null, null);
        }

        void m_graphicsLayer_MouseMove(object sender, MouseEventArgs e)
        {
            Point pnt = e.GetPosition(this.m_mapContainer);
            Point logpnt = this.m_mapContainer.ScreenToLogic(pnt.X, pnt.Y);
            if (this.m_graphicsLayer.DrawingType == DrawingType.Polyline)
            {
                IMSMark centermeasureMark = this.measureCenterMarks[this.measureCenterMarks.Count - 1];
                (centermeasureMark.MarkControl as Label).Content = getCurrentLength(-1, -1, logpnt);
                IMSCircle pntCircle = this.measurePnts[this.measurePnts.Count - 1];
                centermeasureMark.X = (logpnt.X + 3 * pntCircle.CenX) / 4;
                centermeasureMark.Y = (logpnt.Y + 3 * pntCircle.CenY) / 4;
                Point scrPnt = this.m_mapContainer.LogicToScreen(pntCircle.CenX, pntCircle.CenY);
                double angle = Math.Atan2(scrPnt.Y - pnt.Y, pnt.X - scrPnt.X);
                ((centermeasureMark.MarkControl as Label).RenderTransform as RotateTransform).Angle = 360 - angle * 360 / (2 * Math.PI);
                if ((bool)checkBox1.IsChecked)
                    centermeasureMark.Visibility = System.Windows.Visibility.Visible;
                else
                    centermeasureMark.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (this.measureMark == null)
            {
                Label measureLabel = new Label();
                measureLabel.Content = "";
                measureLabel.Name = "measureResultLabel";
                measureLabel.FontSize = 12;
                this.measureMark = new IMSMark(measureLabel, CoordinateType.Logic);
                this.measureMark.EnableDrag = false;
                mymarklayer.AddMark(this.measureMark);
            }
            this.measureMark.X = logpnt.X + 0.0055;
            this.measureMark.Y = logpnt.Y - 0.0055;

            try
            {
                if (measureMark != null)
                {
                    this.measureMark.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                return;
            }



            (this.measureMark.MarkControl as Label).Content = this.m_graphicsLayer.DrawingType == DrawingType.Polyline ? getLength(logpnt) : getArea(logpnt);
        }
        private void typeChange(GraphicsLayer gLayer, DrawingType oldDrawingType, DrawingType newDrawingType)
        {
            clearMeasurePnts(null, null, null);
        }

        /**
        * 清除测量结果
        */
        private void clearMeasurePnts(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            measureleng = 0;
            for (int i = 0; i < this.measurePnts.Count; i++)
            {
                this.m_graphicsLayer.RemoveGraphics(this.measurePnts[i]);
            }
            for (int i = 0; i < this.measureMarks.Count; i++)
            {
                this.mymarklayer.RemoveMark(this.measureMarks[i]);
            }
            for (int i = 0; i < this.measureCenterMarks.Count; i++)
            {
                this.mymarklayer.RemoveMark(this.measureCenterMarks[i]);
            }
            for (int i = 0; i < this.measureGraphic.Count; i++)
            {
                this.measureGraphic[i].EnableEdit = false;
                this.m_graphicsLayer.RemoveGraphics(this.measureGraphic[i]);
            }
            this.measurePnts.Clear();
            this.measureMarks.Clear();
            this.measureCenterMarks.Clear();
            this.measurePntsTmp.Clear();
            this.measureGraphic.Clear();
            if (this.measureMark != null)
            {
                (this.measureMark.MarkControl as Label).Content = "";
                (this.measureMark.MarkControl as Label).Visibility = System.Windows.Visibility.Collapsed;
            }
            if (this.m_mapContainer != null)
                this.m_mapContainer.MouseMove -= new MouseEventHandler(m_graphicsLayer_MouseMove);
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (this.measureCenterMarks != null)
            {
                for (int i = 0; i < this.measureCenterMarks.Count; i++)
                {
                    IMSMark centermeasureMark = this.measureCenterMarks[i];
                    centermeasureMark.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.measureCenterMarks != null)
            {
                for (int i = 0; i < this.measureCenterMarks.Count; i++)
                {
                    IMSMark centermeasureMark = this.measureCenterMarks[i];
                    centermeasureMark.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
    }
}
