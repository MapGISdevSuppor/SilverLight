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
using ZDIMSDemo.Controls;
using ZDIMS.BaseLib;
using ZDIMS.Drawing;
using ZDIMS.Map;
using ZDIMS.Util;
using ZDIMS.Event;
using ZDIMS.Interface;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using ZDIMS.Util;

namespace SilverlightDemo
{
    public partial class DiffriendWindow : BaseUserControl
    {
        string TileLayerAddress = "";
        public DiffriendWindow()
        {
            InitializeComponent();
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
            iMSMap2.OperType = IMSOperType.Drag;
            tileLayerWh.ServerAddress = TileLayerAddress;
        }
    }
}
