﻿#pragma checksum "E:\Code2010\ZDIMS\ZDIMS\ZDIMSDemo\Controls\BufferAnalyse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2058662E0FEBE7A23D6301527B4D733A"
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
    
    
    public partial class BufferAnalyse : ZDIMSDemo.Controls.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel dialogPanel1;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Button buffer;
        
        internal System.Windows.Controls.TextBox bufferRadius;
        
        internal System.Windows.Controls.TextBox traceRadius;
        
        internal System.Windows.Controls.TextBox resultLayerName;
        
        internal System.Windows.Controls.Button cancle;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/Controls/BufferAnalyse.xaml", System.UriKind.Relative));
            this.dialogPanel1 = ((EasySL.Controls.DialogPanel)(this.FindName("dialogPanel1")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.buffer = ((System.Windows.Controls.Button)(this.FindName("buffer")));
            this.bufferRadius = ((System.Windows.Controls.TextBox)(this.FindName("bufferRadius")));
            this.traceRadius = ((System.Windows.Controls.TextBox)(this.FindName("traceRadius")));
            this.resultLayerName = ((System.Windows.Controls.TextBox)(this.FindName("resultLayerName")));
            this.cancle = ((System.Windows.Controls.Button)(this.FindName("cancle")));
        }
    }
}
