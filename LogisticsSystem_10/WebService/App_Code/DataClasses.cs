using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// DataClasses 的摘要说明
/// </summary>
public class DataClasses
{
	public DataClasses()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 门店信息
    /// </summary>
    public class marketsInfo
    {
        public int MarketID ;
        public string MarketName ;
        public int CenterID ;
        public int CenterFlag ;
        public string Address ;
        public double X;
        public double Y ;
        public marketsInfo()
        {
            MarketID = 0;
            MarketName = "";
            CenterID = 0;
            CenterFlag = 0;
            Address = "";
            X = 0;
            Y = 0;
        }
    }
    /// <summary>
    /// 货物信息
    /// </summary>
    public class prodectsInfo
    {
        public int MarketID ;
        public string MarketName;
        public int ProdectID ;
        public string ProdectName ;
        public double Price ;
        public int SaleNum ;
        public int InstocksNum ;
        public int PreNum ;
        public int ReceNum ;
        public double InerPrice ;
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
        public int CenterID ;
        public int prodectID ;
        public string prodectName ;
        public int prodectNum ;
        public string MarketName;
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
        public int MarketID ;
        public string MarketName;
        public double totalAmount ;
        public double SaleAmount ;
        public double InerAmount ;

        public FinancialInfo()
        {
            MarketID = 0;
            totalAmount = 0;
            SaleAmount = 0;
            InerAmount = 0;
            MarketName = "";
        }
    }
    public class OrderSatement
    {
        public int MarketID;
        public int ProdectID;
        public DateTime PreTime;
        public DateTime ReceTime;
        public int PreNum;
        public int ReceNum;
        public int CenterID;
        public string prodectName;
        public string MarketName;
        public OrderSatement()
        {
            MarketID = 0;
            ProdectID = 0;
            PreTime = DateTime.Now;
            ReceTime = DateTime.Now;
            CenterID = 0;
            PreNum = 0;
            ReceNum = 0;
            prodectName = "";
            MarketName = "";
        }
    }
}
