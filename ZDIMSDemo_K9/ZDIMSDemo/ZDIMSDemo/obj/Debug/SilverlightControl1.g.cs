﻿#pragma checksum "F:\Code2010\ZDIMS\ZDIMS\ZDIMSDemo\SilverlightControl1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9987BE416134CBDEB88FE75EADA7360F"
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


namespace ZDIMSDemo {
    
    
    public partial class SilverlightControl1 : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.VisualStateGroup MouseOverStates;
        
        internal System.Windows.VisualState MouseIsOverTrue;
        
        internal System.Windows.VisualState MouseIsOverFalse;
        
        internal System.Windows.Controls.Image ThumbnailImage;
        
        internal System.Windows.Controls.Button ZoomBtn;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/SilverlightControl1.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MouseOverStates = ((System.Windows.VisualStateGroup)(this.FindName("MouseOverStates")));
            this.MouseIsOverTrue = ((System.Windows.VisualState)(this.FindName("MouseIsOverTrue")));
            this.MouseIsOverFalse = ((System.Windows.VisualState)(this.FindName("MouseIsOverFalse")));
            this.ThumbnailImage = ((System.Windows.Controls.Image)(this.FindName("ThumbnailImage")));
            this.ZoomBtn = ((System.Windows.Controls.Button)(this.FindName("ZoomBtn")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
        }
    }
}

