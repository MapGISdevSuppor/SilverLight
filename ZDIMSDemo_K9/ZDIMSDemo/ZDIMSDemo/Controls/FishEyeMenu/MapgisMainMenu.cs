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
using ZDIMS.Drawing;

namespace ZDIMSDemo.Controls.FishEyeMenu
{
    public partial class MapgisMainMenu :FishEyeMenuV
    {
        private IMSMap imsMap = null;
        private MapgisToolSubMenu subMenu = null;
        private FloatTreeView floatTree = null;
        private GraphicsLayer graphicLayer = null;
        private MarkLayer markLayer = null;
        //private NavigationBar navBar = null;

        public NavigationBar NavBar
        {
            get;
            set;
        }

        public IMSEagleEye EagleEye
        {
            get;
            set;
        }

        public MarkLayer MarkLayer
        {
            get { return markLayer; }
            set { markLayer = value; }
        }

        public GraphicsLayer GraphicLayer
        {
            get { return graphicLayer; }
            set { graphicLayer = value; }
        }

        public IMSMap ImsMap
        {
            get { return imsMap; }
            set { imsMap = value; }
        }

        public FloatTreeView FloatTree
        {
            get
            {
                if (this.floatTree == null)
                {
                    this.floatTree = new FloatTreeView();
                }
                return floatTree; 
            }
            set { floatTree = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MapgisMainMenu()
        {
            this.Loaded += new RoutedEventHandler(AddClickEvents);
        }


        private void CreateSubMenu()
        {
            if (subMenu == null)
            {
                if (this.ImsMap != null)
                {
                    subMenu = new MapgisToolSubMenu();
                    subMenu.MarkLayer = this.MarkLayer;
                    subMenu.GraphicsLayer = this.GraphicLayer;
                    subMenu.ImsMap = this.ImsMap;
                    subMenu. EagleEye = this. EagleEye;
                    subMenu.IMSCatalog = FloatTree.MapTree;
                    subMenu. Navigator = NavBar;
                    subMenu.MouseLeftButtonDown += new MouseButtonEventHandler(subMenu_MouseLeftButtonDown);
                    this.ImsMap.AddChild(subMenu);
                    Canvas.SetZIndex(subMenu, 20000);
                }
            }
        }

        void subMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void AddClickEvents(object sender,RoutedEventArgs e)
        {
            String[] mainMenuPics = {"images/widget/MainMenu/Display.png","images/widget/MainMenu/Select.png",
                            "images/widget/MainMenu/Edit.png","images/widget/MainMenu/Anlysis.png","images/widget/MainMenu/View.png",
                            "images/widget/MainMenu/DataTree.png"};
            String[] mainMenuToolTip = {"显示","查询","编辑","分析","视图","图层控制树"};
            this. CreateControl( mainMenuPics , mainMenuToolTip );
            List<Image> pics = this.ImageItems;
            pics[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnDisplayItem_Click);
            pics[1].MouseLeftButtonDown += new MouseButtonEventHandler(OnSelectItem_Click);
            pics[2].MouseLeftButtonDown += new MouseButtonEventHandler(OnEditItem_Click);
            pics[3].MouseLeftButtonDown += new MouseButtonEventHandler(OnAnlysisItem_Click);
            pics[4].MouseLeftButtonDown += new MouseButtonEventHandler(OnViewItem_Click);
            pics[5].MouseLeftButtonDown += new MouseButtonEventHandler(OnFloatMapTreeDisplay_Click);

        }

        private void OnDisplayItem_Click(object sender, MouseButtonEventArgs e)
        {
            ShowDisplaySubMenu( );
        }

        public void ShowDisplaySubMenu ( )
        {
            this. CreateSubMenu( );
            this. subMenu. MenuFun = EnumMenuFun. Display;
        }

        private void OnSelectItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.Select;
        }

        private void OnViewItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.View;
        }

        private void OnAnlysisItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.Anlysis;
        }

        private void OnEditItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.CreateSubMenu();
            this.subMenu.MenuFun = EnumMenuFun.Editor;
        }

        private void OnFloatMapTreeDisplay_Click(object sender, MouseButtonEventArgs e)
        {
            if (imsMap != null)
            {
                FloatTreeView tempFloatTree = this.FloatTree;
                if (!tempFloatTree.IsPopedUp)
                {
                    tempFloatTree. MapTree. MapContainer = this. ImsMap;
                    tempFloatTree. SetValue( Canvas. ZIndexProperty , 9999 );
                    tempFloatTree.Show();
                    tempFloatTree.IsPopedUp = true;
                }
                else
                {
                    floatTree.Close();
                    tempFloatTree.IsPopedUp = false;
                }
            }
        }

        private void LocationSubMenu()
        {
            if (this.subMenu == null)
                return;
            this.subMenu.Visibility = Visibility.Visible;
            double locationY = 10;
            double locationX = this.ImsMap.Width / 2 - this.subMenu.Width / 2;
            this.subMenu.SetValue(Canvas.TopProperty, locationY);
            this.subMenu.SetValue(Canvas.LeftProperty, locationX);
        }
        public void ReLocation()
        {
            if (!double.IsNaN(this.Width))
            {
                double locationX = this.ImsMap.Width - Width;
                double locationY = this.ImsMap.Height / 2 - Height / 2;
                this.SetValue(Canvas.TopProperty, locationY);
                this.SetValue(Canvas.LeftProperty, locationX);
                LocationSubMenu();
            }
        }
    }
}
