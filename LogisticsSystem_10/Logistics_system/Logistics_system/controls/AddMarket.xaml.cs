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
using Logistics_system;
using System.Collections.ObjectModel;
using Logistics_system.ServiceClient;
using ZDIMS.Map;
using ZDIMS.Drawing;
using ZDIMS.Interface;

using ZDIMS.Util;
namespace Logistics_system.controls
{
    public partial class AddMarket : BaseUserControl
    {
        
        
        private List<marketsInfo> m_marketsInfos = null;
        private List<CenterSInfo> m_centersInfos = null;
        private List<FinancialInfo> m_FinancialInfos=null;
        private List<prodectsInfo> m_prodectsInfos = null;
        private marketsInfo _marketsInfo= null;
        private CenterSInfo _centersInfo = null;
        private FinancialInfo _FinancialInfo = null;
        private prodectsInfo _prodectsInfo = null;
        private checkTextLegality _checkText = null;
        public IMSMark tmp;
        private addMarks AddMarkControl;
        private int  flag=-1;
        ServiceSoapClient ServiceClient = new ServiceSoapClient();
        public Point markPnt;
        public IMSMap mapContainer;
        public List<marketsInfo> marketsInfolist
        {
            get { return m_marketsInfos; }
            set { m_marketsInfos = value; }

        }
        public List<CenterSInfo> CenterSInfolist
        {
            get { return m_centersInfos; }
            set { m_centersInfos = value; }

        }

        public List<FinancialInfo> FinancialInfolist
        {
            get { return m_FinancialInfos; }
            set { m_FinancialInfos = value; }
        }
        public List<prodectsInfo> prodectsInfolist
        {
            get { return m_prodectsInfos; }
            set { m_prodectsInfos = value; }
        }
        public MarkLayer markLayer;
        public AddMarket()
        {
            InitializeComponent();
            this.dialog.OnClose += new RoutedEventHandler(close);
            _checkText = new checkTextLegality();
           
        }
        public void close(object sender, RoutedEventArgs e)
        {
            
            this.isRadio.IsChecked = false;
            this.NoRadio.IsChecked = false;
            this.centerID0.IsEnabled = true;
            this.tabitem4.IsEnabled = true;
            this.tabitem3.IsEnabled = true;
            this.tabitem2.IsEnabled = true;
            this.ImportMoney.IsEnabled = true; ;
            this.SaledMoney.IsEnabled = true;
            this.markLayer.RemoveAll();
            this.Close();
        }
    
     
        /// <summary>
        /// 设置门店信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set_market_Click(object sender, RoutedEventArgs e)
        {

            if (!_checkText.checkNumIsEmpty(this.marketID0.Text))
            
            {
                return;
            }
            if (!_checkText.checkTextIsEmpty(this.MarketName0.Text))
            {
                return;
            }
            if (!_checkText.checkTextIsEmpty(this.address0.Text))
            {
                return;
            }
            if (this.centerID0.IsEnabled)
            {
                if (!_checkText.checkNumIsEmpty(this.centerID0.SelectedItem.ToString()))
                {
                    return;
                }
                _marketsInfo.CenterID = Convert.ToInt32(this.centerID0.SelectedItem.ToString());
            }
            else
            {
                _marketsInfo.CenterID = -1;
            }
            _marketsInfo.MarketID = Convert.ToInt32(this.marketID0.Text);         
            _marketsInfo.MarketName = this.MarketName0.Text;
            _marketsInfo.Address = this.address0.Text;
            _marketsInfo.X = markPnt.X;
            _marketsInfo.Y = markPnt.Y;
            this.marketID1.Text = this.marketID0.Text;
            
            this.marketID2.Text = this.marketID0.Text;
            this.centerID3.Text = this.marketID0.Text;
            this.centerName3.Text=this.MarketName2.Text= this.marketName1.Text = this.MarketName0.Text;     
            string addmarket=XMLSerialization.Serialize(_marketsInfo);
            addInfo("marketsInfo", addmarket);
            //ServiceClient.addData_marketsInfoAsync(_marketsInfo);
            //ServiceClient += new EventHandler<addDataCompletedEventArgs>(ServiceClient_addDataCompleted);        
        }

      
        /// <summary>
        /// 财务信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set_finaicnal_Click(object sender, RoutedEventArgs e)
        {
            if(_FinancialInfo==null)
            {
                _FinancialInfo=new FinancialInfo();
            }
            if (!_checkText.checkNumIsEmpty(this.marketID1.Text))
            {
                return;
            }

            if (!_checkText.checkNumIsEmpty(this.unitmoney.Text))
            {
                return;
            }
            if (!_checkText.checkTextIsEmpty(this.marketName1.Text))
            {
                return;
            }
            if (flag == 0)
            {
                if (!_checkText.checkNumIsEmpty(this.ImportMoney.Text))
                {
                    return;
                }
                if (!_checkText.checkNumIsEmpty(this.SaledMoney.Text))
                { return; }
                _FinancialInfo.InerAmount = Convert.ToDouble(this.ImportMoney.Text);
                _FinancialInfo.SaleAmount = Convert.ToDouble(this.SaledMoney.Text);
            }
            _FinancialInfo.MarketID = Convert.ToInt32(this.marketID1.Text);

            _FinancialInfo.totalAmount = Convert.ToDouble(this.unitmoney.Text);
            _FinancialInfo.MarketName = this.marketName1.Text;
            string addFinancial = XMLSerialization.Serialize(_FinancialInfo);
            addInfo("Financial", addFinancial);
        }
        string Type = "";
        public void addInfo(string type, string data)
        {
            Type = type;
            ServiceClient.addDataAsync(type, data);

            ServiceClient.addDataCompleted += new EventHandler<addDataCompletedEventArgs>(ServiceClient_addDataCompleted);
        }

