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
    public delegate void ChooseOkEvevt(string chartType);
    public partial class ChartControl : BaseUserControl
    {
        private getProperties property;
        private string _ChartType = "Columnar";
        public ChooseOkEvevt ChartTypeCallBack;
        private DataPointCollection _dynamicData = null;
      // Visifire.Charts. Chart mychart = new Visifire.Charts.Chart();
       
        public List<FinancialInfo>  financialsInfo;
        public List<prodectsInfo> prodectsInfo;
        public bool IsProdects = false;
        
        public ChartControl()
        {
            InitializeComponent();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
         

            //setDataProvider(d);
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
        private void okBtn_clickHandler(object sender, RoutedEventArgs e)
        {
            this.mychart.Series.Clear();
            _dynamicData = new DataPointCollection();
            string Xa = "";// this.xaxis.SelectedItem.ToString();
            string Ya = "";// this.yaxis.SelectedItem.ToString();
            
            property = new getProperties();
            DataSeries dataS = new DataSeries();
            if (!IsProdects)
            {
                Xa =  dataBing. getSelectItem(financialsInfo[0], this.xaxis.SelectedIndex);
                Ya = dataBing. getSelectItem(financialsInfo[0], this.yaxis.SelectedIndex);
                for (int i = 0; i < financialsInfo.Count; i++)
                {
                    dataS.DataPoints.Add(property.getDynamic(financialsInfo[i], Xa, Ya));
                }
            }
            else
            {
                Xa = dataBing.getSelectItem(prodectsInfo[0], this.xaxis.SelectedIndex+7);
                Ya = dataBing.getSelectItem(prodectsInfo[0], this.yaxis.SelectedIndex);
                for (int i = 0; i < prodectsInfo.Count; i++)
                {
                    dataS.DataPoints.Add(property.getDynamic(prodectsInfo[i], Xa, Ya));
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
        private void closeBtn_clickHandler(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /**
       * 绑定数据
       */
        public void setDataProvider(List<string> Xproperty, List<string> Yproperty)
        {
            this.xaxis.ItemsSource = Xproperty;
            this.xaxis.SelectedIndex = 0;
            this.yaxis.ItemsSource = Yproperty;
            this.yaxis.SelectedIndex = 0;
            if(this.mychart!=null)
            this.mychart.Series.Clear();
        }
    }
}
