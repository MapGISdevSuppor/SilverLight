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
using Logistics_system.chart1;
using ZDIMS.BaseLib;
using Logistics_system.ServiceClient;
namespace Logistics_system
{
    public partial class DetailsForms : BaseUserControl
    {
        bool Isedit = false;
        bool IsFinancial = true;
        ServiceSoapClient ServiceClient = new ServiceSoapClient();
        getProperties properties = null;
        /// <summary>
        /// 财务数据
        /// </summary>
        public List<FinancialInfo> _fincials;
        /// <summary>
        /// 货物数据
        /// </summary>
        public List<prodectsInfo> _prodects;
        /// <summary>
        /// 查询范围
        /// </summary>
        public Circle circle;
        /// <summary>
        /// 门店ID
        /// </summary>
        public int MarketID;
        List<prodectsInfo> uppPData;
        List<FinancialInfo> uppFData = null;
        ChartControl chart = null;
        /// <summary>
        /// 不同门店信息统计
        /// </summary>
        public bool IsStaticAnalyse = false;
        public DetailsForms()
        {
            InitializeComponent();
            this.dialog.OnClose += new RoutedEventHandler(close);
            this.grid.SelectionMode = DevExpress.AgDataGrid.DataGridSelectionMode.None;

            //this.grid.RowEdited += new DevExpress.AgDataGrid.Data.RowEditedEventHandler(grid_RowEdited);
            this.grid.FocusedRowChanged += new DevExpress.AgDataGrid.Data.FocusedRowChangedEventHandler(grid_FocusedRowChanged);
        }



        private void close(object sender, RoutedEventArgs e)
        {
            this.grid.Columns.Clear();
            this.financialRb.IsChecked = false;
            this.prodectRb.IsChecked = false;
            if (this._prodects != null)
            {
                _prodects.Clear();
            }
            if (this._fincials != null)
            {
                _fincials.Clear();
            }
            if (uppFData != null)
            {
                uppPData.Clear();
            }
            if (uppPData != null)
            {

                uppFData.Clear();
            }
            this.Close();
        }
        private List<int> FInx = new List<int>();
        private List<int> PInx = new List<int>();
        /// <summary>
        /// 编辑完获取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grid_FocusedRowChanged(object sender, DevExpress.AgDataGrid.Data.FocusedRowChangedEventArgs e)
        {
            int index = e.NewRowHandle;
            if (index < 0)
            {
                return;
            }
            bool s = false;
            if (IsFinancial)
            {
                for (int i = 0; i < FInx.Count; i++)
                {
                    if (FInx[i] == index)
                    {
                        s = true;
                        break;
                    }

                }

                if (!s)
                {
                    FInx.Add(index);

                }
            }
            else
            {
                for (int i = 0; i < PInx.Count; i++)
                {
                    if (PInx[i] == index)
                    {
                        s = true;
                        break;
                    }

                }

                if (!s)
                {
                    PInx.Add(index);

                }
            }

            //if (Isedit && IsFinancial)
            //{
            //    FinancialInfo f = new FinancialInfo();
            //    f = e.OldDataRow as FinancialInfo;
            //    if (_fincials[index] != f)
            //    {
            //        uppFData.Add(f);
            //    }
            //}
            //if (Isedit && !IsFinancial)
            //{
            //    prodectsInfo p = new prodectsInfo();
            //    p = e.OldDataRow as prodectsInfo;
            //    if (_prodects[index] != p)
            //    {
            //        uppPData.Add(p);
            //    }
            //}


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
            //this.grid.Columns.Add(textColumn);
            textColumn = new AgDataGridTextColumn();

            textColumn.FieldName = "MarketName";
            textColumn.HeaderContent = "门店名称";
            textColumn.AllowEditing = DefaultBoolean.False;
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
            grid.DataSource = _fincials;
            this.grid.FocusedRowVisibleIndex = -1;
        }
        /// <summary>
        /// 选择显示财务
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
            if (IsStaticAnalyse)
            {
                GetInfo(circle);
            }
            else
            {
                this.static1.Visibility = System.Windows.Visibility.Collapsed;
                ServiceClient.SearchDetailsByIDAsync(this.MarketID, "Financial");
                ServiceClient.SearchDetailsByIDCompleted += new EventHandler<SearchDetailsByIDCompletedEventArgs>(SearchByID_SearchProdectsByIDCompleted);
            }


        }
        /// <summary>
        /// ID查询结果处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchByID_SearchProdectsByIDCompleted(object sender, SearchDetailsByIDCompletedEventArgs e)
        {
            ServiceClient.SearchDetailsByIDCompleted -= SearchByID_SearchProdectsByIDCompleted;
            if (e.Error == null)
            {
                string tmp = e.Result;
                if (tmp == "")
                {
                    MessageBox.Show("查询失败！");
                    return;
                }
                if (tmp != "无信息")
                {
                    if (IsFinancial)
                    {

                        _fincials = new List<FinancialInfo>();
                        _fincials = XMLSerialization.Desrialize(_fincials, tmp);
                        if (_fincials.Count <= 0)
                        {
                            MessageBox.Show("无相关信息！");
                            return;
                        }
                        showfinancial();
                    }
                    else
                    {
                        _prodects = new List<prodectsInfo>();
                        _prodects = XMLSerialization.Desrialize(_prodects, tmp);
                        if (_prodects.Count <= 0)
                        {
                            MessageBox.Show("无相关信息！");
                            return;
                        }
                        showProdects();
                    }
                }
            }
        }
        /// <summary>
        /// 画圆查询数据库
        /// </summary>
        /// <param name="key"></param>
        private void GetInfo(Circle key)
        {
            if (IsFinancial)
            {
                if (_fincials.Count <= 0)
                {
                    //查数据库
                }
            }
            else
            {
                if (_prodects.Count <= 0)
                {
                    //查数据库
                }

            }


        }
        /// <summary>
        /// 选择显示货物
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
            this.static1.Visibility = System.Windows.Visibility.Visible;
            ServiceClient.SearchDetailsByIDAsync(this.MarketID, "prodectsInfo");
            ServiceClient.SearchDetailsByIDCompleted += new EventHandler<SearchDetailsByIDCompletedEventArgs>(SearchByID_SearchProdectsByIDCompleted);


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
            //this.grid.Columns.Add(textColumn);

