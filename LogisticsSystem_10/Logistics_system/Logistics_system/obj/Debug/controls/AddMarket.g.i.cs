﻿#pragma checksum "E:\GIS\3-示例Demo\官网示例\官网开发案例\新建文件夹\基于Silverlight物流配送系统\客户端站点\Logistics_system\Logistics_system\controls\AddMarket.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FF142603415BA4809157BBF64A66AA8E"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using EasySL.Controls;
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
using ZDIMSDemo.Controls;


namespace Logistics_system.controls {
    
    
    public partial class AddMarket : ZDIMSDemo.Controls.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel dialog;
        
        internal System.Windows.Controls.Grid layoutRoot;
        
        internal System.Windows.Controls.Canvas ResultsDisplayCanvas;
        
        internal System.Windows.Controls.TextBlock DataDisplayTitle;
        
        internal System.Windows.Controls.TabControl tabControl1;
        
        internal System.Windows.Controls.TabItem tabItem1;
        
        internal System.Windows.Shapes.Path Path_Copy71;
        
        internal System.Windows.Shapes.Path Path_Copy72;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Shapes.Path Path_Copy73;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Shapes.Path Path_Copy74;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.TextBox MarketName0;
        
        internal System.Windows.Controls.TextBox address0;
        
        internal System.Windows.Controls.RadioButton isRadio;
        
        internal System.Windows.Controls.RadioButton NoRadio;
        
        internal System.Windows.Controls.Button set_market;
        
        internal System.Windows.Controls.TextBox marketID0;
        
        internal System.Windows.Controls.Label label0;
        
        internal System.Windows.Shapes.Path path1;
        
        internal System.Windows.Controls.ComboBox centerID0;
        
        internal System.Windows.Controls.Label label10;
        
        internal System.Windows.Controls.TabItem tabitem2;
        
        internal System.Windows.Shapes.Path Path_Copy75;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.TextBox marketName1;
        
        internal System.Windows.Shapes.Path Path_Copy34;
        
        internal System.Windows.Controls.Label labe45;
        
        internal System.Windows.Controls.TextBox marketID1;
        
        internal System.Windows.Shapes.Path Path_Copy76;
        
        internal System.Windows.Controls.Label label6;
        
        internal System.Windows.Controls.TextBox unitmoney;
        
        internal System.Windows.Shapes.Path Path_Copy77;
        
        internal System.Windows.Controls.Label label7;
        
        internal System.Windows.Controls.TextBox ImportMoney;
        
        internal System.Windows.Shapes.Path Path_Copy78;
        
        internal System.Windows.Controls.Label label8;
        
        internal System.Windows.Controls.TextBox SaledMoney;
        
        internal System.Windows.Controls.Button set_finaicnal;
        
        internal System.Windows.Controls.TabItem tabitem3;
        
        internal System.Windows.Shapes.Path path11;
        
        internal System.Windows.Controls.Label label9;
        
        internal System.Windows.Controls.TextBox marketID2;
        
        internal System.Windows.Controls.TextBox MarketName2;
        
        internal System.Windows.Controls.Label label21;
        
        internal System.Windows.Shapes.Path path23;
        
        internal System.Windows.Controls.TextBox SaleNum;
        
        internal System.Windows.Controls.Label label11;
        
        internal System.Windows.Shapes.Path path3;
        
        internal System.Windows.Controls.TextBox PreNum2;
        
        internal System.Windows.Controls.Label label12;
        
        internal System.Windows.Shapes.Path path4;
        
        internal System.Windows.Controls.TextBox inerPrice;
        
        internal System.Windows.Controls.Label label13;
        
        internal System.Windows.Shapes.Path path5;
        
        internal System.Windows.Controls.TextBox prodectID2;
        
        internal System.Windows.Controls.Label label14;
        
        internal System.Windows.Shapes.Path path6;
        
        internal System.Windows.Controls.TextBox Price2;
        
        internal System.Windows.Controls.Label label15;
        
        internal System.Windows.Shapes.Path path7;
        
        internal System.Windows.Controls.TextBox InstockNum;
        
        internal System.Windows.Controls.Label label16;
        
        internal System.Windows.Shapes.Path path8;
        
        internal System.Windows.Controls.TextBox ReceNum2;
        
        internal System.Windows.Controls.Label label17;
        
