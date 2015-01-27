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
    public partial class MarkInfo : BaseUserControl
    {
        public IMSMap m_mapcontainer=null;
        public double m_logicX = 0;
        public double m_logicY = 0;
        public MarkInfo()
        {
            InitializeComponent();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
        }

        private void Close(object sender,RoutedEventArgs e)
        {
            this.Close();
        }
       public IMSMap mapContainer
       {
           get { return m_mapcontainer; }
           set { m_mapcontainer = value; }
       }
       public double logicX
       {
           get { return m_logicX; }
           set { m_logicX = value; }
       }

       public double logicY
       {
           get { return m_logicY; }
           set { m_logicY = value; }
       }
       public void updateposition()
       {
           Point pnt = new Point();
           pnt = this.mapContainer.LogicToScreen(logicX,logicY);
           this.VerticalAlignment = VerticalAlignment.Top;
           this.HorizontalAlignment = HorizontalAlignment.Left;
           this.Margin = new Thickness(pnt.X+300,pnt.Y+130,0,0);
       }
    }
}
