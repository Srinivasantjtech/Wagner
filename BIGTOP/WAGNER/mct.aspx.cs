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
using Antlr3.ST;
namespace WES
{
    public partial class mct : System.Web.UI.Page
    {
        AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        HelperDB objhelperDb = new HelperDB();
        GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
        string MicroSiteTemplate = "";
        public string strsupplier_bannar1 = "";
        public string strsupplier_bannar2 = "";
        public string strsupplier_bannar3 = "";

        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
        string stlistprod = "";
        string stlistprodtitle = string.Empty;
        string sProd = "";
        string stcategory = "";
        string stcategorylisttitle = string.Empty;
        string stcategorylistkey = string.Empty;
        string stitle = string.Empty;
        string skeyword = string.Empty;
        OrderServices objOrderServices = new OrderServices();
        OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnclose.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";
            MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"]);
            Session["PageUrl"] = Request.RawUrl.ToString();
            PopupRetailerLoginMsg.Visible = false;
            PopupOrderMsgNew.Visible = false;
            if (!IsPostBack)
            {
                Session["PageUrl_ms"] = HttpContext.Current.Request.RawUrl.ToString();

                if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                {
                    //string strHostName = System.Net.Dns.GetHostName();
                    //string clientIPAddress = "";
                    //if (System.Net.Dns.GetHostAddresses(strHostName) != null)
                    //{
                    //    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                    //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    //    else
                    //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                    //}

                    DataSet ordds = new DataSet();
                    //int orduseridupdate = 0;
                    int orditemuseridupdate = 0;
                    int newordid = 0;
                    int getnewordid = 0;
                    int order_id = 0;
                    //int sessionflgupdate = 0;
                    int chkcurordid = 0;
                    string userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                    if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                    {
                        order_id = objOrderServices.GetOrderID(Convert.ToInt32(userid));
                        if (order_id > 0)
                        {
                            ordds = objOrderServices.GetOrderItemsAmtDetails(order_id);

                            String sessionId;
                            sessionId = Session.SessionID;
                            if (sessionId != "" && HttpContext.Current.Session["DUMMY_FLAG"].ToString() == "1")
                            {
                                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"].ToString() != "" && HttpContext.Current.Session["DUMMY_FLAG"].Equals("1"))
                                {
                                    Session["DUMMY_FLAG"] = "0";
                                    oOrdInfo.UserID = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);
                                    chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
                                    if (chkcurordid == 0)
                                        newordid = objOrderServices.InitilizeOrder(oOrdInfo);
                                    getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
                                    orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(order_id, Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), sessionId, getnewordid, sessionId);
                                    if (getnewordid > 0)
                                    {
                                        Session["ORDER_ID"] = getnewordid;
                                        oOrdInfo.OrderID = getnewordid;
                                        objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                    }


                                    if (orditemuseridupdate > 0)
                                        Response.Redirect("/mOrderDetails.aspx?ORDER_ID=" + getnewordid + "&bulkorder=1&ViewOrder=View", false);

                                }

                            }
                        }
                 CallRetailerMsg();
                    }
                    else
                    {
                        order_id = objOrderServices.GetOrderID(Convert.ToInt32(userid));
                        if (order_id > 0)
                        {
                            ordds = objOrderServices.GetOrderItemsAmtDetails(order_id);

                            String sessionId;
                            sessionId = Session.SessionID;

                            if (sessionId != "" && HttpContext.Current.Session["DUMMY_FLAG"].ToString() == "1")
                            {
                                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"].ToString() != "" && HttpContext.Current.Session["DUMMY_FLAG"].ToString() == "1")
                                {
                                    Session["DUMMY_FLAG"] = "0";
                                    //Session["PrevOrderID"] = "0";

                                    oOrdInfo.UserID = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);

                                    // chkcurordid = objOrderServices.OrderId_checkcurruserid(Convert.ToInt32(HttpContext.Current.Session["USER_ID"]));
                                    chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);

                                    if (chkcurordid == 0)
                                        newordid = objOrderServices.InitilizeOrder(oOrdInfo);

                                    getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
                                    orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(order_id, Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), sessionId, getnewordid, sessionId);
                                    if (getnewordid > 0)
                                    {
                                        Session["ORDER_ID"] = getnewordid;
                                        oOrdInfo.OrderID = getnewordid;
                                        objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                    }


                                    if (orditemuseridupdate > 0)
                                        Response.Redirect("/mOrderDetails.aspx?ORDER_ID=" + getnewordid + "&bulkorder=1&ViewOrder=View", false);

                                }

                            }


                        }

                        CallOrderMsg();
                    }
                }
                else
                {

                    CallOrderMsg();

                }

            }
        }
        private void CallOrderMsg()
        {

          //  this.ModalPanel1.Visible = false;
            this.modalPop.Hide();
            modalPop = new AjaxControlToolkit.ModalPopupExtender();
            if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
            {
          //ModalPanel1.Visible = true;
          //      modalPop.ID = "popUp";
          //      modalPop.PopupControlID = "ModalPanel";
          //     // modalPop.BackgroundCssClass = "modalBackground";
          //      modalPop.BackgroundCssClass = "modal fade bs-example-modal-lg in bagrndinline";
          //      modalPop.DropShadow = false;
          //      modalPop.TargetControlID = "btnHiddenTestPopupExtender";
          //    this.ModalPanel1.Controls.Add(modalPop);
          //      this.modalPop.Show();

                PopupOrderMsgNew.Visible = true;
            }
            else
            {
                PopupOrderMsgNew.Visible = false;
                //this.modalPop.Hide();
            }
        }
        protected void btnContinueOrder_Click(object sender, EventArgs e)
        {
            try
            {
                //objErrorHandler.CreateLog("inside btncondi");

                if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
                {
                    this.modalPop.Hide();
                    Session["ORDER_ID"] = Session["PrevOrderID"];
                    Session["PrevOrderID"] = "0";
                    Response.Redirect("/mOrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View", false);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignore it
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
        protected void btnClearOrder_Click(object sender, EventArgs e)
        {
            try
            {
                this.modalPop.Hide();
                PopupOrderMsgNew.Visible = false;
                if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
                {
                    OrderServices objOrderServices = new OrderServices();
                    objOrderServices.RemoveItem("AllProd", Convert.ToInt32(Session["PrevOrderID"]), Convert.ToInt32(Session["USER_ID"]), "");
                    objOrderServices.RemoveOrder(Convert.ToInt32(Session["PrevOrderID"]), Convert.ToInt32(Session["USER_ID"]));

                    Session["PrevOrderID"] = "0";
                    Session["ORDER_ID"] = "0";
                }
                Response.Redirect(Session["PageUrl_ms"].ToString(),false);
               
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }

        }

        //[System.Web.Services.WebMethod]
        //public static string ClearOrder()
        //{
        //    try
        //    {
        //        this.modalPop.Hide();
        //        PopupOrderMsgNew.Visible = false;
        //        if (HttpContext.Current.Session["PrevOrderID"] != null && Convert.ToInt32(HttpContext.Current.Session["PrevOrderID"]) > 0)
        //        {
        //            OrderServices objOrderServices = new OrderServices();
        //            objOrderServices.RemoveItem("AllProd", Convert.ToInt32(HttpContext.Current.Session["PrevOrderID"]), Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), "");
        //            objOrderServices.RemoveOrder(Convert.ToInt32(HttpContext.Current.Session["PrevOrderID"]), Convert.ToInt32(HttpContext.Current.Session["USER_ID"]));

        //            HttpContext.Current.Session["PrevOrderID"] = "0";
        //            HttpContext.Current.Session["ORDER_ID"] = "0";
        //        }
        //        return "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }

        //}
        private void CallRetailerMsg()
        {
            //this.ModalPanel.Visible = false;
            //this.modalPop.Hide();
            modalPop = new AjaxControlToolkit.ModalPopupExtender();

            if (Session["RetailerMsg"] == null)
            {

                PopupRetailerLoginMsg.Visible = true;
                //ModalPanel1.Visible = true;
                //modalPop.ID = "popUp1";
                //modalPop.PopupControlID = "ModalPanel1";
                //modalPop.BackgroundCssClass = "modalBackground";
                //modalPop.DropShadow = false;
                //modalPop.TargetControlID = "btnHiddenTestPopupExtender";
                //this.ModalPanel1.Controls.Add(modalPop);
                //this.modalPop.Show();
            }
            else
            {
                PopupRetailerLoginMsg.Visible = false;
                //this.ModalPanel1.Visible = false;
                //this.modalPop.Hide();
            }
        }
        public void GetSupplierdetail()
        {
            if (strsupplierName == "")
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);
                DataTable ttbl = tbwtMSEngine.GetSupplierDetail();
                if (ttbl != null && ttbl.Rows.Count > 0)
                {
                    strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
                    strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
                    strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();
                    strsupplier_bannar1=System.Configuration.ConfigurationManager.AppSettings["CDN"]+ objHelperServices.GetSupplierOptionValues( strsupplierId,"HOME_BANNER1"); 
                        strsupplier_bannar2=System.Configuration.ConfigurationManager.AppSettings["CDN"] + objHelperServices.GetSupplierOptionValues( strsupplierId,"HOME_BANNER2");
                        strsupplier_bannar3 =System.Configuration.ConfigurationManager.AppSettings["CDN"] +  objHelperServices.GetSupplierOptionValues(strsupplierId, "HOME_BANNER3");
                }
            }



        }
        //public string GetSupplierName()
        //{
        //    GetSupplierdetail();
        //    return strsupplierName;

        //}
        //public string GetSupplierDesc()
        //{

        //    GetSupplierdetail();
        //    return strsupplierDesc;
        //}
        public string ST_NewProduct()
        {
            try 
            {
                TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);

                return tbwtMSEngine.ST_NewProduct_Load();
            }


            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }

     
        private string Get_MetaDescription(string stcategory, string parentcatid)
        {

            string catid = string.Empty;
            try
            {


                DataTable Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, stcategory, parentcatid, "GET_CAT_DETAILS", HelperDB.ReturnType.RTTable);
                if (Sqltb != null)
                {
                    if (Sqltb.Rows[0][1] != null)
                    {
                        string metades = Sqltb.Rows[0][1].ToString();

                        if (metades != "")
                        {
                            metades = objgetmetadata.Replace_SpecialChar(metades);
                            //Page.MetaDescription = metades;
                            Page.MetaDescription = metades;
                        }
                    }
                    if (Sqltb.Rows[0][0] != null)
                    {
                        catid = Sqltb.Rows[0][0].ToString();
                        return catid;
                    }
                }
            }
            catch
            {

            }

            return catid;
        }
   
        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
            try
            {

               // Page.MetaDescription = strsupplierDesc; 
                if (Request.QueryString["tsb"] == null)
                {

                    string urlstring = string.Empty;
                    string pcatid = string.Empty;
                    //Page.Title = "Cellink";
                    if (Session["EA"] != null)
                    {
                        string EA = Session["EA"].ToString();
                        DataSet ds = (DataSet)Session["BreadCrumbDS"];
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {


                            if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "category")
                            {

                                if (i != 0)
                                {

                                    stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                                    pcatid = Get_MetaDescription(stcategory, pcatid);
                                    stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                                    Session["prodlisttitle"] = stcategory;
                                    Session["prodlistname"] = stcategory;
                                    if (stcategorylisttitle == string.Empty)
                                    {
                                        stcategorylisttitle = stcategory;
                                        //h3_2.InnerText = stcategory;
                                    }
                                    else
                                    {

                                        //h3_3.InnerText = stcategory;
                                    }

                                }
                                else
                                {
                                    stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                                    pcatid = Get_MetaDescription(stcategory, "0");
                                    stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                                    //h3_1.InnerText = stcategory;
                                }
                            }

                        }

                        string title_key = objgetmetadata.FetchData(ds);
                        if (title_key != "|")
                        {
                            string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                            stitle = StrValues[0];
                            skeyword = StrValues[1];
                            urlstring = StrValues[2];
                            //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString(); ;
                            // Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString(); 
                            Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString();
                            // Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword);
                            Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword) + " - Wagner Electronics, wagneronline.com.au";



                        }

                    }

                    // Page.MetaDescription = "List of products from Maincategory";

                    if (Session["prodmodel"] != null)
                    {
                        string prodmodel = Session["prodmodel"].ToString();

                        //h3_3.InnerText = objgetmetadata.Replace_SpecialChar(prodmodel);
                    }


                    //if (h3_2.InnerText == "")
                    //{

                    //    h3_2.Visible = false;
                    //}
                    //if (h3_3.InnerText == "")
                    //{

                    //    h3_3.Visible = false;
                    //}
                }
                else
                {

                    string pagetitle = "";

                    if ((DataSet)Session["BreadCrumbDS"] != null)
                    {
                        DataSet ds = (DataSet)Session["BreadCrumbDS"];

                        if (ds.Tables[0].Rows[0]["ItemType"].ToString().ToLower() == "category")
                        {

                            //h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                            pagetitle = objgetmetadata.Replace_SpecialChar(ds.Tables[0].Rows[0]["ItemType"].ToString());
                        }
                        if (ds.Tables[0].Rows[1]["ItemType"].ToString().ToLower() == "brand")
                        {



                            stcategory = ds.Tables[0].Rows[1]["Itemvalue"].ToString();
                            stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                            //h2.InnerText = ds.Tables[0].Rows[1]["Itemvalue"].ToString();
                            // Page.Title = stcategory + " " + pagetitle;
                            Page.Title = stcategory; //+ " " + pagetitle ;
                            //  Page.MetaKeywords = pagetitle + "," + stcategory;
                            // Page.MetaDescription = stcategory + "," + "Cellular Phone Models and Accessories";

                            Page.MetaKeywords = pagetitle + " | " + stcategory + " - Wagner Electronics, wagneronline.com.au";
                        }

                    }
                }

                string pagetitle1 = stitle;
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string TextTitle = textInfo.ToTitleCase(pagetitle1);
                Page.Title = TextTitle + " | " + "Wagner Electronics"; ;
            }
            catch
            { }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {

               // this.modalPop.Hide();
                //PopupRetailerLoginMsg.Visible = false;
                Session["RetailerMsg"] = "true";
                PopupRetailerLoginMsg.Visible = false;
                string[] pageurl = Request.RawUrl.ToString().Split(new string[] { "/mct/" }, StringSplitOptions.None);
                string pageurl1 = pageurl[0] + "/mct/";
                Response.Redirect(pageurl1,false);
              // CallOrderMsg();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
        //[System.Web.Services.WebMethod()]
        //public static string pagewidth(int Strvalue)
        //{

        //    HttpContext.Current.Session["screenwidth"] = Strvalue;
        //    return "";
        //}
    }

  }
