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

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_newproductlognav : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_Newproduct()
    {
         try
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperServices = new HelperServices();

            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCTLOGNAV", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            tbwtEngine.RenderHTML("Column");
            return tbwtEngine.ST_NewProductLogNav_Load(null,HttpContext.Current  );
    
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
