
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
using System.Data.SqlClient;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_bybrand : System.Web.UI.UserControl
{
    
    EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
    ErrorHandler objErrorHandler = new ErrorHandler();
    int iCatalogId;
    int iInventoryLevelCheck;
    int iRecordsPerPage = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["iRecordsPerPage"].ToString());
   // bool bIsStartOver = true;
    bool bDoPaging;
    int iPageNo = 1;
    string catID = "";
    HelperServices objhelperservice = new HelperServices(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        string hfclickedattr = Request.Form["hfclickedattr"];
       // objErrorHandler.CreateLog(hfclickedattr);
        if ((hfclickedattr != null) && (hfclickedattr != ""))
        {

            string[] url = Request.RawUrl.ToString().Split(new string[] { "bb.aspx?" }, StringSplitOptions.None);
            Session["hfclickedattr_bb"] = hfclickedattr.Replace("doublequot", @"""");
            if (Session["hfclickedattr_bb"] != null)
            {
                string[] gettype = null;
                gettype = Session["hfclickedattr_bb"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);

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

        if (!IsPostBack)
        {
            if (Request.QueryString["pgno"] != null)
            {
                iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
            }
            GetStoreConfig();
            GetPageConfig();

            HiddenField1.Value = "0";
            HiddenField2.Value = "0";
            hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            hfnewurl.Value = Request.RawUrl.ToString();
                //.Replace("bb/","bb.aspx?");
            hfcheckload.Value = "0";
            HFcnt.Value = "1";
            hfback.Value = "";
            hfbackdata.Value = "";
        }
    }
    public string ST_Categories()
    {
        UC_maincategory ucmain = new UC_maincategory();
        return ucmain.ST_Categories();

    }
    private void GetStoreConfig()
    {
        iCatalogId = Convert.ToInt32(Session["CATALOG_ID"].ToString());
        iInventoryLevelCheck = Convert.ToInt32(Session["INVENTORY_LEVEL_CHECK"].ToString());
    }

    private void GetPageConfig()
    {
        try
        {
            //if (Session["PS_IS_START_OVER"].ToString() == "YES")
            //{
            //    bIsStartOver = true;
            //}
            //else
            //{
            //    bIsStartOver = false;
            //}
            bDoPaging = Convert.ToBoolean(Session["DO_PAGING"].ToString());
            if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
            {
                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE"].ToString());
            }
            else
            {
                iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
                Session["RECORDS_PER_PAGE"] = HidItemPage.Value.ToString();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public string Bread_Crumbs()
    {
        string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "", byp = "";
        if (Request.QueryString["pid"] != null)
        {
            paraPID = Request.QueryString["pid"].ToString();
        }
        if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "List all products")
            paraFID = Request.QueryString["fid"].ToString();
        if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "List all models")
            paraCID = Request.QueryString["cid"].ToString();
        if (Request.QueryString["byp"] != null && Request.QueryString["byp"].ToString() != "")
            byp = Request.QueryString["byp"].ToString();
        
        if (Request.QueryString["cid"] != null)
            catID = Request.QueryString["cid"].ToString();
        try
        {
            breadcrumb = ObjEasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
        }
        catch(Exception ex)
        {
            return ex.Message.ToString();
        }
        return breadcrumb;

    }
       
}
