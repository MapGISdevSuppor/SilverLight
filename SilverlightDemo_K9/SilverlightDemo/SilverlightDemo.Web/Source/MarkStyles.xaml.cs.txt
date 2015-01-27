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

namespace SilverlightDemo
{
    public partial class MarkStyles : Page
    {
        string TileLayerAddress = "";
        private IMSMarkerSymbol _markers;//点样式对象
        public MarkStyles()
        {
            InitializeComponent();
            //添加颜色选择结束事件，获取选择的颜色
            this.colorsPicker.ColorChangedOverCallBack += new ZdimsExtends.util.ColorsPicker.SelectedColorHander(colorsPicker_ColorhangedOverCallBack);
            //添加地图准备事件
            this.iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);
        }
        /// <summary>
        /// 地图就绪事件
        /// </summary>
        /// <param name="e"></param>
        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                //监听地图容器鼠标左键弹起事件
                this.iMSMap1.MouseLeftButtonUp += new MouseButtonEventHandler(iMSMap1_MouseLeftButtonUp);
                this.iMSMap1.SetInfoText("                                                    鼠标选择点编辑，点击地图取消选择！", Colors.Black);
                //定义点样式对象，添加到地图上
                IMSSimpleMarkerSymbol markers;
                markers = new IMSSimpleMarkerSymbol();
                //添加预定义点样式对象选择监听事件
                markers.ChossedMarkerOverCallBack += new IMSMarkerSymbol.ChossedMarkerHander(markers_ChossedShapeOverCallBack);
                IMSMark mark;
                mark = new IMSMark(markers.control, ZDIMS.Interface.CoordinateType.Logic);
                mark.X = 6.99919517012924;
                mark.Y = 30.671057152220655;
                mark.EnableAnimation = false;
                this.markLayer.AddMark(mark);

                markers = new IMSSimpleMarkerSymbol();
                markers.ChossedMarkerOverCallBack += markers_ChossedShapeOverCallBack;
                markers.SymbolStyle = MarkSymbolStyle.Star;
                mark = new IMSMark(markers.control, ZDIMS.Interface.CoordinateType.Logic);
                mark.EnableAnimation = false;
                mark.X = 8.89919517012924;
                mark.Y = 23.971057152220655;
                this.markLayer.AddMark(mark);

                //添加图片点样式对象
                IMSPictureMarkerSymbol picMarker;
                picMarker = new IMSPictureMarkerSymbol();
                //添加对象选择监听事件
                picMarker.ChossedMarkerOverCallBack += markers_ChossedShapeOverCallBack;
                mark = new IMSMark(picMarker.control, ZDIMS.Interface.CoordinateType.Logic);
                mark.X = 18.99919517012924;
                mark.Y = 30.671057152220655;
                mark.EnableAnimation = false;
                this.markLayer.AddMark(mark);

