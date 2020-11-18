using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Services ; 
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.HtmlControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
//Modified by:Indu
//Modified on:11-Apr-2013
//Mofified reason:To Build encrption logic
public partial class UC_login : System.Web.UI.UserControl
{
  
    #region "Declarations..."

    UserServices objUserServices = new UserServices();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    CountryServices objCountryServices = new CountryServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    Security objcrpengine = new Security();
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    UserServices.RegistrationInfo oRegInfo = new UserServices.RegistrationInfo();
    NotificationServices objNotificationServices = new NotificationServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    Security objSecurity = new Security();
    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    private static int _HitCount;
    private static string sReferralURL;
    MailChimpManagerServices ObjMailChimService = new MailChimpManagerServices();
    int cOrderID = 0;
    int cUserID = 0;
    bool redirectflag = false;
    #endregion

    #region "Events..."

    public static bool IsDate(Object obj)
    {
        if (obj != null)
        {
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if (dt >= DateTime.Now)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!Page.IsPostBack)
        {
            chkShopCart.Checked = true;
            //txtcompname.Focus();
            LoadCountryList();
            intercust.Visible = false;
            string countryCode = drpCountry.SelectedValue.ToString();
            LoadStates(drpCountry.SelectedValue);
            ErrorStatusHiddenField.Value = "0";
           // cVerifyMsg.Visible = false;
            chkterms.Checked = false;
            this.ModalPanelRRAE.Visible = false;
            this.modalPop.Hide();

            if (Request.Cookies["BigtopLoginInfo"] != null && Request.Cookies["BigtopLoginInfo"].Value.ToString().Trim() != "")
            {
                HttpCookie LoginInfoCookie = Request.Cookies["BigtopLoginInfo"];

                txtUserName.Text = objSecurity.StringDeCrypt_password(LoginInfoCookie["UserName"].ToString());
                chkKeepme.Checked = true;
                 if (IsDate(LoginInfoCookie["Expires"]))
                 {
                      hidpwd.Value = objSecurity.StringDeCrypt(LoginInfoCookie["Password"].ToString());
                 }
                 else
                 {
                     hidpwd.Value = "";
                 }               
                   
            }
            else
            {
                HttpCookie LoginInfoCookie = Request.Cookies["BigtopLoginInfo"];
                if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
                    LoginInfoCookie["Password"] ="";

                chkKeepme.Checked = false;
            }

            chkShopCart.Disabled = true;
        }

        InitLoad();
        txtUserName.Focus();
        if (Request["Result"] == "SUCCESS")
            RegSucess.Visible = true;
        
        //if (Session["FORGOTPWD"] != null && Session["FORGOTPWD"].ToString() != "")
        //{
        //    RegSucess.Text = Session["FORGOTPWD"].ToString();
        //    RegSucess.Visible = true;
        //    Session["FORGOTPWD"] = "";
        //}
        
        //if (Session["RESETPWD"] != null && Session["RESETPWD"].ToString() != "")
        //{
        //    RegSucess.Text = Session["RESETPWD"].ToString();
        //    RegSucess.Visible = true;
        //    Session["RESETPWD"] = "";
        //}
        //
        //Modified by :Indu
        //Message Problem in chrome
        if (Request.QueryString["Result"] != null)
        {
            if (Request.QueryString["Result"] == "FORGOTPWD")
            {
                RegSucess.Text = GetGlobalResourceObject("login", "FORGOTPWD").ToString(); ;
                RegSucess.Visible = true;
                Session["FORGOTPWD"] = "";

            }
            if (Request.QueryString["Result"] == "RESETPWD")
            {
                RegSucess.Text = GetGlobalResourceObject("login", "RESETPWD").ToString(); ;
                RegSucess.Visible = true;
                Session["RESETPWD"] = "";

            }
            if (Request.QueryString["Result"] == "FORGOTUSER")
            {
                RegSucess.Text = GetGlobalResourceObject("login", "FORGOTUSER").ToString(); ;
                RegSucess.Visible = true;
                Session["FORGOTPWD"] = "";

            }
        }
        int i = 1;
        txtUserName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");
        txtPassword.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");

        if (!IsPostBack)
        {
            try
            {
                 sReferralURL = Request.ServerVariables["HTTP_REFERER"].ToString();
            }
            catch (Exception ex)
            {
            }
        }

        if (Session["ForgotAction"] != null && Session["ForgotAction"].ToString().Trim() != string.Empty)
        {
            lblErrMsg.Text = Session["ForgotAction"].ToString();
        }
        else
        {
            lblErrMsg.Text = string.Empty;
        }

        lnkResetPassword.Attributes.Add("onclick", "javascript:ForgotLinkPage();"); 
        lnkForgotPWDPage.Attributes.Add("onclick", "javascript:ForgotLinkPage();");
        //lnkForgotNamePage.Attributes.Add("onclick", "javascript:ForgotLinkPageUserID();");
       // CmdSignuphp.Attributes.Add("onclick", "javascript:CreateanAccount();");

        txtUserName.Attributes.Add("onkeypress", "javascript:return checkUserName(event);");
     // txtPassword.Attributes.Add("onkeypress", "javascript:return check(event);");
       // txtchgPassnew.Text = "";
      //  txtchgCPass.Text = "";
      // CmdSignup.OnClientClick = "fn();return false;";


       txtfname.Attributes.Add("onkeypress", "javascript:return check(event);");
       txtlname.Attributes.Add("onkeypress", "javascript:return check(event);");
       txtemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
       txtcemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
       txtsadd.Attributes.Add("onkeypress", "javascript:return check(event);");
       txtadd2.Attributes.Add("onkeypress", "javascript:return check(event);");
       txttown.Attributes.Add("onkeypress", "javascript:return check(event);");
       txtstate.Attributes.Add("onkeypress", "javascript:return check(event);");
      // Txt1password.Attributes.Add("onkeypress", "javascript:return check(event);");
      // TxtConfirmPassword.Attributes.Add("onkeypress", "javascript:return check(event);");
       ForgotPassword.Attributes.Add("onmouseover", "javascipt:MouseHover(1);");
       ForgotPassword.Attributes.Add("onmouseout", "javascipt:MouseOut(1);");
       Close.Attributes.Add("onmouseover", "javascipt:MouseHover(2);");
       Close.Attributes.Add("onmouseout", "javascipt:MouseOut(2);");
       if (!chkterms.Checked)
       {
           lblCheckTerms.Visible = true;
       }
    }

  

    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        string strMsg = "";
        OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
      
