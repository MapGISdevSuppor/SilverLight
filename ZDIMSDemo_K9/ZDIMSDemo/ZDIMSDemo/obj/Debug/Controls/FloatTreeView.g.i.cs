﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\ZDIMSDemo\ZDIMSDemo\Controls\FloatTreeView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0B770229E219E608B77701A441ABC448"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
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
    
    
    public partial class FloatTreeView : ZDIMSDemo.Controls.BaseUserControl {
        
        internal ZDIMSDemo.Controls.BaseUserControl baseCtr;
        
        internal EasySL.Controls.DialogPanel diapnl;
        
        internal ZDIMSDemo.Controls.IMSCatalog MapTree;
        
        internal System.Windows.Media.Animation.Storyboard showFade1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/Controls/FloatTreeView.xaml", System.UriKind.Relative));
            this.baseCtr = ((ZDIMSDemo.Controls.BaseUserControl)(this.FindName("baseCtr")));
            this.diapnl = ((EasySL.Controls.DialogPanel)(this.FindName("diapnl")));
            this.MapTree = ((ZDIMSDemo.Controls.IMSCatalog)(this.FindName("MapTree")));
            this.showFade1 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("showFade1")));
        }
    }
}

