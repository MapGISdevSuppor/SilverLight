﻿#pragma checksum "D:\MyProjects\SilverlightPlate\ZDIMS\ZDIMSDemo\FishTest.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1121690F232CFAE05E9C7B8EEA889949"
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
    
    
    public partial class FishTest : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button showBtn;
        
        internal System.Windows.Controls.Border Popup;
        
        internal System.Windows.Controls.Button hideBtn;
        
        internal System.Windows.Media.Animation.Storyboard ShowPopup;
        
        internal System.Windows.Media.Animation.Storyboard HidePopup;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/FishTest.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.showBtn = ((System.Windows.Controls.Button)(this.FindName("showBtn")));
            this.Popup = ((System.Windows.Controls.Border)(this.FindName("Popup")));
            this.hideBtn = ((System.Windows.Controls.Button)(this.FindName("hideBtn")));
            this.ShowPopup = ((System.Windows.Media.Animation.Storyboard)(this.FindName("ShowPopup")));
            this.HidePopup = ((System.Windows.Media.Animation.Storyboard)(this.FindName("HidePopup")));
        }
    }
}

