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
public partial class AboutUs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);       
        HelperServices objHelperServices = new HelperServices();
      //  Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        //Page.Title = "Wagner-About Us";
        Page.Title = "About Us | Bigtop";
        //Page.MetaKeywords = "AboutUs,Wagner Australia";
        Page.MetaKeywords = "AboutUs, Bigtop, Wagneronline.com.au";
        Page.MetaDescription = "Wagner Australia is a supplier of quality Mobile Phone, Data, Media and Navigation Accessories. Our extensive range includes Parts and accessories to suit a vast range mobile phones, personal multimedia equipment and other devices, Wagner electronics, Wagneronline.com.au";
    }
}
