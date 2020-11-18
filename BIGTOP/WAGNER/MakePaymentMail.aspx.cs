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
public partial class MakePaymentMail : System.Web.UI.Page
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
        //string key = "";
        protected override void OnPreRender(EventArgs e)
        {
            //base.OnPreRender(e);
            //string sb;
            //sb = "<script language=javascript>\n";
            //sb += "window.history.forward(1);\n";
            //sb += "\n</script>";
            //ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);


            //sb = "";
            //sb = "<script type=javascript>\n";
            //sb += "window.onload = function () { Clear(); }\n";
            //sb += "function Clear() { \n";
            //sb += " var Backlen=history.length; \n";
            //sb += " if (Backlen > 0) history.go(-Backlen); \n";
            //sb += "\n}</script>";
            //ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);



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
            

            string output = "";
            if (Request.QueryString["Resendmail"] == null)
            {
                try
                {
                    int rtnmail = 0;
                    DataTable temptbl = objOrderServices.GetOrder_Mate_Payment_mail_detail();
                    if (temptbl != null && temptbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in temptbl.Rows)//For Records
                        {
                            try
                            {
                                rtnmail = SendMail(Convert.ToInt32(dr["ORDER_ID"].ToString()), (int)OrderServices.OrderStatus.Proforma_Payment_Required, true);
                                if (rtnmail == 1)
                                {
                                    int flgsentmail = 0;
                                    if (dr["MAKE_PAYMENT_MAIL"].ToString() == "0")
                                        flgsentmail = 1;
                                    //else if (dr["MAKE_PAYMENT_MAIL"].ToString() == "1")
                                    //    flgsentmail = 2;

                                    //else
                                    //    flgsentmail = 1;
                                    //objOrderServices.UpdateMake_Payment_mail_status(Convert.ToInt32(dr["ORDER_ID"].ToString()), 1);
                                    objOrderServices.UpdateMake_Payment_mail_status(Convert.ToInt32(dr["ORDER_ID"].ToString()), flgsentmail);
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
            else
            {

                try
                {

                    int rtnmail = 0;
                    DataTable temptbl = objOrderServices.GET_MAKE_PAYMENT_ORDERS_RESEND();
                    if (temptbl != null && temptbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in temptbl.Rows)//For Records
                        {
                            try
                            {
                                rtnmail = SendMail(Convert.ToInt32(dr["ORDER_ID"].ToString()), (int)OrderServices.OrderStatus.Proforma_Payment_Required, true);
                                if (rtnmail == 1)
                                {
                                    int flgsentmail = 0;
                                    if (dr["MAKE_PAYMENT_MAIL"].ToString() == "0")
                                        flgsentmail = 1;

                                    else if (dr["MAKE_PAYMENT_MAIL"].ToString() == "1")
                                        flgsentmail = 2;
                                    else if (dr["MAKE_PAYMENT_MAIL"].ToString() == "2")
                                        flgsentmail = 3;
                                    else if (dr["MAKE_PAYMENT_MAIL"].ToString() == "3")
                                        flgsentmail = 4;
                                    else if (dr["MAKE_PAYMENT_MAIL"].ToString() == "4")
                                        flgsentmail = 5;
                                    else
                                        flgsentmail = 1;
                                    //objOrderServices.UpdateMake_Payment_mail_status(Convert.ToInt32(dr["ORDER_ID"].ToString()), 1);
                                    objOrderServices.UpdateMake_Payment_mail_status(Convert.ToInt32(dr["ORDER_ID"].ToString()), flgsentmail);
                                }
                            }
                            catch (Exception er)
                            {
                                objErrorHandler.ErrorMsg = er;
                                objErrorHandler.CreateLog();
                            }
                        }
                    }

                    SendMail_admin();





                }
                catch (Exception ex)
                {

                }
            
            
            
            
            
            
            
            
            
            }

        }


        private int SendMail_admin()
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


                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;




                //  oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);

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
                    DataTable temptbl = objOrderServices.Awaiting_Make_Payment_mail_detail();
                    
                    //  dsOItem.Tables.Add(temptbl);
                    if (temptbl != null && temptbl.Rows.Count > 0)
                    {
                        // DataTable dt = null;
                        objErrorHandler.CreateLog("inside sendmail_admin" + temptbl.Rows.Count );
                        _stg_records = new StringTemplateGroup("row", stemplatepath);
                        _stg_container = new StringTemplateGroup("main", stemplatepath);

                        if (dsOItem != null)
                        {
                            lstrecords = new TBWDataList[temptbl.Rows.Count + 1];

                            int ictrecords = 0;

                            try
                            {
                                foreach (DataRow dr in temptbl.Rows)//For Records
                                {
                                    // objErrorHandler.CreateLog("Order_id " + dr["Order_id"].ToString() + " TOTAL_AMOUNT "+ dr["TOTAL_AMOUNT"].ToString());
                                    //if (websiteid == 3)
                                    //   _stmpl_records = _stg_records.GetInstanceOf("mail-wagner" + "\\" + "row");
                                    //   else
                                    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "Row_AwaitingPayment");
                                    if (dr["WebSite"].ToString() == "1")
                                    {
                                        _stmpl_records.SetAttribute("WebSite", "WES");
                                    }
                                    else if (dr["WebSite"].ToString() == "2")
                                    {
                                        _stmpl_records.SetAttribute("WebSite", "CELLINK");
                                    }
                                    else if (dr["WebSite"].ToString() == "3")
                                    {
                                        _stmpl_records.SetAttribute("WebSite", "WAGNER");
                                    }
                                    else if (dr["WebSite"].ToString() == "4")
                                    {
                                        _stmpl_records.SetAttribute("WebSite", "WIRETEK");
                                    }
                                    else if (dr["WebSite"].ToString() == "5")
                                    {
                                        _stmpl_records.SetAttribute("WebSite", "BIGTOP");
                                    }
                                _stmpl_records.SetAttribute("CustomerNo", dr["CustomerNo"].ToString());
                                    _stmpl_records.SetAttribute("WESInvoiceNo", dr["WESInvoiceNo"].ToString());
                                    _stmpl_records.SetAttribute("CustOrderNo", dr["CustOrderNo"].ToString());
                                    _stmpl_records.SetAttribute("TotalAmount", dr["TotalAmount"].ToString());
                                    _stmpl_records.SetAttribute("WebOrderID", dr["WebOrderID"].ToString());
                                    _stmpl_records.SetAttribute("CompanyName", dr["CompanyName"].ToString());
                                    _stmpl_records.SetAttribute("UserName", dr["UserName"].ToString());
                                    _stmpl_records.SetAttribute("ModifiedTime", dr["ModifiedTime"].ToString());

                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                    ictrecords++;
                                }
                            }
                            catch (Exception ex)
                            {
                                objErrorHandler.ErrorMsg = ex;
                                objErrorHandler.CreateLog();
                            }
                        }
                        else
                        {
                            //objErrorHandler.CreateLog("Order Item is null for orderid -" + oOrderInfo.OrderID);

                        }
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "AwaitingPayment");
                        _stmpl_container.SetAttribute("TBWDataList", lstrecords);






                        sHTML = _stmpl_container.ToString();
                    }

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
                    //objErrorHandler.CreateLog("Email Id Make Payment Send Mail" + Emailadd.ToString() + "inside if");

                    MessageObj.To.Add(System.Configuration.ConfigurationManager.AppSettings["Awaitingemail"].ToString());
                    // MessageObj.Bcc.Add(objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString());


                    //objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    MessageObj.Bcc.Add(System.Configuration.ConfigurationManager.AppSettings["Reviewemail"].ToString());

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "Order Awaiting Payment Notificaiton";
                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;
                    objErrorHandler.CreateLog(sHTML);

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
        //human-readable representation of an NVC

        private int SendMail(int OrderId, int OrderStatus, bool isau)
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

                int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
               // objErrorHandler.CreateLog(oOrderInfo.CreatedUser.ToString()+"Createduser");
               // oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
             //   objErrorHandler.CreateLog(oOrderInfo.UserID.ToString() + "UserID");
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
            //    objErrorHandler.CreateLog(OrderId.ToString() + "OrderId");
             //   objErrorHandler.CreateLog(dsOItem.Tables[0].Rows.Count.ToString() );
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

                        if (dsOItem != null)
                        {
                            lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];

                            int ictrecords = 0;

                            try
                            {
                                foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                                {
                                    //if (websiteid == 3)
                                    //   _stmpl_records = _stg_records.GetInstanceOf("mail-wagner" + "\\" + "row");
                                    //   else
                                    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "Row_Makepayment");

                                    _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                                    _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                    ictrecords++;
                                }
                            }
                            catch (Exception ex)
                            {
                                objErrorHandler.ErrorMsg = ex;
                                objErrorHandler.CreateLog();
                            }
                        }
                        else
                        {
                            //objErrorHandler.CreateLog("Order Item is null for orderid -" + oOrderInfo.OrderID);

                        }
                        //if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                        //if( websiteid == 3)
                        //   _stmpl_container = _stg_container.GetInstanceOf("mail-wagner" + "\\" + "OrderSubmitted");
                        //    else
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedMakePay");


                        //else
                        //    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");


                        //_stmpl_container.SetAttribute("CONNOTNO", oOrderInfo.TrackingNo);  
                        //_stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                        //_stmpl_container.SetAttribute("SHIPPEDBY", oOrderInfo.ShipCompany);
                        _stmpl_container.SetAttribute("PAY_METHOD", "PAY_METHOD");
                        _stmpl_container.SetAttribute("CANCEL_ORDER", "CANCEL_ORDER");
                        _stmpl_container.SetAttribute("AMOUNT", oOrderInfo.TotalAmount);
                        _stmpl_container.SetAttribute("DELIVERY_CHARGE", oOrderInfo.ShipCost);
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

                    sHTML = sHTML.Replace("PAY_METHOD", "http://" + url1 + "/checkout.aspx?" + oOrderInfo.OrderID.ToString()+ "#####" + "PaySP" ); 
                    sHTML = sHTML.Replace("CANCEL_ORDER", "http://" + url1 + "/checkout.aspx?" + oOrderInfo.OrderID.ToString()+ "#####" + "CancelOrder=true"); 
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
      
                    
               

                    //string emails = "";
                    //string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    //objErrorHandler.CreateLog("Email Id Make Payment Send Mail" + Emailadd.ToString() + "inside if");
                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                    
                        MessageObj.To.Add(Emailadd.ToString());
                       // MessageObj.Bcc.Add(objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString());
                    }
                    else
                    {
                      

                        MessageObj.To.Add(Emailadd.ToString());

                       // MessageObj.Bcc.Add(objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString());
                    }

                    //objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    MessageObj.Bcc.Add(webadminmail);
                   
                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "Bigtop Order Payment Notification - Order No : " + CustomerOrderNo;
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

                //  sShippingAddress = sShippingAddress + oOI.ShipFName + oOI.ShipLName + "<BR>";
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
        public string GetBillingAddress(int OrderID)
        {
           
            string sBillingAddress = "";
            try
            {
                OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
                oBI = objOrderServices.GetOrder(OrderID);
                //if (oBI.BillcompanyName.Trim().Length > 0)
                //    sBillingAddress = oBI.BillcompanyName + "<BR>";
                //else
                //    sBillingAddress = "";

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
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }


            return sBillingAddress;
        }
    }
