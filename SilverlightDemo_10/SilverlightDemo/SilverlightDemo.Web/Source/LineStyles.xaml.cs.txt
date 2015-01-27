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
using ZdimsExtends.style;
using ZDIMS.Drawing;
using System.ComponentModel;
using ZDIMS.Util;
using System.Windows.Browser;
namespace SilverlightDemo
{
    public partial class LineStyles : Page
    {
        private IMSPredefinedLineStyle _Lines;//线样式对象
        string TileLayerAddress = "";
        public LineStyles()
        {
            InitializeComponent();
            //添加颜色选择结束事件，获取选择的颜色
            this.colorsPicker.ColorChangedOverCallBack += new ZdimsExtends.util.ColorsPicker.SelectedColorHander(colorsPicker_ColorhangedOverCallBack);
            //添加地图准备事件
            this.iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);
            this.iMSMap1.MapInitComplete += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapInitComplete);
        }
        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                //添加线样式对象
                IMSPredefinedLineStyle lines;
                lines = new IMSPredefinedLineStyle();
                this.graphicsLayer.AddGraphics(lines);
                //添加对象选择监听事件
                lines.ChossedMarkerOverCallBack += new IMSPredefinedLineStyle.ChossedMarkerHander(chossedCallBack);
                //设置坐标
                lines.Points.Add(new Point(45.99919517012924, 30.671057152220655));
                lines.Points.Add(new Point(20.99919517012924, 10.671057152220655));
                lines.LineSymbol = LineType.DashDotDot;
                lines.Draw();

                lines = new IMSPredefinedLineStyle();
                lines.StrokeThickness = 5;
                lines.LineSymbol = LineType.Solid;
                this.graphicsLayer.AddGraphics(lines);
                lines.ChossedMarkerOverCallBack += new IMSPredefinedLineStyle.ChossedMarkerHander(chossedCallBack);
                lines.Points.Add(new Point(4.99919517012924, 29.671057152220655));
                lines.Points.Add(new Point(-10.99919517012924, 3.671057152220655));
                lines.Points.Add(new Point(-40.99919517012924, 35.671057152220655));
                lines.LineSymbol = LineType.DashDotDot;
                lines.Draw();

            }
        }

        void iMSMap1_MapInitComplete(ZDIMS.Event.IMSMapEvent e)
        {
            //添加地图容器右键菜单
            MenuItem item = new MenuItem();
            item.Header = "取消选择";
            this.iMSMap1.ContextMenu.Items.Add(item);
            item.Click += new RoutedEventHandler(item_Click);
            this.iMSMap1.SetInfoText("                                                    鼠标选择线编辑，右键菜单“取消选择”！", Colors.Black);
              
        }

        /// <summary>
        /// 右键菜单“取消选择”，监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click(object sender, RoutedEventArgs e)
        {
            //取消当前选择的对象
            if (this._Lines != null)
            {
                this._Lines.EnableEdit = false;
                this._Lines = null;
            }
        }
        /// <summary>
        /// 对象选择处理事件
        /// </summary>
        /// <param name="sender"></param>
        void chossedCallBack(IMSPredefinedLineStyle sender)
        {
            //清除上次选择的对象
            if (this._Lines != null)
            {
                this._Lines.EnableEdit = false;
            }
            //设置当前选择的对象
            this._Lines = sender;
            this._Lines.EnableEdit = true;
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="strColor"></param>
        void colorsPicker_ColorhangedOverCallBack(Color strColor)
        {
            if (this.colorsPicker != null)
            {
                if (this._Lines != null)
                {
                    IMSPredefinedLineStyle tmp = _Lines;
                    tmp.Color = strColor;//设置线颜色
                }
                else
                {
                    MessageBox.Show("未选择编辑的线对象！");
                    return;
                }
            }

        }
        /// <summary>
        /// 设置线的透明度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.setOpacity != null)
            {
                if (this._Lines != null)
                {
                    //设置线透明度
                    this._Lines.Opacity = this.setOpacity.Value;
                }
                else
                {
                    MessageBox.Show("未选择编辑的线对象！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置线宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetSize != null)
            {
                if (this._Lines != null)
                {
                    //设置线宽
                    this._Lines.StrokeThickness = this.SetSize.Value;
                }
                else
                {
                    MessageBox.Show("未选择编辑的线对象！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置线显示样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setType_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setType != null)
            {
                if (this._Lines != null)
                {
                    //根据选择的类型，设置线显示样式
                    switch (((Image)(this.setType.SelectionBoxItem)).Tag.ToString())
                    {
                        case "Soild":
                            {
                                _Lines.LineSymbol = LineType.Solid;
                                break;
                            }
                        case "Dash":
                            {
                                _Lines.LineSymbol = LineType.Dash;
                                break;
                            }
                        case "DashDot":
                            {
                                _Lines.LineSymbol = LineType.DashDot;
                                break;
                            }
                        case "Dot":
                            {
                                _Lines.LineSymbol = LineType.Dot;
                                break;
                            }
                        case "DashDotDot":
                            {
                                _Lines.LineSymbol = LineType.DashDotDot;
                                break;
                            }
                    }
                }
                else
                {
                    MessageBox.Show("不是选择的几何点！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置拐角样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setJoin_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setJoin!= null)
            {
                if (this._Lines != null)
                {
                    switch (this.setJoin.SelectionBoxItem.ToString())
                    {
                        case "普通":
                            {
                                this._Lines.StrokeLineJoin = PenLineJoin.Miter;
                                break;
                            }
                        case "斜角":
                            {
                                this._Lines.StrokeLineJoin = PenLineJoin.Bevel;
                                break;
                            }
                        case "圆角":
                            {
                                this._Lines.StrokeLineJoin = PenLineJoin.Round;
                                break;
                            }
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的线对象！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置线端点样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setCap_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setCap != null)
            {
                if (this._Lines != null)
                {
                    switch (((ComboBoxItem)(this.setCap.SelectedItem)).Content.ToString())
                    {
                        case "普通":
                            {
                                this._Lines.StrokeLineCap = PenLineCap.Flat;
                                break;
                            }
                        case "半圆":
                            {
                                this._Lines.StrokeLineCap = PenLineCap.Round;
                                break;
                            }
                        case "三角":
                            {
                                this._Lines.StrokeLineCap = PenLineCap.Triangle;
                                break;
                            }
                        case "矩形":
                            {
                                this._Lines.StrokeLineCap = PenLineCap.Square;
                                break;
                            }
                    }


                }
                else
                {
                    MessageBox.Show("未选择编辑的线对象！");
                    return;
                }
            }
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
