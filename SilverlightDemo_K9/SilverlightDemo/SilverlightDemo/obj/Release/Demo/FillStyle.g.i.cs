﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\Demo\FillStyle.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C6C42BB25329EAD9E22CD420D3A5B6D8"
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


namespace SilverlightDemo.Demo {
    
    
    public partial class FillStyle : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
        internal ZDIMS.Drawing.GraphicsLayer graphicsLayer;
        
        internal System.Windows.Controls.Canvas ResultsDisplayCanvas;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.ComboBox setStrokeType1;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.Label label6;
        
        internal System.Windows.Controls.Slider SetStrookeOpa1;
        
        internal ZdimsExtends.util.ColorsPicker StrokecolorsPicker1;
        
        internal System.Windows.Controls.ComboBox setFillType;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.Slider SetFillOpa;
        
        internal ZdimsExtends.util.ColorsPicker fillcolorsPicker;
        
        internal System.Windows.Controls.Canvas Picstyle;
        
        internal System.Windows.Controls.Label label34;
        
        internal System.Windows.Controls.ComboBox setStrokeType;
        
        internal System.Windows.Controls.Label label89;
        
        internal System.Windows.Controls.Label label76;
        
        internal System.Windows.Controls.Slider SetStrookeOpa;
        
        internal ZdimsExtends.util.ColorsPicker colorsPicker;
        
        internal System.Windows.Controls.Label label7;
        
        internal System.Windows.Controls.Slider SetOpacity;
        
        internal System.Windows.Controls.Label label8;
        
        internal System.Windows.Controls.ComboBox setSource;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/FillStyle.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
            this.graphicsLayer = ((ZDIMS.Drawing.GraphicsLayer)(this.FindName("graphicsLayer")));
            this.ResultsDisplayCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ResultsDisplayCanvas")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.setStrokeType1 = ((System.Windows.Controls.ComboBox)(this.FindName("setStrokeType1")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
            this.SetStrookeOpa1 = ((System.Windows.Controls.Slider)(this.FindName("SetStrookeOpa1")));
            this.StrokecolorsPicker1 = ((ZdimsExtends.util.ColorsPicker)(this.FindName("StrokecolorsPicker1")));
            this.setFillType = ((System.Windows.Controls.ComboBox)(this.FindName("setFillType")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.SetFillOpa = ((System.Windows.Controls.Slider)(this.FindName("SetFillOpa")));
            this.fillcolorsPicker = ((ZdimsExtends.util.ColorsPicker)(this.FindName("fillcolorsPicker")));
            this.Picstyle = ((System.Windows.Controls.Canvas)(this.FindName("Picstyle")));
            this.label34 = ((System.Windows.Controls.Label)(this.FindName("label34")));
            this.setStrokeType = ((System.Windows.Controls.ComboBox)(this.FindName("setStrokeType")));
            this.label89 = ((System.Windows.Controls.Label)(this.FindName("label89")));
            this.label76 = ((System.Windows.Controls.Label)(this.FindName("label76")));
            this.SetStrookeOpa = ((System.Windows.Controls.Slider)(this.FindName("SetStrookeOpa")));
            this.colorsPicker = ((ZdimsExtends.util.ColorsPicker)(this.FindName("colorsPicker")));
            this.label7 = ((System.Windows.Controls.Label)(this.FindName("label7")));
            this.SetOpacity = ((System.Windows.Controls.Slider)(this.FindName("SetOpacity")));
            this.label8 = ((System.Windows.Controls.Label)(this.FindName("label8")));
            this.setSource = ((System.Windows.Controls.ComboBox)(this.FindName("setSource")));
        }
    }
}

