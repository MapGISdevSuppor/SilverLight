﻿#pragma checksum "F:\02-Silverlight_Demo\Sl集锦\Logistics_system\Logistics_system\chart\ChartControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E43888F3AEC25F546D5721DBFD567061"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.239
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using EasySL.Controls;
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
using Visifire.Charts;
using ZDIMSDemo.Controls;


namespace Logistics_system.chart {
    
    
    public partial class ChartControl : ZDIMSDemo.Controls.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel dialogPanel1;
        
        internal System.Windows.Controls.Canvas canvas;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Button buffer;
        
        internal System.Windows.Controls.Button cancle;
        
        internal System.Windows.Controls.ComboBox xaxis;
        
        internal System.Windows.Controls.ComboBox yaxis;
        
        internal Visifire.Charts.Chart mychart;
        
        internal System.Windows.Controls.Image image1;
        
        internal System.Windows.Controls.Image image2;
        
        internal System.Windows.Controls.RadioButton radioButton1;
        
        internal System.Windows.Controls.RadioButton radioButton2;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Logistics_system;component/chart/ChartControl.xaml", System.UriKind.Relative));
            this.dialogPanel1 = ((EasySL.Controls.DialogPanel)(this.FindName("dialogPanel1")));
            this.canvas = ((System.Windows.Controls.Canvas)(this.FindName("canvas")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.buffer = ((System.Windows.Controls.Button)(this.FindName("buffer")));
            this.cancle = ((System.Windows.Controls.Button)(this.FindName("cancle")));
            this.xaxis = ((System.Windows.Controls.ComboBox)(this.FindName("xaxis")));
            this.yaxis = ((System.Windows.Controls.ComboBox)(this.FindName("yaxis")));
            this.mychart = ((Visifire.Charts.Chart)(this.FindName("mychart")));
            this.image1 = ((System.Windows.Controls.Image)(this.FindName("image1")));
            this.image2 = ((System.Windows.Controls.Image)(this.FindName("image2")));
            this.radioButton1 = ((System.Windows.Controls.RadioButton)(this.FindName("radioButton1")));
            this.radioButton2 = ((System.Windows.Controls.RadioButton)(this.FindName("radioButton2")));
        }
    }
}
