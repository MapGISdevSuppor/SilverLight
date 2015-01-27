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
using ZDIMSDemo.Controls;
using DevExpress.AgDataGrid;
using System.Windows.Media.Imaging;
using ZDIMS.BaseLib;
using ZDIMS.Map;
using ZDIMS.Drawing;
using Logistics_system.controls;
using Logistics_system.ServiceClient;
namespace Logistics_system
{
    public partial class DataForm : BaseUserControl
    {
        ServiceSoapClient SaerchByKey;
        List<marketsInfo> searchRlt;
        checkTextLegality checkText = new checkTextLegality();
        DetailsForms form = null;
        // List<marketsInfo> _markets = null;
        addMarks setMark = null;
        int index;
        Point pnt;
        addMarks Marks = new addMarks();
        List<IMSMark> shopMarks;
        public MarkLayer markLayer;
        public IMSMap mapContainer;
        public DataForm()
        {
            InitializeComponent();

            //  grid.DataSource = ff();// EmployeesData.DataSource;
            grid.Loaded += new RoutedEventHandler(grid_Loaded);
            this.dialog.OnClose += new RoutedEventHandler(close);
            this.grid.FocusedRowChanged += new DevExpress.AgDataGrid.Data.FocusedRowChangedEventHandler(grid_FocusedRowChanged);

        }
        public void close(object sender, RoutedEventArgs e)
        {
            clear();
            this.Close();
        }
        private void clear()
        {
            if (this.grid.RowCount > 0)
            {
                for (int i = 0; i < this.grid.RowCount; i++)
                {
                    this.grid.DeleteRow(i);
                }
            }
            if (this.searchRlt != null && this.searchRlt.Count > 0)
            {
                this.searchRlt.Clear();
            }
            if (this.shopMarks != null && markLayer != null)
            {
                for (int i = 0; i < shopMarks.Count; i++)
                {
                    markLayer.RemoveMark(shopMarks[i]);
                }
                shopMarks.Clear();
            }
        }
        public List<marketsInfo> ff()
        {

            List<marketsInfo> i = new List<marketsInfo>();
            marketsInfo f = new marketsInfo();
            f.Address = "dfsfdw";
            f.MarketID = 1;
            f.MarketName = "超市1";

            i.Add(f);
            f = new marketsInfo();

            f.MarketName = "超市2";
            f.MarketID = 2;
            f.Address = "fsfsf";

            i.Add(f);
            f = new marketsInfo();

            f.MarketName = "超市3";
            f.MarketID = 3;
            f.Address = "fsfsf";

            i.Add(f);
            return i;
        }
        void grid_Loaded(object sender, RoutedEventArgs e)
        {

            grid.ApplyTemplate();
        }
        void grid_FocusedRowChanged(object sender, DevExpress.AgDataGrid.Data.FocusedRowChangedEventArgs e)
        {
            index = e.NewRowHandle;
            pnt = new Point();
            this.pnt.X = searchRlt[index].X;
            this.pnt.Y = searchRlt[index].Y;
            //OnFocusedRowChanged(e.NewDataRow);
        }
        protected void OnFocusedRowChanged(object focusedRow)
        {

            //if (PreviewContent.Content == null) PreviewContent.Content = grid.PreviewTemplate.LoadContent();
            //PreviewContent.Height = double.NaN;
            //PreviewContent.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //PreviewContent.Height = Math.Round(PreviewContent.DesiredSize.Height);
            //PreviewContent.DataContext = focusedRow;
        }
        void grid_NewDataRow(object sender, NewDataRowEventArgs e)
        {
            //if (RBToolTip == null || RBToolTip.IsChecked == false) return;
            //AgPreviewToolTip tt = new AgPreviewToolTip();
            //tt.DataContext = e.DataRow;
            //tt.Content = grid.PreviewTemplate.LoadContent();
            //ToolTipService.SetToolTip(e.Row, tt);
        }
        #region OptionsPanel
        string currentChecked;
        void rb_Checked(object sender, RoutedEventArgs e)
        {
            //RadioButton rb = (RadioButton)sender;
            //if (currentChecked == rb.Name) return;
            //currentChecked = rb.Name;
            //grid.BeginUpdate();
            //try
            //{
            //    if (sender == RBNormal)
            //    {
            //        LayoutRoot.RowDefinitions[0].MaxHeight = 592;
            //        grid.PreviewVisibility = DataPreviewVisibility.ForFocusedRow;
            //        PreviewContent.Visibility = Visibility.Collapsed;
            //    }
            //    if (sender == RBToolTip)
            //    {
            //        //grid.PreviewVisibility = DataPreviewVisibility.Collapsed;
            //        //PreviewContent.Visibility = Visibility.Collapsed;
            //        //LayoutRoot.RowDefinitions[0].MaxHeight = 307;
            //    }
            //    if (sender == RBOutside)
            //    {
            //        grid.PreviewVisibility = DataPreviewVisibility.Collapsed;
            //        PreviewContent.Visibility = Visibility.Visible;
            //        OnFocusedRowChanged(grid.FocusedDataRow);
            //        LayoutRoot.RowDefinitions[0].MaxHeight = 307;
            //    }
            //    grid.TopRow = 0;
            //}
            //finally
            //{
            //    grid.EndUpdate();
            //}
        }
        #endregion
        void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            clear();
            checkText.checkTextIsEmpty(this.StateNameTextBox.Text);
            SaerchByKey = new ServiceSoapClient();
            SaerchByKey.SearchMarketBykeyAsync(this.StateNameTextBox.Text);
            SaerchByKey.SearchMarketBykeyCompleted += new EventHandler<SearchMarketBykeyCompletedEventArgs>(SaerchByKey_SearchMarketBykeyCompleted);
            //QueryTask queryTask =
            //    new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Census_USA/MapServer/5");
            //queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            //queryTask.Failed += QueryTask_Failed;

