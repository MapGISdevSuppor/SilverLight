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
using ZDIMS.Util;

namespace ZDIMSDemo.Controls
{
    public partial class NavigationBar : UserControl
    {
        private IMSMap m_mapContainer = null;
        /// <summary>
        /// 关联的地图容器
        /// </summary>
        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set { m_mapContainer = value;
            if (value != null)
            {
                this.slider1.Maximum = m_mapContainer.LevelNum - 1;
                this.m_mapContainer.MapViewChange += new ZDIMS.Event.IMSMapEventHandler(m_mapContainer_MapViewChange);
            }
            }
        }

        void m_mapContainer_MapViewChange(ZDIMS.Event.IMSMapEvent e)
        {
            if (this.m_mapContainer.TileLayerLength > 0)
                this.slider1.Value = this.m_mapContainer.CurMapLevel;
        }
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer==null&&this.Parent != null && this.Parent is IMSMap)
                this.MapContainer = this.Parent as IMSMap;

        }

        private bool CheckMapContainer()
        {
            if (this.m_mapContainer == null)
            {
                MessageBox.Show("MapContainer属性为空","属性错误",MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private void image_up_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckMapContainer()) return;
            double buffer = this.MapContainer.GetBuffer(256);
            this.MapContainer.PanTo(this.MapContainer.CenPntLogCoor.X,this.MapContainer.CenPntLogCoor.Y+buffer);
            e.Handled = true;
        }

        private void image_zoomin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckMapContainer()) return;
            if (this.MapContainer.TileLayerLength > 0)
            {
                int lvl = this.MapContainer.CurMapLevel + 1;
                lvl = lvl >= this.MapContainer.LevelNum ? this.MapContainer.LevelNum - 1 : lvl;
                this.MapContainer.SetLevel(lvl);
                this.slider1.Value = lvl;
            }
            else
            {
                RectBound rc = this.MapContainer.WinViewBound;
                double dx = (rc.XMax - rc.XMin)/4;
                double dy = (rc.YMax - rc.YMin)/4;
                rc.XMin += dx;
                rc.XMax -= dx;
                rc.YMin += dy;
                rc.YMax -= dy;
                this.MapContainer.JumpByRectBound(rc);
            }
            e.Handled = true;
        }

        private void image_zoomout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckMapContainer()) return;
            if (this.MapContainer.TileLayerLength > 0)
            {
                int lvl = this.MapContainer.CurMapLevel - 1;
                lvl = lvl < 0 ? 0 : lvl;
                this.MapContainer.SetLevel(lvl);
                this.slider1.Value = lvl;
            }
            else
            {
                RectBound rc = this.MapContainer.WinViewBound;
                double dx = (rc.XMax - rc.XMin) / 2;
                double dy = (rc.YMax - rc.YMin) / 2;
                rc.XMin -= dx;
                rc.XMax += dx;
                rc.YMin -= dy;
                rc.YMax += dy;
                this.MapContainer.JumpByRectBound(rc);
            }
            e.Handled = true;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!CheckMapContainer()) return;
            if(this.MapContainer.TileLayerLength>0)
                this.MapContainer.SetLevel((int)e.NewValue);
        }

        private void image_restore_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckMapContainer()) return;
            this.MapContainer.Restore();
            e.Handled = true;
        }

        private void image_down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckMapContainer()) return;
            double buffer = this.MapContainer.GetBuffer(128);
            this.MapContainer.PanTo(this.MapContainer.CenPntLogCoor.X, this.MapContainer.CenPntLogCoor.Y - buffer);
            e.Handled = true;
        }

        private void image_left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckMapContainer()) return;
            double buffer = this.MapContainer.GetBuffer(128);
            this.MapContainer.PanTo(this.MapContainer.CenPntLogCoor.X-buffer, this.MapContainer.CenPntLogCoor.Y);
            e.Handled = true;
        }

        private void image_right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckMapContainer()) return;
            double buffer = this.MapContainer.GetBuffer(128);
            this.MapContainer.PanTo(this.MapContainer.CenPntLogCoor.X + buffer, this.MapContainer.CenPntLogCoor.Y);
            e.Handled = true;
        }
    }
}
