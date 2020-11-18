using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Data;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.Net.Mime;
using System.Net.Mail;


namespace WES
{

    public partial class Sorrymail : System.Web.UI.Page
    {
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();

        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

        // OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        //  UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

        PayOnlineService objPayOnlineService = new PayOnlineService();

        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";

        // string environment = "test"; // Change to "live" to process real transactions.
        // (For a live transaction, you must use a real, valid CC and billing address.)

        public int OrderID = 0;
        public string Txn_id = "";
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
        DataTable tblPaymentInfo = new DataTable();
        PayPalService objPayPalService = new PayPalService();
        //string strOrderLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        // string strPaymentLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";

        //  string strBackLink = "";
        string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
        protected void Page_Load(object sender, EventArgs e)
        {
            SendMail("indumathi@jtechindia.com");
        }
        private int SendMail(string emailid)
        {
            try
            {


                string BillAdd;
                string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();



                string Emailadd = emailid;

                string url = HttpContext.Current.Request.Url.Authority.ToString();
                string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

            
               
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

                    _stg_container = new StringTemplateGroup("main", stemplatepath);



                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "Order_Sorry");



                 
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
                    string url1 = HttpContext.Current.Request.Url.Authority.ToString();


                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());




                    //string emails = "";
                    //string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                   
                        MessageObj.To.Add(Emailadd.ToString());
                        //   MessageObj.Bcc.Add(objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString());
                  
                  
                    //objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    // MessageObj.Bcc.Add(webadminmail);

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "Bigtop - Address Error";
                   

                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;


                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);
                  

                    return 1;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
            return -1;
        }

    }
}