            //ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query();
            //query.Text = StateNameTextBox.Text;

            //query.OutFields.Add("*");
            //queryTask.ExecuteAsync(query);
        }

        void SaerchByKey_SearchMarketBykeyCompleted(object sender, SearchMarketBykeyCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string tmp = e.Result;
                if (tmp == null)
                {
                    MessageBox.Show("查询失败！");
                    return;
                }
                searchRlt = new List<marketsInfo>();
                if (e.Result != "无信息！")
                {
                    searchRlt = XMLSerialization.Desrialize(searchRlt, e.Result);
                    if (searchRlt.Count <= 0)
                    {
                        MessageBox.Show("数据库无相关信息！");
                        return;
                    }
                    grid.DataSource = searchRlt;
                    grid.AllowEditing = false;
                    Marks.markLayer1 = this.markLayer;
                    List<Point> Pnts = new List<Point>();
                    for (int i = 0; i < searchRlt.Count; i++)
                    {
                        Point pnt = new Point(searchRlt[i].X, searchRlt[i].Y);
                        Pnts.Add(pnt);
                    }
                    Marks.PntList = Pnts;
                    Marks.markets = searchRlt;
                    Marks.mapContainer = this.mapContainer;
                    shopMarks = Marks.AddMarks();
                }
                else
                {
                    MessageBox.Show("没有相关门店");
                }
            }
        }
        /// <summary>
        /// 鼠标移入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Location_MouseEnter(object sender, MouseEventArgs e)
        {
            Image send = sender as Image;
            send.Width = 50;
            send.Height = 50;
            send.Cursor = Cursors.Hand;
        }
        /// <summary>
        /// 鼠标移出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Location_MouseLeave(object sender, MouseEventArgs e)
        {
            Image send = sender as Image;
            send.Width = 40;
            send.Height = 40;
            send.Cursor = Cursors.None;
        }
        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Location_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mapContainer == null)
            {
                MessageBox.Show("地图容器为空！");
                return;
            }
            this.mapContainer.SetCenter(this.searchRlt[index].X, this.searchRlt[index].Y);
            this.mapContainer.Refresh();
        }

        private void detail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (form == null)
            {
                form = new DetailsForms();
            }
            form.MarketID = this.searchRlt[index].MarketID;
            form.Show();
        }

        private void detail_MouseEnter(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            l.Foreground = new SolidColorBrush(Colors.Blue);//设置文本颜色
        }

        private void detail_MouseLeave(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            l.Foreground = new SolidColorBrush(Colors.Black);//设置文本颜色
        }

        private void StateNameTextBox_TextInputStart(object sender, TextCompositionEventArgs e)
        {
            this.StateNameTextBox.Text = "";
        }



    }
}
