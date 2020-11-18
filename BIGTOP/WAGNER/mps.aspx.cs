using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.EasyAsk;
using System.Data;
using System.Web.Services;


using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
using TradingBell5.CatalogX;

using System.Web.Configuration;
using System.Configuration;
using System.Globalization;
using TradingBell.WebCat.TemplateRender; 
namespace WES
{
    public partial class mps : System.Web.UI.Page
    {
        EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        string MicroSiteTemplate = "";
        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
        string _AttType = "";
        string _AttValue = "";
        int _resultpage = 10;
        int _PageNo = 1;
        string ParentCatID = "";
        string _Brand = "";
        string _searchstr = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"]);

            Session["PageUrl"] = Request.RawUrl.ToString();

        }
      
        public string Bread_Crumbs_MS(Boolean withhome)
        {
            string breadcrumb = "";
            //if (Request.QueryString["pid"] != null)
            //{
            //    paraPID = Request.QueryString["pid"].ToString();
            //}
            //if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "List all products")
            //    paraFID = Request.QueryString["fid"].ToString();
            //if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "List all models")
            //    paraCID = Request.QueryString["cid"].ToString();
            //if (Request.QueryString["byp"] != null && Request.QueryString["byp"].ToString() != "")
            //    byp = Request.QueryString["byp"].ToString();

            //if (Request.QueryString["cid"] != null)
            //    catID = Request.QueryString["cid"].ToString();
            try
            {
                breadcrumb = ObjEasyAsk.GetBreadCrumb_Simple_MS(MicroSiteTemplate, withhome);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            return breadcrumb;

        }
        
        public void GetSupplierdetail()
        {
            if (strsupplierName == "")
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);
                DataTable ttbl = tbwtMSEngine.GetSupplierDetail();
                if (ttbl != null && ttbl.Rows.Count > 0)
                {
                    strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
                    strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
                    strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();
                }
            }

            

        }
        public string GetSupplierName()
        {
            GetSupplierdetail();
            return strsupplierName;

        }
        public string GetSupplierDesc()
        {

            GetSupplierdetail();
            return strsupplierDesc;
        }
        public string ST_NewProduct()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_NewProduct_Load();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {

            try
            {

                string urlstring = string.Empty;
                if (strsupplierName == "")
                {
                    strsupplierName = GetSupplierName();
                }
                string skeyword = string.Empty;
                DataSet ds = (DataSet)Session["BreadCrumbDS"];
                if (Request.QueryString["srctext"] != null)
                {
                    //Page.Title = "Cellink" + "-" + Request.QueryString["srctext"].ToString();
                    Page.Title = Request.QueryString["srctext"].ToString();
                    skeyword = Request.QueryString["srctext"].ToString();
                    urlstring = skeyword;
                    string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);

                    Page.MetaKeywords = skeywordRe + "|" + strsupplierName + " | " + "Wagner Electronics, wagneronline.com.au";
                    Page.Title = skeyword.ToUpper();
                    Page.MetaDescription = skeyword +"," + strsupplierName + ", Wagner Electronics, wagneronline.com.au";

                    string srctext = Request.QueryString["srctext"].ToString();
                    srctext = objgetmetadata.Replace_SpecialChar(srctext);


                }
                else if (Request.QueryString["Value"] != null)
                {
                    _AttValue = Request.QueryString["Value"];
                    if (Request.QueryString["searchstr"] != null)
                        _searchstr = Request.QueryString["searchstr"];
                    //Page.Title = "Cellink" + "-" + _AttValue;  

                    Page.Title = _AttValue;
                    Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(_AttValue);
                    Page.MetaDescription = _AttValue + ", Wagner Electronics, wagneronline.com.au";
                    if (_searchstr != "")
                    {

                        //string title = "Cellink" + "-" + _AttValue + "-" + _searchstr;
                        string title = _AttValue + ", " + _searchstr;
                        Page.Title = title;
                        urlstring = _AttValue + "/" + _searchstr;
                        string skeywordattvalueandsearchstr = _searchstr + "," + _AttValue;
                        string skeywordattvalueandsearchstrRe = skeywordattvalueandsearchstr.Replace(",", ", ").Replace("&", "and").ToString();
                        Page.MetaKeywords = skeywordattvalueandsearchstrRe + "|"+strsupplierName+" | " + " | " + "Wagner Electronics, wagneronline.com.au";
                        Page.MetaDescription = skeywordattvalueandsearchstrRe + "," + strsupplierName + ", Wagner Electronics, wagneronline.com.au";

                    }



                }
                if (ds.Tables[0].Rows.Count > 2)
                {
                    string title_key = objgetmetadata.FetchData(ds);
                    string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                    string stitle = StrValues[0];
                    skeyword = StrValues[1];
                    urlstring = StrValues[2];
                    Page.Title = stitle;
                    Page.MetaKeywords = skeyword + "|"+strsupplierName+" | " + "Wagner Electronics, wagneronline.com.au";
                    Page.MetaDescription = skeyword + "," + strsupplierName + ", Wagner Electronics, wagneronline.com.au";
                }

                string pagetitle = Page.Title.ToLower();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
             

                if (pagetitle == "")
                {
                    pagetitle = skeyword + "," + strsupplierName;
                }
                else 
                {

                    pagetitle = pagetitle + "," + strsupplierName;
                }
                string TextTitle = textInfo.ToTitleCase(pagetitle);
                // Page.Title = TextTitle + " | " + "Wagner Electronics";   
                TextTitle = objgetmetadata.SetTitleCount(TextTitle);
                Page.Title = TextTitle;
  
            }
            catch
            {

            }
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
                    PowerSearchAndBM objnew = new PowerSearchAndBM();
                    System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();

                    eapath = eapath.Replace("###", "'");

                    BCEAPath = BCEAPath.Replace("###", "'");
                    getPostsText.Append(objnew.DynamicPagJson(val, ipageno, eapath, BCEAPath, ViewMode, irecords));

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
        [System.Web.Services.WebMethod]
        public static string SetViewType(string viewtype)
        {


         
                HttpContext.Current.Session["PL_VIEW_MODE"] = viewtype;
         
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string Assignattr(string strvalue)
        {



            HttpContext.Current.Session["hfclickedattr_mps"] = strvalue;

            return "";
        }



        [System.Web.Services.WebMethod]
        public static string SetSortOrder(string orderVal, string url)
        {

            string rtn = "-1";
            if (orderVal.ToLower() == "latest")
            {
                if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "Latest")
                {
                    HttpContext.Current.Session["SortOrder"] = "Latest";
                    rtn = "1";
                }

            }
            else if (orderVal.ToLower() == "ltoh")
            {
                if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "ltoh")
                {
                    HttpContext.Current.Session["SortOrder"] = "ltoh";
                    rtn = "1";
                }

            }
            else if (orderVal.ToLower() == "htol")
            {
                if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "htol")
                {

                    HttpContext.Current.Session["SortOrder"] = "htol";
                    rtn = "1";
                }
            }

            else if (orderVal.ToLower() == "relevance")
            {
                if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "relevance")
                {

                    HttpContext.Current.Session["SortOrder"] = "relevance";
                    rtn = "1";
                }
            }

            else if (orderVal.ToLower() == "popularity")
            {
                if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "popularity")
                {
                    HttpContext.Current.Session["SortOrder"] = "popularity";
                    rtn = "1";
                }
            }
            if (url.Contains("/mps/") == true)
            {
                HttpContext.Current.Session["SortOrder_mps"] = "SortOrder_mps";
            }
            else
            {
                HttpContext.Current.Session["SortOrder_mps"] = "SortOrder_mop";
            }
            return rtn;
        }
    }
       
}