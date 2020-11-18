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
using System.Globalization;

namespace WES
{
    public partial class MakeOpenOrder_abo : System.Web.UI.Page
    {
        OrderServices objOrderServices = new OrderServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        Security objSecurity = new Security();
        protected void Page_Load(object sender, EventArgs e)
        {
                  DataTable temptbl1 = objOrderServices.Get_Order_Open_Status_Details_ABO();
                if (temptbl1 != null && temptbl1.Rows.Count > 0)
                {
                    foreach (DataRow dr in temptbl1.Rows)//For Records
                    {
                        try
                        {
                          
                         int   rtnmail = SendMail_Abordant(Convert.ToInt32(dr["ORDER_ID"].ToString()), Convert.ToInt32(dr["ORDER_STATUS"].ToString()));

                        }
                        catch (Exception er)
                        {
                            objErrorHandler.ErrorMsg = er;
                            objErrorHandler.CreateLog();
                        }
                    }
                }

              
          
    
        }


        private int SendMail_Abordant(int OrderId, int OrderStatus)
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

                // oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);

                int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

                oUserInfo = objUserServices.GetUserInfo(UserID);
                //  oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
                // BillAdd = GetBillingAddress(OrderId);
                //  ShippAdd = GetShippingAddress(OrderId);

                // string ShippingMethod = oOrderInfo.ShipMethod;
                // string CustomerOrderNo = oPayInfo.PORelease;
                //  string shippingnotes = oOrderInfo.ShipNotes;

                if (dsOItem == null || dsOItem.Tables.Count <= 0)
                    return 0;


                // oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                //  string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                //  string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;


                string url = HttpContext.Current.Request.Url.Authority.ToString();
                // string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

                int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
                oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
                //string ApprovedUserEmailadd = oUserInfo.AlternateEmail;




                string sHTML = "";

                try
                {
                    StringTemplateGroup _stg_container = null;
                    StringTemplateGroup _stg_records = null;
                    StringTemplate _stmpl_container = null;
                    StringTemplate _stmpl_records = null;

                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];


                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());


                    DataSet dscat = new DataSet();
                    DataTable dt = null;
                    _stg_records = new StringTemplateGroup("row", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);


                    lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];

                    int ictrecords = 0;

                    foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                    {

                        _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "openrow_aba");

                        _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                        _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());
                        _stmpl_records.SetAttribute("Description", dr["Description"].ToString());
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }


                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderOpenStatus_aba");

                    _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID);

                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    string titlecase = textInfo.ToTitleCase(oUserInfo.Contact.ToLower());

                    _stmpl_container.SetAttribute("CONTACT", titlecase);

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                    string url1 = HttpContext.Current.Request.Url.Authority.ToString();
                    // url1 = url1+ "/checkout.aspx?" +EncryptSP(oOrderInfo.OrderID.ToString());

                    url1 = url1 + "/OrderDetails.aspx?ORDER_ID=" + oOrderInfo.OrderID.ToString() + "&bulkorder=1&ViewOrder=View&OrdFlg=" + objSecurity.StringEnCrypt_password("OPENORDER");
                    objErrorHandler.CreateLog(url1);

                    _stmpl_container.SetAttribute("TBT_CHKOUT_URL", url1);

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


                    //sHTML = sHTML.Replace("PAY_METHOD", "http://" + url1 + "/checkout.aspx?" + oOrderInfo.OrderID.ToString()+ "#####" + "PaySP" ); 
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    MessageObj.Subject = "Do you have questions or feedback?";
                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;
                    MessageObj.To.Add(Emailadd.ToString());
                    MessageObj.Bcc.Add("webadmin@wagneronline.com.au");
                    MessageObj.Bcc.Add("indumathi@jtechindia.com");
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