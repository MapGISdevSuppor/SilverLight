﻿#pragma checksum "E:\WorkSpaceForSilverlight\MapGIS 10 space\SilverlightDemo\SilverlightDemo\buscomponets\BusControl\setPlay.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4BFADB9FD295F38E874DF4C2A31E3AB8"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using SilverlightDemo.buscomponets;
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


namespace SilverlightDemo.buscomponets.BusControl {
    
    
    public partial class setPlay : SilverlightDemo.buscomponets.BaseUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Canvas dialogPanel1;
        
        internal System.Windows.Controls.Image close;
        
        internal System.Windows.Controls.Slider slider1;
        
        internal System.Windows.Controls.TextBlock tooltip1;
        
        internal System.Windows.Controls.Image pause;
        
        internal System.Windows.Controls.TextBlock tooltip;
        
        internal System.Windows.Controls.Image stop;
        
        internal System.Windows.Controls.Label label1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/buscomponets/BusControl/setPlay.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dialogPanel1 = ((System.Windows.Controls.Canvas)(this.FindName("dialogPanel1")));
            this.close = ((System.Windows.Controls.Image)(this.FindName("close")));
            this.slider1 = ((System.Windows.Controls.Slider)(this.FindName("slider1")));
            this.tooltip1 = ((System.Windows.Controls.TextBlock)(this.FindName("tooltip1")));
            this.pause = ((System.Windows.Controls.Image)(this.FindName("pause")));
            this.tooltip = ((System.Windows.Controls.TextBlock)(this.FindName("tooltip")));
            this.stop = ((System.Windows.Controls.Image)(this.FindName("stop")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
        }
    }
}

