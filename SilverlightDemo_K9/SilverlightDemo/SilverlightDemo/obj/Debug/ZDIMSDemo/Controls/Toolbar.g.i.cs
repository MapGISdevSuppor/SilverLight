﻿#pragma checksum "E:\Silverlight\SilverlightDemo\SilverlightDemo\ZDIMSDemo\Controls\Toolbar.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F474AFCF8F3208ACAC49C131309C4B53"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.225
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


namespace ZDIMSDemo.Controls {
    
    
    public partial class Toolbar : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.TabControl tabControl1;
        
        internal System.Windows.Controls.TabItem tabItem1;
        
        internal System.Windows.Controls.Button ZoomIn;
        
        internal System.Windows.Controls.Image image1;
        
        internal System.Windows.Controls.Button ZoomOut;
        
        internal System.Windows.Controls.Image image3;
        
        internal System.Windows.Controls.Button MapMove;
        
        internal System.Windows.Controls.Image image2;
        
        internal System.Windows.Controls.Button MapUpdate;
        
        internal System.Windows.Controls.Image image4;
        
        internal System.Windows.Controls.Button MapRestore;
        
        internal System.Windows.Controls.Image image5;
        
        internal System.Windows.Controls.Button MapEagleEye;
        
        internal System.Windows.Controls.Image image6;
        
        internal System.Windows.Controls.Button Magnifier;
        
        internal System.Windows.Controls.Image image_magnifier;
        
        internal System.Windows.Controls.Button Marker;
        
        internal System.Windows.Controls.Image image_Marker;
        
        internal System.Windows.Controls.Button DotSelect;
        
        internal System.Windows.Controls.Button RectSelect;
        
        internal System.Windows.Controls.Button CircleSelect;
        
        internal System.Windows.Controls.Button LineSelect;
        
        internal System.Windows.Controls.Button PloySelect;
        
        internal System.Windows.Controls.Button AttSelect;
        
        internal System.Windows.Controls.Button DotConSelect;
        
        internal System.Windows.Controls.Button RectConSelect;
        
        internal System.Windows.Controls.Button CircleConSelect;
        
        internal System.Windows.Controls.Button LineConSelect;
        
        internal System.Windows.Controls.Button PloyConSelect;
        
        internal System.Windows.Controls.Button AddDot;
        
        internal System.Windows.Controls.Button AddLine;
        
        internal System.Windows.Controls.Button AddArea;
        
        internal System.Windows.Controls.Button BusAnalyse;
        
        internal System.Windows.Controls.Button NetAnalyse;
        
        internal System.Windows.Controls.Button button_circleClip;
        
        internal System.Windows.Controls.Image image7;
        
        internal System.Windows.Controls.Button button_polygonClip;
        
        internal System.Windows.Controls.Image image8;
        
        internal System.Windows.Controls.Button button_overLayerAnalyse;
        
        internal System.Windows.Controls.Image image9;
        
        internal System.Windows.Controls.Button button_topAnalyse;
        
        internal System.Windows.Controls.Image image12;
        
        internal System.Windows.Controls.Button button_project;
        
        internal System.Windows.Controls.Image image13;
        
        internal System.Windows.Controls.Button Navigator;
        
        internal System.Windows.Controls.Button dispSet;
        
        internal System.Windows.Controls.Button measure;
        
        internal System.Windows.Controls.Button position;
        
