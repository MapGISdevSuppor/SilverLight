﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\Demo\LineStyles.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B7A1FE3C715D87D1919CF7DBF457542C"
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
using ZdimsExtends.util;


namespace SilverlightDemo {
    
    
    public partial class LineStyles : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
        internal ZDIMS.Drawing.GraphicsLayer graphicsLayer;
        
        internal System.Windows.Controls.Canvas ResultsDisplayCanvas;
        
        internal System.Windows.Controls.ComboBox setType;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.Slider SetSize;
        
        internal ZdimsExtends.util.ColorsPicker colorsPicker;
        
        internal System.Windows.Controls.ComboBox setJoin;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.Slider setOpacity;
        
        internal System.Windows.Controls.ComboBox setCap;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.Label label6;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/LineStyles.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
            this.graphicsLayer = ((ZDIMS.Drawing.GraphicsLayer)(this.FindName("graphicsLayer")));
            this.ResultsDisplayCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ResultsDisplayCanvas")));
            this.setType = ((System.Windows.Controls.ComboBox)(this.FindName("setType")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.SetSize = ((System.Windows.Controls.Slider)(this.FindName("SetSize")));
            this.colorsPicker = ((ZdimsExtends.util.ColorsPicker)(this.FindName("colorsPicker")));
            this.setJoin = ((System.Windows.Controls.ComboBox)(this.FindName("setJoin")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.setOpacity = ((System.Windows.Controls.Slider)(this.FindName("setOpacity")));
            this.setCap = ((System.Windows.Controls.ComboBox)(this.FindName("setCap")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
        }
    }
}

