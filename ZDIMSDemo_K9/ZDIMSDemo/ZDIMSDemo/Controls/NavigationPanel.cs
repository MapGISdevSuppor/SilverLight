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
using ZDIMS.Map;

namespace ZDIMSDemo.Controls
{
    [TemplatePart(Name = PART_horizontalSplineDoubleKeyFrame, Type = typeof(SplineDoubleKeyFrame))]
    [TemplatePart(Name = PART_borderHorizontalToolbar, Type = typeof(Border))]
    [TemplatePart(Name = PART_rightButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_downButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_leftButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_upButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_spinButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_zoomInButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_zoomOutButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_NavigationGrid, Type = typeof(Panel))]
    [TemplateVisualState(Name = VSM_Expanded, GroupName = VSM_DashboardStates)]
    [TemplateVisualState(Name = VSM_Collapsed, GroupName = VSM_DashboardStates)]
    [TemplateVisualState(Name = VSM_Normal, GroupName = VSM_CommonStates)]
    [TemplateVisualState(Name = VSM_MouseOver, GroupName = VSM_CommonStates)]
    public class NavigationPanel : ContentControl
    {
        private const int panAmount = 250;
        private const string PART_borderHorizontalToolbar = "PART_borderHorizontalToolbar";
        private const string PART_downButton = "PART_downButton";
        private const string PART_horizontalSplineDoubleKeyFrame = "PART_HorizontalSplineDoubleKeyFrame";
        private const string PART_leftButton = "PART_leftButton";
        private const string PART_rightButton = "PART_rightButton";
        private const string PART_spinButton = "PART_spinButton";
        private const string PART_upButton = "PART_upButton";
        private const string PART_zoomInButton = "PART_zoomInButton";
        private const string PART_zoomOutButton = "PART_zoomOutButton";
        private const string PART_NavigationGrid = "PART_NavigationGrid";
        private const string VSM_Collapsed = "Collapsed";
        private const string VSM_CommonStates = "CommonStates";
        private const string VSM_DashboardStates = "DashboardStates";
        private const string VSM_Expanded = "Expanded";
        private const string VSM_MouseOver = "MouseOver";
        private const string VSM_Normal = "Normal";

        public static readonly DependencyProperty HorizontalToolBarLeftProperty =
            DependencyProperty.Register("HorizontalToolBarLeft",
                                        typeof(double),
                                        typeof(NavigationPanel),
                                        null);

        public static readonly DependencyProperty NavCircleScaleProperty =
            DependencyProperty.Register("NavCircleScale",
                                        typeof(double),
                                        typeof(NavigationPanel),
                                        null);

        public static readonly DependencyProperty VerticalToolBarTopProperty =
            DependencyProperty.Register("VerticalToolBarTop",
                                        typeof(double),
                                        typeof(NavigationPanel),
                                        null);

        private Border borderHorizontalToolbar;
        private RepeatButton downButton;
        private SplineDoubleKeyFrame horizontalSplineDoubleKeyFrame;
        private bool isExpanded = true;
        private bool isMouseOver;
        private RepeatButton leftButton;
        //private string mapName;
        private bool panelDimensionsSet;
        private RepeatButton rightButton;
        private Button spinButton;
        private RepeatButton upButton;
        private RepeatButton zoomInButton;
        private RepeatButton zoomOutButton;
        private Panel navigationGrid;
        private IMSMap m_mapContainer;

        public NavigationPanel()
        {
            DefaultStyleKey = typeof(NavigationPanel);
            Loaded += NavigationPanel_Loaded;
        }

        public double HorizontalToolBarLeft
        {
            get { return (double)GetValue(HorizontalToolBarLeftProperty); }
            set { SetValue(HorizontalToolBarLeftProperty, value); }
        }

