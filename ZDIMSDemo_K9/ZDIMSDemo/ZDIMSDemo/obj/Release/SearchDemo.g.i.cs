﻿#pragma checksum "E:\Code2010\ZDIMS\ZDIMS\ZDIMSDemo\SearchDemo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C3086D9662F7AC12F3F18C716EA4C458"
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
using ZDIMS.Drawing;
using ZDIMS.Map;
using ZDIMSDemo.Controls;


namespace ZDIMSDemo {
    
    
    public partial class SearchDemo : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMSDemo.Controls.Toolbar toolbar1;
        
        internal System.Windows.Controls.Border leftPanel;
        
        internal System.Windows.Controls.Image expand;
        
        internal System.Windows.Controls.Image collapse;
        
        internal System.Windows.Controls.Grid leftMenuPanel;
        
        internal ZDIMSDemo.Controls.IMSCatalog iMSCatalog1;
        
        internal System.Windows.Controls.Border rightPanel;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
        internal ZDIMS.Map.IMSEagleEye iMSEagleEye1;
        
        internal ZDIMS.Map.VectorMapDoc vectorMapDoc2;
        
        internal ZDIMS.Map.VectorLayer vectorLayer1;
        
        internal ZDIMS.Map.GDBItem gDBItem1;
        
        internal ZDIMS.Map.LayerItem layerItem1;
        
        internal ZDIMS.Map.LayerItem layerItem2;
        
        internal ZDIMS.Map.LayerItem layerItem3;
        
        internal ZDIMS.Drawing.GraphicsLayer graphicsLayer1;
        
        internal ZDIMS.Drawing.MarkLayer markLayer1;
        
        internal ZDIMS.Drawing.MarkLayer markLayer2;
        
        internal System.Windows.Media.Animation.Storyboard hideMenu;
        
        internal System.Windows.Media.Animation.Storyboard showMenu;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/SearchDemo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.toolbar1 = ((ZDIMSDemo.Controls.Toolbar)(this.FindName("toolbar1")));
            this.leftPanel = ((System.Windows.Controls.Border)(this.FindName("leftPanel")));
            this.expand = ((System.Windows.Controls.Image)(this.FindName("expand")));
            this.collapse = ((System.Windows.Controls.Image)(this.FindName("collapse")));
            this.leftMenuPanel = ((System.Windows.Controls.Grid)(this.FindName("leftMenuPanel")));
            this.iMSCatalog1 = ((ZDIMSDemo.Controls.IMSCatalog)(this.FindName("iMSCatalog1")));
            this.rightPanel = ((System.Windows.Controls.Border)(this.FindName("rightPanel")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
            this.iMSEagleEye1 = ((ZDIMS.Map.IMSEagleEye)(this.FindName("iMSEagleEye1")));
            this.vectorMapDoc2 = ((ZDIMS.Map.VectorMapDoc)(this.FindName("vectorMapDoc2")));
            this.vectorLayer1 = ((ZDIMS.Map.VectorLayer)(this.FindName("vectorLayer1")));
            this.gDBItem1 = ((ZDIMS.Map.GDBItem)(this.FindName("gDBItem1")));
            this.layerItem1 = ((ZDIMS.Map.LayerItem)(this.FindName("layerItem1")));
            this.layerItem2 = ((ZDIMS.Map.LayerItem)(this.FindName("layerItem2")));
            this.layerItem3 = ((ZDIMS.Map.LayerItem)(this.FindName("layerItem3")));
            this.graphicsLayer1 = ((ZDIMS.Drawing.GraphicsLayer)(this.FindName("graphicsLayer1")));
            this.markLayer1 = ((ZDIMS.Drawing.MarkLayer)(this.FindName("markLayer1")));
            this.markLayer2 = ((ZDIMS.Drawing.MarkLayer)(this.FindName("markLayer2")));
            this.hideMenu = ((System.Windows.Media.Animation.Storyboard)(this.FindName("hideMenu")));
            this.showMenu = ((System.Windows.Media.Animation.Storyboard)(this.FindName("showMenu")));
        }
    }
}
