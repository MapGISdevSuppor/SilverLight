using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ZDIMS.Drawing;
using System.Collections.Generic;
using ZDIMS.Interface;
using System.Windows.Media.Imaging;

namespace Logistics_system.controls
{
    public class BusRoad
    {
        public MarkLayer MarkLayer;
        public GraphicsLayer GraphicsLayer;
        public string[] RoadCoorArr;
        List<IMSMark> m_stopList = new List<IMSMark>();
        IMSPolyline m_road;
        public BusRoad(MarkLayer MarkLayer, GraphicsLayer GraphicsLayer)
        {
            this.MarkLayer = MarkLayer;
            this.GraphicsLayer = GraphicsLayer;
        }

        public void AddNode(string name, double x, double y, string iconSrc)
        {
            if (MarkLayer != null)
            {
                StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
                panel.Children.Add(new Image() { Source = new BitmapImage(new Uri(iconSrc, UriKind.Relative)) });
                panel.Children.Add(new TextBlock() { Text = name });
                IMSMark mark = new IMSMark(panel, CoordinateType.Logic, MarkLayer) { EnableAnimation = false, EnableDrag = false };
                mark.X = x;
                mark.Y = y;
                MarkLayer.AddMark(mark);
                m_stopList.Add(mark);
            }
        }
        public void Draw()
        {
            if (GraphicsLayer != null && RoadCoorArr.Length>0)
            {
                if (m_road == null)
                {
                    m_road = new IMSPolyline(CoordinateType.Logic);
                    m_road.Shape.Stroke = new SolidColorBrush(Colors.Red);
                    GraphicsLayer.AddGraphics(m_road);
                }
                for (int i = 0; i < RoadCoorArr.Length- 1; i+=2)
                {
                    if (RoadCoorArr[i].Length > 0 && RoadCoorArr[i+1].Length > 0)
                    {
                        m_road.Points.Add(new Point(Convert.ToDouble(RoadCoorArr[i]), Convert.ToDouble(RoadCoorArr[i+1])));
                    }
                }
                m_road.Draw();
                for (int i = 0; i < m_stopList.Count; i++)
                {
                    MarkLayer.AddMark(m_stopList[i]);
                    m_stopList[i].RevisedPosition();
                }
            }
        }
        public void Clear()
        {
            //if (m_road != null && GraphicsLayer != null)
            //{
            //    GraphicsLayer.RemoveGraphics(m_road);
            //    m_road = null;
            //}   
            if (MarkLayer != null)
            {
                for (int i = 0; i < m_stopList.Count; i++)
                    MarkLayer.RemoveMark(m_stopList[i]);
                //m_stopList.Clear();
            }
        }
    }
}
