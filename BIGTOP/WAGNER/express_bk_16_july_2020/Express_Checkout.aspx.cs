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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;
using Braintree;
namespace WES
{
    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class priceValue
    {
        public string orderType { get; set; }
        public string deliveryCharge { get; set; }
        public string taxAmount { get; set; }
        public string totalTaxAmount { get; set; }
        public string totalAmount { get; set; }

    }

    public class OrderAmount {
        public string OrderId;
        public string ProductTotalCost;
        public string TaxAmount;
        public string TotalAmount;
    }

    public class orderItems
    {
        public int pid { get; set; }
        public string catlogitem { get; set; }
        public string description { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal Amt { get; set; }
        public decimal Qty { get; set; }
        public decimal subtot { get; set; }
        public string imageSrc { get; set; }
        public string isppp { get; set; }
    }

    public class TopCart
    {
        public string VIEW_ORDER;
        public bool SHOW_CART;
        public string Cart_Redirect;
        public string CART_AMOUNT;
        public string CART_COUNT;
        public IList<TopCartList> TopCartList;
    }

    public class TopCartList
    {
        public string TBT_REWRITEURL;
        public string FAMILY_NAME;
        public string TWebImage1;
        public string COST;
        public string CART_COUNT;
        public string CODE;
    }

    public partial class Express_Checkout : System.Web.UI.Page
    {
        protected string ClientToken = string.Empty;
        AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
        AjaxControlToolkit.ModalPopupExtender modalPop_loreg = new AjaxControlToolkit.ModalPopupExtender();
        AjaxControlToolkit.ModalPopupExtender modalpop_itemerror = new AjaxControlToolkit.ModalPopupExtender();
        HelperDB objHelperDB = new HelperDB();
        HelperServices objHelperServices = new HelperServices();
        ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
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
        BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();
        int CatalogID = 0;
        int QuoteStatusID = (int)QuoteServices.QuoteStatus.OPEN;
        bool _IsShippingFree = false;
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
        public bool Paytab = false;
        public string PaytabType = "";
        public bool paidtab = false;
        int Userid;
        bool IsZipCodeChange = false;
        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
        NotificationServices objNotificationServices = new NotificationServices();
        string refid = "";
        double SubTotal = 0.0;
        String UserList = "";

