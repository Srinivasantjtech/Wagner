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
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
public partial class product_list : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
    HelperDB objhelperDb = new HelperDB();
    string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    string stlistprod = "";
    string stlistprodtitle = string.Empty;
    string sProd = "";
    string stcategory = "";
    string stcategorylisttitle = string.Empty;
    string stcategorylistkey = string.Empty;
    string stitle = string.Empty;
    string skeyword = string.Empty;

    //protected void Page_PreInit(object sender, EventArgs e)
    //{

    //    string[] formurl = Request.Url.ToString().Split('?');

    //    // string url = Request.Url.LocalPath + "?" + formurl[0];
    //    string newurl = Request.Url.PathAndQuery.Replace("/product_list.aspx?", "").Replace("?", "");
    //    string Formname = Request.Url.LocalPath;


    //    string val = objHelperServices.URL_Rewrite_New(newurl, 1, "product_list.aspx");
    //    if (val != string.Empty)
    //    {


    //        Context.RewritePath(val, false);
    //    }


    //}
   // Stopwatch sw = new Stopwatch();

    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string[] formurl = Request.Url.ToString().Split('?');

    //        string url = Request.Url.LocalPath + "?" + formurl[1];
    //        Session["TOPURL"] = Request.Url.PathAndQuery.ToString();
    //        string Formname = Request.Url.LocalPath;
    //        if (url.Contains("=") == false)
    //        {


    //            string newurl = HttpUtility.UrlDecode(Request.Url.Query.ToString().Replace("?", ""));



    //            string val = objHelperServices.URL_Rewrite_New(newurl, 1, Request.Url.LocalPath);
    //            if (val != string.Empty)
    //            {


    //                Context.RewritePath(val, false);
    //            }
    //            else
    //            {
    //                //objErrorHandler.CreateLog(newurl + "CategoryList");
    //                Response.Redirect("Home.aspx");
    //            }
    //        }
    //    }

    //    catch (Exception ex)
    //    {
    //        //  objErrorHandler.CreateLog(ex.ToString() + "CategoryList");
    //    }

    //}

    protected void Page_Load(object sender, EventArgs e)
    {
       // ErrorHandler objErrorHandler = new ErrorHandler();
       // sw.Start();
        //***********************later want to be update with default page************//

        try
        {
           
       //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        if(Session["RECORDS_PER_PAGE"] == null || Session["RECORDS_PER_PAGE"] == "")
            Session["RECORDS_PER_PAGE"] = objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        Session["iPageNo"] = 1;
            //**********************End**************
        if(IsPostBack)        
        {
            if (Request.Form["ctl00$maincontent$productlist1$__EVENTTARGET"].ToString() == "Compare")                
                {
                    string s = Request.Form["ctl00$maincontent$productlist1$__EVENTARGUMENT"].ToString().Substring(0, Request.Form["ctl00$maincontent$productlist1$__EVENTARGUMENT"].ToString().Length - 1);//.LastIndexOf("Fid".ToCharArray());
                    string[] str = Request.Form["ctl00$maincontent$productlist1$__EVENTARGUMENT"].Split('$');
                    int FamilyID = objHelperServices.CI(str[0]);
                    Session["CloseWin"] = str[1];
                    Session["FAMILY_ID"] = str[0];
                    Response.Redirect("Compare.aspx",false);
                }           
        }

        //if (!IsPostBack)
        //{
            //Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
        //}
        }

        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
       // sw.Stop();
        // Console.WriteLine("Elapsed={0}", sw.Elapsed);

        //StackTrace st = new StackTrace();
        //StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        //// objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
        //objErrorHandler.CreateTimeLog(); 
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

 
    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {
        try
        {
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
                                h3_2.InnerText = stcategory;
                            }
                            else
                            {

                                h3_3.InnerText = stcategory;
                            }

                        }
                        else
                        {
                           
                            stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                            pcatid=Get_MetaDescription(stcategory, "0");
                            stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                            h3_1.InnerText = stcategory;
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
                    Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString() + " | " + "Wagner Electronics, wagneronline.com.au";
                    if (dscat != null )
                    {
                        if (dscat.Tables.Contains("Product Tags") == true && dscat.Tables["Product Tags"].Rows.Count > 0)
                        skeyword = objHelperServices.MetaTagProductkeyword(dscat.Tables["Product Tags"]);
                    }
                   // Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword);

                    Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword).Replace(",", " |").ToString() + " - Wagner Electronics, wagneronline.com.au";



                }

                
            }

          // Page.MetaDescription = "List of products from Maincategory";

            if (Session["prodmodel"] != null)
            {
                string prodmodel = Session["prodmodel"].ToString();

                h3_3.InnerText = objgetmetadata.Replace_SpecialChar(prodmodel);
            }


            if (h3_2.InnerText == "")
            {

                h3_2.Visible = false;
            }
            if (h3_3.InnerText == "")
            {

                h3_3.Visible = false;
            }
            //stlistprod = objHelperServices.URLRewriteToAddressBar("product_list.aspx?" , urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_ProductList.ini"),false);
            //Session["PRODUCTLISTPARENT"] = stlistprod;
            //if (Page.Request.RawUrl.ToString().Contains("="))
            //{
            //    //stlistprod = objHelperServices.URLRewriteToAddressBar("product_list.aspx?" + urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_ProductList.ini"));
            //    //stlistprod = objHelperServices.URL_Rewrite_New(Request.Url.PathAndQuery.ToString(), 0); 
            //    //ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "processAjaxData( '" + stlistprod + "');", true);
            //}



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
                if (strvalue != null)
                {
                    string val = strvalue;
                    val = val.Replace("amp;", "");
                    HttpContext.Current.RewritePath(val, false);
                    search_searchrsltproductfamily objnew = new search_searchrsltproductfamily();
                    System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();
                    eapath = eapath.Replace("###", "'");
                    BCEAPath = BCEAPath.Replace("###", "'");
                    getPostsText.Append(objnew.DynamicPag(val, ipageno, eapath, BCEAPath, ViewMode, irecords));
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
}
