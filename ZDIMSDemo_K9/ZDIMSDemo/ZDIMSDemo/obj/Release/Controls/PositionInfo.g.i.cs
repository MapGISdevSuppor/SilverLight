﻿#pragma checksum "E:\Code2010\ZDIMS\ZDIMS\ZDIMSDemo\Controls\PositionInfo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5996D426EA76E0E02F8A061454ACA555"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
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
using ZDIMSDemo.Controls;


namespace ZDIMSDemo.Controls {
    
    
    public partial class PositionInfo : ZDIMSDemo.Controls.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel mydialogPanel;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock LabelMouseX;
        
        internal System.Windows.Controls.TextBlock LabelMouseY;
        
        internal System.Windows.Controls.TextBlock LableMouseLogicX;
        
        internal System.Windows.Controls.TextBlock LableMouseLogicY;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/Controls/PositionInfo.xaml", System.UriKind.Relative));
            this.mydialogPanel = ((EasySL.Controls.DialogPanel)(this.FindName("mydialogPanel")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LabelMouseX = ((System.Windows.Controls.TextBlock)(this.FindName("LabelMouseX")));
            this.LabelMouseY = ((System.Windows.Controls.TextBlock)(this.FindName("LabelMouseY")));
            this.LableMouseLogicX = ((System.Windows.Controls.TextBlock)(this.FindName("LableMouseLogicX")));
            this.LableMouseLogicY = ((System.Windows.Controls.TextBlock)(this.FindName("LableMouseLogicY")));
        }
    }
}

