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
using CustomCaptcha;
using TradingBell5.CatalogX;
using TradingBell.WebCat.TemplateRender;
using System.Web.Configuration;
using System.Configuration;

namespace WES
{
    public partial class mcontactus : System.Web.UI.Page
    {
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        string MicroSiteTemplate = "";
        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
        }
        public string ST_mcontactus()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("MCONTACTUS", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_mcontactus_Load();
              
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        protected void cVerify_preRender(object sender, EventArgs e)
        {
            //Session["AQ_FL_CAPTCH_VALUE"] = cVerify.Text;
            //Session["AQ_FL_CAPTCH_IMAGE"] = cVerify.GuidPath;
            //contactcaptcha.Src = cVerify.GuidPath;
            //capchacode.Text = cVerify.Text;

        }
        [System.Web.Services.WebMethod]
        public static string SendmscontactusMail(string fullname,  string fromid, string phone, string notesandaddtionalinfo)
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


                string message = "";
                MessageObj.Subject = "Wagner-Vip-Vision-Form-Customer-Enquiry";
                MessageObj.IsBodyHtml = true;
               // message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
               // message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
               // message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
                message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fullname + "</td></tr>";
               // message = message + "<tr><td>QTY </td><td>&nbsp;</td><td>" + qty + "</td></tr>";
                message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
              //  message = message + "<tr><td>Delivery Time </td><td>&nbsp;</td><td>" + deliverytime + "</td></tr>";
                message = message + "<tr><td>Contact Number </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
               // message = message + "<tr><td>Target Price</td><td>&nbsp;</td><td>" + targetprice + "</td></tr>";
                message = message + "<tr><td>Enquiry/Comments</td><td>&nbsp;</td><td>" + notesandaddtionalinfo + "</td></tr>";
                message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"].ToString()  != "")
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

    }
}