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
using System.Data.SqlClient;

using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
namespace WES
{
    public partial class mpd : System.Web.UI.Page
    {
        EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        string MicroSiteTemplate = "";
        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        string templatepath = "";
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
     
        ProductServices objProductServices = new ProductServices();
        string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();     
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
                currenturl = currenturl.Substring(0, currenturl.Length - 1); 
                if (strsupplierName == "")
                {
                    strsupplierName = GetSupplierName();
                }
                DataSet dscat = new DataSet();
                dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                //Page.Title = "Cellink";
                DataTable dtprod = new DataTable();
                string procodeorg = string.Empty;
                string urlstring = string.Empty;
                string productcode = string.Empty;
                string productid = string.Empty;
                string sortkey = string.Empty;
                var title = "";
                var img = "";
                var ogdesc = "";
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
                            //if (i==0)
                            // {

                            //     h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                            // }
                        }

                        else if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "family")
                        {

                            sfamily = ds.Tables[0].Rows[i]["FamilyName"].ToString();

                            //h1.InnerText = objgetmetadata.Replace_SpecialChar(sfamily);

                        }


                        else if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "product")
                        {

                            productcode = ds.Tables[0].Rows[i]["Productcode"].ToString();
                            //h4.InnerText = productcode;
                            title = "<meta property=\"og:title\" content='" + productcode + "' />";
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
                    Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString();
                    procodeorg = productcode.Replace("<ars>g</ars>", " ").ToString();
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
                        if (dscat.Tables.Contains("Product Tags") == true && dscat.Tables["Product Tags"].Rows.Count > 0)
                            skeyword = objHelperServices.MetaTagProductkeyword(dscat.Tables["Product Tags"]);
                    }

                    //if (ds != null && ds.Tables[0].Rows.Count > 2)
                    //    productid = ds.Tables[0].Rows[2]["ItemValue"].ToString();

                    if (productcode != "")
                    {
                        sortkey = objProductServices.GetProductSortKeyCode(productid);
                        skeyword = skeyword.Replace(",", " |") + sortkey;
                    }
                    string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);
                    //Page.MetaKeywords = skeywordRe;
                    Page.MetaKeywords = skeywordRe + "|"+strsupplierName+" - Wagner Electronics, wagneronline.com.au";


                }


                Page.MetaDescription = "List of products from Maincategory";


                Session["prodmodel"] = string.Empty;
                Session["S_FName"] = string.Empty;

                if (Session["FamilyProduct"] != null)
                {
                    DataSet dsdesc = (DataSet)Session["FamilyProduct"];
                    dtprod = dsdesc.Tables[0];
                    string expression = "attribute_Name in ('Description','Prod_Desc','Descriptions','Short Description','Description 1','Features','Notes','Note','Web Image1') ";
                    DataRow[] foundRows;
                    foundRows = dtprod.Select(expression);
                    string desc = string.Empty;
                    string DescForh2 = string.Empty;
                    for (int j = 0; j < foundRows.Length; j++)
                    {
                     string   frjattr_name = foundRows[j]["attribute_Name"].ToString();
                        if (frjattr_name == "Web Image1")
                        {
                             
                            string imgpath = objHelperServices.SetImageFolderPath(foundRows[j][1].ToString(), "_th", "_images");
                            img = "<meta property=\"og:image\" content='" + currenturl + imgpath + "' />";
                            //ogimage.InnerText =currenturl+ foundRows[j][1].ToString();
                            //h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                        }
                        if ((frjattr_name == "Description") || (frjattr_name == "Descriptions"))
                        {
                            DescForh2 = foundRows[j][1].ToString();
                            ogdesc = "<meta property=\"og:description\" content='" + DescForh2 + "'/>";
                            //h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                        }
                        if (j == 0)
                        {
                            if (frjattr_name == "Short Description")
                            {
                                DescForh2 = foundRows[j][1].ToString();
                                //h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                            }
                            desc = foundRows[j][1].ToString();
                        }
                        else
                        {
                            //if ((h2.InnerText == string.Empty) && (foundRows[j]["attribute_Name"].ToString() == "Short Description"))
                            //{
                            //    DescForh2 = foundRows[j][1].ToString();
                            //    h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                            //}

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
                    //if (h2.InnerText == string.Empty)
                    //{
                    //    h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                    //}


                    //Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);
                    Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc) + sortkey;

                }
                string pagetitle = stitle.ToLower() + "," + strsupplierName;
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string TextTitle = textInfo.ToTitleCase(pagetitle);
              string  newTextTitle = objgetmetadata.SetTitleCount(TextTitle);

              if (newTextTitle == "")
              {
                  Page.Title = newTextTitle;
              }
              else

              {
                  Page.Title = procodeorg + " " + TextTitle;
              }

              var ogtype = "<meta property=\"og:type\" content=\"Wagner:Product\" />";
              var ogsitename = "<meta property=\"og_site_name\" content='" + currenturl + "'/>";
              var ogurl = "<meta property=\"og_url\" content='" + currenturl + Request.RawUrl + "' />";

              litMeta.Text = ogtype + ogdesc + title + img + ogsitename + ogurl;
                // Page.Title = TextTitle + " | " + "Wagner Electronics";  
         
                //if (h1.InnerText == "")
                //{
                //    h1.Visible = false;
                //}
                //if (h2.InnerText == "")
                //{
                //    h2.Visible = false;
                //}
                //if (h3_2.InnerText == "")
                //{
                //    h3_2.Visible = false;
                //}
                //if (h3_3.InnerText == "")
                //{
                //    h3_3.Visible = false;
                //}


            }
            catch
            { }


        }

        [System.Web.Services.WebMethod]
        public static string SendAskQuestionMail(string fromid, string fname, string phone, string qustion, string productcode)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                string sHTML = "";
                HelperServices objHelperServices = new HelperServices();
                UserServices objUserServices = new UserServices();
                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

                UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
                string _UserID = "0";

                MessageObj.From = new System.Net.Mail.MailAddress(fromid);
                MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                //MessageObj.To.Add(fromid);
                string message = "";
                MessageObj.Subject = "Wagner-Form-Product-Enquiry";
                MessageObj.IsBodyHtml = true;


                string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"]);


                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();



                _stg_records = new StringTemplateGroup("row",   templatepath);
                _stg_container = new StringTemplateGroup("main", templatepath);
               // _stmpl_container.SetAttribute("SUPPIER_NAME", templatepath.Trim());

                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "product-AskQuestion");
                _stmpl_container.SetAttribute("PRODUCT_CODE", productcode);
                _stmpl_container.SetAttribute("URL", HttpContext.Current.Request.UrlReferrer.OriginalString);
                _stmpl_container.SetAttribute("FIRSTNAME", fname);
                _stmpl_container.SetAttribute("EMAILID", fromid);
                _stmpl_container.SetAttribute("PHONE", phone);
                _stmpl_container.SetAttribute("QUESTION", qustion);
               

                //message = message + "<tr><td>Product Code </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
                //message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
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
                        message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                        message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                    }
                    else
                    {
                       // message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
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
        public static string SendBulkBuyProjectPricing(string productcode, string fullname, string qty, string fromid, string deliverytime, string phone, string targetprice, string notesandaddtionalinfo)
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



                string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"]);


                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();



                _stg_records = new StringTemplateGroup("row", templatepath);
                _stg_container = new StringTemplateGroup("main", templatepath);

                string message = "";
                MessageObj.Subject = "Wagner-Form-BulkBuy-Enquiry";
                MessageObj.IsBodyHtml = true;

                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "product-BulkBuy");
                _stmpl_container.SetAttribute("PRODUCT_CODE", productcode);
                _stmpl_container.SetAttribute("URL", HttpContext.Current.Request.UrlReferrer.OriginalString);
                _stmpl_container.SetAttribute("FIRSTNAME", fullname);
                _stmpl_container.SetAttribute("QTY", qty);
                _stmpl_container.SetAttribute("EMAILID", fromid);
                _stmpl_container.SetAttribute("DELIVERYTIME", deliverytime);
                _stmpl_container.SetAttribute("PHONE", phone);
                _stmpl_container.SetAttribute("TARGETPRICE", targetprice);
                _stmpl_container.SetAttribute("NOTES", notesandaddtionalinfo);


                //message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
                //message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
                //message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
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
                        message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                        message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                    }
                    else
                    {
                        // message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
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
               // objErrorHandler.CreateLog(ex.ToString());
                return "-1".ToString();
            }
        }


        [System.Web.Services.WebMethod]
        public static string DownloadUpdate(string fullname, string fromid, string phone, string downloadrequire, string productcode)
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


                string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"]);


                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
                _stg_records = new StringTemplateGroup("row", templatepath);
                _stg_container = new StringTemplateGroup("main", templatepath);

                string message = "";
                MessageObj.Subject = "Wagner-Form-Download-Request";
                MessageObj.IsBodyHtml = true;


                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "product-DownloadUpdate");
                _stmpl_container.SetAttribute("PRODUCT_CODE", productcode);
                _stmpl_container.SetAttribute("URL", HttpContext.Current.Request.UrlReferrer.OriginalString);
                _stmpl_container.SetAttribute("FIRSTNAME", fullname);
                _stmpl_container.SetAttribute("EMAILID", fromid);
                _stmpl_container.SetAttribute("PHONE", phone);
                _stmpl_container.SetAttribute("DOWNLOADREQUIRED", downloadrequire);
               

                //message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
                //message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
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
    }
}