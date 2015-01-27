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
using ZDIMS.Interface;
using ZDIMS.Map;

namespace SilverlightDemo.components
{
    /// <summary>
    /// mark点击回调委托
    /// </summary>
    /// <param name="mark">mark对象</param>
    public delegate void MarkClickDelegate(IMSmyMark mark);
    /// <summary>
    /// mark鼠标操作回调委托
    /// </summary>
    /// <param name="mark">mark对象</param>
    /// <param name="sender">事件发送对象</param>
    /// <param name="e">包含事件的数据源</param>
    public delegate void MouseOperDelegate(IMSmyMark mark, object sender, MouseEventArgs e);
    /// <summary>
    /// 标注类
    /// </summary>
    public class IMSmyMark : MarkBase
    {
        /// <summary>
        /// mark点击回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MarkClickDelegate MarkClickCallback = null;

        #region 内部成员变量
        private bool m_enableAnimation = true;
        private bool m_mouseDown = false;
        private Point m_mouseDownCoordinate;
        /// <summary>
        /// 鼠标从标注上移走时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseLeaveCallback = null;
        /// <summary>
        /// 鼠标移上标注时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseEnterCallback = null;
        /// <summary>
        /// 鼠标在标注上左键弹起时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseLeftButtonUpCallback = null;
        /// <summary>
        /// 鼠标在标注上右键弹起时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseRightButtonUpCallback = null;
        /// <summary>
        /// 鼠标在标注上移动时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseMoveCallback = null;
        /// <summary>
        /// 鼠标在标注上左键按下时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseLeftButtonDownCallback = null;
        /// <summary>
        /// 鼠标在标注上右键按下时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseRightButtonDownCallback = null;
        /// <summary>
        /// 鼠标在标注上点击时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        private MouseOperDelegate m_mouseClickCallback = null;
        #endregion

