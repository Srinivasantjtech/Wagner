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
public partial class ProductMS : System.Web.UI.UserControl
{
    // ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    string breadcrumb = "";
    string stemplatepath = "";
    EasyAsk_WAGNER objEasyAsk = new EasyAsk_WAGNER();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
    }
    public string Bread_Crumbs()
    {

        breadcrumb = objEasyAsk.GetBreadCrumb_Simple_MS(Server.MapPath("Templates"),true);
        return breadcrumb;
    }
    //protected void cVerify_preRender(object sender, EventArgs e)
    //{
    //    Session["AQ_PD_CAPTCH_VALUE"] = cVerify.Text;
    //    Session["AQ_PD_CAPTCH_IMAGE"] = cVerify.GuidPath;

    //}
    public string ST_Product()
    {
        try
        {
        ConnectionDB objConnectionDB = new ConnectionDB();
        TBWTemplateEngineMS tbwtEngineMS = new TBWTemplateEngineMS("PRODUCT", stemplatepath, objConnectionDB.ConnectionString);
        if (Request.QueryString["pid"] != null)
        {
            tbwtEngineMS.paraValue = Request.QueryString["pid"].ToString();
            tbwtEngineMS.paraPID = Request.QueryString["pid"].ToString();
        }
        if (Request.QueryString["fid"] != null)
            tbwtEngineMS.paraFID = Request.QueryString["fid"].ToString();
        if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")
            tbwtEngineMS.paraCID = Request.QueryString["cid"].ToString();
        tbwtEngineMS.RenderHTML("Column");
        objConnectionDB.CloseConnection();
        return (tbwtEngineMS.RenderedHTML);
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

}
