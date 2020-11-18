using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic; 
using System.Xml.Linq;

using System.Web.Mail;
using System.Net.Mime;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;



public partial class UC_contactus : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {
       // string s = cVerify.ClientID.ToString();         
    }
    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //   // string s = cVerify.ClientID.ToString();
        
        
    //}
    //protected void cVerify_PreRender(object sender, EventArgs e)
    //{
    //    //string s = cVerify.ClientID.ToString();
    //}
    public string ST_contactus()
    {
        try
        {
        ConnectionDB objConnectionDB = new ConnectionDB();

        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CONTACTUS", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        tbwtEngine.RenderHTML("Row");
        return (tbwtEngine.RenderedHTML);
             }
        catch (Exception ex)
      {
          objErrorHandler.ErrorMsg = ex;
          objErrorHandler.CreateLog();
          return string.Empty;
    }
    }
    //protected void BtnRequest_Click(object sender, ImageClickEventArgs e)
    //{
    //   // cVerify.ValidateCaptcha(cText.Text);
    //   // if (!cVerify.UserValidated)
    //   // {
    //        //lblresult.Text = "Invalid Verification Code";
    //   // }
    //   // else
    //   // {
    //        SaveContactUs();
    //        SendEnquiry();
    //   // }
    //}


    //protected void BtnRequest_Click(object sender,EventArgs e)
    //{
    //    SaveContactUs();
    //    SendEnquiry();
    //}
    public string ST_Newproduct()
    {
        try
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperServices = new HelperServices();

            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCTLOGNAV", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            tbwtEngine.RenderHTML("Column");
            return tbwtEngine.ST_NewProductLogNav_Load(null, HttpContext.Current);

            //if (Session["NewProductLogNav"] != null)
            //{
            //    //return tbwtEngine.ST_NewProductLogNav_Load((DataSet)Session["NewProductLogNav"]);
            //    return Session["NewProductLogNav"].ToString();
            //}
            //else
            //    return "";

            //return "";
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    public void SaveContactUs(string fullname,string email,string phone,string enquiry)
    {
        try
        {
            UserServices objUserServices = new UserServices();
            // objUserServices.SaveContact(HttpUtility.HtmlEncode(T1.Value), HttpUtility.HtmlEncode(T2.Value), HttpUtility.HtmlEncode(T3.Value), HttpUtility.HtmlEncode(S1.Value));
            objUserServices.SaveContact(HttpUtility.HtmlEncode(fullname), HttpUtility.HtmlEncode(email), HttpUtility.HtmlEncode(phone), HttpUtility.HtmlEncode(enquiry));
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void SendEnquiry(string fullname, string email, string phone, string enquiry)
    {
        try
        {
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            MessageObj.From = new System.Net.Mail.MailAddress(email.ToString());
            MessageObj.To.Add(objHelperServices.GetOptionValues("SALES EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("SALES EMAIL").ToString());
            //MessageObj.To.Add(email);

            string message = "";
            MessageObj.Subject = "Bigtop-Customer-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td>Full Name</td><td>&nbsp;</td><td>" + fullname.ToString() + "</td></tr>";
            message = message + "<tr><td>Email Address</td><td>&nbsp;</td><td>" + email.ToString() + "</td></tr>";
            message = message + "<tr><td>Contact Number</td><td>&nbsp;</td><td>" +phone.ToString() + "</td></tr>";
            message = message + "<tr><td>Enquiry/Comments</td><td>&nbsp;</td><td>" + enquiry.ToString() + "</td></tr>";
            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
           // Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT",true);
        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT",false);
        }
    }

    protected void btnsend_click(object sender, EventArgs e)
    {
       // try
        //{
       //     Notification oNot = new Notification();
       //     oNot.SMTPServer = oHelper.GetOptionValues("MAIL SERVER").ToString();
       //     oNot.NotifyTo.Add(oHelper.GetOptionValues("ADMIN EMAIL").ToString());
       //     oNot.NotifyFrom = txtemail.Value.ToString() + "<support@tradingbell.com>";
       //     oNot.NotifySubject = "Mail from " + txtorgname.Value.ToString();
       //     string message = "";
       //     message = message + "Full Name               : " + txtfname.Value.ToString() + Environment.NewLine;
       //     message = message + "Organization Name  : " + txtorgname.Value.ToString() + Environment.NewLine;
       //     message = message + "Organization Type   : " + txtorgtype.Value.ToString() + Environment.NewLine;
       //     message = message + "Address                  : " + txtaddress.Value.ToString() + Environment.NewLine;
       //     message = message + "Post Code              : " + txtpostcode.Value.ToString() + Environment.NewLine;
       //     message = message + "Phone Number       : " + txtphone.Value.ToString() + Environment.NewLine;
       //     message = message + "Mobile Number      : " + txtmobile.Value.ToString() + Environment.NewLine;

       //     oNot.NotifyMessage = message;
       //     oNot.UserName = "support@tradingbell.com";
       //     oNot.Password = "catalog@5";
       //     int chkmail = oNot.SendMessage();
       //     if (chkmail > 0)
       //     {
       //         Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
       //     }
       //     else
       //     {
       //         Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");

       //     }

       // }
       // catch (Exception ex)
       //{
       //    Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");

       //    oErr.CreateLog();
       // }
        //try
        //{
            
        //    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage(oHelper.GetOptionValues("ADMIN EMAIL").ToString(), txtemail.Value.ToString());
        //    string message = "";
        //    MessageObj.Subject = "Mail from " + txtorgname.Value.ToString();
        //    MessageObj.IsBodyHtml = true;
        //    message = message + "Full Name&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;:&nbsp;" + txtfname.Value.ToString() + "<br/>";
        //    message = message + "Organization Name&nbsp; &nbsp;:&nbsp;" + txtorgname.Value.ToString() + "<br/>";
        //    message = message + "Organization Type&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + txtorgtype.Value.ToString() + "<br/>";
        //    message = message + "Address&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + txtaddress.Value.ToString() + "<br/>";
        //    message = message + "Post Code&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;:&nbsp;" + txtpostcode.Value.ToString() + "<br/>";
        //    message = message + "Phone Number&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;:&nbsp;" + txtphone.Value.ToString() + "<br/>";
        //    message = message + "Mobile Number&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;:&nbsp;" + txtmobile.Value.ToString() + "<br/>";
        //    MessageObj.Body = "<html><body><b>" + message + "</b>" + " <font color=\"red\"></font></body></html>";
        //    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(oHelper.GetOptionValues("MAIL SERVER").ToString());
        //    smtpclient.UseDefaultCredentials =false;
        //    smtpclient.Send(MessageObj);
        //    Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT",false);
        //}
        //catch (Exception exc)
        //{
        //    Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");
        //}

    }

    protected void btn1_Click(object sender, EventArgs e)
    {

    }

    //public void BtnRequest_Click(object sender, EventArgs e)
    //{
    //    SaveContactUs();
    //    SendEnquiry();
    //}
}
