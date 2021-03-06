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
using System.Security.Cryptography;
using System.Security;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class OrderDet_uc : System.Web.UI.UserControl
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();
    PaymentServices objPaymentServices = new PaymentServices();
    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
    UserServices objUserServices = new UserServices();
    UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
    Security objSecurity = new Security();
   public  const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
    int OrderID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        
        int OrderID = 0;
        //  Page.Title = oHelper.GetOptionValues("BROWSER TITLE"].ToString();
       // objErrorHandler.CreateLog("inside orderdet ascx"); 
        if (string.IsNullOrEmpty(Request["OrderId"]))
        {
            //Added By indu to bind order item when clicked from mail


            GetParams();
        


             if (OrderID == 0)
             {
                 OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
             }

        }


        else
        {
            OrderID = objHelperServices.CI(Request["OrderId"].ToString());
        }

         string BillAdd;
        string ShippAdd;

        oPayInfo = objPaymentServices.GetPayment(OrderID);
        oOrderInfo = objOrderServices.GetOrder(OrderID);
        int UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        oUserInfo = objUserServices.GetUserInfo(UserID);
        //lblOrderID.Text = "Order No: " + oPayInfo.PORelease;
        BillAdd = BuildBillAddress();
        ShippAdd = BuildShippAddress();
        //lblBillContent.Text = BillAdd;
        //lblShippContent.Text = ShippAdd;
        //if (BillAdd == "")
        //{
        //    lblBillAdd.Visible = false;
        // }
        if (ShippAdd == "")
        {
            //lblSAdd.Visible = false;
        }
        if (oOrderInfo.ShipCompany == null)
        {
            //lblShipProvider.Visible = false;
            //lblShippProName.Visible = false;
        }
        else
        {
            //lblShippProName.Text = oOrderInfo.ShipCompany;
        }
        if (oOrderInfo.ShipMethod == null)
        {
            //lblShipMethod.Visible = false;
            //lblShipMethodName.Visible = false;
        }
        else
        {
            //lblShipMethodName.Text = oOrderInfo.ShipMethod;
        }

    }

    private void GetParams()
    {
        try
        {
           // objErrorHandler.CreateLog("inside getparams in orderdet ascx"); 
            if (Request.Url.Query != null && Request.Url.Query != "")
            {

                string id = null;
                if (Request.Url.Query.Contains("key=") == false)
                {

                    id = Request.Url.Query.Replace("?", "");
                    int n;
                    bool isNumeric = int.TryParse(id, out n);
                    //  objErrorHandler.CreateLog("Getparams" + id);
                    id = DecryptSP(id);
                  //  objErrorHandler.CreateLog("inside getparams "+ id); 

                    if ((id == null))
                    {
                        id = Request.Url.Query.Replace("?", "");

                        if (id.Contains("PaySP") == false)
                        {
                            id = id + "#####" + "PaySP";
                        }
                    }
                }

               
                if (id != null)
                {
                    string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);

                    OrderID = objHelperServices.CI(ids[0]);

  //objErrorHandler.CreateLog("getparam orderid"+ OrderID.ToString());
                 

                  
                }
                else
                {
                    OrderID = 0;
                  
                    //  div1.Visible = false;
                    //  return 0;
                }

            }
            else
            {
                OrderID = 0;
              
                // return 0;
            }
            //  return 1;
        }
        catch (Exception ex)
        {
            //  return 0;
            objErrorHandler.CreateLog(ex.ToString());
        }

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
    #region "Function.."
    //public string BuildBillAddress()
    //{
    //    Payment.PayInfo oBillInfo;
    //    oBillInfo = (Payment.PayInfo)Session["BILLING INFO"];

    //    oOrder.GetOrder(OrderID);
    //    string sBillAdd = "";
    //    if (oOrderInfo.BillFName != null)
    //    {
    //        sShippAdd = oOrderInfo.ShipFName + "<br>";
    //        sBillAdd = sBillAdd + oOrderInfo.BillFName + " ";
    //    }
    //    if (oOrderInfo.BillLName != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillLName + "<br>";
    //    }
    //    if (oOrderInfo.BillAdd1 != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd1 + "<br>";
    //    }
    //    if (oOrderInfo.BillAdd2 != "")
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd2 + "<br>";
    //    }
    //    else
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd2;
    //    if (oOrderInfo.BillAdd3 != "")
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd3 + "<br>";
    //    }
    //    else
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd3;
    //    }
    //    if (oOrderInfo.BillCity != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillCity + "<br>";
    //    }
    //    if (oOrderInfo.BillState != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillState + "<br>";
    //    }
    //    if (oOrderInfo.BillCountry != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillCountry + "<br>";
    //    }
    //    if (oOrderInfo.BillZip != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillZip + "<br>";
    //    }
    //    if (oOrderInfo.BillPhone != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillPhone + "<br>";
    //    }
    //    return sBillAdd;
    //}
    public string BuildBillAddress()
    {
        try
        {
            string sBillAdd = "";
            if (oOrderInfo.BillFName != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillFName) + " ";
            }
            if (oOrderInfo.BillMName != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillMName) + " ";
            }
            if (oOrderInfo.BillLName != null)
            {
                sBillAdd = sBillAdd  + WrapText(oOrderInfo.BillLName) + "<br>";
            }
            if (oOrderInfo.BillAdd1 != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd1) + "<br>";
            }
            if (oOrderInfo.BillAdd2 != null && oOrderInfo.BillAdd2 != "")
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2) + "<br>";
            }
            else
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2);
            if (oOrderInfo.BillAdd3 != null && oOrderInfo.BillAdd3 != "")
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd3) + "<br>";
            }
            else
            {
                sBillAdd = sBillAdd +WrapText(oOrderInfo.BillAdd3);
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
            if (oOrderInfo.ShipAdd2 !=  null && oOrderInfo.ShipAdd2 != "")
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2) + "<br>";
            }
            else
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2);
            if (oOrderInfo.ShipAdd3 !=null &&  oOrderInfo.ShipAdd3 != "")
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
                sShippAdd = sShippAdd + "Phone No:"+WrapText(oOrderInfo.ShipPhone) + "<br>";
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
        if (BillAdd != null)
        {
            if (BillAdd.Length > 50 & BillAdd.Length <= 100)
                BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51) + newline;
            else if (BillAdd.Length > 100 & BillAdd.Length <= 150)
                BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51, 49) + newline + BillAdd.Substring(101);
            return BillAdd;
        }
        return "";
    }
    #endregion
}