                picMarker = new IMSPictureMarkerSymbol();
                picMarker.Source = "../images/Ring.png";//设置图片路径
                //添加对象选择监听事件
                picMarker.ChossedMarkerOverCallBack += markers_ChossedShapeOverCallBack;
                mark = new IMSMark(picMarker.control, ZDIMS.Interface.CoordinateType.Logic);
                mark.X = 20.99919517012924;
                mark.Y = 23.671057152220655;
                mark.EnableAnimation = false;
                this.markLayer.AddMark(mark);
            }
        }
        /// <summary>
        /// 左键弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void iMSMap1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //清除鼠标选择的对象
            if (this._markers != null)
            {
                this._markers.EnableEdit = false;
                this._markers = null;
            }
        }
        /// <summary>
        /// 对象选择处理事件
        /// </summary>
        /// <param name="sender"></param>
        void markers_ChossedShapeOverCallBack(IMSMarkerSymbol sender)
        {
            //清除上次鼠标选择的对象
            if (_markers != null)
            {
                _markers.EnableEdit = false;
            }
            //设置鼠标当前选择的对象
            _markers = sender;
            _markers.EnableEdit = true;
        }
        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="strColor"></param>
        void colorsPicker_ColorhangedOverCallBack(Color strColor)
        {
            if (this.colorsPicker != null)
            {
                if (this._markers != null)
                {
                    if (this._markers is IMSSimpleMarkerSymbol)
                    {
                        IMSSimpleMarkerSymbol tmp = _markers as IMSSimpleMarkerSymbol;
                        //设置点颜色
                        tmp.Color = new SolidColorBrush(strColor);
                    }
                    else
                    {
                        MessageBox.Show("不是选择的几何点！");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
                }
            }

        }
        /// <summary>
        /// 设置点大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetSize != null)
                if (this._markers != null)
                {
                    if (this._markers is IMSSimpleMarkerSymbol)
                    {
                        IMSSimpleMarkerSymbol tmp = _markers as IMSSimpleMarkerSymbol;
                        tmp.Size = this.SetSize.Value;//设置大小
                    }
                    else
                    {
                        MessageBox.Show("不是选择的几何点！");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
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
        /// <summary>
        /// 设置图片点样式的宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetWidth != null)
            {
                if (this._markers != null)
                {
                    if (this._markers is IMSPictureMarkerSymbol)
                    {
                        IMSPictureMarkerSymbol tmp = _markers as IMSPictureMarkerSymbol;
                        tmp.Width = this.SetWidth.Value;//设置宽
                    }
                    else
                    {
                        MessageBox.Show("不是选择的图片点");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置图片点样式高
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetWidth != null)
            {
                if (this._markers != null)
                {
                    if (this._markers is IMSPictureMarkerSymbol)
                    {
                        IMSPictureMarkerSymbol tmp = _markers as IMSPictureMarkerSymbol;
                        tmp.Height = this.SetHeight.Value;//设置高度
                    }
                    else
                    {
                        MessageBox.Show("不是选择的图片点");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置图片角度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetAngle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetWidth != null)
            {
                if (this._markers != null)
                {
                    if (this._markers is IMSPictureMarkerSymbol)
                    {
                        IMSPictureMarkerSymbol tmp = _markers as IMSPictureMarkerSymbol;
                        tmp.Angle = this.SetAngle.Value;//设置角度
                    }
                    else
                    {
                        MessageBox.Show("不是选择的图片点");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置图片点透明度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.SetOpacity != null)
            {
                if (this._markers != null)
                {
                    if (this._markers is IMSPictureMarkerSymbol)
                    {
                        IMSPictureMarkerSymbol tmp = _markers as IMSPictureMarkerSymbol;
                        tmp.Opacity = this.SetOpacity.Value;//设置透明度
                    }
                    else
                    {
                        MessageBox.Show("不是选择的图片点");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
                }
            }
        }
        //设置图片路径
        private void setSource_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setSource != null)
            {
                if (this._markers != null)
                {
                    if (this._markers is IMSPictureMarkerSymbol)
                    {
                        IMSPictureMarkerSymbol tmp = _markers as IMSPictureMarkerSymbol;
                        tmp.Source = ((Image)(this.setSource.SelectionBoxItem)).Tag.ToString();
                    }
                    else
                    {
                        MessageBox.Show("不是选择的图片点");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
                }
            }
        }
        /// <summary>
        /// 设置点样式类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void setType_DropDownClosed(object sender, EventArgs e)
        {
            if (this.setType != null)
            {
                if (this._markers != null)
                {
                    //根据选择设置点的样式
                    if (this._markers is IMSSimpleMarkerSymbol)
                    {
                        IMSSimpleMarkerSymbol tmp = _markers as IMSSimpleMarkerSymbol;

                        switch (((Image)(this.setType.SelectionBoxItem)).Tag.ToString())
                        {
                            case "圆":
                                {
                                    tmp.SymbolStyle = MarkSymbolStyle.Circle;
                                    break;
                                }
                            case "矩形":
                                {
                                    tmp.SymbolStyle = MarkSymbolStyle.Square;
                                    break;
                                }
                            case "三角形":
                                {
                                    tmp.SymbolStyle = MarkSymbolStyle.Triangle;
                                    break;
                                }
                            case "扇形":
                                {
                                    tmp.SymbolStyle = MarkSymbolStyle.Sector;
                                    break;
                                }
                            case "菱形":
                                {
                                    tmp.SymbolStyle = MarkSymbolStyle.Diamond;
                                    break;
                                }
                            case "五角星":
                                {
                                    tmp.SymbolStyle = MarkSymbolStyle.Star;
                                    break;
                                }
                            default:
                                {
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
                else
                {
                    MessageBox.Show("未选择编辑的点对象！");
                    return;
                }
            }

        }
    }
}

