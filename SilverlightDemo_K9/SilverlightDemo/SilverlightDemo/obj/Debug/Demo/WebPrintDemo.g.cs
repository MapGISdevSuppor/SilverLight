﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\Demo\WebPrintDemo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "750A7E43C676D31C681587721D8D8BA3"
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
using ZdimsExtends.WebPrint.Frame;
using ZdimsExtends.util;


namespace SilverlightDemo {
    
    
    public partial class WebPrintDemo : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid container;
        
        internal ZdimsExtends.WebPrint.Frame.editFrame Frame;
        
        internal ZDIMS.Map.IMSMap iMSMap1;
        
        internal ZDIMS.Map.TileLayer tileLayer1;
        
        internal ZDIMS.Drawing.MarkLayer markLayer;
        
        internal ZDIMS.Drawing.GraphicsLayer graphicesLaye;
        
        internal ZdimsExtends.util.Adorner adorn;
        
        internal System.Windows.Controls.TabControl tabControl1;
        
        internal System.Windows.Controls.TabItem set;
        
        internal System.Windows.Controls.Button AddPnt;
        
        internal System.Windows.Controls.Image image6;
        
        internal System.Windows.Controls.Button AddLine;
        
        internal System.Windows.Controls.Image image7;
        
        internal System.Windows.Controls.Button AddPolygon;
        
        internal System.Windows.Controls.Image image8;
        
        internal System.Windows.Controls.Button ScaleCol;
        
        internal System.Windows.Controls.Image image3;
        
        internal System.Windows.Controls.Button addCompass;
        
        internal System.Windows.Controls.Button addText;
        
        internal System.Windows.Controls.Button addImage;
        
        internal System.Windows.Controls.Button addFrame;
        
        internal System.Windows.Controls.Button delFrame;
        
        internal System.Windows.Controls.TabItem print;
        
        internal System.Windows.Controls.Button button6;
        
        internal System.Windows.Controls.Button saveImg;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.TextBox XPos;
        
        internal System.Windows.Controls.TextBox YPos;
        
        internal System.Windows.Controls.Button SetPos;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.Label label6;
        
        internal System.Windows.Controls.TextBox H;
        
        internal System.Windows.Controls.TextBox W;
        
        internal System.Windows.Controls.Button SetSize;
        
        internal System.Windows.Controls.Button PrintClick;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/Demo/WebPrintDemo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.container = ((System.Windows.Controls.Grid)(this.FindName("container")));
            this.Frame = ((ZdimsExtends.WebPrint.Frame.editFrame)(this.FindName("Frame")));
            this.iMSMap1 = ((ZDIMS.Map.IMSMap)(this.FindName("iMSMap1")));
            this.tileLayer1 = ((ZDIMS.Map.TileLayer)(this.FindName("tileLayer1")));
            this.markLayer = ((ZDIMS.Drawing.MarkLayer)(this.FindName("markLayer")));
            this.graphicesLaye = ((ZDIMS.Drawing.GraphicsLayer)(this.FindName("graphicesLaye")));
            this.adorn = ((ZdimsExtends.util.Adorner)(this.FindName("adorn")));
            this.tabControl1 = ((System.Windows.Controls.TabControl)(this.FindName("tabControl1")));
            this.set = ((System.Windows.Controls.TabItem)(this.FindName("set")));
            this.AddPnt = ((System.Windows.Controls.Button)(this.FindName("AddPnt")));
            this.image6 = ((System.Windows.Controls.Image)(this.FindName("image6")));
            this.AddLine = ((System.Windows.Controls.Button)(this.FindName("AddLine")));
            this.image7 = ((System.Windows.Controls.Image)(this.FindName("image7")));
            this.AddPolygon = ((System.Windows.Controls.Button)(this.FindName("AddPolygon")));
            this.image8 = ((System.Windows.Controls.Image)(this.FindName("image8")));
            this.ScaleCol = ((System.Windows.Controls.Button)(this.FindName("ScaleCol")));
            this.image3 = ((System.Windows.Controls.Image)(this.FindName("image3")));
            this.addCompass = ((System.Windows.Controls.Button)(this.FindName("addCompass")));
            this.addText = ((System.Windows.Controls.Button)(this.FindName("addText")));
            this.addImage = ((System.Windows.Controls.Button)(this.FindName("addImage")));
            this.addFrame = ((System.Windows.Controls.Button)(this.FindName("addFrame")));
            this.delFrame = ((System.Windows.Controls.Button)(this.FindName("delFrame")));
            this.print = ((System.Windows.Controls.TabItem)(this.FindName("print")));
            this.button6 = ((System.Windows.Controls.Button)(this.FindName("button6")));
            this.saveImg = ((System.Windows.Controls.Button)(this.FindName("saveImg")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.XPos = ((System.Windows.Controls.TextBox)(this.FindName("XPos")));
            this.YPos = ((System.Windows.Controls.TextBox)(this.FindName("YPos")));
            this.SetPos = ((System.Windows.Controls.Button)(this.FindName("SetPos")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
            this.H = ((System.Windows.Controls.TextBox)(this.FindName("H")));
            this.W = ((System.Windows.Controls.TextBox)(this.FindName("W")));
            this.SetSize = ((System.Windows.Controls.Button)(this.FindName("SetSize")));
            this.PrintClick = ((System.Windows.Controls.Button)(this.FindName("PrintClick")));
        }
    }
}

