using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ZDIMS.Map;
using ZDIMS.Drawing;
using ZDIMSDemo.Controls;
using ZDIMS.Util;
using System.Windows.Threading;
namespace Logistics_system.controls
{
    public partial class trackplaycontrol : BaseUserControl
    {
        //public MarkLayer MarkLayer;
        trackplay track;
        bool playFlag = false;
        public List<Point> Points;
        string[] pnts;
        List<BusRoad> _road = new List<BusRoad>();
        BusRoad curRoad;
        ComboBox _selectComboBox;
        public String ServerAddress { get; set; }
        public MarkLayer MarkLayer { get; set; }
        public GraphicsLayer GraphicsLayer { get; set; }
        DispatcherTimer m_timer;
        public marketsInfo centerInfo;
        public marketsInfo marketInfo;
        List<Point> pntList;
        List<stops> stopsData = new List<stops>();
        string m_key = "";
        public trackplaycontrol(MarkLayer markLayer, GraphicsLayer graphicsLayer)
        {
            InitializeComponent();
            MarkLayer = markLayer;
            GraphicsLayer = graphicsLayer;
            this.pause.MouseEnter += new MouseEventHandler(pause_MouseEnter);
            this.stop.MouseEnter += new MouseEventHandler(pause_MouseEnter);
            this.stop.MouseLeave += new MouseEventHandler(stop_MouseLeave);
            this.pause.MouseLeave += new MouseEventHandler(stop_MouseLeave);
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
            stopsData = setstopsData();

        }

        void stop_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img=sender as  Image;
            img.Width=35;
            img.Height=35;
        }

        void pause_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = sender as Image;
            img.Width = 40;
            img.Height = 40;
        }

        private void stop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.goOn = false;
            this.playFlag = false;
            this.track.stop();
            reSetbutton();
            this.track.m_markLayer.MapContainer.Refresh();
            //playFlag = false;
            //track.stop();
            this.pause.Source = new BitmapImage(new Uri("../images/play/play.png", UriKind.Relative));
            //this.stop.IsEnabled = false;
        }
        private bool goOn = false;
        private void pause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (track == null)
            {
                Points = pntList;
                track = new trackplay();
                track.m_markLayer = this.MarkLayer;
                track.GetAllPntsOnLine(Points);
            }
            track.playOverBackHander += playOver;
            if (!playFlag)
            {

                playFlag = true;
                this.pause.Source = new BitmapImage(new Uri("../images/play/pause.png", UriKind.Relative));
                //this.stop.IsEnabled = true;
                //track.play();
                if (this.goOn)
                {
                    this.goOn = false;
                    this.track.Start();
                }
                else
                {
                    track.play();
                }
              
            }
            else
            {

                this.goOn = true;
                playFlag = false;
                track.pause();
                this.pause.Source = new BitmapImage(new Uri("../images/play/play.png", UriKind.Relative));
            }
            //Image img = sender as Image;
            //img.Source = new BitmapImage(new Uri("../images/play/pause.png", UriKind.Relative));
        }
        private void playOver()
        {

            reSetbutton();
            this.track.playOverBackHander -= playOver;
        }
        public void reSetbutton()
        {
            this.playFlag = false;

            this.pause.Source = new BitmapImage(new Uri("../images/play/play.png", UriKind.Relative));
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Storyboard stbContentBgBack = Resources["stbContentBgBack"] as Storyboard;
            this.Close();
        }

        public List<stops> setstopsData()
        {
            List<stops> list = new List<stops>();
            stops tmp = new stops();
            tmp.X = 12728647.712585449;
            tmp.Y = 3579603.3365478516;
            tmp.StopName = "团结村";
            list.Add(tmp);

            tmp = new stops();
            tmp.X = 12729488.519714356;
            tmp.Y = 3578924.9580688477;
            tmp.StopName = "徐东大街";
            list.Add(tmp);


            tmp = new stops();
            tmp.X = 12730635.074890137;
            tmp.Y = 3577921.7222900391;
            tmp.StopName = "徐东大街";
            list.Add(tmp);


            tmp = new stops();
            tmp.X = 12731867.621704102;
            tmp.Y = 3577596.8649902344;
            tmp.StopName = "东湖路";
            list.Add(tmp);

            return list;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            Clear();
            this.Close();
        }
        private void Clear()
        {
            if (this.track != null)
            {
                this.track.stop();
            }
            curRoad.Clear();
            pntList.Clear();
        }

        public void Btn_Submit(string startPos, string stopPos)
        {
            pntList = new List<Point>();
            Point pnt = new Point(centerInfo.X, centerInfo.Y);
            pntList.Add(pnt);
            curRoad = new BusRoad(this.MarkLayer, this.GraphicsLayer);
            curRoad.AddNode(centerInfo.MarketName, centerInfo.X, centerInfo.Y, "../images/bus/stop.PNG");
            for (int i = 0; i < stopsData.Count; i++)
            {
                pnt = new Point(stopsData[i].X, stopsData[i].Y);
                pntList.Add(pnt);
                curRoad.AddNode(stopsData[i].StopName, stopsData[i].X, stopsData[i].Y, "../images/bus/stop.PNG");
            }

            pnt = new Point(marketInfo.X, marketInfo.Y);
            pntList.Add(pnt);
            IMSPolyline line = new IMSPolyline(ZDIMS.Interface.CoordinateType.Logic);
            line.Points = pntList;
            line.StrokeThickness = 2;
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.Draw();
            this.GraphicsLayer.AddGraphics(line);
            
            curRoad.AddNode(marketInfo.MarketName, marketInfo.X, marketInfo.Y, "../images/bus/stop.PNG");
            this.Show();
        }
    }
}
