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


public partial class ExpressCheckout : System.Web.UI.Page
    {
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
        //ConnectionDB objConnectionDB = new ConnectionDB();
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
        //ListItem oLstItem = new ListItem();
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
        String sessionId;


        protected override void OnLoad(EventArgs e)
        {
            //Handling autocomplete issue generically
            if (this.Form != null)
            {
                this.Form.Attributes.Add("autocomplete", "off");
            }

            base.OnLoad(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
                //Level3.Visible = true; 
                // return;
              
                ImageButton3.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";
                ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_check.png";
                ImagePay.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_uncheck.png";
                btnclose1.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";

                SCP.Style.Add("display", "none");
                if (!Page.IsPostBack)
                {
                    if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Session["USER_ID"].ToString() != "999" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
                    {
                        int UseID = Convert.ToInt32(Session["USER_ID"].ToString());
                        UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                        if (ouserinfo.FirstName == "" || ouserinfo.FirstName == null)
                        {
                            Userid = UseID;
                            BtnEditLogin_Click(sender, e);
                            return;
                        }
                    }
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
                  
                    if (drpSM1.SelectedValue == "Standard Shipping")
                    {

                        lblshipcost.Text = objHelperServices.GetOptionValues("COURIER CHARGE").ToString();

                        //shipping_charge_domestic_zero.Style.Add("display", "none");
                        //shipping_charge_domestic_value.Style.Add("display", "block");
                    }
                    else
                    {
                        //shipping_charge_domestic_zero.Style.Add("display", "block");
                        //shipping_charge_domestic_value.Style.Add("display", "none");
                        lblshipcost.Text = "0.00";
                    }
                    LoadCountryList1();
                    LoadCountryListBill();
                    intercust.Visible = false;
                    string countryCode = drpCountry.SelectedValue.ToString();
                    LoadStates(drpCountry.SelectedValue);

                    string countrycode_bill = drpcountry_bill.SelectedValue.ToString();
                    LoadStatesBill(drpcountry_bill.SelectedValue);

                    startdivpwd.Visible = false;
                    getstartwelcome.Visible = false;
                    BtnGetStartLogin.Visible = false;
                    letstartregister.Visible = false;
                    Level2.Visible = false;
                    Level3.Visible = false;
                    Level4.Visible = false;
                    Level4_Submit.Visible = false;
                    checkoutrightL4.Visible = false;


                    L4AEditAddress.Visible = false;
                    L3AEditAddress.Visible = false;
                    L4AEditShippingMethod.Visible = false;

                    divOk.Visible = false;

                    //  PayType.Visible = false;
                    // PayPaypalAcc.Visible = false;


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
                            divTimeout.Visible = true;
                            divCC.Visible = false;
                            return;
                        }
                    }


                    //if (OrderID <= 0)
                    //{
                    //    OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
                    //}
                    //else
                    //{
                    //    Session["ORDER_ID"] = OrderID;
                    //    //OrderID = System.Convert.ToInt32(Request["OrderID"]);
                    //}
                    //if (objOrderServices.IsNativeCountry(OrderID) == 0)
                    //{

                    if (Session["ORDER_ID"] != null && Session["ORDER_ID"].ToString() != "")
                    {
                        OrderID = Convert.ToInt32(Session["ORDER_ID"].ToString());
                    }

                    //if (Userid != 999)
                    //    GetParams_orderid(Userid.ToString());



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
                            objErrorHandler.CreateOrderSummarylog("orderdstatus: " + orderdstatus + "OrderID: " + OrderID +"b4 cart empty");
                            Session["ORDER_ID"] = 0;
                            Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                            return;
                        }
                    }


                    DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK", "");
                    DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE", sessionId);

                    this.HiddenpopuploginregPanel.Visible = false;
                    this.modalPop_loreg.Hide();

                    // this.Panelerroritems.Visible = false;
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

                                    //if (Request["QteId"] != null)
                                    //    QuoteID = objHelperServices.CI(Request["QteId"].ToString());
                                    //else
                                    //    QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);

                                    //if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                                    //    lblShoppingCart.Text = "Quote Cart";

                              // decimal OrdtotCost = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));
                          
                                    
                                    //  decimal QtetotCost = objHelperServices.CDEC(objQuoteServices.GetCurrentProductTotalCost(QuoteID));
                                    //&& OrdtotCost > 0

                                    if ((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0 ) || Request["QteFlag"] == "1")
                                    {
                                        int a = 5;

                                        string txtadd = "";


                                        LoadCountryList();


                                        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                        oOrderInfo = objOrderServices.GetOrder(OrderID);


                                        //   Load_UserRole(Session["USER_ID"].ToString());
                                    }
                                    // }
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
                  //  Load_UserRole(Session["USER_ID"].ToString());

                    //ttOrder.Attributes.Add("onchange", "return isAlphabetic();");
                    //ttOrder.Attributes.Add("onblur", "return isAlphabetic();");

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
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:HidePanels();", true);

                    drpSM1.Attributes.Add("onclick", "javascript:CheckShippment();");
                    drpSM1.Attributes.Add("onchange", "javascript:CheckShippment();");
                    // ImageButton4.Attributes.Add("onchange", "javascript:DRPshippment();");
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
                    if (Paytab == true)
                    {
                        //PnlOrderInvoice.Visible = true;
                        // PnlOrderContents.Visible = false;

                        liPayOption.Visible = true;

                        div2.Visible = false;
                        //ttOrder.Enabled = false;
                        //drpSM1.Enabled = false;




                        if (!Page.IsPostBack)
                        {
                            PHOrderConfirm.Visible = true;
                            // Level4_Payment.Visible = false;
                            // Level4_Submit.Visible = true;
                            //new start
                            //txtfname.Enabled = false;
                            // txtlname.Enabled = false;
                            //txtphone.Enabled = false;
                            // txtemail.Enabled = false;
                            txtsadd.Enabled = false;
                            txtadd2.Enabled = false;
                            txttown.Enabled = false;
                            drpstate1.Enabled = false;
                            txtzip.Enabled = false;
                            drpCountry.Enabled = false;
                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);
                            oPayInfo = objPaymentServices.GetPayment(OrderID);
                            //ttOrder.Text = oPayInfo.PORelease;
                            //objErrorHandler.CreatePayLog_Final("page load" + oOrdInfo.TotalAmount);


                            Setdrpdownlistvalue(drpSM1, oOrderInfo.ShipMethod);
                            TextBox1.Text = oOrderInfo.ShipNotes;
                            spanPay.Visible = true;
                           // objErrorHandler.CreatePayLog_Final("inside page load" + oOrdInfo.TotalAmount);
                            if (oOrderInfo.ShipMethod == "Standard Shipping")
                            {
                                calctotalcost_shipping(oOrderInfo.ProdTotalPrice);

                            }
                            else
                            {
                                calctotalcost(oOrderInfo.ProdTotalPrice);
                            }

                            TotAmt = Convert.ToDouble(oOrderInfo.TotalAmount);
                            // new start
                            //txtfname.Text = oOrderInfo.BillFName;
                            // txtlname.Text = oOrderInfo.BillLName;
                            // txtphone.Text = oOrderInfo.BillPhone;
                            // txtemail.Text = objUserServices.GetUserEmailAdd(Userid);
                            txtsadd.Text = oOrderInfo.ShipAdd1;
                            txtadd2.Text = oOrderInfo.ShipAdd2;
                            txttown.Text = oOrderInfo.ShipCity;
                            drpstate1.Text = oOrderInfo.ShipState;
                            txtzip.Text = oOrderInfo.ShipZip;

                            //new end


                            Level1.Visible = false;
                            Level2.Visible = false;
                            Level3.Visible = false;
                            Level4.Visible = true;
                            //Level4_Submit.Visible = false;

                            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                            ouserinfo = objUserServices.GetUserInfo(Userid);
                            L4name.Text = ouserinfo.Contact;
                            L4Email.Text = ouserinfo.AlternateEmail;
                            L4Phone.Text = ouserinfo.Phone;
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
                                divshopcounter.Visible = false;
                            }
                            else
                            {
                                divshopcounter.Visible = true;
                            }
                            oOrdInfo = objOrderServices.GetOrder(OrderID);
                            lblShippingMethod.Text = oOrdInfo.ShipMethod.Replace("Counter Pickup", "Shop Counter Pickup");


                        }



                        h3Pay.Visible = true;
                        h3Pay1.Visible = false;

                        //  SetTabsetting("ship", false);
                        SetTabsetting("Pay", true);

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
                                PayType.Visible = true;
                                PaypalApiDiv.Visible = true;
                                btnPay.Visible = true;
                                btnPayApi.Visible = false;

                                if ((TotAmt > 300))
                                {
                                    ImagePay.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                                    // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                                    ImagePaySP.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                                    btnPayApi.Visible = false;
                                    btnPay.Visible = true;
                                    //PaySPDiv.Visible = false;
                                    PaypalApiDiv.Visible = true;
                                    PayPaypalAcc.Visible = true;
                                    PayPaypalAcc.Style.Add("display", "block");

                                    SecurePayAcc.Visible = false;
                                    ScrollToControl("ctl00_maincontent_Level4_Payment", false);
                                }
                                else
                                {
                                    // btnSP.Focus();
                                    ScrollToControl("ctl00_maincontent_Level4_Payment", false);
                                }
                                //  PaySPDiv.Visible = false;
                                // if ((TotAmt <= 100) && (objOrderServices.IsNativeCountry(OrderID) == 1))
                                // {
                                //     PaySPDiv.Visible = true;
                                // }

                            }


                            else
                            {
                                PayType.Visible = true;
                                if ((TotAmt <= 300) && (objOrderServices.IsNativeCountry_Express(OrderID) == 1))
                                {
                                    PaypalApiDiv.Visible = false;
                                    //btnSP.Focus(); 
                                    ScrollToControl("ctl00_maincontent_Level4_Payment", false);
                                    // PaySPDiv.Visible = true;
                                }
                                else if ((TotAmt > 300))
                                {
                                    ImagePay.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                                    // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                                    ImagePaySP.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                                    btnPayApi.Visible = false;
                                    btnPay.Visible = true;
                                    //PaySPDiv.Visible = false;
                                    PaypalApiDiv.Visible = true;
                                    PayPaypalAcc.Visible = true;
                                    PayPaypalAcc.Style.Add("display", "block");

                                    SecurePayAcc.Visible = false;
                                    //btnPay.Focus();
                                    ScrollToControl("ctl00_maincontent_Level4_Payment", false);
                                }
                                else
                                {
                                    PaypalApiDiv.Visible = true;
                                    //btnSP.Focus();
                                    ScrollToControl("ctl00_maincontent_Level4_Payment", false);
                                    // PaySPDiv.Visible = false;
                                    //ImagePay.ImageUrl = "/images/paypal_check.png";
                                }
                            }

                        }




                    }
                    else
                    {
                        Paydiv.Visible = false;
                        spanPay.Visible = false;
                        h3Pay.Visible = false;
                        h3Pay1.Visible = true;

                        PayType.Visible = true;

                        //checkoutrightL1.Visible = false;
                        //checkoutrightL4.Visible = true;

                    }

                    if (paidtab == true)
                    {
                        //objErrorHandler.CreatePayLog("paidtab == true start");

                        //PnlOrderInvoice.Visible = true;
                        //PnlOrderContents.Visible = false;
                        Session["OrderDetExp_orderid"] = OrderID;
                        Session["OrderDetExp_userid"] = Userid;
                        of_ptag.Visible = false;
                        liFinalReview.Visible = true;
                        paiddiv.Visible = true;
                        btnPay.Visible = false;
                        btnPayApi.Visible = false;
                        spanpaid.Visible = true;
                        btneditlogin4.Visible = false;
                        L4AEditShippingMethod.Visible = false;
                        L4AEditShippingMethod.Style.Add("display", "none");
                        PaypalApiDiv.Visible = false;
                        //  PaySPDiv.Visible = false; 
                        // PaySPDiv.Visible = true;
                        hpaid.Visible = true;
                        hpaid1.Visible = false;
                        SetTabsetting("ship", false);
                        SetTabsetting("Pay", false);
                        SetTabsetting("paid", true);
                        ImgBtnEditShipping.Visible = false;
                        string output = "";
                        try
                        {
                            //objErrorHandler.CreatePayLog("before check isnative OrderID" + OrderID);
                            int isnative = objOrderServices.IsNativeCountry_Express(OrderID);
                            //objErrorHandler.CreatePayLog("after check isnative OrderID" + OrderID);
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
                                    {
                                        //objPayPalService.SetPayPalStatus(response, response["cm"].ToString());
                                        objPayPalService.ExpressCheckout_SetPayPalStatus(response, response["cm"].ToString());
                                    }
                                    //string rtn = 
                                    //objErrorHandler.CreatePayLog("setpaypalstatus httpRequestVariables tx not null inner OrderID" + OrderID);
                                    //output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(key) +"\";</script>";
                                    output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(OrderID + "#####" + "Pay" + "#####" + "Paid") + "\";</script>";
                                    paiddiv.InnerHtml = output;
                                    return;
                                }
                                else if (httpRequestVariables()["Token"] != null && httpRequestVariables()["PayerID"] != null)
                                {
                                    objPayPalApiService.DoECStatus(httpRequestVariables());
                                    //objErrorHandler.CreatePayLog("Token and PayerID not null inner OrderID=" + OrderID);
                                    output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(OrderID + "#####" + "PayApi" + "#####" + "Paid") + "\";</script>";
                                    paiddiv.InnerHtml = output;
                                    return;
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


                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn);

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
                                        divOk.Visible = true;
                                        //divOk.InnerHtml = "<img src=\"/images/checkout_tick.png\" class=\"approved_icon\"/>" + "Transaction Approved! Your Order will now be processed, thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                        divOk.InnerHtml = "Transaction Approved! <br/> Your Order will now be processed, Thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                        PayOkDiv.Visible = true;


                                        //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                                        divlink.InnerHtml = "Payment Method: Paypal Guest Checkout";
                                        checkoutrightL1.Visible = false;
                                        checkoutrightL4.Visible = true;
                                        ttOrder.Enabled = false;
                                        ttOrder.ReadOnly = true;
                                        divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
                                        Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                                        divpaypalaccount.Visible = false;
                                        PayPaypalAcc.Style.Add("display", "none");

                                        L4AEditAddress.Visible = false;
                                        L3AEditAddress.Visible = false;
                                        L4AEditShippingMethod.Visible = false;
                                        BtnEditAddress.Visible = false;
                                        BtnL3EditAddress.Visible = false;
                                        BtnL4EditShippingMethod.Visible = false;
                                        ImageButton1.Visible = false;
                                        PayType.Visible = false;
                                        SecurePayAcc.Visible = false;
                                   
                                        divshopcounter.Visible = false;
                                        Session["OrderDetExp_orderid"] = OrderID;
                                        Session["OrderDetExp_userid"] = Userid;
                                        //Level4_Payment.Visible = false;
                                        //Level4_Submit.Visible = fa;
                                        //Session.RemoveAll();
                                        // Session.Clear();
                                        // Session.Abandon();
                                        //Session["USER_ID"] = "0";
                                        //Session["DUMMY_FLAG"] = "0";
                                        Session["ORDER_ID"] = "0";
                                        //ScrollToControl("ctl00_maincontent_PayOkDiv", true);
                                        //Session["USER_ROLE"] = "0";
                                    }
                                    else
                                    {
                                        PayType.Visible = true;
                                        //objErrorHandler.CreatePayLog("Transaction failed payflowresponse! Please try again Orderid=" + OrderID);
                                        divOk.InnerHtml = "";
                                        divOk.Visible = false;
                                        PayOkDiv.Visible = false;
                                        divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";
                                        if (isnative == 1)
                                            divlink.InnerHtml = "<a href=\"expresscheckout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                        else
                                            divlink.InnerHtml = "<a href=\"expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                    }
                                    HttpContext.Current.Session["payflowresponse"] = "";
                                    HttpContext.Current.Session["IPN"] = "";
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

                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, isipn);
                                        //objErrorHandler.CreatePayLog("after UpdatePaymentOrderStatus APIres Orderid=" + OrderID);


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

                                        PayOkDiv.Visible = true;
                                        divError.InnerHtml = "";
                                        divOk.InnerHtml = "<img src=\"/images/checkout_tick.png\" class=\"approved_icon\"/>" + "Transaction Approved! Your Order will now be processed, thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                        //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                                        divlink.InnerHtml = "Payment Method: Paypal Express Checkout";
                                        divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
                                        Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                                        ttOrder.Text = oPayInfo.PORelease;
                                        ttOrder.Enabled = false;
                                        ttOrder.ReadOnly = true;
                                        L4AEditAddress.Visible = false;
                                        L3AEditAddress.Visible = false;
                                        L4AEditShippingMethod.Visible = true;
                                        BtnEditAddress.Visible = false;
                                        BtnL3EditAddress.Visible = false;
                                        BtnL4EditShippingMethod.Visible = false;
                                        PayPaypalAcc.Style.Add("display", "none");
                                        ImageButton1.Visible = false;
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
                                        PayType.Visible = true;
                                        divOk.Visible = false;
                                        PayOkDiv.Visible = true;
                                        divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";
                                        if (isnative == 1)
                                            divlink.InnerHtml = "<a href=\"expresscheckout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                        else
                                            divlink.InnerHtml = "<a href=\"expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PayApi") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";

                                    }
                                    HttpContext.Current.Session["payAPIresponse"] = "";
                                    HttpContext.Current.Session["IPN"] = "";
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

                                        objOrderServices.UpdatePaymentOrderStatus_Express(OrderID, PaymentID, false);

                                        objErrorHandler.CreatePayLog("SP after UpdatePaymentOrderStatus Orderid=" + OrderID);
                                        //if (objOrderServices.IsNativeCountry(OrderID) == 1)
                                        //    SendMail_AfterPayment(OrderID, (int)OrderServices.OrderStatus.Payment_Successful);
                                        //else
                                        //    SendMail_AfterPayment(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);

                                        objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                                        SendMail_AfterPaymentSP(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                        objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                                        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                        tbwtEngine.SendMail_Review(OrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                        divOk.Visible = true;
                                        PayOkDiv.Visible = true;
                                        divError.InnerHtml = "";
                                        divOk.InnerHtml = "Transaction Approved! <br/> Your Order will now be processed, Thanks for shopping at Wagner Online!";// "Transaction approved! Thank you for your order.";
                                        //divlink.InnerHtml="<br/><a href=\"/Home.aspx\" class=\"toplinkatest\" >Home</a>";
                                        divlink.InnerHtml = "Payment Method: Credit Card";
                                        ttOrder.ReadOnly = true;
                                        ttOrder.Enabled = false;
                                        PaySPDiv.Visible = false;
                                        PayType.Visible = false;
                                        checkoutrightL1.Visible = false;
                                        checkoutrightL4.Visible = true;
                                        L4AEditAddress.Visible = false;
                                        L3AEditAddress.Visible = false;
                                        L4AEditShippingMethod.Visible = true;
                                        BtnEditAddress.Visible = false;
                                        BtnL3EditAddress.Visible = false;
                                        BtnL4EditShippingMethod.Visible = false;
                                        ImageButton1.Visible = false;
                                        Session["OrderDetExp_orderid"] = OrderID;
                                        Session["OrderDetExp_userid"] = Userid;
                                        ttOrder.Text = oPayInfo.PORelease;
                                      
                                        //Session.RemoveAll();
                                        //  Session.Clear();
                                        // Session.Abandon();
                                        //Session["USER_ID"] = "999";
                                        //Session["DUMMY_FLAG"] = "0";
                                        Session["ORDER_ID"] = "0";
                                        //objErrorHandler.CreateLog("ctl00_maincontent_PayOkDiv"); 
                                        //divl4payment.Attributes.Remove("active");
                                        divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
                                        Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                                        PayOkDiv.Focus();
                                        //ScrollToControl("ctl00_maincontent_PayOkDiv", false);
                                        //Session["USER_ROLE"] = "0";
                                    }
                                    else
                                    {
                                        divOk.InnerHtml = "";
                                        divOk.Visible = false;
                                        objErrorHandler.CreatePayLog("SP Transaction failed! Please try again Orderid=" + OrderID);
                                        divError.InnerHtml = "Transaction failed! Please try again.<br/><br/>";

                                        PayType.Visible = true;
                                        divOk.InnerHtml = "";
                                        divOk.Visible = false;
                                        PayOkDiv.Visible = true;
                                        divError.InnerHtml = "Transaction failed! Please try again.<br/>";
                                        if (isnative == 1)
                                            divlink.InnerHtml = "<a href=\"expresscheckout.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                                        else
                                            divlink.InnerHtml = "<a href=\"expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PaySP") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";




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
                        paiddiv.Visible = false;
                        spanpaid.Visible = false;
                        hpaid1.Visible = true;
                        hpaid.Visible = false;
                    }
                  
                }
         
            catch (Exception ex)
            { }


          
        }


        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
          
       
        }
        private void ScrollToControl(string controlId,bool Withposition)
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
        private void SendMailRegiExpressCheckout(string LoginId,string Password,string Email,string Fname, string Lname)
        {
            try
            {
               // string url = HttpContext.Current.Request.Url.Authority.ToString();

               // objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
                MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                MessageObj.To.Add(Email.ToString());
               
          
            
                MessageObj.Subject = "Wagner Online Registration Confirmation";
                MessageObj.IsBodyHtml = true;

                StringTemplateGroup _stg_container = null;
                StringTemplate _stmpl_container = null;

                string stemplatepath = Server.MapPath("Templates");
                _stg_container = new StringTemplateGroup("Mail-wagner", stemplatepath);
                _stmpl_container = _stg_container.GetInstanceOf("Mail-wagner" + "\\" + "regiexpresscheckout");

                _stmpl_container.SetAttribute("FirstLastName", Fname.ToString() + " " + Lname.ToString());
                _stmpl_container.SetAttribute("UserName", LoginId);
                //_stmpl_container.SetAttribute("Password", Password);
              
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

                Level3.Visible = false;
                Level4.Visible = true;
               
            }
            if (tab == "paid")
            {
                sp = spanpaid;
                hp = hpaid;
                dp = divpaid;

            }
            if (tab == "ship")
            {
               // sp = spanship;
               // hp = hship;
                dp = divship;
                Level3.Visible = true;
                // ;BtnL3Continue.Focus();
                //BtnL3Continue.Focus();
                ScrollToControl("ctl00_maincontent_divl3continue",true);
                Level4.Visible = false;

            }

            if (isOpen == false)
            {
                //sp.Attributes["class"] = "collapsed";
                dp.Attributes["class"] = "panel-collapse collapse";
                dp.Attributes["style"] = "display:none;";

            }
            else
            {
               // sp.Attributes["class"] = "";
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

                        //if (Userid != 999)
                        //{
                        //    int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;
                        //    id = Convert.ToString(objOrderServices.GetOrderID(Userid, tmpOrdStatus));
                        //}
                        //else
                        //{
                        //    id = DecryptSP(id);
                        //}


                        if ((id == null) && (isNumeric == true))
                        {
                            id = Request.Url.Query.Replace("?", "");
                            // new
                            //if (Userid != 999)
                            //{
                            //    int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;
                            //    id = Convert.ToString(objOrderServices.GetOrderID(Userid, tmpOrdStatus));
                            //}
                            //else
                            //{
                            //    id = Request.Url.Query.Replace("?", "");
                            //}

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
                        if (Userid != 999 || OrderID ==0)
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
        
        private void ShowPopUpMessage()
        {
         
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

                    //lblUserRoleName.Text = UserList.ToString();
                }
                //else
                //    lblUserRoleName.Text = "";
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
                str = str + (string.IsNullOrEmpty(oOrdShippInfo.MiddleName.Trim()) ? "" : " " + oOrdShippInfo.MiddleName.Trim());
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
                //double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
                //if (Dist <= 50 && Dist > 0)
                //{
                //    oLstItem.Text = "Friendly Driver";
                //    oLstItem.Value = "FRIENDLYDRIVER";
                //    oLstItem.Selected = true;
                //   // cmbProvider.Items.Add(oLstItem);
                //}

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
                str = str + (string.IsNullOrEmpty(oOrdBillInfo.MiddleName.Trim()) ? "" : " " + oOrdBillInfo.MiddleName.Trim());
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

        //public void LoadShippingInfo(string sUserID)
        //{
        //    try
        //    {
        //        int _UserID;
        //        _UserID = objHelperServices.CI(sUserID);
        //        oOrdShippInfo = objUserServices.GetUserShipInfo(_UserID);

        //        txtSFName.Text = oOrdShippInfo.FirstName;
        //        txtSLName.Text = oOrdShippInfo.LastName;
        //        txtbillMName.Text = oOrdShippInfo.MiddleName;
        //        txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
        //        txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
        //        txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
        //        txtSCity.Text = oOrdShippInfo.ShipCity;
        //        drpShipState.Text = oOrdShippInfo.ShipState;
        //        txtSZip.Text = oOrdShippInfo.ShipZip;
        //        drpShipCountry.SelectedValue = oOrdShippInfo.ShipCountry;
        //        Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
        //        txtSPhone.Text = oOrdShippInfo.ShipPhone;
        //        double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
        //        if (Dist <= 50 && Dist > 0)
        //        {
        //            oLstItem.Text = "Friendly Driver";
        //            oLstItem.Value = "FRIENDLYDRIVER";
        //            oLstItem.Selected = true;
        //            cmbProvider.Items.Add(oLstItem);
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}

        //public void LoadBillInfo(string sUserID)
        //{
        //    UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
        //    try
        //    {
        //        int _UserID;
        //        _UserID = objHelperServices.CI(sUserID);
        //        oOrdBillInfo = objUserServices.GetUserBillInfo(_UserID);
        //        txtbillFName.Text = oOrdBillInfo.FirstName;
        //        txtbillLName.Text = oOrdBillInfo.LastName;
        //        txtbillMName.Text = oOrdBillInfo.MiddleName;
        //        txtbilladd1.Text = oOrdBillInfo.BillAddress1;
        //        txtbilladd2.Text = oOrdBillInfo.BillAddress2;
        //        txtbilladd3.Text = oOrdBillInfo.BillAddress3;
        //        txtbillcity.Text = oOrdBillInfo.BillCity;
        //        drpBillState.Text = oOrdBillInfo.BillState;
        //        txtbillzip.Text = oOrdBillInfo.BillZip;
        //        drpBillCountry.SelectedValue = oOrdBillInfo.BillCountry;
        //        Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
        //        txtbillphone.Text = oOrdBillInfo.BillPhone;
        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}

        //public void ClearShippingInfo()
        //{
        //    txtSFName.Text = "";
        //    txtSLName.Text = "";
        //    txtSAdd1.Text = "";
        //    txtSAdd2.Text = "";
        //    txtSAdd3.Text = "";
        //    txtSCity.Text = "";
        //    txtSZip.Text = "";
        //    txtSPhone.Text = "";
        //}

        //public void ClearBillingInfo()
        //{
        //    txtbilladd1.Text = "";
        //    txtbilladd2.Text = "";
        //    txtbilladd3.Text = "";
        //    txtbillcity.Text = "";
        //    txtbillFName.Text = "";
        //    txtbillLName.Text = "";
        //    txtbillphone.Text = "";
        //    txtbillzip.Text = "";
        //}

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

        //protected void btnShipProceed_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        QuoteServices objQuoteServices = new QuoteServices();
        //        int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
        //        decimal TaxAmount;
        //        decimal ProdTotCost;
        //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
        //        int QuoteId = 0;
        //        QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));

        //        GetParams();
        //        //  if (Request["OrderId"] != null)
        //        //     OrderID = objHelperServices.CI(Request["OrderId"].ToString());
        //        if (Request["QteId"] != null)
        //        {
        //            QuoteID = objHelperServices.CI(Request["QteId"].ToString());
        //            OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
        //            OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
        //        }

              

        //        ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
        //        TaxAmount = objOrderServices.CalculateTaxAmount_Express(ProdTotCost, OrderID.ToString());
        //        decimal UpdRst = 0;
        //        oOrdInfo.OrderID = OrderID;
        //        // oOrdInfo.OrderStatus = OrdStatus;
        //        oOrdInfo.OrderStatus = OrdStatus;
        //        oOrdInfo.ShipCompany = "";// cmbProvider.SelectedValue;
        //        oOrdInfo.ShipMethod = drpSM1.SelectedValue;

        //        if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
        //        {
        //            //oOrdInfo.ShipCompName = objHelperServices.Prepare(txtCompany.Text);
        //            //oOrdInfo.ShipFName = objHelperServices.Prepare(txtAttentionTo.Text);
        //            //oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtAddressLine1.Text);
        //            //oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtAddressLine2.Text);
        //            //oOrdInfo.ShipCity = objHelperServices.Prepare(txtSuburb.Text);
        //            //oOrdInfo.ShipState = objHelperServices.Prepare(drpState.Text);
        //            //oOrdInfo.ShipCountry = objHelperServices.Prepare(txtCountry.Text);
        //            //oOrdInfo.ShipZip = objHelperServices.Prepare(txtPostCode.Text);
        //            ////   oOrdInfo.ReceiverContact = objHelperServices.Prepare(txtReceiverContactName.Text);
        //            //oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDeliveryInstructions.Text);
        //            //oOrdInfo.ShipPhone = objHelperServices.Prepare(txtShipPhoneNumber.Text);
        //        }
        //        else
        //        {
        //            //oOrdInfo.ShipFName = objHelperServices.Prepare(txtfname.Text);
        //           // oOrdInfo.ShipLName = objHelperServices.Prepare( txtlname.Text);
        //            oOrdInfo.ShipMName = "";// objHelperServices.Prepare(txtbillMName.Text);
        //            oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtsadd.Text);
        //            oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtadd2.Text);
        //            oOrdInfo.ShipAdd3 = "";// objHelperServices.Prepare(txtSAdd3.Text);
        //            oOrdInfo.ShipCity = objHelperServices.Prepare(txttown.Text);
        //            oOrdInfo.ShipState = objHelperServices.Prepare(drpstate1.Text);
        //            oOrdInfo.ShipCountry = objHelperServices.Prepare(drpCountry.Text);
        //            oOrdInfo.ShipZip = objHelperServices.Prepare(txtzip.Text);

        //        }

        //        //oOrdInfo.ShipPhone = objHelperServices.Prepare(txtphone.Text);
        //       // oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);
        //        oOrdInfo.isEmailSent = false;
        //        oOrdInfo.isInvoiceSent = false;
        //        oOrdInfo.IsShipped = false;

        //        //oOrdInfo.BillFName = objHelperServices.Prepare(txtfname.Text);
        //        //oOrdInfo.BillLName = objHelperServices.Prepare(txtlname.Text);
        //        oOrdInfo.BillMName = "";// objHelperServices.Prepare(txtbillMName.Text);
        //        oOrdInfo.BillAdd1 = objHelperServices.Prepare(txtsadd.Text);
        //        oOrdInfo.BillAdd2 = objHelperServices.Prepare(txtadd2.Text);
        //        oOrdInfo.BillAdd3 = "";// objHelperServices.Prepare(txtbilladd3.Text);
        //        oOrdInfo.BillCity = objHelperServices.Prepare(txttown.Text);
        //        oOrdInfo.BillState = objHelperServices.Prepare(drpstate1.Text);
        //        oOrdInfo.BillCountry = objHelperServices.Prepare(drpCountry.Text);
        //        oOrdInfo.BillZip = objHelperServices.Prepare(txtzip.Text);
        //        oOrdInfo.BillPhone = "";// objHelperServices.Prepare(txtphone.Text);
        //        oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
        //        oOrdInfo.ShipCost = CalculateShippingCost(OrderID);
        //        oOrdInfo.TaxAmount = objOrderServices.CalculateTaxAmount_Express(oOrdInfo.ProdTotalPrice + oOrdInfo.ShipCost, OrderID.ToString());
        //        oOrdInfo.TotalAmount = ProdTotCost + oOrdInfo.TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
        //        oOrdInfo.TrackingNo = "";
        //        oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        //        UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
        //        double Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
        //        if (UpdRst > 0)
        //        {
        //            Session["ShipCost"] = oOrdInfo.ShipCost;
        //            if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
        //            {
        //                Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
        //            }
        //            else
        //            {
        //                Response.Redirect("Payment.aspx?OrdId=" + OrderID, false);
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}

     

        //protected void ChkShippingAdd_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ChkShippingAdd.Checked == false)
        //        {
        //            ClearShippingInfo();
        //        }
        //        else
        //        {
        //            LoadShippingInfo(Session["USER_ID"].ToString());
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}

        //protected void ChkbillingAdd_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ChkbillingAdd.Checked == false)
        //        {
        //            ClearBillingInfo();
        //        }
        //        else
        //        {
        //            LoadBillInfo(Session["USER_ID"].ToString());
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}

        //Update User table Shipping Address
        //protected void ChkDefaultShipAdd_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ChkShipDefaultaddr.Checked == true)
        //        {
        //            UserServices objUserServices = new UserServices();
        //            UserServices.UserInfo oOrdShipAddr = new UserServices.UserInfo();
        //            oOrdShipAddr.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        //            oOrdShipAddr.FirstName = objHelperServices.Prepare(txtSFName.Text);
        //            oOrdShipAddr.LastName = objHelperServices.Prepare(txtSLName.Text);
        //            oOrdShippInfo.MiddleName = objHelperServices.Prepare(txtSMName.Text);
        //            oOrdShipAddr.ShipAddress1 = objHelperServices.Prepare(txtSAdd1.Text);
        //            oOrdShipAddr.ShipAddress2 = objHelperServices.Prepare(txtSAdd2.Text);
        //            oOrdShipAddr.ShipAddress3 = objHelperServices.Prepare(txtSAdd3.Text);
        //            oOrdShipAddr.ShipCity = objHelperServices.Prepare(txtSCity.Text);
        //            oOrdShipAddr.ShipState = objHelperServices.Prepare(drpShipState.Text);
        //            oOrdShipAddr.ShipCountry = objHelperServices.Prepare(drpShipCountry.Text);
        //            oOrdShipAddr.ShipZip = objHelperServices.Prepare(txtSZip.Text);
        //            oOrdShipAddr.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
        //            objUserServices.UpdateShippingInfo(oOrdShipAddr);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}
        //Update User table Billing Address
        //protected void ChkDefaultBillAdd_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ChkBillDefaultaddr.Checked == true)
        //        {
        //            UserServices objUserServices = new UserServices();
        //            UserServices.UserInfo oOrdBillAddr = new UserServices.UserInfo();
        //            oOrdBillAddr.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        //            oOrdBillAddr.FirstName = objHelperServices.Prepare(txtbillFName.Text);
        //            oOrdBillAddr.LastName = objHelperServices.Prepare(txtbillLName.Text);
        //            oOrdBillAddr.MiddleName = objHelperServices.Prepare(txtbillMName.Text);
        //            oOrdBillAddr.BillAddress1 = objHelperServices.Prepare(txtbilladd1.Text);
        //            oOrdBillAddr.BillAddress2 = objHelperServices.Prepare(txtbilladd2.Text);
        //            oOrdBillAddr.BillAddress3 = objHelperServices.Prepare(txtbilladd3.Text);
        //            oOrdBillAddr.BillCity = objHelperServices.Prepare(txtbillcity.Text);
        //            oOrdBillAddr.BillState = objHelperServices.Prepare(drpBillState.Text);
        //            oOrdBillAddr.BillCountry = objHelperServices.Prepare(drpBillCountry.Text);
        //            oOrdBillAddr.BillZip = objHelperServices.Prepare(txtbillzip.Text);
        //            oOrdBillAddr.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
        //            objUserServices.UpdateBillingInfo(oOrdBillAddr);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}

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
                drpCountry.Items.Clear();
                drpCountry.DataSource = oDs;
                drpCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
                drpCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
                drpCountry.DataBind();
                drpCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
        }

        public void LoadCountryListBill()
        {
            //drpcountry_bill

            try
            {
                DataSet oDs = new DataSet();

                oDs = objCountryServices.GetCountries();
                drpcountry_bill.Items.Clear();
                drpcountry_bill.DataSource = oDs;
                drpcountry_bill.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
                drpcountry_bill.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
                drpcountry_bill.DataBind();
                drpcountry_bill.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
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
                string shipcost = objHelperServices.GetOptionValues("COURIER CHARGE");
                if (shipcost != "")
                    ShippingValue = Convert.ToDecimal(shipcost);

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
        //protected void txtSZip_TextChanged(object sender, EventArgs e)
        //{
        //    IsZipCodeChange = true;
        //}

        protected void ImageButton5_Click(object sender, EventArgs e)
        {
            try
            {

                if (ttOrder.Text == "")
                {
                    Session["ORDER_NO"] =refid;
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

        protected void BtnL3EditAddress_Click(object sender, EventArgs e)
        {

            Level1.Visible = false;
            Level2.Visible = true;
           BtnL2Continue.Focus();
           ScrollToControl("ctl00_maincontent_l2div",false);
            Level3.Visible = false;
            Level4.Visible = false;
            Session["EditAddress"] = true;
            int UserID = 0;
            UserID = Convert.ToInt32(Session["USER_ID"].ToString());

            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            ouserinfo = objUserServices.GetUserInfo(UserID);

            L2name.Text = ouserinfo.Contact;
            L2Email.Text = ouserinfo.AlternateEmail;
            L2Phone.Text = ouserinfo.Phone;
            L3ship_company_name.Text = ouserinfo.COMPANY_NAME;
            if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null) 
            {
                L3ship_attn.Text = ouserinfo.Contact;
            }
            else
            {
                L3ship_attn.Text = ouserinfo.Receiver_Contact;
            }
            L3Ship_Street.Text = oOrdBillInfo.ShipAddress1;
            L3Ship_Address.Text = oOrdBillInfo.ShipAddress2;
            L3Ship_Suburb.Text = oOrdBillInfo.ShipCity;
            L3Ship_State.Text = oOrdBillInfo.ShipState;
            L3Ship_Zipcode.Text = oOrdBillInfo.ShipZip;
            L3Ship_Country.Text = oOrdBillInfo.ShipCountry;
            int ordID = 0;
            ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
            oOrdInfo = objOrderServices.GetOrder(ordID);
            L3Ship_DELIVERYINST.Text = oOrdInfo.DeliveryInstr;

            if (ouserinfo.ShipCountry.ToLower() == "australia")
            {
                drpstate1.Visible = true;
                // drpstate1.Text = ouserinfo.ShipState;
                txtzip.Visible = true;
                txtzip.Text = ouserinfo.ShipZip;
                txtzip_inter.Visible = false;
                txtstate.Visible = false;
                rfvstate.Enabled = false;
                aucust.Visible = true;
                intercust.Visible = false;
                Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
            }
            else
            {
                txtzip.Visible = false;
                txtzip_inter.Visible = true;
                txtzip_inter.Visible = false;
                drpstate1.Visible = false;
                txtzip_inter.Text = ouserinfo.ShipZip;
                txtstate.Visible = true;
                txtstate.Text = ouserinfo.ShipState;
                rfvddlstate.Enabled = false;
                aucust.Visible = false;
                intercust.Visible = true;
            }
           // ScrollToControl("ctl00_maincontent_l2div");
        }

        protected void BtnL4EditShippingMethod_Click(object sender, EventArgs e)
        {

            Level1.Visible = false;
            Level2.Visible = false;
            Level3.Visible = true;
             //BtnL3Continue.Focus();
            ScrollToControl("ctl00_maincontent_divl3continue",true);
            Level4.Visible = false;
            int UserID = 0;
            UserID = Convert.ToInt32(Session["USER_ID"].ToString());
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            ouserinfo = objUserServices.GetUserInfo(UserID);

            L3Name.Text = ouserinfo.Contact;
            L3Email.Text = ouserinfo.AlternateEmail;
            L3Phone.Text = ouserinfo.Phone;
            L3ship_company_name.Text = ouserinfo.COMPANY_NAME;
            if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
            {
                L3ship_attn.Text = ouserinfo.Contact;
            }
            else
            {
                L3ship_attn.Text = ouserinfo.Receiver_Contact;
            }
            L3Ship_Street.Text = ouserinfo.ShipAddress1;
            L3Ship_Address.Text = ouserinfo.ShipAddress2;
            L3Ship_Suburb.Text = ouserinfo.ShipCity;
            L3Ship_State.Text = ouserinfo.ShipState;
            L3Ship_Zipcode.Text = ouserinfo.ShipZip;
            L3Ship_Country.Text = ouserinfo.ShipCountry;
     //       hfphonenumber.Value = ouserinfo.MobilePhone;
            hfchange.Value = "1";
            lblorderready.Text = hfordernumber.Value;
            if (ouserinfo.MobilePhone != null && ouserinfo.MobilePhone != "" && ouserinfo.MobilePhone.Substring(0, 2) == "04" && ouserinfo.MobilePhone.ToString().Trim().Length == 10)
            {
                hfphonenumber.Value = ouserinfo.MobilePhone;
            }
            else if (ouserinfo.ShipPhone != null && ouserinfo.ShipPhone != "" && ouserinfo.ShipPhone.Substring(0, 2) == "04" && ouserinfo.ShipPhone.ToString().Trim().Length == 10)
            {
                hfphonenumber.Value = ouserinfo.ShipPhone;
            }

            int ordID = 0;
            ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
            oOrdInfo = objOrderServices.GetOrder(ordID);
            L3Ship_DELIVERYINST.Text = oOrdInfo.DeliveryInstr;
            lblorderready.Text = oOrdInfo.ShipPhone;
            hfordernumber.Value = oOrdInfo.ShipPhone;
            txtchangemobilenumber.Text = oOrdInfo.ShipPhone;
            if (hfordernumber.Value == "" && oOrdInfo.ShipMethod == "Counter Pickup")
            {
                txtchangemobilenumber.Text = hfphonenumber.Value;
                cbmobilechange.Checked = true;
                lblorderreadytext.Text = "SMS Order ready notification will NOT be sent.";
            }


            if (drpCountry.SelectedValue == "AU")
            {
                if (Page.IsPostBack)
                {
                    drpSM1.Items.Clear();
                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                    drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                    drpSM1.SelectedIndex = 0;
                }
            }
            else
            {
                drpSM1.Items.Clear();
                drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                drpSM1.SelectedIndex = 1;

            }
        }

        protected void BtnEditAddress_Click(object sender, EventArgs e)
        {
            drpSM1.SelectedIndex = 0;
            int UserID = 0;
            UserID = Convert.ToInt32(Session["USER_ID"].ToString());
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            ouserinfo = objUserServices.GetUserInfo(UserID);

            L2name.Text = ouserinfo.Contact;
            L2Email.Text = ouserinfo.AlternateEmail;
            L2Phone.Text = ouserinfo.Phone;
         
         //   tblbus.Visible = false;
            Level2.Visible = true;
            BtnL2Continue.Focus();
            ScrollToControl("ctl00_maincontent_l2div", false);
            Session["EditAddress"] = true;
            //shipping
            txtComname.Text = ouserinfo.COMPANY_NAME;
            if ( ouserinfo.Receiver_Contact =="" ) 
            {
                txt_attnto.Text = ouserinfo.Contact;
            }

            else
            {
                txt_attnto.Text = ouserinfo.Receiver_Contact;

            }
            if (ChkBillingAdd.Checked == true)
            {
                txtbillbusname.Text = ouserinfo.COMPANY_NAME;
                txtbillname.Text = txt_attnto.Text;
            }
            else
            
            {
                txtbillbusname.Text = ouserinfo.Bill_Company;
                txtbillname.Text = ouserinfo.Bill_Name;
            
            }
           // txtABN.Text = "";
            txtsadd.Text = ouserinfo.ShipAddress1;
            txtadd2.Text = ouserinfo.ShipAddress2;
            txttown.Text = ouserinfo.ShipCity;
          //  txtMobilePhone1.Text = ouserinfo.MobilePhone;
            drpCountry.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.ShipCountry);

            if (ouserinfo.ShipCountry.ToLower() == "australia") 
            {
                drpstate1.Visible = true;
                // drpstate1.Text = ouserinfo.ShipState;
                txtzip.Visible = true;
                txtzip.Text = ouserinfo.ShipZip;
                txtzip_inter.Visible = false;
                txtstate.Visible = false;
                rfvstate.Enabled = false;
                aucust.Visible = true;
                intercust.Visible = false;
                Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
            }
            else
            {
                txtzip.Visible = false;
                txtzip_inter.Visible = true;
               
                drpstate1.Visible = false;
                txtzip_inter.Text = ouserinfo.ShipZip;
                txtstate.Visible = true;
                txtstate.Text = ouserinfo.ShipState;
                rfvddlstate.Enabled = false;
                aucust.Visible = false;
                intercust.Visible = true;
            }

            //Billing
            txtDELIVERYINST.Text = ouserinfo.DELIVERYINST;  
            txtsadd_Bill.Text = ouserinfo.BillAddress1;
            txtadd2_Bill.Text = ouserinfo.BillAddress2;
            txttown_Bill.Text = ouserinfo.BillCity;

            drpcountry_bill.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.BillCountry);
            int ordID = 0;
            if (Session["ORDER_ID"] != null)
            {
                ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
            }
            oOrdInfo = objOrderServices.GetOrder(ordID);
            L3Ship_DELIVERYINST.Text = oOrdInfo.DeliveryInstr;
           
            if (ouserinfo.BillCountry.ToLower() == "australia")
            {
                // drpstate2.Text = ouserinfo.BillState;
                drpstate2.Visible = true;
                txtzip_bill.Text = ouserinfo.BillZip;
                txtstate_Bill.Visible = false;
                txtstate_Bill.Style.Add("display", "none"); 
                RequiredFieldValidator14.Enabled = false;

                Setdrpdownlistvalue(drpstate2, ouserinfo.BillState.ToString());
            }
            else
            {
                txtstate_Bill.Visible = true;
                txtstate_Bill.Style.Add("display", "block");
             
                txtstate_Bill.Text = ouserinfo.BillState;
                txtzip_bill.Text = ouserinfo.BillZip;
                drpstate2.Visible = false;
                RequiredFieldValidator13.Enabled = false;
            }
             
            //drpcountry_bill.Text = ouserinfo.BillCountry; 
            if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower() && ouserinfo.BillZip == ouserinfo.ShipZip && ouserinfo.BillState == ouserinfo.ShipState && ouserinfo.BillAddress1 == ouserinfo.ShipAddress1)
          //  if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower())
            {
                ChkBillingAdd.Checked = true;
                L2DivBilling.Visible = false;
            }
            else
            {

                ChkBillingAdd.Checked = false;
                L2DivBilling.Visible = true;
            }



            if (drpCountry.SelectedValue == "AU")
            {
               
                    drpSM1.Items.Clear();
                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                    drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                    drpSM1.SelectedIndex = 0;
                    intorder.Visible = false;
                
            }
            else
            {
                drpSM1.Items.Clear();
                drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                drpSM1.SelectedIndex = 1;
                intorder.Visible = true;

            }


            Level1.Visible = false;
            Level3.Visible = false;
            Level4.Visible = false;

            letstartregister.Visible = false;
            startdivpwd.Visible = false;
            getstartwelcome.Visible = false;
            BtnGetStartLogin.Visible = false;
            letstartdivlogin.Visible = false;
            startdivpwd.Visible = false;
            getstartwelcome.Visible = true;
            BtnGetStartLogin.Visible = false;
            letstartregister.Visible = false;
            BtnGetStartContinue.Visible = false;


            txtsadd.Enabled = true;
            txtadd2.Enabled = true;
            txttown.Enabled = true;
            txtstate.Enabled = true;
            txtzip_inter.Enabled = true;
            drpCountry.Enabled = true;
            TextBox1.Enabled = true;
            drpSM1.Enabled = true;
            drpstate1.Enabled = true;
            txtzip.Enabled = true;

            TextBox1.ReadOnly = false;
            drpSM1.Enabled = true;
            ScrollToControl("ctl00_maincontent_l2div",true);
           // BtnL3Continue.Focus();
        }



        private void Combian_Prodid(int order_id,string productid)
        {
            string LeaveDuplicateProds = GetLeaveDuplicateProducts();
           
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
                        objOrderServices.RemoveItem(PrdId.ToString(), order_id, objHelperServices.CI(Session["USER_ID"]), order_item_id.ToString());
                    }

                    AddOrderItem(order_id, objHelperServices.CI(Session["USER_ID"]), (int)TotQty, PrdId);

                    if (OrderID > 0)
                        oOrdInfo.OrderID = OrderID;
                    else
                        oOrdInfo.OrderID = order_id;
                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                }

            }

        
        
        
        }

        protected decimal CalculateShippingCost(int OID, int ProdId, decimal ProdApplyPrice, int itemQty)
        {
            _IsShippingFree = false;
            DataSet dsOItem = new DataSet();
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
                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount_Express(oItemInFo.Quantity * untPrice, OrID.ToString());

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
                            Session["tabl3" + OrderID] = lblshipcost.Text;
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
        protected void BtnGetStartContinue_Click(object sender, EventArgs e)
        {
            int i = 1;
            bool tmp = objUserServices.CheckUserRegisterEmail(letstarttxtemail.Text.Trim(), "Retailer");
            if (txtregemail.Text != "" && txtregemail.Text != letstarttxtemail.Text)
            {
                tmp = objUserServices.CheckUserRegisterEmail(txtregemail.Text.Trim(), "Retailer");
                if (tmp == true)
                {
                    letstarttxtemail.Text = txtregemail.Text;
                    letstartdivlogin.Visible = true;

                }
                else
                {
                    letstarttxtemail.Text = txtregemail.Text;
                  i=1;
                }
            }
            if (tmp == false)
            {
             
                //if (txtregemail.Text != "" && txtregemail.Text != letstarttxtemail.Text)
                //{
                //    i = 1;
                   
                //}
                //else
                //{
                //    i = SaveUserProfileInit();
                //}
                if( i > 0)
                {
                    txtregemail.Text = letstarttxtemail.Text;

                    txtregemail.ReadOnly = false;
                    letstartregister.Visible = true;
                    startdivpwd.Visible = false;
                    getstartwelcome.Visible = false;
                    BtnGetStartLogin.Visible = false;
                    letstartdivlogin.Visible = false;

                    Level2.Visible = false;
                    Level3.Visible = false;
                    Level4.Visible = false;
                    Session["ExpressLevel"] = "Start_Reg";
                    ScrollToControl("ctl00_maincontent_l2div", false);
                    BtnL2Continue.Focus();
                    //tblbus.Visible = false;
                }
            }
            else
            {
                startdivpwd.Visible = true;
                getstartwelcome.Visible = true;
                BtnGetStartLogin.Visible = true;
                letstartregister.Visible = false;
                BtnGetStartContinue.Visible = false;
                Session["ExpressLevel"] = "Start";
                Level2.Visible = false;
                Level3.Visible = false;
                Level4.Visible = false;
                startdivpwd.Focus();
            }
        }

        private void SetCookie(string UserName, string Password)
        {
            
                HttpCookie LoginInfoCookie = new HttpCookie("WagnerLoginInfo");
                LoginInfoCookie["UserName"] = objSecurity.StringEnCrypt_password(UserName);
                LoginInfoCookie["Password"] = objSecurity.StringEnCrypt_password(Password);
                LoginInfoCookie["Expires"] = DateTime.Now.AddDays(1).ToString();
                LoginInfoCookie["Login"] = objSecurity.StringEnCrypt_password("True");
                LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.AppendCookie(LoginInfoCookie);

         

        }

        protected void BtnGetStartLogin_Click(object sender, EventArgs e)
        {
            
            bool validUser;
            string username;
            string password;
            DataSet tmpds = null;
            int UserID;
            DataSet tmpdsdealercheck = null;


            username = letstarttxtemail.Text;
            password = letstarttxtpwd.Text;
            password = objSecurity.StringEnCrypt_password(letstarttxtpwd.Text);

            if (objHelperServices.ValidateEmail(username) == true)
            {
                tmpds = objUserServices.CheckMultipleUserMail(username);
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    if (tmpds.Tables[0].Rows.Count > 1)
                    {
                        lblErrMsg.Text = "Email Id is associated with multiple login";
                        return;
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
                    lblErrMsg.Text = "Invalid Login Id";
                    return;
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
                        sessionId = Session.SessionID;
                        int orditemuseridupdate = 0;
                        int newordid = 0;
                        int getnewordid = 0;
                        int order_id = 0;

                        int chkcurordid = 0;

                        //int UserID = objUserServices.GetUserID(txtregemail.Text.Trim());
                        // string Role;
                        // Role = objUserServices.GetRole(UserID);
                        oOrdInfo.UserID = UserID;
                        chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);
                        if (chkcurordid == 0)
                            newordid = objOrderServices.InitilizeOrder(oOrdInfo);
                        getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);

                        orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(OrderID, UserID, sessionId, getnewordid, sessionId);

                        if (getnewordid > 0)
                        {

                            Session["ORDER_ID"] = getnewordid;
                            Session["USER_ID"] = UserID;
                            Session["EXPRESS_CHECKOUT"] = "True";
                            Session["USER_ROLE"] = Role;
                            oOrdInfo.OrderID = getnewordid;
                            OrderID = getnewordid;
                            objOrderServices.UpdateOrderPrice_ExpressCheckout(oOrdInfo, true, sessionId);
                        }
                        else
                        {
                            Session["ORDER_ID"] = chkcurordid;
                            Session["USER_ID"] = UserID;
                            Session["EXPRESS_CHECKOUT"] = "True";
                            Session["USER_ROLE"] = Role;
                            oOrdInfo.OrderID = chkcurordid;
                            OrderID = chkcurordid;
                        }













                        UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                        ouserinfo = objUserServices.GetUserInfo(UserID);

                        Session["USER_NAME"] = ouserinfo.LoginName;

                        Session["USER_ROLE"] = ouserinfo.USERROLE;
                        Session["COMPANY_ID"] = ouserinfo.CompanyID;
                        Session["CUSTOMER_TYPE"] = ouserinfo.CUSTOMER_TYPE;
                        Session["DUMMY_FLAG"] = "1";
                        Session["Emailid"] = ouserinfo.AlternateEmail;
                        Session["Firstname"] = ouserinfo.FirstName;

                        Session["Lastname"] = ouserinfo.LastName;
                        Session["LOGIN_NAME_TOP"] = ouserinfo.Contact;
                       // SetCookie(username, letstarttxtpwd.Text);

                        if (ouserinfo.FirstName == "" || ouserinfo.FirstName == null)
                        {
                            Userid = ouserinfo.UserID;
                            BtnEditLogin_Click(sender, e);
                            
                            return;
                        }
                        int ordID = 0;
                        ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
                        oOrdInfo = objOrderServices.GetOrder(ordID);
                        hfordernumber.Value = oOrdInfo.ShipPhone;

                        if (ouserinfo.MobilePhone != null && ouserinfo.MobilePhone != "" && ouserinfo.MobilePhone.Substring(0, 2) == "04" && ouserinfo.MobilePhone.ToString().Trim().Length == 10)
                        {
                            hfphonenumber.Value = ouserinfo.MobilePhone;
                            lblorderready.Text = ouserinfo.MobilePhone;
                            txtchangemobilenumber.Text = ouserinfo.MobilePhone;
                        }
                        else if (ouserinfo.ShipPhone != null && ouserinfo.ShipPhone != "" && ouserinfo.ShipPhone.Substring(0, 2) == "04" && ouserinfo.ShipPhone.ToString().Trim().Length == 10)
                        {
                            hfphonenumber.Value = ouserinfo.ShipPhone;
                            lblorderready.Text = ouserinfo.ShipPhone;
                            txtchangemobilenumber.Text = ouserinfo.ShipPhone;
                        }
                        else
                        {
                            hfphonenumber.Value = "";
                            lblorderready.Text = "";
                            txtchangemobilenumber.Text = "";
                        }


                        oOrdShippInfo = objUserServices.GetUserShipInfo(UserID);

                        if (oOrdShippInfo.ShipCountry != "" && oOrdShippInfo.ShipAddress1 !="")
                        {

                            Level1.Visible = false;
                            Level3.Visible = true;
                             //BtnL3Continue.Focus();
                            Level4.Visible = false;
                            Level2.Visible = false;

                            L3Name.Text = ouserinfo.Contact;
                            L3Email.Text = ouserinfo.AlternateEmail;
                            L3Phone.Text = ouserinfo.Phone;
                            L3ship_company_name.Text = ouserinfo.COMPANY_NAME;
                            if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                            {
                                L3ship_attn.Text = ouserinfo.Contact;
                            }
                            else
                            {
                                L3ship_attn.Text = ouserinfo.Receiver_Contact;
                            }
                            L3Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                            L3Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                            L3Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                            L3Ship_State.Text = oOrdShippInfo.ShipState;
                            L3Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                            L3Ship_Country.Text = oOrdShippInfo.ShipCountry;
                            L3Ship_DELIVERYINST.Text = oOrdShippInfo.DELIVERYINST;
                            ScrollToControl("ctl00_maincontent_divl3continue",true);
                        }
                        else
                        {
                            Level2.Visible = true;
                            BtnL2Continue.Focus();
                            ScrollToControl("ctl00_maincontent_l2div", false);
                            Level1.Visible = false;
                            Level3.Visible = false;
                            Level4.Visible = false;
                            //shipping
                            L2name.Text = ouserinfo.Contact;
                            L2Email.Text = ouserinfo.AlternateEmail;
                            L2Phone.Text = ouserinfo.Phone;

                            txtComname.Text = ouserinfo.COMPANY_NAME;
                            //if (ouserinfo.ATTN_TO == "" || ouserinfo.ATTN_TO == null)
                            //{
                                txt_attnto.Text = ouserinfo.Contact;
                            //}
                            //else
                            //{

                            //    txt_attnto.Text = ouserinfo.ATTN_TO;
                            //}

                            //txtABN.Text = "";
                            txtsadd.Text = ouserinfo.ShipAddress1;
                            txtadd2.Text = ouserinfo.ShipAddress2;
                            txttown.Text = ouserinfo.ShipCity;
                 //           txtMobilePhone1.Text = ouserinfo.MobilePhone;
                            drpCountry.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.ShipCountry);

                            if (ouserinfo.ShipCountry.ToLower() == "australia")
                            {
                                drpstate1.Visible = true;
                                // drpstate1.Text = ouserinfo.ShipState;
                                txtzip.Visible = true;
                                txtzip.Text = ouserinfo.ShipZip;
                                txtzip_inter.Visible = false;
                                txtstate.Visible = false;
                                rfvstate.Enabled = false;
                                aucust.Visible = true;
                                intercust.Visible = false;
                                Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
                            }
                            else
                            {
                                txtzip.Visible = false;
                                txtzip_inter.Visible = true;
                                drpstate1.Visible = false;
                                txtzip_inter.Text = ouserinfo.ShipZip;
                                txtstate.Visible = true;
                                txtstate.Text = ouserinfo.ShipState;
                                rfvddlstate.Enabled = false;
                                aucust.Visible = false;
                                intercust.Visible = true;
                            }

                            //drpCountry.Text = ouserinfo.ShipCountry;

                            //drpCountry.SelectedValue = ouserinfo.ShipCountry;
                            //Setdrpdownlistvalue(drpShipCountry, oUserinfo.ShipCountry.ToString());



                            //Billing
                            txtsadd_Bill.Text = ouserinfo.BillAddress1;
                            txtadd2_Bill.Text = ouserinfo.BillAddress2;
                            txttown_Bill.Text = ouserinfo.BillCity;

                            drpcountry_bill.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.BillCountry);

                            if (ouserinfo.BillCountry.ToLower() == "australia")
                            {
                                // drpstate2.Text = ouserinfo.BillState;
                                drpstate2.Visible = true;
                                txtzip_bill.Text = ouserinfo.BillZip;
                                txtstate_Bill.Visible = false;
                                txtstate_Bill.Style.Add("display", "none");
                                RequiredFieldValidator14.Enabled = false;

                                Setdrpdownlistvalue(drpstate2, ouserinfo.BillState.ToString());
                            }
                            else
                            {
                                txtstate_Bill.Visible = true;
                                txtstate_Bill.Style.Add("display", "block");
                                txtstate_Bill.Text = ouserinfo.BillState;
                                txtzip_bill.Text = ouserinfo.BillZip;
                                drpstate2.Visible = false;
                                RequiredFieldValidator13.Enabled = false;
                            }

                            //drpcountry_bill.Text = ouserinfo.BillCountry; 

                           // if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower())
                                if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower() && ouserinfo.BillZip == ouserinfo.ShipZip && ouserinfo.BillState == ouserinfo.ShipState && ouserinfo.BillAddress1 == ouserinfo.ShipAddress1)
                            {
                                ChkBillingAdd.Checked = true;
                                L2DivBilling.Visible = false;
                            }
                            else
                            {

                                ChkBillingAdd.Checked = false;
                                L2DivBilling.Visible = true;
                            }



                            if (objOrderServices.IsNativeCountry_Express_userid(UserID) == 1)
                            {
                                if (Page.IsPostBack)
                                {
                                    drpSM1.Items.Clear();
                                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                                    drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                                    drpSM1.SelectedIndex = 0;
                                    intorder.Visible = false;
                                }
                            }
                            else
                            {
                                drpSM1.Items.Clear();
                                drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                                drpSM1.SelectedIndex = 1;
                                intorder.Visible = true;
                            }


                            ScrollToControl("ctl00_maincontent_l2div", false);
                            txtsadd_Bill.Focus();

                        }
                        string LeaveDuplicateProds = "";
                        DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds, sessionId);

                        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
                        {
                            int DupProdCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
                            for (int i = 0; i <= DupProdCount; i++)
                            {
                                Combian_Prodid(OrderID, dsDuplicateItem_Prod_id.Tables[0].Rows[0][0].ToString());

                            }
                        }
                        if (objOrderServices.IsNativeCountry_Express_userid(UserID) == 1)
                        {
                            if (Page.IsPostBack)
                            {
                                drpSM1.Items.Clear();
                                drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                                drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                                drpSM1.SelectedIndex = 0;
                                intorder.Visible = false;
                            }
                        }
                        else
                        {
                            drpSM1.Items.Clear();
                            drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                            drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                            drpSM1.SelectedIndex = 1;
                            intorder.Visible = true;

                        }

                        letstartregister.Visible = false;
                        startdivpwd.Visible = false;
                        getstartwelcome.Visible = false;
                        BtnGetStartLogin.Visible = false;
                        letstartdivlogin.Visible = false;
                        startdivpwd.Visible = false;
                        getstartwelcome.Visible = true;
                        BtnGetStartLogin.Visible = false;
                        letstartregister.Visible = false;
                        BtnGetStartContinue.Visible = false;

                        Session["ExpressLevel"] = "2Compl";
                       
                    }
                }
                else
                {
                    lblErrMsg.Text = "Invalid Password";
                }
            }
            else
            {
                lblErrMsg.Text = "Invalid Login Id";
            }
        }
  
        protected void BtnRegLetStartUpdate_Click(object sender, EventArgs e)
        {
            try
            {
             
                UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                ouserinfo = objUserServices.GetUserInfo(Userid);
                ouserinfo.FirstName=txtRegFname.Text;
                ouserinfo.LastName=txtRegLname.Text;
                Security objcrpengine = new Security();
                string Newpassword = objcrpengine.StringEnCrypt_password(txtRegpassword.Text.ToString());
                ouserinfo.Password = Newpassword;
                ouserinfo.Phone=txtRegphone.Text;
                ouserinfo.MobilePhone = txtRegMobilePhone.Text;
                hfphonenumber.Value = txtRegMobilePhone.Text;

                int i=objUserServices.UpdateUserInfo_loginExp(ouserinfo);
                if (i>=1)
                {
                    ouserinfo = objUserServices.GetUserInfo(Userid);
                            Session["USER_NAME"] = ouserinfo.LoginName;

                            Session["USER_ROLE"] = ouserinfo.USERROLE;
                            Session["COMPANY_ID"] = ouserinfo.CompanyID;
                            Session["CUSTOMER_TYPE"] = ouserinfo.CUSTOMER_TYPE;
                            Session["DUMMY_FLAG"] = "1";
                            Session["Emailid"] = ouserinfo.AlternateEmail;
                            Session["Firstname"] = ouserinfo.FirstName;

                            Session["Lastname"] = ouserinfo.LastName;
                            Session["LOGIN_NAME_TOP"] = ouserinfo.Contact;
                            //SetCookie(ouserinfo.LoginName, txtRegpassword.Text);

                            //Edited by smith
                            int ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
                            //objErrorHandler.CreateLog("shipcountry" + oOrdShippInfo.ShipCountry);
                            oOrdInfo = objOrderServices.GetOrder(ordID);
                            L3Ship_DELIVERYINST.Text = ouserinfo.DELIVERYINST;
                            hfordernumber.Value = oOrdInfo.ShipPhone;
                            if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null && oOrdInfo.ShipPhone.Substring(0, 2) == "04" && oOrdInfo.ShipPhone.ToString().Trim().Length == 10)
                            {
                                lblorderready.Text = oOrdInfo.ShipPhone;
                                txtchangemobilenumber.Text = oOrdInfo.ShipPhone;
                            }
                            else if (ouserinfo.MobilePhone != null && ouserinfo.MobilePhone != "" && ouserinfo.MobilePhone.Substring(0, 2) == "04" && ouserinfo.MobilePhone.ToString().Trim().Length == 10)
                            {
                                txtchangemobilenumber.Text = ouserinfo.MobilePhone;
                                hfphonenumber.Value = ouserinfo.MobilePhone;
                                if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.") {
                                    lblorderready.Text = ouserinfo.MobilePhone;
                                }
                            }
                            else if (ouserinfo.ShipPhone != null && ouserinfo.ShipPhone != "" && ouserinfo.ShipPhone.Substring(0, 2) == "04" && ouserinfo.ShipPhone.ToString().Trim().Length == 10)
                            {
                                txtchangemobilenumber.Text = ouserinfo.ShipPhone;
                                hfphonenumber.Value = ouserinfo.ShipPhone;
                                if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.")
                                {
                                    lblorderready.Text = ouserinfo.ShipPhone;
                                }
                            }
                            else {
                                lblorderready.Text = "";
                                txtchangemobilenumber.Text ="";
                                hfphonenumber.Value = "";
                            }

                            if (oOrdInfo.ShipPhone == "" && oOrdInfo.ShipMethod == "Counter Pickup")
                            {
                                txtchangemobilenumber.Text = hfphonenumber.Value;
                                lblorderreadytext.Text = "SMS Order ready notification will NOT be sent.";
                            }
                            //end of edit
                            

                            //     Session["ExpressLevel"] = "2Compl";


                            //UserIDUpdateOrd = objOrderServices.OrderItemsUpdate_UserId_ExpressCheckout(OrderID, UserID, sessionId);
                      

                        //L2name.Text = txtRegFname.Text.ToString() + " " + txtRegLname.Text.ToString();
                        //L2Email.Text = txtregemail.Text.Trim();
                        //L2Phone.Text = txtRegphone.Text.ToString();
                        //Level2.Visible = true;
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

                        //OuterL2.Visible = false;
                        //OuterL3.Visible = false;
                        //OuterL4.Visible = false;


                            oOrdShippInfo = objUserServices.GetUserShipInfo(Userid);

                            if (oOrdShippInfo.ShipCountry != "" && oOrdShippInfo.ShipAddress1 != "" && oOrdShippInfo.ShipState !="")
                            {

                                Level1.Visible = false;
                                Level3.Visible = true;
                               
                                Level4.Visible = false;
                                Level2.Visible = false;

                                L3Name.Text = ouserinfo.Contact;
                                L3Email.Text = ouserinfo.AlternateEmail;
                                L3Phone.Text = ouserinfo.Phone;
                                L3ship_company_name.Text = ouserinfo.COMPANY_NAME;
                                //if (ouserinfo.ATTN_TO == "" || ouserinfo.ATTN_TO == null)
                                //{
                                    L3ship_attn.Text = ouserinfo.Contact;
                                    txt_attnto.Text = ouserinfo.Contact;

                                //}
                                //else
                                //{
                                //    L3ship_attn.Text = ouserinfo.ATTN_TO;
                                //}
                                L3Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                                L3Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                                L3Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                                L3Ship_State.Text = oOrdShippInfo.ShipState;
                                L3Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                                L3Ship_Country.Text = oOrdShippInfo.ShipCountry;
                                L3Ship_DELIVERYINST.Text = oOrdShippInfo.DELIVERYINST;
                                ScrollToControl("ctl00_maincontent_divl3continue",true);
                            }
                            else
                            {
                                Level2.Visible = true;
                                BtnL2Continue.Focus();
                                ScrollToControl("ctl00_maincontent_l2div", false);
                                Level1.Visible = false;
                                Level3.Visible = false;
                                Level4.Visible = false;
                                //shipping
                                L2name.Text = ouserinfo.Contact;
                                L2Email.Text = ouserinfo.AlternateEmail;
                                L2Phone.Text = ouserinfo.Phone;


                                txtComname.Text = ouserinfo.COMPANY_NAME;
                                //if (ouserinfo.ATTN_TO == "" || ouserinfo.ATTN_TO == null)
                                //{
                                    txt_attnto.Text = ouserinfo.Contact;
                                //}
                                //else
                                //{

                                //    txt_attnto.Text = ouserinfo.ATTN_TO;
                                //}

                                //txtABN.Text = "";
                                txtsadd.Text = ouserinfo.ShipAddress1;
                                txtadd2.Text = ouserinfo.ShipAddress2;
                                txttown.Text = ouserinfo.ShipCity;
                              //  txtMobilePhone1.Text = ouserinfo.MobilePhone;
                                txtDELIVERYINST.Text = ouserinfo.DELIVERYINST;
                                drpCountry.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.ShipCountry);

                                if (ouserinfo.ShipCountry.ToLower() == "australia")
                                {
                                    drpstate1.Visible = true;
                                    // drpstate1.Text = ouserinfo.ShipState;
                                    txtzip.Visible = true;
                                    txtzip.Text = ouserinfo.ShipZip;
                                    txtzip_inter.Visible = false;
                                    txtstate.Visible = false;
                                    rfvstate.Enabled = false;
                                    aucust.Visible = true;
                                    intercust.Visible = false;
                                    Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
                                }
                                else
                                {
                                    txtzip.Visible = false;
                                    txtzip_inter.Visible = true;
                                    drpstate1.Visible = false;
                                    txtzip_inter.Text = ouserinfo.ShipZip;
                                    txtstate.Visible = true;
                                    txtstate.Text = ouserinfo.ShipState;
                                    rfvddlstate.Enabled = false;
                                    aucust.Visible = false;
                                    intercust.Visible = true;
                                }

                                //drpCountry.Text = ouserinfo.ShipCountry;

                                //drpCountry.SelectedValue = ouserinfo.ShipCountry;
                                //Setdrpdownlistvalue(drpShipCountry, oUserinfo.ShipCountry.ToString());



                                //Billing
                                txtsadd_Bill.Text = ouserinfo.BillAddress1;
                                txtadd2_Bill.Text = ouserinfo.BillAddress2;
                                txttown_Bill.Text = ouserinfo.BillCity;

                                drpcountry_bill.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.BillCountry);

                                if (ouserinfo.BillCountry.ToLower() == "australia")
                                {
                                    // drpstate2.Text = ouserinfo.BillState;
                                    drpstate2.Visible = true;
                                    txtzip_bill.Text = ouserinfo.BillZip;
                                    txtstate_Bill.Visible = false;
                                    txtstate_Bill.Style.Add("display", "none");
                                    RequiredFieldValidator14.Enabled = false;

                                    Setdrpdownlistvalue(drpstate2, ouserinfo.BillState.ToString());
                                }
                                else
                                {
                                    txtstate_Bill.Visible = true;
                                    txtstate_Bill.Style.Add("display", "block");
                                    txtstate_Bill.Text = ouserinfo.BillState;
                                    txtzip_bill.Text = ouserinfo.BillZip;
                                    drpstate2.Visible = false;
                                    RequiredFieldValidator13.Enabled = false;
                                }

                                //drpcountry_bill.Text = ouserinfo.BillCountry; 

                                //if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower())
                                    if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower() && ouserinfo.BillZip == ouserinfo.ShipZip && ouserinfo.BillState == ouserinfo.ShipState && ouserinfo.BillAddress1 == ouserinfo.ShipAddress1)
                                {
                                    ChkBillingAdd.Checked = true;
                                    L2DivBilling.Visible = false;
                                }
                                else {

                                    ChkBillingAdd.Checked = false;
                                    L2DivBilling.Visible = true;
                                }



                                if (objOrderServices.IsNativeCountry_Express_userid(Userid) == 1)
                                {
                                    if (Page.IsPostBack)
                                    {
                                        drpSM1.Items.Clear();
                                        drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                        drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                                        drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                                        drpSM1.SelectedIndex = 0;
                                        intorder.Visible = false;
                                    }
                                }
                                else
                                {
                                    drpSM1.Items.Clear();
                                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                    drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                                    drpSM1.SelectedIndex = 1;
                                    intorder.Visible = true;
                                }

                                

                                BtnL2Continue.Focus();

                            }











                    }
               
                else
                {
                    lblerror_l1update.Text = "Update Failed";
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }



        protected void BtnRegLetStart_Click(object sender, EventArgs e)
        {
            try
            {
                String sessionId;
                sessionId = Session.SessionID;
                int orditemuseridupdate = 0;
                int newordid = 0;
                int getnewordid = 0;
                int order_id = 0;

                int chkcurordid = 0;
                bool tmp = false;
              
                //if (txtregemail.Text == letstarttxtemail.Text)
                //{
                    //If user din't change the emaiil id
                    tmp = objUserServices.CheckUserRegisterEmail(txtregemail.Text.Trim(), "Retailer");
                //}
                //else
                //{
                    //If user din't change the emaiil id
            //        tmp = objUserServices.CheckUserRegisterEmail(txtregemail.Text.Trim(), "Retailer");
            //        if (tmp == false)
            //        {
            //            tmp = true;
            //            //If not registered then update the mailid
            //        }
            //        else
            //        { 
            //      //If already registered then ask for password process
            //    startdivpwd.Visible = true;
            //    getstartwelcome.Visible = true;
            //    BtnGetStartLogin.Visible = true;
            //    letstartregister.Visible = false;
            //    BtnGetStartContinue.Visible = false;
            //    Level1.Visible = true;
            //    Level2.Visible = false;
            //    Level3.Visible = false;
            //    Level4.Visible = false;
            //    startdivpwd.Focus();
            //    return ;
            //}
                  
                //}
                    int k = 0;
                    if (tmp == false)
                    {
                        k = SaveUserProfileInit();
                       
                    }
                if (k > 0)
                {
                    int i = SaveUserProfile();
                    if (i > 0)
                    {
                        Session["ExpressLevel"] = "Start";
                     
                        DataSet tmpds = objUserServices.CheckMultipleUserMail(txtregemail.Text.Trim(), "Retailer");
                        if ((tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0))
                        {
                            int UserID = objUserServices.GetUserID(txtregemail.Text.Trim());
                            string Role;
                            Role = objUserServices.GetRole(UserID);
                            oOrdInfo.UserID = UserID;
                            chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);
                            if (chkcurordid == 0)
                                newordid = objOrderServices.InitilizeOrder(oOrdInfo);
                            getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);

                            orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(OrderID, UserID, sessionId, getnewordid, sessionId);
                            //objOrderServices.OrderItemsUpdate_UserId(order_id, Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), sessionId, getnewordid, sessionId);
                            if (getnewordid > 0)
                            {

                                Session["ORDER_ID"] = getnewordid;
                                Session["USER_ID"] = UserID;
                                Session["EXPRESS_CHECKOUT"] = "True";
                                Session["USER_ROLE"] = Role;
                                oOrdInfo.OrderID = getnewordid;
                                objOrderServices.UpdateOrderPrice_ExpressCheckout(oOrdInfo, true, sessionId);
                            }
                            else
                            {
                                Session["ORDER_ID"] = chkcurordid;
                                Session["USER_ID"] = UserID;
                                Session["EXPRESS_CHECKOUT"] = "True";
                                Session["USER_ROLE"] = Role;
                                oOrdInfo.OrderID = chkcurordid;
                                OrderID = chkcurordid;
                            }
                            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                            ouserinfo = objUserServices.GetUserInfo(UserID);

                            Session["USER_NAME"] = ouserinfo.LoginName;

                            Session["USER_ROLE"] = ouserinfo.USERROLE;
                            Session["COMPANY_ID"] = ouserinfo.CompanyID;
                            Session["CUSTOMER_TYPE"] = ouserinfo.CUSTOMER_TYPE;
                            Session["DUMMY_FLAG"] = "1";
                            Session["Emailid"] = ouserinfo.AlternateEmail;
                            Session["Firstname"] = ouserinfo.FirstName;

                            Session["Lastname"] = ouserinfo.LastName;
                            //SetCookie(ouserinfo.LoginName, txtRegpassword.Text);

                         
                                                
                       //     Session["ExpressLevel"] = "2Compl";


                            //UserIDUpdateOrd = objOrderServices.OrderItemsUpdate_UserId_ExpressCheckout(OrderID, UserID, sessionId);
                        }
                        try
                        {

                            Security objcrpengine = new Security();
                            string Newpassword = objcrpengine.StringEnCrypt_password(txtRegpassword.Text.ToString());
                            SendMailRegiExpressCheckout(txtregemail.Text.Trim(), Newpassword, txtregemail.Text.Trim(), txtRegFname.Text.ToString(), txtRegLname.Text.ToString());
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.ErrorMsg = ex;
                            objErrorHandler.CreateLog();
                        }
                       

                        L2name.Text = txtRegFname.Text.ToString() + " " + txtRegLname.Text.ToString();
                        L2Email.Text = txtregemail.Text.Trim();
                        L2Phone.Text = txtRegphone.Text.ToString();
                        txt_attnto.Text = L2name.Text;
                        Level2.Visible = true;
                        BtnL2Continue.Focus();
                        ScrollToControl("ctl00_maincontent_l2div", false);
                        L2DivBilling.Visible = false;
                        Session["EditAddress"] = true;
                        Level1.Visible = false;
                        Level3.Visible = false;
                        Level4.Visible = false;

                        letstartregister.Visible = false;
                        startdivpwd.Visible = false;
                        getstartwelcome.Visible = false;
                        BtnGetStartLogin.Visible = false;
                        letstartdivlogin.Visible = false;
                        startdivpwd.Visible = false;
                        getstartwelcome.Visible = true;
                        BtnGetStartLogin.Visible = false;
                        letstartregister.Visible = false;
                        BtnGetStartContinue.Visible = false;
                        //txtbillbusname.Focus();
                        BtnL2Continue.Focus();
                        ScrollToControl("ctl00_maincontent_l2div", false);
                        //OuterL2.Visible = false;
                        //OuterL3.Visible = false;
                        //OuterL4.Visible = false;

                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    private void goto_level2(){
                     //Level2.Visible = true;
                     //   BtnL2Continue.Focus();
                     //   ScrollToControl("ctl00_maincontent_l2div", false);
                     //   L2DivBilling.Visible = false;
                        Session["EditAddress"] = true;
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
                        ////txtbillbusname.Focus();
                        //BtnL2Continue.Focus();
                        //ScrollToControl("ctl00_maincontent_l2div", false);
}
      

protected void BtnL2Continue_Click(object sender, EventArgs e)
        {

            int i = 0;
            i = UpdateUserData();
           // objErrorHandler.CreatePayLog("btnl2" + i.ToString());
            if (i > 0)
            {
                

                Level1.Visible = false;
                Level2.Visible = false;
                Level3.Visible = true;
                
                Level4.Visible = false;
                Session["EditAddress"] = null;
                UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                ouserinfo = objUserServices.GetUserInfo(Userid);

                L3Name.Text = ouserinfo.Contact;
                L3Email.Text = ouserinfo.AlternateEmail;
                L3Phone.Text = ouserinfo.Phone;
                L3ship_company_name.Text = ouserinfo.COMPANY_NAME;

                if (ouserinfo.DELIVERYINST == "")
                {
                    L3Ship_DELIVERYINST.Text = ouserinfo.DELIVERYINST;

                }
                else
                {

                    L3Ship_DELIVERYINST.Text = txtDELIVERYINST.Text;
                }
                if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                {
                    L3ship_attn.Text = ouserinfo.Contact;
                }
                else
                {
                    L3ship_attn.Text = ouserinfo.Receiver_Contact;
                }
                //L3Ship_Street.Text = oOrdBillInfo.ShipAddress1;
                //L3Ship_Address.Text = oOrdBillInfo.ShipAddress2;
                //L3Ship_Suburb.Text = oOrdBillInfo.ShipCity;
                //L3Ship_State.Text = oOrdBillInfo.ShipState;
                //L3Ship_Zipcode.Text = oOrdBillInfo.ShipZip;
                //L3Ship_Country.Text = oOrdBillInfo.ShipCountry;
                oOrdShippInfo = objUserServices.GetUserShipInfo(Userid);
                L3Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                L3Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                L3Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                L3Ship_State.Text = oOrdShippInfo.ShipState;
                L3Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                L3Ship_Country.Text = oOrdShippInfo.ShipCountry;
                L3Ship_DELIVERYINST.Text = oOrdShippInfo.DELIVERYINST;

                int ordID = 0;
                ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
                oOrdInfo = objOrderServices.GetOrder(ordID);
                hfordernumber.Value = oOrdInfo.ShipPhone;

                if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null && oOrdInfo.ShipPhone.Substring(0, 2) == "04" && oOrdInfo.ShipPhone.ToString().Trim().Length == 10)
                {
                    lblorderready.Text = oOrdInfo.ShipPhone;
                    txtchangemobilenumber.Text = oOrdInfo.ShipPhone;
                }
                else if (ouserinfo.MobilePhone != null && ouserinfo.MobilePhone != "" && ouserinfo.MobilePhone.Substring(0, 2) == "04" && ouserinfo.MobilePhone.ToString().Trim().Length == 10 )
                {
                    hfphonenumber.Value = ouserinfo.MobilePhone;
                    txtchangemobilenumber.Text = ouserinfo.MobilePhone;
                    if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.") {
                        lblorderready.Text = ouserinfo.MobilePhone;
                    }
                }
                else if (ouserinfo.ShipPhone != null && ouserinfo.ShipPhone != "" && ouserinfo.ShipPhone.Substring(0, 2) == "04" && ouserinfo.ShipPhone.ToString().Trim().Length == 10 )
                {
                    hfphonenumber.Value = ouserinfo.ShipPhone;
                    txtchangemobilenumber.Text = ouserinfo.ShipPhone;
                    if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.")
                    {
                        lblorderready.Text = ouserinfo.ShipPhone;
                    }
                }
                else
                {
                    hfphonenumber.Value ="";
                    lblorderready.Text = "";
                    txtchangemobilenumber.Text = "";
                }
                if (oOrdInfo.ShipPhone == "" && oOrdInfo.ShipMethod == "Counter Pickup")
                {
                    txtchangemobilenumber.Text = hfphonenumber.Value;
                    lblorderreadytext.Text = "SMS Order ready notification will NOT be sent.";
                }

                Session["ExpressLevel"] = "2Compl";
                // objErrorHandler.CreatePayLog("btnl2" + oOrdShippInfo.ShipCountry);
                if (oOrdShippInfo.ShipCountry.ToUpper() == "AUSTRALIA")
                {
                
 
                    drpSM1.Items.Clear();
                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                    drpSM1.Items.Add(new  ListItem("Shop Counter Pickup", "Counter Pickup"));
                    drpSM1.SelectedIndex = 0;
                    intorder.Visible = false;
               
            }
            else
            {
                drpSM1.Items.Clear();
                drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                drpSM1.SelectedIndex = 1;
                intorder.Visible = true;

            }
               
        }
            ScrollToControl("ctl00_maincontent_divl3continue",true);

            

        }
        public DataSet GetOrderItemDetailSum(int OrderID)
        {
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

        protected void BtnNothanks_Click(object sender, EventArgs e) {
            txtMobileNumber.Text = "";
            hfordernumber.Value = "";
            hfchange.Value = "1";
            BtnL3Continue_Click(sender, e);
        }

        protected void btnNoThanksChange_Click(object sender, EventArgs e)
        {
            txtchangemobilenumber.Text = hfphonenumber.Value;
            cbmobilechange.Checked = true;
            lblorderreadytext.Text = "SMS Order ready notification will NOT be sent.";
            lblorderready.Text = "";
            hfordernumber.Value = "";
            hfchange.Value = "1";
            txtMobileNumber.Text = "";
            lblShippingMethod.Text = drpSM1.SelectedValue;
            Level1.Visible = false;
            Level2.Visible = false;
            Level3.Visible = true;
            Level4.Visible = false;
            ScrollToControl("ctl00_MainContent_divl3continue", true);
            Session["ExpressLevel"] = "2Compl";
        }

        protected void MobileNoChange_Click(object sender, EventArgs e)
        {
            hfchange.Value = "1";
            if (cbmobilechange.Checked == true)
            {
                int _UserrID;
                _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdInfo.ShipPhone = txtchangemobilenumber.Text;
                lblorderreadytext.Text = "SMS Order ready notification message will be sent to:";
                decimal Updpr = objOrderServices.Update_MOBILE_NUMBER(txtchangemobilenumber.Text, _UserrID, OrderID, true);
                if (Updpr > 0)
                {
                    lblorderready.Text = txtchangemobilenumber.Text;
                    hfphonenumber.Value = txtchangemobilenumber.Text;
                    hfordernumber.Value = txtchangemobilenumber.Text;
                    txtMobileNumber.Text = txtchangemobilenumber.Text;
          //          txtchangemobilenumber.Text="";
                }

            }
            else
            {

                txtMobileNumber.Text = txtchangemobilenumber.Text;
                oOrdInfo.ShipPhone = txtchangemobilenumber.Text;
                lblorderreadytext.Text = "SMS Order ready notification message will be sent to:";
                int UP = objOrderServices.Update_SHIP_NUMBER(txtchangemobilenumber.Text, OrderID);
                if (UP > 0)
                {
                    lblorderready.Text = txtchangemobilenumber.Text;
                    hfordernumber.Value = txtchangemobilenumber.Text;
          //          txtchangemobilenumber.Text = "";
                }
            }
            lblShippingMethod.Text = drpSM1.SelectedValue;
            Level1.Visible = false;
            Level2.Visible = false;
            Level3.Visible = true;
            Level4.Visible = false;
            ScrollToControl("ctl00_MainContent_divl3continue", true);
            Session["ExpressLevel"] = "2Compl";
        }

        protected void BtnL3Continue_Click(object sender, EventArgs e)
        {

           
            if (drpSM1.SelectedValue.ToString().Trim() != "Please Select Shipping Method ")
            {




                try
                {
                    rfvdrpSM1.Visible = false;
                    QuoteServices objQuoteServices = new QuoteServices();
                    OrderDB objOrderDB = new OrderDB();
                    HelperServices objHelperService = new HelperServices();
                    int OrdStatus = 0;
                    string ApproveOrder = string.Empty;
                    string clientIPAddress = "";

                    if (OrderID > 0)
                    {
                        if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
                            clientIPAddress = Session["IP_ADDR"].ToString();
                        else
                            clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                        objOrderServices.InitilizeOrder_ipaddress(OrderID, clientIPAddress);

                    }
                    if (drpCountry.SelectedItem.ToString().ToLower() == "")
                        return;


                    if (Request.QueryString["ApproveOrder"] == null)
                    {
                        if (Session["USER_ROLE"] != null)
                        {
                            switch (Convert.ToInt16(Session["USER_ROLE"]))
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
                    else if (Request.QueryString["ApproveOrder"] != null && (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2))
                    {
                        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                    }
                    else if (Request.QueryString["ApproveOrder"] != null)
                        OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;

                    decimal TaxAmount;
                    decimal ProdTotCost;
                    GetParams();

                    if (OrderID <= 0)
                        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);

                    int oldorderID = OrderID;
                    objErrorHandler.CreateOrderSummarylog("Inside  BtnL3Continue_Click" + "OrderID :" + OrderID + "UserID :" + Session["USER_ID"] );
                    //int QuoteId = 0;
                    //QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                    //if (ttOrder.Text == "")
                    //    ttOrder.Text = "WAG" + OrderID.ToString();

                    //refid = objHelperServices.CS(ttOrder.Text);


                    //if (OrderID <= 0 || ttOrder.Text.Length < 1)
                    //{
                    //    txterr.Text = "Please enter valid Order No, Order No should be more than 1 digits";
                    //    OrderID = -1;
                    //    Session["OrderId"] = "-1";
                    //}
                    //else
                    //{
                    //    txterr.Text = "";
                    //    string querystr = "";

                    //    if (Request.QueryString["ApproveOrder"] == null)
                    //    {
                    //        DataSet DS = new DataSet();
                    //        DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, OrderID.ToString(), "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);
                    //        if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
                    //        {
                    //            txterr.Text = "Order No already exists, please Re-enter Order No";
                    //            OrderID = -1;
                    //        }
                    //    }

                    //}
                    //if (Request["QteId"] != null)
                    //{
                    //    QuoteID = objHelperServices.CI(Request["QteId"].ToString());
                    //    OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
                    //    OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
                    //}

                    if (OrderID > 0)
                    {
                        int _UserrID;
                        _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
                        oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                        oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                        decimal Updpro = 0;
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                        {
                            if (ttinter_order.Text != "")
                            {
                                refid = objHelperServices.CS(ttinter_order.Text);
                                try
                                {
                                     string sSQL = string.Format("EXEC STP_TBWC_CHECK_PO_NUMBER_EXIST '" + HttpContext.Current.Session["ORDER_ID"].ToString() + "','" + HttpContext.Current.Session["USER_ID"].ToString() + "','" + refid + "'");
                                     objErrorHandler.CreateLog("l3continue" + " " + sSQL); 
                    DataTable   DS = objHelperDB.GetDataTableDB(sSQL);
                
                   
                            if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                            {
                                            lblerror_ttinter.Text = "Order No already exists, please Re-enter Order No";
                                            ttinter_order.Focus();
                                            return;
                                        }
                                    
                                }
                                catch (Exception ex)
                                {
                                    objErrorHandler.CreateLog(ex.ToString());
                                }
                            }
                        
                        }

                        objErrorHandler.CreateOrderSummarylog("txtMobileNumbervalue " + txtMobileNumber.Text + "hfchangevalue " + hfchange.Value + "Order ship_phone " + oOrdInfo.ShipPhone);

                        //Update User Mobile_Phone
                        if (chksavemobile.Checked == true)
                        {
                            if (txtMobileNumber.Text != "" && txtMobileNumber.Text != null && hfchange.Value == "0" && drpSM1.SelectedValue=="Counter Pickup")
                            {

                                Updpro = objOrderServices.Update_MOBILE_NUMBER(txtMobileNumber.Text, _UserrID, OrderID, false);
                                if (Updpro > 0)
                                {
                                    oOrdInfo.ShipPhone = txtMobileNumber.Text;
                                    hfphonenumber.Value = txtMobileNumber.Text;
                                    lblorderready.Text = txtMobileNumber.Text;
                                }
                            }
                        }

                        if (drpSM1.SelectedValue.ToString().Trim() == "Counter Pickup" )
                        {
                            //if (txtMobileNumber.Text!= "" && hfchange.Value == "0")
                            //{

                            //    oOrdInfo.ShipPhone = txtMobileNumber.Text;
                            //}
                            //else
                            //{
                            //    oOrdInfo.ShipPhone = "";
                            //}
                            OrderServices.OrderInfo orderInfo = new OrderServices.OrderInfo();
                            orderInfo = objOrderServices.GetOrder(OrderID);
                            objErrorHandler.CreateOrderSummarylog("text is " + orderInfo.ShipPhone);

                            if (txtMobileNumber.Text != "" && txtMobileNumber.Text != null && hfchange.Value == "0")
                            {
                                oOrdInfo.ShipPhone = txtMobileNumber.Text;
                                hfordernumber.Value = txtMobileNumber.Text;
                            }
                            else if (orderInfo.ShipPhone != "" && orderInfo.ShipPhone != null && orderInfo.ShipPhone.Substring(0, 2) == "04" && orderInfo.ShipPhone.ToString().Trim().Length == 10 && hfchange.Value == "0")
                            {
                                oOrdInfo.ShipPhone = orderInfo.ShipPhone;
                            }
                            else if (txtMobileNumber.Text == "" && hfchange.Value == "0")
                            {
                                oOrdInfo.ShipPhone = hfphonenumber.Value;
                                hfordernumber.Value = hfphonenumber.Value;
                            }
                            else
                            {
                                oOrdInfo.ShipPhone = hfordernumber.Value;
                            }
                        }
                        objErrorHandler.CreateOrderSummarylog("Order ship phone" + oOrdInfo.ShipPhone);
                        
                        
                        //End of edit


                        //  if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                            OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                        else
                            OrdStatus = (int)OrderServices.OrderStatus.OPEN;

                        ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
                        TaxAmount = objOrderServices.CalculateTaxAmount_Express(ProdTotCost, OrderID.ToString());
                        decimal UpdRst = 0;
                        oOrdInfo.OrderID = OrderID;
                        oOrdInfo.OrderStatus = OrdStatus;
                       // oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);

                        //double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
                        //if (Dist <= 50 && Dist > 0)
                        //{
                        //    oLstItem.Text = "Friendly Driver";
                        //    oLstItem.Value = "FRIENDLYDRIVER";
                        //    oLstItem.Selected = true;

                        //}


                        //oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);

                        //oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);



                        oOrdInfo.OrderStatus = OrdStatus;
                        oOrdInfo.ShipMethod = drpSM1.SelectedValue;

                        //if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
                        //{
                        //}
                        //else
                        //{
                            //oOrdInfo.ShipFName = oOrdShippInfo.FirstName;
                            //oOrdInfo.ShipLName = oOrdShippInfo.LastName;
                        if (oOrdShippInfo.Receiver_Contact == null || oOrdShippInfo.Receiver_Contact == "")
                        {

                            oOrdInfo.ShipFName = oOrdShippInfo.FirstName +" "+ oOrdShippInfo.LastName;
                        }
                        else {

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
      //                      oOrdInfo.ShipPhone = oOrdShippInfo.ShipPhone;// objHelperServices.Prepare(txtphone.Text);       //doubt
                           // oOrdInfo.ShipCompany = oOrdShippInfo.COMPANY_NAME;
                            oOrdInfo.ShipCompName = oOrdShippInfo.COMPANY_NAME;
                          

                            if (oOrdShippInfo.Receiver_Contact == null || oOrdShippInfo.Receiver_Contact == "")
                            {

                                oOrdInfo.Receiver_Contact = oOrdShippInfo.FirstName +" "+ oOrdShippInfo.LastName;
                            }
                            else

                            {
                                oOrdInfo.Receiver_Contact = oOrdShippInfo.Receiver_Contact;
                            }
                            //DataSet objds = new DataSet();
                            //objds = (DataSet)objOrderDB.GetGenericDataDB(objHelperServices.CI(Session["USER_ID"].ToString()).ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
                            //if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                            //{
                            //    oOrdInfo.DeliveryInstr = objHelperService.CS(objds.Tables[0].Rows[0]["DELIVERY_INST"].ToString());
                            //}

                        //}


                        oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);



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
                        //if (oOrdInfo.ProdTotalPrice == 0)
                        //{ 
                          DataSet tmpds = GetOrderItemDetailSum(oOrdInfo.OrderID);
                          if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                          {
                              oOrdInfo.ProdTotalPrice = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                          }
                          if (oOrdInfo.ProdTotalPrice == 0)
                          {
                              objErrorHandler.CreateLog("btnSecurepay start Orderid=" + OrderID + "ProdTotalPrice" + "0");
                              Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                          }

                        //}
                        oOrdInfo.ShipCost = CalculateShippingCost(OrderID);
                        if ((oOrdInfo.ShipCost > 0) || (drpSM1.SelectedValue == "Standard Shipping"))
                        {
                            oOrdInfo.SHIP_CODE = "DELWG";

                        }
                        else
                        {

                            oOrdInfo.SHIP_CODE = "";
                        }
                        lblshipcost.Text = oOrdInfo.ShipCost.ToString();
                        oOrdInfo.TaxAmount = objOrderServices.CalculateTaxAmount_Express(oOrdInfo.ProdTotalPrice + oOrdInfo.ShipCost, OrderID.ToString());
                       // oOrdInfo.TotalAmount = ProdTotCost + oOrdInfo.TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
                        oOrdInfo.TotalAmount = oOrdInfo.ProdTotalPrice + oOrdInfo.TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
                      
                        
                        oOrdInfo.TrackingNo = "";

                        if (txtDELIVERYINST.Text != "")
                        {

                            oOrdInfo.DeliveryInstr =  objHelperServices.Prepare(txtDELIVERYINST.Text);
                        }
                        else if (L3Ship_DELIVERYINST.Text != "")
                        {
                            oOrdInfo.DeliveryInstr =  objHelperServices.Prepare(L3Ship_DELIVERYINST.Text);
                        }
                        else
                        
                        {
                            oOrdInfo.DeliveryInstr =  objHelperServices.Prepare(L3Ship_DELIVERYINST.Text);
                        
                        }


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

                       // Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
                        if (UpdRst > 0)
                        {
                            objOrderServices.UpdateCustomFields(oOrdInfo);


                            Level1.Visible = false;
                            Level2.Visible = false;
                            Level3.Visible = false;
                            Level4.Visible = true;
                            Level4_Submit.Visible = false;

                            divOk.Visible = false;

                            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                            ouserinfo = objUserServices.GetUserInfo(Userid);

                            L4name.Text = ouserinfo.Contact;
                            L4Email.Text = ouserinfo.AlternateEmail;
                            L4Phone.Text = ouserinfo.Phone;

                            // OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            // oOrderInfo = objOrderServices.GetOrder(OrderID);
                            // oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                            L4Ship_Company.Text = ouserinfo.COMPANY_NAME;

                            if (ouserinfo.Receiver_Contact == "" || ouserinfo.Receiver_Contact == null)
                            {
                                L4Ship_Attnto.Text = ouserinfo.Contact;
                            }
                            else
                            {
                                L4Ship_Attnto.Text = ouserinfo.Receiver_Contact;
                            }
                            L4Ship_Street.Text = oOrdShippInfo.ShipAddress1;
                            L4Ship_Address.Text = oOrdShippInfo.ShipAddress2;
                            L4Ship_Suburb.Text = oOrdShippInfo.ShipCity;
                            L4Ship_State.Text = oOrdShippInfo.ShipState;
                            L4Ship_Zipcode.Text = oOrdShippInfo.ShipZip;
                            L4Ship_Country.Text = oOrdShippInfo.ShipCountry;
                            L4Ship_DELIVERYINST.Text = oOrdInfo.DeliveryInstr;

                            oOrdInfo = objOrderServices.GetOrder(OrderID);
                            lblShippingMethod.Text = oOrdInfo.ShipMethod.Replace("Counter Pickup", "Shop Counter Pickup");
                            L4Comments.Text = oOrdInfo.ShipNotes;

                            if (drpSM1.SelectedValue == "Standard Shipping")
                            {
                                divshopcounter.Visible = false;
                            }
                            else
                            {
                                divshopcounter.Visible = true;
                            }


                            Session["ORDER_NO"] = null;
                            Session["SHIPPING"] = null;
                            Session["DELIVERY"] = null;
                            Session["DROPSHIP"] = null;

                            Session["ShipCost"] = oOrdInfo.ShipCost;
                            //if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                            //{
                            //    Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
                            //}
                            //else
                            //{
                            objErrorHandler.CreateOrderSummarylog("Before ProceedFunction" + "OrderID :" + OrderID + "UserID :" + Session["USER_ID"] + "ProdTotalPrice :" + oOrdInfo.ProdTotalPrice);
                                ProceedFunction();
                                //PnlOrderInvoice.Visible = true;
                                //PnlOrderContents.Visible = false;

                                //if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                //{
                                //    Level4_Submit.Visible = true;
                                //}



                                PHOrderConfirm.Visible = true;
                                //Level4_Payment.Visible = false;
                                //Level4_Submit.Visible = true;
                                //ttOrder.Enabled = false;
                                //drpSM1.Enabled = false;

                                // new code pk
                                // txtfname.Enabled = false;
                                // txtlname.Enabled = false;
                                // txtphone.Enabled = false;
                                // txtemail.Enabled = false;
                                txtsadd.Enabled = false;
                                txtadd2.Enabled = false;
                                txttown.Enabled = false;
                                txtstate.Enabled = false;
                                txtzip_inter.Enabled = false;
                                drpCountry.Enabled = false;

                                //ImageButton1.Visible = false;

                                //TextBox1.ReadOnly = true;
                                // ImageButton2.Visible = false;

                               // Level4.Focus();
                                Session["ExpressLevel"] = "3Compl";

                                if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                                {
                                    Level4_Payment.Visible = true;
                                    Level4_Submit.Visible = false;
                                    ImageButton1.Visible = false;
                                    //PayPaypalAcc.Visible = false;
                                    PayType.Visible = true;

                                    //  SecurePayService objSecurePayService = new SecurePayService();
                                    //  if (objSecurePayService.CheckSecurePay() == true)
                                    ////      Response.Redirect("expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PaySP"), false);
                                    //   else
                                    Response.Redirect("expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay"), false);

                                }
                                else
                                {
                                    //if (ttOrder.Text=="")
                                    //{
                                    //    ttOrder.Text = refid;
                                   
                                    //}

                                   
                                   
                                    if (ttinter_order.Text == "")
                                    {
                                        ttinter_order.Text = refid;
                                        lblintorderid.Text = refid;
                                        txtintporelease_No.Text = refid;
                                    }
                                    else
                                    {
                                        lblintorderid.Text = ttinter_order.Text;
                                        txtintporelease_No.Text = ttinter_order.Text;
                                    }
                                   
                                    Session["OrderDetExp_orderid"] = OrderID;
                                    Session["OrderDetExp_userid"] = Userid;
                                    ImageButton1.Visible = false;
                                    divshopcounter.Visible = false;
                                    Level4_Payment.Visible = false;
                                    Level4_Submit.Visible = true;
                                    PHOrderConfirm.Visible = true;
                                    
                                    checkoutrightL1.Visible = false;
                                    checkoutrightL4.Visible = true;


                                    L4AEditAddress.Visible = false;
                                    BtnEditAddress.Visible = false;

                                    L3AEditAddress.Visible = false;
                                    BtnL3EditAddress.Visible = false;
                                  
                                    L4AEditShippingMethod.Visible = false;
                                    BtnL4EditShippingMethod.Visible = false;

                                    L4AEditAddress.Visible = false;
                                    btneditlogin4.Visible = false;
                                    //PayPaypalAcc.Visible = false;
                              PayType.Visible = true;
                                    SecurePayAcc.Visible = false;
                                    Session["ORDER_ID"] = "0";
                                   
                               
                                    checkoutrightL1.Visible = false;
                                    checkoutrightL4.Visible = true;
                                 
                                 
                                
                                       divl4payment.Attributes["class"] = divl4payment.Attributes["class"].Replace("headingwrap active clearfix mt20 mb20", "headingwrap visited clearfix").Trim();
                                        Level4_Payment.Attributes["class"] = Level4_Payment.Attributes["class"].Replace("checkoutleft", "col-sm-19 pv15 br_dark m10").Trim();
                                    }
                                   
                            }
                        }
                    //}

                }
                catch (Exception Ex)
                {
                    objErrorHandler.ErrorMsg = Ex;
                    objErrorHandler.CreateLog();
                }

            }
            else
            {
                rfvdrpSM1.Visible = true;
            }
            
            
        }
        //protected void ttOrder_TextChanged(object sender, EventArgs e)
        //{
        //    //if (ttOrder.Text == "")
        //    //    ttOrder.Text = "WAG" + OrderID.ToString();

        //    refid = objHelperServices.CS("WAG" + OrderID.ToString());


        //    if (OrderID <= 0 || ttOrder.Text.Length < 1)
        //    {
        //        //txterr.Text = "Please enter valid Order No, Order No should be more than 1 digits";
        //        //ttOrder.Text = "";
        //        //OrderID = -1;
        //        //Session["OrderId"] = "-1";
        //    }
        //    else
        //    {
        //        txterr.Text = "";
        //        string querystr = "";

        //        if (Request.QueryString["ApproveOrder"] == null)
        //        {
        //            DataSet DS = new DataSet();
        //            DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, OrderID.ToString(), "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);
        //            if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
        //            {
        //                txterr.Text = "Order No already exists, please Re-enter Order No";
        //               // OrderID = -1;
        //                ttOrder.Text = "";
        //            }
        //        }

        //    }
        //    PayType.Visible = true;
        //}


        //protected void ttinter_order_TextChanged(object sender, EventArgs e)
        //{
        //    if (ttinter_order.Text == "")
        //        ttinter_order.Text = "WAG" + OrderID.ToString();

        //    refid = objHelperServices.CS(ttinter_order.Text);


        //    if (OrderID <= 0 || ttinter_order.Text.Length < 1)
        //    {
              
        //        lblerror_ttinter.Text = "Please enter valid Order No, Order No should be more than 1 digits";
        //        ttinter_order.Text = "";
        //        //OrderID = -1;
        //        //Session["OrderId"] = "-1";
        //    }
        //    else
        //    {
              
        //        lblerror_ttinter.Text = "";
        //        string querystr = "";

        //        if (Request.QueryString["ApproveOrder"] == null)
        //        {
        //            DataSet DS = new DataSet();
        //            DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, OrderID.ToString(), "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);
        //            if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
        //            {
        //                lblerror_ttinter.Text = "Order No already exists, please Re-enter Order No";
        //                ttinter_order.Text = "";
        //            }
        //        }

        //    }
        //}

        [System.Web.Services.WebMethod]
        public static string GetData(string PO)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            try
            {
                //if (ttOrder.Text == "")
                //    ttOrder.Text = "WAG" + OrderID.ToString();

                string refid = "";
                HelperServices objHelperServices = new HelperServices();
                HelperDB objHelperDB = new HelperDB();
                refid = objHelperServices.CS(PO);


                if (PO.Length < 1)
                {
                    //txterr.Text = "Please enter valid Order No, Order No should be more than 1 digits";
                   // OrderID = -1;
                   // Session["OrderId"] = "-1";
                  //  return "1";
                    return "1";
                }
                else
                {
                   // string querystr = "";
                    
                        //DS = (DataSet)objHelperDB.GetGenericPageDataDB("", HttpContext.Current.Session["USER_ID"].ToString(), refid, HttpContext.Current.Session["ORDER_ID"].ToString(), "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);
                        string sSQL = string.Format("EXEC STP_TBWC_CHECK_PO_NUMBER_EXIST '" + HttpContext.Current.Session["ORDER_ID"].ToString() + "','" + HttpContext.Current.Session["USER_ID"].ToString() + "','" + refid + "'");
                        //objErrorHandler.CreateLog(sSQL); 
                    DataTable   DS = objHelperDB.GetDataTableDB(sSQL);
                
                   
                            if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                            {
                                // txterr.Text = "Order No already exists, please Re-enter Order No";
                                // OrderID = -1;
                              
                                    return "2";
                              
                            }
                      
                        //else
                        //{
                        //    int i = 0;
                        //    PaymentServices objPaymentServices = new PaymentServices();
                        //    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                        //    oPayInfo.PORelease = PO.ToString();
                        //    oPayInfo.OrderID = Convert.ToInt32( HttpContext.Current.Session["ORDER_ID"].ToString());
                        //    i = objPaymentServices.UpdatePaymentExpress(oPayInfo);
                        //    if (i > 0)
                        //    {
                        //        return "3";
                        //    }
                        //}
                  

                }

                return "false".ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                return "false".ToString();
            }
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

        protected void BtnMakePaymentPP_Click(object sender, EventArgs e)
        {
        }
        protected void ChkBillingAdd_CheckedChanged1(object sender, EventArgs e)
        {

            if (ChkBillingAdd.Checked == true)
            {
                txtsadd_Bill.Text = txtsadd.Text;
                txtadd2_Bill.Text = txtadd2.Text;
                txttown_Bill.Text = txttown.Text;
                if (drpCountry.SelectedValue == "AU")
                {
                    drpstate2.Visible = true;
                    drpstate2.SelectedValue = drpstate1.SelectedValue;
                    RequiredFieldValidator14.Enabled = false;
                    txtzip_bill.Text = txtzip.Text;
                   // txtzip_inter_bill.Visible = false;
                   // intercust_bill.Visible = false;
                    txtstate_Bill.Visible = false;
                    txtstate_Bill.Style.Add("display", "none"); 
                }
                else
                {
                    txtstate_Bill.Text = txtstate.Text;
                    txtstate_Bill.Style.Add("display", "block"); 

                    txtstate_Bill.Visible = true;
                    RequiredFieldValidator13.Enabled = false;
                    txtzip_bill.Text = txtzip_inter.Text;
                    drpstate2.Visible = false;
                   // txtzip_bill.Visible = false;
                   // aucust_bill.Visible = false;
                }
                drpcountry_bill.Text = drpCountry.SelectedValue;
                L2DivBilling.Visible = false;
            }
            else
            {
                //LoadBillInfo(Session["USER_ID"].ToString());
                txtsadd_Bill.Text = string.Empty;
                txtadd2_Bill.Text = string.Empty;
                txttown_Bill.Text = string.Empty;
                if (drpCountry.SelectedValue == "AU")
                {
                    drpstate2.SelectedValue = drpstate1.SelectedValue;
                    RequiredFieldValidator14.Enabled = true;
                    txtzip_bill.Text = string.Empty;
                  
                }
                else
                {
                    txtstate_Bill.Text = string.Empty;
                    RequiredFieldValidator13.Enabled = true;
                    txtzip_bill.Text = string.Empty;
                 
                }
                drpcountry_bill.Text = drpCountry.SelectedValue;
                L2DivBilling.Visible = true;
            }

            //lblError.Text = "";
            //lblErrorusername.Text = "";
            //tabcontrolprofileHiddenField.Value = "true";
            //if (ChkBillingAdd.Checked == true)
            //{
            //    txtbilladd1.Text = txtAdd1.Text;
            //    txtbilladd2.Text = txtAdd2.Text;
            //    txtbilladd3.Text = txtAdd3.Text;
            //    txtbillcity.Text = txtCity.Text;
            //    if (drpCountry.SelectedValue == "AU")
            //    {
            //        ddlbillstate.SelectedValue = ddlstate.SelectedValue;
            //        drpBillState.Visible = false;
            //        ddlbillstate.Visible = true;
            //        RVddlBillstate.Enabled = true;
            //        RVtxtBillstate.Enabled = false;
            //    }
            //    else
            //    {
            //        drpBillState.Text = drpState.Text;
            //        drpBillState.Visible = true;
            //        ddlbillstate.Visible = false;
            //        RVddlBillstate.Enabled = false;
            //        RVtxtBillstate.Enabled = true;
            //    }
            //    drpBillCountry.Text = drpCountry.SelectedValue;
            //    txtbillphone.Text = txtPhone.Text;
            //    txtbillzip.Text = txtZip.Text;
            //}
            //else
            //{
            //    LoadBillInfo(Session["USER_ID"].ToString());
            //}
            Level3.Visible = false;
            Level4.Visible = false;
           
            Session["EditAddress"] = true;
           
            Level4.Visible = false;
            ScrollToControl("ctl00_maincontent_l2div", false);
        }
        //protected void ImageButton4_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        QuoteServices objQuoteServices = new QuoteServices();
        //        OrderDB objOrderDB = new OrderDB();
        //        HelperServices objHelperService = new HelperServices();
        //        //int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
        //        int OrdStatus = 0;
        //        string ApproveOrder = string.Empty;
        //        //Direct  Order / Approve Order (Comes from Pending order Page)
        //        //lblpostcode2err.Text = "";
        //        //lbladdline1err.Text = "";
        //        //lbladdline2err.Text = "";
        //        string clientIPAddress = "";

        //        if (OrderID > 0)
        //        {
        //            if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
        //                clientIPAddress = Session["IP_ADDR"].ToString();
        //            else
        //                clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

        //            objOrderServices.InitilizeOrder_ipaddress(OrderID, clientIPAddress);

        //        }
        //        if (drpCountry.SelectedItem.ToString().ToLower() == "")
        //            return;

        //        //if (OrderID > 0) //&& drpCountry.SelectedItem.ToString().ToLower() == "australia"
        //        //{
        //        //    String sessionId;
        //        //    sessionId = Session.SessionID;
        //        //    int orditemuseridupdate = 0;
        //        //    int newordid = 0;
        //        //    int getnewordid = 0;
        //        //    int order_id = 0;

        //        //    int chkcurordid = 0;
        //        //    try
        //        //    {
        //        //        bool tmp = objUserServices.CheckUserRegisterEmail(txtemail.Text.Trim(), "Retailer");
        //        //        if (tmp == false)
        //        //        {
        //        //            int i = SaveUserProfile();
        //        //            if (i > 0)
        //        //            {
        //        //                DataSet tmpds = objUserServices.CheckMultipleUserMail(txtemail.Text.Trim(), "Retailer");
        //        //                if ((tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0))
        //        //                {
        //        //                    int UserID = objUserServices.GetUserID(txtemail.Text.Trim());
        //        //                    //int UserIDUpdateOrd = 0;
        //        //                    string Role;
        //        //                    Role = objUserServices.GetRole(UserID);
        //        //                    oOrdInfo.UserID = UserID;
        //        //                    chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);
        //        //                    if (chkcurordid == 0)
        //        //                        newordid = objOrderServices.InitilizeOrder(oOrdInfo);
        //        //                    getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);

        //        //                    orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(OrderID, UserID, sessionId, getnewordid, sessionId);
        //        //                    //objOrderServices.OrderItemsUpdate_UserId(order_id, Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), sessionId, getnewordid, sessionId);
        //        //                    if (getnewordid > 0)
        //        //                    {
                                        
        //        //                        Session["ORDER_ID"] = getnewordid;
        //        //                        Session["USER_ID"] = UserID;
        //        //                        Session["EXPRESS_CHECKOUT"] = "True";
        //        //                        Session["USER_ROLE"] = Role;
        //        //                        oOrdInfo.OrderID = getnewordid;
        //        //                        objOrderServices.UpdateOrderPrice_ExpressCheckout(oOrdInfo, true, sessionId);
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        Session["ORDER_ID"] = chkcurordid;
        //        //                        Session["USER_ID"] = UserID;
        //        //                        Session["EXPRESS_CHECKOUT"] = "True";
        //        //                        Session["USER_ROLE"] = Role;
        //        //                        oOrdInfo.OrderID = chkcurordid;
        //        //                        OrderID = chkcurordid;
        //        //                    }




        //        //                    //UserIDUpdateOrd = objOrderServices.OrderItemsUpdate_UserId_ExpressCheckout(OrderID, UserID, sessionId);
        //        //                }
        //        //                try
        //        //                {
        //        //                    string SMPass="wag" + txtfname.Text.ToString() + txtlname.Text.ToString();
        //        //                    SendMailRegiExpressCheckout(txtemail.Text.Trim(), SMPass, txtemail.Text.Trim(), txtfname.Text.ToString(), txtlname.Text.ToString());
        //        //                }
        //        //                catch (Exception ex)
        //        //                {
        //        //                    objErrorHandler.ErrorMsg = ex;
        //        //                    objErrorHandler.CreateLog();
        //        //                }
        //        //            }
        //        //        }
        //        //        else
        //        //        {
        //        //            //already exists email address
        //        //            DataSet tmpds = objUserServices.CheckMultipleUserMail(txtemail.Text.Trim(), "Retailer");
        //        //            if ((tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0))
        //        //            {
        //        //                if (tmpds.Tables[0].Rows[0]["Country"].ToString() != "" && tmpds.Tables[0].Rows[0]["Country"].ToString().ToLower() == "australia" && drpCountry.SelectedItem.ToString().ToLower() == "australia")
        //        //                {
        //        //                    int UserID = objUserServices.GetUserID(txtemail.Text.Trim());
        //        //                    //int UserIDUpdateOrd = 0;
        //        //                    string Role;
        //        //                    Role = objUserServices.GetRole(UserID);
        //        //                    oOrdInfo.UserID = UserID;
        //        //                    chkcurordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);
        //        //                    if (chkcurordid == 0)
        //        //                        newordid = objOrderServices.InitilizeOrder(oOrdInfo);
        //        //                    getnewordid = objOrderServices.GetOrderID(objHelperServices.CI(UserID), 1);

        //        //                    orditemuseridupdate = objOrderServices.OrderItemsUpdate_UserId(OrderID, UserID, sessionId, getnewordid, sessionId);
        //        //                    //objOrderServices.OrderItemsUpdate_UserId(order_id, Convert.ToInt32(HttpContext.Current.Session["USER_ID"]), sessionId, getnewordid, sessionId);
        //        //                    if (getnewordid > 0)
        //        //                    {
        //        //                        Session["ORDER_ID"] = getnewordid;
        //        //                        Session["USER_ID"] = UserID;
        //        //                        Session["EXPRESS_CHECKOUT"] = "True";
        //        //                        Session["USER_ROLE"] = Role;
        //        //                        oOrdInfo.OrderID = getnewordid;
        //        //                        objOrderServices.UpdateOrderPrice_ExpressCheckout(oOrdInfo, true, sessionId);
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        Session["ORDER_ID"] = chkcurordid;
        //        //                        Session["USER_ID"] = UserID;
        //        //                        Session["EXPRESS_CHECKOUT"] = "True";
        //        //                        Session["USER_ROLE"] = Role;
        //        //                        oOrdInfo.OrderID = chkcurordid;
        //        //                        OrderID = chkcurordid;
        //        //                    }
        //        //                }
        //        //                else if (tmpds.Tables[0].Rows[0]["Country"].ToString() != "" && tmpds.Tables[0].Rows[0]["Country"].ToString().ToLower() != "australia" && drpCountry.SelectedItem.ToString().ToLower() == "australia")
        //        //                {

        //        //                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('This Email ID already register australia customer in wagner.Please try another Email address.');", true);
        //        //                    return;
        //        //                }
        //        //                else
        //        //                {
        //        //                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('This Email ID already register in outside australia customer in wagner.Please try another Email address.');", true);
        //        //                    return;
        //        //                }


        //        //            }
        //        //        }


        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        objErrorHandler.ErrorMsg = ex;
        //        //        objErrorHandler.CreateLog();
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    //inter

        //        //}
             


        //        if (Request.QueryString["ApproveOrder"] == null)
        //        {
        //            if (Session["USER_ROLE"] != null)
        //            {
        //                switch (Convert.ToInt16(Session["USER_ROLE"]))
        //                {
        //                    case 1:
        //                        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
        //                        break;
        //                    case 2:
        //                        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
        //                        break;
        //                    case 3:
        //                        OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
        //            }
        //        }
        //        else if (Request.QueryString["ApproveOrder"] != null && (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2))
        //        {
        //            OrdStatus = (int)OrderServices.OrderStatus.OPEN;
        //        }
        //        else if (Request.QueryString["ApproveOrder"] != null)
        //            OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;

        //        //OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
        //        decimal TaxAmount;
        //        decimal ProdTotCost;
        //        GetParams();
        //        if (OrderID <= 0)
        //            OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
        //        //else 
        //        //    OrderID = Convert.ToInt32(Request["OrderID"].ToString());

        //        int oldorderID = OrderID;
        //        int QuoteId = 0;
        //        QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
        //        if (ttOrder.Text == "")
        //            ttOrder.Text = "WAG" + OrderID.ToString();

        //        refid = objHelperServices.CS(ttOrder.Text);


        //        if (OrderID <= 0 || ttOrder.Text.Length < 1)
        //        {
        //            txterr.Text = "Please enter valid Order No, Order No should be more than 1 digits";
        //            OrderID = -1;
        //            Session["OrderId"] = "-1";
        //        }
        //        else
        //        {
        //            txterr.Text = "";
        //            string querystr = "";

        //            if (Request.QueryString["ApproveOrder"] == null)
        //            {
        //                DataSet DS = new DataSet();
        //                //querystr = "select count(*) from TBWC_PAYMENT where order_id in(select order_id from dbo.tbwc_order where [user_id] in(select [user_id] from TBWC_COMPANY_BUYERS where company_id in(select company_id from dbo.TBWC_COMPANY_BUYERS where [user_id]=" + objHelperServices.CI(Session["USER_ID"].ToString()) + "))) and po_release='" + refid + "'";
        //                //objHelperServices.SQLString = querystr;
        //                //DS = objHelperServices.GetDataSet();
        //                DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, OrderID.ToString(), "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);

        //                if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
        //                {
        //                    txterr.Text = "Order No already exists, please Re-enter Order No";
        //                    OrderID = -1;
        //                }
        //            }

        //        }
        //        if (Request["QteId"] != null)
        //        {
        //            QuoteID = objHelperServices.CI(Request["QteId"].ToString());
        //            OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
        //            OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
        //        }

        //        //if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
        //        //{
        //        //    if (objOrderServices.GetDropShipmentKeyExist(txtPostCode.Text.ToString(), "PostCode") == true)
        //        //    {
        //        //        //lblpostcode2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
        //        //        //OrderID = -1;
        //        //        //ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
        //        //        if (objOrderServices.GetDropShipmentKeyExist(txtAddressLine1.Text.ToString(), "") == true)
        //        //        {
        //        //            //lbladdline1err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
        //        //            //   OrderID = -1;
        //        //            ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
        //        //            // ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>var x=window.confirm('Non-Standard Delivery Area. We will contact you to confirm costing');if (x)window.alert('Good!')</script>", false);

        //        //        }
        //        //        if (txtAddressLine2.Text.ToString() != "" && objOrderServices.GetDropShipmentKeyExist(txtAddressLine2.Text.ToString(), "") == true)
        //        //        {
        //        //            //lbladdline2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
        //        //            //   OrderID = -1;
        //        //            ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
        //        //        }
        //        //    }
                  
        //        //}

            
        //        if (OrderID > 0)
        //        {
        //            int _UserrID;
        //            _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
        //            oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
        //            oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);



        //            //  if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
        //            if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
        //                OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
        //            else
        //                OrdStatus = (int)OrderServices.OrderStatus.OPEN;

        //            ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
        //            TaxAmount = objOrderServices.CalculateTaxAmount_Express(ProdTotCost, OrderID.ToString());
        //            decimal UpdRst = 0;
        //            oOrdInfo.OrderID = OrderID;
        //            oOrdInfo.OrderStatus = OrdStatus;
        //            ///////////////////////////


        //            oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
        //            //txtSFName.Text = oOrdShippInfo.FirstName;
        //            //txtSLName.Text = oOrdShippInfo.LastName;
        //            //txtSMName.Text = oOrdShippInfo.MiddleName;
        //            //txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
        //            //txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
        //            //txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
        //            //txtSCity.Text = oOrdShippInfo.ShipCity;
        //            //drpShipState.Text = oOrdShippInfo.ShipState;
        //            //txtSZip.Text = oOrdShippInfo.ShipZip;
        //            //Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
                  

        //           // txtSPhone.Text = oOrdShippInfo.ShipPhone;
        //            double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
        //            if (Dist <= 50 && Dist > 0)
        //            {
        //                oLstItem.Text = "Friendly Driver";
        //                oLstItem.Value = "FRIENDLYDRIVER";
        //                oLstItem.Selected = true;
        //               // cmbProvider.Items.Add(oLstItem);
        //            }

                  
        //            oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
        //            //txtbillFName.Text = oOrdBillInfo.FirstName;
        //            //txtbillLName.Text = oOrdBillInfo.LastName;
        //            //txtbillMName.Text = oOrdBillInfo.MiddleName;
        //            //txtbilladd1.Text = oOrdBillInfo.BillAddress1;
        //            //txtbilladd2.Text = oOrdBillInfo.BillAddress2;
        //            //txtbilladd3.Text = oOrdBillInfo.BillAddress3;
        //            //txtbillcity.Text = oOrdBillInfo.BillCity;
        //            //drpBillState.Text = oOrdBillInfo.BillState;
        //            //txtbillzip.Text = oOrdBillInfo.BillZip;
        //            //Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
                   
        //            //txtbillphone.Text = oOrdBillInfo.BillPhone;
                   
        //            oOrdInfo.OrderStatus = OrdStatus;
        //           // oOrdInfo.ShipCompany = cmbProvider.SelectedValue;
        //            oOrdInfo.ShipMethod = drpSM1.SelectedValue;

        //            if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
        //            {
        //               // oOrdInfo.ShipCompName = objHelperServices.Prepare(txtCompany.Text);
        //               // oOrdInfo.ShipFName = objHelperServices.Prepare(txtAttentionTo.Text);
        //               // oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtAddressLine1.Text);
        //               // oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtAddressLine2.Text);
        //               // oOrdInfo.ShipCity = objHelperServices.Prepare(txtSuburb.Text);
        //               // oOrdInfo.ShipState = objHelperServices.Prepare(drpState.Text);
        //              //  oOrdInfo.ShipCountry = objHelperServices.Prepare(txtCountry.Text);
        //              //  oOrdInfo.ShipZip = objHelperServices.Prepare(txtPostCode.Text);
        //              //  oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDeliveryInstructions.Text);
        //                //  oOrdInfo.ReceiverContact = objHelperServices.Prepare(txtReceiverContactName.Text);
        //               // oOrdInfo.ShipPhone = objHelperServices.Prepare(txtShipPhoneNumber.Text);

        //            }
        //            else
        //            {
        //                //oOrdInfo.ShipFName = objHelperServices.Prepare(txtSFName.Text);
        //                //oOrdInfo.ShipLName = objHelperServices.Prepare(txtSLName.Text);
        //                //oOrdInfo.ShipMName = objHelperServices.Prepare(txtbillMName.Text);
        //                //oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtSAdd1.Text);
        //                //oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtSAdd2.Text);
        //                //oOrdInfo.ShipAdd3 = objHelperServices.Prepare(txtSAdd3.Text);
        //                //oOrdInfo.ShipCity = objHelperServices.Prepare(txtSCity.Text);
        //                //oOrdInfo.ShipState = objHelperServices.Prepare(drpShipState.Text);
        //                //oOrdInfo.ShipCountry = objHelperServices.Prepare(drpShipCountry.SelectedItem.ToString());
        //                //oOrdInfo.ShipZip = objHelperServices.Prepare(txtSZip.Text);
        //                //oOrdInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);


        //                oOrdInfo.ShipFName = ""; // objHelperServices.Prepare(txtfname.Text);
        //                oOrdInfo.ShipLName = "";// objHelperServices.Prepare(txtlname.Text);
        //                oOrdInfo.ShipMName = "";// objHelperServices.Prepare(txtbillMName.Text);
        //                oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtsadd.Text);
        //                oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtadd2.Text);
        //                oOrdInfo.ShipAdd3 = "";// objHelperServices.Prepare(txtSAdd3.Text);
        //                oOrdInfo.ShipCity = objHelperServices.Prepare( txttown.Text);
        //                oOrdInfo.ShipState = objHelperServices.Prepare(drpstate1.Text);
        //                oOrdInfo.ShipCountry = objHelperServices.Prepare(drpCountry.SelectedItem.ToString());
        //                oOrdInfo.ShipZip = objHelperServices.Prepare( txtzip.Text);
        //                oOrdInfo.ShipPhone = ""; // objHelperServices.Prepare(txtphone.Text);

        //                DataSet objds = new DataSet();
        //                objds = (DataSet)objOrderDB.GetGenericDataDB(objHelperServices.CI(Session["USER_ID"].ToString()).ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
        //                if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
        //                {
        //                    oOrdInfo.DeliveryInstr = objHelperService.CS(objds.Tables[0].Rows[0]["DELIVERY_INST"].ToString());
        //                }

        //            }


        //            oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);



        //            oOrdInfo.ClientIPAddress = clientIPAddress;   
        //            oOrdInfo.isEmailSent = false;
        //            oOrdInfo.isInvoiceSent = false;
        //            oOrdInfo.IsShipped = false;

        //            oOrdInfo.BillFName = "";//objHelperServices.Prepare(txtfname.Text);
        //            oOrdInfo.BillLName = "";// objHelperServices.Prepare(txtlname.Text);
        //            oOrdInfo.BillMName = "";// objHelperServices.Prepare(txtbillMName.Text);
        //            oOrdInfo.BillAdd1 = objHelperServices.Prepare(txtsadd.Text);
        //            oOrdInfo.BillAdd2 = objHelperServices.Prepare(txtadd2.Text);
        //            oOrdInfo.BillAdd3 = "";// objHelperServices.Prepare(txtbilladd3.Text);
        //            oOrdInfo.BillCity = objHelperServices.Prepare(txttown.Text);
        //            oOrdInfo.BillState = objHelperServices.Prepare(drpstate1.Text);
        //            oOrdInfo.BillCountry = objHelperServices.Prepare(drpCountry.SelectedItem.Text);
        //            oOrdInfo.BillZip = objHelperServices.Prepare(txtzip.Text);
        //            oOrdInfo.BillPhone = "";// objHelperServices.Prepare(txtphone.Text);
        //            oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
        //            oOrdInfo.ShipCost = CalculateShippingCost(OrderID);
        //            oOrdInfo.TaxAmount = objOrderServices.CalculateTaxAmount_Express(oOrdInfo.ProdTotalPrice + oOrdInfo.ShipCost, OrderID.ToString());
        //            oOrdInfo.TotalAmount = ProdTotCost + oOrdInfo.TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
        //            oOrdInfo.TrackingNo = "";
        //            if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
        //            {
        //                oOrdInfo.DropShip = 1;
        //            }
        //            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        //            UpdRst = objOrderServices.UpdateOrder(oOrdInfo);

        //            if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
        //            {
        //                Session["PrevOrderID"] = "0";
        //            }

        //            Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
        //            if (UpdRst > 0)
        //            {
        //                objOrderServices.UpdateCustomFields(oOrdInfo);

        //                Session["ORDER_NO"] = null;
        //                Session["SHIPPING"] = null;
        //                Session["DELIVERY"] = null;
        //                Session["DROPSHIP"] = null;

        //                Session["ShipCost"] = oOrdInfo.ShipCost;
        //                if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
        //                {
        //                    Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
        //                }
        //                else
        //                {
        //                    ProceedFunction();
        //                   // PnlOrderInvoice.Visible = true;
        //                   // PnlOrderContents.Visible = false;

        //                    PHOrderConfirm.Visible = true;
        //                   // Level4_Payment.Visible = false;
        //                   // Level4_Submit.Visible = false;

        //                    //ttOrder.Enabled = false;
        //                    //drpSM1.Enabled = false;

        //                    // new code pk
        //                   // txtfname.Enabled = false;
        //                   // txtlname.Enabled = false;
        //                   // txtphone.Enabled = false;
        //                   // txtemail.Enabled = false;



        //                    //txtsadd.Enabled = false;
        //                    //txtadd2.Enabled = false;
        //                    //txttown.Enabled = false;
        //                    //txtstate.Enabled = false;
        //                    //txtzip_inter.Enabled = false;
        //                    //drpCountry.Enabled = false;

        //                    /* Drop Shipment Fields  */
        //                    //drpState.Enabled = false;
        //                    //txtCompany.Enabled = false;
        //                    //txtPostCode.Enabled = false;
        //                    //txtAddressLine1.Enabled = false;
        //                    //txtAddressLine2.Enabled = false;
        //                    //txtAttentionTo.Enabled = false;
        //                    //txtCountry.Enabled = false;
        //                    //drpShipState.Enabled = false;
        //                    //// txtReceiverContactName.Enabled=false;
        //                    //txtSuburb.Enabled = false;
        //                    //txtDeliveryInstructions.Enabled = false;
        //                    //txtShipPhoneNumber.Enabled = false;
        //                    //txtDeliveryInstructions.Enabled = false;

        //                    //ImageButton4.Visible = false;
        //                   // ImageButton1.Visible = false;
        //                    // ImageButton5.Visible = false;
        //                    //TextBox1.Enabled = false;
        //                    //TextBox1.ReadOnly = true;
        //                   // ImageButton2.Visible = false;
        //                    //Response.Redirect("Confirm.aspx?OrdId=99999&ViewType=Confirm");
        //                    //if (oOrdBillInfo.BillCountry.ToLower().Trim() == "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() == "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() == "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
        //                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
        //                    {
                              
        //                      //  SecurePayService objSecurePayService = new SecurePayService();
        //                      //  if (objSecurePayService.CheckSecurePay() == true)
        //                      ////      Response.Redirect("expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "PaySP"), false);
        //                     //   else
        //                            Response.Redirect("expresscheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay"), false);

        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        objErrorHandler.ErrorMsg = Ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}

        public int UpdateUserData()
        {
            try
            {
                string BusinessDsc = string.Empty;
                string comp_ABN = string.Empty;
                oOrdBillInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdBillInfo.COMPANY_NAME = objHelperServices.Prepare( txtComname.Text);
                //objErrorHandler.CreatePayLog("atto"+txt_attnto.Text);
                oOrdBillInfo.DELIVERYINST =  objHelperServices.Prepare(txtDELIVERYINST.Text);
     //           oOrdBillInfo.MobilePhone = txtMobilePhone1.Text;                                    //Uncomment the Line
      //          hfphonenumber.Value = txtMobilePhone1.Text;
                
                       UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                            ouserinfo = objUserServices.GetUserInfo(Userid);

                            if (txt_attnto.Text == ouserinfo.FirstName.Trim() + ouserinfo.LastName.Trim())
                            {
                                txt_attnto.Text = ouserinfo.FirstName + " " + ouserinfo.LastName;
                                 oOrdBillInfo.Receiver_Contact = ouserinfo.FirstName +" " + ouserinfo.LastName;
                            }
                            else
                            {
                                oOrdBillInfo.Receiver_Contact = objHelperServices.Prepare(txt_attnto.Text);
                            }

                //if (RBPersonal.Checked == true)
                //    BusinessDsc = "Personal";
                //else
                //    BusinessDsc = "Business";
                //comp_ABN = txtABN.Text;


                if (ChkBillingAdd.Checked == true)
                {
                    oOrdBillInfo.ShipAddress1 = objHelperServices.Prepare( txtsadd.Text);
                    oOrdBillInfo.ShipAddress2 =  objHelperServices.Prepare(txtadd2.Text);
                    oOrdBillInfo.ShipAddress3 = "";
                    oOrdBillInfo.ShipCity = txttown.Text;
                    if (drpCountry.SelectedValue == "AU")
                    {
                        oOrdBillInfo.ShipState = drpstate1.SelectedValue;
                        oOrdBillInfo.ShipZip = txtzip.Text;
                    }
                    else
                    {
                        oOrdBillInfo.ShipState =  objHelperServices.Prepare(txtstate.Text);
                        oOrdBillInfo.ShipZip = txtzip_inter.Text;
                    }


                    oOrdBillInfo.ShipCountry = drpCountry.SelectedItem.ToString();


                    oOrdBillInfo.BillAddress1 = objHelperServices.Prepare( txtsadd.Text);
                    oOrdBillInfo.BillAddress2 = objHelperServices.Prepare( txtadd2.Text);
                    oOrdBillInfo.BillAddress3 = "";// txtbilladd3.Text;
                    oOrdBillInfo.BillCity =  objHelperServices.Prepare(txttown.Text);
                    oOrdBillInfo.Bill_Company =  objHelperServices.Prepare(txtComname.Text);
                    oOrdBillInfo.Bill_Name = objHelperServices.Prepare( txt_attnto.Text);

                    if (drpCountry.SelectedValue == "AU")
                    {
                        oOrdBillInfo.BillState = drpstate1.SelectedValue;
                        oOrdBillInfo.BillZip = txtzip.Text;
                    }
                    else
                    {
                        oOrdBillInfo.BillState = objHelperServices.Prepare( txtstate.Text);
                        oOrdBillInfo.BillZip = txtzip_inter.Text;
                    }

                    oOrdBillInfo.BillCountry = drpCountry.SelectedItem.ToString();
                    oOrdBillInfo.ShipPhone = L2Phone.Text;
                    oOrdBillInfo.BillPhone = L2Phone.Text; 
                }
                else
                {

                    oOrdBillInfo.ShipAddress1 =  objHelperServices.Prepare(txtsadd.Text);
                    oOrdBillInfo.ShipAddress2 = objHelperServices.Prepare( txtadd2.Text);
                    oOrdBillInfo.ShipAddress3 = "";
                    oOrdBillInfo.ShipCity =  objHelperServices.Prepare(txttown.Text);
                    if (drpCountry.SelectedValue == "AU")
                    {
                        oOrdBillInfo.ShipState = drpstate1.SelectedValue;
                        oOrdBillInfo.ShipZip = txtzip.Text;
                    }
                    else
                    {
                        oOrdBillInfo.ShipState = objHelperServices.Prepare( txtstate.Text);
                        oOrdBillInfo.ShipZip = txtzip_inter.Text;
                    }


                    oOrdBillInfo.ShipCountry = drpCountry.SelectedItem.ToString();


                    oOrdBillInfo.BillAddress1 = objHelperServices.Prepare( txtsadd_Bill.Text);
                    oOrdBillInfo.BillAddress2 =  objHelperServices.Prepare(txtadd2_Bill.Text);
                    oOrdBillInfo.BillAddress3 = "";// txtbilladd3.Text;
                    oOrdBillInfo.BillCity = objHelperServices.Prepare( txttown_Bill.Text);
                    if (drpcountry_bill.SelectedValue == "AU")
                    {
                        oOrdBillInfo.BillState = drpstate2.SelectedValue;
                        oOrdBillInfo.BillZip = txtzip_bill.Text;
                    }
                    else
                    {
                        oOrdBillInfo.BillState = txtstate_Bill.Text;
                        oOrdBillInfo.BillZip = txtzip_bill.Text;
                    }

                    oOrdBillInfo.BillCountry = drpcountry_bill.SelectedItem.ToString();
                    oOrdBillInfo.Bill_Company = objHelperServices.Prepare( txtbillbusname.Text);
                    oOrdBillInfo.Bill_Name = objHelperServices.Prepare( txtbillname.Text);
                    oOrdBillInfo.DELIVERYINST =  objHelperServices.Prepare(txtDELIVERYINST.Text);
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

        public int SaveUserProfileInit()
        {
             UserServices.RegistrationInfo oRegInfo = new UserServices.RegistrationInfo();
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
                oRegInfo.Email = letstarttxtemail.Text.ToString();
                oRegInfo.BusinessType = "NA";
                oRegInfo.BusinessDsc = "Personal";
                oRegInfo.SiteID = "3";
                oRegInfo.CustStatus = "False";
                oRegInfo.Status = "I";
                oRegInfo.RegType = "N";
              //  string Password = "wag" + letstarttxtemail.Text.ToString();
               // string Password = "wag" + letstarttxtemail.Text.ToString();
                string Password =txtRegpassword.Text;
                string Newpassword = objcrpengine.StringEnCrypt_password(Password.ToString());
                oRegInfo.Password = Newpassword;
                string clientIPAddress = "";
               
                //if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
                //    clientIPAddress = Session["IP_ADDR"].ToString();
                //else
                //    clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

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

        public int SaveUserProfile()
        {
            UserServices.RegistrationInfo oRegInfo = new UserServices.RegistrationInfo();
            Security objcrpengine = new Security();
            try
            {
                //oRegInfo.Customer_Type = "Retailer";

                //oRegInfo.CompanyName = "";
                //oRegInfo.AbnAcn = "";


                //oRegInfo.Address1 = ""; 
                //oRegInfo.Address2 = "";
                //oRegInfo.SubCity = "";
 
                //oRegInfo.State = "";


                //oRegInfo.Country = "Australia";
                //oRegInfo.PostZipcode = "";
        
                oRegInfo.Fname = txtRegFname.Text.ToString();
                oRegInfo.Lname = txtRegLname.Text.ToString();
                //oRegInfo.Position = "";
                oRegInfo.Phone = txtRegphone.Text.ToString();
                //oRegInfo.Mobile = "";
                //oRegInfo.Fax = "";
                oRegInfo.Email = txtregemail.Text.ToString();
                oRegInfo.Mobile = txtRegMobilePhone.Text.ToString();
                //oRegInfo.BusinessType = "NA";
                //     oRegInfo.BusinessDsc = "Personal";
                oRegInfo.SiteID = "3";
                //oRegInfo.CustStatus = "False";
                //oRegInfo.Status = "I";
                //oRegInfo.RegType = "N";
                string Password = txtRegpassword.Text.Trim();
                string Newpassword = objcrpengine.StringEnCrypt_password(Password.ToString());
                oRegInfo.Password = Newpassword;
                string clientIPAddress = "";
               
                if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
                    clientIPAddress = Session["IP_ADDR"].ToString();
                else
                    clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                oRegInfo.IpAddr = clientIPAddress;   
               
                //oRegInfo.LastInvNo = "N/A";
                //oRegInfo.WesAccNo = "N/A";
                
                //return objUserServices.CreateRegistration(oRegInfo);
                return objUserServices.UpdateRegistrationExpress(oRegInfo);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
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
             //   QuoteServices objQuoteServices = new QuoteServices();
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

                   

                    int _UserrID;
                    _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
                    oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                    oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);



                    //if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
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
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                        {
                            //if (ttOrder.Text == "")
                            //    ttOrder.Text = "WAG" + OrderID.ToString();

                            refid = objHelperServices.CS("WAG" + OrderID.ToString());
                            oPayInfo.PORelease = refid;
                        }
                        else
                        {
                            if (ttinter_order.Text == "")
                            {
                                ttinter_order.Text = "WAG" + OrderID.ToString();
                                refid = objHelperServices.CS(ttinter_order.Text);
                            }
                            else if (ttinter_order.Text!="")
                            {
                                refid = objHelperServices.CS(ttinter_order.Text);
                               
                              
                            
                            }

                          
                            oPayInfo.PORelease = refid;
                        
                        }
                       // oPayInfo.Amount = TotCost;
                        oPayInfo.Amount = oOrdInfo.TotalAmount;
                        oPayInfo.UserId = OrderID;
                        objErrorHandler.CreateOrderSummarylog("Inside Proceed Function" + "PORelease :" + refid + "  oPayInfo.Amount :" + oOrdInfo.TotalAmount);
                    }
                    if (objUserServices.GetUserStatus(objHelperServices.CI(Session["USER_ID"].ToString())) == 1)
                    {
                        if (ChkOrderExist == 0)
                        {
                            ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                            int cStatus = 0;
                            //cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                            if (isau == false)
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
                         if (isau == false)
                             SendMail(OrderID, OrdStatus, isau);
                        //if (UptOrderStatus != -1)
                        //{
                        //   // int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                        //    objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        //    //SendNotification(OrderID);
                        //    if (isau == false)
                        //        SendMail(OrderID, OrdStatus, isau);

                        //    if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                        //    {
                        //        Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                        //    }
                        //    else
                        //    {
                        //        //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                        //    }
                        //}

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
                        //if (UptOrderStatus != -1)
                        //{
                        //   // int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                        //    objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        //    if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                        //    {
                        //        Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                        //    }
                        //    else
                        //    {
                        //        //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                        //    }
                        //}

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
                    objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
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
                   

                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        MessageObj.Bcc.Add(webadminmail);
                        objErrorHandler.CreateLog("order submitted mail to " + Emailadd + "--" + webadminmail + "CustomerOrderNo:"+ CustomerOrderNo);
                    }
                    else
                    {
                        emails = Get_ADMIN_APPROVED_UserEmils();

                        MessageObj.To.Add(Emailadd.ToString());
                        // MessageObj.Bcc.Add(addressBCC);
                        objErrorHandler.CreateLog("order submitted mail to " + Emailadd + "CustomerOrderNo:"+ CustomerOrderNo);
                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
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




                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                            objErrorHandler.CreateLog("order submitted mail to " + ApprovedUserEmailadd + "CustomerOrderNo:"+ CustomerOrderNo);
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
                                         objErrorHandler.CreateLog("order submitted mail to " + id + "CustomerOrderNo:"+ CustomerOrderNo);
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
                objHelperServices.Mail_Error_Log("ICOS", OrderID, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
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

        public void GetImage(int Product_id)
        {
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            ProductServices objProductServices = new ProductServices();
            int family_id = 0;
            family_id = objProductServices.GetFamilyID(Product_id);
            if (family_id > 0)
            {

                DataTable dt = objProductServices.GetPopProduct(Product_id, family_id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["TWeb Image1"].ToString() != null && dt.Rows[0]["TWeb Image1"].ToString() != String.Empty && dt.Rows[0]["TWeb Image1"].ToString() != "")
                    {
                        lblProImage.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dt.Rows[0]["TWeb Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
                    }
                    else
                    {
                        lblProImage.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
                    }

                    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dt.Rows[0]["TWeb Image1"].ToString().Replace("\\", "/"));
                    //if (Fil.Exists)
                    //    lblProImage.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dt.Rows[0]["TWeb Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
                    //else
                    //    lblProImage.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";


                }
            }
            else
            {
                lblProImage.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";
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



                //if (oOI.Receiver_Contact.Trim().Length > 0)
                //{
                //    sShippingAddress = sShippingAddress + "Attn To:" + oOI.Receiver_Contact + "<BR>";

                //}
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
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }
            return sShippingAddress;
        }
        public void LoadStates(String conCode)
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            drpstate1.DataSource = oDs;
            drpstate1.DataTextField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate1.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate1.DataBind();
            drpstate1.Items.Insert(0, new ListItem("Select", ""));

        }

        public void LoadStatesBill(String conCode)
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            drpstate2.DataSource = oDs;
            drpstate2.DataTextField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate2.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpstate2.DataBind();
            drpstate2.Items.Insert(0, new ListItem("Select", ""));
        }
        protected void drpCountry_bill_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (drpcountry_bill.SelectedValue == "AU")
            {
                //if (Page.IsPostBack)
                //{
                //    drpSM1.Items.Clear();
                //    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                //    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                //    drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                //    drpSM1.SelectedIndex = 0;
                //    //ImageButton2.Text = "Continue";
                //}
                drpstate2.Visible = true;
                LoadStates(drpcountry_bill.SelectedValue);
                drpcountry_bill.Focus();
                txtstate_Bill.Visible = false;
                txtstate_Bill.Style.Add("display", "none"); 
                RequiredFieldValidator13.Enabled = false;
                RequiredFieldValidator14.Enabled = true;
                txtstate_Bill.Text = "";               
              //  aucust_bill.Visible = true;
               // intercust_bill.Visible = false;
            }
            else
            {
                txtstate_Bill.Visible = true;
                txtstate_Bill.Style.Add("display", "block"); 
                txtstate_Bill.Focus();
                drpstate2.Visible = false;
                RequiredFieldValidator13.Enabled = true;
                RequiredFieldValidator14.Enabled = false;
               // aucust_bill.Visible = false;
               // intercust_bill.Visible = true;

                //drpSM1.Items.Clear();
                //drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                //drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                //drpSM1.SelectedIndex = 1;
                liPayOption.Visible = false;
                liFinalReview.Visible = false;
                //ImageButton2.Text = "Submit Order";


            }
            Level1.Visible = false;
            Level2.Visible = true;
            Session["EditAddress"] = true;
            Level3.Visible = false;
            Level4.Visible = false;
        }


        protected void drpSM1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (drpSM1.SelectedValue == "Standard Shipping")
            {
                SCP.Style.Add("display", "none");
                lblshipcost.Text = objHelperServices.GetOptionValues("COURIER CHARGE").ToString();
      //          counterPickupModalExt.TargetControlID = "lblhidcontinue";

                //shipping_charge_domestic_zero.Style.Add("display", "none");
                //shipping_charge_domestic_value.Style.Add("display", "block");
            }
            else if (   drpSM1.SelectedValue == "Counter Pickup")
            {
              SCP.Style.Add("display","block");




              SCP.Attributes.Remove("modal fade lgn-orderinfoscp");
                 SCP.Attributes.Add("class", "modal fade lgn-orderinfoscp in");
       //          counterPickupModalExt.TargetControlID = "BtnL3Continue";
              
            }
            else
            {
                //shipping_charge_domestic_zero.Style.Add("display", "block");
                //shipping_charge_domestic_value.Style.Add("display", "none");
                SCP.Style.Add("display", "none");
                lblshipcost.Text = "0.00";
       //         counterPickupModalExt.TargetControlID = "lblhidcontinue";
            }
            lblShippingMethod.Text = drpSM1.SelectedValue;
          //  PayType.Visible = true; 
         

            Level1.Visible = false;
            Level2.Visible = false;
            Level3.Visible = true;
            Level4.Visible = false;
            ScrollToControl("ctl00_maincontent_divl3continue",true);
            Session["ExpressLevel"] = "2Compl";
            
        }


        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (drpCountry.SelectedValue == "AU")
            {
                txtstate.Visible = false;
                rfvstate.Enabled = false;
                rfvstate.Visible = false;
                if (Page.IsPostBack)
                {
                    drpSM1.Items.Clear();
                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                    drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                    drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                    drpSM1.SelectedIndex = 0;
                    //ImageButton2.Text = "Continue";
                }
                drpstate1.Visible = true;
                LoadStates(drpCountry.SelectedValue);
                drpstate1.Focus();
               
                rfvddlstate.Enabled = true;
                txtstate.Text = "";
                aucust.Visible = true;
                intercust.Visible = false;
                txtzip.Visible = true;
                intorder.Visible = false;               
              //  aucust_bill.Visible = true;
              //  intercust_bill.Visible = false;
            }
            else
            {
                txtstate.Visible = true;
                txtstate.Focus();
                drpstate1.Visible = false;
                rfvstate.Enabled = true;
                rfvstate.Visible = true;
                rfvddlstate.Enabled = false;
                aucust.Visible = false;
                intercust.Visible = true;
                txtzip_inter.Visible = true;
               // aucust_bill.Visible = false;
               // intercust_bill.Visible = true;
                drpSM1.Items.Clear();
                drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                drpSM1.SelectedIndex = 1;
                intorder.Visible = true;
                liPayOption.Visible = false;
                liFinalReview.Visible = false;
               // ScrollToControl("ctl00_maincontent_l2div");
                //ImageButton2.Text = "Submit Order";
            }
            Level2.Visible = true;
            Level1.Visible = false;
            Level3.Visible = false;
            Level4.Visible = false;
            Session["EditAddress"] = true;
        }
        public string GetBillingAddress(int OrderID)
        {
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



                //  sBillingAddress = sBillingAddress + oBI.BillFName + oBI.BillLName + "<BR>";
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
                lblshipcost.Text = CalculateShippingCost(OrderID).ToString();
               // objErrorHandler.CreatePayLog_Final(lblshipcost.Text); 

            }
            catch(Exception ex)
            {}
            return returnselected;
        }
        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            //  this.modalPop.Hide();
            PopupMsg.Visible = false;
            string MicroSiteTemplate = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
            ConnectionDB objConnectionDB = new ConnectionDB();
            TBWTemplateEngineMS tbwtMSEngine = new TBWTemplateEngineMS("NEWPRODUCT", MicroSiteTemplate, objConnectionDB.ConnectionString);
            DataTable ttbl = tbwtMSEngine.GetSupplierDetail();
            string micrositeurl = "";
            if (ttbl != null && ttbl.Rows.Count > 0)
            {
                string strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
                string strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
                string strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();
                micrositeurl = objHelperServices.SimpleURL_MS_Str(strsupplierName, "mct.aspx", true) + "/mct/";
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
        //protected void btnPayPalPayLink_Click(object sender, EventArgs e)
        //{
        //    //Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false);
        //    ImagePay.ImageUrl = "/images/paypal_check.png";
        //    // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
        //    ImagePaySP.ImageUrl = "/images/master_uncheck.png";
        //    btnPayApi.Visible = false;
        //    btnPay.Visible = true;
        //    //PaySPDiv.Visible = false;
        //    PaypalApiDiv.Visible = true;
        //    //PayPaypalAcc.Visible = true;
        //    SecurePayAcc.Visible = false;
        //   //PayType.Visible = false;

        //    //oPayInfo = objPaymentServices.GetPayment(OrderID);
        //    //lblpaypaltotamt.Text = oPayInfo.Amount.ToString();

        //}
        //protected void btnPayPalAPIPayLink_Click(object sender, EventArgs e)
        //{
        //    //Response.Redirect("ppapi.aspx?" + EncryptSP(OrderID.ToString()), false);
        //   // ImagePay.ImageUrl = "/images/pay2ch.png";
        //    // ImagePayApi.ImageUrl = "/images/express_Checkout_select.png";
        //    //ImagePaySP.ImageUrl = "/images/pay1uch.png";
        //    btnPayApi.Visible = true;
        //    btnPay.Visible = false;
        //    //PaySPDiv.Visible = false;
        //    PaypalApiDiv.Visible = true;
        //    //PayPaypalAcc.Visible = false;
        //}
        //protected void btnSecurePayLink_Click(object sender, EventArgs e)
        //{
        //    ImagePay.ImageUrl = "/images/paypal_uncheck.png";
        //    //   ImagePayApi.ImageUrl = "/images/express_Checkout.png";
        //    ImagePaySP.ImageUrl = "/images/master_check.png";
        //    // Response.Redirect("paysp.aspx?" + EncryptSP(OrderID.ToString()), false);
        //    btnPayApi.Visible = false;
        //    btnPay.Visible = false;
            
        //    PaypalApiDiv.Visible = false;

        //    SecurePayAcc.Visible = true;
        //    //PayPaypalAcc.Visible = false;
        //    //PayType.Visible = false;
        //}

        protected void BtnEditLogin_Click(object sender, EventArgs e)
        {
            letstartdivlogin.Visible = false;
            Level1.Visible = true;
            letstartregister.Visible = true;
            BtnRegLetStart.Visible = false;
            BtnRegLetStart_Edit.Visible = true;
            Level2.Visible = false;
            Level3.Visible = false;
            Level4.Visible = false;
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            ouserinfo = objUserServices.GetUserInfo(Userid);
            txtregemail.Text = ouserinfo.Email;
            txtregemail.ReadOnly = true;
            txtRegFname.Text = ouserinfo.FirstName;
            txtRegLname.Text = ouserinfo.LastName;
            txtRegphone.Text = ouserinfo.Phone;
            txtRegMobilePhone.Text = ouserinfo.MobilePhone;

            //Security objcrpengine = new Security();
            //txtRegpassword.Text = objcrpengine.StringDeCrypt_password(ouserinfo.Password);
            //txtRegConfirmPassword.Text = objcrpengine.StringDeCrypt_password(ouserinfo.Password);

}

        protected void btnSecurePay_Click(object sender, EventArgs e)
        {

           
            if (OrderID == 0)
            {
                if (HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                }
            }
            //decimal OrdtotCost = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));


            //if ((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0 && OrdtotCost > 0))
            //{
            SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            oOrderInfo = objOrderServices.GetOrder(OrderID);
            UserServices objUserServices = new UserServices();
            GetParams();
                objErrorHandler.CreateOrderSummarylog("inside securepay");
             
                if (oOrderInfo.TotalAmount.ToString() != lblAmount.Text)
                {
                    objErrorHandler.CreateOrderSummarylog("TotalAmount:" + oOrderInfo.TotalAmount + " " + "lblAmount :" + lblAmount.Text);
                    BtnL4EditShippingMethod_Click(sender, e);
                    return;
                }
               
                if (oOrderInfo.ProdTotalPrice == 0)
                {
                    objErrorHandler.CreateOrderSummarylog("btnSecurepay start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                }
                DataSet tmpds = GetOrderItemDetailSum(OrderID);
                decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                }
                
                if (oOrderInfo.ProdTotalPrice != totalitemsum )
                {
                   objErrorHandler.CreateOrderSummarylog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                    BtnL4EditShippingMethod_Click(sender, e);
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
                        objErrorHandler.CreateOrderSummarylog("securepay" + " " + sSQL);
                        DataTable DS = objHelperDB.GetDataTableDB(sSQL);

                         
                        if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                        {
                            txterr.Text = "Order No already exists, please Re-enter Order No";
                            ttOrder.Focus();
                            ScrollToControl("ctl00_maincontent_Level4_Payment", true);
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

                PayOkDiv.Visible = false;

                //PayType.Visible = false;

                string rtnstr = "";
                try
                {
                    txtCardNumber.Style.Remove("border");
                    drpExpmonth.Style.Remove("border");
                    drpExpyear.Style.Remove("border");
                    txtCardCVVNumber.Style.Remove("border");
                    //drppaymentmethod.Style.Remove("border");
                }
                catch (Exception ex)
                {

                }

              
               // UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

                try
                {

                 


                    if (Session["XpayMS"] != null)
                    {
                        if (Convert.ToInt32(Session["XpayMS"]) > 3)
                        {
                            // div1.InnerHtml = "";
                            div2.InnerHtml = "More than 5 attempt. try again" + "<br/>";
                                //+ "<a href=\"ExpressCheckout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
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
                    objErrorHandler.CreateOrderSummarylog("PaymentID : " + PaymentID + "orderid : " + OrderID);
                    //if (PaymentID == 0)
                    //    Response.Redirect("OrderDetailsExpress.aspx?bulkorder=1&Pid=0&ORDER_ID=" + OrderID);
                   
                   
                    if (oPayInfo.PayResponse.ToLower() == "yes")
                    {
                        objErrorHandler.CreateOrderSummarylog("Already payment has been made");
                        divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                        return;
                    }

                    //objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, drppaymentmethod.SelectedValue, txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
                    objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
                    objErrorHandler.CreateOrderSummarylog(objPRInfo.Error_Text);
                    if (objPRInfo.Error_Text != "")
                    {
                        btnSP.Style.Add("display", "block");
                        BtnProgressSP.Style.Add("display", "none");
                        // ImgBtnEditShipping.Style.Add("display", "block");

                        div2.InnerHtml = "Error found in details you have entered. Please check all fields for errors and try again."; //objPRInfo.Error_Text;
                        div2.Visible = true;
                        ImagePaySP.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+ "images/master_check.png";
                        ImagePay.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()+ "images/paypal_uncheck.png";


                        ImagePaySP.Visible = true;
                        ImagePay.Visible = true;
                        //btnSP.Focus(); 
                        ScrollToControl("ctl00_maincontent_Level4_Payment", true);
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
                        PayType.Visible = true;
                        // btnSP.Focus(); 
                        ScrollToControl("ctl00_maincontent_Level4_Payment", true);
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
                        HttpContext.Current.Session["Mchkout"] = EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                        Response.Redirect("ExpressCheckout.aspx?key=" + EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid"));
                    }



                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }

            //}
            //else
            //{
            //    objErrorHandler.CreateLog("QTEEMPTY" + "OrderID" + OrderID + "Userid" + Userid);
            //    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", false);
            //}
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
            //objErrorHandler.CreatePayLog("btnPayApi_Click start Orderid=" + OrderID);

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

            //objErrorHandler.CreatePayLog("btnPayApi_Click end Orderid=" + OrderID);

        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            objErrorHandler.CreateOrderSummarylog("btnPay_Click start Orderid=" + OrderID);
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            //if (OrderID == 0)
            //{
            //    if (HttpContext.Current.Session["ORDER_ID"] != null)
            //    {
            //        OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
            //    }
            //}
            //decimal OrdtotCost = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));


            //if ((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0 && OrdtotCost > 0))
            //{

            if (OrderID == 0)
            {
                if (HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                }
            }
            //decimal OrdtotCost = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));


            //if ((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0 && OrdtotCost > 0))
            //{
         
         
            oOrderInfo = objOrderServices.GetOrder(OrderID);
            UserServices objUserServices = new UserServices();
            GetParams();
            objErrorHandler.CreateOrderSummarylog("inside Paypal");
          
            if (oOrderInfo.TotalAmount.ToString() != lblpaypaltotamt.Text)
            {
                objErrorHandler.CreateOrderSummarylog("TotalAmount: " + oOrderInfo.TotalAmount + " " + "lblAmount : " + lblAmount.Text);
                BtnL4EditShippingMethod_Click(sender, e);
                return;
            }

            if (oOrderInfo.ProdTotalPrice == 0)
            {
                objErrorHandler.CreateOrderSummarylog("btnpaypal start Orderid= " + OrderID + "ProdTotalPrice" + "0");
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
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
                BtnL4EditShippingMethod_Click(sender, e);
                return;
            }
            
            if (ttOrder.Text == "" && HttpContext.Current.Session["ORDER_ID"]!=null)
            {
                //ttOrder.Text = "WAG" + HttpContext.Current.Session["ORDER_ID"].ToString();

                oPayInfo.PORelease = "WAG" + HttpContext.Current.Session["ORDER_ID"].ToString();
                oPayInfo.OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                objPaymentServices.UpdatePaymentExpress(oPayInfo);
            }
            else if (ttOrder.Text !="")
            {

                refid = objHelperServices.CS(ttOrder.Text);
                try
                {
                     string sSQL = string.Format("EXEC STP_TBWC_CHECK_PO_NUMBER_EXIST '" + HttpContext.Current.Session["ORDER_ID"].ToString() + "','" + HttpContext.Current.Session["USER_ID"].ToString() + "','" + refid + "'");
                                     objErrorHandler.CreateLog("paypal" + " " + sSQL); 
                    DataTable   DS = objHelperDB.GetDataTableDB(sSQL);
                
                   
                            if (Convert.ToInt32(DS.Rows[0][0]) > 0)
                            {
                            txterr.Text = "Order No already exists, please Re-enter Order No";
                            ttOrder.Focus();
                            SecurePayAcc.Style.Add("display", "none");
                            ImagePay.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                            ImagePaySP.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                            PayPaypalAcc.Style.Add("display", "block");
                            ScrollToControl("ctl00_maincontent_Level4_Payment", false);

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

            PayOkDiv.Visible = false;

           // PayType.Visible = false;
SecurePayAcc.Style.Add("display","none");
            GetParams();
         

            //UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();


            renUrl = renUrl.Replace("MCheckOut", "MCheckOut");
            renUrl = renUrl + "?key=" + EncryptSP("Paid");

           
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

            //
           // string Requeststr = objPayPalService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);
            objErrorHandler.CreateOrderSummarylog("Before PayPalInitRequest_ExpressCheckout " +"OrderID : " +OrderID +"PaymentID : "+PaymentID + "renUrl : "+ renUrl);
            string Requeststr = objPayPalService.PayPalInitRequest_ExpressCheckout(OrderID, PaymentID, oOrderInfo, renUrl);

            if (Requeststr.Contains("Form") == false)
            {
                objErrorHandler.CreateLog(Requeststr + "to open new page");
                divContent.InnerHtml = Requeststr;
            }
            else
            {
                objErrorHandler.CreateLog(Requeststr +"to open new page");
                this.Page.Controls.Add(new LiteralControl(Requeststr));
            }

            btnPay.Visible = false;
            BtnProgress.Visible = true;
            // }
            //else
            //{
            //    objErrorHandler.CreateLog("QTEEMPTY" + "OrderID" + OrderID + "Userid" + Userid);
            //    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", false);
            //}
            //objErrorHandler.CreatePayLog("btnPay_Click End Orderid=" + OrderID);
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

        private void calctotalcost_shipping(decimal ProdTotalPrice)
        {
            decimal ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
            decimal totnor = ProdTotalPrice + ShippingValue;

            decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
            decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
            RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
            lblAmount.Text = (ProdTotalPrice + ShippingValue + RetTax_nor).ToString();
            lblpaypaltotamt.Text = lblAmount.Text;
        }
        private void calctotalcost(decimal ProdTotalPrice)
        {
            //decimal ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
            decimal totnor = ProdTotalPrice;

            decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
            decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
            RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
            lblAmount.Text = (ProdTotalPrice + RetTax_nor).ToString();
            lblpaypaltotamt.Text = lblAmount.Text;
        }
        public void LoadData()
        {
            try
            {
                int UseID=0;
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
                           
                           // ouserinfo.UserID = ouserinfo;
                            if (ExpressLevel == "Start")
                            {

                               

                                Level1.Visible = false;
                                Level3.Visible = false;
                                Level4.Visible = false;

                                Level2.Visible = true;
                                BtnL2Continue.Focus();
                                ScrollToControl("ctl00_maincontent_l2div", false);
                                L2name.Text = ouserinfo.Contact;
                                L2Email.Text = ouserinfo.AlternateEmail;
                                L2Phone.Text = ouserinfo.Phone;





                                //shipping
                                txtComname.Text = ouserinfo.COMPANY_NAME;
                                //if (txt_attnto.Text == "" )
                                //{
                                //    txt_attnto.Text = ouserinfo.Contact;
                                //}
                                 if (ouserinfo.Receiver_Contact == "")
                                {
                                    txt_attnto.Text = ouserinfo.Contact;
                                }
                                else
                                {
                                    txt_attnto.Text = ouserinfo.Receiver_Contact;
                                }
                               // txtABN.Text = "";
                                txtsadd.Text = ouserinfo.ShipAddress1;
                                txtadd2.Text = ouserinfo.ShipAddress2;
                                txttown.Text = ouserinfo.ShipCity;
                      //          txtMobilePhone1.Text = ouserinfo.MobilePhone;
                                drpCountry.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.ShipCountry);

                                if (ouserinfo.ShipCountry.ToLower() == "australia")
                                {
                                    drpstate1.Visible = true;
                                    // drpstate1.Text = ouserinfo.ShipState;
                                    txtzip.Visible = true;
                                    txtzip.Text = ouserinfo.ShipZip;
                                    txtzip_inter.Visible = false;
                                    txtstate.Visible = false;
                                    rfvstate.Enabled = false;
                                    aucust.Visible = true;
                                    intercust.Visible = false;
                                    Setdrpdownlistvalue(drpstate1, ouserinfo.ShipState.ToString());
                                }
                                else
                                {
                                    txtzip.Visible = false;
                                    txtzip_inter.Visible = true;
                                    drpstate1.Visible = false;
                                    txtzip_inter.Text = ouserinfo.ShipZip;
                                    txtstate.Visible = true;
                                    txtstate.Text = ouserinfo.ShipState;
                                    rfvddlstate.Enabled = false;
                                    aucust.Visible = false;
                                    intercust.Visible = true;
                                }

                                //drpCountry.Text = ouserinfo.ShipCountry;

                                //drpCountry.SelectedValue = ouserinfo.ShipCountry;
                                //Setdrpdownlistvalue(drpShipCountry, oUserinfo.ShipCountry.ToString());



                                //Billing
                                txtsadd_Bill.Text = ouserinfo.BillAddress1;
                                txtadd2_Bill.Text = ouserinfo.BillAddress2;
                                txttown_Bill.Text = ouserinfo.BillCity;

                                drpcountry_bill.SelectedValue = objUserServices.GetUserCountryCode(ouserinfo.BillCountry);

                                if (ouserinfo.BillCountry.ToLower() == "australia")
                                {
                                    // drpstate2.Text = ouserinfo.BillState;
                                    drpstate2.Visible = true;
                                    txtzip_bill.Text = ouserinfo.BillZip;
                                    txtstate_Bill.Visible = false;
                                    txtstate_Bill.Style.Add("display", "none"); 
                                    RequiredFieldValidator14.Enabled = false;

                                    Setdrpdownlistvalue(drpstate2, ouserinfo.BillState.ToString());
                                }
                                else
                                {
                                    txtstate_Bill.Visible = true;
                                    txtstate_Bill.Text = ouserinfo.BillState;
                                    txtzip_bill.Text = ouserinfo.BillZip;
                                    drpstate2.Visible = false;
                                    RequiredFieldValidator13.Enabled = false;
                                }

                              

                                //if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower())
                                    if (ouserinfo.BillCountry.ToLower() == ouserinfo.ShipCountry.ToLower() && ouserinfo.BillZip == ouserinfo.ShipZip && ouserinfo.BillState == ouserinfo.ShipState && ouserinfo.BillAddress1 == ouserinfo.ShipAddress1)
                                {
                                    ChkBillingAdd.Checked = true;
                                    L2DivBilling.Visible = false;
                                }
                                else
                                {

                                    ChkBillingAdd.Checked = false;
                                    L2DivBilling.Visible = true;
                                }



                                if (drpCountry.SelectedValue == "AU")
                                {
                                    if (Page.IsPostBack)
                                    {
                                        drpSM1.Items.Clear();
                                        drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                        drpSM1.Items.Add(new ListItem("Standard Shipping", "Standard Shipping"));
                                        drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Counter Pickup"));
                                       
                                        drpSM1.SelectedIndex = 0;
                                        intorder.Visible = false;
                                    }
                                }
                                else
                                {
                                    drpSM1.Items.Clear();
                                    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                    drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                                    drpSM1.SelectedIndex = 1;
                                    intorder.Visible = true;
                                }


                                Level1.Visible = false;
                                Level3.Visible = false;
                                Level4.Visible = false;

                                letstartregister.Visible = false;
                                startdivpwd.Visible = false;
                                getstartwelcome.Visible = false;
                                BtnGetStartLogin.Visible = false;
                                letstartdivlogin.Visible = false;
                                startdivpwd.Visible = false;
                                getstartwelcome.Visible = true;
                                BtnGetStartLogin.Visible = false;
                                letstartregister.Visible = false;
                                BtnGetStartContinue.Visible = false;
                                BtnL2Continue.Focus();
                                ScrollToControl("ctl00_maincontent_l2div", false);
                               // ScrollToControl("ctl00_maincontent_l2div");
                            }
                            else if (ExpressLevel == "Start_Reg")
                            {
                                letstartregister.Visible = true;
                                startdivpwd.Visible = false;
                                getstartwelcome.Visible = false;
                                BtnGetStartLogin.Visible = false;
                                letstartdivlogin.Visible = false;
                                Level2.Visible = false;
                                Level3.Visible = false;
                                Level4.Visible = false;

                                txtregemail.Text = letstarttxtemail.Text;
                                txtregemail.ReadOnly = true;

                            }
                            else if (ExpressLevel == "2Compl")
                            {
                                    Level1.Visible = false;
                                    Level2.Visible = false;

                                    L3Name.Text = ouserinfo.Contact;
                                    L3Email.Text = ouserinfo.AlternateEmail;
                                    L3Phone.Text = ouserinfo.Phone;
                                    L3ship_company_name.Text = ouserinfo.COMPANY_NAME;

                                    //                    if (ouserinfo.MobilePhone != null && ouserinfo.MobilePhone != "")
                                    //                    {
                                    //                        lblorderready.Text = ouserinfo.MobilePhone;
                                    ////                        txtMobileNumber.Text = ouserinfo.MobilePhone;
                                    //                    }
                                    //else
                                    //{
                                    //    hfphonenumber.Value = ouserinfo.ShipPhone;
                                    //    lblorderready.Text = ouserinfo.ShipPhone;
                                    //}


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
                                    if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null && oOrdInfo.ShipPhone.Substring(0, 2) == "04" && oOrdInfo.ShipPhone.ToString().Trim().Length == 10)
                                    {
                                        lblorderready.Text = oOrdInfo.ShipPhone;
                                    }
                                    else if (ouserinfo.MobilePhone != null && ouserinfo.MobilePhone != "" && ouserinfo.MobilePhone.Substring(0, 2) == "04" && ouserinfo.MobilePhone.ToString().Trim().Length == 10)
                                    {
                                        hfphonenumber.Value = ouserinfo.MobilePhone;
                                        if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.")
                                        {
                                            lblorderready.Text = ouserinfo.MobilePhone;
                                        }
                                    }
                                    else if (ouserinfo.ShipPhone != null && ouserinfo.ShipPhone != "" && ouserinfo.ShipPhone.Substring(0, 2) == "04" && ouserinfo.ShipPhone.ToString().Trim().Length == 10)
                                    {
                                        hfphonenumber.Value = ouserinfo.ShipPhone;
                                        if (lblorderreadytext.Text != "SMS Order ready notification will NOT be sent.")
                                        {
                                            lblorderready.Text = ouserinfo.ShipPhone;
                                        }
                                    }
                                    else
                                    {
                                        hfphonenumber.Value = ouserinfo.MobilePhone;
                                    }
                                    if (hfchange.Value == "0")
                                    {
                                        txtchangemobilenumber.Text = lblorderready.Text;
                                    }

                                    //if (objOrderServices.IsNativeCountry(ordID) == 0)
                                    if (oOrdShippInfo.ShipAddress1 != "")
                                    {
                                        Level3.Visible = true;
                                        ScrollToControl("ctl00_maincontent_divl3continue", true);
                                        Level4.Visible = false;
                                    }
                                    else
                                    {

                                        //Level2.Visible = true;
                                        //BtnL2Continue.Focus();
                                        //Level3.Visible = false;
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
                                            intorder.Visible = false;
                                        }

                                    }
                                    else
                                    {
                                        drpSM1.Items.Clear();
                                        drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                                        drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                                        drpSM1.SelectedIndex = 1;
                                        intorder.Visible = true;
                                    }
                                    // ScrollToControl("ctl00_maincontent_divl3continue");
                                
                            }
                            else if (ExpressLevel == "3Compl")
                            {

                                int ordID = 0;
                                ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());

                                if (objOrderServices.IsNativeCountry_Express(ordID) == 1)
                                {
                                    Level1.Visible = false;
                                    Level2.Visible = false;
                                    Level3.Visible = false;
                                    Level4.Visible = true;


                                    Level1.Visible = false;
                                    Level2.Visible = false;
                                    Level3.Visible = false;
                                 
                                    Level4_Submit.Visible = false;

                                    divOk.Visible = false;
                                    PayType.Visible = true;

                                    // UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                                    // ouserinfo = objUserServices.GetUserInfo(Userid);

                                    L4name.Text = ouserinfo.Contact;
                                    L4Email.Text = ouserinfo.AlternateEmail;
                                    L4Phone.Text = ouserinfo.Phone;
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
                                    //ttOrder.Text = oPayInfo.PORelease;
                                    //lblAmount.Text = oOrdInfo.TotalAmount.ToString();
                                    //lblpaypaltotamt.Text = oOrdInfo.TotalAmount.ToString();
                                   

                                    div2.Visible = false;


                                    if (lblShippingMethod.Text == "Standard Shipping")
                                    {
                                        divshopcounter.Visible = false;
                                        calctotalcost_shipping(oOrdInfo.ProdTotalPrice);
                                      
                                        objErrorHandler.CreatePayLog_Final("after calculation load data" +  lblAmount.Text);
                                  
                                    }
                                    else
                                    {
                                        calctotalcost(oOrdInfo.ProdTotalPrice);
                                        divshopcounter.Visible = true;
                                    }
                                    if (Convert.ToDecimal(lblAmount.Text) > 300)
                                    {
                                        ImagePay.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                                        // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                                        ImagePaySP.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                                        btnPayApi.Visible = false;
                                        btnPay.Visible = true;
                                        //PaySPDiv.Visible = false;
                                        PaypalApiDiv.Visible = true;

                                        PayPaypalAcc.Visible = true;
                                        PayPaypalAcc.Style.Add("display", "block");

                                        SecurePayAcc.Visible = false;
                                      // btnPay.Focus();
                                        ScrollToControl("ctl00_maincontent_Level4_Payment",true);
                                    }
                                    else
                                    {
                                      // btnSP.Focus();
                                        ScrollToControl("ctl00_maincontent_Level4_Payment",true);
                                    }
                                }
                                else
                                {
                                    Level1.Visible = false;
                                    Level2.Visible = false;
                                    Level3.Visible = false;
                                    Level4.Visible = true;


                                    L4name.Text = ouserinfo.Contact;
                                    L4Email.Text = ouserinfo.AlternateEmail;
                                    L4Phone.Text = ouserinfo.Phone;
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
                                    div2.Visible = false;
                                    divshopcounter.Visible = false;

                                    ImageButton1.Visible = false;
                                    divshopcounter.Visible = false;
                                    Level4_Payment.Visible = false;
                                    Level4_Submit.Visible = true;
                                    PHOrderConfirm.Visible = true;
                                    checkoutrightL1.Visible = false;
                                    checkoutrightL4.Visible = true;

                                    if (oOrdInfo.TotalAmount > 300)
                                    {
                                        ImagePay.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/paypal_check.png";
                                        // ImagePayApi.ImageUrl = "/images/express_Checkout.png";
                                        ImagePaySP.ImageUrl =System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/master_uncheck.png";
                                        btnPayApi.Visible = false;
                                        btnPay.Visible = true;
                                        //PaySPDiv.Visible = false;
                                        PaypalApiDiv.Visible = true;
                                        PayPaypalAcc.Visible = true;
                                        PayPaypalAcc.Style.Add("display", "block");
                                        SecurePayAcc.Visible = false;
                                       //btnPay.Focus();
                                        ScrollToControl("ctl00_maincontent_Level4_Payment",true);
                                    }
                                    else
                                    {
                                        //btnSP.Focus();
                                       ScrollToControl("ctl00_maincontent_Level4_Payment",true);
                                    }
                                    

                                }
                                PayType.Visible = true;
                                
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
                                letstartregister.Visible = true;
                                startdivpwd.Visible = false;
                                getstartwelcome.Visible = false;
                                BtnGetStartLogin.Visible = false;
                                letstartdivlogin.Visible = false;
                                Level2.Visible = false;
                                Level3.Visible = false;
                                Level4.Visible = false;

                            }
                    }

                }
                //Session["ExpressLevel"] 
            }
            catch(Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
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

        #endregion
    }
