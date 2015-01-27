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
using ZDIMS.Map;
using ZDIMS.Drawing;
using ZDIMS.Util;
using ZDIMS.Interface;
using ZDIMSDemo.Controls.Layer;
using System.Windows.Threading;
namespace ZDIMSDemo.Controls
{
    public partial class OverLayAnalyse : BaseUserControl
    {
        private SpacialAnalyse _spatial;
        private GraphicsLayer m_graphicsLayer = null;
        public LayerDataViewer layerDataViewer;
        public VectorLayerBase vectorObj;
        private string _UID = "";
        private int timerCnt = 0;
        /**
         * 时间控制属性
         */
        private DispatcherTimer overLayTimer;
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
        private IMSCatalog IMSCatalog1 = null;
        public IMSCatalog IMSCatalog
        {
            get { return IMSCatalog1; }
            set { IMSCatalog1 = value; }
        }
        public OverLayAnalyse()
        {
            InitializeComponent();
            this.myDialog1.OnClose += new RoutedEventHandler(close);

        }
        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 获取数据源列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getGdbSvrList(object sender, RoutedEventArgs e)
        {
            this.svrList.Items.Clear();
            if (this. vectorObj == null)
            {
                MessageBox. Show( "没有激活的图层。" );
                return;
            }
            this._spatial = new SpacialAnalyse(this.vectorObj);
            this._spatial.GetGdbServerList(new UploadStringCompletedEventHandler(onGetSvrList));
        }
        /// <summary>
        /// 返回数据源列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onGetSvrList(object sender, UploadStringCompletedEventArgs e)
        {
            CGetListName svr = this._spatial.OnGetGdbServerList(e);
            for (int i = 0; i < svr.Name.Length; i++)
            {
                this.svrList.Items.Add(svr.Name[i]);
            }
            this.svrList.SelectedIndex = 0;
        }
        /// <summary>
        /// 连接数据源，获取gdb列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connect(object sender, RoutedEventArgs e)
        {
            if (this.vectorObj == null)
            {
                MessageBox.Show("没有激活的图层。");
                return;
            }
            this.gdbName.Items.Clear();
            CGetGdbList obj = new CGetGdbList();
            obj.GdbSvrName = this.svrList.SelectedItem.ToString();
            obj.GdbSvrUser = this.username.Text;
            obj.GdbSvrPwds = this.password.Text;
            this._spatial = new SpacialAnalyse(this.vectorObj);
            this._spatial.GetGdbList(obj, onConnect);
        }
        /// <summary>
        /// 返回gdb列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onConnect(object sender, UploadStringCompletedEventArgs e)
        {
            CGetListName svr = this._spatial.OnGetGdbList(e);
            if (svr == null)
                return;
            for (int i = 0; i < svr.Name.Length; i++)
            {
                this.gdbName.Items.Add(svr.Name[i]);
            }
            this.gdbName.SelectedIndex = 0;
        }
        private void getLayerList(object sender, RoutedEventArgs e)
        {
            if (this.vectorObj == null)
            {
                MessageBox.Show("没有激活的图层。");
                return;
            }
            this.layer1.Items.Clear();
            this.layer2.Items.Clear();
            CGetXClsList obj = new CGetXClsList();
            obj.GdbSvrName = this.svrList.SelectedItem.ToString();
            obj.GdbSvrUser = this.username.Text;
            obj.GdbSvrPwds = this.password.Text;
            obj.GdbName = this.gdbName.SelectedItem.ToString();
            string dataT = this.dataType.SelectionBoxItem.ToString();
            obj.XclsType = dataT == "简单要素类" ? XClsType.SFeatureCls : XClsType.FeatureCls;
            this._spatial = new SpacialAnalyse(this.vectorObj);
            this._spatial.GetXClsList(obj, onGetLayer);
        }
        private void onGetLayer(object sender, UploadStringCompletedEventArgs e)
        {

            CGetListName svr = this._spatial.OnGetXClsList(e);
            for (int i = 0; i < svr.Name.Length; i++)
            {
                this.layer1.Items.Add(svr.Name[i]);
                this.layer2.Items.Add(svr.Name[i]);
            }
            if (this.layer1.Items.Count > 0)
                this.layer1.SelectedIndex = 0;
            if (this.layer2.Items.Count > 0)
                this.layer2.SelectedIndex = 0;

        }
        /// <summary>
        /// 叠加分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submit(object sender, RoutedEventArgs e)
        {
            if (this.vectorObj == null)
            {
                MessageBox.Show("没有激活的图层。");
                return;
            }
            COverlayByLayer obj = new COverlayByLayer();
            obj.FClsNameRtn = "overlay";
            CGdbInfo gdbInfo = new CGdbInfo();
            gdbInfo.GDBName = this.gdbName.SelectedValue.ToString();
            gdbInfo.GDBSvrName = this.svrList.SelectedValue.ToString();
            gdbInfo.User = this.username.Text;
            gdbInfo.Password = this.password.Text;
            obj.GdbInfo = gdbInfo;
            obj.GdbInfoOverlay = gdbInfo;
            bool c = Convert.ToBoolean(this.radio_no.IsChecked);
            if (c)
            {
                obj.IsReCalculate = c;
            }
            else
            {
                obj.IsReCalculate = Convert.ToBoolean(this.radio_yes.IsChecked);
            }
            obj.LayerName = this.layer1.SelectedItem.ToString();
            obj.OverlayLayerName = this.layer2.SelectedItem.ToString();
            bool t;
            if (t = Convert.ToBoolean(this.radioButton_bing.IsChecked))
            {
                obj.OverlayType = 0;
            }
            if (t = Convert.ToBoolean(this.radioButton_cha.IsChecked))
            {
                obj.OverlayType = 6;
            }
            if (t = Convert.ToBoolean(this.radioButton_jiao.IsChecked))
            {
                obj.OverlayType = 1;
            }
            if (t = Convert.ToBoolean(this.radioButton_jian.IsChecked))
            {
                obj.OverlayType = 2;
            }
            obj.Radius = Convert.ToDouble(this.radius.Text);
            obj.XClsType = this.dataType.SelectionBoxItem.ToString() == "简单要素类" ? XClsType.SFeatureCls : XClsType.FeatureCls;
            this._spatial = new SpacialAnalyse(this.vectorObj);
            _spatial.OverlayByLayer(obj, new UploadStringCompletedEventHandler(onSubmit));
            this.button_submit.IsEnabled = false;

        }
        private void onSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            string uid = this._spatial.OnOverlayByLayer(e);
            if (uid != null && uid != "")
            {
                if (overLayTimer == null)
                {
                    overLayTimer = new DispatcherTimer();
                    overLayTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                    overLayTimer.Tick += new EventHandler(overLayTimerTimer_Tick);
                }
                overLayTimer.Start();
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
        protected void overLayTimerTimer_Tick(object sender = null, EventArgs e = null)
        {
            if (button_submit.IsEnabled)
            {
                if (sender is DispatcherTimer)
                    (sender as DispatcherTimer).Stop();
                timerCnt = 0;
                this.button_submit.Content = "提交"; return;
            }
            timerCnt++;
            this.button_submit.Content = timerCnt.ToString() + "秒";

            this._spatial.GetAnalyseResult(_UID, new UploadStringCompletedEventHandler(getOverlayResult));//获取裁剪分析进度情况
        }
        private void getOverlayResult(object sender, UploadStringCompletedEventArgs e)
        {
            string rltName = this._spatial.OnGetAnalyseResult(e);
            if (rltName == null || rltName == "")
            {
                return;
            }
            if (rltName == "*#*")
            {
                MessageBox.Show("叠加分析失败.");
                return;
            }
            else
            {
                cancle(null, null);
                MessageBoxResult result = MessageBox.Show("叠加分析成功。是否需要查看结果图层属性记录？", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    onShow(rltName);
                }
                else
                {
                    return;
                }
            }
        }
        /// <summary>
        /// 返回叠加结果图层的数据
        /// </summary>
        /// <param name="rltName"></param>
        private void onShow(string rltLayerName)
        {
            if (layerDataViewer == null)
                layerDataViewer = new LayerDataViewer() { IsPopup = true };
            layerDataViewer.IMSCatalog = this.IMSCatalog1;
            if (this.layerDataViewer == null)
            {
                MessageBox.Show("layerDataViewer属性为空！请创建LayerDataViewer控件！");
            }
            this.layerDataViewer.GraphicsLayer = GraphicsLayerObj;
            XClsType layerType = this.dataType.SelectionBoxItem.ToString() == "简单要素类" ? XClsType.SFeatureCls : XClsType.FeatureCls;
            this.layerDataViewer.ShowLayerAttRecord(rltLayerName, layerType, this.vectorObj.ServerAddress);
        }
        /// <summary>
        /// 取消叠加分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancle(object sender, RoutedEventArgs e)
        {
            if (this.overLayTimer != null)
            {
                this.overLayTimer.Stop();
                timerCnt = 0;
            }
            this.button_submit.Content = "提交";
            this.button_submit.IsEnabled = true;
        }
    }
}
