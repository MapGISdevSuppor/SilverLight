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
    public partial class PointStyle : UserControl, IFeatureStyleEditor
    {
        WebGraphicsInfo m_gInfo;

        public GInfoType FeatureGinfoType { get { return GInfoType.PntInfo; } } 
        public PointStyle(WebGraphicsInfo gInfo = null)
        {
            InitializeComponent();
            m_gInfo = gInfo;
            if (gInfo != null)
            {
                this.Loaded += new RoutedEventHandler(OnLoaded);
            }
            else
            {
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
                if (this.patternID._TextBoxInput.Text == "0" || this.patternID._TextBoxInput.Text.Length == 0)
                    patternID._TextBoxInput.Text = m_gInfo.PntInfo.SymID.ToString();
                if (this.patternWidth.Text.Length == 0)
                    patternWidth.Text = m_gInfo.PntInfo.SymWidth.ToString();
                if (this.patternHeight.Text.Length == 0)
                    patternHeight.Text = m_gInfo.PntInfo.SymHeight.ToString();
                if (this.patternAngle.Text.Length == 0)
                    patternAngle.Text = m_gInfo.PntInfo.Angle.ToString();
                if (this.patternColor._TextBoxInput.Text == "0" || this.patternColor._TextBoxInput.Text.Length == 0)
                    patternColor._TextBoxInput.Text = m_gInfo.PntInfo.Color.ToString();
            }            
        }
    }
}
