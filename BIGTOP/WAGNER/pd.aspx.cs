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
using System.Globalization;
using System.Net;
using System.Web.Services;
using StringTemplate = Antlr3.ST.StringTemplate;  
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.TemplateRender;
using System.IO;
using System.Diagnostics;
public partial class ProductDetails : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    string stlistprodtitle = string.Empty;
   // string stlistprod = "";
    string stcategory = string.Empty;
  public  string productcode = string.Empty;
   // string sProd = "";
    string sfamily = string.Empty;
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
    protected static string ReCaptcha_Key = "6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY";
    protected static string ReCaptcha_Secret = "6LfMvyATAAAAAA2z47PSzT0QbWCXH72Yih8BMIer";
    string currenturl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString();

    string downloadST = "";
   private bool isdownload = false;
    private string _Package = "PRODUCT";
    private string _SkinRootPath = HttpContext.Current.Server.MapPath("~\\Templates");
    Stopwatch sw = new Stopwatch();
   // private bool chkdwld = false;

    //protected void Page_PreInit(object sender, EventArgs e)
    //{

    //    string newurl = Request.Url.PathAndQuery.Replace("/pd.aspx?", "").Replace("?", "");
    //    string Formname = Request.Url.LocalPath;

    //    string val = objHelperServices.URL_Rewrite_New(newurl, 1, "pd.aspx");
    //    if (val != string.Empty)
    //    {


    //        Context.RewritePath(val, false);
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorHandler objErrorHandler = new ErrorHandler();
        //sw.Start();
        //Session["PageUrl"] = Request.Url.PathAndQuery.ToString();
            //.Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Session["PageUrl"] = Request.RawUrl.ToString();
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE");
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

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {
       // sw.Start();
        ErrorHandler objErrorhandler = new ErrorHandler();
        try
        {
           
           // objErrorhandler.CreateLog("B4   Page_SaveStateComplete" + DateTime.Now);
            currenturl = currenturl.Substring(0, currenturl.Length - 1); 
            //DataSet dscat = new DataSet();
           // dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
            //Page.Title = "Cellink";
            DataTable dtprod = new DataTable();
            string procodeorg = string.Empty;
            string urlstring = string.Empty;
            var title="";
            var img = "";
            var ogdesc = "";
            string productid = string.Empty;
            string sortkey = string.Empty;
            if (Session["BreadCrumbDS"] != null)
            {

                DataSet ds = (DataSet)Session["BreadCrumbDS"];
              
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dsritemtype = string.Empty;
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
                       //if (i==0)
                       // {

                       //     h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                       // }
                    }

                    else if (dsritemtype == "family")
                    {

                        sfamily = ds.Tables[0].Rows[i]["FamilyName"].ToString();
                    
                        //h1.InnerText = objgetmetadata.Replace_SpecialChar(sfamily);

                    }


                    else if (dsritemtype == "product")
                    {

                        productcode = ds.Tables[0].Rows[i]["Productcode"].ToString();
                      //  ogtitle.cont = productcode;
                        //h4.InnerText = productcode;
                         title = "<meta property=\"og:title\" content='"+ productcode +"' />";
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
                
                Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString() ;
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

                //if (dscat != null)
                //{
                //    if((dscat.Tables.Contains("Product Tags")) && dscat.Tables["Product Tags"].Rows.Count > 0)
                //          skeyword = objHelperServices.MetaTagProductkeyword(dscat.Tables["Product Tags"]);
                //}

                //if (ds != null && ds.Tables[0].Rows.Count > 2)
                //    productid = ds.Tables[0].Rows[2]["ItemValue"].ToString();
              
                if (productcode != string.Empty)
                {
                    sortkey = objProductServices.GetProductSortKeyCode(productid);
                    skeyword = skeyword.Replace(",", " |") + sortkey;
                }
                string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);
                //Page.MetaKeywords = skeywordRe;
                Page.MetaKeywords = skeywordRe + " - Bigtop, wagneronline.com.au";


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
                string frjattr_name = string.Empty;
                for (int j = 0; j < foundRows.Length; j++)
                {
                    frjattr_name = foundRows[j]["attribute_Name"].ToString();

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
                        DescForh2 = objgetmetadata.Replace_SpecialChar(DescForh2);
                         ogdesc = "<meta property=\"og:description\" content='"+ DescForh2 +"'/>";
                        //ogdescription.InnerText = DescForh2; 
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

                        if (foundRows[j - 1]["STRING_VALUE"].ToString() != foundRows[j]["STRING_VALUE"].ToString() && frjattr_name != "Web Image1")
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
            string pagetitle = Page.Title.ToLower();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string TextTitle = textInfo.ToTitleCase(pagetitle);
            TextTitle = objgetmetadata.SetTitleCount(TextTitle);
           // Page.Title = TextTitle + " | " + "Wagner Electronics";  
            Page.Title = procodeorg + " " + TextTitle;
            //ogurl.InnerText =currenturl+ Request.RawUrl.ToString();   
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


            var ogtype = "<meta property=\"og:type\" content=\"Wagner:Product\" />";
               var ogsitename= "<meta property=\"og_site_name\" content='"+currenturl+"'/>";
               var ogurl = "<meta property=\"og_url\" content='"+currenturl+ Request.RawUrl +"' />";
            
               litMeta.Text = ogtype + ogdesc + title + img + ogsitename+ogurl;
        }
        catch
        { }

       // sw.Stop();
       //// StackTrace st = new StackTrace();
       //// StackFrame sf = st.GetFrame(0);
       // objErrorHandler.ExeTimelog = "Page_SaveStateComplete = " + sw.Elapsed.TotalSeconds.ToString();
      //  objErrorHandler.createexecutiontmielog(); 
       // objErrorhandler.CreateLog("after   Page_SaveStateComplete" + DateTime.Now);
    }
    [System.Web.Services.WebMethod]
    public static string SendAskQuestionMail(string fromid, string fname, string phone, string qustion, string productcode)
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
            MessageObj.Subject = "Bigtop-Form-Product-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td>Product Code </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
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
           // objErrorHandler.CreateLog(ex.ToString());
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

            string message = string.Empty;
            MessageObj.Subject = "Bigtop-Form-BulkBuy-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
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
            //objErrorHandler.CreateLog(ex.ToString());
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

            string message = string.Empty;
            MessageObj.Subject = "Bigtop-Form-Download-Request";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
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
           // objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
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
                 ProductDetails pd = new ProductDetails();

                 _stg_container = new StringTemplateGroup("main", pd._SkinRootPath);

                 _stmpl_container = _stg_container.GetInstanceOf(pd._Package + "\\shippinginfo");



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
    public static string AskaQuestionLoad(string value)
    {
         string sHTML = string.Empty;

         try
         {
             ProductDetails pd = new ProductDetails();
             StringTemplateGroup _stg_container = null;
             StringTemplate _stmpl_container = null;
             _stg_container = new StringTemplateGroup("main", pd._SkinRootPath);
             _stmpl_container = _stg_container.GetInstanceOf(pd._Package + "\\askaquestion");

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

        try
        {
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;
            ProductDetails pd = new ProductDetails();
            _stg_container = new StringTemplateGroup("main", pd._SkinRootPath );
            _stmpl_container = _stg_container.GetInstanceOf(pd._Package + "\\bulkbuy");

            if(HttpContext.Current.Session["FNAME_PRODUCT"] != null && HttpContext.Current.Session["FNAME_PRODUCT"] != "")
                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", HttpContext.Current.Session["FNAME_PRODUCT"].ToString());
            if(HttpContext.Current.Session["PRODUCT_CODE"] != null && HttpContext.Current.Session["PRODUCT_CODE"] != "")
                 _stmpl_container.SetAttribute("TBT_CODE", HttpContext.Current.Session["PRODUCT_CODE"].ToString());
            
            sHTML = _stmpl_container.ToString();
            return sHTML;
        }
        catch (Exception ex)
        {
     
        }
        return sHTML;
    }

    [System.Web.Services.WebMethod]
    public static string ProductDownloadLoad(string value)
    {
        string sHTML = string.Empty;
        string dwldmrge = string.Empty;
         ProductDetails pd = new ProductDetails();
        try
        {
          // HttpContext.Current.Session["isdownload"] = "false";
            pd.isdownload = false;
            sHTML = pd.ST_Product_Download(value);

           if (sHTML != "" && pd.isdownload == true)
           {
             //  pd.chkdwld = true;
               dwldmrge = pd.ST_Downloads_Update(true);
               sHTML = sHTML + dwldmrge;
           }
           else
           {
              // pd.chkdwld = false;
               dwldmrge = pd.ST_Downloads_Update(false);
               sHTML = sHTML + dwldmrge;
           }

 

            return sHTML;
        }
        catch (Exception ex)
        {
      
        }
        return sHTML;
    }
    public string ST_Product_Download(string _pid)
    {
        string rtnstr = string.Empty;
        StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_container = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];

        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];
        FamilyServices objFamilyServices = new FamilyServices();


        DataTable dt = new DataTable();
        // DataRow[] dr = null;

        int ictrecords = 0;
        downloadST = "";
        isdownload = false;
        if (_pid != "")
        {

            DataSet TempEADs = objFamilyServices.GetFamilyPageProduct(_pid, "PRODUCT_ATTACHMENT");
            if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[0].Rows.Count > 0)
            {
                //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");




                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                lstrecords = new TBWDataList[1];
                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadMain");


                rtnstr = ST_Productpage_Download(TempEADs.Tables[0]);
                if (rtnstr != "")
                {
                    lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
                    ictrecords = ictrecords + 1;
                }



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                if (ictrecords > 0)
                {

                    downloadST = _stmpl_container.ToString();
                    isdownload = true;
                    //HttpContext.Current.Session["isdownload"]="true";
                 }




            }
        }


        return downloadST;

    }
    public string ST_Productpage_Download(DataTable Adt)
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
        if (Adt != null && Adt.Rows.Count > 0)
        {




            DataSet dscat = new DataSet();


            try
            {
                _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_container = new StringTemplateGroup("row", _SkinRootPath);


                lstrecords = new TBWDataList[Adt.Rows.Count + 1];



                int ictrecords = 0;

                foreach (DataRow dr in Adt.Rows)//For Records
                {
                    strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


                    FileInfo Fil;


                    string strImgFilesnew = System.Configuration.ConfigurationManager.AppSettings["VirtualPathJPG"].ToString(); 
                    if (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg") == true)
                        Fil = new FileInfo(strImgFilesnew + dr["PRODUCT_ATT_FILE"].ToString());
                    else
                        Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());


                    if (Fil.Exists)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "DownloadCell");

                        strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");
                        strfile = strfile.Replace(@"\", "/");

                        file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
                        if (file.Length > 0)
                        {
                            _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
                            if (file[file.Length - 1].ToString().ToLower().Contains(".jpg") == true)
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
                            else
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "attachments");
                        }

                        //  FileInBytes = Fil.Length;
                        FileInKB = Fil.Length / 1024;

                        _stmpl_records.SetAttribute("TBT_PRODUCT_ATT_DESC", dr["PRODUCT_ATT_DESC"].ToString());
                        //Modified by indu 10Sep2015
                        //_stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["PRODUCT_ATT_FILE"].ToString());
                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", strfile.Replace("/Attachments/", "/attachments/").Replace(".PDF", ".pdf"));
                        _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadRow");


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
        ProductDetails pd = new ProductDetails();
        try
        {

            _stg_container = new StringTemplateGroup("main", _SkinRootPath);


               // if (strchkdwld == "false")

            if (dwnld == false)
                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DowloadUpdate");
            else
                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "WithDowloadUpdate");

                if (HttpContext.Current.Session["FNAME_PRODUCT"] != null)
                    _stmpl_container.SetAttribute("FNAME_DU", HttpContext.Current.Session["FNAME_PRODUCT"].ToString());

            return _stmpl_container.ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return "";
    }

    [System.Web.Services.WebMethod]
    public static string VerifyCaptcha(string response)
    {
        string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + ReCaptcha_Secret + "&response=" + response;
        return (new WebClient()).DownloadString(url);
    }
}


