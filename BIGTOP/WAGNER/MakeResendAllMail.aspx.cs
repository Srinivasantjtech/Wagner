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
using System.Configuration;
public partial class MakeResendAllMail : System.Web.UI.Page
    {
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();
        CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

       // OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
        NotificationServices objNotificationServices = new NotificationServices();
        UserServices objUserServices = new UserServices();
        //UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

        PayOnlineService objPayOnlineService = new PayOnlineService();

        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";

        //string environment = "test"; // Change to "live" to process real transactions.
        // (For a live transaction, you must use a real, valid CC and billing address.)

        public int OrderID = 0;
        public string Txn_id = "";
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
        DataTable tblPaymentInfo = new DataTable();
        PayPalService objPayPalService = new PayPalService();
       
        
        //string strBackLink = "";
        string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
       // string key = "";
        protected override void OnPreRender(EventArgs e)
        {
           


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
        protected void Page_Load(object sender, EventArgs e)
        {
            

           // string output = "";
            
            try
            {
               
                DataTable temptbl = objOrderServices.Get_Error_Mail_List_Resend();
                //bool isau = false;
                if (temptbl != null && temptbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in temptbl.Rows)//For Records
                    {
                        try
                        {
                            if (dr["MAIL_TYPE"].ToString() != "" && dr["MAIL_TYPE"].ToString() == "ICOS")
                            {
                                int order_status;
                                int rtnmail = 0;
                                order_status = objOrderServices.Get_Error_Mail_Log_OrderStatus(Convert.ToInt32(dr["ORDER_ID"].ToString()));
                                if (order_status == (int)OrderServices.OrderStatus.Intl_Waiting_Verification)
                                {
                                    rtnmail = SendMail(Convert.ToInt32(dr["ORDER_ID"].ToString()), Convert.ToInt32(order_status), Convert.ToInt32(777), Convert.ToInt32(0), Convert.ToInt32(dr["IS_SITE"].ToString()));
                                    if (rtnmail == 1)
                                        objOrderServices.Update_Mail_Error_Log(Convert.ToInt32(dr["ORDER_ID"].ToString()), 1);
                                }
                            }
                            else if(dr["MAIL_TYPE"].ToString() != "" && dr["MAIL_TYPE"].ToString() == "SP" )
                            {
                                int order_status_sp;
                                int rtnmail_sp = 0;
                                order_status_sp = objOrderServices.Get_Error_Mail_Log_OrderStatus(Convert.ToInt32(dr["ORDER_ID"].ToString()));
                                rtnmail_sp = SendMail_AfterPaymentSP(Convert.ToInt32(dr["ORDER_ID"].ToString()), Convert.ToInt32(order_status_sp), Convert.ToInt32(777), Convert.ToInt32(0));
                                TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                tbwtEngine.SendMail_Review(Convert.ToInt32(dr["ORDER_ID"].ToString()), (int)OrderServices.OrderStatus.Payment_Successful, false);
                                if (rtnmail_sp == 1)
                                    objOrderServices.Update_Mail_Error_Log(Convert.ToInt32(dr["ORDER_ID"].ToString()), 1);
                            }
                            else if (dr["MAIL_TYPE"].ToString() != "" && dr["MAIL_TYPE"].ToString() == "PP")
                            {
                                int order_status_pp;
                                int rtnmail_pp = 0;
                                order_status_pp = objOrderServices.Get_Error_Mail_Log_OrderStatus(Convert.ToInt32(dr["ORDER_ID"].ToString()));
                                rtnmail_pp = SendMail_AfterPaymentPP(Convert.ToInt32(dr["ORDER_ID"].ToString()), Convert.ToInt32(order_status_pp), Convert.ToInt32(777), Convert.ToInt32(0));
                                TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                tbwtEngine.SendMail_Review(Convert.ToInt32(dr["ORDER_ID"].ToString()), (int)OrderServices.OrderStatus.Payment_Successful, false);
                                if (rtnmail_pp == 1)
                                    objOrderServices.Update_Mail_Error_Log(Convert.ToInt32(dr["ORDER_ID"].ToString()), 1);
                            }
                            else if (dr["MAIL_TYPE"].ToString() != "" && dr["MAIL_TYPE"].ToString() == "MS_SMAF")
                            {
                                int order_status_ms_smap;
                                int rtnmail_smap = 0;
                                order_status_ms_smap = objOrderServices.Get_Error_Mail_Log_OrderStatus(Convert.ToInt32(dr["ORDER_ID"].ToString()));
                                rtnmail_smap = SendMail_AfterPayment(Convert.ToInt32(dr["ORDER_ID"].ToString()), Convert.ToInt32(order_status_ms_smap), Convert.ToInt32(777), Convert.ToInt32(0));
                                TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                tbwtEngine.SendMail_Review(Convert.ToInt32(dr["ORDER_ID"].ToString()), (int)OrderServices.OrderStatus.Payment_Successful, false);
                                if (rtnmail_smap == 1)
                                    objOrderServices.Update_Mail_Error_Log(Convert.ToInt32(dr["ORDER_ID"].ToString()), 1);
                            }
                           
                        }
                        catch (Exception er)
                        {
                            objErrorHandler.ErrorMsg = er;
                            objErrorHandler.CreateLog();
                        }
                    }
                }




              
            }            
            catch (Exception ex)
            {
              


            }
    

        }
        private int SendMail(int OrderId, int OrderStatus,int user_id,int user_role,int is_site)
        {
            string toemail = "";
            try
            {
           

                string BillAdd;
                string ShippAdd;
                string stemplatepath;

                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

                oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);
                user_id = oOrderInfo.UserID;
                int UserID = user_id;
                if (UserID != 777)
                {
                    oUserInfo = objUserServices.GetUserInfo(UserID);
                }
                else
                {
            oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                }
              
                user_role = oUserInfo.USERROLE;
                    dsOItem = objOrderServices.GetOrderItems(OrderId);
                BillAdd = GetBillingAddress(OrderId);
                ShippAdd = GetShippingAddress(OrderId);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;
                



             //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
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
                   // StringTemplate _stmpl_records1 = null;
                    //StringTemplate _stmpl_recordsrows = null;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                   // StringTemplateGroup _stg_container1 = null;
                   // StringTemplateGroup _stg_records1 = null;
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    if(is_site == 2)
                        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
                    else
                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());

                    //int ictrows = 0;

                    DataSet dscat = new DataSet();
                    //DataTable dt = null;
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

                    if (user_role == 1 || user_role == 2)
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                    else
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");


                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                    if (user_role == 1 || user_role == 2)
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
                    
                    string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                  

                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();


                    if (user_role == 1 || user_role == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        MessageObj.Bcc.Add(webadminmail);

                    }
                    else
                    {
                        emails = Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());
                        // MessageObj.Bcc.Add(addressBCC);

                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    if (user_role == 1 || user_role == 2)
                    {
                        MessageObj.Subject = "Bigtop Order Confirmation - Order No : " + CustomerOrderNo;
                    }
                    else
                    {
                        MessageObj.Subject = "Bigtop Pending Order Notification - Order No : " + CustomerOrderNo;
                    }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;


                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);
                   // return 1;



                    if (user_role == 1 || user_role == 2)
                    {
                        try
                        {
                            if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                            {
                                //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                                MessageObj.To.Clear();
                                MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                                smtpclient.Send(MessageObj);
                                //return 1;
                            }
                        }
                        catch
                        { }
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
                                        //return 1;
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
                                    //return 1;
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
                                       // return 1;
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
                                    //return 1;
                                }
                            }

                        }


                    }


                }
                return 1;
            }
            catch (Exception ex)
            {
              
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;

            }
            
        }
        //private string Get_ADMIN_APPROVED_UserEmils()
        //{
        //    DataSet oDs = new DataSet();
        //    string emails = "";

        //    string userid = Session["USER_ID"].ToString();
        //    if (userid == "")
        //        userid = "0";


        //    try
        //    {

        //        oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_APPROVED_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
        //        if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
        //        {

        //            oDs.Tables[0].TableName = "Users";

        //            foreach (DataRow rItem in oDs.Tables["Users"].Rows)
        //            {
        //                if (rItem["EMAILADDR"].ToString() != "")
        //                    emails = emails + rItem["EMAILADDR"].ToString() + ",";

        //            }
        //            if (emails != "")
        //                emails = emails.Substring(0, emails.Length - 1) + "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return emails;
        //}
        

        private int SendMail_AfterPaymentSP(int OrderId, int OrderStatus, int user_id, int user_role)
        {
            string toemail = "";
            try
            {


                string BillAdd;
                string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

                oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);
                user_id = oOrderInfo.UserID;
                int UserID = user_id;

                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                user_role = oUserInfo.USERROLE;
              //  oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
              
                BillAdd = GetBillingAddress(OrderId);
                ShippAdd = GetShippingAddress(OrderId);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;




              //  oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
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
                   // StringTemplate _stmpl_recordsrows = null;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                    //StringTemplateGroup _stg_container1 = null;
                    //StringTemplateGroup _stg_records1 = null;
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                   // int ictrows = 0;

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

                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedAfterPay");

                    _stmpl_container.SetAttribute("PAY_METHOD", "Credit Card SP");
                    _stmpl_container.SetAttribute("AMOUNT", oOrderInfo.TotalAmount);
                    _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID);
                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                   
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


                }
                catch (Exception ex)
                {
                 
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";

                }
                if (sHTML != "")
                {
                   


                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());





                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    if (user_role == 1 || user_role == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        MessageObj.Bcc.Add(webadminmail);

                        
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
                    
                    MessageObj.Subject = "Bigtop Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
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
                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                   // return 1;


                    if (user_role == 1 || user_role == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());

                            smtpclient.Send(MessageObj);
                            objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                           // return 1;
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
                                      //  return 1;
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
                                   // return 1;
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
                                        //return 1;
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
                                    //MessageObj.CC.Add(emails.ToString());
                                    //return 1;
                                }
                            }

                        }


                    }


                }
                return 1;
            }
            catch (Exception ex)
            {
              
                objHelperServices.Mail_Log("SP", OrderId, "", ex.Message);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;

            }
            //return -1;
        }

        private int SendMail_AfterPaymentPP(int OrderId, int OrderStatus, int user_id, int user_role)
        {
            string toemail = "";
            try
            {
               

                string BillAdd;
                string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

                oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);
                user_id = oOrderInfo.UserID;
                int UserID = user_id;

                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                user_role = oUserInfo.USERROLE;
               // oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
                BillAdd = GetBillingAddress(OrderId);
                ShippAdd = GetShippingAddress(OrderId);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;




            //    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
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
                   // StringTemplate _stmpl_records1 = null;
                  //  StringTemplate _stmpl_recordsrows = null;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                    //StringTemplateGroup _stg_container1 = null;
                    //StringTemplateGroup _stg_records1 = null;
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    //int ictrows = 0;

                    DataSet dscat = new DataSet();
                    //DataTable dt = null;
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
                   
                    //objErrorHandler.CreateLog(sHTML);
                }
                catch (Exception ex)
                {
                  
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";

                }
                if (sHTML != "")
                {
                   

                    
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    if (user_role == 1 || user_role == 2)
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

                  
                    MessageObj.Subject = "Bigtop Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
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
                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                 
                    //return 1;

                    if (user_role == 1 || user_role == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                            smtpclient.Send(MessageObj);
                            objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                            //return 1;
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
                                        objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                       // return 1;
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
                                    //return 1;
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
                                        //return 1;
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
                                   // return 1;
                                }
                            }

                        }


                    }


                }
                return 1;
            }
            catch (Exception ex)
            {
             
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;

            }
            //return -1;
        }


        private int SendMail_AfterPayment(int OrderId, int OrderStatus, int user_id, int user_role)
        {
            
            try
            {


                string BillAdd;
                string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

             
                oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);
                user_id = oOrderInfo.UserID;
              
                int UserID = oOrderInfo.UserID;
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                user_role = oUserInfo.USERROLE;
              //  oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
                BillAdd = GetBillingAddress(OrderId);
                ShippAdd = GetShippingAddress(OrderId);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;




              //  oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;


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
                    StringTemplate _stmpl_records1 = null;
                    StringTemplate _stmpl_recordsrows = null;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                    StringTemplateGroup _stg_container1 = null;
                    StringTemplateGroup _stg_records1 = null;
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    //  stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"].ToString());
                    int ictrows = 0;

                    DataSet dscat = new DataSet();
                    DataTable dt = null;
                    _stg_records = new StringTemplateGroup("row", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);


                    lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];



                    int ictrecords = 0;
                    
                    foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                    {
                        // if (websiteid == 3)
                        _stmpl_records = _stg_records.GetInstanceOf("mail-order-payment-success" + "\\" + "row");
                        // else
                        //   _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");

                        _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                        _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }

                  
                    _stmpl_container = _stg_container.GetInstanceOf("mail-order-payment-success" + "\\" + "OrderSubmitted");
                

                    _stmpl_container.SetAttribute("CONNOTNO", oOrderInfo.TrackingNo);
                    _stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                    _stmpl_container.SetAttribute("SHIPPEDBY", oOrderInfo.ShipCompany);
                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                   
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
                   
                }
                catch (Exception ex)
                {
                    
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";
                }
                if (sHTML != "")
                {
                    


                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                    string emails = "";
                    string Adminemails = "";
                    
                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                       
                    }
                    else
                    {
                        emails = Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());


                    }

                   
                    MessageObj.Subject = "Bigtop Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
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
                    



                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
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
                return 1;
            }
            catch (Exception ex)
            {
                
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }

           
        }

        private string Get_ADMIN_APPROVED_UserEmils(string user_id)
        {
            DataSet oDs = new DataSet();
            string emails = "";

            string userid = user_id;
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

    


        // merges two NVCs

        #region "Function.."
  
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

        public string GetShippingAddress(int OrderID)
        {
            string sShippingAddress = "";
            OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
            oOI = objOrderServices.GetOrder(OrderID);

            //if (oOI.ShipCompName.Trim().Length > 0)
            //    sShippingAddress = oOI.ShipCompName + "<BR>";
            //else
            //    sShippingAddress = "";

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

            return sShippingAddress;
        }
        public string GetBillingAddress(int OrderID)
        {
            string sBillingAddress = "";
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
           
            //if (oBI.BillcompanyName.Trim().Length > 0)
            //    sBillingAddress = oBI.BillcompanyName + "<BR>";
            //else
            //    sBillingAddress = "";

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

            sBillingAddress = sBillingAddress + "<BR><p style='color:white'>...</p>";




            return sBillingAddress;
        }
    }
