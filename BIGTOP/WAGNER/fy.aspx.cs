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
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;     
public partial class family : System.Web.UI.Page
{

    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
    string stlistprod = string.Empty;
    string stcategory = string.Empty;
    string stcategorylisttitle = string.Empty;
    string stcategorylistkey = string.Empty;
    string sfamily = string.Empty;
    string stype = string.Empty;

    string sbrand = string.Empty;
    string smodel = string.Empty;
    string ssize = string.Empty;
    string spower = string.Empty;
    string stitle = string.Empty;
    string skeyword = string.Empty;
  //  Stopwatch sw = new Stopwatch();

    //protected void Page_PreInit(object sender, EventArgs e)
    //{

    //    string newurl = Request.Url.PathAndQuery.Replace("/fy.aspx?", "").Replace("?", "");
    //    string Formname = Request.Url.LocalPath;

    //    string val = objHelperServices.URL_Rewrite_New(newurl, 1, "fy.aspx");
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
      //  ErrorHandler objErrorHandler = new ErrorHandler();
       // sw.Start();
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString();//.Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();        
       // Session["PageUrl"] = "fy.aspx?fid=" + Request["fid"];
       // sw.Stop();
       // Console.WriteLine("Elapsed={0}", sw.Elapsed);

      //  StackTrace st = new StackTrace();
       // StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        // objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
       // objErrorHandler.CreateTimeLog(); 
        Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
    }

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {

        try
        {

            string Fname = "";
            string CFname = "";
            string urlstring = "";
            //Page.Title = "Cellink";
     

            if (Session["BreadCrumbDS"] != null)
            {

                DataSet ds = (DataSet)Session["BreadCrumbDS"];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "category")
                    {
                        if (i != 0)
                        {

                            stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

                            stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
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
                        if (i == 0)
                        {
                            h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                        
                        }
                    }
                    else if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "family")
                    {

                        sfamily = ds.Tables[0].Rows[i]["FamilyName"].ToString();

                        h1.InnerText = objgetmetadata.Replace_SpecialChar(sfamily);

                    }


                }

                string title_key = objgetmetadata.FetchData(ds);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
                //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString();
               // Page.Title = stitle.Replace("<ars>g</ars>", "-").ToString();
                Page.Title = stitle.Replace("<ars>g</ars>", "-").ToString() + " | " + "Wagner Electronics, wagneronline.com.au";
                string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);
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
                                skeywordRe = skeywordRe + "," + strkeyword1;
                            }

                        }
                    }
                }
               // Page.MetaKeywords = skeywordRe;
                Page.MetaKeywords = skeywordRe.Replace(",", " |").ToString() + " - Wagner Electronics, wagneronline.com.au";
                string shtdesc = string.Empty;
                string desc = string.Empty;
                if (Session["FamilyProduct"] != null)
                {
                    DataSet dsdesc = (DataSet)Session["FamilyProduct"];

                    DataTable dtprod = new DataTable();

                    dtprod = dsdesc.Tables[0];
                    string expression = "attribute_Name in ('Short Description','Description','Features','Descriptions','prod_dsc','Notes','Note') ";
                    DataRow[] foundRows;
                    foundRows = dtprod.Select(expression);

                    for (int j = 0; j < foundRows.Length; j++)
                    {
                        if (j == 0)
                        {
                            if (foundRows[j]["attribute_Name"].ToString() == "Short Description")
                            {
                                string h2desc = foundRows[j]["STRING_VALUE"].ToString();
                                h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                            }
                            desc = foundRows[j]["STRING_VALUE"].ToString();

                        }
                        else
                        {
                            if ((h2.InnerText == string.Empty) && (foundRows[j]["attribute_Name"].ToString() == "Short Description"))
                            {
                                string h2desc = foundRows[j]["STRING_VALUE"].ToString();

                                h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                            }
                            if (foundRows[j - 1]["STRING_VALUE"].ToString() != foundRows[j]["STRING_VALUE"].ToString())
                            {
                                if (desc != string.Empty)
                                {
                                    desc = desc + ". " + foundRows[j]["STRING_VALUE"].ToString();
                                }
                                else
                                {
                                    desc = foundRows[j]["STRING_VALUE"].ToString();
                                }
                            }
                        }

                    }

                   // Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);
                    Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);


                }
                if (h2.InnerText == string.Empty)
                {
                    h2.InnerText = Page.MetaDescription;
                }


                if (h3_2.InnerText == "")
                {

                    h3_2.Visible = false;
                }
                if (h3_3.InnerText == "")
                {

                    h3_3.Visible = false;
                }
                if (h2.InnerText == string.Empty)
                {

                    h2.Visible = false;
                }
            }
            //stlistprod = objHelperServices.URLRewriteToAddressBar("fy.aspx?" ,urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_Family.ini"),false);

            //if (Page.Request.RawUrl.ToString().Contains("="))
            //{
            //    stlistprod = objHelperServices.URLRewriteToAddressBar("fy.aspx?" + urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_Family.ini"));
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "processAjaxData( '" + stlistprod + "');", true);
            //}
            //else
            //{
            //  string[]  PARENTFAMILY= Page.Request.RawUrl.Split(new string[] { "?" }, StringSplitOptions.None);
            //  Session["PARENTFAMILY"] = PARENTFAMILY[1];

            //}
            string[] PARENTFAMILY = Page.Request.RawUrl.Split(new string[] { "?" }, StringSplitOptions.None);
            Session["PARENTFAMILY"] = PARENTFAMILY[1];
        }
        catch
        { }







    }
}