        int UsrStatus = (int)UserServices.UserStatus.ACTIVE;
        PayPalService objPayPalService = new PayPalService();
        PayPalAPIService objPayPalApiService = new PayPalAPIService();
        SecurePayService objSecurePayService = new SecurePayService();
        string key = "";
        public double TotAmt = 0.0;
        String sessionId;
        protected void GenerateClientToken()
        {
            try
            {
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.SANDBOX,
                    MerchantId = "mjff7p7mgb4qmp77",
                    PublicKey = "p78kxf6s7zhb8z8x",
                    PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7",

                };
                //var gateway = new BraintreeGateway
                //{
                //    Environment = Braintree.Environment.PRODUCTION,
                //    MerchantId = "wrv3fq8x3r269ycd",
                //    PublicKey = "nm7v4wm8dmw7b6rq",
                //    PrivateKey = "a3d333f589d80552db255c34c1407c40"
                //};



                this.ClientToken = gateway.ClientToken.Generate();


                //     objErrorHandler.CreateLogEA("clientToken:" + this.ClientToken);
            }
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }
        }
        protected void Page_PreInit()
        {
             GenerateClientToken();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
       //         string clickHandler = string.Format(
       //"document.body.style.cursor = 'wait'; this.value='Please wait...'; this.disabled = true; {0};level3Continue()",
       //this.ClientScript.GetPostBackEventReference(BtnL3Continue, string.Empty));
       //         BtnL3Continue.Attributes.Add("onclick", clickHandler);
                ImageButton3.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";
                //ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_check.png";
               // ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_uncheck.png";
                btnclose1.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";

                //            SCP.Style.Add("display", "none");
                //            Level1.Style.Add("display", "block");                                               //comment the line when testing is done
        //        BtnGetStartContinue.Style.Add("display", "block");                                                //comment the line when testing is done

                objErrorHandler.CreateOrderSummarylog("Session No thanks " + Session["ORDER_ID_NOTHANKS"]);
                if (Session["ORDER_ID_NOTHANKS"] != null)
                {
                    hfnothanks.Value = Session["ORDER_ID_NOTHANKS"].ToString();
                }

                //if (hfhidepaypaldiv.Value == "1")
                //{
                //    payment_whiteScreen.Attributes.Add("display", "block");
                //    payment_popup.Attributes.Add("display", "block");
                //}
                //else
                //{
                //    payment_whiteScreen.Attributes.Add("display", "none");
                //    payment_popup.Attributes.Add("display", "none");
                //}

                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"].ToString() != "" && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0 && HttpContext.Current.Session["USER_ID"].ToString() != "999")
                {
                    logoutLink.Attributes.Add("style", "display:block");
                    loginLink.Attributes.Add("style", "display:none");
                    username.InnerText ="Welcome "+ getUserName();

              //      var str = getUserName();
                }
                else 
                {
                    logoutLink.Attributes.Add("style", "display:none");
                    loginLink.Attributes.Add("style", "display:block");
                }
              
                if (!Page.IsPostBack)
                {
                    ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/3d_Secure-Check.png";
                    ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_uncheck.png";


                   // GenerateClientToken();
                    txtRegFname.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtRegLname.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtregemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
                    // txtcemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
                    txtsadd.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtadd2.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txttown.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtstate.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtsadd_Bill.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtadd2_Bill.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txttown_Bill.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtstate_Bill.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txt_attnto.Attributes.Add("onkeypress", "javascript:return check(event);");

                    txtbillbusname.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtbillname.Attributes.Add("onkeypress", "javascript:return check(event);");
                    txtComname.Attributes.Add("onkeypress", "javascript:return check(event);");

                    LinkButton1.Attributes.Add("onclick", "btnPayPalPayLink();return false;");
                    LinkButton2.Attributes.Add("onclick", "btnSecurePayLink();return false;");
                    sessionId = Session.SessionID;

                    if (HttpContext.Current.Session["USER_ID"].ToString() == "0" || Session["USER_ID"].ToString() == "")
                    {
                        HttpContext.Current.Session["USER_ID"] = "999";
                        Response.Redirect("/Home.aspx");
                    }
                    //if (drpSM1.SelectedValue == "Standard Shipping")
                    //{

                    //    lblshipcost.Text = objHelperServices.GetOptionValues("COURIER CHARGE").ToString();
                    //}
                    //else
                    //{
                    //    lblshipcost.Text = "0.00";
                    //}
                    LoadCountryList1();
                    LoadCountryListBill();
                    //     intercust.Visible = false;
            //        string countryCode = drpcountry.SelectedValue.ToString();
                    LoadStates("AU");

          //          string countrycode_bill = drpcountry_bill.SelectedValue.ToString();
                    LoadStatesBill("AU");

        //            intercust.Attributes.Add("style", "display:none");
                    startdivpwd.Attributes.Add("style", "display:none");
                    getstartwelcome.Attributes.Add("style", "display:none");
                    BtnGetStartLogin.Attributes.Add("style", "display:none");
                    letstartregister.Attributes.Add("style", "display:none");
                    Level2.Attributes.Add("style", "display:none");
                    Level3.Attributes.Add("style", "display:none");
                    Level4.Attributes.Add("style", "display:none");
                    Level4_Submit.Attributes.Add("style", "display:none");
                    checkoutrightL4.Attributes.Add("style", "display:none");
                    L4AEditAddress.Attributes.Add("style", "display:none");
                    L3AEditAddress.Attributes.Add("style", "display:none");
                    L4AEditShippingMethod.Attributes.Add("style", "display:none");
                    divOk.Attributes.Add("style", "display:none");

                    if (Request.QueryString.AllKeys.Length > 0)
                    {
                        if (Request.QueryString["Editcart"] != null && Request.QueryString["Editcart"] != "")
                        {
                            if (Request.QueryString["Editcart"].ToLower() == "true")
                            {
                                LoadData();
                            }
                        }


                    }
                }

                int DuplicateItem_Prod_idCount = 0;
                string LeaveDuplicateProds = GetLeaveDuplicateProducts();
               

                PopupMsg.Visible = false;

                int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;

                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
                {
                    Userid = Convert.ToInt32(Session["USER_ID"].ToString());
                }
                else
                {
                    int i = GetParams_orderid("0");
                    if (i == 0)
                    {
                        Session["PageUrl"] = Request.Url.ToString();
                        Response.Redirect("/Login.aspx");
                        divTimeout.Style.Add("display", "block");
                        divCC.Style.Add("display", "none");
                  //      divTimeout.Visible = true;
                  //      divCC.Visible = false;
                        return;
                    }
                }

                if (Session["ORDER_ID"] != null && Session["ORDER_ID"].ToString() != "")
                {
                    OrderID = Convert.ToInt32(Session["ORDER_ID"].ToString());
                }


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

                string orderdstatus = objOrderServices.GetOrderStatus(OrderID);
                if (paidtab == false && objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                {

                    if (orderdstatus != Enum.GetName(typeof(OrderServices.OrderStatus), tmpOrdStatus))
                    {
                        objErrorHandler.CreateOrderSummarylog("orderdstatus: " + orderdstatus + "OrderID: " + OrderID + "b4 cart empty");
                        Session["ORDER_ID"] = 0;
                        Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                        return;
                    }
                }


                DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK", "");
                DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE", sessionId);

                this.HiddenpopuploginregPanel.Visible = false;
                this.modalPop_loreg.Hide();
                this.modalpop_itemerror.Hide();

                string custtype = "";

                if (Convert.ToInt16(Session["USER_ROLE"]) == 4 && custtype == "Dealer")
                {
                    Response.Redirect("/Home.aspx");
                }

                Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
                Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
                if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString().ToUpper() == "YES")
                {

                    if (!IsPostBack && (DuplicateItem_Prod_idCount == 0)) //tbErrorItems == null &&
                    {
                        try
                        {
                            if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
                            {
                                int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                                Userid = objHelperServices.CI(Session["USER_ID"]);
                                GetParams();
                                if (OrderID <= 0)
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
                                    }
                                }

                                if ((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0) || Request["QteFlag"] == "1")
                                {
                                    int a = 5;

                                    string txtadd = "";


                                    LoadCountryList();


                                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                    oOrderInfo = objOrderServices.GetOrder(OrderID);


                                }
                                else
                                {

                                    objErrorHandler.CreateOrderSummarylog("QTEEMPTY" + "OrderID:" + OrderID + " Userid:" + Userid);
                                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", false);

                                }
                            }


                        }
                        catch (System.Threading.ThreadAbortException)
                        {
                            // ignore it
                        }
                        catch (Exception Ex)
                        {

                            objErrorHandler.ErrorMsg = Ex;
                            objErrorHandler.CreateLog();
                        }
                    }
                    else
                    {

                        if (DuplicateItem_Prod_idCount > 0)
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



                    }
                }
                else
                {
                    Response.Redirect("ConfirmMessage.aspx?Result=NOECOMMERCE", false);
                }

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





                if (Request.QueryString["ApproveOrder"] != null && IsPostBack == false)
                {
                    GetApproveOrderDetails(OrderID);
                }
                else
                {
                    SetSessionVlaue();
                }
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:HidePanels();", true);

                //          drpSM1.Attributes.Add("onclick", "javascript:CheckShippment();");
                //          drpSM1.Attributes.Add("onchange", "javascript:CheckShippment();");
                if (Session["tabl3" + OrderID] != null)
                {

                    //if(Session["tabl3" + OrderID].ToString()=="true")
                    //{
                    //     Level1.Visible = false;
                    // Level2.Visible=false;

                    //Level3.Visible = true;
                    //Level4.Visible = false;
                    //Paytab = false;
                    //PaytabType = "ship";
                    //Session["tabl3" + OrderID] = null;
                    //    }
                }

                if (Request.QueryString["Editcart"] == null && Session["EditAddress"] == null && Paytab == false && paidtab == false && OrderID > 0 && (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "" || Session["USER_ID"].ToString() != "999"))
                {
                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    oOrderInfo = objOrderServices.GetOrder(OrderID);

                    if (oOrderInfo.OrderStatus == 1)
                    {
                        Session["ExpressLevel"] = "2Compl";
                        LoadData();
                    }
                }
                //else if (Session["ExpressLevel"].ToString() == "3Compl" && paidtab == false)
                //{
                //    LoadData();
                //}
                if (Paytab == true)
                {

          //          liPayOption.Visible = true;
                    liPayOption.Style.Add("display", "block");

                    div2.Style.Add("display", "none");
                    //           div2.Visible = false;

                    if (!Page.IsPostBack)
                    {
                        //             PHOrderConfirm.Style.Add("display", "block");

                        PHOrderConfirm.Attributes.Add("style", "display:block");
                        //                     PHOrderConfirm.Visible = true;
                        txtsadd.Enabled = false;
                        txtadd2.Enabled = false;
                        txttown.Enabled = false;
                //        drpstate1.Enabled = false;
                        txtzip.Enabled = false;
                //        drpCountry.Enabled = false;
                        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                        oOrderInfo = objOrderServices.GetOrder(OrderID);
                        oPayInfo = objPaymentServices.GetPayment(OrderID);

                        Setdrpdownlistvalue(drpSM1, oOrderInfo.ShipMethod);
                        TextBox1.Text = oOrderInfo.ShipNotes;
                        spanPay.Style.Add("display", "block");
                        //              spanPay.Visible = true;
                        // objErrorHandler.CreatePayLog_Final("inside page load" + oOrdInfo.TotalAmount);
                        if (oOrderInfo.ShipMethod == "Standard Shipping")
                        {
                            //            calctotalcost_shipping(oOrderInfo.ProdTotalPrice);
                            lblAmount.Text = calctotalcost_shipping(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();
                            lblpaypaltotamt.Text = calctotalcost_shipping(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();

                        }
                        else
                        {
                            //                          calctotalcost(oOrderInfo.ProdTotalPrice);
                            lblAmount.Text = calctotalcost(oOrdInfo.ProdTotalPrice,OrderID.ToString()).ToString();
                            lblpaypaltotamt.Text = calctotalcost(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();
                        }

                        TotAmt = Convert.ToDouble(oOrderInfo.TotalAmount);
                        // new start
                        txtsadd.Text = oOrderInfo.ShipAdd1;
                        txtadd2.Text = oOrderInfo.ShipAdd2;
                        txttown.Text = oOrderInfo.ShipCity;
                        txtstate.Text = oOrderInfo.ShipState;
             //           drpstate1.Text = oOrderInfo.ShipState;
                        txtzip.Text = oOrderInfo.ShipZip;

                        //new end


                        //Level1.Visible = false;
                        //Level2.Visible = false;
                        //Level3.Visible = false;
                        //Level4.Visible = true;
                        //Level4_Submit.Visible = false;
                        Level1.Style.Add("display", "none");
                        Level2.Style.Add("display", "none");
                        Level3.Style.Add("display", "none");
                        Level4.Style.Add("display", "block");
                        Level4_Submit.Style.Add("display", "none");
                        PaymentText.Style.Add("display", "block");
                        SubmitText.Style.Add("display", "none");
                        Div3.Style.Add("display", "none");

                        UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                        ouserinfo = objUserServices.GetUserInfo(Userid);
                        L4name.Text = ouserinfo.Contact;
                        L4Email.Text = ouserinfo.AlternateEmail;
                        L4Phone.Text = ouserinfo.Phone;
                        L4Mobile.Text = ouserinfo.MobilePhone;
                        if (ouserinfo.MobilePhone.ToString().Trim() == "")
                        {
                            P4Mobile.Visible = false;
                        }
                        else
                        {
                            P4Mobile.Visible = true;
                        }
                        L4Ship_Company.Text = ouserinfo.COMPANY_NAME;

                        if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                        {
                            L4Ship_Attnto.Text = ouserinfo.Contact;
                        }
                        else
                        {
                            L4Ship_Attnto.Text = ouserinfo.Receiver_Contact;
                        }
                        L4Ship_Street.Text = oOrderInfo.ShipAdd1;
                        L4Ship_Address.Text = oOrderInfo.ShipAdd2;
                        L4Ship_Suburb.Text = oOrderInfo.ShipCity;
                        L4Ship_State.Text = oOrderInfo.ShipState;
                        L4Ship_Zipcode.Text = oOrderInfo.ShipZip;
                        L4Ship_Country.Text = oOrderInfo.ShipCountry;
                        L4Ship_Attnto.Text = oOrderInfo.Receiver_Contact;
                        L4Comments.Text = oOrderInfo.ShipNotes;

                        L4Ship_DELIVERYINST.Text = oOrderInfo.DeliveryInstr;
                        if (drpSM1.SelectedValue == "Standard Shipping")
                        {
                            //                   divshopcounter.Visible = false;
                            divshopcounter.Style.Add("display", "none");
                        }
                        else
                        {
                            //                divshopcounter.Visible = true;
                            divshopcounter.Style.Add("display", "block");
                        }
                        oOrdInfo = objOrderServices.GetOrder(OrderID);
                        lblShippingMethod.Text = oOrdInfo.ShipMethod.Replace("Counter Pickup", "Shop Counter Pickup");


                    }

                    h3Pay.Style.Add("display", "block");
                    h3Pay1.Style.Add("display", "none");
                    Paydiv.Style.Add("display", "block");

                    //           h3Pay.Visible = true;
                    //           h3Pay1.Visible = false;

                    //  SetTabsetting("ship", false);
                    SetTabsetting("Pay", true);

                    //              Paydiv.Visible = true;


                    if (IsPostBack)
                    {

                        string CtrlID = string.Empty;
                        if (Request.Form[hidSourceID.UniqueID] != null && Request.Form[hidSourceID.UniqueID] != string.Empty)
                        {
                            CtrlID = Request.Form[hidSourceID.UniqueID];
                            if (CtrlID.Contains("btnPay"))
                            {
                                //             BtnProgress.Visible = true;
                                BtnProgress.Attributes.Add("style", "display:block;");
                                btnPay.Style.Add("display", "none");
                                //             btnPay.Visible = false;
                            }
                            if (CtrlID.Contains("btnSP"))
                            {
                                // BtnProgressSP.Visible = true;
                                // BtnProgressSP.Attributes.Add("style", "display:block;");
                                // btnSP.Visible = false;
                            }
                        }


                    }
                    else
                    {

                        if (PaytabType == "Pay" || PaytabType == "PayApi")
                        {
                            PayType.Attributes.Add("style", "display:block;");
                            PaypalApiDiv.Attributes.Add("style", "display:block;");
                            btnPay.Attributes.Add("style", "display:block;");
                            btnPayApi.Attributes.Add("style", "display:none;");

                            //PayType.Visible = true;
                            //PaypalApiDiv.Visible = true;
                            //btnPay.Visible = true;
                            //btnPayApi.Visible = false;
                            ScrollToControl("Level4_Payment", false);

                            //if ((TotAmt > 300))
                            //{
                            //    PaypalApiDiv.Attributes.Add("style", "display:block;");
                            //    btnPay.Attributes.Add("style", "display:block;");
                            //    btnPayApi.Attributes.Add("style", "display:none;");

                            //    ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                            //    // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                            //    ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                            //    //           btnPayApi.Visible = false;
                            //    //          btnPay.Visible = true;
                            //    //PaySPDiv.Visible = false;
                            //    //        PaypalApiDiv.Visible = true;
                            //    //          PayPaypalAcc.Visible = true;
                            //    PayPaypalAcc.Style.Add("display", "block");
                            //    SecurePayAcc.Style.Add("display", "none");

                            //    //                SecurePayAcc.Visible = false;
                            //    ScrollToControl("Level4_Payment", false);
                            //}
                            //else
                            //{
                            //    ScrollToControl("Level4_Payment", false);
                            //}


                        }


                        else
                        {
                            PayType.Style.Add("display", "block");
                            //               PayType.Visible = true;
                            if ((TotAmt <= 300) && (objOrderServices.IsNativeCountry_Express(OrderID) == 1))
                            {
                                PaypalApiDiv.Style.Add("display", "none");
                                //                    PaypalApiDiv.Visible = false;
                                ScrollToControl("Level4_Payment", false);
                            }
                            //else if ((TotAmt > 300))
                            //{
                            //    btnPayApi.Style.Add("display", "none");
                            //    btnPay.Style.Add("display", "block");
                            //    PaypalApiDiv.Style.Add("display", "block");
                            //    SecurePayAcc.Style.Add("display", "none");
                            //    ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                            //    // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                            //    ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                            //    //            btnPayApi.Visible = false;
                            //    //            btnPay.Visible = true;
                            //    //PaySPDiv.Visible = false;
                            //    //            PaypalApiDiv.Visible = true;
                            //    //            PayPaypalAcc.Visible = true;
                            //    PayPaypalAcc.Style.Add("display", "block");

                            //    //         SecurePayAcc.Visible = false;
                            //    //btnPay.Focus();
                            //    ScrollToControl("Level4_Payment", false);
                            //}
                            else
                            {
                                PaypalApiDiv.Style.Add("display", "block");
                                //             PaypalApiDiv.Visible = true;
                                //btnSP.Focus();
                                ScrollToControl("Level4_Payment", false);
                                // PaySPDiv.Visible = false;
                                //ImagePay.ImageUrl = "/images/paypal_check.png";
                            }
                        }

                    }




                }
                else
                {
                    Paydiv.Attributes.Add("style", "display:none");
                    spanPay.Attributes.Add("style", "display:none");
                    h3Pay.Attributes.Add("style", "display:none");
                    h3Pay1.Attributes.Add("style", "display:block");
                    PayType.Attributes.Add("style", "display:block");
                    //Paydiv.Visible = false;
                    //spanPay.Visible = false;
                    //h3Pay.Visible = false;
                    //h3Pay1.Visible = true;
                    //PayType.Visible = true;
                }

                if (paidtab == true)
                {
                 //   payment_whiteScreen.Attributes.Add("display", "block");
                 //   payment_popup.Attributes.Add("display", "block");
                    Session["OrderDetExp_orderid"] = OrderID;
                    Session["OrderDetExp_userid"] = Userid;
                    of_ptag.Attributes.Add("style", "display:none");
                    liFinalReview.Attributes.Add("style", "display:block");
                    paiddiv.Attributes.Add("style", "display:block");
                    btnPay.Attributes.Add("style", "display:none");
                    btnPayApi.Attributes.Add("style", "display:none");
                    spanpaid.Attributes.Add("style", "display:block");
                    btneditlogin4.Attributes.Add("style", "display:none");
                    PaypalApiDiv.Attributes.Add("style", "display:none");
                    //      of_ptag.Visible = false;
                    //      liFinalReview.Visible = true;
                    //      paiddiv.Visible = true;
                    //       btnPay.Visible = false;
                    //       btnPayApi.Visible = false;
                    //      spanpaid.Visible = true;
                    //      btneditlogin4.Visible = false;
                    //       L4AEditShippingMethod.Visible = false;
                    L4AEditShippingMethod.Style.Add("display", "none");
                    hpaid.Style.Add("display", "block");
                    hpaid1.Style.Add("display", "none");
                    ImgBtnEditShipping.Style.Add("display", "none");
                    //      PaypalApiDiv.Visible = false;
                    //      hpaid.Visible = true;
                    //      hpaid1.Visible = false;
                    SetTabsetting("ship", false);
                    SetTabsetting("Pay", false);
                    SetTabsetting("paid", true);
                    //       ImgBtnEditShipping.Visible = false;
                    string output = "";
                    try
                    {
                        //objErrorHandler.CreatePayLog("before check isnative OrderID" + OrderID);
                        int isnative = objOrderServices.IsNativeCountry_Express(OrderID);
                        //objErrorHandler.CreatePayLog("after check isnative OrderID" + OrderID);
                        if (PaytabType == "Pay" || PaytabType == "PayApi")
                        {
                            payment_whiteScreen.Attributes.Add("style", "display:block");
                            payment_success_popup.Attributes.Add("style", "display:block");
                            if (Request["key"] != null)
                            {
                                // key = DecryptSP(Request["key"].ToString());
                            }

                            if (httpRequestVariables()["tx"] != null)
                            {
                                HttpContext.Current.Session["payflowresponse"] = httpRequestVariables();
                                var response = HttpContext.Current.Session["payflowresponse"] as NameValueCollection;
                                if (response["cm"] != null)
                                {
                                    //objPayPalService.SetPayPalStatus(response, response["cm"].ToString());
                                    objPayPalService.ExpressCheckout_SetPayPalStatus(response, response["cm"].ToString());
                                }
                                //string rtn = 
                                //objErrorHandler.CreatePayLog("setpaypalstatus httpRequestVariables tx not null inner OrderID" + OrderID);
                                //output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(key) +"\";</script>";
                                output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(OrderID + "#####" + "Pay" + "#####" + "Paid") + "\";</script>";
                                //paiddiv.InnerHtml = output;
                                //return;
                            }
                            else if (httpRequestVariables()["Token"] != null && httpRequestVariables()["PayerID"] != null)
                            {
                                objPayPalApiService.DoECStatus(httpRequestVariables());
                                //objErrorHandler.CreatePayLog("Token and PayerID not null inner OrderID=" + OrderID);
                                output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(OrderID + "#####" + "PayApi" + "#####" + "Paid") + "\";</script>";
                                //paiddiv.InnerHtml = output;
                                //return;
                            }


                            if (IsPostBack)
                                return;

                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

                            //objErrorHandler.CreatePayLog("Get Pay and Order info start OrderID=" + OrderID);
                            oPayInfo = objPaymentServices.GetPayment(OrderID);
                            oOrderInfo = objOrderServices.GetOrder(OrderID);
                            //objErrorHandler.CreatePayLog("Get Pay and Order info end OrderID" + OrderID);
                            // lblOrderNo.Text = oPayInfo.PORelease;
                            lblShippingMethod.Text = oOrderInfo.ShipMethod;

                            PaymentID = oPayInfo.PaymentID;
                            bool isipn = false;
                            if (HttpContext.Current.Session["payflowresponse"] != null && HttpContext.Current.Session["payflowresponse"].ToString() != "")
                            {
                                
                                if (HttpContext.Current.Session["payflowresponse"].ToString() == "SUCCESS")
                                {
                                    //objErrorHandler.CreatePayLog("page load  payflowresponse SUCCESS=" + HttpContext.Current.Session["payflowresponse"] + "OrderID=" + OrderID);
                                    // string ordstatus = objOrderServices.GetOrderStatus(OrderID);
                                    string[] ipn = null;

                                    if (HttpContext.Current.Session["IPN"] != null && HttpContext.Current.Session["IPN"].ToString() != "")
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
                                            //objErrorHandler.CreatePayLog("page load  ipn=" + isipn + "OrderID=" + OrderID);
                                        }
                                    }


                                    //objErrorHandler.CreatePayLog("before UpdatePaymentOrderStatus Orderid=" + OrderID);


               //                     objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn);

                                    if (oOrderInfo.isppppp == 1)
                                    {
                                        objErrorHandler.CreateLog("1");
                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn, 1);
                                    }
                                    else
                                    {
                                        objErrorHandler.CreateLog("2");
                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn, 0);
                                    }
                                   
                                    //objErrorHandler.CreatePayLog("after UpdatePaymentOrderStatus Orderid=" + OrderID);

                                    //objErrorHandler.CreatePayLog("before SendMail_AfterPaymentPP CheckMailLog Orderid=" + OrderID);
                                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                                    {
                                        DataTable dtcheckmailsend = objOrderServices.CheckMailLog(OrderID);
                                        if (dtcheckmailsend == null || dtcheckmailsend.Rows.Count == 0)
                                        {
                                            //objErrorHandler.CreatePayLog("after SendMail_AfterPaymentPP CheckMailLog Orderid=" + OrderID);
                                            SendMail_AfterPaymentPP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                            tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                        }
                                    }

                                    //objErrorHandler.CreatePayLog("after SendMail_AfterPaymentPP Orderid=" + OrderID);
                                    ttOrder.Text = oPayInfo.PORelease;
                                    divError.InnerHtml = "";
                                    //           divOk.Visible = true;
                                    //divOk.InnerHtml = "<img src=\"/images/checkout_tick.png\" class=\"approved_icon\"/>" + "Transaction Approved! Your Order will now be processed, thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                    divOk.InnerHtml = "Transaction Approved! <br/> Your Order will now be processed, Thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                    //                 PayOkDiv.Visible = true;
                                    divOk.Style.Add("display", "block");
                                    PayOkDiv.Style.Add("display", "block");

                                    //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                                    divlink.InnerHtml = "Payment Method: Paypal Guest Checkout";
                                    checkoutrightL1.Style.Add("display", "none");
                                    checkoutrightL4.Style.Add("display", "block");
                                    //                checkoutrightL1.Visible = false;
                                    //                checkoutrightL4.Visible = true;
                                    ttOrder.Enabled = false;
                                    ttOrder.ReadOnly = true;
                                    divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
             //                       Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                                    //                divpaypalaccount.Visible = false;
                                    divpaypalaccount.Style.Add("display", "none");
                                    PayPaypalAcc.Style.Add("display", "none");
                                    L4AEditAddress.Style.Add("display", "none");
                                    L3AEditAddress.Style.Add("display", "none");
                                    L4AEditShippingMethod.Style.Add("display", "none");
                                    BtnEditAddress.Style.Add("display", "none");
                                    BtnL3EditAddress.Style.Add("display", "none");
                                    BtnL4EditShippingMethod.Style.Add("display", "none");
                                    ImageButton1.Style.Add("display", "none");
                                    PayType.Style.Add("display", "none");
                                    SecurePayAcc.Style.Add("display", "none");
                                    divshopcounter.Style.Add("display", "none");
                                    SubmitText.Style.Remove("display");
                                    PaymentText.Style.Remove("display");
                                    SubmitText.Visible = false;
                                    PaymentText.Visible = true;

                                    //        L4AEditAddress.Visible = false;
                                    //       L3AEditAddress.Visible = false;
                                    //       L4AEditShippingMethod.Visible = false;
                                    //         BtnEditAddress.Visible = false;
                                    //         BtnL3EditAddress.Visible = false;
                                    //          BtnL4EditShippingMethod.Visible = false;
                                    //          ImageButton1.Visible = false;
                                    //           PayType.Visible = false;
                                    //           SecurePayAcc.Visible = false;

                                    //           divshopcounter.Visible = false;
                                    Session["OrderDetExp_orderid"] = OrderID;
                                    Session["OrderDetExp_userid"] = Userid;

                                    Session["ORDER_ID"] = "0";
                                }
                                else
                                {
                                    divOk.Style.Add("display", "none");
                                    PayOkDiv.Style.Add("display", "none");
                                    PayType.Style.Add("display", "block");
                                    //           PayType.Visible = true;
                                    //objErrorHandler.CreatePayLog("Transaction failed payflowresponse! Please try again Orderid=" + OrderID);
                                    divOk.InnerHtml = "";
                                    //            divOk.Visible = false;
                                    //             PayOkDiv.Visible = false;
                                    divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";
                                    if (isnative == 1)
                                        divlink.InnerHtml = "<a href=\"express_checkout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                    else
                                        divlink.InnerHtml = "<a href=\"express_checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                }
                                HttpContext.Current.Session["payflowresponse"] = "";
                                HttpContext.Current.Session["IPN"] = "";
                                payment_whiteScreen.Attributes.Add("style", "display:none");
                                payment_success_popup.Attributes.Add("style", "display:none");
                                //output += "<p>(server response follows)</p>\n";
                                //output += print_r(payflowresponse);
                                //divContent.InnerHtml = output;
                                return;
                            }
                            else if (HttpContext.Current.Session["payAPIresponse"] != null && HttpContext.Current.Session["payAPIresponse"].ToString() != "")
                            {
                                if (HttpContext.Current.Session["payAPIresponse"].ToString() == "SUCCESS")
                                {

                                    //objErrorHandler.CreatePayLog("before UpdatePaymentOrderStatus APIres Orderid=" + OrderID);

                   //                 objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn);
                                    //objErrorHandler.CreatePayLog("after UpdatePaymentOrderStatus APIres Orderid=" + OrderID);

                                    if (oOrderInfo.isppppp == 1 )
                                    {
                                        objErrorHandler.CreateLog("1");
                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn, 1);
                                    }
                                    else
                                    {
                                        objErrorHandler.CreateLog("2");
                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn, 0);
                                    }


                                    //objErrorHandler.CreatePayLog("before SendMail_AfterPaymentPP APIres Orderid=" + OrderID);
                                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                                    {
                                        SendMail_AfterPaymentPP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                        tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                    }
                                    //else
                                    //    SendMail_AfterPayment(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                                    //objErrorHandler.CreatePayLog("after SendMail_AfterPaymentPP APIres Orderid=" + OrderID);

                                    PayOkDiv.Style.Add("display", "none");
                                    //          PayOkDiv.Visible = true;
                                    divError.InnerHtml = "";
                                    divOk.InnerHtml = "<img src=\"/images/checkout_tick.png\" class=\"approved_icon\"/>" + "Transaction Approved! Your Order will now be processed, thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                    //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                                    divlink.InnerHtml = "Payment Method: Paypal Express Checkout";
                                    divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
                //                    Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                                    ttOrder.Text = oPayInfo.PORelease;
                                    ttOrder.Enabled = false;
                                    ttOrder.ReadOnly = true;

                                    L4AEditAddress.Style.Add("display", "none");
                                    L3AEditAddress.Style.Add("display", "none");
                                    L4AEditShippingMethod.Style.Add("display", "none");
                                    BtnEditAddress.Style.Add("display", "none");
                                    BtnL3EditAddress.Style.Add("display", "none");
                                    ImageButton1.Style.Add("display", "none");
                                    BtnL4EditShippingMethod.Style.Add("display", "none");
                                    SubmitText.Style.Remove("display");
                                    PaymentText.Style.Remove("display");
                                    SubmitText.Visible = false;
                                    PaymentText.Visible = true;


                                    //              L4AEditAddress.Visible = false;
                                    //              L3AEditAddress.Visible = false;
                                    ////              L4AEditShippingMethod.Visible = true;
                                    //              BtnEditAddress.Visible = false;
                                    //               BtnL3EditAddress.Visible = false;
                                    //               BtnL4EditShippingMethod.Visible = false;
                                    PayPaypalAcc.Style.Add("display", "none");
                                    //                ImageButton1.Visible = false;
                                    Session["OrderDetExp_orderid"] = OrderID;
                                    Session["OrderDetExp_userid"] = Userid;
                                    // Session.RemoveAll();
                                    //Session.Clear();
                                    // Session.Abandon();
                                    //Session["USER_ID"] = "999";
                                    //Session["DUMMY_FLAG"] = "0";
                                    Session["ORDER_ID"] = "0";
                                    //Session["USER_ROLE"] = "0";
                                    //ScrollToControl("ctl00_maincontent_PayOkDiv", true);
                                }
                                else
                                {
                                    //objErrorHandler.CreatePayLog("Transaction failed payAPIresponse! Please try again Orderid=" + OrderID);
                                    divOk.InnerHtml = "";
                                    PayType.Style.Add("display", "block");
                                    divOk.Style.Add("display", "none");
                                    PayOkDiv.Style.Add("display", "block");
                                    //     PayType.Visible = true;
                                    //      divOk.Visible = false;
                                    //       PayOkDiv.Visible = true;
                                    divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";
                                    if (isnative == 1)
                                        divlink.InnerHtml = "<a href=\"express_checkout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                    else
                                        divlink.InnerHtml = "<a href=\"express_checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PayApi") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";

                                }
                                HttpContext.Current.Session["payAPIresponse"] = "";
                                HttpContext.Current.Session["IPN"] = "";
                                payment_whiteScreen.Attributes.Add("style", "display:none");
                                payment_success_popup.Attributes.Add("style", "display:none");
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
                            // lblOrderNo.Text = oPayInfo.PORelease;
                            lblShippingMethod.Text = oOrderInfo.ShipMethod;

                            if (HttpContext.Current.Session["paySPresponse"] != null && HttpContext.Current.Session["paySPresponse"].ToString() != "")
                            {

                                if (HttpContext.Current.Session["paySPresponse"].ToString() == "SUCCESS")
                                {
                                    objErrorHandler.CreatePayLog("SP before UpdatePaymentOrderStatus Orderid=" + OrderID);

                        //            objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, false);

                                    if (oOrderInfo.isppppp == 1)
                                    {
                                        objErrorHandler.CreateLog("1");
                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, false, 1);
                                    }
                                    else
                                    {
                                        objErrorHandler.CreateLog("2");
                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, false, 0);
                                    }


                                    objErrorHandler.CreatePayLog("SP after UpdatePaymentOrderStatus Orderid=" + OrderID);
                                    objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                                    SendMail_AfterPaymentSP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                    objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                    tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                    divOk.Style.Add("display", "block");
                                    PayOkDiv.Style.Add("display", "block");
                                    //                 divOk.Visible = true;
                                    //                 PayOkDiv.Visible = true;
                                    divError.InnerHtml = "";
                                    divOk.InnerHtml = "Transaction Approved! <br/> Your Order will now be processed, Thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                    //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                                    divlink.InnerHtml = "Payment Method: Credit Card";
                                    ttOrder.ReadOnly = true;
                                    ttOrder.Enabled = false;
                                    PaySPDiv.Style.Add("display", "none");
                                    PayType.Style.Add("display", "none");
                                    checkoutrightL1.Style.Add("display", "none");
                                    checkoutrightL4.Style.Add("display", "block");
                                    L4AEditAddress.Style.Add("display", "none");
                                    L3AEditAddress.Style.Add("display", "none");
                                    L4AEditShippingMethod.Style.Add("display", "none");
                                    BtnEditAddress.Style.Add("display", "none");
                                    BtnL3EditAddress.Style.Add("display", "none");
                                    BtnL4EditShippingMethod.Style.Add("display", "none");
                                    ImageButton1.Style.Add("display", "none");
                                    SubmitText.Style.Remove("display");
                                    PaymentText.Style.Remove("display");
                                    SubmitText.Visible = false;
                                    PaymentText.Visible = true;

                                    //              PaySPDiv.Visible = false;
                                    //             PayType.Visible = false;
                                    //checkoutrightL1.Visible = false;
                                    //checkoutrightL4.Visible = true;
                                    //L4AEditAddress.Visible = false;
                                    //L3AEditAddress.Visible = false;
                                    //L4AEditShippingMethod.Visible = true;
                                    //BtnEditAddress.Visible = false;
                                    //BtnL3EditAddress.Visible = false;
                                    //BtnL4EditShippingMethod.Visible = false;
                                    //ImageButton1.Visible = false;
                                    Session["OrderDetExp_orderid"] = OrderID;
                                    Session["OrderDetExp_userid"] = Userid;
                                    ttOrder.Text = oPayInfo.PORelease;
                                    Session["ORDER_ID"] = "0";
                                    divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
                     //               Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                                    PayOkDiv.Focus();
                                    
                                }
                                else
                                {
                                    divOk.Style.Add("display", "none");
                                    PayType.Style.Add("display", "none");
                                    PayOkDiv.Style.Add("display", "none");
                                    divOk.InnerHtml = "";
                                    //          divOk.Visible = false;
                                    objErrorHandler.CreatePayLog("SP Transaction failed! Please try again Orderid=" + OrderID);
                                    divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";

                                    //         PayType.Visible = true;
                                    //          divOk.InnerHtml = "";
                                    //         divOk.Visible = false;
                                    //          PayOkDiv.Visible = true;
                                    divError.InnerHtml = "Transaction failed! Please try again.<br/>";
                                    if (isnative == 1)
                                        divlink.InnerHtml = "<a href=\"express_checkout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                    else
                                        divlink.InnerHtml = "<a href=\"express_checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PaySP") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                }
                                HttpContext.Current.Session["paySPresponse"] = "";
                                return;
                            }
                            else
                            {
                                Response.Redirect("/Home.aspx");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.ErrorMsg = ex;
                        objErrorHandler.CreateLog();

                    }
                    //objErrorHandler.CreatePayLog("paidtab == true end");
                }
                else
                {
                    paiddiv.Attributes.Add("style", "display:none");
                    spanpaid.Attributes.Add("style", "display:none");
                    hpaid1.Attributes.Add("style", "display:block");
                    hpaid.Attributes.Add("style", "display:none");
                    //paiddiv.Visible = false;
                    //spanpaid.Visible = false;
                    //hpaid1.Visible = true;
                    //hpaid.Visible = false;
                }

            }

            catch (Exception ex)
            { }



        }



        private void ScrollToControl(string controlId, bool Withposition)
        {
            //scroll to button

            if (Withposition == true)
            {
                string script =
                    "$(document).ready(function() {" +
                        "$('html,body').animate({ " +
                            "scrollTop: $('#" + controlId + "').offset().top -100" +
                        "}, 0);" +
                    "});";

                if (!Page.ClientScript.IsStartupScriptRegistered("ScrollToElement"))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ScrollToElement", script, true);
            }
            else
            {

                string script =
                        "$(document).ready(function() {" +
                            "$('html,body').animate({ " +
                                "scrollTop: $('#" + controlId + "').offset().top -50 " +
                            "}, 0);" +
                        "});";

                if (!Page.ClientScript.IsStartupScriptRegistered("ScrollToElement"))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ScrollToElement", script, true);
            }
        }


        private void SendMail_AfterPaymentPP(int OrderId, int OrderStatus, bool isau)
        {
            string toemail = "";
            try
            {
                //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner1 OrderId=" + OrderId);

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


                if (oOrderInfo.CreatedUser != 999)
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
                    //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner2");
                    //objErrorHandler.CreatePayLog(sHTML);
                }
                catch (Exception ex)
                {
                    objHelperServices.Mail_Error_Log("PP", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, "", ex.Message);
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";

                }
                if (sHTML != "")
                {

                    //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner3 OrderId=" + OrderId);
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

                    }
                    else
                    {
                        emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());


                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "Wagner Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;

                    //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner4 OrderId=" + OrderId);
                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);
                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                    //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner5 OrderId=" + OrderId);

                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                            smtpclient.Send(MessageObj);
                            objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
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
                                        MessageObj.Subject = "Wagner International Order Alert - Order No : " + CustomerOrderNo.ToString();
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                        objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
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
                                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
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
                                        objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
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
                                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    //MessageObj.CC.Add(emails.ToString());
                                }
                            }

                        }


                    }


                }
            }
            catch (Exception ex)
            {
                objHelperServices.Mail_Error_Log("PP", OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
                objHelperServices.Mail_Log("PP", OrderId, "", ex.Message);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();


            }
            //objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner6 OrderId=" + OrderId);
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
                if (oOrderInfo.CreatedUser != 999)
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
                    //objErrorHandler.CreatePayLog(sHTML);

                }
                catch (Exception ex)
                {
                    objHelperServices.Mail_Error_Log("SP", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
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
                    MessageObj.Subject = "Wagner Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
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
                                        MessageObj.Subject = "Wagner Order Alert - Order No : " + CustomerOrderNo.ToString();
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
                objHelperServices.Mail_Error_Log("SP", OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
                objHelperServices.Mail_Log("SP", OrderId, "", ex.Message);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner14 OrderId=" + OrderId);

            }
            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP end OrderId=" + OrderId);
        }

        private static void SendMailRegiExpressCheckout(string LoginId, string Password, string Email, string Fname, string Lname)
        {
            HelperServices objHelperServices = new HelperServices();
            try
            {

                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                MessageObj.To.Add(Email.ToString());
                MessageObj.Subject = "Wagner Online Registration Confirmation";
                MessageObj.IsBodyHtml = true;

                StringTemplateGroup _stg_container = null;
                StringTemplate _stmpl_container = null;

                string stemplatepath = HttpContext.Current.Server.MapPath("Templates");
                _stg_container = new StringTemplateGroup("Mail-wagner", stemplatepath);
                _stmpl_container = _stg_container.GetInstanceOf("Mail-wagner" + "\\" + "regiexpresscheckout");
                _stmpl_container.SetAttribute("FirstLastName", Fname.ToString() + " " + Lname.ToString());
                _stmpl_container.SetAttribute("UserName", LoginId);
                MessageObj.Body = _stmpl_container.ToString();

                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);
                // Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignore it
            }
            catch (Exception)
            {
                //objUserServices.DeleteRegistration(reg_id);
                //Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");
            }
        }

        private void GetParams()
        {
            try
            {
                if (Request.Url.Query != null && Request.Url.Query != "")
                {

                    string id = null;

                    if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
                    {
                        Userid = Convert.ToInt32(Session["USER_ID"].ToString());
                    }
                    if (Request.Url.Query.Contains("key=") == false)
                    {

                        id = Request.Url.Query.Replace("?", "");
                        int n;
                        bool isNumeric = int.TryParse(id, out n);
                        // objErrorHandler.CreatePayLog_Final("Getparams" + id);

                        if (Request.Url.Query.Contains("Editcart=true") == false)
                        {
                            id = DecryptSP(id);
                        }
                        else
                        {
                            id = DecryptSP(id.Replace("&Editcart=true", ""));
                        }

                        if ((id == null) && (isNumeric == true))
                        {
                            id = Request.Url.Query.Replace("?", "");
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

                        //add new
                        int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;
                        if (Userid != 999 || OrderID == 0)
                        {
                            OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
                        }


                        OrderIDaspx = OrderID;

                        if (ids.Length > 1)
                        {

                            PaytabType = ids[1];
                            oPayInfo = objPaymentServices.GetPayment(OrderID);
                            if (oPayInfo.PaymentID == 0)
                            {
                                Session["ExpressLevel"] = "2Compl";
                                Paytab = false;
                            }
                            {
                                Paytab = true;
                            }

                        }
                        if (ids.Length > 2)
                        {
                            paidtab = true;
                        }
                    }
                    else
                    {
                        OrderID = 0;
                        PaytabType = "";
                        OrderIDaspx = 0;
                        div2.InnerHtml = "Invalid Data";
                        div2.Style.Add("display", "block");
             //           div2.Visible = true;

                    }

                }
                else
                {
                    OrderID = 0;
                    PaytabType = "";
                    OrderIDaspx = 0;
                    div1.Style.Add("display", "none");
             //       div1.Visible = false;
                    div2.InnerHtml = "Invalid Data";
                    div2.Style.Add("display", "block");
                 //   div2.Visible = true;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }

        }

        private void SetTabsetting(string tab, bool isOpen)
        {
            HtmlGenericControl sp = null;
            HtmlGenericControl hp = null;
            HtmlGenericControl dp = null;
            if (tab == "Pay")
            {
                sp = spanPay;
                hp = h3Pay;
                dp = divPay;
                Level3.Style.Add("display", "none");
                Level4.Style.Add("display", "block");
                //        Level3.Visible = false;
                //        Level4.Visible = true;

            }
            if (tab == "paid")
            {
                sp = spanpaid;
                hp = hpaid;
                dp = divpaid;

            }
            if (tab == "ship")
            {
                dp = divship;
                Level3.Style.Add("display", "block");
                //          Level3.Visible = true;
                ScrollToControl("ctl00_maincontent_divl3continue", true);
                //          Level4.Visible = false;
                Level4.Style.Add("display", "none");

            }

            if (isOpen == false)
            {
                dp.Attributes["class"] = "panel-collapse collapse";
                dp.Attributes["style"] = "display:none;";

            }
            else
            {
                dp.Attributes["class"] = "panel-collapse collapse in";

                dp.Attributes["style"] = "display:block;";

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
                        int n;
                        bool isNumeric = int.TryParse(id, out n);
                        //  objErrorHandler.CreateLog("Invalid user in checkout" + "isNumeric-" + isNumeric);
                        if (isNumeric == true)
                        {
                            OrderDB objOrderDB = new OrderDB();
                            //  objErrorHandler.CreateLog("before userid");
                            string orderuserid = (string)objOrderDB.GetGenericDataDB("", id.ToString(), "GETUSERID_ORDER", OrderDB.ReturnType.RTString);

                            //  objErrorHandler.CreateLog("orderuserid--" + orderuserid);
                            //  objErrorHandler.CreateLog("Userid--" + Userid.ToString());
                            if ((orderuserid != Userid.ToString()) && (orderuserid != "999") && (Userid != 999))
                            {
                                Userid = Convert.ToInt32(orderuserid);
                                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                                oUserInfo = objUserServices.GetUserInfo(Userid);
                                {

                                    Session.RemoveAll();
                                    Session.Clear();
                                    Session.Abandon();
                                    Session["USER_ID"] = "";
                                    Session["DUMMY_FLAG"] = "0";
                                    Session["ORDER_ID"] = "0";

                                    Session["Userid"] = orderuserid;
                                    Session["USER_NAME"] = oUserInfo.LoginName;
                                    Session["USER_ID"] = orderuserid;
                                    Session["Emailid"] = oUserInfo.Email;
                                    Session["Firstname"] = oUserInfo.FirstName;
                                    Session["Lastname"] = oUserInfo.LastName;
                                    Session["ORDER_ID"] = id;
                                    Session["USER_ROLE"] = oUserInfo.USERROLE;

                                    Session["COMPANY_ID"] = oUserInfo.CompanyID;
                                    Session["CUSTOMER_TYPE"] = oUserInfo.CUSTOMER_TYPE;
                                    Session["DUMMY_FLAG"] = "1";
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

        public static string GetLeaveDuplicateProducts()
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                string LeaveDuplicateProds = "";
                if (HttpContext.Current.Session["LeaveDuplicateProds"] != null && HttpContext.Current.Session["LeaveDuplicateProds"].ToString() != "")
                {
                    LeaveDuplicateProds = HttpContext.Current.Session["LeaveDuplicateProds"].ToString();
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

        #region "Functions"

        private void GetApproveOrderDetails(int OrderID)
        {
            DataSet dsOD = new DataSet();
            dsOD = objOrderServices.GetApproveOrderItems(OrderID);

            oOrdInfo1 = objOrderServices.GetOrder(OrderID);

            if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsOD.Tables[0].Rows)
                {
                    //ttOrder.Text = string.IsNullOrEmpty(row["PO_RELEASE"].ToString()) ? "" : row["PO_RELEASE"].ToString();

                    if (row["SHIP_METHOD"].ToString() == "Drop Shipment Order" && Checkdrpdownlistvalue(drpSM1, row["SHIP_METHOD"].ToString()) == false)
                        drpSM1.Items.Add(new ListItem("Drop Shipment Order", "Drop Shipment Order"));

                    drpSM1.SelectedValue = string.IsNullOrEmpty(row["SHIP_METHOD"].ToString()) ? "" : row["SHIP_METHOD"].ToString();
                    Setdrpdownlistvalue(drpSM1, string.IsNullOrEmpty(row["SHIP_METHOD"].ToString()) ? "" : row["SHIP_METHOD"].ToString());

                    TextBox1.Text = string.IsNullOrEmpty(row["COMMENTS"].ToString()) ? "" : row["COMMENTS"].ToString();
                    if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
                    {
                        //txtCompany.Text = objHelperServices.Prepare(oOrdInfo1.ShipCompName);
                        //txtAttentionTo.Text = objHelperServices.Prepare(oOrdInfo1.ShipFName);
                        //txtAddressLine1.Text = objHelperServices.Prepare(oOrdInfo1.ShipAdd1);
                        //txtAddressLine2.Text = objHelperServices.Prepare(oOrdInfo1.ShipAdd2);
                        //txtSuburb.Text = objHelperServices.Prepare(oOrdInfo1.ShipCity);
                        //drpState.Text = objHelperServices.Prepare(oOrdInfo1.ShipState);
                        //txtCountry.Text = objHelperServices.Prepare(oOrdInfo1.ShipCountry);
                        //txtPostCode.Text = objHelperServices.Prepare(oOrdInfo1.ShipZip);
                        //// txtReceiverContactName.Text = objHelperServices.Prepare(oOrdInfo1.ReceiverContact);
                        //txtDeliveryInstructions.Text = objHelperServices.Prepare(oOrdInfo1.DeliveryInstr);
                        //txtShipPhoneNumber.Text = objHelperServices.Prepare(oOrdInfo1.ShipPhone);

                        if (objOrderServices.IsUserCanDropShip(objHelperServices.CI(Session["USER_ID"])) == false)  // Drop shipment available as per user role
                        {
                            //txtCompany.Enabled = false;
                            //txtAttentionTo.Enabled = false;
                            //txtAddressLine1.Enabled = false;
                            //txtAddressLine2.Enabled = false;
                            //txtSuburb.Enabled = false;
                            //drpState.Enabled = false;
                            //txtCountry.Enabled = false;
                            //txtPostCode.Enabled = false;
                            //txtDeliveryInstructions.Enabled = false;
                            //txtShipPhoneNumber.Enabled = false;
                            //drpSM1.Enabled = false;
                            //ttOrder.Enabled = false;
                            //TextBox1.Enabled = false;
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

        public void LoadCountryList()
        {
            try
            {
                DataSet oDs = new DataSet();
                oDs = new DataSet();
                oDs = objCountryServices.GetCountries();
                //drpShipCountry.Items.Clear();
                //drpShipCountry.DataSource = oDs;
                //drpShipCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
                //drpShipCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
                //drpShipCountry.DataBind();
                //drpShipCountry.Items.Add(new ListItem("(Select Country)", "", true));

                //drpBillCountry.Items.Clear();
                //drpBillCountry.DataSource = oDs;
                //drpBillCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
                //drpBillCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
                //drpBillCountry.DataBind();
                //drpBillCountry.Items.Add(new ListItem("(Select Country)", "", true));
            }
            catch (Exception Ex)
            {
                objErrorHandler.ErrorMsg = Ex;
                objErrorHandler.CreateLog();
            }
        }

        public void LoadCountryList1()
        {
            try
            {
                DataSet oDs = new DataSet();

                oDs = objCountryServices.GetCountries();
                drpcountry.Items.Clear();
                drpcountry.DataSource = oDs;
                drpcountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
                drpcountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
                drpcountry.DataBind();
                drpcountry.SelectedIndex = drpcountry.Items.IndexOf(new ListItem("Australia", "AU"));

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
        }

        public void LoadCountryListBill()
        {
            try
            {
                DataSet oDs = new DataSet();

                oDs = objCountryServices.GetCountries();
                drpcountry_Bill.Items.Clear();
                drpcountry_Bill.DataSource = oDs;
                drpcountry_Bill.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
                drpcountry_Bill.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
                drpcountry_Bill.DataBind();
                drpcountry_Bill.SelectedIndex = drpcountry.Items.IndexOf(new ListItem("Australia", "AU"));

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
        }

        public void LoadStates(String conCode)
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            drpstate.DataSource = oDs;
            drpstate.DataTextField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate.DataBind();
            drpstate.Items.Insert(0, new ListItem("Select", ""));

        }

        public void LoadStatesBill(String conCode)
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            drpstate_Bill.DataSource = oDs;
            drpstate_Bill.DataTextField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate_Bill.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate_Bill.DataBind();
            drpstate_Bill.Items.Insert(0, new ListItem("Select", ""));
        }

        protected decimal CalculateShippingCost(int OrderID)
        {
            DataSet dsOItem = new DataSet();
            decimal ShippingValue = 0;
            dsOItem = objOrderServices.GetOrderItems(OrderID);
            decimal ProductCost;

            if (drpSM1.SelectedValue == "Standard Shipping")
            {
                if (hfisppp.Value == "0")
                {
                    string shipcost = objHelperServices.GetOptionValues("COURIER CHARGE");
                    if (shipcost != "")
                        ShippingValue = Convert.ToDecimal(shipcost);
                }

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
                    //ttOrder.Text = Session["ORDER_NO"].ToString();
                }
                if (Session["SHIPPING"] != null)
                {

                    Setdrpdownlistvalue(drpSM1, string.IsNullOrEmpty(Session["SHIPPING"].ToString()) ? "" : Session["SHIPPING"].ToString());

                }
                if (Session["DELIVERY"] != null)
                {

                    TextBox1.Text = Session["DELIVERY"].ToString();
                }
            }
        }

        public static DataSet GetOrderItemDetailSum(int OrderID)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                OrderDB objOrderDB = new OrderDB();
                return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "GET_ORDER_ITEM_DETAIL_SUM", OrderDB.ReturnType.RTDataSet);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        protected  string EncryptSP(string ordid)
        {
          
            string enc = "";
            enc = objSecurity.StringEnCrypt(ordid, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            return HttpUtility.UrlEncode(enc);
        }
        protected static string EncryptSP_webmethod(string ordid)
        {
            Security objSecurity = new Security();
            string enc = "";
            enc = objSecurity.StringEnCrypt(ordid, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            return HttpUtility.UrlEncode(enc);
        }
        protected string DecryptSP(string ordid)
        {
            string enc = "";

            enc = HttpUtility.UrlDecode(ordid);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            return enc;
        }

        protected static void ProceedFunction(string drpSM1, string ttinter_order, string TextBox1, string hfisppp)
        {
            OrderServices objOrderServices = new OrderServices();
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
            UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            PaymentServices objPaymentServices = new PaymentServices();
            PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                bool isau = false;
                int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                if (objOrderServices.GetOrderStatus(OrderID) != "")
                {
                    int OrdStatusVerify = (int)OrderServices.OrderStatus.MANUALPROCESS;
                    DataSet oDs = new DataSet();
                    oDs = objOrderServices.GetOrderItems(OrderID);
                    int ChkOrderExist = 0;
                    int UptOrderStatus = -1;
                    int OrdStatus = 0;
                    string refid = "";
                    int _UserrID;
                    _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
                    oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                    oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                    oOrdInfo = objOrderServices.GetOrder(OrderID);

                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                    {
                        isau = false;
                        OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                    }
                    else if (hfisppp == "1" && drpSM1 != "Counter Pickup")
                    {
                        objErrorHandler.CreateLog("hfispp " + hfisppp);
                        isau = false;
                        OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
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
                        HttpContext.Current.Session["PAYMENT_TYPE"] = PaymentServices.PaymentType.CODPayment;
                        decimal TotCost = objHelperServices.CDEC(objOrderServices.GetOrderTotalCost(OrderID));
                        oPayInfo.PayResponse = "";
                        oPayInfo.PaymentType = PaymentServices.PaymentType.CODPayment;
                        oPayInfo.OrderID = OrderID;
                        oPayInfo.PONumber = objHelperServices.Prepare("");
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                        {

                            refid = objHelperServices.CS("WAG" + OrderID.ToString());
                            oPayInfo.PORelease = refid;
                        }
                        else
                        {
                            if (ttinter_order == "")
                            {
                                ttinter_order = "WAG" + OrderID.ToString();
                                refid = objHelperServices.CS(ttinter_order);
                            }
                            else if (ttinter_order != "")
                            {
                                refid = objHelperServices.CS(ttinter_order);

                            }


                            oPayInfo.PORelease = refid;

                        }
                        oPayInfo.Amount = oOrdInfo.TotalAmount;
                        oPayInfo.UserId = OrderID;
                        objErrorHandler.CreateOrderSummarylog("Inside Proceed Function" + "PORelease :" + refid + "  oPayInfo.Amount :" + oOrdInfo.TotalAmount);
                    }
                    if (objUserServices.GetUserStatus(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString())) == 1)
                    {
                        if (ChkOrderExist == 0)
                        {
                            ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                            int cStatus = 0;
                            if (isau == false)
                                cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                        }
                        else if (ChkOrderExist == 1)
                        {
                            ChkOrderExist = objPaymentServices.UpdatePayment(oPayInfo);
                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                            int cStatus = 0;
                            if (isau == false)
                                cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");

                        }
                        if (isau == false && objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                        {
                            SendMail(OrderID, TextBox1, OrdStatus, isau);
                        }
                        else if (isau == false && drpSM1 != "Counter Pickup")
                        {
                            SendMail_BeforePaymentSO(OrderID,TextBox1, OrdStatus, isau);
                        }

                    }
                    else if (objUserServices.GetUserStatus(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString())) == 4)
                    {
                        if (HttpContext.Current.Session["PAYMENTINFO"] != null)
                        {
                            oPayInfo = (PaymentServices.PayInfo)HttpContext.Current.Session["PAYMENTINFO"];
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


                    }
                }


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }

        private static void SendMail(int OrderId, string TextBox1, int OrderStatus, bool isau)
        {
            string toemail = "";
            OrderServices objOrderServices = new OrderServices();
            HelperServices objHelperServices = new HelperServices();
            PaymentServices objPaymentServices = new PaymentServices();
            PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
            ErrorHandler objErrorHandler = new ErrorHandler();
            NotificationServices objNotificationServices = new NotificationServices();
            int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
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

                int UserID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderID);
                BillAdd = GetBillingAddress(OrderID);
                ShippAdd = GetShippingAddress(OrderID);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = TextBox1.Trim();




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

                    stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
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

                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                    else
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");


                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
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
                    objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";

                }
                if (sHTML != "")
                {


                    string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());

                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();


                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        MessageObj.Bcc.Add(webadminmail);
                        objErrorHandler.CreateLog("order submitted mail to " + Emailadd + "--" + webadminmail + "CustomerOrderNo:" + CustomerOrderNo);
                    }
                    else
                    {
                        emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());
                        // MessageObj.Bcc.Add(addressBCC);
                        objErrorHandler.CreateLog("order submitted mail to " + Emailadd + "CustomerOrderNo:" + CustomerOrderNo);
                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
                    {
                        MessageObj.Subject = "Wagner Order Confirmation - Order No : " + CustomerOrderNo.ToString();
                    }
                    else
                    {
                        MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;


                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);




                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                            objErrorHandler.CreateLog("order submitted mail to " + ApprovedUserEmailadd + "CustomerOrderNo:" + CustomerOrderNo);
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
                                        MessageObj.Subject = "Wagner International Order Alert - Order No : " + CustomerOrderNo.ToString();
                                        MessageObj.To.Clear();

                                        MessageObj.To.Add(id.ToString());
                                        objErrorHandler.CreateLog("order submitted mail to " + id + "CustomerOrderNo:" + CustomerOrderNo);
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
                                    objErrorHandler.CreateLog("order submitted mail to " + Adminemails + "CustomerOrderNo:" + CustomerOrderNo);
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
                                        objErrorHandler.CreateLog("order submitted mail to " + id + "CustomerOrderNo:" + CustomerOrderNo);
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
                                    objErrorHandler.CreateLog("order submitted mail to " + emails + "CustomerOrderNo:" + CustomerOrderNo);
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
                objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();


            }
        }


        private static void SendMail_BeforePaymentSO(int OrderId, string TextBox1, int OrderStatus, bool isau)
        {
            OrderServices objOrderServices = new OrderServices();
            HelperServices objHelperServices = new HelperServices();
            PaymentServices objPaymentServices = new PaymentServices();
            PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
            ErrorHandler objErrorHandler = new ErrorHandler();
            NotificationServices objNotificationServices = new NotificationServices();
            int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
            objErrorHandler.CreateLog("SendMail_BeforePaymentSO ");
            string toemail = "";
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

                int UserID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderID);
                BillAdd = GetBillingAddress(OrderID);
                ShippAdd = GetShippingAddress(OrderID);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = TextBox1.Trim();




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

                    stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
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


                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedPPP");



                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
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
                    objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";

                }
                if (sHTML != "")
                {


                    string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());

                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();


                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        MessageObj.Bcc.Add(webadminmail);

                    }
                    else
                    {
                        emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());
                        // MessageObj.Bcc.Add(addressBCC);

                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
                    {
                        MessageObj.Subject = "Wagner Order Confirmation - Order No : " + CustomerOrderNo.ToString();
                    }
                    else
                    {
                        MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;


                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);




                    if (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2)
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
                                        MessageObj.Subject = "Wagner International Order Alert - Order No : " + CustomerOrderNo.ToString();
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
                objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]), 1);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();


            }
        }
        
        public static string GetImage(int Product_id)
        {
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            ProductServices objProductServices = new ProductServices();
            int family_id = 0;
            family_id = objProductServices.GetFamilyID(Product_id);
            string ImageUrl = string.Empty;
            if (family_id > 0)
            {

                DataTable dt = objProductServices.GetPopProduct(Product_id, family_id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["TWeb Image1"].ToString() != null && dt.Rows[0]["TWeb Image1"].ToString() != String.Empty && dt.Rows[0]["TWeb Image1"].ToString() != "")
                    {
                        ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dt.Rows[0]["TWeb Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
                    }
                    else
                    {
                        ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                    }
                }
            }
            else
            {
                ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
            }
            return ImageUrl;
        }

        public static string GetShippingAddress(int OrderID)
        {
            string sShippingAddress = "";
            OrderServices objOrderServices = new OrderServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
                oOI = objOrderServices.GetOrder(OrderID);

                if (oOI.ShipCompName.Trim().Length > 0)
                    sShippingAddress = oOI.ShipCompName + "<BR>";
                else
                    sShippingAddress = "";

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
                sShippingAddress = sShippingAddress + oOI.ShipPhone + "<BR>";
                if (oOI.DeliveryInstr.Trim().Length > 0)
                {
                    sShippingAddress = sShippingAddress + "<BR>" + oOI.DeliveryInstr + "<BR>";
                }
            }
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }
            return sShippingAddress;
        }

        public static string GetBillingAddress(int OrderID)
        {
            OrderServices objOrderServices = new OrderServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            string sBillingAddress = "";
            try
            {
                OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
                oBI = objOrderServices.GetOrder(OrderID);
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

            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }
            return sBillingAddress;
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

        private string Setdrpdownlistvalue(DropDownList d, string val)
        {
            string returnselected = "";
            try
            {
                ListItem li;
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
                //objErrorHandler.CreatePayLog_Final("hidden fild" + OrderID); 
                //         lblshipcost.Text = CalculateShippingCost(OrderID).ToString();
                // objErrorHandler.CreatePayLog_Final(lblshipcost.Text); 
            }
            catch (Exception ex)
            { }
            return returnselected;
        }

        protected void btnSecurePay_Click(object sender, EventArgs e)
        {
            Session["ORDER_ID_NOTHANKS"] = null;
            decimal UpdRst = 0;
            if (OrderID == 0)
            {
                if (HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                }
            }
            SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            oOrderInfo = objOrderServices.GetOrder(OrderID);
            UserServices objUserServices = new UserServices();
            GetParams();
            objErrorHandler.CreateOrderSummarylog("inside securepay");
            Session["ExpressLevel"] = "3Compl";
     //       SubmitText.Style.Add("display", "none");
     //       PaymentText.Style.Add("display", "block");

            SubmitText.Visible = false;
            PaymentText.Visible = true;
            //if (oOrderInfo.TotalAmount.ToString() != lblAmount.Text)
            //{
            //    objErrorHandler.CreateOrderSummarylog("TotalAmount:" + oOrderInfo.TotalAmount + " " + "lblAmount :" + lblAmount.Text);
            //    //         BtnL4EditShippingMethod_Click(sender, e);                    //commented due to express page
            //    return;
            //}

            if (oOrderInfo.ProdTotalPrice == 0)
            {
                objErrorHandler.CreateOrderSummarylog("btnSecurepay start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }
            if (oOrderInfo.isppppp == 1)
            {
                oOrderInfo.OrderStatus = (int)OrderServices.OrderStatus.Online_Payment;
                UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
            }
            DataSet tmpds = GetOrderItemDetailSum(OrderID);
            decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
            {
                totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            }

            if (oOrderInfo.ProdTotalPrice != totalitemsum)
            {
                objErrorHandler.CreateOrderSummarylog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                //            BtnL4EditShippingMethod_Click(sender, e);               //commented due to express page
                return;
            }
            if (ttOrder.Text == "" && HttpContext.Current.Session["ORDER_ID"] != null)
            {
                oPayInfo.PORelease = "WAG" + HttpContext.Current.Session["ORDER_ID"].ToString();
                oPayInfo.OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                objPaymentServices.UpdatePaymentExpress(oPayInfo);
            }
            else if (ttOrder.Text != "")
            {
                refid = objHelperServices.CS(ttOrder.Text);
                try
                {
                    string sSQL = string.Format("EXEC STP_TBWC_CHECK_PO_NUMBER_EXIST '" + HttpContext.Current.Session["ORDER_ID"].ToString() + "','" + HttpContext.Current.Session["USER_ID"].ToString() + "','" + refid + "'");
                    objErrorHandler.CreateOrderSummarylog("securepay" + " " + sSQL);
                    DataTable DS = objHelperDB.GetDataTableDB(sSQL);

                    if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                    {
                        txterr.Text = "Order No already exists, please Re-enter Order No";
                        ttOrder.Focus();
                        LoadData();
                        Div3.Style.Add("display", "none");
                        txterr.Style.Add("display", "block");
                        ScrollToControl("Level4_Payment", true);
                        return;
                    }
                    else
                    {
                        oPayInfo.PORelease = refid;
                        oPayInfo.OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                        objPaymentServices.UpdatePaymentExpress(oPayInfo);
                    }

                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }
            }

            ttOrder.Enabled = false;

       //     PayOkDiv.Visible = false;
            PayOkDiv.Style.Add("display", "none");
            string rtnstr = "";
            try
            {
                txtCardNumber.Style.Remove("border");
                drpExpmonth.Style.Remove("border");
                drpExpyear.Style.Remove("border");
                txtCardCVVNumber.Style.Remove("border");
            }
            catch (Exception ex)
            {

            }

            try
            {
                if (Session["XpayMS"] != null)
                {
                    if (Convert.ToInt32(Session["XpayMS"]) > 3)
                    {
                        LoadData();
                        div2.InnerHtml = "More than 5 attempt. try again" + "<br/>";
                        //+ "<a href=\"express_checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
              //          div2.Visible = true;
                        div2.Style.Add("display", "block");
                        ScrollToControl("ctl00_maincontent_div2", true);
                        txtCardNumber.Focus();
                        return;
                    }
                    else
                        Session["XpayMS"] = Convert.ToInt32(Session["XpayMS"]) + 1;
                }
                else
                    Session["XpayMS"] = 0;

                oPayInfo = objPaymentServices.GetPayment(OrderID);

                PaymentID = oPayInfo.PaymentID;
                objErrorHandler.CreateOrderSummarylog("PaymentID : " + PaymentID + "orderid : " + OrderID);

                if (oPayInfo.PayResponse.ToLower() == "yes")
                {
                    LoadData();
                    objErrorHandler.CreateOrderSummarylog("Already payment has been made");
                    divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                    Div3.Style.Add("display", "none");
                    divContent.Style.Add("display", "block");
                    ScrollToControl("Level4_Payment", true);
                    return;
                }

                objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
                objErrorHandler.CreateOrderSummarylog(objPRInfo.Error_Text);
                if (objPRInfo.Error_Text != "")
                {
                    LoadData();
                    btnSP.Style.Add("display", "block");
                    BtnProgressSP.Style.Add("display", "none");
                    div2.InnerHtml = "Error found in details you have entered. Please check all fields for errors and try again."; //objPRInfo.Error_Text;
                    txtCardNumber.Focus();
          //          div2.Visible = true;
            //        div2.Style.Add("display", "block");
             //       div2.Style.Add("display", "block");
                    ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_check.png";
                    ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_uncheck.png";

                    ImagePaySP.Style.Add("display", "block");
                    ImagePay.Style.Add("display", "block");

            //        ImagePaySP.Visible = true;
             //       ImagePay.Visible = true;
            //        ScrollToControl("ctl00_maincontent_Level4_Payment", true);
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
            //        PayType.Visible = true;
                    Level3.Style.Add("display", "none");
                    Level4.Style.Add("display", "block");
                    PayType.Style.Add("display", "block");
                    Div3.Style.Add("display", "none");
                    div2.Style.Add("display", "block");
       //             ScrollToControl("Level4_Payment", true);
                    ScrollToControl("ctl00_maincontent_div2", true);
                    
                }
                else
                {
                    Session["XpayMS"] = null;
                    HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                    HttpContext.Current.Session["Mchkout"] = EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                    Response.Redirect("express_checkout.aspx?key=" + EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid"));
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }
        }

        protected void btnPayApi_Click(object sender, EventArgs e)
        {
            //objErrorHandler.CreatePayLog("btnPayApi_Click start Orderid=" + OrderID);
            decimal UpdRst = 0;
            GetParams();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

            renUrl = renUrl.Replace("MCheckOut", "MCheckOut");
            renUrl = renUrl + "?key=" + EncryptSP("Paid");

            oOrderInfo = objOrderServices.GetOrder(OrderID);
            oUserInfo = objUserServices.GetUserInfo(Userid);
            if (oOrderInfo.isppppp == 1)
            {
                oOrderInfo.OrderStatus = (int)OrderServices.OrderStatus.Online_Payment;
                UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
            }
            Session["ExpressLevel"] = "3Compl";
   //         SubmitText.Style.Add("display", "none");
   //         PaymentText.Style.Add("display", "block");
            SubmitText.Visible = false;
            PaymentText.Visible = true;

            if (oPayInfo.PayResponse.ToLower() == "yes")
            {
                LoadData();
                divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                divContent.Style.Add("display", "block");
                return;
            }

            string Requeststr = objPayPalApiService.PayPalSetECRequest(OrderID, PaymentID, oOrderInfo, renUrl);

            if (Requeststr.Contains("Form") == false)
                divContent.InnerHtml = Requeststr;
            else
                this.Page.Controls.Add(new LiteralControl(Requeststr));

            //objErrorHandler.CreatePayLog("btnPayApi_Click end Orderid=" + OrderID);
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            Session["ORDER_ID_NOTHANKS"] = null;
            Session["paypalconfirmation"] = true;
            decimal UpdRst = 0;
            objErrorHandler.CreateOrderSummarylog("btnPay_Click start Orderid=" + OrderID);
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            if (OrderID == 0)
            {
                if (HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                }
            }

            oOrderInfo = objOrderServices.GetOrder(OrderID);
            UserServices objUserServices = new UserServices();
            GetParams();
            objErrorHandler.CreateOrderSummarylog("inside Paypal");
            Session["ExpressLevel"] = "3Compl";
            //        SubmitText.Style.Add("display", "none");
            //         PaymentText.Style.Add("display", "block");

            SubmitText.Visible = false;
            PaymentText.Visible = true;
            //if (oOrderInfo.TotalAmount.ToString() != lblpaypaltotamt.Text)
            //{
            //    objErrorHandler.CreateOrderSummarylog("TotalAmount: " + oOrderInfo.TotalAmount + " " + "lblAmount : " + lblAmount.Text);
            //    //         BtnL4EditShippingMethod_Click(sender, e);                               //commented due to express page
            //    return;
            //}

            if (oOrderInfo.ProdTotalPrice == 0)
            {
                objErrorHandler.CreateOrderSummarylog("btnpaypal start Orderid= " + OrderID + "ProdTotalPrice" + "0");
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }
            if (oOrderInfo.isppppp == 1)
            {
                oOrderInfo.OrderStatus = (int)OrderServices.OrderStatus.Online_Payment;
                UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
            }

            DataSet tmpds = GetOrderItemDetailSum(OrderID);
            decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
            {
                totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            }

            if (oOrderInfo.ProdTotalPrice != totalitemsum)
            {
                objErrorHandler.CreateOrderSummarylog("Prodtitalprice: " + oOrderInfo.ProdTotalPrice + " " + "totalitemsum : " + totalitemsum);
                //          BtnL4EditShippingMethod_Click(sender, e);                                 //commented due to express page
                return;
            }

            if (ttOrder.Text == "" && HttpContext.Current.Session["ORDER_ID"] != null)
            {
                //ttOrder.Text = "WAG" + HttpContext.Current.Session["ORDER_ID"].ToString();

                oPayInfo.PORelease = "WAG" + HttpContext.Current.Session["ORDER_ID"].ToString();
                oPayInfo.OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                objPaymentServices.UpdatePaymentExpress(oPayInfo);
            }
            else if (ttOrder.Text != "")
            {
                refid = objHelperServices.CS(ttOrder.Text);
                try
                {
                    string sSQL = string.Format("EXEC STP_TBWC_CHECK_PO_NUMBER_EXIST '" + HttpContext.Current.Session["ORDER_ID"].ToString() + "','" + HttpContext.Current.Session["USER_ID"].ToString() + "','" + refid + "'");
                    objErrorHandler.CreateLog("paypal" + " " + sSQL);
                    DataTable DS = objHelperDB.GetDataTableDB(sSQL);

                    if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                    {
                        LoadData();
                        txterr.Text = "Order No already exists, please Re-enter Order No";
                        ttOrder.Focus();
                        SecurePayAcc.Style.Add("display", "none");
                        ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                        ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                        PayPaypalAcc.Style.Add("display", "block");
                        Level3.Style.Add("display", "none");
                        Level4.Style.Add("display", "block");
                        Div3.Style.Add("display", "none");
                        txterr.Style.Add("display", "block");

                        ScrollToControl("Level4_Payment", false);
                        return;
                    }
                    else
                    {
                        oPayInfo.PORelease = refid;
                        oPayInfo.OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                        objPaymentServices.UpdatePaymentExpress(oPayInfo);
                    }
                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }
            }
            ttOrder.Enabled = false;
            //      PayOkDiv.Visible = false;
            PayOkDiv.Style.Add("display", "none");

            SecurePayAcc.Style.Add("display", "none");
            GetParams();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

            renUrl = renUrl.Replace("MCheckOut", "MCheckOut");
            renUrl = renUrl + "?key=" + EncryptSP("Paid");

            oUserInfo = objUserServices.GetUserInfo(Userid);
            oPayInfo = objPaymentServices.GetPayment(OrderID);
            PaymentID = oPayInfo.PaymentID;
            if (oPayInfo.PayResponse.ToLower() == "yes")
            {
                LoadData();
                divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                divContent.Style.Add("display", "block");
                return;
            }
            objErrorHandler.CreateOrderSummarylog("Before PayPalInitRequest_ExpressCheckout " + "OrderID : " + OrderID + "PaymentID : " + PaymentID + "renUrl : " + renUrl);
            string Requeststr = objPayPalService.PayPalInitRequest_ExpressCheckout(OrderID, PaymentID, oOrderInfo, renUrl);

            if (Requeststr.Contains("Form") == false)
            {
                objErrorHandler.CreateLog(Requeststr + "to open new page");
                divContent.InnerHtml = Requeststr;
            }
            else
            {
                objErrorHandler.CreateLog(Requeststr + "to open new page");
                this.Page.Controls.Add(new LiteralControl(Requeststr));
            }

            //      btnPay.Visible = false;
            //      BtnProgress.Visible = true;
            btnPay.Style.Add("display", "none");
            BtnProgress.Style.Add("display", "block");
        }

        public static void Combian_Prodid(int order_id, string productid)
        {
            HelperServices objHelperServices = new HelperServices();
            OrderServices objOrderServices = new OrderServices();
            string LeaveDuplicateProds = GetLeaveDuplicateProducts();
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            String sessionId;
            sessionId = HttpContext.Current.Session.SessionID;

            int PrdId = objHelperServices.CI(productid);

            DataSet dsDuplicateItem = objOrderServices.GetOrderItemsWithDuplicate(order_id, LeaveDuplicateProds, sessionId);
            if (dsDuplicateItem != null && dsDuplicateItem.Tables.Count > 0 && dsDuplicateItem.Tables[0].Rows.Count > 0)
            {
                DataRow[] Dr = dsDuplicateItem.Tables[0].Select("PRODUCT_ID='" + PrdId + "'");

                if (Dr.Length > 0)
                {
                    DataTable temptbl = Dr.CopyToDataTable();


                    decimal TotQty = 0;

                    foreach (DataRow row in temptbl.Rows)
                    {
                        TotQty = TotQty + objHelperServices.CI(row["QTY"].ToString());
                        int nQty = objHelperServices.CI(row["QTY"].ToString());
                        string order_item_id = row["ORDER_ITEM_ID"].ToString();
                        objOrderServices.RemoveItem(PrdId.ToString(), order_id, objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), order_item_id.ToString());
                    }

                    staticAddOrderItem(order_id, objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), (int)TotQty, PrdId);
                    int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

                    if (OrderID > 0)
                        oOrdInfo.OrderID = OrderID;
                    else
                        oOrdInfo.OrderID = order_id;
                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                }
            }
        }

        protected static decimal CalculateShippingCost(int OID, int ProdId, decimal ProdApplyPrice, int itemQty)
        {
            bool _IsShippingFree = false;
            DataSet dsOItem = new DataSet();
            OrderServices objOrderServices = new OrderServices();
            HelperServices objHelperServices = new HelperServices();
            decimal ShippingValue = 0;
            decimal ProdShippCost = 0;
            dsOItem = objOrderServices.GetItemDetailsFromInventory(ProdId);
            decimal ProductCost = (ProdApplyPrice * itemQty);
            if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
            {
                if (dsOItem != null)
                {
                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                    {
                        if (objHelperServices.CB(rItem["IS_SHIPPING"]) == 1)
                        {
                            ProdShippCost = ((ProductCost * objHelperServices.CDEC(rItem["PROD_SHIP_COST"])) / 100);
                        }
                    }
                }
            }
            else
            {
                if ((objOrderServices.GetCurrentProductTotalCost(OID) + ProdApplyPrice) < objHelperServices.CDEC(objHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
                {
                    if (dsOItem != null)
                    {
                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                        {
                            ProdShippCost = ((ProductCost * objHelperServices.CI(objHelperServices.GetOptionValues("SHIPPING CHARGE").ToString())) / 100);
                        }
                    }
                }
                else
                {
                    _IsShippingFree = true;
                }
            }
            return objHelperServices.CDEC(ProdShippCost);
        }

        public void AddOrderItem(int OrID, int UsrID, int qty, int Product_id)
        {
            try
            {

                objErrorHandler.CreateOrderSummarylog("Add OrderItem inside express checkout" + "orderid :" + OrID + "userid :" + UsrID + "qty :" + qty + "Product_id :" + Product_id);
                OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
                decimal untPrice = 0.00M;
                DataSet dsBgPrice = new DataSet();
                DataSet dsBgDisc = new DataSet();

                if (objProductPromotionServices.CheckPromotion(Product_id))
                {
                    decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));

                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);

                    DiscPrice = (untPrice * DiscPrice) / 100;
                    untPrice = untPrice - DiscPrice;
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));

                }
                else
                {
                    //Check the user default buyer group or custome buyer group.
                    int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                    string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                    if (BGPriceID == 4 && BGName == "DEFAULTBG")
                    {
                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                    }
                    else
                    {


                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                        //To calculate the discount price.
                        dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                {
                                    ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                }
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                {
                                    untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                }
                            }
                        }
                        untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));



                    }
                }//Buyergroup price.
                oItemInFo.ORDER_ITEM_ID = 0;
                oItemInFo.ProductID = Product_id;
                oItemInFo.OrderID = OrID;
                oItemInFo.PriceApplied = untPrice;
                oItemInFo.UserID = UsrID;
                oItemInFo.Quantity = qty;
                oItemInFo.Ship_Cost = CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));



                // if (Convert.ToInt32(Session["USER_ID"].ToString()) == Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                //    oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount_Express_DumUser(oItemInFo.Quantity * untPrice, OrID.ToString());
                //else
                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());

                String sessionId;
                sessionId = Session.SessionID;
                if (sessionId != "")
                    oItemInFo.PRD_SESSION_ID = sessionId;
                else
                    oItemInFo.PRD_SESSION_ID = "";

                objOrderServices.AddOrderItem(oItemInFo);


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }


        public static void staticAddOrderItem(int OrID, int UsrID, int qty, int Product_id)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            HelperServices objHelperServices = new HelperServices();
            OrderServices objOrderServices = new OrderServices();
            ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
            BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();
            HelperDB objHelperDB = new HelperDB();
            try
            {

                objErrorHandler.CreateOrderSummarylog("Add OrderItem inside express checkout" + "orderid :" + OrID + "userid :" + UsrID + "qty :" + qty + "Product_id :" + Product_id);
                OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
                decimal untPrice = 0.00M;
                int CatalogID = 0;
                DataSet dsBgPrice = new DataSet();
                DataSet dsBgDisc = new DataSet();

                if (objProductPromotionServices.CheckPromotion(Product_id))
                {
                    decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));

                    string user_id = HttpContext.Current.Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);

                    DiscPrice = (untPrice * DiscPrice) / 100;
                    untPrice = untPrice - DiscPrice;
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));

                }
                else
                {
                    //Check the user default buyer group or custome buyer group.
                    int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                    string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                    if (BGPriceID == 4 && BGName == "DEFAULTBG")
                    {
                        string user_id = HttpContext.Current.Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                    }
                    else
                    {


                        string user_id = HttpContext.Current.Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                        //To calculate the discount price.
                        dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                {
                                    ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                }
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                {
                                    untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                }
                            }
                        }
                        untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));



                    }
                }//Buyergroup price.
                oItemInFo.ORDER_ITEM_ID = 0;
                oItemInFo.ProductID = Product_id;
                oItemInFo.OrderID = OrID;
                oItemInFo.PriceApplied = untPrice;
                oItemInFo.UserID = UsrID;
                oItemInFo.Quantity = qty;
                oItemInFo.Ship_Cost = CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));



                // if (Convert.ToInt32(Session["USER_ID"].ToString()) == Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                //    oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount_Express_DumUser(oItemInFo.Quantity * untPrice, OrID.ToString());
                //else
                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());

                String sessionId;
                sessionId = HttpContext.Current.Session.SessionID;
                if (sessionId != "")
                    oItemInFo.PRD_SESSION_ID = sessionId;
                else
                    oItemInFo.PRD_SESSION_ID = "";

                objOrderServices.AddOrderItem(oItemInFo);


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }


        protected void ImageButton1_Click(object sender, EventArgs e)
        {
            try
            {

                if (ttOrder.Text == "")
                {
                    Session["ORDER_NO"] = refid;
                }
                Session["SHIPPING"] = drpSM1.Text.Trim();
                Session["DELIVERY"] = TextBox1.Text.Trim();
                //Session["DROPSHIP"] = txtCompany.Text.Trim() + "####" + txtAttentionTo.Text.Trim()
                //        + "####" + txtAddressLine1.Text.Trim()
                //        + "####" + txtAddressLine2.Text.Trim()
                //        + "####" + txtSuburb.Text.Trim()
                //        + "####" + drpState.Text.Trim()
                //        + "####" + txtCountry.Text.Trim()
                //        + "####" + txtPostCode.Text.Trim()
                //        + "####" + txtDeliveryInstructions.Text.Trim()
                //        + "####" + txtShipPhoneNumber.Text.Trim()
                //    ;


                if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
                {
                    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                    {
                        if (lblAmount.Text != "" || lblpaypaltotamt.Text != "")
                        {
                            Session["tabl3" + OrderID] = lbDeliveryText.Text;
                        }
                        Response.Redirect("OrderDetailsExpress.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + Session["ORDER_ID"], false);
                    }
                    else
                    {
                        Response.Redirect("OrderDetailsExpress.aspx?bulkorder=1&amp;Pid=0", false);
                    }
                }

            }
            catch (Exception Ex)
            {

            }
        }

        public static string getUserName()
        {
            string retvalue = string.Empty;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            DataTable objDt = new DataTable();
            string _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
            HelperDB objHelperDB = new HelperDB();
            ErrorHandler objErrorHandler = new ErrorHandler();
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

        public static string Formval(NameValue[] formVars, string name)
        {
            var matches1 = formVars.Where(nv => nv.name.ToLower().Contains(name.ToLower())).FirstOrDefault();
            if (matches1 != null)
                return matches1.value;

            var matches = formVars.Where(nv => nv.name.ToLower() == name.ToLower()).FirstOrDefault();
            if (matches != null)
                return matches.value;


            return string.Empty;
        }


        public static int UpdateUserData(NameValue[] formVars)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            HelperServices objHelperServices = new HelperServices();
            UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            CountryServices objCountryServices = new CountryServices();
            try
            {
                string BusinessDsc = string.Empty;
                string comp_ABN = string.Empty;
               
                ouserinfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                HttpContext.Current.Session["ShipCountry"] = ouserinfo.ShipCountry.ToString();
                oOrdBillInfo.UserID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());

                oOrdBillInfo.COMPANY_NAME = objHelperServices.Prepare(Formval(formVars, "txtComname"));
                oOrdBillInfo.DELIVERYINST = objHelperServices.Prepare(Formval(formVars, "txtDELIVERYINST"));

                if (Formval(formVars, "txt_attnto") == ouserinfo.FirstName.Trim() + " " + ouserinfo.LastName.Trim())
                {
                    oOrdBillInfo.Receiver_Contact = ouserinfo.FirstName.Trim() + " " + ouserinfo.LastName.Trim();
                }
                else
                {
                    oOrdBillInfo.Receiver_Contact = objHelperServices.Prepare(Formval(formVars, "txt_attnto"));
                }

                oOrdBillInfo.ShipAddress1 = objHelperServices.Prepare(Formval(formVars, "txtsadd"));
                oOrdBillInfo.ShipAddress2 = objHelperServices.Prepare(Formval(formVars, "txtadd2"));
                oOrdBillInfo.ShipAddress3 = "";
                oOrdBillInfo.ShipCity = objHelperServices.Prepare(Formval(formVars, "txttown"));
          //      oOrdBillInfo.ShipState = objHelperServices.Prepare(Formval(formVars, "txtstate"));
                oOrdBillInfo.ShipZip = Formval(formVars, "txtzip");
        //        oOrdBillInfo.ShipCountry = Formval(formVars, "txtcountry");
                oOrdBillInfo.ShipCountry = objCountryServices.GetCountryName(Formval(formVars, "drpcountry")).Tables[0].Rows[0]["COUNTRY_NAME"].ToString();
                if (oOrdBillInfo.ShipCountry.ToString().ToLower() == "australia")
                {
                    oOrdBillInfo.ShipState = objHelperServices.Prepare(Formval(formVars, "drpstate"));
                }
                else
                {
                    oOrdBillInfo.ShipState = objHelperServices.Prepare(Formval(formVars, "txtstate"));
                }

                if (Formval(formVars, "ChkBillingAdd") == "on")
                {
                    oOrdBillInfo.BillAddress1 = oOrdBillInfo.ShipAddress1;
                    oOrdBillInfo.BillAddress2 = oOrdBillInfo.ShipAddress2;
                    oOrdBillInfo.BillAddress3 = "";
                    oOrdBillInfo.BillCity = oOrdBillInfo.ShipCity;
                    oOrdBillInfo.BillState = oOrdBillInfo.ShipState;
                    oOrdBillInfo.BillZip = oOrdBillInfo.ShipZip;
                    oOrdBillInfo.BillCountry = oOrdBillInfo.ShipCountry;
                    oOrdBillInfo.Bill_Company = oOrdBillInfo.Receiver_Contact;
                }
                else
                {
                    oOrdBillInfo.BillAddress1 = objHelperServices.Prepare(Formval(formVars, "txtsadd_Bill"));
                    oOrdBillInfo.BillAddress2 = objHelperServices.Prepare(Formval(formVars, "txtadd2_Bill"));
                    oOrdBillInfo.BillAddress3 = "";
                    oOrdBillInfo.BillCity = objHelperServices.Prepare(Formval(formVars, "txttown_Bill"));
                    oOrdBillInfo.BillZip = Formval(formVars, "txtzip_bill");
                    oOrdBillInfo.Bill_Company = objHelperServices.Prepare(Formval(formVars, "txtbillbusname"));
                    oOrdBillInfo.Bill_Name = objHelperServices.Prepare(Formval(formVars, "txtbillname"));
           //         oOrdBillInfo.BillState = objHelperServices.Prepare(Formval(formVars, "txtstate_Bill"));
           //         oOrdBillInfo.BillCountry = Formval(formVars, "txtcountry_Bill");
                    oOrdBillInfo.BillCountry = objCountryServices.GetCountryName(Formval(formVars, "drpcountry_Bill")).Tables[0].Rows[0]["COUNTRY_NAME"].ToString();
                    if (oOrdBillInfo.BillCountry.ToString().ToLower() == "australia")
                    {
                        oOrdBillInfo.BillState = objHelperServices.Prepare(Formval(formVars, "drpstate_Bill"));
                    }
                    else
                    {
                        oOrdBillInfo.BillState = objHelperServices.Prepare(Formval(formVars, "txtstate_Bill"));
                    }
                }
                return objUserServices.UpdateUserInfoExpress(oOrdBillInfo, BusinessDsc, comp_ABN);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public static int SaveUserProfileInit(string letstarttxtemail, string txtRegpassword)
        {
            UserServices.RegistrationInfo oRegInfo = new UserServices.RegistrationInfo();
            UserServices objUserServices = new UserServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            Security objcrpengine = new Security();
            try
            {
                oRegInfo.Customer_Type = "Retailer";

                oRegInfo.CompanyName = "";
                oRegInfo.AbnAcn = "";
                oRegInfo.Address1 = "";
                oRegInfo.Address2 = "";
                oRegInfo.SubCity = "";
                oRegInfo.State = "";
                oRegInfo.Country = "Australia";
                oRegInfo.PostZipcode = "";
                oRegInfo.Fname = "";
                oRegInfo.Lname = "";
                oRegInfo.Position = "";
                oRegInfo.Phone = "";
                oRegInfo.Mobile = "";
                oRegInfo.Fax = "";
                oRegInfo.Email = letstarttxtemail.ToString();
                oRegInfo.BusinessType = "NA";
                oRegInfo.BusinessDsc = "Personal";
                oRegInfo.SiteID = "3";
                oRegInfo.CustStatus = "False";
                oRegInfo.Status = "I";
                oRegInfo.RegType = "N";
                string Password = txtRegpassword;
                string Newpassword = objcrpengine.StringEnCrypt_password(Password.ToString());
                oRegInfo.Password = Newpassword;
                string clientIPAddress = "";
                oRegInfo.IpAddr = clientIPAddress;

                oRegInfo.LastInvNo = "N/A";
                oRegInfo.WesAccNo = "N/A";

                return objUserServices.CreateRegistration(oRegInfo);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public static int SaveUserProfile(string txtRegFname, string txtRegLname, string txtRegphone, string txtregemail, string txtRegMobilePhone, string txtRegpassword)
        {
            UserServices.RegistrationInfo oRegInfo = new UserServices.RegistrationInfo();
            UserServices objUserServices = new UserServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            Security objcrpengine = new Security();
            try
            {

                oRegInfo.Fname = txtRegFname.ToString();
                oRegInfo.Lname = txtRegLname.ToString();
                oRegInfo.Phone = txtRegphone.ToString();
                oRegInfo.Email = txtregemail.ToString();
                oRegInfo.Mobile = txtRegMobilePhone.ToString();
                oRegInfo.SiteID = "3";
                string Password = txtRegpassword.Trim();
                string Newpassword = objcrpengine.StringEnCrypt_password(Password.ToString());
                oRegInfo.Password = Newpassword;
                string clientIPAddress = "";

                if (HttpContext.Current.Session["IP_ADDR"] != null && HttpContext.Current.Session["IP_ADDR"].ToString() != "")
                    clientIPAddress = HttpContext.Current.Session["IP_ADDR"].ToString();
                else
                    clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                oRegInfo.IpAddr = clientIPAddress;
                return objUserServices.UpdateRegistrationExpress(oRegInfo);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        //private static void GetParams()
        //{
        //    try
        //    {
        //        if (Request.Url.Query != null && Request.Url.Query != "")
        //        {

        //            string id = null;

        //            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"].ToString() != "" && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0)
        //            {
        //                Userid = Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString());
        //            }
        //            if (Request.Url.Query.Contains("key=") == false)
        //            {

        //                id = Request.Url.Query.Replace("?", "");
        //                int n;
        //                bool isNumeric = int.TryParse(id, out n);
        //                // objErrorHandler.CreatePayLog_Final("Getparams" + id);

        //                if (Request.Url.Query.Contains("Editcart=true") == false)
        //                {
        //                    id = DecryptSP(id);
        //                }
        //                else
        //                {
        //                    id = DecryptSP(id.Replace("&Editcart=true", ""));
        //                }

        //                if ((id == null) && (isNumeric == true))
        //                {
        //                    id = Request.Url.Query.Replace("?", "");
        //                    if (id.Contains("PaySP") == false)
        //                    {
        //                        id = id + "#####" + "PaySP";
        //                    }
        //                }
        //            }

        //            else
        //            {

        //                if (HttpContext.Current.Session["Mchkout"] != null)
        //                {
        //                    id = HttpContext.Current.Session["Mchkout"].ToString();
        //                    id = DecryptSP(id);
        //                }
        //            }
        //            if (id != null)
        //            {
        //                string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);

        //                OrderID = objHelperServices.CI(ids[0]);

        //                //add new
        //                int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;
        //                if (Userid != 999 || OrderID == 0)
        //                {
        //                    OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
        //                }


        //                OrderIDaspx = OrderID;

        //                if (ids.Length > 1)
        //                {

        //                    PaytabType = ids[1];
        //                    oPayInfo = objPaymentServices.GetPayment(OrderID);
        //                    if (oPayInfo.PaymentID == 0)
        //                    {
        //                        Session["ExpressLevel"] = "2Compl";
        //                        Paytab = false;
        //                    }
        //                    {
        //                        Paytab = true;
        //                    }

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
        //                div2.InnerHtml = "Invalid Data";
        //                div2.Visible = true;
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
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.ToString());
        //    }

        //}


        protected static decimal CalculateShippingCost(string drpSM1, int OrderID, string hfisppp)
        {
            OrderServices objOrderServices = new OrderServices();
            HelperServices objHelperServices = new HelperServices();
            DataSet dsOItem = new DataSet();
            decimal ShippingValue = 0.00M;
            dsOItem = objOrderServices.GetOrderItems(OrderID);
            decimal ProductCost;

            if (drpSM1.ToString().Trim() == "Standard Shipping")
            {
                if (hfisppp == "0")
                {
                    string shipcost = objHelperServices.GetOptionValues("COURIER CHARGE");
                    if (shipcost != "")
                        ShippingValue = Convert.ToDecimal(shipcost);
                }

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



        [System.Web.Services.WebMethod]
        public static String BtnGetStartContinueMethod(string letstarttxtemail, string txtregemail)
        {
            int i = 1;
            UserServices objUserServices = new UserServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                bool tmp = objUserServices.CheckUserRegisterEmail(letstarttxtemail.Trim(), "Retailer");
                if (txtregemail != "" && txtregemail != null && txtregemail != letstarttxtemail)
                {
                    tmp = objUserServices.CheckUserRegisterEmail(txtregemail.Trim(), "Retailer");
                    if (tmp == true)
                    {
                        letstarttxtemail = txtregemail;
                    }
                    else
                    {
                        letstarttxtemail = txtregemail;
                        i = 1;
                    }
                }
                if (tmp == false)
                {
                    if (i > 0)
                    {
                        txtregemail = letstarttxtemail;
                        return "1".ToString();
                    }
                }
                else
                {
                    return "2".ToString();
                }
                return "false".ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                return "false".ToString();
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static Object BtnGetStartLoginMethod(string letstarttxtemail, string letstarttxtpwd)
        {
            bool validUser;
            string username;
            string password;
            DataSet tmpds = null;
            int UserID;
            int OrderID = 0;
            DataSet tmpdsdealercheck = null;
            string returnval = string.Empty;
            OrderID=Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

            Security objSecurity = new Security();
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            OrderServices objOrderServices = new OrderServices();
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();

            username = letstarttxtemail;
            password = letstarttxtpwd;
            password = objSecurity.StringEnCrypt_password(letstarttxtpwd);

            if (objHelperServices.ValidateEmail(username) == true)
            {
                tmpds = objUserServices.CheckMultipleUserMail(username);
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    if (tmpds.Tables[0].Rows.Count > 1)
                    {
                        //               lblErrMsg.Text = "Email Id is associated with multiple login";
                        return "Email Id is associated with multiple login";
                    }
                    else
                    {
                        username = tmpds.Tables[0].Rows[0]["LOGIN_NAME"].ToString();
                    }
                }
            }
            tmpdsdealercheck = objUserServices.CheckMultipleUserMail(username);
            if (tmpdsdealercheck != null && tmpdsdealercheck.Tables.Count > 0 && tmpdsdealercheck.Tables[0].Rows.Count > 0)
            {
                if (tmpdsdealercheck.Tables[0].Rows[0]["CUSTOMER_TYPE"].ToString() == "Dealer")
                {
                    //             lblErrMsg.Text = "Invalid Login Id";
                    return "Invalid Login Id";
                }
            }

            validUser = objUserServices.CheckUserName(username);
            UserID = objUserServices.GetUserID(username);

            if (UserID != -1 && username != string.Empty)
            {
                if (objUserServices.CheckUser(username, password))
                {
                    string Role;
                    Role = objUserServices.GetRole(UserID);
                     


                    if (objUserServices.GetUserStatus(UserID) != 4)
                    {
                        String sessionId;
                        sessionId = HttpContext.Current.Session.SessionID;
                        int orditemuseridupdate = 0;
                        int newordid = 0;
                        int getnewordid = 0;
                        int order_id = 0;

                        int chkcurordid = 0;

                        oOrdInfo.UserID = UserID;
                        chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);
                        if (chkcurordid == 0)
                            newordid = objOrderServices.InitilizeOrder(oOrdInfo);
                        getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);

                        orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(OrderID, UserID, sessionId, getnewordid, sessionId);


                        if (getnewordid > 0)
                        {
                            HttpContext.Current.Session["ORDER_ID_NOTHANKS"] = "0";
                            HttpContext.Current.Session["ORDER_ID"] = getnewordid;
                            HttpContext.Current.Session["USER_ID"] = UserID;
                            HttpContext.Current.Session["EXPRESS_CHECKOUT"] = "True";
                            HttpContext.Current.Session["USER_ROLE"] = Role;
                            oOrdInfo.OrderID = getnewordid;
                            OrderID = getnewordid;
                            objOrderServices.UpdateOrderPrice_ExpressCheckout(oOrdInfo, true, sessionId);
                        }
                        else
                        {
                            HttpContext.Current.Session["ORDER_ID"] = chkcurordid;
                            HttpContext.Current.Session["USER_ID"] = UserID;
                            HttpContext.Current.Session["EXPRESS_CHECKOUT"] = "True";
                            HttpContext.Current.Session["USER_ROLE"] = Role;
                            oOrdInfo.OrderID = chkcurordid;
                            OrderID = chkcurordid;
                        }


                        ouserinfo = objUserServices.GetUserInfo(UserID);

                        HttpContext.Current.Session["USER_NAME"] = ouserinfo.LoginName;

                        HttpContext.Current.Session["USER_ROLE"] = ouserinfo.USERROLE;
                        HttpContext.Current.Session["COMPANY_ID"] = ouserinfo.CompanyID;
                        HttpContext.Current.Session["CUSTOMER_TYPE"] = ouserinfo.CUSTOMER_TYPE;
                        HttpContext.Current.Session["DUMMY_FLAG"] = "1";
                        HttpContext.Current.Session["Emailid"] = ouserinfo.AlternateEmail;
                        HttpContext.Current.Session["Firstname"] = ouserinfo.FirstName;
                        HttpContext.Current.Session["Lastname"] = ouserinfo.LastName;
                        HttpContext.Current.Session["LOGIN_NAME_TOP"] = ouserinfo.Contact;

                        oOrdShippInfo = objUserServices.GetUserShipInfo(UserID);

                        //if (oOrdShippInfo.ShipCountry != "" && oOrdShippInfo.ShipAddress1 != "")
                        //{
                        //    returnval = "proceed to level3";
                        //    Level1.Visible = false;
                        //    Level3.Visible = true;
                        //    Level4.Visible = false;
                        //    Level2.Visible = false;

                        //    L3Name.Text = ouserinfo.Contact;
                        //    L3Email.Text = ouserinfo.AlternateEmail;
                        //    L3Phone.Text = ouserinfo.Phone;
                        //    L3ship_company_name.Text = ouserinfo.COMPANY_NAME;
                        //    if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                        //    {
                        //        L3ship_attn.Text = ouserinfo.Contact;
                        //    }
                        //    else
                        //    {
                        //        L3ship_attn.Text = ouserinfo.Receiver_Contact;
                        //    }
                        //    L3Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                        //    L3Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                        //    L3Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                        //    L3Ship_State.Text = oOrdShippInfo.ShipState;
                        //    L3Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                        //    L3Ship_Country.Text = oOrdShippInfo.ShipCountry;
                        //    L3Ship_DELIVERYINST.Text = oOrdShippInfo.DELIVERYINST;
                        //    ScrollToControl("ctl00_maincontent_divl3continue", true);
                        //}
                        //else
                        //{
                        //    returnval = "proceed to level2";
                        //    Level2.Visible = true;
                        //    BtnL2Continue.Focus();
                        //    ScrollToControl("ctl00_maincontent_l2div", false);
                        //    Level1.Visible = false;
                        //    Level3.Visible = false;
                        //    Level4.Visible = false;
                        //    //shipping
                        //    L2name.Text = ouserinfo.Contact;
                        //    L2Email.Text = ouserinfo.AlternateEmail;
                        //    L2Phone.Text = ouserinfo.Phone;

                        //    txtComname.Text = ouserinfo.COMPANY_NAME;

                        //    txt_attnto.Text = ouserinfo.Contact;

                        //    txtsadd.Text = ouserinfo.ShipAddress1;
                        //    txtadd2.Text = ouserinfo.ShipAddress2;
                        //    txttown.Text = ouserinfo.ShipCity;
                        //    drpCountry.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.ShipCountry);

                        //    if (ouserinfo.ShipCountry.ToLower() == "australia")
                        //    {
                        //        drpstate1.Visible = true;
                        //        txtzip.Visible = true;
                        //        txtzip.Text = ouserinfo.ShipZip;
                        //        txtzip_inter.Visible = false;
                        //        txtstate.Visible = false;
                        //        rfvstate.Enabled = false;
                        //        aucust.Visible = true;
                        //        intercust.Visible = false;
                        //        Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
                        //    }
                        //    else
                        //    {
                        //        txtzip.Visible = false;
                        //        txtzip_inter.Visible = true;
                        //        drpstate1.Visible = false;
                        //        txtzip_inter.Text = ouserinfo.ShipZip;
                        //        txtstate.Visible = true;
                        //        txtstate.Text = ouserinfo.ShipState;
                        //        rfvddlstate.Enabled = false;
                        //        aucust.Visible = false;
                        //        intercust.Visible = true;
                        //    }


                        //    //Billing
                        //    txtsadd_Bill.Text = ouserinfo.BillAddress1;
                        //    txtadd2_Bill.Text = ouserinfo.BillAddress2;
                        //    txttown_Bill.Text = ouserinfo.BillCity;

                        //    drpcountry_bill.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.BillCountry);

                        //    if (ouserinfo.BillCountry.ToLower() == "australia")
                        //    {
                        //        drpstate2.Visible = true;
                        //        txtzip_bill.Text = ouserinfo.BillZip;
                        //        txtstate_Bill.Visible = false;
                        //        txtstate_Bill.Style.Add("display", "none");
                        //        RequiredFieldValidator14.Enabled = false;

                        //        Setdrpdownlistvalue(drpstate2, ouserinfo.BillState.ToString());
                        //    }
                        //    else
                        //    {
                        //        txtstate_Bill.Visible = true;
                        //        txtstate_Bill.Style.Add("display", "block");
                        //        txtstate_Bill.Text = ouserinfo.BillState;
                        //        txtzip_bill.Text = ouserinfo.BillZip;
                        //        drpstate2.Visible = false;
                        //        RequiredFieldValidator13.Enabled = false;
                        //    }

                        //    if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower() && ouserinfo.BillZip == ouserinfo.ShipZip && ouserinfo.BillState == ouserinfo.ShipState && ouserinfo.BillAddress1 == ouserinfo.ShipAddress1)
                        //    {
                        //        ChkBillingAdd.Checked = true;
                        //        L2DivBilling.Visible = false;
                        //    }
                        //    else
                        //    {

                        //        ChkBillingAdd.Checked = false;
                        //        L2DivBilling.Visible = true;
                        //    }



                        //    if (objOrderServices.IsNativeCountry_Express_userid(UserID) == 1)
                        //    {
                        //        if (Page.IsPostBack)
                        //        {
                        //            drpSM1.Items.Clear();
                        //            drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                        //            drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                        //            drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                        //            drpSM1.SelectedIndex = 0;
                        //            intorder.Visible = false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        drpSM1.Items.Clear();
                        //        drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                        //        drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                        //        drpSM1.SelectedIndex = 1;
                        //        intorder.Visible = true;
                        //    }


                        //    ScrollToControl("ctl00_maincontent_l2div", false);
                        //    txtsadd_Bill.Focus();

                        //}
                        string LeaveDuplicateProds = "";
                        DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds, sessionId);

                        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
                        {
                            int DupProdCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
                            for (int i = 0; i <= DupProdCount; i++)
                            {
                                 Combian_Prodid(OrderID, dsDuplicateItem_Prod_id.Tables[0].Rows[0][0].ToString());         //commented for checking

                            }
                        }
                        if (objOrderServices.IsNativeCountry_Express_userid(UserID) == 1)
                        {
                            //if (Page.IsPostBack)
                            //{
                            //    drpSM1.Items.Clear();
                            //    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                            //    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                            //    drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                            //    drpSM1.SelectedIndex = 0;
                            //    intorder.Visible = false;
                            //}
                        }
                        else
                        {
                            //drpSM1.Items.Clear();
                            //drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                            //drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                            //drpSM1.SelectedIndex = 1;
                            //intorder.Visible = true;

                        }

                        //letstartregister.Visible = false;
                        //startdivpwd.Visible = false;
                        //getstartwelcome.Visible = false;
                        //BtnGetStartLogin.Visible = false;
                        //letstartdivlogin.Visible = false;
                        //startdivpwd.Visible = false;
                        //getstartwelcome.Visible = true;
                        //BtnGetStartLogin.Visible = false;
                        //letstartregister.Visible = false;
                        //BtnGetStartContinue.Visible = false;

                        //Session["ExpressLevel"] = "2Compl";
                        //               var sdff = JsonConvert.SerializeObject(ouserinfo) + "" + JsonConvert.SerializeObject(oOrdShippInfo);
                        //               string stringval = ouserinfo.ToString() + oOrdShippInfo.ToString();
                        object orderInfo = objOrderServices.GetOrder(Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString()));
                        object userInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                        returnval = JsonConvert.SerializeObject(new { userInfo, orderInfo });
                        return returnval;

                        //                   return ouserinfo;
                    }
                }
                else
                {
                    //             lblErrMsg.Text = "Invalid Password";
                    return "Invalid Password";
                }
            }
            else
            {
                //          lblErrMsg.Text = "Invalid Login Id";
                return "Invalid Login Id";
            }
            return "";
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getUserNameMethod(string str) 
        {
            string name=getUserName();
            return name;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object BtnRegLetStartMethod(string letstarttxtemail, string txtRegpassword, string txtRegFname, string txtRegLname, string txtRegphone, string txtregemail, string txtRegMobilePhone)
        {

            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                String sessionId;
                sessionId = HttpContext.Current.Session.SessionID;
                int orditemuseridupdate = 0;
                int newordid = 0;
                int getnewordid = 0;
                int order_id = 0;
                int OrderID = 0;

                int chkcurordid = 0;
                bool tmp = false;
                UserServices objUserServices = new UserServices();

                OrderServices objOrderServices = new OrderServices();
                HelperServices objHelperServices = new HelperServices();
                OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
                OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());



                tmp = objUserServices.CheckUserRegisterEmail(txtregemail.Trim(), "Retailer");

                int k = 0;
                if (tmp == false)
                {
                    k = SaveUserProfileInit(letstarttxtemail, txtRegpassword);

                }
                if (k > 0)
                {
                    int i = SaveUserProfile(txtRegFname, txtRegLname, txtRegphone, txtregemail, txtRegMobilePhone, txtRegpassword);
                    if (i > 0)
                    {
                        HttpContext.Current.Session["ExpressLevel"] = "Start";

                        DataSet tmpds = objUserServices.CheckMultipleUserMail(txtregemail.Trim(), "Retailer");
                        if ((tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0))
                        {
                            int UserID = objUserServices.GetUserID(txtregemail.Trim());
                            string Role;
                            Role = objUserServices.GetRole(UserID);
                            oOrdInfo.UserID = UserID;
                            chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);
                            if (chkcurordid == 0)
                                newordid = objOrderServices.InitilizeOrder(oOrdInfo);
                            getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);

                            orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(OrderID, UserID, sessionId, getnewordid, sessionId);
                            if (getnewordid > 0)
                            {
                                HttpContext.Current.Session["ORDER_ID"] = getnewordid;
                                HttpContext.Current.Session["USER_ID"] = UserID;
                                HttpContext.Current.Session["EXPRESS_CHECKOUT"] = "True";
                                HttpContext.Current.Session["USER_ROLE"] = Role;
                                oOrdInfo.OrderID = getnewordid;
                                objOrderServices.UpdateOrderPrice_ExpressCheckout(oOrdInfo, true, sessionId);
                            }
                            else
                            {
                                HttpContext.Current.Session["ORDER_ID"] = chkcurordid;
                                HttpContext.Current.Session["USER_ID"] = UserID;
                                HttpContext.Current.Session["EXPRESS_CHECKOUT"] = "True";
                                HttpContext.Current.Session["USER_ROLE"] = Role;
                                oOrdInfo.OrderID = chkcurordid;
                                OrderID = chkcurordid;
                            }
                            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                            ouserinfo = objUserServices.GetUserInfo(UserID);

                            HttpContext.Current.Session["USER_NAME"] = ouserinfo.LoginName;

                            HttpContext.Current.Session["USER_ROLE"] = ouserinfo.USERROLE;
                            HttpContext.Current.Session["COMPANY_ID"] = ouserinfo.CompanyID;
                            HttpContext.Current.Session["CUSTOMER_TYPE"] = ouserinfo.CUSTOMER_TYPE;
                            HttpContext.Current.Session["DUMMY_FLAG"] = "1";
                            HttpContext.Current.Session["Emailid"] = ouserinfo.AlternateEmail;
                            HttpContext.Current.Session["Firstname"] = ouserinfo.FirstName;

                            HttpContext.Current.Session["Lastname"] = ouserinfo.LastName;
                            try
                            {

                                Security objcrpengine = new Security();
                                string Newpassword = objcrpengine.StringEnCrypt_password(txtRegpassword.ToString());
                                SendMailRegiExpressCheckout(txtregemail.ToString().Trim(), Newpassword, txtregemail.ToString().Trim(), txtRegFname.ToString().Trim(), txtRegLname.ToString().Trim());
                            }
                            catch (Exception ex)
                            {
                                objErrorHandler.ErrorMsg = ex;
                                objErrorHandler.CreateLog();
                            }
                            HttpContext.Current.Session["EditAddress"] = true;

                            return ouserinfo;
                        }
                      
                        //L2name.Text = txtRegFname.Text.ToString() + " " + txtRegLname.Text.ToString();
                        //L2Email.Text = txtregemail.Text.Trim();
                        //L2Phone.Text = txtRegphone.Text.ToString();
                        //txt_attnto.Text = L2name.Text;
                        //Level2.Visible = true;
                        //BtnL2Continue.Focus();
                        //ScrollToControl("ctl00_maincontent_l2div", false);
                        //L2DivBilling.Visible = false;
                        //Session["EditAddress"] = true;
                        //Level1.Visible = false;
                        //Level3.Visible = false;
                        //Level4.Visible = false;

                        //letstartregister.Visible = false;
                        //startdivpwd.Visible = false;
                        //getstartwelcome.Visible = false;
                        //BtnGetStartLogin.Visible = false;
                        //letstartdivlogin.Visible = false;
                        //startdivpwd.Visible = false;
                        //getstartwelcome.Visible = true;
                        //BtnGetStartLogin.Visible = false;
                        //letstartregister.Visible = false;
                        //BtnGetStartContinue.Visible = false;
                        //txtbillbusname.Focus(); 
                        //BtnL2Continue.Focus();
                        //ScrollToControl("ctl00_maincontent_l2div", false);

                    }
                }
                else
                {
                    HttpContext.Current.Response.Redirect("home.aspx");
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return "";
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static Object BtnEditLoginMethod(string str)
        {

            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            UserServices objUserServices = new UserServices();
            int userid = Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString());
            ouserinfo = objUserServices.GetUserInfo(userid);
            //txtregemail.Text = ouserinfo.Email;
            //txtregemail.ReadOnly = true;
            //txtRegFname.Text = ouserinfo.FirstName;
            //txtRegLname.Text = ouserinfo.LastName;
            //txtRegphone.Text = ouserinfo.Phone;
            //txtRegMobilePhone.Text = ouserinfo.MobilePhone;
            return ouserinfo;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static Object BtnRegLetStartUpdateMethod(string letstarttxtemail, string txtRegpassword, string txtRegFname, string txtRegLname, string txtRegphone, string txtregemail, string txtRegMobilePhone)
        {

            ErrorHandler objErrorHandler = new ErrorHandler();
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            UserServices objUserServices = new UserServices();
            OrderServices objOrderServices = new OrderServices();
            try
            {

                ouserinfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                ouserinfo.FirstName = txtRegFname;
                ouserinfo.LastName = txtRegLname;
                Security objcrpengine = new Security();
                string Newpassword = objcrpengine.StringEnCrypt_password(txtRegpassword.ToString());
                ouserinfo.Password = Newpassword;
                ouserinfo.Phone = txtRegphone;
                ouserinfo.MobilePhone = txtRegMobilePhone;


                int i = objUserServices.UpdateUserInfo_loginExp(ouserinfo);
                if (i >= 1)
                {
                    ouserinfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                    HttpContext.Current.Session["USER_NAME"] = ouserinfo.LoginName;

                    HttpContext.Current.Session["USER_ROLE"] = ouserinfo.USERROLE;
                    HttpContext.Current.Session["COMPANY_ID"] = ouserinfo.CompanyID;
                    HttpContext.Current.Session["CUSTOMER_TYPE"] = ouserinfo.CUSTOMER_TYPE;
                    HttpContext.Current.Session["DUMMY_FLAG"] = "1";
                    HttpContext.Current.Session["Emailid"] = ouserinfo.AlternateEmail;
                    HttpContext.Current.Session["Firstname"] = ouserinfo.FirstName;
                    HttpContext.Current.Session["Lastname"] = ouserinfo.LastName;
                    HttpContext.Current.Session["LOGIN_NAME_TOP"] = ouserinfo.Contact;

                    //Edited by smith
                    int ordID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                    oOrdInfo = objOrderServices.GetOrder(ordID);

                    object orderInfo = objOrderServices.GetOrder(ordID);
                    object userInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                    var returnval = JsonConvert.SerializeObject(new { userInfo, orderInfo });
                    return returnval;

                    //L3Ship_DELIVERYINST.Text = ouserinfo.DELIVERYINST;
                    //hfordernumber.Value = oOrdInfo.ShipPhone;
                    //if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null)
                    //{
                    //    lblorderready.Text = oOrdInfo.ShipPhone;
                    //}
                    //else if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.")
                    //{
                    //    lblorderready.Text = ouserinfo.MobilePhone;
                    //}
                    ////end of edit

                    //oOrdShippInfo = objUserServices.GetUserShipInfo(Userid);

                    //if (oOrdShippInfo.ShipCountry != "" && oOrdShippInfo.ShipAddress1 != "" && oOrdShippInfo.ShipState != "")
                    //{

                    //    Level1.Visible = false;
                    //    Level3.Visible = true;

                    //    Level4.Visible = false;
                    //    Level2.Visible = false;

                    //    L3Name.Text = ouserinfo.Contact;
                    //    L3Email.Text = ouserinfo.AlternateEmail;
                    //    L3Phone.Text = ouserinfo.Phone;
                    //    L3ship_company_name.Text = ouserinfo.COMPANY_NAME;

                    //    L3ship_attn.Text = ouserinfo.Contact;
                    //    txt_attnto.Text = ouserinfo.Contact;

                    //    L3Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                    //    L3Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                    //    L3Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                    //    L3Ship_State.Text = oOrdShippInfo.ShipState;
                    //    L3Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                    //    L3Ship_Country.Text = oOrdShippInfo.ShipCountry;
                    //    L3Ship_DELIVERYINST.Text = oOrdShippInfo.DELIVERYINST;
                    //    ScrollToControl("ctl00_maincontent_divl3continue", true);
                    //}
                    //else
                    //{
                    //    Level2.Visible = true;
                    //    BtnL2Continue.Focus();
                    //    ScrollToControl("ctl00_maincontent_l2div", false);
                    //    Level1.Visible = false;
                    //    Level3.Visible = false;
                    //    Level4.Visible = false;
                    //    //shipping
                    //    L2name.Text = ouserinfo.Contact;
                    //    L2Email.Text = ouserinfo.AlternateEmail;
                    //    L2Phone.Text = ouserinfo.Phone;
                    //    txtComname.Text = ouserinfo.COMPANY_NAME;

                    //    txt_attnto.Text = ouserinfo.Contact;
                    //    txtsadd.Text = ouserinfo.ShipAddress1;
                    //    txtadd2.Text = ouserinfo.ShipAddress2;
                    //    txttown.Text = ouserinfo.ShipCity;
                    //    txtDELIVERYINST.Text = ouserinfo.DELIVERYINST;
                    //    drpCountry.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.ShipCountry);

                    //    if (ouserinfo.ShipCountry.ToLower() == "australia")
                    //    {
                    //        drpstate1.Visible = true;
                    //        txtzip.Visible = true;
                    //        txtzip.Text = ouserinfo.ShipZip;
                    //        txtzip_inter.Visible = false;
                    //        txtstate.Visible = false;
                    //        rfvstate.Enabled = false;
                    //        aucust.Visible = true;
                    //        intercust.Visible = false;
                    //        Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
                    //    }
                    //    else
                    //    {
                    //        txtzip.Visible = false;
                    //        txtzip_inter.Visible = true;
                    //        drpstate1.Visible = false;
                    //        txtzip_inter.Text = ouserinfo.ShipZip;
                    //        txtstate.Visible = true;
                    //        txtstate.Text = ouserinfo.ShipState;
                    //        rfvddlstate.Enabled = false;
                    //        aucust.Visible = false;
                    //        intercust.Visible = true;
                    //    }

                    //    //Billing
                    //    txtsadd_Bill.Text = ouserinfo.BillAddress1;
                    //    txtadd2_Bill.Text = ouserinfo.BillAddress2;
                    //    txttown_Bill.Text = ouserinfo.BillCity;

                    //    drpcountry_bill.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.BillCountry);

                    //    if (ouserinfo.BillCountry.ToLower() == "australia")
                    //    {
                    //        drpstate2.Visible = true;
                    //        txtzip_bill.Text = ouserinfo.BillZip;
                    //        txtstate_Bill.Visible = false;
                    //        txtstate_Bill.Style.Add("display", "none");
                    //        RequiredFieldValidator14.Enabled = false;

                    //        Setdrpdownlistvalue(drpstate2, ouserinfo.BillState.ToString());
                    //    }
                    //    else
                    //    {
                    //        txtstate_Bill.Visible = true;
                    //        txtstate_Bill.Style.Add("display", "block");
                    //        txtstate_Bill.Text = ouserinfo.BillState;
                    //        txtzip_bill.Text = ouserinfo.BillZip;
                    //        drpstate2.Visible = false;
                    //        RequiredFieldValidator13.Enabled = false;
                    //    }

                    //    if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower() && ouserinfo.BillZip == ouserinfo.ShipZip && ouserinfo.BillState == ouserinfo.ShipState && ouserinfo.BillAddress1 == ouserinfo.ShipAddress1)
                    //    {
                    //        ChkBillingAdd.Checked = true;
                    //        L2DivBilling.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        ChkBillingAdd.Checked = false;
                    //        L2DivBilling.Visible = true;
                    //    }



                    //    if (objOrderServices.IsNativeCountry_Express_userid(Userid) == 1)
                    //    {
                    //        if (Page.IsPostBack)
                    //        {
                    //            drpSM1.Items.Clear();
                    //            drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                    //            drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                    //            drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                    //            drpSM1.SelectedIndex = 0;
                    //            intorder.Visible = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        drpSM1.Items.Clear();
                    //        drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                    //        drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                    //        drpSM1.SelectedIndex = 1;
                    //        intorder.Visible = true;
                    //    }
                    //    BtnL2Continue.Focus();
                    //}
                }

                else
                {
                    //            lblerror_l1update.Text = "Update Failed";
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return "";
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static Object BtnL2ContinueMethod(NameValue[] formVars)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            UserServices objUserServices = new UserServices();
            
            HelperServices objHelperServices = new HelperServices();
            HelperDB objHelperDB = new HelperDB();
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            int j = 0;
                          
            j = UpdateUserData(formVars);
            OrderServices objOrderServices = new OrderServices();
            objErrorHandler.CreateLog(" j " + j);
            if (j > 0)
            {
                
                
                int ordID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                int Userid = Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString());

                //Level1.Visible = false;
                //Level2.Visible = false;
                //Level3.Visible = true;

                //Level4.Visible = false;
                HttpContext.Current.Session["EditAddress"] = null;
                HttpContext.Current.Session["ExpressLevel"] = "2Compl";

                //          ouserinfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));

                ouserinfo = objUserServices.GetUserInfo(Userid);

                if (HttpContext.Current.Session["ShipCountry"] != null && HttpContext.Current.Session["ShipCountry"].ToString().ToLower().Trim() != ouserinfo.ShipCountry.ToString().ToLower().Trim())
                {
                    if (objOrderServices.GetOrderItemCount(ordID) > 0)
                    {
                        OrderServices.OrderItemInfo oOrdItemInfo = new OrderServices.OrderItemInfo();
                        DataSet oDSOrderItems = objOrderServices.GetOrderItems(ordID);
                        for (int i = 0; i < oDSOrderItems.Tables[0].Rows.Count; i++)
                        {
                            try
                            {
                                int ProductID = Convert.ToInt32(objHelperServices.CI(oDSOrderItems.Tables[0].Rows[i]["product_id"].ToString()));
                                int Quantity =  Convert.ToInt32(objHelperServices.CI(oDSOrderItems.Tables[0].Rows[i]["QTY"].ToString()));
                                decimal UntPrice = objHelperDB.GetProductPrice_Exc(ProductID, Quantity, Userid.ToString());
                                decimal TotalAmt = UntPrice * Quantity;
                                oOrdItemInfo.ORDER_ITEM_ID = objHelperServices.CD(oDSOrderItems.Tables[0].Rows[i]["order_item_id"].ToString());
                                oOrdItemInfo.UserID = objHelperServices.CI(Userid);
                                oOrdItemInfo.ProductID = ProductID;
                                oOrdItemInfo.Quantity = Quantity;
                                oOrdItemInfo.OrderID = ordID;
                                oOrdItemInfo.PriceApplied = UntPrice;
                                oOrdItemInfo.Ship_Cost = CalculateShippingCost(ordID, ProductID, UntPrice, Quantity);
                                oOrdItemInfo.Tax_Amount = objOrderServices.CalculateTaxAmount(TotalAmt, ordID.ToString(), ProductID.ToString());
                                objOrderServices.UpdateOrderItem(oOrdItemInfo);

                                oOrdInfo.OrderID = ordID;
                                objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                                //objOrderServices.RemoveItem(PrdId.ToString(), ordID, objHelperServices.CI(Userid), order_item_id.ToString());
                                //staticAddOrderItem(ordID, objHelperServices.CI(Userid), (int)pQty, PrdId);
                                HttpContext.Current.Session["ShipCountry"] = ouserinfo.ShipCountry;
                            }
                            catch (Exception ex)
                            {

                                objErrorHandler.CreateLog(ex.ToString());
                            }

                        }
                    }
                }

                object orderInfo = objOrderServices.GetOrder(ordID);
                object userInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                var returnval = JsonConvert.SerializeObject(new { userInfo, orderInfo });
                return returnval;
                //      return ouserinfo;

                //L3Name.Text = ouserinfo.Contact;
                //L3Email.Text = ouserinfo.AlternateEmail;
                //L3Phone.Text = ouserinfo.Phone;
                //L3ship_company_name.Text = ouserinfo.COMPANY_NAME;

                //if (ouserinfo.DELIVERYINST == "")
                //{
                //    L3Ship_DELIVERYINST.Text = ouserinfo.DELIVERYINST;

                //}
                //else
                //{

                //    L3Ship_DELIVERYINST.Text = txtDELIVERYINST.Text;
                //}
                //if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                //{
                //    L3ship_attn.Text = ouserinfo.Contact;
                //}
                //else
                //{
                //    L3ship_attn.Text = ouserinfo.Receiver_Contact;
                //}

                //oOrdShippInfo = objUserServices.GetUserShipInfo(Userid);
                //L3Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                //L3Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                //L3Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                //L3Ship_State.Text = oOrdShippInfo.ShipState;
                //L3Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                //L3Ship_Country.Text = oOrdShippInfo.ShipCountry;
                //L3Ship_DELIVERYINST.Text = oOrdShippInfo.DELIVERYINST;


                //Session["ExpressLevel"] = "2Compl";

                //if (oOrdShippInfo.ShipCountry.ToUpper() == "AUSTRALIA")
                //{


                //    drpSM1.Items.Clear();
                //    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                //    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                //    drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                //    drpSM1.SelectedIndex = 0;
                //    intorder.Visible = false;

                //}
                //else
                //{
                //    drpSM1.Items.Clear();
                //    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                //    drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                //    drpSM1.SelectedIndex = 1;
                //    intorder.Visible = true;

                //}

            }
            return "";
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object EditAddressMethod(string str)
        {
            UserServices objUserServices = new UserServices();
            OrderServices objOrderServices = new OrderServices();
            int ordID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

            object orderInfo = objOrderServices.GetOrder(ordID);
            object userInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
            var returnval = JsonConvert.SerializeObject(new { userInfo, orderInfo });
            return returnval;
        }


        [System.Web.Services.WebMethod]
        public static string GetOptionValuesMethod(String oName)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            if (oName != "COURIER CHARGE")
            {
                try
                {
                    DataSet objoptName_all;
                    bool isfirst = false;
                    // first Attempt
                    String _tempstring = "";
                    HelperServices helperServices = new HelperServices();
                    objoptName_all = helperServices.GetOptionValuesAll(false);
                    if (objoptName_all != null)
                    {
                        DataRow[] dr = objoptName_all.Tables[0].Select("OPTION_NAME='" + oName + "'");
                        if (dr.Length > 0)
                        {
                            _tempstring = dr.CopyToDataTable().Rows[0]["OPTION_VALUE"].ToString();
                            isfirst = true;
                        }
                    }
                    // second Attempt
                    if (!(isfirst))
                    {
                        objoptName_all = helperServices.GetOptionValuesAll(true);
                        if (objoptName_all != null)
                        {
                            DataRow[] dr = objoptName_all.Tables[0].Select("OPTION_NAME='" + oName + "'");
                            if (dr.Length > 0)
                            {
                                _tempstring = dr.CopyToDataTable().Rows[0]["OPTION_VALUE"].ToString();
                            }
                        }
                    }

                    //_tempstring = (string)objHelper.GetGenericDataDB(oName, "OPTION_NAME", HelperDB.ReturnType.RTString);

                    return _tempstring;
                }
                catch (Exception objException)
                {
                    objErrorHandler.ErrorMsg = objException;
                    objErrorHandler.CreateLog();
                    return "";
                }

            }
            else
            {
                string price = "0";
                HelperDB objHelperDB = new HelperDB();
                decimal x = objHelperDB.GetProductPrice_Exc(83480, 1, "999");

                price = x.ToString();
                return price;
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int btnMobileNoChangeMethod(string mobilenumber, string checked_val)
        {
            HelperServices objHelperServices = new HelperServices();
            OrderServices objOrderServices = new OrderServices();
            int _UserrID;
            _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
            int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
            HttpContext.Current.Session["ORDER_ID_NOTHANKS"] = "0";
            if (checked_val.ToString().Trim() == "true")
            {

                //oOrdInfo.ShipPhone = mobilenumber;
                //lblorderreadytext.Text = "SMS Order ready notification message will be sent to:";
                decimal Updpr = objOrderServices.Update_MOBILE_NUMBER(mobilenumber, _UserrID, OrderID, true);
                if (Updpr > 0)
                {
                    return 1;
                    //lblorderready.Text = txtchangemobilenumber.Text;
                    //hfphonenumber.Value = txtchangemobilenumber.Text;
                    //hfordernumber.Value = txtchangemobilenumber.Text;
                    //txtMobileNumber.Text = txtchangemobilenumber.Text;
                    //txtchangemobilenumber.Text = "";
                }

            }
            else
            {

                //txtMobileNumber.Text = txtchangemobilenumber.Text;
                //oOrdInfo.ShipPhone = txtchangemobilenumber.Text;
                //lblorderreadytext.Text = "SMS Order ready notification message will be sent to:";
                int UP = objOrderServices.Update_SHIP_NUMBER(mobilenumber, OrderID);
                if (UP > 0)
                {
                    return 2;
                    //lblorderready.Text = txtchangemobilenumber.Text;
                    //hfordernumber.Value = txtchangemobilenumber.Text;
                    //txtchangemobilenumber.Text = "";
                }
            }
            return 0;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int btnNoThanksChangeMethod(string str)
        {
            HelperServices objHelperServices = new HelperServices();
            OrderServices objOrderServices = new OrderServices();
            int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

            int UP = objOrderServices.Update_SHIP_NUMBER("", OrderID);
            if (UP > 0)
            {
                //txtchangemobilenumber.Text = hfphonenumber.Value;
                //cbmobilechange.Checked = true;
                //lblorderready.Text = "";
                //hfordernumber.Value = "";
                //hfnothanks.Value = "1";
                HttpContext.Current.Session["ORDER_ID_NOTHANKS"] = "1";
                string sdt = HttpContext.Current.Session["ORDER_ID_NOTHANKS"].ToString();
                return 1;
                //hfchange.Value = "1";
                //txtMobileNumber.Text = "";
            }
            return 0;
        }

         [System.Web.Services.WebMethod(EnableSession = true)]
        public static object LoadMobileNoChangeMethod(string str)
        {
            OrderServices objOrderServices = new OrderServices();
            UserServices objUserServices = new UserServices();
            int ordID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

            object orderInfo = objOrderServices.GetOrder(ordID);
            object userInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
            var returnval = JsonConvert.SerializeObject(new { userInfo, orderInfo });
            return returnval;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
         public static object BtnL3ContinueMethod(string drpSM1, string TextBox1, string ttinter_order, string txtMobileNumber, string chksavemobile, string lblorderready, string hfisppp)
        {
            if (drpSM1.ToString().Trim() != "Please Select Shipping Method ")
            {
                OrderServices objOrderServices = new OrderServices();
                HelperServices objHelperServices = new HelperServices();
                ErrorHandler objErrorHandler = new ErrorHandler();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
                UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
                OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
                HelperDB objHelperDB = new HelperDB();

                objErrorHandler.CreateOrderSummarylog("drpSM1" + drpSM1 + "," + TextBox1 + "," + ttinter_order + "," + txtMobileNumber + "," + lblorderready);
               
                try
                {
                    //           rfvdrpSM1.Visible = false;
                    QuoteServices objQuoteServices = new QuoteServices();
                    OrderDB objOrderDB = new OrderDB();
                    int OrdStatus = 0;
                    string ApproveOrder = string.Empty;
                    string clientIPAddress = "";
                    string refid = "";
                    int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

                    if (OrderID > 0)
                    {
                        if (HttpContext.Current.Session["IP_ADDR"] != null && HttpContext.Current.Session["IP_ADDR"].ToString() != "")
                            clientIPAddress = HttpContext.Current.Session["IP_ADDR"].ToString();
                        else
                            clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                        objOrderServices.InitilizeOrder_ipaddress(OrderID, clientIPAddress);

                    }
                    //if (drpCountry.SelectedItem.ToString().ToLower() == "")
                    //    return;


                    if (HttpContext.Current.Request.QueryString["ApproveOrder"] == null)
                    {
                        if (HttpContext.Current.Session["USER_ROLE"] != null)
                        {
                            switch (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]))
                            {
                                case 1:
                                    OrdStatus = (int)OrderServices.OrderStatus.OPEN;
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
                    else if (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2))
                    {
                        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                    }
                    else if (HttpContext.Current.Request.QueryString["ApproveOrder"] != null)
                        OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;

                   // decimal TaxAmount;
                    decimal ProdTotCost;
                    //           GetParams();                                                               //uncomment when testing is done

                    if (OrderID <= 0)
                        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), OrdStatus);

                    int oldorderID = OrderID;
                    objErrorHandler.CreateOrderSummarylog("Inside  BtnL3Continue_Click" + "OrderID :" + OrderID + "UserID :" + HttpContext.Current.Session["USER_ID"]);

                    if (OrderID > 0)
                    {
                        int _UserrID;
                        _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
                        oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                        oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                        decimal Updpro = 0;
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                        {
                            if (ttinter_order != "")
                            {
                                refid = objHelperServices.CS(ttinter_order);
                                try
                                {
                                    string sSQL = string.Format("EXEC STP_TBWC_CHECK_PO_NUMBER_EXIST '" + HttpContext.Current.Session["ORDER_ID"].ToString() + "','" + HttpContext.Current.Session["USER_ID"].ToString() + "','" + refid + "'");
                                    objErrorHandler.CreateLog("l3continue" + " " + sSQL);
                                    DataTable DS = objHelperDB.GetDataTableDB(sSQL);


                                    if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                                    {
                                        //                                   lblerror_ttinter.Text = "Order No already exists, please Re-enter Order No";
                                        //                                 ttinter_order.Focus();
                                        return "Order No already exists, please Re-enter Order No";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    objErrorHandler.CreateLog(ex.ToString());
                                }
                            }

                        }


                        //Update User Mobile_Phone
                        if (chksavemobile == "true")
                        {
                            if (txtMobileNumber != "" && txtMobileNumber != null && drpSM1 == "Counter Pickup")
                            {

                                Updpro = objOrderServices.Update_MOBILE_NUMBER(txtMobileNumber, _UserrID, OrderID, false);
                                if (Updpro > 0)
                                {
                                    oOrdInfo.ShipPhone = txtMobileNumber;
                                    HttpContext.Current.Session["ORDER_ID_NOTHANKS"] = "0";
                                }
                            }
                        }

                        if (drpSM1.ToString().Trim() == "Counter Pickup")
                        {
                            OrderServices.OrderInfo orderInfo = new OrderServices.OrderInfo();
                            orderInfo = objOrderServices.GetOrder(OrderID);
                            objErrorHandler.CreateOrderSummarylog("text is " + orderInfo.ShipPhone);

                            if (txtMobileNumber != "" && txtMobileNumber != null)
                            {
                                oOrdInfo.ShipPhone = txtMobileNumber;
                            }
                            else
                            {
                                oOrdInfo.ShipPhone = lblorderready;
                            }
                        }
                        else
                        {
                            oOrdInfo.ShipPhone = oOrdShippInfo.ShipPhone;
                        }
                        objErrorHandler.CreateOrderSummarylog("Order ship phone" + oOrdInfo.ShipPhone);


                        //End of edit


                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                            OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                        else
                            OrdStatus = (int)OrderServices.OrderStatus.OPEN;

                        ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
                        //TaxAmount = objOrderServices.CalculateTaxAmount_Express(ProdTotCost, OrderID.ToString());
                        decimal UpdRst = 0;
                        oOrdInfo.OrderID = OrderID;
                        oOrdInfo.OrderStatus = OrdStatus;

                        oOrdInfo.OrderStatus = OrdStatus;
                        oOrdInfo.ShipMethod = drpSM1;


                        if (oOrdShippInfo.Receiver_Contact == null || oOrdShippInfo.Receiver_Contact == "")
                        {

                            oOrdInfo.ShipFName = oOrdShippInfo.FirstName + " " + oOrdShippInfo.LastName;
                        }
                        else
                        {

                            oOrdInfo.ShipFName = oOrdShippInfo.Receiver_Contact;
                        }
                        oOrdInfo.ShipMName = "";
                        oOrdInfo.ShipAdd1 = oOrdShippInfo.ShipAddress1;
                        oOrdInfo.ShipAdd2 = oOrdShippInfo.ShipAddress2;
                        oOrdInfo.ShipAdd3 = "";
                        oOrdInfo.ShipCity = oOrdShippInfo.ShipCity;
                        oOrdInfo.ShipState = oOrdShippInfo.ShipState;
                        oOrdInfo.ShipCountry = oOrdShippInfo.ShipCountry;
                        oOrdInfo.ShipZip = oOrdShippInfo.ShipZip;
                        oOrdInfo.ShipCompName = oOrdShippInfo.COMPANY_NAME;


                        if (oOrdShippInfo.Receiver_Contact == null || oOrdShippInfo.Receiver_Contact == "")
                        {

                            oOrdInfo.Receiver_Contact = oOrdShippInfo.FirstName + " " + oOrdShippInfo.LastName;
                        }
                        else
                        {
                            oOrdInfo.Receiver_Contact = oOrdShippInfo.Receiver_Contact;
                        }

                        oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1);
                        oOrdInfo.ClientIPAddress = clientIPAddress;
                        oOrdInfo.isEmailSent = false;
                        oOrdInfo.isInvoiceSent = false;
                        oOrdInfo.IsShipped = false;

                        oOrdInfo.BillFName = oOrdBillInfo.FirstName;
                        oOrdInfo.BillLName = oOrdBillInfo.LastName;
                        oOrdInfo.BillMName = "";
                        oOrdInfo.BillAdd1 = oOrdBillInfo.BillAddress1;
                        oOrdInfo.BillAdd2 = oOrdBillInfo.BillAddress2;
                        oOrdInfo.BillAdd3 = "";
                        oOrdInfo.BillCity = oOrdBillInfo.BillCity;
                        oOrdInfo.BillState = oOrdBillInfo.BillState;
                        oOrdInfo.BillCountry = oOrdBillInfo.BillCountry;
                        oOrdInfo.BillZip = oOrdBillInfo.BillZip;
                        oOrdInfo.BillPhone = oOrdBillInfo.BillPhone;//objHelperServices.Prepare(txtphone.Text);
                        oOrdInfo.BillcompanyName = oOrdBillInfo.Bill_Company;
                        oOrdInfo.Bill_Name = oOrdBillInfo.Bill_Name;

                        oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
                        DataSet tmpds = GetOrderItemDetailSum(oOrdInfo.OrderID);
                        if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                        {
                            oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                        }
                        if (oOrdInfo.ProdTotalPrice == 0)
                        {
                            objErrorHandler.CreateLog("btnSecurepay start Orderid=" + OrderID + "ProdTotalPrice" + "0");
             //               HttpContext.Current.Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                            return "ConfirmMessage.aspx?Result=QTEEMPTY";
                        }

                        oOrdInfo.ShipCost = CalculateShippingCost(drpSM1, OrderID, hfisppp);
                        if (hfisppp == "1" && objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                        {
                            oOrdInfo.SHIP_CODE = "";
                        }
                        else if (objOrderServices.IsNativeCountry_Express(OrderID) == 1 && oOrdInfo.ShipCost > 0)
                        {
                            oOrdInfo.SHIP_CODE = "DELWG";
                        }
                        else
                        {
                            oOrdInfo.SHIP_CODE = "";
                        }
                        //                 lblshipcost.Text = oOrdInfo.ShipCost.ToString();
                        oOrdInfo.TaxAmount = objOrderServices.CalculateTaxAmount(oOrdInfo.ProdTotalPrice , oOrdInfo.ShipCost, OrderID.ToString());
                        oOrdInfo.TotalAmount = oOrdInfo.ProdTotalPrice + oOrdInfo.TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
                        oOrdInfo.isppppp = Convert.ToInt16(hfisppp);
                        if (hfisppp == "1" && drpSM1 != "Counter Pickup")
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                            oOrdInfo.OrderStatus = OrdStatus;
                        }

                        oOrdInfo.TrackingNo = "";
                       
                        oOrdInfo.DeliveryInstr = oOrdBillInfo.DELIVERYINST;

                        //if (txtDELIVERYINST.Text != "")
                        //{

                        //    oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDELIVERYINST.Text);
                        //}
                        //else if (L3Ship_DELIVERYINST.Text != "")
                        //{
                        //    oOrdInfo.DeliveryInstr = objHelperServices.Prepare(L3Ship_DELIVERYINST.Text);
                        //}
                        //else
                        //{
                        //    oOrdInfo.DeliveryInstr = objHelperServices.Prepare(L3Ship_DELIVERYINST.Text);

                        //}


                        if (drpSM1.ToString().Trim().Contains("Drop Shipment Order"))
                        {
                            oOrdInfo.DropShip = 1;
                        }

                        oOrdInfo.UserID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());

                        UpdRst = objOrderServices.UpdateOrder(oOrdInfo);

                        if (HttpContext.Current.Session["PrevOrderID"] != null && Convert.ToInt32(HttpContext.Current.Session["PrevOrderID"]) > 0)
                        {
                            HttpContext.Current.Session["PrevOrderID"] = "0";
                        }

                        // Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
                        if (UpdRst > 0)
                        {
                            objOrderServices.UpdateCustomFields(oOrdInfo);


                            //Level1.Visible = false;
                            //Level2.Visible = false;
                            //Level3.Visible = false;
                            //Level4.Visible = true;
                            //Level4_Submit.Visible = false;

                            //divOk.Visible = false;

                            UserServices.UserInfo userInfo = new UserServices.UserInfo();
                            OrderServices.OrderInfo orderInfo = new OrderServices.OrderInfo();
                            userInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));

                            orderInfo = objOrderServices.GetOrder(Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString()));
                            var returnval = JsonConvert.SerializeObject(new { userInfo, orderInfo });

                            //    return ouserinfo;
                            //L4name.Text = ouserinfo.Contact;
                            //L4Email.Text = ouserinfo.AlternateEmail;
                            //L4Phone.Text = ouserinfo.Phone;
                            //L4Ship_Company.Text = ouserinfo.COMPANY_NAME;

                            //if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                            //{
                            //    L4Ship_Attnto.Text = ouserinfo.Contact;
                            //}
                            //else
                            //{
                            //    L4Ship_Attnto.Text = ouserinfo.Receiver_Contact;
                            //}
                            //L4Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                            //L4Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                            //L4Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                            //L4Ship_State.Text = oOrdShippInfo.ShipState;
                            //L4Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                            //L4Ship_Country.Text = oOrdShippInfo.ShipCountry;
                            //L4Ship_DELIVERYINST.Text = oOrdInfo.DeliveryInstr;

                            //oOrdInfo = objOrderServices.GetOrder(OrderID);
                            //lblShippingMethod.Text = oOrdInfo.ShipMethod.Replace("Counter Pickup", "Shop Counter Pickup");
                            //L4Comments.Text = oOrdInfo.ShipNotes;

                            //if (drpSM1.SelectedValue == "Standard Shipping")
                            //{
                            //    divshopcounter.Visible = false;
                            //}
                            //else
                            //{
                            //    divshopcounter.Visible = true;
                            //}


                            HttpContext.Current.Session["ORDER_NO"] = null;
                            HttpContext.Current.Session["SHIPPING"] = null;
                            HttpContext.Current.Session["DELIVERY"] = null;
                            HttpContext.Current.Session["DROPSHIP"] = null;
                            int ord = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                            HttpContext.Current.Session["ShipCost"] = orderInfo.ShipCost;
                            //objErrorHandler.CreateOrderSummarylog("Before ProceedFunction" + "OrderID :" + OrderID + "UserID :" + HttpContext.Current.Session["USER_ID"] + "ProdTotalPrice :" + oOrdInfo.ProdTotalPrice);
                            ProceedFunction(drpSM1, ttinter_order, TextBox1, hfisppp);

                            //              PHOrderConfirm.Attributes.Add("style", "display:block");
                            //PHOrderConfirm.Visible = true;
                            //txtsadd.Enabled = false;
                            //txtadd2.Enabled = false;
                            //txttown.Enabled = false;
                            //txtstate.Enabled = false;
                            //txtzip_inter.Enabled = false;
                            //drpCountry.Enabled = false;
                            HttpContext.Current.Session["ExpressLevel"] = "3Compl";
                            if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                            {
                                HttpContext.Current.Session["ORDER_ID"] = "0";
                            }
                            else if (hfisppp == "1" && drpSM1 != "Counter Pickup")
                            {
                                HttpContext.Current.Session["ORDER_ID"] = "0";
                            }
                            

                            //if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                            //{
                            //    Level4_Payment.Visible = true;
                            //    Level4_Submit.Visible = false;
                            //    ImageButton1.Visible = false;
                            //    PayType.Visible = true;
                            //    Response.Redirect("express_checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay"), false);

                            //}
                            //else
                            //{
                            //    if (ttinter_order.Text == "")
                            //    {
                            //        ttinter_order.Text = refid;
                            //        lblintorderid.Text = refid;
                            //        txtintporelease_No.Text = refid;
                            //    }
                            //    else
                            //    {
                            //        lblintorderid.Text = ttinter_order.Text;
                            //        txtintporelease_No.Text = ttinter_order.Text;
                            //    }

                            HttpContext.Current.Session["OrderDetExp_orderid"] = OrderID;
                            HttpContext.Current.Session["OrderDetExp_userid"] = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
                            //    ImageButton1.Visible = false;
                            //    divshopcounter.Visible = false;
                            //    Level4_Payment.Visible = false;
                            //    Level4_Submit.Visible = true;
                            //    PHOrderConfirm.Visible = true;

                            //    checkoutrightL1.Visible = false;
                            //    checkoutrightL4.Visible = true;


                            //    L4AEditAddress.Visible = false;
                            //    BtnEditAddress.Visible = false;

                            //    L3AEditAddress.Visible = false;
                            //    BtnL3EditAddress.Visible = false;

                            //    L4AEditShippingMethod.Visible = false;
                            //    BtnL4EditShippingMethod.Visible = false;

                            //    L4AEditAddress.Visible = false;
                            //    btneditlogin4.Visible = false;
                            //    PayType.Visible = true;
                            //    SecurePayAcc.Visible = false;
                                
                            //    checkoutrightL1.Visible = false;
                            //    checkoutrightL4.Visible = true;
                            //    divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
                            //    Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                            //}
                            return returnval;
                        }
                    }
                    return "";
                }
                catch (Exception Ex)
                {
                    objErrorHandler.ErrorMsg = Ex;
                    objErrorHandler.CreateLog();
                }

            }
            else
            {
                //             rfvdrpSM1.Visible = true;
                return "Please Select Shipping Method";
            }

            return "";
        }

        [System.Web.Services.WebMethod]
        public static object BtnL4EditShippingClickMethod(string str)
        {
            OrderServices objOrderServices = new OrderServices();
            UserServices objUserServices = new UserServices();
            int ordID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

            object orderInfo = objOrderServices.GetOrder(ordID);
            object userInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
            var returnval = JsonConvert.SerializeObject(new { userInfo, orderInfo });
            return returnval;
        }


        [System.Web.Services.WebMethod]
        public static object showPriceMethod(string drpSM1, decimal ProdTotalPrice, string hfisppp)
        {
            OrderServices objOrderServices = new OrderServices();
            UserServices objUserServices = new UserServices();
            HelperServices objHelperServices = new HelperServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            int Userid = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
            if (Userid <= 0)
                Userid = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());
            int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            string orderType = string.Empty;
            string totalAmount = string.Empty;
            string taxAmount = string.Empty;
            string sessionId;
            sessionId = HttpContext.Current.Session.SessionID;
           

            if (Userid.ToString()=="999")
            {
                orderType = "Domestic";
                taxAmount = objOrderServices.GetTotalOrderTaxAmount_Without_Duplicate(objHelperServices.CI(OrderID), sessionId).ToString();
                totalAmount = calctotalcost_without_duplicate(ProdTotalPrice, OrderID.ToString(), sessionId).ToString();
            }
            else if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
            {
                oOrdInfo = objOrderServices.GetOrder(OrderID);
                orderType = "InterNational";
                taxAmount = "0.00";
                totalAmount = oOrdInfo.ProdTotalPrice.ToString();
            }
            else
            {
                orderType = "Domestic";
                oOrdInfo = objOrderServices.GetOrder(OrderID);
                if (drpSM1 == "Standard Shipping" && hfisppp=="0")
                {
                    taxAmount = calctax_shipping(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();
                    totalAmount = calctotalcost_shipping(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();
                }
                else
                {
                    taxAmount = objOrderServices.GetTotalOrderTaxAmount(objHelperServices.CI(OrderID)).ToString() ;
                    totalAmount = calctotalcost(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();

                }
            }

            var chk = new priceValue
            {
                orderType = orderType,
                deliveryCharge = CalculateShippingCost(drpSM1, OrderID, hfisppp).ToString(),
                taxAmount = taxAmount,
                totalAmount = totalAmount,
            };
            return chk;
        }




        //[System.Web.Services.WebMethod]
        //public static object showPriceMethod1(string drpSM1, string hfisppp)
        //{
        //    OrderServices objOrderServices = new OrderServices();
        //    //UserServices objUserServices = new UserServices();
        //    HelperServices objHelperServices = new HelperServices();
        //    //ErrorHandler objErrorHandler = new ErrorHandler();
        //    //UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
        //    int Userid = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
        //    if (Userid <= 0)
        //        Userid = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());
        //    int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
        //    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
        //    oOrdInfo = objOrderServices.GetOrder(OrderID);
        //    decimal ProdTotalPrice = oOrdInfo.ProdTotalPrice;
        //    //string orderType = string.Empty;
        //    //string totalAmount = string.Empty;
        //    //string taxAmount = string.Empty;
        //    string sessionId;
        //    sessionId = HttpContext.Current.Session.SessionID;
        //    OrderAmount orderAmount = new OrderAmount();
        //    decimal ProdTotCost = 0;
        //    string TaxAmount = string.Empty;
        //    string TotalAmount = string.Empty;
        //    ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
        //    //TaxAmount = objOrderServices.CalculateTaxAmount_Express(ProdTotCost, OrderID.ToString());
        //    if (Userid.ToString() == ConfigurationManager.AppSettings["DUM_USER_ID"].ToString())
        //    {
        //        TotalAmount = calctotalcost_without_duplicate(ProdTotalPrice, OrderID.ToString(), sessionId).ToString();
        //    }
        //    else if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
        //    {
        //        TaxAmount = "0.00";
        //        TotalAmount = ProdTotalPrice.ToString();
        //    }
        //    else
        //    {
        //        if (drpSM1 == "Standard Shipping" && hfisppp == "0")
        //        {
        //            TaxAmount = calctax_shipping(ProdTotalPrice, OrderID.ToString()).ToString();
        //            TotalAmount = calctotalcost_shipping(ProdTotalPrice, OrderID.ToString()).ToString();
        //        }
        //        else
        //        {
        //            TaxAmount = objOrderServices.GetTotalOrderTaxAmount(objHelperServices.CI(OrderID)).ToString();
        //            TotalAmount = calctotalcost(ProdTotalPrice, OrderID.ToString()).ToString();

        //        }
        //    }

        //    if (Userid.ToString() == "999")
        //    {
        //        orderType = "Domestic";
        //        taxAmount = objOrderServices.GetTotalOrderTaxAmount_Without_Duplicate(objHelperServices.CI(OrderID), sessionId).ToString();
        //        totalAmount = calctotalcost_without_duplicate(ProdTotalPrice, OrderID.ToString(), sessionId).ToString();
        //    }
        //    else if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
        //    {
        //        orderType = "InterNational";
        //        taxAmount = "0.00";
        //        totalAmount = ProdTotalPrice.ToString();
        //    }
        //    else
        //    {
        //        orderType = "Domestic";
        //        if (drpSM1 == "Standard Shipping" && hfisppp == "0")
        //        {
        //            taxAmount = calctax_shipping(ProdTotalPrice, OrderID.ToString()).ToString();
        //            totalAmount = calctotalcost_shipping(ProdTotalPrice, OrderID.ToString()).ToString();
        //        }
        //        else
        //        {
        //            taxAmount = objOrderServices.GetTotalOrderTaxAmount(objHelperServices.CI(OrderID)).ToString();
        //            totalAmount = calctotalcost(ProdTotalPrice, OrderID.ToString()).ToString();

        //        }
        //    }

        //    var chk = new priceValue
        //    {
        //        orderType = orderType,
        //        deliveryCharge = CalculateShippingCost(drpSM1, OrderID, hfisppp).ToString(),
        //        taxAmount = taxAmount,
        //        totalAmount = totalAmount,
        //    };
        //    return chk;
        //}


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object cartItemsMethod(string str)
        {

            HelperServices objHelperServices = new HelperServices();
            OrderServices objOrderServices = new OrderServices();
            ProductServices objProductServices = new ProductServices();
            DataSet dsOItem = new DataSet();
            HelperDB objHelperDB = new HelperDB();

            int OrderID = 0;
            int Userid;
            int ProductId;
            decimal subtot = 0.00M;
            decimal taxamt = 0.00M;
            decimal Total = 0.00M;

            string SelProductId = "";
            string OrdStatus = "";

            int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
            UserServices objUserServices = new UserServices();

            Userid = objHelperServices.CI(HttpContext.Current.Session["USER_ID"]);
            if (Userid <= 0)
                Userid = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());
            if (!string.IsNullOrEmpty(HttpContext.Current.Session["ORDER_ID"].ToString()))
            {
                OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(Userid, OpenOrdStatusID);
            }

            OrdStatus = objOrderServices.GetOrderStatus(OrderID);
            ProductId = objHelperServices.CI(HttpContext.Current.Request.QueryString["Pid"]);

            String sessionId_dum;
            sessionId_dum = HttpContext.Current.Session.SessionID;


            if (Userid != 999)
            {
                dsOItem = objOrderServices.GetOrderItems(OrderID);
            }
            else
            {
                dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, "", sessionId_dum);
            }

            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            oOrderInfo = objOrderServices.GetOrder(OrderID);

            UserServices.UserInfo oOrdBillInfo1 = objUserServices.GetUserBillInfo(Userid);
            UserServices.UserInfo oOrdShippInfo1 = objUserServices.GetUserShipInfo(Userid);


            string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
            decimal ProdShippCost = 0.00M;
            decimal TotalShipCost = 0.00M;
            decimal ShippingValue = 0;
            string catlogitem = "";
            SelProductId = "";

            if (OrdStatus == OrderServices.OrderStatus.OPEN.ToString() || OrdStatus == "CAU_PENDING")
            {
                if (dsOItem != null)
                {
                    //           object obj=
                    object orderInfo = objOrderServices.GetOrder(OrderID);

                    //int i = 0;
                    var returnval = string.Empty;
                    List<orderItems> orderitemslist = new List<orderItems>();
                    //               orderItems orderitems = new orderItems();
                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                    {
                        decimal ProductUnitPrice;
                        int pid;
                        decimal Amt = 0;
                        orderItems orderitems = new orderItems();
                        orderitems.pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                        orderitems.catlogitem = rItem["CATALOG_ITEM_NO"].ToString();
                        ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                        orderitems.ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                        orderitems.description = rItem["DESCRIPTION"].ToString();
                        orderitems.Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice);
                        orderitems.imageSrc = GetImage(orderitems.pid);
                        int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                        decimal ProdTotal = Qty * ProductUnitPrice;
                        subtot = subtot + ProdTotal;
                        orderitems.Qty = Qty;
                        orderitems.subtot = subtot;
                        string chkShipping_Method = string.Empty;
                        DataSet dsCSM = (DataSet)objHelperDB.GetGenericDataDB("", orderitems.pid.ToString(), "GET_ZIP_SHIPPING_METHOD", HelperDB.ReturnType.RTDataSet);
                        if (dsCSM != null)
                        {
                            if (dsCSM.Tables.Count > 0 && dsCSM.Tables[0].Rows.Count > 0)
                            {


                                chkShipping_Method = dsCSM.Tables[0].Rows[0]["PROD_LEGISTATED_STATE"].ToString();
                                if (chkShipping_Method.Trim() == "PPPPPPP")
                                {

                                    orderitems.isppp = "1";
                                }

                            }
                        }
                        orderitemslist.Add(orderitems);

                    }
                    //             returnval = JsonConvert.SerializeObject(orderitemslist);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    //                HttpContext.Current.Response.Write();
                    return js.Serialize(orderitemslist);
                }
            }
            return "";
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object TopCartItems(string str)
        {
            HelperServices objHelperServices = new HelperServices();
            OrderServices objOrderServices = new OrderServices();
            HelperDB objHelperDB = new HelperDB();
            DataSet dsCart = new DataSet();
            string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
            string sessionId_dum;
            sessionId_dum = HttpContext.Current.Session.SessionID.ToString();

            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            if (userid == string.Empty)
            {
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
            }


            string _Tbt_Order_Id = "";
            string _Tbt_Ship_URL = "";
            int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
            if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
            {
                _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
            }
            else
            {
                _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(userid), OpenOrdStatusID).ToString();

            }
            string OrderStatus = objOrderServices.GetOrderStatus(objHelperServices.CI(_Tbt_Order_Id));
            if (objHelperServices.CI(_Tbt_Order_Id) > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
            {

                dsCart = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_CART_ITEM " + _Tbt_Order_Id + ",'" + sessionId_dum + "',''");
            }
            else
            {
                dsCart = null;
            }

            if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
            {
                // _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                _Tbt_Ship_URL = "/orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;

            }
            else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
            {
                _Tbt_Ship_URL = "/orderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;
                //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
            }

            TopCart topCart = new TopCart();

            if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
            {
                topCart.VIEW_ORDER = _Tbt_Ship_URL;
                topCart.SHOW_CART = false;
                topCart.CART_AMOUNT = "0.00";
                topCart.CART_COUNT = "0";

            }
            else
            {
                List<TopCartList> lstTopCartList = new List<TopCartList>();
                foreach (DataRow dr in dsCart.Tables[0].Rows)//For Records
                {
                    TopCartList topCartList = new TopCartList();
              
                    string category = dr["Category_Path"].ToString();

                    // category = strsupplierName;
                    string newurl = string.Empty;

                    DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), "", "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {
                        string[] catpath = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                        string familyid = tmpds.Tables[0].Rows[0]["parent_FAMILY_ID"].ToString();

                        if (familyid == "0")
                        {
                            familyid = tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString();
                        }
                        //newurl = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString() + "////" + tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString(), "pd.aspx", true);
                        newurl = objHelperServices.SimpleURL_Str("AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["FAMILY_ID"].ToString() + "////" + tmpds.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString() + "////" + "////" + (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString(), "pd.aspx", true);
                        topCartList.TBT_REWRITEURL = newurl + "/pd/";
                    }

                    string imgpath = GetImagePath(dr["TWEB Image1"].ToString());
                    topCartList.FAMILY_NAME = dr["Family_name"].ToString();
                    topCartList.COST = dr["Cost"].ToString();
                    topCartList.CODE = dr["Code"].ToString();
                    topCartList.TWebImage1 = imgpath;
                    lstTopCartList.Add(topCartList);
                }
                
                decimal ORDER_AMOUNT = Convert.ToDecimal(dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"]);

                if (objOrderServices.IsNativeCountry_Express(Convert.ToInt32(_Tbt_Order_Id)) == 0 && userid != "999")
                {
                    decimal tax_amt = objOrderServices.GetTaxAmount(Convert.ToInt32(_Tbt_Order_Id));
                    ORDER_AMOUNT = ORDER_AMOUNT - tax_amt;
                }

                topCart.CART_AMOUNT = ORDER_AMOUNT.ToString();
                topCart.CART_COUNT = dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString();
                topCart.VIEW_ORDER = _Tbt_Ship_URL;
                topCart.SHOW_CART = true;
                topCart.VIEW_ORDER = _Tbt_Ship_URL;
                topCart.TopCartList = lstTopCartList;
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            //                HttpContext.Current.Response.Write();
            return js.Serialize(topCart);
        }


        public static string GetImagePath(object Path)
        {
            // System.IO.FileInfo Fil = null;
            string retpath = string.Empty;
            string strprodimg_sepdomain = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"];
            try
            {
                if (Path.ToString().Contains("noimage.gif"))
                    retpath = strprodimg_sepdomain + "/images/noimage.gif";
                else
                {
                    //Fil = new System.IO.FileInfo(strProdImages + Path.ToString().ToLower().Replace("\\", "/").Replace("_th", "_Images_200"));
                    //if (Fil.Exists)
                    //{
                    //    retpath = strprodimg_sepdomain + Path.ToString().ToLower().Replace("\\", "/").Replace("_th", "_Images_200");
                    //}
                    //else
                    //    retpath = strprodimg_sepdomain + "/images/noimage.gif";

                    //objErrorHandler.CreateLog(Path + "newproductlognav");   
                    if (Path == "")
                    {
                        retpath = strprodimg_sepdomain + "/images/noimage.gif";
                    }
                    else
                    {
                        retpath = strprodimg_sepdomain + Path.ToString().ToLower().Replace("\\", "/").Replace("_th", "_Images_200");
                    }
                }

                return retpath;
            }
            catch
            {
                return retpath;
            }
            finally
            {
                // Fil = null;
            }
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int loadShipMethod(string str)
        {
            OrderServices objOrderServices = new OrderServices();
            if (objOrderServices.IsNativeCountry_Express_userid(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString())) == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int IsNativeCountry_Express(string str)
        {
            OrderServices objOrderServices = new OrderServices();
            int OrderID = Convert.ToInt32(str);
            if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
            {
                return 1;
            }
            //else
            //{
            //    return 0;
            //}
            return 0;
        }

        [System.Web.Services.WebMethod]
        public static string EncryptSPMethod(string ordid)
        {
            return EncryptSP_webmethod(ordid);
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string btnPayMethod(string ttOrder)
        {
           HttpContext.Current.Session["ORDER_ID_NOTHANKS"] = null;
           HttpContext.Current.Session["paypalconfirmation"] = true;
            int OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
            string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
            ErrorHandler objErrorHandler = new ErrorHandler();
            OrderServices objOrderServices = new OrderServices();
            HelperServices objHelperServices = new HelperServices();
            PaymentServices objPaymentServices = new PaymentServices();
            PayPalService objPayPalService = new PayPalService();
            PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
            HelperDB objHelperDB = new HelperDB();
            string refid = "";


            objErrorHandler.CreateOrderSummarylog("btnPay_Click start Orderid=" + OrderID);
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            if (OrderID == 0)
            {
                if (HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                }
            }

            oOrderInfo = objOrderServices.GetOrder(OrderID);
            UserServices objUserServices = new UserServices();
       //     GetParams();
            objErrorHandler.CreateOrderSummarylog("inside Paypal");
            HttpContext.Current.Session["ExpressLevel"] = "3Compl";
            //        SubmitText.Style.Add("display", "none");
            //         PaymentText.Style.Add("display", "block");

       //     SubmitText.Visible = false;
     //       PaymentText.Visible = true;
            //if (oOrderInfo.TotalAmount.ToString() != lblpaypaltotamt.Text)
            //{
            //    objErrorHandler.CreateOrderSummarylog("TotalAmount: " + oOrderInfo.TotalAmount + " " + "lblAmount : " + lblAmount.Text);
            //    //         BtnL4EditShippingMethod_Click(sender, e);                               //commented due to express page
            //    return;
            //}

            if (oOrderInfo.ProdTotalPrice == 0)
            {
                objErrorHandler.CreateOrderSummarylog("btnpaypal start Orderid= " + OrderID + "ProdTotalPrice" + "0");
     //           Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                return "ConfirmMessage.aspx?Result=QTEEMPTY";
            }
            DataSet tmpds = GetOrderItemDetailSum(OrderID);
            decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
            {
                totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            }

            if (oOrderInfo.ProdTotalPrice != totalitemsum)
            {
                objErrorHandler.CreateOrderSummarylog("Prodtitalprice: " + oOrderInfo.ProdTotalPrice + " " + "totalitemsum : " + totalitemsum);
                //          BtnL4EditShippingMethod_Click(sender, e);                                 //commented due to express page
          //      return "";
            }

            if (ttOrder == "" && HttpContext.Current.Session["ORDER_ID"] != null)
            {
                //ttOrder.Text = "WAG" + HttpContext.Current.Session["ORDER_ID"].ToString();

                oPayInfo.PORelease = "WAG" + HttpContext.Current.Session["ORDER_ID"].ToString();
                oPayInfo.OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                objPaymentServices.UpdatePaymentExpress(oPayInfo);
            }
            else if (ttOrder != "")
            {
                refid = objHelperServices.CS(ttOrder);
                try
                {
                    string sSQL = string.Format("EXEC STP_TBWC_CHECK_PO_NUMBER_EXIST '" + HttpContext.Current.Session["ORDER_ID"].ToString() + "','" + HttpContext.Current.Session["USER_ID"].ToString() + "','" + refid + "'");
                    objErrorHandler.CreateLog("paypal" + " " + sSQL);
                    DataTable DS = objHelperDB.GetDataTableDB(sSQL);

                    if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                    {
                        //LoadData();
                        //txterr.Text = "Order No already exists, please Re-enter Order No";
                        //ttOrder.Focus();
                        //SecurePayAcc.Style.Add("display", "none");
                        //ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                        //ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                        //PayPaypalAcc.Style.Add("display", "block");
                        //Level3.Style.Add("display", "none");
                        //Level4.Style.Add("display", "block");
                        //Div3.Style.Add("display", "none");
                        //txterr.Style.Add("display", "block");

                        //ScrollToControl("Level4_Payment", false);
                        return "Order No already exists, please Re-enter Order No";
                    }
                    else
                    {
                        oPayInfo.PORelease = refid;
                        oPayInfo.OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                        objPaymentServices.UpdatePaymentExpress(oPayInfo);
                    }
                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }
            }
            //ttOrder.Enabled = false;
            ////      PayOkDiv.Visible = false;
            //PayOkDiv.Style.Add("display", "none");

            //SecurePayAcc.Style.Add("display", "none");
            //GetParams();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

            renUrl = renUrl.Replace("MCheckOut", "MCheckOut");
            renUrl = renUrl + "?key=" + EncryptSP_webmethod("Paid");
            objErrorHandler.CreateLog("Express_Checkout" + renUrl);
            oUserInfo = objUserServices.GetUserInfo(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
            oPayInfo = objPaymentServices.GetPayment(OrderID);
            int PaymentID = oPayInfo.PaymentID;
            if (oPayInfo.PayResponse.ToLower() == "yes")
            {
                //LoadData();
                //divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                //divContent.Style.Add("display", "block");
                return "Already payment has been made , Ref. Payment History";
            }
            objErrorHandler.CreateOrderSummarylog("Before PayPalInitRequest_ExpressCheckout " + "OrderID : " + OrderID + "PaymentID : " + PaymentID + "renUrl : " + renUrl);
            string Requeststr = objPayPalService.PayPalInitRequest_ExpressCheckout(OrderID, PaymentID, oOrderInfo, renUrl);

            if (Requeststr.Contains("Form") == false)
            {
                objErrorHandler.CreateLog(Requeststr + "to open new page");
                return Requeststr;
      //          divContent.InnerHtml = Requeststr;
            }
            else
            {
                objErrorHandler.CreateLog(Requeststr + "to open new page");
           //      page.Controls.Add(new LiteralControl(Requeststr));
                using (Page page = new Page())
                {
            //        UserControl userControl = (UserControl)page.LoadControl("Express_Checkout.aspx");
           //         (userControl.FindControl("lblMessage") as Label).Text = message;
                    page.Controls.Add(new LiteralControl(Requeststr));
                    using (StringWriter writer = new StringWriter())
                    {
                        page.Controls.Add(new LiteralControl(Requeststr));
                        HttpContext.Current.Server.Execute(page, writer, false);
                        return writer.ToString();
                    }
                }
        //        return Requeststr;
     //           Page.Controls.Add(new LiteralControl(Requeststr));
            }

            //      btnPay.Visible = false;
            //      BtnProgress.Visible = true;
     //       btnPay.Style.Add("display", "none");
    //        BtnProgress.Style.Add("display", "block");
            return "";
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object GetCountryList(string str)
        {
            List<string> result = new List<string>();
            ConnectionDB objConnection = new ConnectionDB();

            using (SqlCommand cmd = new SqlCommand("select COUNTRY_NAME,COUNTRY_CODE FROM TBWC_COUNTRY where COUNTRY_NAME like ''+@SearchText+'%' ORDER BY COUNTRY_NAME  ", objConnection.GetConnection()))
            {



                cmd.Parameters.AddWithValue("@SearchText", str);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    //  result.Add(dr["suburb"].ToString());
                    result.Add(string.Format("{0}/{1}", dr["COUNTRY_NAME"], dr["COUNTRY_CODE"]));

                }



            }
            return result;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object GetAddress(string addressType)
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            HelperDB objHelperDB = new HelperDB();
            string returnval = string.Empty;
            try
            {
                int _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
                object address = null;
                if (addressType == "shipAddress")
                {
                    address = objHelperDB.GetAddress_AddressBook(_UserrID, "SHIP_ADDRESS");
                }
                else if (addressType == "billAddress")
                {
                    address = objHelperDB.GetAddress_AddressBook(_UserrID, "BILL_ADDRESS");
                }
                object company = objUserServices.GetCompanyName(_UserrID);
                returnval = JsonConvert.SerializeObject(new { address, company });
                return returnval;
            }
            catch (Exception ex)
            {

            }
            return "";
        }


        #region "Function.."

        private static decimal calctotalcost_shipping(decimal ProdTotalPrice, string OrderID)
        {
            HelperServices objHelperServices = new HelperServices();
            string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
            decimal ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
            decimal totnor = ShippingValue;
            decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
            decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
            RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
            OrderServices objOrderServices = new OrderServices();
            decimal Totaltaxamount = objOrderServices.GetTotalOrderTaxAmount(objHelperServices.CI(OrderID));
            decimal amt = (ProdTotalPrice + ShippingValue + RetTax_nor + Totaltaxamount);

            return objHelperServices.CDEC(objHelperServices.FixDecPlace(amt));
        }

        private static decimal calctotalcost(decimal ProdTotalPrice, string OrderID)
        {
            HelperServices objHelperServices = new HelperServices();
            string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
            decimal totnor = ProdTotalPrice;
            decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
            decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));

          //  RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
            OrderServices objOrderServices = new OrderServices();
            RetTax_nor = objOrderServices.GetTotalOrderTaxAmount(objHelperServices.CI(OrderID));
            decimal amt = (ProdTotalPrice + RetTax_nor);
            return objHelperServices.CDEC(objHelperServices.FixDecPlace(amt));
        }
        private static decimal calctotalcost_without_duplicate(decimal ProdTotalPrice, string OrderID, string prd_session_id)
        {
            HelperServices objHelperServices = new HelperServices();
            string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
            decimal totnor = ProdTotalPrice;
            decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
            decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));

            //  RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
            OrderServices objOrderServices = new OrderServices();
            RetTax_nor = objOrderServices.GetTotalOrderTaxAmount_Without_Duplicate(objHelperServices.CI(OrderID),prd_session_id);
            decimal amt = (ProdTotalPrice + RetTax_nor);
            return objHelperServices.CDEC(objHelperServices.FixDecPlace(amt));
        }

        private static decimal calctax_shipping(decimal ProdTotalPrice, string OrderID)
        {
            HelperServices objHelperServices = new HelperServices();
            //string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
            decimal ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
        
            //decimal totnor = ProdTotalPrice + ShippingValue;

            //decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
            //decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
            //RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
            OrderServices objOrderServices = new OrderServices();
            decimal RetTax_nor = objOrderServices.CalculateTaxAmount(ProdTotalPrice, ShippingValue, OrderID);
            return objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
        }

        //private static decimal calctax(decimal ProdTotalPrice)
        //{
        //    //HelperServices objHelperServices = new HelperServices();
        //    //string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
        //    //decimal totnor = ProdTotalPrice;
        //    //decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
        //    //decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
        //    //RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
        //    return RetTax_nor;
        //}

        public void LoadData()
        {
            try
            {
                int UseID = 0;
                string ExpressLevel = string.Empty;
                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Session["USER_ID"].ToString() != "999" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
                {
                    UseID = Convert.ToInt32(Session["USER_ID"].ToString());

                    if (UseID > 0)
                    {
                        if (Session["ExpressLevel"].ToString() != "" && Session["ExpressLevel"].ToString() != null)
                        {
                            ExpressLevel = Session["ExpressLevel"].ToString();

                            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                            ouserinfo = objUserServices.GetUserInfo(UseID);
                            if (ExpressLevel == "Start")
                            {
                                //Level1.Visible = false;
                                //Level3.Visible = false;
                                //Level4.Visible = false;

                                //Level2.Visible = true;
                                Level1.Attributes.Add("style", "display:none");
                                Level2.Attributes.Add("style", "display:block");
                                Level3.Attributes.Add("style", "display:none");
                                Level4.Attributes.Add("style", "display:none");
                                BtnL2Continue.Focus();
                                ScrollToControl("ctl00_maincontent_l2div", false);
                                L2name.Text = ouserinfo.Contact;
                                L2Email.Text = ouserinfo.AlternateEmail;
                                L2Phone.Text = ouserinfo.Phone;
                                L2Mobile.Text = ouserinfo.MobilePhone;
                                if (ouserinfo.MobilePhone.ToString().Trim() == "")
                                {
                                    P2Mobile.Visible = false;
                                }
                                else
                                {
                                    P2Mobile.Visible = true;
                                }
                                txtComname.Text = ouserinfo.COMPANY_NAME;
                                if (ouserinfo.Receiver_Contact == "")
                                {
                                    txt_attnto.Text = ouserinfo.Contact;
                                }
                                else
                                {
                                    txt_attnto.Text = ouserinfo.Receiver_Contact;
                                }
                                txtsadd.Text = ouserinfo.ShipAddress1;
                                txtadd2.Text = ouserinfo.ShipAddress2;
                                txttown.Text = ouserinfo.ShipCity;
                                txtzip.Text = ouserinfo.ShipZip;
                                txtstate.Text = ouserinfo.ShipState;
                                drpcountry.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.ShipCountry);
                 //               txtcountry.Text = ouserinfo.ShipCountry;

                       //         if (ouserinfo.ShipCountry.ToLower() == "australia")
                       //         {
                       //             //drpstate1.Attributes.Add("style", "display:block");
                       //             //txtzip.Attributes.Add("style", "display:block");
                       //             //txtzip_inter.Attributes.Add("style", "display:none");
                       //             //txtstate.Attributes.Add("style", "display:none");
                       //             //aucust.Attributes.Add("style", "display:block");
                       //  //           intercust.Attributes.Add("style", "display:none");
                       //             //            drpstate1.Visible = true;
                       //             //            txtzip.Visible = true;
                       //             txtzip.Text = ouserinfo.ShipZip;
                       //             //            txtzip_inter.Visible = false;
                       //             //            txtstate.Visible = false;
                       //             //rfvstate.Enabled = false;
                       //             //            aucust.Visible = true;
                       //             //             intercust.Visible = false;
                       //             Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
                       //         }
                       //         else
                       //         {
                       ////             txtstate.Attributes.Add("style", "display:block");
                       //             //drpstate1.Attributes.Add("style", "display:none");
                       //             //txtzip.Attributes.Add("style", "display:none");
                       //             //txtzip_inter.Attributes.Add("style", "display:block");
                       //             //aucust.Attributes.Add("style", "display:none");
                       //             //intercust.Attributes.Add("style", "display:block");
                       //             //            txtzip.Visible = false;
                       //             //           txtzip_inter.Visible = true;
                       //             //           drpstate1.Visible = false;
                       //             txtzip_inter.Text = ouserinfo.ShipZip;
                       //             //           txtstate.Visible = true;
                       //             txtstate.Text = ouserinfo.ShipState;
                       //             //rfvddlstate.Enabled = false;
                       //             //             aucust.Visible = false;
                       //             //            intercust.Visible = true;
                       //         }

                                //Billing
                                txtsadd_Bill.Text = ouserinfo.BillAddress1;
                                txtadd2_Bill.Text = ouserinfo.BillAddress2;
                                txttown_Bill.Text = ouserinfo.BillCity;
                                txtstate_Bill.Text = ouserinfo.BillState;
                                txtzip_bill.Text = ouserinfo.BillZip;
                    //            txtcountry_Bill.Text = ouserinfo.BillCountry;

                                drpcountry_Bill.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.BillCountry);

                                //if (ouserinfo.BillCountry.ToLower() == "australia")
                                //{
                                //    drpstate2.Attributes.Add("style", "display:block");
                                //    txtstate_Bill.Attributes.Add("style", "display:none");
                                //    //       drpstate2.Visible = true;
                                //    txtzip_bill.Text = ouserinfo.BillZip;
                                //    //        txtstate_Bill.Visible = false;
                                //    txtstate_Bill.Style.Add("display", "none");
                                //    //RequiredFieldValidator14.Enabled = false;

                                //    Setdrpdownlistvalue(drpstate2, ouserinfo.BillState.ToString());
                                //}
                                //else
                                //{
                                //    txtstate_Bill.Attributes.Add("style", "display:block");
                                //    drpstate2.Attributes.Add("style", "display:none");
                                //    //           txtstate_Bill.Visible = true;
                                //    txtstate_Bill.Text = ouserinfo.BillState;
                                //    txtzip_bill.Text = ouserinfo.BillZip;
                                //    //          drpstate2.Visible = false;
                                //    //RequiredFieldValidator13.Enabled = false;
                                //}

                                if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower() && ouserinfo.BillZip == ouserinfo.ShipZip && ouserinfo.BillState == ouserinfo.ShipState && ouserinfo.BillAddress1 == ouserinfo.ShipAddress1)
                                {
                                    ChkBillingAdd.Checked = true;
                                    //                     L2DivBilling.Visible = false;
                                    L2DivBilling.Attributes.Add("style", "display:none");
                                }
                                else
                                {

                                    ChkBillingAdd.Checked = false;
                                    //                  L2DivBilling.Visible = true;
                                    L2DivBilling.Attributes.Add("style", "display:block");
                                }

                                if (drpcountry.SelectedValue == "AU")
                                {
                                    if (Page.IsPostBack)
                                    {
                                        drpSM1.Items.Clear();
                                        drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                        drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                                        drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));

                                        drpSM1.SelectedIndex = 0;
                                        intorder.Attributes.Add("style", "display:none");
                                        //                    intorder.Visible = false;
                                    }
                                }
                                else
                                {
                                    drpSM1.Items.Clear();
                                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                    drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                                    drpSM1.SelectedIndex = 1;
                                    intorder.Attributes.Add("style", "display:block");
                                    //               intorder.Visible = true;
                                }

                                divOk.Attributes.Add("style", "display:none");
                                startdivpwd.Attributes.Add("style", "display:none");
                                getstartwelcome.Attributes.Add("style", "display:none");
                                BtnGetStartLogin.Attributes.Add("style", "display:none");
                                startdivpwd.Attributes.Add("style", "display:none");
                                getstartwelcome.Attributes.Add("style", "display:block");
                                BtnGetStartLogin.Attributes.Add("style", "display:none");
                                letstartregister.Attributes.Add("style", "display:none");
                                BtnGetStartContinue.Attributes.Add("style", "display:none");


                                //Level1.Visible = false;
                                //Level3.Visible = false;
                                //Level4.Visible = false;

                                //              divOk.Visible = false;
                                //              startdivpwd.Visible = false;
                                //              getstartwelcome.Visible = false;
                                //             BtnGetStartLogin.Visible = false;
                                //                      letstartdivlogin.Visible = false;
                                //              startdivpwd.Visible = false;
                                //              getstartwelcome.Visible = true;
                                //              BtnGetStartLogin.Visible = false;
                                //              letstartregister.Visible = false;
                                //              BtnGetStartContinue.Visible = false;
                                BtnL2Continue.Focus();
                                ScrollToControl("ctl00_maincontent_l2div", false);
                                // ScrollToControl("ctl00_maincontent_l2div");
                            }
                            else if (ExpressLevel == "Start_Reg")
                            {
                                letstartregister.Attributes.Add("style", "display:block");
                                startdivpwd.Attributes.Add("style", "display:none");
                                getstartwelcome.Attributes.Add("style", "display:none");
                                BtnGetStartLogin.Attributes.Add("style", "display:none");
                                letstartdivlogin.Attributes.Add("style", "display:none");

                                Level1.Attributes.Add("style", "display:block");
                                Level2.Attributes.Add("style", "display:none");
                                Level3.Attributes.Add("style", "display:none");
                                Level4.Attributes.Add("style", "display:none");
                                //letstartregister.Visible = true;
                                //startdivpwd.Visible = false;
                                //getstartwelcome.Visible = false;
                                //BtnGetStartLogin.Visible = false;
                                //letstartdivlogin.Visible = false;
                                //Level2.Visible = false;
                                //Level3.Visible = false;
                                //Level4.Visible = false;

                                txtregemail.Text = letstarttxtemail.Text;
                                txtregemail.ReadOnly = true;

                            }
                            else if (ExpressLevel == "2Compl")
                            {
                                //Level1.Visible = false;
                                //Level2.Visible = false;
                                Level1.Attributes.Add("style", "display:none");
                                Level2.Attributes.Add("style", "display:none");



                                L3Name.Text = ouserinfo.Contact;
                                L3Email.Text = ouserinfo.AlternateEmail;
                                L3Phone.Text = ouserinfo.Phone;
                                L3Mobile.Text = ouserinfo.MobilePhone;
                                if (ouserinfo.MobilePhone.ToString().Trim() == "")
                                {
                                    P3Mobile.Visible = false;
                                }
                                else
                                {
                                    P3Mobile.Visible = true;
                                }
                                L3ship_company_name.Text = ouserinfo.COMPANY_NAME;

                                oOrdInfo = objOrderServices.GetOrder(Convert.ToInt32(Session["ORDER_ID"].ToString()));
                                if (Session["ORDER_ID_NOTHANKS"] == null || Session["ORDER_ID_NOTHANKS"].ToString() == "0")
                                {
                                    if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null && oOrdInfo.ShipPhone.Substring(0, 2) == "04" && oOrdInfo.ShipPhone.ToString().Trim().Length == 10)
                                    {
                                        lblorderready.Text = oOrdInfo.ShipPhone.ToString().Trim();
                                    }
                                    else if (ouserinfo.MobilePhone != "" && ouserinfo.MobilePhone != null && ouserinfo.MobilePhone.Substring(0, 2) == "04" && ouserinfo.MobilePhone.ToString().Trim().Length == 10)
                                    {
                                        lblorderready.Text = ouserinfo.MobilePhone.ToString().Trim();
                                    }
                                    else if (ouserinfo.ShipPhone != "" && ouserinfo.ShipPhone != null && ouserinfo.ShipPhone.Substring(0, 2) == "04" && ouserinfo.ShipPhone.ToString().Trim().Length == 10)
                                    {
                                        lblorderready.Text = ouserinfo.ShipPhone.ToString().Trim();
                                    }
                                    else if (ouserinfo.Phone != "" && ouserinfo.Phone != null && ouserinfo.Phone.Substring(0, 2) == "04" && ouserinfo.Phone.ToString().Trim().Length == 10)
                                    {
                                        lblorderready.Text = ouserinfo.Phone.ToString().Trim();
                                    }
                                }
                                else
                                {
                                    if (ouserinfo.MobilePhone != "" && ouserinfo.MobilePhone != null && ouserinfo.MobilePhone.Substring(0, 2) == "04" && ouserinfo.MobilePhone.ToString().Trim().Length == 10)
                                    {
                                        hfphonenumber.Value = ouserinfo.MobilePhone.ToString().Trim();
                                    }
                                    else if (ouserinfo.ShipPhone != "" && ouserinfo.ShipPhone != null && ouserinfo.ShipPhone.Substring(0, 2) == "04" && ouserinfo.ShipPhone.ToString().Trim().Length == 10)
                                    {
                                        hfphonenumber.Value = ouserinfo.ShipPhone.ToString().Trim();
                                    }
                                    else if (ouserinfo.Phone != "" && ouserinfo.Phone != null && ouserinfo.Phone.Substring(0, 2) == "04" && ouserinfo.Phone.ToString().Trim().Length == 10)
                                    {
                                        hfphonenumber.Value = ouserinfo.Phone.ToString().Trim();
                                    }
                                }

                                if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                                {
                                    L3ship_attn.Text = ouserinfo.Contact;
                                }
                                else
                                {
                                    L3ship_attn.Text = ouserinfo.Receiver_Contact;
                                }
                                //oOrdBillInfo = objUserServices.GetUserBillInfo(UseID);
                                oOrdShippInfo = objUserServices.GetUserShipInfo(UseID);

                                L3Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                                L3Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                                L3Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                                L3Ship_State.Text = oOrdShippInfo.ShipState;
                                L3Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                                L3Ship_Country.Text = oOrdShippInfo.ShipCountry;
                                L3Ship_DELIVERYINST.Text = oOrdShippInfo.DELIVERYINST;
                                int ordID = 0;
                                ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
                                //objErrorHandler.CreateLog("shipcountry" + oOrdShippInfo.ShipCountry);
                                oOrdInfo = objOrderServices.GetOrder(ordID);
                                L3Ship_DELIVERYINST.Text = ouserinfo.DELIVERYINST;
                                //            hfordernumber.Value = oOrdInfo.ShipPhone;
                                //if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null)
                                //{
                                //    lblorderready.Text = oOrdInfo.ShipPhone;
                                //}
                                //else if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.")
                                //{
                                //    lblorderready.Text = ouserinfo.MobilePhone;
                                //}
                                //if (objOrderServices.IsNativeCountry(ordID) == 0)
                                if (oOrdShippInfo.ShipAddress1 != "")
                                {
                                    Level3.Attributes.Add("style", "display:block");
                                    Level4.Attributes.Add("style", "display:none");
                                    //            Level3.Visible = true;
                                    ScrollToControl("ctl00_maincontent_divl3continue", true);
                                    //           Level4.Visible = false;
                                }
                                else
                                {
                                    Session["ExpressLevel"] = "Start";
                                    LoadData();
                                }


                                if ((L3Ship_Country.Text.ToLower() == "australia"))
                                {

                                    if (drpSM1.SelectedValue == "Please Select Shipping Method")
                                    {
                                        drpSM1.Items.Clear();
                                        drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                        drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                                        drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                                        drpSM1.SelectedIndex = 0;
                                        intorder.Attributes.Add("style", "display:none");
                                        //                                    intorder.Visible = false;
                                    }

                                }
                                else
                                {
                                    drpSM1.Items.Clear();
                                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                    drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                                    drpSM1.SelectedIndex = 1;
                                    intorder.Attributes.Add("style", "display:block");
                                    //                           intorder.Visible = true;
                                }
                                // ScrollToControl("ctl00_maincontent_divl3continue");
                            }
                            else if (ExpressLevel == "3Compl")
                            {

                                int ordID = 0;
                                ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());

                                if (objOrderServices.IsNativeCountry_Express(ordID) == 1)
                                {
                                    //Level1.Visible = false;
                                    //Level2.Visible = false;
                                    //Level3.Visible = false;
                                    //Level4.Visible = true;


                                    //Level1.Visible = false;
                                    //Level2.Visible = false;
                                    //Level3.Visible = false;

                                    //Level4_Submit.Visible = false;

                                    Level1.Attributes.Add("style", "display:none");
                                    Level2.Attributes.Add("style", "display:none");
                                    Level3.Attributes.Add("style", "display:none");
                                    Level4.Attributes.Add("style", "display:block");
                                    Level4_Submit.Attributes.Add("style", "display:none");

                                    divOk.Attributes.Add("style", "display:none");
                                    PayType.Attributes.Add("style", "display:block");
                                    div2.Attributes.Add("style", "display:none");
                                    Div3.Attributes.Add("style", "display:none");
                                    SubmitText.Attributes.Add("style", "display:none");


                                    //          divOk.Visible = false;
                                    //          PayType.Visible = true;
                                    L4name.Text = ouserinfo.Contact;
                                    L4Email.Text = ouserinfo.AlternateEmail;
                                    L4Phone.Text = ouserinfo.Phone;
                                    L4Mobile.Text = ouserinfo.MobilePhone;
                                    if (ouserinfo.MobilePhone.ToString().Trim() == "")
                                    {
                                        P4Mobile.Visible = false;
                                    }
                                    else
                                    {
                                        P4Mobile.Visible = true;
                                    }
                                    L4Ship_Company.Text = ouserinfo.COMPANY_NAME;
                                    if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                                    {
                                        L4Ship_Attnto.Text = ouserinfo.Contact;
                                    }
                                    else
                                    {
                                        L4Ship_Attnto.Text = ouserinfo.Receiver_Contact;
                                    }
                                    oOrdShippInfo = objUserServices.GetUserShipInfo(UseID);

                                    L4Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                                    L4Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                                    L4Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                                    L4Ship_State.Text = oOrdShippInfo.ShipState;
                                    L4Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                                    L4Ship_Country.Text = oOrdShippInfo.ShipCountry;





                                    oOrdInfo = objOrderServices.GetOrder(ordID);
                                    lblShippingMethod.Text = oOrdInfo.ShipMethod.Replace("Counter Pickup", "Shop Counter Pickup");
                                    L4Comments.Text = oOrdInfo.ShipNotes;

                                    L4Ship_DELIVERYINST.Text = ouserinfo.DELIVERYINST;
                                    oPayInfo = objPaymentServices.GetPayment(ordID);
                                    //             div2.Visible = false;

                                    if (lblShippingMethod.Text == "Standard Shipping")
                                    {
                                        divshopcounter.Attributes.Add("style", "display:none");
                                        //                            divshopcounter.Visible = false;
                                        //                                   calctotalcost_shipping(oOrdInfo.ProdTotalPrice);
                                        lblAmount.Text = calctotalcost_shipping(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();
                                        lblpaypaltotamt.Text = calctotalcost_shipping(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();
                                        objErrorHandler.CreatePayLog_Final("after calculation load data" + lblAmount.Text);
                                    }
                                    else
                                    {
                                        divshopcounter.Attributes.Add("style", "display:block");
                                        //                                 calctotalcost(oOrdInfo.ProdTotalPrice);
                                        lblAmount.Text = calctotalcost(oOrdInfo.ProdTotalPrice,OrderID.ToString()).ToString();
                                        lblpaypaltotamt.Text = calctotalcost(oOrdInfo.ProdTotalPrice, OrderID.ToString()).ToString();
                                        //                            divshopcounter.Visible = true;
                                    }
                                    if (Convert.ToDecimal(lblAmount.Text) > 300)
                                    {
                                        ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                                        // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                                        ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                                        //            btnPayApi.Visible = false;
                                        //           btnPay.Visible = true;
                                        //            PaypalApiDiv.Visible = true;
                                        //             PayPaypalAcc.Visible = true;
                                        //lblsp.Attributes.Add("style", "display:none");   //commented on 07-07-2020
                                        btnPayApi.Attributes.Add("style", "display:none");
                              //          btnPay.Attributes.Add("style", "display:block");
                                        PaypalApiDiv.Attributes.Add("style", "display:block");
                                        PayPaypalAcc.Attributes.Add("style", "display:block");
                                        SecurePayAcc.Attributes.Add("style", "display:none");
                                        //             SecurePayAcc.Visible = false;
                                        ScrollToControl("Level4_Payment", true);
                                    }
                                    else
                                    {
                                        ScrollToControl("Level4_Payment", true);
                                    }
                                }
                                else
                                {
                                    //Level1.Visible = false;
                                    //Level2.Visible = false;
                                    //Level3.Visible = false;
                                    //Level4.Visible = true;
                                    Level1.Attributes.Add("style", "display:none");
                                    Level2.Attributes.Add("style", "display:none");
                                    Level3.Attributes.Add("style", "display:none");
                                    Level4.Attributes.Add("style", "display:block");



                                    L4name.Text = ouserinfo.Contact;
                                    L4Email.Text = ouserinfo.AlternateEmail;
                                    L4Phone.Text = ouserinfo.Phone;
                                    L4Mobile.Text = ouserinfo.MobilePhone;
                                    if (ouserinfo.MobilePhone.ToString().Trim() == "")
                                    {
                                        P4Mobile.Visible = false;
                                    }
                                    else
                                    {
                                        P4Mobile.Visible = true;
                                    }
                                    L4Ship_Company.Text = ouserinfo.COMPANY_NAME;
                                    if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                                    {
                                        L4Ship_Attnto.Text = ouserinfo.Contact;
                                    }
                                    else
                                    {
                                        L4Ship_Attnto.Text = ouserinfo.Receiver_Contact;
                                    }
                                    oOrdShippInfo = objUserServices.GetUserShipInfo(UseID);

                                    L4Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                                    L4Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                                    L4Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                                    L4Ship_State.Text = oOrdShippInfo.ShipState;
                                    L4Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                                    L4Ship_Country.Text = oOrdShippInfo.ShipCountry;
                                    L4Ship_DELIVERYINST.Text = oOrdInfo.DeliveryInstr;

                                    oOrdInfo = objOrderServices.GetOrder(ordID);
                                    lblShippingMethod.Text = oOrdInfo.ShipMethod.Replace("Counter Pickup", "Shop Counter Pickup");
                                    L4Comments.Text = oOrdInfo.ShipNotes;


                                    oPayInfo = objPaymentServices.GetPayment(ordID);
                                    //ttOrder.Text = oPayInfo.PORelease;
                                    if (oPayInfo.Amount != oOrdInfo.TotalAmount)
                                    {
                                        oPayInfo.Amount = oOrdInfo.TotalAmount;
                                    }
                                    //  lblpaypaltotamt.Text = oPayInfo.Amount.ToString();

                                    div2.Attributes.Add("style", "display:none");
                                    divshopcounter.Attributes.Add("style", "display:none");
                                    ImageButton1.Attributes.Add("style", "display:none");
                                    divshopcounter.Attributes.Add("style", "display:none");
               //                     Level4_Payment.Attributes.Add("style", "display:none");
                                    Level4_Submit.Attributes.Add("style", "display:block");
                                    PHOrderConfirm.Attributes.Add("style", "display:block");                  //uncomment the line 
                                    checkoutrightL1.Attributes.Add("style", "display:none");
                                    checkoutrightL4.Attributes.Add("style", "display:block");
                                    PaymentText.Attributes.Add("style", "display:none");

                                    //          div2.Visible = false;
                                    //          divshopcounter.Visible = false;

                                    //          ImageButton1.Visible = false;
                                    //         divshopcounter.Visible = false;
                                    //         Level4_Payment.Visible = false;
                                    //         Level4_Submit.Visible = true;
                                    //            PHOrderConfirm.Visible = true;
                                    //         checkoutrightL1.Visible = false;
                                    //          checkoutrightL4.Visible = true;

                                    if (oOrdInfo.TotalAmount > 300)
                                    {
                                        ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                                        // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                                        ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                                        //          btnPayApi.Visible = false;
                                        //           btnPay.Visible = true;
                                        //PaySPDiv.Visible = false;
                                        //          PaypalApiDiv.Visible = true;
                                        //           PayPaypalAcc.Visible = true;
                                        PayPaypalAcc.Style.Add("display", "block");

                                        btnPayApi.Attributes.Add("style", "display:none");
                            //            btnPay.Attributes.Add("style", "display:block");
                                        PaypalApiDiv.Attributes.Add("style", "display:block");
                                        PayPaypalAcc.Attributes.Add("style", "display:block");
                                        SecurePayAcc.Attributes.Add("style", "display:none");

                                        //           SecurePayAcc.Visible = false;
                                        //btnPay.Focus();
                                        ScrollToControl("Level4_Payment", true);
                                    }
                                    else
                                    {
                                        //btnSP.Focus();
                                        ScrollToControl("Level4_Payment", true);
                                    }
                                }
                                //                PayType.Visible = true;
                                PayType.Attributes.Add("style", "display:block");
                            }
                            else
                            {
                            }
                        }
                    }
                }
                else
                {
                    if (Session["ExpressLevel"].ToString() != "" && Session["ExpressLevel"].ToString() != null)
                    {
                        ExpressLevel = Session["ExpressLevel"].ToString();
                        if (ExpressLevel == "Start_Reg")
                        {
                            Level1.Attributes.Add("style", "display:block");
                            Level2.Attributes.Add("style", "display:none");
                            Level3.Attributes.Add("style", "display:none");
                            Level4.Attributes.Add("style", "display:none");

                            letstartregister.Attributes.Add("style", "display:block");
                            startdivpwd.Attributes.Add("style", "display:none");
                            getstartwelcome.Attributes.Add("style", "display:none");
                            BtnGetStartLogin.Attributes.Add("style", "display:none");
                            letstartdivlogin.Attributes.Add("style", "display:none");
                            //letstartregister.Visible = true;
                            //startdivpwd.Visible = false;
                            //getstartwelcome.Visible = false;
                            //BtnGetStartLogin.Visible = false;
                            //letstartdivlogin.Visible = false;
                            //Level2.Visible = false;
                            //Level3.Visible = false;
                            //Level4.Visible = false;

                        }
                    }

                }
                //Session["ExpressLevel"] 
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }

        [System.Web.Services.WebMethod]
        public static string SaleTrans(string nounce, string Amount)
        {
            try
            {

                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    string x = "";
                    var gateway = new BraintreeGateway
                    {
                        Environment = Braintree.Environment.SANDBOX,
                        MerchantId = "mjff7p7mgb4qmp77",
                        PublicKey = "p78kxf6s7zhb8z8x",
                        PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
                    };

                    //var gateway = new BraintreeGateway
                    //{
                    //    Environment = Braintree.Environment.PRODUCTION,
                    //    MerchantId = "wrv3fq8x3r269ycd",
                    //    PublicKey = "nm7v4wm8dmw7b6rq",
                    //    PrivateKey = "a3d333f589d80552db255c34c1407c40"
                    //};
                    SecurePayService objSecurePayService = new SecurePayService();
                    SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
                    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                    PaymentServices objPaymentServices = new PaymentServices();


                    string OrderID = HttpContext.Current.Session["ORDER_ID"].ToString();
                    int intOrderID = Convert.ToInt32(OrderID);

                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    OrderServices objOrderServices = new OrderServices();
                    oOrderInfo = objOrderServices.GetOrder(intOrderID);

                    ErrorHandler objerrhandler = new ErrorHandler();


                    if (oOrderInfo.TotalAmount != Convert.ToDecimal(Amount))

                    {

                        x = "Error " + "Order Amount Missmatch";
                        objerrhandler.CreateLogEA("Order Amount Missmatch" + oOrderInfo.TotalAmount + "--" + Amount + "--Orderid" + OrderID);
                        return x;
                    }
                    if (oOrderInfo.ShipFName == "" || oOrderInfo.ShipAdd1 == "" || oOrderInfo.ShipMethod == "Please Select Shipping Method")

                    {

                        x = "Error " + "Invalid User Data .Please Fill Mandatory Fields";
                        objerrhandler.CreateLogEA(x);
                        return x;
                    }
                    if (oOrderInfo.OrderStatus == 1)

                    {


                        oPayInfo = objPaymentServices.GetPayment(intOrderID);



                        int PaymentID = oPayInfo.PaymentID;

                        //MerchantAccountId = "wagnerelectronicsAUD",


                        var request = new TransactionRequest
                        {
                            Amount = Convert.ToDecimal(oOrderInfo.TotalAmount),
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
                            DeviceData = HttpContext.Current.Request.Form["device_data"]
                        };

                        if (nounce == "no")
                        {
                            x = "Error " + "Please Try again or use a different card / payment method.";
                            objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", "No Nounce", "", "No", "", "No Nounce", "", "", "Error Processing PARes");
                            objerrhandler.CreateLogEA(" br  Orderid=" + OrderID + "Error Processing PARes");

                            return x;
                        }
                        //MerchantAccountId = "wagnerelectronicsAUD",
                        PaymentMethodNonce paymentMethodNonce = null;
                        try
                        {
                            paymentMethodNonce = gateway.PaymentMethodNonce.Find(nounce);
                        }
                        catch (Exception ex)
                        {
                            objerrhandler.CreateLog(ex.ToString() + "nounce" + nounce);

                        }
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

                            objerrhandler.CreateLogEA("IsSucess:" + result.IsSuccess());

                            if (result.IsSuccess() == true)
                            {


                                Transaction transaction = result.Target;
                                objerrhandler.CreateLogEA("Status:" + transaction.Status.ToString());
                                string ResponseId = transaction.Id;
                                string ResponseText = transaction.ProcessorResponseText;
                                string Responsecode = transaction.ProcessorResponseCode;

                                string cardtype = result.Target.CreditCard.CardType.GetType().ToString();
                                objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, result.Target.CreditCard.CardType.ToString(), result.Target.CreditCard.CardholderName, result.Target.CreditCard.MaskedNumber, result.Target.CvvResponseCode, result.Target.CreditCard.ExpirationDate, "YES", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, LiabilityShifted.ToString(), Status);
                                HttpContext.Current.Session["paySPresponse"] = "SUCCESS";

                                objOrderServices.UpdatePaymentOrderStatus_Express(intOrderID, PaymentID, false);
                                // objOrderServices.UpdatePAYMENTSELECTION(intOrderID, "BR-" + result.Target.CreditCard.CardType.ToString());
                                objOrderServices.UpdatePAYMENTSELECTION(intOrderID, "BR");
                                objerrhandler.CreateLogEA("Card No:" + result.Target.CreditCard.MaskedNumber);
                                //  ErrorHandler objErrorHandler = new ErrorHandler();
                                objerrhandler.CreateLogEA("************************");
                                //   objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                                TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                tbwtEngine.SendMail_AfterPaymentSP(intOrderID, (int)OrderServices.OrderStatus.Payment_Successful, false, result.Target.CreditCard.CardType.ToString());
                                tbwtEngine.SendMail_Review(intOrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                HttpContext.Current.Session["ORDER_ID"] = "0";
                                HttpContext.Current.Session["OrderDetExp_orderid"] = OrderID;
                                HttpContext.Current.Session["OrderDetExp_userid"] = oPayInfo.UserId;
                                HttpContext.Current.Session["hfisordercompleted"] = OrderID;



                                return oPayInfo.PORelease;

                                //  x = "true";
                            }
                            else
                            {
                                try
                                {
                                    Transaction transaction = result.Transaction;
                                    // objerrhandler.CreateLogEA("Card No:" + result.Transaction.CreditCard.MaskedNumber);
                                    string errorMessages = "";
                                    // objerrhandler.CreateLogEA("Error Status:" + transaction.Status.ToString());

                                    objerrhandler.CreateLogEA("************************");
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

                                    objerrhandler.CreateLogEA(" br  Orderid=" + OrderID + errorMessages);
                                    objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, LiabilityShifted.ToString(), Status);
                                    x = "Error " + "Please Try again or use a different card / payment method.";
                                }
                                catch (Exception ex)
                                {
                                    x = "Error " + "Please Try again or use a different card / payment method.";
                                    objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", TRANSACTIONID, "", "No", "", "Failed", "", LiabilityShifted.ToString(), Status);
                                    objerrhandler.CreateLogEA(" br  Orderid=" + OrderID + Status);
                                }
                            }


                            return x;
                        }
                        else
                        {
                            //   objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);
                            x = "Error " + "Please Try again or use a different card / payment method.";
                            objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", TRANSACTIONID, "", "No", "", "Failed", "", LiabilityShifted.ToString(), Status);
                            objerrhandler.CreateLogEA(" br  Orderid=" + OrderID + Status);

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
        [System.Web.Services.WebMethod]
        public static string SaleTrans_Paypal(string nounce, string Amount, string devicedata)
        {
            try
            {

                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    string x = "";
                    var gateway = new BraintreeGateway
                    {
                        Environment = Braintree.Environment.SANDBOX,
                        MerchantId = "mjff7p7mgb4qmp77",
                        PublicKey = "p78kxf6s7zhb8z8x",
                        PrivateKey = "c747edab3bb504dc5fb9606a1716ccd7"
                    };


                    PayPalService objPayService = new PayPalService();
                    PayPalService.PaymentRequestInfo objPRInfo = new PayPalService.PaymentRequestInfo();
                    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                    PaymentServices objPaymentServices = new PaymentServices();


                    string OrderID = HttpContext.Current.Session["ORDER_ID"].ToString();
                    int intOrderID = Convert.ToInt32(OrderID);

                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    OrderServices objOrderServices = new OrderServices();
                    oOrderInfo = objOrderServices.GetOrder(intOrderID);







                    if (oOrderInfo.OrderStatus == 1)

                    {
                        oPayInfo = objPaymentServices.GetPayment(intOrderID);

                        int PaymentID = oPayInfo.PaymentID;




                        //var request = new TransactionRequest
                        //{
                        //    Amount = Convert.ToDecimal(Amount),

                        //    PaymentMethodNonce = nounce,

                        //    Options = new TransactionOptionsRequest
                        //    {
                        //        SubmitForSettlement = true
                        //    },
                        //    BillingAddress = new PaymentMethodAddressRequest
                        //    {
                        //        PostalCode = oPayInfo.Zip,
                        //        FirstName = oPayInfo.BillFName,
                        //        LastName = oPayInfo.BillLName,
                        //        StreetAddress = oPayInfo.Address1,
                        //        Locality = oPayInfo.City,
                        //        Company = oPayInfo.Country

                        //    },
                        //    OrderId = OrderID,
                        //    DeviceData = HttpContext.Current.Request.Form["device_data"]
                        //};

                        //Result<Transaction> result = gateway.Transaction.Sale(request);

                        var request1 = new PaymentMethodRequest
                        {

                            PaymentMethodNonce = nounce,

                            DeviceData = devicedata
                        };
                        TransactionRequest request = new TransactionRequest
                        {
                            Amount = Convert.ToDecimal(Amount),
                            PaymentMethodNonce = nounce,
                            DeviceData = devicedata,
                            OrderId = OrderID,
                            Descriptor = new DescriptorRequest
                            {
                                Name = "company*my product",
                                Phone = "3125551212",
                                Url = "company.com"
                            },
                            Options = new TransactionOptionsRequest
                            {
                                SubmitForSettlement = true,
                                PayPal = new TransactionOptionsPayPalRequest
                                {
                                    CustomField = "PayPal custom field",
                                    Description = "Description for PayPal email receipt"
                                }
                            },
                            BillingAddress = new AddressRequest
                            {
                                PostalCode = oPayInfo.Zip,
                                FirstName = oPayInfo.BillFName,
                                LastName = oPayInfo.BillLName,
                                StreetAddress = oPayInfo.Address1,
                                Locality = oPayInfo.City,
                                Company = oPayInfo.Country

                            },
                            ShippingAddress = new AddressRequest
                            {
                                PostalCode = oPayInfo.Zip,
                                FirstName = oPayInfo.BillFName,
                                LastName = oPayInfo.BillLName,
                                StreetAddress = oPayInfo.Address1,
                                Locality = oPayInfo.City,
                                Company = oPayInfo.Country

                            }
                        };

                        Result<Transaction> result = gateway.Transaction.Sale(request);


                        if (result.IsSuccess() == true)
                        {


                            Transaction transaction = result.Target;
                            string ResponseId = transaction.Id;
                            string ResponseText = transaction.ProcessorResponseText;
                            string Responsecode = transaction.ProcessorResponseCode;

                            string cardtype = result.Target.CreditCard.CardType.GetType().ToString();
                            string Request_info = objPayService.PayPalInitRequest_ExpressCheckout_BR(intOrderID, PaymentID, oOrderInfo, "ExpressCheckout.aspx");



                            objPRInfo.Amount = Convert.ToDecimal(Amount);
                            objPRInfo.Order_id = intOrderID;
                            objPRInfo.Payment_id = PaymentID;
                            objPRInfo.Payment_Request_id = transaction.PayPalDetails.PaymentId;
                            objPRInfo.Request_info = Request_info;
                            objPRInfo.Response_Approved = "Yes";
                            objPRInfo.Response_Code = transaction.ProcessorResponseCode;
                            objPRInfo.Response_Receipt_ID = transaction.PayPalDetails.AuthorizationId;
                            objPRInfo.Response_Status_Code = transaction.ProcessorSettlementResponseCode;
                            objPRInfo.Response_Status_desc = transaction.ProcessorSettlementResponseText;
                            objPRInfo.Response_Text = transaction.ProcessorResponseText;
                            objPRInfo.Response_Txn_ID = ResponseId;
                            objPRInfo.request_type = transaction.PaymentInstrumentType.ToString();
                            objPRInfo.Request_id = HttpContext.Current.Session["X_P"].ToString();
                            objPayService.UpdateRequest(objPRInfo);
                            HttpContext.Current.Session["payflowresponse"] = "SUCCESS";

                            objOrderServices.UpdatePaymentOrderStatus_Express(intOrderID, PaymentID, false);
                            objOrderServices.UpdatePAYMENTSELECTION(intOrderID, "PP");
                            //   objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                            IPNHandler tbwtEngine = new IPNHandler();
                            tbwtEngine.SendMail_AfterPaymentPP(intOrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);

                            // int Rtn = objOrderServices.SentSignal(objPRInfo.Payment_id.ToString(), objPRInfo.Order_id.ToString(), "190");
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
                            string Request_info = objPayService.PayPalInitRequest_ExpressCheckout_BR(intOrderID, PaymentID, oOrderInfo, "ExpressCheckout.aspx");
                            //  objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);


                            objPRInfo.Amount = Convert.ToDecimal(Amount);
                            objPRInfo.Order_id = intOrderID;
                            objPRInfo.Payment_id = PaymentID;
                            objPRInfo.Response_Approved = "No";
                            objPRInfo.Response_Code = transaction.ProcessorResponseCode;
                            objPRInfo.Response_Receipt_ID = transaction.Id;
                            objPRInfo.Response_Status_Code = transaction.ProcessorResponseCode;
                            objPRInfo.request_type = transaction.PaymentInstrumentType.ToString();
                            objPRInfo.Response_Status_desc = errorMessages;
                            objPRInfo.Response_Text = transaction.ProcessorResponseText;


                            objPayService.UpdateRequest(objPRInfo);
                            x = "Error " + errorMessages;
                        }


                        return x;
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



        #endregion
    }
}