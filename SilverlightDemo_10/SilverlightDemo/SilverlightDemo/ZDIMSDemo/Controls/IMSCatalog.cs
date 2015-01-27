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
using ZDIMS.Map;
using System.Windows.Media.Imaging;
using ZDIMS.Util;
using ZDIMS.Interface;
using ZDIMSDemo.Controls;
using ZDIMS.BaseLib;

namespace ZDIMSDemo.Controls
{
    public class IMSCatalog : TreeView
    {
        private IMSMap m_mapContainer = null;//地图容器对象
        private TreeViewItem mapDocItem = null;//
        private TreeViewItem vLayerItem = null;
        public int  OnlyVecFlag=0;
        /// <summary>
        /// 当前激活的文档对象
        /// </summary>
        public VectorMapDoc ActiveMapDoc = null;
        /// <summary>
        /// 当前激活的图层对象
        /// </summary>
        public VectorLayer ActiveLayerObj = null;
        /// <summary>
        /// 地图容器对象
        /// </summary>
        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set
            {
                m_mapContainer = value;
                InitCatalogTree();
            }
        }
        ContextMenu GetContextMenu1(TreeViewItem tItem)
        {
            ContextMenu m_menu = new ContextMenu();
            m_menu.Tag = tItem;
            MenuItem item = new MenuItem() { Tag = tItem };
            item.Header = " 隐   藏";
            item.Click += new RoutedEventHandler(ContextMenuClick_Hidden);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 可   见";
            item.Click += new RoutedEventHandler(ContextMenuClick_Visible);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 编   辑";
            item.Click += new RoutedEventHandler(ContextMenuClick_Edit);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 查   询";
            item.Click += new RoutedEventHandler(ContextMenuClick_Select);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 激   活";
            item.Click += new RoutedEventHandler(ContextMenuClick_Activation);
            m_menu.Items.Add(item);
            return m_menu;
        }
        ContextMenu GetContextMenu2(TreeViewItem tItem)
        {
            ContextMenu m_menu = new ContextMenu();
            m_menu.Tag = tItem;
            MenuItem item = new MenuItem() { Tag = tItem };
            item.Header = " 隐   藏";
            item.Click += new RoutedEventHandler(ContextMenuClick_Hidden);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 可   见";
            item.Click += new RoutedEventHandler(ContextMenuClick_Visible);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 展   开";
            item.Click += new RoutedEventHandler(ContextMenuClick_Expend);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 折   叠";
            item.Click += new RoutedEventHandler(ContextMenuClick_Close);
            m_menu.Items.Add(item);
            return m_menu;
        }
        ContextMenu GetContextMenu3(TreeViewItem tItem)
        {
            ContextMenu m_menu = new ContextMenu();
            m_menu.Tag = tItem;
            MenuItem item = new MenuItem() { Tag = tItem };
            item.Header = " 展   开";
            item.Click += new RoutedEventHandler(ContextMenuClick_Expend);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 折   叠";
            item.Click += new RoutedEventHandler(ContextMenuClick_Close);
            m_menu.Items.Add(item);
            return m_menu;
        }
        ContextMenu GetContextMenu4(TreeViewItem tItem)
        {
            ContextMenu m_menu = new ContextMenu();
            m_menu.Tag = tItem;
            MenuItem item = new MenuItem() { Tag = tItem };
            item.Header = " 隐   藏";
            item.Click += new RoutedEventHandler(ContextMenuClick_Hidden);
            m_menu.Items.Add(item);
            item = new MenuItem() { Tag = tItem };
            item.Header = " 可   见";
            item.Click += new RoutedEventHandler(ContextMenuClick_Visible);
            m_menu.Items.Add(item);
            return m_menu;
        }
        /// <summary>
        /// 刷新树目录
        /// </summary>
        public void Refresh()
        {
            InitCatalogTree();
        }
        private void InitCatalogTree()
        {
            this.Items.Clear();
            if (m_mapContainer != null)
            {


                if (OnlyVecFlag == 1)
                {
                    StackPanel panel = new StackPanel();

                    TreeViewItem item = new TreeViewItem() { Header = panel };

                    //panel.Orientation = Orientation.Horizontal;
                    //panel.Children.Add(new Image()
                    //{
                    //    Source = new BitmapImage(new Uri("/images/tree/data.PNG", UriKind.Relative))
                    //});
                    //panel.Children.Add(new TextBlock()
                    //{
                    //    Text = "瓦片集",
                    //    Margin = new Thickness(2)
                    //});

                    //this.Items.Add(item);
                    //item.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                    //ContextMenuService.SetContextMenu(item, GetContextMenu3(item));


                    //for (int i = 0; i < m_mapContainer.TileLayerList.Length; i++)
                    //{
                    //    panel = new StackPanel();
                    //    panel.Orientation = Orientation.Horizontal;
                    //    Image img = new Image();
                    //    panel.Children.Add(img);
                    //    panel.Children.Add(new TextBlock()
                    //    {
                    //        Text = (m_mapContainer.TileLayerList.GetItemByIndex(i) as TileLayer).HdfName,
                    //        Margin = new Thickness(2)
                    //    });
                    //    item.Items.Add(new TreeViewItem() { Header = panel });
                    //    item.Tag = new LayerStateManager(this, m_mapContainer.TileLayerList.GetItemByIndex(i) as IMap, item, img, LayerTreeNodeType.ParentLayer);
                    //    ContextMenuService.SetContextMenu(item, GetContextMenu4(item));
                    //}

                    //panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    panel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri("/images/tree/data.PNG", UriKind.Relative))
                    });
                    panel.Children.Add(new TextBlock()
                    {
                        Text = "矢量图(文档)",
                        Margin = new Thickness(2)
                    });
                    mapDocItem = new TreeViewItem() { Header = panel };

                    this.Items.Add(mapDocItem);
                    mapDocItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                    ContextMenuService.SetContextMenu(mapDocItem, GetContextMenu3(mapDocItem));


                    //panel = new StackPanel();
                    //panel.Orientation = Orientation.Horizontal;
                    //panel.Children.Add(new Image()
                    //{
                    //    Source = new BitmapImage(new Uri("/images/tree/data.PNG", UriKind.Relative))
                    //});
                    //panel.Children.Add(new TextBlock()
                    //{
                    //    Text = "矢量图(图层)",
                    //    Margin = new Thickness(2)
                    //});
                    //vLayerItem = new TreeViewItem() { Header = panel };
                    //this.Items.Add(vLayerItem);
                    //vLayerItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                    //ContextMenuService.SetContextMenu(vLayerItem, GetContextMenu3(vLayerItem));


                    VectorMapDoc vectorMapDoc;
                    VectorLayer vectorLayer;
                    for (int i = 0; i < m_mapContainer.VectorLayerList.Length; i++)
                    {
                        if (m_mapContainer.VectorLayerList.GetItemByIndex(i) is VectorMapDoc)
                        {
                            vectorMapDoc = m_mapContainer.VectorLayerList.GetItemByIndex(i) as VectorMapDoc;
                            if (vectorMapDoc.LoadMapResult != null && vectorMapDoc.LoadMapResult.LayerCount > 0)
                                VectorMapDoc_LayerOpenSuccEvent(vectorMapDoc);
                            else
                                vectorMapDoc.LayerOpenSuccEvent += new LayerOpenSuccEventHandler(VectorMapDoc_LayerOpenSuccEvent);
                            if (this.ActiveMapDoc == null)
                                this.ActiveMapDoc = vectorMapDoc;
                        }
                        else
                        {
                            vectorLayer = m_mapContainer.VectorLayerList.GetItemByIndex(i) as VectorLayer;

                            panel = new StackPanel();
                            panel.Orientation = Orientation.Horizontal;
                            RadioButton vRadioButton = new RadioButton()
                            {
                                GroupName = "VectorLayerRadioButton",
                                Tag = vectorLayer
                            };
                            if (this.ActiveLayerObj == null)
                            {
                                this.ActiveLayerObj = vectorLayer;
                                vRadioButton.IsChecked = true;
                            }
                            vRadioButton.Checked += new RoutedEventHandler(OnChecked);
                            panel.Children.Add(vRadioButton);
                            Image img = new Image();
                            panel.Children.Add(img);

                            int s = vectorLayer.ServerAddress.IndexOf("//");
                            s = s < 0 ? 0 : s;
                            string str = vectorLayer.ServerAddress.Substring(s + 2);
                            int e = str.IndexOf("/");
                            e = e < 0 ? str.Length - 1 : e;
                            panel.Children.Add(new TextBlock()
                            {
                                Text = "位于:" + str.Substring(0, e),//vectorLayer.ServerAddress,
                                Margin = new Thickness(2)
                            });
                            TreeViewItem vlItem = new TreeViewItem() { Header = panel };
                            if (vLayerItem == null)
                            {
                                vLayerItem = new TreeViewItem() { Header = panel };
                            }
                            vLayerItem.Items.Add(vlItem);
                            vlItem.Tag = new LayerStateManager(this, vectorLayer, vlItem, img, LayerTreeNodeType.ParentLayer);

                            vlItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                            ContextMenuService.SetContextMenu(vlItem, GetContextMenu2(vlItem));


                            TreeViewItem layerItem;
                            RadioButton radioButton;
                            for (int j = 0; j < vectorLayer.LayerObj.LayerAccessInfo.Length; j++)
                            {
                                panel = new StackPanel();
                                panel.Orientation = Orientation.Horizontal;
                                img = new Image();
                                panel.Children.Add(img);
                                panel.Children.Add(new TextBlock()
                                {
                                    Text = vectorLayer.LayerObj.LayerAccessInfo[j].GdbInfo.GDBName,
                                    Margin = new Thickness(2)
                                });
                                layerItem = new TreeViewItem() { Header = panel };
                                vlItem.Items.Add(layerItem);
                                layerItem.Tag = new LayerStateManager(this, vectorLayer, layerItem, img, LayerTreeNodeType.GDBLayer);
                                layerItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                                ContextMenuService.SetContextMenu(layerItem, GetContextMenu2(layerItem));


                                for (int k = 0; k < vectorLayer.LayerObj.LayerAccessInfo[j].LayerInfoList.Length; k++)
                                {
                                    panel = new StackPanel();
                                    panel.Orientation = Orientation.Horizontal;
                                    radioButton = new RadioButton()
                                    {
                                        IsChecked = false,
                                        GroupName = "ActiveVectorLayerRadioButton" + i
                                    };
                                    panel.Children.Add(radioButton);
                                    img = new Image();
                                    img.Tag = radioButton;
                                    panel.Children.Add(img);
                                    panel.Children.Add(new TextBlock()
                                    {
                                        Text = vectorLayer.LayerObj.LayerAccessInfo[j].LayerInfoList[k].LayerDataName,
                                        Margin = new Thickness(2)
                                    });
                                    item = new TreeViewItem() { Header = panel };
                                    layerItem.Items.Add(item);
                                    item.Tag = new LayerStateManager(this, vectorLayer, item, img) { LayerRadioBtn = vRadioButton };
                                    radioButton.Tag = item.Tag;
                                    item.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                                    ContextMenuService.SetContextMenu(item, GetContextMenu1(item));
                                }
                            }
                        }
                    }
                   
                }
                else {
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    panel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri("/images/tree/data.PNG", UriKind.Relative))
                    });
                    panel.Children.Add(new TextBlock()
                    {
                        Text = "瓦片集",
                        Margin = new Thickness(2)
                    });
                    TreeViewItem item = new TreeViewItem() { Header = panel };
                    this.Items.Add(item);
                    item.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                    ContextMenuService.SetContextMenu(item, GetContextMenu3(item));


                    for (int i = 0; i < m_mapContainer.TileLayerList.Length; i++)
                    {
                        panel = new StackPanel();
                        panel.Orientation = Orientation.Horizontal;
                        Image img = new Image();
                        panel.Children.Add(img);
                        panel.Children.Add(new TextBlock()
                        {
                            Text = (m_mapContainer.TileLayerList.GetItemByIndex(i) as TileLayer).HdfName,
                            Margin = new Thickness(2)
                        });
                        item.Items.Add(new TreeViewItem() { Header = panel });
                        item.Tag = new LayerStateManager(this, m_mapContainer.TileLayerList.GetItemByIndex(i) as IMap, item, img, LayerTreeNodeType.ParentLayer);
                        ContextMenuService.SetContextMenu(item, GetContextMenu4(item));
                    }

                    panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    panel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri("/images/tree/data.PNG", UriKind.Relative))
                    });
                    panel.Children.Add(new TextBlock()
                    {
                        Text = "矢量图(文档)",
                        Margin = new Thickness(2)
                    });
                    mapDocItem = new TreeViewItem() { Header = panel };
                    this.Items.Add(mapDocItem);
                    mapDocItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                    ContextMenuService.SetContextMenu(mapDocItem, GetContextMenu3(mapDocItem));


                    panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    panel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri("/images/tree/data.PNG", UriKind.Relative))
                    });
                    panel.Children.Add(new TextBlock()
                    {
                        Text = "矢量图(图层)",
                        Margin = new Thickness(2)
                    });
                    vLayerItem = new TreeViewItem() { Header = panel };
                    this.Items.Add(vLayerItem);
                    vLayerItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                    ContextMenuService.SetContextMenu(vLayerItem, GetContextMenu3(vLayerItem));


                    VectorMapDoc vectorMapDoc;
                    VectorLayer vectorLayer;
                    for (int i = 0; i < m_mapContainer.VectorLayerList.Length; i++)
                    {
                        if (m_mapContainer.VectorLayerList.GetItemByIndex(i) is VectorMapDoc)
                        {
                            vectorMapDoc = m_mapContainer.VectorLayerList.GetItemByIndex(i) as VectorMapDoc;
                            if (vectorMapDoc.LoadMapResult != null && vectorMapDoc.LoadMapResult.LayerCount > 0)
                                VectorMapDoc_LayerOpenSuccEvent(vectorMapDoc);
                            else
                                vectorMapDoc.LayerOpenSuccEvent += new LayerOpenSuccEventHandler(VectorMapDoc_LayerOpenSuccEvent);
                            if (this.ActiveMapDoc == null)
                                this.ActiveMapDoc = vectorMapDoc;
                        }
                        else
                        {
                            vectorLayer = m_mapContainer.VectorLayerList.GetItemByIndex(i) as VectorLayer;

                            panel = new StackPanel();
                            panel.Orientation = Orientation.Horizontal;
                            RadioButton vRadioButton = new RadioButton()
                            {
                                GroupName = "VectorLayerRadioButton",
                                Tag = vectorLayer
                            };
                            if (this.ActiveLayerObj == null)
                            {
                                this.ActiveLayerObj = vectorLayer;
                                vRadioButton.IsChecked = true;
                            }
                            vRadioButton.Checked += new RoutedEventHandler(OnChecked);
                            panel.Children.Add(vRadioButton);
                            Image img = new Image();
                            panel.Children.Add(img);

                            int s = vectorLayer.ServerAddress.IndexOf("//");
                            s = s < 0 ? 0 : s;
                            string str = vectorLayer.ServerAddress.Substring(s + 2);
                            int e = str.IndexOf("/");
                            e = e < 0 ? str.Length - 1 : e;
                            panel.Children.Add(new TextBlock()
                            {
                                Text = "位于:" + str.Substring(0, e),//vectorLayer.ServerAddress,
                                Margin = new Thickness(2)
                            });
                            TreeViewItem vlItem = new TreeViewItem() { Header = panel };
                            vLayerItem.Items.Add(vlItem);
                            vlItem.Tag = new LayerStateManager(this, vectorLayer, vlItem, img, LayerTreeNodeType.ParentLayer);
                            vlItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                            ContextMenuService.SetContextMenu(vlItem, GetContextMenu2(vlItem));


                            TreeViewItem layerItem;
                            RadioButton radioButton;
                            for (int j = 0; j < vectorLayer.LayerObj.LayerAccessInfo.Length; j++)
                            {
                                panel = new StackPanel();
                                panel.Orientation = Orientation.Horizontal;
                                img = new Image();
                                panel.Children.Add(img);
                                panel.Children.Add(new TextBlock()
                                {
                                    Text = vectorLayer.LayerObj.LayerAccessInfo[j].GdbInfo.GDBName,
                                    Margin = new Thickness(2)
                                });
                                layerItem = new TreeViewItem() { Header = panel };
                                vlItem.Items.Add(layerItem);
                                layerItem.Tag = new LayerStateManager(this, vectorLayer, layerItem, img, LayerTreeNodeType.GDBLayer);
                                layerItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                                ContextMenuService.SetContextMenu(layerItem, GetContextMenu2(layerItem));


                                for (int k = 0; k < vectorLayer.LayerObj.LayerAccessInfo[j].LayerInfoList.Length; k++)
                                {
                                    panel = new StackPanel();
                                    panel.Orientation = Orientation.Horizontal;
                                    radioButton = new RadioButton()
                                    {
                                        IsChecked = false,
                                        GroupName = "ActiveVectorLayerRadioButton" + i
                                    };
                                    panel.Children.Add(radioButton);
                                    img = new Image();
                                    img.Tag = radioButton;
                                    panel.Children.Add(img);
                                    panel.Children.Add(new TextBlock()
                                    {
                                        Text = vectorLayer.LayerObj.LayerAccessInfo[j].LayerInfoList[k].LayerDataName,
                                        Margin = new Thickness(2)
                                    });
                                    item = new TreeViewItem() { Header = panel };
                                    layerItem.Items.Add(item);
                                    item.Tag = new LayerStateManager(this, vectorLayer, item, img) { LayerRadioBtn = vRadioButton };
                                    radioButton.Tag = item.Tag;
                                    item.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                                    ContextMenuService.SetContextMenu(item, GetContextMenu1(item));
                                }
                            }
                        }
                    }
                
                
                }
            }
        }
        private void VectorMapDoc_LayerOpenSuccEvent(IVector vLayer)
        {
            if (vLayer is VectorMapDoc)
            {
                VectorMapDoc vectorMapDoc = vLayer as VectorMapDoc;

                vectorMapDoc.LayerOpenSuccEvent -= new LayerOpenSuccEventHandler(VectorMapDoc_LayerOpenSuccEvent);
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                RadioButton vRadioButton = new RadioButton()
                {
                    GroupName = "VectorMapDocRadioButton",
                    Tag = vectorMapDoc
                };
                if (this.ActiveMapDoc != null)
                {
                    if (this.ActiveMapDoc.Equals(vLayer))
                        vRadioButton.IsChecked = true;
                }
                vRadioButton.Checked += new RoutedEventHandler(OnChecked);
                panel.Children.Add(vRadioButton);
                Image img = new Image();
                panel.Children.Add(img);
                panel.Children.Add(new TextBlock()
                {
                    Text = vectorMapDoc.MapDocName,
                    Margin = new Thickness(2)
                });
                TreeViewItem item = new TreeViewItem() { Header = panel };
                mapDocItem.Items.Add(item);
                item.Tag = new LayerStateManager(this, vectorMapDoc, item, img, LayerTreeNodeType.ParentLayer);
                item.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                ContextMenuService.SetContextMenu(item, GetContextMenu2(item));

                TreeViewItem layerItem;
                RadioButton radioButton;
                int index = m_mapContainer.VectorLayerList.IndexOf(vectorMapDoc);
                for (int j = 0; j < vectorMapDoc.LoadMapResult.LayerCount-1; j++)
                {
                    panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    radioButton = new RadioButton()
                    {
                        IsChecked = false,
                        GroupName = "ActiveVectorMapDocRadioButton" + index
                    };
                    panel.Children.Add(radioButton);
                    img = new Image();
                    img.Tag = radioButton;
                    panel.Children.Add(img);
                    panel.Children.Add(new TextBlock()
                    {
                        Text = vectorMapDoc.GetLayerInfo(j).LayerDataName,
                        Margin = new Thickness(2)
                    });
                    layerItem = new TreeViewItem() { Header = panel };
                    item.Items.Add(layerItem);
                    layerItem.Tag = new LayerStateManager(this, vectorMapDoc, layerItem, img,
                        LayerTreeNodeType.ChildLayer, GetLayerStatus(vectorMapDoc.GetMapLayerInfo(j).LayerStatus))
                        {
                            LayerRadioBtn = vRadioButton
                        };
                    layerItem.MouseRightButtonDown += new MouseButtonEventHandler(OnMouseRightButtonDown);
                    ContextMenuService.SetContextMenu(layerItem, GetContextMenu1(layerItem));
                    radioButton.Tag = layerItem.Tag;
                }
            }
        }
        /// <summary>
        /// 获取转换图层状态
        /// </summary>
        public LayerState GetLayerStatus(EnumLayerStatus state)
        {
            switch (state)
            {
                case EnumLayerStatus.Invisiable:
                    return LayerState.Hidden;
                case EnumLayerStatus.Editable:
                    return LayerState.Edit;
                case EnumLayerStatus.Selectable:
                    return LayerState.Select;
                default:
                    return LayerState.Visible;
            }
        }
        internal void OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton rb = sender as RadioButton;
                if ((bool)rb.IsChecked)
                {
                    if (rb.Tag is VectorMapDoc)
                    {
                        this.ActiveMapDoc = rb.Tag as VectorMapDoc;
                    }
                    else if (rb.Tag is VectorLayer)
                    {
                        this.ActiveLayerObj = rb.Tag as VectorLayer;
                    }
                }
            }
        }
        public void AddTileNode(TileLayer tLayer)
        {
            //TreeViewItem item = new TreeViewItem() { Header = panel };
            //this.Items.Add(item);
            //for (int i = 0; i < m_mapContainer.TileLayerList.Length; i++)
            //{
            //    panel = new StackPanel();
            //    panel.Orientation = Orientation.Horizontal;
            //    Image img = new Image();
            //    panel.Children.Add(img);
            //    panel.Children.Add(new TextBlock()
            //    {
            //        Text = (m_mapContainer.TileLayerList.GetItemByIndex(i) as TileLayer).HdfName,
            //        Margin = new Thickness(2)
            //    });
            //    item.Items.Add(new TreeViewItem() { Header = panel });
            //    item.Tag = new LayerStateManager(this, m_mapContainer.TileLayerList.GetItemByIndex(i) as IMap, item, img, LayerTreeNodeType.ParentLayer);
            //}   
        }
        public void AddVectorNode(IVector vLayer)
        {

        }
        void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem)
            {
                ((TreeViewItem)(sender)).IsSelected = true;
            }
        }
        void ContextMenuClick_Expend(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Tag is TreeViewItem)
                    (item.Tag as TreeViewItem).IsExpanded = true;
            }
        }
        void ContextMenuClick_Close(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Tag is TreeViewItem)
                    (item.Tag as TreeViewItem).IsExpanded = false;
            }
        }
        void ContextMenuClick_Hidden(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Tag is TreeViewItem)
                {
                    ((item.Tag as TreeViewItem).Tag as LayerStateManager).ChangeState(LayerState.Hidden);
                }
            }
        }
        void ContextMenuClick_Visible(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Tag is TreeViewItem)
                {
                    ((item.Tag as TreeViewItem).Tag as LayerStateManager).ChangeState(LayerState.Visible);
                }
            }
        }
        void ContextMenuClick_Edit(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Tag is TreeViewItem)
                {
                    ((item.Tag as TreeViewItem).Tag as LayerStateManager).ChangeState(LayerState.Edit);
                }
            }
        }
        void ContextMenuClick_Select(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Tag is TreeViewItem)
                {
                    ((item.Tag as TreeViewItem).Tag as LayerStateManager).ChangeState(LayerState.Select);
                }
            }
        }
        void ContextMenuClick_Activation(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Tag is TreeViewItem)
                {
                    ((item.Tag as TreeViewItem).Tag as LayerStateManager).ChangeState(LayerState.Activation);
                }
            }
        }
    }
}
