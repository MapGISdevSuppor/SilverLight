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

namespace SilverlightDemo.Demo
{
    public partial class MapMagnifier : Page
    {
        private ZDIMSDemo.Controls.Magnifier m_magnifier;
        public MapMagnifier()
        {
            InitializeComponent();
            iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);
        }
        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            Canvas.SetZIndex(imgwin, 180);
            Canvas.SetZIndex(img, 200);
            Canvas.SetZIndex(imgContainer,Canvas.GetZIndex(iMSMap1)+12);
        }
        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (this.m_magnifier == null)
            {
                this.m_magnifier = new ZDIMSDemo.Controls.Magnifier();
               this.iMSMap1.AddChildAt(this.m_magnifier, 1000);
               // this.iMSMap1.AddChild(this.m_magnifier);
                m_magnifier.MapContainer = iMSMap1;
                m_magnifier.imgContainer = this.imgContainer;
                this.imgContainer.Visibility = Visibility.Visible;
                m_magnifier.Visibility = Visibility.Visible;
            }
            else
            {

                if (this.iMSMap1.Contains(this.m_magnifier))
                {
                    this.iMSMap1.RemoveChild(this.m_magnifier);
                    this.imgContainer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.m_magnifier = new ZDIMSDemo.Controls.Magnifier();
                    m_magnifier.MapContainer = iMSMap1;
                    m_magnifier.imgContainer = this.imgContainer;
                    this.imgContainer.Visibility = Visibility.Visible;
                   this.iMSMap1.AddChildAt(this.m_magnifier, 1000);
                }
            }
        }
 
    }
}
