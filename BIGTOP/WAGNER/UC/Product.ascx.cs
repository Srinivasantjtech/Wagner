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
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
public partial class UC_Products : System.Web.UI.UserControl
{
    // ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    string breadcrumb = string.Empty;
    EasyAsk_WAGNER objEasyAsk = new EasyAsk_WAGNER();
    ErrorHandler objErrorHandler = new ErrorHandler();
    Stopwatch sw = new Stopwatch();
    protected void Page_Load(object sender, EventArgs e)
    {
     
    }
    public string Bread_Crumbs()
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        breadcrumb = objEasyAsk.GetBreadCrumb(Server.MapPath("Templates"));

       // StackTrace st = new StackTrace();
       // StackFrame sf = st.GetFrame(0);
       // Security objErrorHandler = new Security();
        //sw.Stop();
        //objErrorHandler.ExeTimelog = "Bread_Crumbs = " + sw.Elapsed.TotalSeconds.ToString();
        //objErrorHandler.createexecutiontmielog();

        return breadcrumb;
    }
    //protected void cVerify_preRender(object sender, EventArgs e)
    //{
    //    Session["AQ_PD_CAPTCH_VALUE"] = cVerify.Text;
    //    Session["AQ_PD_CAPTCH_IMAGE"] = cVerify.GuidPath;

    //}
    public string ST_Product()
    {
        ErrorHandler objErrorhandler = new ErrorHandler();
        try
        {
           // objErrorhandler.CreateLog("B4   ST_Product" + DateTime.Now.ToString("hh.mm.ss.ffffff"));
        ConnectionDB objConnectionDB = new ConnectionDB();
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PRODUCT", Server.MapPath("Templates"), objConnectionDB.ConnectionString);
        if (Request.QueryString["pid"] != null)
        {
            tbwtEngine.paraValue = Request.QueryString["pid"].ToString();
            tbwtEngine.paraPID = Request.QueryString["pid"].ToString();
        }
        if (Request.QueryString["fid"] != null)
            tbwtEngine.paraFID = Request.QueryString["fid"].ToString();
        if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")
        {
            tbwtEngine.paraCID = Request.QueryString["cid"].ToString();
        }

        //string coopid_fid = tbwtEngine.paraPID + "," + tbwtEngine.paraFID;
        //if (Request.Cookies["recentpid"] == null)
        //{
        //    HttpCookie _userActivity = new HttpCookie("recentpid");
        //    _userActivity["Product"] = coopid_fid;
        //    _userActivity.Expires = DateTime.Now.AddDays(5);

        //    //Adding cookies to current web response
        //    Response.Cookies.Add(_userActivity);
        //}
        //else

        //{
        //    HttpCookie _userActivity = Request.Cookies["recentpid"];
        //    string prevCookie = coopid_fid + "|" + _userActivity["Product"];
        //    _userActivity["Product"] = prevCookie;
        //    Response.Cookies.Add(_userActivity);
        
        //}
      //tbwtEngine.RenderHTML("Column");
      //objConnectionDB.CloseConnection();
      // return (tbwtEngine.RenderedHTML);
     return (tbwtEngine.ST_Product_Load() );

        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }  

    }
    public string ST_RecentProduct()
    {
        try
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperServices = new HelperServices();

            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("RecentProduct", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            tbwtEngine.RenderHTML("Column");
            if (Request.QueryString["pid"] != null)
            {

                tbwtEngine.paraPID = Request.QueryString["pid"].ToString();
            }
            if (Request.QueryString["fid"] != null)
            {
                tbwtEngine.paraFID = Request.QueryString["fid"].ToString();
            }
            string currentpfid = tbwtEngine.paraPID + "," + tbwtEngine.paraFID + "|";
             string recentprod ="";
            if (HttpContext.Current.Request.Cookies["recentpid"] != null)
            {
                recentprod = tbwtEngine.ST_RECENT_COOKIE_PRODUCT(currentpfid);

            }

            string coopid_fid = tbwtEngine.paraPID + "," + tbwtEngine.paraFID;
            if (Request.Cookies["recentpid"] == null)
            {
                HttpCookie _userActivity = new HttpCookie("recentpid");
                _userActivity["Product"] = coopid_fid;              
                Response.Cookies.Add(_userActivity);
            }
            else
            {
               
                HttpCookie _userActivity = Request.Cookies["recentpid"];
                if ((!_userActivity["Product"].Contains(coopid_fid)) && (coopid_fid!=""))
                {
                    string prevCookie = coopid_fid + "|" + _userActivity["Product"];
                    _userActivity["Product"] = prevCookie;

                    Response.Cookies.Add(_userActivity);
                }

            }

            return recentprod;
            //if (Session["NewProductLogNav"] != null)
            //{
            //    //return tbwtEngine.ST_NewProductLogNav_Load((DataSet)Session["NewProductLogNav"]);
            //    return Session["NewProductLogNav"].ToString();
            //}
            //else
            //    return "";

            //return "";
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
}
