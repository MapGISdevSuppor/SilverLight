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
using ZdimsExtends.trackPlay;
using System.Windows.Media.Imaging;

using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity;
using Microsoft.Expression.Interactivity.Layout;
namespace SilverlightDemo.buscomponets.BusControl
{
    public partial class setPlay : BaseUserControl
    {
        private trackplay _trck;
        /// <summary>
        /// 播放线路
        /// </summary>
        public trackplay track
        {
            get { return this._trck; }
            set
            {
                if (this._trck == null)
                {
                    this._trck = value;
                    this.reSetbutton();
                }
                else
                {
                    if (value.Pnts != this._trck.Pnts)
                    {
                        this._trck = value;
                        this.reSetbutton();
                    }
                }
             
        }
        }
        bool playFlag = false;
        public setPlay()
        {
            InitializeComponent();
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard stb = Resources["stbShowHeader"] as Storyboard;
            stb.Completed += new EventHandler(stb_Completed);
            stb.Begin();
        }
        public bool IsShow;
        public void showCtrl()
        {
            this.Show();
            this.IsShow = true;
            Storyboard stb = Resources["stbShowHeader"] as Storyboard;
           // stb.Completed += new EventHandler(stb_Completed);
            this.LayoutRoot.Visibility = Visibility.Visible;
            this.LayoutRoot.Opacity = 1;
            stb.Begin();
        }
        void stb_Completed(object sender, EventArgs e)
        {
            Storyboard stbImgBg = Resources["stbImgBg"] as Storyboard;
            //stbImgBg.Completed += new EventHandler(stbImgBg_Completed);
           // stbImgBg.Begin();
        }

        private void close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
           // this.Close();
            Storyboard stbContentBgBack = Resources["stbContentBgBack1"] as Storyboard;
            stbContentBgBack.Completed += new EventHandler(stbContentBgBack_Completed);
            stbContentBgBack.Begin();
          
            //this.Close();
        }

        void stbContentBgBack_Completed(object sender, EventArgs e)
        {
            Storyboard stbContentBgBack = Resources["stbContentBgBack"] as Storyboard;
            stbContentBgBack.Completed += new EventHandler(stbContentBgBack_Completed1);
            stbContentBgBack.Begin();
            //TranslateTransform x = new TranslateTransform();
            //x.X = -200;
            //this.RenderTransform = x;
           
        }
        void stbContentBgBack_Completed1(object sender, EventArgs e)
        {
            this.IsShow = false;
            this.Close();
        }
        private bool goOn=false;
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (track == null)
            {
                MessageBox.Show("无播放线路！");
                return;
            }
            track.playOverBackHander += playOver;
            if (!playFlag)
            {

                playFlag = true;
                this.pause.Source = new BitmapImage(new Uri("/images/play/pause-32.png", UriKind.Relative));
                this.tooltip.Text = "暂停";
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
                this.tooltip.Text = "播放";
                this.pause.Source = new BitmapImage(new Uri("/images/play/play-32.png", UriKind.Relative));
            }
        }

        private void playOver()
        {
            
            reSetbutton();
            this.track.playOverBackHander -= playOver;
        }
        public void reSetbutton()
        {
            this.playFlag = false;
            this.tooltip.Text = "播放";
            this.pause.Source = new BitmapImage(new Uri("/images/play/play-32.png", UriKind.Relative));
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            this.goOn = false;
            this.playFlag = false;
            this.track.stop();
            reSetbutton();
            this.track.m_markLayer.MapContainer.Refresh();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.track != null)
            {
                this.tooltip1.Text = (-this.slider1.Value).ToString();
                this.track.milliseconds = -Convert.ToInt32(this.slider1.Value);
            }
        }

        MouseDragElementBehavior dragBehavior = new MouseDragElementBehavior();
        private bool isDrag;
        /// <summary>
        /// 是否允许被拖拽和鼠标右键
        /// </summary>
        public bool IsDrag
        {
            get { return isDrag; }
            set
            {
                isDrag = value;
                if (isDrag == true)
                {
                    dragBehavior.Attach(this);//将本对象加入到可以鼠标拖动的行为对象中去
                    dragBehavior.DragFinished += new MouseEventHandler(dragBehavior_DragFinished);
                    //在对象移动成功之后加载一个处理事件。
                }
                else if (isDrag == false)
                {
                    try
                    {
                        //设置本控件移动行为取消，并且取消DragFinished处理事件
                        dragBehavior.Detach();
                        dragBehavior.DragFinished -= new MouseEventHandler(dragBehavior_DragFinished);
                    }
                    catch
                    {

                    }
                }

            }
        }
        
        /// <summary>
        /// 被拖动完成之后触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dragBehavior_DragFinished(object sender, MouseEventArgs e)
        {
            MouseDragElementBehavior dragBehavior = sender as MouseDragElementBehavior;
            this.Tag = dragBehavior.X + "|" + dragBehavior.Y; // this.Tag设置为相应的值
           
            //设置鼠标拖动本控件之后，在label1中显示当前控件的坐标位置
        }

        private void close_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           // this.Close();
        }

        private void close_MouseEnter(object sender, MouseEventArgs e)
        {
            this.IsDrag = false;
        }

        private void close_MouseLeave(object sender, MouseEventArgs e)
        {
            this.IsDrag= true;
        }
        private bool IsPlay = false;
        private void pause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (track == null)
            {
                MessageBox.Show("无播放线路！");
                return;
            }
            IsPlay = true;
            track.playOverBackHander += playOver;
            if (!playFlag)
            {

                playFlag = true;
                this.pause.Source = new BitmapImage(new Uri("/images/play/pause-32.png", UriKind.Relative));
                this.tooltip.Text = "暂停";
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
                this.tooltip.Text = "播放";
                this.pause.Source = new BitmapImage(new Uri("/images/play/play-32.png", UriKind.Relative));
            }
        }

        
        private void stop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            reSetControl();
        }
      
       
        public void reSetControl()
        {
            this.goOn = false;
            this.playFlag = false;
            if(this.IsPlay)
                this.track.stop();
            reSetbutton();
        }
    }
}
