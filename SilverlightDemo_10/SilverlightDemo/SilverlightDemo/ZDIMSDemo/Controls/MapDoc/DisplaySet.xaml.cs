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

namespace ZDIMSDemo.Controls.MapDoc
{
    public partial class DisplaySet : BaseUserControl
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

        public DisplaySet()
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
            if (this.IMSCatalog.ActiveMapDoc == null)
            {
                MessageBox.Show("当前没有激活的地图文档，请激活！","提示",MessageBoxButton.OK);
                return;
            }
            CSetMapDispStyle style = new CSetMapDispStyle();
            style.DispStyle = new CDisplayStyle();
            style.DispStyle.DriverQuality = this.radioButton_img_high.IsChecked.Value ? 2 : 0;
            //style.DispStyle.FollowScale = this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.AnnSizeFixed = !this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.LinPenWidFixed = !this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.LinSizeFixed = !this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.PntPenWidFixed = !this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.PntSizeFixed = !this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.RegPenWidFixed = !this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.RegSizeFixed = !this.checkBox_followshow.IsChecked.Value;
            style.DispStyle.ShowCoordPnt = this.checkBox_showcoor.IsChecked.Value;
            style.DispStyle.SymbleShow = this.checkBox_origin.IsChecked.Value;
            try
            {
                this.IMSCatalog.ActiveMapDoc.SetMapDispStyle(style, OnSetStyle);
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
                COperResult rlt = this.IMSCatalog.ActiveMapDoc.OnSetMapDispStyle(e);
                if (rlt.OperResult)
                {
                    if (MessageBox.Show("操作成功，您可以更新地图以应用新的设置。\n点击【确定】立即关闭本窗口。", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        this.Close();
                }
                else
                    MessageBox.Show("操作失败。"+rlt.ErrorInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public override void Show()
        {
            if(GetMapStyle())
                base.Show();
        }

        public override void ShowAsModal()
        {
            if(GetMapStyle())
                base.ShowAsModal();
        }

        private bool GetMapStyle()
        {
            if (this.IMSCatalog.ActiveMapDoc == null)
            {
                MessageBox.Show("当前没有激活的地图文档，请激活！", "提示", MessageBoxButton.OK);
                this.Close();
                return false;
            }
            try
            {
                this.IMSCatalog.ActiveMapDoc.GetMapDispStyle(OnGetMapStyle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }
        private void OnGetMapStyle(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                CDisplayStyle style = this.IMSCatalog.ActiveMapDoc.OnGetMapDispStyle(e);
                this.checkBox_followshow.IsChecked = !style.AnnSizeFixed;
                this.checkBox_origin.IsChecked = style.SymbleShow;
                this.checkBox_showcoor.IsChecked = style.ShowCoordPnt;
                if (style.DriverQuality == 2)
                    this.radioButton_img_high.IsChecked = true;
                else
                    this.radioButton_img_low.IsChecked = true;
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
