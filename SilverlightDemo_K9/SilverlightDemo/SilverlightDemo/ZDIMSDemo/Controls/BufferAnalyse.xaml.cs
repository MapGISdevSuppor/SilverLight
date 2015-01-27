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
using ZDIMS.Util;
using ZDIMS.Map;
using ZDIMS.BaseLib;
using ZDIMSDemo.Controls.Layer;

namespace ZDIMSDemo.Controls
{
    public partial class BufferAnalyse : BaseUserControl
    {
        private SpacialAnalyse _spatial;
        private IMSCatalog m_catalog = null;//目录
        private LayerDataViewer m_layerDataViewer;
        private int timerCnt = 0;
        /**
		* 时间控制属性
		*/
        private DispatcherTimer clipTimer;
        private VectorLayerBase m_vectorBase = null;
        /**
		* 缓冲区分析的结果图层名
		*/ 
		private string _bufferRltLayerName;
        /// <summary>
        /// 关联的地图或者图层对象
        /// </summary>
        public VectorLayerBase VectorBaseObject
        {
            get { return m_vectorBase; }
            set
            {
                m_vectorBase = value;
                _spatial = new SpacialAnalyse(m_vectorBase);
            }
        }
        /// <summary>
        /// 树目录
        /// </summary>
        public IMSCatalog IMSCatalog
        {
            get { return m_catalog; }
            set { m_catalog = value; }
        }
        /**
		* 缓冲区分析操作需要的参数
		*/ 
        private SFeatureGeometry[] _sfGeometry;
        private CAttStruct _attStrct;
        private CAttDataRow[] _attRows;
        public BufferAnalyse()
        {
            InitializeComponent();
            this.m_layerDataViewer = new LayerDataViewer();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
        }
        /**
		* 设置传入参数
		*/
        public void setTargetsfGeometry(SFeatureGeometry[] target)
		{
            _sfGeometry = target;
		}
        /**
       * 设置传入参数
       */
        public void setTargetattStrct(CAttStruct target)
        {
            _attStrct = target;
        }
        /**
       * 设置传入参数
       */
        public void setTargetattRows(CAttDataRow[] target)
        {
            _attRows = target;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void buffer_Click(object sender, RoutedEventArgs e)
        {
            if(this.bufferRadius.Text == "") 
				{
					MessageBox.Show("请输入缓冲分析半径。");
					return;
				}
				if(this.traceRadius.Text == "") 
				{
					MessageBox.Show("请输入缓冲跟踪半径。");
					return;
				}
                if (this._sfGeometry == null)
				{
                    //if(this._targetGeometry == null) 
                    //{
						MessageBox.Show("请输入需要缓冲的信息。");
						return;
                    //}
				}
				else
				{
                    CBufferByGeometry bufferObj = new CBufferByGeometry();
					
					bufferObj.BufferRadius = Convert.ToDouble(this.bufferRadius.Text);
					bufferObj.TraceRadius = Convert.ToDouble(this.traceRadius.Text);
					if(bufferObj.BufferRadius.ToString()=="NaN" ||bufferObj.TraceRadius.ToString()=="NaN")
					{
						MessageBox.Show("请输入正确的半径参数。");
						return;	
					}
                    bufferObj.AttRows = this._attRows;
                    bufferObj.AttStrct = this._attStrct;
                    bufferObj.SfGeometry = this._sfGeometry;
                    bufferObj.ResultLayerName = this.resultLayerName.Text + Guid.NewGuid().ToString();// DateTime.Now.ToString();
					bufferObj.ResultLayerType = XClsType.SFeatureCls;
					this._bufferRltLayerName = bufferObj.ResultLayerName;
					this._spatial.BufferByGeometry(bufferObj,new UploadStringCompletedEventHandler(onSubmit));
				}
                clipTimer = new DispatcherTimer();
                clipTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                clipTimer.Tick += new EventHandler(clipTimer_Tick);
                //clipTimer.addEventListener(TimerEvent.TIMER,onTimer);
                this.buffer.Content = "0秒";
                clipTimer.Start();
                this.buffer.IsEnabled = false;
        }
         /// <summary>
        /// 计数器响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void clipTimer_Tick(object sender = null, EventArgs e = null)
        {
            if (this.buffer.IsEnabled)
            {
                this.buffer.Content = "运行中";
                timerCnt++;
                this.buffer.Content = timerCnt.ToString() + "秒";
            }
            else if (sender != null && (sender is DispatcherTimer))
                (sender as DispatcherTimer).Stop();
        }
        /**
		* 缓冲区分析回调函数
		*/
        private void onSubmit(object sender, UploadStringCompletedEventArgs e)
        {
            if (this.buffer.IsEnabled)
                return;
            COperResult Rlt = this._spatial.OnBufferByGeometry(e);        
            showRlt(Rlt);
            cancel();
        }
        /**
		* 取消按钮触发函数
		*/ 
		public void cancel()
		{
            if (this.clipTimer != null)
            {
                this.clipTimer.Stop();
                timerCnt = 0;
            }
			this.buffer.Content = "提交";
			this.buffer.IsEnabled = true;
		}
        public void showRlt(COperResult Rlt)
        {
            if(Rlt == null) 
            {
                return;
            }            
            if(Rlt.OperResult)
            {
                //Alert.yesLabel = "查看结果";  
                //Alert.noLabel = "取消"; 
                if (MessageBox.Show("缓冲区分析成功。是否需要查看结果图层属性记录？", "缓冲区分析成功", MessageBoxButton.OKCancel) == MessageBoxResult.OK)//"提示",Alert.YES|Alert.NO , this , onAlert , null , Alert.NO);  
                {
                    showLayer();
                }
            }
            else
            {
                MessageBox.Show("缓冲区分析失败。错误信息：" + Rlt.ErrorInfo);
            }
        }
        /**
        * 查看图层属性记录回调函数
        */ 
        public void showLayer()
        {
            if(this.m_layerDataViewer==null)
            {
                MessageBox.Show("layerDataViewer属性为空！请创建LayerDataViewer控件！");
                return;
            }
            this.m_layerDataViewer.IMSCatalog = m_catalog;
            this.m_layerDataViewer.ShowLayerAttRecord(this._bufferRltLayerName, XClsType.SFeatureCls, m_vectorBase.ServerAddress);
        }
        private void cancle_Click(object sender, RoutedEventArgs e)
        {
            cancel();            
            this.Close();
        }
    }
}
