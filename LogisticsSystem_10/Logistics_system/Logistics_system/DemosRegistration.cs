
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DevExpress.AgDataGrid.Data;
using DevExpress.AgDataGrid.Internal;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using DevExpress.AgDataGrid.Data.Helpers;
using System.Text;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Windows.Browser;
using System.Windows.Data;
using System.Globalization;
using System.Linq;

namespace Logistics_system
{
   
        public partial class AgToggleButton :  ContentControl
        {
            
            bool isChecked;
            public AgToggleButton()
                : base()
            {
                DefaultStyleKey = typeof(AgToggleButton);
                this.isChecked = true;
                MouseLeftButtonDown += new MouseButtonEventHandler(AgToggleButton_MouseLeftButtonDown);
                MouseLeftButtonUp += new MouseButtonEventHandler(AgButton_MouseLeftButtonUp);
            }

            void AgToggleButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                
                e.Handled = true;
            }
            public event RoutedEventHandler Click;
            void AgButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                IsChecked = !IsChecked;
                if (Click != null) Click(this, new RoutedEventArgs());
            }
            public bool IsChecked
            {
                get { return isChecked; }
                set
                {
                    if (value == isChecked) return;
                    isChecked = value;
                    OnStateChanged();
                }
            }
            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                OnStateChanged();
            }

            protected virtual void OnStateChanged()
            {
                FrameworkElement elem = GetTemplateChild("Checked") as FrameworkElement;
                if (elem == null) return;
                elem.Opacity = IsChecked ? 1 : 0;
                SolidColorBrush txt = GetTemplateChild("TextColor") as SolidColorBrush;
                if (txt == null) return;
                txt.Color = IsChecked ? Colors.White : Colors.Black;
            }

        }

        public class FlowStackPanel : Panel
        {
            Dictionary<UIElement, Rect> calculatedChildBoundsList = new Dictionary<UIElement, Rect>();
            Size calculatedSize;

            protected override Size ArrangeOverride(Size finalSize)
            {
                foreach (UIElement child in calculatedChildBoundsList.Keys)
                    child.Arrange(calculatedChildBoundsList[child]);
                return finalSize;
            }
            protected override Size MeasureOverride(Size availableSize)
            {
                foreach (UIElement child in Children)
                    child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                CalcLayout(availableSize);
                return this.calculatedSize;
            }
            private void CalcLayout(Size availableSize)
            {
                calculatedChildBoundsList.Clear();
                calculatedSize = new Size();
                double rowHeight = 0, x = 0, xMax = 0, y = 0;
                int rowFirstChildIndex = 0;
                for (int i = 0; i < Children.Count; i++)
                {
                    UIElement child = Children[i];
                    if (child.Visibility != Visibility.Visible) continue;
                    Size childSize = child.DesiredSize;
                    if (childSize.Width == 0 || childSize.Height == 0) continue;
                    if (i > rowFirstChildIndex && x + childSize.Width > availableSize.Width && availableSize.Width != 0)
                    {
                        CenterRowChildrenVertically(rowFirstChildIndex, i - 1, rowHeight);
                        x = 0;
                        y += rowHeight;
                        rowHeight = 0;
                        rowFirstChildIndex = i;
                    }
                    if (i > rowFirstChildIndex)
                        x += 0;
                    calculatedChildBoundsList.Add(child, new Rect(x, y, childSize.Width, childSize.Height));
                    x += childSize.Width;
                    xMax = Math.Max(xMax, x);
                    rowHeight = Math.Max(rowHeight, childSize.Height);
                }
                if (Children.Count > 0)
                    CenterRowChildrenVertically(rowFirstChildIndex, Children.Count - 1, rowHeight);
                calculatedSize = new Size(xMax, y + rowHeight);
            }
            void CenterRowChildrenVertically(int firstChildIndex, int lastChildIndex, double rowHeight)
            {
                for (int i = firstChildIndex; i <= lastChildIndex; i++)
                {
                    UIElement child = Children[i];
                    if (!calculatedChildBoundsList.ContainsKey(child)) continue;
                    Rect r = calculatedChildBoundsList[child];
                    r = new Rect(r.Left, r.Top + (rowHeight - r.Height) / 2, r.Width, r.Height);
                    calculatedChildBoundsList[child] = r;
                }
            }
        }

        public class DateToLongDateStringConverter : IValueConverter
        {
            public string DateToString(DateTime date)
            {
                string[] Months = { "January", "February", "Marth", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                return date.Day.ToString() + "th of " + Months[date.Month - 1] + " in " + date.Year.ToString();
            }
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is DateTime)
                {
                    return DateToString((DateTime)value);
                }
                else
                {
                    return value;
                }
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class CurrencyToStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return string.Format("${0}", value);
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
        public class AgToolTip : ContentControl
        {
        }
        public class AgPreviewToolTip : ContentControl
        {
        }
    
}
