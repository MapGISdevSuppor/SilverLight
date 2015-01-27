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

using ZDIMS.Map;
using ZDIMSDemo.Controls.FishEyeMenu;

namespace ZDIMSDemo.Controls.FishEyeToolMenu
{
    public partial class MapgisMainMenu : UserControl
    {
        private IMSMap imsMap = null;
        private FishEyeMenuV fishMenu = null;
        private MapgisToolSubMenu subMenu = null;

        public IMSMap ImsMap
        {
            get { return imsMap; }
            set { imsMap = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MapgisMainMenu()
        {
            InitializeComponent();
            fishMenu = new FishEyeMenuV();
            this.LayoutRoot.Children.Add(fishMenu);
            this.Loaded += new RoutedEventHandler(ResizeControl);
            this.AddClickEvents();
        }

        private void ResizeControl(object sender,RoutedEventArgs e)
        {
            this.c1.Width = fishMenu.Width;
            this.c1.Height = fishMenu.Height;
        }

        private void CreateSubMenu()
        {
            if (subMenu == null)
            {
                subMenu = new MapgisToolSubMenu();
                subMenu.ImsMap = this.ImsMap;
                if (this.ImsMap != null)
                {
                    this.ImsMap.AddChild(subMenu);
                    subMenu.SetValue(Canvas.ZIndexProperty, 10000);
                    subMenu.Visibility = Visibility.Collapsed;
                }
            } 
        }

        private void AddClickEvents()
        {
            if (this.fishMenu != null)
            {
                List<Image> pics = this.fishMenu.ImageItems;
                pics[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnDisplayItem_Click);
                pics[1].MouseLeftButtonDown += new MouseButtonEventHandler(OnSelectItem_Click);
                pics[2].MouseLeftButtonDown += new MouseButtonEventHandler(OnEditItem_Click);
                pics[3].MouseLeftButtonDown += new MouseButtonEventHandler(OnAnlysisItem_Click);
                pics[4].MouseLeftButtonDown += new MouseButtonEventHandler(OnViewItem_Click);
             }
        }

        private void OnDisplayItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.Display;
            LocationSubMenu();
        }

        private void OnSelectItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.Select;
            LocationSubMenu();
        }

        private void OnViewItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.View;
            LocationSubMenu();
        }

        private void OnAnlysisItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.Anlysis;
            LocationSubMenu();
        }

        private void OnEditItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.Editor;
            LocationSubMenu();
        }

        private void LocationSubMenu()
        {
            this.subMenu.Visibility = Visibility.Visible;
            double locationY = this.ImsMap.Height - this.subMenu.Height;
            double locationX = this.ImsMap.Width / 2 - this.subMenu.Width / 2;
            this.subMenu.SetValue(Canvas.TopProperty, locationY);
            this.subMenu.SetValue(Canvas.LeftProperty, locationX);
        }
    }
}
