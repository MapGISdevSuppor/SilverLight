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
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using ZDIMSDemo.Controls;
namespace Logistics_system
{
    public partial class EditMark : BaseUserControl
    {
       
        public RichTextBox txtInfo = null;
        DetailsForms form;
        public List<marketsInfo> searchRlt;
        int MarketID;
        private TextBox textInfo;
        private string name, ID, address, xiangxi;
        Border b;
        /// <summary>
        /// 标注信息
        /// </summary>
        public string MarkInfo { get { return textInfo.Text; } set { textInfo.Text = value; } }
        public EditMark(string name, string ID, string address, string xiangxi)
        {
            InitializeComponent(); 
            this.name = name;
            this.ID = ID;
            this.address = address;
            this.xiangxi = xiangxi;
          
            
        }     
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
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.hiddenInfo();
        }
        void txtblockXiangxi_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (form == null)
            {
                form = new DetailsForms();
            }
            form.MarketID = MarketID;
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
        void img_MouseLeave(object sender, MouseEventArgs e)
        {
            Image s = sender as Image;
            s.Width = 38;
            s.Height = 48;
        }

        void img_MouseEnter(object sender, MouseEventArgs e)
        {
            Image s = sender as Image;
            s.Width = 40;
            s.Height = 50;
        }
        void markImg_MouseLeave(object sender, MouseEventArgs e)
        {
            hiddenInfo();
           
        }
     
        
      
       
      

        public void hiddenInfo()
        {
            this.p1.IsOpen = false;
           
        
        }
        public void showInfo()
        {
            this.p1.IsOpen = true;

            //Canvas.SetLeft(this.LayoutRoot1, this.img.Width * 0.5);
            //Canvas.SetTop(this.LayoutRoot1, this.img.Height);
           
        }
        Popup p;
        /// <summary>
        /// 显示标注信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void markImg_MouseEnter(object sender, MouseEventArgs e)
        {

            showInfo();
           


        }
   
       

        #region 信息框
        void close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            p1.IsOpen = false;
            
        }
      
       
        void stb_Completed(object sender, EventArgs e)
        {
            Storyboard stbImgBg = Resources["stbImgBg"] as Storyboard;
            stbImgBg.Completed += new EventHandler(stbImgBg_Completed);
            stbImgBg.Begin();
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
        void stbImgBg_Completed(object sender, EventArgs e)
        {
            Storyboard stbContentBg = Resources["stbContentBg"] as Storyboard;
            stbContentBg.Begin();
        }
        private void EditBaseContainer_Loaded(object sender, RoutedEventArgs e)
        {
            this.img.Width = this.Width;
            this.img.Height = this.Height;
          
           
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


            this.showInfo();
            setLocation(name, ID, address, xiangxi);
        }
        #endregion
        #region 废弃
       
        #endregion
    }
}
