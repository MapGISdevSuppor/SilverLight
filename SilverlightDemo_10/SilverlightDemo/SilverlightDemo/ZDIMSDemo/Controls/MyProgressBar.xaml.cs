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

namespace ZDIMSDemo.Controls
{
    public partial class MyProgressBar : ChildWindow
    {
        public string TextInfo
        {
            get { return textBlock1.Text; }
            set { textBlock1.Text = value; }
        }
        public MyProgressBar(string txtInfo = "更新图层状态中,请稍后..")
        {
            InitializeComponent();
            textBlock1.Text = txtInfo;
        }
    }
}

