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

namespace Logistics_system
{//构造矢量线类
    public class VectorLine
    {
        double paraY;

        public double ParaY
        {
            get { return paraY; }
            set { paraY = value; }
        }
        double paraX;

        public double ParaX
        {
            get { return paraX; }
            set { paraX = value; }
        }

        public VectorLine()
        {

        }

        //fp：起点，tp：终点
        public VectorLine(Point fp, Point tp)
        {
            paraY = tp.Y - fp.Y;
            paraX = tp.X - fp.X;

        }
        public VectorLine(double pY, double pX)
        {
            paraY = pY;
            paraX = pX;
        }

    }
}
