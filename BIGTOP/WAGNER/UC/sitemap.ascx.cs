using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.EasyAsk;
public partial class UC_sitemap : System.Web.UI.UserControl
{
    HelperServices objHelperServices = new HelperServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    Security objsecurity = new Security(); 

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_Newproduct()
    {
        try
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperServices = new HelperServices();

            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCTLOGNAV", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            tbwtEngine.RenderHTML("Column");
            return tbwtEngine.ST_NewProductLogNav_Load(null, HttpContext.Current);

            //if (Session["NewProductLogNav"] != null)
            //{
            //    //return tbwtEngine.ST_NewProductLogNav_Load((DataSet)Session["NewProductLogNav"]);
            //    return Session["NewProductLogNav"].ToString();
            //}
            //else
            //    return "";

            //return "";
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    public string ST_Sitemap()
    {

        return (objHelperServices.StripWhitespace( Category_RenderHTML("SITEMAP", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()))));
    }


    public string Category_RenderHTML(string Package, string SkinRootPath)
    {


       

            string skin_container = null;
            int grid_cols = 0;
            int grid_rows = 0;
            string skin_sql_container = null;
            string skin_sql_param_container = null;
            string skin_records = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrecordsmain = new TBWDataList[0];
            StringTemplateGroup stg_records = null;
            StringTemplate bodyST = null;
            StringTemplate bodyST_categorylist = null;
            StringTemplate bodyST_head = null;
            StringTemplate bodyST_list1 = null;
            string firstval = null;
            //List<string> name = new List<string>();
            StringBuilder name = new StringBuilder();
            System.Text.StringBuilder ct = new StringBuilder();
            System.Text.StringBuilder categoryrowlist = new StringBuilder();
            int indV = 0;
            int bodyValue = 0;
            DataSet dspkg = new DataSet();
            string sHtmls = string.Empty;  
            //string sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
            //sqlpkginfo = sqlpkginfo + " WHERE PACKAGE_NAME = '" + Package + "'";
            //dspkg = GetDataSet(sqlpkginfo);
            dspkg = (DataSet)objHelperDB.GetGenericDataDB(Package, "GET_PACKAGE_WITHOUT_ISROOT", HelperDB.ReturnType.RTDataSet);
            if (dspkg != null)
            {
                if (dspkg.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dspkg.Tables[0].Rows)
                    {
                        skin_container = dr["SKIN_NAME"].ToString();
                        grid_cols = 5;
                        grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                        skin_sql_container = dr["SKIN_SQL"].ToString();
                        skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                        skin_records = dr["SKIN_NAME"].ToString();
                    }
                }
            }

            //Build the inner body of the HTML
            stg_records = new StringTemplateGroup(skin_records, SkinRootPath + "\\" + skin_records);
            DataSet dsrecords=null;
            DataSet dsrecords1=null;
            //if (HttpContext.Current.Session["MainCategory"] == null)
            //{
            //    EasyAsk.GetCategoryAndBrand("MainCategory");
            //    dsrecords = (DataSet)HttpContext.Current.Session["MainCategory"];
            //}
            //else
            //{
            //    dsrecords = (DataSet)HttpContext.Current.Session["MainCategory"];
            //}
            if (HttpContext.Current.Application["key_MainCategory"] != null)
            {
              //  EasyAsk.GetCategoryAndBrand("MainCategory");
                dsrecords = (DataSet)HttpContext.Current.Application["key_MainCategory"];
            }
            //else
            //{
            //    dsrecords = (DataSet)HttpContext.Current.Session["MainCategory"];
            //}
            //if (HttpContext.Current.Session["SubCategory"] == null)
            //{
            //    EasyAsk.GetCategoryAndBrand("SubCategory");
            //    dsrecords1 = (DataSet)HttpContext.Current.Session["SubCategory"];
            //}
            //else
            //{
            //    dsrecords1 = (DataSet)HttpContext.Current.Session["SubCategory"];
            //}
            if (HttpContext.Current.Application["key_SubCategory"] != null)
            {
                //EasyAsk.GetCategoryAndBrand("SubCategory");
                dsrecords1 = (DataSet)HttpContext.Current.Application["key_SubCategory"];
            }
            //else
            //{
            //    dsrecords1 = (DataSet)HttpContext.Current.Session["SubCategory"];
            //}
            //DataSet dsrecords = objHelperDB.GetDataSetDB(skin_sql_container);

