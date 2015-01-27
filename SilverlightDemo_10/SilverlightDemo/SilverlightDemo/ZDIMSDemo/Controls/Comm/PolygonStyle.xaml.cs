using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ZDIMS.BaseLib;

namespace ZDIMSDemo.Controls.Comm
{
    public partial class PolygonStyle : UserControl, IFeatureStyleEditor
    {
        WebGraphicsInfo m_gInfo;
        public GInfoType FeatureGinfoType { get { return GInfoType.LinInfo; } } 
        public PolygonStyle(WebGraphicsInfo gInfo = null)
        {
            InitializeComponent();
            m_gInfo = gInfo;
            if (gInfo != null)
            {
                this.Loaded += new RoutedEventHandler(OnLoaded);
            }
            else
            {
                fillcolor.SelectedIndex = 0;
                patternID.SelectedIndex = 0;
                patternColor.SelectedIndex = 0;
            }
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
        public void Update()
        {
            if (m_gInfo != null)
            {
                if (this.fillcolor._TextBoxInput.Text == "0" || this.fillcolor._TextBoxInput.Text.Length == 0)
                    this.fillcolor._TextBoxInput.Text = m_gInfo.RegInfo.FillColor.ToString();
                if (this.patternHeight.Text.Length == 0)
                    this.patternHeight.Text = m_gInfo.RegInfo.PatHeight.ToString();
                if (this.patternID._TextBoxInput.Text == "0" || this.patternID._TextBoxInput.Text.Length == 0)
                    this.patternID._TextBoxInput.Text = m_gInfo.RegInfo.PatID.ToString();
                if (this.patternWidth.Text.Length == 0)
                    this.patternWidth.Text = m_gInfo.RegInfo.PatWidth.ToString();
                if (this.patternColor._TextBoxInput.Text == "0" || this.patternColor._TextBoxInput.Text.Length == 0)
                    this.patternColor._TextBoxInput.Text = m_gInfo.RegInfo.PatColor.ToString();
                if (this.patternPenWidth.Text.Length == 0)
                    this.patternPenWidth.Text = m_gInfo.RegInfo.OutPenWidth.ToString();
            }            
        }
    }
}
