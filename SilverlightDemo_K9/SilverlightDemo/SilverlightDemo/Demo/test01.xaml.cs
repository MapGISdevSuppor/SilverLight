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
using System.Windows.Browser;
using ZDIMS.Util;

namespace SilverlightDemo.Demo
{
    public partial class test01 : Page
    {
        string TileLayerAddress = "";
       // string VecLayerAddress = "";

        public test01()
        {
            InitializeComponent();
           // HtmlPage.RegisterScriptableObject("SilverlightFunction", this);
        }

      
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();
               // VecLayerAddress = HtmlPage.Window.Eval("VecLayerAddress").ToString();
            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            tileLayer1.ServerAddress = "http://192.168.17.78/RelayHandlerSite/RelayHandler.ashx";
   
        }

    }
}