            if (dsrecords != null && dsrecords.Tables[0].Rows.Count != 0)
            {

           
                int st = dsrecords.Tables[0].Rows.Count;
                int noofr = st / 5; int k=0;
                lstrecordsmain = new TBWDataList[noofr];
                for (int i = 0; i < noofr; i++)
                {

                    int j;
                    if (i == 0)
                    {

                        k = 5;
                        j = 0;
                    }
                    else
                    {
                        j = k;
                        k = k + 5 ;

                    }
                    lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count];
                    for (int l = j; l < k; l++)
                    {
                        if (dsrecords.Tables[0].Rows[0][1].ToString() != null && dsrecords.Tables[0].Rows[0][1].ToString() != string.Empty)
                        {


                            //Build the Sub heading 
                            firstval = dsrecords.Tables[0].Rows[0][1].ToString().ToUpper();
                            bodyST_categorylist = stg_records.GetInstanceOf("cell");
                            //                    DataRow ddr = dsrecords.Tables[0].Rows[0];
                            bodyST_categorylist.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                            bodyST_categorylist.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(dsrecords.Tables[0].Rows[l]["EA_PATH"].ToString())));
                            bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dsrecords.Tables[0].Rows[l]["CATEGORY_NAME"].ToString()));
                            bodyST_categorylist.SetAttribute("TBT_PARENT_CATEGORY_NAME", dsrecords.Tables[0].Rows[l]["CATEGORY_NAME"].ToString());
                            objHelperServices.SimpleURL(bodyST_categorylist, "//// //// ////" + dsrecords.Tables[0].Rows[l]["CATEGORY_NAME"].ToString() + "////" + bodyST_categorylist.GetAttribute("TBT_ATTRIBUTE_VALUE"), "pl.aspx");

                        }
                        DataTable dt = new DataTable();
                        DataRow[] dr1 = null;
                        ct = new StringBuilder();
                        if(dsrecords1.Tables.Count >0 && dsrecords1.Tables[0].Rows.Count > 0)
                           dr1 = dsrecords1.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + dsrecords.Tables[0].Rows[l]["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME");

                        if (dr1!= null && dr1.Length > 0)
                        {
                            dt = dr1.CopyToDataTable();

                            foreach (DataRow dr in dt.Rows)
                            {

                                if (dr["TBT_SHORT_DESC"].ToString().ToLower() != "not for web")
                                {
                                    bodyST_list1 = stg_records.GetInstanceOf("cell1");
                                    bodyST_list1.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
                                    bodyST_list1.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                                    bodyST_list1.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"].ToString());
                                    bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(dr["TBT_CUSTOM_NUM_FIELD3"]).ToString());
                                    bodyST_list1.SetAttribute("TBT_PARENT_CATEGORY_ID", dr["TBT_PARENT_CATEGORY_ID"].ToString());
                                    bodyST_list1.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objsecurity.StringEnCrypt(dsrecords.Tables[0].Rows[l]["EA_PATH"].ToString())));
                                    bodyST_list1.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["CATEGORY_NAME"].ToString()));

                                    DataTable dt1 = (DataTable)HttpContext.Current.Session["dtcid"];

                                    DataRow[] foundRows;
                                    string parentcat = string.Empty;
                                    string cid = dr["TBT_PARENT_CATEGORY_ID"].ToString();
                                    if (dt1 != null)
                                    {
                                        foundRows = dt1.Select("cid='" + cid + "' ");
                                        if (foundRows.Length > 0)
                                        {
                                            parentcat = foundRows[0][1].ToString();

                                        }
                                         
                                    else
                                    {
                                        parentcat = dr["TBT_PARENT_CATEGORY_NAME"].ToString();
                                    }
                                    }
                                   
                                    else
                                    {
                                        parentcat = dr["TBT_PARENT_CATEGORY_NAME"].ToString();
                                    }
                                    objHelperServices.SimpleURL(bodyST_list1, dr["EA_PATH"].ToString() + "////" + parentcat + "////" + dr["CATEGORY_NAME"].ToString(), "pl.aspx");
                                    ct.Append(bodyST_list1.ToString());
                                }

                            }
                        }
                        bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", ct.ToString());
                        name.Append(bodyST_categorylist.ToString());
                        indV++;
                        if (indV == grid_cols)
                        {
                            bodyST = stg_records.GetInstanceOf("row");
                            bodyST.SetAttribute("TBWDataList", name);
                            lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                            bodyValue++;
                            indV = 0;
                            name = new StringBuilder();
                        }
                    }


                    if (dsrecords == null || dsrecords.Tables[0].Rows.Count == 0)
                    {
                        bodyST_categorylist = stg_records.GetInstanceOf("cell");
                    }


                    bodyST = stg_records.GetInstanceOf("row");
                    bodyST.SetAttribute("TBWDataList", name);
                    if (lstrecords.Length != 0)
                    {
                        lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                    }
                 
  StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
            bodyST_main.SetAttribute("TBWDataList", lstrecords);
            lstrecordsmain[i] = new TBWDataList(bodyST_main.ToString());
                }
            }
            StringTemplate bodyST_mainAll = stg_records.GetInstanceOf("Allmain");
            bodyST_mainAll.SetAttribute("TBWDataList", lstrecordsmain);
            bodyST_mainAll.SetAttribute("CDN", System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString());
            sHtmls = bodyST_mainAll.ToString(); 
        
          
            if (sHtmls.Contains("src=\"/prodimages\""))
                sHtmls = sHtmls.Replace("src=\"/prodimages\"", "src=\"/images/noimage.gif\"");
            if (sHtmls.Contains("src=\"\""))
                sHtmls = sHtmls.Replace("src=\"\"", "src=\"/images/noimage.gif\"");
       
       
        return sHtmls;
     
    }



    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(";") + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
}