        try
        {
            bool validUser;
            string username;
            string password;
            DataSet tmpds = null;
            int UserID;
            DataSet tmpdsdealercheck = null;
            string WebSiteID = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            string WesUrl = ConfigurationManager.AppSettings["WesUrl"].ToString();


            username = txtUserName.Text;
            password = txtPassword.Text;
           password = objSecurity.StringEnCrypt_password(txtPassword.Text);
         //  tmpdsdealercheck = objUserServices.CheckMultipleUserMail(username);
           //if (tmpdsdealercheck != null && tmpdsdealercheck.Tables.Count > 0 && tmpdsdealercheck.Tables[0].Rows.Count > 0)
           //{
           //    if (tmpdsdealercheck.Tables[0].Rows[0]["CUSTOMER_TYPE"].ToString() == "Dealer")
           //    {
           //        lblErrMsg.Text = "Invalid Login Id";
           //        return;
           //    }
           //}

            if (objHelperServices.ValidateEmail(username) == true)
            {
                tmpds = objUserServices.CheckMultipleUserMail(username);
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    if( tmpds.Tables[0].Rows.Count > 1)
                    {
                        strMsg = GetGlobalResourceObject("login", "ErrorMsg10").ToString();
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = strMsg;
                        RegSucess.Visible = false;                        
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
                WebSiteID = tmpdsdealercheck.Tables[0].Rows[0]["WEBSITE_ID"].ToString();
                if (tmpdsdealercheck.Tables[0].Rows[0]["CUSTOMER_TYPE"].ToString() == "Dealer" && tmpdsdealercheck.Tables[0].Rows[0]["WEBSITE_ID"].ToString()=="5") 
                {
                    lblErrMsg.Text = "Invalid Login Id";
                    return;
                }
                else if (tmpdsdealercheck.Tables[0].Rows[0]["WEBSITE_ID"].ToString()=="1")
                {
                    if (objUserServices.CheckUser(username, password))
                    {
                        Boolean login = true;
                        string querystring = "username=" + objSecurity.StringEnCryptWes(username).Replace("+", "%252b") + "&password=" + objSecurity.StringEnCryptWes(password) + "&login=" + objSecurity.StringEnCryptWes(login.ToString());
                        Response.Redirect(WesUrl + "sessionabo.aspx?" + querystring, false);
                    }
                    else
                    {
                        if (Session["USER"] == null || Session["USER"].ToString() == "")
                        {
                            Session["USER"] = username;
                            Session["COUNT"] = "1";
                        }
                        else
                        {
                            if (Session["USER"].ToString() != username)
                            {
                                Session["USER"] = username;
                                Session["COUNT"] = "1";
                            }
                            else if (Session["USER"].ToString() == username)
                            {
                                Session["COUNT"] = (objHelperServices.CI(Session["COUNT"].ToString()) + 1).ToString();
                            }
                        }
                        //Modified by:indu
                        //Ontime defect:661
                        if (objHelperServices.CI(Session["COUNT"].ToString()) < 6)
                        {

                            if (txtPassword.Text == "")
                            {
                                strMsg = GetGlobalResourceObject("login", "ErrorMsg05").ToString();
                                lblErrMsg.Text = strMsg;
                                RegSucess.Visible = false;
                                txtPassword.Text = "";
                                txtPassword.Focus();

                            }
                            else
                            {
                                strMsg = GetGlobalResourceObject("login", "ErrorMsg01").ToString();
                                lblErrMsg.Text = strMsg;
                                RegSucess.Visible = false;
                                txtPassword.Text = "";
                                txtPassword.Focus();
                                HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                            }
                        }
                        else
                        {
                            objUserServices.LockUser(username);
                            //Modified by:indu
                            strMsg = GetGlobalResourceObject("login", "ErrorMsg02").ToString();
                            lblErrMsg.Text = strMsg;
                            lnkResetPassword.Visible = true;
                            RegSucess.Visible = false;
                            HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                        }
                    }
                }
            }

                validUser = objUserServices.CheckUserName(username);
                UserID = objUserServices.GetUserID(username);
            
            if (UserID != -1 && username != string.Empty && tmpdsdealercheck.Tables[0].Rows[0]["WEBSITE_ID"].ToString()=="5")
            {

                if (objUserServices.GetPassword(UserID).EndsWith("W@9$"))
                {
                    //password = password + "W@9$";
                }
                if (Session["USER_NAME"] != null && Session["USER_NAME"].ToString() == username)
                {
                    strMsg = GetGlobalResourceObject("login", "ErrorMsg07").ToString();

                    lblErrMsg.Text = strMsg;
                    txtUserName.Focus();
                    RegSucess.Visible = false;
                }
                else
                {
                    if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                    {
                        if (validUser == true)
                        {
                            bool HasAdminUser = objUserServices.HasAdmin(UserID);
                            if (objUserServices.IsUserActive(UserID.ToString()))
                            {
                                if (objUserServices.CheckUser(username, password))
                                {
                                  
                                    SetCookie(txtUserName.Text, txtPassword.Text);
                                    
                                    string Role;
                                    Role = objUserServices.GetRole(UserID);

                                    if (Role != null)
                                    {
                                        if (objUserServices.GetUserStatus(UserID) != 4)
                                        {
                                            objUserServices.OnLineFlag(true, UserID);
                                            Session["USER_NAME"] = username;
                                            Session["USER_ID"] = UserID;
                                            Session["USER_ROLE"] = Role;
                                            Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                            Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);
                                           
                                            Session["Emailid"] = tmpdsdealercheck.Tables[0].Rows[0]["EmailAddr"].ToString();
                                            Session["Firstname"] = tmpdsdealercheck.Tables[0].Rows[0]["First_Name"].ToString();
                                            Session["Lastname"] = tmpdsdealercheck.Tables[0].Rows[0]["Last_Name"].ToString();
                                            Session["USER_ID"] = UserID;
                                            Session["DUMMY_FLAG"] = "1";
                                            Session["EXPRESS_CHECKOUT"] = "False";
                                            
                                            
                                            LogSession();
                                        }
                                        if (objUserServices.GetUserStatus(UserID) == 4)
                                        {
                                            //objErrorHandler.CreateLog(UserID.ToString()); 
                                           // string uname = objSecurity.StringEnCry pt(UserID.ToString());
                                           // string uname = objSecurity.StringEnCrypt("60343");
                                           // objErrorHandler.CreateLog(uname);
                                            Session["USER_ID_Ch"] = UserID;
                                           // Response.Redirect("PopChangePwd.aspx?Uname=" + HttpUtility.UrlEncode(uname) + "");  
                                            Response.Redirect("PopChangePwd.aspx",false);  
                                           // ChgPassPop.Show();
                                            //txtchgPassnew.Text = "";
                                           // txtchgCPass.Text = "";
                                            //Response.Redirect("ChangePassword.aspx?ChangePass=True");
                                        }
                                        else if (Session["PageUrl"] != null)
                                        {
                                            if (chkShopCart.Checked == false)
                                            {
                                                {

                                                }
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
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
                                                if (OrderID > 0 && (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
                                                {
                                                    DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderID);
                                                    if (oDSOrderItems != null)
                                                    {
                                                        foreach (DataRow dr in oDSOrderItems.Tables[0].Rows)
                                                        {
                                                            objOrderServices.UpdateQuantity(Convert.ToInt32(dr["PRODUCT_ID"].ToString()), Convert.ToInt32(dr["QTY"].ToString()) + Convert.ToInt32(dr["QTY_AVAIL"].ToString()));
                                                        }
                                                    }
                                                    int chk = cOrderID == 0 ? objOrderServices.RemoveItem("AllProd", OrderID, objHelperServices.CI(Session["USER_ID"]),"") : 0;
                                                    oOrdInfo.OrderID = OrderID;
                                                    oOrdInfo.ProdTotalPrice = 0.00M;
                                                    oOrdInfo.TotalAmount = 0.00M;
                                                    oOrdInfo.TaxAmount = 0.00M;
                                                    oOrdInfo.ShipCost = 0.00M;
                                                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                                }
                                            }
                                            else
                                            {
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                                                cOrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
                                            }

                                            if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=PASSWORDCHANGED"))
                                            {
                                                hlink.NavigateUrl = "MyAccount.aspx";
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                    Response.Redirect("MyAccount.aspx",false);
                                            }
                                            else
                                            {
                                                hlink.NavigateUrl = Session["PageUrl"].ToString();
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                {
                                                    //modified by indu
                                                  //  Response.Redirect(Session["PageUrl"].ToString());
                                                    if (Session["PageUrl"] != null)
                                                    {
                                                        if ((Session["PageUrl"].ToString().ToLower().Contains("checkout.aspx")) && (Session["PageUrl"].ToString().ToLower().Contains("paysp")))
                                                        {
                                                            Response.Redirect(Session["PageUrl"].ToString());
                                                        }
                                                        else
                                                        {
                                                            Response.Redirect("/Home.aspx", false);
                                                      
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Response.Redirect("/Home.aspx", false);
                                                    }
                                                
                                                }

                                            }
                                        }
                                        else
                                        {

                                            if (chkShopCart.Checked == false)
                                            {
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
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
                                                if (OrderID > 0 && (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
                                                {
                                                    DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderID);
                                                    if (oDSOrderItems != null)
                                                    {
                                                        foreach (DataRow dr in oDSOrderItems.Tables[0].Rows)
                                                        {
                                                            objOrderServices.UpdateQuantity(Convert.ToInt32(dr["PRODUCT_ID"].ToString()), Convert.ToInt32(dr["QTY"].ToString()) + Convert.ToInt32(dr["QTY_AVAIL"].ToString()));
                                                        }
                                                    }
                                                    int chk = cOrderID == 0 ? objOrderServices.RemoveItem("AllProd", OrderID, objHelperServices.CI(Session["USER_ID"]),"") : 0;
                                                    objOrderServices.RemoveOrder(OrderID, objHelperServices.CI(Session["USER_ID"]));

                                                    oOrdInfo.OrderID = OrderID;
                                                    oOrdInfo.ProdTotalPrice = 0.00M;
                                                    oOrdInfo.TotalAmount = 0.00M;
                                                    oOrdInfo.TaxAmount = 0.00M;
                                                    oOrdInfo.ShipCost = 0.00M;
                                                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                                }
                                            }
                                            else
                                            {
                                                OrderServices objOrderServices = new OrderServices();
                                                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                                                cOrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
                                            }
                                            if (Request.QueryString["URL"] != null)
                                            {
                                                hlink.NavigateUrl = Request["URL"].ToString();
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                    Response.Redirect(Request["URL"].ToString(),false);
                                            }
                                            else
                                            {
                                                hlink.NavigateUrl = Request["URL"].ToString();
                                                if (!HasAdminUser)
                                                {
                                                    ShowAdminAlert.Show();
                                                }
                                                else
                                                {
                                                    Response.Redirect("/Home.aspx", false);
                                                    
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //For Checking that different users log in to same time..
                                    //Initially it will locked even different users log in with wrong password (3 user)
                                    //3rd userd will be locked in the first time
                                    //To solve this following code is used.....
                                    if (Session["USER"] == null || Session["USER"].ToString() == "")
                                    {
                                        Session["USER"] = username;
                                        Session["COUNT"] = "1";
                                    }
                                    else
                                    {
                                        if (Session["USER"].ToString() != username)
                                        {
                                            Session["USER"] = username;
                                            Session["COUNT"] = "1";
                                        }
                                        else if (Session["USER"].ToString() == username)
                                        {
                                            Session["COUNT"] = (objHelperServices.CI(Session["COUNT"].ToString()) + 1).ToString();
                                        }
                                    }
                                    //Modified by:indu
                                    //Ontime defect:661
                                    if (objHelperServices.CI(Session["COUNT"].ToString()) < 6)
                                    {

                                        if (txtPassword.Text == "")
                                        {
                                            strMsg = GetGlobalResourceObject("login", "ErrorMsg05").ToString();
                                            lblErrMsg.Text = strMsg;
                                            RegSucess.Visible = false;
                                            txtPassword.Text = "";
                                            txtPassword.Focus();

                                        }
                                        else
                                        {
                                            strMsg = GetGlobalResourceObject("login", "ErrorMsg01").ToString();
                                            lblErrMsg.Text = strMsg;
                                            RegSucess.Visible = false;
                                            txtPassword.Text = "";
                                            txtPassword.Focus();
                                            HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                                        }
                                    }
                                    else
                                    {
                                        objUserServices.LockUser(username);
                                        //Modified by:indu
                                        //Ontime defect:661
                                        //strMsg = GetGlobalResourceObject("login", "ErrorMsg021").ToString();
                                        strMsg = GetGlobalResourceObject("login", "ErrorMsg02").ToString();
                                        lblErrMsg.Text = strMsg;
                                        lnkResetPassword.Visible = true; 
                                        RegSucess.Visible = false;
                                        HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                                    }
                                }

                            }
                            else if (objUserServices.GetUserStatus(UserID)== 3) 
                            {
                                strMsg = GetGlobalResourceObject("login", "ErrorMsg02").ToString();
                                lblErrMsg.Text = strMsg;
                                lnkResetPassword.Visible =true ; 
                                RegSucess.Visible = false;
                                txtUserName.Text = "";
                                txtPassword.Text = "";
                                HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                            }
                            else
                            {
                                strMsg = GetGlobalResourceObject("login", "ErrorMsg04").ToString();
                                lblErrMsg.Text = strMsg;
                                lnkResetPassword.Visible = false;
                                RegSucess.Visible = false;
                                txtUserName.Text = "";
                                txtPassword.Text = "";
                                HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                            }
                          


                        }
                        else
                        {
                            strMsg = GetGlobalResourceObject("login", "ErrorMsg03").ToString();
                            lblErrMsg.Text = strMsg;
                            RegSucess.Visible = false;
                            txtUserName.Text = "";
                            txtUserName.Focus();
                            HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                        }
                    }
                    else
                    {
                        if (validUser == false)
                        {
                            strMsg = GetGlobalResourceObject("login", "ErrorMsg03").ToString();
                            lblErrMsg.Text = strMsg;
                            RegSucess.Visible = false;
                            txtUserName.Focus();
                            HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                        }
                        else
                        {
                           // strMsg = GetGlobalResourceObject("login", "ErrorMsg04").ToString();
                           // lblErrMsg.Text = strMsg;
                            lblErrMsg.Text = "Invalid Login Id";
                            txtUserName.Focus();
                            RegSucess.Visible = false;
                            HttpContext.Current.Response.Cookies.Remove("BigtopLoginInfo");
                        }
                    }
                }
            }

            if (UserID == -1 && username == string.Empty)
            {
              //  strMsg = GetGlobalResourceObject("login", "ErrorMsg06").ToString();
                strMsg = GetGlobalResourceObject("login", "rfvUserName").ToString();               
               // lblErrMsg.Text = strMsg;
                lblErrMsg.Text = "Please Enter UserID";
                RegSucess.Visible = false;
                txtUserName.Focus();

            }
            if (txtPassword.Text == "" && password == string.Empty && username != string.Empty)
            {
                strMsg = GetGlobalResourceObject("login", "rfvPassWord").ToString();
                lblErrMsg.Text = strMsg;
                RegSucess.Visible = false;
                txtPassword.Focus();

            }
            if (UserID == -1 && username != string.Empty && password != string.Empty)
            {
                strMsg = GetGlobalResourceObject("login", "ErrorMsg03").ToString();
                lblErrMsg.Text = strMsg;
                RegSucess.Visible = false;
                txtUserName.Focus();
            }


            if (cOrderID > 0)
            {
                OrderServices objOrderServices= new OrderServices();
                if (objOrderServices.GetOrderItemCount(cOrderID) > 0)
                {
                    Session["PrevOrderID"] = cOrderID;
                }
                else
                {
                    Session["PrevOrderID"] = "0";
                }

                Response.Redirect("/Home.aspx",false);
                
            }
            else
            {
                Session["PrevOrderID"] = "0";
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
            strMsg = "";
        }
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script> alert('Confirm password does not match')</script>", true);
    }

    private void SetCookie(string UserName, string Password)
    {
        if (chkKeepme.Checked == true)
        {
            HttpCookie LoginInfoCookie = new HttpCookie("BigtopLoginInfo");
            LoginInfoCookie["UserName"] = objSecurity.StringEnCrypt_password(UserName);
            LoginInfoCookie["Password"] = objSecurity.StringEnCrypt_password(Password);
            LoginInfoCookie["Expires"] = DateTime.Now.AddDays(1).ToString();
            LoginInfoCookie["Login"] = objSecurity.StringEnCrypt_password("True");
            LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.AppendCookie(LoginInfoCookie);

        }
        else
        {
            HttpCookie LoginInfoCookie = Request.Cookies["BigtopLoginInfo"];
            if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
                LoginInfoCookie["Password"] = "";

            Response.Cookies["BigtopLoginInfo"].Expires = DateTime.Now.AddDays(-665);
            hidpwd.Value = "";
        }

    }

    private void SetCookieWes(string UserName, string Password)
    {
        if ((chkKeepme.Checked))
        {
            HttpCookie LoginInfoCookie = new HttpCookie("LoginInfo");
            LoginInfoCookie["UserName"] = objSecurity.StringEnCrypt(UserName);
            LoginInfoCookie["Password"] = objSecurity.StringEnCrypt(Password);
            LoginInfoCookie["Expires"] = DateTime.Now.AddDays(1).ToString();
            LoginInfoCookie["Login"] = objSecurity.StringEnCrypt("True");
            LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
        }
        else
        {
            HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
            if (LoginInfoCookie != null && LoginInfoCookie["Password"] != null)
                LoginInfoCookie["Password"] = "";

            Response.Cookies["LoginInfo"].Expires = DateTime.Now.AddDays(-665);
            hidpwd.Value = string.Empty;
        }

    }
    
    public void LoadCountryList()
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

    //protected void btntest_Click(object sender, EventArgs e)
    //{
    //    MailChimpManagerServices objmcms = new MailChimpManagerServices();
    //      string email = "palani@jtechindia.com";
    //      string dataCenter = "";
    //      string firstname = "palani1551";
    //      string lastname = "kumar1551";
    //      string addr1 = "kjshsakjhddska";
    //    objmcms.CreateMailChimpMember(dataCenter, email, firstname, lastname, addr1,"city","tamilnadu","625019","india");

    //    //  objmcms.UnSubscribeChimpMember("", email);

    //}

    protected void btnsubmit_Click(object sender, EventArgs e)
    {

        try
        {
            //cVerify.ValidateCaptcha(cText.Text);        //Validate Captcha Control Text
            //if (cText.Text.Trim() == "")
            //{
            //    //chkterms.Checked = false;
            //    cText.Text = "";
            //    cVerifyMsg.Text = "Required";
            //    cVerifyMsg.Visible = true;
            //    // ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: checkEnableSubmit(); ", true);
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript: checkEnableSubmit(); ", true);
            //}
            //else if (cVerify.UserValidated)
            //{
            if (!Page.IsValid)
            {
                //chkterms.Checked = false;
               // cVerifyMsg.Visible = false;
               // cText.Text = "";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript: checkEnableSubmit(); ", true);
                return;
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript: checkselectedlist(); ", true);

            if (Convert.ToInt16(ErrorStatusHiddenField.Value) == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Please complete the field What Describes you and/or your company the best!');", true);
             //   cText.Text = "";
             //   cVerifyMsg.Text = "Required";
             //   cVerifyMsg.Visible = true;
                return;
            }

            if (chkterms.Checked == false)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('To Submit form you must agree to the Sales Terms and Conditions');", true);
              //  cText.Text = "";
               // cVerifyMsg.Text = "Required";
               // cVerifyMsg.Visible = true;
                return;
            }
                DataSet tmpds = objUserServices.CheckCustomerRegistrationExists(txtemail.Text.Trim(), "Retailer");

                bool tmp = objUserServices.CheckUserRegisterEmail(txtemail.Text.Trim(), "Retailer");
                if ((tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0))
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Mail address already exists');", true);

                    //return;
                    if (tmp == true)
                    {
                        this.ModalPanelRRAE.Visible = true;
                        modalPop.ID = "popUp";
                        modalPop.PopupControlID = "ModalPanelRRAE";
                        modalPop.BackgroundCssClass = "modalBackground";
                        modalPop.DropShadow = false;
                        modalPop.TargetControlID = "btnHiddenTestPopupExtender";
                        this.ModalPanelRRAE.Controls.Add(modalPop);
                        this.modalPop.Show();
                        return;
                    }

                }
                //if (tmp==true )
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Mail address already exists');", true);
                //    return;
                //}

               // cVerifyMsg.Visible = false;
                int i = SaveUserProfile();
                int issubscribe = chkissubscribe.Checked == true ? 1 : 0;
                //SendNotification();

                //chkterms.Checked = false;
                //btnsubmit.Enabled = false;

                if (i > 0)
                {
                    DataSet dsUser = new DataSet();
                    dsUser = objUserServices.GetUserDateSet(i);


                    ActiveCampaignService objActiveCampaignService = new ActiveCampaignService();
                    if (dsUser != null && dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
                    {
                        objUserServices.SetUserSubscriber(dsUser.Tables[0].Rows[0]["USER_ID"].ToString(), issubscribe);
                        //objActiveCampaignService.SetContact_Subscribe(txtemail.Text, txtfname.Text, txtlname.Text, (issubscribe == 1 ? 1 : 2), false);  

                        try
                        {
                            if (issubscribe == 1)
                            {
                                ObjMailChimService.CreateMailChimpMember("", txtemail.Text, txtfname.Text, txtlname.Text, oRegInfo.Address1, oRegInfo.SubCity, oRegInfo.State, oRegInfo.PostZipcode, oRegInfo.Country);
                               // ObjMailChimService.CreateMailChimpMember("", txtemail.Text, txtfname.Text, txtlname.Text, "", oRegInfo.SubCity, oRegInfo.State, oRegInfo.PostZipcode, oRegInfo.Country);
                            }
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.ErrorMsg = ex;
                            objErrorHandler.CreateLog();
                        }
                       

                        SendNewCustomer(i, Convert.ToInt32(dsUser.Tables[0].Rows[0]["USER_ID"]));



                        Session["USER_NAME"] = dsUser.Tables[0].Rows[0]["LOGIN_NAME"].ToString();
                        Session["USER_ID"] = dsUser.Tables[0].Rows[0]["USER_ID"];
                        Session["USER_ROLE"] = dsUser.Tables[0].Rows[0]["USER_ROLE"];
                        Session["COMPANY_ID"] = dsUser.Tables[0].Rows[0]["COMPANY_ID"];
                        Session["CUSTOMER_TYPE"] = dsUser.Tables[0].Rows[0]["CUSTOMER_TYPE"];
                        Session["DUMMY_FLAG"] = "1";
                        Session["Emailid"] = dsUser.Tables[0].Rows[0]["EmailAddr"].ToString();
                        Session["Firstname"] = dsUser.Tables[0].Rows[0]["First_Name"].ToString();
                        Session["Lastname"] = dsUser.Tables[0].Rows[0]["Last_Name"].ToString();
                        //if (Session["PageUrl"] != null)
                        //{
                        //    Response.Redirect(Session["PageUrl"].ToString(),true);  
                        //}
                        //else
                        //{
                            Response.Redirect("/Home.aspx", true);
                        //}

                    }
                    else
                        Response.Redirect("ConfirmMessage.aspx?Result=REGISTRATION", true);

                }
           // }
            //else
            //{
            //    //cVerify.CustomValidatorErrorMessage = "Invalid Verification Code";
            //    cVerifyMsg.Text = "Invalid Verification Code";
            //    cVerifyMsg.Visible = true;
            //    this.ModalPanelRRAE.Visible = false;
            //    this.modalPop.Hide();
            //    cText.Text = "";
            //    Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: checkEnableSubmit(); ", true);
            //    return;
            //}
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    public int SaveUserProfile()
    {
        try
        {
            oRegInfo.Customer_Type = "Retailer";

            if (txtcompname.Text.Trim().Length > 0)
                oRegInfo.CompanyName = txtcompname.Text.ToString();
            else
                oRegInfo.CompanyName = "";
            if (txtcompno.Text.Trim().Length > 0)
                oRegInfo.AbnAcn = txtcompno.Text.ToString();
            else
                oRegInfo.AbnAcn = "";


            oRegInfo.Address1 = txtsadd.Text.ToString();
            oRegInfo.Address2 = txtadd2.Text.ToString();
            oRegInfo.SubCity = txttown.Text.ToString();
            if (txtstate.Text != "")
            {
                oRegInfo.State = txtstate.Text.ToString();
            }
            else

            {
                oRegInfo.State = drpState.SelectedValue.ToString();   
            }

           
            //oRegInfo.Country = drpCountry.Text.ToString();
            oRegInfo.Country = drpCountry.SelectedItem.ToString();
            if(drpCountry.SelectedItem.ToString().ToLower() == "australia")
             oRegInfo.PostZipcode = txtzip.Text.ToString();
            else
             oRegInfo.PostZipcode = txtzip_inter.Text.ToString();

            oRegInfo.Fname = txtfname.Text.ToString();
            oRegInfo.Lname = txtlname.Text.ToString();
            oRegInfo.Position = "";
            oRegInfo.Phone = txtphone.Text.ToString();
            oRegInfo.Mobile = txtMobile.Text.ToString();
            oRegInfo.Fax = txtfax.Text.ToString();
            oRegInfo.Email = txtemail.Text.ToString();
            //BusinessType = BusinessType + (txtothers.Text.Trim().Length > 0 ? "Others : " + txtothers.Text.ToString() : "");
            oRegInfo.BusinessType = "NA";
            if (RBPersonal.Checked == true)
                oRegInfo.BusinessDsc = "Personal";
            else
                oRegInfo.BusinessDsc = "Business";

            oRegInfo.SiteID = "5";
            oRegInfo.CustStatus = "False";
            oRegInfo.Status = "I";
            oRegInfo.RegType = "N";
            oRegInfo.Password = Txt1password.Text;
            string Newpassword = objcrpengine.StringEnCrypt_password(Txt1password.Text);
            oRegInfo.Password = Newpassword;
            //string strHostName = System.Net.Dns.GetHostName();
            string clientIPAddress = "";
            //if (System.Net.Dns.GetHostAddresses(strHostName) != null)
            //{
            //    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
            //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            //    else
            //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
            //}
            if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
                clientIPAddress = Session["IP_ADDR"].ToString();
            else
                clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

            oRegInfo.IpAddr = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
            //oRegInfo.IpAddr = Request.UserHostAddress.ToString();
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

    private void SendNewCustomer(int reg_id, int user_id)
    {
        try
        {
            string url = HttpContext.Current.Request.Url.Authority.ToString();
            // string activeLink = string.Format("http://" + url + "/Activation.aspx?id={0}", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(user_id.ToString())));
            objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            MessageObj.To.Add(txtemail.Text.ToString());
            string message = "";
            string Topmessage = "";
            string bottommessage = "";
            string strstate = string.Empty;
            if (txtstate.Text == "")
            {
                strstate = drpState.SelectedValue;
            }
            else
            {
                strstate = txtstate.Text;
            }
            MessageObj.Subject = "Bigtop Online Registration Confirmation";
            MessageObj.IsBodyHtml = true;

            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;

            string stemplatepath = Server.MapPath("Templates");
            _stg_container = new StringTemplateGroup("Mail-wagner", stemplatepath);
            _stmpl_container = _stg_container.GetInstanceOf("Mail-wagner" + "\\" + "register");

            _stmpl_container.SetAttribute("FirstLastName", txtfname.Text + " " + txtlname.Text.ToString());

            //Topmessage = Topmessage + "Hi " + txtfname.Text.ToString() + " " + txtlname.Text.ToString();
            //Topmessage = Topmessage + "<br/><br/>";
            //Topmessage = Topmessage + "Thanks for registering with WAGNER online store";
            //Topmessage = Topmessage + "<br/><br/>";
            //// Topmessage = Topmessage + "To get started shopping please confirm your registration by clicking on the activation link below";
            ////Topmessage = Topmessage + "<br/><br/> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\"> <a href=" + activeLink + ">" + activeLink + "</a></font>";
            ////Topmessage = Topmessage + "<br/><br/>";
            //Topmessage = Topmessage + "Your Details: <br/><br/> ";




            //message = message + "<tr><td>Company Name</td><td>&nbsp;</td><td>" + txtcompname.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>ABN No</td><td>&nbsp;</td><td>" + txtcompno.Text.ToString() + "</td></tr>";

            //message = message + "<tr><td>First Name</td><td>&nbsp;</td><td>" + txtfname.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>Last Name</td><td>&nbsp;</td><td>" + txtlname.Text.ToString() + "</td></tr>";
            ////  message = message + "<tr><td>Position</td><td>&nbsp;</td><td>" + txtposition.Text.ToString() + "</td></tr>";

            //message = message + "<tr><td>Phone</td><td>&nbsp;</td><td>" + txtphone.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>Mobile/Cell Phone</td><td>&nbsp;</td><td>" + txtMobile.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>Fax</td><td>&nbsp;</td><td>" + txtfax.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>Email</td><td>&nbsp;</td><td>" + txtemail.Text.ToString() + "</td></tr>";


            //message = message + "<tr><td>Street Address</td><td>&nbsp;</td><td>" + txtsadd.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>Address Line2</td><td>&nbsp;</td><td>" + txtadd2.Text.ToString() + " </td></tr>";
            //message = message + "<tr><td>Suburp/Town</td><td>&nbsp;</td><td>" + txttown.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>State/Province</td><td>&nbsp;</td><td>" + strstate + "</td></tr>";
            //message = message + "<tr><td>Postcode/ZipCode</td><td>&nbsp;</td><td>" + txtzip.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>Country</td><td>&nbsp;</td><td>" + drpCountry.SelectedItem.ToString() + "</td></tr>";
            ////  message = message + "<tr><td>Password</td><td>&nbsp;</td><td>" + Txt1password.Text.ToString() + "</td></tr>";
            //message = message + "<tr><td>LOGIN NAME</td><td>&nbsp;</td><td>" + "WAG" + reg_id.ToString().PadLeft(5, '0') + "</td></tr>";

            //bottommessage = bottommessage + "<br/><br/>";
            //bottommessage = bottommessage + "Best Regards<br/>";
            //bottommessage = bottommessage + "WAGNER";

            //MessageObj.Body = "<html><body><br/>" + Topmessage + " <table>" + message + "</table>" + bottommessage + "</body></html>";

            MessageObj.Body = _stmpl_container.ToString();

            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception)
        {
            //objUserServices.DeleteRegistration(reg_id);
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");
        }
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
        Response.Redirect("ForgotPassWord.aspx");
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.ModalPanelRRAE.Visible = false;
        this.modalPop.Hide();
        txtemail.Text = "";
        txtcemail.Text = "";
        //cText.Text = "";
        txtemail.Focus();
        return;
    }

    #endregion

    #region "Functions..."

    public void InitLoad()
    {
        if (!IsPostBack)
        {
            _HitCount = 0;
        }
        try
        {
            if (Session.Count > 0)
            {
                if (Session["USER_ID"].ToString() != "" && Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"].ToString() != ConfigurationManager.AppSettings["DUM_USER_ID"].ToString() && objUserServices.GetUserStatus(Convert.ToInt32(Session["USER_ID"].ToString())) == 1)
                {
                    if (Request.QueryString["URL"] != null)
                        Response.Redirect(Request["URL"].ToString(),false);
                    else
                        Response.Redirect("MyAccount.aspx",false);
                }
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

    private void LogSession()
    {
        try
        {
            UserSessionServices objUserSession = new UserSessionServices();
            UserSessionServices.UserSessionInfo oUSI = new UserSessionServices.UserSessionInfo();

            oUSI.Session_ID = Session.SessionID;
            oUSI.Referal_URL = sReferralURL;
            oUSI.User_ID = objHelperServices.CI(Session["USER_ID"]);
            oUSI.Last_IP = Request.ServerVariables["REMOTE_ADDR"].ToString();
            if (oUSI.User_ID > 0)
            {
                objUserSession.TrackSession(oUSI);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    #endregion
   
    protected void btnOk_Click(object sender, EventArgs e)
    {
        Button1.ImageUrl= System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "Images/close_btn.png";
       // string pass = txtchgPassnew1.Text.Trim().Substring(1);
        //string Cpass = txtchgCPass.Text.Trim().Substring(1);
        string pass = txtchgPassnew1.Text.Trim();
        string Cpass = txtchgCPass.Text.Trim();
        pass = objSecurity.StringEnCrypt_password(txtchgPassnew1.Text);
        Cpass = objSecurity.StringEnCrypt_password(txtchgCPass.Text);
        if (pass != "")
        {
            if (Cpass == pass)
            {
                objUserServices.ChangePassword(System.Convert.ToInt32(Session["USER_ID"]), pass);
                HttpContext.Current.Response.Redirect("/Home.aspx",false);
                HttpContext.Current.Response.Close();
            }
            else
            {
                txtchgPassnew1.Text = "";
                txtchgCPass.Text = "";
                lblCMsg.Text = "Confirm password does not match";
                lblCMsg1.Text = "Confirm password does not match";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script language='javascript' type='text/javascript'> alert('Confirm password does not match')</script>", true);
            }
        }
        else
            lblCMsg.Text = "Invalid Password";

        //if (txtchgPassnew1.Text.Trim() != "")
        //{
        //    if (txtchgCPass.Text.Trim() == txtchgPassnew1.Text.Trim())
        //    {
        //        objUserServices.ChangePassword(System.Convert.ToInt32(Session["USER_ID"]), txtchgPassnew1.Text.Trim());
        //        HttpContext.Current.Response.Redirect("/Home.aspx");
        //        HttpContext.Current.Response.Close();
        //    }
        //    else
        //    {
        //        lblCMsg.Text = "Confirm password does not match";
        //        lblCMsg1.Text = "Confirm password does not match";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "<script> alert('Confirm password does not match')</script>", true);
        //    }
        //}
        //else
        //    lblCMsg.Text = "Invalid Password";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Redirect("Logout.aspx",false);
        HttpContext.Current.Response.Close();
    }

    protected void CmdSignup_Click(object sender, EventArgs e)
    {
        redirectflag = true;
        HttpContext.Current.Response.Redirect("CreateanAccount.aspx",false);
        
    }
    protected void CmdSignupNew_Click(object sender, EventArgs e)
    {

    }

    protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (drpCountry.SelectedValue == "AU")
        {
            drpState.Visible = true;
            LoadStates(drpCountry.SelectedValue);
            drpState.Focus();
            txtstate.Visible = false;
            rfvstate.Enabled = false;
            rfvddlstate.Enabled = true;
            txtstate.Text = "";
            aucust.Visible = true;
            intercust.Visible = false;
        }
        else
        { 
            txtstate.Visible = true;
            txtstate.Focus();  
            drpState.Visible = false;
            rfvstate.Enabled = true;
            rfvddlstate.Enabled = false;
            aucust.Visible = false;
            intercust.Visible = true;
        }
    }

    public void LoadStates(String conCode)
    {
        DataSet oDs = new DataSet();
        oDs = objCountryServices.GetStates(conCode);
        drpState.DataSource = oDs;
        drpState.DataTextField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
        drpState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
        drpState.DataBind();
        drpState.Items.Insert(0, new ListItem("Select", ""));
       
    }
}
