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
using ZDIMS.Event;
using ZDIMS.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZDIMS.Interface;
using ZDIMSDemo. Controls;

namespace ZDIMSDemo
{
    public partial class SearchDemo : UserControl
    {
        public SearchDemo()
        {
            InitializeComponent();
            iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
            iMSMap1.Loaded += new RoutedEventHandler(IMSMap_OnLoaded);
            this.toolMainMenu.MouseLeftButtonDown += new MouseButtonEventHandler(toolMainMenu_MouseLeftButtonDown);
        }

        private void IMSMap_OnLoaded ( object sender , RoutedEventArgs e )
        {
            LocationFishEyeMainMenu();
            ShowDefaultSubMenu( LoadNavigator( ));
        }

        /// <summary>
        /// 加载默认数据目录，状态为隐藏
        /// </summary>
        private void CreateHiddenTree ( )
        {
            FloatTreeView tempFloatTree = new FloatTreeView( );
            tempFloatTree. MapTree. MapContainer = iMSMap1;
            this. toolMainMenu. FloatTree = tempFloatTree;
            tempFloatTree. SetValue( Canvas. ZIndexProperty , 9999 );
            tempFloatTree. Visibility = Visibility. Collapsed;
        }

        private NavigationBar LoadNavigator ( )
        {
            NavigationBar navBar = new NavigationBar( );
            Canvas. SetZIndex( navBar , 2000 );
            iMSMap1. AddChild( navBar );
            this. toolMainMenu. NavBar = navBar;
            return navBar;
        }

        private void ShowDefaultSubMenu (NavigationBar nav)
        {
            this. toolMainMenu. NavBar = nav;
            this. toolMainMenu. ShowDisplaySubMenu( );
        }

        void toolMainMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void floatTreeWindow_LeftBtnDown(object sender,MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void iMSMap1_MapReady(IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {

                IMSMark mark = new IMSMark(new Image()
                {
                    Source = new BitmapImage(new Uri("images/pin.png", UriKind.Relative)),
                    Width = 36,
                    Height = 48
                }, ZDIMS.Interface.CoordinateType.Logic)
                {
                    X = 114.125686114315,
                    Y = 30.4582609083728,
                    EnableDrag = true
                };
                markLayer1.AddMark(mark);
                int totalNumber = 50;
                int count = 0;
                double xMin = 114.125686114315;
                double yMin = 30.4582609083728;
                double xMax = 114.500788705197;
                double yMax = 30.7085740673183;
                double centerX = (xMin + xMax) / 2;
                double centerY = (yMin + yMax) / 2;
                Point pnt;
                //IMSMark mark;
                Random rnd = new Random();
                markLayer1.EnableZoomAnimation = true;
                markLayer1.EnableMarkHiden = false;
                markLayer1.EnablePolymericMark = true;
                int n = 1;
                while (count < totalNumber)
                {
                    pnt = GetRandomCoordinate(rnd.Next(0, 1000) + rnd.Next(0, 100) + rnd.Next(0, 10), centerX, centerY, 0.025);
                    centerX = pnt.X;
                    centerY = pnt.Y;
                    if (centerX > xMin && centerX < xMax && centerY > yMin && centerY < yMax)
                    {
                        count++;
                        mark = new IMSMark(new Image()
                        {
                            Source = new BitmapImage(new Uri("/images/mark/p" + n.ToString() + ".png", UriKind.Relative)),
                            Width = 16,
                            Height = 18
                        }, CoordinateType.Logic, markLayer1);
                        mark.X = centerX;//114.28793904776113;//
                        mark.Y = centerY;//30.549563956006946;//
                        //mark.EnableDrag = true;
                        markLayer1.AddMark(mark);
                        //mark.Flicker(400, 10);
                        n = (++n) % 8 > 0 ? n % 8 : 1;
                    }
                    else
                    {
                        centerX = (xMin + xMax) / 2;
                        centerY = (yMin + yMax) / 2;
                    }
                }
                markLayer1.InitPolymericMark();
            }
        }

        void LocationFishEyeMainMenu()
        {
            this.toolMainMenu.SetValue(Canvas.ZIndexProperty, 10000);
            this.toolMainMenu.ImsMap = this.iMSMap1;
            this.toolMainMenu.GraphicLayer = this.graphicsLayer1;
            this.toolMainMenu.MarkLayer = this.markLayer2;
            this. toolMainMenu. EagleEye = this. iMSEagleEye1;
            this.toolMainMenu.ReLocation();
            this.iMSMap1.IMSResizeOver += new SizeChangedEventHandler(iMSMap1_IMSResizeOver);
        }

        void iMSMap1_IMSResizeOver(object sender, SizeChangedEventArgs e)
        {
            this.toolMainMenu.ReLocation();
        }

        void collapse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
        }
        private Point GetRandomCoordinate(int index, double centerX,
                                        double centerY, double temp)
        {//获取随机点
            switch (index % 16)
            {
                case 0:
                    centerX = centerX + temp;
                    //centerY = centerY;
                    break;
                case 1:
                    centerX = centerX + temp * 0.866;
                    centerY = centerY + temp / 2;
                    break;
                case 2:
                    centerX = centerX + temp * 0.707;
                    centerY = centerY + temp * 0.707;
                    break;
                case 3:
                    centerX = centerX + temp / 2;
                    centerY = centerY + temp * 0.866;
                    break;
                case 4:
                    //centerX = centerX;
                    centerY = centerY + temp;
                    break;
                case 5:
                    centerX = centerX - temp / 2;
                    centerY = centerY + temp * 0.866;
                    break;
                case 6:
                    centerX = centerX - temp * 0.707;
                    centerY = centerY + temp * 0.707;
                    break;
                case 7:
                    centerX = centerX - temp * 0.866;
                    centerY = centerY + temp / 2;
                    break;
                case 8:
                    centerX = centerX - temp;
                    //centerY = centerY;
                    break;
                case 9:
                    centerX = centerX - temp * 0.866;
                    centerY = centerY - temp / 2;
                    break;
                case 10:
                    centerX = centerX - temp * 0.707;
                    centerY = centerY - temp * 0.707;
                    break;
                case 11:
                    centerX = centerX - temp / 2;
                    centerY = centerY - temp * 0.866;
                    break;
                case 12:
                    //centerX = centerX;
                    centerY = centerY - temp;
                    break;
                case 13:
                    centerX = centerX + temp / 2;
                    centerY = centerY - temp * 0.866;
                    break;
                case 14:
                    centerX = centerX + temp * 0.707;
                    centerY = centerY - temp * 0.707;
                    break;
                case 15:
                    centerX = centerX + temp * 0.866;
                    centerY = centerY - temp / 2;
                    break;
            }
            return new Point(centerX, centerY);
        }
    }
}
