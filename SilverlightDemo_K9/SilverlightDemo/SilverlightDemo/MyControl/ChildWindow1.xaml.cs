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
using System.Windows.Controls.Primitives;
using System.Windows.Browser;
using ZDIMS.Util;

namespace SilverlightDemo
{
    public partial class ChildWindow1 : ChildWindow
    {
        string TileLayerAddress = "";
        public ChildWindow1()
        {
            InitializeComponent();
        }       
        private void ChildWindow_Closed(object sender, EventArgs e)
        {
            Popup pop = (Popup)this.Parent;
            pop.IsOpen = false;
        }
        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Popup pop = (Popup)this.Parent;
            pop.IsOpen = false;
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

