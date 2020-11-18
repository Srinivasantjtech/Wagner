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
using System.Globalization;
using MSCaptcha;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.TemplateRender;
using System.IO;
public partial class fl : System.Web.UI.Page
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
    string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();


    string downloadST = "";
    private bool isdownload = false;
    private string _Package = "CSFAMILYPAGE";
    private string _SkinRootPath = HttpContext.Current.Server.MapPath("~\\Templates");
    private bool chkdwld = false;

  //  Stopwatch sw = new Stopwatch();

    //protected void Page_PreInit(object sender, EventArgs e)
    //{

    //    string newurl = Request.Url.PathAndQuery.Replace("/fl.aspx?", "").Replace("?", "");
    //    string Formname = Request.Url.LocalPath;

    //    string val = objHelperServices.URL_Rewrite_New(newurl, 1, "fl.aspx");
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
      //  ErrorHandler objErrorHandler = new ErrorHandler();
       // sw.Start();
        //Session["PageUrl"] = Request.Url.PathAndQuery.ToString();//.Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Session["PageUrl"] = Request.RawUrl.ToString();
        
        //Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();        
       // Session["PageUrl"] = "fl.aspx?fid=" + Request["fid"];
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
            currenturl=currenturl.Substring(0,currenturl.Length-1); 
            //string Fname = "";
           // string CFname = "";
            string urlstring = string.Empty;
            //Page.Title = "Cellink";

            var title = "";
            var img = "";
            var ogdesc = "";
            if (Session["BreadCrumbDS"] != null)
            {

                DataSet ds = (DataSet)Session["BreadCrumbDS"];
                string dsritemtype = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dsritemtype = ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower();
                    if (dsritemtype == "category")
                    {
                        if (i != 0)
                        {

                            stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

                            stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
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
                        //if (i == 0)
                        //{
                        //    h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                        
                        //}
                    }
                    else if (dsritemtype == "family")
                    {

                        sfamily = ds.Tables[0].Rows[i]["FamilyName"].ToString();
                        title = "<meta property=\"og:title\" content='" + sfamily + "' />";
                        //h1.InnerText = objgetmetadata.Replace_SpecialChar(sfamily);

                    }


                }

                string title_key = objgetmetadata.FetchData(ds);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
                //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString();
               // Page.Title = stitle.Replace("<ars>g</ars>", "-").ToString();
                Page.Title = stitle.Replace("<ars>g</ars>", "-").ToString();
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
                Page.MetaKeywords = skeywordRe.Replace(",", " |").ToString() + " - Bigtop, wagneronline.com.au";
                string shtdesc = string.Empty;
                string desc = string.Empty;
                if (Session["FamilyProduct"] != null)
                {
                    DataSet dsdesc = (DataSet)Session["FamilyProduct"];

                    DataTable dtprod = new DataTable();

                    dtprod = dsdesc.Tables[0];
                    string expression = "attribute_Name in ('Short Description','Description','Features','Descriptions','prod_dsc','Notes','Note','Family Image1') ";
                    DataRow[] foundRows;
                    foundRows = dtprod.Select(expression);

                    for (int j = 0; j < foundRows.Length; j++)
                    {
                        if (j == 0)
                        {
                            if (foundRows[j]["attribute_Name"].ToString() == "Short Description")
                            {
                                string h2desc = foundRows[j]["STRING_VALUE"].ToString();
                               

                                //h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                            }
                            desc = objgetmetadata.Replace_SpecialChar(foundRows[j]["STRING_VALUE"].ToString());
                        }
                        else
                        {
                            //if ((h2.InnerText == string.Empty) && (foundRows[j]["attribute_Name"].ToString() == "Short Description"))
                            //{
                            //    string h2desc = foundRows[j]["STRING_VALUE"].ToString();

                            //    h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                            //}

                          string  frjattr_name = foundRows[j]["attribute_Name"].ToString();

                          if (frjattr_name == "Family Image1")
                            {
                                img = "<meta property=\"og:image\" content='" + currenturl + foundRows[j]["STRING_VALUE"].ToString() + "' />";
                                //ogimage.InnerText =currenturl+ foundRows[j][1].ToString();
                                //h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                            }

                            if ((frjattr_name == "Description") || (frjattr_name == "Descriptions"))
                            {
                                string DescForh2 =objgetmetadata.Replace_SpecialChar( foundRows[j]["STRING_VALUE"].ToString());
                                ogdesc = "<meta property=\"og:description\" content='" + DescForh2 + "'/>";
                                //ogdescription.InnerText = DescForh2; 
                                //h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                            }
                            if (foundRows[j - 1]["STRING_VALUE"].ToString() != foundRows[j]["STRING_VALUE"].ToString() && frjattr_name != "Family Image1")
                            {
                                if (desc != string.Empty)
                                {
                                    desc = desc + ". " + foundRows[j]["STRING_VALUE"].ToString();
                                }
                                else
                                {
                                    desc = foundRows[j]["STRING_VALUE"].ToString();
                                }
                                desc = objgetmetadata.Replace_SpecialChar(desc);
                                ogdesc = "<meta property=\"og:description\" content='" + desc + "'/>";
                            }
                        }

                    }

                   // Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);
                   // ogdesc = objgetmetadata.Replace_SpecialChar(ogdesc);
                    Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);


                }
                //if (h2.InnerText == string.Empty)
                //{
                //    h2.InnerText = Page.MetaDescription;
                //}


                //if (h3_2.InnerText == "")
                //{

                //    h3_2.Visible = false;
                //}
                //if (h3_3.InnerText == "")
                //{

                //    h3_3.Visible = false;
                //}
                //if (h2.InnerText == string.Empty)
                //{

                //    h2.Visible = false;
                //}
            }
            //stlistprod = objHelperServices.URLRewriteToAddressBar("fl.aspx?" ,urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_Family.ini"),false);

            //if (Page.Request.RawUrl.ToString().Contains("="))
            //{
            //    stlistprod = objHelperServices.URLRewriteToAddressBar("fl.aspx?" + urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_Family.ini"));
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "processAjaxData( '" + stlistprod + "');", true);
            //}
            //else
            //{
            //  string[]  PARENTFAMILY= Page.Request.RawUrl.Split(new string[] { "?" }, StringSplitOptions.None);
            //  Session["PARENTFAMILY"] = PARENTFAMILY[1];

            //}
            string[] PARENTFAMILY = Page.Request.RawUrl.Replace("fl/","fl.aspx?"). Split(new string[] { "?" }, StringSplitOptions.None);
            Session["PARENTFAMILY"] = PARENTFAMILY[1];

            string pagetitle=Page.Title.ToLower();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string TextTitle = textInfo.ToTitleCase(pagetitle);
            TextTitle = objgetmetadata.SetTitleCount(TextTitle);
            //Page.Title = TextTitle + " | " + "Wagner Electronics"; 
            Page.Title = TextTitle;
            var ogtype = "<meta property=\"og:type\" content=\"Wagner:Family\" />";
            var ogsitename = "<meta property=\"og_site_name\" content='"+currenturl+"' />";
            var ogurl = "<meta property=\"og_url\" content='" + currenturl + Request.RawUrl + "' />";
            
            litMeta.Text = ogtype + ogdesc + title + img + ogsitename + ogurl;
        }
        catch
        { }







    }


    [System.Web.Services.WebMethod]
    public static  int ValidateCaptcha(string secCode)
    {
        if (HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"] != null && HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"] != "")
        {
            if (HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString() == secCode)
                return 0;
            else
                return -1;
        }
        return -2;
        
    }

    [System.Web.Services.WebMethod]
    public static  string SendAskQuestionMail(string fromid, string fname, string phone, string qustion, string familyName)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            string _UserID = "0";

            MessageObj.From = new System.Net.Mail.MailAddress(fromid);
            MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(fromid);
            string message = string.Empty;
            MessageObj.Subject = "Bigtop-Form-Product-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td>Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
            message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
            message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fname + "</td></tr>";
            message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
            message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
            message = message + "<tr><td>Question</td><td>&nbsp;</td><td>" + qustion + "</td></tr>";
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                }
                else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                    message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                }
                else
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";

            }
            else
            {
                message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
            }

            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }
    [System.Web.Services.WebMethod]
    public static string SendBulkBuyProjectPricing(string productcode, string fullname, string qty, string fromid, string deliverytime, string phone, string targetprice, string notesandaddtionalinfo, string familyName)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            string _UserID = "0";

            MessageObj.From = new System.Net.Mail.MailAddress(fromid);
            MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(fromid);

            string message = string.Empty;
            MessageObj.Subject = "Bigtop-Form-BulkBuy-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
            // message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
            message = message + "<tr><td>ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fullname + "</td></tr>";
            message = message + "<tr><td>QTY </td><td>&nbsp;</td><td>" + qty + "</td></tr>";
            message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
            message = message + "<tr><td>Delivery Time </td><td>&nbsp;</td><td>" + deliverytime + "</td></tr>";
            message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
            message = message + "<tr><td>Target Price</td><td>&nbsp;</td><td>" + targetprice + "</td></tr>";
            message = message + "<tr><td>Notes / Addtional Info</td><td>&nbsp;</td><td>" + notesandaddtionalinfo + "</td></tr>";
            message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                }
                else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                    message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                }
                else
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";

            }
            else
            {
                message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
            }

            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }

    [System.Web.Services.WebMethod]
    public static string DownloadUpdate(string fullname, string fromid, string phone, string downloadrequire, string familyName)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            string _UserID = "0";
            MessageObj.From = new System.Net.Mail.MailAddress(fromid);
            MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(fromid);

            string message = string.Empty;
            MessageObj.Subject = "Bigtop-Form-Download-Request";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
            message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
            message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fullname + "</td></tr>";
            message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
            message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
            message = message + "<tr><td>Download Required / Comments</td><td>&nbsp;</td><td>" + downloadrequire + "</td></tr>";
            message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                }
                else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                    message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                }
                else
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";

            }
            else
            {
                message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
            }
            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }

    [System.Web.Services.WebMethod]

    public static string DynamicPag(string ipageno, string _Fid, string eapath, string Rawurl, string Pagecnt)
    {
        try
        {
            UC_family fm = new UC_family();
            //int i = fm.GetFamilyAllData(_Fid);

            //DataSet dsPriceTableAll = fm.dsPriceTableAll;
            //DataSet EADs = (DataSet)HttpContext.Current.Session["FamilyPro    duct"];
            //DataSet Ds = fm.Ds;
            //string CScontentvalue = FamilyServices.Dynamic_GenerateHorizontalHTMLJson(CNT, _Fid, Ds, dsPriceTableAll, EADs);
            if (Convert.ToInt16(ipageno) < Convert.ToInt16(Pagecnt))
            {
                ErrorHandler objErrorHandler = new ErrorHandler();
               // objErrorHandler.CreateLog("before family page dynamic pagination" + ipageno);
                string CScontentvalue = fm.Dynamic_pagination(ipageno, _Fid, eapath, Rawurl);
               // objErrorHandler.CreateLog("after family page dynamic pagination");
                return CScontentvalue;
            }
            else
            {
                HttpContext.Current.Session["hfprevfid"] = null;
                return "";
            }
        }
        catch (Exception ex)
        {
            return "";
        }

    }


    [System.Web.Services.WebMethod]
    public static string Shiipinginfo(string value)
    {

        string sHTML = string.Empty;

        try
        {
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;
            fl family = new fl();
             
            _stg_container = new StringTemplateGroup("main", family._SkinRootPath);

            _stmpl_container = _stg_container.GetInstanceOf(family._Package + "\\shippinginfo");



            sHTML = _stmpl_container.ToString();
            return sHTML;
        }
        catch (Exception ex)
        {
            // objErrorHandler.ErrorMsg = ex;
            // objErrorHandler.CreateLog();
        }
        return value;
    }

    [System.Web.Services.WebMethod]
    public static string SortProduct(ProductSort productSort)
    {
        string sHTML = string.Empty;

        try
        {
            UC_family family = new UC_family();
            return family.sortProduct(productSort.fid, productSort.url, productSort.sortoptions, productSort.screen);

        }
        catch (Exception ex)
        {

        }
        return sHTML;
    }


    [System.Web.Services.WebMethod]
    public static string AskaQuestionLoad(string value)
    {
        string sHTML = string.Empty;

        try
        {
            fl family = new fl();
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;
            _stg_container = new StringTemplateGroup("main", family._SkinRootPath);
            _stmpl_container = _stg_container.GetInstanceOf(family._Package + "\\askaquestion");

            sHTML = _stmpl_container.ToString();
            
            return sHTML;
        }
        catch (Exception ex)
        {

        }
        return sHTML;
    }


    [System.Web.Services.WebMethod]
    public static string BulkBuyLoad(string value)
    {
        string sHTML = string.Empty;

        StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_container = null;
        StringTemplateGroup _stg_records1 = null;
        StringTemplate _stg_container_records1 = null;
        fl family = new fl();
        try
        {

            _stg_container = new StringTemplateGroup("main", family._SkinRootPath);

            _stmpl_container = _stg_container.GetInstanceOf(family._Package  + "\\bulkbuy");


            _stg_records1 = new StringTemplateGroup("bulkbuyrow", family._SkinRootPath);
            // _stg_container_records1 = _stg_records1.GetInstanceOf("Familypage" + "\\" + "multilistitem");
            string shtml = string.Empty;
            ErrorHandler objErrorHandler = new ErrorHandler();
            //objErrorHandler.CreateLog("webmethod prodcodedesc" + HttpContext.Current.Session["prodcodedesc"]);
            if (HttpContext.Current.Session["prodcodedesc"] != null)
            {
                string codedescall = HttpContext.Current.Session["prodcodedesc"].ToString();
                string[] codedesc = codedescall.Split('|');
                for (int i = 0; i < codedesc.Length - 1; i++)
                {
                    _stg_container_records1 = _stg_records1.GetInstanceOf(family._Package +  "\\multilistitembb");
                    string prodcode = string.Empty;
                    prodcode = codedesc[i].Trim();
                    //_stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                    //if (i == 0)
                    //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");

                    if (codedesc.Length > 2)
                    {
                        _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                        _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", true);
                    }
                    else
                    {
                        _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                        _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
                        _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", false);
                    }


                    //if (i == 0)
                    //{
                    //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", "Please Select Product");
                    //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
                    //}
                    //else
                    //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);

                    shtml = shtml + _stg_container_records1.ToString();
                }
                //if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
                //{
                //    _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
                //    _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
                //}

                if (HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] != null)
                    _stmpl_container.SetAttribute("TBT_FAMILY_NAME_BB", HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString());
                // _stmpl_container.SetAttribute("TBW_DDL_VALUE", _stg_container_records1.ToString());
                _stmpl_container.SetAttribute("TBW_DDL_VALUE", shtml.ToString());
            }
            else
            {
              HttpContext.Current.Response.Redirect("home.aspx", false);
            }
            return _stmpl_container.ToString();
        }
        catch (Exception ex)
        {
            //objErrorHandler.ErrorMsg = ex;
           // objErrorHandler.CreateLog();

        }
        return "";
    }

    [System.Web.Services.WebMethod]
    public static string FamilyDownloadLoad(string value)
    {
        string sHTML = string.Empty;
        string dwldmrge = string.Empty;
        fl family = new fl();
         UC_family fm = new UC_family();
         string fid = value;
         DataSet EADs = new DataSet();
        DataSet Dsall = new DataSet();
        //if (HttpContext.Current.Request.QueryString["fid"] != null)
        //    fid = HttpContext.Current.Request.QueryString["fid"].ToString();
        try
        {
            // HttpContext.Current.Session["isdownload"] = "false";
            family.isdownload = false;
            //sHTML = family.ST_Product_Download(value);
            fm.GetFamilyAllData(fid);

            EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            Dsall = (DataSet)HttpContext.Current.Session["Dsall"];
          //  _stmpl_container.SetAttribute("ST_Family_Download", ST_Family_Download(EADs, Dsall, Dsall.Tables.Count - 1));
           // sHTML = fm.ST_Family_Download(EADs, Dsall, Dsall.Tables.Count - 1, fid);
            sHTML = family.ST_Family_Download(EADs, Dsall, Dsall.Tables.Count - 1, fid);

            if (sHTML == string.Empty || sHTML == "none")
            {

               // chkdwld = false;
               // _stmpl_container.SetAttribute("DownloadST", ST_Downloads_Update());
               // _stmpl_container.SetAttribute("ST_Family_Download", "block");

                sHTML = family.ST_Downloads_Update(false);

            }
            else
            {
               // chkdwld = true;
               // string dwldmrge = ST_Downloads_Update();
               // dwldmrge = DownloadST + dwldmrge;
               // _stmpl_container.SetAttribute("DownloadST", dwldmrge);
                sHTML = sHTML + family.ST_Downloads_Update(true);
            }




            //if (sHTML != "" && family.isdownload == true)
            //{
            //    //  pd.chkdwld = true;
            //    dwldmrge = family.ST_Downloads_Update(true);
            //    sHTML = sHTML + dwldmrge;
            //}
            //else
            //{
            //    // pd.chkdwld = false;
            //    dwldmrge = family.ST_Downloads_Update(false);
            //    sHTML = sHTML + dwldmrge;
            //}



            return sHTML;
        }
        catch (Exception ex)
        {

        }
        return sHTML;
    }
    public string ST_Family_Download(DataSet TmpDs, DataSet TempEADs, int tableinx,string fid)
    {
        fl family = new fl();
        try
        {
            
            string rtnstr = string.Empty;
            StringTemplateGroup _stg_container = null;
            // StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            // StringTemplate _stmpl_records = null;
            // StringTemplate _stmpl_records1 = null;
            // StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            // StringTemplateGroup _stg_container1 = null;
            // StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];


            DataTable dt = new DataTable();
            DataRow[] dr = null;

            int ictrecords = 0;

            //if (Request.QueryString["fid"] != null)
            //    _Fid = Request.QueryString["fid"].ToString();

            string _Familyids = fid;

            ////  DataSet TmpDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];

            //  dr = TmpDs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
            //  if (dr.Length > 0)
            //  {
            //      SFamtb = dr.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
            //      if (SFamtb != null)
            //      {
            //          for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
            //          {
            //              _Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
            //              _Familyids = _Familyids.Replace(",,", ",");
            //          }
            //      }
            //  }

            _stg_container = new StringTemplateGroup("main", family._SkinRootPath);

            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadMain");



            if (_Familyids != "")
            {

                // DataSet TempEADs = GetFamilyPageProduct(_Familyids, "ATTACHMENT");
                if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[tableinx].Rows.Count > 0)
                {
                    //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");



                    string[] strf = _Familyids.Split(new string[] { "," }, StringSplitOptions.None);
                    if (strf.Length > 0)
                    {
                        lstrecords = new TBWDataList[strf.Length];
                        for (int intfam = 0; intfam <= strf.Length - 1; intfam++)
                        {
                            dr = null;
                            dr = TempEADs.Tables[tableinx].Select("FAMILY_ID='" + strf[intfam] + "'", "Sno");
                            if (dr.Length > 0)
                            {
                                //dt = new DataTable();
                                //dt = dr.CopyToDataTable();
                                rtnstr = ST_Familypage_Download(_Familyids, strf[intfam], dr);
                                if (rtnstr != "")
                                {
                                    lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
                                    ictrecords = ictrecords + 1;
                                }

                            }
                        }





                    }
                }





            }
            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
            string DownloadST_Product = ST_Product_Download(TmpDs);
            _stmpl_container.SetAttribute("PRODUCT_DOWNLOAD", DownloadST_Product);

            if (ictrecords > 0 || DownloadST_Product != "")
            {

                 family.downloadST = _stmpl_container.ToString();
               // isDownload = true;
              //  return "block";
            }


           // DownloadST = "";
           // isDownload = false;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return family.downloadST;


    }
    public string ST_Product_Download(DataSet TmpDs)
    {
        try
        {
            string rtnstr = string.Empty;
            // StringTemplateGroup _stg_container = null;
            // StringTemplateGroup _stg_records = null;
            // StringTemplate _stmpl_container = null;
            // StringTemplate _stmpl_records = null;
            // StringTemplate _stmpl_records1 = null;
            // StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            //  StringTemplateGroup _stg_container1 = null;
            // StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];
            string downloadst_pro = string.Empty;


            DataTable dt = new DataTable();
            //DataRow[] dr = null;

            //int ictrecords = 0;



            string DownloadST_Product = string.Empty;




            string _pid_multiple = string.Empty;
            string pcode_multiple = string.Empty;
            for (int prd = 0; prd <= TmpDs.Tables["FamilyPro"].Rows.Count - 1; prd++)
            {

                string _pid = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_ID"].ToString();
                string prodcode = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_CODE"].ToString() + " - Product Downloads";
                if (_pid != "")
                {
                    if (_pid_multiple == string.Empty)
                    {
                        _pid_multiple = _pid;
                        pcode_multiple = prodcode;
                    }
                    else
                    {
                        _pid_multiple = _pid_multiple + "," + _pid;
                        pcode_multiple = pcode_multiple + "," + prodcode;

                    }
                }

            }
            FamilyServices objFamilyServices = new FamilyServices();

            DataSet TempEADs_pid = objFamilyServices.GetFamilyPageProduct(_pid_multiple, "PRODUCT_ATTACHMENT");
            string[] pid = _pid_multiple.Split(',');
            string[] pcode = pcode_multiple.Split(',');
            for (int i = 0; i <= pid.Length - 1; i++)
            {
                DataRow[] drpid = TempEADs_pid.Tables[0].Select("PRODUCT_ID='" + pid[i] + "'");
                if (drpid.Length > 0)
                {

                    rtnstr = ST_Productpage_Download(drpid, pcode[i].ToString());
                    if (rtnstr != "")
                    {
                        DownloadST_Product = DownloadST_Product + rtnstr;
                    }
                }

            }
            return DownloadST_Product;
        }
        catch
        {
            return "";
        }


    }
    public string ST_Productpage_Download(DataRow[] Adt, string Protitle)
    {

        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;

        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];


        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];


        string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
        string strImgFiles1 = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        long FileInKB;
        string[] file = null;
        string strfile = string.Empty;
        fl family = new fl();
        if (Adt != null && Adt.Length > 0)
        {




            DataSet dscat = new DataSet();


            try
            {
                _stg_records = new StringTemplateGroup("cell", family._SkinRootPath);
                _stg_container = new StringTemplateGroup("row", family._SkinRootPath);

                // _stg_container = new StringTemplateGroup("row", _SkinRootPath);


                lstrecords = new TBWDataList[Adt.Length + 1];



                int ictrecords = 0;

                foreach (DataRow dr in Adt)//For Records
                {
                    strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


                    FileInfo Fil;


                    string strImgFilesnew = System.Configuration.ConfigurationManager.AppSettings["VirtualPathJPG"].ToString(); 
                    if ((dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg")))
                        Fil = new FileInfo(strImgFilesnew + dr["PRODUCT_ATT_FILE"].ToString());
                    else
                        Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());
                  //objErrorHandler.CreateLog(dr["PRODUCT_ATT_FILE"].ToString() +"pdffilename");


                    if ((Fil.Exists) && (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains("wes_public_files") == false) && (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains("wes_secure_files") == false))
                    {
                  //  objErrorHandler.CreateLog(dr["PRODUCT_ATT_FILE"].ToString() + "Inside pdffilename");
                        _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell_Product");

                        strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");
                        strfile = strfile.Replace(@"\", "/");

                        file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
                        if (file.Length > 0)
                        {
                            _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
                            if ((file[file.Length - 1].ToString().ToLower().Contains(".jpg")))
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
                            else
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "attachments");
                        }

                        //  FileInBytes = Fil.Length;
                        FileInKB = Fil.Length / 1024;

                        _stmpl_records.SetAttribute("TBT_PRODUCT_ATT_DESC", dr["PRODUCT_ATT_DESC"].ToString());
                        //Modified by indu 10Sep2015
                        //_stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH",  dr["PRODUCT_ATT_FILE"].ToString());

                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", strfile.Replace("/Attachments/", "/attachments/").Replace(".PDF", ".pdf"));

                        _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf("FamilyPage" + "\\" + "DownloadRow_Product");

                _stmpl_container.SetAttribute("TBT_PRODUCT_CODE", Protitle);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                if (ictrecords > 0)
                {
                    //isdownload_product = true;
                    return _stmpl_container.ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }

            return "";
        }
        return "";
    }
    public string ST_Familypage_Download(string pFamilyid, string Familyid, DataRow[] Adt)
    {
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;

        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];


        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];
        string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
        string strImgFiles1 = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

        fl family = new fl();
        long FileInKB;
        string[] file = null;
        string strfile = string.Empty;
        
        if (Adt.Length > 0)  //if (Adt != null && Adt.Rows.Count > 0)
        {




            DataSet dscat = new DataSet();


            try
            {
                _stg_records = new StringTemplateGroup("cell", family._SkinRootPath);
                _stg_container = new StringTemplateGroup("row", family._SkinRootPath);


                lstrecords = new TBWDataList[Adt.Length + 1];



                int ictrecords = 0;

                foreach (DataRow dr in Adt)//For Records
                {
                    strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\", "/");


                    FileInfo Fil;

                  string   strImgFilesnew = System.Configuration.ConfigurationManager.AppSettings["VirtualPathJPG"].ToString(); 
                    if ((dr["FAMILY_ATT_FILE"].ToString().ToLower().Contains(".jpg")))
                        Fil = new FileInfo(strImgFilesnew + dr["FAMILY_ATT_FILE"].ToString());
                    else
                        Fil = new FileInfo(strPDFFiles1 + dr["FAMILY_ATT_FILE"].ToString());


                    if ((Fil.Exists) && (dr["FAMILY_ATT_FILE"].ToString().ToLower().Contains("wes_public_files") == false) && (dr["FAMILY_ATT_FILE"].ToString().ToLower().Contains("wes_secure_files") == false))
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell");

                        strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\", "/");
                        strfile = strfile.Replace(@"\", "/");

                        file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
                        if (file.Length > 0)
                        {

                            _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
                            if ((file[file.Length - 1].ToString().ToLower().Contains(".jpg")))
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
                            else
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "attachments");
                        }

                        //  FileInBytes = Fil.Length;
                        FileInKB = Fil.Length / 1024;


                        _stmpl_records.SetAttribute("TBT_FAMILY_ATT_DESC", dr["FAMILY_ATT_DESC"].ToString());

                        //Modified by indu 10Sep2015 prod issue
                        //_stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["FAMILY_ATT_FILE"].ToString());

                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", strfile.Replace("/Attachments/", "/attachments/").Replace(".PDF", ".pdf"));
                        _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadRow");

                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", Adt[0]["FAMILY_NAME"].ToString());
                if (pFamilyid.ToLower() == Familyid.ToLower())
                {
                    _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", false);
                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", true);
                }



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                if (ictrecords > 0)
                    return _stmpl_container.ToString();

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }

            return "";
        }
        return "";
    }
    public string ST_Downloads_Update(bool dwnld)
    {
        StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_container = null;
        fl family = new fl();
        try
        {

            _stg_container = new StringTemplateGroup("main", family._SkinRootPath);

            if (!(dwnld))
                _stmpl_container = _stg_container.GetInstanceOf(family._Package + "\\" + "DowloadUpdate");
            else
                _stmpl_container = _stg_container.GetInstanceOf(family._Package + "\\" + "WithDowloadUpdate");

            //if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
            //{
            //    _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
            //    _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
            //}

            if (HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] != null)
                _stmpl_container.SetAttribute("TBT_FAMILY_NAME_DU", HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString());
            //if (chkdwld == false)
            //    _stmpl_container.SetAttribute("IS_DWLD_MRGE", true);
            //  else
            //    _stmpl_container.SetAttribute("IS_DWLD_MRGE", false);

            return _stmpl_container.ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return "";
    }
}
