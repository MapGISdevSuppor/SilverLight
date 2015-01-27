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
    public partial class LineStyle : UserControl, IFeatureStyleEditor
    {
        WebGraphicsInfo m_gInfo;
        public GInfoType FeatureGinfoType { get { return GInfoType.LinInfo; } } 
        public LineStyle(WebGraphicsInfo gInfo = null)
        {
            InitializeComponent();
            m_gInfo = gInfo;
            if (gInfo != null)
            {
                this.Loaded += new RoutedEventHandler(OnLoaded);
            }
            else
            {
                this.color.SelectedIndex = 0;
                patternID2.SelectedIndex = 0;
                patternID.SelectedIndex = 0;
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
                if (this.color._TextBoxInput.Text == "0" || this.color._TextBoxInput.Text.Length == 0)
                    this.color._TextBoxInput.Text = m_gInfo.LinInfo.Color.ToString();
                if (this.lineScaleX.Text.Length == 0)
                    this.lineScaleX.Text = m_gInfo.LinInfo.Xscale.ToString();
                if (this.lineScaleY.Text.Length == 0)
                    this.lineScaleY.Text = m_gInfo.LinInfo.Yscale.ToString();
                if (this.patternID._TextBoxInput.Text == "0" || this.patternID._TextBoxInput.Text.Length == 0)
                    this.patternID._TextBoxInput.Text = m_gInfo.LinInfo.LinStyleID.ToString();
                if (this.patternID2._TextBoxInput.Text == "0" || this.patternID2._TextBoxInput.Text.Length == 0)
                    this.patternID2._TextBoxInput.Text = m_gInfo.LinInfo.LinStyleID2.ToString();
                if (this.penWidth.Text.Length == 0)
                    this.penWidth.Text = m_gInfo.LinInfo.LinWidth.ToString();
            }            
        }
    }
}
