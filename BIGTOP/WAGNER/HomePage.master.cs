using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using TradingBell.WebCat.CatalogDB ;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
using System.Configuration;
using System.Web.UI.HtmlControls;
public partial class MasterPage : System.Web.UI.MasterPage
{
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"];
    public string Cartdetail = "";
    public string CartCount = "";
    public string CartCount_mobile = "0";
    public string Cartdetail_mobile = "";
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    protected void Page_Load(object sender, EventArgs e)
    {


       
        loadheader();
        //loadleftnav();
       // loadmaincontentblock();
       // loadrightnav();
        loadfooter();
        //HtmlLink link = new HtmlLink();
        //link.Href =  "css/DynamicPageCSS.css";
        //link.Attributes.Add("type", "text/css");
        //link.Attributes.Add("rel", "stylesheet");
        //header.Controls.Add(link);
        //if (HttpContext.Current.Session["URLINI"] == null)
        //{
        //    objHelperServices.Createnewdt();
        //}  
        //if (Session["Notification"] == null)
        //{
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript: checksubscription(); ", true);
        //    Session["Notification"] = "Yes";
        //}
    }
    private void loadheader()
    {
        //FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\home\\header.st"), FileMode.Open);
        //System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        //string dataString = streamWriter.ReadToEnd();
        //streamWriter.Close();
        //fileStream.Close();
        //string[] str = dataString.Split('$');
        //for (int strc = 1; strc < str.Length; strc = strc + 2)
        //{
        //    //if (str[strc].ToUpper().Equals("TOP"))
        //    //{
        //    //    // if (Session["USER_ID"].ToString() == null || Session["USER_ID"].ToString().Equals("") || HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
        //    //    // {
        //    //    str[strc] = "toplog";
        //    //    // }
        //    //}
        //    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

        //    header.Controls.Add(ctl);
        //}


                  Control ctl = LoadControl("~/UC/" + "toplog.ascx");
                  header.Controls.Add(ctl);
    }

    private void loadleftnav()
    {
        FileStream fileStream = new FileStream(Server.MapPath("~\\Templates"+ "\\home\\leftnav.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc=strc+2){
            if ((str[strc] != "browsebycategory"))
            {
                Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
            }

            
           // leftnavigator.Controls.Add(ctl);
        }
    }

    private void loadmaincontentblock()
    {
        FileStream fileStream = new FileStream(Server.MapPath("~\\Templates" + "\\home\\maincontentblock.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            //maincontentblock.Controls.Add(ctl);
        }
    }

    private void loadrightnav()
    {
        FileStream fileStream = new FileStream(Server.MapPath("~\\Templates\\home\\rightnav.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            //rightnavigator.Controls.Add(ctl);
        }
    }

    public string ST_TOP_CATEGORY_SCROLLMENU()
    {
        try
        {
            //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TopScrollCategory", Server.MapPath("~\\Templates"), objConnectionDB.ConnectionString);
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TopScrollCategory", Server.MapPath("~\\Templates"), "");



            return tbwtEngine.ST_TOP_Category_Scroll();
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }
    public string ST_TOP_CatMenuMobile()
    {
        try
        {
           // TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TopMobile", Server.MapPath("~\\Templates"), objConnectionDB.ConnectionString);
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("TopMobile", Server.MapPath("~\\Templates"), "");
            return tbwtEngine.ST_Top_Load_Mobile();
        }


        catch (Exception ex)
        {
            //objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    public string ST_Top_Cart_item()
    {
        string RtnData;
        try
        {
          //  TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CartItems", Server.MapPath("~\\Templates"), objConnectionDB.ConnectionString);
            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CartItems", Server.MapPath("~\\Templates"), "");
            // tbwtMSEngine.cartitem = cartcount();
            //tbwtEngine.RenderHTML("Row");
            //return (tbwtEngine.RenderedHTML);
            //string formname = Request.Url.Segments[1].ToString();
            RtnData = tbwtEngine.ST_Top_Cart_item();
            //objErrorHandler.CreateLog(RtnData);
            if (RtnData != "")
            {
                string[] strsplit = RtnData.Split('~');

                if (strsplit.Length >= 2)
                {
                    CartCount = strsplit[0].ToString();
                    Cartdetail = strsplit[1].ToString();
                    CartCount_mobile = strsplit[2].ToString();
                    Cartdetail_mobile = strsplit[5].ToString();

                }
                else
                {
                    Cartdetail = "";
                    CartCount = "<a href=\"\" class=\"dropdown-toggle cart_drop\" data-toggle=\"dropdown\"><span ><img src=\"/images/MicroSiteimages/cart.png\" class=\"margin_left margin_right\"></span><span class=\"white_color font_weight font_size_22\">0<span class=\"font_size_16\">item (s)</span> - &#36;  0.00</span></a>";
                    CartCount_mobile = "0";
                  //  CartCount_mobile = "";

                }


            }
            else
            {

                Cartdetail = "";
                CartCount = "<a href=\"\" class=\"dropdown-toggle cart_drop\" data-toggle=\"dropdown\"><span ><img src=\"/images/MicroSiteimages/cart.png\" class=\"margin_left margin_right\"></span><span class=\"white_color font_weight font_size_22\">0<span class=\"font_size_16\">item (s)</span> - &#36;  0.00</span></a>";
                CartCount_mobile = "0";
              
            }

        }


        catch (Exception ex)
        {
          //  objErrorHandler.ErrorMsg = ex;
          //  objErrorHandler.CreateLog();
            Cartdetail = "";
            CartCount = "<a href='' class=''> 0 item(s) - &#36; 0.00  </a>,";
            CartCount_mobile = "0";
            //CartCount_mobile = "";
        }
        return "";
    }


    public string GetLoginName_top()
    {
        string retvalue = string.Empty;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        DataTable objDt = new DataTable();
       string _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
        try
        {
            if (!string.IsNullOrEmpty(userid))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"];
                string iLoginName = string.Empty;
                if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                {
                    objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                    if (objDt != null && objDt.Rows.Count > 0)
                    {
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                        HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                    }

                }
                else
                {
                    objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                    if (objDt != null && objDt.Rows.Count > 0)
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                }
                retvalue = iLoginName;
            }
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            retvalue = "-1";
        }
        finally
        {
            objDt.Dispose();
            objDt = null;
        }
        return retvalue;
    }
    private void loadfooter()
    {
        //FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\home\\footer.st"), FileMode.Open);
        //System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        //string dataString = streamWriter.ReadToEnd();
        //streamWriter.Close();
        //fileStream.Close();
        //string[] str = dataString.Split('$');
        //for (int strc = 1; strc < str.Length; strc = strc + 2)
        //{
        //    Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

        //    footer.Controls.Add(ctl);
        //}

           Control ctl = LoadControl("~/UC/" + "bottom.ascx");

           footer.Controls.Add(ctl);
    }
  
}
