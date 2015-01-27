using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Logistics_system
{

    public class DynamicData
    {
        public double X
        { get; set; }
        public double Y
        { get; set; }
    }

   
    /// <summary>
    /// 门店信息
    /// </summary>
    public class marketsInfo
    {
        public int MarketID { get; set; }
        public string MarketName { get; set; }
        public int CenterID { get; set; }
        public int CenterFlag { get; set; }
        public string Address { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public marketsInfo()
        {
            MarketID = 0;
            MarketName = "";
            CenterID = 0;
            CenterFlag = 0;
            Address = "";
        }
    }
    /// <summary>
    /// 货物信息
    /// </summary>
    public class prodectsInfo
    {
       
        public int SaleNum { get; set; }
        public int InstocksNum { get; set; }
        public int PreNum { get; set; }
        public int ReceNum { get; set; }
        public double  InerPrice { get; set; }

       
        public int MarketID { get; set; }
        public string MarketName { get; set; }
        public int ProdectID { get; set; }
       
        public string ProdectName { get; set; }
        public double Price { get; set; }
        public prodectsInfo()
        {
            MarketID = 0;
            ProdectID = 0;
            ProdectName = "";
            Price = 0;
            SaleNum = 0;
            InstocksNum = 0;
            PreNum = 0;
            ReceNum = 0;
            InerPrice = 0;
            MarketName = "";
        }

    }
    /// <summary>
    /// 中心店信息
    /// </summary>
    public class CenterSInfo
    {
        public int CenterID { get; set; }
        public int prodectID { get; set; }
        public string prodectName { get; set; }
        public int prodectNum { get; set; }
        public string MarketName { get; set; }
        public CenterSInfo()
        {
            CenterID = 0;
            prodectID = 0;
            prodectName = "";
            prodectNum = 0;
            MarketName = "";
        }

    }
    /// <summary>
    /// 财务信息
    /// </summary>
    public class FinancialInfo
    {
        public int MarketID { get; set; }
        public string MarketName { get; set; }
        public double  totalAmount { get; set; }
        public double  SaleAmount { get; set; }
        public double InerAmount { get; set; }
        public FinancialInfo()
        {
            MarketID = 0;
            totalAmount = 0;
            SaleAmount = 0;
            InerAmount = 0;
        }
    }
    public class OrderSatement
    {
        public int MarketID { get; set; }
        public int ProdectID { get; set; }
        public DateTime PreTime { get; set; }
        public DateTime ReceTime { get; set; }
        public int PreNum { get; set; }
        public int ReceNum { get; set; }
        public int CenterID { get; set; }
        public string prodectName;
        public string MarketName { get; set; }
        public OrderSatement()
        {
            MarketID = 0;
            ProdectID = 0;
            PreTime = DateTime.Now;
            ReceTime = DateTime.Now;
            CenterID = 0;
            PreNum = 0;
            ReceNum = 0;
            MarketName = "";
        }
    }

    public class stops
    {
        public double X;
        public double Y;
        public string StopName;



       

    }
    
    [XmlRoot("NewDataSet")]
    public class StopsData : List<stops>
    {

        List<stops> list = new List<stops>();
            stops tmp = new stops();
          

          
        //public static object DataSource
        //{
        //    get
        //    {
        //        XmlSerializer s = new XmlSerializer(typeof(StopsData));
        //        return s.Deserialize(typeof(StopsData).Assembly.GetManifestResourceStream(typeof(StopsData).Namespace + ".Data.stops.xml"));
        //    }
        //}
    }
}