        internal System.Windows.Shapes.Path path9;
        
        internal System.Windows.Controls.TextBox prodectName2;
        
        internal System.Windows.Controls.Label label18;
        
        internal System.Windows.Shapes.Path path10;
        
        internal System.Windows.Controls.Button ProdectInfo;
        
        internal System.Windows.Controls.TabItem tabitem4;
        
        internal System.Windows.Shapes.Path Path_Copy81;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.TextBox centerID3;
        
        internal System.Windows.Shapes.Path Path_Copy821;
        
        internal System.Windows.Controls.Label labeld1;
        
        internal System.Windows.Controls.TextBox centerName3;
        
        internal System.Windows.Shapes.Path Path_Copy82;
        
        internal System.Windows.Controls.Label label82;
        
        internal System.Windows.Controls.TextBox ProdectID3;
        
        internal System.Windows.Shapes.Path Path_Copy83;
        
        internal System.Windows.Controls.Label label83;
        
        internal System.Windows.Controls.TextBox ProdectName3;
        
        internal System.Windows.Shapes.Path Path_Copy84;
        
        internal System.Windows.Controls.Label label89;
        
        internal System.Windows.Controls.TextBox StocksNum3;
        
        internal System.Windows.Controls.Button CenterInfo;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Logistics_system;component/controls/AddMarket.xaml", System.UriKind.Relative));
            this.dialog = ((EasySL.Controls.DialogPanel)(this.FindName("dialog")));
            this.layoutRoot = ((System.Windows.Controls.Grid)(this.FindName("layoutRoot")));
            this.ResultsDisplayCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ResultsDisplayCanvas")));
            this.DataDisplayTitle = ((System.Windows.Controls.TextBlock)(this.FindName("DataDisplayTitle")));
            this.tabControl1 = ((System.Windows.Controls.TabControl)(this.FindName("tabControl1")));
            this.tabItem1 = ((System.Windows.Controls.TabItem)(this.FindName("tabItem1")));
            this.Path_Copy71 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy71")));
            this.Path_Copy72 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy72")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.Path_Copy73 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy73")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.Path_Copy74 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy74")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.MarketName0 = ((System.Windows.Controls.TextBox)(this.FindName("MarketName0")));
            this.address0 = ((System.Windows.Controls.TextBox)(this.FindName("address0")));
            this.isRadio = ((System.Windows.Controls.RadioButton)(this.FindName("isRadio")));
            this.NoRadio = ((System.Windows.Controls.RadioButton)(this.FindName("NoRadio")));
            this.set_market = ((System.Windows.Controls.Button)(this.FindName("set_market")));
            this.marketID0 = ((System.Windows.Controls.TextBox)(this.FindName("marketID0")));
            this.label0 = ((System.Windows.Controls.Label)(this.FindName("label0")));
            this.path1 = ((System.Windows.Shapes.Path)(this.FindName("path1")));
            this.centerID0 = ((System.Windows.Controls.ComboBox)(this.FindName("centerID0")));
            this.label10 = ((System.Windows.Controls.Label)(this.FindName("label10")));
            this.tabitem2 = ((System.Windows.Controls.TabItem)(this.FindName("tabitem2")));
            this.Path_Copy75 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy75")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.marketName1 = ((System.Windows.Controls.TextBox)(this.FindName("marketName1")));
            this.Path_Copy34 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy34")));
            this.labe45 = ((System.Windows.Controls.Label)(this.FindName("labe45")));
            this.marketID1 = ((System.Windows.Controls.TextBox)(this.FindName("marketID1")));
            this.Path_Copy76 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy76")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
            this.unitmoney = ((System.Windows.Controls.TextBox)(this.FindName("unitmoney")));
            this.Path_Copy77 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy77")));
            this.label7 = ((System.Windows.Controls.Label)(this.FindName("label7")));
            this.ImportMoney = ((System.Windows.Controls.TextBox)(this.FindName("ImportMoney")));
            this.Path_Copy78 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy78")));
            this.label8 = ((System.Windows.Controls.Label)(this.FindName("label8")));
            this.SaledMoney = ((System.Windows.Controls.TextBox)(this.FindName("SaledMoney")));
            this.set_finaicnal = ((System.Windows.Controls.Button)(this.FindName("set_finaicnal")));
            this.tabitem3 = ((System.Windows.Controls.TabItem)(this.FindName("tabitem3")));
            this.path11 = ((System.Windows.Shapes.Path)(this.FindName("path11")));
            this.label9 = ((System.Windows.Controls.Label)(this.FindName("label9")));
            this.marketID2 = ((System.Windows.Controls.TextBox)(this.FindName("marketID2")));
            this.MarketName2 = ((System.Windows.Controls.TextBox)(this.FindName("MarketName2")));
            this.label21 = ((System.Windows.Controls.Label)(this.FindName("label21")));
            this.path23 = ((System.Windows.Shapes.Path)(this.FindName("path23")));
            this.SaleNum = ((System.Windows.Controls.TextBox)(this.FindName("SaleNum")));
            this.label11 = ((System.Windows.Controls.Label)(this.FindName("label11")));
            this.path3 = ((System.Windows.Shapes.Path)(this.FindName("path3")));
            this.PreNum2 = ((System.Windows.Controls.TextBox)(this.FindName("PreNum2")));
            this.label12 = ((System.Windows.Controls.Label)(this.FindName("label12")));
            this.path4 = ((System.Windows.Shapes.Path)(this.FindName("path4")));
            this.inerPrice = ((System.Windows.Controls.TextBox)(this.FindName("inerPrice")));
            this.label13 = ((System.Windows.Controls.Label)(this.FindName("label13")));
            this.path5 = ((System.Windows.Shapes.Path)(this.FindName("path5")));
            this.prodectID2 = ((System.Windows.Controls.TextBox)(this.FindName("prodectID2")));
            this.label14 = ((System.Windows.Controls.Label)(this.FindName("label14")));
            this.path6 = ((System.Windows.Shapes.Path)(this.FindName("path6")));
            this.Price2 = ((System.Windows.Controls.TextBox)(this.FindName("Price2")));
            this.label15 = ((System.Windows.Controls.Label)(this.FindName("label15")));
            this.path7 = ((System.Windows.Shapes.Path)(this.FindName("path7")));
            this.InstockNum = ((System.Windows.Controls.TextBox)(this.FindName("InstockNum")));
            this.label16 = ((System.Windows.Controls.Label)(this.FindName("label16")));
            this.path8 = ((System.Windows.Shapes.Path)(this.FindName("path8")));
            this.ReceNum2 = ((System.Windows.Controls.TextBox)(this.FindName("ReceNum2")));
            this.label17 = ((System.Windows.Controls.Label)(this.FindName("label17")));
            this.path9 = ((System.Windows.Shapes.Path)(this.FindName("path9")));
            this.prodectName2 = ((System.Windows.Controls.TextBox)(this.FindName("prodectName2")));
            this.label18 = ((System.Windows.Controls.Label)(this.FindName("label18")));
            this.path10 = ((System.Windows.Shapes.Path)(this.FindName("path10")));
            this.ProdectInfo = ((System.Windows.Controls.Button)(this.FindName("ProdectInfo")));
            this.tabitem4 = ((System.Windows.Controls.TabItem)(this.FindName("tabitem4")));
            this.Path_Copy81 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy81")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.centerID3 = ((System.Windows.Controls.TextBox)(this.FindName("centerID3")));
            this.Path_Copy821 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy821")));
            this.labeld1 = ((System.Windows.Controls.Label)(this.FindName("labeld1")));
            this.centerName3 = ((System.Windows.Controls.TextBox)(this.FindName("centerName3")));
            this.Path_Copy82 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy82")));
            this.label82 = ((System.Windows.Controls.Label)(this.FindName("label82")));
            this.ProdectID3 = ((System.Windows.Controls.TextBox)(this.FindName("ProdectID3")));
            this.Path_Copy83 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy83")));
            this.label83 = ((System.Windows.Controls.Label)(this.FindName("label83")));
            this.ProdectName3 = ((System.Windows.Controls.TextBox)(this.FindName("ProdectName3")));
            this.Path_Copy84 = ((System.Windows.Shapes.Path)(this.FindName("Path_Copy84")));
            this.label89 = ((System.Windows.Controls.Label)(this.FindName("label89")));
            this.StocksNum3 = ((System.Windows.Controls.TextBox)(this.FindName("StocksNum3")));
            this.CenterInfo = ((System.Windows.Controls.Button)(this.FindName("CenterInfo")));
        }
    }
}

