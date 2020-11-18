using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
public partial class Mainpage : System.Web.UI.MasterPage
{
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();
    public string Cartdetail = "";
    public string CartCount = "";
    public string CartCount_mobile = "";
    public string Cartdetail_mobile = "";
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.Url.ToString().Contains("ct.aspx"))
        {
            //Page.ClientScript.RegisterClientScriptInclude("css", ResolveUrl(currenturl+"/css/Wag_AllCss_1.css"));

        }
        //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
        //{
        //    int iTotalPages = 0;


        //    if (Session["iTotalPages"] != null)
        //    {
        //        iTotalPages = Convert.ToInt32(Session["iTotalPages"]);


        //    }

        //    if (Request.Url.ToString().Contains("pageno") == true)
        //    {
        //        string querystring = Request.RawUrl.ToString();
        //        string[] checkurl = querystring.ToString().Replace("/ct/", "").Replace("/pl/", "").Replace("/bb/", "").Split('/');
        //        string pno = checkurl[checkurl.Length - 1].ToString().Replace("pageno-", "");
        //        int iPageNo = Convert.ToInt32(pno);
        //        int inexturl = iPageNo + 1;
        //        int iprevurl = iPageNo - 1;

              
        //        if (iTotalPages > 0 && inexturl < iTotalPages)
        //        {
        //            string nexturl = querystring.Replace("pageno-" + pno, "pageno-" + inexturl.ToString());
        //            next.Href = nexturl;
        //        }
               
        //        string prevurl = querystring.Replace("pageno-" + pno, "pageno-" + iprevurl.ToString());
        //        prev.Href = prevurl;

        //    }
        //    else
        //    {
        //        string querystring = Request.RawUrl.ToString();
        //        string[] checkurl = querystring.ToString().Split('/');
        //        string pagename = checkurl[checkurl.Length - 2]+"/";
        //        string newquerystring = querystring.ToString().Replace(pagename, "");
          
        //        if (iTotalPages > 1)
        //        {
        //            string nexturl = newquerystring + "pageno-2" +"/"+ pagename;
        //            next.Href = nexturl;
                   
        //        }
               
        //    }
         
        //}
        //string urlpathquery = string.Empty;
        //urlpathquery = HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower();

        //if (urlpathquery.Contains("ct.aspx")
        //       || urlpathquery.Contains("microsite.aspx")
        //     || urlpathquery.Contains("fl.aspx")
        //    || urlpathquery.Contains("pl.aspx")
        //     || urlpathquery.Contains("pd.aspx")
        //      || urlpathquery.Contains("ps.aspx")
        //      || urlpathquery.Contains("sitemap.aspx")
        //                 || urlpathquery.ToLower().Contains("bb.aspx")
        //      || urlpathquery.Contains("browsekeyword.aspx")
        //       || urlpathquery.Contains("browseproducttag.aspx")
        //        || urlpathquery.Contains("onlinecatalogue.aspx")
        //     )
        //{


        //    SeoTag.Href = currenturl.Substring(0, currenturl.Length - 1).Replace("https", "http") + Request.RawUrl.ToString();

        //}

        //else
        //{
        //    string vnum = System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString();
        //    HtmlLink linkcritical = new HtmlLink();
        //    linkcritical.Attributes.Add("rel", "stylesheet");
        //    linkcritical.Attributes.Add("type", "text/css");
        //    linkcritical.Attributes.Add("media", "screen");
        //    linkcritical.Href = currenturl + "css/Critical.css?v=" + vnum;
        //    header.Controls.Add(linkcritical);


        //}


        ////if (urlpathquery.Contains("sitemap.aspx")
        ////   || urlpathquery.Contains("browsekeyword.aspx")
        ////    || urlpathquery.Contains("browseproducttag.aspx"))
        ////{
        ////    HtmlLink linkcritical = new HtmlLink();
        ////    linkcritical.Href = currenturl + "css/Critical.css";
        ////    linkcritical.Attributes.Add("type", "text/css");
        ////    linkcritical.Attributes.Add("rel", "stylesheet");
        ////    header.Controls.Add(linkcritical);

        ////}
        //HtmlLink seoTag = new HtmlLink();
        //seoTag.Attributes.Add("rel", "canonical");
        //seoTag.Href = "http://www.erate.co.za/";
        //Header.Controls.Add(seoTag);
        loadheader();
        loadmaincontent();
        //Modified by:indu
        string requrl = Request.Url.ToString().ToLower();

        if (!(requrl.Contains("login.aspx")) && !(requrl.Contains("onlinecatalogue.aspx")))
        {
            loadleftnav();
            if (!(requrl.Contains("pd.aspx")) && !(requrl.Contains("ct.aspx")) && !(requrl.Contains("pl.aspx")))
            {
                loadrightnav();
            }
        }
        loadfooter();
        //if (HttpContext.Current.Session["URLINI"] == null)
        //{
        //    objHelperServices.Createnewdt();
        //}

    }
    private void loadheader()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\header.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
           // if (str[strc].ToUpper() == "TOP")
           // {
           //     if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == string.Empty || HttpContext.Current.Session["USER_ID"].ToString() == ConfigurationManager.AppSettings["DUM_USER_ID"].ToString())
           //     {
                    str[strc] = "toplog";
            //    }
          //  }

            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            header.Controls.Add(ctl);
        }
    }

    public string ST_prev_next()
    {
        string returnval=string.Empty;

        string urlpathquery = string.Empty;
        urlpathquery = HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower();

        if (urlpathquery.Contains("ct.aspx")
               || urlpathquery.Contains("microsite.aspx")
             || urlpathquery.Contains("fl.aspx")
            || urlpathquery.Contains("pl.aspx")
             || urlpathquery.Contains("pd.aspx")
              || urlpathquery.Contains("ps.aspx")
              || urlpathquery.Contains("sitemap.aspx")
                         || urlpathquery.ToLower().Contains("bb.aspx")
              || urlpathquery.Contains("browsekeyword.aspx")
               || urlpathquery.Contains("browseproducttag.aspx")
                || urlpathquery.Contains("onlinecatalogue.aspx")
             )
        {


          string  cururl = currenturl.Substring(0, currenturl.Length - 1).Replace("https", "http") + Request.RawUrl.ToString();
          returnval = "<link rel=\"canonical\" href=\"" + cururl + "\"  />";
        }

        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
        {
            int iTotalPages = 0;


         
 if(HttpContext.Current.Request.QueryString["cnt"] !=null)     
            {

                iTotalPages =Convert.ToInt32(Request.QueryString["cnt"].ToString());  
            }
 else if (Session["iTotalPages"] != null)
 {
     iTotalPages = Convert.ToInt32(Session["iTotalPages"]);


 }
            if (Request.RawUrl.ToString().Contains("pno") == true)
            {
                string querystring = Request.RawUrl.ToString();
                string[] checkurl = querystring.ToString().Replace("/ct/", "").Replace("/pl/", "").Replace("/bb/", "").Split('/');
                string pno = checkurl[checkurl.Length -1].ToString().Replace("pno-", "");
                int iPageNo = Convert.ToInt32(pno);
                int inexturl = iPageNo + 1;
                int iprevurl = iPageNo - 1;


                if (iTotalPages > 0 && inexturl < iTotalPages)
                {
                    string nexturl = querystring.Replace("pno-" + pno, "pno-" + inexturl.ToString());
                    returnval = "<link rel=\"next\" href=\"" + nexturl + "\"  />";
                }
                string prevurl = string.Empty;
                if (iprevurl > 1)
                {
                    prevurl = querystring.Replace("pno-" + pno, "pno-" + iprevurl.ToString());
                }
                else
                {
                    prevurl = querystring.Replace("/pno-" + pno, "").Replace("/cnt-"+iTotalPages,"");
                }
                return returnval = returnval + "<link rel=\"prev\" href=\"" + prevurl + "\"  />";

            }
            else
            {
                string querystring = Request.RawUrl.ToString();
                string[] checkurl = querystring.ToString().Split('/');
                string pagename = checkurl[checkurl.Length - 2] + "/";
                string newquerystring = querystring.ToString().Replace(pagename, "");

                if (iTotalPages > 1)
                {
                    string nexturl = newquerystring + "cnt-" + iTotalPages + "/pno-2" + "/" + pagename;
                    //  next.Href = nexturl;
                    return returnval = "<link rel=\"next\" href=\"" + nexturl + "\"  />";
                }
                else
                {
                    return returnval;
                
                }

            }

        }
        else
        {
            return returnval;
        }

    }

    public string ST_TOP_CATEGORY_SCROLLMENU()
    {
        try
        {
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TopScrollCategory", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);

            return tbwtEngine.ST_TOP_Category_Scroll();
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }
    public string ST_TOP_CatMenuMobile()
    {
        try
        {
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TopMobile", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);

            return tbwtEngine.ST_Top_Load_Mobile();
        }


        catch (Exception ex)
        {
            //objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    private void loadleftnav()
    {
        string reqURL = string.Empty;
        reqURL = Request.Url.ToString().ToLower();

        if (!reqURL.Contains("orderhistory"))
        {
            FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\leftnav.st"), FileMode.Open);
            System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
            string dataString = streamWriter.ReadToEnd();
            streamWriter.Close();
            fileStream.Close();
            if (reqURL.Contains("ct.aspx") || reqURL.Contains("microsite.aspx"))
            {
                dataString = dataString.Replace("$newproductsnav$", "");
            }
            //if (Request.Url.ToString().ToLower().Contains("pd.aspx"))
            //    dataString = dataString.Replace("$RecentViewProducts$", "$RecentViewProducts$");
            //else
            //    dataString = dataString.Replace("$RecentViewProducts$", "");

            if (reqURL.Contains("paymenthistory.aspx") || reqURL.Contains("billinfo.aspx") || reqURL.Contains("ppapi.aspx") || reqURL.Contains("billinfop.aspx"))
            {
                dataString = dataString.Replace("$newproductsnav$", "");
                dataString = dataString.Replace("$newproductlognav$", "");
            }
            string[] str = dataString.Split('$');
            for (int strc = 1; strc < str.Length; strc = strc + 2)
            {
                if (str[strc].ToUpper() == "NEWPRODUCTSNAV" && !reqURL.Contains("ct.aspx") && !reqURL.Contains("microsite.aspx"))
                {
                    if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == "")
                    {
                        str[strc] = "NEWPRODUCTLOGNAV";

                    }
                    //if (str[strc].ToUpper() == "NEWPRODUCTLOGNAV")
                    //{

                    //    if (str[strc].ToUpper() == "NEWPRODUCTLOGNAV" && !Request.Url.ToString().ToLower().Contains("/Login.aspx") && !Request.Url.ToString().ToLower().Contains("createanaccount.aspx") && !Request.Url.ToString().ToLower().Contains("dealerregistration.aspx") && !Request.Url.ToString().ToLower().Contains("existcustomerregistration.aspx"))
                    //    {
                    //        Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //        leftnav.Controls.Add(ctl);
                    //     }


                    //}

                    //if (str[strc].ToUpper() == "NEWPRODUCTSNAV")
                    //{

                    //    if (str[strc].ToUpper() == "NEWPRODUCTSNAV" && !Request.Url.ToString().ToLower().Contains("/Login.aspx") && !Request.Url.ToString().ToLower().Contains("createanaccount.aspx") && !Request.Url.ToString().ToLower().Contains("dealerregistration.aspx") && !Request.Url.ToString().ToLower().Contains("existcustomerregistration.aspx"))
                    //    {
                    //        Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //        leftnav.Controls.Add(ctl);
                    //    }


                    //}

                    // Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    //leftnav.Controls.Add(ctl);
                }
                else if (str[strc].ToUpper() == "NEWPRODUCTLOGNAV")
                {

                    if (str[strc].ToUpper() == "NEWPRODUCTLOGNAV" && !reqURL.Contains("paysp.aspx")
                        && !reqURL.Contains("billinfosp.aspx")
                        && !reqURL.Contains("confirmmessage.aspx")
                        && !reqURL.Contains("contactus.aspx")
                         && !reqURL.Contains("aboutus.aspx")
                        && !reqURL.Contains("payonlinecc.aspx")
                        && !reqURL.Contains("ppapi.aspx")
                        && !reqURL.Contains("billinfop.aspx")
                        && !reqURL.Contains("createanaccount.aspx")
                        && !reqURL.Contains("dealerregistration.aspx")
                        && !reqURL.Contains("existcustomerregistration.aspx")
                        && !reqURL.Contains("ct.aspx")
                        && !reqURL.Contains("microsite.aspx")
                        && !reqURL.Contains("microsite.aspx")
                        && !reqURL.Contains("retailerregistration.aspx")
                        && !reqURL.Contains("shipping.aspx")
                        && !reqURL.Contains("checkout.aspx")
                        && !reqURL.Contains("getdeal.aspx")
                        && !reqURL.Contains("/Login.aspx")
                         && !reqURL.Contains("/ps.aspx")
                         && !reqURL.Contains("/pl.aspx")
                        && !reqURL.Contains("/bb.aspx")
                             && !reqURL.Contains("/sitemap.aspx")
                        && !reqURL.Contains("/browsekeyword.aspx") 
                        && !reqURL.Contains("/browseproducttag.aspx")
                         && !reqURL.Contains("/termsandconditions.aspx")
                        && !reqURL.Contains("/privacepolicy.aspx")) // remove ship page      && !Request.Url.ToString().ToLower().Contains("shipping.aspx")
                    {
                        Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                        leftnav.Controls.Add(ctl);
                    }


                }
                else if (str[strc].ToUpper() == "NEWPRODUCTLOGNAV" && !reqURL.Contains("ct.aspx"))
                {
                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    leftnav.Controls.Add(ctl);
                }
                //else if (str[strc] != "maincategory" && str[strc] != "browsebybrandWES" && str[strc] != "browsebyproductWES")
                //{
                //    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                //    //comment by palani
                //   leftnav.Controls.Add(ctl);
                //}
                else if (str[strc] == "maincategory" && (reqURL.Contains("pl.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    leftnav.Controls.Add(ctl);
                }
                else if (str[strc] == "maincategory" && (reqURL.Contains("pd.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    leftnav.Controls.Add(ctl);
                }
                else if (str[strc] == "maincategory" && (reqURL.Contains("fl.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    leftnav.Controls.Add(ctl);
                }
                else if (str[strc] == "maincategory" && (reqURL.Contains("bb.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    leftnav.Controls.Add(ctl);
                }
                //else if (str[strc] == "maincategory" && (reqURL.Contains("byproduct.aspx")))
                //{
                //    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                //    //comment by palani
                //   leftnav.Controls.Add(ctl);
                //}
                else if (str[strc] == "maincategory" && ((reqURL.Contains("ct.aspx")) || (reqURL.Contains("microsite.aspx"))))
                {
                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    leftnav.Controls.Add(ctl);
                }
                else if (str[strc] == "maincategory" && (reqURL.Contains("ps.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                    //comment by palani
                    leftnav.Controls.Add(ctl);
                }

            }
        }
    }
    public string ST_Top_Cart_item()
    {
        string RtnData;
        try
        {
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CartItems", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            // tbwtMSEngine.cartitem = cartcount();
            //tbwtEngine.RenderHTML("Row");
            //return (tbwtEngine.RenderedHTML);
            //string formname = Request.Url.Segments[1].ToString();
            RtnData = tbwtEngine.ST_Top_Cart_item();
            if (RtnData != "")
            {
                string[] strsplit = RtnData.Split('~');

                if (strsplit.Length >= 2)
                {
                    CartCount = strsplit[0].ToString();
                    Cartdetail = strsplit[1].ToString();
                    CartCount_mobile = strsplit[2].ToString();
                    Cartdetail_mobile = strsplit[5].ToString();

                }
                else
                {
                    Cartdetail = "";
                    CartCount = "<a href=\"\" class=\"dropdown-toggle cart_drop\" data-toggle=\"dropdown\"><span ><img src=\"/images/MicroSiteimages/cart.png\" class=\"margin_left margin_right\"></span><span class=\"white_color font_weight font_size_22\">0<span class=\"font_size_16\">item (s)</span> - &#36;  0.00</span></a>";
                    CartCount_mobile = "0";
                    CartCount_mobile = "";

                }


            }
            else
            {

                Cartdetail = "";
                CartCount = "<a href=\"\" class=\"dropdown-toggle cart_drop\" data-toggle=\"dropdown\"><span ><img src=\"/images/MicroSiteimages/cart.png\" class=\"margin_left margin_right\"></span><span class=\"white_color font_weight font_size_22\">0<span class=\"font_size_16\">item (s)</span> - &#36;  0.00</span></a>";
                CartCount_mobile = "0";
                CartCount_mobile = "";
            }

        }


        catch (Exception ex)
        {
            //  objErrorHandler.ErrorMsg = ex;
            //  objErrorHandler.CreateLog();
            Cartdetail = "";
            CartCount = "<a href='' class=''> 0 item(s) - &#36; 0.00  </a>,";
            CartCount_mobile = "0";
            CartCount_mobile = "";
        }
        return "";
    }
    public string GetLoginName_top()
    {
        string retvalue = string.Empty;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        DataTable objDt = new DataTable();
        string _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        try
        {
            if (!string.IsNullOrEmpty(userid))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string iLoginName = string.Empty;
                if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                {
                    objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                    if (objDt != null && objDt.Rows.Count > 0)
                    {
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                        HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                    }

                }
                else
                {
                    objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                    if (objDt != null && objDt.Rows.Count > 0)
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                }
                retvalue = iLoginName;
            }
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            retvalue = "-1";
        }
        finally
        {
            objDt.Dispose();
            objDt = null;
        }
        return retvalue;
    }

    private void loadmaincontent()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\maincontent.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        string requrl = Request.Url.ToString().ToUpper();
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            if (requrl.Contains(str[strc].ToUpper() + ".as".ToUpper())) //if (requrl.Contains(str[strc].ToUpper().Replace("FAMILY","FL") + ".as".ToUpper()))
            {
                Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                maincontent.Controls.Add(ctl);
            }
        }
    }

    private void loadrightnav()
    {

        string reqURL = string.Empty;
        reqURL = Request.Url.ToString().ToLower();

        if (!reqURL.Contains("bulkorder") && !reqURL.Contains("orderdetail") && !reqURL.Contains("orderhistory") && !reqURL.Contains("pendingorder"))
        {
            FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\rightnav.st"), FileMode.Open);
            System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
            string dataString = streamWriter.ReadToEnd();
            streamWriter.Close();
            fileStream.Close();
            if (reqURL.Contains("ct.aspx") || reqURL.Contains("microsite.aspx"))     //|| Request.Url.ToString().ToLower().Contains("pd.aspx") || Request.Url.ToString().ToLower().Contains("pl.aspx")
            {
                dataString = dataString.Replace("$quickbuy$", "$newproductsnav$");

                //if (Request.Url.ToString().ToLower().Contains("byp=2"))
                //{
                //    dataString = dataString.Replace("$advertisement$", "$newproductsnav$");
                //}
                if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                {
                    dataString = dataString.Replace("$advertisement$", "");
                }
                dataString = dataString.Replace("$announcements$", "");
            }
            if (reqURL.Contains("pd.aspx") || reqURL.Contains("shipping.aspx") || reqURL.Contains("checkout.aspx") ||
                reqURL.Contains("payonlinecc.aspx") || reqURL.Contains("paymenthistory.aspx") ||
                reqURL.Contains("ordertemplate.aspx") || reqURL.Contains("contactus.aspx") ||
                reqURL.Contains("aboutus.aspx") || reqURL.Contains("myaccount.aspx") ||
                reqURL.Contains("paysp.aspx") || reqURL.Contains("billinfosp.aspx") ||
                reqURL.Contains("termsandconditions.aspx") || reqURL.Contains("privacepolicy.aspx") ||
                reqURL.Contains("changepassword.aspx") || reqURL.Contains("changeusername.aspx") ||
                reqURL.Contains("sitemap.aspx") || reqURL.Contains("getdeal.aspx")
                || reqURL.Contains("billinfo.aspx")
                || reqURL.Contains("billinfop.aspx")
                || reqURL.Contains("ppapi.aspx")
                || reqURL.Contains("browseproducttag.aspx")
                || reqURL.Contains("confirmmessage.aspx")
                    || reqURL.Contains("browsekeyword.aspx")
                )
            {
                dataString = dataString.Replace("$advertisement$", "");
            }
            string[] str = dataString.Split('$');
            for (int strc = 1; strc < str.Length; strc = strc + 2)
            {
                if (!(str[strc].Contains("quickbuy") && reqURL.Contains("bb.aspx")))
                    if (!Request.Url.ToString().ToLower().Contains("ps.aspx"))
                    {

                        if (!(str[strc] == "browserby" && reqURL.Contains("pd")) && str[strc] != "browsebycategory" && str[strc] != "browsebybrandWES" && str[strc] != "browsebyproductWES" && (!(str[strc] == "quickbuy" && reqURL.Contains("shipping"))) && (!(str[strc] == "quickbuy" && reqURL.Contains("orderdetails"))) && (!(str[strc] == "announcements" && reqURL.Contains("shipping"))) && (!(str[strc] == "announcements" && reqURL.Contains("bulkorder"))))
                        {
                            if (reqURL.Contains("bb.aspx"))
                            {
                                if (str[strc] != "browserby" && str[strc] != "advertisement")
                                {

                                    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                                    //comment by palani
                                    rightnav.Controls.Add(ctl);

                                }


                            }
                            else
                            {
                                if (str[strc] != "browserby")
                                {
                                    //Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                                    //comment by palani
                                    if (str[strc].ToUpper() == "browserby" && !reqURL.Contains("/Login.aspx"))
                                    {
                                        Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                                        rightnav.Controls.Add(ctl);
                                    }

                                    if (str[strc] == "advertisement" && !reqURL.Contains("/Login.aspx") && !reqURL.Contains("ct.aspx") && !reqURL.Contains("microsite.aspx") && !reqURL.Contains("pl.aspx")) //&& !Request.Url.ToString().ToLower().Contains("pl.aspx")
                                    {
                                        Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                                        rightnav.Controls.Add(ctl);
                                    }
                                    //if (str[strc] == "advertisement" && Request.Url.ToString().ToLower().Contains("agencies.aspx"))
                                    //{
                                    //    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                                    //    rightnav.Controls.Add(ctl);
                                    //}
                                }
                            }

                        }
                        else if (str[strc] == "browsebybrandWES" && ((reqURL.Contains("pl.aspx")) || (reqURL.Contains("bb.aspx"))) && !reqURL.Contains("bulkorder"))
                        {
                            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

                            rightnav.Controls.Add(ctl);
                        }
                        else if (str[strc] == "browsebyproductWES" && ((reqURL.Contains("pl.aspx")) || (reqURL.Contains("byproduct.aspx"))) && !reqURL.Contains("bulkorder"))
                        {
                            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

                            rightnav.Controls.Add(ctl);
                        }
                        else if (reqURL.Contains("shipping_info.aspx") && !reqURL.Contains("bulkorder"))
                        {
                            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                            //comment by palani
                            rightnav.Controls.Add(ctl);
                        }

                    }
            }
        }
    }

    private void loadfooter()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\footer.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            footer.Controls.Add(ctl);
        }
    }

}
