﻿#pragma checksum "E:\Code2010\ZDIMS\ZDIMS\ZDIMSDemo\OGCDemo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "388433790AC3A8CA27B65AE51407B8A8"
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


namespace ZDIMSDemo {
    
    
    public partial class OGCDemo : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Border leftPanel;
        
        internal System.Windows.Controls.Image expand;
        
        internal System.Windows.Controls.Image collapse;
        
        internal System.Windows.Controls.Grid leftMenuPanel;
        
        internal System.Windows.Controls.TreeView treeView1;
        
        internal System.Windows.Controls.Border rightPanel;
        
        internal System.Windows.Controls.TextBlock rightTitle;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.OGCWSLayer oGCWSLayer1;
        
        internal ZDIMS.Map.IMSEagleEye iMSEagleEye1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/OGCDemo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.leftPanel = ((System.Windows.Controls.Border)(this.FindName("leftPanel")));
            this.expand = ((System.Windows.Controls.Image)(this.FindName("expand")));
            this.collapse = ((System.Windows.Controls.Image)(this.FindName("collapse")));
            this.leftMenuPanel = ((System.Windows.Controls.Grid)(this.FindName("leftMenuPanel")));
            this.treeView1 = ((System.Windows.Controls.TreeView)(this.FindName("treeView1")));
            this.rightPanel = ((System.Windows.Controls.Border)(this.FindName("rightPanel")));
            this.rightTitle = ((System.Windows.Controls.TextBlock)(this.FindName("rightTitle")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.oGCWSLayer1 = ((ZDIMS.Map.OGCWSLayer)(this.FindName("oGCWSLayer1")));
            this.iMSEagleEye1 = ((ZDIMS.Map.IMSEagleEye)(this.FindName("iMSEagleEye1")));
            this.hideMenu = ((System.Windows.Media.Animation.Storyboard)(this.FindName("hideMenu")));
            this.showMenu = ((System.Windows.Media.Animation.Storyboard)(this.FindName("showMenu")));
        }
    }
}
