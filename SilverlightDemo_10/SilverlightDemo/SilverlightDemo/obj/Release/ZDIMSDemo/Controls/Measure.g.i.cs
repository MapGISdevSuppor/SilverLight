﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\SilverlightDemo\SilverlightDemo\ZDIMSDemo\Controls\Measure.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "445990771F369E6D5A59600EC76413BA"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
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


namespace ZDIMSDemo.Controls {
    
    
    public partial class Measure : ZDIMSDemo.Controls.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel dialogPanel1;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.RadioButton Measurelen;
        
        internal System.Windows.Controls.RadioButton Measurearea;
        
        internal System.Windows.Controls.Button clearMeasure;
        
        internal System.Windows.Controls.CheckBox checkBox1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightDemo;component/ZDIMSDemo/Controls/Measure.xaml", System.UriKind.Relative));
            this.dialogPanel1 = ((EasySL.Controls.DialogPanel)(this.FindName("dialogPanel1")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.Measurelen = ((System.Windows.Controls.RadioButton)(this.FindName("Measurelen")));
            this.Measurearea = ((System.Windows.Controls.RadioButton)(this.FindName("Measurearea")));
            this.clearMeasure = ((System.Windows.Controls.Button)(this.FindName("clearMeasure")));
            this.checkBox1 = ((System.Windows.Controls.CheckBox)(this.FindName("checkBox1")));
        }
    }
}
