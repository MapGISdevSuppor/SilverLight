﻿#pragma checksum "E:\GIS\3-示例Demo\官网示例\官网开发案例\新建文件夹\基于Silverlight物流配送系统\客户端站点\Logistics_system\Logistics_system\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9887DBBCC7866AEF4D86E136F1850459"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Logistics_system.controls;
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
using ZDIMS.Drawing;
using ZDIMS.Map;
using ZDIMSDemo.Controls;


namespace Logistics_system {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Drawing.GraphicsLayer graphicsLayer1;
        
        internal ZDIMS.Drawing.MarkLayer markLayer1;
        
        internal ZDIMSDemo.Controls.NavigationBar bar;
        
        internal Logistics_system.controls.ToolBar toolBar1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Logistics_system;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.graphicsLayer1 = ((ZDIMS.Drawing.GraphicsLayer)(this.FindName("graphicsLayer1")));
            this.markLayer1 = ((ZDIMS.Drawing.MarkLayer)(this.FindName("markLayer1")));
            this.bar = ((ZDIMSDemo.Controls.NavigationBar)(this.FindName("bar")));
            this.toolBar1 = ((Logistics_system.controls.ToolBar)(this.FindName("toolBar1")));
        }
    }
}

