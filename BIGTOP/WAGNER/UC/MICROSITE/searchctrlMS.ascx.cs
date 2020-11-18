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
using TradingBell.WebCat.EasyAsk;

public partial class searchctrlMS : System.Web.UI.UserControl
{
    string breadcrumb = "";
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

        string hfclickedattr = Request.Form["hfclickedattr"] ;

        if (hfclickedattr != null)
        {

            string[] url = Request.RawUrl.ToString().Split(new string[] { "mps.aspx?" }, StringSplitOptions.None);
            //objErrorHandler.CreateLog(hfclickedattr);
            Session["hfclickedattr_mps"] = hfclickedattr.Replace("doublequot", @"""");

            Response.Redirect(url[0].ToLower().Replace("undefined", ""));
        }
        if (Request.RawUrl.ToString().ToLower().Contains("undefined"))
        {
            Response.Redirect(Request.RawUrl.ToString().ToLower().Replace("undefined", ""));
        }
        if (!IsPostBack)
        {
            HiddenField1.Value = "0";
            HiddenField2.Value = "0";
            hfcheckload.Value = "0";
            HFcnt.Value = "1";
            hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            hfnewurl.Value = Request.RawUrl.ToString();
                //.Replace("ps/","ps.aspx?") ;
            hfback.Value = "";
            hfbackdata.Value = "";
        }
    }
    //public string Bread_Crumbs()
    //{
    //    string breadcrumb = "";
    //    if (HttpContext.Current.Session["BreadCrumbName"] != null)
    //    {
    //        breadcrumb = HttpContext.Current.Session["BreadCrumbName"].ToString();
    //    }
    //     return breadcrumb;
    //}

    //public string Spell_Correction()
    //{
    //    string SpellCorrection = "";
    //    if (HttpContext.Current.Session["Spell_Correction"] != null || HttpContext.Current.Session["Spell_Correction"] == "")
    //    {
    //        SpellCorrection = "<strong>" + HttpContext.Current.Session["Spell_Correction"].ToString() + "</strong>";
    //    }
    //    return SpellCorrection;
    //}
    public string Bread_Crumbs()
    {

        breadcrumb = EasyAsk.GetBreadCrumb_Simple_MS(Server.MapPath("Templates"),true );
        return breadcrumb;
    }

    public string Spell_Correction()
    {
        try
        {
            string SpellCorrection = "";
            if (HttpContext.Current.Session["Spell_Correction"] != null || HttpContext.Current.Session["Spell_Correction"] == "")
            {
                SpellCorrection = "<strong>" + HttpContext.Current.Session["Spell_Correction"].ToString() + "</strong>";
            }
            return SpellCorrection;
        }
      
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
}