        internal System.Windows.Controls.Button gps;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/ZDIMSDemo/Controls/Toolbar.xaml", System.UriKind.Relative));
            this.tabControl1 = ((System.Windows.Controls.TabControl)(this.FindName("tabControl1")));
            this.tabItem1 = ((System.Windows.Controls.TabItem)(this.FindName("tabItem1")));
            this.ZoomIn = ((System.Windows.Controls.Button)(this.FindName("ZoomIn")));
            this.image1 = ((System.Windows.Controls.Image)(this.FindName("image1")));
            this.ZoomOut = ((System.Windows.Controls.Button)(this.FindName("ZoomOut")));
            this.image3 = ((System.Windows.Controls.Image)(this.FindName("image3")));
            this.MapMove = ((System.Windows.Controls.Button)(this.FindName("MapMove")));
            this.image2 = ((System.Windows.Controls.Image)(this.FindName("image2")));
            this.MapUpdate = ((System.Windows.Controls.Button)(this.FindName("MapUpdate")));
            this.image4 = ((System.Windows.Controls.Image)(this.FindName("image4")));
            this.MapRestore = ((System.Windows.Controls.Button)(this.FindName("MapRestore")));
            this.image5 = ((System.Windows.Controls.Image)(this.FindName("image5")));
            this.MapEagleEye = ((System.Windows.Controls.Button)(this.FindName("MapEagleEye")));
            this.image6 = ((System.Windows.Controls.Image)(this.FindName("image6")));
            this.Magnifier = ((System.Windows.Controls.Button)(this.FindName("Magnifier")));
            this.image_magnifier = ((System.Windows.Controls.Image)(this.FindName("image_magnifier")));
            this.Marker = ((System.Windows.Controls.Button)(this.FindName("Marker")));
            this.image_Marker = ((System.Windows.Controls.Image)(this.FindName("image_Marker")));
            this.DotSelect = ((System.Windows.Controls.Button)(this.FindName("DotSelect")));
            this.RectSelect = ((System.Windows.Controls.Button)(this.FindName("RectSelect")));
            this.CircleSelect = ((System.Windows.Controls.Button)(this.FindName("CircleSelect")));
            this.LineSelect = ((System.Windows.Controls.Button)(this.FindName("LineSelect")));
            this.PloySelect = ((System.Windows.Controls.Button)(this.FindName("PloySelect")));
            this.AttSelect = ((System.Windows.Controls.Button)(this.FindName("AttSelect")));
            this.DotConSelect = ((System.Windows.Controls.Button)(this.FindName("DotConSelect")));
            this.RectConSelect = ((System.Windows.Controls.Button)(this.FindName("RectConSelect")));
            this.CircleConSelect = ((System.Windows.Controls.Button)(this.FindName("CircleConSelect")));
            this.LineConSelect = ((System.Windows.Controls.Button)(this.FindName("LineConSelect")));
            this.PloyConSelect = ((System.Windows.Controls.Button)(this.FindName("PloyConSelect")));
            this.AddDot = ((System.Windows.Controls.Button)(this.FindName("AddDot")));
            this.AddLine = ((System.Windows.Controls.Button)(this.FindName("AddLine")));
            this.AddArea = ((System.Windows.Controls.Button)(this.FindName("AddArea")));
            this.BusAnalyse = ((System.Windows.Controls.Button)(this.FindName("BusAnalyse")));
            this.NetAnalyse = ((System.Windows.Controls.Button)(this.FindName("NetAnalyse")));
            this.button_circleClip = ((System.Windows.Controls.Button)(this.FindName("button_circleClip")));
            this.image7 = ((System.Windows.Controls.Image)(this.FindName("image7")));
            this.button_polygonClip = ((System.Windows.Controls.Button)(this.FindName("button_polygonClip")));
            this.image8 = ((System.Windows.Controls.Image)(this.FindName("image8")));
            this.button_overLayerAnalyse = ((System.Windows.Controls.Button)(this.FindName("button_overLayerAnalyse")));
            this.image9 = ((System.Windows.Controls.Image)(this.FindName("image9")));
            this.button_topAnalyse = ((System.Windows.Controls.Button)(this.FindName("button_topAnalyse")));
            this.image12 = ((System.Windows.Controls.Image)(this.FindName("image12")));
            this.button_project = ((System.Windows.Controls.Button)(this.FindName("button_project")));
            this.image13 = ((System.Windows.Controls.Image)(this.FindName("image13")));
            this.Navigator = ((System.Windows.Controls.Button)(this.FindName("Navigator")));
            this.dispSet = ((System.Windows.Controls.Button)(this.FindName("dispSet")));
            this.measure = ((System.Windows.Controls.Button)(this.FindName("measure")));
            this.position = ((System.Windows.Controls.Button)(this.FindName("position")));
            this.gps = ((System.Windows.Controls.Button)(this.FindName("gps")));
        }
    }
}

