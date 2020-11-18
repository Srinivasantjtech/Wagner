using System;
using System.Collections.Generic;

using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;



    public partial class PaySP : System.Web.UI.Page
    {
       
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();

        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

        SecurePayService objSecurePayService = new SecurePayService();
     
        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";

        public int UserID = 0; 

        string environment = "test"; // Change to "live" to process real transactions.
        // (For a live transaction, you must use a real, valid CC and billing address.)

        public int OrderID = 0;
        public string Txn_id = "";
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
        DataTable tblPaymentInfo = new DataTable();

        string strOrderLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        string strPaymentLink = "<br/><a href=\"PaymentHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        string strBackLink = "";
        string renUrl =HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string sb;
            sb = "<script language=javascript>\n";
            sb += "window.history.forward(1);\n";
            sb += "\n</script>";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);


            sb = "";
            sb = "<script type=javascript>\n";
            sb += "window.onload = function () { Clear(); }\n";
            sb += "function Clear() { \n";
            sb += " var Backlen=history.length; \n";
            sb += " if (Backlen > 0) history.go(-Backlen); \n";
            sb += "\n}</script>";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);



        }
       
       
        protected void ImgBtnEditShipping_Click(object sender, EventArgs e)
        {
            if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
            {
                if (OrderID>0)
                {
                    Response.Redirect("/orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + OrderID.ToString(), false);
                }
                else
                {
                    Response.Redirect("/orderDetails.aspx?&bulkorder=1&Pid=0", false);
                }
            }

            //Response.Redirect("Shipping.aspx?shipping.aspx?OrderID=" + OrderID + "&ApproveOrder=Approve", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Buffer = true;
            ////Response.ExpiresAbsolute = DateTime.Now;
            //Response.ExpiresAbsolute = DateTime.Now.AddHours(-1);


            //Response.Expires = 0;
            //Response.AddHeader("Expires", "-1");
            //Response.AddHeader("pragma", "no-cache");
            //Response.AddHeader("cache-control", "private");
            //Response.CacheControl = "no-cache";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();

           
            string output = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //Response.ClearHeaders();
            //Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
            //Response.AppendHeader("Cache-Control", "private"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1
            //Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.1
            //Response.AppendHeader("Keep-Alive", "timeout=3, max=993"); // HTTP 1.1
            //Response.AppendHeader("Expires", "Mon, 26 Jul 1997 05:00:00 GMT"); // HTTP 1.1
            div2.Visible = false;
            if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
            {
                UserID = Convert.ToInt32(Session["USER_ID"].ToString());
            }
            else
            {
                divTimeout.Visible = true;
                divCC.Visible = false;
                return;
                //Response.Redirect("/Login.aspx");
            }
            LoadIds();

            oUserInfo = objUserServices.GetUserInfo(UserID);

            //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower()=="au" )
            //{
            //    div1.InnerHtml = "";
            //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
            //    return;
            //}

            if (oPayInfo.PayResponse.ToLower() == "yes")
            {
                div1.InnerHtml = "";
                div2.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
                div2.Visible = true;
                return;
            }
            if (IsPostBack)
            {
              
                return;
            }
          
           
            

            if (!IsPostBack)
            {
                string BillAdd;
                string ShippAdd;

                oPayInfo = objPaymentServices.GetPayment(OrderID);
                oOrderInfo = objOrderServices.GetOrder(OrderID);
                BillAdd = BuildBillAddress();
                ShippAdd = BuildShippAddress();
                lblDeliveryTo.Text = BillAdd;
                lblShipTo.Text = ShippAdd;
                initcontrol();
                //lblOrderNo.Text = " : " + oPayInfo.PORelease;
                LoadOrderItem();
                //renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
                //renUrl = renUrl + "?key=" + EncryptSP(OrderID.ToString()) + "&";

            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
        }
        protected void btnSecurePayLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("paysp.aspx?" + EncryptSP(OrderID.ToString()), false);
        }

        protected void btnPayPalPayLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false);
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
        private void initcontrol()
        {

            txtCardNumber.Attributes.Add("onkeypress", "javascript:return Numbersonlypay(event);");
            txtCardCVVNumber.Attributes.Add("onkeypress", "javascript:return Numbersonlypay(event);");
            LoadCardList();
            for (int y = DateTime.Now.Year ; y <= DateTime.Now.Year + 20; y++)
            {
                drpExpyear.Items.Add(y.ToString());
            }
        }
        public void LoadCardList()
        {
            try
            {
                //DataSet oDs = new DataSet();
                //oDs = objSecurePayService.GetCardList();
                //drppaymentmethod.Items.Clear();
                //drppaymentmethod.DataSource = oDs;
                //drppaymentmethod.DataValueField = oDs.Tables[0].Columns["CARD_ID"].ToString();
                //drppaymentmethod.DataTextField = oDs.Tables[0].Columns["CARD_TYPE"].ToString();
                //drppaymentmethod.DataBind();

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
        }
        protected void OnClick_Cancel(object sender, EventArgs e)
        {
            Response.Redirect("OrderHistory.aspx");
        }
        //protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    if (isValidCreditCardNumber(drppaymentmethod.SelectedValue.ToString(), txtCardNumber.Text) == false)
        //    {
        //        args.IsValid = false;
        //    }
        //    else
        //    {
        //        args.IsValid = true;
        //    }
        //}
        protected void btnSecurePay_Click(object sender, EventArgs e)
        {


     

            string rtnstr = "";
            try
            {
                txtCardNumber.Style.Remove("border");
                drpExpmonth.Style.Remove("border");
                drpExpyear.Style.Remove("border");
                txtCardCVVNumber.Style.Remove("border");
                //drppaymentmethod.Style.Remove("border");
            }
            catch
            {
            }

            SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
            try
            {

                LoadIds();
                if (oPayInfo.PayResponse.ToLower() == "yes")
                {
                    div1.InnerHtml = "";
                    div2.InnerHtml = "Already Payment has been made" + strBackLink;
                    div2.Visible = true;
                    return;
                }

                if (Session["XpayC"] != null)
                {
                    if (Convert.ToInt32(Session["XpayC"]) > 3)
                    {
                        div1.InnerHtml = "";
                        div2.InnerHtml = "More than 3 attempt. try again" + "<br/><a href=\"PaySP.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"toplinkatest\" >Back</a>";
                        div2.Visible = true;
                        return;
                    }
                    else
                        Session["XpayC"] = Convert.ToInt32(Session["XpayC"]) + 1;
                }
                else
                    Session["XpayC"] = 0;



                //objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, drppaymentmethod.SelectedValue, txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
                objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);

                if (objPRInfo.Error_Text != "")
                {                  
                    btnPay.Style.Add("display", "block");                    
                    BtnProgress.Style.Add("display", "none");
                    ImgBtnEditShipping.Style.Add("display", "block");

                    div2.InnerHtml = " Error found in details you have entered. Please check all fields for errors and try again."; //objPRInfo.Error_Text;
                    div2.Visible = true;
                    HttpContext.Current.Session["payflowresponse"]="FAIL";

                    if (objPRInfo.Error_Text.ToLower().Contains("card number") == true)
                        txtCardNumber.Style.Add("border", "1px solid #FF0000");

                    if (objPRInfo.Error_Text.ToLower().Contains("cvv") == true || objPRInfo.Error_Text.ToLower().Contains("do not honour") == true)
                        txtCardCVVNumber.Style.Add("border", "1px solid #FF0000");

                    if (objPRInfo.Error_Text.ToLower().Contains("date") == true || objPRInfo.Error_Text.ToLower().Contains("expired") == true)
                    {
                        drpExpmonth.Style.Add("border", "1px solid #FF0000");
                        drpExpyear.Style.Add("border", "1px solid #FF0000");
                    }
                    if (objPRInfo.Error_Text.ToLower().Contains("card type") == true )
                    {
                        //drppaymentmethod.Style.Add("border", "1px solid #FF0000");                       
                    }

                }
                else
                {
                    //Session["Pay"] = "End";
                    Session["XpayC"] = null;
                    div1.InnerHtml = "";
                    //div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX " + OrderID.ToString() + " Payment succeeded" + strBackLink;
                    //div2.InnerHtml = "";
                    div2.Visible = false;
                    HttpContext.Current.Session["payflowresponse"]="SUCCESS";
                    HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
                    Response.Redirect("BillinfoSP.aspx");
                }



            }
            catch (Exception ex)
            {
            }

            //LoadIds();
            //renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
            //renUrl = renUrl + "?key=" + EncryptSP(OrderID.ToString()) ;
            
            //oOrderInfo = objOrderServices.GetOrder(OrderID);
            //oUserInfo = objUserServices.GetUserInfo(UserID);

            ////if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
            ////{
            ////    div1.InnerHtml = "";
            ////    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
            ////    return;
            ////}
            //if (oPayInfo.PayResponse.ToLower() == "yes")
            //{
            //    //divContent.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
            //    return;
            //}
            
            //string Requeststr = objSecurePayService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);

            //if (Requeststr.Contains("Form") == false)
            //    //divContent.InnerHtml = Requeststr;
            //else
            //    this.Page.Controls.Add(new LiteralControl(Requeststr));
             
        }

        private void LoadIds()
        {
            if (Request.Url.Query != null && Request.Url.Query != "")
            {

                string id = Request.Url.Query.Replace("?", "");
                id = DecryptSP(id);
                string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);


                OrderID = objHelperServices.CI(ids[0]);
                if (ids.Length > 1)
                {
                    Txn_id = ids[1];
                    strBackLink = strPaymentLink;
                }
                else
                    strBackLink = strOrderLink;


            }
            else
            {
                div1.InnerHtml = "";
                div2.InnerHtml = "Invalid Data" + strPaymentLink;
                div2.Visible = true;
                return;

            }
            oPayInfo = objPaymentServices.GetPayment(OrderID);
            PaymentID = oPayInfo.PaymentID;
            
           // lblTotalAmount1.Text = oPayInfo.Amount.ToString();

        }
        private void LoadOrderItem()
        {
            dsOItem = objOrderServices.GetOrderItems(OrderID);


            OrderitemdetailRepeater.DataSource = dsOItem;
            OrderitemdetailRepeater.DataBind();

            Product_Total_price.Text = oOrderInfo.ProdTotalPrice.ToString();
            Tax_amount.Text = oOrderInfo.TaxAmount.ToString();
            Total_Amount.Text = oOrderInfo.TotalAmount.ToString();  
            if (objOrderServices.IsNativeCountry(OrderID) == 0)
                {
                    lblTotalCap.Text = "Total";
                }
                else
                {
                    lblTotalCap.Text = "Total Inc GST";
                }
            lblCourier.Text = oOrderInfo.ShipCost.ToString();
            lblAmount.Text = oPayInfo.Amount.ToString();

            //lblSubTotal1.Text = oOrderInfo.ProdTotalPrice.ToString();
           // lblTaxAmount1.Text = oOrderInfo.TaxAmount.ToString();
           // lblCourierAmt1.Text = oOrderInfo.ShipCost.ToString();
           // lblTotalAmount1.Text = oOrderInfo.TotalAmount.ToString();

        }
       
       
        private string Setdrpdownlistvalue(DropDownList d, string val)
        {
            ListItem li;
            string returnselected = "";
            for (int i = 0; i < d.Items.Count; i++)
            {
                li = d.Items[i];
                if (li.Value.ToUpper() == val.ToUpper())
                {
                    d.SelectedIndex = i;
                    returnselected = li.Text.ToUpper();
                    break;
                }
            }
            return returnselected;
        }
       


        #region "Function.."
        public string BuildBillAddress()
        {
            try
            {
                string sBillAdd = "";

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
        public string BuildShippAddress()
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

        #endregion'

        
   // private bool isValidCreditCardNumber(string type, string ccnum)
   // {
   //     string regExp = "";

   //    if (type == "5") {
   //   // Visa: length 16, prefix 4, dashes optional.^4\d{3}-?\d{4}-?\d{4}-?\d{4}$---->
   //    regExp   = "/^4[0-9]{12}(?:[0-9]{3})?$/";
   //} else if (type == "6") {
   //   // Mastercard: length 16, prefix 51-55, dashes optional.
   //    regExp   = "/^5[1-5][0-9]{14}$/";
   //}  else if (type == "2") 
   //    {
   //   // American Express: length 15, prefix 34 or 37.^3[47][0-9]{13}$--->^3[4,7]\d{13}$
   //   regExp   = "/^3[47][0-9]{13}$/";
   //}        

   ////    else if (type == "Diners") {
   ////   // Diners: length 14, prefix 30, 36, or 38.^3(?:0[0-5]|[68][0-9])[0-9]{11}$--->^3[0,6,8]\d{12}$
   ////   regExp   = "/^(30[0-5]|3[68][0-9]|54\d{3}|55\d{3})[0-9]{11}$/";
   ////} 
   ////    else if (type == "JCB") {
   ////   // JCB cards beginning with 2131 or 1800 have 15 digits. JCB cards beginning with 35 have 16 digits.^(?:2131|1800|35\d{3})\d{11}$--->^(?:2131|1800|35\d{3})\d{11}$
   ////   regExp   = "/^(?:2131|1800|3528\d{1}|3529\d{1}|35[3-8][0-9]\d{1})\d{11}$/;///^(?:2131|1800|3528|3529|35[3-8][0-9])\d{11}$/";
   ////}
   ////    else if (type == "Disc") {
   ////   // Discover: length 16, prefix 6011, dashes optional.^6011-?\d{4}-?\d{4}-?\d{4}$--->6011, 622126-622925, 644-649, 65
   ////    regExp   = "/^6(?:011|5[0-9]{2}|4[4-9]\d{1})[0-9]{12}|622(12[6-9]|1[3-9][0-9]|[2-8][0-9][0-9]|9[01][0-9]|92[0-5])[0-9]{10}$/";
   ////}

   //     if (!Regex.IsMatch(ccnum, regExp))
   //         return false;

   //     string[] tempNo = ccnum.Split('-');
   //     ccnum = String.Join("", tempNo);

   //     int checksum = 0;
   //     for (int i = (2 - (ccnum.Length % 2)); i <= ccnum.Length; i += 2)
   //     {
   //         checksum += Convert.ToInt32(ccnum[i - 1].ToString());
   //     }

   //     int digit = 0;
   //     for (int i = (ccnum.Length % 2) + 1; i < ccnum.Length; i += 2)
   //     {
   //         digit = 0;
   //         digit = Convert.ToInt32(ccnum[i - 1].ToString()) * 2;
   //         if (digit < 10)
   //         { checksum += digit; }
   //         else
   //         { checksum += (digit - 9); }
   //     }
   //     if ((checksum % 10) == 0)
   //         return true;
   //     else
   //         return false;

   // }
    }


