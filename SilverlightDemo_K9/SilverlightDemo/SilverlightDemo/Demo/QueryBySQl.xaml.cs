using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ZDIMS.Interface;
using ZDIMS.BaseLib;
using ZDIMSDemo.Controls;
using ZDIMS.Util;
using ZdimsExtends.QueryControl;
using ZDIMSDemo.Controls.Layer;
using ZDIMS.Event;
using ZDIMS.Drawing;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Browser;
namespace SilverlightDemo
{
    public partial class QueryBySQl : Page
    {
        string VecLayerAddress = "";
        string TileLayerAddress = "";
        private LayerDataViewer layer_dataView = null;
        public QueryBySQl()
        {
            InitializeComponent();
            //添加地图容器地图准备完毕事件
            this.iMSMap1.MapReady += new IMSMapEventHandler(iMSMap1_MapReady);
        }
        /// <summary>
        /// 地图加载就绪事件
        /// </summary>
        /// <param name="e"></param>
        void iMSMap1_MapReady(IMSMapEvent e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                //初始化查询结果显示控件
                layer_dataView = new LayerDataViewer()
                {
                    activeLayer = this.vectorLayer,
                    GraphicsLayer = this.graphicsLayer
                };
                //设置图层状态
                CSetEnumLayerStatus setLayerStatus;
                //设置状态更新参数
                setLayerStatus = new CSetEnumLayerStatus();
                setLayerStatus.GdbIndex = 0;//图层所在gdb数据库索引号
                //设置索引号为0的图层状态：可查询
                setLayerStatus.LayerStatus = new EnumLayerStatus[1];
                setLayerStatus.LayerStatus[0] = EnumLayerStatus.Editable;
                //调用矢量图层的设置图层状态方法，设置图层状态
                this.vectorLayer.SetEnumLayerStatus(setLayerStatus, onSetSingleLayerStatus);
            }
        }
        /// <summary>
        /// 更新图层返回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void onSetSingleLayerStatus(object sender, UploadStringCompletedEventArgs e)
        {
            this.vectorLayer.ActiveGdbIndex = 0;
            this.vectorLayer.ActiveLayerIndex = 0;
            this.iMSMap1.Refresh();
        }
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void select_Click(object sender, RoutedEventArgs e)
        {
            if (this.sql.Text != "")
            {
                CSetEnumLayerStatus setLayerStatus;
                setLayerStatus = new CSetEnumLayerStatus();
                setLayerStatus.GdbIndex = 0;
                setLayerStatus.LayerStatus = new EnumLayerStatus[1];
                setLayerStatus.LayerStatus[0] = EnumLayerStatus.Editable;
                this.vectorLayer.SetEnumLayerStatus(setLayerStatus, onSetSingleLayerStatus);
                //设置web查询参数
                CLayerSelectParam mapsel = new CLayerSelectParam();
                CWebSelectParam websel = new CWebSelectParam();
                websel.CompareRectOnly = this.vectorLayer.CompareRectOnly;
                websel.Geometry = null;
                websel.MustInside = this.vectorLayer.MustInside;//查询要素是否在图形里面
                websel.NearDistance = this.vectorLayer.NearDistanse;//查询半径
                websel.SelectionType = SelectionType.Condition;//查询类型：属性
                websel.WhereClause = this.sql.Text;//查询sql语句
                mapsel.WebSelectParam = websel;//web查询参数对象
                mapsel.PageCount = 0;//查询起始页数
                this.layer_dataView._lastSelectParam = mapsel;
                //调用矢量图层的查询方法查询
                this.vectorLayer.LayerSelectAndGetAtt(mapsel, this.layer_dataView.SelectCallback);
                if (this.HightLight.IsChecked == true)
                {//高亮显示：传入地图容器对象、查询参数、绘图层对象、矢量图层对象
                    drawHighLight.AddhighLightFeatures(this.iMSMap1, mapsel, this.graphicsLayer1, this.vectorLayer);
                }
            }
            else
            {
                MessageBox.Show("查询参数为空！");
                return;
            }
        }
        /// <summary>
        /// 清除高亮显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.graphicsLayer1.RemoveAll();
            this.iMSMap1.Refresh();
        }

      
        /// <summary>
        /// 控件加载完毕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //设置矢量、瓦片数据地址
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
            iMSMap1.OperType = IMSOperType.Drag;
            tileLayer1.ServerAddress = TileLayerAddress;
            this.vectorLayer.ServerAddress = VecLayerAddress;
        }

        private void ClearRemove(object sender, RoutedEventArgs e)
        {
            if (graphicsLayer != null)
            {
                this.graphicsLayer.RemoveAll();
            }
            if (layer_dataView != null)
            {
                this.layer_dataView.Visibility = Visibility.Collapsed;
            }
        }

      
    }
}
