﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\Demo\QueryByBoth.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3066D8A55AD0A9939CB5BFDE0A1AF60B"
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


namespace SilverlightDemo {
    
    
    public partial class QueryByBoth : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
        internal ZDIMS.Map.VectorLayer vectorLayer;
        
        internal ZDIMS.Map.GDBItem gdb;
        
        internal ZDIMS.Drawing.GraphicsLayer graphicsLayer1;
        
        internal ZDIMS.Drawing.GraphicsLayer graphicsLayer;
        
        internal System.Windows.Controls.Canvas ResultsDisplayCanvas;
        
        internal System.Windows.Controls.ComboBox selType;
        
        internal System.Windows.Controls.ComboBoxItem cb1;
        
        internal System.Windows.Controls.ComboBoxItem cb2;
        
        internal System.Windows.Controls.ComboBoxItem cb3;
        
        internal System.Windows.Controls.ComboBoxItem cb4;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.CheckBox HightLight;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.TextBox sql;
        
        internal System.Windows.Controls.Button clear;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/QueryByBoth.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
            this.vectorLayer = ((ZDIMS.Map.VectorLayer)(this.FindName("vectorLayer")));
            this.gdb = ((ZDIMS.Map.GDBItem)(this.FindName("gdb")));
            this.graphicsLayer1 = ((ZDIMS.Drawing.GraphicsLayer)(this.FindName("graphicsLayer1")));
            this.graphicsLayer = ((ZDIMS.Drawing.GraphicsLayer)(this.FindName("graphicsLayer")));
            this.ResultsDisplayCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ResultsDisplayCanvas")));
            this.selType = ((System.Windows.Controls.ComboBox)(this.FindName("selType")));
            this.cb1 = ((System.Windows.Controls.ComboBoxItem)(this.FindName("cb1")));
            this.cb2 = ((System.Windows.Controls.ComboBoxItem)(this.FindName("cb2")));
            this.cb3 = ((System.Windows.Controls.ComboBoxItem)(this.FindName("cb3")));
            this.cb4 = ((System.Windows.Controls.ComboBoxItem)(this.FindName("cb4")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.HightLight = ((System.Windows.Controls.CheckBox)(this.FindName("HightLight")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.sql = ((System.Windows.Controls.TextBox)(this.FindName("sql")));
            this.clear = ((System.Windows.Controls.Button)(this.FindName("clear")));
        }
    }
}
