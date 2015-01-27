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
using Logistics_system;
using Logistics_system.controls;
namespace Logistics_system.controls
{
    public partial class tooltip : UserControl
    {

        DetailsForms form = new DetailsForms();
        public tooltip()
        {
            InitializeComponent();
        }

        public void Addtip(marketsInfo content)
        {
            this.TextID.Text = content.MarketID.ToString();
            this.TextName.Text = content.MarketName;
            this.TextAddr.Text = content.Address;
            //this.tip.Text = content;
        }

        private void detail_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            form.MarketID = Convert.ToInt32(this.TextID);
            form.Show();
        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            Image s=sender as Image;
            s.Width=44;
            s.Height=54;
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            Image s = sender as Image;
            s.Width = 38;
            s.Height = 45;
        }
    }
}
