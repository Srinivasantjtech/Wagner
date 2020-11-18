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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
using System.Globalization;
public partial class bb : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    string stitle = string.Empty;
    string skeyword = string.Empty;
    HelperServices objHelperServices = new HelperServices();
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();

    //protected void Page_PreInit(object sender, EventArgs e)
    //{

    //    string newurl = Request.Url.PathAndQuery.Replace("/bb.aspx?", "").Replace("?", "");
    //    string Formname = Request.Url.LocalPath;

    //    string val = objHelperServices.URL_Rewrite_New(newurl, 1, "bb.aspx");
    //    if (val != string.Empty)
    //    {


    //        Context.RewritePath(val, false);
    //    }


    //}
    //Stopwatch sw = new Stopwatch();
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
          
    //            int lng = 0;
    //            if (urlnew.Length >= 2)
    //            {
    //                lng = urlnew.Length - 2;
    //            }
              
    //            HttpContext.Current.Session["RECORDS_PER_PAGE_BYNRAND"] = "10";
              
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

    //            if (val != string.Empty)
    //            {
    //                val = val.Replace("amp;", "");
    //                Context.RewritePath(val, false);

    //                Session["PRVURL_BRAND"] = val;
    //                Session["orgurl_BRAND"] = formurl[1];

    //                Session["iPageNo_BRAND"] = 1;
                  
    //            }
    //            else
    //            {
                  
    //                Response.Redirect("/Home.aspx");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorHandler objErrorHandler = new ErrorHandler();
        //sw.Start();
        //***********************later want to be update with default page************//
        try
        {
       // Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString();
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        if(Session["RECORDS_PER_PAGE"] == null || Session["RECORDS_PER_PAGE"] == string.Empty)
            Session["RECORDS_PER_PAGE"] = objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        //**********************End**************
        if(IsPostBack)        
        {
                if (Request.Form["__EVENTTARGET"].ToString()=="compare,")                
                {
                    string s = Request.Form["__EVENTARGUMENT"].ToString().Substring(0,Request.Form["__EVENTARGUMENT"].ToString().Length-1);//.LastIndexOf("Fid".ToCharArray());
                    string[] str = Request.Form["__EVENTARGUMENT"].Split('$');
                    int FamilyID = objHelperServices.CI(str[0]);
                    Session["CloseWin"] = str[1];
                    Session["FAMILY_ID"] = str[0];
                    Response.Redirect("/Compare.aspx",false);
                }           
        }
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }

        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
           
        }

        Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        //sw.Stop();
        //Console.WriteLine("Elapsed={0}", sw.Elapsed);

        //StackTrace st = new StackTrace();
        //StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        //// objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
        //objErrorHandler.CreateTimeLog(); 
    }
    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {
        try
        {

            string prodmodel = string.Empty;
            string prodname = string.Empty;
            string prodlistname = string.Empty;
            string urlstring = string.Empty;
            
           
           
            if (Session["prodlistname"] != null)
            {
                Session["prodlisttitle"] = Session["prodlistname"].ToString();
                prodname = Session["prodlistname"].ToString();
                Page.Title = Session["prodlisttitle"].ToString();
                prodlistname = Session["prodlistname"].ToString();
                //h2.InnerText = objgetmetadata.Replace_SpecialChar(prodlistname);
            }
            if (Session["prodmodel"] != null)
            {
                prodmodel = Session["prodmodel"].ToString();
                prodmodel = objgetmetadata.Replace_SpecialChar(prodmodel);
                Page.Title = prodmodel + " " + Session["prodlisttitle"];
                //h1.InnerText = prodmodel;
            }
            if (Session["BreadCrumbDS"] != null)
            {
                DataSet dsbc = (DataSet)Session["BreadCrumbDS"];
                string title_key = objgetmetadata.FetchData(dsbc);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
            }
         
          //  Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString();
            Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString() + " | " + "Wagner Electronics";
            skeyword=objgetmetadata.Replace_SpecialChar(skeyword);
             if (HttpContext.Current.Session["LHSAttributes"] != null)
                {
                    DataSet dsproductattr = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                    if (dsproductattr != null)
                    {
                        if (dsproductattr.Tables.Contains("Product Tags"))
                        {
                            if (dsproductattr.Tables["Product Tags"].Rows.Count > 0)
                            {
                                string strkeyword1 = objHelperServices.MetaTagProductkeyword(dsproductattr.Tables["Product Tags"]);
                                skeyword = skeyword + "," + strkeyword1;
                            }

                        }
                    }
                }
            // Page.MetaKeywords = skeyword;
             Page.MetaKeywords = skeyword.Replace(",", " |") + " - Wagner Electronics, wagneronline.com.au";
             if (prodmodel != "")
             {
                 prodmodel = prodmodel + ",";
             }
             if (prodname != "")
             {
                 prodname = prodname + ",";
             }
            string Allmetadesc = prodmodel +  prodname +  "Cellular Phone Models and Accessories";
          //  Page.MetaDescription = objgetmetadata.Replace_SpecialChar(Allmetadesc);
            Page.MetaDescription = objgetmetadata.Replace_SpecialChar(Allmetadesc);
            Session["prodmodel"] = string.Empty;
            //if (h2.InnerText == "")
            //{
            //    h2.Visible = false;
            //}
            //if (h1.InnerText == "")
            //{
            //    h1.Visible = false;
            //}
            string pagetitle = Page.Title.ToLower();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string TextTitle = textInfo.ToTitleCase(pagetitle);
            Page.Title = TextTitle.Replace("| Wagner Electronics","| Wagner Online Store");   
        }
        catch
        { }
    }

    [System.Web.Services.WebMethod]
    public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string BCEAPath, string ViewMode, string irecords)
    {
        try
        {
          

            if (ipageno <= iTotalPages)
            {
              
                    int iPageNo = ipageno;
                  
                    if (strvalue != null)
                    {

                        string val = strvalue;
                        val = val.Replace("amp;", "");
                        HttpContext.Current.RewritePath(val, false);
                        search_searchrsltproducts objnew = new search_searchrsltproducts();
                        System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();
                        eapath = eapath.Replace("###", "'");
                        BCEAPath = BCEAPath.Replace("###", "'");
                        getPostsText.Append(objnew.DynamicPag_BrandAndModelProductListNewJson(val,iPageNo, eapath, BCEAPath,ViewMode,irecords  ));
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
            return "";
        }

    


    }


    [System.Web.Services.WebMethod]
    public static string Assignattr(string strvalue)
    {



        HttpContext.Current.Session["hfclickedattr_bb"] = strvalue;

        return "";
    }
}
