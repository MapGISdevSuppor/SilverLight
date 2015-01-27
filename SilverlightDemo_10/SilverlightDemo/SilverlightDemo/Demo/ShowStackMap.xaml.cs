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
using ZDIMS.BaseLib;
using System.Windows.Browser;
using ZDIMS.Map;
using ZDIMS.Interface;
using ZDIMS.Util;

namespace SilverlightDemo
{
    public partial class ShowStackMap : Page
    {
        string TileLayerAddress = "";
        string VecLayerAddress = "";
        private CDynNoteInfo dynNoteInfo;
        private DynShowStyle dynStyle;
        private CDisplayStyle displayStyle;
        private CSetLayerDisplayStyle setDisplayStyle = null;
        public ShowStackMap()
        {
            InitializeComponent();
            Canvas.SetZIndex(canvas1, Canvas.GetZIndex(this.iMSMap1) + 10);
            this.tileAlpha.ValueChanged += new RoutedPropertyChangedEventHandler<double>(tileAlpha_ValueChanged);
            this.vectAlpha.ValueChanged += new RoutedPropertyChangedEventHandler<double>(vectAlpha_ValueChanged);
            this.tilelayer.Checked += new RoutedEventHandler(tilelayer_Checked);
            this.tilelayer.Unchecked += new RoutedEventHandler(tilelayer_Checked);
            this.riverlayer.Checked += new RoutedEventHandler(riverlayer_Checked);
            this.riverlayer.Unchecked += new RoutedEventHandler(riverlayer_Checked);
        }


        private void layername_Click(object sender, RoutedEventArgs e)
        {
            if (layername.IsChecked.Value)
            {
                //设置显示河流名称的动态注记
                this.dynStyle.DynNoteFlag = true;	
            }
            else
            {
                //设置不显示河流名称的动态注记
                this.dynStyle.DynNoteFlag = false;
            }
            //更新图层的地图显示样式
            this.displayStyle.ShowStyle[0] = this.dynStyle;
            //更新地图显示参数
            this.setDisplayStyle.DisplayStyle[0] = this.displayStyle;
            this.river.SetLayerDisplayStyle(setDisplayStyle, OnSetStyle);
        }
        public void OnSetStyle(object sender, UploadStringCompletedEventArgs e)
        {
            //刷新河流图层
            this.river.Refresh();
        }


        private void riverlayer_Checked(object sender, RoutedEventArgs e)
        {
            // 河流图层显示控制	
            if (riverlayer.IsChecked.Value)
            {
                //显示河流图层时解开控制面板的河流名称显示控制按钮
                layername.IsEnabled = true;
                //设置显示河流图层
                river.Display = true;
            }
            else
            {
                //隐藏河流图层时锁定控制面板的河流名称显示控制按钮
                layername.IsEnabled = false;
                layername.IsChecked = false;
                //设置隐藏河流图层
                river.Display = false;
                //设置图层不透明
                this.river.Opacity = this.vectAlpha.Value = 1;
            }
        }
        private void tileAlpha_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //设置世界地图图层的透明度
            baseworld.Opacity = this.tileAlpha.Value;
        }
        private void vectAlpha_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //设置矢量河流图层的透明度
            river.Opacity = this.vectAlpha.Value;	
        }

        private void tilelayer_Checked(object sender, RoutedEventArgs e)
        {
            // 世界地图显示控制	
            if (tilelayer.IsChecked.Value)
                //设置显示世界地图
                baseworld.Display = true;
            else
            {
                //设置隐藏世界地图
                baseworld.Display = false;
                //设置图层不透明
                this.baseworld.Opacity = this.tileAlpha.Value = 1;
            }
        }

        private void river_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化动态注记类对象
            dynNoteInfo = new CDynNoteInfo();
            //设置显示动态注记的字段名 
            dynNoteInfo.FieldName = "NAME";
            //设置显示动态注记的字体大小 
            dynNoteInfo.FontSize = 12;
            //初始化地图显示参数对象
            dynStyle = new DynShowStyle();
            //设置显示动态注记
            dynStyle.DynNoteFlag = true;
            //设置动态注记参数
            dynStyle.DynNoteInfo = dynNoteInfo;
            //初始化地图显示样式类对象
            displayStyle = new CDisplayStyle();
            displayStyle.ShowStyle = new DynShowStyle[1];
            //设置第一个图层的地图显示样式
            displayStyle.ShowStyle[0] = dynStyle;
            //初始化设置图像显示参数 
            setDisplayStyle = new CSetLayerDisplayStyle();
            setDisplayStyle.DisplayStyle = new CDisplayStyle[1];
            //设置需要的显示参数
            setDisplayStyle.DisplayStyle[0] = displayStyle;
            this.tilelayer.IsChecked = true;
        }
        //页面加载的时候给瓦片地图和矢量地图图层的地址ServerAddress赋值
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();
                VecLayerAddress = HtmlPage.Window.Eval("VecLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            baseworld.ServerAddress = TileLayerAddress;
            river.ServerAddress = VecLayerAddress;

        }

    }
}
