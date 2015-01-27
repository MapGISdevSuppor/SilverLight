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
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace ZDIMSDemo.Controls.FishEyeToolMenu
{
    public partial class FishEyeMenuV : UserControl
    {
        #region 私有变量
        private String[] imageUris = { "images/image1.png", "images/image2.png", "images/image3.png", "images/image4.png", "images/image5.png", "images/image6.png", "images/image7.png" };    // images
        private static double IMAGE_WIDTH = 48;	// Image width
        private static double IMAGE_HEIGHT = 48;	// Image height
        private static double MAX_SCALE = 2;		// Max scale 
        private static double MULTIPLIER = 60;		// Control the effectiveness of the mouse
        private static double PADDING = 20;
        private static double MARGIN = 15;			// Margin between images

        private int imageTotal;
        private List<Image> imageItems = new List<Image>();		// Store the added images
        #endregion

        #region 公共属性
        [Browsable(false)]
        public String[] ImageUris
        {
            get { return this.imageUris; }
            set { this.imageUris = value; }
        }
        [Browsable(false)]
        public List<Image> ImageItems
        {
            get { return imageItems; }
        }
        #endregion

        public FishEyeMenuV()
        {
            InitializeComponent();
            imageTotal = ImageUris.Length;
            addImages();
            this.MouseMove += new MouseEventHandler(FishEyeMenu_MouseMove);
            this.Loaded += new RoutedEventHandler(ResizeControl);
        }

        private void ResizeControl(object sender, RoutedEventArgs e)
        {
            this.c1.Width = LayoutRoot.Width;
            this.c1.Height = LayoutRoot.Height;
        }

        private void addImages()
        {
            LayoutRoot.Width = PADDING * 2 + IMAGE_WIDTH;
            LayoutRoot.Height = PADDING * 2 + imageTotal * IMAGE_HEIGHT + (imageTotal - 1) * MARGIN;
           
            for (int i = 0; i < ImageUris.Length; i++)
            {
                string url = ImageUris[i];
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri(url, UriKind.Relative))
                };

                image = ResizeIMage(image, IMAGE_WIDTH, IMAGE_HEIGHT, i, ImageUris.Length);

                LayoutRoot.Children.Add(image);
                imageItems.Add(image);
            }
        }

        void FishEyeMenu_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < imageItems.Count; i++)
            {
                Image image = imageItems[i];


                // compute the scale of each image according to the mouse position
                double imageScale = MAX_SCALE - Math.Min(MAX_SCALE - 1, Math.Abs(e.GetPosition(this.LayoutRoot).Y - ((double)image.GetValue(Canvas.TopProperty) + image.Height / 2)) / MULTIPLIER);
                // resize the image
                ResizeIMage(image, IMAGE_WIDTH * imageScale, IMAGE_HEIGHT * imageScale, i, ImageUris.Length);
                // sort the children according to the scale
                image.SetValue(Canvas.ZIndexProperty, (int)Math.Round(IMAGE_WIDTH * imageScale));
            }
        }

        private Image ResizeIMage(Image image, double imageWidth, double imageHeight, int index, int total)
        {
            image.Width = imageWidth;
            image.Height = imageHeight;
            image.SetValue(Canvas.LeftProperty, LayoutRoot.Width / 2 - image.Width / 2);
           // image.SetValue(Canvas.TopProperty, PADDING + (image.Height * index) + MARGIN * index);
            image.SetValue(Canvas.TopProperty, LayoutRoot.Height / 2 + (index - (total - 1) / 2) * (MARGIN + IMAGE_HEIGHT) - image.Height / 2);
            return image;
        }
    }
}
