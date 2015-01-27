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
using System.Collections.Generic;
namespace Logistics_system.chart1
{
    public class dataBing
    {
        public static List<string> financialXStr()
        {
            List<string> tmp = new List<string>();
            tmp.Add("门店ID");
            tmp.Add("门店名称");
            tmp.Add("总额");
            tmp.Add("销售额");
            tmp.Add("进货资金");
            return tmp;
        }


        public static List<string> financialYStr1()
        {
            List<string> tmp = new List<string>();
            //tmp.Add("门店ID");
            //tmp.Add("门店名称");
            tmp.Add("总额");
            tmp.Add("销售额");
            tmp.Add("进货资金");
            return tmp;
        }
        public static List<string> prodectsXStr()
        {
            List<string> tmp = new List<string>();

            //tmp.Add("销售量");
            //tmp.Add("库存");
            //tmp.Add("订货量");
            //tmp.Add("到货量");

            //tmp.Add("进价");
            //tmp.Add("门店ID");
            //tmp.Add("门店名称");
            tmp.Add("货物ID");           
            tmp.Add("货物名称");

            //tmp.Add("单价");
            return tmp;
        }
        public static List<string> prodectsYStr()
        {
            List<string> tmp = new List<string>();
           
            tmp.Add("销售量");
            tmp.Add("库存");
            tmp.Add("订货量");
            tmp.Add("到货量");

            //tmp.Add("进价");
            //tmp.Add("门店ID");

            //tmp.Add("货物ID");
            //tmp.Add("货物名称");
            //tmp.Add("单价");
            return tmp;
        }
        public static List<string> prodectsStr1()
        {
            List<string> tmp = new List<string>();        
            tmp.Add("销售量");
            tmp.Add("库存");
            tmp.Add("订货量");
            tmp.Add("到货量");
            return tmp;
        }
        public  static  string getSelectItem<T>(T obj,int index)
        {
            string tmp = "";
            getProperties pro = new getProperties();
            List<string> tmpStr = pro.getproperty(obj);
            tmp = tmpStr[index];
            return tmp;
        }
    }
}
