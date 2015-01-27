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
using Logistics_system.ServiceClient;
using ZDIMS.Map;
using ZDIMS.Util;
using ZDIMS.Drawing;
using ZDIMSDemo.Controls;
using DevExpress.AgDataGrid;
using ZDIMS.Interface;
namespace Logistics_system.controls
{
    public partial class oradrForm : BaseUserControl
    {
        public List<IMSMark> warnMarks;
        public marketsInfo markInfo;
        public List<OrderSatement> orders;
        public IMSMap mapContainer;
        public marketsInfo CenterMarketInfo;
        ServiceSoapClient serviceClient = new ServiceSoapClient();
        addMarks mark;
        public MarkLayer markLayer;
        public GraphicsLayer graphicsLayer;
        trackplaycontrol busAnalyse;
        GIFToolTip warnMark = new GIFToolTip();
        string pntStr = "";
        public VectorLayerBase VecObj;
        public oradrForm()
        {
            InitializeComponent();
            this.dialog.OnClose += new RoutedEventHandler(close);
        }
        public void initInfo()
        {
            
             if (mark == null)
            {
                mark = new addMarks();
                mark.graphicsLayer = this.graphicsLayer;
                mark.markLayer1 = this.markLayer;
                mark.mapContainer = this.mapContainer;
            }
            #region 废弃
            //warnMark = new GIFToolTip();
            //warnMark.Addtip(markInfo);
            //IMSMark markPnt = new IMSMark(warnMark, CoordinateType.Logic) { EnableAnimation = false };
            //Point pn = new Point(markInfo.X, markInfo.Y);
            ////pn = logicPnt;
            //pn = this.mapContainer.LogicToScreen(pn.X, pn.Y);
            //pn.X = pn.X - 11; //- 17;
            //pn.Y = pn.Y - 11; //- 44;
            //pn = this.mapContainer.ScreenToLogic(pn.X, pn.Y);
            //markPnt.X = pn.X;
            //markPnt.Y = pn.Y;
            //this.mapContainer.SetCenter(markInfo.X, markInfo.Y);
            //// markLayer1.ManuallyAddMarkObj = markPnt;
            //this.markLayer.AddMark(markPnt);
            #endregion 
                

                pntStr =  CenterMarketInfo.X + "," + CenterMarketInfo.Y + ","+markInfo.X + "," + markInfo.Y + "," ;
                this.StateNameTextBox.Text = markInfo.MarketName;    
            this.grid.DataSource = orders;
        }
        public void close(object sender, RoutedEventArgs e)
        {
            if (this.markLayer != null)
            {
                
                    for (int i = 0; i < warnMarks.Count; i++)
                        markLayer.RemoveMark(warnMarks[i]);
                    warnMarks.Clear();
                
            }
            if (this.grid.RowCount > 0)
            {
                for (int i = 0; i < this.grid.RowCount; i++)
                {
                    this.grid.DeleteRow(i);

                }
            }
            this.StateNameTextBox.Text = "";
            this.Close();
        }
       
        private void xianlu_Click(object sender, RoutedEventArgs e)
        {
           warnMarks.Add( mark.AddMark(CenterMarketInfo));
            busAnalyse = new   trackplaycontrol(this.markLayer, this.graphicsLayer);
            busAnalyse.centerInfo = CenterMarketInfo;
            busAnalyse.marketInfo = markInfo;
            busAnalyse.Btn_Submit(CenterMarketInfo.MarketName, markInfo.MarketName);
            List<OrderSatement> SendInfo=new List<OrderSatement>();
             OrderSatement tmpvalue;
            for(int i=0;i<orders.Count;i++)
            {
                 tmpvalue=new OrderSatement();
                tmpvalue.MarketID=orders[i].MarketID;
                tmpvalue.ReceTime=DateTime.Now;
                tmpvalue.ReceNum=orders[i].PreNum;
                tmpvalue.ProdectID=orders[i].ProdectID;
                SendInfo.Add(tmpvalue);
            }
            string dataValue=XMLSerialization.Serialize(SendInfo);
            serviceClient.UppDateOrderStatmentAsync(markInfo.MarketID, dataValue);
            serviceClient.UppDateOrderStatmentCompleted += new EventHandler<UppDateOrderStatmentCompletedEventArgs>(serviceClient_UppDateOrderStatmentCompleted);
        }

        void serviceClient_UppDateOrderStatmentCompleted(object sender, UppDateOrderStatmentCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("更新成功");
            }
        }

        #region 废弃
        void grid_FocusedRowChanged(object sender, DevExpress.AgDataGrid.Data.FocusedRowChangedEventArgs e)
        {
            //index = e.NewRowHandle;
            //pnt = new Point();
            //this.pnt.X = searchRlt[index].X;
            //this.pnt.Y = searchRlt[index].Y;
            //OnFocusedRowChanged(e.NewDataRow);
        }

        void grid_NewDataRow(object sender, NewDataRowEventArgs e)
        {
            //if (RBToolTip == null || RBToolTip.IsChecked == false) return;
            //AgPreviewToolTip tt = new AgPreviewToolTip();
            //tt.DataContext = e.DataRow;
            //tt.Content = grid.PreviewTemplate.LoadContent();
            //ToolTipService.SetToolTip(e.Row, tt);
        }
        #endregion
    }
}
