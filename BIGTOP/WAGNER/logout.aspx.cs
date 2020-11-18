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
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class logout : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    Security objSecurity = new Security();

    string suppliername = string.Empty; 
    protected void Page_Load(object sender, EventArgs e)
    {     
   
        try
        {
        HelperServices objHelperServices = new HelperServices();
        //Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Page.Title = "Logout | Bigtop";
            UserServices objUserServices = new UserServices();
        objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
        if (Session["pdffile"] != null && Session["pdffile"].ToString() != string.Empty)
        {
            string filename = Session["PdfFileName"].ToString();
            System.IO.FileInfo filin = new System.IO.FileInfo(Server.MapPath("~/Invoices\\" + "In" + filename + ".pdf"));
            bool fileExists = filin.Exists;
            if ((fileExists))
            {
                filin.Delete();
            }
            else
            {
                HttpContext.Current.Session["PdfFileName"] = null;
            }
        }
        if (Request.QueryString["Status"] != null)
        {
            if (Request.QueryString["Status"].ToLower() == "microsite")
            {
                if (Session["SUPPLIER_NAME"] != null)
                {
                    suppliername = Session["SUPPLIER_NAME"].ToString();
                }
            }
        }
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        Session["DUMMY_FLAG"] = "0";
        Session["ORDER_ID"] = "0";
        if (Request.Cookies["BigtopLoginInfo"] != null && Request.Cookies["BigtopLoginInfo"].Value.ToString().Trim() != "")
        {
            HttpCookie LoginInfoCookie = Request.Cookies["BigtopLoginInfo"];
            LoginInfoCookie["Login"] = objSecurity.StringEnCrypt("False");
            HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
        }

        if (Request.QueryString["Status"] != null)
        {
            if (Request.QueryString["Status"].ToLower() == "microsite")
            {
                objHelperServices.SimpleURL_MS_Str(suppliername, "mct.aspx", false);
                if (suppliername != "")
                {
                    Response.Redirect("/" + suppliername + "/mct/");
                }
                else
                {

                    Response.Redirect("/mLogin.aspx");
                }
            }
            else
            {
                Response.Redirect("/Login.aspx");
            }
        }
        else

        {
          Response.Redirect("/Login.aspx");
        }
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
}
