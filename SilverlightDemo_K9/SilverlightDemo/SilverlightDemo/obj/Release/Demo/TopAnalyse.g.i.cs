﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\Demo\TopAnalyse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A689A53A9976637613E8759A29BA347A"
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
using ZDIMSDemo.Controls;


namespace SilverlightDemo.Demo {
    
    
    public partial class TopAnalyse : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
        internal ZDIMS.Map.VectorMapDoc mapDoc;
        
        internal System.Windows.Controls.Button topButton1;
        
        internal System.Windows.Controls.Button topButton2;
        
        internal System.Windows.Controls.Button topButton3;
        
        internal System.Windows.Controls.Button clearBuf;
        
        internal System.Windows.Controls.Label topResult;
        
        internal ZDIMSDemo.Controls.IMSCatalog iMSCatalog1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/TopAnalyse.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
            this.mapDoc = ((ZDIMS.Map.VectorMapDoc)(this.FindName("mapDoc")));
            this.topButton1 = ((System.Windows.Controls.Button)(this.FindName("topButton1")));
            this.topButton2 = ((System.Windows.Controls.Button)(this.FindName("topButton2")));
            this.topButton3 = ((System.Windows.Controls.Button)(this.FindName("topButton3")));
            this.clearBuf = ((System.Windows.Controls.Button)(this.FindName("clearBuf")));
            this.topResult = ((System.Windows.Controls.Label)(this.FindName("topResult")));
            this.iMSCatalog1 = ((ZDIMSDemo.Controls.IMSCatalog)(this.FindName("iMSCatalog1")));
        }
    }
}

