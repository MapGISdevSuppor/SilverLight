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
using ZDIMS.Drawing;
using ZDIMS.Util;
using System.Text;
using System.Collections.ObjectModel;
using ZDIMS.Map;
using System.Windows.Threading;
using ZDIMS.Interface;
using ZDIMSDemo.Controls.Comm;
using System.Windows.Media.Imaging;

namespace ZDIMSDemo.Controls
{
    public partial class BusAnalyse : BaseUserControl
    {        
        List<BusRoad> _road = new List<BusRoad>();
        BusRoad curRoad;
        ComboBox _selectComboBox;
        public String ServerAddress { get; set; }
        public MarkLayer MarkLayer { get; set; }
        public GraphicsLayer GraphicsLayer { get; set; }
        DispatcherTimer m_timer;
        string m_key="";
        public BusAnalyse(MarkLayer markLayer, GraphicsLayer graphicsLayer)
        {
            ServerAddress = "http://127.0.0.1/relayhandlersite/relayhandler.ashx";
            MarkLayer = markLayer;
            markLayer.EnablePolymericMark = false;
            GraphicsLayer = graphicsLayer;
            InitializeComponent();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
            m_timer = new DispatcherTimer();
            m_timer.Interval = new TimeSpan(0, 0, 0, 0, 900);
            m_timer.Tick += new EventHandler(TextChanged_Tick);
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
            for (int i = 0; i < _road.Count; i++)
                _road[i].Clear();
            _road.Clear();
            busReslut.Blocks.Clear();
        }
        private void Btn_Submit(object sender, RoutedEventArgs e)
        {
            if (startPos.SelectedIndex > -1 && stopPos.SelectedIndex > -1)
            {
                Clear();
                button1.IsEnabled = false;
                GetBusChangeRlt((startPos.SelectedItem as ComboBoxItem).Content.ToString(), (stopPos.SelectedItem as ComboBoxItem).Content.ToString(), new UploadStringCompletedEventHandler(OnGetBusRlt));
            }
        }
        private void start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_timer.IsEnabled)
                m_timer.Stop();
            m_timer.Start();
            _selectComboBox = startPos;
            m_key = start.Text;
            //GetBusStopName(start.Text, new UploadStringCompletedEventHandler(SetStopList));
        }

        private void stop_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_timer.IsEnabled)
                m_timer.Stop();
            m_timer.Start();
            _selectComboBox = stopPos;
            m_key = stop.Text;
            //GetBusStopName(stop.Text, new UploadStringCompletedEventHandler(SetStopList));
        }
        void TextChanged_Tick(object sender, EventArgs e)
        {
            m_timer.Stop();
            if(m_key.Length>0)
                GetBusStopName(m_key, new UploadStringCompletedEventHandler(SetStopList));
        }
        void SetStopList(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null && _selectComboBox != null)
            {
                if (e.Result.IndexOf('$') > -1 || e.Result.Length<25)
                {
                    _selectComboBox.Items.Clear();
                    string[] arr = e.Result.Split('$');
                    for (int i = 0; i < arr.Length; i++)
                        _selectComboBox.Items.Add(new ComboBoxItem() { Content = arr[i] });
                    if (_selectComboBox.Items.Count > 0)
                        _selectComboBox.SelectedIndex = 0;
                }
            }
        }
        void OnGetBusRlt(object sender, UploadStringCompletedEventArgs e)
        {
            button1.IsEnabled = true;
            if (e.Error == null)
            {
                string response = e.Result;
                if (response == null || response == "")
                {
                    MessageBox.Show("没有查到线路!", "提示", MessageBoxButton.OK);
                    return;
                }
                else if (response.IndexOf('$') < 0)
                {
                    MessageBox.Show(response, "提示", MessageBoxButton.OK);
                    return;
                }
                Clear();
                int _idx = 0;
                string[] busWayAnly = response.Split('$');  //方案数组
                string[] oneBusWay;//一个方案中的所有公交路线
                for (int i = 0; i < busWayAnly.Length; i++)
                {
                    string stopInfo = "";
                    int wayRecom = 0;
                    string busLin = "";
                    //int busStopsCount			= 0;//一个换乘方案的车站站点计数
				    //List<Node> tmpSpecialNodeArray	= new List<Node>();//暂时存放特殊站点的Array，结构为Node
                    //List<Node> tmpNormalNodeArray = new List<Node>();//暂时存放普通站点的Array，结构为Node
                    int stopsSeq = -1;              //标识一个乘车方案的车站站点序列,0为起点序列
                    int isStartWalked = -1;          //标识是否在起点车站步行
                    double walkLength			= 0;   //行走距离
                    int hcTimes					= 0;      //换乘次数
                    String curtail				= "";     //简略的换乘方案
                    String lstStopName			= "";          //记录最后一个站点的名称
				    double lstStopX				= -1;
				    double lstStopY				= -1;
                    if (busWayAnly[i] != null)
                    {
                        oneBusWay = busWayAnly[i].Split('#');////一个方案中的所有公交路线
                        Paragraph paragraph = new Paragraph();
                        Label busRoadLabel = new Label() { FontWeight = FontWeights.Bold, FontSize = 14 };
                        paragraph.Inlines.Add(new InlineUIContainer() { Child = busRoadLabel });
                        busReslut.Blocks.Add(paragraph);
                        Label roadLenInfo = new Label();
                        paragraph = new Paragraph();
                        paragraph.Inlines.Add(new InlineUIContainer() { Child = roadLenInfo });
                        paragraph.Inlines.Add(new Run() { Text = "   " });
                        paragraph.Inlines.Add(GetRoadShowButton(_idx++));
                        busReslut.Blocks.Add(paragraph);
                        _road.Add(new BusRoad(MarkLayer, GraphicsLayer));
                        for (int j = 0; j < oneBusWay.Length; j++)
                        {
                            if (oneBusWay[j] == null || oneBusWay[j] == "" || oneBusWay[j].Split('|')[1] == null || oneBusWay[j].Split('|')[1] == "")
                                continue;
                            String oneBusName = oneBusWay[j].Split('|')[0]; //一条公交路线的名称
                            String oneStopInfo = oneBusWay[j].Split('|')[1].Split('@')[0];//一条公交路线的所有车站名称及坐标数组
                            if (oneStopInfo == null || oneStopInfo == "")
                                continue;
                            string[] tmpStopInfo = oneStopInfo.Split(',');//存放站点名称坐标数组
                            String oneBusLin = oneBusWay[j].Split('|')[1].Split('@')[1];//一条公交线路的坐标
                            String oneStartStop = tmpStopInfo[0];
                            double oneStartStopX = Convert.ToDouble(tmpStopInfo[1]);
                            double oneStartStopY = Convert.ToDouble(tmpStopInfo[2]);
                            String oneLastStop = tmpStopInfo[tmpStopInfo.Length - 3];//一条线路终止站点
                            double oneLastStopX = Convert.ToDouble(tmpStopInfo[tmpStopInfo.Length - 2]);
                            double oneLastStopY = Convert.ToDouble(tmpStopInfo[tmpStopInfo.Length - 1]);
                            stopInfo += oneStopInfo + "@";//一套方案中的所有车站信息
                            wayRecom = (oneStopInfo.Split(',').Length / 3) - 1;//一条线路经过的车站数
                            busLin += oneBusLin + ",";
                            string fx = CheckDirction(oneStartStopX, oneStartStopY, oneLastStopX, oneLastStopY);
                            Paragraph myParagraph = new Paragraph();
                            string txt = "";
                            if (oneBusName == null || oneBusName.Length == 0)
                            {
                                if (j == 0)
                                {   //起点车站需要步行
                                    isStartWalked = 1;
                                    stopsSeq++;
                                    //tmpSpecialNodeArray.Add(new Node(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), stopsSeq, 0)); //起点车站需要步行 只标识其为起点
                                    //IMSRoad(this._road[i]).addNode(tmpStopInfo[0],tmpStopInfo[1],tmpStopInfo[2],stopsSeq,"image/bus/walk.png"); 
                                    _road[_road.Count - 1].AddNode(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), "image/bus/walk.png");
                                    walkLength = Math.Round(GetLength(oneBusLin), 1);
                                    myParagraph.Inlines.Add(GetImage("/images/bus/walk.png"));
                                    myParagraph.Inlines.Add(GetLinkButton(oneStartStop, oneStartStopX, oneStartStopY));
                                    txt = "出发往|" + fx + "|方向行走约|" + walkLength + "|米";
                                    if (oneStartStop == oneLastStop)
                                        txt += "至附近同名站点";
                                    else
                                        txt += "至";
                                    GetText(myParagraph, txt);
                                    myParagraph.Inlines.Add(GetLinkButton(oneLastStop, oneLastStopX, oneLastStopY));
                                }
                                else if (j == oneBusWay.Length - 2)  //终点车站需要步行
                                {
                                    stopsSeq++;
                                    //tmpSpecialNodeArray.Add(new Node(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), stopsSeq, 3));
                                    //IMSRoad(this._road[i]).addNode(tmpStopInfo[0],tmpStopInfo[1],tmpStopInfo[2],stopsSeq,"image/bus/walk.png");		
                                    _road[_road.Count - 1].AddNode(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), "/images/bus/walk.png");
                                    walkLength = Math.Round(GetLength(oneBusLin), 1);
                                    myParagraph.Inlines.Add(GetImage("/images/bus/walk.png"));
                                    GetText(myParagraph, "往" + fx + "方向行走约|" + walkLength + "|米至");
                                    myParagraph.Inlines.Add(GetLinkButton(oneLastStop, oneLastStopX, oneLastStopY));
                                }
                                else
                                { //换乘车站间需要步行
                                    stopsSeq++;
                                    //tmpSpecialNodeArray.Add(new Node(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), stopsSeq, 3));
                                    //IMSRoad(this._road[i]).addNode(tmpStopInfo[0],tmpStopInfo[1],tmpStopInfo[2],stopsSeq,"image/bus/walk.png");    
                                    _road[_road.Count - 1].AddNode(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), "/images/bus/walk.png");
                                    walkLength = Math.Round(GetLength(oneBusLin), 1);
                                    myParagraph.Inlines.Add(GetImage("/images/bus/walk.png"));
                                    txt = "从";
                                    myParagraph.Inlines.Add(GetLinkButton(oneStartStop, oneStartStopX, oneStartStopY));
                                    if (oneStartStop == oneLastStop)
                                        txt += "出发往" + fx + "方向行走约|" + walkLength + "|米至附近同名站点";
                                    else
                                        txt += "出发往" + fx + "方向行走约|" + walkLength + "|米至";
                                    GetText(myParagraph, txt);
                                    myParagraph.Inlines.Add(GetLinkButton(oneLastStop, oneLastStopX, oneLastStopY));
                                }
                            }
                            else if (j == 0)
                            {
                                hcTimes++;
                                curtail += oneBusName.Split('(')[0] + "路";
                                myParagraph.Inlines.Add(GetImage("/images/bus/qidian.png"));
                                stopsSeq++;
                                //tmpSpecialNodeArray.Add(new Node(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), stopsSeq, 0)); //标识为起点
                                //IMSRoad(this._road[i]).addNode(tmpStopInfo[0],tmpStopInfo[1],tmpStopInfo[2],stopsSeq,"image/bus/qidian.png");
                                _road[_road.Count - 1].AddNode(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), "/images/bus/qidian.png");
                                myParagraph.Inlines.Add(GetLinkButton(oneStartStop, oneStartStopX, oneStartStopY));
                                txt = "乘 |" + oneBusName + "| 经 |" + wayRecom + "| 个站点";
                                for (int k = 3; k < tmpStopInfo.Length - 3; k += 3)
                                {
                                    stopsSeq++;
                                    //tmpNormalNodeArray.Add(new Node(tmpStopInfo[k], Convert.ToDouble(tmpStopInfo[k + 1]), Convert.ToDouble(tmpStopInfo[k + 2]), stopsSeq, 2));
                                    //IMSRoad(this._road[i]).addNode(tmpStopInfo[k],tmpStopInfo[k + 1],tmpStopInfo[k + 2],stopsSeq,"image/bus/stop.png");
                                    _road[_road.Count - 1].AddNode(tmpStopInfo[k], Convert.ToDouble(tmpStopInfo[k + 1]), Convert.ToDouble(tmpStopInfo[k + 2]), "/images/bus/stop.png");
                                }
                                GetText(myParagraph, txt + "在");
                                myParagraph.Inlines.Add(GetLinkButton(oneLastStop, oneLastStopX, oneLastStopY));
                                GetText(myParagraph, "下车");
                            }
                            else
                            {
                                hcTimes++;
                                if (isStartWalked == 1)
                                {
                                    isStartWalked = -1;
                                    stopsSeq++;
                                    //tmpSpecialNodeArray.Add(new Node(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), stopsSeq, 1)); //起点车站需要步行 只标识其为乘车点
                                    //IMSRoad(this._road[i]).addNode(tmpStopInfo[0],tmpStopInfo[1],tmpStopInfo[2],stopsSeq,"image/bus/bus.png");
                                    _road[_road.Count - 1].AddNode(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), "/images/bus/bus.png");
                                    myParagraph.Inlines.Add(GetImage("/images/bus/bus.png"));
                                    txt = " 乘 |" + oneBusName + "| 经 |" + wayRecom + "| 个站点";
                                    for (int k = 3; k < tmpStopInfo.Length - 3; k += 3)
                                    {
                                        stopsSeq++;
                                        //tmpNormalNodeArray.Add(new Node(tmpStopInfo[k], Convert.ToDouble(tmpStopInfo[k + 1]), Convert.ToDouble(tmpStopInfo[k + 2]), stopsSeq, 2));
                                        //IMSRoad(this._road[i]).addNode(tmpStopInfo[k],tmpStopInfo[k+1],tmpStopInfo[k+2],stopsSeq,"image/bus/stop.png");
                                        _road[_road.Count - 1].AddNode(tmpStopInfo[k], Convert.ToDouble(tmpStopInfo[k + 1]), Convert.ToDouble(tmpStopInfo[k + 2]), "/images/bus/stop.png");
                                    }
                                    GetText(myParagraph, txt + "在");
                                    myParagraph.Inlines.Add(GetLinkButton(oneLastStop, oneLastStopX, oneLastStopY));
                                    GetText(myParagraph, "下车");
                                    curtail += oneBusName.Split('(')[0] + "路";
                                }
                                else
                                {
                                    myParagraph.Inlines.Add(GetImage("/images/bus/shift.png"));                          //换乘
                                    stopsSeq++;
                                    //tmpSpecialNodeArray.Add(new Node(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), stopsSeq, 4)); //标识为换乘点
                                    //IMSRoad(this._road[i]).addNode(tmpStopInfo[0],tmpStopInfo[1],tmpStopInfo[2],stopsSeq,"image/bus/shift.png");
                                    _road[_road.Count - 1].AddNode(tmpStopInfo[0], Convert.ToDouble(tmpStopInfo[1]), Convert.ToDouble(tmpStopInfo[2]), "/images/bus/shift.png");
                                    txt = " 换乘 |" + oneBusName + "| 经 |" + wayRecom + "| 个站点";
                                    for (int k = 3; k < tmpStopInfo.Length - 3; k += 3)
                                    {
                                        stopsSeq++;
                                        //tmpNormalNodeArray.Add(new Node(tmpStopInfo[k], Convert.ToDouble(tmpStopInfo[k + 1]), Convert.ToDouble(tmpStopInfo[k + 2]), stopsSeq, 2));
                                        //IMSRoad(this._road[i]).addNode(tmpStopInfo[k],tmpStopInfo[k+1],tmpStopInfo[k+2],stopsSeq,"image/bus/stop.png");     
                                        _road[_road.Count - 1].AddNode(tmpStopInfo[k], Convert.ToDouble(tmpStopInfo[k + 1]), Convert.ToDouble(tmpStopInfo[k + 2]), "/images/bus/stop.png");
                                    }
                                    GetText(myParagraph, txt + "在");
                                    myParagraph.Inlines.Add(GetLinkButton(oneLastStop, oneLastStopX, oneLastStopY));
                                    GetText(myParagraph, "下车");
                                    curtail += " 转 " + oneBusName.Split('(')[0] + "路";
                                }
                            }
                            int tmpIndex = tmpStopInfo.Length - 1;
                            lstStopName = tmpStopInfo[tmpIndex - 2];
                            lstStopX = Convert.ToDouble(tmpStopInfo[tmpIndex - 1]);
                            lstStopY = Convert.ToDouble(tmpStopInfo[tmpIndex]);
                            busReslut.Blocks.Add(myParagraph);
                        }
                        stopsSeq++;
                        //tmpSpecialNodeArray.Add(new Node(lstStopName, lstStopX, lstStopY, stopsSeq, 5)); //标识终点车站
                        //IMSRoad(this._road[i]).addNode(lstStopName,lstStopX,lstStopY,stopsSeq,"image/bus/zhongdian.png");
                        _road[_road.Count - 1].AddNode(lstStopName, lstStopX, lstStopY, "/images/bus/zhongdian.png");
                        //IMSRoad(this._road[i])._roadCoorArr = busLin;
                        _road[_road.Count - 1].RoadCoorArr = busLin.Split(',');
                        String busInfo = hcTimes == 1 ? "[直达]" : ("[换乘" + (hcTimes - 1) + "次]");
                        busRoadLabel.Content = (i + 1).ToString() + ". " + curtail + busInfo;
                        roadLenInfo.Content = "全程约 " + Math.Round(GetLength(busLin), 1) + " 公里";
                    }                    
                }
                ShowRoad(0);
            }
        }
        void ShowRoad(int roadIndex)
        {
            if (curRoad != null)
            {
                curRoad.Clear();
                curRoad = null;
            }
            if (roadIndex > -1 && roadIndex < _road.Count)
            {
                _road[roadIndex].Draw();
                curRoad = _road[roadIndex];
            }
        }
        InlineUIContainer GetRoadShowButton(int roadIndex)
        {
            //Hyperlink hyperlink = new Hyperlink();
            InlineUIContainer container = new InlineUIContainer();
            Button hyperlink = new Button() { VerticalAlignment = VerticalAlignment.Bottom, Content = "显示", Cursor = Cursors.Hand, Padding = new Thickness(0), BorderThickness = new Thickness(0), Foreground = new SolidColorBrush(Colors.Blue), Background = new SolidColorBrush(Colors.White) };
            //hyperlink.Inlines.Add("显示");
            //hyperlink.FontWeight = FontWeights.Bold;
            hyperlink.Click += (s, e) =>
            {
                ShowRoad(roadIndex);
            };
            container.Child = hyperlink;
            return container;//hyperlink;
        }
        void GetText(Paragraph myParagraph,string txt)
        {
            //Paragraph myParagraph = new Paragraph();
            string[] arr = txt.Split('|');
            int sIndex = -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0 || i == arr.Length - 1)
                {
                    if (arr[i].Length > 0)
                        myParagraph.Inlines.Add(new Run() { Text = arr[i] });
                }
                else if (i == sIndex + 2)
                {
                    sIndex = i;
                    Bold bold = new Bold() { Foreground = new SolidColorBrush(Colors.Red) };
                    bold.Inlines.Add(arr[i]);
                    myParagraph.Inlines.Add(bold);
                }
                else if (arr[i].Length > 0)
                    myParagraph.Inlines.Add(new Run() { Text = arr[i] });
            }
            //return myParagraph;
        }
        InlineUIContainer GetLinkButton(string buttonLabel, double linkx, double linky)
        {
            //Paragraph myParagraph = new Paragraph();
            //Hyperlink hyperlink = new Hyperlink();
            InlineUIContainer container = new InlineUIContainer();
            Button hyperlink = new Button() { VerticalAlignment = VerticalAlignment.Bottom, Content = buttonLabel, Cursor = Cursors.Hand, Padding = new Thickness(0), BorderThickness = new Thickness(0), Foreground = new SolidColorBrush(Colors.Blue), Background = new SolidColorBrush(Colors.White) };
            //hyperlink.Inlines.Add(buttonLabel);
            hyperlink.Click += (s, e) =>
            {
                if (GraphicsLayer != null)
                    GraphicsLayer.MapContainer.PanTo(linkx, linky);
            };
            container.Child = hyperlink;
            //myParagraph.Inlines.Add(hyperlink);
            return container;// myParagraph;
        }
        InlineUIContainer GetImage(string src)
        {
            //Paragraph myParagraph = new Paragraph();
            InlineUIContainer container = new InlineUIContainer();
            BitmapImage bmp = new BitmapImage(new Uri(src, UriKind.Relative));
            Image img = new Image();
            img.Stretch = Stretch.None;
            img.Source = bmp;
            container.Child = img;
            //myParagraph.Inlines.Add(container);
            return container;//myParagraph;
        }
        double GetLength(String str)
        {
            if (str == null || str.Length <= 0)
                return 0;
            double length = 0;
            string[] coordinateXY = str.Split(',');
            double x1, y1, x2, y2;
            for (int k = 0; k < coordinateXY.Length; k += 2)
            {
                if (k + 3 < coordinateXY.Length)
                {
                    x1 = Convert.ToDouble(coordinateXY[k]);
                    y1 = Convert.ToDouble(coordinateXY[k + 1]);
                    x2 = Convert.ToDouble(coordinateXY[k + 2]);
                    y2 = Convert.ToDouble(coordinateXY[k + 3]);
                    length += Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                }
            }
            return length * 111000;
        }
        string CheckDirction(double sx,double sy,double ex,double ey)
        {
            string dir = "";
            if (ex > sx)
                dir += "东";
            if (ex < sx)
                dir += "西";
            if (ey > sy)
                dir += "北";
            if (ey < sy)
                dir += "南";
            return dir;    
        }
        /// <summary>
        /// 根据关键字获取公交站点
        /// </summary>
        /// <param name="key">根据关键字查询公交站点</param>
        /// <param name="callback">结果处理函数</param>
        /// <param name="spliter">站点分隔符</param>
        /// <param name="maxNum">最大结果数</param>
        void GetBusStopName(string key, UploadStringCompletedEventHandler callback, string spliter = "$", int maxNum = 8)
        {
            key = CommFun.Escape(key.Trim());
            string svrUrl = ServerAddress + "?svcType=SDS&_method=GetBusStop";
            string data = "_method=GetBusStop&key=" + key + "&spliter=" + spliter + "&encode=0&num=" + maxNum;
            WebClient cli = new WebClient();
            cli.Encoding = Encoding.UTF8;
            cli.Headers[HttpRequestHeader.ContentType] = "type=text/xml;charset=utf-8";
            cli.Headers[HttpRequestHeader.ContentEncoding] = "utf-8";
            cli.UploadStringCompleted += callback;
            cli.UploadStringAsync(new Uri(svrUrl), data);
        }
        /// <summary>
        /// 获取公交换乘方案
        /// </summary>
        /// <param name="startStop">起始站点</param>
        /// <param name="endStop">结束站点</param>
        /// <param name="callback">结果处理函数</param>
        /// <param name="returnXml">是否以XML形式返回结果</param>
        void GetBusChangeRlt(string startStop, string endStop, UploadStringCompletedEventHandler callback, bool returnXml = false)
        {
            startStop = CommFun.Escape(startStop.Trim());
            endStop = CommFun.Escape(endStop.Trim());
            String returnType = returnXml ? "0" : "1";
            string svrUrl = ServerAddress + "?svcType=SDS&_method=GetBusWay";
            string data = "_method=GetBusWay&staPos=" + startStop + "&endPos=" + endStop + "&encode=0&outputType=" + returnType;
            WebClient cli = new WebClient();
            cli.Encoding = Encoding.UTF8;
            cli.Headers[HttpRequestHeader.ContentType] = "type=text/xml;charset=utf-8";
            cli.Headers[HttpRequestHeader.ContentEncoding] = "utf-8";
            cli.UploadStringCompleted += callback;
            cli.UploadStringAsync(new Uri(svrUrl), data);
        }
        
    }
}
