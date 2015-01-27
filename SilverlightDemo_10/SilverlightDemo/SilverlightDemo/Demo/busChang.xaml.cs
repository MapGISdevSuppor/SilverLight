﻿using System;
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
using SilverlightDemo.buscomponets.BusControl;
using System.ComponentModel;
using ZDIMS.Event;
using ZDIMS.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMS.Util;
using ZdimsExtends.QueryControl;
using ZdimsExtends.style;
using System.Windows.Browser;

namespace SilverlightDemo
{
    public partial class busChang : Page
    {
        BusAnalyse busCtl;//公交换乘控件
        setPlay setTrack;//设置播放控件
        private IMSSimpleMarkerSymbol d;
        string TileLayerAddress = "";
        public busChang()
        {
            InitializeComponent();
            //添加地图准备完毕事件
            this.iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);

        }

        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                if (this.busCtl == null)
                {
                    //初始化公交换乘控件,传入相关参数
                    busCtl = new BusAnalyse(this.markLayer, this.graphicsLayer);
                    this.busCtl.IsDrag = true;   //允许拖拽自定义控件
                    setTrack = new buscomponets.BusControl.setPlay();
                    this.setTrack.IsDrag = true;//允许拖拽自定义控件
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

        private void busSch_Click(object sender, RoutedEventArgs e)
        {
            if (busCtl!=null)
            {
             this.busCtl.SvrUrl = this.TileLayerAddress;
             this.busCtl.showCtl();
            }

        }

        private void unRemove(object sender, RoutedEventArgs e)
        {
            if (busCtl!=null)
            {
                busCtl.Close();
            }
            if (setTrack!=null)
            {
                setTrack.Close();
            }
        }
    }
}
