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

using System.Security.Cryptography;
using System.Text;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
//Modified by:Indu
//Modified on:11-Apr-2013
//Mofified reason:To Build encrption logic
public partial class MForgotPassWord : System.Web.UI.Page
{
    #region "Delecarations..."
    private static int _UserID;
    UserServices objUserServices = new UserServices();
    Security objcrpengine = new Security(); 
    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    NotificationServices objNotificationServices = new NotificationServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    private const int DefaultMinimum = 6;
    private const int DefaultMaximum = 10;
    private const int UBoundDigit = 61;

    private RNGCryptoServiceProvider rng;
    private int minSize;
    private int maxSize;
    private bool hasRepeating;
    private bool hasConsecutive;
    private bool hasSymbols;
    private string exclusionSet;
    private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$".ToCharArray();
    string loginname = "";
    #endregion
    #region property
    public string Exclusions
    {
        get
        {
            return this.exclusionSet;
        }
        set
        {
            this.exclusionSet = value;
        }
    }
    public bool ExcludeSymbols
    {
        get { return this.hasSymbols; }
        set { this.hasSymbols = value; }
    }

    public bool RepeatCharacters
    {
        get { return this.hasRepeating; }
        set { this.hasRepeating = value; }
    }

    public bool ConsecutiveCharacters
    {
        get { return this.hasConsecutive; }
        set { this.hasConsecutive = value; }
    }
    public int Minimum
    {
        get { return this.minSize; }
        set
        {
            this.minSize = value;
            if (DefaultMinimum > this.minSize)
            {
                this.minSize = DefaultMinimum;
            }
        }
    }

