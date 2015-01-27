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
using System.Windows.Browser;
using System.IO;
using System.Xml.Linq;

namespace SilverlightDemo
{
    public partial class MainPage : UserControl
    {
        private List<Category> categoryList;
        private CategoryItem selectedItem;
        private Category category;
        private string xmlFile;
        //private string _target;
        private ListBox selectList;
        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(string fileName)
            : this()
        {
            Uri uri = HtmlPage.Document.DocumentUri;
            string absolutePath = uri.AbsolutePath;
            string htmlPageName = absolutePath.Substring(absolutePath.LastIndexOf('/') + 1);
            string absoluteUri = uri.AbsoluteUri;
            string url = absoluteUri.Substring(0, absoluteUri.IndexOf(htmlPageName));
            this.xmlFile = url + fileName;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(this.client_OpenReadCompleted);

            Uri uri = new Uri(this.xmlFile, UriKind.RelativeOrAbsolute);

            base.Dispatcher.BeginInvoke(delegate
            {
                client.OpenReadAsync(uri);
            });

        }

        private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                XDocument document = null;
                using (Stream stream = e.Result)
                {
                    document = XDocument.Load(stream);
                }
                this.categoryList = document.Root.Elements("Category").Select<XElement, Category>(delegate(XElement f)
                {
                    if (string.IsNullOrEmpty((string)f.Element("name")))
                    {
                        return null;
                    }

                    return new Category { Name = (string)f.Element("name"), CategoryItems = f.Elements("items").Elements<XElement>("item").Select<XElement, CategoryItem>(((Func<XElement, CategoryItem>)(o => new CategoryItem { ID = (string)o.Element("id"), XAML = (string)o.Element("xaml"), Source = (string)o.Element("source"), Code = (string)o.Element("code"), Explain = (string)o.Element("explain") }))).ToArray<CategoryItem>() };
                }).Where(ca => ca != null).ToList();

                this.expand.ItemsSource = categoryList;

            }
        }

        private void expand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectList != null)
                selectList.SelectedIndex = -1;
        }

        private void listItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.mapFrameBorder.BorderBrush = tabContentBorderBrush;
                selectList = (ListBox)sender;
                category = selectList.Tag as Category;
                selectedItem = e.AddedItems[0] as CategoryItem;
                string[] paths = selectedItem.XAML.Split(new char[] { '.' });
                string page = string.Concat("/", paths[0] + "/" + paths[1], ".xaml");
                ContentFrame.Navigate(new Uri(page, UriKind.Relative));
                this.tabPanel.SelectedIndex = 0;
                this.SampleCaption.Text = selectedItem.ID;
            }
        }



        private void tabPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.tabPanel != null) && (this.selectedItem != null))
            {
                switch (this.tabPanel.SelectedIndex)
                {
                    case 1:
                        this.txtXaml.SourceCode = string.Empty;
                        this.sourceViewer(this.selectedItem.Source);
                        // this.copyToClipboard.Visibility = Visibility.Visible;
                        return;
                    case 2:
                        this.txtSrc.SourceCode = string.Empty;
                        this.sourceViewer(this.selectedItem.Code);
                        // this.copyToClipboard.Visibility = Visibility.Visible;
                        return;
                    case 3:
                        this.txtSrcExplain.Text = string.Empty;
                        this.sourceViewer(this.selectedItem.Explain);
                        // this.copyToClipboard.Visibility = Visibility.Visible;
                        return;
                }
                // this.copyToClipboard.Visibility = Visibility.Collapsed;
            }
        }

        private void sourceViewer(string srcFile)
        {
            string absolutePath = HtmlPage.Document.DocumentUri.AbsolutePath;
            string subAbsolutePath = absolutePath.Substring(0, absolutePath.LastIndexOf('/'));
            string url = HtmlPage.Document.DocumentUri.AbsoluteUri.Substring(0, HtmlPage.Document.DocumentUri.AbsoluteUri.IndexOf(HtmlPage.Document.DocumentUri.AbsolutePath)) + subAbsolutePath + "/" + srcFile;
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(this.sourceView_OpenReadCompleted);
            client.OpenReadAsync(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        private void sourceView_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string str = null;
                using (Stream stream = e.Result)
                {
                    str = new StreamReader(stream).ReadToEnd();
                }
                switch (this.tabPanel.SelectedIndex)
                {
                    case 1:
                        this.txtXaml.SourceCode = str;
                        break;

                    case 2:
                        this.txtSrc.SourceCode = str;
                        break;

                    case 3:
                        this.txtSrcExplain.Text = str;
                        break;
                }
            }
        }


    }
}
