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
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class byproduct : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {
        //***********************later want to be update with default page************//
         try
        {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        if(Session["RECORDS_PER_PAGE"] == null || Session["RECORDS_PER_PAGE"] == "")
            Session["RECORDS_PER_PAGE"] = objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        //**********************End**************
        if(IsPostBack)        
        {
                if (Request.Form["__EVENTTARGET"].ToString()=="compare,")                
                {
                    string s = Request.Form["__EVENTARGUMENT"].ToString().Substring(0,Request.Form["__EVENTARGUMENT"].ToString().Length-1);//.LastIndexOf("Fid".ToCharArray());
                    string[] str = Request.Form["__EVENTARGUMENT"].Split('$');
                    int FamilyID = objHelperServices.CI(str[0]);
                    Session["CloseWin"] = str[1];
                    Session["FAMILY_ID"] = str[0];
                    Response.Redirect("/Compare.aspx");
                }           
        }

        }


         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();

         }
    }

}
