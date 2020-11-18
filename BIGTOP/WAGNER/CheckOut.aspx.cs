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
using TradingBell.WebCat;
using System.Data.SqlClient;
using System.Text;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.TemplateRender;
using System.Net.Mail;
using System.Web.Services;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using Braintree;

public partial class CheckOut : System.Web.UI.Page
{
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender modalPop_loreg = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender modalpop_itemerror = new AjaxControlToolkit.ModalPopupExtender();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();
    QuoteServices objQuoteServices = new QuoteServices();
    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
    UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
    UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
    UserServices.UserStatus userstatusinfo = new UserServices.UserStatus();
    UserServices objUserServices = new UserServices();
    CountryServices objCountryServices = new CountryServices();
    CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
    Security objSecurity = new Security();
    //ConnectionDB objConnectionDB = new ConnectionDB();
    int QuoteStatusID = (int)QuoteServices.QuoteStatus.OPEN;
    
    public string FIXED_TAX = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX"].ToString();
    public string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
    public string DefaultPay = System.Configuration.ConfigurationManager.AppSettings["DEFAULTPAY"].ToString();
    const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
    OrderServices.OrderInfo oOrdInfo1 = new OrderServices.OrderInfo();
    string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
    public int OrderID = 0;
    public int OrderIDaspx = 0;
    int QuoteID = 0;
    public int PaymentID = 0;
    public bool  Paytab = false;
    public string PaytabType = "";
    public bool paidtab = false;
    int Userid;
    bool IsZipCodeChange = false;
    ListItem oLstItem = new ListItem();
    PaymentServices objPaymentServices = new PaymentServices();
    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
    NotificationServices objNotificationServices = new NotificationServices();
    // ConnectionDB oCon = new ConnectionDB();
    string refid = "";
    // UserServices objUserServices = new UserServices();
    double SubTotal = 0.0;
    String UserList = "";

    int UsrStatus = (int)UserServices.UserStatus.ACTIVE;
    PayPalService objPayPalService = new PayPalService();
    PayPalAPIService objPayPalApiService = new PayPalAPIService();
    SecurePayService objSecurePayService = new SecurePayService();
    string key = "";
    public double TotAmt = 0.0;
    public string ClientToken = string.Empty;

    protected void GenerateClientToken()
    {
        //var gateway = new BraintreeGateway
        //{
        //    Environment = Braintree.Environment.SANDBOX,
        //    MerchantId = "mjff7p7mgb4qmp77",
        //    PublicKey = "p78kxf6s7zhb8z8x",
        //    PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
        //};
        var gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.PRODUCTION,
            MerchantId = "wrv3fq8x3r269ycd",
            PublicKey = "nm7v4wm8dmw7b6rq",
            PrivateKey = "a3d333f589d80552db255c34c1407c40"
        };

        this.ClientToken = gateway.ClientToken.Generate();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      //  btnnewsecurepay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/3d_Secure-Check.png";
      //  btnnewpaypal.ImageUrl = "images/paypal_uncheck.png";

        btnclose1.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";
        ImageButton3.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";
      
        int DuplicateItem_Prod_idCount = 0;
        string LeaveDuplicateProds = GetLeaveDuplicateProducts();
        String sessionId;
        sessionId = Session.SessionID;
        PopupMsg.Visible = false;
        CancelOrderPopup.Style.Add("display", "none");
 
        int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;
        //objErrorHandler.CreateLog("Session_USER_ID" + Session["USER_ID"] + " Userid " + Userid);
        string orderidparam = string.Empty;
        //objErrorHandler.CreateLog(Request.Url.Query);
        //objErrorHandler.CreateLog("url contains CancelOrder " + Request.Url.Query.Contains("CancelOrder"));
        //if (Request.Url.Query.Contains("key=") == false && Request.Url.Query.Contains("CancelOrder") == true)
        //{

        //    orderidparam = Request.Url.Query.Replace("?", "");
        //    orderidparam = orderidparam.Replace("&" + "CancelOrder=true", "");
        //    OrderID = Convert.ToInt32(orderidparam);
        //    //CancelOrderPopup.Visible = true;
        //    CancelOrderPopup.Style.Add("display", "block");
        //}

            if (Session["USER_ID"] != null &&  Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
            {
                Userid = Convert.ToInt32(Session["USER_ID"].ToString());
            }
            else
            {
                objErrorHandler.CreateLog("Userid12" + Userid);
                objErrorHandler.CreateLog("OriginalString url " + Request.Url.OriginalString);
                objErrorHandler.CreateLog("url " + Request.Url.Query);
           int i=  GetParams_orderid("0");
           objErrorHandler.CreateLog("i " + i);
           if (i == 0)
           {
               Session["PageUrl"] = Request.Url.ToString();
               Response.Redirect("/Login.aspx");
               divTimeout.Visible = true;
               divCC.Visible = false;
               return;
           }
            }
         //objErrorHandler.CreateLog(Request.RawUrl.ToString().ToLower() + " Rawurlchecked PaySP");
         objErrorHandler.CreateLog("OriginalString " + Request.Url.OriginalString);
         objErrorHandler.CreateLog("Userid " + Userid);
       

        //For International customer only
        //if(Request.Url.ToString().ToLower().Contains("paysp"))
        //{
       
             if (OrderID <= 0)
             {
                 OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
             }
             else
             {
                 Session["ORDER_ID"] = OrderID;
                 //OrderID = System.Convert.ToInt32(Request["OrderID"]);
             }
      
        //if (objOrderServices.IsNativeCountry(OrderID) == 0)
        //{
        // GetParams_orderid(Userid.ToString());
        //if (valid == 0)
        //{

        //    divonlinesubmitordererror.Visible = true;
        //   PopupOrderMsg.Visible = false;
        //    this.Title = "WAGNER Alert";
        //    return;
        //}
        //else
        //{
        //    divonlinesubmitordererror.Visible = false;

        //}
        //}
        GetParams();
     if (OrderID <= 0)
     {
         OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
     }
     else
     {
         Session["ORDER_ID"] = OrderID;
         //OrderID = System.Convert.ToInt32(Request["OrderID"]);
     }


        Session["Pay_ORDER_ID"] = OrderID;
        //if (valid == 0)
        //{
        //    divonlinesubmitordererror.Visible = true;
        //    PopupOrderMsg.Visible = false;
        //    this.Title = "WAGNER Alert";
        //    return;
        //}
        //else
        //{
        //    divonlinesubmitordererror.Visible = false;

        //}




        //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0))
        //{
        //    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
        //}
        //else
        //{
        //    OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);

        //}
        
        string orderdstatus = objOrderServices.GetOrderStatus(OrderID);
        oOrdInfo = objOrderServices.GetOrder(OrderID);
        if (paidtab == false && objOrderServices.IsNativeCountry(OrderID) == 1 && oOrdInfo.isppppp == 0)
        {

            if (orderdstatus != Enum.GetName(typeof(OrderServices.OrderStatus), tmpOrdStatus))
            {
                Session["ORDER_ID"] = 0;
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY_ORDER", true);
                return;
            }
        }
        else if (orderdstatus == Enum.GetName(typeof(OrderServices.OrderStatus), (int)OrderServices.OrderStatus.DELETED))
        {    
            Session["ORDER_ID"] = 0;
            Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            return;
          
        }
        else if (orderdstatus == Enum.GetName(typeof(OrderServices.OrderStatus), (int)OrderServices.OrderStatus.Proforma_Payment_Success))
        {
            Session["ORDER_ID"] = 0;
            Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY_ORDER", true);
            return;

        }
        DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds,"");
        DataTable tbErrorItems = new DataTable();
       
        //if ( Userid != Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
               // tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "","");
       // else
            tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_ERROR", sessionId);

        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
        {
            DuplicateItem_Prod_idCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
        }

        //string ErrItems = "", ClrItems = "", ClrQty = "";
        //if (Session["ITEM_ERROR"] != null || Session["ITEM_CHK"] != null)
        //{
        //    if (!string.IsNullOrEmpty(Session["ITEM_ERROR"].ToString()))
        //        ErrItems = Session["ITEM_ERROR"].ToString().Trim().Replace(",", "");

        //    if (!string.IsNullOrEmpty(Session["ITEM_CHK"].ToString()))
        //        ClrItems = Session["ITEM_CHK"].ToString().Trim().Replace(",", "");

        //    if (!string.IsNullOrEmpty(Session["QTY_CHK"].ToString()))
        //        ClrQty = Session["QTY_CHK"].ToString().Trim().Replace(",", "");

        //}

        DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK", "");
        DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE", sessionId);
       //Modified by indu
        //this.ModalPanel.Visible = false;
        //this.modalPop.Hide();
        PopupOrderMsg.Visible = false;
        this.HiddenpopuploginregPanel.Visible = false;
        this.modalPop_loreg.Hide();

       // this.Panelerroritems.Visible = false;
        this.modalpop_itemerror.Hide();


        if (tbErrorItems != null && Session["USER_NAME"] == "" && tbErrorItems.Rows.Count > 0 || tbErrorChk != null && tbErrorChk.Rows.Count > 0 || tbErrorReplace != null && tbErrorReplace.Rows.Count > 0)
        {
            // var page = (Page)HttpContext.Current.Handler;
            // page.Title = "WAGNER Alert";
            // this.Panelerroritems.Visible = true;
            // modalpop_itemerror.ID = "popUp";
            // modalpop_itemerror.PopupControlID = "Panelerroritems";
            // modalpop_itemerror.BackgroundCssClass = "modalBackground";
            // modalpop_itemerror.TargetControlID = "btnitemerrors";
            // modalpop_itemerror.DropShadow = true;
            //// modalpop_itemerror.CancelControlID = "btnCancelerroritems";
            // this.Panelerroritems.Controls.Add(modalpop_itemerror);
            // this.modalpop_itemerror.Show();
            // return;


            PopupMsg.Visible = true;
        }
        else
        {
            PopupMsg.Visible = false;
        }

