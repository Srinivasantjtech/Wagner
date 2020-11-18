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

using TradingBell5.CatalogX;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Web.Services;
using System.Web.Configuration;
using System.Configuration;
using System.Globalization; 
namespace WES
{
    public partial class mpl : System.Web.UI.Page
    {
        EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
        HelperDB objhelperDb = new HelperDB();
        string MicroSiteTemplate = "";
        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        string stlistprod = "";
        string stlistprodtitle = string.Empty;
        string sProd = "";
        string stcategory = "";
        string stcategorylisttitle = string.Empty;
        string stcategorylistkey = string.Empty;
        string stitle = string.Empty;
        string skeyword = string.Empty;
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];

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
                if (strsupplierName == "")
                {
                    strsupplierName = GetSupplierName();
                }
                DataSet dscat = new DataSet();
                dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                string urlstring = string.Empty;
                string pcatid = string.Empty;
                //Page.Title = "Cellink";
                if (Session["EA"] != null)
                {
                    string EA = Session["EA"].ToString();
                    DataSet ds = (DataSet)Session["BreadCrumbDS"];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "category")
                        {

                            if (i != 0)
                            {

                                stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                                pcatid = Get_MetaDescription(stcategory, pcatid);
                                stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                                Session["prodlisttitle"] = stcategory;
                                Session["prodlistname"] = stcategory;
                                if (stcategorylisttitle == string.Empty)
                                {
                                    stcategorylisttitle = stcategory;
                                    //h3_2.InnerText = stcategory;
                                }
                                else
                                {

                                    //h3_3.InnerText = stcategory;
                                }

                            }
                            else
                            {

                                stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                                pcatid = Get_MetaDescription(stcategory, "0");
                                stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                                //h3_1.InnerText = stcategory;
                            }
                        }

                    }


                    string title_key = objgetmetadata.FetchData(ds);
                    if (title_key != "|")
                    {
                        string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                        stitle = StrValues[0];
                        skeyword = StrValues[1];
                        urlstring = StrValues[2];

                        // Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString(); ;
                        Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString();
                        if (dscat != null)
                        {
                            if (dscat.Tables.Contains("Product Tags") == true && dscat.Tables["Product Tags"].Rows.Count > 0)
                                skeyword = objHelperServices.MetaTagProductkeyword(dscat.Tables["Product Tags"]);
                        }
                        // Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword);

                        Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword).Replace(",", " |").ToString() +"|"+ strsupplierName + " - Wagner Electronics, wagneronline.com.au";



                    }


                }

                // Page.MetaDescription = "List of products from Maincategory";

                if (Session["prodmodel"] != null)
                {
                    string prodmodel = Session["prodmodel"].ToString();

                    //h3_3.InnerText = objgetmetadata.Replace_SpecialChar(prodmodel);
                }

                string pagetitle = stitle.ToLower();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string TextTitle = textInfo.ToTitleCase(pagetitle);
                //Page.Title = TextTitle + " | " + "Wagner Electronics";  
                TextTitle = objgetmetadata.SetTitleCount(TextTitle);
                Page.Title = TextTitle;
                //if (h3_2.InnerText == "")
                //{

                //    h3_2.Visible = false;
                //}
                //if (h3_3.InnerText == "")
                //{

                //    h3_3.Visible = false;
                //}
                //stlistprod = objHelperServices.URLRewriteToAddressBar("pl.aspx?" , urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_ProductList.ini"),false);
                //Session["PRODUCTLISTPARENT"] = stlistprod;
                //if (Page.Request.RawUrl.ToString().Contains("="))
                //{
                //    //stlistprod = objHelperServices.URLRewriteToAddressBar("pl.aspx?" + urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_ProductList.ini"));
                //    //stlistprod = objHelperServices.URL_Rewrite_New(Request.Url.PathAndQuery.ToString(), 0); 
                //    //ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "processAjaxData( '" + stlistprod + "');", true);
                //}



            }

            catch

            { }
        }

        private string Get_MetaDescription(string stcategory, string parentcatid)
        {

            string catid = string.Empty;
            try
            {


                DataTable Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, stcategory, parentcatid, "GET_CAT_DETAILS", HelperDB.ReturnType.RTTable);
                if (Sqltb != null)
                {
                    if (Sqltb.Rows[0][1] != null)
                    {
                        string metades = Sqltb.Rows[0][1].ToString();

                        if (metades != "")
                        {
                            metades = objgetmetadata.Replace_SpecialChar(metades);
                            // Page.MetaDescription = metades;
                            Page.MetaDescription = metades;
                        }
                    }
                    if (Sqltb.Rows[0][0] != null)
                    {
                        catid = Sqltb.Rows[0][0].ToString();
                        return catid;
                    }
                }
            }
            catch
            {

            }

            return catid;
        }
        [System.Web.Services.WebMethod]
        public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string BCEAPath, string ViewMode, string irecords)
        {
            try
            {
                if (ipageno <= iTotalPages)
                {
                    if (strvalue != null)
                    {
                        string val = strvalue;
                        val = val.Replace("amp;", "");
                        HttpContext.Current.RewritePath(val, false);
                        ProductListMS objnew = new ProductListMS();
                        System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();
                        eapath = eapath.Replace("###", "'");
                        BCEAPath = BCEAPath.Replace("###", "'");
                        getPostsText.Append(objnew.DynamicPagJson(val, ipageno, eapath, BCEAPath, ViewMode, irecords));
                        return getPostsText.ToString();
                    }
                    else
                    {
                        return "";
                    }
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
        public static string Assignds(string strvalue)
        {



           HttpContext.Current.Session["hfclickedattr_top_temp"] = strvalue;
        
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string Assignattr(string strvalue)
        {



            HttpContext.Current.Session["hfclickedattr_mpl"] = strvalue;

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