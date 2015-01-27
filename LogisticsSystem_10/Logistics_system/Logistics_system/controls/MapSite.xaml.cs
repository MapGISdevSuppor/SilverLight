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
//using SuperMap.Web.Mapping;
using System.Windows.Media.Imaging;
//using SuperMap.Web.Core;
using Logistics_system.controls;
using ZDIMSDemo.Controls;
namespace Logistics_system
{
    public partial class MapSite : BaseUserControl
    {
        DetailsForms form;
        public List<marketsInfo> searchRlt;
        int MarketID;
        public MapSite()
        {
            InitializeComponent();
           
            Thickness myThickness = new Thickness();
            myThickness.Bottom = (double)-216;
            myThickness.Left = (double)-18;
            myThickness.Right = (double)0;
            myThickness.Top = (double)0;
            this.SetValue(UserControl.MarginProperty, myThickness);
        }

        public MapSite(Image  img,string name,string ID,string address,string xiangxi)
        {
            img.MouseEnter += new MouseEventHandler(img_MouseEnter);
            img.MouseLeave += new MouseEventHandler(img_MouseLeave);
            InitializeComponent();
            this.UserType.Children.Add(img);
            setLocation(name,ID,address,xiangxi);
        }

        void img_MouseLeave(object sender, MouseEventArgs e)
        {
            Image s = sender as Image;
            s.Width = 38 ;
            s.Height = 48;
        }

        void img_MouseEnter(object sender, MouseEventArgs e)
        {
            Image s = sender as Image;
            s.Width = 40;
            s.Height = 50;
        }
        //public MapSite(StrobeIcon strobeIcon, string name, string tel, string address, string xiangxi)
        //{
        //    InitializeComponent();
        //    this.UserType.Children.Add(strobeIcon);
        //    setLocation(name, tel, address, xiangxi);
        //}
        //public MapSite(CustomCross customCross, string name, string tel, string address, string xiangxi)
        //{
        //    InitializeComponent();
        //    this.UserType.Children.Add(customCross);
        //    setLocation(name, tel, address, xiangxi);
        //}

        public void setLocation(string name, string id, string address, string xiangxi)
        {
            this.txtblockTitle.Text = "名称：" + name;
            this.txtblockTel.Text = "门店代号：" + id;
            MarketID = Convert.ToInt32(id);
            this.txtblockAddress.Text = "地址：" + address;
            this.txtblockXiangxi.Text = "详细信息";
            this.txtblockXiangxi.Cursor = Cursors.Hand;
            this.txtblockXiangxi.MouseEnter += new MouseEventHandler(MapSite_MouseEnter);
            this.txtblockXiangxi.MouseLeave += new MouseEventHandler(txtblockXiangxi_MouseLeave);
            this.txtblockXiangxi.MouseLeftButtonUp += new MouseButtonEventHandler(txtblockXiangxi_MouseLeftButtonUp);
            this.txtblockTitle.Visibility = Visibility.Collapsed;
            this.btnClose.Visibility = Visibility.Collapsed;
            this.imgHeader.Visibility = Visibility.Collapsed;

            Thickness myThickness = new Thickness();
            myThickness.Bottom = (double)-216;
            myThickness.Left = (double)-18;
            myThickness.Right = (double)0;
            myThickness.Top = (double)0;

            this.SetValue(UserControl.MarginProperty, myThickness); 
        }

        void txtblockXiangxi_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (form == null)
            {
                form = new DetailsForms();
            }
            form.MarketID = MarketID;//Convert.ToInt32(this.txtblockTel.Text); ; //this.searchRlt[index].MarketID;
            form.Show();    
        }

        void txtblockXiangxi_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock t = sender as TextBlock;
            t.Foreground = new SolidColorBrush(Colors.Black);//设置文本颜色
        }

        void MapSite_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock t = sender as TextBlock;
            t.Foreground = new SolidColorBrush(Colors.Blue);//设置文本颜色
        }
        private bool f = false;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.txtblockTitle.Visibility = Visibility.Visible;
            this.btnClose.Visibility = Visibility.Visible;
            this.imgHeader.Visibility = Visibility.Visible;

            this.p.IsOpen = true;
            Storyboard stb = Resources["stbShowHeader"] as Storyboard;
            stb.Completed += new EventHandler(stb_Completed);
            stb.Begin();
        }
        void stb_Completed(object sender, EventArgs e)
        {
            Storyboard stbImgBg = Resources["stbImgBg"] as Storyboard;
            stbImgBg.Completed += new EventHandler(stbImgBg_Completed);
            stbImgBg.Begin();
        }

        void stbImgBg_Completed(object sender, EventArgs e)
        {
            Storyboard stbContentBg =Resources["stbContentBg"] as Storyboard;
            stbContentBg.Begin();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Storyboard stbContentBgBack = Resources["stbContentBgBack"] as Storyboard;
            stbContentBgBack.Completed += new EventHandler(stbContentBgBack_Completed);
            stbContentBgBack.Begin();
            this.p.IsOpen = false;
            
            this.txtblockTitle.Visibility = Visibility.Collapsed;
            this.btnClose.Visibility = Visibility.Collapsed;
            this.imgHeader.Visibility = Visibility.Collapsed;
        }

        void stbContentBgBack_Completed(object sender, EventArgs e)
        {
            Storyboard stbImgBgBack = Resources["stbImgBgBack"] as Storyboard;
            stbImgBgBack.Completed += new EventHandler(stbImgBgBack_Completed);
            stbImgBgBack.Begin();
        }
        void stbImgBgBack_Completed(object sender, EventArgs e)
        {
            Storyboard stbShowHeaderBack = Resources["stbShowHeaderBack"] as Storyboard;
            stbShowHeaderBack.Begin();
        }
    }
}
