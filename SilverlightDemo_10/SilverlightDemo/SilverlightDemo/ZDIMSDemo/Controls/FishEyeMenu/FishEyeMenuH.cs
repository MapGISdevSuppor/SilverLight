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

using ZDIMS. Util;

namespace ZDIMSDemo.Controls.FishEyeMenu
{
    public class FishEyeMenuH:Canvas
    {
        #region 私有成员
        private String[] imageUris = { "images/image1.png", "images/image2.png", "images/image3.png", "images/image4.png", "images/image5.png", "images/image6.png", "images/image7.png" };    // images
        protected  double MARGIN = 30;			// Margin between images
        protected  double IMAGE_WIDTH = 32;  	// Image width 
        protected  double IMAGE_HEIGHT = 32;	// Image height
        protected  double MAX_SCALE = 2;		// Max scale 
        protected  double MULTIPLIER = 60;		// Control the effectiveness of the mouses
        protected  double PADDING = 10;
        private int imageTotal;
        private String[] imageToolTip = null;

        private List<Image> imageItems = new List<Image>();		// Store the added images
        #endregion

        [Browsable(false)]
        public String[] ImageUris
        {
            get { return this.imageUris; }
            set
            {
                this.imageUris = value;
                this.imageTotal = imageUris.Length;
                this.imageToolTip = new String[imageTotal];
            }
        }

        /// <summary>
        /// 设置每一个按钮的Hint
        /// </summary>
        [Browsable(false)]
        public String[] ImageToolTip
        {
            get
            {
                if (this.imageToolTip == null)
                {
                    for (int i = 0; i < this.imageTotal; i++)
                    {
                        this.imageToolTip[i] = String.Empty;
                    }
                }
                return imageToolTip;
            }
            set
            {
                imageToolTip = value;
            }
        }

        public int ImageTotal
        {
            get { return imageTotal; }
        }

        [Browsable(false)]
        public List<Image> ImageItems
        {
            get { return imageItems; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FishEyeMenuH()
        {
            this.imageTotal = 7;
            this.ImageToolTip = new String[7];
            this.Loaded += new RoutedEventHandler(OnLoaded);
            this.MouseMove += new MouseEventHandler(FishEyeMenu_MouseMove);
            this. MouseEnter += delegate( object sender , MouseEventArgs e )
            {
                this. Cursor = Cursors. Hand;
            };
        }

        public FishEyeMenuH(double w, double h)
        {
            this.imageTotal = 7;
            this.ImageToolTip = new String[7];
            this.IMAGE_HEIGHT = h;
            this.IMAGE_WIDTH = w;
            this.Loaded += new RoutedEventHandler(OnLoaded);
            this.MouseMove += new MouseEventHandler(FishEyeMenu_MouseMove);
            this. MouseEnter += delegate(object sender,MouseEventArgs e ) {
                this. Cursor = Cursors. Hand;
            };
        }

        private void OnLoaded(object sender,RoutedEventArgs e)
        {
            this.CreateControl();
            SolidColorBrush brush = new SolidColorBrush( CommFun. FromUInt32( 0xFFFFFFF0 ) );
            this. Background = brush;
            this. Background. Opacity = 0.5;
        }

        /// <summary>
        /// 向Canvas动态添加图片，定为图片，并绑定MouseMove事件
        /// </summary>
        private void CreateControl()
        {
            if (imageTotal > 0)
            {
                this.addImages();
            }
        }

        public void CreateControl(String[] imgUrls, String[] tipsUrls)
        {
            this.ImageUris = imgUrls;
            this.ImageToolTip = tipsUrls;
            if (imageTotal > 0)
            {
                this.addImages();
            }
        }

        protected void addImages()
        {
            imageItems.Clear();
            this.Children.Clear();
            double[] size = this.CalCanvasSize();
            this. Width = size[0];
            this. Height = size[1];
            for (int i = 0; i < ImageUris.Length; i++)
            {
                string url = imageUris[i];
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri(url, UriKind.Relative))
                };
                image = ResizeIMage(image, IMAGE_WIDTH, IMAGE_HEIGHT, i, imageUris.Length);
                this.Children.Add(image);
                imageItems.Add(image);
                this.BindToolTip();
            }
        }

        private void BindToolTip()
        {
            for (int i = 0; i < ImageItems.Count; i++)
            {
                Image image = ImageItems[i];
                image = ResizeIMage(image, IMAGE_WIDTH, IMAGE_HEIGHT, i, imageUris.Length);
                ToolTipService.SetToolTip(image, ImageToolTip[i]);
                ToolTipService.SetPlacement(image, PlacementMode.Top);
            }
        }

        protected virtual double[] CalCanvasSize()
        {
            double[] size = new double[2];
            size[1] = PADDING * 2 + IMAGE_HEIGHT;
            size[0] = ImageTotal * IMAGE_WIDTH + (ImageTotal) * MARGIN;
            return size;
        }

        protected virtual void FishEyeMenu_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < imageItems.Count; i++)
            {
                Image image = imageItems[i];
                // compute the scale of each image according to the mouse position
                double imageScale = MAX_SCALE - Math.Min(MAX_SCALE - 1, Math.Abs(e.GetPosition(this).X - ((double)image.GetValue(Canvas.LeftProperty) + image.Width / 2)) / MULTIPLIER);
                // resize the image
                ResizeIMage(image, IMAGE_WIDTH * imageScale, IMAGE_HEIGHT * imageScale, i, imageUris.Length);
                // sort the children according to the scale
                image.SetValue(Canvas.ZIndexProperty, (int)Math.Round(IMAGE_WIDTH * imageScale));
            }
        }

        protected virtual Image ResizeIMage(Image image, double imageWidth, double imageHeight, int index, int total)
        {
            image.Width = imageWidth;
            image.Height = imageHeight;
            image.SetValue(Canvas.TopProperty, this.Height / 2 - image.Height / 2);
            if (total % 2 == 0)
            {
                image. SetValue( Canvas. LeftProperty , this. Width / 2 + ( index - total / 2 ) * ( MARGIN + IMAGE_WIDTH ) );
            }
            else
            {
                image. SetValue( Canvas. LeftProperty , this. Width / 2 + ( index - ( total - 1 ) / 2 ) * ( MARGIN + IMAGE_WIDTH ) - image. Width / 2 );
            }
            return image;
        }
    }
}
