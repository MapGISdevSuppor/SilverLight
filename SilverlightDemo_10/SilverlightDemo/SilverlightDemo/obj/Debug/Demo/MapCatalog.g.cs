﻿#pragma checksum "E:\WorkSpaceForSilverlight\MapGIS 10 space\SilverlightDemo\SilverlightDemo\Demo\MapCatalog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9F68E8889FB4FB504727720BE8B85EBE"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
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


namespace SilverlightDemo {
    
    
    public partial class MapCatalog : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMapCatag;
        
        internal ZDIMS.Map.VectorMapDoc vectorMapDoc1;
        
        internal System.Windows.Controls.ChildWindow cwin;
        
        internal ZDIMSDemo.Controls.IMSCatalog iMSCatalog1;
        
        internal System.Windows.Controls.Canvas img;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/MapCatalog.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMapCatag = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMapCatag")));
            this.vectorMapDoc1 = ((ZDIMS.Map.VectorMapDoc)(this.FindName("vectorMapDoc1")));
            this.cwin = ((System.Windows.Controls.ChildWindow)(this.FindName("cwin")));
            this.iMSCatalog1 = ((ZDIMSDemo.Controls.IMSCatalog)(this.FindName("iMSCatalog1")));
            this.img = ((System.Windows.Controls.Canvas)(this.FindName("img")));
        }
    }
}

