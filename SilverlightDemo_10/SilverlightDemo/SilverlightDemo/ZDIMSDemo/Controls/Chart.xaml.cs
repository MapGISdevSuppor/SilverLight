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
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using ZDIMS.BaseLib;

namespace ZDIMSDemo.Controls
{
    public partial class Chart : BaseUserControl
    {
        public class DynamicData
        {
            public string X
            { get; set; }
            public double Y
            { get; set; }
        }
        private CAttDataTable _data;
        public Chart()
        {
            InitializeComponent();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
            this.chartType.Items.Add("柱状图");
            this.chartType.Items.Add("饼图");
            this.chartType.Items.Add("点状图");
            this.chartType.Items.Add("线形图");
            this.chartType.Items.Add("区域图");
            this.chartType.SelectedIndex = 0;
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
        private void buffer_Click(object sender, RoutedEventArgs e)
        {
            this.mychart.Series.Clear();
            List<DynamicData> drLst = new List<DynamicData>();
            int xax = this.xaxis.SelectedIndex;
            int yax = this.yaxis.SelectedIndex;
            try
            {
                Convert.ToDouble(_data.Rows[0].Values[yax]);
            }
            catch
            {
                MessageBox.Show("字段" + this.yaxis.SelectedItem.ToString() + "无法统计，请重新选择！");
                return;
            }
            for (int i = 0; i < _data.Rows.Length; i++)
            {
                DynamicData dd = new DynamicData();
                dd. X = _data. Rows[i]. Values[xax]. ToString( );
                dd.Y = Convert.ToDouble(_data.Rows[i].Values[yax]);
                drLst.Add(dd);
            }
            switch (this.chartType.SelectedItem.ToString())
            {
                case "柱状图":
                    this.mychart.Title = "柱状图";
                    this.mychart.LegendTitle = "";
                    ColumnSeries myColumn = new ColumnSeries
                    {
                        ItemsSource = drLst,
                        Title = "柱状图",
                        IndependentValueBinding = new Binding("X"),//X轴 
                        DependentValueBinding = new Binding("Y"),  //Y轴
                    };
                    this.mychart.Series.Add(myColumn);
                    break;
                case "饼图":
                    this.mychart.Title = "饼图";
                    this.mychart.LegendTitle = "饼图";
                    PieSeries myPie = new PieSeries
                    {
                        ItemsSource = drLst,
                        Title = "饼图",
                        IndependentValueBinding = new Binding("X"),//X轴 
                        DependentValueBinding = new Binding("Y"),  //Y轴
                    };
                    this.mychart.Series.Add(myPie);
                    break;
                case "点状图":
                    this.mychart.Title = "点状图";
                    this.mychart.LegendTitle = "";
                    ScatterSeries myScatter = new ScatterSeries
                    {
                        ItemsSource = drLst,
                        Title = "点状图",
                        IndependentValueBinding = new Binding("X"),//X轴 
                        DependentValueBinding = new Binding("Y"),  //Y轴
                    };
                    this.mychart.Series.Add(myScatter);
                    break;
                case "线形图":
                    this.mychart.Title = "线形图";
                    this.mychart.LegendTitle = "";
                    LineSeries myLine = new LineSeries
                   {
                       ItemsSource = drLst,
                       Title = "线形图",
                       IndependentValueBinding = new Binding("X"),//X轴 
                       DependentValueBinding = new Binding("Y"),  //Y轴
                   };
                    this.mychart.Series.Add(myLine);
                    break;
                case "区域图":
                    this.mychart.Title = "区域图";
                    this.mychart.LegendTitle = "";
                    AreaSeries myArea = new AreaSeries
                   {
                       ItemsSource = drLst,
                       Title = "区域图",
                       IndependentValueBinding = new Binding("X"),//X轴 
                       DependentValueBinding = new Binding("Y"),  //Y轴
                   };
                    this.mychart.Series.Add(myArea);
                    break;
            }
        }
        /**
		* 绑定数据
		*/
		public void setDataProvider(CAttDataTable dataTable)
		{
			this._data = dataTable;
			if(this._data == null)
			{ 
				return;
			}
			int fldNum = dataTable.Columns.FldNumber;
			List<string> data=new List<string>();
				
			for(int i=0;i<fldNum;i++)
			{
				data.Add(dataTable.Columns.FldName[i]);
			}
			this.xaxis.ItemsSource = data;
            this.xaxis.SelectedIndex = 0;
            this.yaxis.ItemsSource = data;
            this.yaxis.SelectedIndex = 0;
            this.mychart.Series.Clear();
		}
        private void cancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
