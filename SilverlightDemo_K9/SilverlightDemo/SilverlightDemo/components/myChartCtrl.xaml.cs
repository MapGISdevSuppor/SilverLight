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
using Visifire.Charts;

namespace SilverlightDemo.components
{
    public partial class myChartCtrl : BaseUserControl
    {
        private Chart m_chart = null;
        public DataSeries m_dataS = null;
        public myChartCtrl()
        {
            InitializeComponent();
            this.chartType.Items.Add("Column");
            this.chartType.Items.Add("Line");
            this.chartType.Items.Add("Pie");
            this.chartType.Items.Add("Bar");
            this.chartType.Items.Add("Area");
            this.chartType.Items.Add("Doughnut");
            this.chartType.Items.Add("StackedColumn");
            this.chartType.Items.Add("StackedColumn100");
            this.chartType.Items.Add("StackedBar");
            this.chartType.Items.Add("StackedBar100");
            this.chartType.Items.Add("StackedArea");
            this.chartType.Items.Add("StackedArea100");
            this.chartType.Items.Add("Bubble");
            this.chartType.Items.Add("Point");
            this.chartType.Items.Add("StreamLineFunnel");
            this.chartType.Items.Add("SectionFunnel");
            this.chartType.Items.Add("Stock");
            this.chartType.Items.Add("CandleStick");
            this.chartType.Items.Add("StepLine");
            this.chartType.Items.Add("Spline");
            this.chartType.Items.Add("Radar");
            this.chartType.Items.Add("Polar");
            this.chartType.Items.Add("Pyramid");
            this.chartType.Items.Add("QuickLine");
            this.chartType.SelectedIndex = 0;
            this.chartType.SelectionChanged+=new SelectionChangedEventHandler(chartType_SelectionChanged);
            this.sliderH.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderH_ValueChanged);
            this.sliderW.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderW_ValueChanged);
            this.sliderA.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderA_ValueChanged);
            this.ThreeDviewer.Checked+=new RoutedEventHandler(ThreeDviewer_Checked);
            this.ThreeDviewer.Unchecked+=new RoutedEventHandler(ThreeDviewer_Unchecked);
            //this.EnableAnimation.Checked+=new RoutedEventHandler(EnableAnimation_Checked);
            //this.EnableAnimation.Unchecked+=new RoutedEventHandler(EnableAnimation_Unchecked);
            this.EnableToolBar.Checked+=new RoutedEventHandler(EnableToolBar_Checked);
            this.EnableToolBar.Unchecked+=new RoutedEventHandler(EnableToolBar_Unchecked);
            this.radioButton1.Checked+=new RoutedEventHandler(radioButton1_Checked);
            this.radioButton2.Checked+=new RoutedEventHandler(radioButton2_Checked);
            this.radioButton3.Checked+=new RoutedEventHandler(radioButton3_Checked);
            this.dialogPanel1.OnClose += new RoutedEventHandler(Close);
        }

        /// <summary>
        /// 是否加载保存统计图为本地图片工具条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableToolBar_Checked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.ToolBarEnabled = true;
        }

        private void EnableToolBar_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.ToolBarEnabled = false;
        }

        /// <summary>
        /// 是否允许动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableAnimation_Checked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.AnimationEnabled = true;
        }

        private void EnableAnimation_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.AnimationEnabled = false;
        }
        /// <summary>
        /// 统计图主题设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.Theme = "Theme1";
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.Theme = "Theme2";
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.Theme = "Theme3";
        }

        /// <summary>
        /// 是否显示3D效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeDviewer_Checked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.View3D = true;
        }

        private void ThreeDviewer_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.View3D = false;
        }
        
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="render"></param>
        /// <param name="e"></param>
        private void Close(object render, RoutedEventArgs e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.Close();
        }

        /// <summary>
        /// 关联统计图
        /// </summary>
        public Chart chart {
            get { return m_chart; }
            set { m_chart = value; }
        }

        /// <summary>
        /// 关联统计数据序列
        /// </summary>
        public DataSeries dataSC
        {
            get { return m_dataS; }
            set { m_dataS = value; }
        }

        /// <summary>
        /// 统计图类型设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.m_chart != null)
            {
                switch (chartType.SelectedIndex)
                {
                    case 0:
                        dataSC.RenderAs = RenderAs.Column;
                        break;
                    case 1:
                        dataSC.RenderAs = RenderAs.Line;
                        break;
                    case 2:
                        dataSC.RenderAs = RenderAs.Pie;
                        break;
                    case 3:
                        dataSC.RenderAs = RenderAs.Bar;
                        break;
                    case 4:
                        dataSC.RenderAs = RenderAs.Area;
                        break;
                    case 5:
                        dataSC.RenderAs = RenderAs.Doughnut;
                        break;
                    case 6:
                        dataSC.RenderAs = RenderAs.StackedColumn;
                        break;
                    case 7:
                        dataSC.RenderAs = RenderAs.StackedColumn100;
                        break;
                    case 8:
                        dataSC.RenderAs = RenderAs.StackedBar;
                        break;
                    case 9:
                        dataSC.RenderAs = RenderAs.StackedBar100;
                        break;
                    case 10:
                        dataSC.RenderAs = RenderAs.StackedArea;
                        break;
                    case 11:
                        dataSC.RenderAs = RenderAs.StackedArea100;
                        break;
                    case 12:
                        dataSC.RenderAs = RenderAs.Bubble;
                        break;
                    case 13:
                        dataSC.RenderAs = RenderAs.Point;
                        break;
                    case 14:
                        dataSC.RenderAs = RenderAs.StreamLineFunnel;
                        break;
                    case 15:
                        dataSC.RenderAs = RenderAs.SectionFunnel;
                        break;
                    case 16:
                        dataSC.RenderAs = RenderAs.Stock;
                        break;
                    case 17:
                        dataSC.RenderAs = RenderAs.CandleStick;
                        break;
                    case 18:
                        dataSC.RenderAs = RenderAs.StepLine;
                        break;
                    case 19:
                        dataSC.RenderAs = RenderAs.Spline;
                        break;
                    case 20:
                        dataSC.RenderAs = RenderAs.Radar;
                        break;
                    case 21:
                        dataSC.RenderAs = RenderAs.Polar;
                        break;
                    case 22:
                        dataSC.RenderAs = RenderAs.Pyramid;
                        break;
                    case 23:
                        dataSC.RenderAs = RenderAs.QuickLine;
                        break;
                    default:
                        dataSC.RenderAs = RenderAs.Column;
                        break;
                }
            }
            else
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
        }
        
        /// <summary>
        /// 统计图高度设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderH_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.Height = sliderH.Value;
        }

        /// <summary>
        /// 统计图宽度设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderW_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.Width = sliderW.Value;
        }

        /// <summary>
        /// 统计图透明度设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderA_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (chart == null)
            {
                MessageBox.Show("请先添加或选择统计图再进行样式设置");
                return;
            }
            else
            this.chart.Opacity = Convert.ToDouble(sliderA.Value);
        }
    }
}
