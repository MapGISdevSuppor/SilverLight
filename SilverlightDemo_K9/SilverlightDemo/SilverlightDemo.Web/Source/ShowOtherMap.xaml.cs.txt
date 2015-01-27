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

namespace SilverlightDemo
{
    public partial class ShowOtherMap : Page
    {
        public ShowOtherMap()
        {
            InitializeComponent();
            Canvas.SetZIndex(omap, 200);
        }
        //显示雅虎地图
        private void yahooBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            yahooTile.Display = true;
            bingTile.Display = false;
            googleTile.Display = false;
            iMSMap1.Restore();
        }
        //显示必应地图
        private void bingBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bingTile.Display = true;
            yahooTile.Display = false;
            googleTile.Display = false;
            iMSMap1.Restore();
        }
        //显示谷歌地图
        private void googleBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            googleTile.Display = true;
            bingTile.Display = false;
            yahooTile.Display = false;
            iMSMap1.Restore();
        }
        //页面加载的时候显示雅虎地图
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            googleTile.Display = false;
            bingTile.Display = false;
            yahooTile.Display = true;
        }
    }
}
