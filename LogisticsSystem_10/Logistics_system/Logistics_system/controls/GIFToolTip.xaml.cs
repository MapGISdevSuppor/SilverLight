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
using System.Windows.Resources;
namespace Logistics_system.controls
{
    public partial class GIFToolTip : UserControl
    {
        public GIFToolTip()
        {
            InitializeComponent();
            StreamResourceInfo sr = Application.GetResourceStream(new Uri("/Logistics_system;component/images/flashpoint.gif", UriKind.Relative));
            this.gifExample.SetSoruce(sr.Stream);
        }

        public void Addtip(marketsInfo content)
        {
            
            this.TextID.Text = content.MarketID.ToString();
            this.TextName.Text = content.MarketName;
            this.TextAddr.Text = content.Address;//
            this.Text.Text="补货";
           //this.tip.Text = content;
        }
    }
}
