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
using ZDIMS.Map;
using System.Collections.ObjectModel;

namespace ZDIMSDemo.Controls.MapDoc
{
    public partial class ConditionInput : BaseUserControl
    {
        VectorMapDoc activeMapDoc = null;
        MapDocDataViewer m_mapDocDataViewer = null;
        SelectionType m_selectionType = SelectionType.Condition;
        Object _geoObj = null;
        public ConditionInput(MapDocDataViewer mapDocDataViewer)
        {
            m_mapDocDataViewer = mapDocDataViewer;
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
            if (m_mapDocDataViewer != null)
                m_mapDocDataViewer.Select(_geoObj, m_selectionType, 0, this.conditionText.Text);
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

        private void layerList_DropDownOpened(object sender, EventArgs e)
        {
            if (m_mapDocDataViewer != null && m_mapDocDataViewer.IMSCatalog.ActiveMapDoc == null)
            {
                MessageBox.Show("请激活一个文档图层!", "提示", MessageBoxButton.OK);
                return;
            }
            if (m_mapDocDataViewer != null &&
                (!m_mapDocDataViewer.IMSCatalog.ActiveMapDoc.Equals(activeMapDoc) || layerList.Items.Count <= 0))
            {
                activeMapDoc = m_mapDocDataViewer.IMSCatalog.ActiveMapDoc;
                if (activeMapDoc.LoadMapResult == null || activeMapDoc.LoadMapResult.LayerAccessInfo.LayerAccessInfo.Length == 0)
                    return;
                int layerNum = activeMapDoc.LoadMapResult.LayerCount;
                for (int i = 0; i < layerNum; i++)
                {
                    layerList.Items.Add(activeMapDoc.GetLayerInfo(i).LayerDataName);//new FontFamily(activeMapDoc.GetLayerInfo(i).LayerDataName));                    
                }
            }
        }
        private void layerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = layerList.SelectedIndex;
            if (index >= 0 && index < activeMapDoc.LoadMapResult.LayerCount)
            {
                CAttStruct curLayerAttStru = activeMapDoc.LoadMapResult.LayerAttStruct[index];
                if (curLayerAttStru != null)
                {
                    fieldList.Items.Clear();
                    for (int i = 0; i < curLayerAttStru.FldName.Length; i++)
                    {
                        fieldList.Items.Add(curLayerAttStru.FldName[i]);//new FontFamily(curLayerAttStru.FldName[i]));
                    }
                }
                else
                {
                    activeMapDoc.GetMapLayerAttStruct(index, new UploadStringCompletedEventHandler(GetAttStructCallBack));
                }
            }
        }
        private void GetAttStructCallBack(object sender, UploadStringCompletedEventArgs e)
        {
            CAttStruct curLayerAttStru = activeMapDoc.OnGetMapLayerAttStruct(e);
            if (curLayerAttStru != null)
            {
                for (int i = 0; i < curLayerAttStru.FldName.Length; i++)
                {
                    fieldList.Items.Add(curLayerAttStru.FldName[i]);//new FontFamily(curLayerAttStru.FldName[i]));
                }
            }
        }
        private void fieldList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.fieldList.SelectedValue != null)
                this.conditionText.Text += this.fieldList.SelectedValue.ToString();            
        }
    }
}
