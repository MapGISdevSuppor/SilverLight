﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\Demo\TheMaticDemo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DF1F6CC3670192109331B7C7E2E56808"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.296
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
using ZDIMS.Drawing;
using ZDIMS.Map;


namespace SilverlightDemo.Demo {
    
    
    public partial class TheMaticDemo : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
        internal ZDIMS.Drawing.MarkLayer markLayer1;
        
        internal System.Windows.Controls.Button addMatic;
        
        internal System.Windows.Controls.Button SandH;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/TheMaticDemo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
            this.markLayer1 = ((ZDIMS.Drawing.MarkLayer)(this.FindName("markLayer1")));
            this.addMatic = ((System.Windows.Controls.Button)(this.FindName("addMatic")));
            this.SandH = ((System.Windows.Controls.Button)(this.FindName("SandH")));
        }
    }
}
