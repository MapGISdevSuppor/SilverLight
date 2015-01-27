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

namespace SilverlightDemo.Data
{
    public class PolyData
    {
        public string PlaceName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public PolyData()
        {
            PlaceName = "";
            X = 0;
            Y = 0;
        }
    }
}
