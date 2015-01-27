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
using ZDIMS.Drawing;
using ZDIMS.Map;
using System.Windows.Media.Imaging;
using ZDIMS.Interface;

namespace ZDIMSDemo.Controls
{
    public partial class MarkerEditer : BaseUserControl
    {
        private IMSMap m_mapContainer = null;//地图容器对象
        private IMSMark m_marker = null;
        /// <summary>
        /// 当前编辑的标注
        /// </summary>
        public IMSMark Marker
        {
            get { return m_marker; }
            set { m_marker = value; }
        }
        private MarkLayer m_markLayer = null;
        /// <summary>
        /// 地图容器
        /// </summary>
        public IMSMap MapContainer
        {
            get { return m_mapContainer; }
            set { m_mapContainer = value; }
        }
        public MarkerEditer()
        {
            InitializeComponent();
            this.dialog.OnClose += new RoutedEventHandler(OnClose);
        }
        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void image_marker1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.MapContainer == null)
            {
                MessageBox.Show("MapContainer属性为空，不能继续操作！");
                return;
            }
            if (this.m_markLayer == null)
            {
                this.m_markLayer = new MarkLayer();
                this.MapContainer.AddChild(this.m_markLayer);
            }
            Image targetimg = sender as Image;
            if (this.m_marker == null)
            {
                Image img = new Image();
                img.Source = targetimg.Source;
                ContextMenu menu = new ContextMenu();
                MenuItem del = new MenuItem();
                del.Header = "删除";
                del.Click += new RoutedEventHandler(del_Click);
                menu.Items.Add(del);
                MenuItem edit = new MenuItem();
                edit.Header = "编辑";
                edit.Click += new RoutedEventHandler(edit_Click);
                menu.Items.Add(edit);
                ContextMenuService.SetContextMenu((DependencyObject)img, menu);
                this.m_marker = new IMSMark(img);
                this.m_marker.CoordinateType = CoordinateType.Logic;
                this.m_marker.X = this.MapContainer.CenPntLogCoor.X;
                this.m_marker.Y = this.MapContainer.CenPntLogCoor.Y;
                this.textBox_markerx.Text = m_marker.X.ToString();
                this.textBox_markery.Text = m_marker.Y.ToString();
                this.m_marker.EnableDrag = true;
                this.m_markLayer.AddMark(this.m_marker);
                this.m_marker.MarkDragOverCallback = new MarkDragOverDelegate(onDragEnd);
            }
            else
            {
                ((Image)this.m_marker.MarkControl).Source = targetimg.Source;
            }
            this.image_marker.Source = targetimg.Source;
            e.Handled=true;
        }

        void edit_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        void del_Click(object sender, RoutedEventArgs e)
        {
            if (this.m_markLayer.Contains(this.m_marker))
            {
                this.m_markLayer.RemoveMark(this.m_marker);
                this.Close();
            }
        }
        private void onDragEnd(MarkBase obj)
        {
            this.textBox_markerx.Text=obj.X.ToString();
            this.textBox_markery.Text= obj.Y.ToString();

        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.MapContainer == null && this.Parent != null && this.Parent is IMSMap)
                this.MapContainer = this.Parent as IMSMap;
        }

        private void textBox_markerx_TextChanged(object sender, TextChangedEventArgs e)
        {
            string x=this.textBox_markerx.Text;
            string y = this.textBox_markery.Text;
            if (this.m_marker != null && x != "" && y != "")
            {
                try
                {
                    this.m_marker.X = Convert.ToDouble(x);
                    this.m_marker.Y = Convert.ToDouble(y);
                }
                catch
                {
                    MessageBox.Show("请输入正确的坐标值！");
                }
            }
        }
    }
}