        #region 属性
        /// <summary>
        /// 鼠标从标注上移走时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseLeaveCallback
        {
            get { return m_mouseLeaveCallback; }
            set
            {
                m_mouseLeaveCallback = value;
                if (m_markControl != null)
                {
                    if (m_mouseLeaveCallback != null)
                        m_markControl.MouseLeave += new MouseEventHandler(OnMouseLeaveCallback);
                    else
                        m_markControl.MouseLeave -= new MouseEventHandler(OnMouseLeaveCallback);
                }
            }
        }
        /// <summary>
        /// 鼠标移上标注时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseEnterCallback
        {
            get { return m_mouseEnterCallback; }
            set
            {
                m_mouseEnterCallback = value;
                if (m_markControl != null)
                {
                    if (m_mouseEnterCallback != null)
                        m_markControl.MouseEnter += new MouseEventHandler(OnMouseEnterCallback);
                    else
                        m_markControl.MouseEnter -= new MouseEventHandler(OnMouseEnterCallback);
                }
            }
        }
        /// <summary>
        /// 鼠标在标注上左键弹起时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseLeftButtonUpCallback
        {
            get { return m_mouseLeftButtonUpCallback; }
            set
            {
                m_mouseLeftButtonUpCallback = value;
                if (m_markControl != null)
                {
                    if (m_mouseLeftButtonUpCallback != null)
                        m_markControl.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseLeftButtonUpCallback);
                    else
                        m_markControl.MouseLeftButtonUp -= new MouseButtonEventHandler(OnMouseLeftButtonUpCallback);
                }
            }
        }
        /// <summary>
        /// 鼠标在标注上右键弹起时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseRightButtonUpCallback
        {
            get { return m_mouseRightButtonUpCallback; }
            set
            {
                m_mouseRightButtonUpCallback = value;
                if (m_markControl != null)
                {
                    if (m_mouseRightButtonUpCallback != null)
                        m_markControl.MouseRightButtonUp += new MouseButtonEventHandler(OnMouseRightButtonUpCallback);
                    else
                        m_markControl.MouseRightButtonUp -= new MouseButtonEventHandler(OnMouseRightButtonUpCallback);
                }
            }
        }
        /// <summary>
        /// 鼠标在标注上移动时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseMoveCallback
        {
            get { return m_mouseMoveCallback; }
            set
            {
                m_mouseMoveCallback = value;
                if (m_markControl != null)
                {
                    if (m_mouseMoveCallback != null)
                        m_markControl.MouseMove += new MouseEventHandler(OnMouseMoveCallback);
                    else
                        m_markControl.MouseMove -= new MouseEventHandler(OnMouseMoveCallback);
                }
            }
        }
        /// <summary>
        /// 鼠标在标注上左键按下时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseLeftButtonDownCallback
        {
            get { return m_mouseLeftButtonDownCallback; }
            set
            {
                m_mouseLeftButtonDownCallback = value;
                if (m_markControl != null)
                {
                    if (m_mouseLeftButtonDownCallback != null)
                        m_markControl.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDownCallback);
                    else
                        m_markControl.MouseLeftButtonDown -= new MouseButtonEventHandler(OnMouseLeftButtonDownCallback);
                }
            }
        }
        /// <summary>
        /// 鼠标在标注上右键按下时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseRightButtonDownCallback
        {
            get { return m_mouseRightButtonDownCallback; }
            set
            {
                m_mouseRightButtonDownCallback = value;
                if (m_markControl != null)
                {
                    if (m_mouseRightButtonDownCallback != null)
                        m_markControl.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDownCallback);
                    else
                        m_markControl.MouseRightButtonDown -= new MouseButtonEventHandler(OnMouseRightButtonDownCallback);
                }
            }
        }
        /// <summary>
        /// 鼠标在标注上点击时回调（不需要该回调时，请赋值为null） 
        /// </summary>
        public MouseOperDelegate MouseClickCallback
        {
            get { return m_mouseClickCallback; }
            set
            {
                m_mouseClickCallback = value;
            }
        }        
        /// <summary>
        /// 是否允许使用动画效果
        /// </summary>
        public bool EnableAnimation
        {
            get { return m_enableAnimation && m_markControl.Parent is MarkLayer; }
            set
            {
                m_enableAnimation = value;
                if (m_markControl != null)
                {
                    m_markControl.MouseEnter -= new MouseEventHandler(OnMouseEnter);
                    m_markControl.MouseLeave -= new MouseEventHandler(OnMouseLeave);
                    if (m_enableAnimation)
                    {
                        m_markControl.MouseEnter += new MouseEventHandler(OnMouseEnter);
                        m_markControl.MouseLeave += new MouseEventHandler(OnMouseLeave);
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="markControl">要显示的标注点控件对象</param>
        /// <param name="coordinateType">坐标类型（默认是屏幕坐标，如果是逻辑坐标，在绘图时会根据地图容器的LogicToScreen方法转换坐标）</param>
        /// <param name="mLayer">标注图层对象，为“null”时会内部自动获取</param>
        /// <remarks>
        /// 下载示例代码<br/>
        /// <a href="http://www.lbsmap.net/imsdownload/IMSMark_Demo.rar">IMSMark_Demo</a>
        /// </remarks>
        public IMSmyMark(FrameworkElement markControl, CoordinateType coordinateType = CoordinateType.Screen, MarkLayer mLayer = null) :
            base(markControl, coordinateType, mLayer)
        {
            if (markControl != null)
            {
                markControl.Cursor = Cursors.Hand;
                markControl.MouseEnter += new MouseEventHandler(OnMouseEnter);
                markControl.MouseLeave += new MouseEventHandler(OnMouseLeave);
            }
        }

        #region 事件回调
        /// <summary>
        /// 鼠标从Mark上移开事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (m_markControl != null && EnableAnimation)
            {
                m_markControl.RenderTransform = new ScaleTransform()
                {
                    ScaleX = 1,
                    ScaleY = 1
                };
                if (m_markControl.ActualHeight > 0 && m_markControl.ActualWidth > 0)
                {
                    Canvas.SetLeft(m_markControl, Canvas.GetLeft(m_markControl) + m_markControl.ActualWidth * 0.2);
                    Canvas.SetTop(m_markControl, Canvas.GetTop(m_markControl) + m_markControl.ActualHeight * 0.2);
                }
            }
        }
        /// <summary>
        /// 鼠标移上Mark事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (m_mapContainer != null)
            {
                if (m_mapContainer.MouseEventLockFlg)
                    return;
            }
            if (m_markControl != null && EnableAnimation)
            {
                if (m_markControl.ActualHeight > 0 && m_markControl.ActualWidth > 0)
                {
                    Canvas.SetLeft(m_markControl, Canvas.GetLeft(m_markControl) - m_markControl.ActualWidth * 0.2);
                    Canvas.SetTop(m_markControl, Canvas.GetTop(m_markControl) - m_markControl.ActualHeight * 0.2);
                }
                m_markControl.RenderTransform = new ScaleTransform()
                {
                    ScaleX = 1.4,
                    ScaleY = 1.4
                };
            }
        }
        /// <summary>
        /// 鼠标左键按下事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected override void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (m_mapContainer != null)
            {
                if (m_mapContainer.MouseEventLockFlg)
                    return;
            }
            base.OnMouseLeftButtonDown(sender, e);
            if (m_markControl != null)
            {
                this.m_mouseDown = true;
                this.m_mouseDownCoordinate = e.GetPosition(this.m_markControl);
                //e.Handled = true;
            }
        }
        /// <summary>
        /// 鼠标左键弹起事件回调
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件数据源</param>
        protected override void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(sender, e);
            if (m_markControl != null && m_mouseDown)
            {
                Point position = e.GetPosition(this.m_markControl);
                if ((position.X == this.m_mouseDownCoordinate.X) && (position.Y == this.m_mouseDownCoordinate.Y))
                {
                    OnClick(sender, e);                    
                    this.m_mouseDownCoordinate = new Point(-1, -1);
                }
                m_mouseDown = false;
                //e.Handled = true;
            }
        }
        /// <summary>
        /// mark上点击
        /// </summary>
        protected virtual void OnClick(object sender, MouseButtonEventArgs e)
        {
            if (MarkClickCallback != null)
                MarkClickCallback(this);
            if (m_mouseClickCallback != null)
                m_mouseClickCallback(this, sender, e);
        }

        private void OnMouseRightButtonDownCallback(object sender, MouseButtonEventArgs e)
        {
            if (m_mouseRightButtonDownCallback != null)
                m_mouseRightButtonDownCallback(this, sender, e);
        }
        private void OnMouseLeftButtonDownCallback(object sender, MouseButtonEventArgs e)
        {
            if (m_mouseLeftButtonDownCallback != null)
                m_mouseLeftButtonDownCallback(this, sender, e);
        }
        private void OnMouseMoveCallback(object sender, MouseEventArgs e)
        {
            if (m_mouseMoveCallback != null)
                m_mouseMoveCallback(this, sender, e);
        }
        private void OnMouseRightButtonUpCallback(object sender, MouseButtonEventArgs e)
        {
            if (m_mouseRightButtonUpCallback != null)
                m_mouseRightButtonUpCallback(this, sender, e);
        }
        private void OnMouseLeftButtonUpCallback(object sender, MouseButtonEventArgs e)
        {
            if (m_mouseLeftButtonUpCallback != null)
                m_mouseLeftButtonUpCallback(this, sender, e);
        }
        private void OnMouseEnterCallback(object sender, MouseEventArgs e)
        {
            if (m_mouseEnterCallback != null)
                m_mouseEnterCallback(this, sender, e);
        }
        private void OnMouseLeaveCallback(object sender, MouseEventArgs e)
        {
            if (m_mouseLeaveCallback != null)
                m_mouseLeaveCallback(this, sender, e);
        }
        #endregion
    }
}
