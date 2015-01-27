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
using System.Windows.Threading;
using ZDIMS.BaseLib;
using ZDIMS.Util;
using ZDIMS.Drawing;
using ZDIMS.Interface;
using ZDIMS.Event;
using ZDIMS.Map;
using ZDIMSDemo.Controls;
using ZDIMSDemo.Controls.Layer;
using System.Diagnostics;
namespace ZDIMSDemo.Controls
{
    public partial class ClipAnalyse : BaseUserControl
    {
        private SpacialAnalyse _spatial;
        private GraphicsLayer m_graphicsLayer = null;
        private string _serverAddr = null;
        private Dot_2D[] dot = null;
        private CLayerAccessInfo _clipLayerAcsInfo = null;
        private GRegion _clipRegion = null;
        private IMSCatalog IMSCatalog1 = null;
        private LayerDataViewer layerDataViewer1;
        private string _UID = "";
        private int timerCnt = 0;
        /// <summary>
        /// 绘图层
        /// </summary>
        public GraphicsLayer GraphicsLayerObj
        {
            get { return m_graphicsLayer; }
            set
            {
                m_graphicsLayer = value;
            }
        }
        /**
       * 时间控制属性
       */
        private DispatcherTimer clipTimer;

        public IMSCatalog IMSCatalog
        {
            get { return IMSCatalog1; }
            set { IMSCatalog1 = value; }
        }
        public ClipAnalyse()
        {
            InitializeComponent();
            this.myDialog1.OnClose += close;
        }
        /// <summary>
        /// 多边形查询
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        public void LayerPloySelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 2)
            {
                ZDIMS.BaseLib.Polygon ploy = new ZDIMS.BaseLib.Polygon();
                ploy.Dots = new Dot_2D[logPntArr.Count + 1];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    ploy.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                if (dot == null)
                {
                    dot = new Dot_2D[ploy.Dots.Length];
                }
                ploy.Dots[ploy.Dots.Length - 1] = ploy.Dots[0];
                dot = ploy.Dots;
            }
            this.clipLayer();
        }

        public void LayerCircleSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                Circle obj = new Circle();
                obj.Center = new Dot_2D()
                {
                    x = logPntArr[0].X,
                    y = logPntArr[0].Y
                };
                obj.Radius = Math.Sqrt(Math.Pow(logPntArr[0].X - logPntArr[1].X, 2) + Math.Pow(logPntArr[0].Y - logPntArr[1].Y, 2));
                dot = obj.GetDots();
                this.clipLayer();
            }
        }
        /// <summary>
        /// 多边形查询
        /// </summary>
        /// <param name="gLayer"></param>
        /// <param name="graphics"></param>
        /// <param name="logPntArr"></param>
        public void PloySelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 2)
            {
                ZDIMS.BaseLib.Polygon ploy = new ZDIMS.BaseLib.Polygon();
                ploy.Dots = new Dot_2D[logPntArr.Count + 1];
                for (int i = 0; i < logPntArr.Count; i++)
                {
                    ploy.Dots[i] = new Dot_2D()
                    {
                        x = logPntArr[i].X,
                        y = logPntArr[i].Y
                    };
                }
                if (dot == null)
                {
                    dot = new Dot_2D[ploy.Dots.Length];
                }
                ploy.Dots[ploy.Dots.Length - 1] = ploy.Dots[0];
                dot = ploy.Dots;
            }
            this.clipDoc();
        }

        public void CircleSelect(GraphicsLayer gLayer, IGraphics graphics, List<Point> logPntArr)
        {
            if (logPntArr.Count > 1)
            {
                Circle obj = new Circle();
                obj.Center = new Dot_2D()
                {
                    x = logPntArr[0].X,
                    y = logPntArr[0].Y
                };
                obj.Radius = Math.Sqrt(Math.Pow(logPntArr[0].X - logPntArr[1].X, 2) + Math.Pow(logPntArr[0].Y - logPntArr[1].Y, 2));
                dot = obj.GetDots();
                this.clipDoc();
            }
        }
        public void setClipRegion(GRegion region)
        {
            this._clipRegion = region;
        }
        /// <summary>
        /// 圆裁剪地图文档
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public void clipDoc()
        {
            if (this.IMSCatalog.ActiveMapDoc.ActiveLayerIndex == -1)
            {
                MessageBox.Show("矢量文档中没有激活的图层!");
                return;
            }
            GRegion region = new GRegion();
            region.Rings = new AnyLine[1];
            region.Rings[0] = new AnyLine();
            region.Rings[0].Arcs = new Arc[1];
            region.Rings[0].Arcs[0] = new Arc();
            region.Rings[0].Arcs[0].Dots = new Dot_2D[dot.Length];
            region.Rings[0].Arcs[0].Dots = dot;
            this.setClipRegion(region);
            this.Show();
            this._clipLayerAcsInfo = this.IMSCatalog.ActiveMapDoc.ActiveLayerAccessInfo;
            this._serverAddr = this.IMSCatalog.ActiveMapDoc.ServerAddress;
            this._spatial = new SpacialAnalyse(this.IMSCatalog.ActiveMapDoc);
        }
        /**
          * 多边形裁剪处理事件
          * @param e
          *
          */
        public void clipLayer()
        {
            if (this.IMSCatalog.ActiveLayerObj == null || this.IMSCatalog.ActiveLayerObj.ActiveLayerAccessInfo==null) return;

            GRegion region = new GRegion();
            region.Rings = new AnyLine[1];
            region.Rings[0] = new AnyLine();
            region.Rings[0].Arcs = new Arc[1];
            region.Rings[0].Arcs[0] = new Arc();
            region.Rings[0].Arcs[0].Dots = dot;
            this.setClipRegion(region);
            this.Show();
            CLayerAccessInfo layerAccessInfo = new CLayerAccessInfo();
            layerAccessInfo.GdbInfo = this.IMSCatalog.ActiveLayerObj.ActiveLayerAccessInfo.GdbInfo;
            layerAccessInfo.LayerInfoList = new CLayerInfo[1];
            int idx = this.IMSCatalog.ActiveLayerObj.ActiveLayerIndex;
            layerAccessInfo.LayerInfoList[0] = this.IMSCatalog.ActiveLayerObj.ActiveLayerAccessInfo.LayerInfoList[idx];
            this._clipLayerAcsInfo = layerAccessInfo;
            this._serverAddr = this.IMSCatalog.ActiveLayerObj.ServerAddress;
            this._spatial = new SpacialAnalyse(this.IMSCatalog.ActiveLayerObj);
        }
        private void submit(object sender, RoutedEventArgs e)
        {
            if (this._clipLayerAcsInfo == null)
            {
                MessageBox.Show("请激活需要裁剪的图层后重新运行。");
                return;
            }
            if (this.clipRadius.Text == "")
            {
                MessageBox.Show("请输入裁剪分析半径。");
                return;
            }
            if (this._clipRegion == null)
            {
                MessageBox.Show("还未设置裁剪范围。");
                return;
            }
            CClipByPolygon clip = new CClipByPolygon();
            clip.Region = this._clipRegion;
            CLayerAccessInfo clipLayer = this._clipLayerAcsInfo;
            clip.GdbInfo = clipLayer.GdbInfo;
            clip.LayerName = clipLayer.LayerInfoList[0].LayerDataName;
            clip.XClsType = clipLayer.LayerInfoList[0].LayerType;
            clip.FClsNameRtn = "clip" + clip.LayerName;
            clip.Radius = Convert.ToDouble(this.clipRadius.Text);
            clip.Username = "";
            clip.UserIP = "";
            bool check = false;

            if (check = Convert.ToBoolean(this.radio_in.IsChecked) && this.radio_in.Content.ToString() == "内裁")
            {
                clip.ClipFlg = 3;
            }
            else
            {
                clip.ClipFlg = 4;
            }
            if (check = Convert.ToBoolean(this.radio_no.IsChecked) && this.radio_no.Content.ToString() == "否")
            {
                clip.IsReCalculate = false;
            }
            else
            {
                clip.IsReCalculate = true;
            }
            this._spatial.ClipByPolygon(clip, new UploadStringCompletedEventHandler(onSubmit));
            this.button_submit.IsEnabled = false;
        }
        private void onSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            string uid = this._spatial.OnClipByPolygon(e);//获取区裁剪结果
            if (uid != null && uid != "")
            {
                clipTimer = new DispatcherTimer();
                clipTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                clipTimer.Tick += new EventHandler(clipTimer_Tick);
                clipTimer.Start();
                _UID = uid;
                this.button_submit.Content = "0秒";
            }
            else
            {
                cancle(null, null);
            }
            
        }
        /// <summary>
        /// 计数器响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void clipTimer_Tick(object sender = null, EventArgs e = null)
        {
            this.button_submit.Content = "分析中";
            timerCnt++;
            this.button_submit.Content = timerCnt.ToString() + "秒";
            
            this._spatial.GetAnalyseResult(_UID, new UploadStringCompletedEventHandler(getClipResult));//获取裁剪分析进度情况
        }
        private void getClipResult(object sender, UploadStringCompletedEventArgs e)
        {
            string rltName = this._spatial.OnGetAnalyseResult(e);//获取裁剪结果名称
            if (rltName == null || rltName == "")
            {
                return;
            }
            if (rltName == "*#*")
            {
                timerCnt = 0;
                MessageBox.Show("裁剪失败");
                return;
            }
            else
            {
                cancle(null, null);
                MessageBoxResult result = MessageBox.Show("裁剪分析成功。是否需要查看结果图层属性记录？", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    ShowResult(rltName);
                    this._clipLayerAcsInfo = null;
                    this.Close();
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 显示结果记录集
        /// </summary>
        /// <param name="rltName"></param>
        private void ShowResult(string rltName)
        {
            if (layerDataViewer1 == null)
                layerDataViewer1 = new LayerDataViewer() { IsPopup = true };
            layerDataViewer1.IMSCatalog = this.IMSCatalog1;
            if (this.layerDataViewer1 == null)
            {
                MessageBox.Show("layerDataViewer属性为空！请创建LayerDataViewer控件！");
            }
            this.layerDataViewer1.GraphicsLayer = GraphicsLayerObj;
            this.layerDataViewer1.ShowLayerAttRecord(rltName, this._clipLayerAcsInfo.LayerInfoList[0].LayerType, this._serverAddr);
        }
        public void close(object sender, RoutedEventArgs e)
        {
            this.Close();
            this._clipLayerAcsInfo = null;
        }
        /// <summary>
        /// 取消按钮触发函数:取消分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cancle(object sender, RoutedEventArgs e)
        {
            if (this.clipTimer != null)
            {
                this.clipTimer.Stop();
                timerCnt = 0;
            }
            this.button_submit.Content = "提交";
            this.button_submit.IsEnabled = true;
        }
    }
}
