using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace ZDIMSDemo.Controls
{
    public enum FadeType
    {
      Display,
      Dispear
    }

    public class BaseUserControl : UserControl
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
            set {
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
            IsPopup = true;
            this.Opacity = 0;
        }

        protected virtual void OnClickOutside(object sender, MouseButtonEventArgs e) { e.Handled = true; }

        #region 公共方法

        public virtual  void Show()
        {
            Application.Current.Host.Content.Resized -= new EventHandler(Content_Resized);
            UpdateSize();
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (this.Equals(grid.Children[i]))
                {
                    popup.IsOpen = true;
                    this.CreateFadeInOutAnimation(FadeType.Display);
                    return;
                }
            }
            grid.Children.Add(this);
            popup.IsOpen = true;
            this.CreateFadeInOutAnimation(FadeType.Display);
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
               Storyboard story=this.CreateFadeInOutAnimation(FadeType.Dispear);
                story.Completed += new EventHandler(StoryDispearCompleted);
            }
        }
        #endregion
        #region 私有方法
        private void StoryDispearCompleted(object sender,EventArgs e)
        {
            popup.IsOpen = false;
        }

        private void Content_Resized(object sender, EventArgs e)
        {
            UpdateSize();
        }
        private void UpdateSize()
        {
            if (grid != null)
            {
                grid.Width = Application.Current.Host.Content.ActualWidth;
                grid.Height = Application.Current.Host.Content.ActualHeight;
                if (canvas != null)
                {
                    canvas.Width = grid.Width;
                    canvas.Height = grid.Height;
                }
            }
        }

        /// <summary>
        /// 生成并调用淡出动画效果
        /// </summary>
        private Storyboard CreateFadeInOutAnimation(FadeType fadeType)
        {
            string storyName = fadeType.ToString() + "Fade";
            if (!this.Resources.Contains(storyName))
            {
                Duration duration = new Duration(TimeSpan.FromSeconds(1.5));
                DoubleAnimation fadeAnimation = new DoubleAnimation();
                fadeAnimation.Duration = duration;
                Storyboard story = new Storyboard();
                story.Duration = duration;
                Storyboard.SetTarget(fadeAnimation, this);
                Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath("Opacity"));
                if (fadeType == FadeType.Display)
                {
                    fadeAnimation.From = 0;
                    fadeAnimation.To = 0.85;
                }
                else
                {
                    fadeAnimation.From = 0.85;
                    fadeAnimation.To = 0;
                }
                story.Children.Add(fadeAnimation);
                story.SetValue(NameProperty, storyName);
                this.Resources.Add(storyName, story);
                story.Begin();
                return story;
            }
            else
            {
                Storyboard tempSb = new Storyboard();
                IEnumerator keyEnum = this.Resources.Values.GetEnumerator();
                while (keyEnum.MoveNext())
                {
                    Storyboard sb = keyEnum.Current as Storyboard;
                    if (sb != null)
                    {
                        string name=sb.GetValue(NameProperty).ToString();
                        if (name == storyName)
                        {
                            sb.Begin();
                            tempSb = sb;
                            break;
                        }
                    }
                }
                return tempSb;
            }
        }
        #endregion
    }
}
