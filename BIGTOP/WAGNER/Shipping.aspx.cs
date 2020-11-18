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
public partial class shipping : System.Web.UI.Page
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

    int OrderID = 0;
    int QuoteID = 0;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        int DuplicateItem_Prod_idCount = 0;
        string LeaveDuplicateProds = GetLeaveDuplicateProducts();
        String sessionId;
        sessionId = Session.SessionID;
        int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;
        Userid = objHelperServices.CI(Session["USER_ID"]);
        if (string.IsNullOrEmpty(Request["OrderID"]))
        {
            OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
        }
        else
        {
            Session["ORDER_ID"] = System.Convert.ToInt32(Request["OrderID"]);
            OrderID = System.Convert.ToInt32(Request["OrderID"]);
        }
        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0))
        {
            OrderID = Convert.ToInt32(Session["ORDER_ID"]);
        }
        else
        {
            OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);

        }
        string orderdstatus = objOrderServices.GetOrderStatus(OrderID);
        if (orderdstatus != Enum.GetName(typeof(OrderServices.OrderStatus), tmpOrdStatus))
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


        this.ModalPanel.Visible = false;
        this.modalPop.Hide();

        this.HiddenpopuploginregPanel.Visible = false;
        this.modalPop_loreg.Hide();

        this.Panelerroritems.Visible = false;
        this.modalpop_itemerror.Hide();


        if (tbErrorItems != null && Session["USER_NAME"] == "" && tbErrorItems.Rows.Count > 0 || tbErrorChk != null && tbErrorChk.Rows.Count > 0 || tbErrorReplace != null && tbErrorReplace.Rows.Count > 0)
        {
            var page = (Page)HttpContext.Current.Handler;
            page.Title = "WAGNER Alert";
            this.Panelerroritems.Visible = true;
            modalpop_itemerror.ID = "popUp";
            modalpop_itemerror.PopupControlID = "Panelerroritems";
            modalpop_itemerror.BackgroundCssClass = "modalBackground";
            modalpop_itemerror.TargetControlID = "btnitemerrors";
            modalpop_itemerror.DropShadow = true;
           // modalpop_itemerror.CancelControlID = "btnCancelerroritems";
            this.Panelerroritems.Controls.Add(modalpop_itemerror);
            this.modalpop_itemerror.Show();
            return;
            //ShowPopUpMessage();
            //return;
        }


        if (Session["USER_NAME"] == null || Session["USER_NAME"] == "")
        {
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
                page.Title = "WAGNER Alert";
                this.ModalPanel.Visible = true;
                modalPop.ID = "popUp";
                modalPop.PopupControlID = "ModalPanel";
                modalPop.BackgroundCssClass = "modalBackgroundshi";
                modalPop.DropShadow = false;
                modalPop.TargetControlID = "btnHiddenTestPopupExtender";
                this.ModalPanel.Controls.Add(modalPop);
                this.modalPop.Show();
                return;
            }

        }

        if (!(Session["PageUrl"].ToString().Contains("Confirm.aspx")))
        {
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
                            if (string.IsNullOrEmpty(Request["OrderID"]))
                            {
                                OrderID = objOrderServices.GetOrderID(Userid, OrdStatus);
                            }
                            else
                            {
                                if (System.Convert.ToInt32(Request["OrderID"]) == 0)
                                {
                                   
                                    OrderID = objOrderServices.GetOrderID(Userid, OrdStatus);
                                    Session["ORDER_ID"] = OrderID;
                                }
                                else
                                {
                                    Session["ORDER_ID"] = System.Convert.ToInt32(Request["OrderID"]);
                                    OrderID = System.Convert.ToInt32(Request["OrderID"]);
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
                                // tt1.Enabled = false;
                                string txtadd = "";
                                LoadCountryList();
                                Ta2.Value = LoadShippingInfostr(Session["USER_ID"].ToString());
                                Ta3.Value = LoadBillInfostr(Session["USER_ID"].ToString());
                                Ta4.Value = LoadBillInfostr(Session["USER_ID"].ToString());

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
                                Load_UserRole(Session["USER_ID"].ToString());
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
                            Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"], true);
                        }
                        else
                        {
                            Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", true);
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
            Load_UserRole(Session["USER_ID"].ToString());
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
            tt1.Attributes.Add("onchange", "return isAlphabetic();");
            tt1.Attributes.Add("onblur", "return isAlphabetic();");
        }
        else
        {
            Response.Redirect("/Home.aspx", false);
        }



        if (objOrderServices.IsUserCanDropShip(objHelperServices.CI(Session["USER_ID"])))  // Drop shipment available as per user role
        {
            drpSM1.Items.Add(new ListItem("Drop Shipment Order", "Drop Shipment Order"));
        }



        if (IsPostBack == false && objOrderServices.IsNativeCountry(Convert.ToInt32(Request.QueryString["OrderID"])) == 0) // is other then au
        {
            drpSM1.Items.Clear();
            drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));             
            drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
            drpSM1.SelectedIndex = 1; 
            liPayOption.Visible = false;
            liFinalReview.Visible = false;
            ImageButton2.Text = "Submit Order";
        }
        if (Request.QueryString["ApproveOrder"] != null && IsPostBack == false)
        {
            GetApproveOrderDetails(Convert.ToInt32(Request.QueryString["OrderID"]));
        }
        else
        {
            SetSessionVlaue();
        }
        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:HidePanels();", true);

        drpSM1.Attributes.Add("onclick", "javascript:CheckShippment();");
        drpSM1.Attributes.Add("onchange", "javascript:CheckShippment();");
        // ImageButton4.Attributes.Add("onchange", "javascript:DRPshippment();");


    }
    private void ShowPopUpMessage()
    {
        var page = (Page)HttpContext.Current.Handler;
        page.Title = "WAGNER Alert";
        this.Panelerroritems.Visible = true;
        modalpop_itemerror.ID = "popUp";
        modalpop_itemerror.PopupControlID = "Panelerroritems";
        modalpop_itemerror.BackgroundCssClass = "modalBackground";
        modalpop_itemerror.TargetControlID = "btnitemerrors";
        modalpop_itemerror.DropShadow = true;
        modalpop_itemerror.CancelControlID = "btnCancel";
        this.Panelerroritems.Controls.Add(modalpop_itemerror);
        this.modalpop_itemerror.Show();
        return;
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
            str = "DELIVERY TO : ";
            cmpName = objUserServices.GetCompanyName(_UserID);
            if (cmpName != "")
                str = str + "\n\n" + cmpName;
            else
                str = str + "\n" + cmpName;

            str = str + (string.IsNullOrEmpty(oOrdShippInfo.FirstName.Trim()) ? "" : "\n" + oOrdShippInfo.FirstName.Trim());
            if (oOrdShippInfo.MiddleName != "")
            {
                str = str + (string.IsNullOrEmpty(oOrdShippInfo.MiddleName.Trim()) ? "" : " " + oOrdShippInfo.MiddleName.Trim());
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
            str = "BILL TO : ";
            if (strcmpName != "")
                str = str + "\n\n" + strcmpName;
            else
                str = str + "\n";
            //str = str + (string.IsNullOrEmpty(oOrdBillInfo.FirstName.Trim()) ? "" : "\n" + oOrdBillInfo.FirstName.Trim());
            //str = str + (string.IsNullOrEmpty(oOrdBillInfo.MiddleName.Trim()) ? "" : " " + oOrdBillInfo.MiddleName.Trim());
            //str = str + (string.IsNullOrEmpty(oOrdBillInfo.LastName.Trim()) ? "" : " " + oOrdBillInfo.LastName.Trim());
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
                tt1.Text = string.IsNullOrEmpty(row["PO_RELEASE"].ToString()) ? "" : row["PO_RELEASE"].ToString();

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
                        tt1.Enabled = false;
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
            if (Request["OrderId"] != null)
                OrderID = objHelperServices.CI(Request["OrderId"].ToString());
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
                tt1.Text = Session["ORDER_NO"].ToString();
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
            Session["ORDER_NO"] = tt1.Text.Trim();
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
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"], false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", false);
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
            Session["ORDER_NO"] = tt1.Text.Trim();
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
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"], false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", false);
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
           // decimal TaxAmount;
            decimal ProdTotCost;
            if (string.IsNullOrEmpty(Request["OrderID"]))
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            else OrderID = Convert.ToInt32(Request["OrderID"].ToString());
            int oldorderID = OrderID;
            int QuoteId = 0;
            QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
            if (tt1.Text == "")
                tt1.Text="WAG" + OrderID.ToString();

            refid = objHelperServices.CS(tt1.Text);

             
            if (OrderID <= 0 || tt1.Text.Length < 1)
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
                    DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);

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
             //   TaxAmount = objOrderServices.CalculateTaxAmount(ProdTotCost, OrderID.ToString());
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

                string strHostName = System.Net.Dns.GetHostName();
                string clientIPAddress = "";
                if (System.Net.Dns.GetHostAddresses(strHostName) != null)
                {
                    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    else
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                }


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
                        tt1.Enabled = false;
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
                        {   //if (DefaultPay=="PPAPI")
                            //    Response.Redirect("ppapi.aspx?" + EncryptSP(OrderID.ToString()), false); 
                            //else if (DefaultPay=="PP")
                            //    Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false); 
                            //else
                            SecurePayService objSecurePayService=new SecurePayService();
                                if (objSecurePayService.CheckSecurePay()==true ) 
                                    Response.Redirect("PaySP.aspx?" + EncryptSP(OrderID.ToString()), false);
                                else
                                    Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false); 
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
    protected void ProceedFunction()
    {
        try
        {
            //color.BgColor = "FFFFFF";
            //  color1.BgColor = "FFFFFF";
           // colo2.BgColor = "FFFFFF";
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



    private void SendMail(int OrderId, int OrderStatus ,bool isau)
    {
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


            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string PendingorderURL = string.Format("https://" + url + "/PendingOrder.aspx");

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
                if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                {
                    MessageObj.To.Add(Emailadd.ToString());
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
        OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
        oOI = objOrderServices.GetOrder(OrderID);

        if (oOI.ShipCompName.Trim().Length > 0)
            sShippingAddress = oOI.ShipCompName + "<BR>";
        else
            sShippingAddress = "";

        sShippingAddress = sShippingAddress + oOI.ShipFName + oOI.ShipLName + "<BR>";
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

        return sShippingAddress;
    }

    public string GetBillingAddress(int OrderID)
    {
        string sBillingAddress = "";
        OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
        oBI = objOrderServices.GetOrder(OrderID);
        if (oBI.BillcompanyName.Trim().Length > 0)
            sBillingAddress = oBI.BillcompanyName + "<BR>";
        else
            sBillingAddress = "";

        //sBillingAddress = sBillingAddress + oBI.BillFName + oBI.BillLName + "<BR>";
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
        this.modalPop.Hide();
        Response.Redirect("/Home.aspx");
    }

    protected void btnCancelerroritems_Click(object sender, EventArgs e)
    {

        this.modalpop_itemerror.Hide();
        Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"], true);
    }
    protected void loginregrdir_Click(object sender, EventArgs e)
    {
        this.modalPop_loreg.Hide();
        Response.Redirect("/Login.aspx");
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

   

}
