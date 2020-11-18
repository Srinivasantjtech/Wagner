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

public partial class UC_searchctrl : System.Web.UI.UserControl
{
    string breadcrumb = string.Empty;
    EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
    ErrorHandler objErrorHandler = new ErrorHandler();
      HelperServices objhelperservice = new HelperServices();

      public string strsortval = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        string hfclickedattr = Request.Form["hfclickedattr"] ;
        //objErrorHandler.CreateLog(hfclickedattr);
        //urlping="ps.aspx?"+HttpContext.Current.Request.QueryString.ToString(); 
        if ((hfclickedattr != null) && (hfclickedattr != ""))
        {

            string[] url = Request.RawUrl.ToString().Split(new string[] { "ps.aspx?" }, StringSplitOptions.None);
           // Session["hfclickedattr_ps"] = hfclickedattr.Replace("doublequot", @"""");

            Session["hfclickedattr_ps"] = HttpUtility.HtmlDecode(hfclickedattr);
            if (Session["hfclickedattr_ps"] != null)
            {
                string[] gettype = null;
                gettype = Session["hfclickedattr_ps"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);

                string _value = gettype[2];
                if (_value.Contains("::"))
                {
                    gettype = _value.Split(new string[] { "::" }, StringSplitOptions.None);

                    _value = gettype[1];
                }
                _value = objhelperservice.SimpleURL_Str(_value, "", false);

                Response.Redirect("/" + _value + url[0]);

            }
            else
            {
                Response.Redirect(url[0]);
            }
        }
        //else if (Request.RawUrl.ToString().Contains("ps.aspx?"))
        //{
        //    string[] url = Request.RawUrl.ToString().Split(new string[] { "ps.aspx?" }, StringSplitOptions.None);
        //    Response.Redirect(url[0]);
        //}
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


        if (HttpContext.Current.Session["SortOrder_ps"] != null && HttpContext.Current.Session["SortOrder_ps"] != "")
        {
            if (HttpContext.Current.Session["SortOrder_ps"].ToString() != "SortOrder_ps")
            {
                HttpContext.Current.Session["SortOrder"] = null;
             }
        }


        if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"] != "")
        {

            if (HttpContext.Current.Session["SortOrder"].ToString() == "Latest")
            {
                strsortval = "Latest";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "ltoh")
            {
                strsortval = "Price Low To High";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "htol")
            {
                strsortval = "Price High To Low";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "relevance")
            {
                strsortval = "Relevance";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "popularity")
            {
                strsortval = "Popular";
            }
            HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_op";
        }
        else
        {
            strsortval = "Relevance";
            HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_op";
        }


    }

    public string ST_Categories()
    {
       
          
                UC_maincategory ucmain = new UC_maincategory();
                return ucmain.ST_Categories();
           
       

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

        breadcrumb = EasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
        return breadcrumb;
    }

    public string Spell_Correction()
    {
        try
        {
            string SpellCorrection = string.Empty;
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

