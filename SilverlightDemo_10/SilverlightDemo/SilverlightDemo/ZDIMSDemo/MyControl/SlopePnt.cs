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

namespace ZDIMSDemo.MyControl
{
    public class SlopePnt
    {
        private Point pnt;

        public Point Pnt
        {
            get { return pnt; }
            set { pnt = value; }
        }

        private double pntSlopeAngle;

        public double PntSlopeAngle
        {
            get { return pntSlopeAngle; }
            set { pntSlopeAngle = value; }
        }

    }
}
