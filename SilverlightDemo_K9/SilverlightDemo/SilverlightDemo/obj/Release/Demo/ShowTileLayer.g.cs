﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\Demo\ShowTileLayer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "87A630EB249671D63796032CBF224B88"
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
using ZDIMS.Map;


namespace SilverlightDemo {
    
    
    public partial class ShowTileLayer : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/ShowTileLayer.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
        }
    }
}

