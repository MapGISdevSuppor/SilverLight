﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\components\MarkInfo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4A3FC9588BB45D27A297B0BD37954949"
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
using SilverlightDemo.components;
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


namespace SilverlightDemo.components {
    
    
    public partial class MarkInfo : SilverlightDemo.components.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel dialogPanel1;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.TextBlock placename;
        
        internal System.Windows.Controls.TextBlock xInfo;
        
        internal System.Windows.Controls.TextBlock yInfo;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/components/MarkInfo.xaml", System.UriKind.Relative));
            this.dialogPanel1 = ((EasySL.Controls.DialogPanel)(this.FindName("dialogPanel1")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.placename = ((System.Windows.Controls.TextBlock)(this.FindName("placename")));
            this.xInfo = ((System.Windows.Controls.TextBlock)(this.FindName("xInfo")));
            this.yInfo = ((System.Windows.Controls.TextBlock)(this.FindName("yInfo")));
        }
    }
}

