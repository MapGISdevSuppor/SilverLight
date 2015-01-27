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

using EasySL.Controls;

namespace ZDIMSDemo.Controls
{
    public partial class FloatTreeView :BaseUserControl
    {
        private bool isPopedUp = false;

        public bool IsPopedUp
        {
            get { return isPopedUp; }
            set { isPopedUp = value; }
        }

        public FloatTreeView()
        {
            InitializeComponent();
            diapnl.DragEnable=true;
            diapnl.OnClose = new RoutedEventHandler(OnMapTreeClose);
        }

        private void OnMapTreeClose(object sender, RoutedEventArgs e)
        {
            this.Close();
            IsPopedUp = false;
        }
    }
}
