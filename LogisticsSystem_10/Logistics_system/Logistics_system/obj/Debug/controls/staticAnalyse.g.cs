﻿#pragma checksum "E:\GIS\3-示例Demo\官网示例\官网开发案例\新建文件夹\基于Silverlight物流配送系统\客户端站点\Logistics_system\Logistics_system\controls\staticAnalyse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "27FCEB14AD6284CCB701EDCF82785992"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.AgDataGrid;
using EasySL.Controls;
using Logistics_system;
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
    
    
    public partial class staticAnalyse : ZDIMSDemo.Controls.BaseUserControl {
        
        internal System.Windows.DataTemplate OptionsPanelTemplate;
        
        internal EasySL.Controls.DialogPanel dialog;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel OptionsPanel;
        
        internal System.Windows.Controls.RadioButton financialRb;
        
        internal System.Windows.Controls.RadioButton prodectRb;
        
        internal System.Windows.Controls.Button @static;
        
        internal DevExpress.AgDataGrid.AgDataGrid grid;
        
        internal Logistics_system.CurrencyToStringConverter CurrencyToStringConverter;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Logistics_system;component/controls/staticAnalyse.xaml", System.UriKind.Relative));
            this.OptionsPanelTemplate = ((System.Windows.DataTemplate)(this.FindName("OptionsPanelTemplate")));
            this.dialog = ((EasySL.Controls.DialogPanel)(this.FindName("dialog")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.OptionsPanel = ((System.Windows.Controls.StackPanel)(this.FindName("OptionsPanel")));
            this.financialRb = ((System.Windows.Controls.RadioButton)(this.FindName("financialRb")));
            this.prodectRb = ((System.Windows.Controls.RadioButton)(this.FindName("prodectRb")));
            this.@static = ((System.Windows.Controls.Button)(this.FindName("static")));
            this.grid = ((DevExpress.AgDataGrid.AgDataGrid)(this.FindName("grid")));
            this.CurrencyToStringConverter = ((Logistics_system.CurrencyToStringConverter)(this.FindName("CurrencyToStringConverter")));
        }
    }
}

