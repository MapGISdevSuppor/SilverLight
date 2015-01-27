using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Logistics_system;
using Logistics_system.chart1;
using ZDIMS.Interface;
using ZDIMS.Drawing;
using ZDIMS.Event;
using Logistics_system.controls;
namespace Logistics_system
{
    public partial class SilverlightControl2 : UserControl
    {
        public SilverlightControl2()
        {
            InitializeComponent();
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.p1.IsOpen = true;
        }

        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.p1.IsOpen = false;
        }

        
    }
}
