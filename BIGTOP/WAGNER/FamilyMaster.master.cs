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

public partial class FamilyMaster : System.Web.UI.MasterPage
{




    HelperServices objHelperServices = new HelperServices();
    string currenturl = System.Configuration.ConfigurationManager.AppSettings["CDN"];

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.Url.ToString().Contains("ct.aspx"))
        {
            //Page.ClientScript.RegisterClientScriptInclude("css", ResolveUrl(currenturl+"/css/Wag_AllCss_1.css"));

        }

        string urlpathquery = string.Empty;
        urlpathquery = HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower();
        //HtmlLink link = new HtmlLink();
        //link.Href = currenturl + "css/DynamicPageCSS.css";
        //link.Attributes.Add("type", "text/css");
        //link.Attributes.Add("rel", "stylesheet");
        //header.Controls.Add(link);
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


            //SeoTag.Href = currenturl.Substring(0, currenturl.Length - 1).Replace("https", "http") + Request.RawUrl.ToString();
            SeoTag.Href = currenturl.Substring(0, currenturl.Length - 1)+ Request.RawUrl.ToString();

        }

        else
        {
            string vnum = System.Configuration.ConfigurationManager.AppSettings["CSSVersion"];
            HtmlLink linkcritical = new HtmlLink();
            linkcritical.Attributes.Add("rel", "stylesheet");
            linkcritical.Attributes.Add("type", "text/css");
            linkcritical.Attributes.Add("media", "screen");
            linkcritical.Href = currenturl + "css/Critical.css?v=" + vnum;
            header.Controls.Add(linkcritical);


        }


        if (urlpathquery.Contains("sitemap.aspx")
           || urlpathquery.Contains("browsekeyword.aspx")
            || urlpathquery.Contains("browseproducttag.aspx"))
        {
            HtmlLink linkcritical = new HtmlLink();
            linkcritical.Href = currenturl + "css/Critical.css";
            linkcritical.Attributes.Add("type", "text/css");
            linkcritical.Attributes.Add("rel", "stylesheet");
            header.Controls.Add(linkcritical);

        }
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
        FileStream fileStream = new FileStream(Server.MapPath("~\\Templates\\homepage\\header.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            if (str[strc].ToUpper() == "TOP")
            {
                if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == string.Empty || HttpContext.Current.Session["USER_ID"].ToString() == ConfigurationManager.AppSettings["DUM_USER_ID"])
                {
                    str[strc] = "toplog";
                }
            }

            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            header.Controls.Add(ctl);
        }
    }

    private void loadleftnav()
    {
        string reqURL = string.Empty;
        reqURL = Request.Url.ToString().ToLower();

        if (!reqURL.Contains("orderhistory"))
        {
            FileStream fileStream = new FileStream(Server.MapPath("~\\Templates\\homepage\\leftnav.st"), FileMode.Open);
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
                dataString = dataString.Replace("$ newproductlognav$", "");
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
                        && !reqURL.Contains("/Login.aspx")) // remove ship page      && !Request.Url.ToString().ToLower().Contains("shipping.aspx")
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

    private void loadmaincontent()
    {
        FileStream fileStream = new FileStream("~\\Templates\\homepage\\maincontent.st", FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        string requrl = Request.Url.ToString().ToUpper();
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            if (requrl.Contains(str[strc].ToUpper() + ".as".ToUpper()))
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
            FileStream fileStream = new FileStream(Server.MapPath("~\\Templates\\homepage\\rightnav.st"), FileMode.Open);
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
            if (reqURL.Contains("pd.aspx") || reqURL.Contains("shipping.aspx") ||
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
        FileStream fileStream = new FileStream(Server.MapPath("~\\Templates\\homepage\\footer.st"), FileMode.Open);
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
