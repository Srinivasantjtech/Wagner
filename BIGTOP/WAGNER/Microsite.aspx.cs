using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Globalization;
public partial class Microsite : System.Web.UI.Page
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

    protected void Page_PreInit(object sender, EventArgs e)
    {

        //string newurl = Request.Url.PathAndQuery.Replace("/ct.aspx?", "").Replace("?", "");
        //string Formname = Request.Url.LocalPath;

        //string val = objHelperServices.URL_Rewrite_New(newurl, 1, "ct.aspx");
        //if (val != string.Empty)
        //{


        //    Context.RewritePath(val, false);
        //}
    }
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
    //                //objErrorHandler.CreateLog(newurl + "ct");
    //                Response.Redirect("/Home.aspx");
    //            }
    //        }
    //    }

    //    catch (Exception ex)
    //    {
    //        //  objErrorHandler.CreateLog(ex.ToString() + "ct");
    //    }

    //}
    protected void Page_Load(object sender, EventArgs e)
    {
         try
        {
            
        HelperServices objHelperServices = new HelperServices();
        //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Session["PageUrl"] = Request.RawUrl.ToString();
             Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        if (Session["RECORDS_PER_PAGE"] == null || Session["RECORDS_PER_PAGE"] == "")
            Session["RECORDS_PER_PAGE"] = objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Session["iPageNo"] = 1;
        if (!IsPostBack)
        {
            if (Request.QueryString["ActionResult"] != null && Request.QueryString["ActionResult"].ToString() == "CATALOGUE")
            {
                //multiTabs.ActiveViewIndex = 0;
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:__doPostBack('ctl00$maincontent$menuTabs','0');", true);
            }
            else if (Request.QueryString["ActionResult"] != null && Request.QueryString["ActionResult"].ToString() == "NEWS")
            {
                //multiTabs.ActiveViewIndex = 1;
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:__doPostBack('ctl00$maincontent$menuTabs','1');", true);
            }
            //string _ViewType = string.Empty ;
            //if (Request.Cookies["PLVIEWMODE"] != null)
            //{
            //    if (Request.Cookies["PLVIEWMODE"].Value != null)
            //    {
            //        _ViewType = Request.Cookies["PLVIEWMODE"].Value;

            //    }
            //}
            //else
            //{
            //    _ViewType = Request.Cookies["PLVIEWMODE"].Value;
            
            //}
            //Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
        }

        }


         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();

         }
    }


    [System.Web.Services.WebMethod]
    public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string BCEAPath, string ViewMode, string irecords, string balrecords)
    {
        try
        {
            if (ipageno <= iTotalPages)
            {


                

                string val = strvalue;
                HttpContext.Current.RewritePath(val, false);
                UC_categorylist objnew = new UC_categorylist();
                System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();
                eapath = eapath.Replace("###", "'");
                BCEAPath = BCEAPath.Replace("###", "'");
                getPostsText.Append(objnew.DynamicPag_RenderHTMLjson(val, ipageno, eapath, BCEAPath, ViewMode, irecords, balrecords));
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
                        //Page.MetaDescription = metades;
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
            if (Request.QueryString["tsb"] == null)
            {

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
                        //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString(); ;
                       // Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString(); 
                        Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString() ;
                       // Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword);
                        Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword) + " - Wagner Electronics, wagneronline.com.au";



                    }

                }

               // Page.MetaDescription = "List of products from Maincategory";

                if (Session["prodmodel"] != null)
                {
                    string prodmodel = Session["prodmodel"].ToString();

                    //h3_3.InnerText = objgetmetadata.Replace_SpecialChar(prodmodel);
                }


                //if (h3_2.InnerText == "")
                //{

                //    h3_2.Visible = false;
                //}
                //if (h3_3.InnerText == "")
                //{

                //    h3_3.Visible = false;
                //}
            }
            else
            {

                string pagetitle = "";

                if ((DataSet)Session["BreadCrumbDS"] != null)
                {
                    DataSet ds = (DataSet)Session["BreadCrumbDS"];

                    if (ds.Tables[0].Rows[0]["ItemType"].ToString().ToLower() == "category")
                    {

                        //h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                        pagetitle = objgetmetadata.Replace_SpecialChar(ds.Tables[0].Rows[0]["ItemType"].ToString());
                    }
                    if (ds.Tables[0].Rows[1]["ItemType"].ToString().ToLower() == "brand")
                    {



                        stcategory = ds.Tables[0].Rows[1]["Itemvalue"].ToString();
                        stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                        //h2.InnerText = ds.Tables[0].Rows[1]["Itemvalue"].ToString();
                       // Page.Title = stcategory + " " + pagetitle;
                        Page.Title = stcategory + " " + pagetitle ;
                      //  Page.MetaKeywords = pagetitle + "," + stcategory;
                       // Page.MetaDescription = stcategory + "," + "Cellular Phone Models and Accessories";

                        Page.MetaKeywords = pagetitle + " | " + stcategory + " - Wagner Electronics, wagneronline.com.au";
                    }

                }
            }

            string pagetitle1 = Page.Title.ToLower();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string TextTitle = textInfo.ToTitleCase(pagetitle1);
            Page.Title = TextTitle + " | " + "Wagner Electronics"; ;   
        }
        catch
        { }

    }

}
