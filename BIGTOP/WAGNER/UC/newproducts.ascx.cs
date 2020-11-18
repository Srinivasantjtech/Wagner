using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;


public partial class UC_newpeoducts : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
   
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    public string ST_Newproduct()
    {

        try
        {
           // ConnectionDB objConnectionDB = new ConnectionDB();
           //// HelperServices objHelperServices = new HelperServices();

           //// TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCT", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCT", Server.MapPath("~\\Templates"), "");
           // //tbwtEngine.RenderHTML("Column");
           // //return (tbwtEngine.RenderedHTML);
           //// objErrorHandler.CreateLog("ST_RecentProduct"); 
            //return tbwtEngine.ST_NewProduct_Load();
            return tbwtEngine.ST_HOME_PRODUCT();
            //string html;
            //if (Cache["Cache_NEWPRODUCT"] != null)
            //{
            //     html = (string)Cache["Cache_NEWPRODUCT"];
            //}
            //else
            //{
            //    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCT", HttpContext.Current.Server.MapPath("~\\Templates"), "");
            //    html = tbwtEngine.ST_NewProduct_Load(); ;
            //    HttpRuntime.Cache.Insert("Cache_NEWPRODUCT",
            //                html,
            //             null,
            //       HttpRuntime.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60));
            //}
            //return html;
         }
         catch (Exception e)
         {
             objErrorHandler.ErrorMsg = e;
             objErrorHandler.CreateLog();
             return string.Empty;
         }
    }

    public string ST_PopularProduct()
    {
        try
        {
            if (Request.RawUrl.ToLower().Contains("home.aspx") || Request.RawUrl.ToLower() == "/")
            {


               // ConnectionDB objConnectionDB = new ConnectionDB();
               // HelperServices objHelperServices = new HelperServices();
                
                //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PopularProduct", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
             TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PopularProduct", Server.MapPath("~\\Templates"),"");
              //  tbwtEngine.RenderHTML("Column");



              //  objErrorHandler.CreateLog("ST_PopularProduct");

                return tbwtEngine.ST_POPULAR_PRODUCT();
                //string html = (string)Cache["Cache_POPULARPRODUCT"];
                //return html;
            }
            else
            {
                return "";
            }


        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

  
    //public string ST_RecentProduct()
    //{
    //    try
    //    {
    //        if (Request.RawUrl.ToLower().Contains("home.aspx") || Request.RawUrl.ToLower()=="/")
    //         {

    //             objErrorHandler.CreateLog("ST_RecentProduct"); 
    //        ConnectionDB objConnectionDB = new ConnectionDB();
    //        HelperServices objHelperServices = new HelperServices();

    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("RecentProduct", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
    //        tbwtEngine.RenderHTML("Column");


    //        if (HttpContext.Current.Request.Cookies["recentpid"] != null)
    //        {

    //            return tbwtEngine.ST_RECENT_COOKIE_PRODUCT("");
    //        }
    //        else
    //        {
    //            return "";
    //        }
    //         }
    //         else

    //         {
    //             return "";
    //         }


    //    }
    //    catch (Exception e)
    //    {
    //        objErrorHandler.ErrorMsg = e;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}
}