    public int Maximum
    {
        get { return this.maxSize; }
        set
        {
            this.maxSize = value;
            if (this.minSize >= this.maxSize)
            {
                this.maxSize = DefaultMaximum;
            }
        }
    }
    #endregion
    # region "Events..."

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["loginName"] != null && Request.QueryString["loginName"].ToString().Trim() != "")
        //{
        //    bool validUser;
        //    string username;
        //    int UserID;

        //    txtLoginName.Text = Request.QueryString["loginName"];
        //    username = txtLoginName.Text;
        //    validUser = oUser.CheckUserName(username);
        //    UserID = oUser.GetUserID(username);
        //    if (UserID == -1 || username == string.Empty || validUser == false)
        //    {
        //        Session["ForgotAction"] = "Invalid User Name!";
        //        Response.Redirect("/Login.aspx", true);
        //    }
        //}
        //else
        //{
        //    Session["ForgotAction"] = "Invalid User Name!";
        //    Response.Redirect("/Login.aspx", true);
        //}

        //Session["ForgotAction"] = "";
      //  txtLoginName.Attributes.Add("onkeypress", "javascript:return Email(event);");
      //  txtUserMail.Attributes.Add("onkeypress", "javascript:return Email(event);");
      ////  CusType.Attributes.Add("onclick", "javascript:CheckCusType();");
      //  CusType.Attributes.Add("onchange", "javascript:CheckCusType();");

       //txtUserMail.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnUser.UniqueID + "').click();return false;}} else {return true}; ");
        //txtYourAnswer.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.UniqueID + "').click();return false;}} else {return true}; ");
        try
        {

            this.Minimum = DefaultMinimum;
            this.Maximum = DefaultMaximum;
            this.ConsecutiveCharacters = false;
            this.RepeatCharacters = true;
            this.ExcludeSymbols = false;
            this.Exclusions = null;

            rng = new RNGCryptoServiceProvider();
            Session["COUNT"] = "0";

           // Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            Page.Title = "Wagner-Forgot Password";
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
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
       

       // txtLoginName.Focus();
    }

    //protected void btnUser_Click(object sender, EventArgs e)
    //{
    //    tblError.Visible = false;
    //    try
    //    {
    //        if (tblSecurityQuestion.Visible)
    //        {
    //            if (txtYourAnswer.Text.Trim().Length > 0)
    //            {
    //                string sAnswer = objUserServices.GetSecurityAnswer(_UserID);
    //                if (sAnswer.Equals(txtYourAnswer.Text.Trim()))
    //                {
    //                    SendNotification();
    //                    Response.Redirect("ConfirmMessage.aspx?Result=FORGOTPASSWORD");
    //                }
    //                else
    //                {
    //                    tblError.Visible = true;
    //                    lblError.Text = GetLocalResourceObject("msgAnswerError").ToString();
    //                }
    //            }
    //            else
    //            {
    //                tblError.Visible = true;
    //                lblError.Text = GetLocalResourceObject("msgAnswerEmptyError").ToString();
    //            }
    //        }
    //        if (tblUserID.Visible)
    //        {
    //            if (txtUserMail.Text.Trim().Length > 0)
    //            {
    //                if (CusType.Text != "Retailer")
    //                {
    //                    if (objCompanyGroupServices.CheckCompanyStatus(objUserServices.GetUserID(txtLoginName.Text)) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
    //                    {
    //                        if (objUserServices.CheckUserEmail(txtUserMail.Text, CusType.Text) && objUserServices.CheckForgotUserEmail(txtLoginName.Text, txtUserMail.Text))
    //                        {
    //                            _UserID = objUserServices.GetUserID(txtLoginName.Text);
    //                            SendNotification();
    //                            Response.Redirect("ConfirmMessage.aspx?Result=FORGOTPASSWORD");
    //                            //RetriveSecurityQuestion(_UserID);
    //                        }
    //                        else
    //                        {
    //                            tblError.Visible = true;
    //                            lblError.Text = GetLocalResourceObject("msgUserIDError").ToString();
    //                        }
    //                    }
    //                    else
    //                    {
    //                        tblError.Visible = true;
    //                        lblError.Text = GetLocalResourceObject("msgCompanyError").ToString();
    //                    }
    //                }
    //                else
    //                {
    //                    DataSet tmpds = null;
    //                    tmpds = objUserServices.CheckMultipleUserMail(txtUserMail.Text, CusType.Text);
    //                    if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (tmpds.Tables[0].Rows.Count > 1 && txtLoginName.Text.Trim().Length == 0)
    //                        {
                                
    //                                tblError.Visible = true;
    //                                lblError.Text = "Email ID is associated to multiple logins. Please Enter your unique 'Login Name'";
    //                                return;
                               
    //                        }
    //                        else
    //                        { 
                               
    //                            loginname = tmpds.Tables[0].Rows[0]["LOGIN_NAME"].ToString();
    //                            if (objCompanyGroupServices.CheckCompanyStatus(objUserServices.GetUserID(loginname)) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
    //                            {
    //                                if (objUserServices.CheckUserEmail(txtUserMail.Text, CusType.Text) && objUserServices.CheckForgotUserEmail(loginname, txtUserMail.Text))
    //                                {
    //                                    _UserID = objUserServices.GetUserID(loginname);
    //                                    SendNotification();
    //                                    Response.Redirect("ConfirmMessage.aspx?Result=FORGOTPASSWORD");
    //                                    //RetriveSecurityQuestion(_UserID);
    //                                }
    //                                else
    //                                {
    //                                    tblError.Visible = true;
    //                                    lblError.Text = GetLocalResourceObject("msgUserIDError").ToString();
    //                                }
    //                            }
    //                            else
    //                            {
    //                                tblError.Visible = true;
    //                                lblError.Text = GetLocalResourceObject("msgCompanyError").ToString();
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        tblError.Visible = true;
    //                        lblError.Text = "Entered email address does not exists";
    //                    }
    //                    }

                   

    //            }
    //            else
    //            {
    //                tblError.Visible = true;
    //                lblError.Text = GetLocalResourceObject("msgUserIDEmptyError").ToString();
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet tmpds = null;
            tmpds = objUserServices.CheckMultipleUserMail(txtUserMail.Text, "Retailer");
            objErrorHandler.CreateLog("count "+ tmpds.Tables.Count);
            if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
            {
                if (tmpds.Tables[0].Rows.Count > 1)
                {

                    // tblError.Visible = true;
                    lblError.Visible = true;
                    lblError.Text = "Email ID is associated to multiple logins. Please Enter your unique 'Login Name'";
                    return;

                }
                else
                {

                    loginname = tmpds.Tables[0].Rows[0]["LOGIN_NAME"].ToString();
                    objErrorHandler.CreateLog("loginname "+ loginname);
                    if (objCompanyGroupServices.CheckCompanyStatus(objUserServices.GetUserID(loginname)) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                    {
                        if (objUserServices.CheckUserEmail(txtUserMail.Text, "Retailer") && objUserServices.CheckForgotUserEmail(loginname, txtUserMail.Text))
                        {
                            _UserID = objUserServices.GetUserID(loginname);
                            objErrorHandler.CreateLog("_UserID "+ _UserID);
                           SendNotification();
                            Response.Redirect("mConfirmMessage.aspx?Result=FORGOTPASSWORD",false);
                            //RetriveSecurityQuestion(_UserID);
                        }
                        else
                        {
                            // tblError.Visible = true;
                            lblError.Visible = true;
                            lblError.Text = GetLocalResourceObject("msgUserIDError").ToString();
                        }
                    }
                    else
                    {
                        // tblError.Visible = true;
                        lblError.Visible = true;
                        lblError.Text = GetLocalResourceObject("msgCompanyError").ToString();
                    }
                }
            }
            else
            {

                lblError.Visible = true;
                lblError.Text = "Invalid Email Id";
            }
       
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    
    #endregion

    #region "Functions..."
    public string Generate()
    {
        // Pick random length between minimum and maximum   
        int pwdLength = GetCryptographicRandomNumber(this.Minimum,
            this.Maximum);

        StringBuilder pwdBuffer = new StringBuilder();
        pwdBuffer.Capacity = this.Maximum;

        // Generate random characters
        char lastCharacter, nextCharacter;

        // Initial dummy character flag
        lastCharacter = nextCharacter = '\n';

        for (int i = 0; i < pwdLength; i++)
        {
            nextCharacter = GetRandomCharacter();

            if (false == this.ConsecutiveCharacters)
            {
                while (lastCharacter == nextCharacter)
                {
                    nextCharacter = GetRandomCharacter();
                }
            }

            if (false == this.RepeatCharacters)
            {
                string temp = pwdBuffer.ToString();
                int duplicateIndex = temp.IndexOf(nextCharacter);
                while (-1 != duplicateIndex)
                {
                    nextCharacter = GetRandomCharacter();
                    duplicateIndex = temp.IndexOf(nextCharacter);
                }
            }

            if ((null != this.Exclusions))
            {
                while (-1 != this.Exclusions.IndexOf(nextCharacter))
                {
                    nextCharacter = GetRandomCharacter();
                }
            }

            pwdBuffer.Append(nextCharacter);
            lastCharacter = nextCharacter;
        }

        if (null != pwdBuffer)
        {
            return pwdBuffer.ToString();
        }
        else
        {
            return String.Empty;
        }
    }
    protected char GetRandomCharacter()
    {
        int upperBound = pwdCharArray.GetUpperBound(0);

        if (true == this.ExcludeSymbols)
        {
            upperBound = UBoundDigit;
        }

        int randomCharPosition = GetCryptographicRandomNumber(
            pwdCharArray.GetLowerBound(0), upperBound);

        char randomChar = pwdCharArray[randomCharPosition];

        return randomChar;
    }

    protected int GetCryptographicRandomNumber(int lBound, int uBound)
    {
        // Assumes lBound >= 0 && lBound < uBound
        // returns an int >= lBound and < uBound
        uint urndnum;
        byte[] rndnum = new Byte[4];
        if (lBound == uBound - 1)
        {
            // test for degenerate case where only lBound can be returned
            return lBound;
        }

        uint xcludeRndBase = (uint.MaxValue -
            (uint.MaxValue % (uint)(uBound - lBound)));

        do
        {
            rng.GetBytes(rndnum);
            urndnum = System.BitConverter.ToUInt32(rndnum, 0);
        } while (urndnum >= xcludeRndBase);

        return (int)(urndnum % (uBound - lBound)) + lBound;
    }
    //public int PasswordGenerator() 
    //    {
    //        this.Minimum               = DefaultMinimum;
    //        this.Maximum               = DefaultMaximum;
    //        this.ConsecutiveCharacters = false;
    //        this.RepeatCharacters      = true;
    //        this.ExcludeSymbols        = false;
    //        this.Exclusions            = null;

    //        rng = new RNGCryptoServiceProvider();
    //    }	
    //public void RetriveSecurityQuestion(int UserID)
    //{
    //    tblUserID.Visible = false;
    //    tblusedidbtn.Visible = false;
    //    tblSecurityQuestion.Visible = true;
    //    tblSecurityQuestionbtn.Visible = true;
    //    lblSecurityQuestion.Text = objUserServices.GetSecurityQuestion(UserID);
    //}

 //   public void SendNotification()
 //   {
 //       try
 //       {
 //           string UserName = "";
 //           string sEmailContent = "";
 //           string UserFullName = "";
 //           string ResetPasswordLink = "";
 //           objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
 //           DataSet dsFP = objNotificationServices.BuildNotifyInfo();
 //           DataRow row = dsFP.Tables[0].NewRow();
 //           row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.USERID.ToString() + objNotificationServices.UniqueEndSymbol;
 //           row["ColumnValue"] = objUserServices.GetUserEmailAdd(_UserID);
 //           UserName = objUserServices.GetUserEmailAdd(_UserID);
 //           //row["ColumnValue"] = oUser.GetUserEmail(txtUserMail.Text);
 //           //UserName = oUser.GetUserEmail(txtUserMail.Text);
 //           dsFP.Tables[0].Rows.Add(row);
 //           row = dsFP.Tables[0].NewRow();
 //           row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.FULLNAME.ToString() + objNotificationServices.UniqueEndSymbol;
 //           row["ColumnValue"] = objUserServices.GetUserFullName(_UserID);
 //           UserFullName = objUserServices.GetUserFullName(_UserID);
 //           dsFP.Tables[0].Rows.Add(row);
 //           row = dsFP.Tables[0].NewRow();
 //           row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.NEWPASSWORD.ToString() + objNotificationServices.UniqueEndSymbol;
 //           row["ColumnValue"] = objUserServices.GetPassword(_UserID);
 //           dsFP.Tables[0].Rows.Add(row);
 //           string sTemplateContent = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
 //           string newPassword = "";
 //           newPassword = Generate();
 //           string url = HttpContext.Current.Request.Url.Authority.ToString();
 //          // ResetPasswordLink = string.Format("http://staging.wesonline.com.au/ResetPassword/{0}/{1}/",txtLoginName.Text, newPassword);
 //          // ResetPasswordLink = string.Format("http://" + url + "/ResetPassword.aspx?LoginName={0}&pwdKey={1}", txtLoginName.Text, newPassword);

 //           //if (txtLoginName.Text.Trim().Length >0)
 //           //    ResetPasswordLink = string.Format("http://" + url + "/ResetPassword.aspx?LoginName={0}&pwdKey={1}&UserId={2}", txtLoginName.Text, newPassword, _UserID);
 //           //else
 //           ResetPasswordLink = string.Format("http://" + url + "/mResetPwd.aspx?LoginName={0}&pwdKey={1}&UserId={2}", loginname, newPassword, _UserID);
 //           sEmailContent = sTemplateContent;
 //           //sEmailContent = sEmailContent.Replace("{FULLNAME}", UserFullName);
 //           //sEmailContent = sEmailContent.Replace("{NEWPASSWORD}", newPassword);
 //           //sEmailContent = sEmailContent.Replace("{USER_ID}", _UserID.ToString());
 //           sEmailContent = sEmailContent.Replace("{RESETPWDLINK}", ResetPasswordLink.ToString());
 //           objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
 //           objNotificationServices.NotifyTo.Add(UserName);
 //           objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
 //           objNotificationServices.NotifySubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
 //           objNotificationServices.NotifyMessage = sEmailContent;
 //           objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
 //           int chkmail = objNotificationServices.SendMessage();
 //           if (chkmail > 0)
 //           {
 // //Modified by:indu
 //               //Modified on:11-apr-2013
 //               //Mofified reason:To Build encrption logic
 //newPassword = objcrpengine.StringEnCrypt_password(newPassword);
 //               int Userchk;
 //                //if (txtLoginName.Text.Trim().Length >0)
 //                //    Userchk = objUserServices.UpdateUserName(newPassword, _UserID, txtLoginName.Text, txtUserMail.Text);
 //                //else
 //                    Userchk = objUserServices.UpdateUserName(newPassword, _UserID, loginname, txtUserMail.Text);
 //           }
 //       }
 //       catch (Exception ex)
 //       {
 //           objErrorHandler.ErrorMsg = ex;
 //           objErrorHandler.CreateLog();
 //       }
 //   }



    //public void SendNotification()
    //{
    //    try
    //    {
    //        string UserName = "";
    //        string sEmailContent = "";
    //        string UserFullName = "";
    //        string ResetPasswordLink = "";
    //        objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
    //        DataSet dsFP = objNotificationServices.BuildNotifyInfo();
    //        DataRow row = dsFP.Tables[0].NewRow();
    //        row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.USERID.ToString() + objNotificationServices.UniqueEndSymbol;
    //        row["ColumnValue"] = objUserServices.GetUserEmailAdd(_UserID);
    //        UserName = objUserServices.GetUserEmailAdd(_UserID);
    //        //row["ColumnValue"] = oUser.GetUserEmail(txtUserMail.Text);
    //        //UserName = oUser.GetUserEmail(txtUserMail.Text);
    //        dsFP.Tables[0].Rows.Add(row);
    //        row = dsFP.Tables[0].NewRow();
    //        row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.FULLNAME.ToString() + objNotificationServices.UniqueEndSymbol;
    //        row["ColumnValue"] = objUserServices.GetUserFullName(_UserID);
    //        UserFullName = objUserServices.GetUserFullName(_UserID);
    //        dsFP.Tables[0].Rows.Add(row);
    //        row = dsFP.Tables[0].NewRow();
    //        row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.NEWPASSWORD.ToString() + objNotificationServices.UniqueEndSymbol;
    //        row["ColumnValue"] = objUserServices.GetPassword(_UserID);
    //        dsFP.Tables[0].Rows.Add(row);
    //        string sTemplateContent = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
    //        string newPassword = "";
    //        newPassword = Generate();
    //        string url = HttpContext.Current.Request.Url.Authority.ToString();
    //        // ResetPasswordLink = string.Format("http://staging.wesonline.com.au/ResetPassword/{0}/{1}/",txtLoginName.Text, newPassword);
    //        // ResetPasswordLink = string.Format("http://" + url + "/ResetPassword.aspx?LoginName={0}&pwdKey={1}", txtLoginName.Text, newPassword);

    //        //if (txtLoginName.Text.Trim().Length >0)
    //        //    ResetPasswordLink = string.Format("http://" + url + "/ResetPassword.aspx?LoginName={0}&pwdKey={1}&UserId={2}", txtLoginName.Text, newPassword, _UserID);
    //        //else
    //        ResetPasswordLink = string.Format("http://" + url + "/mResetPwd.aspx?LoginName={0}&pwdKey={1}&UserId={2}", loginname, newPassword, _UserID);

    //        StringTemplateGroup _stg_container = null;
    //        StringTemplate _stmpl_container = null;

    //        string stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
    //        _stg_container = new StringTemplateGroup("mail", stemplatepath);
    //        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "ForgotPwd");

    //        //sEmailContent = sEmailContent.Replace("{FULLNAME}", UserFullName);
    //        //sEmailContent = sEmailContent.Replace("{NEWPASSWORD}", newPassword);
    //        //sEmailContent = sEmailContent.Replace("{USER_ID}", _UserID.ToString());
    //        //  sEmailContent = sEmailContent.Replace("{RESETPWDLINK}", ResetPasswordLink.ToString());

    //        _stmpl_container.SetAttribute("ResetPwdLink", ResetPasswordLink);

    //        sEmailContent = _stmpl_container.ToString(); ;
    //        objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
    //        objNotificationServices.NotifyTo.Add(UserName);
    //        objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
    //        objNotificationServices.NotifySubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
    //        objNotificationServices.NotifyMessage = sEmailContent;

    //        objNotificationServices.NotifyIsHTML = true;
    //        //objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
    //        int chkmail = objNotificationServices.SendMessage();
    //        if (chkmail > 0)
    //        {
    //            //Modified by:indu
    //            //Modified on:11-apr-2013
    //            //Mofified reason:To Build encrption logic
    //            newPassword = objcrpengine.StringEnCrypt_password(newPassword);
    //            int Userchk;
    //            //if (txtLoginName.Text.Trim().Length >0)
    //            //    Userchk = objUserServices.UpdateUserName(newPassword, _UserID, txtLoginName.Text, txtUserMail.Text);
    //            //else
    //            Userchk = objUserServices.UpdateUserName(newPassword, _UserID, loginname, txtUserMail.Text);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //    }
    //}




    private void SendNotification()
    {
        try
        {


            // HelperServices objHelperServices = new HelperServices();
            // UserServices objUserServices = new UserServices();
            // UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
           // string _UserID = "0";

            //MessageObj.From = new System.Net.Mail.MailAddress(txtUserMail.Text);
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            MessageObj.To.Add(txtUserMail.Text);
            string message = "";
            MessageObj.Subject = "Reset Password";
            MessageObj.IsBodyHtml = true;

            string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());

            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            DataSet dsrecords = new DataSet();
            _stg_records = new StringTemplateGroup("row", templatepath);
            _stg_container = new StringTemplateGroup("main", templatepath);
            string ResetPasswordLink = "";
            string newPassword = "";
            string href = "";

         
            newPassword = Generate();
            objErrorHandler.CreateLog("templatepath "+ templatepath);
            objErrorHandler.CreateLog("newPassword "+ newPassword);
            string url = HttpContext.Current.Request.Url.Authority.ToString();
            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "ForgotPwd");
            ResetPasswordLink = "https://" + url + "/mResetPwd.aspx?LoginName=" + loginname + "&pwdKey=" + newPassword + "&UserId=" + _UserID;
            _stmpl_container.SetAttribute("URL", ResetPasswordLink);
           //  <a href="$URL$">$URL$ </a>
            href = "<a href=" + ResetPasswordLink + ">" + ResetPasswordLink + "</a>";
            _stmpl_container.SetAttribute("href", href);
           // _stmpl_container.SetAttribute("srcimg1", img1);
          //  _stmpl_container.SetAttribute("srcimg2", img2);
            message = _stmpl_container.ToString();
            MessageObj.Body = message;
            objErrorHandler.CreateLog(message);
            // MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
   smtpclient.Send(MessageObj);
            newPassword = objcrpengine.StringEnCrypt_password(newPassword);
            int Userchk;
           
            Userchk = objUserServices.UpdateUserName(newPassword, _UserID, loginname, txtUserMail.Text);


            //string UserName = "";
            ////string sEmailContent = "";
            //string UserFullName = "";
            //string ResetPasswordLink = "";
            ////objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            ////DataSet dsFP = objNotificationServices.BuildNotifyInfo();
            ////DataRow row = dsFP.Tables[0].NewRow();
            ////row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.USERID.ToString() + objNotificationServices.UniqueEndSymbol;
            ////row["ColumnValue"] = objUserServices.GetUserEmailAdd(_UserID);
            ////UserName = objUserServices.GetUserEmailAdd(_UserID);
            //////row["ColumnValue"] = oUser.GetUserEmail(txtUserMail.Text);
            //////UserName = oUser.GetUserEmail(txtUserMail.Text);
            ////dsFP.Tables[0].Rows.Add(row);
            ////row = dsFP.Tables[0].NewRow();
            ////row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.FULLNAME.ToString() + objNotificationServices.UniqueEndSymbol;
            ////row["ColumnValue"] = objUserServices.GetUserFullName(_UserID);
            ////UserFullName = objUserServices.GetUserFullName(_UserID);
            ////dsFP.Tables[0].Rows.Add(row);
            ////row = dsFP.Tables[0].NewRow();
            ////row["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.ResetPassword.NEWPASSWORD.ToString() + objNotificationServices.UniqueEndSymbol;
            ////row["ColumnValue"] = objUserServices.GetPassword(_UserID);
            ////dsFP.Tables[0].Rows.Add(row);
            ////string sTemplateContent = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.RESETPASSWORD.ToString());
            //string newPassword = "";
            //newPassword = Generate();
            //string url = HttpContext.Current.Request.Url.Authority.ToString();
            //System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

            ////MessageObj.From = new System.Net.Mail.MailAddress(txtUserMail.Text);
            ////MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(txtUserMail.Text);
            //string message = "";
            //MessageObj.Subject = "Reset Password";
            //MessageObj.IsBodyHtml = true;
            //string url1 = "";
            //string url2 = "";
            //StringTemplateGroup _stg_container = null;
            //StringTemplate _stmpl_container = null;

            //string templatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
            //_stg_container = new StringTemplateGroup("mail", templatepath);
            //_stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "ForgotPwd");
            //// ResetPasswordLink = string.Format("http://" + url + "/mResetPwd.aspx?LoginName={0}&pwdKey={1}&UserId={2}", loginname, newPassword, _UserID);
            //ResetPasswordLink = "http://" + url + "/mResetPwd.aspx?LoginName=" + loginname + "&pwdKey=" + newPassword + "&UserId=" + _UserID;

            //_stmpl_container.SetAttribute("URL", ResetPasswordLink);
            //_stmpl_container.SetAttribute("URL1", url1);
            //_stmpl_container.SetAttribute("URL2", url2);

            //message = _stmpl_container.ToString();
            //MessageObj.Body = message;
            //// MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            //System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            //smtpclient.UseDefaultCredentials = false;
            //smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            //smtpclient.Send(MessageObj);

            //newPassword = objcrpengine.StringEnCrypt_password(newPassword);
            //int Userchk;
            ////if (txtLoginName.Text.Trim().Length >0)
            ////    Userchk = objUserServices.UpdateUserName(newPassword, _UserID, txtLoginName.Text, txtUserMail.Text);
            ////else
            //Userchk = objUserServices.UpdateUserName(newPassword, _UserID, loginname, txtUserMail.Text);
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            //lblError.Text = ex.ToString();
            //lblErrorMessage.Text = ex.ToString(); 
           objHelperServices.writelog(ex.ToString());  
            //objUserServices.DeleteRegistration(reg_id);
         Response.Redirect("MConfirmMessage.aspx?Result=MESSAGENOTSENT",false);
        }
    }
    
    
    
    
    #endregion
}
