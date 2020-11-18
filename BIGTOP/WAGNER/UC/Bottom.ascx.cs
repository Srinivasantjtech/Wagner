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
using System.Diagnostics; 
public partial class UC_Bottom : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_bottom()
    {
       
          try
        {
      //  HelperServices objHelperServices = new HelperServices();
       // ConnectionDB objConnectionDB = new ConnectionDB();
       // Stopwatch sw = Stopwatch.StartNew();
         //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BOTTOM", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
       // TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BOTTOM", Server.MapPath("~\\Templates"), "");
        //tbwtEngine.RenderHTML("Row");
        //return (tbwtEngine.RenderedHTML);
        
             // string ss=tbwtEngine.ST_Bottom_Load();
              //sw.Stop();

              //return sw.Elapsed.ToString() + ss; 
        // return tbwtEngine.ST_Bottom_Load();
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
        string html = (string)Cache["Cache_bottom"];
              
            //  HttpContext.Current.Application["key_bottom"].ToString();

        if (!HttpContext.Current.Session["USER_ID"].Equals(""))
        {
            if (HttpContext.Current.Session["USER_ID"].ToString().Equals(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"]))
            {
                html = html.Replace("footer-myaccount-show ", "footer-myaccount-hide");
              
            }
            else 
            {
                html = html.Replace("footer-myaccount-hide", "footer-myaccount-show");
            }
        }
        else
        {
            html = html.Replace("footer-myaccount-show ", "footer-myaccount-hide");
        }
        //sw.Stop();
        //objErrorHandler.ExeTimelog = "ST_Bottom = " + sw.Elapsed.TotalSeconds.ToString();
        //objErrorHandler.createexecutiontmielog();

        return html;
        }
          catch (Exception ex)
          {
              objErrorHandler.ErrorMsg = ex;
              objErrorHandler.CreateLog();
              return string.Empty;
          }
          
    }

   
    
    //public string ST_bottom1()
    //{
    //    try
    //    {
    //        HelperServices objHelperServices = new HelperServices();
    //        ConnectionDB objConnectionDB = new ConnectionDB();
    //        Stopwatch sw = Stopwatch.StartNew();
    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BOTTOM", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
    //        tbwtEngine.RenderHTML("Row");
    //        string ss = (tbwtEngine.RenderedHTML);
            
    //         //tbwtEngine.ST_Bottom_Load();
    //        sw.Stop();

    //        return sw.Elapsed.ToString() + ss;

    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return string.Empty;
    //    }
    //}
}
