﻿#pragma checksum "E:\WorkSpaceForSilverlight\Silverlight\ZDIMSDemo\ZDIMSDemo\Controls\BusAnalyse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "222CCB76A6BF370423AAC338BC71262D"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
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
    
    
    public partial class BusAnalyse : ZDIMSDemo.Controls.BaseUserControl {
        
        internal EasySL.Controls.DialogPanel dialogPanel1;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.TextBox start;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.TextBox stop;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.ComboBox startPos;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.ComboBox stopPos;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.RichTextBox busReslut;
        
        internal System.Windows.Controls.Button button1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ZDIMSDemo;component/Controls/BusAnalyse.xaml", System.UriKind.Relative));
            this.dialogPanel1 = ((EasySL.Controls.DialogPanel)(this.FindName("dialogPanel1")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.start = ((System.Windows.Controls.TextBox)(this.FindName("start")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.stop = ((System.Windows.Controls.TextBox)(this.FindName("stop")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.startPos = ((System.Windows.Controls.ComboBox)(this.FindName("startPos")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.stopPos = ((System.Windows.Controls.ComboBox)(this.FindName("stopPos")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.busReslut = ((System.Windows.Controls.RichTextBox)(this.FindName("busReslut")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
        }
    }
}
