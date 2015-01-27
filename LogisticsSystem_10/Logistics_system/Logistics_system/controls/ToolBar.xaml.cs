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
using DevExpress.AgMenu;
using Logistics_system;
using ZDIMS.Map;
using ZDIMS.Drawing;
using ZDIMS.Event;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using System.Threading;
using Logistics_system.controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Logistics_system.ServiceClient;
namespace Logistics_system.controls
{
    public partial class ToolBar : UserControl
    {
        // AgMenu menu;
        DataForm market_S;//门店查询框
        DetailsForms forms = null;
        staticAnalyse staticControl = null;
        DispatcherTimer TimeController = new DispatcherTimer();
        marketsInfo AddMarkInfo;
        AddMarket NewMarkInfo;
        List<marketsInfo> CenterMarketInfo;
        ServiceSoapClient serviceClient = new ServiceSoapClient();
        public IMSMap mapContainer;
        public MarkLayer markLayer;
        public GraphicsLayer graphicsLayer;
        List<marketsInfo> orderMarket = new List<marketsInfo>();
        List<OrderSatement> Orders = new List<OrderSatement>();
        oradrForm oradrsForm;
        List<IMSMark> warnMarks;
        public VectorLayerBase vecObj;
        public ToolBar()
        {
            InitializeComponent();
            TimeController.Interval = new TimeSpan(0, 0, 0, 30, 0);
            TimeController.Start();
            TimeController.Tick += new EventHandler(TimeController_Tick);
        }

        void TimeController_Tick(object sender, EventArgs e)
        {
            if (warnMarks != null && warnMarks.Count > 0)
            {
                warnMarks.Clear();
            }
            TimeController.Tick -= new EventHandler(TimeController_Tick);
            serviceClient.ImportInfoAsync(1);
            serviceClient.ImportInfoCompleted += new EventHandler<ImportInfoCompletedEventArgs>(serviceClient_ImportInfoCompleted);
        }

        void serviceClient_ImportInfoCompleted(object sender, ImportInfoCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string tmp = e.Result;
                string[] t = tmp.Split(';');
                if (t[0] != "无信息")
                {
                    Orders = XMLSerialization.Desrialize(Orders, t[0]);
                }
                if (Orders.Count <= 0)
                {
                }
                else
                {
                    if (t[1] != "无信息")
                    {

                        orderMarket = XMLSerialization.Desrialize(orderMarket, t[1]);
                        AddMarkGif(orderMarket[0]);
                        this.warn.Width = 65;
                        this.warn.Height = 65;
                    }

                }
            }
        }

