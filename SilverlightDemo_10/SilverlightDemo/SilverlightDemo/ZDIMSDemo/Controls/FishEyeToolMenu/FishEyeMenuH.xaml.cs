using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace ZDIMSDemo.Controls.FishEyeToolMenu
{
    public partial class FishEyeMenuH : UserControl
    {
        #region 私有成员
        private String[] imageUris = { "images/image1.png", "images/image2.png", "images/image3.png", "images/image4.png", "images/image5.png", "images/image6.png", "images/image7.png" };    // images
        private static double MARGIN = 15;			// Margin between images
        private static double IMAGE_WIDTH = 32;	// Image width
        private static double IMAGE_HEIGHT = 32;	// Image height
        private static double MAX_SCALE = 2;		// Max scale 
        private static double MULTIPLIER = 60;		// Control the effectiveness of the mouse
        private static double PADDING = 10;
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
                this.CreateControl();
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
                this.CreateControl();
            }
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
            InitializeComponent();
            this.imageTotal = 7;
            this.ImageToolTip = new String[7];
            this.CreateControl();
        }

        private void ResizeControl()
        {
            this.c1.Width = LayoutRoot.Width;
            this.c1.Height = LayoutRoot.Height;
        }

        /// <summary>
        /// 向Canvas动态添加图片，定为图片，并绑定MouseMove事件
        /// </summary>
        private void CreateControl()
        {
            if(imageTotal>0)
            {
                LayoutRoot.Children.Clear();
                this.addImages();
                this.ResizeControl();
                this.MouseMove += new MouseEventHandler(FishEyeMenu_MouseMove);
            }
        }

        private void addImages()
        {
            imageItems.Clear();
            LayoutRoot.Height = PADDING * 2 + IMAGE_HEIGHT;
            LayoutRoot.Width = imageTotal * IMAGE_WIDTH + (imageTotal - 1) * MARGIN;
            for (int i = 0; i < imageUris.Length; i++)
            {
                string url = imageUris[i];
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri(url, UriKind.Relative))
                };
                image = ResizeIMage(image, IMAGE_WIDTH, IMAGE_HEIGHT, i, imageUris.Length);
                ToolTipService.SetToolTip(image, ImageToolTip[i]);
                ToolTipService.SetPlacement(image, PlacementMode.Top);
                LayoutRoot.Children.Add(image);
                imageItems.Add(image);
            }
        }

        private void FishEyeMenu_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < imageItems.Count; i++)
            {
                Image image = imageItems[i];


                // compute the scale of each image according to the mouse position
                double imageScale = MAX_SCALE - Math.Min(MAX_SCALE - 1, Math.Abs(e.GetPosition(this.LayoutRoot).X - ((double)image.GetValue(Canvas.LeftProperty) + image.Width / 2)) / MULTIPLIER);
                // resize the image
                 ResizeIMage(image, IMAGE_WIDTH * imageScale, IMAGE_HEIGHT * imageScale, i, imageUris.Length);
                // sort the children according to the scale
                image.SetValue(Canvas.ZIndexProperty, (int)Math.Round(IMAGE_WIDTH * imageScale));
            }
        }

        private Image ResizeIMage(Image image, double imageWidth, double imageHeight, int index, int total)
        {
            image.Width = imageWidth;
            image.Height = imageHeight;
            image.SetValue(Canvas.TopProperty, LayoutRoot.Height / 2 - image.Height / 2);
           // image.SetValue(Canvas.TopProperty, PADDING + (image.Height * index) + MARGIN * index);
            image.SetValue(Canvas.LeftProperty, LayoutRoot.Width / 2 + (index - (total - 1) / 2) * (MARGIN + IMAGE_WIDTH) - image.Width / 2);
            return image;
        }
    }
}