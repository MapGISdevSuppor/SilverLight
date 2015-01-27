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
using System.Reflection;
using System.Collections.Generic;
using Logistics_system;
using Visifire.Charts;
using Visifire.Gauges;
using Visifire.Commons;
namespace Logistics_system
{
    public class getProperties
    {

        public List<string> getproperty<T>(T obj)
        {
            
            List<string> tmp=new List<string>();
            Type t = typeof(T);
            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                tmp.Add(pi.Name);

            }
            return tmp;
        }

        public DataPoint getDynamic<T>(T obj,string Xa,string Ya)
        {
            DataPoint tmp = new DataPoint();
            Type t = typeof(T);
            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Xa == pi.Name)
                {
                    //try
                    //{
                    //    Convert.ToDouble(pi.GetValue(obj, null));
                    //}
                    //catch
                    //{
                    //    MessageBox.Show("字段" + Xa + "无法统计，请重新选择！");
                    //    return null;
                    //}
                    tmp.AxisXLabel =Convert.ToString( pi.GetValue(obj, null)) ;
                }
                if (Ya == pi.Name)
                {
                    try
                    {
                        Convert.ToDouble(pi.GetValue(obj, null));
                    }
                    catch
                    {
                        MessageBox.Show("字段" + Ya + "无法统计，请重新选择！");
                        return null;
                    }
                    tmp.YValue= Convert.ToDouble(pi.GetValue(obj, null));
                }
            }
            return tmp;
        }
       


    }
}
