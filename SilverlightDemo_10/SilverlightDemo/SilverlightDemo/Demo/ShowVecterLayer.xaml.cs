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

namespace SilverlightDemo
{
    public partial class ShowVecterLayer : Page
    {
        string VecLayerAddress = "";
        public ShowVecterLayer()
        {
            InitializeComponent();
        }
        //页面加载的时候给瓦片地图的地址ServerAddress赋值
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                VecLayerAddress = HtmlPage.Window.Eval("VecLayerAddress").ToString();
            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            this.vectorMapDoc1.ServerAddress = VecLayerAddress;
        }
    }
}
