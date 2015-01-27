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
using System.ComponentModel;
using ZdimsExtends.style;
using ZDIMS.Drawing;
using ZDIMS.Util;
using System.Windows.Browser;
namespace SilverlightDemo.Demo
{
    public partial class FillStyle : Page
    {
        string TileLayerAddress = "";
        GraphicsBase _polygonStyle;
        public FillStyle()
        {
            InitializeComponent();
            //添加边框颜色选择结束事件，获取选择的颜色
            this.StrokecolorsPicker1.ColorChangedOverCallBack += new ZdimsExtends.util.ColorsPicker.SelectedColorHander(colorsPicker_ColorhangedOverCallBack);
            //添加图片填充边框颜色选择结束事件
            this.colorsPicker.ColorChangedOverCallBack += new ZdimsExtends.util.ColorsPicker.SelectedColorHander(piccolorsPicker_ColorhangedOverCallBack);
            //添加边框颜色选择事件
            this.fillcolorsPicker.ColorChangedOverCallBack += new ZdimsExtends.util.ColorsPicker.SelectedColorHander(fillcolorsPicker_ColorChangedOverCallBack);
            //添加地图准备事件
            this.iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);
            this.iMSMap1.MapInitComplete += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapInitComplete);
        }

        void iMSMap1_MapInitComplete(ZDIMS.Event.IMSMapEvent e)
        {
            this.iMSMap1.SetInfoText("                                                    鼠标选择点编辑，点击地图取消选择！", Colors.Black);
            //添加图片填充区域对象
            //添加地图容器右键菜单
            MenuItem item = new MenuItem();
            item.Header = "取消选择";
            this.iMSMap1.ContextMenu.Items.Add(item);
            item.Click += new RoutedEventHandler(item_Click);
        }

        /// <summary>
        /// 颜色选择处理事件
        /// </summary>
        /// <param name="strColor">选择的颜色</param>
        void fillcolorsPicker_ColorChangedOverCallBack(Color strColor)
        {
            //设置填充颜色
            if (this._polygonStyle != null)
            {
                if (this._polygonStyle is IMSSimpleFillStyle)
                {
                    IMSSimpleFillStyle tmp = this._polygonStyle as IMSSimpleFillStyle;
                    tmp.FillColor = strColor;
                }
                else
                {
                    MessageBox.Show("请选择预定义区样式图形！");
                    return;
                }
            }
        }
        /// <summary>
        /// 地图就绪事件
        /// </summary>
        /// <param name="e"></param>
        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
           
               //添加图片填充区域对象
                IMSPictureFillStyle PicFill = new IMSPictureFillStyle();
                //添加对象选择监听事件
                PicFill.ChossedMarkerOverCallBack += new IMSPictureFillStyle.ChossedMarkerHander(callback);
                this.graphicsLayer.AddGraphics(PicFill);//添加对象
                //设置坐标
                PicFill.Points.Add(new Point(8.99919517012924, 29.671057152220655));
                PicFill.Points.Add(new Point(12.99919517012924, 10.671057152220655));
                PicFill.Points.Add(new Point(-40.99919517012924, 12.671057152220655));
                PicFill.Draw();//绘制对象
                //添加预定义多边形对象
                IMSSimpleFillStyle fill = new IMSSimpleFillStyle();
                this.graphicsLayer.AddGraphics(fill);
                fill.ChossedMarkerOverCallBack += callback;//监听选择事件
                //设置坐标
                fill.Points.Add(new Point(10.99919517012924, 29.671057152220655));
                fill.Points.Add(new Point(19.99919517012924, 10.671057152220655));
                fill.Points.Add(new Point(33.99919517012924, 32.671057152220655));
                fill.Draw();
            }
        }
        /// <summary>
        /// 右键菜单“取消选择”，监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click(object sender, RoutedEventArgs e)
        {
            //取消当前选择的对象
            if (this._polygonStyle != null)
            {
                this._polygonStyle.EnableEdit = false;
                this._polygonStyle = null;
            }
        }
        /// <summary>
        /// 对象选择处理事件
        /// </summary>
        /// <param name="sender"></param>
        public void callback(GraphicsBase sender)
        {
            //清除上次选择的对象
            if (this._polygonStyle != null)
            {
                this._polygonStyle.EnableEdit = false;
            }
            //设置当前选择的对象
            this._polygonStyle = sender;
            this._polygonStyle.EnableEdit = true;
        }
        /// <summary>
        /// 填充透明度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this._polygonStyle != null)
            {
                if (this._polygonStyle is IMSPictureFillStyle)
                {
                    IMSPictureFillStyle tmp = this._polygonStyle as IMSPictureFillStyle;
                    tmp.fillOpacity = ((Slider)sender).Value;//设置透明度属性
                }
                else
                {
                    MessageBox.Show("选择图片填充区对象");
                    return;
                }
            }

        }
        /// <summary>
        /// 设置边框颜色
        /// </summary>
        /// <param name="strColor"></param>
        void colorsPicker_ColorhangedOverCallBack(Color strColor)
        {
            if (this.colorsPicker != null)
            {
                if (this._polygonStyle != null)
                {
                    if (this._polygonStyle is IMSSimpleFillStyle)
                    {
                        IMSSimpleFillStyle tmp = this._polygonStyle as IMSSimpleFillStyle;
                        tmp.StrokeColor = strColor;//设置边框颜色
                    }
                    else
                    {
                        MessageBox.Show("未选择编辑的几何图形填充区对象！");
                        return;
                    }
                }
            }

        }
        /// <summary>
        /// 设置边框颜色
        /// </summary>
        /// <param name="strColor"></param>
        void piccolorsPicker_ColorhangedOverCallBack(Color strColor)
        {
            if (this.colorsPicker != null)
            {
                if (this._polygonStyle != null)
                {
                    if (this._polygonStyle is IMSPictureFillStyle)
                    {
                        IMSPictureFillStyle tmp = this._polygonStyle as IMSPictureFillStyle;
                        tmp.StrokeColor = strColor;//设置图片填充对象边框颜色
                    }

                    else
                    {
                        MessageBox.Show("未选择编辑的图片填充区对象！");
                        return;
                    }
                }
            }

        }
        /// <summary>
        /// 图片填充区边框透明度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetStrookeOpa_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetStrookeOpa != null)
            {

                if (_polygonStyle != null)
                {
                    if (this._polygonStyle is IMSPictureFillStyle)
                    {
                        IMSPictureFillStyle tmp = this._polygonStyle as IMSPictureFillStyle;
                        tmp.StrokeOpacity = ((Slider)sender).Value;//设置边框透明度
                    }
                    else
                    {
                        MessageBox.Show("选图片填充区对象");
                        return;
                    }
                }
            }


        }
        /// <summary>
        /// 预定义区边框透明度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetStrookeOpa1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetStrookeOpa1 != null)
            {
                if (this._polygonStyle != null)
                {
                    if (this._polygonStyle is IMSSimpleFillStyle)
                    {
                        IMSSimpleFillStyle tmp = this._polygonStyle as IMSSimpleFillStyle;
                        tmp.StrokeOpacity = ((Slider)sender).Value;//设置边框透明度
                    }
                    else
                    {
                        MessageBox.Show("选择几何图形填充区对象");
                        return;
                    }
                }

            }
        }
        private void SetFillOpa_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this._polygonStyle != null)
            {
                if (this._polygonStyle is IMSSimpleFillStyle)
                {
                    IMSSimpleFillStyle tmp = this._polygonStyle as IMSSimpleFillStyle;
                    tmp.fillOpacity = ((Slider)sender).Value;//设置填充透明度
                }
                else
                {
                    MessageBox.Show("选择几何图形填充区对象");
                    return;
                }
            }
        }
        /// <summary>
        /// 图片填充区边框类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setStrokeType_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setStrokeType != null)
            {
                if (this._polygonStyle != null)
                {
                    //根据选择的类型设置边框样式
                    LineType tmp = LineType.Dot;
                    string type = ((Image)(this.setStrokeType.SelectionBoxItem)).Tag.ToString();
                    switch (type)
                    {
                        case "Soild":
                            {
                                tmp = LineType.Solid;
                                break;
                            }
                        case "Dash":
                            {
                                tmp = LineType.Dash;
                                break;
                            }
                        case "DashDot":
                            {
                                tmp = LineType.DashDot;
                                break;
                            }
                        case "Dot":
                            {
                                tmp = LineType.Dot;
                                break;
                            }
                        case "DashDotDot":
                            {
                                tmp = LineType.DashDotDot;
                                break;
                            }
                    }
                    if (this._polygonStyle is IMSPictureFillStyle)
                    {
                        IMSPictureFillStyle t = this._polygonStyle as IMSPictureFillStyle;
                        t.StrokeLineSymbol = tmp;//设置边框样式
                    }
                    else
                    {
                        MessageBox.Show("选图片填充区对象");
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 图片路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setSource_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setSource != null)
            {
                if (this._polygonStyle != null)
                {
                    if (this._polygonStyle is IMSPictureFillStyle)
                    {
                        IMSPictureFillStyle tmp = this._polygonStyle as IMSPictureFillStyle;
                        //设置图片路径
                        tmp.ImgSource = ((Image)(this.setSource.SelectionBoxItem)).Tag.ToString();
                    }
                    else
                    {
                        MessageBox.Show("未选择编辑的图片填充区对象！");
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 预定义区边框样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setStrokeType1_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setStrokeType1 != null)
            {

                if (this._polygonStyle != null)
                {
                    if (this._polygonStyle is IMSSimpleFillStyle)
                    {
                        LineType tmp = LineType.Dot;
                        string type = ((Image)(this.setStrokeType1.SelectionBoxItem)).Tag.ToString();
                        switch (type)
                        {
                            case "Soild":
                                {
                                    tmp = LineType.Solid;
                                    break;
                                }
                            case "Dash":
                                {
                                    tmp = LineType.Dash;
                                    break;
                                }
                            case "DashDot":
                                {
                                    tmp = LineType.DashDot;
                                    break;
                                }
                            case "Dot":
                                {
                                    tmp = LineType.Dot;
                                    break;
                                }
                            case "DashDotDot":
                                {
                                    tmp = LineType.DashDotDot;
                                    break;
                                }

                        }
                        IMSSimpleFillStyle f = this._polygonStyle as IMSSimpleFillStyle;
                        f.StrokeLineSymbol = tmp;//设置边框样式
                    }
                    else
                    {
                        MessageBox.Show("选择几何图形填充区对象");
                        return;
                    }
                }

            }
        }
        /// <summary>
        /// 预定义区填充符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setFillType_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setFillType != null)
            {
                IMSSimpleFillStyle tmp = new IMSSimpleFillStyle();
                if (this._polygonStyle != null)
                {
                    //根据选择的设置填充样式
                    if (this._polygonStyle is IMSSimpleFillStyle)
                    {
                        tmp = this._polygonStyle as IMSSimpleFillStyle;
                    }
                    else
                    {
                        MessageBox.Show("选择几何图形填充区对象");
                        return;
                    }

                    switch (((Image)(this.setFillType.SelectionBoxItem)).Tag.ToString())
                    {
                        case "颜色填充":
                            {
                                tmp.FillSymbol = FillSymbol.Solid;
                                break;
                            }
                        case "斜线":
                            {
                                tmp.FillSymbol = FillSymbol.Slash;
                                break;
                            }
                        case "反斜线":
                            {
                                tmp.FillSymbol = FillSymbol.BackSlash;
                                break;
                            }
                        case "网格":
                            {
                                tmp.FillSymbol = FillSymbol.Cross;
                                break;
                            }
                        case "横线":
                            {
                                tmp.FillSymbol = FillSymbol.Horizontal;
                                break;
                            }
                        case "竖线":
                            {
                                tmp.FillSymbol = FillSymbol.Vertical;
                                break;
                            }
                    }
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

