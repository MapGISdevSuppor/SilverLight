﻿#pragma checksum "E:\Silverlight\SilverlightDemo\SilverlightDemo\ZDIMSDemo\Controls\Layer\LayerDisplaySet.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C0E4B847F7376E8534DF68C6F72D2C5F"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.225
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


namespace ZDIMSDemo.Controls.Layer {
    
    
    public partial class LayerDisplaySet : ZDIMSDemo.Controls.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel dialog;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.CheckBox checkBox_origin;
        
        internal System.Windows.Controls.CheckBox checkBox_followshow;
        
        internal System.Windows.Controls.CheckBox checkBox_showcoor;
        
        internal System.Windows.Controls.RadioButton radioButton_img_low;
        
        internal System.Windows.Controls.RadioButton radioButton_img_high;
        
        internal System.Windows.Controls.Button button_submit;
        
        internal System.Windows.Controls.Button button_cancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/ZDIMSDemo/Controls/Layer/LayerDisplaySet.xaml", System.UriKind.Relative));
            this.dialog = ((EasySL.Controls.DialogPanel)(this.FindName("dialog")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.checkBox_origin = ((System.Windows.Controls.CheckBox)(this.FindName("checkBox_origin")));
            this.checkBox_followshow = ((System.Windows.Controls.CheckBox)(this.FindName("checkBox_followshow")));
            this.checkBox_showcoor = ((System.Windows.Controls.CheckBox)(this.FindName("checkBox_showcoor")));
            this.radioButton_img_low = ((System.Windows.Controls.RadioButton)(this.FindName("radioButton_img_low")));
            this.radioButton_img_high = ((System.Windows.Controls.RadioButton)(this.FindName("radioButton_img_high")));
            this.button_submit = ((System.Windows.Controls.Button)(this.FindName("button_submit")));
            this.button_cancel = ((System.Windows.Controls.Button)(this.FindName("button_cancel")));
        }
    }
}

