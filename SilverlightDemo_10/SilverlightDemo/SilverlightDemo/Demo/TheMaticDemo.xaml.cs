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
using SilverlightDemo.Data;
using Visifire.Charts;
using Visifire.Commons;
using SilverlightDemo.components;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMS.Map;
using ZDIMS.Drawing;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using ZDIMS.Util;
using System.Windows.Browser;

namespace SilverlightDemo.Demo
{
    public partial class TheMaticDemo : Page
    {
        string TileLayerAddress = "";
        private MaticData maticdata = null;
        private List<MaticData> list = null;
        private List<myData> pntList = null;
        private myData pntInfo = null;
        private DataSeries dataS = null;
        private myChartCtrl myChartCtrl1;
        public TheMaticDemo()
        {
            InitializeComponent();
            iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);//地图准备就绪事件
        }

        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            initData();//初始化统计数据序列
            initParam();//初始化统计点数据
            //实例化统计图控件，并设置其样式
            myChartCtrl1 = new myChartCtrl();
            myChartCtrl1.HorizontalAlignment = HorizontalAlignment.Right;
            myChartCtrl1.VerticalAlignment = VerticalAlignment.Top;
            myChartCtrl1.SetValue(MarginProperty, new Thickness(0,200, 50, 0));
            //myChartCtrl1.Show();
           
        }

        /// <summary>
        /// 初始化统计数据
        /// </summary>
        private void initData()
        {
            list = new List<MaticData>();
            maticdata = new MaticData();
            maticdata.XLabel = "x1";
            maticdata.YValue = 20;
            list.Add(maticdata);

            maticdata = new MaticData();
            maticdata.XLabel = "x2";
            maticdata.YValue = 15;
            list.Add(maticdata);

            maticdata = new MaticData();
            maticdata.XLabel = "x3";
            maticdata.YValue = 30;
            list.Add(maticdata);
        }

        private void initParam()
        {
            pntList = new List<myData>();
            pntInfo = new myData();
            pntInfo.PlaceName = "温哥华";
            pntInfo.X = -122.995850187958;
            pntInfo.Y = 49.31925247766244;
            pntList.Add(pntInfo);

            pntInfo = new myData();
            pntInfo.PlaceName = "华盛顿";
            pntInfo.X = -76.96333753185513;
            pntInfo.Y = 38.94447278589375;
            pntList.Add(pntInfo);

            pntInfo = new myData();
            pntInfo.PlaceName = "巴西利亚";
            pntInfo.X = -47.93359120189035;
            pntInfo.Y = -15.786919655279648;
            pntList.Add(pntInfo);

            pntInfo = new myData();
            pntInfo.PlaceName = "巴马科";
            pntInfo.X = -8.007674098072965;
            pntInfo.Y = 12.672907851195195;
            pntList.Add(pntInfo);

            pntInfo = new myData();
            pntInfo.PlaceName = "莫斯科";
            pntInfo.X = 37.695301337318895;
            pntInfo.Y = 55.79601312718498;
            pntList.Add(pntInfo);
        }

        /// <summary>
        /// 添加统计图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addMatic_Click(object sender, RoutedEventArgs e)
        {
            if (myChartCtrl1!=null)
            {
                myChartCtrl1.m_chart = null;
                myChartCtrl1.m_dataS = null;
                myChartCtrl1.Show();
            }
            
            clearChart();
            this.iMSMap1.OperType = ZDIMS.Util.IMSOperType.None;
            for (int m = 0; m < pntList.Count; m++)
            {
                Chart chart = new Chart();
                chart.BorderThickness = new Thickness(0);//去掉边框
                chart.Background = new SolidColorBrush(Colors.Transparent);//设置背景色透明
                //去掉x轴线
                Axis xaxis = new Axis();
                xaxis.Enabled = false;
                ChartGrid xgrid = new ChartGrid();
                //xgrid.Enabled = false;
                xaxis.Grids.Add(xgrid);
                chart.AxesX.Add(xaxis);
                //去掉y轴线
                Axis yaxis = new Axis();
                //yaxis.Enabled = false;
                ChartGrid ygrid = new ChartGrid();
                ygrid.Enabled = false;
                yaxis.Grids.Add(ygrid);
                chart.AxesY.Add(yaxis);
                //统计数据设置
                dataS = new DataSeries();
                dataS.RenderAs = RenderAs.Column;//设置默认统计图类型为柱状图
                DataPoint point;
                for (int i = 0; i < list.Count; i++)
                {
                    point = new DataPoint();
                    point.YValue = list[i].YValue;
                    point.AxisXLabel = list[i].XLabel;
                    dataS.DataPoints.Add(point);
                }

                chart.Width = 200;
                chart.Height = 150;
                chart.Opacity = 1.00;
                Title title = new Title();
                title.Text = pntList[m].PlaceName;
                chart.Titles.Add(title);//统计图标题
                chart.Series.Add(dataS);
                dataS.LabelEnabled = true;//显示标签
                chart.AnimationEnabled = true;
                chart.AnimatedUpdate = true;
                dataS.LabelStyle = LabelStyles.OutSide;//设置标签显示位置

                chart.MouseLeftButtonDown += new MouseButtonEventHandler(chart_MouseLeftButtonDown);//鼠标左键按下事件监听

                IMSMark mark = new IMSMark(chart);
                mark.CoordinateType = CoordinateType.Logic;
                mark.X = pntList[m].X;
                mark.Y = pntList[m].Y;
                mark.EnableRevisedPos = true;
                mark.EnableAnimation = false;
                mark.EnableDrag = false;
                this.markLayer1.AddMark(mark);
            }
            this.iMSMap1.SetInfoText("选择统计图，进行样式设置；右键菜单“取消选择状态”，取消选择", Colors.Black);
            MenuItem item = new MenuItem();
            item.Header = "取消选择状态";
            this.iMSMap1.ContextMenu.Items.Add(item);
            item.Click += new RoutedEventHandler(item_Click);

        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            //取消选择
            for (int i = 0; i < this.markLayer1.ChildrenCount; i++)
            {
                IMSMark mark = this.markLayer1.GetMarkByIndex(i) as IMSMark;
                (mark.MarkControl as Chart).BorderThickness = new Thickness(0);
            }
            this.myChartCtrl1.chart = null;
        }

        //对选择的统计图样式改变事件处理
        void chart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.myChartCtrl1.chart = sender as Chart;
            for (int i = 0; i < this.markLayer1.ChildrenCount; i++)
            {
                IMSMark mark = this.markLayer1.GetMarkByIndex(i) as IMSMark;
                if ((mark.MarkControl as Chart) != this.myChartCtrl1.chart)
                {
                    (mark.MarkControl as Chart).BorderThickness = new Thickness(0);
                }
            }
            (sender as Chart).BorderThickness = new Thickness(1);
            this.myChartCtrl1.dataSC = (sender as Chart).Series[0];
        }

        /// <summary>
        /// 清除统计图
        /// </summary>
        private void clearChart()
        {
            this.markLayer1.RemoveAll();
        }

        /// <summary>
        /// 统计图设置面板的显示与隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SandH_Click(object sender, RoutedEventArgs e)
        {
            myChartCtrl1.Show();
        }

        private void unView(object sender, RoutedEventArgs e)
        {
            myChartCtrl1.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            tileLayer1.ServerAddress = TileLayerAddress;
        }

    }
}

