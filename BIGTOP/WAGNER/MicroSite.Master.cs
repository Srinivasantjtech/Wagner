using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

using System.Web.Services;
using System.Web.Configuration;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
namespace WES
{
    public partial class MicroSite : System.Web.UI.MasterPage
    {
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        Security objSecurity = new Security();
        string MicroSiteTemplate = "";
       // public string domainname = "";
        public string strsupplier_logo = "";
        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        public string Cartdetail = "";
        public string CartCount = "";
        public string CartCount_mobile = "";
        public string Cartdetail_mobile = "";
        public string micrositeurl = "";
        public string Checkout_URL = "";
        public string Orderdet_URL = "";
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        public string homemenu, aboutmenu, contactmenu, homemenu_m, aboutmenu_m, contactmenu_m, activemenu_faq, activemenu_cl;
        string currenturl = System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            //domainname = objHelperServices.AddDomainname();  
            string hfclickedattr = Request.Form["hfclickedattr_top"];

            if (hfclickedattr != null && hfclickedattr !="")
            {


                Session["hfclickedattr_top"] = hfclickedattr;
                HttpContext.Current.Session["hfclickedattr_top_temp"] = null;
                string[] url = hfclickedattr.Split(new string[] { "@@" }, StringSplitOptions.None);
                Response.Redirect("/" + url[0].ToLower() + "/mpl/");
            }
            else if (HttpContext.Current.Session["hfclickedattr_top_temp"] != null && Request.Form["hfclickedattr_top"] == null)
            {
                hfclickedattr = Session["hfclickedattr_top_temp"].ToString();
                Session["hfclickedattr_top"] = hfclickedattr;
                HttpContext.Current.Session["hfclickedattr_top_temp"] = null;
                string[] url = hfclickedattr.Split(new string[] { "@@" }, StringSplitOptions.None);

                Response.Redirect("/" + url[0].ToLower() + "/mpl/");
            }
            if ((Request.RawUrl.ToString().ToLower() == "/download.aspx") || (Request.RawUrl.ToString().ToLower() == "/support.aspx") )
            {
                micrositeurl = "/judge-cctv-surveillance/mct/";
                strsupplierId = "SPF-JUDGECCTV";
                Page.Title = "Judge CCTV Surveillance";
                strsupplier_logo =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() +"images/supplier_images/JUDGECCTV-LOGO.png";
            }                                    
      
            //if ((HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower().Contains("MForgotPassWord.aspx") == false)
            //|| (HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower().Contains("MConfirmMessage.aspx") == false))
            //{
                loadleftnav();
            //}
                string urlpathquery = string.Empty;
                urlpathquery = HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower();
                //HtmlLink link = new HtmlLink();
                //link.Href = currenturl + "css/DynamicPageCSS.css";
                //link.Attributes.Add("type", "text/css");
                //link.Attributes.Add("rel", "stylesheet");
                //header.Controls.Add(link);
                //if (urlpathquery.Contains("mct.aspx")                 
                //    || urlpathquery.Contains("mfl.aspx")
                //   || urlpathquery.Contains("mpl.aspx")
                //    || urlpathquery.Contains("mpd.aspx")
                //     || urlpathquery.Contains("mps.aspx")
                  
                //    )
                //{


                    //SeoTag.Href = currenturl.Substring(0, currenturl.Length - 1).Replace("https", "http") + Request.RawUrl.ToString();
                SeoTag.Href = currenturl.Substring(0, currenturl.Length - 1) + Request.RawUrl.ToString();
                //}
         
            MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
            //lblCartItem.Text = cartcount();
            if (Session["SUPPLIER_NAME"] != null)
            {
                strsupplierName = Session["SUPPLIER_NAME"].ToString();
                micrositeurl = objHelperServices.SimpleURL_MS_Str( strsupplierName, "mct.aspx",true) + "/mct/";
            }
             
               // TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("MetaDesc", MicroSiteTemplate, objConnectionDB.ConnectionString);
            DataSet tmpds=new DataSet(); 
            //if (HttpContext.Current.Session["MainCategory"] != null)
            if (HttpContext.Current.Application["key_MainCategory"] != null)
                    {
                        tmpds = (DataSet)HttpContext.Current.Application["key_MainCategory"];
                    }
                    //else
                    //{
                    //    string strxml = HttpContext.Current.Server.MapPath("xml");

                    //   // tmpds.ReadXml(strxml + "\\" + "mainds.xml");
                    //   // JObject o1 = JObject.Parse(File.ReadAllText(strxml + "\\" +"mainds.txt"));
                    //   // tmpds = JsonConvert.DeserializeObject<DataSet>(o1.ToString());

                    //    using (StreamReader mainds = File.OpenText(strxml + "\\" + "mainds.txt"))
                    //    {
                    //        using (JsonReader reader = new JsonTextReader(mainds))
                    //        {
                    //            JsonSerializer serializer = new JsonSerializer();
                    //            tmpds = (DataSet)serializer.Deserialize(mainds, typeof(DataSet));
                    //            reader.Close();
                    //        }
                    //        mainds.Dispose();
                    //    }
                    //}
            if (!IsPostBack)
            {
                string querystring = Request.RawUrl.ToString().ToLower();
                if ((querystring.Contains("/mct/")))
                {
                   // querystring = objHelperServices.URlStringReverse_MS(querystring);
                    string[] ConsURL = querystring.Split('/');
                 
                      
                        DataTable dt = tmpds.Tables[0];
                        DataRow[] foundRows;

                        foundRows = dt.Select("category_name = '" + ConsURL[1].ToLower().Replace("~..~", " / ").Replace("~..", " /").Replace("..~", "/ ").Replace(".~.", "/") + "' ");
                        if (foundRows.Length > 0)
                        {
                            strsupplierDesc = foundRows[0]["SHORT_DESC"].ToString();

                        }
                        Page.MetaDescription = strsupplierDesc;
                    }
                }
            }

     
            //if (!Page.IsPostBack)
            //{
            //    Session["screenwidth"] = hfwidth.Value;
            //}
           // lblcompanyname.Text = "Wagner Online Store for Nokia";
           // Control ctl = LoadControl("~/UC/" + "maincategory" + ".ascx");
      
      
        public void GetSupplierdetail()
        {
          //  if (strsupplierName == "")
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);
                DataTable ttbl = tbwtMSEngine.GetSupplierDetail();
                if (ttbl != null && ttbl.Rows.Count > 0)
                {
                    strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
                    strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
                    Page.MetaDescription = strsupplierDesc;
                    strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();
                    micrositeurl =  objHelperServices.SimpleURL_MS_Str( strsupplierName, "mct.aspx",true) + "/mct/";
                    strsupplier_logo =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+ objHelperServices.GetSupplierOptionValues(strsupplierId, "LOGO");
                }
            }



        }
        private void loadleftnav()
        {
           // if (HttpContext.Current.Request.Url.PathAndQuery.ToString().ToLower().Contains("mcontactus.aspx") == false)
           // {
                Control ctl = LoadControl("~/UC/maincategory.ascx");
                //Control ctl = LoadControl("~/UC/MStop.ascx");
                leftnav.Controls.Add(ctl);
          //  }
        }
        public string GetSupplierName()
        {
            GetSupplierdetail();


            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strsupplierName);
        }
        private void loadmaincontent()
        {
          //  Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
           // maincontent.Controls.Add(ctl);
        }
        public string ST_bottom()
        {
            try
            {
                HelperServices objHelperServices = new HelperServices();
                ConnectionDB objConnectionDB = new ConnectionDB();

                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("BOTTOM", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_Bottom_Load();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        public string ST_top()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("TOP", MicroSiteTemplate, objConnectionDB.ConnectionString);
                tbwtMSEngine.cartitem = cartcount();
                //tbwtEngine.RenderHTML("Row");
                //return (tbwtEngine.RenderedHTML);
                return tbwtMSEngine.ST_Top_Load();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        public string ST_MainMenu()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("MAINMENU", MicroSiteTemplate, objConnectionDB.ConnectionString);
             
                return tbwtMSEngine.ST_MainMenu();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        public string ST_Botton_CategoryMenu()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("BottomCatMenu", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_Bootom_Cat_menu();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        public string ST_TOP_CategoryMenu()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("TopMenu", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_TopMenu();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        public string ST_TOP_CategoryMenuMobile()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("TopMenuMobile", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_TopMenuMobile();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        public string ST_NewProductNav()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NewProductNav", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_NewProductLogNav_Load();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }

        public string ST_Header()
        {
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("HEADER", MicroSiteTemplate, objConnectionDB.ConnectionString);
               // tbwtMSEngine.cartitem = cartcount();
                //tbwtEngine.RenderHTML("Row");
                //return (tbwtEngine.RenderedHTML);
                return tbwtMSEngine.ST_Header_Load();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        public string ST_Top_Cart_item()
        {
            string RtnData;
            try
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("CartItems", MicroSiteTemplate, objConnectionDB.ConnectionString);
               // tbwtMSEngine.cartitem = cartcount();
                //tbwtEngine.RenderHTML("Row");
                //return (tbwtEngine.RenderedHTML);
                //string formname = Request.Url.Segments[1].ToString();
                RtnData = tbwtMSEngine.ST_Top_Cart_item();
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
                        CartCount_mobile = "";
  
                    }


                }
                else
                {

                    Cartdetail = "";
                    CartCount = "<a href=\"\" class=\"dropdown-toggle cart_drop\" data-toggle=\"dropdown\"><span ><img src=\"/images/MicroSiteimages/cart.png\" class=\"margin_left margin_right\"></span><span class=\"white_color font_weight font_size_22\">0<span class=\"font_size_16\">item (s)</span> - &#36;  0.00</span></a>";
                    CartCount_mobile = "0";
                    CartCount_mobile = "";
                }

            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                Cartdetail = "";
                CartCount= "<a href='' class=''> 0 item(s) - &#36; 0.00  </a>,";
                CartCount_mobile = "0";
                CartCount_mobile = "";
            }
            return "";
        }

        public string GetCheckoutURL()
        {
            string _Tbt_Order_Id = "";

           

            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            if (userid == string.Empty)
            {
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
            }   

            OrderServices objOrderServices = new OrderServices();
            int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
            if (HttpContext.Current.Session["ORDER_ID"] != null &&  Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
            {
                _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
            }
            else 
            {
                _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(userid), OpenOrdStatusID).ToString();
                //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
            }

            if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) )
            {
                Orderdet_URL = "/morderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;
                Checkout_URL = "/Express_Checkout.aspx?" + EncryptSP(_Tbt_Order_Id.ToString());
            }
            else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
            {
                Orderdet_URL = "/morderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;
                Checkout_URL = "/Express_Checkout.aspx?" + EncryptSP(_Tbt_Order_Id.ToString());
            }
            else
            {
                Orderdet_URL = "#";
                Checkout_URL = "#";

            }

            return "";
        }

        protected string EncryptSP(string ordid)
        {
            string enc = "";
            enc = objSecurity.StringEnCrypt(ordid, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            return HttpUtility.UrlEncode(enc);
        }
        public string cartcount()
        {
            try
            {
                HelperServices objHelperServices = new HelperServices();
                ErrorHandler oErr = new ErrorHandler();
                OrderServices objOrderServices = new OrderServices();
                string cartitem = "0";
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                if (Session["USER_ID"] != null && !Session["USER_ID"].ToString().Equals(""))
                {

                    int OrderID = 0;

                    if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
                    {
                        OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                    }
                    else
                    {
                        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
                    }

                    string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
                    if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
                    {
                        if (objOrderServices.GetOrderItemCount(OrderID) == 0)
                            cartitem = "0";
                        else
                            cartitem = objOrderServices.GetOrderItemCount(OrderID).ToString();

                    }
                    else
                    {
                        cartitem = "0";
                    }
                }
                return cartitem;
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }


        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
            Page.MetaDescription = strsupplierDesc;  
        }
    } 
}
