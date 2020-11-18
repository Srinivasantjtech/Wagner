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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
public partial class MConfirmMessage : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    NotificationServices objNotificationServices = new NotificationServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    UserServices objUserServices = new UserServices();

    protected void Page_Load(object sender, EventArgs e)
    {                
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        //Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Page.Title = "Wagner-ConfirmMessage";
        Page.MetaKeywords = "ConfirmMessage,Registration,Shipping";
        Page.MetaDescription = "ConfirmMessage based on user action ";
        try
        {
            if (Request["Result"] == "NOPRICEAMT")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgnoprice");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "UPDATE")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgUpdate");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "SUCCESS")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgSuccess");
                lnkResult.Text = (string)GetLocalResourceObject("lnkResult.Text");
                lnkResult.Visible = false;
                lblErrormsg.Visible = false;
            }
            if (Request["Result"] == "FORGOTPASSWORD")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgForgotPassWord");
                Session["FORGOTPWD"] = (string)GetLocalResourceObject("msgForgotPassWord");
                Response.Redirect("mlogin.aspx?Result=FORGOTPWD",false);
                //lblErrormsg.Visible = false;
                //lnkResult.Visible = false;
            }
            if (Request["Result"] == "FORGOTUSERID")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgForgotuserid");
                Session["FORGOTPWD"] = (string)GetLocalResourceObject("msgForgotuserid");
                Response.Redirect("mlogin.aspx?Result=FORGOTUSER",false);
              
                //lblErrormsg.Visible = false;
                //lnkResult.Visible = false;
            }
            if (Request["Result"] == "RESETPASSWORD")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgResetPassword");
                Session["RESETPWD"] = (string)GetLocalResourceObject("msgResetPassword");
                Response.Redirect("mlogin.aspx?Result=RESETPWD",false);
                //lblErrormsg.Visible = false;
                //lnkResult.Visible = false;
            }
            if (Request["Result"] == "PASSWORDCHANGED")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgPasswordChanged");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "LOGINNAMECHANGED")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgloginnameChanged");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "ACTIVATED")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgActivation");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "ACTIVATION_FAILED")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("msgAvtivationFailed");                
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "REMAILACTIVATION")
            {
                Session["RetailerMsg"] = "true";
                if (HttpContext.Current.Session["USER_ID"]!=null)
                {
                    ReMailActivation(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                }
                
                lblConfirmmsg.Text = (string)GetLocalResourceObject("MsgReMailActivation");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
                
            }
            
            if (Request["Result"] == "CARTEMPTY")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("msgCartEmpty");
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "GETDEAL")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgGetDeal");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "GETDEALFAIL")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("lmsgGetDealfail");
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "SUBSCRIBE_ACTIVATED")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgGetDealActive");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "SUBSCRIBE_ACTIVATION_FAILED")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("msgGetDealActiveFail");
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "MESSAGESENT")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("lbConfirmMsg.Text");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "MESSAGENOTSENT")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("lbConfirmMsgFailed.Text");
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "QUOTERESULT")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgQuoteConfirmation");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "QTEEMPTY")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgCartEmpty");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "QTEEMPTY_ORDER")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgCartEmpty_Order");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "QTECANCEL")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgQuoteCancel1") + " " + Session["QUOTEID"].ToString() + " " + (string)GetLocalResourceObject("msgQuoteCancel");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "NOQUOTELIST")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgNoQuoteList");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "NOORDERS")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgNoOrder");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            else if (Request["Result"] == "WSLISTEMPTY")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgWsListEmpty");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            else if (Request["Result"] == "QUICKEMPTY")
            {
                string qmin = "";
                string qmax = "";
                string msg = "";
                if (Request["qmax"] != null)
                {
                    qmax = Request["qmax"].ToString();
                }
                if (Request["qmin"] != null)
                {
                    qmin = Request["qmin"].ToString();
                }
                if (Request["msg"] != null)
                {
                    msg = Request["msg"].ToString();
                }
                if (msg.Length > 0)
                {
                    msg += ". ";
                }
                string errmsg = "Incorrect Codes Found on Order. Please Check! Incorrect Codes : " + msg.Replace("%22", "\"").Replace("%26", "&").Replace("%23", "#") + qmin + qmax;
                int stringlen = 80;
                while (errmsg.Length > stringlen)
                {
                    errmsg = errmsg.Insert(stringlen, "<br/>");
                    stringlen = stringlen + 80;
                }
                lblCartEmpty.Text = errmsg;
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "CATALOGREQUEST")
            {
                string HtmlNotification = "<table width=\"400px\"  cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td width=\"400px\">Please send me the requested catalog</td></tr></table>";
                if (SendNotification(HtmlNotification) == 1)
                {
                    Imgstat.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/submitted_cat.jpg";
                    Imgstat.Visible = true;
                }
            }
            if (Request["Result"] == "REGISTRATION")
            {
                Session["PageUrl"] = "/mlogin.aspx";
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

    //private void ReMailActivation(int User_id)
    //{
    //    try
    //    {
    //        UserServices ObjUserServices = new UserServices();
    //        UserServices.UserInfo ObjUserInfo = new UserServices.UserInfo();
    //        Security ObjSecurity = new Security();

    //        ObjUserInfo = ObjUserServices.GetUserInfo(User_id);

    //        string url = HttpContext.Current.Request.Url.Authority.ToString();
    //        string activeLink = string.Format("http://" + url + "/Activation.aspx?id={0}", HttpUtility.UrlEncode(ObjSecurity.StringEnCrypt(User_id.ToString())));
    //        objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
    //        System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

    //        //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
    //        MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
    //        //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
    //        MessageObj.To.Add(ObjUserInfo.AlternateEmail.ToString().Trim () );
    //        string message = "";
    //        MessageObj.Subject = "WAGNER-Form-Re-Mail Activation";
    //        MessageObj.IsBodyHtml = true;
    //        MessageObj.Body = "<html><body>" + "</br> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\"> <a href=" + activeLink + ">" + activeLink + "</a></font></body></html>";
    //        System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
    //        smtpclient.UseDefaultCredentials = false;
    //        smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
    //        smtpclient.Send(MessageObj);
    //        //Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
    //    }
    //    catch (Exception)
    //    {
    //        Response.Redirect("mConfirmMessage.aspx?Result=MESSAGENOTSENT");
    //    }
    //}


    private void ReMailActivation(int User_id)
    {
        try
        {
            UserServices ObjUserServices = new UserServices();
            UserServices.UserInfo ObjUserInfo = new UserServices.UserInfo();
            Security ObjSecurity = new Security();

            ObjUserInfo = ObjUserServices.GetUserInfo(User_id);

            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string activeLink = string.Format("https://" + url + "/mActivation.aspx?id={0}", HttpUtility.UrlEncode(ObjSecurity.StringEnCrypt(User_id.ToString())));
            objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

            //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            lblmsg.Text = ObjUserInfo.AlternateEmail.ToString();
            MessageObj.To.Add(ObjUserInfo.AlternateEmail.ToString().Trim());
            string message = "";
            MessageObj.Subject = "WAGNER-Form-Re-Mail Activation";
            MessageObj.IsBodyHtml = true;
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;

            string stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
            _stg_container = new StringTemplateGroup("mail", stemplatepath);
            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "ReActivation");
            _stmpl_container.SetAttribute("Reactivation", activeLink);
            MessageObj.Body = _stmpl_container.ToString(); 

          //  MessageObj.Body = "<html><body>" + "</br> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\"> <a href=" + activeLink + ">" + activeLink + "</a></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            //Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
        }
        catch (Exception)
        {
            Response.Redirect("mConfirmMessage.aspx?Result=MESSAGENOTSENT",false);
        }
    }
    public int SendNotification(string mailmessage)
    {
        objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        int i;
        string sEmailMessage = "";
        string sUser = "";
        sUser = objUserServices.GetUserEmailAdd(objHelperServices.CI(Session["USER_ID"]));
        try
        {
            //sTemplate = oNot.GetTemplateContent(NotificationVariables.NotificationList.NEWORDER.ToString());
            //sEmailMessage = oNot.ParseTemplateMessage(sTemplate, dsOrder);
            sEmailMessage = mailmessage;
            objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
            ArrayList CCList = new ArrayList();
            CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            objNotificationServices.NotifyCC = CCList;
            objNotificationServices.NotifyTo.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            objNotificationServices.NotifyFrom = sUser;
            string EmailSubject = "Catalog Request"; //= oNot.GetEmailSubject(NotificationVariables.NotificationList.NEWORDER.ToString());
            //EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
            objNotificationServices.NotifySubject = EmailSubject;
            objNotificationServices.NotifyMessage = sEmailMessage;
            objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
            objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
            objNotificationServices.NotifyIsHTML = true; //oNot.IsHTMLNotification(NotificationVariables.NotificationList.NEWORDER.ToString());
            i = objNotificationServices.SendMessage();
            return i;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }

    }
}
