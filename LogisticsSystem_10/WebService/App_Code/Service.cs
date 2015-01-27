using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    string ConnectionString = "";
        // "data source=127.0.0.1;initial catalog=marketInfo;user id=sa;password=guest;persist security info=False;";
    string fff = "";
    List<DataClasses.marketsInfo> marks = new List<DataClasses.marketsInfo>();
    List<DataClasses.prodectsInfo> Prodects = new List<DataClasses.prodectsInfo>();
    List<DataClasses.FinancialInfo> financials = new List<DataClasses.FinancialInfo>();
    public Service()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public string HelloWorld() {
    //    return "Hello World";
    //}
    [WebMethod]
    //关键字查询
    public string SearchMarketBykey(string Key)
    {
    ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        string s = " '%" + Key + "%'";
        string strSql = "select X,Y,M_ID,marketName,Address ,CenterFlag,CenterID from marketsInfos where marketName like" + s;
        DataSet ds = ExecuteQueryBySql(strSql);
        string rlt = ToXml("marketsInfo", ds);
        return rlt;
    }
    public string ToString(DataSet ds)
    {
        if (ds == null)
            return "查询失败";
        string result = string.Empty;//查询结果字符串
        StringBuilder sb = new StringBuilder(0);
        int columns = ds.Tables[0].Columns.Count;//查询结果的列数
        if (columns > 1)
        {
            for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
            {
                for (int n = 0; n < ds.Tables[0].Columns.Count - 1; n++)
                {
                    if (ds.Tables[0].Rows[m][n] == null)
                        sb.Append("");
                    else
                        sb.Append(ds.Tables[0].Rows[m][n].ToString().Trim());
                    sb.Append(",");
                }
                if (ds.Tables[0].Rows[m][ds.Tables[0].Columns.Count - 1] == null)
                    sb.Append("");
                else
                    sb.Append(ds.Tables[0].Rows[m][ds.Tables[0].Columns.Count - 1].ToString().Trim());
                sb.Append(";");
            }
        }
        else//只有一列
        {
            for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
            {
                if (ds.Tables[0].Rows[m][0] == null)
                    sb.Append("");
                else
                    sb.Append(ds.Tables[0].Rows[m][0].ToString().Trim());
                sb.Append(",");
            }
        }
        if (sb.Length > 0)
        {
            result = sb.ToString().Substring(0, sb.Length - 1);
        }
        return result;
    }
    public DataSet ExecuteQueryBySql(string strSql)
    {

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            SqlDataAdapter da = new SqlDataAdapter(strSql, connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }
    [WebMethod]
    //根据ID查数据库
    public string SearchDetailsByID(int ID, string flg)
    {
        ConnectionString=ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        //string ConnectionString = "data source=(local);initial catalog=marketInfo;user id=sa;password=guest;persist security info=False;";
        string strSql = "";
        switch (flg)
        {
            //查门店财务
            case "Financial":
                {
                    strSql = "select M_ID, marketName,Total_amount,Sale_amount,Order_amount,MarketName  from Financial_statement where M_ID =" + ID;
                    break;
                }
            case "prodectsInfo":
                {//查门店货物库存、销量

                    strSql = "select M_ID,ProdectID,ProdectName,NumInStocks,SaleNum ,Price,InerPrice ,MarketName from  prodectsInfo where M_ID =" + ID;
                    break;
                }
            case "OrderStatement":
                {//订单信息
                    strSql = "select OrderID,ProdectID,ProdectName,OrderNum,MarketName  from  OrderStatement where M_ID =" + ID;
                    break;
                }
            case "CenterInfo":
                {
                    strSql = "select * from Centers_ProdectsInfos where M_ID= " + ID;
                    break;
                }
            case "marketsInfo":
                {
                    strSql = "select X,Y,M_ID,marketName,Address ,CenterFlag,CenterID from marketsInfos where M_ID= " + ID;
                    break;
                }
        }
        DataSet ds = ExecuteQueryBySql(strSql);
        string rlt = ToXml(flg, ds);
        return rlt;
    }

    //更新数据库
    [WebMethod]
    public string UppdateFinancial(string data)
    {
        ConnectionString=System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        bool f = false;
        data = xmlChang(data);
        List<DataClasses.FinancialInfo> fincinfo = new List<DataClasses.FinancialInfo>();
        fincinfo = XMLSerialization.Desrialize(fincinfo, data);
        for (int i = 0; i < fincinfo.Count; i++)
        {
            string str = " update Financial_statement set Order_amount=" + fincinfo[i].InerAmount + ",Total_amount=" + fincinfo[i].totalAmount
            + ",Sale_amount=" + fincinfo[i].SaleAmount + " where M_ID=" + fincinfo[i].MarketID;
            f = upp(str);
        }
        if (f)
        {
            return ("更新成功！");
        }
        else
        {
            return "更新失败！";
        }
    }

    [WebMethod]
    public string Uppdateprodect(string data)
    {
       ConnectionString= System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        bool f = false; ;
        data = xmlChang(data);
        List<DataClasses.prodectsInfo> pro = new List<DataClasses.prodectsInfo>();
        pro = XMLSerialization.Desrialize(pro, data);
        for (int i = 0; i < pro.Count; i++)
        {
            string str = "update prodectsInfo set Price=" + pro[i].Price + ",InerPrice=" + pro[i].InerPrice + "where (M_ID=" + pro[i].MarketID + "and ProdectID=" + pro[i].ProdectID + ")";
            f = upp(str);

        }
        if (f)
        {
            return ("更新成功！");
        }
        else
        {
            return "更新失败！";
        }
    }
    [WebMethod]
    //补货
    public string ImportInfo(int ID)
    {
        ConnectionString = ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        string strSql = "select M_ID,ProdectID,ProdectName,NumInStocks,InerPrice,MarketName  from  prodectsInfo where M_ID =" + ID;
        string rltinfo = "";
        DataSet ds = ExecuteQueryBySql(strSql);
        //if (ds == null)
        //  return "查询失败";
        string result = string.Empty;//查询结果字符串
        StringBuilder sb = new StringBuilder(0);
        int columns = ds.Tables[0].Columns.Count;//查询结果的列数
        double inerAmount = 0;
        if (columns > 1)
        {
            for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[m][3].ToString()) < 50)
                {
                    int importNum = 50 - Convert.ToInt32(ds.Tables[0].Rows[m][3].ToString());

                    string str = "update prodectsInfo set PreNum=" + importNum + " where (M_ID=" + ds.Tables[0].Rows[m][0].ToString() + "and ProdectID=" + ds.Tables[0].Rows[m][1].ToString() + ")";//in (select StationNum from tabRainEffective where RainEffective>=50.0)";";
                    inerAmount += importNum * Convert.ToDouble(ds.Tables[0].Rows[m][4].ToString());
                    if (!upp(str))
                    {
                        return "更新数据库失败！";
                    }
                    //Insert Into 产品(代号,名称,价格,数量) Values('C2000','Computre 2000',2000,2)" 
                    string f = ds.Tables[0].Rows[m][2].ToString();
                    string name = ds.Tables[0].Rows[m][5].ToString();
                    string strOrder = "insert into OrderStatement( M_ID,ProdectID,ProdectName,OrderNum,preData,ImportFlg,MarketName) Values ( " + ds.Tables[0].Rows[m][0].ToString() + "," + ds.Tables[0].Rows[m][1].ToString() + ",'" + f + "','" + importNum + "','" + DateTime.Now + "'," + 0 + ",'" + name + "')";
                    if (!upp(strOrder))
                    {
                        return "更新数据库失败！";
                    }
                }

            }

            if (inerAmount > 0)
            {
                string strAmount = "update  Financial_statement set  Order_amount=" + inerAmount + "where M_ID=" + ds.Tables[0].Rows[0][0].ToString();
                if (!upp(strAmount))
                {
                    return "更新数据库失败！";
                }
                else
                {
                    string strOrderInfo = "select M_ID, ProdectID,ProdectName,OrderNum, preData,MarketName from OrderStatement where ImportFlg=" + 0;
                    DataSet ds1 = ExecuteQueryBySql(strOrderInfo);

                    rltinfo = ToXml("OrderStatement", ds1);
                    rltinfo += ";" + SearchDetailsByID(ID, "marketsInfo");
                }

            }
        }
        return rltinfo;

    }
    /// <summary>
    /// 更新数据库
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool upp(string str)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            int flag;
            connection.Open();
            try
            {
                SqlCommand comm = new SqlCommand(str, connection);
                flag = comm.ExecuteNonQuery();
                fff += "," + flag.ToString();
            }
            catch
            {
                return false;
            }
            if (flag > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    
    public string ToXml(string DataType, DataSet ds)
    {
        string rlt = "";

        switch (DataType)
        {
            case "marketsInfo":
                {

                    DataClasses.marketsInfo mark = null;
                    //string result = string.Empty;//查询结果字符串

                    int columns = ds.Tables[0].Columns.Count;//查询结果的列数

                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {
                        try
                        {
                            //{//X,Y,M_ID,marketName,Address ,CenterFlag
                            mark = new DataClasses.marketsInfo();
                            mark.X = Convert.ToDouble(ds.Tables[0].Rows[m][0]);
                            mark.Y = Convert.ToDouble(ds.Tables[0].Rows[m][1]);
                            mark.MarketID = Convert.ToInt32(ds.Tables[0].Rows[m][2]);
                            mark.MarketName = ds.Tables[0].Rows[m][3].ToString();
                            mark.Address = ds.Tables[0].Rows[m][4].ToString();
                            mark.CenterFlag = Convert.ToInt32(ds.Tables[0].Rows[m][5]);
                            mark.CenterID = Convert.ToInt32(ds.Tables[0].Rows[m][6]);
                            marks.Add(mark);
                        }
                        catch
                        {
                        }

                    }
                    if (marks.Count > 0)
                    {
                        //string s = XMLSerialization.Serialize(mark);
                        rlt = XMLSerialization.Serialize(marks);
                    }
                    else
                    {
                        rlt = "无信息！";
                    }
                    break;
                }
            case "prodectsInfo":
                {
                    //select M_ID、ProdectID,ProdectName,NumInStocks,SaleNum 、Price、InerPrice


                    DataClasses.prodectsInfo Prodect = new DataClasses.prodectsInfo();
                    int columns = ds.Tables[0].Columns.Count;//查询结果的列数

                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {


                        try
                        {
                            Prodect = new DataClasses.prodectsInfo();
                            Prodect.MarketID = Convert.ToInt32(ds.Tables[0].Rows[m][0]);
                            Prodect.ProdectID = Convert.ToInt32(ds.Tables[0].Rows[m][1]);
                            Prodect.ProdectName = ds.Tables[0].Rows[m][2].ToString();
                            Prodect.InstocksNum = Convert.ToInt32(ds.Tables[0].Rows[m][3]);
                            Prodect.SaleNum = Convert.ToInt32(ds.Tables[0].Rows[m][4]);
                            Prodect.Price = Convert.ToDouble(ds.Tables[0].Rows[m][5]);
                            Prodect.InerPrice = Convert.ToDouble(ds.Tables[0].Rows[m][6]);
                            Prodect.MarketName = ds.Tables[0].Rows[m][7].ToString();
                            Prodects.Add(Prodect);
                        }
                        catch
                        {
                        }
                    }
                    if (Prodects.Count > 0)
                    {
                        rlt = XMLSerialization.Serialize(Prodects);
                    }
                    else
                    {
                        rlt = "无信息";
                    }

                    break;

                }
            case "CenterInfo":
                {
                    List<DataClasses.CenterSInfo> Centers = new List<DataClasses.CenterSInfo>();
                    DataClasses.CenterSInfo Center = new DataClasses.CenterSInfo();
                    int columns = ds.Tables[0].Columns.Count;//查询结果的列数

                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {
                        try
                        {
                            Center = new DataClasses.CenterSInfo();
                            Center.MarketName = ds.Tables[0].Rows[m][0].ToString();
                            Center.CenterID = Convert.ToInt32(ds.Tables[0].Rows[m][1]);
                            Center.prodectID = Convert.ToInt32(ds.Tables[0].Rows[m][2]);
                            Center.prodectName = ds.Tables[0].Rows[m][3].ToString();
                            Center.prodectNum = Convert.ToInt32(ds.Tables[0].Rows[m][4]);
                            
                            Centers.Add(Center);
                        }
                        catch
                        {
                        }
                    }
                    if (Centers.Count > 0)
                    {
                        rlt = XMLSerialization.Serialize(Centers);
                    }
                    else
                    {
                        rlt = "无信息";
                    }
                    break;

                }
            case "Financial":
                {
                    // M_ID, marketName,Total_amount,Sale_amount,Order_amount

                    DataClasses.FinancialInfo finacial = new DataClasses.FinancialInfo();
                    int columns = ds.Tables[0].Columns.Count;//查询结果的列数

                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {
                        try
                        {
                            finacial = new DataClasses.FinancialInfo();

                            finacial.MarketID = Convert.ToInt32(ds.Tables[0].Rows[m][0]);
                            finacial.MarketName = ds.Tables[0].Rows[m][1].ToString();
                            finacial.totalAmount = Convert.ToDouble(ds.Tables[0].Rows[m][2]);
                            finacial.SaleAmount = Convert.ToDouble(ds.Tables[0].Rows[m][3]);
                            finacial.InerAmount = Convert.ToDouble(ds.Tables[0].Rows[m][4]);
                            financials.Add(finacial);
                        }
                        catch
                        {
                        }
                    }
                    if (financials.Count > 0)
                    {

                        rlt = XMLSerialization.Serialize(financials);
                    }
                    else
                    {
                        rlt = "无信息";
                    }
                    break;
                }
            case "OrderStatement":
                {//M_ID, ProdectID,ProdectName,OrderNum, preData
                    List<DataClasses.OrderSatement> tmpList = new List<DataClasses.OrderSatement>();
                    DataClasses.OrderSatement Order = new DataClasses.OrderSatement();
                    int columns = ds.Tables[0].Columns.Count;
                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {
                        try
                        {
                            Order = new DataClasses.OrderSatement();

                            Order.MarketID = Convert.ToInt32(ds.Tables[0].Rows[m][0]);
                            Order.ProdectID = Convert.ToInt32(ds.Tables[0].Rows[m][1]);
                            Order.prodectName = ds.Tables[0].Rows[m][2].ToString();
                            Order.PreNum = Convert.ToInt32(ds.Tables[0].Rows[m][3]);
                            Order.PreTime = Convert.ToDateTime(ds.Tables[0].Rows[m][4]);
                            Order.MarketName = ds.Tables[0].Rows[m][5].ToString();
                            tmpList.Add(Order);
                        }
                        catch
                        {
                        }
                    }
                    if (tmpList.Count > 0)
                    {
                        rlt = XMLSerialization.Serialize(tmpList);
                    }
                    else
                    {
                        rlt = "无信息";
                    }

                    break;
                }


        }
        return rlt;

    }
    /// <summary>
    /// 添加信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    [WebMethod]
    public string addData(string type,string info)
    {
        string rlt = "";
        string strSql = "";
        info = xmlChang(info);
        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        switch (type)
        {
            case "marketsInfo":
                {
        DataClasses.marketsInfo market = new DataClasses.marketsInfo();
        
        market = XMLSerialization.Desrialize(market, info);
        strSql = "insert into marketsInfos values (" + market.X + "," + market.Y + "," + market.CenterFlag + "," + market.MarketID + ",'" + market.MarketName + "','" + market.Address + "'," + market.CenterID + ")";
                    break;
                }

            case "prodectsInfo":
                {
                    DataClasses.prodectsInfo prodect = new DataClasses.prodectsInfo();
                    prodect = XMLSerialization.Desrialize(prodect, info);
                    strSql = "insert into prodectsInfo values ('"+prodect.MarketName +"',"+ prodect.MarketID + "," + prodect.ProdectID + "," + prodect.Price + "," + prodect.SaleNum + "," + prodect.InstocksNum + "," + prodect.PreNum + "," + prodect.ReceNum + ",'" + prodect.ProdectName + "'," + prodect.InerPrice + ")";

                    break;
                }

            case "CenterInfo":
                {
                    DataClasses.CenterSInfo center = new DataClasses.CenterSInfo();
                    center = XMLSerialization.Desrialize(center, info);
                    strSql = "insert into Centers_ProdectsInfos values ('" +center.MarketName+"',"+ center.CenterID + "," + center.prodectID + ",'" + center.prodectName + "'," + center.prodectNum + ")";
                    break;
                }

            case "Financial":
                {
                    DataClasses.FinancialInfo financial = new DataClasses.FinancialInfo();
                    financial = XMLSerialization.Desrialize(financial, info);
                    strSql = "insert into Financial_statement values ('" + financial.MarketName + "'," + financial.MarketID + "," + financial.totalAmount + "," + financial.SaleAmount + "," + financial.InerAmount + ")";
                }
                break;
        }
        if (!upp(strSql))
        {
            rlt = "添加失败！";
            return rlt;
        }
        return "添加成功！";
    }
   private string xmlChang(string xmlStr)
    {
        string tmp = xmlStr;
        tmp = tmp.Replace("<?xml version=\\\"1.0\\\" encoding=\\\"utf-16\\\"?>", "<?xml version=\"1.0\" encoding=\"utf-16\"?>");
        tmp = tmp.Replace("xmlns:xsi=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" xmlns:xsd=\\\"http://www.w3.org/2001/XMLSchema\\\"", "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"");
        tmp = tmp.Replace("\\r\\n", "\r\n");

        return tmp;
    }
    string rlt = "";

    [WebMethod]
    public string SearchByCircle(double x, double y, double r)
    {
       ConnectionString= System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        rlt = "";
        string sql = "select X,Y,M_ID,marketName,Address ,CenterFlag,CenterID from marketsInfos where ((" + x + "-marketsInfos.X)*(" + x + "-marketsInfos.X)+(" + y + "-marketsInfos.Y)*(" + y + "-marketsInfos.Y))<(" + r + "*" + r + ")";
        DataSet ds = ExecuteQueryBySql(sql);
        string markTmp = ToXml("marketsInfo", ds);
        rlt += markTmp + ";";
        string proStr = "";
        List<DataClasses.prodectsInfo> prodects = new List<DataClasses.prodectsInfo>();
        List<DataClasses.FinancialInfo> financials = new List<DataClasses.FinancialInfo>();
        for (int i = 0; i < marks.Count; i++)
        {
            sql = "select M_ID,ProdectID,ProdectName,NumInStocks,SaleNum ,Price,InerPrice,MarketName  from  prodectsInfo where M_ID =" + marks[i].MarketID;
            DataSet ds1 = ExecuteQueryBySql(sql);
            DataClasses.prodectsInfo Prodect = new DataClasses.prodectsInfo();
            int columns = ds1.Tables[0].Columns.Count;//查询结果的列数

            for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
            {


                try
                {
                    Prodect = new DataClasses.prodectsInfo();
                    Prodect.MarketID = Convert.ToInt32(ds1.Tables[0].Rows[m][0]);
                    Prodect.ProdectID = Convert.ToInt32(ds1.Tables[0].Rows[m][1]);
                    Prodect.ProdectName = ds1.Tables[0].Rows[m][2].ToString();
                    Prodect.InstocksNum = Convert.ToInt32(ds1.Tables[0].Rows[m][3]);
                    Prodect.SaleNum = Convert.ToInt32(ds1.Tables[0].Rows[m][4]);
                    Prodect.Price = Convert.ToDouble(ds1.Tables[0].Rows[m][5]);
                    Prodect.InerPrice = Convert.ToDouble(ds1.Tables[0].Rows[m][6]);
                    Prodect.MarketName = ds1.Tables[0].Rows[m][7].ToString();
                    prodects.Add(Prodect);
                }
                catch
                {
                    break;
                }
                
            }
            string Sql = "select M_ID, marketName,Total_amount,Sale_amount,Order_amount,MarketName  from Financial_statement where M_ID =" + marks[i].MarketID;
            ds1 = ExecuteQueryBySql(Sql);
            DataClasses.FinancialInfo finacial = new DataClasses.FinancialInfo();
            try
            {
                finacial.MarketID = Convert.ToInt32(ds1.Tables[0].Rows[0][0]);
                finacial.MarketName = ds1.Tables[0].Rows[0][1].ToString();
                finacial.totalAmount = Convert.ToDouble(ds1.Tables[0].Rows[0][2]);
                finacial.SaleAmount = Convert.ToDouble(ds1.Tables[0].Rows[0][3]);
                finacial.InerAmount = Convert.ToDouble(ds1.Tables[0].Rows[0][4]);
                finacial.MarketName = ds1.Tables[0].Rows[0][5].ToString();
                financials.Add(finacial);
            }
            catch
            {
            }

        }
        if (prodects.Count > 0)
        {
            proStr = XMLSerialization.Serialize(prodects);
        }
        else
        {
            proStr = "无信息";
        }
        if (financials.Count > 0)
        {
          proStr+=";"+  XMLSerialization.Serialize(financials);
        }
        else
        {
            proStr += ";" + "无信息";
        }
        rlt += proStr;
        return rlt;

    }
    [WebMethod]
    public string   UppDateOrderStatment(int MarketID, string data)
    {
        ConnectionString=System.Configuration.ConfigurationManager.ConnectionStrings["markets"].ToString();
        bool f = false;
        string strOrder = "";
        List<DataClasses.OrderSatement> value = new List<DataClasses.OrderSatement>();
        data=xmlChang(data);
        value = XMLSerialization.Desrialize(value, data);
        for (int i=0; i < value.Count; i++)
        {
            strOrder = "update OrderStatement set ReceiveData='" + value[i].ReceTime + "',ReceiveNum=" + value[i].ReceNum + ",ImportFlg=" + 1 + " where (M_ID=" + MarketID + " and ProdectID=" + value[i].ProdectID + " and ImportFlg=" + 0 + ")";
            //f = upp(strOrder);
            //return "";
            if (!upp(strOrder))
            {
                return "更新库失败！";
            }
            else
            {
                string strSql = "select NumInStocks,InerPrice  from  prodectsInfo  where (M_ID= " + MarketID + "and ProdectID=" + value[i].ProdectID + ")";
                DataSet numStock = ExecuteQueryBySql(strSql);
                DataRow Row = numStock.Tables[0].Rows[0];
                int num = Convert.ToInt32(Row.ItemArray[0]);
                strOrder = " update prodectsInfo set PreNum=" + 0 + ",ReceNum= " + (value[i].ReceNum + num) + ",NumInStocks=" + value[i].ReceNum + " where (M_ID=" + MarketID + "and ProdectID=" + value[i].ProdectID + ")";//in (select StationNum from tabRainEffective where RainEffective>=50.0)";";
                f = upp(strOrder);
            }
        }

        if (!f)
        {
            return "更新库失败！";
        }
        else
        {
            return "更新库成功！";
        }
        //string strOrder = "insert into OrderStatement( M_ID,ProdectID,ProdectName,OrderNum,preData,ImportFlg) Values ( " + ds.Tables[0].Rows[m][0].ToString() + "," + ds.Tables[0].Rows[m][1].ToString() + ",'" + f + "','" + importNum + "','" + DateTime.Now + "'," + 0 + ")";
    }
}
