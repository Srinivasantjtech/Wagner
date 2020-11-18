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

public partial class ContactUs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Title = "Wagner-ContactUs";
        Page.Title = "ContactUs | Bigtop";
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
       // Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
   
    }

    [System.Web.Services.WebMethod]
    public static string SendEnquiryMail(string fullname, string email, string phone, string enquiry)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            UC_contactus obj = new UC_contactus();
            obj.SaveContactUs(fullname,email,phone,enquiry);
            obj.SendEnquiry(fullname, email, phone, enquiry);
           //  HttpContext.Current.Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            // objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }
   
}
