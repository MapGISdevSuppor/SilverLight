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
using ZDIMS.Map;
using ZDIMS.BaseLib;

namespace ZDIMSDemo.Controls.Layer
{
    public partial class LayerConditionInput : BaseUserControl
    {
        VectorLayer activeLayer = null;
        LayerDataViewer m_layerDataViewer = null;
        SelectionType m_selectionType = SelectionType.Condition;
        Object _geoObj = null;
        public LayerConditionInput(LayerDataViewer layerDataViewer)
        {
            m_layerDataViewer = layerDataViewer;
            InitializeComponent();
            dialogPanel1.OnClose += new RoutedEventHandler(Close);
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        public object QueryGeoObj { set { _geoObj = value; } }
        /// <summary>
        /// 查询类型
        /// </summary>
        public SelectionType SelectionType
        {
            set { m_selectionType = value; }
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit(object sender, RoutedEventArgs e)
        {
            if (this.conditionText.Text.Replace(" ", "").Length == 0)
            {
                MessageBox.Show("请输入查询条件。", "提示", MessageBoxButton.OK);
                return;
            }
            if (m_layerDataViewer != null)
                m_layerDataViewer.Select(_geoObj, m_selectionType, 0, this.conditionText.Text);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearText(object sender, RoutedEventArgs e)
        {
            conditionText.Text = "";
        }
        /// <summary>
        /// 输入条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CnBtnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                this.conditionText.Text += " " + (sender as Button).Content.ToString();
                this.conditionText.Focus();
            }
        }
        private void layerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int gdbIndex = gdbList.SelectedIndex;
            int layerIndex = layerList.SelectedIndex;
            if (gdbIndex >= 0 && gdbIndex < activeLayer.LayerObj.LayerAccessInfo.Length &&
                layerIndex >= 0 && layerIndex < activeLayer.LayerObj.LayerAccessInfo[gdbIndex].LayerInfoList.Length)
            {
                CSetLayerIndex obj = new CSetLayerIndex();
                obj.GdbIndex = gdbIndex;
                obj.LayerIndex = layerIndex;
                activeLayer.GetLayerAttStruct(obj, new UploadStringCompletedEventHandler(GetAttStructCallBack));
            }
        }
        private void GetAttStructCallBack(object sender, UploadStringCompletedEventArgs e)
        {
            CAttStruct attStru = activeLayer.OnGetLayerAttStruct(e);
            fieldList.Items.Clear();
            for (int i = 0; i < attStru.FldName.Length; i++)
            {
                fieldList.Items.Add(attStru.FldName[i]);
            }
        }
        private void fieldList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.fieldList.SelectedValue != null)
                this.conditionText.Text += this.fieldList.SelectedValue.ToString();
        }

        private void gdbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = gdbList.SelectedIndex;
            if (index >= 0 && index < activeLayer.LayerObj.LayerAccessInfo.Length)
            {
                layerList.Items.Clear();
                for (int i = 0; i < activeLayer.LayerObj.LayerAccessInfo[index].LayerInfoList.Length; i++)
                {
                    layerList.Items.Add(activeLayer.LayerObj.LayerAccessInfo[index].LayerInfoList[i].LayerDataName);
                }
            }
        }

        private void gdbList_DropDownOpened(object sender, EventArgs e)
        {
            if (m_layerDataViewer != null && m_layerDataViewer.IMSCatalog.ActiveLayerObj == null)
            {
                MessageBox.Show("请激活一个矢量的图层!", "提示", MessageBoxButton.OK);
                return;
            }
            if (m_layerDataViewer != null &&
                (!m_layerDataViewer.IMSCatalog.ActiveLayerObj.Equals(activeLayer) || gdbList.Items.Count <= 0))
            {
                activeLayer = m_layerDataViewer.IMSCatalog.ActiveLayerObj;
                for (int i = 0; i < activeLayer.LayerObj.LayerAccessInfo.Length; i++)
                {
                    gdbList.Items.Add(activeLayer.LayerObj.LayerAccessInfo[i].GdbInfo.GDBSvrName + "_" + activeLayer.LayerObj.LayerAccessInfo[i].GdbInfo.GDBName + "_" + i);
                }
            }
        }
    }
}
