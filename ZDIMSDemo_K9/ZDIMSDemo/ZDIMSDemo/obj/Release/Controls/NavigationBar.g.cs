﻿#pragma checksum "E:\Code2010\ZDIMS\ZDIMS\ZDIMSDemo\Controls\NavigationBar.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2B89BFFE104B5328DCFB111266F67D41"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace ZDIMSDemo.Controls {
    
    
    public partial class NavigationBar : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image image_up;
        
        internal System.Windows.Controls.Image image_down;
        
        internal System.Windows.Controls.Image image_left;
        
        internal System.Windows.Controls.Image image_right;
        
        internal System.Windows.Controls.Image image_restore;
        
        internal System.Windows.Controls.Image image_zoomin;
        
        internal System.Windows.Controls.Image image_zoomout;
        
        internal System.Windows.Controls.Slider slider1;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/Controls/NavigationBar.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.image_up = ((System.Windows.Controls.Image)(this.FindName("image_up")));
            this.image_down = ((System.Windows.Controls.Image)(this.FindName("image_down")));
            this.image_left = ((System.Windows.Controls.Image)(this.FindName("image_left")));
            this.image_right = ((System.Windows.Controls.Image)(this.FindName("image_right")));
            this.image_restore = ((System.Windows.Controls.Image)(this.FindName("image_restore")));
            this.image_zoomin = ((System.Windows.Controls.Image)(this.FindName("image_zoomin")));
            this.image_zoomout = ((System.Windows.Controls.Image)(this.FindName("image_zoomout")));
            this.slider1 = ((System.Windows.Controls.Slider)(this.FindName("slider1")));
        }
    }
}