            textColumn = new AgDataGridTextColumn();
            textColumn.AllowEditing = DefaultBoolean.False;
            textColumn.FieldName = "MarketName";
            textColumn.HeaderContent = "门店名称";
          
            this.grid.Columns.Add(textColumn);

            textColumn = new AgDataGridTextColumn();
            textColumn.FieldName = "ProdectID";
            textColumn.HeaderContent = "货物ID";
            //this.grid.Columns.Add(textColumn);

            textColumn = new AgDataGridTextColumn();
            textColumn.FieldName = "ProdectName";
            textColumn.AllowEditing = DefaultBoolean.False;
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
            if (!IsStaticAnalyse)
            {
                textColumn.FieldName = "Price";
                textColumn.HeaderContent = "单价";
                this.grid.Columns.Add(textColumn);
                textColumn = new AgDataGridTextColumn();



                textColumn.FieldName = "InerPrice";
                textColumn.HeaderContent = "批发价";
                this.grid.Columns.Add(textColumn);
                textColumn = new AgDataGridTextColumn();

                textColumn.FieldName = "PreNum";
                textColumn.HeaderContent = "订货量";
                this.grid.Columns.Add(textColumn);
                textColumn = new AgDataGridTextColumn();

                textColumn.FieldName = "ReceNum";
                textColumn.HeaderContent = "到货量";
                this.grid.Columns.Add(textColumn);

            }
            this.grid.DataSource = _prodects;
            this.grid.FocusedRowVisibleIndex = -1;
        }
        /// <summary>
        /// 统计信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void static_Click(object sender, RoutedEventArgs e)
        {
            List<string> Attributes = null;
            if (chart == null)
            {
                chart = new ChartControl();

            }
            if (properties == null)
            {
                properties = new getProperties();
            }


            if (IsFinancial)
            {
                MessageBox.Show("财务没有统计信息");
                return;
                //Attributes = new List<string>();
                //Attributes = dataBing.financialXStr();//properties.getproperty(_prodects[0]);
                //chart.setXDataProvider(Attributes);
                //Attributes.Clear();
                //Attributes = dataBing.financialYStr1();
                //chart.setYDataProvider(Attributes);
                //chart.IsProdects = false;
                //chart.financialsInfo = _fincials;

            }
            else
            {
                if (_prodects != null && _prodects.Count > 0)
                {
                    Attributes = new List<string>();
                    Attributes = dataBing.prodectsXStr();//properties.getproperty(_prodects[0]);
                    List<string> YAttributes = dataBing.prodectsYStr();
                    chart.setDataProvider(Attributes, YAttributes);
                    chart.IsProdects = true;
                    chart.prodectsInfo = _prodects;
                    chart.Show();
                }
                else
                {
                    MessageBox.Show("货物没有统计的信息");
                }
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (!this.grid.IsEditorShown)
            {
                uppPData = new List<prodectsInfo>();
                uppFData = new List<FinancialInfo>();
                Isedit = true;
               
                this.grid.AllowEditing = true;
                this.grid.Refresh();
            }
            else
            {
                Isedit = false;              
                this.grid.AllowEditing = false;
            }
        }
        /// <summary>
        ///保存更新到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Savebutton_Click(object sender, RoutedEventArgs e)
        {
            string StrUpp;
            if (Isedit)
            {
                if (IsFinancial)
                {
                    for (int i = 0; i < FInx.Count; i++)
                    {
                        FinancialInfo f = new FinancialInfo();
                        f = this.grid.GetDataRow(FInx[i]) as FinancialInfo;
                        uppFData.Add(f);
                    }
                    if (uppFData != null && uppFData.Count != 0)
                    {
                        StrUpp = XMLSerialization.Serialize(uppFData);
                        ServiceClient.UppdateFinancialAsync(StrUpp);
                        uppFData.Clear();
                        ServiceClient.UppdateFinancialCompleted += new EventHandler<UppdateFinancialCompletedEventArgs>(ServiceClient_UppdateFinancialCompleted);
                    }
                }
                else
                {
                    for (int i = 0; i < PInx.Count; i++)
                    {
                        prodectsInfo f = new prodectsInfo();
                        f = this.grid.GetDataRow(PInx[i]) as prodectsInfo;
                        uppPData.Add(f);
                    }
                    if (uppPData != null && uppPData.Count != 0)
                    {
                        StrUpp = XMLSerialization.Serialize(uppPData);
                        ServiceClient.UppdateprodectAsync(StrUpp);
                        uppPData.Clear();
                        ServiceClient.UppdateprodectCompleted += new EventHandler<UppdateprodectCompletedEventArgs>(ServiceClient_UppdateprodectCompleted);
                    }
                }
            }
        }

        void ServiceClient_UppdateprodectCompleted(object sender, UppdateprodectCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string rlt = e.Result;
                MessageBox.Show(rlt);
            }
        }

        void ServiceClient_UppdateFinancialCompleted(object sender, UppdateFinancialCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string rlt = e.Result;
                MessageBox.Show(rlt);
            }
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsStaticAnalyse)
            {
                this.edit.IsEnabled = false;
            }
            else
            {
                this.edit.IsEnabled = true;
            }
        }
        /// <summary>
        /// 清理上次结果
        /// </summary>
        public void ClearValue()
        {
            if (this._fincials != null)
            {
                FInx.Clear();
                this._fincials.Clear();
            }
            if (this._prodects != null)
            {
                PInx.Clear();
                this._prodects.Clear();
            }
            this.grid.Columns.Clear();
        }
    }
}
