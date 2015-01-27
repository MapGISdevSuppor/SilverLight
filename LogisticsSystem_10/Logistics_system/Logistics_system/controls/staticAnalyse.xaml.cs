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
using ZDIMSDemo.Controls;
using DevExpress.AgDataGrid;
using Logistics_system.chart1;
using ZDIMS.BaseLib;
using ZDIMS.Interface;
using ZDIMS.Drawing;
using ZDIMS.Map;
namespace Logistics_system.controls
{
    public partial class staticAnalyse : BaseUserControl
    {


        bool IsFinancial = true;
        ServiceSoapClient ServiceClient = new ServiceSoapClient();
        getProperties properties = null;
        List<marketsInfo> MarketList;
        List<IMSMark> addMarkList;
        prodectChart chart;
        string proName = "";
        int SelectIndex;
        /// <summary>
        /// 财务数据
        /// </summary>
        public List<FinancialInfo> _fincials;
        /// <summary>
        /// 货物数据
        /// </summary>
        public List<prodectsInfo> _prodects;
        public MarkLayer markLayer;
        private bool first = false;
        /// <summary>
        /// 查询范围
        /// </summary>
        public Circle circle;
        public IMSMap mapContainer;
        public staticAnalyse()
        {
            InitializeComponent();
            this.grid.FocusedRowChanged += new DevExpress.AgDataGrid.Data.FocusedRowChangedEventHandler(grid_FocusedRowChanged);
            this.dialog.OnClose += new RoutedEventHandler(close);
        }
        /// <summary>
        /// 选择某行，统计对应列的货物或财务信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grid_FocusedRowChanged(object sender, DevExpress.AgDataGrid.Data.FocusedRowChangedEventArgs e)
        {

            SelectIndex = e.NewRowHandle;
            if (this.SelectIndex < 0)
            {
                return;
            }
            if (!IsFinancial)
            {
                proName = (e.NewDataRow as prodectsInfo).ProdectName;
            }
        }

        private void close(object sender, RoutedEventArgs e)
        {
            this.grid.Columns.Clear();
            this.financialRb.IsChecked = false;
            this.prodectRb.IsChecked = false;
            this.proName = "";
            if (addMarkList.Count >= 0)
            {
                for (int i = 0; i < addMarkList.Count; i++)
                {
                    this.markLayer.RemoveMark(addMarkList[i]);
                }
            }
            if (this._prodects != null)
            {
                _prodects.Clear();
            }
            if (this._fincials != null)
            {
                _fincials.Clear();
            }

            this.Close();
        }
        /// <summary>
        /// 显示财务
        /// </summary>
        private void showfinancial()
        {

            AgDataGridTextColumn textColumn;
            textColumn = new AgDataGridTextColumn();
            // textColumn.AllowEditing = DefaultBoolean.False;
            textColumn.FieldName = "MarketID";
            textColumn.HeaderContent = "门店ID";
            this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();

            textColumn.FieldName = "MarketName";
            textColumn.HeaderContent = "门店名称";
            this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();

            textColumn.FieldName = "totalAmount";
            textColumn.HeaderContent = "总金额";
            this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();

            textColumn.FieldName = "SaleAmount";
            textColumn.HeaderContent = "销售额";
            this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();

            textColumn.FieldName = "InerAmount";
            textColumn.HeaderContent = "进货金额";
            this.grid.Columns.Add(textColumn);
            this.grid.AllowEditing = false;
            if (_fincials == null || _fincials.Count <= 0)
            {
                MessageBox.Show("无财务信息！");
                return;
            }
            grid.DataSource = _fincials;
        }

        /// <summary>
        /// 显示货物
        /// </summary>
        private void showProdects()
        {
            //prodectsInfo Pro_Data = new prodectsInfo();
            AgDataGridTextColumn textColumn;
            textColumn = new AgDataGridTextColumn();
            textColumn.AllowEditing = DefaultBoolean.False;
            textColumn.FieldName = "MarketID";
            textColumn.HeaderContent = "门店ID";
            this.grid.Columns.Add(textColumn);


            textColumn = new AgDataGridTextColumn();
            textColumn.AllowEditing = DefaultBoolean.False;
            textColumn.FieldName = "MarketName";
            textColumn.HeaderContent = "门店名称";
            this.grid.Columns.Add(textColumn);

            textColumn = new AgDataGridTextColumn();
            textColumn.FieldName = "ProdectID";
            textColumn.HeaderContent = "货物ID";
            this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();

            textColumn.FieldName = "ProdectName";
            textColumn.HeaderContent = "名称";
            this.grid.Columns.Add(textColumn);

            textColumn = new AgDataGridTextColumn();
            textColumn.FieldName = "SaleNum";
            textColumn.HeaderContent = "销量";
            this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();

            textColumn.FieldName = "InstocksNum";
            textColumn.HeaderContent = "库存";
            this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();
            #region 废弃
            //if (!IsStaticAnalyse)
            //{
            //    textColumn.FieldName = "Price";
            //    textColumn.HeaderContent = "单价";
            //    this.grid.Columns.Add(textColumn);
            //    textColumn = new AgDataGridTextColumn();



            //    textColumn.FieldName = "InerPrice";
            //    textColumn.HeaderContent = "批发价";
            //    this.grid.Columns.Add(textColumn);
            //    textColumn = new AgDataGridTextColumn();

            //    textColumn.FieldName = "PreNum";
            //    textColumn.HeaderContent = "订货量";
            //    this.grid.Columns.Add(textColumn);
            //    textColumn = new AgDataGridTextColumn();

            //    textColumn.FieldName = "ReceNum";
            //    textColumn.HeaderContent = "到货量";
            //    this.grid.Columns.Add(textColumn);

            //}
            #endregion
            if (_prodects == null || _prodects.Count <= 0)
            {
                MessageBox.Show("无货物信息！");
                return;
            }
            this.grid.DataSource = _prodects;
            this.grid.FocusedRowVisibleIndex = -1;

        }

