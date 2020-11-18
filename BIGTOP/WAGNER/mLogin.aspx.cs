using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.EasyAsk;
using System.Data;
using System.Web.Services;

using TradingBell5.CatalogX;

using TradingBell.WebCat.TemplateRender;

using System.Web.Configuration;
using System.Configuration;
using System.Globalization;
public partial class mLogin : System.Web.UI.Page
{
   
   // ErrorHandler objErrorHandler = new ErrorHandler();
    EasyAsk_WAGNER ObjEasyAsk = new EasyAsk_WAGNER();
    string MicroSiteTemplate = string.Empty;  
    protected void Page_Load(object sender, EventArgs e)
    {
        HelperServices objHelperServices = new HelperServices();
        MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"]);
        string login = System.Configuration.ConfigurationManager.AppSettings["login"];
        if (login == "no")
            objHelperServices.microsite_redirect();  
      
        //Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Page.Title = "Login | Wagner Electronics";
        Security objSecurity=new Security();
        string e1 = objSecurity.StringDeCrypt("");

        string e2  = objSecurity.StringDeCrypt("");
      
        //objHelperServices.Createnewdt_Login(); 

        
    }
   
}
