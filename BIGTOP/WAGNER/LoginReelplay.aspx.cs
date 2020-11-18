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
using System.Text;
using System.IO;
public partial class Login : System.Web.UI.Page
{
    Stopwatch sw = new Stopwatch();
    ErrorHandler objErrorHandler = new ErrorHandler();
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["IP_ADDR"] == null)
        {
            Session["IP_ADDR"] = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
         }

        string login = System.Configuration.ConfigurationManager.AppSettings["login"].ToString();
        if (login=="no")
        Response.Redirect("/Home.aspx",false  );  

        HelperServices objHelperServices = new HelperServices();
        //Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Page.Title = "Login | Wagner Electronics";
        Security objSecurity=new Security();
       // string e1 = objSecurity.StringDeCrypt("");

       // string e2  = objSecurity.StringDeCrypt("");
      
        //objHelperServices.Createnewdt_Login(); 

       
            
           
        

        //HtmlMeta meta = new HtmlMeta();
        //meta.Name = "keywords";
        //meta.Content = oHelper.GetOptionValues("Meta keyword").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Description" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Description";
        //meta.Content = oHelper.GetOptionValues("Meta Description").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Abstraction" content="Some words listed here" />

        //meta.Name = "Abstraction";
        //meta.Content = oHelper.GetOptionValues("Meta Abstraction").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Distribution" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Distribution";
        //meta.Content = oHelper.GetOptionValues("Meta Distribution").ToString();
        //this.Header.Controls.Add(meta);
      
        //meta.Name = "keywords";
        //meta.Content = oHelper.GetOptionValues("Meta keyword").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Description" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Description";
        //meta.Content = oHelper.GetOptionValues("Meta Description").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Abstraction" content="Some words listed here" />

        //meta.Name = "Abstraction";
        //meta.Content = oHelper.GetOptionValues("Meta Abstraction").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Distribution" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Distribution";
        //meta.Content = oHelper.GetOptionValues("Meta Distribution").ToString();
        //this.Header.Controls.Add(meta);
        //InitLoad();
        //txtUserName.Focus();
        //if (Request["Result"] == "SUCCESS")
        //    RegSucess.Visible = true;
        //int i = 1;
        //txtUserName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");
        //txtPassword.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");
        
        //if (!IsPostBack)
        //{
        //    try
        //    {
        //        sReferralURL = Request.ServerVariables["HTTP_REFERER"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }

}
