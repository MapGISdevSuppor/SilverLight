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
using ZDIMS.Map;
using System.ComponentModel;

namespace ZDIMSDemo.Controls
{
    public partial class Scale : UserControl
    {
        private IMSMap m_mapContainer = null;
        private double m_rate = 1;
        /// <summary>
        /// 实际距离与图面距离的比值
        /// </summary>
       [Browsable(true), Category("MapGisIMS"), Description("实际距离与图面距离的比值")]
        public double Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }
        private string m_label = "米";
        /// <summary>
        /// 显示的长度单位标签
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("显示的长度单位标签")]
        public string Lable
        {
            get { return m_label; }
            set { m_label = value; }
        }
        /// <summary>
        /// 关联的地图容器
        /// </summary>
        [Browsable(true), Category("MapGisIMS"), Description("关联的地图容器")]
        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set
            {
                m_mapContainer = value;
                this.m_mapContainer.MapZoomOver += new ZDIMS.Event.IMSMapEventHandler(m_mapContainer_MapViewChange);
                this.m_mapContainer.IMSResizeOver += new SizeChangedEventHandler(m_mapContainer_IMSResizeOver);
            }
        }

        void m_mapContainer_IMSResizeOver(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(this, 100);
            Canvas.SetTop(this, this.MapContainer.Height - 50);
            Canvas.SetZIndex(this, 1000);
        }
        public Scale()
        {
            InitializeComponent();
        }
        void m_mapContainer_MapViewChange(ZDIMS.Event.IMSMapEvent e)
        {
            double l=this.m_mapContainer.GetBuffer(100)*100000;
            if (l > 1000)
            {
                l = l / 1000;
                Lable = "千米";
            }
            else
                Lable = "米";
            string len = Math.Round(l).ToString();
            this.length.Content = len + Lable;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null && this.Parent != null && this.Parent is IMSMap)
                this.MapContainer = this.Parent as IMSMap;
            if (this.MapContainer != null)
                m_mapContainer_IMSResizeOver(null, null);
        }
    }
}
