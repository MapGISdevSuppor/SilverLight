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
using Visifire.Charts;
using Visifire.Gauges;
using Visifire.Commons;
namespace Logistics_system.chart1
{
    public partial class prodectChart : BaseUserControl
    {
        private getProperties property;
        private DataPointCollection _dynamicData = null;
        public List<FinancialInfo> financialsInfo;
        public List<prodectsInfo> prodectsInfo;
        public List<marketsInfo> markdInfo;
        getProperties properties = null;
        List<string> Attributes = null;
        public bool IsFinancial = false;
        private string _ChartType = "Columnar";
        public string Name
        {
            get
            {
                return this.proName.Text;
            }
            set
            {
                this.proName.Text = value;
            }
        }

        public prodectChart()
        {
            InitializeComponent();
            this.dialogPanel1.OnClose += new RoutedEventHandler(Close);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            try
            {
                _ChartType = rb.Tag.ToString();
            }
            catch
            {
            }
        }

        private void closeBtn_clickHandler(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void initChart()
        {
            ////if (properties == null)
            ////{
            ////    properties = new getProperties();
            ////}
            ////if (IsFinancial)
            ////{
            ////    Attributes = new List<string>();
            ////    Attributes = dataBing.financialStr(); // properties.getproperty(financialsInfo[0]);


            ////}
            ////else
            ////{
            ////    Attributes = new List<string>();
            ////    Attributes = dataBing.prodectsStr();// properties.getproperty(prodectsInfo[0]);

            ////}
        }

        /**
       * 绑定数据
       */
        public void setDataProvider(List<string> property)
        {
            

            this.yaxis.ItemsSource = property;
            this.yaxis.SelectedIndex = 0;
            this.mychart.Series.Clear();
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okBtn_clickHandler(object sender, RoutedEventArgs e)
        {
            this.mychart.Series.Clear();
            _dynamicData = new DataPointCollection();
            string Xa = "MarketName";
            string Ya = "";// this.yaxis.SelectedItem.ToString();
            property = new getProperties();
            DataSeries dataS = new DataSeries();
            if (IsFinancial)
            {
                Ya = dataBing.getSelectItem(financialsInfo[0], this.yaxis.SelectedIndex+2);
                for (int i = 0; i < financialsInfo.Count; i++)
                {
                    dataS.DataPoints.Add(property.getDynamic(financialsInfo[i], Xa, Ya));
                }
            }
            else
            {
                Ya = dataBing.getSelectItem(prodectsInfo[0], this.yaxis.SelectedIndex);
                for (int i = 0; i < prodectsInfo.Count; i++)
                {
                    if (prodectsInfo[i].ProdectName == this.Name)
                    {
                        dataS.DataPoints.Add(property.getDynamic(prodectsInfo[i], Xa, Ya));
                    }
                }
            }
            switch (_ChartType)
            {
                case "Columnar":
                    {
                        dataS.RenderAs = RenderAs.Column;
                        break;
                    }
                case "Pie":
                    {
                        dataS.RenderAs = RenderAs.Pie;
                        break;
                    }
            }

            this.mychart.Series.Add(dataS);

        }


    }
}
