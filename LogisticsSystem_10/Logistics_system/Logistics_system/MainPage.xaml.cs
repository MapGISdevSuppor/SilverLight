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
using ZDIMS.BaseLib;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZDIMSDemo.Controls.MapDoc;
using System.Windows.Browser;
using Logistics_system.controls;
using ZDIMS.Map;
using ZDIMS.BaseLib;
namespace Logistics_system
{
    public partial class MainPage : UserControl
    {
        string TileLayerAddress = "";
        string VecLayerAddress = "";
        private MapDocEditor m_docEditor = null;
        //private GraphicsLayer graphicsLayer1;
        public TileLayer tileLayer;
        BusRoad curRoad;
        List<Point> pntList;
        List<stops> stopsData = new List<stops>();
        public MainPage()
        {
            string address = "";
            try
            {
                address = "http://" + HtmlPage.Window.Eval("Address");
            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
            InitializeComponent();
            tileLayer = new TileLayer();
            tileLayer.HdfName = "clipggwh7";
            tileLayer.DataVersion = 0;
            tileLayer.BigImgSize = 2048;
            tileLayer.ServerAddress = address + ":6163/igs/rest/ims/relayhandler";
            this.iMSMap1.AddChild(tileLayer);
          
        }
        private void onLoadMap(object sender, RoutedEventArgs e)
        {
           
          
            try
            {
                TileLayerAddress = HtmlPage.Window.Eval("TileLayerAddress").ToString();
                VecLayerAddress = HtmlPage.Window.Eval("VecLayerAddress").ToString();

            }
            catch
            {
                MessageBox.Show("地图容器中未获取地图配置信息！");
                return;
            }
           
            this.tileLayer.ServerAddress = TileLayerAddress;


            this.iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);

        }
        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            
         
            this.bar.MapContainer = this.iMSMap1;
            //if (graphicsLayer1 == null)
            //{
            //    graphicsLayer1 = new GraphicsLayer();
            //    this.iMSMap1.AddChild(graphicsLayer1);
            //}
            this.toolBar1.mapContainer = this.iMSMap1;
            this.toolBar1.markLayer = this.markLayer1;
            this.toolBar1.graphicsLayer = this.graphicsLayer1;
            //setstopsData();
            //this.toolBar1.vecObj = this.vectorMapDoc1;
            this.iMSMap1.SetCenter(12730635.074890137, 3577921.7222900391);
        }

        public void setstopsData()
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

            stopsData = new List<stops>();
            pntList = new List<Point>();
            stopsData = list;
            Point pnt;
            curRoad = new BusRoad(this.markLayer1, this.graphicsLayer1);
            for (int i = 0; i < stopsData.Count; i++)
            {
                pnt = new Point(stopsData[i].X, stopsData[i].Y);
                pntList.Add(pnt);
                curRoad.AddNode(stopsData[i].StopName, stopsData[i].X, stopsData[i].Y, "../images/bus/stop.PNG");
            }

            IMSPolyline line = new IMSPolyline(CoordinateType.Logic);
            line.Points = pntList;
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 2;
            line.Draw();
            line.Flicker();
            this.graphicsLayer1.AddGraphics(line);
            this.iMSMap1.SetCenter(pntList[0].X, pntList[0].Y);
            //return list;
        }
        public void ue(object sender, UploadStringCompletedEventArgs e)
        {
           // this.vectorMapDoc1.Refresh();
        }
        void g(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {


            this.graphicsLayer1.DrawingType = DrawingType.Circle;
            
        }

       
    }
    public interface IDemoItemOwner
    {
        
        Style ScrollViewerStyle { get; }
    }
}
