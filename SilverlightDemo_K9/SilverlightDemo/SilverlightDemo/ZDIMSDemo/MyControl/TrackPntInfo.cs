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

namespace ZDIMSDemo.MyControl
{
    public class TrackPntInfo:IComparable
    {
        private DateTime getPntTime;

        public DateTime GetPntTime
        {
            get { return getPntTime; }
            set { getPntTime = value; }
        }

        private Point localPnt;

        public Point LocalPnt
        {
            get { return localPnt; }
            set { localPnt = value; }
        }
        #region 实现比较接口的CompareTo方法
        /// <summary>
        /// 为了在调用SORT时，按时间的先后顺序排序
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            int res = 0;
            try
            {
                TrackPntInfo sObj = (TrackPntInfo)obj;
                if (this.getPntTime.CompareTo(sObj.getPntTime) > 0)
                {
                    res = 1;
                }
                else if (this.getPntTime.CompareTo(sObj.getPntTime) < 0)
                {
                    res = -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("排序异常", ex.InnerException);
            }
            return res;
        }
        #endregion


    }
}
