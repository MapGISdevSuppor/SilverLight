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
using ZDIMS.Map;
using ZDIMS.Drawing;
using System.Threading;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using ZDIMS.Interface;
using System.Diagnostics;
using System.Windows.Threading;

namespace ZDIMSDemo.Controls
{
    public partial class GPSSample : BaseUserControl
    {
        private IMSMap m_mapContainer = null;//地图容器对象
        private string m_gpsSvcUrl = "http://localhost/GPSService/gpsHandler.ashx";
        private DispatcherTimer m_timer = null;
        private IMSMark m_posMarker = null;
        private MarkLayer m_markLayer=null;
        private double PLon = 0;
        private double PLat = 0;
        private double lastPlon = 0;
        private double lastPLat = 0;
        private string device = "";
        private SynchronizationContext context=null;
        private IMSPolyline pathLine;
        private IMSPolyline historyPath;
        private GraphicsLayer pathLayer;
        /// <summary>
        /// GPS服务地址
        /// </summary>
        public string GpsSvcUrl
        {
            get { return m_gpsSvcUrl; }
            set { m_gpsSvcUrl = value; }
        }
        /// <summary>
        /// 地图容器
        /// </summary>
        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set { m_mapContainer = value; }
        }
        public GPSSample()
        {
            InitializeComponent();
            this.dialog.OnClose += new RoutedEventHandler(OnClose);
        }

