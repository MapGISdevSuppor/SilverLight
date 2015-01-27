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
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace ZDIMSDemo.Controls.FishEyeMenu
{
    public class FishEyeMenuV:FishEyeMenuH
    {
        public FishEyeMenuV():base(48,48)
        {
            // InitializeComponent();
        }

        protected override double[] CalCanvasSize()
        {
            double[] size = new double[2];
            size[1] = PADDING * 2 + ImageTotal * IMAGE_HEIGHT + (ImageTotal - 1) * MARGIN;
            size[0] = PADDING * 4 + IMAGE_WIDTH;
            return size;
        }

        protected override void FishEyeMenu_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < ImageItems.Count; i++)
            {
                Image image = ImageItems[i];

                // compute the scale of each image according to the mouse position
                double imageScale = MAX_SCALE - Math.Min(MAX_SCALE - 1, Math.Abs(e.GetPosition(this).Y - ((double)image.GetValue(Canvas.TopProperty) + image.Height / 2)) / MULTIPLIER);
                // resize the image
                ResizeIMage(image, IMAGE_WIDTH * imageScale, IMAGE_HEIGHT * imageScale, i, ImageUris.Length);
                // sort the children according to the scale
                image.SetValue(Canvas.ZIndexProperty, (int)Math.Round(IMAGE_WIDTH * imageScale));
            }
        }

        protected override Image ResizeIMage(Image image, double imageWidth, double imageHeight, int index, int total)
        {
            image.Width = imageWidth;
            image.Height = imageHeight;
            image.SetValue(Canvas.LeftProperty, this.Width / 2 - image.Width / 2);
            // image.SetValue(Canvas.TopProperty, PADDING + (image.Height * index) + MARGIN * index);
            if (total % 2 == 0)
            {
                 image. SetValue( Canvas. TopProperty , this. Height / 2 + ( index - total / 2 ) * ( MARGIN + IMAGE_HEIGHT ));
            }
            else
            {
                image. SetValue( Canvas. TopProperty , this. Height / 2 + ( index - ( total - 1 ) / 2 ) * ( MARGIN + IMAGE_HEIGHT ) - image. Height / 2 );
            }
            return image;
        }
    }
}

