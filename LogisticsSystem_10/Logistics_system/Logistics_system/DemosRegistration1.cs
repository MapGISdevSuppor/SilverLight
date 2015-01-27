using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DevExpress.AgDataGrid;
using System.Collections.ObjectModel;
using Logistics_system;
namespace Logistics_system
{
    public class DemoItem
    {
        string name, description;
        Type userControlType;
        UserControl demoControl;
        FrameworkElement XAMLControl;
        IDemoItemOwner owner;

        public DemoItem(string name, string description, Type userControlType, IDemoItemOwner owner)
        {
            this.name = name;
            this.description = description;
            this.owner = owner;
            this.userControlType = userControlType;
        }
        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public override string ToString() { return Name; }
        UserControl DemoControl
        {
            get
            {
                if (demoControl == null)
                {
                    demoControl = userControlType.GetConstructor(new Type[] { }).Invoke(new Type[] { }) as UserControl;
                    if (demoControl is IDemoItem)
                    {
                        (demoControl as IDemoItem).Owner = owner;
                    }
                }
                return demoControl;
            }
        }
        IDemoItem Demo { get { return DemoControl as IDemoItem; } }
        public FrameworkElement ContentElement
        {
            get
            {
                if (Demo != null) return (FrameworkElement)Demo;
                return DemoControl;
            }
        }
        protected FrameworkElement CreateColoredTextBlockControl(string text, CodeLanguage language)
        {
            CodeBlockPresenter cb = new CodeBlockPresenter(language);
            TextBlock colorElem = cb.ToTextBlock(text);
            AgDataGridTextBox selectElem = new AgDataGridTextBox() { Text = text };
            selectElem.Foreground = new SolidColorBrush(Colors.Transparent);
            Grid g = new Grid();
            g.Children.Add(colorElem);
            g.Children.Add(selectElem);
            return g;
        }
        ScrollViewer CreateViewer(object content) { return new ScrollViewer() { Content = content, Style = owner.ScrollViewerStyle, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, Background = new SolidColorBrush(Colors.White), Padding = new Thickness(10, 10, 0, 0) }; }
        public FrameworkElement XAMLBlock
        {
            get
            {
                if (Demo == null) return null;
                if (XAMLControl != null) return XAMLControl;
                string res;
                ResourceLoader.GetEmbeddedResource(typeof(DemoItem).Assembly, Demo.XAMLBlock, out res);
                return XAMLControl = CreateViewer(CreateColoredTextBlockControl(res.Remove(0, 3).Replace("	", "    "), CodeLanguage.XAML));
            }
        }
        //public FrameworkElement CSBlock
        //{
        //    get
        //    {
        //        if (Demo == null) return null;
        //        if (CSControl != null) return CSControl;
        //        string res;
        //        ResourceLoader.GetEmbeddedResource(typeof(CarsData).Assembly.GetManifestResourceStream(typeof(CarsData).Namespace + "." + Demo.CSBlock), out res);
        //        return CSControl = CreateViewer(CreateColoredTextBlockControl(res.Remove(0, 3).Replace("	", "    "), CodeLanguage.CS));
        //    }
        //}
        public Panel OptionsElement
        {
            get
            {
                if (Demo != null && Demo.OptionsPanelTemplate != null)
                {
                    return (Panel)Demo.OptionsPanelTemplate.LoadContent();
                }
                return null;
            }
        }
    }
    interface IDemoItem
    {
        DataTemplate OptionsPanelTemplate { get; }
        IDemoItemOwner Owner { set; }
        string XAMLBlock { get; }
        string CSBlock { get; }
    }
    public class DemosList
    {
        static List<DemoItem> demos = new List<DemoItem>();
        static public void AddItem(string name, string description, Type userControlType, IDemoItemOwner owner)
        {
            demos.Add(new DemoItem(name, description, userControlType, owner));
        }
        static public ReadOnlyCollection<DemoItem> Demos { get { return demos.AsReadOnly(); } }
    }
}
