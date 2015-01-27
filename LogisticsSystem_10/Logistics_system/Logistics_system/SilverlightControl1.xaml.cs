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
using ZDIMS.Interface;
using ZDIMS.Map;
using ZDIMS.BaseLib;
using Logistics_system;
namespace Logistics_system
{
    public partial class SilverlightControl1 : UserControl
    {
        public SilverlightControl1()
        {
            InitializeComponent();
        }

          TileLayer tl = null;
        VectorMapDoc vm = null;
        VectorLayer vl = null;
       
        public void show_tile(object sender, RoutedEventArgs e)
        {
            //if (vm != null)
            //    iMSMap1.RemoveChild(vm);
            //if (vl != null)
            //    iMSMap1.RemoveChild(vl);
            //if (tl == null)
            //{
            //    tl = new TileLayer();
            //    tl.AutoGetMapInfo = true;
            //    tl.HdfName = "whMap.HDF";
            //    tl.ServerAddress = "http://127.0.0.1/RelayHandlerSite/RelayHandler.ashx";
            //}
            //iMSMap1.AddChild(tl);
        }
        public void show_doc(object sender, RoutedEventArgs e)
        {
        //    if (tl != null)
        //        iMSMap1.RemoveChild(tl);
        //    if (vl != null)
        //        iMSMap1.RemoveChild(vl);
        //    if (vm == null)
        //    {
        //        vm = new VectorMapDoc();
        //        vm.MapDocName = "wh.Map";
        //        vm.ServerAddress = "http://127.0.0.1/RelayHandlerSite/RelayHandler.ashx";
        //        vm.AutoGetMapInfo = true;
        //    }
        //    iMSMap1.AddChild(vm);
        }
        public void show_layer(object sender, RoutedEventArgs e)
        {
            //if (tl != null)
            //    iMSMap1.RemoveChild(tl);
            //if (vm != null)
            //    iMSMap1.RemoveChild(vm);
            //if (vl == null)
            //{
            //    vl = new VectorLayer();
            //    vl.Name = "ss";
            //    COpenLayer openobj = new COpenLayer();
            //    CLayerAccessInfo LAinfo = new CLayerAccessInfo();
            //    CGdbInfo gdbinfo = new CGdbInfo();
            //    CLayerInfo linfo1 = new CLayerInfo();
            //    CLayerInfo linfo2 = new CLayerInfo();
            //    CLayerInfo linfo3 = new CLayerInfo();
            //    CLayerInfo linfo4 = new CLayerInfo();
            //    gdbinfo.GDBName = "world";
            //    //linfo1.LayerDataName = "水系.wp";
            //    //linfo1.LayerType = XClsType.SFeatureCls;
            //    //linfo2.LayerDataName = "中心线";
            //    //linfo2.LayerType = XClsType.SFeatureCls;
            //    //linfo3.LayerDataName = "文化教育.wt";
            //    //linfo3.LayerType = XClsType.SFeatureCls;
            //    linfo4.LayerDataName = "行政区.wp";
            //    linfo4.LayerType = XClsType.SFeatureCls;
            //    LAinfo.GdbInfo = gdbinfo;
            //    LAinfo.LayerInfoList = new CLayerInfo[1] { linfo4};
            //    openobj.LayerAccessInfo = new CLayerAccessInfo[1] { LAinfo };
            //    vl.LayerObj = openobj;
            //    vl.ServerAddress = "http://127.0.0.1/RelayHandlerSite/RelayHandler.ashx";
            //    vl.AutoGetMapInfo = true;
            //}

          
            //iMSMap1.AddChild(vl);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {

            DetailsForms f = new DetailsForms();
            f.Show();
            //VectorLayer f = this.iMSMap1.FindName("ss") as VectorLayer;
            //this.iMSMap1.RemoveChild(f);
            //this.iMSMap1.Refresh();
        }
    }
}