        private void OnClose(object sender,RoutedEventArgs e)
        {
            //停止GPS跟踪再关闭
            this.Close();
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null && this.Parent != null && this.Parent is IMSMap)
                this.MapContainer = this.Parent as IMSMap;
            this.comboBox_device.SelectedIndex = 0;
            this.datePicker1.SelectedDate = DateTime.Now;
            this.datePicker2.SelectedDate = DateTime.Now;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (this.checkBox1 == null)
                return;
            if (this.checkBox1.IsChecked.Value)
            {
                context = SynchronizationContext.Current;
                if (this.m_timer == null)
                {
                    this.m_timer = new DispatcherTimer();
                    this.m_timer.Tick+=new EventHandler(OnTimer);
                }
                this.m_timer.Interval = new TimeSpan(0,0,(int)this.slider1.Value);
                this.m_timer.Start();
            }
            else
            {
                if(this.m_timer!=null&&this.m_timer.IsEnabled)
                    this.m_timer.Stop();
            }
        }
        private void OnTimer(object obj,EventArgs e)
        {
            if(this.MapContainer==null)
                return;
            this.m_timer.Stop();
            string url=string.Format("{0}?method=getLastPos&gpsType=GPRMC&PDevice={1}&rnd={2}",this.m_gpsSvcUrl,this.device,Guid.NewGuid().ToString());
			WebClient cli = new WebClient();
            cli.AllowReadStreamBuffering = true;
            cli.OpenReadCompleted+= new OpenReadCompletedEventHandler(OnGetPosInfo);
            try
            {
                cli.OpenReadAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ShowMarker(object obj)
        {
            //显示标记
            if (this.m_markLayer == null)
            {
                this.m_markLayer = new MarkLayer();
                this.MapContainer.AddChild(this.m_markLayer);
            }
            if (this.m_posMarker == null)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("images/mark/marker2/image2/p1.png",UriKind.Relative));
                img.Width = 40;
                img.Height = 40;
                this.m_posMarker = new IMSMark(img);
                this.m_posMarker.CoordinateType = CoordinateType.Logic;
                this.m_posMarker.X = this.PLon;
                this.m_posMarker.Y = this.PLat;
                this.m_markLayer.AddMark(this.m_posMarker);
            }
            else
            {
                this.m_posMarker.X = this.PLon;
                this.m_posMarker.Y = this.PLat;
            }
            //居中显示标记
            if (this.checkBox2.IsChecked.Value)
            {
                Point pnt = this.MapContainer.LogicToScreen(this.PLon,this.PLat);
                if (Math.Abs(pnt.X - this.MapContainer.CenPntScrCoor.X) > 10 || Math.Abs(pnt.Y - this.MapContainer.CenPntScrCoor.Y)>10)
                {
                    this.MapContainer.PanTo(PLon, PLat);
                    return;
                }
            }
            if (!m_posMarker.IsInMapViewBound)
            {
                this.MapContainer.PanTo(PLon, PLat);
            }
        }
        private void OnGetPosInfo(Object sender,OpenReadCompletedEventArgs e)
        {
            if(e.Error!=null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }
            StreamReader sr=new StreamReader(e.Result);
            string gpsinfo=sr.ReadToEnd();
			if(gpsinfo==null||gpsinfo=="")
				return;
			string[] infoArray = gpsinfo.Split('*');
			if(infoArray.Length < 2)
			{
				return;
			}
			string[] colName = infoArray[0].Split(',');
			string[] row = infoArray[1].Split(',');
            for(int i = 0;i < colName.Length;i++)
            {
                if (colName[i] == "PLon")
                {
                    this.lastPlon = PLon;
                    this.PLon = Convert.ToDouble(row[i]) / 100;
                }
                if (colName[i] == "PLat")
                {
                    this.lastPLat = PLat;
                    this.PLat = Convert.ToDouble(row[i]) / 100;
                }
            }
            context.Post(ShowMarker,null);	
            //绘制轨迹
            if(pathLayer==null)
            {
                pathLayer = new GraphicsLayer();
                this.MapContainer.AddChild(pathLayer);
            }
            if (pathLine == null)
            {
                pathLine = new IMSPolyline(CoordinateType.Logic);
                this.pathLayer.AddGraphics(pathLine);
            }
            pathLine.Points.Add(new Point(this.PLon, this.PLat));
            pathLine.Visibility = this.checkBox3.IsChecked.Value?Visibility.Visible:Visibility.Collapsed;
            this.m_timer.Start();
        }

        private void comboBox_device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.device = (this.comboBox_device.SelectedItem as ComboBoxItem).Content.ToString();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            checkBox1_Checked(null,null);
            if (this.slider1 != null && this.interval!=null)
                this.interval.Content = ((int)this.slider1.Value).ToString() + "秒";
        }

        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {
            if (pathLine != null)
                pathLine.Visibility = this.checkBox3.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        private void button_showHistoryPath_Click(object sender, RoutedEventArgs e)
        {
            if (pathLayer == null)
            {
                pathLayer = new GraphicsLayer();
                this.MapContainer.AddChild(pathLayer);
            }
            if (historyPath == null)
            {
                historyPath = new IMSPolyline(CoordinateType.Logic);
                this.pathLayer.AddGraphics(historyPath);
            }
            string startTime = string.Format("{0} {1}", this.datePicker1.SelectedDate.Value.ToShortDateString(), this.timePicker1.Value.Value.ToLongTimeString());
            string endTime = string.Format("{0} {1}", this.datePicker2.SelectedDate.Value.ToShortDateString(), this.timePicker2.Value.Value.ToLongTimeString());
            string url = string.Format("{0}?method=getHistoryPath&gpsType=GPRMC&beginTime={1}&endTime={2}&PDevice={3}&rnd={4}", this.m_gpsSvcUrl, startTime,endTime,this.device, Guid.NewGuid().ToString());
			WebClient cli = new WebClient();
            cli.AllowReadStreamBuffering = true;
            cli.OpenReadCompleted+= new OpenReadCompletedEventHandler(OnGetHistoryPath);
            try
            {
                cli.OpenReadAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            historyPath.Visibility = Visibility.Visible;
        }
        private void OnGetHistoryPath(object obj, OpenReadCompletedEventArgs e)
        {
            this.historyPath.Clear();
            StreamReader sr=new StreamReader(e.Result);
			String gpsinfo = sr.ReadToEnd();
			if(gpsinfo==null||gpsinfo=="")
			{
				return;
			}
			string[] infoArray = gpsinfo.Split('*');
			if(infoArray.Length < 2)
			{
				MessageBox.Show("未查到历史路径");
				return;
			}
			string[] colName = infoArray[0].Split(',');
			string[] rows = infoArray[1].Split(';');
			int rowCount = rows.Length;
			for(int r = 0;r < rowCount;r++)
			{
				string[] row = rows[r].Split(',');
                Point pnt = new Point();
				for(int i = 0;i < colName.Length;i++)
				{
					if(colName[i] == "PLon")
					{
						pnt.X = Convert.ToDouble(row[i])/100;
					}
					if(colName[i] == "PLat")
					{
                        pnt.Y = Convert.ToDouble(row[i]) / 100;
					}
				}
                this.historyPath.AddPoint(pnt);
			}
            this.historyPath.Draw();
        }

        private void button_clearHistoryPath_Click(object sender, RoutedEventArgs e)
        {
            if (historyPath != null)
                historyPath.Clear();
        }

        
    }
}
