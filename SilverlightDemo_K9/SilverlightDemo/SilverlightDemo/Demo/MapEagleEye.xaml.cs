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
using ZDIMS.Util;
using System.Windows.Browser;

namespace SilverlightDemo
{
    public partial class MapEagleEye : Page
    {
         string TileLayerAddress = "";
        public MapEagleEye()
        {
            InitializeComponent();
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
            tileLayer2.ServerAddress=TileLayerAddress;
        }
    }
}
