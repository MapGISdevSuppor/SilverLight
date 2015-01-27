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
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace SilverlightDemo.Data
{
    public class MaticData
    {
        public string XLabel { get; set; }
        public double YValue { get; set; }
        public MaticData()
        {
            XLabel = "";
            YValue = 0;
        }
    }

    public class myData
    {
        public string PlaceName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public myData()
        {
            PlaceName = "";
            X = 0;
            Y = 0;
        }
    }

    //[XmlRoot("NewDataSet")]
    //public class datas : List<myData>
    //{
    //    List<myData> list = new List<myData>();
    //    myData data = new myData();
       
    //    public static object DataSource()
    //    {
         
    //            XmlSerializer s = new XmlSerializer(typeof(datas));
    //            return s.Deserialize(typeof(datas).Assembly.GetManifestResourceStream(typeof(datas).Namespace+".Data.myData.xml"));
    //        }
      
    //}
}
