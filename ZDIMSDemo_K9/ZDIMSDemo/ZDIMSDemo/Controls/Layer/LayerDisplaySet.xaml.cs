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
using ZDIMS.BaseLib;
using ZDIMSDemo.Controls;
using ZDIMS.Map;
using ZDIMS.Interface;
using ZDIMS.Util;
using ZDIMSDemo.Controls.Layer;

namespace ZDIMSDemo.Controls.Layer
{
    public partial class LayerDisplaySet : BaseUserControl
    {
        private IMSCatalog m_catalog = null;//目录

        /// <summary>
        /// 树目录
        /// </summary>
        public IMSCatalog IMSCatalog
        {
            get { return m_catalog; }
            set { m_catalog = value; }
        }

        public LayerDisplaySet()
        {
            InitializeComponent();
            this.dialog.OnClose += new RoutedEventHandler(OnClose);
        }
        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_submit_Click(object sender, RoutedEventArgs e)
        {
            if (this.IMSCatalog.ActiveLayerObj == null)
            {
                MessageBox.Show("当前没有激活的地图图层，请激活！", "提示", MessageBoxButton.OK);
                return;
            }
            CSetLayerDisplayStyle style = new CSetLayerDisplayStyle();
            style.DisplayStyle = new CDisplayStyle[this.IMSCatalog.ActiveLayerObj.LayerObj.LayerAccessInfo.Length];
            for (int i = 0; i < this.IMSCatalog.ActiveLayerObj.LayerObj.LayerAccessInfo.Length; i++)
            {
                style.DisplayStyle[i] = new CDisplayStyle();
                style.DisplayStyle[i].DriverQuality = this.radioButton_img_high.IsChecked.Value ? 2 : 0;
                //style.DisplayStyle[i].FollowScale = this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].AnnSizeFixed = !this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].LinPenWidFixed = !this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].LinSizeFixed = !this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].PntPenWidFixed = !this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].PntSizeFixed = !this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].RegPenWidFixed = !this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].RegSizeFixed = !this.checkBox_followshow.IsChecked.Value;
                style.DisplayStyle[i].ShowCoordPnt = this.checkBox_showcoor.IsChecked.Value;
                style.DisplayStyle[i].SymbleShow = this.checkBox_origin.IsChecked.Value;
            }
            try
            {
                this.IMSCatalog.ActiveLayerObj.SetLayerDisplayStyle(style, OnSetStyle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void OnSetStyle(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                COperResult rlt = this.IMSCatalog.ActiveLayerObj.OnSetLayerDisplayStyle(e);
                if (rlt.OperResult)
                {
                    if (MessageBox.Show("操作成功，您可以更新地图以应用新的设置。\n点击【确定】立即关闭本窗口。", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        this.Close();
                }
                else
                    MessageBox.Show("操作失败。" + rlt.ErrorInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DialogPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
