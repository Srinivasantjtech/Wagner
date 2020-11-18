using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using TradingBell5.CatalogX;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using CustomCaptcha;
using System.Configuration;
using System.Globalization;
namespace WES
{
    public partial class mfl : System.Web.UI.Page
    {
        EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();

      
        GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
        string MicroSiteTemplate = "";
        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
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
        protected void Page_Load(object sender, EventArgs e)
        {
            string hfclickedattr = Request.Form["hfclickedattr"];

            if (hfclickedattr != null)
            {

                string[] url = Request.RawUrl.ToString().Split(new string[] { "mfl.aspx?" }, StringSplitOptions.None);
                Session["hfclickedattr_mfl"] = hfclickedattr.Replace("&quot;", @""""); 

                Response.Redirect(url[0].ToLower().Replace("undefined", ""));
            }
            if (Request.RawUrl.ToString().ToLower().Contains("undefined"))
            {
                Response.Redirect(Request.RawUrl.ToString().ToLower().Replace("undefined", ""));
            }
            Session["PageUrl"] = Request.RawUrl.ToString();
            MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
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
            string TextTitle = string.Empty;
            currenturl = currenturl.Substring(0, currenturl.Length - 1);
            var title = "";
            var img = "";
            var ogdesc = "";
            try
            {

               // string Fname = "";
               // string CFname = "";
                string urlstring = "";
                //Page.Title = "Cellink";

                if (strsupplierName == "")
                {
                    strsupplierName = GetSupplierName();
                }
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
                        else if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "family")
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
                    Page.MetaKeywords = skeywordRe.Replace(",", " |").ToString() +"|"+strsupplierName+ " - Wagner Electronics, wagneronline.com.au";
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
                                string frjattr_name = foundRows[j]["attribute_Name"].ToString();
                                if (frjattr_name == "Family Image1")
                                {
                                    img = "<meta property=\"og:image\" content='" + currenturl + foundRows[j]["STRING_VALUE"].ToString() + "' />";
                                    //ogimage.InnerText =currenturl+ foundRows[j][1].ToString();
                                    //h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                                }
                                if ((frjattr_name == "Description") || (frjattr_name == "Descriptions"))
                                {
                                    string DescForh2 = foundRows[j]["STRING_VALUE"].ToString();
                                    ogdesc = "<meta property=\"og:description\" content='" + DescForh2 + "'/>";
                                    //ogdescription.InnerText = DescForh2; 
                                    //h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                                }
                                if (frjattr_name == "Short Description")
                                {
                                    string h2desc = foundRows[j]["STRING_VALUE"].ToString();
                                    //h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                                }
                                desc = foundRows[j]["STRING_VALUE"].ToString();

                            }
                            else
                            {
                                //if ((h2.InnerText == string.Empty) && (foundRows[j]["attribute_Name"].ToString() == "Short Description"))
                                //{
                                //    string h2desc = foundRows[j]["STRING_VALUE"].ToString();

                                //    h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                                //}
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
                string[] PARENTFAMILY = Page.Request.RawUrl.Replace("fl/", "fl.aspx?").Split(new string[] { "?" }, StringSplitOptions.None);
                Session["PARENTFAMILY"] = PARENTFAMILY[1];

                string pagetitle = stitle.ToLower() +","+ strsupplierName;
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
             
                TextTitle = textInfo.ToTitleCase(pagetitle);
               
           string      newTextTitle = objgetmetadata.SetTitleCount(TextTitle);
        
                //Page.Title = TextTitle + " | " + "Wagner Electronics"; 
           if (newTextTitle == "")
                {
                    Page.Title = TextTitle;
                }
                else
                {
                    Page.Title = newTextTitle;
                }
           var ogtype = "<meta property=\"og:type\" content=\"Wagner:Family\" />";
           var ogsitename = "<meta property=\"og_site_name\" content='" + currenturl + "' />";
           var ogurl = "<meta property=\"og_url\" content='" + currenturl + Request.RawUrl + "' />";

           litMeta.Text = ogtype + ogdesc + title + img + ogsitename + ogurl;
              
            }
            catch(Exception ex)
            {
               // objErrorHandler.CreateLog(ex.ToString() + TextTitle); 
            
            }







        }
        [System.Web.Services.WebMethod]
        public static string SendAskQuestionMail(string fromid, string fname, string phone, string qustion, string familyName)
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
                string message = "";
                MessageObj.Subject = "Wagner-Form-FamilyProduct-Enquiry";
                MessageObj.IsBodyHtml = true;

                string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
                _stg_records = new StringTemplateGroup("row", templatepath);
                _stg_container = new StringTemplateGroup("main", templatepath);

                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "family-AskQuestion");
                _stmpl_container.SetAttribute("FAMILY_NAME", familyName);
                _stmpl_container.SetAttribute("TBT_FAMILY_TITLE", familyName.Replace('"', ' '));
                _stmpl_container.SetAttribute("URL", HttpContext.Current.Request.UrlReferrer.OriginalString);
                _stmpl_container.SetAttribute("FIRSTNAME", fname);
                _stmpl_container.SetAttribute("EMAILID", fromid);
                _stmpl_container.SetAttribute("PHONE", phone);
                _stmpl_container.SetAttribute("QUESTION", qustion);

                //message = message + "<tr><td>Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
                //message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
                //message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
                //message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fname + "</td></tr>";
                //message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
                //message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
                //message = message + "<tr><td>Question</td><td>&nbsp;</td><td>" + qustion + "</td></tr>";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                {
                    _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                    oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                    if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                    {
                        //message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                        _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                    }
                    else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                    {
                       // message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                       // message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                    }
                    else
                    {
                        //message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                        _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                    }

                }
                else
                {
                    //message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                    _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                }
                message = _stmpl_container.ToString();
                MessageObj.Body = message;
               // MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);
                return "1".ToString();
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString());
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

                string message = "";
                MessageObj.Subject = "Wagner-Form-Family-BulkBuy-Enquiry";
                MessageObj.IsBodyHtml = true;

                string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
               _stg_records = new StringTemplateGroup("row", templatepath);
                _stg_container = new StringTemplateGroup("main", templatepath);
                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "family-BulkBuy");
                _stmpl_container.SetAttribute("FAMILYNAME", familyName);
                _stmpl_container.SetAttribute("PRODUCT_CODE", productcode);
                _stmpl_container.SetAttribute("URL", HttpContext.Current.Request.UrlReferrer.OriginalString);
                _stmpl_container.SetAttribute("FIRSTNAME", fullname);
                _stmpl_container.SetAttribute("QTY", qty);
                _stmpl_container.SetAttribute("EMAILID", fromid);
                _stmpl_container.SetAttribute("DELIVERYTIME", deliverytime);
                _stmpl_container.SetAttribute("PHONE", phone);
                _stmpl_container.SetAttribute("TARGETPRICE", targetprice);
                _stmpl_container.SetAttribute("NOTES", notesandaddtionalinfo);
                //message = message + "<tr><td style=\"width:112px\">Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
                //message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
                //message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
                //message = message + "<tr><td>ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
                //message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fullname + "</td></tr>";
                //message = message + "<tr><td>QTY </td><td>&nbsp;</td><td>" + qty + "</td></tr>";
                //message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
                //message = message + "<tr><td>Delivery Time </td><td>&nbsp;</td><td>" + deliverytime + "</td></tr>";
                //message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
                //message = message + "<tr><td>Target Price</td><td>&nbsp;</td><td>" + targetprice + "</td></tr>";
                //message = message + "<tr><td>Notes / Addtional Info</td><td>&nbsp;</td><td>" + notesandaddtionalinfo + "</td></tr>";
                //message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                {
                    _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                    oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                    if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                    {
                        //message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                        _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                    }
                    else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                    {
                        // message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                        // message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                    }
                    else
                    {
                        //message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                        _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                    }

                }
                else
                {
                   // message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                    _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                }
                message = _stmpl_container.ToString();
                MessageObj.Body = message;
               // MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);
                return "1".ToString();
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString());
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


                string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
                _stg_records = new StringTemplateGroup("row", templatepath);
                _stg_container = new StringTemplateGroup("main", templatepath);
                string message = "";
                MessageObj.Subject = "Wagner-Form-Family-Download-Request";
                MessageObj.IsBodyHtml = true;

                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "family-DownloadUpdate");
                _stmpl_container.SetAttribute("FAMILYNAME", familyName);
                _stmpl_container.SetAttribute("URL", HttpContext.Current.Request.UrlReferrer.OriginalString);
                _stmpl_container.SetAttribute("FIRSTNAME", fullname);
                _stmpl_container.SetAttribute("EMAILID", fromid);
                _stmpl_container.SetAttribute("PHONE", phone);
                _stmpl_container.SetAttribute("DOWNLOADREQUIRED", downloadrequire);
                //message = message + "<tr><td style=\"width:112px\">Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
                //message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
                //message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
                //message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fullname + "</td></tr>";
                //message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
                //message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
                //message = message + "<tr><td>Download Required / Comments</td><td>&nbsp;</td><td>" + downloadrequire + "</td></tr>";
                //message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                {
                    _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                    oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                    if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                    {
                        //message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                        _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                    }
                    else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                    {
                        // message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                        // message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                        _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                        // message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                    }

                }
                else
                {
                    //message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                    _stmpl_container.SetAttribute("CUSTOMER_TYPE", "Retail");
                }
                message = _stmpl_container.ToString();
                MessageObj.Body = message;
                //MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);
                return "1".ToString();
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString());
                return "-1".ToString();
            }
        }

        [System.Web.Services.WebMethod]

        public static string DynamicPag(string ipageno, string _Fid, string eapath, string Rawurl, string Pagecnt)
        {
            try
            {
familyMS fm = new familyMS();
                //int i = fm.GetFamilyAllData(_Fid);

                //DataSet dsPriceTableAll = fm.dsPriceTableAll;
                //DataSet EADs = (DataSet)HttpContext.Current.Session["FamilyPro    duct"];
                //DataSet Ds = fm.Ds;
                //string CScontentvalue = FamilyServices.Dynamic_GenerateHorizontalHTMLJson(CNT, _Fid, Ds, dsPriceTableAll, EADs);
                if (Convert.ToInt16(ipageno) < Convert.ToInt16(Pagecnt))
                {
                    string CScontentvalue = fm.Dynamic_pagination(ipageno, _Fid, eapath, Rawurl);
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
        
        //public string ST_FamilypageALLData()
        //{
        //    StringTemplateGroup _stg_container = null;
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplate _stmpl_container = null;
        //    StringTemplate _stmpl_records = null;

        //    TBWDataList[] lstrecords = new TBWDataList[0];
        //    TBWDataList[] lstrows = new TBWDataList[0];


        //    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //    TBWDataList1[] lstrows1 = new TBWDataList1[0];



        //    _stg_container = new StringTemplateGroup("main", stemplatepath);

        //    _stmpl_container = _stg_container.GetInstanceOf("MICROSITE\\CSFAMILYPAGE" + "\\" + "FPmain");
        //    //EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
        //    int rtn = GetFamilyAllData();
        //    if (rtn == -1)
        //        return "";

        //    _stmpl_container.SetAttribute("Generateparentfamilyhtml", Generateparentfamilyhtml());
        //    _stmpl_container.SetAttribute("ST_Family_Download", ST_Family_Download(EADs, Dsall, Dsall.Tables.Count - 1));
        //    _stmpl_container.SetAttribute("ST_Familypage", ST_Familypage());
        //    //_stmpl_container.SetAttribute("DownloadST", DownloadST);
        //    if (DownloadST == "")
        //    {
        //        chkdwld = false;
        //        _stmpl_container.SetAttribute("DownloadST", ST_Downloads_Update());
        //        _stmpl_container.SetAttribute("ST_Family_Download", "block");

        //    }
        //    else
        //    {
        //        chkdwld = true;
        //        string dwldmrge = ST_Downloads_Update();
        //        dwldmrge = DownloadST + dwldmrge;
        //        _stmpl_container.SetAttribute("DownloadST", dwldmrge);
        //    }
        //    _stmpl_container.SetAttribute("ST_BulkBuyPP", ST_BulkBuyPP());
        //    if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
        //    {
        //        _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
        //        _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
        //    }
        //    return _stmpl_container.ToString();

        //}
        //public int GetFamilyAllData()
        //{

        //    //have to get the id
        //    EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];

        //    DataSet tempDs = new DataSet();
        //    DataTable Ftb = new DataTable();


        //    DataTable Atttbl = new DataTable();

        //    DataRow[] DrMain = null;
        //    DataRow[] Drsub = null;
        //    DataRow[] Dr = null;
        //    string _UserID = "";

        //    string _cid = "";
        //    string _pcr = "";

        //    if (Request.QueryString["fid"] != null)
        //        _Fid = Request.QueryString["fid"].ToString();
        //    if (Request.QueryString["cid"] != null)
        //        _cid = Request.QueryString["cid"].ToString();
        //    if (Request.QueryString["pcr"] != null)
        //        _pcr = Request.QueryString["pcr"].ToString();

        //    if (HttpContext.Current.Session["USER_ID"].ToString() != null)
        //        _UserID = HttpContext.Current.Session["USER_ID"].ToString();


        //    if (_UserID == "" || _UserID == null)
        //        _UserID = System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

        //    string ExecString = "";



        //    int tblinx = 0;


        //    _Familyids = _Fid;
        //    if (EADs != null)
        //    {



        //        ExecString = "exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Fid + "','PRODUCT'";


        //        DrMain = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");

        //        Drsub = EADs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
        //        if (Drsub.Length > 0)
        //        {
        //            SFamtb = Drsub.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
        //            foreach (DataRow dr in SFamtb.Rows)
        //            {
        //                ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + dr["FAMILY_ID"].ToString() + "','PRODUCT'";

        //                _Familyids = _Familyids + "," + dr["FAMILY_ID"].ToString();
        //                _Familyids = _Familyids.Replace(",,", ",");
        //            }

        //        }

        //        //ExecString=ExecString+";exec STP_TBWC_PICKFPRODUCTPRICE '"+ _Familyids +"','','"+_UserID+"'";

        //        //ExecString=ExecString+";exec STP_TBWC_PICKGENERICDATA '2',"+EADs.Tables[0].Rows[0]["Family_id"].ToString()+",'2','','','GET_FAMILY_ATTRIBUTE'";

        //        ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Familyids + "','ATTACHMENT'";




        //        string tmpProds = "";
        //        if (Convert.ToInt32(_UserID) > 0)
        //        {
        //            foreach (DataRow drpid in EADs.Tables["FamilyPro"].Rows)
        //            {
        //                tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
        //                tmpProds = tmpProds.Replace(",,", ",");
        //            }
        //            if (tmpProds != "")
        //            {
        //                tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);

        //                dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(_UserID));
        //            }
        //        }


        //        Dsall = objHelperDB.GetDataSetDB(ExecString);

        //        if (Dsall == null && Dsall.Tables.Count <= 0)
        //            return -1;


        //        // Main fl
        //        DrMain = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");
        //        if (DrMain.Length > 0)
        //        {
        //            // Famtb = Dr.CopyToDataTable();
        //            ConstructFamilyData(_Fid, DrMain, Dsall, tblinx);
        //        }
        //        // Sub fl


        //        if (Drsub.Length > 0)
        //        {

        //            if (SFamtb != null)
        //            {
        //                for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
        //                {
        //                    Dr = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + SFamtb.Rows[i]["FAMILY_ID"].ToString() + "'");
        //                    if (Dr.Length > 0)
        //                    {
        //                        // Ftb = Dr.CopyToDataTable();
        //                        tblinx = tblinx + 1;
        //                        ConstructFamilyData(SFamtb.Rows[i]["FAMILY_ID"].ToString(), Dr, Dsall, tblinx);
        //                    }
        //                    //_Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
        //                    //_Familyids = _Familyids.Replace(",,", ",");
        //                }

        //            }

        //        }


        //        return 1;


        //    }

        //    return -1;

        //}
        //private void ConstructFamilyData(string familyid, DataRow[] SourceFtb, DataSet tempDs, int tableinx)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        if (tempDs != null && tempDs.Tables.Count > 0 && tempDs.Tables[tableinx].Rows.Count > 0)
        //        {
        //            Ds.Tables.Add(familyid);

        //            Ds.Tables[familyid].Columns.Add("FAMILY_ID", typeof(string));
        //            Ds.Tables[familyid].Columns.Add("PRODUCT_ID", typeof(string));
        //            Ds.Tables[familyid].Columns.Add("TWeb Image1", typeof(string));
        //            Ds.Tables[familyid].Columns.Add("Code", typeof(string));

        //            for (int i = 0; i <= tempDs.Tables[tableinx].Columns.Count - 1; i++)
        //            {
        //                if (tempDs.Tables[tableinx].Columns[i].ColumnName.ToUpper() != "FAMILY_ID" && tempDs.Tables[tableinx].Columns[i].ColumnName.ToUpper() != "PRODUCT_ID")
        //                {
        //                    Ds.Tables[familyid].Columns.Add(tempDs.Tables[tableinx].Columns[i].ColumnName.ToUpper(), typeof(string));
        //                }
        //            }

        //            DataRow[] tempDr = null;
        //            foreach (DataRow tdr in SourceFtb)
        //            {
        //                DataRow Dsdr = Ds.Tables[familyid].NewRow();
        //                Dsdr["FAMILY_ID"] = tdr["FAMILY_ID"];
        //                Dsdr["PRODUCT_ID"] = tdr["PRODUCT_ID"];
        //                Dsdr["TWeb Image1"] = tdr["PRODUCT_TH_IMAGE"];
        //                Dsdr["CODE"] = tdr["PRODUCT_CODE"];
        //                Dsdr["COST"] = tdr["PRODUCT_PRICE"];

        //                tempDr = null;

        //                tempDr = tempDs.Tables[tableinx].Select("FAMILY_ID='" + familyid + "' And PRODUCT_ID='" + tdr["PRODUCT_ID"] + "'");
        //                if (tempDr.Length > 0)
        //                {
        //                    var Dr = tempDr[0].Table.Columns;



        //                    for (int i = 0; i <= Dr.Count - 1; i++)
        //                    {
        //                        if (Dr[i].ColumnName.ToUpper() != "FAMILY_ID" && Dr[i].ColumnName.ToUpper() != "PRODUCT_ID" && Dr[i].ColumnName.ToUpper() != "COST")
        //                        {
        //                            try
        //                            {
        //                                Dsdr[Dr[i].ColumnName.ToUpper()] = tempDr[0][Dr[i].ColumnName.ToUpper()];
        //                            }
        //                            catch
        //                            {
        //                            }
        //                        }
        //                    }
        //                }
        //                Ds.Tables[familyid].Rows.Add(Dsdr);
        //            }




        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog(ex.ToString() + "," + familyid + "," + tableinx);

        //    }
        //}
        //public string Generateparentfamilyhtml()
        //{
        //    try
        //    {
        //        contentvalue = "";
        //        if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "")
        //        {
        //            //DDS = GetDataSetFX(Request.QueryString["fid"].ToString());
        //            templatename = "MICROSITE\\CSFAMILYPAGE";
        //            tbwtEngine = new TBWTemplateEngine(templatename, Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        //            if (Request.QueryString["fid"] != null)
        //            {
        //                tbwtEngine.paraValue = Request.QueryString["fid"].ToString();
        //                tbwtEngine.paraFID = Request.QueryString["fid"].ToString();
        //            }
        //            //tbwtEngine.RenderHTML("Row");            
        //            //if(tbwtEngine.RenderedHTML!=null)
        //            //contentvalue = tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

        //            contentvalue = tbwtEngine.ST_Family_Load(EADs);
        //            if (contentvalue != null)
        //                contentvalue = contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        //        }
        //        return objHelperServices.StripWhitespace(contentvalue);

        //    }


        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return string.Empty;
        //    }

        //}
        //public string ST_Family_Download(DataSet TmpDs, DataSet TempEADs, int tableinx)
        //{
        //    try
        //    {
        //        string rtnstr = "";
        //        StringTemplateGroup _stg_container = null;
        //        StringTemplateGroup _stg_records = null;
        //        StringTemplate _stmpl_container = null;
        //        StringTemplate _stmpl_records = null;
        //        StringTemplate _stmpl_records1 = null;
        //        StringTemplate _stmpl_recordsrows = null;
        //        TBWDataList[] lstrecords = new TBWDataList[0];
        //        TBWDataList[] lstrows = new TBWDataList[0];

        //        StringTemplateGroup _stg_container1 = null;
        //        StringTemplateGroup _stg_records1 = null;
        //        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //        TBWDataList1[] lstrows1 = new TBWDataList1[0];


        //        DataTable dt = new DataTable();
        //        DataRow[] dr = null;

        //        int ictrecords = 0;

        //        if (Request.QueryString["fid"] != null)
        //            _Fid = Request.QueryString["fid"].ToString();

        //        //  _Familyids = _Fid;

        //        ////  DataSet TmpDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];

        //        //  dr = TmpDs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
        //        //  if (dr.Length > 0)
        //        //  {
        //        //      SFamtb = dr.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
        //        //      if (SFamtb != null)
        //        //      {
        //        //          for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
        //        //          {
        //        //              _Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
        //        //              _Familyids = _Familyids.Replace(",,", ",");
        //        //          }
        //        //      }
        //        //  }

        //        _stg_container = new StringTemplateGroup("main", stemplatepath);

        //        _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadMain");



        //        if (_Familyids != "")
        //        {

        //            // DataSet TempEADs = GetFamilyPageProduct(_Familyids, "ATTACHMENT");
        //            if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[tableinx].Rows.Count > 0)
        //            {
        //                //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");



        //                string[] strf = _Familyids.Split(new string[] { "," }, StringSplitOptions.None);
        //                if (strf.Length > 0)
        //                {
        //                    lstrecords = new TBWDataList[strf.Length];
        //                    for (int intfam = 0; intfam <= strf.Length - 1; intfam++)
        //                    {
        //                        dr = null;
        //                        dr = TempEADs.Tables[tableinx].Select("FAMILY_ID='" + strf[intfam] + "'", "Sno");
        //                        if (dr.Length > 0)
        //                        {
        //                            //dt = new DataTable();
        //                            //dt = dr.CopyToDataTable();
        //                            rtnstr = ST_Familypage_Download(_Fid, strf[intfam], dr);
        //                            if (rtnstr != "")
        //                            {
        //                                lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
        //                                ictrecords = ictrecords + 1;
        //                            }

        //                        }
        //                    }





        //                }
        //            }





        //        }
        //        _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //        string DownloadST_Product = ST_Product_Download(TmpDs);
        //        _stmpl_container.SetAttribute("PRODUCT_DOWNLOAD", DownloadST_Product);

        //        if (ictrecords > 0 || DownloadST_Product != "")
        //        {

        //            DownloadST = _stmpl_container.ToString();
        //            isDownload = true;
        //            return "block";
        //        }


        //        DownloadST = "";
        //        isDownload = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();

        //    }
        //    return "none";


        //}
        //public string ST_Familypage()
        //{
        //    string _cid = "";
        //    string _pcr = "";
        //    string _UserID = "";
        //    if (Request.QueryString["fid"] != null)
        //        _Fid = Request.QueryString["fid"].ToString();
        //    if (Request.QueryString["cid"] != null)
        //        _cid = Request.QueryString["cid"].ToString();
        //    if (Request.QueryString["pcr"] != null)
        //        _pcr = Request.QueryString["pcr"].ToString();

        //    if (HttpContext.Current.Session["USER_ID"].ToString() != null)
        //        _UserID = HttpContext.Current.Session["USER_ID"].ToString();

        //    _Familyids = _Fid;
        //    HttpContext.Current.Session["prodcodedesc"] = null;
        //    try
        //    {
        //        contentvalue = "";
        //        if (Request.QueryString["fid"] != null)
        //        {

        //            #region comments

        //            #endregion
        //            //by jtech
        //            CScontentvalue = ObjFamilyPage.GenerateHorizontalHTMLJson(_Fid, Ds, dsPriceTableAll, EADs);

        //            if (Famtb.Rows.Count == 1 && SFamtb.Rows.Count == 0 && (Request.QueryString["ProductResult"] != null && Request.QueryString["ProductResult"].ToString().Equals("SUCCESS")))
        //            {
        //                Response.Redirect("/pd.aspx?&pid=" + Famtb.Rows[0]["Product_ID"].ToString() + "&fid=" + Request.QueryString["fid"].ToString() + "&cid=" + _cid + "&pcr=" + _pcr + "byp=2", true);
        //                return "";
        //            }
        //            else if (SFamtb != null)
        //            {
        //                string subfamproduct = "";
        //                if (HttpContext.Current.Session["prodcodedesc"] != null)
        //                    subfamproduct = HttpContext.Current.Session["prodcodedesc"].ToString();
        //                foreach (DataRow DR in SFamtb.Rows)
        //                {
        //                    string cssubfamilycontent;
        //                    //cssubfamilycontent = getcstable(DR["Family_id"].ToString());

        //                    cssubfamilycontent = ObjFamilyPage.GenerateHorizontalHTMLJson(DR["Family_id"].ToString(), Ds, dsPriceTableAll, EADs);
        //                    if (cssubfamilycontent != "" && cssubfamilycontent.Length > 336)
        //                    {
        //                        templatename = "MICROSITE\\CSFAMILYPAGEWITHSUBFAMILY";
        //                        tbwtEngine = new TBWTemplateEngine(templatename, Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        //                        tbwtEngine.paraValue = DR["Family_id"].ToString();
        //                        tbwtEngine.paraFID = DR["Family_id"].ToString();

        //                        //tbwtEngine.RenderHTML("Row");
        //                        //subfamtemplate = subfamtemplate + tbwtEngine.RenderedHTML;
        //                        subfamtemplate = subfamtemplate + tbwtEngine.ST_SubFamily_Load(EADs);
        //                        subfamtemplate = subfamtemplate + cssubfamilycontent;
        //                    }
        //                    else
        //                    {
        //                        cssubfamilycontent = "";
        //                    }
        //                    subfamproduct = subfamproduct + HttpContext.Current.Session["prodcodedesc"].ToString();
        //                }
        //                HttpContext.Current.Session["prodcodedesc"] = subfamproduct;
        //            }
        //            //templatename = "CSFAMILYPAGELOGO";
        //            //tbwtEngine = new TBWTemplateEngine(templatename, Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        //            //tbwtEngine.RenderHTML("Row");
        //            if (subfamtemplate != "")
        //                subfamtemplate = "<div class=\"title7\">Related Products</div>" + subfamtemplate;
        //            //contentvalue = contentvalue + "<div style=\"overflow:auto; width:780px; height:100%;\">" + CScontentvalue + subfamtemplate + "</div>" + tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
        //            contentvalue = contentvalue + "<div style=\" width:769px; height:100%;\"><div class=\"fpscroll\">" + CScontentvalue + "</div>" + subfamtemplate + "</div>"; //+ tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return e.Message;
        //    }
        //    return objHelperServices.StripWhitespace(contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));

        //}
        //public string ST_Downloads_Update()
        //{
        //    StringTemplateGroup _stg_container = null;
        //    StringTemplate _stmpl_container = null;
        //    try
        //    {

        //        _stg_container = new StringTemplateGroup("main", stemplatepath);

        //        if (chkdwld == false)
        //            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DowloadUpdate");
        //        else
        //            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "WithDowloadUpdate");

        //        if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
        //        {
        //            _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
        //            _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
        //        }

        //        if (HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] != null)
        //            _stmpl_container.SetAttribute("TBT_FAMILY_NAME_DU", HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString());
        //        //if (chkdwld == false)
        //        //    _stmpl_container.SetAttribute("IS_DWLD_MRGE", true);
        //        //  else
        //        //    _stmpl_container.SetAttribute("IS_DWLD_MRGE", false);

        //        return _stmpl_container.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();

        //    }
        //    return "";
        //}
        //public string ST_BulkBuyPP()
        //{
        //    StringTemplateGroup _stg_container = null;
        //    StringTemplate _stmpl_container = null;
        //    StringTemplateGroup _stg_records1 = null;
        //    StringTemplate _stg_container_records1 = null;
        //    try
        //    {

        //        _stg_container = new StringTemplateGroup("main", stemplatepath);

        //        _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "BulkBuyPP");


        //        _stg_records1 = new StringTemplateGroup("row", stemplatepath);
        //        // _stg_container_records1 = _stg_records1.GetInstanceOf("Familypage" + "\\" + "multilistitem");
        //        string shtml = "";
        //        if (HttpContext.Current.Session["prodcodedesc"] != null)
        //        {
        //            string codedescall = HttpContext.Current.Session["prodcodedesc"].ToString();
        //            string[] codedesc = codedescall.Split('|');
        //            for (int i = 0; i < codedesc.Length - 1; i++)
        //            {
        //                _stg_container_records1 = _stg_records1.GetInstanceOf("Familypage" + "\\" + "multilistitem");
        //                string prodcode = "";
        //                prodcode = codedesc[i].Trim();
        //                //_stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
        //                //if (i == 0)
        //                //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");

        //                if (codedesc.Length > 2)
        //                {
        //                    _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
        //                    _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", true);
        //                }
        //                else
        //                {
        //                    _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
        //                    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
        //                    _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", false);
        //                }


        //                //if (i == 0)
        //                //{
        //                //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", "Please Select Product");
        //                //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
        //                //}
        //                //else
        //                //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);

        //                shtml = shtml + _stg_container_records1.ToString();
        //            }
        //            if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
        //            {
        //                _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
        //                _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
        //            }

        //            if (HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] != null)
        //                _stmpl_container.SetAttribute("TBT_FAMILY_NAME_BB", HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString());
        //            // _stmpl_container.SetAttribute("TBW_DDL_VALUE", _stg_container_records1.ToString());
        //            _stmpl_container.SetAttribute("TBW_DDL_VALUE", shtml.ToString());
        //        }
        //        else
        //        {
        //            Response.Redirect("home.aspx");
        //        }
        //        return _stmpl_container.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();

        //    }
        //    return "";
        //}
        //public string ST_Familypage_Download(string pFamilyid, string Familyid, DataRow[] Adt)
        //{
        //    StringTemplateGroup _stg_container = null;
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplate _stmpl_container = null;
        //    StringTemplate _stmpl_records = null;

        //    TBWDataList[] lstrecords = new TBWDataList[0];
        //    TBWDataList[] lstrows = new TBWDataList[0];


        //    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //    TBWDataList1[] lstrows1 = new TBWDataList1[0];



        //    long FileInKB;
        //    string[] file = null;
        //    string strfile = "";
        //    if (Adt.Length > 0)  //if (Adt != null && Adt.Rows.Count > 0)
        //    {




        //        DataSet dscat = new DataSet();


        //        try
        //        {
        //            _stg_records = new StringTemplateGroup("cell", stemplatepath);
        //            _stg_container = new StringTemplateGroup("row", stemplatepath);


        //            lstrecords = new TBWDataList[Adt.Length + 1];



        //            int ictrecords = 0;

        //            foreach (DataRow dr in Adt)//For Records
        //            {
        //                strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\", "/");


        //                FileInfo Fil;


        //                if (dr["FAMILY_ATT_FILE"].ToString().ToLower().Contains(".jpg") == true)
        //                    Fil = new FileInfo(strImgFiles1 + dr["FAMILY_ATT_FILE"].ToString());
        //                else
        //                    Fil = new FileInfo(strPDFFiles1 + dr["FAMILY_ATT_FILE"].ToString());


        //                if (Fil.Exists)
        //                {
        //                    _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell");

        //                    strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\", "/");
        //                    strfile = strfile.Replace(@"\", "/");

        //                    file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
        //                    if (file.Length > 0)
        //                    {

        //                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
        //                        if (file[file.Length - 1].ToString().ToLower().Contains(".jpg") == true)
        //                            _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
        //                        else
        //                            _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "attachments");
        //                    }

        //                    //  FileInBytes = Fil.Length;
        //                    FileInKB = Fil.Length / 1024;


        //                    _stmpl_records.SetAttribute("TBT_FAMILY_ATT_DESC", dr["FAMILY_ATT_DESC"].ToString());


        //                    _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["FAMILY_ATT_FILE"].ToString());
        //                    _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


        //                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
        //                    ictrecords++;
        //                }
        //            }

        //            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadRow");

        //            _stmpl_container.SetAttribute("TBT_FAMILY_NAME", Adt[0]["FAMILY_NAME"].ToString());
        //            if (pFamilyid.ToLower() == Familyid.ToLower())
        //            {
        //                _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", false);
        //            }
        //            else
        //            {
        //                _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", true);
        //            }



        //            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //            if (ictrecords > 0)
        //                return _stmpl_container.ToString();

        //        }
        //        catch (Exception ex)
        //        {
        //            objErrorHandler.ErrorMsg = ex;
        //            objErrorHandler.CreateLog();
        //            return "";
        //        }

        //        return "";
        //    }
        //    return "";
        //}
        //public string ST_Product_Download(DataSet TmpDs)
        //{
        //    string rtnstr = "";
        //    // StringTemplateGroup _stg_container = null;
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplate _stmpl_container = null;
        //    StringTemplate _stmpl_records = null;
        //    StringTemplate _stmpl_records1 = null;
        //    StringTemplate _stmpl_recordsrows = null;
        //    TBWDataList[] lstrecords = new TBWDataList[0];
        //    TBWDataList[] lstrows = new TBWDataList[0];

        //    StringTemplateGroup _stg_container1 = null;
        //    StringTemplateGroup _stg_records1 = null;
        //    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //    TBWDataList1[] lstrows1 = new TBWDataList1[0];
        //    string downloadst_pro = string.Empty;


        //    DataTable dt = new DataTable();
        //    DataRow[] dr = null;

        //    int ictrecords = 0;



        //    string DownloadST_Product = "";




        //    string _pid_multiple = string.Empty;
        //    string pcode_multiple = string.Empty;
        //    for (int prd = 0; prd <= TmpDs.Tables["FamilyPro"].Rows.Count - 1; prd++)
        //    {

        //        string _pid = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_ID"].ToString();
        //        string prodcode = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_CODE"].ToString() + " - Product Downloads";
        //        if (_pid != "")
        //        {
        //            if (_pid_multiple == string.Empty)
        //            {
        //                _pid_multiple = _pid;
        //                pcode_multiple = prodcode;
        //            }
        //            else
        //            {
        //                _pid_multiple = _pid_multiple + "," + _pid;
        //                pcode_multiple = pcode_multiple + "," + prodcode;

        //            }
        //        }

        //    }

        //    DataSet TempEADs_pid = objFamilyServices.GetFamilyPageProduct(_pid_multiple, "PRODUCT_ATTACHMENT");
        //    string[] pid = _pid_multiple.Split(',');
        //    string[] pcode = pcode_multiple.Split(',');
        //    for (int i = 0; i <= pid.Length - 1; i++)
        //    {
        //        DataRow[] drpid = TempEADs_pid.Tables[0].Select("PRODUCT_ID='" + pid[i] + "'");
        //        if (drpid.Length > 0)
        //        {

        //            rtnstr = ST_Productpage_Download(drpid, pcode[i].ToString());
        //            if (rtnstr != "")
        //            {
        //                DownloadST_Product = DownloadST_Product + rtnstr;
        //            }
        //        }

        //    }
        //    return DownloadST_Product;

        //}
        //public string ST_Productpage_Download(DataRow[] Adt, string Protitle)
        //{

        //    StringTemplateGroup _stg_container = null;
        //    StringTemplateGroup _stg_records = null;
        //    StringTemplate _stmpl_container = null;
        //    StringTemplate _stmpl_records = null;

        //    TBWDataList[] lstrecords = new TBWDataList[0];
        //    TBWDataList[] lstrows = new TBWDataList[0];


        //    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //    TBWDataList1[] lstrows1 = new TBWDataList1[0];


        //    string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
        //    string strImgFiles1 = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        //    long FileInKB;
        //    string[] file = null;
        //    string strfile = "";
        //    if (Adt != null && Adt.Length > 0)
        //    {




        //        DataSet dscat = new DataSet();


        //        try
        //        {
        //            _stg_records = new StringTemplateGroup("cell", stemplatepath);
        //            _stg_container = new StringTemplateGroup("row", stemplatepath);

        //            // _stg_container = new StringTemplateGroup("row", _SkinRootPath);


        //            lstrecords = new TBWDataList[Adt.Length + 1];



        //            int ictrecords = 0;

        //            foreach (DataRow dr in Adt)//For Records
        //            {
        //                strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


        //                FileInfo Fil;



        //                if (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg") == true)
        //                    Fil = new FileInfo(strImgFiles1 + dr["PRODUCT_ATT_FILE"].ToString());
        //                else
        //                    Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());


        //                if (Fil.Exists)
        //                {
        //                    _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell_Product");

        //                    strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");
        //                    strfile = strfile.Replace(@"\", "/");

        //                    file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
        //                    if (file.Length > 0)
        //                    {
        //                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
        //                        if (file[file.Length - 1].ToString().ToLower().Contains(".jpg") == true)
        //                            _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
        //                        else
        //                            _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "attachments");
        //                    }

        //                    //  FileInBytes = Fil.Length;
        //                    FileInKB = Fil.Length / 1024;

        //                    _stmpl_records.SetAttribute("TBT_PRODUCT_ATT_DESC", dr["PRODUCT_ATT_DESC"].ToString());

        //                    _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["PRODUCT_ATT_FILE"].ToString());
        //                    _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


        //                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
        //                    ictrecords++;
        //                }
        //            }

        //            _stmpl_container = _stg_container.GetInstanceOf("FamilyPage" + "\\" + "DownloadRow_Product");

        //            _stmpl_container.SetAttribute("TBT_PRODUCT_CODE", Protitle);
        //            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //            if (ictrecords > 0)
        //            {
        //                //isdownload_product = true;
        //                return _stmpl_container.ToString();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            objErrorHandler.ErrorMsg = ex;
        //            objErrorHandler.CreateLog();
        //            return "";
        //        }

        //        return "";
        //    }
        //    return "";
        //}

    }
}