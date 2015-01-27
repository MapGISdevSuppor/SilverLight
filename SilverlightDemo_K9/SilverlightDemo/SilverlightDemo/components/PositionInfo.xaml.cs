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
using SilverlightDemo.components;
using ZDIMS.Map;
namespace SilverlightDemo.components
{
    public partial class PositionInfo : BaseUserControl
    {
        public IMSMap m_mapContainer = new IMSMap();
        public PositionInfo(IMSMap mapContainer)
        {
            InitializeComponent();
            this.m_mapContainer = mapContainer;
            onMousePosChange();
            this.mydialogPanel.OnClose += new RoutedEventHandler(onClose);
        }
        public void onMousePosChange()
        {
            this.m_mapContainer.MouseMove += new MouseEventHandler(m_mapContainer_MouseMove);

        }

        void m_mapContainer_MouseMove(object sender, MouseEventArgs e)
        {
            this.LabelMouseX.Text = "Mouse Screen X:" + this.m_mapContainer.MouseMoveScrPnt.X.ToString();
            this.LabelMouseY.Text = "Mouse Screen Y:" + this.m_mapContainer.MouseMoveScrPnt.Y.ToString();
            this.LableMouseLogicX.Text = "Mouse Logic X:" + this.m_mapContainer.MouseMoveLogicPnt.X.ToString();
            this.LableMouseLogicY.Text = "Mouse Logic Y:" + this.m_mapContainer.MouseMoveLogicPnt.Y.ToString();
        }
        public void onClose(object sender, RoutedEventArgs e)
        {
            this.m_mapContainer.MouseMove -= m_mapContainer_MouseMove;
            this.Close();

        }
    }
}