        /// <summary>
        /// 画圆查询数据库
        /// </summary>
        /// <param name="key"></param>
        public void GetInfo(Circle key)
        {
            first = true;
            ServiceClient.SearchByCircleAsync(key.Center.x, key.Center.y, key.Radius);
            ServiceClient.SearchByCircleCompleted += new EventHandler<SearchByCircleCompletedEventArgs>(ServiceClient_SsarchByCircleCompleted);

        }
        /// <summary>
        /// 圆查询回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServiceClient_SsarchByCircleCompleted(object sender, SearchByCircleCompletedEventArgs e)
        {
            if (first)
            {
                first = false;
                if (e.Error == null)
                {

                    if (e.Result != "")
                    {
                        string tmp = e.Result;
                        string[] str = tmp.Split(';');
                        if (str[0] == "无信息！")
                        {
                            MessageBox.Show("所选区域没有门店");
                            return;
                        }
                        else
                        {
                            MarketList = XMLSerialization.Desrialize(MarketList, str[0]);
                            this.markLayer.RemoveAll();
                            addMarkList = new List<IMSMark>();
                            addMarks marks = new addMarks();
                            marks.markLayer1 = this.markLayer;
                            marks.markets = MarketList;
                            marks.mapContainer = this.mapContainer;
                            addMarkList = marks.AddMarks();
                            if (str[1] != "无信息")
                            {
                                _prodects = XMLSerialization.Desrialize(_prodects, str[1]);
                            }

                            if (str[2] != "无信息")
                            {
                                _fincials = XMLSerialization.Desrialize(_fincials, str[2]);

                            }
                            this.Show();
                        }
                    }

                }

            }

        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void static_Click(object sender, RoutedEventArgs e)
        {
            bool f=(bool)this.financialRb.IsChecked ;
            bool p=(bool) this.prodectRb.IsChecked;
            if (f || p)
            {
                List<string> Attributes = null;
                if (chart == null)
                {
                    chart = new prodectChart();
                }
                if (properties == null)
                {
                    properties = new getProperties();
                }
                if (IsFinancial)
                {
                    if (_fincials != null && _fincials.Count > 0)
                    {
                        Attributes = new List<string>();
                        Attributes = dataBing.financialYStr1();
                        chart.IsFinancial = true;
                        chart.Name = "财务信息";
                        chart.financialsInfo = _fincials;
                    }
                    else
                    {
                        MessageBox.Show("财务没有统计信息");
                        return;
                    }
                }
                else
                {
                    if (_prodects != null && _prodects.Count > 0)
                    {
                        if (this.proName == "")
                        {
                            MessageBox.Show("请选择要统计的货物名称");
                            return;
                        }
                        Attributes = new List<string>();
                        Attributes = dataBing.prodectsStr1();
                        chart.IsFinancial = false;
                        chart.Name = this.proName;
                        chart.prodectsInfo = _prodects;
                    }
                    else
                    {
                        MessageBox.Show("货物没有统计信息");
                        return;
                    }
                }
                chart.setDataProvider(Attributes);
                chart.Show();
            }
            else
            {
                MessageBox.Show("未选则要统计的类型（财务、货物统计）");
            }
        }
        /// <summary>
        /// 清理上次结果
        /// </summary>
        public void ClearValue()
        {
            if (this._fincials != null)
            {
                this._prodects.Clear();
            }
            if (this._prodects != null)
            {
                this._fincials.Clear();
            }
            if (this.grid.Columns.Count > 0)
            {
                this.grid.Columns.Clear();
            }
        }
        /// <summary>
        /// 财务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void financial_Checked(object sender, RoutedEventArgs e)
        {
            if (this.grid.Columns.Count > 0)
            {
                this.grid.Columns.Clear();
            }
            IsFinancial = true;
            showfinancial();
        }
        /// <summary>
        /// 货物
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prodects_Checked(object sender, RoutedEventArgs e)
        {
            if (this.grid.Columns.Count > 0)
            {
                this.grid.Columns.Clear();
            }
            IsFinancial = false;
            showProdects();
        }
        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.grid.AllowEditing = false;
        }
    }
}