        objErrorHandler.CreateLog("user_name " + Session["USER_NAME"]);
        if (Session["USER_NAME"] == null || Session["USER_NAME"] == "")
        {
            objErrorHandler.CreateLog("proceed to login page");
            //var page = (Page)HttpContext.Current.Handler;
            //page.Title = "WAGNER Alert";
            //this.HiddenpopuploginregPanel.Visible = true;
            //modalPop_loreg.ID = "popUp";
            //modalPop_loreg.PopupControlID = "HiddenpopuploginregPanel";
            //modalPop_loreg.BackgroundCssClass = "modalBackgroundshi";
            //modalPop_loreg.DropShadow = false;
            //modalPop_loreg.TargetControlID = "btnHiddenpopuploginreg";
            //this.HiddenpopuploginregPanel.Controls.Add(modalPop_loreg);
            //this.modalPop_loreg.Show();
            //return;
            Response.Redirect("/Login.aspx");
        }
        string custtype = Session["CUSTOMER_TYPE"].ToString();
        if (Convert.ToInt16(Session["USER_ROLE"]) == 4 && custtype == "Dealer")
        {
            Response.Redirect("/Home.aspx");
        }
        //comment
        //this.ModalPanel.Visible = false;
        //this.modalPop.Hide();
        if (Convert.ToInt16(Session["USER_ROLE"]) == 4 && custtype == "Retailer")
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "WESAlert", "alert('Account Activation Required before you can proceed to check out.Please check your email for account activation link as this was emailed to you when you registered your account.');", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "Redirect", "toRedirect()", true);

            // Response.Redirect("/Home.aspx");
            //Type cstype = this.GetType();
            //ClientScriptManager cs = this.ClientScript;
            //if (!cs.IsStartupScriptRegistered(cstype, "Script"))
            //{
            //    String csScriptText = "if(confirm('Account Activation Required before you can proceed to check out.Please check your email for account activation link as this was emailed to you when you registered your account.')) {document.location='/Home.aspx'}";
            //    cs.RegisterStartupScript(cstype, "TestScript", csScriptText, true);

            //}
            if (!IsPostBack)
            {
                var page = (Page)HttpContext.Current.Handler;
                page.Title = "Bigtop";
                //modified by indu
                PopupOrderMsg.Visible = true;
                //this.ModalPanel.Visible = true;
                //modalPop.ID = "popUp";
                //modalPop.PopupControlID = "ModalPanel";
                //modalPop.BackgroundCssClass = "modalBackgroundshi";
                //modalPop.DropShadow = false;
                //modalPop.TargetControlID = "btnHiddenTestPopupExtender";
                //this.ModalPanel.Controls.Add(modalPop);
                //this.modalPop.Show();
                return;
            }

        }

       // if (!(Session["PageUrl"].ToString().Contains("Confirm.aspx")))
      //{
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString().ToUpper() == "YES")
            {
                //if (!IsPostBack && (Session["ITEM_ERROR"] == null || Session["ITEM_ERROR"].ToString().Trim() == "" || Session["ITEM_ERROR"].ToString().Replace(",", "") == ""))
                //if (!IsPostBack && ErrItems == "" && ClrItems == "" && DuplicateItem_Prod_idCount == 0)
                if (!IsPostBack && (tbErrorItems == null && DuplicateItem_Prod_idCount == 0))
                {
                    try
                    {
                        if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
                        {
                            int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                            Userid = objHelperServices.CI(Session["USER_ID"]);
                            GetParams();
                            if (OrderID<=0)
                            {
                                OrderID = objOrderServices.GetOrderID(Userid, OrdStatus);
                            }
                            else
                            {
                                if (OrderID == 0)
                                {
                                   
                                    OrderID = objOrderServices.GetOrderID(Userid, OrdStatus);
                                    Session["ORDER_ID"] = OrderID;
                                }
                                else
                                {
                                    Session["ORDER_ID"] = OrderID;
                                    //OrderID = System.Convert.ToInt32(Request["OrderID"]);
                                }
                            }

                            if (Request["QteId"] != null)
                                QuoteID = objHelperServices.CI(Request["QteId"].ToString());
                            else
                                QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);

                            if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                                lblShoppingCart.Text = "Quote Cart";
                      

                           
                              
                            //   if (OrderID == 0)
                            //  OrderID = objOrderServices.GetOrderIDForQuote(QuoteID);
                            //OrderID = objOrderServices.GetOrderIDForQuote(QuoteID);
                            decimal OrdtotCost = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));
                            decimal QtetotCost = objHelperServices.CDEC(objQuoteServices.GetCurrentProductTotalCost(QuoteID));
                          
                            //   if ((OrdtotCost > 0) || (QtetotCost>0))

                            // if (((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0) || (QuoteID != 0 && objQuoteServices.GetQuoteItemCount(QuoteID) > 0)))

                            if ((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0 && OrdtotCost > 0) || Request["QteFlag"] == "1")
                            {
                                int a = 5;
                                /////////New requirement//////////
                                //tt1.Text = OrderID.ToString();
                                // ttOrder.Enabled = false;
                                string txtadd = "";
                                LoadCountryList();
                                Ta2.Value = LoadShippingInfostr(Session["USER_ID"].ToString());
                                Ta3.Value = LoadBillInfostr(Session["USER_ID"].ToString());
                                Ta4.Value = LoadBillInfostr(Session["USER_ID"].ToString());
                                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                oOrderInfo = objOrderServices.GetOrder(OrderID);
                                txtpaybill.Text = BuildBillAddress(oOrderInfo);
                                txtpayship.Text = BuildShippAddress(oOrderInfo);
                                txtPaidbill.Text = txtpaybill.Text;
                                txtPaidship.Text = txtpayship.Text;

                                /////////New requirement//////////                            
                                LoadShippingInfo(Session["USER_ID"].ToString());
                                LoadBillInfo(Session["USER_ID"].ToString());

                                tbNoItems.Visible = false;
                                ShippingLink.Visible = false;
                                lblRequired.Visible = true;
                                LblStar.Visible = true;
                                ChkShippingAdd.Visible = true;
                                ChkShipDefaultaddr.Visible = true;
                                ChkbillingAdd.Visible = true;
                                ChkBillDefaultaddr.Visible = true;

                                //  LoadStates("US");
                                if (Convert.ToInt16(Session["USER_ROLE"]) == 3)
                                {
                                    Load_UserRole(Session["USER_ID"].ToString());
                                }
                            }
                            // }
                            else
                            {
                                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY",false);
                                btnShipProceed.Enabled = false;
                                tbNoItems.Visible = true;
                                ShippingLink.Visible = true;
                                lblRequired.Visible = false;
                                LblStar.Visible = false;
                                ChkShippingAdd.Visible = false;
                                ChkShipDefaultaddr.Visible = false;
                                ChkbillingAdd.Visible = false;
                                ChkBillDefaultaddr.Visible = false;
                                txtbilladd1.Enabled = false;
                                txtbilladd2.Enabled = false;
                                txtbilladd3.Enabled = false;
                                txtbillcity.Enabled = false;
                                txtbillFName.Enabled = false;
                                txtbillLName.Enabled = false;
                                txtbillphone.Enabled = false;
                                txtbillzip.Enabled = false;
                                txtSAdd1.Enabled = false;
                                txtSAdd2.Enabled = false;
                                txtSAdd3.Enabled = false;
                                txtSCity.Enabled = false;
                                txtSFName.Enabled = false;
                                txtSLName.Enabled = false;
                                txtSPhone.Enabled = false;
                                txtSZip.Enabled = false;
                                cmbShipMethod.Enabled = false;
                                cmbProvider.Enabled = false;
                                drpBillCountry.Enabled = false;
                                drpBillState.Enabled = false;
                                drpShipCountry.Enabled = false;
                                drpShipState.Enabled = false;
                            }
                        }

                        //HtmlMeta meta = new HtmlMeta();
                        //meta.Name = "keywords";
                        //meta.Content = objHelperServices.GetOptionValues("Meta keyword").ToString();
                        //this.Header.Controls.Add(meta);

                        //// Render: <meta name="Description" content="noindex" />
                        //meta = new HtmlMeta();
                        //meta.Name = "Description";
                        //meta.Content = objHelperServices.GetOptionValues("Meta Description").ToString();
                        //this.Header.Controls.Add(meta);

                        //// Render: <meta name="Abstraction" content="Some words listed here" />
                        //meta.Name = "Abstraction";
                        //meta.Content = objHelperServices.GetOptionValues("Meta Abstraction").ToString();
                        //this.Header.Controls.Add(meta);

                        //// Render: <meta name="Distribution" content="noindex" />
                        //meta = new HtmlMeta();
                        //meta.Name = "Distribution";
                        //meta.Content = objHelperServices.GetOptionValues("Meta Distribution").ToString();
                        //this.Header.Controls.Add(meta);
                      
                    }
                    catch (System.Threading.ThreadAbortException)
                    {
                        // ignore it
                    }
                    catch (Exception Ex)
                    {
                        btnShipProceed.Enabled = false;
                        tbNoItems.Visible = true;
                        ShippingLink.Visible = true;
                        lblRequired.Visible = false;
                        LblStar.Visible = false;
                        ChkShippingAdd.Visible = false;
                        ChkShipDefaultaddr.Visible = false;
                        ChkbillingAdd.Visible = false;
                        ChkBillDefaultaddr.Visible = false;
                        objErrorHandler.ErrorMsg = Ex;
                        objErrorHandler.CreateLog();
                    }
                }
                else
                {
                    //if (Session["ITEM_ERROR"] != null)
                    // {
                    //   if (Session["ITEM_ERROR"].ToString().Replace(",", "") != "")
                    //  {
                    //if (ErrItems != "" || ClrItems != "" || DuplicateItem_Prod_idCount > 0)
                    if ((tbErrorItems != null && tbErrorItems.Rows.Count > 0) || DuplicateItem_Prod_idCount > 0)
                    {
                        Session["ShowPop"] = "Yes";

                        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                        {
                            Response.Redirect("orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + Session["ORDER_ID"], true);
                        }
                        else
                        {
                            Response.Redirect("orderDetails.aspx?bulkorder=1&amp;Pid=0", true);
                        }
                    }
                    //}
                    // }


                }
            }
            else
            {
                Response.Redirect("ConfirmMessage.aspx?Result=NOECOMMERCE", false);
            }
            if (Convert.ToInt16(Session["USER_ROLE"]) == 3)
            {
                Load_UserRole(Session["USER_ID"].ToString());
            }
            //For Using Enter Key
            txtSFName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtSLName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtSAdd1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtSAdd2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtSAdd3.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtSCity.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtSPhone.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtSZip.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbillFName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbillLName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbilladd1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbilladd2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbilladd3.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbillcity.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbillphone.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            txtbillzip.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
            ttOrder.Attributes.Add("onchange", "return isAlphabetic();");
            ttOrder.Attributes.Add("onblur", "return isAlphabetic();");

            txtCardNumber.Attributes.Add("onkeypress", "javascript:return Numbersonlypay(event);");
            txtCardCVVNumber.Attributes.Add("onkeypress", "javascript:return Numbersonlypay(event);");
            if (!IsPostBack)
            {
                drpExpyear.Items.Clear(); 
                for (int y = DateTime.Now.Year; y <= DateTime.Now.Year + 20; y++)
                {
                    drpExpyear.Items.Add(y.ToString());
                }
            }
        //}
        //else
        //{
        //    Response.Redirect("/Home.aspx", false);
        //}



        if (objOrderServices.IsUserCanDropShip(objHelperServices.CI(Session["USER_ID"])))  // Drop shipment available as per user role
        {
            drpSM1.Items.Add(new ListItem("Drop Shipment Order", "Drop Shipment Order"));
        }



        if (IsPostBack == false && objOrderServices.IsNativeCountry(OrderID) == 0) // is other then au
        {
            drpSM1.Items.Clear();
            drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));             
            drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
            drpSM1.SelectedIndex = 1; 
            liPayOption.Visible = false;
            liFinalReview.Visible = false;
            ImageButton2.Text = "Submit Order";
        }
        if (IsPostBack == false && oOrdInfo.isppppp == 1)
        {
            liFinalReview.Visible = false;
        }
        if (Request.QueryString["ApproveOrder"] != null && IsPostBack == false)
        {
            GetApproveOrderDetails(OrderID);
        }
        else
        {
            SetSessionVlaue();
        }
        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:HidePanels();", true);

        drpSM1.Attributes.Add("onclick", "javascript:CheckShippment();");
        drpSM1.Attributes.Add("onchange", "javascript:CheckShippment();");
        // ImageButton4.Attributes.Add("onchange", "javascript:DRPshippment();");

        if (Paytab == true)
        {
            PnlOrderInvoice.Visible = true;
            PnlOrderContents.Visible = false;

            liPayOption.Visible =true ;

            div2.Visible = false;
            ttOrder.Enabled = false;
            drpSM1.Enabled = false;
            PHOrderConfirm.Visible = true;
            drpState.Enabled = false;
            txtCompany.Enabled = false;
            txtPostCode.Enabled = false;
            txtAddressLine1.Enabled = false;
            txtAddressLine2.Enabled = false;
            txtAttentionTo.Enabled = false;
            txtCountry.Enabled = false;
            drpShipState.Enabled = false;
            // txtReceiverContactName.Enabled=false;
            txtSuburb.Enabled = false;
            txtDeliveryInstructions.Enabled = false;
            txtShipPhoneNumber.Enabled = false;
            txtDeliveryInstructions.Enabled = false;

            //ImageButton4.Visible = false;
            ImageButton1.Visible = false;
            // ImageButton5.Visible = false;
            //TextBox1.Enabled = false;
            TextBox1.ReadOnly = true;
            ImageButton2.Visible = false;
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            oOrderInfo = objOrderServices.GetOrder(OrderID);
              oPayInfo = objPaymentServices.GetPayment(OrderID);
            ttOrder.Text   =oPayInfo.PORelease ;
            lblAmount.Text = oPayInfo.Amount.ToString();
         //   lblpaypaltotamt.Text = oPayInfo.Amount.ToString();
            TotAmt = Convert.ToDouble(oPayInfo.Amount);
            lblbramount.Text = oPayInfo.Amount.ToString();
            lblpaypaltotamt.Text = oPayInfo.Amount.ToString();
            Setdrpdownlistvalue(drpSM1,oOrderInfo.ShipMethod);
            TextBox1.Text =oOrderInfo.ShipNotes;
            spanPay.Visible = true;

            h3Pay.Visible = true;
            h3Pay1.Visible = false;

            SetTabsetting("ship", false);
            SetTabsetting("Pay", true);
           // PayType.Visible = false;
                Paydiv.Visible = true;
        

            if (IsPostBack)
            {

                string CtrlID = string.Empty;
                if (Request.Form[hidSourceID.UniqueID] != null && Request.Form[hidSourceID.UniqueID] != string.Empty)
                {
                    CtrlID = Request.Form[hidSourceID.UniqueID];
                    if (CtrlID.Contains("btnPay"))
                    {
                        BtnProgress.Visible = true;
                        BtnProgress.Attributes.Add("style", "display:block;");
                        btnPay.Visible = false;
                    }
                    if (CtrlID.Contains("btnSP"))
                    {
                        BtnProgressSP.Visible = true;
                        BtnProgressSP.Attributes.Add("style", "display:block;");
                        btnSP.Visible = false;
                    }
                }


            }
            else
            {

                if (PaytabType == "Pay" || PaytabType == "PayApi")
                {
                    PayType.Visible = false;
                    PaypalApiDiv.Visible = true;
                    btnPay.Visible = true;
                    btnPayApi.Visible = false;
                    PaySPDiv.Visible = false;
                    if ((TotAmt <= 100) && (objOrderServices.IsNativeCountry(OrderID) == 1))
                    {
     PaySPDiv.Visible = true;
}
                  //  PaySPDiv.Visible = true;
                }
                

                else
                {
                    PayType.Visible = true;
                    if ((TotAmt <= 100) && (objOrderServices.IsNativeCountry(OrderID) == 1)) 
                    {
                        PaypalApiDiv.Visible = false;
                        PaySPDiv.Visible = true;
                    }
                    else
                    {
                        PaypalApiDiv.Visible = true;
                        PaySPDiv.Visible = false;
                        ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                    }
                }
                
            }
            
            
                
            
        }
        else
        {
            Paydiv.Visible = false;
            spanPay.Visible = false;
            h3Pay.Visible =false;
            h3Pay1.Visible =true;

            PayType.Visible = false;
            
        }

        if (paidtab == true)
        {
            objErrorHandler.CreatePayLog("paidtab == true start");

            PnlOrderInvoice.Visible = true;
            PnlOrderContents.Visible = false;

            liFinalReview.Visible =true ;
            paiddiv.Visible = true;
            btnPay.Visible = false;
            btnPayApi.Visible = false;
           spanpaid.Visible = true;
            PayType.Visible = false;
            PaypalApiDiv.Visible = false;
        PaySPDiv.Visible = false; 
         //   PaySPDiv.Visible = true; 
            hpaid.Visible = true;
            hpaid1.Visible =false ;
            SetTabsetting("ship", false);
            SetTabsetting("Pay", false);
            SetTabsetting("paid", true);
            ImgBtnEditShipping.Visible = false;
            string output = "";
            try
            {
                objErrorHandler.CreatePayLog("before check isnative OrderID" + OrderID);
                int isnative = objOrderServices.IsNativeCountry(OrderID);
                objErrorHandler.CreatePayLog("after check isnative OrderID" + OrderID);
                if (PaytabType == "Pay" || PaytabType == "PayApi")
                {

                    if (Request["key"] != null)
                    {
                        // key = DecryptSP(Request["key"].ToString());
                    }

                    if (httpRequestVariables()["tx"] != null)
                    {
                        HttpContext.Current.Session["payflowresponse"] = httpRequestVariables();
                        var response = HttpContext.Current.Session["payflowresponse"] as NameValueCollection;
                        if (response["cm"] != null)
                            objPayPalService.SetPayPalStatus(response, response["cm"].ToString());
                        //string rtn = 
                        objErrorHandler.CreatePayLog("setpaypalstatus httpRequestVariables tx not null inner OrderID" + OrderID);
                        //output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(key) +"\";</script>";
                        output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(OrderID + "#####" + "Pay" + "#####" + "Paid") + "\";</script>";
                     //   paiddiv.InnerHtml = output;
                      //  return;
                    }
                    else if (httpRequestVariables()["Token"] != null && httpRequestVariables()["PayerID"] != null)
                    {
                        //  HttpContext.Current.Session["payAPIresponse"] = httpRequestVariables();
                        //   var response = HttpContext.Current.Session["payAPIresponse"] as NameValueCollection;
                        objPayPalApiService.DoECStatus(httpRequestVariables());
                        //if (response["custom"] != null)
                        //    objPayPalService.DoECStatus(response, response["custom"].ToString());
                        //string rtn = 
                        objErrorHandler.CreatePayLog("Token and PayerID not null inner OrderID=" + OrderID);
                        //output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(key) +"\";</script>";
                        output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(OrderID + "#####" + "PayApi" + "#####" + "Paid") + "\";</script>";
                       // paiddiv.InnerHtml = output;
                       // return;
                    }


                    if (IsPostBack)
                        return;







                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

                    objErrorHandler.CreatePayLog("Get Pay and Order info start OrderID=" + OrderID);
                    oPayInfo = objPaymentServices.GetPayment(OrderID);
                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    objErrorHandler.CreatePayLog("Get Pay and Order info end OrderID" + OrderID);
                    lblOrderNo.Text = oPayInfo.PORelease;
                    lblShippingMethod.Text = oOrderInfo.ShipMethod;

                   
                    bool isipn = false;
                    if (HttpContext.Current.Session["payflowresponse"] != null && HttpContext.Current.Session["payflowresponse"].ToString()  != "")
                    {

                        if (HttpContext.Current.Session["payflowresponse"].ToString() == "SUCCESS")
                        {
                            objErrorHandler.CreatePayLog("page load  payflowresponse SUCCESS=" + HttpContext.Current.Session["payflowresponse"] + "OrderID=" + OrderID);
                            // string ordstatus = objOrderServices.GetOrderStatus(OrderID);
                            string[] ipn = null;

                            if (HttpContext.Current.Session["IPN"] != null && HttpContext.Current.Session["IPN"].ToString()  != "")
                            {
                                ipn = HttpContext.Current.Session["IPN"].ToString().Split('#');

                                if (ipn.Length >= 1)
                                {
                                    DataTable dt = objPayPalService.GETIPN(ipn[1].ToString(), ipn[0].ToString());
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        DataRow[] drs = dt.Select("APPROVED='YES'");
                                        if (drs.Length > 0)
                                            isipn = true;
                                    }
                                    objErrorHandler.CreatePayLog("page load  ipn=" + isipn + "OrderID=" + OrderID);
                                }
                            }


                            objErrorHandler.CreatePayLog("before UpdatePaymentOrderStatus Orderid="+OrderID);

                            if (Session["hfisordercompleted_c"] == null)
                            {
                                objOrderServices.UpdatePaymentOrderStatus(OrderID, PaymentID, isipn, oOrderInfo.isppppp);
                            }
                            else if (Session["hfisordercompleted_c"].ToString() == OrderID.ToString())
                            {
                                objErrorHandler.CreateLog("inside hfisordercompleted" + OrderID);
                                Response.Redirect("myaccount.aspx");
                            }
                            else
                            {
                            objOrderServices.UpdatePaymentOrderStatus(OrderID, PaymentID, isipn, oOrderInfo.isppppp);
                            }
                            Session["hfisordercompleted_c"] = OrderID;
                            objErrorHandler.CreatePayLog("after UpdatePaymentOrderStatus Orderid=" + OrderID);

                            objErrorHandler.CreatePayLog("before SendMail_AfterPaymentPP CheckMailLog Orderid=" + OrderID);
                            if (objOrderServices.IsNativeCountry(OrderID) == 1)
                            {
                                DataTable dtcheckmailsend = objOrderServices.CheckMailLog(OrderID);
                                if (dtcheckmailsend == null || dtcheckmailsend.Rows.Count == 0)
                                {
                                    objErrorHandler.CreatePayLog("after SendMail_AfterPaymentPP CheckMailLog Orderid=" + OrderID);
                                
                                    SendMail_AfterPaymentPP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                    tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                }
                            }
                            //else
                            //    SendMail_AfterPayment(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                            //objOrderServices.UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Payment_Successful );
                            objErrorHandler.CreatePayLog("after SendMail_AfterPaymentPP Orderid=" + OrderID);

                            divError.InnerHtml = "";
                            divOk.InnerHtml = "<img src=\"" + System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "\\images/checkout_tick.png\" class=\"approved_icon\"/>" + "Transaction Approved! Your Order will now be processed, thanks for shopping at Bigtop!";// "Transaction approved! Thank you for your order.";
                            PayOkDiv.Visible = true;
                            //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                            divlink.InnerHtml = "Payment Method: Paypal";
                        }
                        else
                        {
                            objErrorHandler.CreatePayLog("Transaction failed payflowresponse! Please try again Orderid=" + OrderID);
                            divOk.InnerHtml = "";
                            divOk.Visible = false;
                            PayOkDiv.Visible = false;
                            divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";
                            if (isnative == 1)
                                divlink.InnerHtml = "<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                            else
                                divlink.InnerHtml = "<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                        }
                        HttpContext.Current.Session["payflowresponse"] = "";
                        HttpContext.Current.Session["IPN"] = "";
                        if (Session["AUTO_LOGIN"] != null)
                        {
                               objErrorHandler.CreatePayLog("paidtab == true end 2 --->   " + Session["AUTO_LOGIN"].ToString());
                            if (paidtab == true && Session["AUTO_LOGIN"].ToString() == "1")
                            {
                                Session.RemoveAll();
                                Session.Clear();
                                Session.Abandon();
                                if (Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "")
                                {
                                    Response.Redirect("/Login.aspx");
                                }
                            }
                        }
                        //output += "<p>(server response follows)</p>\n";
                        //output += print_r(payflowresponse);
                        //divContent.InnerHtml = output;
                        return;
                    }
                    else if (HttpContext.Current.Session["payAPIresponse"] != null && HttpContext.Current.Session["payAPIresponse"].ToString()  != "")
                    {

                        if (HttpContext.Current.Session["payAPIresponse"].ToString() == "SUCCESS")
                        {

                            objErrorHandler.CreatePayLog("before UpdatePaymentOrderStatus APIres Orderid=" + OrderID);
                            if (Session["hfisordercompleted_c"] == null)
                            {
                                objOrderServices.UpdatePaymentOrderStatus(OrderID, PaymentID, isipn, oOrderInfo.isppppp);
                            }
                            else if (Session["hfisordercompleted_c"].ToString() == OrderID.ToString())
                            {
                                objErrorHandler.CreateLog("inside hfisordercompleted" + OrderID);
                                Response.Redirect("myaccount.aspx");
                            }
                            else
                            {
                                objOrderServices.UpdatePaymentOrderStatus(OrderID, PaymentID, isipn, oOrderInfo.isppppp);
                            }
                            objErrorHandler.CreatePayLog("after UpdatePaymentOrderStatus APIres Orderid=" + OrderID);
                            Session["hfisordercompleted_c"] = OrderID;

                            objErrorHandler.CreatePayLog("before SendMail_AfterPaymentPP APIres Orderid=" + OrderID);
                            if (objOrderServices.IsNativeCountry(OrderID) == 1)
                            {
                                   DataTable dtcheckmailsend = objOrderServices.CheckMailLog(OrderID);
                                   if (dtcheckmailsend == null || dtcheckmailsend.Rows.Count == 0)
                                   {
                                       SendMail_AfterPaymentPP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                       TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                       tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                   }
                            }
                            //else
                            //    SendMail_AfterPayment(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                            objErrorHandler.CreatePayLog("after SendMail_AfterPaymentPP APIres Orderid=" + OrderID);
                         
                            PayOkDiv.Visible = true;
                            divError.InnerHtml = "";
                            divOk.InnerHtml = "<img src=\"" + System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "\\images/checkout_tick.png\" class=\"approved_icon\"/>" + "Transaction Approved! Your Order will now be processed, thanks for shopping at Bigtop!";// "Transaction approved! Thank you for your order.";
                            //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                            divlink.InnerHtml = "Payment Method: Paypal Express Checkout";
                        }
                        else
                        {
                            objErrorHandler.CreatePayLog("Transaction failed payAPIresponse! Please try again Orderid=" + OrderID);
                            divOk.InnerHtml = "";
                            divOk.Visible = false;
                            PayOkDiv.Visible = true;
                            divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";
                            if (isnative == 1)
                                divlink.InnerHtml = "<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                            else
                                divlink.InnerHtml = "<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PayApi") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";

                        }
                        HttpContext.Current.Session["payAPIresponse"] = "";
                        HttpContext.Current.Session["IPN"] = "";
                        if (Session["AUTO_LOGIN"] != null)
                        {
                            objErrorHandler.CreatePayLog("paidtab == true end 2 --->   " + Session["AUTO_LOGIN"].ToString());
                            if (paidtab == true && Session["AUTO_LOGIN"].ToString() == "1")
                            {
                                Session.RemoveAll();
                                Session.Clear();
                                Session.Abandon();
                                if (Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "")
                                {
                                    Response.Redirect("/Login.aspx");
                                }
                            }
                        }
                        //output += "<p>(server response follows)</p>\n";
                        //output += print_r(payflowresponse);
                        //divContent.InnerHtml = output;
                        return;
                    }
                    else
                    {
                        Response.Redirect("/Home.aspx");
                    }
                }
                else  /// secure pay
                {
                    if (Request["key"] != null)
                    {
                        key = DecryptSP(Request["key"].ToString());
                    }
                   OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    oPayInfo = objPaymentServices.GetPayment(OrderID);
                    PaymentID = oPayInfo.PaymentID;
                    lblOrderNo.Text = oPayInfo.PORelease;
                    lblShippingMethod.Text = oOrderInfo.ShipMethod;

                    if (HttpContext.Current.Session["paySPresponse"] != null && HttpContext.Current.Session["paySPresponse"].ToString()  != "")
                    {

                        if (HttpContext.Current.Session["paySPresponse"].ToString() == "SUCCESS")
                        {
                            objErrorHandler.CreatePayLog("SP before UpdatePaymentOrderStatus Orderid=" + OrderID);



                            if (Session["hfisordercompleted_c"] == null)
                            {
                                objOrderServices.UpdatePaymentOrderStatus(OrderID, PaymentID, false, oOrderInfo.isppppp);
                            }
                            else if (Session["hfisordercompleted_c"].ToString() == OrderID.ToString())
                            {
                                objErrorHandler.CreateLog("inside hfisordercompleted" + OrderID);
                                Response.Redirect("myaccount.aspx");
                            }
                            else
                            {
                                objOrderServices.UpdatePaymentOrderStatus(OrderID, PaymentID, false, oOrderInfo.isppppp);
                            }
                            Session["hfisordercompleted_c"] = OrderID;
                            objErrorHandler.CreatePayLog("SP after UpdatePaymentOrderStatus Orderid=" + OrderID);
                            //if (objOrderServices.IsNativeCountry(OrderID) == 1)
                            //    SendMail_AfterPayment(OrderID, (int)OrderServices.OrderStatus.Payment_Successful);
                            //else
                            //    SendMail_AfterPayment(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);

                            objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                            SendMail_AfterPaymentSP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                            tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                            objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);

                            PayOkDiv.Visible = true;
                            divError.InnerHtml = "";
                            divOk.InnerHtml = "<img src=\"" + System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "\\images/checkout_tick.png\" class=\"approved_icon\"/>" + "Transaction Approved! Your Order will now be processed, thanks for shopping at Bigtop!";// "Transaction approved! Thank you for your order.";
                            //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                            divlink.InnerHtml = "Payment Method: Credit Card";
                        }
                        else
                        {
                            divOk.InnerHtml = "";
                            divOk.Visible = false;
                            objErrorHandler.CreatePayLog("SP Transaction failed! Please try again Orderid=" + OrderID);
                            divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";
                          

                            divOk.InnerHtml = "";
                            divOk.Visible = false;
                            PayOkDiv.Visible = true;
                            divError.InnerHtml = "Transaction failed! Please try again.<br/>";
                            if (isnative == 1)
                                divlink.InnerHtml = "<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                            else
                                divlink.InnerHtml = "<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PaySP") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";


                            

                        }
                        HttpContext.Current.Session["paySPresponse"] = "";
                        if (Session["AUTO_LOGIN"] != null)
                        {
                            objErrorHandler.CreatePayLog("paidtab == true end 2 --->   " + Session["AUTO_LOGIN"].ToString());
                            if (paidtab == true && Session["AUTO_LOGIN"].ToString() == "1")
                            {
                                Session.RemoveAll();
                                Session.Clear();
                                Session.Abandon();
                                if (Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "")
                                {
                                    Response.Redirect("/Login.aspx");
                                }
                            }
                        }
                        //HttpContext.Current.Session["IPN"] = "";
                        //output += "<p>(server response follows)</p>\n";
                        //output += print_r(payflowresponse);
                        //divContent.InnerHtml = output;
                        return;
                    }
                    else
                    {
                        Response.Redirect("/Home.aspx");
                    }

                }
                if (Session["AUTO_LOGIN"] != null)
                {

                    objErrorHandler.CreatePayLog("paidtab == true end 1");
                    if (paidtab == true && Session["AUTO_LOGIN"].ToString() == "1")
                    {
                        Session.RemoveAll();
                        Session.Clear();
                        Session.Abandon();
                        if (Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "")
                        {
                            Response.Redirect("/Login.aspx");
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
            objErrorHandler.CreatePayLog("paidtab == true end");
            if (Session["AUTO_LOGIN"] != null)
            {
                if (paidtab == true && Session["AUTO_LOGIN"].ToString() == "1")
                {
                    Session.RemoveAll();
                    Session.Clear();
                    Session.Abandon();
                    if (Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "")
                    {
                        Response.Redirect("/Login.aspx");
                    }
                }
            }

        }
        else
        {
            paiddiv.Visible = false;
            spanpaid.Visible = false;
            hpaid1.Visible =true;
            hpaid.Visible =false;
        }

    }
    private void SendMail_AfterPaymentPP(int OrderId, int OrderStatus, bool isau)
    {
        string toemail = "";
        try
        {
            objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner1 OrderId=" + OrderId);

            string BillAdd;
            string ShippAdd;
            string stemplatepath;
            DataSet dsOItem = new DataSet();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

            oPayInfo = objPaymentServices.GetPayment(OrderId);
            oOrderInfo = objOrderServices.GetOrder(OrderId);

            int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

            //oUserInfo = objUserServices.GetUserInfo(UserID);
            oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            dsOItem = objOrderServices.GetOrderItems(OrderId);
            BillAdd = GetBillingAddress(OrderId);
            ShippAdd = GetShippingAddress(OrderId);

            string ShippingMethod = oOrderInfo.ShipMethod;
            string CustomerOrderNo = oPayInfo.PORelease;
            string shippingnotes = oOrderInfo.ShipNotes;


            if (oOrderInfo.CreatedUser != 777)
            {

                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            }
            else
            {
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
            }

         //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
            string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
            string Emailadd = oUserInfo.AlternateEmail;
            toemail = oUserInfo.AlternateEmail; 

            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

            int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
            oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
            string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

            string SubmittedBy = "";
            switch (oOrderInfo.OrderStatus)
            {
                case 6:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
                case 12:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
                default:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
            }

            //string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            string sHTML = "";
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //StringTemplate _stmpl_records1 = null;
                //StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];

                //StringTemplateGroup _stg_container1 = null;
                //StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                TBWDataList1[] lstrows1 = new TBWDataList1[0];

                stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //int ictrows = 0;

                DataSet dscat = new DataSet();
               // DataTable dt = null;
                _stg_records = new StringTemplateGroup("row", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);


                lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];



                int ictrecords = 0;

                foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                {
                    //if (websiteid == 3)
                    //   _stmpl_records = _stg_records.GetInstanceOf("mail-wagner" + "\\" + "row");
                    //   else
                    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");

                    _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                    _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }

                //if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                //if( websiteid == 3)
                //   _stmpl_container = _stg_container.GetInstanceOf("mail-wagner" + "\\" + "OrderSubmitted");
                //    else
                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedAfterPay");
                //else
                //    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");

                _stmpl_container.SetAttribute("PAY_METHOD", "PayPal");
                _stmpl_container.SetAttribute("AMOUNT", oOrderInfo.TotalAmount);
                _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID);
                //_stmpl_container.SetAttribute("CONNOTNO", oOrderInfo.TrackingNo);  
                //_stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                //_stmpl_container.SetAttribute("SHIPPEDBY", oOrderInfo.ShipCompany);
                _stmpl_container.SetAttribute("OrderDate", Createdon);
                _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                _stmpl_container.SetAttribute("CreatedBy", Createdby);
                // if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                // else
                //    _stmpl_container.SetAttribute("SubmittedBy", "");



                _stmpl_container.SetAttribute("ShippingMethod", ShippingMethod);
                _stmpl_container.SetAttribute("BillingAddress", BillAdd);
                _stmpl_container.SetAttribute("ShippingAddress", ShippAdd);
                _stmpl_container.SetAttribute("shippingnotes", shippingnotes);

                if (shippingnotes != "")
                    _stmpl_container.SetAttribute("TBT_shippingnotes", true);
                else
                    _stmpl_container.SetAttribute("TBT_shippingnotes", false);

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner2");
                objErrorHandler.CreatePayLog(sHTML);
            }
            catch (Exception ex)
            {
                objHelperServices.Mail_Error_Log("PP", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]),1);
                objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, "", ex.Message);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
                
            }
            if (sHTML != "")
            {
                //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                //System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

                //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                //MessageObj.To.Add(Emailadd.ToString());
                ////MessageObj.To.Add("jtechalert@gmail.com");
                ////MessageObj.To.Add("mohanarangam.e.r@jtechindia.com");
                //MessageObj.Subject = "Pending Order - WES Australasia";
                //MessageObj.IsBodyHtml = true;
                //MessageObj.Body = sHTML;
                //System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                ////System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                //smtpclient.UseDefaultCredentials = false;
                //smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                ////smtpclient.Port = 587;
                ////smtpclient.Credentials = new System.Net.NetworkCredential("jtechalert@gmail.com", "jtech@#$123");
                //smtpclient.Send(MessageObj);

                //objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                ////ArrayList CCList = new ArrayList();
                ////CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                ////objNotificationServices.NotifyCC = CCList;
                //objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                //objNotificationServices.NotifyTo.Add(Emailadd.ToString());

                // string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                //EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
                //objNotificationServices.NotifySubject = EmailSubject;
                //objNotificationServices.NotifyMessage = sHTML;
                //objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                //objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                //objNotificationServices.NotifyIsHTML = true;
                //objNotificationServices.SendMessage();

                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner3 OrderId=" + OrderId);
                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                string emails = "";
                string Adminemails = "";
                string webadminmail = "";
                webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                {
                    MessageObj.To.Add(Emailadd.ToString());
                    MessageObj.Bcc.Add(webadminmail);
                    //if (isau == false)
                    //{
                    //    if (System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"].ToString() == "9200")
                    //        Adminemails = System.Configuration.ConfigurationManager.AppSettings["ToMail"].ToString();
                    //    else
                    //        Adminemails = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                    //}

                    // Get_ADMIN_UserEmils();
                    //if (ApprovedUserEmailadd.Trim() != "" && Emailadd.ToString() != ApprovedUserEmailadd.ToString())
                    //   MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                }
                else
                {
                    emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                    MessageObj.To.Add(Emailadd.ToString());


                }

                //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                // {
                MessageObj.Subject = "Bigtop Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
                //}
                //else
                //{
                //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                // }

                MessageObj.IsBodyHtml = true;
                MessageObj.Body = sHTML;

                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner4 OrderId=" + OrderId);
                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());


                DataTable dtcheckmailsend = objOrderServices.CheckMailLog_Email(oOrderInfo.OrderID, MessageObj.To.ToString());
                if (dtcheckmailsend == null || dtcheckmailsend.Rows.Count == 0)
                {
                    objErrorHandler.CreatePayLog_Final("Important checkoutpage b4 smtpclientsend-" + oOrderInfo.OrderID + "--" + Emailadd);
                smtpclient.Send(MessageObj);
                         objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                         objErrorHandler.CreatePayLog_Final("Important checkoutpage after smtpclientsend-" + oOrderInfo.OrderID + "--" + Emailadd);
                      }


           
                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner5 OrderId=" + OrderId);

                if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                {
                    if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                    {
                        //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                        MessageObj.To.Clear();
                        MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                          DataTable dtcheckmailsend1 = objOrderServices.CheckMailLog_Email(oOrderInfo.OrderID, MessageObj.To.ToString());
                if (dtcheckmailsend1 == null || dtcheckmailsend1.Rows.Count == 0)
                {

                        smtpclient.Send(MessageObj);
                        objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                        objErrorHandler.CreatePayLog_Final("Important checkoutpage after smtpclientsend-" + oOrderInfo.OrderID + "--" + MessageObj.To);
                }
                    }
                    if (Adminemails != "")
                    {

                        string[] emailid = Adminemails.ToString().Split(',');
                        if (emailid.Length > 0)
                        {
                            foreach (string id in emailid)
                            {
                                if (ApprovedUserEmailadd.ToUpper().ToString() != id.ToUpper().ToString() && Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                {
                                    //MessageObj.CC.Add(id.ToString());
                                    MessageObj.Subject = "Bigtop International Order Alert - Order No : " + CustomerOrderNo.ToString();
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(id.ToString());

                                          DataTable dtcheckmailsend1 = objOrderServices.CheckMailLog_Email(oOrderInfo.OrderID, MessageObj.To.ToString());
                                          if (dtcheckmailsend1 == null || dtcheckmailsend1.Rows.Count == 0)
                                          {
                                              smtpclient.Send(MessageObj);
                                              objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                              objErrorHandler.CreatePayLog_Final("Important checkoutpage1 after smtpclientsend-" + oOrderInfo.OrderID + "--" + MessageObj.To);
                                          }
                                }
                            }
                        }
                        else
                        {
                            if (ApprovedUserEmailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString() && Emailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString())
                            {
                                MessageObj.To.Clear();
                                MessageObj.To.Add(Adminemails.ToString());
                                DataTable dtcheckmailsend1 = objOrderServices.CheckMailLog_Email(oOrderInfo.OrderID, MessageObj.To.ToString());
                                if (dtcheckmailsend1 == null || dtcheckmailsend1.Rows.Count == 0)
                                {
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    objErrorHandler.CreatePayLog_Final("Important checkoutpage2 after smtpclientsend-" + oOrderInfo.OrderID + "--" + MessageObj.To);
                                }
                            }
                            //MessageObj.CC.Add(emails.ToString());
                        }

                    }
                }
                else
                {
                    if (emails != "")
                    {

                        string[] emailid = emails.ToString().Split(',');
                        if (emailid.Length > 0)
                        {
                            foreach (string id in emailid)
                            {
                                if (Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                {
                                    //MessageObj.CC.Add(id.ToString());
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(id.ToString());
                                    DataTable dtcheckmailsend1 = objOrderServices.CheckMailLog_Email(oOrderInfo.OrderID, MessageObj.To.ToString());
                                    if (dtcheckmailsend1 == null || dtcheckmailsend1.Rows.Count == 0)
                                    {
                                        smtpclient.Send(MessageObj);
                                        objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                        objErrorHandler.CreatePayLog_Final("Important checkoutpage3 after smtpclientsend-" + oOrderInfo.OrderID + "--" + MessageObj.To);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Emailadd.ToUpper().ToString() != emails.ToUpper().ToString())
                            {
                                MessageObj.To.Clear();
                                MessageObj.To.Add(emails.ToString());
                                DataTable dtcheckmailsend1 = objOrderServices.CheckMailLog_Email(oOrderInfo.OrderID, MessageObj.To.ToString());
                                if (dtcheckmailsend1 == null || dtcheckmailsend1.Rows.Count == 0)
                                {
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    objErrorHandler.CreatePayLog_Final("Important checkoutpage4 after smtpclientsend-" + oOrderInfo.OrderID + "--" + MessageObj.To);
                                }
                                //MessageObj.CC.Add(emails.ToString());
                            }
                        }

                    }


                }


            }
        }
        catch (Exception ex)
        {
            objHelperServices.Mail_Error_Log("PP", OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]),1);
            objHelperServices.Mail_Log("PP", OrderId, "", ex.Message);
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            

        }
        objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner6 OrderId=" + OrderId);
    }
    private void SendMail_AfterPaymentSP(int OrderId, int OrderStatus, bool isau)
    {

        objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP start OrderId=" + OrderId);
        string toemail = "";
        try
        {

            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner1 OrderId=" + OrderId);
            string BillAdd;
            string ShippAdd;
            string stemplatepath;
            DataSet dsOItem = new DataSet();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

            oPayInfo = objPaymentServices.GetPayment(OrderId);
            oOrderInfo = objOrderServices.GetOrder(OrderId);

            int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

            //oUserInfo = objUserServices.GetUserInfo(UserID);
         //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            dsOItem = objOrderServices.GetOrderItems(OrderId);
            BillAdd = GetBillingAddress(OrderId);
            ShippAdd = GetShippingAddress(OrderId);

            string ShippingMethod = oOrderInfo.ShipMethod;
            string CustomerOrderNo = oPayInfo.PORelease;
            string shippingnotes = oOrderInfo.ShipNotes;


            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner2 OrderId=" + OrderId);
            if (oOrderInfo.CreatedUser != 777)
            {

                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            }
            else
            {
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
            }
           // oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
            string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
            string Emailadd = oUserInfo.AlternateEmail;
            toemail = oUserInfo.AlternateEmail; 

            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

            int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
            oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
            string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

            string SubmittedBy = "";
            switch (oOrderInfo.OrderStatus)
            {
                case 6:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
                case 12:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
                default:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
            }
            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner3 OrderId=" + OrderId);
            //string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                TBWDataList1[] lstrows1 = new TBWDataList1[0];

                stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;

                DataSet dscat = new DataSet();
                DataTable dt = null;
                _stg_records = new StringTemplateGroup("row", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);


                lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];

                int ictrecords = 0;
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner4 OrderId=" + OrderId);
                foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                {
                    //if (websiteid == 3)
                    //   _stmpl_records = _stg_records.GetInstanceOf("mail-wagner" + "\\" + "row");
                    //   else
                    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");

                    _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                    _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }

                //if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                //if( websiteid == 3)
                //   _stmpl_container = _stg_container.GetInstanceOf("mail-wagner" + "\\" + "OrderSubmitted");
                //    else
                _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedAfterPay");


                //else
                //    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");


                //_stmpl_container.SetAttribute("CONNOTNO", oOrderInfo.TrackingNo);  
                //_stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                //_stmpl_container.SetAttribute("SHIPPEDBY", oOrderInfo.ShipCompany);
                _stmpl_container.SetAttribute("PAY_METHOD", "Credit Card SP");
                _stmpl_container.SetAttribute("AMOUNT", oOrderInfo.TotalAmount);
                _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID);
                _stmpl_container.SetAttribute("OrderDate", Createdon);
                _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                _stmpl_container.SetAttribute("CreatedBy", Createdby);
                // if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                // else
                //    _stmpl_container.SetAttribute("SubmittedBy", "");



                _stmpl_container.SetAttribute("ShippingMethod", ShippingMethod);
                _stmpl_container.SetAttribute("BillingAddress", BillAdd);
                _stmpl_container.SetAttribute("ShippingAddress", ShippAdd);
                _stmpl_container.SetAttribute("shippingnotes", shippingnotes);

                if (shippingnotes != "")
                    _stmpl_container.SetAttribute("TBT_shippingnotes", true);
                else
                    _stmpl_container.SetAttribute("TBT_shippingnotes", false);

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                //Added by :Indu For View order button 
                _stmpl_container.SetAttribute("OrderNo", oOrderInfo.OrderID);
                string orderurl = "https://www.wagneronline.com.au/OrderReport.aspx?OrdId=" + oOrderInfo.OrderID;
                _stmpl_container.SetAttribute("OrderURL", orderurl);
                _stmpl_container.SetAttribute("pricecurrency", "$");

                _stmpl_container.SetAttribute("pname", oOrderInfo.BillcompanyName);
                _stmpl_container.SetAttribute("pstreet", oOrderInfo.BillAdd1);
                _stmpl_container.SetAttribute("locality", oOrderInfo.BillCity);
                _stmpl_container.SetAttribute("region", oOrderInfo.BillState);
                _stmpl_container.SetAttribute("country", oOrderInfo.BillCountry);
                //****************************//


                sHTML = _stmpl_container.ToString();
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner5 OrderId=" + OrderId);
                objErrorHandler.CreatePayLog(sHTML);

            }
            catch (Exception ex)
            {
                objHelperServices.Mail_Error_Log("SP", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]),1);
                objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, "", ex.Message);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
                
            }
            if (sHTML != "")
            {
                //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                //System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

                //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                //MessageObj.To.Add(Emailadd.ToString());
                ////MessageObj.To.Add("jtechalert@gmail.com");
                ////MessageObj.To.Add("mohanarangam.e.r@jtechindia.com");
                //MessageObj.Subject = "Pending Order - WES Australasia";
                //MessageObj.IsBodyHtml = true;
                //MessageObj.Body = sHTML;
                //System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                ////System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                //smtpclient.UseDefaultCredentials = false;
                //smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                ////smtpclient.Port = 587;
                ////smtpclient.Credentials = new System.Net.NetworkCredential("jtechalert@gmail.com", "jtech@#$123");
                //smtpclient.Send(MessageObj);

                //objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                ////ArrayList CCList = new ArrayList();
                ////CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                ////objNotificationServices.NotifyCC = CCList;
                //objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                //objNotificationServices.NotifyTo.Add(Emailadd.ToString());

                // string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                //EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
                //objNotificationServices.NotifySubject = EmailSubject;
                //objNotificationServices.NotifyMessage = sHTML;
                //objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                //objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                //objNotificationServices.NotifyIsHTML = true;
                //objNotificationServices.SendMessage();

                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner6 OrderId=" + OrderId);
                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());



                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner7 OrderId=" + OrderId);

                string emails = "";
                string Adminemails = "";
                string webadminmail = "";
                webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                {
                    MessageObj.To.Add(Emailadd.ToString());
                    MessageObj.Bcc.Add(webadminmail);

                    //if (isau == false)
                    //{
                    //    if (System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"].ToString() == "9200")
                    //        Adminemails = System.Configuration.ConfigurationManager.AppSettings["ToMail"].ToString();
                    //    else
                    //        Adminemails = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                    //}

                    // Get_ADMIN_UserEmils();
                    //if (ApprovedUserEmailadd.Trim() != "" && Emailadd.ToString() != ApprovedUserEmailadd.ToString())
                    //   MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                }
                else
                {
                    emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                    MessageObj.To.Add(Emailadd.ToString());


                }
                string addgoogleurl = System.Configuration.ConfigurationManager.AppSettings["addgoogleurl"].ToString();
                if (addgoogleurl == "true")
                {
                    MessageObj.To.Add("schema.whitelisting+sample@gmail.com");
                    MessageObj.To.Add("indumathi@jtechindia.com");
                }
                //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                // {
                MessageObj.Subject = "Bigtop Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
                //}
                //else
                //{
                //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                // }

                MessageObj.IsBodyHtml = true;
                MessageObj.Body = sHTML;
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner8 OrderId=" + OrderId);

                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);
                objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");

                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner9 OrderId=" + OrderId);

                if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                {
                    if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                    {
                        //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                        MessageObj.To.Clear();
                        MessageObj.To.Add(ApprovedUserEmailadd.ToString());

                        smtpclient.Send(MessageObj);
                        objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                        objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner10 OrderId=" + OrderId);
                    }
                    if (Adminemails != "")
                    {

                        string[] emailid = Adminemails.ToString().Split(',');
                        if (emailid.Length > 0)
                        {
                            foreach (string id in emailid)
                            {
                                if (ApprovedUserEmailadd.ToUpper().ToString() != id.ToUpper().ToString() && Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                {
                                    //MessageObj.CC.Add(id.ToString());
                                    MessageObj.Subject = "Bigtop Order Alert - Order No : " + CustomerOrderNo.ToString();
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(id.ToString());
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner11 OrderId=" + OrderId);
                                }
                            }
                        }
                        else
                        {
                            if (ApprovedUserEmailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString() && Emailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString())
                            {
                                MessageObj.To.Clear();
                                MessageObj.To.Add(Adminemails.ToString());
                                smtpclient.Send(MessageObj);
                                objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner12 OrderId=" + OrderId);
                            }
                            //MessageObj.CC.Add(emails.ToString());
                        }

                    }
                }
                else
                {
                    if (emails != "")
                    {

                        string[] emailid = emails.ToString().Split(',');
                        if (emailid.Length > 0)
                        {
                            foreach (string id in emailid)
                            {
                                if (Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                {
                                    //MessageObj.CC.Add(id.ToString());
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(id.ToString());
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner13 OrderId=" + OrderId);
                                }
                            }
                        }
                        else
                        {
                            if (Emailadd.ToUpper().ToString() != emails.ToUpper().ToString())
                            {
                                MessageObj.To.Clear();
                                MessageObj.To.Add(emails.ToString());
                                smtpclient.Send(MessageObj);
                                objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner14 OrderId=" + OrderId);
                                //MessageObj.CC.Add(emails.ToString());
                            }
                        }

                    }


                }


            }
        }
        catch (Exception ex)
        {
            objHelperServices.Mail_Error_Log("SP", OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]),1);
            objHelperServices.Mail_Log("SP", OrderId, "", ex.Message);
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner14 OrderId=" + OrderId);

        }
        objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP end OrderId=" + OrderId);
    }
    private string Get_ADMIN_APPROVED_UserEmils(string user_id)
    {
        DataSet oDs = new DataSet();
        string emails = "";

        string userid = user_id;//Session["USER_ID"].ToString();
        if (userid == "")
            userid = "0";


        try
        {

            oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_APPROVED_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
            if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
            {

                oDs.Tables[0].TableName = "Users";

                foreach (DataRow rItem in oDs.Tables["Users"].Rows)
                {
                    if (rItem["EMAILADDR"].ToString() != "")
                        emails = emails + rItem["EMAILADDR"].ToString() + ",";

                }
                if (emails != "")
                    emails = emails.Substring(0, emails.Length - 1) + "";
            }
        }
        catch (Exception ex)
        {

        }
        return emails;
    }
    private void SetTabsetting(string tab,bool isOpen  )
    {
        HtmlGenericControl sp = null ;
        HtmlGenericControl hp = null;
        HtmlGenericControl dp = null;
    if (tab == "Pay") {
       sp = spanPay;
        hp = h3Pay;
        dp = divPay;
       
    }
    if (tab == "paid") {
        sp = spanpaid;
        hp = hpaid;
        dp = divpaid;
       
    }
    if (tab == "ship") {
        sp =spanship;
        hp = hship;
        dp = divship;
      
    }
   
    if (isOpen==false) {
        sp.Attributes["class"] = "collapsed";
      //  hp.Attributes["class"] = "check ui-accordion-header ui-state-default";
        dp.Attributes["class"] = "panel-collapse collapse";        
        dp.Attributes["style"]="display:none;";
        
    }
    else {
        sp.Attributes["class"]= "";
     //   hp.Attributes["class"] = "check ui-accordion-header ui-state-active ui-accordion-header-active";
        dp.Attributes["class"] = "panel-collapse collapse in";
        
        dp.Attributes["style"]="display:block;";
        
    }


    }
    private int GetParams_orderid(string orderid)
    {
        try
        {
            //objErrorHandler.CreateLog("Invalid user in checkout GetParams_orderid" + "-" + orderid);
            if (Request.Url.Query != null && Request.Url.Query != "")
            {

                string id;
                if (Request.Url.Query.Contains("key=") == false)
                {

                    id = Request.Url.Query.Replace("?", "");
                    id = id.Replace("#####" + "PaySP", "");
                    id = id.Replace("&" + "CancelOrder=true", "");
                     int n;
                bool isNumeric = int.TryParse(id, out n);
                objErrorHandler.CreateLog("Invalid user in checkout" + "isNumeric-" + isNumeric);
                if (isNumeric == true)
                {
                    OrderDB objOrderDB = new OrderDB();
                      objErrorHandler.CreateLog("before userid1"+ id );
                    string orderuserid = (string)objOrderDB.GetGenericDataDB("", id.ToString(), "GETUSERID_ORDER", OrderDB.ReturnType.RTString);

                      objErrorHandler.CreateLog("orderuserid--" + orderuserid);
                      objErrorHandler.CreateLog("Userid--" + Userid.ToString());
                    if ((orderuserid != Userid.ToString()) && (orderuserid != "777") && (Userid != 777))
                    {
                        Userid = Convert.ToInt32(orderuserid);
                          UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                          oUserInfo = objUserServices.GetUserInfo(Userid);
                   {
                       objErrorHandler.CreateLog("abortsession");
                       //Session.RemoveAll();
                       //Session.Clear();
                       //Session.Abandon();
                       Session["USER_ID"] = "";
                       Session["DUMMY_FLAG"] = "0";
                       Session["ORDER_ID"] = "0";
                       Session["AUTO_LOGIN"] = "0";

                       Session["Userid"] = orderuserid;
                       Session["USER_NAME"] = oUserInfo.LoginName;
                       Session["USER_ID"] = orderuserid;
                       Session["Emailid"] =  oUserInfo.Email ;
                       Session["Firstname"] = oUserInfo.FirstName ;
                       Session["Lastname"] = oUserInfo.LastName ;
                       Session["ORDER_ID"] = id;
                       Session["USER_ROLE"] = oUserInfo.USERROLE;

                       Session["COMPANY_ID"] = oUserInfo.CompanyID;
                       Session["CUSTOMER_TYPE"] = oUserInfo.CUSTOMER_TYPE;
                       Session["DUMMY_FLAG"] = "1";
                       Session["AUTO_LOGIN"] = "1";
                       objErrorHandler.CreateLog("Userid " + Session["Userid"] + " USER_NAME " + Session["USER_NAME"]);
                   }
                        //return 0;
                    }
                }
                else
                {
                    return 0;
                }
                }
            }
            return 1;
              
        }
        catch (Exception ex)
        {
       objErrorHandler.CreateLog(ex.ToString());
      return 1;
           
        }

    }



    private void GetParams()
    {
        try
        {
            if (Request.Url.Query != null && Request.Url.Query != "")
            {

                string id = null;
                if (Request.Url.Query.Contains("key=") == false)
                {

                    id = Request.Url.Query.Replace("?", "");
                    id = id.Replace("&" + "CancelOrder=true", "");
                    int n;
                    bool isNumeric = int.TryParse(id, out n);
                      objErrorHandler.CreateLog("Getparams" + id);
                    id = DecryptSP(id);


                    if ((id == null) && (isNumeric == true))
                    {
                        id = Request.Url.Query.Replace("?", "");
                        id = id.Replace("&" + "CancelOrder=true", "");
                        if (id.Contains("PaySP") == false)
                        {
                            id = id + "#####" + "PaySP";
                        }
                    }
                }

                else
                {

                    if (HttpContext.Current.Session["Mchkout"] != null)
                    {
                        id = HttpContext.Current.Session["Mchkout"].ToString();
                        id = DecryptSP(id);
                    }
                }
                if (id != null)
                {
                    string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);

                    OrderID = objHelperServices.CI(ids[0]);
                  
                    
                    objErrorHandler.CreateLog("OrderID"+ OrderID.ToString());
                    OrderIDaspx = OrderID;
                  
                    if (ids.Length > 1)
                    {

                        PaytabType = ids[1];
                        objErrorHandler.CreateLog("PaytabType "+PaytabType);

                        Paytab = true;
                       
                    }
                    if (ids.Length > 2)
                    {
                        objErrorHandler.CreateLog("paidtab " + paidtab);
                        paidtab = true;
                    }
                }
                else
                {
                    OrderID = 0;
                    PaytabType = "";
                    OrderIDaspx = 0;
                    // div1.InnerHtml = "";
                    div2.InnerHtml = "Invalid Data";
                    div2.Visible = true;
                  //  div1.Visible = false;
                  //  return 0;
                }

            }
            else
            {
                OrderID = 0;
                PaytabType = "";
                OrderIDaspx = 0;
                div1.Visible = false;
                div2.InnerHtml = "Invalid Data";
                div2.Visible = true;
               // return 0;
            }
          //  return 1;
        }
        catch (Exception ex)
        {
          //  return 0;
            objErrorHandler.CreateLog(ex.ToString());
        }

    }
    //private void GetParams()
    //{
    //    try
    //    {
    //        if (Request.Url.Query != null && Request.Url.Query != "")
    //        {

    //            string id;
    //            if (Request.Url.Query.Contains("key=") == false)
    //            {
    //                id = Request.Url.Query.Replace("?", "");
    //                id = DecryptSP(id);
    //            }
    //            else
    //            {
    //                id = HttpContext.Current.Session["Mchkout"].ToString();
    //                id = DecryptSP(id);
    //            }
    //            if (id != null)
    //            {
    //                string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);

    //                OrderID = objHelperServices.CI(ids[0]);
    //                OrderIDaspx = OrderID;
    //                if (ids.Length > 1)
    //                {
    //                    PaytabType = ids[1];
    //                    Paytab = true;
    //                }
    //                if (ids.Length > 2)
    //                {
    //                    paidtab = true;
    //                }
    //            }
    //            else
    //            {
    //                OrderID = 0;
    //                PaytabType = "";
    //                OrderIDaspx = 0;
    //                // div1.InnerHtml = "";
    //                div2.InnerHtml = "Invalid Data";
    //                div2.Visible = true;
    //                div1.Visible= false;
    //                return;
    //            }

    //        }
    //        else
    //        {
    //            OrderID = 0;
    //            PaytabType = "";
    //            OrderIDaspx = 0;
    //            div1.Visible = false;
    //            div2.InnerHtml = "Invalid Data";
    //            div2.Visible = true;
    //            return;
    //        }
    //    }
    //    catch (Exception ex)

    //    {
    //        objErrorHandler.CreateLog(ex.ToString()); 
    //    }
       
    //}
    private void ShowPopUpMessage()
    {
        //var page = (Page)HttpContext.Current.Handler;
        //page.Title = "WAGNER Alert";
        //this.Panelerroritems.Visible = true;
        //modalpop_itemerror.ID = "popUp";
        //modalpop_itemerror.PopupControlID = "Panelerroritems";
        //modalpop_itemerror.BackgroundCssClass = "modalBackground";
        //modalpop_itemerror.TargetControlID = "btnitemerrors";
        //modalpop_itemerror.DropShadow = true;
        //modalpop_itemerror.CancelControlID = "btnCancel";
        //this.Panelerroritems.Controls.Add(modalpop_itemerror);
        //this.modalpop_itemerror.Show();
        //return;
    }

    public string GetLeaveDuplicateProducts()
    {

        try
        {
            string LeaveDuplicateProds = "";
            if (Session["LeaveDuplicateProds"] != null && Session["LeaveDuplicateProds"].ToString() != "")
            {
                LeaveDuplicateProds = Session["LeaveDuplicateProds"].ToString();
                LeaveDuplicateProds = LeaveDuplicateProds.Replace("-", "");
                if (LeaveDuplicateProds.StartsWith(",") == true)
                    LeaveDuplicateProds = LeaveDuplicateProds.Substring(1);

                if (LeaveDuplicateProds.EndsWith(",") == true)
                    LeaveDuplicateProds = LeaveDuplicateProds.Substring(0, LeaveDuplicateProds.Length - 1);

            }
            return LeaveDuplicateProds;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    private void Load_UserRole(string UserName)
    {
        try
        {
            //Helper oHelper = new Helper();            
            //ConnectionDB oConStr = new ConnectionDB();
            //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
            //SqlDataAdapter oDa = new SqlDataAdapter("SELECT COMPANY_ID, CONTACT, USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE USER_ID = '" + Session["USER_ID"] + "' ", oCon);
            //SqlDataAdapter oDa = new SqlDataAdapter("SELECT COMPANY_ID,CONTACT,USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE USER_ROLE IN (1,2) AND COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS Where USER_ID = '" + Session["USER_ID"] + "')", oCon);
            DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "Users");
            string userid = Session["USER_ID"].ToString();
            if (userid == "")
                userid = "0";
            int TotalUsers = 0;

            oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_APPROVED", CompanyGroupDB.ReturnType.RTDataSet);
            if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
            {

                oDs.Tables[0].TableName = "Users";
                //for (int i = 0; i < oDs.Tables["Users"].Rows.Count; i++)
                //{
                //    lblUserRoleName.Text = oDs.Tables["Users"].Rows[i][1].ToString();
                //}

                //string str1 = "";
                //str1 = "<table width=100% cellpadding=5 cellspacing=0 border=0 style=border-collapse><tr valign=top> <td align=left>";

                foreach (DataRow rItem in oDs.Tables["Users"].Rows)
                {
                    if (TotalUsers == 6)
                    {
                        UserList = UserList + ", "; //+ "<br/>";
                        TotalUsers = 0;
                    }

                    UserList = UserList + (TotalUsers == 0 ? "" : ", ") + rItem["CONTACT"].ToString();
                    TotalUsers = TotalUsers + 1;
                }

                //lblUserRoleName.Text = UserList.Substring(1,UserList.Length-1);
                //str1 = str1 + UserList.Substring(1, UserList.Length - 1) + "</td> </tr></table>";
                //lblUserRoleName.Text = str1;

                lblUserRoleName.Text = UserList.ToString();
            }
            else
                lblUserRoleName.Text = "";
        }
        catch (Exception ex)
        {

        }
    }
    private string Get_ADMIN_APPROVED_UserEmils()
    {
        DataSet oDs = new DataSet();
        string emails = "";

        string userid = Session["USER_ID"].ToString();
        if (userid == "")
            userid = "0";


        try
        {

            oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_APPROVED_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
            if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
            {

                oDs.Tables[0].TableName = "Users";

                foreach (DataRow rItem in oDs.Tables["Users"].Rows)
                {
                    if (rItem["EMAILADDR"].ToString() != "")
                        emails = emails + rItem["EMAILADDR"].ToString() + ",";

                }
                if (emails != "")
                    emails = emails.Substring(0, emails.Length - 1) + "";
            }
        }
        catch (Exception ex)
        {

        }
        return emails;
    }
    private string Get_ADMIN_UserEmils()
    {
        DataSet oDs = new DataSet();
        string emails = "";

        string userid = Session["USER_ID"].ToString();
        if (userid == "")
            userid = "0";


        try
        {

            oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
            if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
            {

                oDs.Tables[0].TableName = "Users";

                foreach (DataRow rItem in oDs.Tables["Users"].Rows)
                {
                    if (rItem["EMAILADDR"].ToString() != "")
                        emails = emails + rItem["EMAILADDR"].ToString() + ",";

                }
                if (emails != "")
                    emails = emails.Substring(0, emails.Length - 1) + "";
            }
        }
        catch (Exception ex)
        {

        }
        return emails;
    }
    #region "Functions"

    public string LoadShippingInfostr(string sUserID)
    {
        string str = "";
        try
        {
            int _UserID;
            string cmpName = "";
            _UserID = objHelperServices.CI(sUserID);
            oOrdShippInfo = objUserServices.GetUserShipInfo(_UserID);
            str = "";
            cmpName = objUserServices.GetCompanyName(_UserID);
            if (cmpName != "")
                str = str + "\n" + cmpName;
            else
                str = str + "\n" + cmpName;

            str = str + (string.IsNullOrEmpty(oOrdShippInfo.FirstName.Trim()) ? "" : "\n" + oOrdShippInfo.FirstName.Trim());
            if (oOrdShippInfo.MiddleName != "")
            {
                try
                {
                    str = str + (string.IsNullOrEmpty(oOrdShippInfo.MiddleName.Trim()) ? "" : " " + oOrdShippInfo.MiddleName.Trim());
                }
                catch
                {}
                }
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.LastName.Trim()) ? "" : " " + oOrdShippInfo.LastName.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipAddress1.Trim()) ? "" : "\n" + oOrdShippInfo.ShipAddress1.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipAddress2.Trim()) ? "" : "\n" + oOrdShippInfo.ShipAddress2.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipAddress3.Trim()) ? "" : "\n" + oOrdShippInfo.ShipAddress3.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipCity.Trim()) ? "" : "\n" + oOrdShippInfo.ShipCity.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipState.Trim()) ? "" : "\n" + oOrdShippInfo.ShipState.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipZip.Trim()) ? "" : " - " + oOrdShippInfo.ShipZip.Trim());
            str = str + string.Format("\n {0}", oOrdShippInfo.ShipCountry);
            //if (oOrdShippInfo.ShipCountry.Trim().Length < 3)
            //{
            //    drpShipCountry.SelectedValue = oOrdShippInfo.ShipCountry;

            //    str = str + "\n  " + (drpShipCountry.SelectedItem.Text != "(Select Country)" ? drpShipCountry.SelectedItem.Text : "");
            //}
            //else str += string.Format("\n {0}", oOrdShippInfo.ShipCountry);

            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipPhone.Trim()) ? "" : "\nPhone No: " + oOrdShippInfo.ShipPhone.Trim());
            double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
            if (Dist <= 50 && Dist > 0)
            {
                oLstItem.Text = "Friendly Driver";
                oLstItem.Value = "FRIENDLYDRIVER";
                oLstItem.Selected = true;
                cmbProvider.Items.Add(oLstItem);
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        return str;
    }

    public string LoadBillInfostr(string sUserID)
    {
        string str = "";
       
        try
        {
            int _UserID;
            string strcmpName = "";
            _UserID = objHelperServices.CI(sUserID);
            oOrdBillInfo = objUserServices.GetUserBillInfo(_UserID);
            strcmpName = objUserServices.GetBillToCompanyName(_UserID);
            str = "";
            if (strcmpName != "")
                str = str + "\n" + strcmpName;
            else
                str = str + "\n";
            //Modified by Indu
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.FirstName.Trim()) ? "" : "\n" + oOrdBillInfo.FirstName.Trim());
            if (oOrdBillInfo.MiddleName != "")
            {
                try
                {
                    str = str + (string.IsNullOrEmpty(oOrdBillInfo.MiddleName.Trim()) ? "" : " " + oOrdBillInfo.MiddleName.Trim());
                }
                catch
                { }
                }
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.LastName.Trim()) ? "" : " " + oOrdBillInfo.LastName.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillAddress1.Trim()) ? "" : "\n" + oOrdBillInfo.BillAddress1.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillAddress2.Trim()) ? "" : "\n" + oOrdBillInfo.BillAddress2.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillAddress3.Trim()) ? "" : "\n" + oOrdBillInfo.BillAddress3.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillCity.Trim()) ? "" : "\n" + oOrdBillInfo.BillCity.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillState.Trim()) ? "" : "\n" + oOrdBillInfo.BillState.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillZip.Trim()) ? "" : " - " + oOrdBillInfo.BillZip.Trim());
            str = str + string.Format("\n{0}", oOrdBillInfo.BillCountry.Trim());
            //if (oOrdBillInfo.BillCountry.Trim().Length < 3)
            //{
            //    drpBillCountry.SelectedValue = oOrdBillInfo.BillCountry;
            //    str = str + (drpBillCountry.SelectedItem.Text != "(Select Country)" ? "\n" + drpBillCountry.SelectedItem.Text : "");
            //}
            //else str += string.Format("\n{0}", oOrdBillInfo.BillCountry.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillPhone.Trim()) ? "" : "\nPhone No: " + oOrdBillInfo.BillPhone.Trim());
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");

        return str;
    }

    public void LoadShippingInfo(string sUserID)
    {
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oOrdShippInfo = objUserServices.GetUserShipInfo(_UserID);

            txtSFName.Text = oOrdShippInfo.FirstName;
            txtSLName.Text = oOrdShippInfo.LastName;
            txtbillMName.Text = oOrdShippInfo.MiddleName;
            txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
            txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
            txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
            txtSCity.Text = oOrdShippInfo.ShipCity;
            drpShipState.Text = oOrdShippInfo.ShipState;
            txtSZip.Text = oOrdShippInfo.ShipZip;
            drpShipCountry.SelectedValue = oOrdShippInfo.ShipCountry;
            Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
            txtSPhone.Text = oOrdShippInfo.ShipPhone;
            double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
            if (Dist <= 50 && Dist > 0)
            {
                oLstItem.Text = "Friendly Driver";
                oLstItem.Value = "FRIENDLYDRIVER";
                oLstItem.Selected = true;
                cmbProvider.Items.Add(oLstItem);
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void LoadBillInfo(string sUserID)
    {
        UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oOrdBillInfo = objUserServices.GetUserBillInfo(_UserID);
            txtbillFName.Text = oOrdBillInfo.FirstName;
            txtbillLName.Text = oOrdBillInfo.LastName;
            txtbillMName.Text = oOrdBillInfo.MiddleName;
            txtbilladd1.Text = oOrdBillInfo.BillAddress1;
            txtbilladd2.Text = oOrdBillInfo.BillAddress2;
            txtbilladd3.Text = oOrdBillInfo.BillAddress3;
            txtbillcity.Text = oOrdBillInfo.BillCity;
            drpBillState.Text = oOrdBillInfo.BillState;
            txtbillzip.Text = oOrdBillInfo.BillZip;
            drpBillCountry.SelectedValue = oOrdBillInfo.BillCountry;
            Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
            txtbillphone.Text = oOrdBillInfo.BillPhone;
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void ClearShippingInfo()
    {
        txtSFName.Text = "";
        txtSLName.Text = "";
        txtSAdd1.Text = "";
        txtSAdd2.Text = "";
        txtSAdd3.Text = "";
        txtSCity.Text = "";
        txtSZip.Text = "";
        txtSPhone.Text = "";
    }

    public void ClearBillingInfo()
    {
        txtbilladd1.Text = "";
        txtbilladd2.Text = "";
        txtbilladd3.Text = "";
        txtbillcity.Text = "";
        txtbillFName.Text = "";
        txtbillLName.Text = "";
        txtbillphone.Text = "";
        txtbillzip.Text = "";
    }

    private void GetApproveOrderDetails(int OrderID)
    {
        DataSet dsOD = new DataSet();
        dsOD = objOrderServices.GetApproveOrderItems(OrderID);

        oOrdInfo1 = objOrderServices.GetOrder(OrderID);

        if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in dsOD.Tables[0].Rows)
            {
                ttOrder.Text = string.IsNullOrEmpty(row["PO_RELEASE"].ToString()) ? "" : row["PO_RELEASE"].ToString();

                if (row["SHIP_METHOD"].ToString() == "Drop Shipment Order" && Checkdrpdownlistvalue(drpSM1, row["SHIP_METHOD"].ToString()) == false)
                    drpSM1.Items.Add(new ListItem("Drop Shipment Order", "Drop Shipment Order"));

                drpSM1.SelectedValue = string.IsNullOrEmpty(row["SHIP_METHOD"].ToString()) ? "" : row["SHIP_METHOD"].ToString();
                Setdrpdownlistvalue(drpSM1, string.IsNullOrEmpty(row["SHIP_METHOD"].ToString()) ? "" : row["SHIP_METHOD"].ToString());

                TextBox1.Text = string.IsNullOrEmpty(row["COMMENTS"].ToString()) ? "" : row["COMMENTS"].ToString();
                if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
                {
                    txtCompany.Text = objHelperServices.Prepare(oOrdInfo1.ShipCompName);
                    txtAttentionTo.Text = objHelperServices.Prepare(oOrdInfo1.ShipFName);
                    txtAddressLine1.Text = objHelperServices.Prepare(oOrdInfo1.ShipAdd1);
                    txtAddressLine2.Text = objHelperServices.Prepare(oOrdInfo1.ShipAdd2);
                    txtSuburb.Text = objHelperServices.Prepare(oOrdInfo1.ShipCity);
                    drpState.Text = objHelperServices.Prepare(oOrdInfo1.ShipState);
                    txtCountry.Text = objHelperServices.Prepare(oOrdInfo1.ShipCountry);
                    txtPostCode.Text = objHelperServices.Prepare(oOrdInfo1.ShipZip);
                    // txtReceiverContactName.Text = objHelperServices.Prepare(oOrdInfo1.ReceiverContact);
                    txtDeliveryInstructions.Text = objHelperServices.Prepare(oOrdInfo1.DeliveryInstr);
                    txtShipPhoneNumber.Text = objHelperServices.Prepare(oOrdInfo1.ShipPhone);

                    if (objOrderServices.IsUserCanDropShip(objHelperServices.CI(Session["USER_ID"])) == false)  // Drop shipment available as per user role
                    {
                        txtCompany.Enabled = false;
                        txtAttentionTo.Enabled = false;
                        txtAddressLine1.Enabled = false;
                        txtAddressLine2.Enabled = false;
                        txtSuburb.Enabled = false;
                        drpState.Enabled = false;
                        txtCountry.Enabled = false;
                        txtPostCode.Enabled = false;
                        txtDeliveryInstructions.Enabled = false;
                        txtShipPhoneNumber.Enabled = false;
                        drpSM1.Enabled = false;
                        ttOrder.Enabled = false;
                        TextBox1.Enabled = false;
                    }
                }




            }
        }
        else
        {
            SetSessionVlaue();
        }
    }

    #endregion

    #region "Control Events"

    protected void btnShipProceed_Click(object sender, EventArgs e)
    {
        try
        {
            QuoteServices objQuoteServices = new QuoteServices();
            int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
           // decimal TaxAmount;
            decimal ProdTotCost;
            OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            int QuoteId = 0;
            QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));

            GetParams();
          //  if (Request["OrderId"] != null)
           //     OrderID = objHelperServices.CI(Request["OrderId"].ToString());
            if (Request["QteId"] != null)
            {
                QuoteID = objHelperServices.CI(Request["QteId"].ToString());
                OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
                OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
            }

            //string status=objOrderServices.GetOrderStatus(OrderID);
            //if (status != "OPEN")
            //{
            //    if (QuoteId != 0)
            //    {
            //        OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteId));
            //        OrdStatus = (int)OrderServices.OrderStatus.PLACEQUOTE;
            //    }
            //}
            //    else
            //    {
            //        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);

            //    }

            ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
           // TaxAmount = objOrderServices.CalculateTaxAmount(ProdTotCost, OrderID.ToString());
            decimal UpdRst = 0;
            oOrdInfo.OrderID = OrderID;
            // oOrdInfo.OrderStatus = OrdStatus;
            oOrdInfo.OrderStatus = OrdStatus;
            oOrdInfo.ShipCompany = cmbProvider.SelectedValue;
            oOrdInfo.ShipMethod = drpSM1.SelectedValue;

            if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
            {
                oOrdInfo.ShipCompName = objHelperServices.Prepare(txtCompany.Text);
                oOrdInfo.ShipFName = objHelperServices.Prepare(txtAttentionTo.Text);
                oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtAddressLine1.Text);
                oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtAddressLine2.Text);
                oOrdInfo.ShipCity = objHelperServices.Prepare(txtSuburb.Text);
                oOrdInfo.ShipState = objHelperServices.Prepare(drpState.Text);
                oOrdInfo.ShipCountry = objHelperServices.Prepare(txtCountry.Text);
                oOrdInfo.ShipZip = objHelperServices.Prepare(txtPostCode.Text);
                //   oOrdInfo.ReceiverContact = objHelperServices.Prepare(txtReceiverContactName.Text);
                oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDeliveryInstructions.Text);
                oOrdInfo.ShipPhone = objHelperServices.Prepare(txtShipPhoneNumber.Text);
            }
            else
            {
                oOrdInfo.ShipFName = objHelperServices.Prepare(txtSFName.Text);
                oOrdInfo.ShipLName = objHelperServices.Prepare(txtSLName.Text);
                oOrdInfo.ShipMName = objHelperServices.Prepare(txtbillMName.Text);
                oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtSAdd1.Text);
                oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtSAdd2.Text);
                oOrdInfo.ShipAdd3 = objHelperServices.Prepare(txtSAdd3.Text);
                oOrdInfo.ShipCity = objHelperServices.Prepare(txtSCity.Text);
                oOrdInfo.ShipState = objHelperServices.Prepare(drpShipState.Text);
                oOrdInfo.ShipCountry = objHelperServices.Prepare(drpShipCountry.Text);
                oOrdInfo.ShipZip = objHelperServices.Prepare(txtSZip.Text);
                
            }

            oOrdInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
            oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);
            oOrdInfo.isEmailSent = false;
            oOrdInfo.isInvoiceSent = false;
            oOrdInfo.IsShipped = false;

            oOrdInfo.BillFName = objHelperServices.Prepare(txtbillFName.Text);
            oOrdInfo.BillLName = objHelperServices.Prepare(txtbillLName.Text);
            oOrdInfo.BillMName = objHelperServices.Prepare(txtbillMName.Text);
            oOrdInfo.BillAdd1 = objHelperServices.Prepare(txtbilladd1.Text);
            oOrdInfo.BillAdd2 = objHelperServices.Prepare(txtbilladd2.Text);
            oOrdInfo.BillAdd3 = objHelperServices.Prepare(txtbilladd3.Text);
            oOrdInfo.BillCity = objHelperServices.Prepare(txtbillcity.Text);
            oOrdInfo.BillState = objHelperServices.Prepare(drpBillState.Text);
            oOrdInfo.BillCountry = objHelperServices.Prepare(drpBillCountry.Text);
            oOrdInfo.BillZip = objHelperServices.Prepare(txtbillzip.Text);
            oOrdInfo.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
            oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
            oOrdInfo.ShipCost = CalculateShippingCost(OrderID);
            oOrdInfo.TaxAmount = objOrderServices.CalculateTaxAmount(oOrdInfo.ProdTotalPrice , oOrdInfo.ShipCost, OrderID.ToString());
            oOrdInfo.TotalAmount = ProdTotCost + oOrdInfo.TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
            oOrdInfo.TrackingNo = "";
            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
            UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
            double Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
            if (UpdRst > 0)
            {
                Session["ShipCost"] = oOrdInfo.ShipCost;
                if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                {
                    Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
                }
                else
                {
                    Response.Redirect("Payment.aspx?OrdId=" + OrderID, false);
                }
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    //public decimal CalculateTaxAmount(decimal ProdTotalPrice)
    //{
    //    try
    //    {
    //        CountryServices objCountryServices = new CountryServices();
    //        OrderServices objOrderServices = new OrderServices();
    //        string BillState;
    //        string BillCountry;
    //        decimal RetTax = 0;
    //        if (objUserServices.GetTaxExempt(objHelperServices.CI(Session["USER_ID"])) == false)
    //        {

    //            //BillState = drpBillState.Text;
    //            //BillCountry = drpBillCountry.SelectedValue;
    //            //decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));
    //            //RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));

    //            if (FIXED_TAX.ToUpper() == "TRUE")
    //            {

    //                decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
    //                RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
    //            }
    //            else
    //            {
    //                BillState = drpBillState.Text;
    //                BillCountry = drpBillCountry.SelectedValue;
    //                decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));

    //                RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
    //            }

    //            return RetTax;
    //        }
    //        return RetTax;
    //    }
    //    catch (Exception Ex)
    //    {
    //        objErrorHandler.ErrorMsg = Ex;
    //        objErrorHandler.CreateLog();
    //        return -1;
    //    }
    //}

    protected void ChkShippingAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkShippingAdd.Checked == false)
            {
                ClearShippingInfo();
            }
            else
            {
                LoadShippingInfo(Session["USER_ID"].ToString());
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void ChkbillingAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkbillingAdd.Checked == false)
            {
                ClearBillingInfo();
            }
            else
            {
                LoadBillInfo(Session["USER_ID"].ToString());
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    //Update User table Shipping Address
    protected void ChkDefaultShipAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkShipDefaultaddr.Checked == true)
            {
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oOrdShipAddr = new UserServices.UserInfo();
                oOrdShipAddr.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdShipAddr.FirstName = objHelperServices.Prepare(txtSFName.Text);
                oOrdShipAddr.LastName = objHelperServices.Prepare(txtSLName.Text);
                oOrdShippInfo.MiddleName = objHelperServices.Prepare(txtSMName.Text);
                oOrdShipAddr.ShipAddress1 = objHelperServices.Prepare(txtSAdd1.Text);
                oOrdShipAddr.ShipAddress2 = objHelperServices.Prepare(txtSAdd2.Text);
                oOrdShipAddr.ShipAddress3 = objHelperServices.Prepare(txtSAdd3.Text);
                oOrdShipAddr.ShipCity = objHelperServices.Prepare(txtSCity.Text);
                oOrdShipAddr.ShipState = objHelperServices.Prepare(drpShipState.Text);
                oOrdShipAddr.ShipCountry = objHelperServices.Prepare(drpShipCountry.Text);
                oOrdShipAddr.ShipZip = objHelperServices.Prepare(txtSZip.Text);
                oOrdShipAddr.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
                objUserServices.UpdateShippingInfo(oOrdShipAddr);
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }
    //Update User table Billing Address
    protected void ChkDefaultBillAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkBillDefaultaddr.Checked == true)
            {
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oOrdBillAddr = new UserServices.UserInfo();
                oOrdBillAddr.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdBillAddr.FirstName = objHelperServices.Prepare(txtbillFName.Text);
                oOrdBillAddr.LastName = objHelperServices.Prepare(txtbillLName.Text);
                oOrdBillAddr.MiddleName = objHelperServices.Prepare(txtbillMName.Text);
                oOrdBillAddr.BillAddress1 = objHelperServices.Prepare(txtbilladd1.Text);
                oOrdBillAddr.BillAddress2 = objHelperServices.Prepare(txtbilladd2.Text);
                oOrdBillAddr.BillAddress3 = objHelperServices.Prepare(txtbilladd3.Text);
                oOrdBillAddr.BillCity = objHelperServices.Prepare(txtbillcity.Text);
                oOrdBillAddr.BillState = objHelperServices.Prepare(drpBillState.Text);
                oOrdBillAddr.BillCountry = objHelperServices.Prepare(drpBillCountry.Text);
                oOrdBillAddr.BillZip = objHelperServices.Prepare(txtbillzip.Text);
                oOrdBillAddr.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
                objUserServices.UpdateBillingInfo(oOrdBillAddr);
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void LoadCountryList()
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpShipCountry.Items.Clear();
            drpShipCountry.DataSource = oDs;
            drpShipCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpShipCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpShipCountry.DataBind();
            drpShipCountry.Items.Add(new ListItem("(Select Country)", "", true));
            //oDs = new DataSet();
            //oDs = objCountryServices.GetCountries();
            drpBillCountry.Items.Clear();
            drpBillCountry.DataSource = oDs;
            drpBillCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpBillCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpBillCountry.DataBind();
            drpBillCountry.Items.Add(new ListItem("(Select Country)", "", true));
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void LoadStates(String conCode)
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            // drpShipState.DataSource = oDs;
            // drpShipState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            // drpShipState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //  drpShipState.DataBind();

            oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            // drpBillState.DataSource = oDs;
            // drpBillState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            // drpBillState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //drpBillState.DataBind();
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    protected decimal CalculateShippingCost(int OrderID)
    {
        DataSet dsOItem = new DataSet();
        decimal ShippingValue = 0;
        dsOItem = objOrderServices.GetOrderItems(OrderID);
        decimal ProductCost;

        if (drpSM1.SelectedValue == "Standard Shipping")
        {
            if (objHelperServices.GetOptionValues("COURIER CHARGE") != "")
                ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());

        }
        else if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
        {
            if (dsOItem != null)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    ProductCost = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"]) * objHelperServices.CDEC(rItem["QTY"]);
                    DataSet DS = new DataSet();
                    DS = objOrderServices.GetItemDetailsFromInventory(Convert.ToInt32(rItem["PRODUCT_ID"]));
                    foreach (DataRow oDR in DS.Tables[0].Rows)
                    {
                        if (objHelperServices.CB(oDR["IS_SHIPPING"]) == 1)
                        {
                            ShippingValue = ShippingValue + ((ProductCost * objHelperServices.CDEC(oDR["PROD_SHIP_COST"])) / 100);
                            ShippingValue = objHelperServices.CDEC(ShippingValue);
                        }
                    }
                }
            }
        }
        else
        {
            if (objOrderServices.GetCurrentProductTotalCost(OrderID) < objHelperServices.CDEC(objHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
            {
                if (dsOItem != null)
                {
                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                    {
                        ProductCost = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"]) * objHelperServices.CDEC(rItem["QTY"]);
                        ShippingValue = ShippingValue + ((ProductCost * objHelperServices.CI(objHelperServices.GetOptionValues("SHIPPING CHARGE").ToString())) / 100);
                        ShippingValue = objHelperServices.CDEC(ShippingValue);
                    }
                }
            }

        }


        return ShippingValue;
    }


    #endregion
    private void SetSessionVlaue()
    {
        if (!IsPostBack)
        {
            if (Session["ORDER_NO"] != null)
            {
                ttOrder.Text = Session["ORDER_NO"].ToString();
            }
            if (Session["SHIPPING"] != null)
            {

                Setdrpdownlistvalue(drpSM1, string.IsNullOrEmpty(Session["SHIPPING"].ToString()) ? "" : Session["SHIPPING"].ToString());
            }
            if (Session["DELIVERY"] != null)
            {

                TextBox1.Text = Session["DELIVERY"].ToString();
            }
            if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
            {
                if (Session["DROPSHIP"] != null)
                {
                    string[] cmpadd = Session["DROPSHIP"].ToString().Split(new string[] { "####" }, StringSplitOptions.None);
                    if (cmpadd.Length > 0)
                    {
                        txtCompany.Text = cmpadd[0].ToString();
                        txtAttentionTo.Text = cmpadd[1].ToString();
                        txtAddressLine1.Text = cmpadd[2].ToString();
                        txtAddressLine2.Text = cmpadd[3].ToString();
                        txtSuburb.Text = cmpadd[4].ToString();
                        drpState.Text = cmpadd[5].ToString();
                        txtCountry.Text = cmpadd[6].ToString();
                        txtPostCode.Text = cmpadd[7].ToString();
                        txtDeliveryInstructions.Text = cmpadd[8].ToString();
                        txtShipPhoneNumber.Text = cmpadd[9].ToString();
                    }

                }
            }

        }
    }
    protected void txtSZip_TextChanged(object sender, EventArgs e)
    {
        IsZipCodeChange = true;
    }

    protected void ImageButton5_Click(object sender, EventArgs e)
    {
        try
        {
            Session["ORDER_NO"] = ttOrder.Text.Trim();
            Session["SHIPPING"] = drpSM1.Text.Trim();
            Session["DELIVERY"] = TextBox1.Text.Trim();
            Session["DROPSHIP"] = txtCompany.Text.Trim() + "####" + txtAttentionTo.Text.Trim()
                    + "####" + txtAddressLine1.Text.Trim()
                    + "####" + txtAddressLine2.Text.Trim()
                    + "####" + txtSuburb.Text.Trim()
                    + "####" + drpState.Text.Trim()
                    + "####" + txtCountry.Text.Trim()
                    + "####" + txtPostCode.Text.Trim()
                    + "####" + txtDeliveryInstructions.Text.Trim()
                    + "####" + txtShipPhoneNumber.Text.Trim()
                ;

            if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
            {
                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    Response.Redirect("orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + Session["ORDER_ID"], false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?bulkorder=1&amp;Pid=0", false);
                }
            }

        }
        catch (Exception Ex)
        {

        }
    }

    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        try
        {
            Session["ORDER_NO"] = ttOrder.Text.Trim();
            Session["SHIPPING"] = drpSM1.Text.Trim();
            Session["DELIVERY"] = TextBox1.Text.Trim();
            Session["DROPSHIP"] = txtCompany.Text.Trim() + "####" + txtAttentionTo.Text.Trim()
                    + "####" + txtAddressLine1.Text.Trim()
                    + "####" + txtAddressLine2.Text.Trim()
                    + "####" + txtSuburb.Text.Trim()
                    + "####" + drpState.Text.Trim()
                    + "####" + txtCountry.Text.Trim()
                    + "####" + txtPostCode.Text.Trim()
                    + "####" + txtDeliveryInstructions.Text.Trim()
                    + "####" + txtShipPhoneNumber.Text.Trim()
                ;

        
            if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
            {
                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                  
                    Response.Redirect("orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + Session["ORDER_ID"], false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?bulkorder=1&amp;Pid=0", false);
                }
            }

        }
        catch (Exception Ex)
        {

        }
    }
   
    protected void ImageButton4_Click(object sender, EventArgs e)
    {
        try
        {
            QuoteServices objQuoteServices = new QuoteServices();
            OrderDB objOrderDB = new OrderDB();
            HelperServices objHelperService = new HelperServices();
            //int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            int OrdStatus = 0;
            string ApproveOrder = string.Empty;
            //Direct  Order / Approve Order (Comes from Pending order Page)
            lblpostcode2err.Text = "";
            lbladdline1err.Text = "";
            lbladdline2err.Text = "";
            string clientIPAddress = "";

            if (OrderID > 0)
            {            
                if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
                    clientIPAddress = Session["IP_ADDR"].ToString();
                else
                    clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
  
                objOrderServices.InitilizeOrder_ipaddress(OrderID, clientIPAddress);
              
            }


            if (Request.QueryString["ApproveOrder"] == null)
            {
                if (Session["USER_ROLE"] != null)
                {
                    switch (Convert.ToInt16(Session["USER_ROLE"]))
                    {
                        case 1:
                            OrdStatus = (int)OrderServices.OrderStatus.OPEN ;
                            break;
                        case 2:
                            OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                            break;
                        case 3:
                            OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                            break;
                    }
                }
                else
                {
                    OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                }
            }
            else if (Request.QueryString["ApproveOrder"] != null && (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2))
            {
                OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            }
            else if (Request.QueryString["ApproveOrder"] != null)
                OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;

            //OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
            //decimal TaxAmount;
            decimal ProdTotCost;
            GetParams();
            if (OrderID<=0)
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            //else 
            //    OrderID = Convert.ToInt32(Request["OrderID"].ToString());

            int oldorderID = OrderID;
            int QuoteId = 0;
            QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
            if (ttOrder.Text == "")
                ttOrder.Text="BTP" + OrderID.ToString();

            refid = objHelperServices.CS(ttOrder.Text);

             
            if (OrderID <= 0 || ttOrder.Text.Length < 1)
            {
                txterr.Text = "Please enter valid Order No, Order No should be more than 1 digits";
                OrderID = -1;
                Session["OrderId"] = "-1";
            }
            else
            {
                txterr.Text = "";
                string querystr = "";

                if (Request.QueryString["ApproveOrder"] == null)
                {
                    DataSet DS = new DataSet();
                    //querystr = "select count(*) from TBWC_PAYMENT where order_id in(select order_id from dbo.tbwc_order where [user_id] in(select [user_id] from TBWC_COMPANY_BUYERS where company_id in(select company_id from dbo.TBWC_COMPANY_BUYERS where [user_id]=" + objHelperServices.CI(Session["USER_ID"].ToString()) + "))) and po_release='" + refid + "'";
                    //objHelperServices.SQLString = querystr;
                    //DS = objHelperServices.GetDataSet();
                    DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, OrderID.ToString(), "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);

                    if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
                    {
                        txterr.Text = "Order No already exists, please Re-enter Order No";
                        OrderID = -1;
                    }
                }

            }
            if (Request["QteId"] != null)
            {
                QuoteID = objHelperServices.CI(Request["QteId"].ToString());
                OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
                OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
            }

            if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
            {
                if (objOrderServices.GetDropShipmentKeyExist(txtPostCode.Text.ToString(), "PostCode") == true)
                {
                    //lblpostcode2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                    //OrderID = -1;
                    //ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                    if (objOrderServices.GetDropShipmentKeyExist(txtAddressLine1.Text.ToString(), "") == true)
                    {
                        //lbladdline1err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                        //   OrderID = -1;
                        ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                        // ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>var x=window.confirm('Non-Standard Delivery Area. We will contact you to confirm costing');if (x)window.alert('Good!')</script>", false);

                    }
                    if (txtAddressLine2.Text.ToString() != "" && objOrderServices.GetDropShipmentKeyExist(txtAddressLine2.Text.ToString(), "") == true)
                    {
                        //lbladdline2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                        //   OrderID = -1;
                        ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                    }
                }
                //else
                //{
                //    OrderID = -1;
                //    ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                //}
            }

            //string status=objOrderServices.GetOrderStatus(OrderID);
            //if (status != "OPEN")
            //{
            //    if (QuoteId != 0)
            //    {
            //        OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteId));
            //        OrdStatus = (int)OrderServices.OrderStatus.PLACEQUOTE;
            //    }
            //}
            //    else
            //    {
            //        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            //    }
            if (OrderID > 0)
            {
                 int _UserrID;
                 _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
                 oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                 oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);


                
                  //  if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                 if (objOrderServices.IsNativeCountry(OrderID) == 0)
                    OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                else
                    OrdStatus = (int)OrderServices.OrderStatus.OPEN ;

                ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
               // TaxAmount = objOrderServices.CalculateTaxAmount(ProdTotCost, OrderID.ToString());
                decimal UpdRst = 0;
                oOrdInfo.OrderID = OrderID;
                oOrdInfo.OrderStatus = OrdStatus;
                ///////////////////////////
               
               
                oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                txtSFName.Text = oOrdShippInfo.FirstName;
                txtSLName.Text = oOrdShippInfo.LastName;
                txtSMName.Text = oOrdShippInfo.MiddleName;
                txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
                txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
                txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
                txtSCity.Text = oOrdShippInfo.ShipCity;
                drpShipState.Text = oOrdShippInfo.ShipState;
                txtSZip.Text = oOrdShippInfo.ShipZip;
                Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
                //if (oOrdShippInfo.ShipCountry.Length > 3)
                //{
                //    ListItem cntry = drpShipCountry.Items.FindByText(oOrdShippInfo.ShipCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                //else
                //{
                //    ListItem cntry = drpShipCountry.Items.FindByValue(oOrdShippInfo.ShipCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}

                txtSPhone.Text = oOrdShippInfo.ShipPhone;
                double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
                if (Dist <= 50 && Dist > 0)
                {
                    oLstItem.Text = "Friendly Driver";
                    oLstItem.Value = "FRIENDLYDRIVER";
                    oLstItem.Selected = true;
                    cmbProvider.Items.Add(oLstItem);
                }
                
                //UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
                oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                txtbillFName.Text = oOrdBillInfo.FirstName;
                txtbillLName.Text = oOrdBillInfo.LastName;
                txtbillMName.Text = oOrdBillInfo.MiddleName;
                txtbilladd1.Text = oOrdBillInfo.BillAddress1;
                txtbilladd2.Text = oOrdBillInfo.BillAddress2;
                txtbilladd3.Text = oOrdBillInfo.BillAddress3;
                txtbillcity.Text = oOrdBillInfo.BillCity;
                drpBillState.Text = oOrdBillInfo.BillState;
                txtbillzip.Text = oOrdBillInfo.BillZip;
                Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
                //if (oOrdBillInfo.BillCountry.Length > 3)
                //{
                //    ListItem cntry = drpBillCountry.Items.FindByText(oOrdBillInfo.BillCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                //else
                //{
                //    ListItem cntry = drpBillCountry.Items.FindByValue(oOrdBillInfo.BillCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                txtbillphone.Text = oOrdBillInfo.BillPhone;
                ///////////////////////////
                oOrdInfo.OrderStatus = OrdStatus;
                oOrdInfo.ShipCompany = cmbProvider.SelectedValue;
                oOrdInfo.ShipMethod = drpSM1.SelectedValue;

                if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
                {
                    oOrdInfo.ShipCompName = objHelperServices.Prepare(txtCompany.Text);
                    oOrdInfo.ShipFName = objHelperServices.Prepare(txtAttentionTo.Text);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtAddressLine1.Text);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtAddressLine2.Text);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(txtSuburb.Text);
                    oOrdInfo.ShipState = objHelperServices.Prepare(drpState.Text);
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(txtCountry.Text);
                    oOrdInfo.ShipZip = objHelperServices.Prepare(txtPostCode.Text);
                    oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDeliveryInstructions.Text);
                    //  oOrdInfo.ReceiverContact = objHelperServices.Prepare(txtReceiverContactName.Text);
                    oOrdInfo.ShipPhone = objHelperServices.Prepare(txtShipPhoneNumber.Text);

                }
                else
                {
                    oOrdInfo.ShipFName = objHelperServices.Prepare(txtSFName.Text);
                    oOrdInfo.ShipLName = objHelperServices.Prepare(txtSLName.Text);
                    oOrdInfo.ShipMName = objHelperServices.Prepare(txtbillMName.Text);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtSAdd1.Text);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtSAdd2.Text);
                    oOrdInfo.ShipAdd3 = objHelperServices.Prepare(txtSAdd3.Text);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(txtSCity.Text);
                    oOrdInfo.ShipState = objHelperServices.Prepare(drpShipState.Text);
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(drpShipCountry.SelectedItem.ToString());
                    oOrdInfo.ShipZip = objHelperServices.Prepare(txtSZip.Text);
                    oOrdInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);

                    DataSet objds = new DataSet();
                    objds = (DataSet)objOrderDB.GetGenericDataDB(objHelperServices.CI(Session["USER_ID"].ToString()).ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
                    if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                    {
                        oOrdInfo.DeliveryInstr = objHelperService.CS(objds.Tables[0].Rows[0]["DELIVERY_INST"].ToString());
                    }

                }


                oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);

               

                 oOrdInfo.ClientIPAddress = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
                //oOrdInfo.ClientIPAddress = Request.Params["REMOTE_ADDR"];
                //oOrdInfo.ClientIPAddress = Request.UserHostAddress;
                oOrdInfo.isEmailSent = false;
                oOrdInfo.isInvoiceSent = false;
                oOrdInfo.IsShipped = false;

                oOrdInfo.BillFName = objHelperServices.Prepare(txtbillFName.Text);
                oOrdInfo.BillLName = objHelperServices.Prepare(txtbillLName.Text);
                oOrdInfo.BillMName = objHelperServices.Prepare(txtbillMName.Text);
                oOrdInfo.BillAdd1 = objHelperServices.Prepare(txtbilladd1.Text);
                oOrdInfo.BillAdd2 = objHelperServices.Prepare(txtbilladd2.Text);
                oOrdInfo.BillAdd3 = objHelperServices.Prepare(txtbilladd3.Text);
                oOrdInfo.BillCity = objHelperServices.Prepare(txtbillcity.Text);
                oOrdInfo.BillState = objHelperServices.Prepare(drpBillState.Text);
                oOrdInfo.BillCountry = objHelperServices.Prepare(drpBillCountry.SelectedItem.Text);
                oOrdInfo.BillZip = objHelperServices.Prepare(txtbillzip.Text);
                oOrdInfo.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
                oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
                oOrdInfo.ShipCost = CalculateShippingCost(OrderID);
                oOrdInfo.TaxAmount = objOrderServices.CalculateTaxAmount(oOrdInfo.ProdTotalPrice , oOrdInfo.ShipCost, OrderID.ToString());
                oOrdInfo.TotalAmount = ProdTotCost + oOrdInfo.TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
                oOrdInfo.TrackingNo = "";
                if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
                {
                    oOrdInfo.DropShip = 1;
                }
                oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                UpdRst = objOrderServices.UpdateOrder(oOrdInfo);

                if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
                {
                    Session["PrevOrderID"] = "0";
                }

                Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
                if (UpdRst > 0)
                {
                    objOrderServices.UpdateCustomFields(oOrdInfo);

                    Session["ORDER_NO"] = null;
                    Session["SHIPPING"] = null;
                    Session["DELIVERY"] = null;
                    Session["DROPSHIP"] = null;

                    Session["ShipCost"] = oOrdInfo.ShipCost;
                    if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                    {
                        Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
                    }
                    else
                    {
                        ProceedFunction();
                        PnlOrderInvoice.Visible = true;
                        PnlOrderContents.Visible = false;
                        PHOrderConfirm.Visible = true;
                        ttOrder.Enabled = false;
                        drpSM1.Enabled = false;

                        /* Drop Shipment Fields  */
                        drpState.Enabled = false;
                        txtCompany.Enabled = false;
                        txtPostCode.Enabled = false;
                        txtAddressLine1.Enabled = false;
                        txtAddressLine2.Enabled = false;
                        txtAttentionTo.Enabled = false;
                        txtCountry.Enabled = false;
                        drpShipState.Enabled = false;
                        // txtReceiverContactName.Enabled=false;
                        txtSuburb.Enabled = false;
                        txtDeliveryInstructions.Enabled = false;
                        txtShipPhoneNumber.Enabled = false;
                        txtDeliveryInstructions.Enabled = false;

                        //ImageButton4.Visible = false;
                        ImageButton1.Visible = false;
                       // ImageButton5.Visible = false;
                        //TextBox1.Enabled = false;
                        TextBox1.ReadOnly = true;
                        ImageButton2.Visible = false;
                        //Response.Redirect("Confirm.aspx?OrdId=99999&ViewType=Confirm");
                        //if (oOrdBillInfo.BillCountry.ToLower().Trim() == "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() == "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() == "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                        if (objOrderServices.IsNativeCountry(OrderID) == 1)
                        {  
                            //if (DefaultPay=="PPAPI")
                            //    Response.Redirect("ppapi.aspx?" + EncryptSP(OrderID.ToString()), false); 
                            //else if (DefaultPay=="PP")
                            //    Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false); 
                            //else
                            //    Response.Redirect("PaySP.aspx?" + EncryptSP(OrderID.ToString()), false);
                            SecurePayService objSecurePayService = new SecurePayService();
                            if (objSecurePayService.CheckSecurePay()==true) 
                                Response.Redirect("checkout.aspx?" + EncryptSP(OrderID.ToString() +"#####"+"PaySP"), false);
                            else
                                Response.Redirect("checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay"), false);
                      
                        }
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
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
    protected void ImgBtnEditShipping_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
        {
            if (OrderID > 0)
            {
                Response.Redirect("/orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + OrderID.ToString(), false);
            }
            else
            {
                Response.Redirect("/orderDetails.aspx?bulkorder=1&amp;Pid=0", false);
            }
        }

        //   Response.Redirect("Shipping.aspx?shipping.aspx?OrderID="+ OrderID +"&ApproveOrder=Approve", false);           
    }
    protected string DecryptSP(string ordid)
    {
        string enc = "";

        enc = HttpUtility.UrlDecode(ordid);
        //objErrorHandler.CreateLog("AfterUrlDecode" + enc);
        //objErrorHandler.CreateLog("EnDekey" + EnDekey);
      
        enc = objSecurity.StringDeCrypt(enc, EnDekey);
       // objErrorHandler.CreateLog("StringDeCrypt1" + enc );
        enc = objSecurity.StringDeCrypt(enc, EnDekey);
      //  objErrorHandler.CreateLog("StringDeCrypt2" + enc);
        enc = objSecurity.StringDeCrypt(enc, EnDekey);
      //  objErrorHandler.CreateLog("StringDeCrypt3" + enc);
        enc = objSecurity.StringDeCrypt(enc, EnDekey);
      //  objErrorHandler.CreateLog("StringDeCrypt4" + enc);
        enc = objSecurity.StringDeCrypt(enc, EnDekey);
       // objErrorHandler.CreateLog("StringDeCrypt5" + enc);
        return enc;
    }
    protected void ProceedFunction()
    {
        try
        {
            //color.BgColor = "FFFFFF";
            //  color1.BgColor = "FFFFFF";
            //colo2.BgColor = "FFFFFF";
            bool isau = false;
            // color5.BgColor = "FFFFFF";
            QuoteServices objQuoteServices = new QuoteServices();
            //int OrdStatusID = (int)OrderServices.OrderStatus.ORDERPLACED;
            //if (objOrderServices.GetOrderStatus(OrderID) != "ORDERPLACED")
            if (objOrderServices.GetOrderStatus(OrderID) != "")
            {
                int OrdStatusVerify = (int)OrderServices.OrderStatus.MANUALPROCESS;
                DataSet oDs = new DataSet();
                oDs = objOrderServices.GetOrderItems(OrderID);
                int ChkOrderExist = 0;
                int UptOrderStatus = -1;
                //int OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                int OrdStatus = 0;

                //switch (Convert.ToInt16(Session["USER_ROLE"]))
                //{
                //    case 1:
                //        OrdStatus = (int)OrderServices.OrderStatus.OPEN ;
                //        break;
                //    case 2:
                //        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                //        break;
                //    case 3:
                //        OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                //        break;
                //}

                int _UserrID;
                _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);



                //if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                if (objOrderServices.IsNativeCountry(OrderID) == 0)
                {
                    isau = false;
                    OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                }
                else
                {
                    isau = true;
                    OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                }


                oPayInfo = objPaymentServices.GetPayment(OrderID);
                if (oPayInfo.OrderID == OrderID && (oPayInfo.PaymentType == PaymentServices.PaymentType.CCPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CCPaymentDeclined || oPayInfo.PaymentType == PaymentServices.PaymentType.CHEPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CODPayment))
                {
                    ChkOrderExist = 1;
                }
                //if (Session["PAYMENTINFO"] != null || Session["PAYMENTINFO"].ToString() != null)
                {
                    Session["PAYMENT_TYPE"] = PaymentServices.PaymentType.CODPayment;
                    decimal TotCost = objHelperServices.CDEC(objOrderServices.GetOrderTotalCost(OrderID));
                    oPayInfo.PayResponse = "";
                    oPayInfo.PaymentType = PaymentServices.PaymentType.CODPayment;
                    oPayInfo.OrderID = OrderID;
                    oPayInfo.PONumber = objHelperServices.Prepare("");
                    oPayInfo.PORelease = refid;
                    oPayInfo.Amount = TotCost;
                    oPayInfo.UserId = OrderID;

                }
                if (objUserServices.GetUserStatus(objHelperServices.CI(Session["USER_ID"].ToString())) == 1)
                {
                    if (ChkOrderExist == 0)
                    {
                        ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                        UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                        int cStatus = 0;
                        //cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                        if (isau ==false)
                            cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                    }
                    else if (ChkOrderExist == 1)
                    {
                        ChkOrderExist = objPaymentServices.UpdatePayment(oPayInfo);
                        UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                        int cStatus = 0;
                        //cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                        if (isau == false)
                            cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");

                    }
                    if (UptOrderStatus != -1)
                    {
                        int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                        objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        //SendNotification(OrderID);
                        if(isau==false)
                            SendMail(OrderID, OrdStatus,isau);

                        if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                        {
                            Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                        }
                        else
                        {
                            //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                        }
                    }

                }
                else if (objUserServices.GetUserStatus(objHelperServices.CI(Session["USER_ID"].ToString())) == 4)
                {
                    if (Session["PAYMENTINFO"] != null)
                    {
                        oPayInfo = (PaymentServices.PayInfo)Session["PAYMENTINFO"];
                    }
                    if (ChkOrderExist == 0)
                    {
                        ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                        UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatusVerify);
                    }
                    else if (ChkOrderExist == 1)
                    {
                        ChkOrderExist = objPaymentServices.UpdatePayment(oPayInfo);
                        UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatusVerify);
                    }
                    if (UptOrderStatus != -1)
                    {
                        int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                        objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                        {
                            Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                        }
                        else
                        {
                            //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                        }
                    }

                }
            }


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }



    private void SendMail(int OrderId, int OrderStatus, bool isau)
    {
        string toemail ="";
        try
        {

            if (isau == true)
                return;

            string BillAdd;
            string ShippAdd;
            string stemplatepath;
            
            DataSet dsOItem = new DataSet();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

            oPayInfo = objPaymentServices.GetPayment(OrderId);
            oOrderInfo = objOrderServices.GetOrder(OrderId);

            int UserID = objHelperServices.CI(Session["USER_ID"].ToString());

            //oUserInfo = objUserServices.GetUserInfo(UserID);
            oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            dsOItem = objOrderServices.GetOrderItems(OrderID);
            BillAdd = GetBillingAddress(OrderID);
            ShippAdd = GetShippingAddress(OrderID);

            string ShippingMethod = oOrderInfo.ShipMethod;
            string CustomerOrderNo = oPayInfo.PORelease;
            string shippingnotes = TextBox1.Text.Trim();




            oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
            string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
            string Emailadd = oUserInfo.AlternateEmail;
            toemail = oUserInfo.AlternateEmail;

            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string PendingorderURL = string.Format("http://" + url + "/PendingOrder.aspx");

            int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
            oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
            string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

            string SubmittedBy = "";
            switch (oOrderInfo.OrderStatus)
            {
                case 6:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
                case 12:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
                default:
                    SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                    break;
            }


            string sHTML = "";
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                TBWDataList1[] lstrows1 = new TBWDataList1[0];

                stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;

                DataSet dscat = new DataSet();
                DataTable dt = null;
                _stg_records = new StringTemplateGroup("row", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);


                lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];



                int ictrecords = 0;

                foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                {

                    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");
                    _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                    _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }

                if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                else
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");


                _stmpl_container.SetAttribute("OrderDate", Createdon);
                _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                _stmpl_container.SetAttribute("CreatedBy", Createdby);
                if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                else
                    _stmpl_container.SetAttribute("SubmittedBy", "");



                _stmpl_container.SetAttribute("ShippingMethod", ShippingMethod);
                _stmpl_container.SetAttribute("BillingAddress", BillAdd);
                _stmpl_container.SetAttribute("ShippingAddress", ShippAdd);
                _stmpl_container.SetAttribute("shippingnotes", shippingnotes);

                if (shippingnotes != "")
                    _stmpl_container.SetAttribute("TBT_shippingnotes", true);
                else
                    _stmpl_container.SetAttribute("TBT_shippingnotes", false);

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]),1);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
                
            }
            if (sHTML != "")
            {
                //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                //System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

                //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                //MessageObj.To.Add(Emailadd.ToString());
                ////MessageObj.To.Add("jtechalert@gmail.com");
                ////MessageObj.To.Add("mohanarangam.e.r@jtechindia.com");
                //MessageObj.Subject = "Pending Order - WES Australasia";
                //MessageObj.IsBodyHtml = true;
                //MessageObj.Body = sHTML;
                //System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                ////System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                //smtpclient.UseDefaultCredentials = false;
                //smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                ////smtpclient.Port = 587;
                ////smtpclient.Credentials = new System.Net.NetworkCredential("jtechalert@gmail.com", "jtech@#$123");
                //smtpclient.Send(MessageObj);

                //objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                ////ArrayList CCList = new ArrayList();
                ////CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                ////objNotificationServices.NotifyCC = CCList;
                //objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                //objNotificationServices.NotifyTo.Add(Emailadd.ToString());

                string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                //EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
                //objNotificationServices.NotifySubject = EmailSubject;
                //objNotificationServices.NotifyMessage = sHTML;
                //objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                //objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                //objNotificationServices.NotifyIsHTML = true;
                //objNotificationServices.SendMessage();


                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                string emails = "";
                string Adminemails = "";
                string webadminmail = "";
                webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                //MailAddress from = new MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                //MailAddress to = new MailAddress("palanikumarmca@gmail.com");
                //MailMessage message = new MailMessage(from, to);
                //message.Subject = "Using the SmtpClient class.";
                //message.Body = "this is test";
                //MailAddress bcc1 = new MailAddress("palanikumar@jtechindia.com");
                //message.Bcc.Add(bcc1);
                //SmtpClient client = new SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());   
                //client.Send(message);






                if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                {
                     MessageObj.To.Add(Emailadd.ToString());
                     MessageObj.Bcc.Add(webadminmail);

                    //if (isau == false)
                    //{
                    //    if (System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"].ToString() == "9200")
                    //        Adminemails = System.Configuration.ConfigurationManager.AppSettings["ToMail"].ToString();
                    //    else
                    //        Adminemails = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                    //}

                    // Get_ADMIN_UserEmils();
                    //if (ApprovedUserEmailadd.Trim() != "" && Emailadd.ToString() != ApprovedUserEmailadd.ToString())
                    //   MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                }
                else
                {
                    emails = Get_ADMIN_APPROVED_UserEmils();

                    MessageObj.To.Add(Emailadd.ToString());
                   // MessageObj.Bcc.Add(addressBCC);

                }

                //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                {
                    MessageObj.Subject = "Bigtop Order Confirmation - Order No : " + CustomerOrderNo.ToString();
                }
                else
                {
                    MessageObj.Subject = "Bigtop Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                }

                MessageObj.IsBodyHtml = true;
                MessageObj.Body = sHTML;


                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);




                if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                {
                    if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                    {
                        //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                        MessageObj.To.Clear();
                        MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                        smtpclient.Send(MessageObj);
                    }
                    if (Adminemails != "")
                    {

                        string[] emailid = Adminemails.ToString().Split(',');
                        if (emailid.Length > 0)
                        {
                            foreach (string id in emailid)
                            {
                                if (ApprovedUserEmailadd.ToUpper().ToString() != id.ToUpper().ToString() && Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                {
                                    //MessageObj.CC.Add(id.ToString());
                                    MessageObj.Subject = "Bigtop International Order Alert - Order No : " + CustomerOrderNo.ToString();
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(id.ToString());
                                    smtpclient.Send(MessageObj);
                                }
                            }
                        }
                        else
                        {
                            if (ApprovedUserEmailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString() && Emailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString())
                            {
                                MessageObj.To.Clear();
                                MessageObj.To.Add(Adminemails.ToString());
                                smtpclient.Send(MessageObj);
                            }
                            //MessageObj.CC.Add(emails.ToString());
                        }

                    }
                }
                else
                {
                    if (emails != "")
                    {

                        string[] emailid = emails.ToString().Split(',');
                        if (emailid.Length > 0)
                        {
                            foreach (string id in emailid)
                            {
                                if (Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                {
                                    //MessageObj.CC.Add(id.ToString());
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(id.ToString());
                                    smtpclient.Send(MessageObj);
                                }
                            }
                        }
                        else
                        {
                            if (Emailadd.ToUpper().ToString() != emails.ToUpper().ToString())
                            {
                                MessageObj.To.Clear();
                                MessageObj.To.Add(emails.ToString());
                                smtpclient.Send(MessageObj);
                                //MessageObj.CC.Add(emails.ToString());
                            }
                        }

                    }


                }


            }
        }
        catch (Exception ex)
        {
            objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]),1);
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            

        }
    }
    public void SendNotification(int OrderID)
    {
        // objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        if (objNotificationServices.IsNotificationActive(NotificationVariablesServices.NotificationList.NEWORDER.ToString()))
        {
            DataSet dsOrder = objNotificationServices.BuildNotifyInfo();
            OrderServices objOrderServices = new OrderServices();
            string sTemplate = "";
            string sEmailMessage = "";
            string sUser = "";
            sUser = objUserServices.GetUserEmailAdd(objHelperServices.CI(Session["USER_ID"]));
            decimal Tax = objOrderServices.GetTaxAmount(OrderID);
            decimal SCost = objOrderServices.GetShippingCost(OrderID);
            decimal Total = objOrderServices.GetOrderTotalCost(OrderID);
            string currency = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
            try
            {
                DataRow oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.FROMCONTENT.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objHelperServices.GetOptionValues("COMPANY ADDRESS").ToString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.TOCONTENT.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = GetShippingAddress(OrderID);
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.FIRSTNAME.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objUserServices.UserFirstName(OrderID);
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.ORDERDATE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = System.DateTime.Now.ToLongDateString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.ORDERID.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = refid;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.CONSTRUCTTABLE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = ConstructOrderDetails(OrderID);
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.SUBTOTAL.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + SubTotal;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.TAX.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + Tax;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.SHIPCHARGES + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + SCost;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.TOTAL.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + Total;
                dsOrder.Tables[0].Rows.Add(oRow);

                sTemplate = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                sEmailMessage = objNotificationServices.ParseTemplateMessage(sTemplate, dsOrder);


                objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                ArrayList CCList = new ArrayList();
                CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                objNotificationServices.NotifyCC = CCList;
                objNotificationServices.NotifyTo.Add(sUser);
                objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
                objNotificationServices.NotifySubject = EmailSubject;
                objNotificationServices.NotifyMessage = sEmailMessage;
                objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                objNotificationServices.SendMessage();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    }

    public string ConstructOrderDetails(int OrderID)
    {
        int Qty = 0;
        double Price = 0.0;
        string CatalogItemNo = "";
        string sOrderDetails = "";
        string description = "";
        DataSet dsOD = new DataSet();
        dsOD = objOrderServices.GetOrderDetails(OrderID);
        string currency = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
        string oRowHead = "<TABLE><TR><TD ALIGN=CENTER WIDTH=60>Qty</TD><TD ALIGN=CENTER WIDTH=350>Item#</TD><TD ALIGN=LEFT WIDTH=400>Description</TD><TD ALIGN=CENTER WIDTH=100>Price</TD></TR>";
        foreach (DataRow row in dsOD.Tables[0].Rows)
        {
            CatalogItemNo = row["CATALOG_ITEM_NO"].ToString();
            Qty = objHelperServices.CI(row["QTY"]);
            Price = objHelperServices.CD(row["PRICE_EXT_APPLIED"]) * Qty;
            description = row["Description"].ToString().Replace("<ars>g</ars>", "&rarr;");
            SubTotal = SubTotal + Price;
            sOrderDetails = sOrderDetails + @"<TR><TD width=60 align=Center><FONT FACE=TAHOMA SIZE=2>" + Qty.ToString() + "</FONT></TD><TD width=350 align=LEFT><FONT FACE=TAHOMA SIZE=2>" + CatalogItemNo + "</FONT></TD><TD width=400 align=Center><FONT FACE=TAHOMA SIZE=2>" + description + "</TD><TD width=100 align=right><FONT FACE=TAHOMA SIZE=2>" + currency + Price.ToString("#,#0.00") + "</FONT></TD></TR>";
        }
        sOrderDetails = oRowHead + sOrderDetails + "</TABLE>";
        return sOrderDetails;
    }

    public string GetShippingAddress(int OrderID)
    {
        string sShippingAddress = "";

        try
        {
            OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
            oOI = objOrderServices.GetOrder(OrderID);

            if (oOI.ShipCompName.Trim().Length > 0)
                sShippingAddress = oOI.ShipCompName + "<BR>";
            else
                sShippingAddress = "";

            //sShippingAddress = sShippingAddress + oOI.ShipFName + oOI.ShipLName + "<BR>";


            if (oOI.ShipCompName.Trim().Length > 0)
            {
                if (oOI.ShipCompName.ToLower().Contains(oOI.ShipFName.ToLower()) == false)
                {
                    sShippingAddress = sShippingAddress + oOI.ShipFName + "<BR>";
                }
            }
            else
            {
                sShippingAddress = sShippingAddress + oOI.ShipFName + "<BR>";
            }

            if (oOI.ShipAdd1.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + oOI.ShipAdd1.Trim() + "<BR>";
            }
            if (oOI.ShipAdd2.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + oOI.ShipAdd2.Trim() + "<BR>";
            }
            if (oOI.ShipAdd3.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + oOI.ShipAdd3.Trim() + "<BR>";
            }
            if (oOI.ShipCity.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipCity + "<BR>";
            if (oOI.ShipState.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipState + "<BR>";
            if (oOI.ShipZip.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipZip + "<BR>";
            if (oOI.ShipCountry.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipCountry + "<BR>";
            //if (oOI.ReceiverContact.Trim().Length > 0)
            //{
            //    sShippingAddress = sShippingAddress + "<BR>" + oOI.ReceiverContact + "<BR>";
            //}
            sShippingAddress = sShippingAddress + oOI.ShipPhone + "<BR>";
            if (oOI.DeliveryInstr.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + "<BR>" + oOI.DeliveryInstr + "<BR>";
            }
            }
            catch (Exception ex) {

                objErrorHandler.CreateLog(ex.ToString());
            }
        return sShippingAddress;
    }

    public string GetBillingAddress(int OrderID)
    {
        string sBillingAddress = "";

        try
        {
            OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
            oBI = objOrderServices.GetOrder(OrderID);
            // if (oBI.BillcompanyName.Trim().Length > 0)
            //     sBillingAddress = oBI.BillcompanyName + "<BR>";
            // else
            //     sBillingAddress = "";

            //sBillingAddress = sBillingAddress + oBI.BillFName + oBI.BillLName + "<BR>";

            if (oBI.BillcompanyName.Trim().Length > 0)
            {
                sBillingAddress = oBI.BillcompanyName + "<BR>";
                if ((oBI.Bill_Name.Trim().Length > 0) && (oBI.BillcompanyName.ToLower().Contains(oBI.Bill_Name.ToLower()) == false))
                {

                    sBillingAddress = sBillingAddress + oBI.Bill_Name + "<BR>";
                }

                else if (oBI.BillcompanyName.ToLower().Contains(oBI.BillFName.ToLower()) == false)
                {
                    sBillingAddress = sBillingAddress + oBI.BillFName + " " + oBI.BillLName + "<BR>";
                }

            }
            else
            {
                sBillingAddress = "";
                if ((oBI.Bill_Name.Trim().Length > 0))
                {

                    sBillingAddress = sBillingAddress + oBI.Bill_Name + "<BR>";
                }

                else
                {
                    sBillingAddress = sBillingAddress + oBI.BillFName + " " + oBI.BillLName + "<BR>";
                }
            }



            if (oBI.BillAdd1.Trim().Length > 0)
            {
                sBillingAddress = sBillingAddress + oBI.BillAdd1.Trim() + "<BR>";
            }
            if (oBI.BillAdd2.Trim().Length > 0)
            {
                sBillingAddress = sBillingAddress + oBI.BillAdd2.Trim() + "<BR>";
            }
            if (oBI.BillAdd3.Trim().Length > 0)
            {
                sBillingAddress = sBillingAddress + oBI.BillAdd3.Trim() + "<BR>";
            }
            if (oBI.BillCity.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillCity + "<BR>";
            if (oBI.BillState.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillState + "<BR>";
            if (oBI.BillZip.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillZip + "<BR>";
            if (oBI.BillCountry.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillCountry + "<BR>";

            sBillingAddress = sBillingAddress + oBI.BillPhone;

            sBillingAddress = sBillingAddress + "<BR><p style='color:white'>...</p>";


      
           }
            catch (Exception ex) {

                objErrorHandler.CreateLog(ex.ToString());
            }
        return sBillingAddress;
    }
    private string Setdrpdownlistvalue(DropDownList d, string val)
    {
        ListItem li;
        string returnselected = "";
        for (int i = 0; i < d.Items.Count; i++)
        {
            li = d.Items[i];
            if (li.Text.ToUpper() == val.ToUpper())
            {
                d.SelectedIndex = i;
                returnselected = li.Text.ToUpper();
                break;
            }
        }
        return returnselected;
    }
    protected void btnForgotPassword_Click(object sender, EventArgs e)
    {
      //  this.modalPop.Hide();
        PopupMsg.Visible = false;
        string MicroSiteTemplate =  Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
        ConnectionDB objConnectionDB = new ConnectionDB();
        TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);
        DataTable ttbl = tbwtMSEngine.GetSupplierDetail();
        string micrositeurl = "";
        if (ttbl != null && ttbl.Rows.Count > 0)
        {
         string   strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
          string  strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
          string  strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();
            micrositeurl = objHelperServices.SimpleURL_MS_Str(strsupplierName, "mct.aspx",true) + "/mct/";
        }
        Response.Redirect(micrositeurl);
    }

    protected void btnCancelerroritems_Click(object sender, EventArgs e)
    {

        this.modalpop_itemerror.Hide();
        Response.Redirect("orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + Session["ORDER_ID"], true);
    }
    protected void loginregrdir_Click(object sender, EventArgs e)
    {
        this.modalPop_loreg.Hide();
        Response.Redirect("/MLogin.aspx");
    }
    private bool Checkdrpdownlistvalue(DropDownList d, string val)
    {
        ListItem li;
        bool blnreturn = false;
        for (int i = 0; i < d.Items.Count; i++)
        {
            li = d.Items[i];
            if (li.Text.ToUpper() == val.ToUpper())
            {

                blnreturn = true;
                break;
            }
        }
        return blnreturn;
    }
    //[WebMethod]
    //public static bool GetDropShipmentKeyExists(string strvalue,string type)
    //{
    //    OrderServices objOrderServices1 =new OrderServices();

    //    bool retval = false;
    //    retval=objOrderServices1.GetDropShipmentKeyExist(strvalue, type);
    //    return retval;
    //}
    protected void btnPayPalPayLink_Click(object sender, EventArgs e)
    {
        //Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false);
        ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"/images/paypal_check.png";
       // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
        ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"/images/master_uncheck.png";
        btnPayApi.Visible =false;
        btnPay.Visible =true;
        PaySPDiv.Visible = false;
        PaypalApiDiv.Visible = true;
    }
    protected void btnPayPalAPIPayLink_Click(object sender, EventArgs e)
    {
        //Response.Redirect("ppapi.aspx?" + EncryptSP(OrderID.ToString()), false);
        ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"/images/pay2ch.png";
       // ImagePayApi.ImageUrl = "/images/express_Checkout_select.png";
        ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"/images/pay1uch.png";
        btnPayApi.Visible =true;
        btnPay.Visible = false;
        PaySPDiv.Visible = false;
        PaypalApiDiv.Visible = true;
    }
    protected void btnSecurePayLink_Click(object sender, EventArgs e)
    {
        ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"/images/paypal_uncheck.png";
     //   ImagePayApi.ImageUrl = "/images/express_Checkout.png";
        ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+"/images/master_check.png";
       // Response.Redirect("paysp.aspx?" + EncryptSP(OrderID.ToString()), false);
        btnPayApi.Visible = false;
        btnPay.Visible = false;
        PaySPDiv.Visible = true;
        PaypalApiDiv.Visible =false;
    }
    protected void btnSecurePay_Click(object sender, EventArgs e)
    {


       

        string rtnstr = "";
        try
        {
            txtCardNumber.Style.Remove("border");
            drpExpmonth.Style.Remove("border");
            drpExpyear.Style.Remove("border");
            txtCardCVVNumber.Style.Remove("border");
            //drppaymentmethod.Style.Remove("border");
        }
        catch
        {
        }

        SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
      
        try
        {

            GetParams();
          

            if (Session["XpayMS"] != null)
            {
                if (Convert.ToInt32(Session["XpayMS"]) > 3)
                {
                   // div1.InnerHtml = "";
                    div2.InnerHtml = "More than 3 attempt. try again" + "<br/>"+"<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                    div2.Visible = true;
                    return;
                }
                else
                    Session["XpayMS"] = Convert.ToInt32(Session["XpayMS"]) + 1;
            }
            else
                Session["XpayMS"] = 0;

            oPayInfo = objPaymentServices.GetPayment(OrderID);
            PaymentID = oPayInfo.PaymentID;

            if (PaymentID == 0)
                Response.Redirect("orderDetails.aspx?bulkorder=1&Pid=0&ORDER_ID=" + OrderID);

            if (oPayInfo.PayResponse.ToLower() == "yes")
            {
                divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                return;
            }

            //objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, drppaymentmethod.SelectedValue, txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
            objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);

            if (objPRInfo.Error_Text != "")
            {
                btnSP.Style.Add("display", "block");
                BtnProgressSP.Style.Add("display", "none");
               // ImgBtnEditShipping.Style.Add("display", "block");

                div2.InnerHtml = " Error found in details you have entered. Please check all fields for errors and try again."; //objPRInfo.Error_Text;
                div2.Visible = true;
                HttpContext.Current.Session["payflowresponse"] = "FAIL";

                if (objPRInfo.Error_Text.ToLower().Contains("card number") == true)
                    txtCardNumber.Style.Add("border", "1px solid #FF0000");

                if (objPRInfo.Error_Text.ToLower().Contains("cvv") == true || objPRInfo.Error_Text.ToLower().Contains("do not honour") == true)
                    txtCardCVVNumber.Style.Add("border", "1px solid #FF0000");

                if (objPRInfo.Error_Text.ToLower().Contains("date") == true || objPRInfo.Error_Text.ToLower().Contains("expired") == true)
                {
                    drpExpmonth.Style.Add("border", "1px solid #FF0000");
                    drpExpyear.Style.Add("border", "1px solid #FF0000");
                }
                if (objPRInfo.Error_Text.ToLower().Contains("card type") == true)
                {
                    //drppaymentmethod.Style.Add("border", "1px solid #FF0000");                       
                }
                HttpContext.Current.Session["paySPresponse"] = "";
            }
            else
            {
                //Session["Pay"] = "End";
                Session["XpayMS"] = null;
               // div1.InnerHtml = "";
                //div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX " + OrderID.ToString() + " Payment succeeded" + strBackLink;
                //div2.InnerHtml = "";
               // div2.Visible = false;
                HttpContext.Current.Session["paySPresponse"] = "SUCCESS";                
                HttpContext.Current.Session["Mchkout"] = EncryptSP(OrderID.ToString() + "#####" + "PaySP"+ "#####" + "Paid") ;
                Response.Redirect("checkout.aspx?key="+ EncryptSP(OrderID.ToString() + "#####" + "PaySP"+ "#####" + "Paid" ));
            }



        }
        catch (Exception ex)
        {
        }

      

    }
   
    private string GetIP()
    {

        var ip = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
          && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
         ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
         : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        if (ip.Contains(","))
            ip = ip.Split(',').First();
        return ip.Trim();

        //string ip = null;
        //if (HttpContext.Current != null)
        //{ // ASP.NET
        //    ip = string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
        //        ? HttpContext.Current.Request.UserHostAddress
        //        : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //}
        //if (string.IsNullOrEmpty(ip) || ip.Trim() == "::1")
        //{ // still can't decide or is LAN
        //    var lan = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(r => r.AddressFamily == AddressFamily.InterNetwork);
        //    ip = lan == null ? string.Empty : lan.ToString();
        //}
        //return ip;




        //IPHostEntry host;
        //string localIp = "?";
        //string hostName = Dns.GetHostName();
        //host = Dns.GetHostEntry(hostName);
        //foreach (IPAddress ip in host.AddressList)
        //{
        //    if (ip.AddressFamily.ToString() == "InterNetwork")
        //    {
        //        localIp = ip.ToString();
        //    }
        //    //localIp += " " + ip.AddressFamily.ToString() + " ";
        //}
        //return localIp;

       
        //string externalIP = "";
        //externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
        //externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalIP)[0].ToString();
        //return externalIP;

        //string Str = "";
        //Str = System.Net.Dns.GetHostName();
        //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Str);
        //IPAddress[] addr = ipEntry.AddressList;
        //return addr[addr.Length - 1].ToString();

    }
    protected void btnPayApi_Click(object sender, EventArgs e)
    {
        objErrorHandler.CreatePayLog("btnPayApi_Click start Orderid=" + OrderID);

        GetParams();
        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();


        renUrl = renUrl.Replace("MCheckOut", "MCheckOut");
        renUrl = renUrl + "?key=" + EncryptSP("Paid");


        oOrderInfo = objOrderServices.GetOrder(OrderID);
        oUserInfo = objUserServices.GetUserInfo(Userid);

        //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
        //{
        //    div1.InnerHtml = "";
        //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
        //    return;
        //}
        if (oPayInfo.PayResponse.ToLower() == "yes")
        {
            divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
            return;
        }

        string Requeststr = objPayPalApiService.PayPalSetECRequest(OrderID, PaymentID, oOrderInfo, renUrl);

        if (Requeststr.Contains("Form") == false)
            divContent.InnerHtml = Requeststr;
        else
            this.Page.Controls.Add(new LiteralControl(Requeststr));

        objErrorHandler.CreatePayLog("btnPayApi_Click end Orderid=" + OrderID);

    }
    protected void btnPay_Click(object sender, EventArgs e)
    {
        objErrorHandler.CreatePayLog("btnPay_Click start Orderid=" + OrderID);

        GetParams();
        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();


        renUrl = renUrl.Replace("MCheckOut", "MCheckOut");
        renUrl = renUrl + "?key=" + EncryptSP("Paid");
        objErrorHandler.CreateLog("Checkout" + renUrl);
        oOrderInfo = objOrderServices.GetOrder(OrderID);
        oUserInfo = objUserServices.GetUserInfo(Userid); 
        oPayInfo = objPaymentServices.GetPayment(OrderID);
         PaymentID = oPayInfo.PaymentID;
        //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
        //{
        //    div1.InnerHtml = "";
        //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
        //    return;
        //}
        if (oPayInfo.PayResponse.ToLower() == "yes")
        {
            divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
            return;
        }

        string Requeststr = objPayPalService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);

        if (Requeststr.Contains("Form") == false)
            divContent.InnerHtml = Requeststr;
        else
            this.Page.Controls.Add(new LiteralControl(Requeststr));

        btnPay.Visible = false;
        BtnProgress.Visible = true;

        objErrorHandler.CreatePayLog("btnPay_Click End Orderid=" + OrderID);
    }
    protected void Page_PreInit()
    {
        if (httpRequestVariables()["tx"] != null)
        {
            //Page.MasterPageFile = "Blank.Master";
        }
        if (httpRequestVariables()["Token"] != null && httpRequestVariables()["PayerID"] != null)
        {
            //Page.MasterPageFile = "Blank.Master";
        }
        GenerateClientToken();
    }
    protected NameValueCollection httpRequestVariables()
    {
        var post = Request.Form;       // $_POST
        var get = Request.QueryString; // $_GET
        return Merge(post, get);
    }
    public static NameValueCollection Merge(NameValueCollection first, NameValueCollection second)
    {
        if (first == null && second == null)
            return null;
        else if (first != null && second == null)
            return new NameValueCollection(first);
        else if (first == null && second != null)
            return new NameValueCollection(second);

        NameValueCollection result = new NameValueCollection(first);
        for (int i = 0; i < second.Count; i++)
            result.Set(second.GetKey(i), second.Get(i));
        return result;
    }
    #region "Function.."
    public string BuildBillAddress(OrderServices.OrderInfo oOrderInfo)
    {
        try
        {
            string sBillAdd = "";
            //Added by indu as per client requirment dated on 18 -July -2017
            if (oOrderInfo.BillFName != null)
            {
                sBillAdd = WrapText(oOrderInfo.BillFName) + " ";
            }
            if (oOrderInfo.BillMName != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillMName) + " ";
            }
            if (oOrderInfo.BillLName != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillLName) + "<br>";
            }
            if (oOrderInfo.BillAdd1 != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd1) + "<br>";
            }
            if (oOrderInfo.BillAdd2 != "")
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2) + "<br>";
            }
            else
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2);
            if (oOrderInfo.BillAdd3 != "")
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd3) + "<br>";
            }
            else
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd3);
            }
            if (oOrderInfo.BillCity != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillCity) + "<br>";
            }
            if (oOrderInfo.BillState != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillState) + "-";
            }
            if (oOrderInfo.BillZip != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillZip) + "<br>";
            }
            if (oOrderInfo.BillCountry != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillCountry) + "<br>";
            }
            if (oOrderInfo.BillPhone != null)
            {
                sBillAdd = sBillAdd + "Phone No:" + WrapText(oOrderInfo.BillPhone) + "<br>";
            }
            return sBillAdd;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }
    }
    public string BuildShippAddress(OrderServices.OrderInfo oOrderInfo)
    {
        try
        {
            string sShippAdd = "";

            if (oOrderInfo.ShipFName != null)
            {
                sShippAdd = WrapText(oOrderInfo.ShipFName) + " ";
            }
            if (oOrderInfo.ShipMName != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipMName) + " ";
            }
            if (oOrderInfo.ShipLName != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipLName) + "<br>";
            }
            if (oOrderInfo.ShipAdd1 != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd1) + "<br>";
            }
            if (oOrderInfo.ShipAdd2 != "")
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2) + "<br>";
            }
            else
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2);
            if (oOrderInfo.ShipAdd3 != "")
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd3) + "<br>";
            }
            else
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd3);
            if (oOrderInfo.ShipCity != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipCity) + "<br>";
            }
            if (oOrderInfo.ShipState != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipState) + "-";
            }
            if (oOrderInfo.ShipZip != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipZip) + "<br>";
            }
            if (oOrderInfo.ShipCountry != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipCountry) + "<br>";
            }
            if (oOrderInfo.ShipPhone != null)
            {
                sShippAdd = sShippAdd + "Phone No:" + WrapText(oOrderInfo.ShipPhone) + "<br>";
            }
            return sShippAdd;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }
    }
    public string WrapText(string BillAdd)
    {
        string newline = " \n ";
        if (BillAdd.Length > 50 & BillAdd.Length <= 100)
            BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51) + newline;
        else if (BillAdd.Length > 100 & BillAdd.Length <= 150)
            BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51, 49) + newline + BillAdd.Substring(101);
        return BillAdd;
    }
    [System.Web.Services.WebMethod]
    public static string SaleTrans(string nounce, string Amount)
    {
        try
        {

            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["Pay_ORDER_ID"] != null)
            {
                string x = "";
                //var gateway = new BraintreeGateway
                //{
                //    Environment = Braintree.Environment.SANDBOX,
                //    MerchantId = "mjff7p7mgb4qmp77",
                //    PublicKey = "p78kxf6s7zhb8z8x",
                //    PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
                //};
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.PRODUCTION,
                    MerchantId = "wrv3fq8x3r269ycd",
                    PublicKey = "nm7v4wm8dmw7b6rq",
                    PrivateKey = "a3d333f589d80552db255c34c1407c40"
                };

                SecurePayService objSecurePayService = new SecurePayService();
                SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
                PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                PaymentServices objPaymentServices = new PaymentServices();


                string OrderID = HttpContext.Current.Session["Pay_ORDER_ID"].ToString();
                int intOrderID = Convert.ToInt32(OrderID);

                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                OrderServices objOrderServices = new OrderServices();
                oOrderInfo = objOrderServices.GetOrder(intOrderID);



            

                if (oOrderInfo.OrderStatus == 18)

                {
                    oPayInfo = objPaymentServices.GetPayment(intOrderID);

                    int PaymentID = oPayInfo.PaymentID;

                    if (nounce == "no")
                    {
                        x = "Error " + "Please Try again or use a different card / payment method.";
                         objSecurePayService.call_GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", "No Nounce", "", "No", "", "No Nounce", "", "", "Error Processing PARes");
                      //  objerrhandler.CreateLogEA(" br  Orderid=" + OrderID + "Error Processing PARes");

                        return x;
                    }


                    var request = new TransactionRequest
                    {
                        Amount = Convert.ToDecimal(Amount),
                        MerchantAccountId = "wagnerelectronicsAUD",
                        PaymentMethodNonce = nounce,

                        Options = new TransactionOptionsRequest
                        {
                            SubmitForSettlement = true
                        },
                        BillingAddress = new PaymentMethodAddressRequest
                        {
                            PostalCode = oPayInfo.Zip,
                            FirstName = oPayInfo.BillFName,
                            LastName = oPayInfo.BillLName,
                            StreetAddress = oPayInfo.Address1,
                            Locality = oPayInfo.City,
                            Company = oPayInfo.Country

                        },
                        OrderId = OrderID,

                    };

                   // 

                    PaymentMethodNonce paymentMethodNonce = gateway.PaymentMethodNonce.Find(nounce);
                    ThreeDSecureInfo info = paymentMethodNonce.ThreeDSecureInfo;
                    string Enrolled = "";
                    string Status = "";
                    bool? LiabilityShifted = null;
                    bool? LiabilityShiftPossible = null;
                    string TRANSACTIONID = "";
                    if (info != null)
                    {
                        Enrolled = info.Enrolled;
                        Status = info.Status;
                        LiabilityShifted = info.LiabilityShifted;
                        LiabilityShiftPossible = info.LiabilityShiftPossible;
                        TRANSACTIONID = info.DsTransactionId;
                    }
                    if (LiabilityShifted == true && (Status == "authenticate_successful" || Status == "authenticate_attempt_successful"))
                    {

                        Result<Transaction> result = gateway.Transaction.Sale(request);



                        if (result.IsSuccess() == true)
                        {


                            Transaction transaction = result.Target;
                            string ResponseId = transaction.Id;
                            string ResponseText = transaction.ProcessorResponseText;
                            string Responsecode = transaction.ProcessorResponseCode;

                            string cardtype = result.Target.CreditCard.CardType.GetType().ToString();
                            objSecurePayService.call_GetPaymentRequest_braintree(intOrderID, PaymentID, result.Target.CreditCard.CardType.ToString(), result.Target.CreditCard.CardholderName, result.Target.CreditCard.MaskedNumber, result.Target.CvvResponseCode, result.Target.CreditCard.ExpirationDate, "YES", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);
                            HttpContext.Current.Session["paySPresponse"] = "SUCCESS";

                            objOrderServices.UpdateOrderStatus(objPRInfo.Order_id, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                            objOrderServices.UpdatePAYMENTSELECTION(intOrderID, "BR-" + result.Target.CreditCard.CardType.ToString());
                            //   objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                            tbwtEngine.SendMail_AfterPaymentSP(intOrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success, false, result.Target.CreditCard.CardType.ToString());
                            //   tbwtEngine.SendMail_Review(intOrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                            HttpContext.Current.Session["ORDER_ID"] = "0";
                            HttpContext.Current.Session["OrderDetExp_orderid"] = OrderID;
                            HttpContext.Current.Session["OrderDetExp_userid"] = oPayInfo.UserId;
                            HttpContext.Current.Session["hfisordercompleted"] = OrderID;



                            return oPayInfo.PORelease;

                            //  x = "true";
                        }
                        else
                        {
                            Transaction transaction = result.Transaction;
                            string errorMessages = "";
                            if (transaction.Status == TransactionStatus.GATEWAY_REJECTED)
                            {
                                errorMessages = "Reason: Gateway rejected.";
                                // errorMessages += transaction.GatewayRejectionReason;
                                // e.g. "avs"
                            }
                            else if (transaction.Status == TransactionStatus.FAILED)
                            {
                                errorMessages = "Reason: Transaction Failed.";
                            }
                            else if (transaction.Status == TransactionStatus.PROCESSOR_DECLINED)
                            {
                                errorMessages = "Reason: Processor Declined.";
                            }
                            else if (transaction.Status == TransactionStatus.UNRECOGNIZED)
                            {
                                errorMessages = "Reason: Transaction Unrecognized.";
                            }
                            else if (transaction.Status == TransactionStatus.VOIDED)
                            {
                                errorMessages = "Reason: Transaction voided.";
                            }
                            else if (transaction.Status == TransactionStatus.AUTHORIZATION_EXPIRED)
                            {
                                errorMessages = "Reason: Authorization Expired";
                            }
                            else
                            {

                                foreach (ValidationError error in result.Errors.DeepAll())
                                {
                                    errorMessages += (int)error.Code + " - " + error.Message + "\n";
                                }
                            }
                            if (errorMessages == "")
                            {
                                errorMessages = result.Message;
                            }
                            ErrorHandler objErrorHandler = new ErrorHandler();
                            objErrorHandler.CreatePayLog(" br  Orderid=" + OrderID + errorMessages);
                            objSecurePayService.call_GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);
                            x = "Error " + errorMessages;
                        }


                        return x;
                    }
                    else
                    {
                        //   objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);
                        x = "Error " + "Please Try again or use a different card / payment method.";
                        objSecurePayService.call_GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", TRANSACTIONID, "", "No", "", "Failed", "", LiabilityShifted.ToString(), Status);
                     
                        return x;
                    }

                }
                else
                {

                    HttpContext.Current.Session["ORDER_ID"] = 0;

                    return "Error:QTEEMPTY";
                }
            }
            else
            {

                //HttpContext.Current.Response.Redirect("login.aspx");
                return "Error:Session Timed out";
            }
        }

        catch (Exception ex)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            objErrorHandler.CreatePayLog(ex.ToString());
            return "Error: " + "Please Try Again";
        }
    }


    protected void btnDeleteOrder_Click(object sender, EventArgs e)
    {
       // CancelOrderPopup.Visible = false;
        CancelOrderPopup.Style.Add("display", "none");
        int _UserId = Convert.ToInt32(Session["User_id"]);
        int r = objOrderServices.UpdateDeleteOrderStatus(OrderID, _UserId);
        if (r > 0) {
            sendMailDeletedOrderNotification(OrderID);
        }
        Response.Redirect("myaccount.aspx",false);
    }

    protected void btnCancelDeleteOrder_Click(object sender, EventArgs e)
    {
     //   CancelOrderPopup.Visible = false;
        CancelOrderPopup.Style.Add("display", "none");
        Response.Redirect("checkout.aspx?" + OrderID.ToString() + "#####" + "PaySP", false);
    }

    private string sendMailDeletedOrderNotification(int order_id)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            string _UserID = "0";
            oOrdInfo = objOrderServices.GetOrder(order_id);
            oUserinfo = objUserServices.GetUserInfo(oOrdInfo.UserID);
            oPayInfo = objPaymentServices.GetPayment(order_id);
            //MessageObj.From = new System.Net.Mail.MailAddress(fromid);
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
           // MessageObj.To.Add("accounts@wes.net.au");
            MessageObj.To.Add(System.Configuration.ConfigurationManager.AppSettings["Accountsemail"].ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(fromid);

            string message = string.Empty;
            MessageObj.Subject = "Customer Deleted Order Notification";
            MessageObj.IsBodyHtml = true;
            //message = message + "<tr><td>ORDER NO. </td><td>&nbsp;</td><td>" + oOrdInfo.OrderID + "</td></tr>";
            message = message + "<tr><td style=\"border: 1px solid black;\">Customer No </td><td style=\"border: 1px solid black;\"> " + oUserinfo.CompanyID + "</td></tr>";
            message = message + "<tr><td style=\"border: 1px solid black;\">Invoice No </td><td style=\"border: 1px solid black;\"> " + oOrdInfo.InvoiceNo + "</td></tr>";
            message = message + "<tr><td style=\"border: 1px solid black;\">Customer Order No </td><td style=\"border: 1px solid black;\"> WAG" + oPayInfo.PORelease + "</td></tr>";
            message = message + "<tr><td style=\"border: 1px solid black;\">Web Order ID</td><td style=\"border: 1px solid black;\"> " + oOrdInfo.OrderID + "</td></tr>";
            message = message + "<tr><td style=\"border: 1px solid black;\">Amount</td><td style=\"border: 1px solid black;\"> $ " + oOrdInfo.TotalAmount + " </td></tr>";
            //message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
            MessageObj.Body = "<html><body><p>Customer has requested for this Proforma Order to be deleted. Please cancel this Proforma</p><table style=\"border: 1px solid black;border-collapse: collapse;\"><tr><td colspan=\"2\" style=\"border: 1px solid black;\">Order Details</td></tr>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }

    #endregion
}
