
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
using System.Windows.Media.Imaging;
using ZDIMS.Drawing;
using ZDIMS.Map;
using ZDIMS.Interface;
using System.Collections.Generic;
using ZDIMS.BaseLib;
using Logistics_system.controls;
namespace Logistics_system.controls
{

    public class addMarks
    {
    
        private string src = "../images/red2.png";
        private Image img = null;
        public MarkLayer markLayer1;
        public GraphicsLayer graphicsLayer;
        public List<marketsInfo> markets;
        public IMSMap mapContainer;
       // MapSite tips;
        EditMark tips;
        /// <summary>
        /// 坐标序列
        /// </summary>
        public List<Point> PntList;
        /// <summary>
        /// 坐标点
        /// </summary>
        public Point Pnt;

        /// <summary>
        /// 添加单个标注
        /// </summary>
        /// <param name="LogPnt"></param>
        public IMSMark AddMark(marketsInfo market)
        {
            if (this.markLayer1 == null)
            {
                MessageBox.Show("标注图层为空！");
                return null;
            }
           img =new Image();
            img.Source=new BitmapImage(new Uri("../images/SiteIcon.png",UriKind.Relative));
            img.Height=48;
            img.Width=38;

            tips = new  EditMark( market.MarketName, market.MarketID.ToString() , market.Address, "");
            //tips(market);
           IMSMark mark = new IMSMark(tips, CoordinateType.Logic, markLayer1);
            mark.EnableAnimation = false;
            mark.EnableRevisedPos = true;

            Point pnt = new Point(market.X, market.Y);
           

            mark.X = pnt.X;
            mark.Y = pnt.Y;
            mark.OffsetX = -19;
            mark.OffsetY = -48;
            markLayer1.AddMark(mark);

            return mark;
        }
        /// <summary>
        /// 添加单个标注
        /// </summary>
        /// <param name="LogPnt"></param>
        public void AddMarkGif(marketsInfo warnMarket)
        {
            GIFToolTip gif = new GIFToolTip();
            gif.Addtip(warnMarket);
            IMSMark mark = new IMSMark(gif, CoordinateType.Logic, markLayer1);
            mark.EnableAnimation = false;
            mark.EnableRevisedPos = true;

            Point pnt = new Point(warnMarket.X, warnMarket.Y);
            pnt.X = pnt.X -11;
            pnt.Y = pnt.Y -11;
            pnt = this.mapContainer.ScreenToLogic(pnt.X, pnt.Y);

            mark.X = pnt.X;
            mark.Y = pnt.Y;

            markLayer1.AddMark(mark);
        }
        public List<IMSMark> AddMarks()
        {
            List<IMSMark> tmp = new List<IMSMark>();
            int count = 0;

            IMSMark mark;

            while (count < markets.Count)
            {
                img = new Image();
                img.Source = new BitmapImage(new Uri("../images/SiteIcon.png", UriKind.Relative));
                img.Height = 48;
                img.Width = 38;
                tips = new  EditMark( markets[count].MarketName, markets[count].MarketID.ToString(), markets[count].Address,"");            
                    mark = new IMSMark(tips, CoordinateType.Logic, markLayer1);
                    mark.EnableAnimation = false;
                    mark.EnableRevisedPos = true;
                    Point pn = new Point(markets[count].X, markets[count].Y);
                    pn = this.mapContainer.LogicToScreen(pn.X, pn.Y);
                    pn = this.mapContainer.ScreenToLogic(pn.X, pn.Y);                 
                    mark.X = pn.X;
                    mark.Y = pn.Y;
                    mark.OffsetX = -19;
                    mark.OffsetY = -48;
                    markLayer1.AddMark(mark);
                    tmp.Add(mark);
                    count++;
                
            }
            markLayer1.EnableGPUMode = true;
            return tmp;
 
        }

        #region 废弃
        private Point  GetRandomCoordinate(int index, double  centerX, double 	 centerY, double  temp)
        {//获取随机点
            switch (index % 16)
            {
                case 0:
                    centerX = centerX + temp;
                    //centerY = centerY;
                    break;
                case 1:
                    centerX = centerX + temp * 0.866;
                    centerY = centerY + temp / 2;
                    break;
                case 2:
                    centerX = centerX + temp * 0.707;
                    centerY = centerY + temp * 0.707;
                    break;
                case 3:
                    centerX = centerX + temp / 2;
                    centerY = centerY + temp * 0.866;
                    break;
                case 4:
                    //centerX = centerX;
                    centerY = centerY + temp;
                    break;
                case 5:
                    centerX = centerX - temp / 2;
                    centerY = centerY + temp * 0.866;
                    break;
                case 6:
                    centerX = centerX - temp * 0.707;
                    centerY = centerY + temp * 0.707;
                    break;
                case 7:
                    centerX = centerX - temp * 0.866;
                    centerY = centerY + temp / 2;
                    break;
                case 8:
                    centerX = centerX - temp;
                    //centerY = centerY;
                    break;
                case 9:
                    centerX = centerX - temp * 0.866;
                    centerY = centerY - temp / 2;
                    break;
                case 10:
                    centerX = centerX - temp * 0.707;
                    centerY = centerY - temp * 0.707;
                    break;
                case 11:
                    centerX = centerX - temp / 2;
                    centerY = centerY - temp * 0.866;
                    break;
                case 12:
                    //centerX = centerX;
                    centerY = centerY - temp;
                    break;
                case 13:
                    centerX = centerX + temp / 2;
                    centerY = centerY - temp * 0.866;
                    break;
                case 14:
                    centerX = centerX + temp * 0.707;
                    centerY = centerY - temp * 0.707;
                    break;
                case 15:
                    centerX = centerX + temp * 0.866;
                    centerY = centerY - temp / 2;
                    break;
            }
            return new Point(centerX, centerY);
        }
        #endregion 


    }
}
