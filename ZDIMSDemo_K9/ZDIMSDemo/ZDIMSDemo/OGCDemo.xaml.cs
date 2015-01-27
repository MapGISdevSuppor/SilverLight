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
using ZDIMS.Event;
using System.Xml;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ZDIMSDemo
{
    public partial class OGCDemo : UserControl
    {
        List<CheckBox> chkBoxList = new List<CheckBox>();
        DispatcherTimer updaTimer = new DispatcherTimer();
        public OGCDemo()
        {
            InitializeComponent();
            this.collapse.MouseLeftButtonUp += new MouseButtonEventHandler(collapse_MouseLeftButtonUp);
            iMSMap1.Loaded += new RoutedEventHandler(iMSMap1_Loaded);
            updaTimer.Interval = new TimeSpan(0, 0, 0, 1);
            updaTimer.Tick += new EventHandler(UpdateTimer_Tick);
        }
        void iMSMap1_Loaded(object sender, RoutedEventArgs e)
        {
            iMSMap1.Loaded -= new RoutedEventHandler(iMSMap1_Loaded);
            oGCWSLayer1.GetWMSCapabilities(new OpenReadCompletedEventHandler(OnGetWMSCapabilities));
        }
        void OnGetWMSCapabilities(object sender, OpenReadCompletedEventArgs e)
        {            
            string xmlStr = oGCWSLayer1.OnGetWMSCapabilities(sender, e);
            if (xmlStr != null && xmlStr.Length > 0)
            {
                XmlReader xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)));
                if (xmlReader.ReadToFollowing("Layer"))
                {
                    LayerInfo info;
                    StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
                    CheckBox chkBox;// = new CheckBox() { IsChecked = false };
                    //panel.Children.Add(chkBox);
                    panel.Children.Add(new Image() { Source = new BitmapImage(new Uri("images/layer.PNG", UriKind.Relative)) });
                    panel.Children.Add(new TextBlock() { Text = "OGC地图图", Margin = new Thickness(2) });
                    TreeViewItem pItem = new TreeViewItem() { Header = panel };
                    treeView1.Items.Add(pItem);
                    while (xmlReader.ReadToFollowing("Layer"))
                    {
                        panel = new StackPanel() { Orientation = Orientation.Horizontal };
                        chkBox = new CheckBox() { IsChecked = false };
                        panel.Children.Add(chkBox);
                        chkBox.Click += new RoutedEventHandler(chkBox_Click);
                        chkBoxList.Add(chkBox);
                        TreeViewItem item = new TreeViewItem() { Header = panel };
                        pItem.Items.Add(item);
                        info = new LayerInfo();
                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name.ToLower() == "layer")
                                break;
                            if (xmlReader.NodeType == XmlNodeType.Element && (!xmlReader.IsEmptyElement || xmlReader.AttributeCount > 0))
                            {
                                switch (xmlReader.Name.ToLower())
                                {
                                    case "name":
                                        xmlReader.Read();
                                        if (xmlReader.NodeType == XmlNodeType.Text)
                                        {
                                            panel.Children.Add(new TextBlock() { Text = xmlReader.Value, Margin = new Thickness(2) });
                                            info.Name = xmlReader.Value;
                                        }
                                        break;
                                    case "geomtype":
                                        xmlReader.Read();
                                        if (xmlReader.NodeType == XmlNodeType.Text)
                                        {
                                            switch (xmlReader.Value.ToLower())
                                            {
                                                case "pnt":
                                                    panel.Children.Insert(1,new Image() { Source = new BitmapImage(new Uri("images/点.PNG", UriKind.Relative)) });
                                                    break;
                                                case "lin":
                                                    panel.Children.Insert(1,new Image() { Source = new BitmapImage(new Uri("images/线.PNG", UriKind.Relative)) });
                                                    break;
                                                case "reg":
                                                    panel.Children.Insert(1,new Image() { Source = new BitmapImage(new Uri("images/区.PNG", UriKind.Relative)) });
                                                    break;
                                            }
                                        }
                                        break;
                                    case "latlonboundingbox":
                                        info.XMin = Convert.ToDouble(xmlReader.GetAttribute("minx"));
                                        info.YMin = Convert.ToDouble(xmlReader.GetAttribute("miny"));
                                        info.XMax = Convert.ToDouble(xmlReader.GetAttribute("maxx"));
                                        info.YMax = Convert.ToDouble(xmlReader.GetAttribute("maxy"));
                                        break;
                                }
                            }
                        }
                        chkBox.Tag = info;
                    }
                }                
            }
        }
        
        void chkBox_Click(object sender, RoutedEventArgs e)
        {
            if (updaTimer.IsEnabled)
                updaTimer.Stop();
            updaTimer.Start();
        }
        void UpdateTimer_Tick(object sender, EventArgs e)
        {
            updaTimer.Stop();
            double xmin = double.MaxValue;
            double ymin = double.MaxValue;
            double xmax = double.MinValue;
            double ymax = double.MinValue;
            LayerInfo info;
            StringBuilder layerList = new StringBuilder();
            for (int i = 0; i < chkBoxList.Count; i++)
            {
                if (chkBoxList[i].IsChecked == true)
                {
                    info = (LayerInfo)chkBoxList[i].Tag;
                    layerList.Append(info.Name + ",");
                    if (info.XMin < xmin)
                        xmin = info.XMin;
                    if (info.YMin < ymin)
                        ymin= info.YMin ;
                    if (info.XMax > xmax)
                        xmax = info.XMax;
                    if (info.YMax > ymax)
                        ymax = info.YMax;
                }
            }
            if (chkBoxList.Count > 0)
            {
                oGCWSLayer1.LayerStrList = layerList.ToString();
                oGCWSLayer1.LayerStrList = oGCWSLayer1.LayerStrList.Substring(0, oGCWSLayer1.LayerStrList.Length - 1);
                if (xmin != double.MaxValue && ymin != double.MaxValue && xmax != double.MinValue && ymax != double.MinValue)
                {
                    iMSMap1.XMinMap = xmin;
                    iMSMap1.YMinMap = ymin;
                    iMSMap1.XMaxMap = xmax;
                    iMSMap1.YMaxMap = ymax;
                    iMSMap1.Restore(true);
                }
            }
        }
        void collapse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe.Opacity == 1)
            {
                this.hideMenu.Begin();
            }
            else
            {
                this.showMenu.Begin();
            }

        }
        public struct LayerInfo
        {
            public string Name;
            public double XMin;
            public double XMax;
            public double YMin;
            public double YMax;
        }
    }
   
}