        public double VerticalToolBarTop
        {
            get { return (double)GetValue(VerticalToolBarTopProperty); }
            set { SetValue(VerticalToolBarTopProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TheName.  
        // This enables animation, styling, binding, etc...

        public double NavCircleScale
        {
            get { return (double)GetValue(NavCircleScaleProperty); }
            set
            {
                SetValue(NavCircleScaleProperty, value);
                //binding doesn't work for some reason, set it manually
                if (navigationGrid != null)
                {
                    var transformGroup = navigationGrid.RenderTransform as TransformGroup;
                    if (transformGroup != null)
                    {
                        var scaleTransform = (ScaleTransform)transformGroup.Children[0];
                        scaleTransform.ScaleX = value;
                        scaleTransform.ScaleY = value;
                    }
                }
            }
        }

        private void NavigationPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Parent is IMSMap)
            {
                m_mapContainer = this.Parent as IMSMap;
                MouseEnter += NavigationPanel_MouseEnter;
                MouseLeave += NavigationPanel_MouseLeave;
                GoToState(true);
            }
        }

        private void NavigationPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOver = false;
            GoToState(true);
        }

        private void NavigationPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOver = true;
            GoToState(true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rightButton = (RepeatButton)GetTemplateChild(PART_rightButton);
            downButton = (RepeatButton)GetTemplateChild(PART_downButton);
            leftButton = (RepeatButton)GetTemplateChild(PART_leftButton);
            upButton = (RepeatButton)GetTemplateChild(PART_upButton);
            spinButton = (Button)GetTemplateChild(PART_spinButton);
            zoomOutButton = (RepeatButton)GetTemplateChild(PART_zoomOutButton);
            zoomInButton = (RepeatButton)GetTemplateChild(PART_zoomInButton);
            horizontalSplineDoubleKeyFrame = (SplineDoubleKeyFrame)GetTemplateChild(PART_horizontalSplineDoubleKeyFrame);
            borderHorizontalToolbar = (Border)GetTemplateChild(PART_borderHorizontalToolbar);
            navigationGrid = (Panel)GetTemplateChild(PART_NavigationGrid);

            NavCircleScale = NavCircleScale;

            //events
            rightButton.Click += rightButton_Click;
            downButton.Click += downButton_Click;
            leftButton.Click += leftButton_Click;
            upButton.Click += upButton_Click;
            spinButton.Click += spinButton_Click;
            zoomOutButton.Click += zoomOutButton_Click;
            zoomInButton.Click += zoomInButton_Click;
            IsEnabled = true;
        }

        private void zoomInButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer!=null&&IsEnabled)
            {
                m_mapContainer.ZoomIn();
            }
        }

        private void zoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null && IsEnabled)
            {
                m_mapContainer.ZoomOut();
            }
        }

        private void spinButton_Click(object sender, RoutedEventArgs e)
        {
            isExpanded = !isExpanded;
            GoToState(true);
        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null && IsEnabled)
            {
                double buffer = this.m_mapContainer.GetBuffer(256);
                this.m_mapContainer.PanTo(this.m_mapContainer.CenPntLogCoor.X, this.m_mapContainer.CenPntLogCoor.Y + buffer);
            }
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null && IsEnabled)
            {
                double buffer = this.m_mapContainer.GetBuffer(128);
                this.m_mapContainer.PanTo(this.m_mapContainer.CenPntLogCoor.X - buffer, this.m_mapContainer.CenPntLogCoor.Y);
            }
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null && IsEnabled)
            {
                double buffer = this.m_mapContainer.GetBuffer(128);
                this.m_mapContainer.PanTo(this.m_mapContainer.CenPntLogCoor.X, this.m_mapContainer.CenPntLogCoor.Y - buffer);
            }
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_mapContainer != null && IsEnabled)
            {
                double buffer = this.m_mapContainer.GetBuffer(128);
                this.m_mapContainer.PanTo(this.m_mapContainer.CenPntLogCoor.X + buffer, this.m_mapContainer.CenPntLogCoor.Y);
            }
        }

        private void GoToState(bool useTransitions)
        {
            if (!panelDimensionsSet && borderHorizontalToolbar != null)
            {
                borderHorizontalToolbar.Width = borderHorizontalToolbar.ActualWidth;
                if (horizontalSplineDoubleKeyFrame != null)
                    horizontalSplineDoubleKeyFrame.Value = borderHorizontalToolbar.ActualWidth;
                panelDimensionsSet = true;
            }

            if (isExpanded)
            {
                VisualStateManager.GoToState(this, VSM_Expanded, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, VSM_Collapsed, useTransitions);
            }

            if (isMouseOver)
            {
                VisualStateManager.GoToState(this, VSM_MouseOver, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, VSM_Normal, useTransitions);
            }
        }
    }
}
