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
using EasySL.Controls;
using System.Windows.Controls.Primitives;
using ZDIMS.Drawing;
using ZDIMS.Util;
using ZDIMS.Interface;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using ZDIMS.Event;
using System.Windows.Browser;

namespace SilverlightDemo
{
    public partial class ShowDifferentMap : Page
    {
        string TileLayerAddress = "";
        private Point pnt;
        public ShowDifferentMap()
        {
            InitializeComponent();
            iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
        }
        //添加标注到地图上
        void iMSMap1_MapReady(IMSMapEvent e)
        {
            MarkLayer m_markLayer = new MarkLayer();//初始化标注图层
            iMSMap1.AddChild(m_markLayer); //添加到地图容器中
            IMSMark mark; //定义标注对象
            pnt = new Point(114.331901, 30.548992);
            m_markLayer.EnableZoomAnimation = true; //允许随地图缩放
            m_markLayer.EnableMarkHiden = false; //不允许隐藏标注
            m_markLayer.EnablePolymericMark = true; //允许标注聚合功能
            mark = new IMSMark(new Image()//初始化标注对象
            {
                Source = new BitmapImage(new Uri("/Images/markimg1.png", UriKind.Relative)),
                Width = 80,
                Height = 80
            }, CoordinateType.Logic, m_markLayer);
            mark.X = pnt.X; //设置标注坐标
            mark.Y = pnt.Y;
            m_markLayer.AddMark(mark); //添加到标注图层
            mark.MouseClickCallback = new MouseOperDelegate(mark_click);
          //  mark.MouseEnterCallback = new MouseOperDelegate(mark_enter);
           // mark.MouseLeaveCallback = new MouseOperDelegate(mark_leave);
            Canvas.SetZIndex(DFwin, Canvas.GetZIndex(this.iMSMap1) + 12);
            DFwin.Margin = new Thickness(iMSMap1.LogicToScreen(pnt.X, pnt.Y).X + 15, iMSMap1.LogicToScreen(pnt.X, pnt.Y).Y + 15, 0, 0);
            DFwin.Visibility = Visibility.Collapsed;
            DFwin.AllowDrop = false;
            iMSMap1.MapViewChange += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapViewChange);
        }
        public void mark_enter(IMSMark mark,object obj, MouseEventArgs e)
        {
            if (DFwin.Visibility != Visibility.Visible)
            {
         
                DFwin.Margin = new Thickness(iMSMap1.LogicToScreen(pnt.X, pnt.Y).X + 15, iMSMap1.LogicToScreen(pnt.X, pnt.Y).Y + 15, 0, 0);
                DFwin.Visibility = Visibility.Visible;
                DFwin.AllowDrop = false;
            }   
        }
        public void mark_leave(IMSMark mark,object obj, MouseEventArgs e){
            if (DFwin.Visibility == Visibility.Visible)
            {
                DFwin.Visibility = Visibility.Collapsed;
         
            } 
        }

        //控制标注的点击事件
        void mark_click(IMSMark mark, object obj, MouseEventArgs e)
        {
            if (DFwin.Visibility == Visibility.Visible)
            {
                DFwin.Visibility = Visibility.Collapsed;
            }
            else {
                DFwin.Margin = new Thickness(iMSMap1.LogicToScreen(pnt.X, pnt.Y).X + 15, iMSMap1.LogicToScreen(pnt.X, pnt.Y).Y + 15, 0, 0);
                DFwin.Visibility = Visibility.Visible;
                DFwin.AllowDrop = false;
            }
           
        }
        void iMSMap1_MapViewChange(ZDIMS.Event.IMSMapEvent e)
        {
              DFwin.Margin = new Thickness(iMSMap1.LogicToScreen(pnt.X, pnt.Y).X + 15, iMSMap1.LogicToScreen(pnt.X, pnt.Y).Y + 15, 0, 0);
        }
        //页面加载的时候给瓦片地图的地址ServerAddress赋值
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            iMSMap1.OperType = IMSOperType.Drag;
            tileLayer1.ServerAddress = TileLayerAddress;
            tileLayerWh.ServerAddress = TileLayerAddress;
        }

    }
}