        void ServiceClient_addDataCompleted(object sender, addDataCompletedEventArgs e)
        {
            ServiceClient.addDataCompleted -= ServiceClient_addDataCompleted;
            if (e.Error == null)
            {
                MessageBox.Show(e.Result );
                if (Type == "marketsInfo")
                {
                  //移除临时标注
                   bool f = this.markLayer.RemoveMark(tmp);
                   
                    if (AddMarkControl == null)
                    {
                        AddMarkControl = new addMarks();
                        AddMarkControl.markLayer1 = this.markLayer;
                        AddMarkControl.mapContainer = this.mapContainer;
                    }                 
                   tmp= AddMarkControl.AddMark(_marketsInfo);
                   this.markLayer.MapContainer.Refresh();
                }
            }
        }
        /// <summary>
        /// 中心店信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_centersInfo == null)
            {
                _centersInfo = new CenterSInfo();
            }

            if (!_checkText.checkNumIsEmpty(this.centerID3.Text))
            { return; }
            if (!_checkText.checkNumIsEmpty(this.ProdectID3.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.StocksNum3.Text))
                return;
            if (!_checkText.checkTextIsEmpty(this.ProdectName3.Text))
                return;
            _centersInfo.MarketName = this.centerName3.Text;
            _centersInfo.CenterID = Convert.ToInt32(this.centerID3.Text);
            _centersInfo.prodectID = Convert.ToInt32(this.ProdectID3.Text);
            _centersInfo.prodectNum = Convert.ToInt32(this.StocksNum3.Text);
            _centersInfo.prodectName = this.ProdectName3.Text;
            string data = XMLSerialization.Serialize(_centersInfo);
            addInfo("CenterInfo", data);
        }
        /// <summary>
        /// 货物信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProdectInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!_checkText.checkNumIsEmpty(this.marketID2.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.prodectID2.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.Price2.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.InstockNum.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.SaleNum.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.PreNum2.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.ReceNum2.Text))
                return;
            if (!_checkText.checkNumIsEmpty(this.inerPrice.Text))
                return;
            if (!_checkText.checkTextIsEmpty(this.prodectName2.Text))
                return;
            if (!_checkText.checkTextIsEmpty(this.address0.Text))
                return;
            if (_prodectsInfo == null)
            {
                _prodectsInfo = new prodectsInfo();
            }
            _prodectsInfo.MarketName = this.MarketName2.Text;
            _prodectsInfo.MarketID = Convert.ToInt32(this.marketID2.Text);
            _prodectsInfo.ProdectID = Convert.ToInt32(this.prodectID2.Text);
            _prodectsInfo.ProdectName = this.prodectName2.Text;
            _prodectsInfo.Price = Convert.ToDouble(this.Price2.Text);
            _prodectsInfo.SaleNum = Convert.ToInt32(this.SaleNum.Text);
            _prodectsInfo.InstocksNum = Convert.ToInt32(this. InstockNum.Text);
            _prodectsInfo.PreNum = Convert.ToInt32(this.PreNum2.Text);
            _prodectsInfo.ReceNum = Convert.ToInt32(this.ReceNum2.Text);
            this._prodectsInfo.InerPrice = Convert.ToDouble(this.inerPrice.Text);

            string data = XMLSerialization.Serialize(_prodectsInfo);
            addInfo("prodectsInfo", data);
        }
        /// <summary>
        /// 是总店:1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void is_checked(object sender, RoutedEventArgs e)
        {
            if (_marketsInfo == null)
            {
                _marketsInfo = new marketsInfo();
            }
            _marketsInfo.CenterFlag = 1;
            flag = 1;
            this.ImportMoney.IsEnabled = false;
            this.SaledMoney.IsEnabled = false;
            this.centerID0.IsEnabled = false;
            this.tabitem3.IsEnabled = false;
        }
        /// <summary>
        /// 分店：0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void No_checked(object sender, RoutedEventArgs e)
        {
            if (_marketsInfo == null)
            {
                _marketsInfo = new marketsInfo();
            }
            _marketsInfo.CenterFlag = 0;
            setCenterID();
            flag = 0;
            this.centerID0.IsEnabled = true;
            this.tabitem4.IsEnabled = false;
        }
        private void setCenterID()
        {
            this.centerID0.Items.Add(25);
            //this.centerID0.Items.Add(8);
            this.centerID0.Items.Add(15);
            this.centerID0.SelectedIndex = -1;
        }

        private void marketID0_KeyDown(object sender, KeyEventArgs e)
        {
            this.marketID0.Text = "";
           // this.marketID0.TextInputStart += new TextCompositionEventHandler(marketID0_TextInputStart);

        }

        void marketID0_TextInputStart(object sender, TextCompositionEventArgs e)
        {
            this.marketID0.Text = "";
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
