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
using System.Windows.Media.Imaging;
using SilverlightDemo.Data;
using SilverlightDemo.components;
using ZDIMS.Interface;
using System.Windows.Browser;
using ZDIMS.Util;

namespace SilverlightDemo
{
    public partial class PolymericMarkDemo : Page
    {
        private List<PolyData> list = null;
        private PolyData polydata = null;
        private MarkInfo markInfo = null;
        private MarkLayer markLayer1 = null;
        string TileLayerAddress = "";
        public PolymericMarkDemo()
        {
            InitializeComponent();
            iMSMap1.MapReady += new ZDIMS.Event.IMSMapEventHandler(iMSMap1_MapReady);//地图准备就绪事件
        }

        void iMSMap1_MapReady(ZDIMS.Event.IMSMapEvent e)
        {
            initData();//统计数据初始化
           
        }


        /// <summary>
        /// 添加标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addMark_Click(object sender, RoutedEventArgs e)
        {
            //如果标注图层不为空，清除之前的标注
            if (markLayer1 != null)
            {
                markLayer1.RemoveAll();
            }
            //遍历数据文件，添加标注
            for (int i = 0; i < list.Count; i++)
            {
                addmyMark(list[i].PlaceName, list[i].X, list[i].Y);
            }

          
        }

        private void addmyMark(string name, double x, double y)
        {
            //初始化标注图层，并将其作为子元素添加到地图容器
            if (this.markLayer1 == null)
            {
                this.markLayer1 = new MarkLayer();
                this.iMSMap1.AddChild(this.markLayer1);
            }
            //创建并实例化标注控件，对其属性进行设置
            Image img = new Image();
            BitmapImage b = new BitmapImage(new Uri("/images/walk.png", UriKind.RelativeOrAbsolute));
            img.Source = b;
            img.Width = 25;
            img.Height = 30;
            IMSmyMark mark = new IMSmyMark(img);
            mark.CoordinateType = CoordinateType.Logic;
            mark.markName = name;
            mark.X = x;
            mark.Y = y;
            mark.EnableRevisedPos = true;
            mark.MarkClickCallback += new MarkClickDelegate(onClick);//标注点击事件回调
            mark.MarkControl.MouseLeave += new MouseEventHandler(img_MouseLeave);//标注鼠标移出事件监听
            markLayer1.AddMark(mark);//将标注添加到标注图层中
        }

        private void onClick(IMSmyMark mark)
        {
            if (markInfo == null)
            {
                //如果markInfo为空，则实例化markInfo控件，并设置其相关属性
                markInfo = new MarkInfo();
                markInfo.placename.Text = mark.markName;
                markInfo.xInfo.Text = mark.X.ToString();
                markInfo.yInfo.Text = mark.Y.ToString();
                markInfo.mapContainer = iMSMap1;
                if (markLayer1.EnablePolymericMark)
                {
                    markInfo.logicX = this.iMSMap1.MouseDownLogicPnt.X;
                    markInfo.logicY = this.iMSMap1.MouseDownLogicPnt.Y;
                }
                else
                {
                    markInfo.logicX = mark.X;
                    markInfo.logicY = mark.Y;
                }
                markInfo.updateposition();
                markInfo.Show();
            }
            else
            {//如果markInfo不为空，直接赋值
                markInfo.placename.Text = mark.markName;
                markInfo.xInfo.Text = mark.X.ToString();
                markInfo.yInfo.Text = mark.Y.ToString();
                markInfo.mapContainer = iMSMap1;
                if (markLayer1.EnablePolymericMark)
                {
                    markInfo.logicX = this.iMSMap1.MouseDownLogicPnt.X;
                    markInfo.logicY = this.iMSMap1.MouseDownLogicPnt.Y;
                }
                else
                {
                    markInfo.logicX = mark.X;
                    markInfo.logicY = mark.Y;
                }
                markInfo.updateposition();
                markInfo.Show();
            }
        }

        void img_MouseLeave(object sender, MouseEventArgs e)
        {//鼠标移出标注，关闭详细信息显示框
            if (this.markInfo != null)
                markInfo.Close();
        }

        /// <summary>
        /// 聚合标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void polymericMark_Click(object sender, RoutedEventArgs e)
        {
            markLayer1.EnablePolymericMark = true;
        }

        /// <summary>
        /// 解除聚合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calPoly_Click(object sender, RoutedEventArgs e)
        {
            markLayer1.EnablePolymericMark = false;
        }

        /// <summary>
        /// 删除标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delMark_Click(object sender, RoutedEventArgs e)
        {
            if (markLayer1 != null)
            {

                markLayer1.RemoveAll();
            }
        }

        /// <summary>
        /// 统计数据初始化
        /// </summary>
        private void initData()
        {
            list = new List<PolyData>();
            polydata = new PolyData();
            polydata.PlaceName = "温哥华";
            polydata.X = -122.9856932188265;
            polydata.Y = 49.31473826915955;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "维多利亚";
            polydata.X = -123.5725403242023;
            polydata.Y = 48.68274907875484;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "华盛顿";
            polydata.X = -76.94979490634647;
            polydata.Y = 38.91738753487638;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "墨西哥城";
            polydata.X = -99.14502956293464;
            polydata.Y = 19.46453454379436;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "危地马拉";
            polydata.X = -90.53756250004767;
            polydata.Y = 14.638845654204122;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "哈瓦那";
            polydata.X = -82.42665837247868;
            polydata.Y = 23.081544106735578;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "金斯敦";
            polydata.X = -76.80646878637968;
            polydata.Y = 18.02563058349792;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "太子港";
            polydata.X = -72.35997341103229;
            polydata.Y = 18.589906646359267;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "加拉加斯";
            polydata.X = -66.91245230016885;
            polydata.Y = 10.516244738939125;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "乔治敦";
            polydata.X = -58.20002988958967;
            polydata.Y = 6.792022724054252;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "基多";
            polydata.X = -78.55911023762702;
            polydata.Y = -0.18242941291198278;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "卡亚俄";
            polydata.X = -76.86063928841438;
            polydata.Y = -11.990470304348502;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "拉巴斯";
            polydata.X = -68.17078792034965;
            polydata.Y = -16.482107764724816;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "亚松森";
            polydata.X = -57.68766722451156;
            polydata.Y = -25.169702028538108;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "圣地亚哥";
            polydata.X = -70.68068784795692;
            polydata.Y = -33.460045944097;
            list.Add(polydata);

            polydata = new PolyData();
            polydata.PlaceName = "布宜诺斯艾利斯";
            polydata.X = -58.424611762608485;
            polydata.Y = -34.6788822398775;
            list.Add(polydata);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
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

        }
    }
}
