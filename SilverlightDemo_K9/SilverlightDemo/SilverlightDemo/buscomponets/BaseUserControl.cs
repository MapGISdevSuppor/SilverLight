using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
namespace SilverlightDemo.buscomponets
{
    public class BaseUserControl:UserControl
    {
        #region 私有字段
        Popup popup;
        Grid grid;
        Canvas canvas;
        bool m_isPopup = false;
        #endregion
        /// <summary>
        /// 是否是弹出模式
        /// </summary>
        public bool IsPopup
        {
            set
            {
                m_isPopup = value;
                if (value)
                {
                    if (popup == null)
                    {
                        popup = new Popup();
                        grid = new Grid();
                        popup.Child = grid;
                    }
                }
                else
                {
                    if (popup != null)
                    {
                        popup.Child = null;
                        grid = null;
                        popup = null;
                    }
                }
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseUserControl()
        {
            //popup = new Popup();
            //grid = new Grid();
            //popup.Child = grid;
            IsPopup = true;
        }

        protected virtual void OnClickOutside(object sender, MouseButtonEventArgs e) { e.Handled = true; }

        #region 公共方法

        public virtual void Show()
        {
            Application.Current.Host.Content.Resized -= new EventHandler(Content_Resized);
            UpdateSize();
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (this.Equals(grid.Children[i]))
                {
                    popup.IsOpen = true;
                    return;
                }
            }
            grid.Children.Add(this);
            popup.IsOpen = true;

        }
        public virtual void ShowAsModal()
        {
            grid.Children.Clear();
            Application.Current.Host.Content.Resized -= new EventHandler(Content_Resized);
            canvas = new Canvas();
            Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
            UpdateSize();
            canvas.Background = new SolidColorBrush(Colors.Black);
            canvas.Opacity = 0.2;
            canvas.MouseLeftButtonDown += (sender, args) => { OnClickOutside(sender, args); };
            grid.Children.Add(canvas);
            grid.Children.Add(this);
            popup.IsOpen = true;
        }

        public void Close()
        {
            if (popup != null)
            {
                popup.IsOpen = false;
                //popup = null;
                //grid = null;
                //canvas = null;
            }
        }
        #endregion
        #region 私有方法
        private void Content_Resized(object sender, EventArgs e)
        {
            UpdateSize();
        }
        private void UpdateSize()
        {
            if (grid != null)
            {
                //double x = Canvas.GetLeft(this.grid);
                //double y = Canvas.GetTop(this.grid);
                //if (x < 0 || y < 0 || x > Application.Current.Host.Content.ActualWidth || y > Application.Current.Host.Content.ActualHeight)
                //{
                //    Canvas.SetLeft(this, 0);
                //    Canvas.SetTop(this, 0);
                //}
                grid.Width = Application.Current.Host.Content.ActualWidth;
                grid.Height = Application.Current.Host.Content.ActualHeight;
                if (canvas != null)
                {
                    canvas.Width = grid.Width;
                    canvas.Height = grid.Height;
                }
            }
        }
        #endregion
    }
}
