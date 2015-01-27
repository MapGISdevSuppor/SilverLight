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
using ZDIMS.Event;
using ZDIMS.Util;
using System.Windows.Browser;
namespace SilverlightDemo
{
    public partial class MapCommonControls : Page
    {
        string TileLayerAddress = "";
        private ZDIMSDemo.Controls.NavigationBar m_navigator;
        private ZDIMSDemo.Controls.Scale iScale;
        public MapCommonControls()
        {
            InitializeComponent();
            iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
        }
        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                //鹰眼赋值
                iMSEagleEye1.MapContainer = iMSMap1;
                iMSEagleEye1.LevelDiff = 1;
                iMSEagleEye1.LevelNum = 6;
                Canvas.SetZIndex(iMSEagleEye1, Canvas.GetZIndex(this.iMSMap1) + 12);
                //添加比例尺
                iScale = new ZDIMSDemo.Controls.Scale();
                iScale.MapContainer = iMSMap1;
                iMSMap1.AddChild(iScale);
                Canvas.SetZIndex(iScale, Canvas.GetZIndex(this.iMSMap1) + 12);
                //添加导航条
                m_navigator = new ZDIMSDemo.Controls.NavigationBar();
                m_navigator.MapContainer = this.iMSMap1;
                this.iMSMap1.AddChild(this.m_navigator);
                Canvas.SetZIndex(this.m_navigator, Canvas.GetZIndex(this.iMSMap1)+12);
               iMSMap1.Restore();
               
            }
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