        void warn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加报警门店
        /// </summary>
        /// <param name="LogPnt"></param>
        public void AddMarkGif(marketsInfo warnMarket)
        {
            if (warnMarks == null)
            {
                warnMarks = new List<IMSMark>();
            }
            GIFToolTip gif = new GIFToolTip();
            gif.Addtip(warnMarket);
            IMSMark mark = new IMSMark(gif, CoordinateType.Logic, this.markLayer);
            mark.EnableAnimation = false;
            mark.EnableRevisedPos = true;

            Point pnt = new Point(warnMarket.X, warnMarket.Y);
            pnt = this.mapContainer.LogicToScreen(pnt.X, pnt.Y);
            pnt.X = pnt.X - 11;
            pnt.Y = pnt.Y - 11;
            pnt = this.mapContainer.ScreenToLogic(pnt.X, pnt.Y);

            mark.X = pnt.X;
            mark.Y = pnt.Y;
            warnMarks.Add(mark);
            this.markLayer.AddMark(mark);
            this.mapContainer.SetCenter(warnMarket.X, warnMarket.Y);
        }
        /// <summary>
        /// 找总店
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serviceClient_SearchProdectsByIDCompleted(object sender, SearchDetailsByIDCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //查到总店信息
                string tmp = e.Result;
                if (tmp != "无信息")
                {
                    CenterMarketInfo = XMLSerialization.Desrialize(CenterMarketInfo, tmp);
                    oradrsForm = new oradrForm();
                    oradrsForm.warnMarks = this.warnMarks;
                    oradrsForm.markLayer = this.markLayer;
                    oradrsForm.VecObj = this.vecObj;
                    oradrsForm.mapContainer = this.mapContainer;
                    oradrsForm.graphicsLayer = this.graphicsLayer;
                    oradrsForm.markInfo = orderMarket[0];
                    oradrsForm.orders = Orders;
                    oradrsForm.CenterMarketInfo = CenterMarketInfo[0];
                    oradrsForm.initInfo();
                    oradrsForm.Show();
                }
                else
                {
                    MessageBox.Show("没有订货信息");
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        void menu1_ItemClick(object sender, AgMenuEventEventArgs e)
        {
            //tbLastClickedItem.Text = GetItemPath(e.Item);
        }
        /// <summary>
        /// 门店查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void market_Search_Click(object sender, RoutedEventArgs e)
        {
            this.markLayer.ManuallyAddMarkObj = null;
            if (market_S == null)
            {
                market_S = new DataForm();
            }
            this.market_S.mapContainer = this.mapContainer;
            this.market_S.markLayer = this.markLayer;
            market_S.Show();

        }
        /// <summary>
        /// 添加新店
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMarke_Click(object sender, RoutedEventArgs e)
        {
            graphicsLayer.DrawingType = DrawingType.None;
            if (markLayer != null)
            {
                this.markLayer.ManuallyAddMarkObj = null;
                IMSMark markPnt = new IMSMark(new Image() { Source = new BitmapImage(new Uri("../images/red2.png", UriKind.Relative)), Width = 20, Height = 20 }) { EnableAnimation = false };
                markLayer.ManuallyAddMarkObj = markPnt;
                graphicsLayer.DrawingType = DrawingType.Circle;
                markLayer.ManuallyAddMarkOverCallback = new ManuallyAddMarkDelegate(StartPntAdd1);
            }

        }
        private void StartPntAdd1(MarkLayer markLayer, IMark mark, Point logicPnt)
        {
            Point MarkPnt = new Point(logicPnt.X, logicPnt.Y);
            this.markLayer.ManuallyAddMarkObj = null;
            NewMarkInfo = new AddMarket();
            NewMarkInfo.markLayer = this.markLayer;
            NewMarkInfo.tmp = mark as IMSMark;
            NewMarkInfo.markPnt = MarkPnt;
            NewMarkInfo.mapContainer = this.mapContainer;
            NewMarkInfo.Show();
            markLayer.ManuallyAddMarkOverCallback -= new ManuallyAddMarkDelegate(StartPntAdd1);
        }
        private void delieveProdects_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void staticAna_Click(object sender, RoutedEventArgs e)
        {
            this.markLayer.ManuallyAddMarkObj = null;

            if (graphicsLayer == null)
            {
                MessageBox.Show("绘图为空");
            }

            this.markLayer.ManuallyAddMarkObj = null;
            IMSMark markPnt = new IMSMark(new Image() { Source = new BitmapImage(new Uri("../images/btn_08.png", UriKind.Relative)), Width = 20, Height = 20 }) { EnableAnimation = false };
            markLayer.ManuallyAddMarkObj = markPnt;
            graphicsLayer.DrawingType = DrawingType.Circle;
            markLayer.ManuallyAddMarkOverCallback += new ManuallyAddMarkDelegate(cal);
            graphicsLayer.DrawingOverCallback += new DrawingEventHandler(callback);

        }
        private void cal(MarkLayer markLayer, IMark mark, Point logicPnt)
        {
            this.markLayer.RemoveMark(mark);
        }

        private void callback(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            this.markLayer.ManuallyAddMarkObj = null;
            graphicsLayer.DrawingType = DrawingType.None;
            Circle circle = new Circle();
            if (logPntArr.Count > 1)
            {
                circle.Center = new Dot_2D()
                {
                    x = logPntArr[0].X,
                    y = logPntArr[0].Y

                };
                circle.Radius = Math.Sqrt(Math.Pow(logPntArr[0].X - logPntArr[1].X, 2) + Math.Pow(logPntArr[0].Y - logPntArr[1].Y, 2));
                if (staticControl == null)
                    staticControl = new staticAnalyse();
                // staticControl.circle = circle;
                staticControl.markLayer = this.markLayer;
                staticControl.mapContainer = this.mapContainer;
                staticControl.GetInfo(circle);
                staticControl.ClearValue();
                //staticControl.Show();

            }
        }

        void protects_click(object sender, EventArgs e)
        {

        }
        void sItem1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        void item_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void staticAna_MouseMove(object sender, MouseEventArgs e)
        {
            //this.menu1.Visibility = System.Windows.Visibility.Collapsed;
        }
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void warnbt_Click(object sender, RoutedEventArgs e)
        {
            this.markLayer.ManuallyAddMarkObj = null;
            this.warn.Width = 50;
            this.warn.Height = 50;
            if (orderMarket != null && orderMarket.Count > 0)
            {
                serviceClient.SearchDetailsByIDAsync(orderMarket[0].CenterID, "marketsInfo");

                serviceClient.SearchDetailsByIDCompleted += new EventHandler<SearchDetailsByIDCompletedEventArgs>(serviceClient_SearchProdectsByIDCompleted);
            }
        }
    }
}
