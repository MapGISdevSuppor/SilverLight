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
using ZDIMS.Interface;

namespace ZDIMSDemo.Controls
{
   // [TemplatePart(Name = IMG_Container, Type = typeof(Canvas))]
    public class Magnifier : Control
    {
       // private const string IMG_Container = "imgContainer";
        private IMSMap m_mapContainer = null;//地图容器对象
        private double tmpCanvasLeft = 0;
        private double tmpCanvasTop = 0;
        public Canvas imgContainer;

        private double imgContainerWidth = 0;
        private double imgContainerHeight = 0;

        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set { m_mapContainer = value; }
        }
        public Magnifier()
        {
            this.DefaultStyleKey = typeof(Magnifier);
            this.Loaded += new RoutedEventHandler(Magnifier_Loaded);
        }

        void Magnifier_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null && this.Parent is IMSMap)
            {
                this.MapContainer = this.Parent as IMSMap;
                this.MapContainer.OperType = IMSOperType.None;
            }
           // OnApplyTemplate();
            //this.OnApplyTemplate();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.MapContainer == null)
                return;
            //this.imgContainer = (Canvas)this.GetTemplateChild(IMG_Container);
            if (this.imgContainer != null && this.MapContainer != null && this.Parent is IMSMap)
            {
                this.MapContainer.MouseMove += new MouseEventHandler(MapContainer_MouseMove);
                this.MapContainer.MouseLeftButtonDown += new MouseButtonEventHandler(MapContainer_MouseLeftButtonDown);
                this.MapContainer.MapOperTypeBeforeChange += new ZDIMS.Event.IMSMapEventHandler(MapContainer_MapOperTypeBeforeChange);
                this.MapContainer.MouseWheel += new MouseWheelEventHandler(MapContainer_MouseWheel);
                RefreshImage();
            }
        }

        void MapContainer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.MapContainer_MouseLeftButtonDown(null, null);
            e.Handled = true;
        }

        void MapContainer_MapOperTypeBeforeChange(ZDIMS.Event.IMSMapEvent e)
        {
            this.MapContainer_MouseLeftButtonDown(null, null);
        }

        void MapContainer_MapViewChange(ZDIMS.Event.IMSMapEvent e)
        {
            RefreshImage();
        }

        private void RefreshImage()
        {
            this.imgContainerHeight = this.MapContainer.Height * 2;
            this.imgContainerWidth = this.MapContainer.Width * 2;
            HashTable tileLayers = this.MapContainer.TileLayerList;
            ScaleTransform zoom = new ScaleTransform();
            zoom.CenterX = 150;
            zoom.CenterY = 150;
            zoom.ScaleX = 2;
            zoom.ScaleY = 2;
            for (int i = 0; i < tileLayers.Length; i++)
            {
                if ((tileLayers.GetItemByIndex(i) as ITile).EnableFillImg &&
                    (tileLayers.GetItemByIndex(i) as UIElement).Visibility == Visibility.Visible)
                {
                    IMap tmap = (tileLayers.GetItemByIndex(i) as IMap);
                    Image imgTmp = tmap.DrawToImage();
                    imgTmp.RenderTransform = zoom;
                    this.imgContainer.Children.Add(imgTmp);
                    FrameworkElement ui = (FrameworkElement)(tileLayers.GetItemByIndex(i));
                    tmpCanvasLeft = Canvas.GetLeft(ui);
                    tmpCanvasTop = Canvas.GetTop(ui);
                    this.imgContainerWidth = ui.Width * 2;
                    this.imgContainerHeight = ui.Height * 2;
                }
            }
            HashTable vectorLayers = this.MapContainer.VectorLayerList;
            for (int j = 0; j < vectorLayers.Length; j++)
            {
                if ((vectorLayers.GetItemByIndex(j) as UIElement).Visibility == Visibility.Visible)
                {
                    IMap vmap = (vectorLayers.GetItemByIndex(j) as IMap);
                    Image imgTmp = vmap.DrawToImage();
                    imgTmp.RenderTransform = zoom;
                    this.imgContainer.Children.Add(imgTmp);
                    Canvas.SetLeft(imgTmp, -tmpCanvasLeft * 2);
                    Canvas.SetTop(imgTmp, -tmpCanvasTop * 2);
                }
            }
        }
        void MapContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.MapContainer == null)
                return;
            this.MapContainer.MouseMove -= new MouseEventHandler(MapContainer_MouseMove);
            this.MapContainer.MouseLeftButtonDown -= new MouseButtonEventHandler(MapContainer_MouseLeftButtonDown);
            this.MapContainer.MapOperTypeBeforeChange -= new ZDIMS.Event.IMSMapEventHandler(MapContainer_MapOperTypeBeforeChange);
            this.MapContainer.MouseWheel -= new MouseWheelEventHandler(MapContainer_MouseWheel);
            this.MapContainer.RemoveChild(this);
            if (e != null)
                e.Handled = true;
        }
        void MapContainer_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition((UIElement)this.MapContainer);
            EllipseGeometry circle = this.imgContainer.Clip as EllipseGeometry;
            circle.Center = new Point((mousePos.X - tmpCanvasLeft) * 2 - 150, (mousePos.Y - tmpCanvasTop) * 2 - 150);
            this.imgContainer.Width = this.imgContainerWidth;
            this.imgContainer.Height = this.imgContainerHeight;
            Canvas.SetLeft(this, mousePos.X - 150);
            Canvas.SetTop(this, mousePos.Y - 150);
            Canvas.SetLeft(this.imgContainer, (-mousePos.X + tmpCanvasLeft) * 2 + 300);
            Canvas.SetTop(this.imgContainer, (-mousePos.Y + tmpCanvasTop) * 2 + 300);
           
        }
    }
}
