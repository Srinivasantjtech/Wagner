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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
using System.Web.Configuration;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using System.Data.SqlClient;
using System.Diagnostics;





public partial class PowerSearchPage : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
    string _AttType = "";
    string _AttValue = "";
    int _resultpage = 10;
    int _PageNo = 1;
    string ParentCatID = "";
    string _Brand = "";
    string _searchstr = "";
    Stopwatch sw = new Stopwatch();

    //protected void Page_PreInit(object sender, EventArgs e)
    //{

    //    string newurl = Request.Url.PathAndQuery.Replace("/ps.aspx?", "").Replace("?", "");
    //    string Formname = Request.Url.LocalPath;

    //    string val = objHelperServices.URL_Rewrite_New(newurl, 1, "ps.aspx");
    //    if (val != string.Empty)
    //    {


    //        Context.RewritePath(val, false);
    //    }


    //}
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string[] formurl = Request.Url.ToString().Split('?');
    //        string url = Request.Url.LocalPath + "?" + formurl[1];
    //        Session["TOPURL"] = Request.Url.PathAndQuery.ToString();
    //        string Formname = Request.Url.LocalPath;
    //        if (url.Contains('=') == false)
    //        {
    //            string newurl = HttpUtility.UrlDecode(Request.Url.Query.ToString().Replace("?", ""));
    //            string[] urlnew = newurl.Split('/');
               
    //            HttpContext.Current.Session["RECORDS_PER_PAGE_POWERSEARCH"] = "24";
               
    //            string Atrtype = string.Empty;
    //            string val = string.Empty;
    //            if (Request.Cookies["AtrType"] != null)
    //            {
    //                if (Request.Cookies["AtrType"].Value != null)
    //                {
    //                    Atrtype = Request.Cookies["AtrType"].Value;
    //                    if (Atrtype != "null")
    //                    {
    //                        val = objHelperServices.URL_Rewrite_New(newurl, 1, Request.Url.LocalPath, Atrtype);
    //                    }
    //                    else
    //                    {
    //                        val = objHelperServices.URL_Rewrite_New(newurl, 1, Request.Url.LocalPath);
    //                    }
    //                }
    //                Response.Cookies["AtrType"].Value = null;
    //                Response.Cookies["AtrType"].Expires = DateTime.Now.AddDays(-1d);

    //            }
    //            else
    //            {
    //                val = objHelperServices.URL_Rewrite_New(newurl, 1, Request.Url.LocalPath);
    //            }


    //            if ((val != string.Empty) && (val.Contains("ps.aspx")))
    //            {
    //                val = val.Replace("amp;", "");
    //                Context.RewritePath(val, false);
                   
    //            }
    //            else
    //            {
    //                string querystring = "&srctext=" + Request.Url.Query.Replace("?", "");
    //                val = Formname + "?" + querystring;
    //                HttpContext.Current.Session["TOPURL"] = Formname + "?" + querystring.Replace("&srctext=", "");
    //                Context.RewritePath(val, false);
    //            }
    //           // Session["PRVURL_SEARCH"] = val;
    //           // Session["orgurl_SEARCH"] = formurl[1];
    //           // Session["iPageNo_SEARCH"] = 1;
        

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // objErrorHandler.CreateLog(ex.ToString() + "ps");
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorHandler objErrorHandler = new ErrorHandler();
        //sw.Start();

        try
        {
        if (Session["USER_NAME"] == null)
        {
            Session["USER"] = "";
            Session["COUNT"] = "0";
            Response.Redirect("Login.aspx");
        }
        //***********************later want to be update with default page************//
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        Session["RECORDS_PER_PAGE"] = objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        Session["iPageNo"] = 1;
            //**********************End**************
        if(IsPostBack)        
        {
            if (Request.Form["ctl00$maincontent$searchctrl1$__EVENTTARGET1"].ToString() == "OrderDetails")                
                {
                    string MultipleItems = Request.Form["ctl00$maincontent$searchctrl1$__EVENTARGUMENT1"].ToString().Substring(0, Request.Form["ctl00$maincontent$searchctrl1$__EVENTARGUMENT1"].ToString().Length - 1);//.LastIndexOf("Fid".ToCharArray());
                    Session["Multipleitems"] = MultipleItems;
                    Response.Redirect("/OrderDetails.aspx");
                }           
        }
        //if (!IsPostBack)
        //{
        //    Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Cache.SetNoStore();
        //}
        if (Request.QueryString["type"] != null)
            _AttType = Request.QueryString["Type"];
        if (Request.QueryString["value"] != null)
            _AttValue = Request.QueryString["Value"];
        if (Request.QueryString["bname"] != null)
            _Brand = Request.QueryString["bname"];
        if (Request.QueryString["pgno"] != null)
            _PageNo = Convert.ToInt32(Request.QueryString["pgno"]);

        if (Request.QueryString["searchstr"] != null)
            _searchstr = Request.QueryString["searchstr"];

        if (Request.QueryString["srctext"] != null)
            _searchstr = Request.QueryString["srctext"];
        }
    
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        //sw.Stop();
        //Console.WriteLine("Elapsed={0}", sw.Elapsed);

        //StackTrace st = new StackTrace();
        //StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        //// objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
        //objErrorHandler.CreateTimeLog(); 
        //EasyAsk.GetAttributeProducts(_searchstr, _AttType, _AttValue, _Brand, _resultpage.ToString(), (_PageNo - 1).ToString(), "Next");
        //Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetNoStore();
    }

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {

        try
        {
           
            string urlstring = string.Empty;
            string skeyword = string.Empty;
            DataSet ds = (DataSet)Session["BreadCrumbDS"];
            if (Request.QueryString["srctext"] != null)
            {
                //Page.Title = "Cellink" + "-" + Request.QueryString["srctext"].ToString();
                Page.Title = Request.QueryString["srctext"].ToString();
                skeyword = Request.QueryString["srctext"].ToString();
                urlstring = skeyword;
                string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);

                Page.MetaKeywords = skeywordRe + " | " + "Wagner Electronics, wagneronline.com.au";
                Page.Title = skeyword + " | " + "Wagner Electronics, wagneronline.com.au";
                Page.MetaDescription = skeyword + ", Wagner Electronics, wagneronline.com.au";

                string srctext = Request.QueryString["srctext"].ToString();
                srctext = objgetmetadata.Replace_SpecialChar(srctext);
               

            }
            else if ( Request.QueryString["Value"] != null )
            {
                _AttValue = Request.QueryString["Value"];
                if (Request.QueryString["searchstr"] != null)
                    _searchstr = Request.QueryString["searchstr"];
                //Page.Title = "Cellink" + "-" + _AttValue;  
       
                Page.Title = _AttValue + " | " + "Wagner Electronics, wagneronline.com.au";
                Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(_AttValue);
                Page.MetaDescription = _AttValue + ", Wagner Electronics, wagneronline.com.au";
                if (_searchstr != "")
                {

                    //string title = "Cellink" + "-" + _AttValue + "-" + _searchstr;
                    string title = _AttValue + ", " + _searchstr;
                    Page.Title = title + " | " + "Wagner Electronics, wagneronline.com.au";
                    urlstring = _AttValue + "/" + _searchstr;
                    string skeywordattvalueandsearchstr = _searchstr + "," + _AttValue;
                    string skeywordattvalueandsearchstrRe = skeywordattvalueandsearchstr.Replace(",", ", ").Replace("&", "and").ToString();
                    Page.MetaKeywords = skeywordattvalueandsearchstrRe + " | " + "Wagner Electronics, wagneronline.com.au";
                    Page.MetaDescription = skeywordattvalueandsearchstrRe + ", Wagner Electronics, wagneronline.com.au";

                }



            }
            if (ds.Tables[0].Rows.Count > 2)
            {
                string title_key = objgetmetadata.FetchData(ds);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                string stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
                Page.Title = stitle + " | " + "Wagner Electronics, wagneronline.com.au";
                Page.MetaKeywords = skeyword + " | " + "Wagner Electronics, wagneronline.com.au";
                Page.MetaDescription = skeyword + ", Wagner Electronics, wagneronline.com.au";
            }
              


        }
        catch
        {

        }
    }

    [WebMethod]
    public static List<string> WestestAutoCompleteData(string strvalue)
    {
        List<string> result = new List<string>();
        ConnectionDB objConnectionDB = new ConnectionDB();
        OrderDB objOrderDB = new OrderDB();
        /* //SqlCommand objSqlCommand;
     
       //  using (objSqlCommand = new SqlCommand("select top 10 A.PRODUCT_ID,B.STRING_VALUE from TB_PRODUCT A,TB_PROD_SPECS B,TB_ATTRIBUTE C where A.PRODUCT_ID=B.PRODUCT_ID And B.ATTRIBUTE_ID=C.ATTRIBUTE_ID And C.PUBLISH2WEB=1 And C.ATTRIBUTE_ID=1  And  B.STRING_VALUE LIKE '%'+@STRING_VALUE+'%'", objConnectionDB.GetConnection()));
             using (objSqlCommand = new SqlCommand("select top 15 A.PRODUCT_ID,B.STRING_VALUE from TB_PRODUCT A,TB_PROD_SPECS B,TB_ATTRIBUTE C where A.PRODUCT_ID=B.PRODUCT_ID And B.ATTRIBUTE_ID=C.ATTRIBUTE_ID And C.PUBLISH2WEB=1 And C.ATTRIBUTE_ID=1  And  B.STRING_VALUE LIKE '%'+@STRING_VALUE+'%'", objConnectionDB.GetConnection())) ;
             {
                //objSqlCommand.CommandType = CommandType.Text;
                 objSqlCommand.Parameters.Add("@STRING_VALUE", strvalue);
                 SqlDataReader dr = objSqlCommand.ExecuteReader();
                 while (dr.Read())
                 {
                     result.Add(dr["STRING_VALUE"].ToString());
                 }
                 return result;
           //  }
         }
         * */
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

        DataTable Tbl = (DataTable)objOrderDB.GetGenericDataDB(WesCatalogId, strvalue, "GET_POWER_SEARCH_AUTO_COMPLETE_PRODUCT_TEST", OrderDB.ReturnType.RTTable);
        if (Tbl != null)
        {
            foreach (DataRow dr in Tbl.Rows)
            {
                result.Add(dr["ProductCode"].ToString());
            }
        }

        return result;

    }


    [System.Web.Services.WebMethod]
    public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string BCEAPath, string ViewMode, string irecords)
    {
        try
        {

            if (ipageno <= iTotalPages)
            {

                //ErrorHandler objErrorHandler = new ErrorHandler();
                //objErrorHandler.CreateLog("Inside psearch" + "dynamic page");
                string val = strvalue;


                HttpContext.Current.RewritePath(val, false);
                search_searchrsltproducts objnew = new search_searchrsltproducts();
                System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();

                eapath = eapath.Replace("###", "'");
               
                BCEAPath = BCEAPath.Replace("###", "'");
                getPostsText.Append(objnew.DynamicPag(val, ipageno, eapath, BCEAPath, ViewMode, irecords));
                
                //
              
                    return getPostsText.ToString();
             




            }
            else
            {

                return "";
            }


        }
        catch (Exception ex)
        {
            return ex.ToString();
        }




    }
}
