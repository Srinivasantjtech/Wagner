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
using System.Data.Common;

using System.Web.Mail;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
public partial class ProductDetails : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    string stlistprodtitle = string.Empty;
    string stlistprod = "";
    string stcategory = "";
    string sProd = "";
    string sfamily = "";
    string stype = string.Empty;
    string stcategorylisttitle = string.Empty;
    string stcategorylistkey = string.Empty;
    string sbrand = string.Empty;
    string smodel = string.Empty;
    string ssize = string.Empty;
    string spower = string.Empty;
    string stitle = string.Empty;
    string skeyword = string.Empty;
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
    HelperServices objHelperServices = new HelperServices();
    ProductServices objProductServices = new ProductServices();


    //protected void Page_PreInit(object sender, EventArgs e)
    //{

    //    string newurl = Request.Url.PathAndQuery.Replace("/ProductDetails.aspx?", "").Replace("?", "");
    //    string Formname = Request.Url.LocalPath;

    //    string val = objHelperServices.URL_Rewrite_New(newurl, 1, "ProductDetails.aspx");
    //    if (val != string.Empty)
    //    {


    //        Context.RewritePath(val, false);
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorHandler objErrorHandler = new ErrorHandler();
        //sw.Start();
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString();
            //.Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        //sw.Stop();
        //Console.WriteLine("Elapsed={0}", sw.Elapsed);

        //StackTrace st = new StackTrace();
        //StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        //// objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
        //objErrorHandler.CreateTimeLog(); 
        Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
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

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {


        try
        {
            DataSet dscat = new DataSet();
            dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
            //Page.Title = "Cellink";
            DataTable dtprod = new DataTable();

            string urlstring = string.Empty;
            string productcode = string.Empty;
            string productid = string.Empty;
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
                       if (i==0)
                        {

                            h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                        }
                    }

                    else if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "family")
                    {

                        sfamily = ds.Tables[0].Rows[i]["FamilyName"].ToString();

                        h1.InnerText = objgetmetadata.Replace_SpecialChar(sfamily);

                    }


                    else if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "product")
                    {

                        productcode = ds.Tables[0].Rows[i]["Productcode"].ToString();
                        h4.InnerText = productcode;
                        productid = ds.Tables[0].Rows[i]["ItemValue"].ToString();
                    }

                }


                string title_key = objgetmetadata.FetchData(ds);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
                //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString(); ;
               // Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString(); ;
                Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString() + " | " + "Wagner Electronics, wagneronline.com.au";
                if (Session["FamilyProduct"] != null)
                {
                    DataSet dsdesc = (DataSet)Session["FamilyProduct"];
                    dtprod = dsdesc.Tables[0];
                    //if (dtprod != null && dtprod.Rows.Count > 0)
                    //{
                    //    if (dtprod.Rows[0]["PARENT_KEYWORD"].ToString() != "")
                    //        skeyword = skeyword + "," + dtprod.Rows[0]["PARENT_KEYWORD"].ToString();

                    //    if (dtprod.Rows[0]["SUB_KEYWORD"].ToString() != "")
                    //        skeyword = skeyword + "," + dtprod.Rows[0]["SUB_KEYWORD"].ToString();

                    //}
                }
                if (dscat != null)
                {
                    if(dscat.Tables.Contains("Product Tags") == true && dscat.Tables["Product Tags"].Rows.Count > 0)
                          skeyword = objHelperServices.MetaTagProductkeyword(dscat.Tables["Product Tags"]);
                }

                //if (ds != null && ds.Tables[0].Rows.Count > 2)
                //    productid = ds.Tables[0].Rows[2]["ItemValue"].ToString();

                if (productcode != "")
                    skeyword = skeyword + objProductServices.GetProductSortKeyCode(productid);

                string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);
                //Page.MetaKeywords = skeywordRe;
                Page.MetaKeywords = skeywordRe.Replace(","," |") + " - Wagner Electronics, wagneronline.com.au";


            }


            Page.MetaDescription = "List of products from Maincategory";


            Session["prodmodel"] = string.Empty;
            Session["S_FName"] = string.Empty;

            if (Session["FamilyProduct"] != null)
            {
                DataSet dsdesc = (DataSet)Session["FamilyProduct"];
                dtprod = dsdesc.Tables[0];
                string expression = "attribute_Name in ('Description','Prod_Desc','Descriptions','Short Description','Description 1','Features','Notes','Note') ";
                DataRow[] foundRows;
                foundRows = dtprod.Select(expression);
                string desc = string.Empty;
                string DescForh2 = string.Empty;
                for (int j = 0; j < foundRows.Length; j++)
                {

                    if ((foundRows[j]["attribute_Name"].ToString() == "Description") || (foundRows[j]["attribute_Name"].ToString() == "Descriptions"))
                    {
                        DescForh2 = foundRows[j][1].ToString();

                        h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                    }
                    if (j == 0)
                    {
                        if (foundRows[j]["attribute_Name"].ToString() == "Short Description")
                        {
                            DescForh2 = foundRows[j][1].ToString();
                            h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                        }
                        desc = foundRows[j][1].ToString();
                    }
                    else
                    {
                        if ((h2.InnerText == string.Empty) && (foundRows[j]["attribute_Name"].ToString() == "Short Description"))
                        {
                            DescForh2 = foundRows[j][1].ToString();
                            h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                        }

                        if (foundRows[j - 1]["STRING_VALUE"].ToString() != foundRows[j]["STRING_VALUE"].ToString())
                        {
                            if (desc != string.Empty)
                            {
                                desc = desc + ". " + foundRows[j][1].ToString();
                            }
                            else
                            {
                                desc = foundRows[j][1].ToString();

                            }
                        }
                    }

                }
                if (h2.InnerText == string.Empty)
                {
                    h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                }


                //Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);
                Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);

            }

            if (h1.InnerText == "")
            {
                h1.Visible = false;
            }
            if (h2.InnerText == "")
            {
                h2.Visible = false;
            }
            if (h3_2.InnerText == "")
            {
                h3_2.Visible = false;
            }
            if (h3_3.InnerText == "")
            {
                h3_3.Visible = false;
            }


        }
        catch
        { }


    }
}


