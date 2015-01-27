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
using System.Windows.Controls.Primitives;
using System.Windows.Browser;
using ZDIMS.Util;

namespace SilverlightDemo
{
    public partial class MapCatalog : Page
    {
        string VecLayerAddress = "";
        public MapCatalog()
        {
            InitializeComponent();
            iMSMapCatag.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);
        }

        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                iMSCatalog1.ActiveMapDoc = vectorMapDoc1;
                iMSCatalog1.OnlyVecFlag = 1;
                iMSCatalog1.MapContainer = iMSMapCatag;   
                Canvas.SetZIndex(cwin, 2000);
                Canvas.SetZIndex(img, 2001); 
                cwin.Margin = new Thickness(20, 40, 0, 0);
                cwin.Visibility = System.Windows.Visibility.Collapsed;
                
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (cwin.Visibility == System.Windows.Visibility.Visible)
            {
                cwin.Visibility = System.Windows.Visibility.Collapsed;
                 cwin.Margin = new Thickness(20,40,0,0);
            }
            else
            {
                cwin.Visibility = System.Windows.Visibility.Visible;
                cwin.Margin = new Thickness(20, 40, 0, 0);
            }
        }

        private void cwin_Closing(object sender, CancelEventArgs e)
        {
            cwin.Visibility = System.Windows.Visibility.Collapsed;
        }

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
            iMSMapCatag.OperType = IMSOperType.Drag;
            this.vectorMapDoc1.ServerAddress = VecLayerAddress;
        }
    }
}
