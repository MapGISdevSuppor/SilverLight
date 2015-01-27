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
using ZDIMS.BaseLib;
namespace ZDIMSDemo.MyControl
{
    public class OverlayBySelectParam
    {
        /**
         * 图层类型
         */
        public XClsType ClsType;
        /**
         * 要素类1所在的GDB信息
         */
        public CGdbInfo GdbInfo1;
        /**
          * 要素类2所在的GDB信息
          */
        public CGdbInfo GdbInfo2;
        /**
         * 是否重算面积
         */
        public Boolean IsReCalculate;
        /**
         * 要素类1的名称
         */
        public string LayerName1;
        /**
         * 要素类2的名称
         */
        public string LayerName2;
        /**
         * 叠加的类型
         */
        public int OverlayType;
        /**
         * 要素类1所在的GDB信息
         */
        public double Radius;
        /**
         * 要素类1查询参数
         */
        public CWebSelectParam SelectParam1;
        /**
         * 要素类2查询参数
         */
        public CWebSelectParam SelectParam2;
    }
}
