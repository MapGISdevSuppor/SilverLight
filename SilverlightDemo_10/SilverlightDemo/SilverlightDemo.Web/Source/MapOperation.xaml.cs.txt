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
using System.Windows.Navigation;
using ZDIMS.Event;
using System.Windows.Browser;
using ZDIMS.Util;


namespace SilverlightDemo
{
    public partial class MapOperation : Page
    {
        string TileLayerAddress = "";
        public MapOperation()
        {
            InitializeComponent();
            iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);
        }

        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            Canvas.SetZIndex(imgwin, 180);
            Canvas.SetZIndex(img, 200);
        }
        //放大
        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            iMSMap1.OperType = ZDIMS.Util.IMSOperType.ZoomIn;
        }
        //缩小
        private void img2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            iMSMap1.OperType = ZDIMS.Util.IMSOperType.ZoomOut;
        }
        //移动
        private void img3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            iMSMap1.OperType = ZDIMS.Util.IMSOperType.Drag;
        }
        //复位
        private void img4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            iMSMap1.OperType = ZDIMS.Util.IMSOperType.Restore;
        }
        //刷新
        private void img5_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            iMSMap1.OperType = ZDIMS.Util.IMSOperType.Refresh;
        }
        //页面加载的时候给瓦片地图的地址ServerAddress赋值
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
