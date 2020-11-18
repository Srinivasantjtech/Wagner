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
using System.Text.RegularExpressions;
using System.Collections.Generic;
//using System.Windows.Forms;
using TradingBell.WebCat;
using GCheckout.Checkout;
using GCheckout.Util;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class OrderDetails : System.Web.UI.Page
{
    #region "Declarations"
    HelperServices objHelperServices = new HelperServices();
    HelperDB objHelperDB = new HelperDB();
    Security objSecurity = new Security();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();
    // CustomPrice oCustomPrice = new CustomPrice();
    UserServices objUserServices = new UserServices();
    NotificationServices objNotificationServices = new NotificationServices();
    //ConnectionDB objConnectionDB = new ConnectionDB();
    QuoteServices objQuoteServices = new QuoteServices();
    QuoteServices.QuoteInfo oQuoteInfo = new QuoteServices.QuoteInfo();
    QuoteServices.QuoteItemInfo oQuoteItemInfo = new QuoteServices.QuoteItemInfo();
    OrderServices.OrderItemInfo CobItemInFo = new OrderServices.OrderItemInfo();
    ProductServices objProductServices = new ProductServices();
    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
    int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
    BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();
    CountryServices objCountryServices = new CountryServices();
    DataSet QDs = new DataSet();
    OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender msgPop = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender msgAlert = new AjaxControlToolkit.ModalPopupExtender();
    const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
    public string FIXED_TAX = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX"].ToString();
    public string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
    string strFile = HttpContext.Current.Server.MapPath("ProdImages");
    int QtyAvail;
    int MinQtyAvail;
    //int ProductID = 0;
    int OrderID = 0;
    int AvlQty = 0;
    int CatalogID = 0;
    public int NewProduct_id = 0;
    public int NewQty = 0;
    int product_stock_status_sub = -1;
    int product_id_chk_sub = 0;
    int product_qty_chk_sub = 0;
   
    bool _IsShippingFree = false;
    string[] ProdID;
    string[] ord_itemID;
    // int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    #endregion "Declarations"
    protected void Page_PreInit()
    {
        if (Request.QueryString["popup"] != null)
        {
            Page.MasterPageFile = "~/AddtoCardPopup.Master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        String sessionId;
        sessionId = Session.SessionID;
        PopupMsg.Visible = false;
        try
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            ImageButton2.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"] + "images/spacer.gif";
            if (Session["PageUrl"] != null && Session["PageUrl"].ToString() != string.Empty)
            {
                if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=NOPRICEAMT"))
                {
                    Session["PageUrl"] = "ConfirmMessage.aspx?Result=QTEEMPTY";
                }
                else if ((Request.QueryString["ORDER_ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ORDER_ID"])) || (Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0))
                {
                    //Session["PageUrl"] = "OrderDetails.aspx?ORDER_ID=" + Request.QueryString["ORDER_ID"] + "&bulkorder=1&ViewOrder=View";
                }
                else
                {
                    // Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
                }
            }

          //  Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            Page.Title = "Bigtop-Order Details";
            string QuotePurchase = objHelperServices.GetOptionValues("QUOTEPURCHASE").ToString();
            string OrderPurchase = objHelperServices.GetOptionValues("ORDERPURCHASE").ToString();
            if (Session["CATALOGID"] != null && Session["CATALOGID"].ToString() != "")
            {
                CatalogID = objHelperServices.CI(Session["CATALOGID"]);
            }
            else
            {
                Session["CATALOGID"] = objHelperServices.CI(objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString());
                CatalogID = objHelperServices.CI(Session["CATALOGID"]);
            }

            if (Session["ShowPop"] != null)     // DIRECT CHECK OUT OPTION PURPOSE WHEN VIEW CART ITEMS AVAILABLE
            {
                if (Session["ShowPop"].ToString().Trim() == "Yes")
                {
                    ShowPopUpMessage();
                    Session["ShowPop"] = "";
                }
            }
            if (Request.QueryString["CombainProd_id"] != null && (Request.QueryString["ORDER_ID"] != null))
            {

                string LeaveDuplicateProds = GetLeaveDuplicateProducts();
                int order_id = Convert.ToInt32(Request.QueryString["ORDER_ID"].ToString());
                int PrdId = objHelperServices.CI(Request.QueryString["CombainProd_id"].ToString());

                DataSet dsDuplicateItem = objOrderServices.GetOrderItemsWithDuplicate(order_id, LeaveDuplicateProds, sessionId);
                if (dsDuplicateItem != null && dsDuplicateItem.Tables.Count > 0 && dsDuplicateItem.Tables[0].Rows.Count > 0)
                {
                    DataRow[] Dr = dsDuplicateItem.Tables[0].Select("PRODUCT_ID='" + PrdId + "'");

                    if (Dr.Length > 0)
                    {
                        DataTable temptbl = Dr.CopyToDataTable();


                        decimal TotQty = 0;

                        foreach (DataRow row in temptbl.Rows)
                        {
                            TotQty = TotQty + objHelperServices.CI(row["QTY"].ToString());
                            int nQty = objHelperServices.CI(row["QTY"].ToString());
                            string order_item_id = row["ORDER_ITEM_ID"].ToString();
                            objOrderServices.RemoveItem(PrdId.ToString(), order_id, objHelperServices.CI(Session["USER_ID"]), order_item_id.ToString());
                        }

                        AddOrderItem(order_id, objHelperServices.CI(Session["USER_ID"]), (int)TotQty, PrdId);

                        if (OrderID > 0)
                            oOrdInfo.OrderID = OrderID;
                        else
                            oOrdInfo.OrderID = order_id;
                        objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                    }

                }



            }
            if (Request.QueryString["LeaveProd_id"] != null && (Request.QueryString["ORDER_ID"] != null))
            {
                int order_id = Convert.ToInt32(Request.QueryString["ORDER_ID"].ToString());
                int PrdId = objHelperServices.CI(Request.QueryString["LeaveProd_id"].ToString());
                string LeaveDuplicateProds = "";
                if (Session["LeaveDuplicateProds"] != null && Session["LeaveDuplicateProds"].ToString() != "")
                {
                    LeaveDuplicateProds = Session["LeaveDuplicateProds"].ToString();

                    if (LeaveDuplicateProds.Contains("-" + PrdId + "-") == false && PrdId > 0)
                        LeaveDuplicateProds = LeaveDuplicateProds + ",-" + PrdId + "-";
                }
                else
                    LeaveDuplicateProds = "-" + PrdId + "-";

                Session["LeaveDuplicateProds"] = LeaveDuplicateProds;
            }

            if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString().ToUpper() == "YES")
            {
                if (!IsPostBack && (Session["ShowPop"] == null || Session["ShowPop"].ToString().TrimEnd(',') != "" || Session["ShowPop"].ToString().Trim() == ""))
                {

                    if (Request.QueryString["ORDER_ID"] != null)
                    {
                        if (Request.QueryString["ORDER_ID"].ToString().Trim() != "")
                        {
                            Session["ORDER_ID"] = Request.QueryString["ORDER_ID"];
                            OrderServices objOrderServices = new OrderServices();
                            DataTable oDt1 = objOrderServices.GetPendingOrderProducts();
                            Session["Multipleitems"] = "0";
                            Session["Multipleitems_id"] = "0";
                            if (oDt1 != null && oDt1.Rows.Count > 0 && sessionId !="")
                            {
                                foreach (DataRow oDr1 in oDt1.Rows)
                                {
                                    if (sessionId == oDr1["PRD_SESSION_ID"])
                                    {
                                        Session["Multipleitems"] = Session["Multipleitems"] + ", " + oDr1["PRODUCT_ID"];
                                        Session["Multipleitems_id"] = Session["Multipleitems_id"] + ", " + oDr1["ORDER_ITEM_ID"];
                                    }
                                }
                            }
                            else
                            {
                                if (Request.QueryString["ViewOrder"] == null)
                                    Session["ORDER_ID"] = "0";
                                Session["Multipleitems"] = null;
                                Session["Multipleitems_id"] = null;
                            }
                        }
                        else
                        {
                            if (Request.Url.ToString().Contains("OrderDetails.aspx"))
                            {
                                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                                if (userid == string.Empty)
                                {
                                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                                    Session["USER_ID"] = userid;
                                }

                                int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                                int Userid = objHelperServices.CI(Session["USER_ID"]);
                                OrderID = objOrderServices.GetOrderID(Userid, OrdStatus);
                                Session["ORDER_ID"] = OrderID;

                                
                            }
                        }
                    }

                    // if (ProductID >= 0 && Request["Pid"] != null)
                    // {
                    //    ProductID = objHelperServices.CI(Request["Pid"].ToString());

                    //}

                    if (Session["USER_ID"] == null || Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "" || Convert.ToInt32(Session["USER_ID"].ToString()) < 0)
                    {
                        Session["USER_NAME"] = "";
                    }
                    if (Session["USER_NAME"] == null)
                    
                    {
                        Session["USER"] = "";
                        Session["COUNT"] = "0";
                        if (Request.QueryString["popup"] == null)
                            Response.Redirect("/Login.aspx");
                        else
                        {
                            divAddtocart.Visible = false;
                            divTimeout.Visible = true;
                        }
                    }
                    else
                    {
                        string userid = HttpContext.Current.Session["USER_ID"].ToString();
                        if (userid == string.Empty)
                        {
                            userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                            Session["USER_ID"] = userid;
                        }

                        objErrorHandler.CreateLog(Session["USER_ID"].ToString());

                        if (objUserServices.IsUserActive(Session["USER_ID"].ToString()))
                        {


                            if (Session["Multipleitems"] != null)
                            {
                                ProdID = Session["Multipleitems"].ToString().Split(',');
                                ord_itemID = Session["Multipleitems_id"].ToString().Split(',');
                                for (int i = 0; i < ProdID.Length; i++)
                                {
                                    //ProductID = Convert.ToInt32(ProdID[i]);
                                    int product_id = Convert.ToInt32(ProdID[i]);
                                    double order_item_id = Convert.ToInt32(ord_itemID[i]);

                                    if (objProductServices.GetProductAvailability(Convert.ToInt32(product_id)) == 1)
                                    {
                                        AddMultipleItems(product_id, order_item_id);
                                    }
                                }
                                Session["Multipleitems"] = null;
                                //if (Session["pageurl"] != null)
                                //{
                                //    if (!Session["PageUrl"].ToString().Contains("OrderDetails.aspx"))
                                //        if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=QTEEMPTY") || Session["PageUrl"].ToString().Contains("powersearch.aspx"))
                                //        {
                                //            Response.Redirect("OrderDetails.aspx?&bulkorder=1&pid=0");
                                //        }
                                //        else
                                //        {
                                //            Response.Redirect(Session["PageUrl"].ToString());
                                //        }
                                //}
                            }
                            QtyAvail = objOrderServices.GetProductAvilableQty(objHelperServices.CI(Request["Pid"]));
                            MinQtyAvail = objOrderServices.GetProductMinimumOrderQty(objHelperServices.CI(Request["Pid"]));
                            int Productstatus = objProductServices.GetProductAvailability(objHelperServices.CI(Request["Pid"]));
                            int Ord = 0;

                            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                            {
                                Ord = Convert.ToInt32(Session["ORDER_ID"]);
                            }
                            else
                            {
                                Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                            }
                            
                            if (Ord == 0)
                            {
                                oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);
                                objOrderServices.InitilizeOrder(oOrdInfo);
                                //Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                                if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                                {
                                    Ord = Convert.ToInt32(Session["ORDER_ID"]);
                                }
                                else
                                {
                                    Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                                }


                                if (Ord > 0)
                                {
                                    string clientIPAddress = "";
                                    if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
                                        clientIPAddress = Session["IP_ADDR"].ToString();
                                    else
                                        clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                                       objOrderServices.InitilizeOrder_ipaddress(Ord, clientIPAddress);
                                }

                                lblOrdNo.Text = Ord.ToString();
                            }
                            if (Ord != 0)
                            {
                                AvlQty = objOrderServices.GetOrderItemQty(objHelperServices.CI(Request["Pid"]), Ord, 0);
                            }
                            lblOrdNo.Text = Ord.ToString();



                            if (Request["Pid"] != null && Request["Qty"] != null && Productstatus != 0)
                            {


                                int p = objHelperServices.CI(Request["Pid"].ToString());
                                if (QtyAvail == 0 && p > 0)
                                {
                                    string str;
                                    str = "<script language='javascript' type='text/javascript'>";
                                    str = str + "alert('Selected Product is sold out')";
                                    str = str + "</script>";
                                    this.RegisterClientScriptBlock("validate", str);
                                }
                                int txtQty = objHelperServices.CI(Request["Qty"]);
                                //if ((QtyAvail + AvlQty - txtQty) >= 0)

                                if (MinQtyAvail > 0)
                                {
                                        if (objHelperServices.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                                        {
                                            if (objProductServices.GetRestrictedProduct(p).ToString().ToUpper() == "NO")
                                                AddToOrderTable();
                                        }
                                        else
                                        {

                                            AddToOrderTable();
                                        }
                                   
                                }
                                //if (MinQtyAvail == 0)
                                //{
                                //    string enc = System.DateTime.Now.Millisecond.ToString();
                                //    string _notfoundstr = "";
                                //    string product_code_sub = "";
                                //    int user_id_sub = 0;
                                //    if (Request["Pid"] != null && Request["Qty"] != null && Productstatus != 0)
                                //    {
                                //        product_id_chk_sub = objHelperServices.CI(Request["Pid"].ToString());
                                //        product_qty_chk_sub = objHelperServices.CI(Request["Qty"].ToString());
                                //    }
                                //    product_stock_status_sub = GetStockStatus(product_id_chk_sub);
                                //    if (product_stock_status_sub == 0)
                                //    {
                                //        user_id_sub = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());
                                //        product_code_sub = GetProductCode_PS(product_id_chk_sub);
                                //        _notfoundstr += string.Format("{0},", product_code_sub);
                                //        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                                //        oItemClaItemInfo.Clarification_ID = 0;
                                //        oItemClaItemInfo.OrderID = Ord;
                                //        oItemClaItemInfo.ProductDesc = product_code_sub;
                                //        oItemClaItemInfo.Quantity = product_qty_chk_sub;
                                //        if (Convert.ToInt32(Session["ORDER_ID"]) > 0)
                                //            oItemClaItemInfo.UserID = Convert.ToInt32(Session["ORDER_ID"]);
                                //        else
                                //            oItemClaItemInfo.UserID = user_id_sub;
                                //        sessionId = Session.SessionID;
                                //        if (sessionId != "")
                                //            oItemClaItemInfo.PROD_SESSION_ID = sessionId;
                                //        else
                                //            oItemClaItemInfo.PROD_SESSION_ID = "";


                                //        oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
                                //        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                                //        Response.Redirect("/OrderDetails.aspx?&bulkorder=1&" + enc, false);

                                //    }
                                //}
                                if (Session["pageurl"] != null)
                                {
                                    if (!Session["PageUrl"].ToString().Contains("OrderDetails.aspx"))
                                        if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=QTEEMPTY"))
                                        {
                                            Response.Redirect("/OrderDetails.aspx?&bulkorder=1&pid=0");
                                        }
                                }
                                if (Request.QueryString["popup"] != null && Request["fid"] != null && Request["fid"].ToString() != "")
                                {
                                   // AddPopUpItem(txtQty, p, Convert.ToInt32(Request["fid"].ToString()), Ord);
                                    AddPopUpItem(txtQty, p, Convert.ToInt32(Request["fid"].ToString()));
                                    return;
                                }
                            }
                            //else
                            //{
                            //    if (Productstatus == 0 && Request["flgAddItem"] != null && Request["flgAddItem"] == "chkAddItem")
                            //    {
                            //        //DisableProducts(Ord);
                            //        string enc = System.DateTime.Now.Millisecond.ToString();
                            //        Response.Redirect("/OrderDetails.aspx?&bulkorder=1&" + enc, false);
                            //    }
                            //}
                        } 
                        else
                        {
                            if (Request.QueryString["popup"] == null)
                                Response.Redirect("/Login.aspx");
                            else
                            {
                                objErrorHandler.CreateLog("divtimeout");
                                divAddtocart.Visible = false;
                                divTimeout.Visible = true;
                            }
                        }
                    }
                    if (Request["SelAll"] == "1")
                    {
                        //chkSelectAll.Checked = true;
                    }
                    else if (Request["SellAll"] == "0")
                    {
                        // chkSelectAll.Checked = false;
                    }
                    if (Request["SelPid"] != null)
                    {
                        if (Request["SelPid"] != "" && Request["SelPid"] != "AllProd")
                        {
                            int OrderId = 0;
                            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                            {
                                OrderId = Convert.ToInt32(Session["ORDER_ID"]);
                            }
                            else
                            {
                                OrderId = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                            }

                            char[] sep = { ',' };
                            string s1 = Request["SelPid"];
                            string s2 = Request["ProdPrice"];
                            string s3 = Request["ORDER_ITEM_ID"].ToString();
                            decimal SelProdPrice = objHelperServices.CDEC(Request["SelProdPrice"].ToString());
                            string[] cnt = new string[30];
                            string[] cnt1 = new string[30];
                            string[] cnt2 = new string[30];
                            cnt = s1.Split(sep);
                            cnt1 = s2.Split(sep);
                            cnt2 = s3.Split(sep);

                            int len = cnt.Length;
                            int flagship = 1;
                            int flagupdate = 0;



                            //for (int j = 1; j <= cnt1.Length; j++)
                            //{
                            //    SelProdPrice = SelProdPrice + objHelperServices.CDEC(cnt1[j - 1].ToString());
                            //}
                            /* if (objOrderServices.GetOrderItemCount(OrderId) > 0)
                             {
                                 DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderId);
                                 decimal TempShipCost = 0;
                                 for (int i = 1; i <= len; i++)
                                 {
                                     int PrdId = objHelperServices.CI(cnt[i - 1].ToString());
                                     double  order_item_id = objHelperServices.CD (cnt2[i - 1].ToString());
                                     int pQty = objOrderServices.GetOrderItemQty(PrdId, OrderId,order_item_id  );
                                     int nQty = objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                     int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                                     int n = AvailQty + nQty;

                                     DataRow[] oDR = oDSOrderItems.Tables[0].Select("PRODUCT_ID=" + PrdId + " And ORDER_ITEM_ID=" + order_item_id );
                                     if (oDR != null && oDR.Length > 0)
                                     {

                                         if (objOrderServices.RemoveItem(PrdId.ToString(), OrderId, objHelperServices.CI(Session["USER_ID"]),order_item_id.ToString()   ) != -1)
                                         {
                                             if (n >= 0)
                                             {
                                                 objOrderServices.UpdateQuantity(PrdId, n);
                                             }
                                             if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
                                             {
                                                 TempShipCost = TempShipCost + objHelperServices.CDEC(TempShipCost + CalculateShippingCost(OrderId, PrdId, objHelperServices.CDEC(oDR[0]["PRICE_EXT_APPLIED"].ToString()), objHelperServices.CI(oDR[0]["QTY"].ToString())));
                                             }
                                             else
                                             {
                                                 if (flagship == 1)
                                                 {
                                                     TempShipCost = objHelperServices.CDEC(TempShipCost + CalculateShippingCost(OrderId, PrdId, objHelperServices.CDEC(oDR[0]["PRICE_EXT_APPLIED"].ToString()), objHelperServices.CI(oDR[0]["QTY"].ToString())));
                                                     flagship = 0;
                                                 }
                                             }
                                             flagupdate = 1;
                                         }
                                     }
                                     else
                                     {
                                         SelProdPrice = SelProdPrice - objHelperServices.CDEC(cnt1[i - 1].ToString());
                                     }
                                 }
                                 if (flagupdate == 1)
                                 {
                                     decimal Tax = CalculateTaxAmount(SelProdPrice);
                                     if (objOrderServices.GetShippingCost(OrderId) == 0)
                                     {
                                         TempShipCost = -1 * TempShipCost;
                                     }
                                     if (TempShipCost < 0)
                                         TempShipCost = 0;
                                     if (SelProdPrice < 0)
                                         SelProdPrice = 0;
                                     objOrderServices.UpdateRemovedItemsPrice(SelProdPrice, OrderId, objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax)), TempShipCost);
                                 }

                             }
                             else
                             {
                                 oOrdInfo.OrderID = OrderId;
                                 oOrdInfo.ProdTotalPrice = 0.00M;
                                 oOrdInfo.TotalAmount = 0.00M;
                                 oOrdInfo.TaxAmount = 0.00M;
                                 oOrdInfo.ShipCost = 0.00M;
                                 objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                 //objOrderServices.UpdateShippingCost(OrderId, 0.00M);
                             }*/
                            if (objOrderServices.GetOrderItemCount(OrderId) > 0)
                            {
                                DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderId);
                                decimal TempShipCost = 0;
                                for (int i = 1; i <= len; i++)
                                {
                                    int PrdId = objHelperServices.CI(cnt[i - 1].ToString());
                                    double order_item_id = objHelperServices.CD(cnt2[i - 1].ToString());

                                    int pQty = objOrderServices.GetOrderItemQty(PrdId, OrderId, order_item_id);


                                    DataRow[] oDR = oDSOrderItems.Tables[0].Select("PRODUCT_ID=" + PrdId + " And ORDER_ITEM_ID=" + order_item_id);
                                    if (oDR != null && oDR.Length > 0)
                                    {
                                        objOrderServices.RemoveItem(PrdId.ToString(), OrderId, objHelperServices.CI(Session["USER_ID"]), order_item_id.ToString());
                                    }
                                }
                            }
                            oOrdInfo.OrderID = OrderId;
                            objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                            objOrderServices.DeletePaymentRecordUnpaid(oOrdInfo);

                        }
                        else if (Request["SelPid"] == "AllProd")
                        {
                            int OrderId = 0;
                            if (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View"))
                            {
                                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                            }
                            else
                            {
                                OrderId = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                            }

                            int len = objOrderServices.GetOrderItemCount(OrderId);
                            DataSet ds = new DataSet();
                            ds = objOrderServices.GetOrderItems(OrderId);

                            if (len > 0)
                            {
                                /* int cnt = ds.Tables[0].Rows.Count;
                                 foreach (DataRow row in ds.Tables[0].Rows)
                                 {
                                     int PrdId = objHelperServices.CI(row["PRODUCT_ID"].ToString());
                                     int pQty = objHelperServices.CI(row["QTY"].ToString());
                                     int nQty = objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                     int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                                     string order_item_Cid = row["ORDER_ITEM_ID"].ToString()  ;
                                     int n = AvailQty + nQty;
                                     if (n >= 0)
                                         objOrderServices.UpdateQuantity(PrdId, n);
                                 }

                                 if (objOrderServices.RemoveItem(Request["SelPid"], OrderId, objHelperServices.CI(Session["USER_ID"]), "") != -1)
                                 {
                                     oOrdInfo.OrderID = OrderId;
                                     oOrdInfo.ProdTotalPrice = 0.00M;
                                     oOrdInfo.TotalAmount = 0.00M;
                                     oOrdInfo.TaxAmount = 0.00M;
                                     oOrdInfo.ShipCost = 0.00M;
                                     objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                     //objOrderServices.UpdateShippingCost(OrderId, 0.00M);
                                 }*/
                                objOrderServices.RemoveItem(Request["SelPid"], OrderId, objHelperServices.CI(Session["USER_ID"]), "");
                                oOrdInfo.OrderID = OrderId;
                                objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                            }
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("ConfirmMessage.aspx?Result=NOECOMMERCE");
            }
            //lblmsg.Visible = false;
            if (QuotePurchase.ToUpper() == "YES")
            {
                //btnQuoteRequest.Visible = true;
                //btnQuoteRequestTop.Visible = true;
                //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);        
            }
            else
            {
                //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
                //btnQuoteRequest.Visible = false;
                // btnQuoteRequestTop.Visible = false;
            }
            //Modified by indu to show express checkout
            //if (OrderPurchase.ToUpper() == "YES")
            //{
            //    //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            //    lblOrderProceed.Visible = true;
            //    //lblOrderProceed1.Visible = true;
            //}
            //else
            //{
            //    //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            //    //lblOrderProceed1.Visible = false;
            //    lblOrderProceed.Visible = false;
            //    //lblmsg.Visible = true;
            //}
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

    #region "Functions.."

    public void AddMultipleItems(int Productid, double order_item_id)
    {
        QtyAvail = objOrderServices.GetProductAvilableQty(Productid);
        MinQtyAvail = objOrderServices.GetProductMinimumOrderQty(Productid);
        int Ord = 0;

        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        {
            Ord = Convert.ToInt32(Session["ORDER_ID"]);
        }
        else
        {
            Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
        }

        if (Ord == 0)
        {
            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);
            objOrderServices.InitilizeOrder(oOrdInfo);

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                Ord = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
            }

            lblOrdNo.Text = Ord.ToString();
        }

        if (Ord != 0)
            AvlQty = objOrderServices.GetOrderItemQty(Productid, Ord, order_item_id);
        lblOrdNo.Text = Ord.ToString();
        if (Productid != null)
        {
            int p = Productid;
            if (QtyAvail == 0 && p > 0)
            {
                string str;
                str = "<script language='javascript' type='text/javascript'>";
                str = str + "alert('Selected Product is sold out')";
                str = str + "</script>";
                this.RegisterClientScriptBlock("validate", str);
            }
            int txtQty = MinQtyAvail;
            if ((QtyAvail + AvlQty - txtQty) >= 0)
                if (MinQtyAvail > 0)
                    if (objHelperServices.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                    {
                        if (objProductServices.GetRestrictedProduct(p).ToString().ToUpper() == "NO")
                            AddMultipleItemsToOrderTable(Productid, order_item_id);
                    }
                    else
                    {
                        AddMultipleItemsToOrderTable(Productid, order_item_id);
                    }
        }

    }


    public void AddMultipleItemsToOrderTable(int Product_id, double order_item_id)
    {
        try
        {
            int OrderID = 0;
            string OrderStatus = "";

            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);

            //  OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
            }

            OrderStatus = objOrderServices.GetOrderStatus(OrderID);

            if (OrderID == 0 || OrderStatus == OrderServices.OrderStatus.PAYMENT.ToString() || OrderStatus == OrderServices.OrderStatus.SHIPPED.ToString() || OrderStatus == OrderServices.OrderStatus.COMPLETED.ToString() || OrderStatus == OrderServices.OrderStatus.CANCELED.ToString() || OrderStatus == OrderServices.OrderStatus.ORDERPLACED.ToString() || OrderStatus == OrderServices.OrderStatus.MANUALPROCESS.ToString())
            {
                objOrderServices.InitilizeOrder(oOrdInfo);

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                {
                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                }
                else
                {
                    OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
                }



                AddOrderMultipleItems(OrderID, oOrdInfo.UserID, Product_id, order_item_id);
            }
            else if (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING")
            {
                AddOrderMultipleItems(OrderID, oOrdInfo.UserID, Product_id, order_item_id);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public void AddOrderMultipleItems(int OrID, int UsrID, int Product_id, double order_item_id)
    {
        try
        {

            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();
            // OrderServices objOrderServices = new OrderServices();
            int chkExistsItem = 0;
            chkExistsItem = objOrderServices.GetOrderItemQty(Product_id, OrID, order_item_id);
            int ProdQty;
            /*if (MinQtyAvail > 1)
            {
                ProdQty = MinQtyAvail;
            }
            else
            {
                ProdQty = 1;
            }*/
            if (chkExistsItem > 0)
            {
                ProdQty = chkExistsItem;
            }
            else
            {
                ProdQty = 1;
            }
            if (chkExistsItem == 0 || ProdQty != chkExistsItem || ProdQty > 0) // ProdQty > 0
            {
                bool attrcheck = false;
                //Chceck the promotion table.
                if (objProductPromotionServices.CheckPromotion(Product_id))
                {

                    decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));
                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                    //objHelperServices.SQLString = sSQL;
                    //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                    //string strquery = "";
                    //if (pricecode == 1)
                    //{
                    //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}
                    //else
                    //{
                    //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}

                    //DataSet DSprice = new DataSet();
                    //objHelperServices.SQLString = strquery;
                    //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);


                    /* string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                     DataSet DSprice = new DataSet();
                     objHelperServices.SQLString = strquery;
                     DSprice = objHelperServices.GetDataSet();
                     if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                     {

                         foreach (DataRow row in DSprice.Tables[0].Rows)
                         {
                             if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5 && ProdQty >= 1)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                         }
                     }
                     else if (untPrice <= 0)
                     {
                         untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                     }*/
                    DiscPrice = (untPrice * DiscPrice) / 100;
                    untPrice = untPrice - DiscPrice;
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));

                }
                else
                {
                    //Check the user default buyer group or custome buyer group.
                    int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                    string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                    if (BGPriceID == 4 && BGName == "DEFAULTBG")
                    {
                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));


                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}


                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        /*string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                        DataSet DSprice = new DataSet();
                        objHelperServices.SQLString = strquery;
                        DSprice = objHelperServices.GetDataSet();
                        if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow row in DSprice.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                            }
                        }
                        else if (untPrice <= 0)
                        {
                            untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                        }*/
                    }
                    else
                    {

                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}

                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        /*string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                     DataSet DSprice = new DataSet();
                     objHelperServices.SQLString = strquery;
                     DSprice = objHelperServices.GetDataSet();
                     if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                     {

                           foreach (DataRow row in DSprice.Tables[0].Rows)
                             {
                                 if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                             }*/



                        //To calculate the discount price.
                        dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                {
                                    ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                }
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                {
                                    untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                }
                            }
                        }

                       

                        //if (ProdQty >= 50)
                        //{
                        //    string sqlexec = "exec GetPriceTableWagner '" + Product_id.ToString() + "','" + UsrID + "'";
                        //    objErrorHandler.CreateLog(sqlexec);
                        //    DataSet Dsall = objHelperDB.GetDataSetDB(sqlexec);

                        //    if (Dsall != null && Dsall.Tables.Count > 0)
                        //    {
                        //        DataTable Sqltb = Dsall.Tables[0];

                        //        if (Sqltb.Rows.Count > 0)
                        //        {
                        //            untPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                        //        }
                        //    }
                        //}

                        untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));

                        /*}
                        else if (untPrice <= 0)
                        {
                            dsBgPrice = objProductServices.GetProductPriceValue(ProductID, BGPriceID);
                            if (dsBgPrice != null)
                            {
                                untPrice = objHelperServices.CDEC(dsBgPrice.Tables[0].Rows[0].ItemArray[1].ToString());

                                //To calculate the discount price.
                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                        if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                        {
                                            ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        }
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                        {
                                            untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                        }
                                    }
                                }
                                untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));
                            }
                        }*/
                    }
                }//Buyergroup price.
                objErrorHandler.CreateLog("untPrice1 " + untPrice);
                DataSet Dsall = objOrderServices.getBulkBuyPrice(Product_id, UsrID);

                if (Dsall != null && Dsall.Tables.Count > 0)
                {
                    DataTable Sqltb = Dsall.Tables[0];

                    if (Sqltb.Rows.Count > 0 && ProdQty >= Convert.ToInt32(Sqltb.Rows[0]["QTY"].ToString().Replace("+", "")))
                    {
                        untPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                    }
                }
                objErrorHandler.CreateLog("untPrice1 " + untPrice);
                oItemInFo.ORDER_ITEM_ID = order_item_id;
                oItemInFo.ProductID = Product_id;
                oItemInFo.OrderID = OrID;

                oItemInFo.PriceApplied = untPrice;
                oItemInFo.UserID = UsrID;
                oItemInFo.Quantity = objHelperServices.CDEC(ProdQty);
                oItemInFo.Ship_Cost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity)));

                

                //if (Convert.ToInt32(Session["USER_ID"].ToString()) == Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                 //   oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount_DumUser(oItemInFo.Quantity * untPrice, OrID.ToString());
                //else
                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());

                objOrderServices.UpdateOrderItem(oItemInFo);
                //if (untPrice > 0)
                //{    /*
                //    if (ProdQty != 0)
                //    {
                //        //int OrderID = 0;
                //        //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                //        //{
                //        //    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                //        //}
                //        //else
                //        //{
                //        //    OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()));
                //        //}
                //        //int maxqty = objOrderServices.GetOrderItemQty(Product_id, OrderID,order_item_id);
                //        //int MinQty = objOrderServices.GetProductMinimumOrderQty(Product_id);
                //        //int MaxQtyAvl = maxqty + objOrderServices.GetProductAvilableQty(Product_id);

                //        //if (objOrderServices.GetOrderItemQty(Product_id, OrderID, order_item_id) >= ProdQty)
                //        //{
                //        //    oItemInFo.Quantity = objHelperServices.CDEC(maxqty);
                //        //}
                //        //else
                //        //{
                //        //    oItemInFo.Quantity = objHelperServices.CDEC(ProdQty);
                //        //}

                //        //int Qty = objHelperServices.CI(oItemInFo.Quantity);
                //        //ProdQty = MaxQtyAvl - Qty;
                //        //if (ProdQty >= 0)
                //        //    objOrderServices.UpdateQuantity(Product_id, ProdQty);
                //        oItemInFo.Quantity = objHelperServices.CDEC(ProdQty);
                //    }
                //    else
                //    {
                //        oItemInFo.Quantity = 1;
                //    }*/

                //    if (chkExistsItem == 0)
                //    {
                //        objOrderServices.AddOrderItem(oItemInFo);
                //        /* if (objOrderServices.AddOrderItem(oItemInFo) != -1)
                //         {
                //             DataSet dsOrder = new DataSet();
                //             dsOrder = objOrderServices.GetOrderPriceValues(OrID);

                //             if (dsOrder != null)
                //             {
                //                 decimal ProdTotalPrice;
                //                 decimal OrderTotal;
                //                 decimal TotalShipCost;

                //                 //Calculate Shipping Cost
                //                 decimal ProdShippCost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity)));
                //                 decimal ExistProdTotal = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));
                //                 ProdTotalPrice = ExistProdTotal + (oItemInFo.PriceApplied * oItemInFo.Quantity);
                //                 decimal Tax = CalculateTaxAmount(ProdTotalPrice);
                //                 TotalShipCost = objHelperServices.CDEC(objOrderServices.GetShippingCost(OrID)) + ProdShippCost;

                //                 if (_IsShippingFree == true)
                //                 {
                //                     TotalShipCost = 0;
                //                     _IsShippingFree = false;
                //                 }

                //                 oOrdInfo.ShipCost = objHelperServices.CDEC(objHelperServices.FixDecPlace(TotalShipCost));
                //                 OrderTotal = ProdTotalPrice + Tax + TotalShipCost;
                //                 oOrdInfo.OrderID = OrID;
                //                 oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice));
                //                 oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax));
                //                 oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrderTotal));
                //                 objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                //             }
                //         }*/

                //    }
                //    else
                //    {
                //        //update the existing order item.
                //        //Update the new product price to exists products price.
                //        /* DataSet dsOrder = new DataSet();
                //         dsOrder = objOrderServices.GetOrderPriceValues(OrID);

                //         if (dsOrder != null)
                //         {
                //             decimal ProdTotalPrice;
                //             decimal OrderTotal;
                //             decimal TotalShipCost;
                //             decimal ExistProdTotal = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));

                //             decimal Tax = 0.00M;
                //             // objHelperServices.CDEC(ConfigurationManager.AppSettings["SalesTax"].ToString());
                //             if (ProdQty >= chkExistsItem)
                //             {
                //                 ProdTotalPrice = (ExistProdTotal + (oItemInFo.PriceApplied * (oItemInFo.Quantity - chkExistsItem)));
                //             }
                //             else
                //             {
                //                 ProdTotalPrice = (ExistProdTotal - (oItemInFo.PriceApplied * (chkExistsItem - oItemInFo.Quantity)));
                //             }
                //             //Calculate the shipping cost
                //             decimal ProdShippCost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id , oItemInFo.PriceApplied, objHelperServices.CI(chkExistsItem - oItemInFo.Quantity)));

                //             Tax = CalculateTaxAmount(ProdTotalPrice); // (ProdTotalPrice * Tax) / 100;
                //             OrderTotal = ProdTotalPrice + Tax;

                //             oOrdInfo.OrderID = OrID;
                //             oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice));
                //             oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax));
                //             oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrderTotal));
                //             objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                //         }
                //         objOrderServices.UpdateOrderItem(oItemInFo);*/
                //        objOrderServices.UpdateOrderItem(oItemInFo);
                //    }
                //}
            }
            else
            {
                //string myscript = "<script type=\"text/javascript\">";
                ////myscript += "var ParmA ='1'; var ParmB = '2';  var ParmC ='3'; var MyArgs = new Array(ParmA, ParmB, ParmC);  var WinSettings = 'center:yes;resizable:no;dialogHeight:304px;dialogWidth:740px;scroll:no'; MyArgs = window.showModalDialog('viewcartmsg.aspx', MyArgs, WinSettings);";
                //myscript += " alert('Product already added in cart');";
                //myscript += "</script>";
                //if (!ClientScript.IsStartupScriptRegistered("PopupScript"))
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", myscript, true);
                //}

            }
            oOrdInfo.OrderID = OrID;
            objOrderServices.UpdateOrderPrice(oOrdInfo, true);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public int GetRole()
    {
        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
        //objHelperServices.SQLString = sSQL;
        //int iROLE = objHelperServices.CI(objHelperServices.GetValue("USER_ROLE"));
        //return iROLE;
        int iROLE = 0;
        string tempstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, userid, "GET_MULTIUSER_COMPANY_BUYERS_USER_ROLE", HelperDB.ReturnType.RTString);
        if (tempstr != null && tempstr != "")
            iROLE = objHelperServices.CI(tempstr);

        return iROLE;

    }


    public void AddToOrderTable()
    {
        try
        {

            
            int OrderID = 0;
            string OrderStatus = "";
            
            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
            }

            

            OrderStatus = objOrderServices.GetOrderStatus(OrderID);

            

            if (OrderID == 0 || OrderStatus == OrderServices.OrderStatus.PAYMENT.ToString() || OrderStatus == OrderServices.OrderStatus.SHIPPED.ToString() || OrderStatus == OrderServices.OrderStatus.COMPLETED.ToString() || OrderStatus == OrderServices.OrderStatus.CANCELED.ToString() || OrderStatus == OrderServices.OrderStatus.ORDERPLACED.ToString() || OrderStatus == OrderServices.OrderStatus.MANUALPROCESS.ToString())
            {
                objOrderServices.InitilizeOrder(oOrdInfo);

                if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                {
                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                }
                else
                {
                    OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
                }
               

                AddOrderItem(OrderID, oOrdInfo.UserID);
            }
            else if (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING")
            {
                AddOrderItem(OrderID, oOrdInfo.UserID);
             
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void AddPopUpItem(int qty, int Product_id, int family_id)
    {
        DataTable dt = objProductServices.GetPopProduct(Product_id, family_id);
        if (dt != null && dt.Rows.Count > 0)
        {
            lblFamilyName.Text = dt.Rows[0]["Family_name"].ToString();
            lblordercode.Text = dt.Rows[0]["Code"].ToString();
            lblQty.Text = qty.ToString();
           // txtQtypop.Value = qty.ToString();
          //  txtordidpop.Text = Ord_id.ToString();
          //  txtordidpop.ReadOnly = true;
          //  txtpidpop.Text = Product_id.ToString();
           // txtpidpop.ReadOnly = true;
          //  txtordidpop.Value = Ord_id.ToString();
           // txtpidpop.Value = Product_id.ToString();
            lblDesc.Text = dt.Rows[0]["PROD_DSC"].ToString();
  //          String strfile1 = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages";
            String strfile1 = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dt.Rows[0]["TWeb Image1"].ToString().Replace("\\", "/");
           // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dt.Rows[0]["TWeb Image1"].ToString().Replace("\\", "/"));
         //   System.IO.FileInfo Fil = new System.IO.FileInfo(strfile1 + dt.Rows[0]["TWeb Image1"].ToString().Replace("\\", "/"));

            bool ImageExits=objHelperServices.CheckImageExistCDN(strfile1);
            if (ImageExits)
                lblProImage.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + dt.Rows[0]["TWeb Image1"].ToString().Replace("_TH", "_images_200").Replace("\\", "/");
            else
                lblProImage.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/noimage.gif";


            //string userid = "";
            //if (HttpContext.Current.Session["USER_ID"].ToString() != null && HttpContext.Current.Session["USER_ID"].ToString() != "")
            //    userid = HttpContext.Current.Session["USER_ID"].ToString();
            //else
            //    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
            //objHelperServices.DeleteUserBrowseItem(Product_id, family_id, userid);
        }
    }
    public void AddOrderItem(int OrID, int UsrID)
    {
        try
        {
            //ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
            //BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();
            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();
            int Product_id = 0;
            //OrderServices objOrderServices = new OrderServices();
            int chkExistsItem = 0;

            objErrorHandler.CreateOrderSummarylog("Add OrderItem inside order details" + "orderid:" + OrID + "userid:" + UsrID );
            if ((HttpContext.Current.Request["Pid"] != null && HttpContext.Current.Request["Qty"] != null) || (NewProduct_id != 0 && NewQty != 0))
            {


                if (HttpContext.Current.Request["Pid"] != null)
                {
                    Product_id = objHelperServices.CI(HttpContext.Current.Request["Pid"].ToString());
                }
                else
                {
                    objErrorHandler.CreateLog("Add productid");
                    Product_id = NewProduct_id;
                }

               // Product_id = objHelperServices.CI(HttpContext.Current.Request["Pid"].ToString());
                chkExistsItem = objOrderServices.GetOrderItemQty(Product_id, OrID, 0);
                int ProdQty;

                if (HttpContext.Current.Request["Qty"] != null)
                {
                    ProdQty = objHelperServices.CI(HttpContext.Current.Request["Qty"].ToString());
                }
                else
                {
                    ProdQty = NewQty;
                }
                //if (MinQtyAvail >1)
                //{
                //  ProdQty = MinQtyAvail;
                //}
                //else
                //{
              //  ProdQty = objHelperServices.CI(HttpContext.Current.Request["Qty"].ToString());
                //}
                /*if (chkExistsItem == 0 || ProdQty != chkExistsItem)
                {*/

                //Chceck the promotion table.
                if (objProductPromotionServices.CheckPromotion(Product_id))
                {
                    decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));
                    //string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";

                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                    //objHelperServices.SQLString = sSQL;
                    //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                    //string strquery = "";
                    //if (pricecode == 1)
                    //{
                    //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}
                    //else
                    //{
                    //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}

                    //DataSet DSprice = new DataSet();
                    //objHelperServices.SQLString = strquery;
                    //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));


                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                    //DSprice = objHelperServices.GetDataSet();
                    //if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                    //{

                    //    foreach (DataRow row in DSprice.Tables[0].Rows)
                    //    {
                    //        if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //    }
                    //}
                    //else if (untPrice <= 0)
                    //{
                    //    untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                    //}
                    DiscPrice = (untPrice * DiscPrice) / 100;
                    untPrice = untPrice - DiscPrice;
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));

                }
                else
                {
                    //Check the user default buyer group or custome buyer group.
                    int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                    string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                    if (BGPriceID == 4 && BGName == "DEFAULTBG")
                    {
                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}

                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));


                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        //string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id="+ProductID+" and numeric_value>0";
                        //DataSet DSprice = new DataSet();                           
                        //objHelperServices.SQLString = strquery;
                        //DSprice = objHelperServices.GetDataSet();
                        //if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                        //{

                        //    foreach (DataRow row in DSprice.Tables[0].Rows)
                        //    {
                        //        if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //    }
                        //}


                    }
                    else
                    {
                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}

                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        /*string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id="+ProductID+" and numeric_value>0";
                        DataSet DSprice = new DataSet();                           
                        objHelperServices.SQLString = strquery;
                        DSprice = objHelperServices.GetDataSet();
                        if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                        {

                              foreach (DataRow row in DSprice.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                            }
                        }*/


                        //To calculate the discount price.
                        dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                {
                                    ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                }
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                {
                                    untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                }
                            }
                        }

                     


                        //if (ProdQty >= 50)
                        //{
                        //    string sqlexec = "exec GetPriceTableWagner '" + Product_id.ToString() + "','" + UsrID + "'";
                        //    objErrorHandler.CreateLog(sqlexec);
                        //    DataSet Dsall = objHelperDB.GetDataSetDB(sqlexec);
                            
                        //    if (Dsall != null && Dsall.Tables.Count > 0)
                        //    {
                        //        DataTable Sqltb = Dsall.Tables[0];

                        //        if (Sqltb.Rows.Count > 0)
                        //        {
                        //            untPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                        //        }
                        //    }
                        //}

                        untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));


                        /*else if (untPrice <= 0)
                        {
                            dsBgPrice = objProductServices.GetProductPriceValue(ProductID, BGPriceID);

                            if (dsBgPrice != null)
                            {
                                untPrice = objHelperServices.CDEC(dsBgPrice.Tables[0].Rows[0].ItemArray[1].ToString());

                                //To calculate the discount price.
                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                        if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                        {
                                            ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        }
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                        {
                                            untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                        }
                                    }
                                }
                                untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));
                            }
                        }*/
                    }
                }//Buyergroup price.
                objErrorHandler.CreateLog("untPrice2 " + untPrice);
                DataSet Dsall = objOrderServices.getBulkBuyPrice(Product_id, UsrID);

                if (Dsall != null && Dsall.Tables.Count > 0)
                {
                    DataTable Sqltb = Dsall.Tables[0];

                    if (Sqltb.Rows.Count > 0 && ProdQty >= Convert.ToInt32(Sqltb.Rows[0]["QTY"].ToString().Replace("+", "")))
                    {
                        untPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                    }
                }
                objErrorHandler.CreateLog("untPrice2 " + untPrice);
                oItemInFo.ORDER_ITEM_ID = 0;
                oItemInFo.ProductID = objHelperServices.CI(Product_id);
                oItemInFo.OrderID = OrID;
                oItemInFo.PriceApplied = untPrice;
                oItemInFo.UserID = UsrID;
                oItemInFo.Quantity = ProdQty;
                oItemInFo.Ship_Cost = CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));

                

                //if( Convert.ToInt32(Session["USER_ID"].ToString()) == Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                //    oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount_DumUser(oItemInFo.Quantity * untPrice, OrID.ToString());
                //else
                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());


                String sessionId;
                sessionId = Session.SessionID;
                if (sessionId != "")
                    oItemInFo.PRD_SESSION_ID = sessionId;
                else
                    oItemInFo.PRD_SESSION_ID = "";


                if (objProductServices.GetStockStatus(oItemInFo.ProductID, UsrID) == 0)
                {
                    OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                    oItemClaItemInfo.Clarification_ID = 0;
                    oItemClaItemInfo.OrderID = OrID;
                    oItemClaItemInfo.ProductDesc = objProductServices.GetProductCode(oItemInFo.ProductID);
                    oItemClaItemInfo.Quantity = ProdQty;
                    oItemClaItemInfo.UserID = UsrID;
                    oItemClaItemInfo.Clarification_Type = "ITEM_REPLACE";
                    if (sessionId != "")
                        oItemClaItemInfo.PROD_SESSION_ID = sessionId;
                    else
                        oItemClaItemInfo.PROD_SESSION_ID = "";

                    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                }
                else
                {
                    objOrderServices.AddOrderItem(oItemInFo);
                }

        
                /* if (ProdQty != 0 && untPrice != 0)
                 {
                     Session["NOITEMADDED"] = "";
                     int OrderID = OrID;
                     int maxqty = objOrderServices.GetOrderItemQty(Product_id , OrderID,0);
                     int MinQty = objOrderServices.GetProductMinimumOrderQty(Product_id);
                     int MaxQtyAvl = maxqty + objOrderServices.GetProductAvilableQty(Product_id);
                     oItemInFo.Quantity = objHelperServices.CDEC(ProdQty + chkExistsItem);
                     int Qty = objHelperServices.CI(oItemInFo.Quantity);
                     ProdQty = MaxQtyAvl - Qty;
                     if (ProdQty >= 0)
                         objOrderServices.UpdateQuantity(Product_id, ProdQty + chkExistsItem);
                 }
                 else
                 {
                     oItemInFo.Quantity = 1;
                 }*/

                /*  if (chkExistsItem == 0)
                  {
                     if (oItemInFo.PriceApplied != 0)
                      {
                          Session["NOITEMADDED"] = "";
                           if (objOrderServices.AddOrderItem(oItemInFo) != -1)
                          {
                              DataSet dsOrder = new DataSet();
                              dsOrder = objOrderServices.GetOrderPriceValues(OrID);

                              if (dsOrder != null)
                              {
                                  decimal ProdTotalPrice;
                                  decimal OrderTotal;
                                  decimal TotalShipCost;

                                  //Calculate Shipping Cost
                                  decimal ProdShippCost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity)));
                                  decimal ExistProdTotal = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));
                                  ProdTotalPrice = ExistProdTotal + (oItemInFo.PriceApplied * oItemInFo.Quantity);
                                  decimal Tax = CalculateTaxAmount(ProdTotalPrice);
                                  TotalShipCost = objHelperServices.CDEC(objOrderServices.GetShippingCost(OrID)) + ProdShippCost;

                                  if (_IsShippingFree == true)
                                  {
                                      TotalShipCost = 0;
                                      _IsShippingFree = false;
                                  }

                                  oOrdInfo.ShipCost = objHelperServices.CDEC(objHelperServices.FixDecPlace(TotalShipCost));
                                  OrderTotal = ProdTotalPrice + Tax + TotalShipCost;
                                  oOrdInfo.OrderID = OrID;
                                  oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice));
                                  oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax));
                                  oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrderTotal));
                                  objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                              }
                          }
                           
                      }
                      else
                      {
                          Session["NOITEMADDED"] = "NoPrice";
                          //string script = "<script>alert('Invalid price amount.');</script>";
                          //if (!IsClientScriptBlockRegistered("alert"))
                          //{
                          //    this.RegisterClientScriptBlock("alert", script);
                          //}
                      }
                  }
                  else
                  {
                      //update the existing order item.
                      //Update the new product price to exists products price.
                      DataSet dsOrder = new DataSet();
                      dsOrder = objOrderServices.GetOrderPriceValues(OrID);

                      if (dsOrder != null)
                      {
                          decimal ProdTotalPrice;
                          decimal OrderTotal;
                          decimal TotalShipCost;
                          decimal ExistProdTotal = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));

                          decimal Tax = 0.00M;
                          //= objHelperServices.CDEC(ConfigurationManager.AppSettings["SalesTax"].ToString());
                          if (ProdQty >= chkExistsItem)
                          {
                              ProdTotalPrice = (ExistProdTotal + (oItemInFo.PriceApplied * (oItemInFo.Quantity - chkExistsItem)));
                          }
                          else
                          {
                              ProdTotalPrice = (ExistProdTotal - (oItemInFo.PriceApplied * (chkExistsItem - oItemInFo.Quantity)));
                          }
                          //Calculate the shipping cost
                          decimal ProdShippCost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id , oItemInFo.PriceApplied, objHelperServices.CI(chkExistsItem - oItemInFo.Quantity)));

                          Tax = CalculateTaxAmount(ProdTotalPrice); //(ProdTotalPrice * Tax) / 100;
                          OrderTotal = ProdTotalPrice + Tax;

                          oOrdInfo.OrderID = OrID;
                          oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice));
                          oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax));
                          oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrderTotal));
                          objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                      }
                      objOrderServices.UpdateOrderItem(oItemInFo);
                    
                  }
               
                  else
                  {
                      //string myscript = "<script type=\"text/javascript\">";
                      ////myscript += "var ParmA ='1'; var ParmB = '2';  var ParmC ='3'; var MyArgs = new Array(ParmA, ParmB, ParmC);  var WinSettings = 'center:yes;resizable:no;dialogHeight:304px;dialogWidth:740px;scroll:no'; MyArgs = window.showModalDialog('viewcartmsg.aspx', MyArgs, WinSettings);";
                      //myscript += "alert('Product already added in cart');";
                      //myscript += "</script>";
                      //if (!ClientScript.IsStartupScriptRegistered("PopupScript"))
                      //{
                      //    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", myscript, true);
                      //} 
                  }*/
            }
            oOrdInfo.OrderID = OrID;
            objOrderServices.UpdateOrderPrice(oOrdInfo, true);
            objOrderServices.DeletePaymentRecordUnpaid(oOrdInfo);

            if (HttpContext.Current.Request.QueryString["bulkorder"] != null && HttpContext.Current.Request.QueryString["bulkorder"].ToString() == "1")
            {
                if (HttpContext.Current.Request["rma"] != null)
                {
                    string _rma = HttpContext.Current.Request["rma"].ToString();
                    string _rmitem = HttpContext.Current.Request["Item"].ToString();
                    string _rmqty = "";
                    double CalItem_ID = 0;

                    if (HttpContext.Current.Request.QueryString["DelQty"] != null)
                    {
                        _rmqty = HttpContext.Current.Request["DelQty"].ToString();
                    }
                    if (HttpContext.Current.Request.QueryString["cla_id"] != null)
                    {
                        CalItem_ID = Convert.ToDouble(HttpContext.Current.Request["cla_id"].ToString());

                    }
                    if (_rma == "NF")
                    {
                        //Session["ITEM_ERROR"] = Session["ITEM_ERROR"].ToString().Replace(_rmitem + ",", "");
                        objOrderServices.Remove_Clarification_item(CalItem_ID);
                    }
                    if (_rma == "CI")
                    {
                        //Session["ITEM_CHK"] = Session["ITEM_CHK"].ToString().Replace(_rmitem + ",", "");
                        //Session["QTY_CHK"] = Session["QTY_CHK"].ToString().Replace(_rmqty + ",", "");
                        objOrderServices.Remove_Clarification_item(CalItem_ID);
                    }
                }
            }
            if (HttpContext.Current.Request.QueryString["popup"] == null && NewProduct_id == 0)
                Response.Redirect("/OrderDetails.aspx?ORDER_ID=" + OrID + "&bulkorder=1&ViewOrder=View");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void AddOrderItem(int OrID, int UsrID, int qty, int Product_id)
    {
        try
        {
            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();
            objErrorHandler.CreateOrderSummarylog("Add OrderItem inside order details" + "orderid: " + OrID + "userid: " + UsrID + "qty: " + qty + "Product_id: " + Product_id);
            if (objProductPromotionServices.CheckPromotion(Product_id))
            {
                decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));

                string user_id = Session["USER_ID"].ToString();

                untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);

                DiscPrice = (untPrice * DiscPrice) / 100;
                untPrice = untPrice - DiscPrice;
                untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));

            }
            else
            {
                //Check the user default buyer group or custome buyer group.
                int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                if (BGPriceID == 4 && BGName == "DEFAULTBG")
                {
                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                }
                else
                {


                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                    //To calculate the discount price.
                    dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                    if (dsBgDisc != null)
                    {
                        if (dsBgDisc.Tables[0].Rows.Count > 0)
                        {
                            decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                            DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                            if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                            {
                                ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                            }
                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                            bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                            {
                                untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                            }
                        }
                    }

                   
                    //if (qty >= 50)
                    //{
                    //    string sqlexec = "exec GetPriceTableWagner '" + Product_id.ToString() + "','" + UsrID + "'";
                    //    objErrorHandler.CreateLog(sqlexec);
                    //    DataSet Dsall = objHelperDB.GetDataSetDB(sqlexec);

                    //    if (Dsall != null && Dsall.Tables.Count > 0)
                    //    {
                    //        DataTable Sqltb = Dsall.Tables[0];

                    //        if (Sqltb.Rows.Count > 0)
                    //        {
                    //            untPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                    //        }
                    //    }
                    //}
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));



                }
            }//Buyergroup price.

            objErrorHandler.CreateLog("untPrice3 " + untPrice);
            DataSet Dsall = objOrderServices.getBulkBuyPrice(Product_id, UsrID);

            if (Dsall != null && Dsall.Tables.Count > 0)
            {
                DataTable Sqltb = Dsall.Tables[0];

                if (Sqltb.Rows.Count > 0 && qty >= Convert.ToInt32(Sqltb.Rows[0]["QTY"].ToString().Replace("+", "")))
                {
                    untPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                }
            }
            objErrorHandler.CreateLog("untPrice3 " + untPrice);
            oItemInFo.ORDER_ITEM_ID = 0;
            oItemInFo.ProductID = Product_id;
            oItemInFo.OrderID = OrID;
            oItemInFo.PriceApplied = untPrice;
            oItemInFo.UserID = UsrID;
            oItemInFo.Quantity = qty;
            oItemInFo.Ship_Cost = CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));

            

           // if (Convert.ToInt32(Session["USER_ID"].ToString()) == Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
            //    oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount_DumUser(oItemInFo.Quantity * untPrice, OrID.ToString());
            //else
            oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());

            String sessionId;
            sessionId = Session.SessionID;
            if (sessionId != "")
                oItemInFo.PRD_SESSION_ID = sessionId;
            else
                oItemInFo.PRD_SESSION_ID = "";

            objOrderServices.AddOrderItem(oItemInFo);


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
   
    protected decimal CalculateShippingCost(int OID, int ProdId, decimal ProdApplyPrice, int itemQty)
    {
        _IsShippingFree = false;
        DataSet dsOItem = new DataSet();
        decimal ShippingValue = 0;
        decimal ProdShippCost = 0;
        dsOItem = objOrderServices.GetItemDetailsFromInventory(ProdId);
        decimal ProductCost = (ProdApplyPrice * itemQty);
        if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
        {
            if (dsOItem != null)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    if (objHelperServices.CB(rItem["IS_SHIPPING"]) == 1)
                    {
                        ProdShippCost = ((ProductCost * objHelperServices.CDEC(rItem["PROD_SHIP_COST"])) / 100);
                    }
                }
            }
        }
        else
        {
            if ((objOrderServices.GetCurrentProductTotalCost(OID) + ProdApplyPrice) < objHelperServices.CDEC(objHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
            {
                if (dsOItem != null)
                {
                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                    {
                        ProdShippCost = ((ProductCost * objHelperServices.CI(objHelperServices.GetOptionValues("SHIPPING CHARGE").ToString())) / 100);
                    }
                }
            }
            else
            {
                _IsShippingFree = true;
            }
        }
        return objHelperServices.CDEC(ProdShippCost);
    }

    //public decimal CalculateShippingCostUpdateQty(int OID, int ProdId, decimal ProdApplyPrice, int itemQty)
    //{
    //    _IsShippingFree = false;
    //    DataSet dsOItem = new DataSet();
    //    decimal ShippingValue = 0;
    //    decimal ProdShippCost = 0;
    //    dsOItem = objOrderServices.GetItemDetailsFromInventory(ProdId);
    //    decimal ProductCost = (ProdApplyPrice * itemQty);
    //    if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
    //    {
    //        if (dsOItem != null)
    //        {
    //            foreach (DataRow rItem in dsOItem.Tables[0].Rows)
    //            {
    //                if (objHelperServices.CB(rItem["IS_SHIPPING"]) == 1)
    //                {
    //                    ProdShippCost = ((ProductCost * objHelperServices.CDEC(rItem["PROD_SHIP_COST"])) / 100);
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if ((objOrderServices.GetCurrentProductTotalCost(OID) + ProdApplyPrice) < objHelperServices.CDEC(objHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
    //        {
    //            if (dsOItem != null)
    //            {
    //                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
    //                {
    //                    ProdShippCost = ((ProductCost * objHelperServices.CI(objHelperServices.GetOptionValues("SHIPPING CHARGE").ToString())) / 100);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            _IsShippingFree = true;
    //        }
    //    }
    //    return objHelperServices.CDEC(ProdShippCost);
    //}

    #endregion

    #region "Control Events.."

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string SelAll = "";
            Session["SellAllClick"] = 0;
            if (Request["SelAll"] == "1")
            {
                SelAll = "0";
            }
            else if (Request["SelAll"] == null)
            {
                SelAll = "1";
                Session["SellAllClick"] = 1;
            }
            else
            {
                SelAll = "1";
            }
            Response.Redirect("/OrderDetails.aspx?&bulkorder=1&SelAll=" + SelAll); //+ ",SelPid=" + pid ); //",DelFlag=0");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void btnUpdateCart_Click(object sender, EventArgs e)
    {
        try
        {
            OrderServices.OrderItemInfo oOrdItemInfo = new OrderServices.OrderItemInfo();
            int RowCnt;
            decimal TotalAmt = 0.00M;
            decimal UntPrice = 0.00M;
            decimal ProdTotal = 0.00M;
            decimal tax = 0.00M;
            decimal TaxAmt = 0.00M;
            decimal OrdTotal = 0.00M;
            decimal ProdShipCost = 0.00M;
            decimal TotalShipCost = 0.00M;
            int nQty;
            int PrdId;
            string SelAll = "";
            int OrderID = 0;

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
            }
            DataSet dsOItem = null;
            string LeaveDuplicateProds = "";
            string chkuserID = HttpContext.Current.Session["USER_ID"].ToString();
            if (chkuserID != ConfigurationManager.AppSettings["DUM_USER_ID"].ToString())
            {
                dsOItem = objOrderServices.GetOrderItems(OrderID);
            }
            else
            {
                // RowCnt = objOrderServices.GetOrderItemCount(OrderID);
                 LeaveDuplicateProds = GetLeaveDuplicateProducts();
                 dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds, Session.SessionID.ToString());
            }
            if (dsOItem != null && dsOItem.Tables.Count > 0 && dsOItem.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    //bool attrcheck = false;
                    PrdId = objHelperServices.CI(Request.Form["txtPid" + rItem["ORDER_ITEM_ID"].ToString()]);
                    nQty = objHelperServices.CI(Request.Form["txtQty" + rItem["ORDER_ITEM_ID"].ToString()]);
                    if (PrdId > 0)
                    {
                        if (nQty <= 0)
                        {
                            string ErrorMessage = "Invalid Quantity! Quantity Should be Equal/Greater than 1";
                            //MessageBox.Show(ErrorMessage, "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //ShowMessageBoxInformation();

                            //Page page = HttpContext.Current.CurrentHandler as Page;
                            //string script = "<script type='text/javascript' language='javascript'>alert('" + ErrorMessage + "');</script>";
                            //page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);


                            //ErrorMessage = "<script>";
                            //ErrorMessage = ErrorMessage + "alert('Invalid Quantity! Quantity Should be Equal/Greater than 1')";
                            //ErrorMessage = ErrorMessage + "</script>";
                            //this.RegisterClientScriptBlock("alert", ErrorMessage);
                            ShowAlertMessageBox("Invalid Quantity! Quantity Should be Equal/Greater than 1");
                            return;
                        }

                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", HttpContext.Current.Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", PrdId, nQty, HttpContext.Current.Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", PrdId, nQty, HttpContext.Current.Session["USER_ID"]);
                        //}

                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //UntPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));




                        //if (nQty >= 50)
                        //{
                        //    string sqlexec = "exec GetPriceTableWagner '" + PrdId.ToString() + "','" + user_id + "'";
                        //    objErrorHandler.CreateLog(sqlexec);
                        //    DataSet Dsall = objHelperDB.GetDataSetDB(sqlexec);

                        //    if (Dsall != null && Dsall.Tables.Count > 0)
                        //    {
                        //        DataTable Sqltb = Dsall.Tables[0];

                        //        if (Sqltb.Rows.Count > 0)
                        //        {
                        //            UntPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                        //        }
                        //    }
                        //}

                        objErrorHandler.CreateLog("untPrice4 " + UntPrice);
                        string user_id = Session["USER_ID"].ToString();

                        UntPrice = objHelperDB.GetProductPrice_Exc(PrdId, nQty, user_id);

                        DataSet Dsall = objOrderServices.getBulkBuyPrice(PrdId, Convert.ToInt32(user_id));

                        if (Dsall != null && Dsall.Tables.Count > 0)
                        {
                            DataTable Sqltb = Dsall.Tables[0];

                            if (Sqltb.Rows.Count > 0 && nQty >= Convert.ToInt32(Sqltb.Rows[0]["QTY"].ToString().Replace("+", "")))
                            {
                                UntPrice = Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE2"].ToString()), 2);
                            }
                        }
                        objErrorHandler.CreateLog("untPrice4 " + UntPrice);

                        TotalAmt = UntPrice * nQty;
                        oOrdItemInfo.ORDER_ITEM_ID = objHelperServices.CD(rItem["ORDER_ITEM_ID"].ToString());
                        oOrdItemInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                        oOrdItemInfo.ProductID = PrdId;
                        oOrdItemInfo.Quantity = nQty;
                        oOrdItemInfo.OrderID = OrderID;
                        oOrdItemInfo.PriceApplied = UntPrice;
                        oOrdItemInfo.Ship_Cost = CalculateShippingCost(OrderID, PrdId, UntPrice, nQty);
                        //if (Convert.ToInt32(Session["USER_ID"].ToString()) == Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                        //    oOrdItemInfo.Tax_Amount = objOrderServices.CalculateTaxAmount_DumUser(TotalAmt, OrderID.ToString());
                        //else
                        oOrdItemInfo.Tax_Amount = objOrderServices.CalculateTaxAmount(TotalAmt, OrderID.ToString(), PrdId.ToString());


                       // oOrdItemInfo.Tax_Amount = objOrderServices.CalculateTaxAmount(TotalAmt, oOrdItemInfo.Ship_Cost,OrderID.ToString(),pr);

                        // ProdShipCost = CalculateShippingCost(OrderID, PrdId, UntPrice, nQty);
                        // TotalShipCost = TotalShipCost + ProdShipCost;

                        // ProdTotal = objHelperServices.CDEC(ProdTotal + TotalAmt);

                        // Code updated Padmanaban,JTECH
                        // TO update the qty entered by the user, commented the below to block the available qty verification
                        //
                        // int oQty = objOrderServices.GetOrderItemQty(PrdId, OrderID);
                        // int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                        int n = nQty; // AvailQty + oQty - nQty;
                        if (n >= 0)
                        {
                            objOrderServices.UpdateQuantity(PrdId, n);
                            objOrderServices.UpdateOrderItem(oOrdItemInfo);
                        }
                        nQty = objHelperServices.CI(Request.Form["txtQty" + rItem["ORDER_ITEM_ID"].ToString()]);
                        SelAll = "0";
                    }
                }
                //TaxAmt = CalculateTaxAmount(ProdTotal);
                //OrdTotal = objHelperServices.CDEC(ProdTotal + TaxAmt + TotalShipCost);
                oOrdInfo.OrderID = OrderID;
                //oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(TaxAmt));
                //oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotal));
                //oOrdInfo.ShipCost = objHelperServices.CDEC(objHelperServices.FixDecPlace(TotalShipCost));
                //oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrdTotal));
                objOrderServices.UpdateOrderPrice(oOrdInfo, true);
            }
            //Response.Redirect("OrderDetails.aspx?reload=" + SelAll, false);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }


    protected void btnSaveOrdTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            OrderServices.OrderTemplateInfo oOrdtemplate = new OrderServices.OrderTemplateInfo();


            int nQty;
            int PrdId;
            int user_id = objHelperServices.CI(Session["USER_ID"].ToString());
            int OrderID = 0;
            string ErrorMessage = "";
            Page page = null;
            string script = "";
            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
            }


            string LeaveDuplicateProds = GetLeaveDuplicateProducts();
            DataSet dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds, "");


            objOrderServices.RemoveOrderTemplate(user_id);
            if (dsOItem != null && dsOItem.Tables.Count > 0 && dsOItem.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    //bool attrcheck = false;
                    PrdId = objHelperServices.CI(Request.Form["txtPid" + rItem["ORDER_ITEM_ID"].ToString()]);
                    nQty = objHelperServices.CI(Request.Form["txtQty" + rItem["ORDER_ITEM_ID"].ToString()]);

                    if (nQty <= 0)
                    {


                        ShowAlertMessageBox("Invalid Quantity! Quantity Should be Equal/Greater than 1");
                        return;
                    }




                    oOrdtemplate.UserID = user_id;
                    oOrdtemplate.ProductID = PrdId;
                    oOrdtemplate.Quantity = nQty;
                    objOrderServices.AddOrderItemTemplate(oOrdtemplate);
                }
            }

            ShowAlertMessageBox("Successfully saved");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    private void ShowMessageBoxInformation()
    {
        msgPop.ID = "popUp1";
        msgPop.PopupControlID = "MessageboxPanel";
        msgPop.BackgroundCssClass = "modalBackground";
        msgPop.TargetControlID = "updCart";
        msgPop.DropShadow = true;
        msgPop.CancelControlID = "CloseButton";
        this.MessageboxPanel.Controls.Add(msgPop);
        this.msgPop.Show();
    }

    private void ShowAlertMessageBox(string msg)
    {
        lblAlert.Text = msg;
        msgAlert.ID = "DivAlert";
        msgAlert.PopupControlID = "pnlAlert";
        msgAlert.BackgroundCssClass = "modalBackground";
        msgAlert.TargetControlID = "BtnOrdTemplate";
        msgAlert.DropShadow = true;
        msgAlert.CancelControlID = "btnok";
        this.MessageboxPanel.Controls.Add(msgAlert);
        this.msgAlert.Show();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        string ErrItems = "", ClrItems = "", ClrQty = "";
        int DuplicateItem_Prod_idCount = 0;
        String sessionId;
        sessionId = Session.SessionID;
        string LeaveDuplicateProds = GetLeaveDuplicateProducts();

        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        {
            OrderID = Convert.ToInt32(Session["ORDER_ID"]);

        }
        else
        {
            OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);

        }
        DataSet dsDuplicateItem_Prod_id = new DataSet();
       // if (objHelperServices.CI(Session["USER_ID"].ToString()) != Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
           // dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds,"");
      //  else
            dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds, sessionId);

        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
        {
            DuplicateItem_Prod_idCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
        }

        //if (Session["ITEM_ERROR"] != null || Session["ITEM_CHK"] != null)
        //{
        //    if (!string.IsNullOrEmpty(Session["ITEM_ERROR"].ToString()))
        //        ErrItems = Session["ITEM_ERROR"].ToString().Trim().Replace(",", "");

        //    if (!string.IsNullOrEmpty(Session["ITEM_CHK"].ToString()))
        //        ClrItems = Session["ITEM_CHK"].ToString().Trim().Replace(",", "");

        //    if (!string.IsNullOrEmpty(Session["QTY_CHK"].ToString()))
        //        ClrQty = Session["QTY_CHK"].ToString().Trim().Replace(",", "");

        //}


        DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK", "");
        DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE", sessionId);

        //DataTable tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "");
        DataTable tbErrorItems = new DataTable();
     

       // if (objHelperServices.CI(Session["USER_ID"].ToString()) != Convert.ToInt32(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
           // tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "","");
       // else
            tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_ERROR", sessionId);

        //if (ErrItems == "" && ClrItems == "" && DuplicateItem_Prod_idCount == 0)
            if (tbErrorItems == null && DuplicateItem_Prod_idCount == 0 && tbErrorChk == null && tbErrorReplace == null )
        {
            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                    if( OrderID<=0)
                       OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);

                //Response.Redirect("/Shipping.aspx?OrderId=" + OrderID + "&ApproveOrder=Approve");
                Response.Redirect("/checkout.aspx?" + EncryptSP(OrderID.ToString() ));
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                //Response.Redirect("/Shipping.aspx?OrderId=" + OrderID);
                Response.Redirect("/checkout.aspx?" + EncryptSP(OrderID.ToString()));
            }

        }
        else
        {
            //ClientScript.RegisterStartupScript(typeof(string), "alert", "alert(\"Please Review Order Clarifications / Errors before proceeding\")", true);
            //ClientScript.RegisterClientScriptBlock(typeof(string), "alert", "alert(\"Please Review Order Clarifications / Errors before proceeding\")", true);

            //string script = "<script type=\"text/javascript\">alert('Please Review Order Clarifications / Errors before proceeding');</script>";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

            ShowPopUpMessage();
        }
    }

    protected void BtnExpressCheckout_Click(object sender, EventArgs e)
    {
        string ErrItems = "", ClrItems = "", ClrQty = "";
        int DuplicateItem_Prod_idCount = 0;
        String sessionId;
        sessionId = Session.SessionID;
        string LeaveDuplicateProds = GetLeaveDuplicateProducts();

        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        {
            OrderID = Convert.ToInt32(Session["ORDER_ID"]);

        }
        else
        {
            OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);

        }

        if (Request.QueryString["OrdFlg"] != null)
        {
            oOrdInfo = objOrderServices.GetOrder(OrderID);
            if (oOrdInfo.UserID != 777 && oOrdInfo.OrderStatus == 1)
            {
                UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                ouserinfo = objUserServices.GetUserInfo(oOrdInfo.UserID);
                Session["USER_ID"] = ouserinfo.UserID;
                Session["USER_NAME"] = ouserinfo.LoginName;

                Session["USER_ROLE"] = ouserinfo.USERROLE;
                Session["COMPANY_ID"] = ouserinfo.CompanyID;
                Session["CUSTOMER_TYPE"] = ouserinfo.CUSTOMER_TYPE;
                Session["DUMMY_FLAG"] = "1";
                Session["Emailid"] = ouserinfo.AlternateEmail;
                Session["Firstname"] = ouserinfo.FirstName;

                Session["Lastname"] = ouserinfo.LastName;
                Session["LOGIN_NAME_TOP"] = ouserinfo.Contact;
            }

        }
        DataSet dsDuplicateItem_Prod_id = new DataSet();
   
        dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds, sessionId);

        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
        {
            DuplicateItem_Prod_idCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
        }

         DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_CHK", "");
        DataTable tbErrorReplace = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_REPLACE", sessionId);

   
        DataTable tbErrorItems = new DataTable();
        tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "ITEM_ERROR", sessionId);

        //if (ErrItems == "" && ClrItems == "" && DuplicateItem_Prod_idCount == 0)
        if (tbErrorItems == null && DuplicateItem_Prod_idCount == 0 && tbErrorChk == null && tbErrorReplace == null)
        {
            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                if (OrderID <= 0)
                    OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);

                //Response.Redirect("/Shipping.aspx?OrderId=" + OrderID + "&ApproveOrder=Approve");

                UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                if (Session["USER_ID"] != null)
                {
                    ouserinfo = objUserServices.GetUserInfo(objHelperServices.CI(Session["USER_ID"].ToString()));

                    if (ouserinfo.ShipAddress1 != "")
                    {

                        Session["ExpressLevel"] = "2Compl";
                    }
                    else if (ouserinfo.ShipAddress1 == "" && ouserinfo.FirstName != "dummy")
                    {
                        Session["ExpressLevel"] = "2Compl";
                    }
                    else
                    {

                        Session["ExpressLevel"] = "Start";
                    }
                }
                else
                {
                    Session["ExpressLevel"] = "Start";
                }

              
               
                    Response.Redirect("/expressCheckout.aspx?" + EncryptSP(OrderID.ToString()));
                
               
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                //Response.Redirect("/Shipping.aspx?OrderId=" + OrderID);
                UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
                if (Session["USER_ID"] != null)
                {
                    ouserinfo = objUserServices.GetUserInfo(objHelperServices.CI(Session["USER_ID"].ToString()));

                    if (ouserinfo.ShipAddress1 != "")
                    {

                        Session["ExpressLevel"] = "2Compl";
                    }
                    else if (ouserinfo.ShipAddress1 == "" && ouserinfo.FirstName != "dummy")
                    {
                        Session["ExpressLevel"] = "2Compl";
                    }
                    else
                    {

                        Session["ExpressLevel"] = "Start";
                    }
                }
                else
                {
                    Session["ExpressLevel"] = "Start";
                }
              
                    Response.Redirect("/expressCheckout.aspx?" + EncryptSP(OrderID.ToString()));
               
            }

        }
        else
        {
            ShowPopUpMessage();
        }
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
    private void ShowPopUpMessage()
    {
        //modalPop.ID = "popUp";
        //modalPop.PopupControlID = "ModalPanel";
        //modalPop.BackgroundCssClass = "modalBackground";
        //modalPop.TargetControlID = "lblOrderProceed";
        //modalPop.DropShadow = true;
        //modalPop.CancelControlID = "btnCancel";
        //this.ModalPanel.Controls.Add(modalPop);
        //this.modalPop.Show();
        PopupMsg.Visible = true;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        PopupMsg.Visible = false;
        return;
    }
    protected void btnContinueNext_Click(object sender, EventArgs e)
    {
        if (Session["PageUrl"] == null)
        {
            Response.Redirect("/OrderDetails.aspx?&bulkorder=1&pid=0");
        }
        else
        {
            if (Session["PageUrl"].ToString().ToLower().Contains("pl.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("product.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("pd.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("ps.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("/Home.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("fl.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("compare.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("byproduct.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("bb.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("bulkorder.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("cataloguedownload.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("newsupdate.aspx") ||
                Session["pageUrl"].ToString().ToLower().Contains("ct.aspx")||
                Session["pageUrl"].ToString().ToLower().Contains("/ct") ||
                Session["pageUrl"].ToString().ToLower().Contains("/microsite") ||
               Session["pageUrl"].ToString().ToLower().Contains("/fl") ||
               Session["pageUrl"].ToString().ToLower().Contains("/bb") ||
               Session["pageUrl"].ToString().ToLower().Contains("/ps") ||
                Session["pageUrl"].ToString().ToLower().Contains("/pd")||
               Session["pageUrl"].ToString().ToLower().Contains("/pl")
                
                
                
                )
                Response.Redirect(Session["PageUrl"].ToString());
            else
                Response.Redirect("/Home.aspx");
        }
    }

    protected void btnRemove_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnQuoteRequest_Click(object sender, EventArgs e)
    {
        int OrdID = 0;

        if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
        {
            OrdID = Convert.ToInt32(Session["ORDER_ID"]);
        }
        else
        {
            OrdID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
        }

        int Upt = 0;
        DataSet DSQteId = new DataSet();

        oOrdInfo = objOrderServices.GetOrder(OrdID); //GetOrderDetails(OrdID);
        oQuoteInfo.TotalAmount = oOrdInfo.ProdTotalPrice;
        oQuoteInfo.QuoteStatus = objHelperServices.CI(QuoteServices.QuoteStatus.OPEN);
        oQuoteInfo.ProdTotalPrice = oOrdInfo.ProdTotalPrice;
        oQuoteInfo.TaxAmount = oOrdInfo.TaxAmount;
        oQuoteInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        objQuoteServices.CreateQuote(oQuoteInfo);
        QDs = objOrderServices.GetOrderItems(OrdID);
        int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
        if (QDs != null)
        {
            foreach (DataRow oDr in QDs.Tables[0].Rows)
            {
                oQuoteItemInfo.ProductID = objHelperServices.CI(oDr["PRODUCT_ID"]);
                string Pid = objHelperServices.CS(oQuoteItemInfo.ProductID);
                oQuoteItemInfo.Quantity = objHelperServices.CI(oDr["QTY"]);
                oQuoteItemInfo.PriceApplied = objHelperServices.CDEC(oDr["PRICE_EXT_APPLIED"]);
                oQuoteItemInfo.QuoteID = QID;
                oQuoteItemInfo.UserID = (objHelperServices.CI(Session["USER_ID"].ToString()));
                objQuoteServices.AddQuoteItem(oQuoteItemInfo);
            }
        }
        int Ordstatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
        Upt = objQuoteServices.UpdateQuoteStatus(QID, (int)QuoteServices.QuoteStatus.REQUESTQUOTE);

        if (Upt > 0)
        {
            objOrderServices.UpdateOrderStatus(OrdID, Ordstatus);
            objOrderServices.UpdateQuoteID(OrdID, QID);
            SendQuoteNotification(QID);
            //btnQuoteRequest.Enabled = false;
            //btnQuoteRequestTop.Enabled = false;
        }
        Response.Redirect("QuoteReview.aspx?QteId=" + QID + "&ViewType=REVIEW");
    }
    #endregion
    private decimal GetMyPrice_Exc(int ProductID)
    {
        decimal retval = 0.00M;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();

        retval = objHelperDB.GetProductPrice_Exc(ProductID, 1, userid);
        return retval;
    }
    private int GetProductID(string Code)
    {
        object retval = "-1";
        //string sql = string.Format("SELECT PRODUCT_ID FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID = 1 AND STRING_VALUE = '{0}'", Code);
        //oHelper.SQLString = sql;
        //retval = oHelper.GetValue("PRODUCT_ID");
        //return oHelper.CI(retval);
        retval = (string)objHelperDB.GetGenericDataDB(Code, "GET_BULLORDER_PRODUCT_ID", HelperDB.ReturnType.RTString);
        if (retval != null && retval != "")
            retval = objHelperServices.CI(retval);

        return (int)retval;

    }
    //private int GetStockStatus(int ProductID)
    //{
    //    int Retval = 0;
    //    try
    //    {
    //        //string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
    //        //objHelperService.SQLString = sSQL;
    //        //Retval = objHelperService.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
    //        DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY_BULKORDER", HelperDB.ReturnType.RTTable);
    //        if (objrbl != null)
    //        {
    //            string strstockstatus="";
    //            foreach (DataRow dr in objrbl.Rows)
    //            {
    //                strstockstatus = objrbl.Rows[0]["PROD_STOCK_STATUS"].ToString();
    //                if (strstockstatus == "true" || strstockstatus == "True")
    //                    Retval = -1;
    //            }               
                
    //        }
            
    //    }
    //    catch (Exception e)
    //    {
    //        objErrorHandler.ErrorMsg = e;
    //        objErrorHandler.CreateLog();
    //    }
    //    return Retval;
    //}

    public string GetProductCode(int ProductID)
    {
        string Retval = "";
        try
        {
            DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_CODE_WAG", HelperDB.ReturnType.RTTable);
            if (objrbl != null)
            {
                Retval = objrbl.Rows[0]["PRODUCT_CODE"].ToString();
            }
            else
                Retval ="";
        }
        catch(Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
        }
        return Retval;
    }

    public string GetProductCode_PS(int ProductID)
    {
        string Retval = "";
        try
        {
           DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_CODE", HelperDB.ReturnType.RTTable);
                     
            if (objrbl != null)
            {
                Retval = objrbl.Rows[0]["STRING_VALUE"].ToString();
            }
            else
                Retval = "";
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
        }
        return Retval;
    }
   
    //private void DisableProducts(int Ord)
    //{
     
    //    string _notfoundstr = "";
    //    string product_code_sub = "";
    //    int user_id_sub = 0;
    //    user_id_sub = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());


    //    if (Request["Pid"] != null && Request["Qty"] != null )
    //    {
    //        product_id_chk_sub = objHelperServices.CI(Request["Pid"].ToString());
    //        product_qty_chk_sub = objHelperServices.CI(Request["Qty"].ToString());
    //    }
    //    product_stock_status_sub = GetStockStatus(product_id_chk_sub);
    //    product_code_sub = GetProductCode(product_id_chk_sub);
    //    if (product_code_sub == "")
    //        product_code_sub = GetProductCode_PS(product_id_chk_sub);
    //    _notfoundstr += string.Format("{0},", product_code_sub);
    //    oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //    oItemClaItemInfo.Clarification_ID = 0;
    //    oItemClaItemInfo.OrderID = Ord;
    //    oItemClaItemInfo.ProductDesc = product_code_sub;
    //    oItemClaItemInfo.Quantity = product_qty_chk_sub;
    //    if (Convert.ToInt32(Session["ORDER_ID"]) > 0)
    //        oItemClaItemInfo.UserID = Convert.ToInt32(Session["ORDER_ID"]);
    //    else
    //        oItemClaItemInfo.UserID = user_id_sub;

    //    String sessionId;
    //    sessionId = Session.SessionID;
    //    if (sessionId != "")
    //        oItemClaItemInfo.PROD_SESSION_ID = sessionId;
    //    else
    //        oItemClaItemInfo.PROD_SESSION_ID = "";


    //    oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
    //    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
        
    //}
    [System.Web.Services.WebMethod]
    public static string UpdateOrderQty(string Qty, string OrdId, string pid)
    {
        string rtn = "";
        OrderServices objOrderServices = new OrderServices();

        int chkqty = 0;
        chkqty = Convert.ToInt32(Qty.ToString());
        if (chkqty <= 0)
            return rtn;

        if (Qty != "" && OrdId != "" && pid != "" && Qty != "1")
        {
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            HelperDB objHelperDB = new HelperDB();
            HelperServices objHelperServices = new HelperServices();
            OrderServices.OrderItemInfo oOrdItemInfo = new OrderServices.OrderItemInfo();
            OrderDetails ord = new OrderDetails();
            decimal TotalAmt = 0.00M;
            decimal UntPrice = 0.00M;
           // decimal ProdTotal = 0.00M;
           // decimal tax = 0.00M;
            //decimal TaxAmt = 0.00M;
           // decimal OrdTotal = 0.00M;
           // decimal ProdShipCost = 0.00M;
           // decimal TotalShipCost = 0.00M;
            int ORDER_ITEM_ID = 0;
            string userid = "";
            if (HttpContext.Current.Session["USER_ID"].ToString() != null && HttpContext.Current.Session["USER_ID"].ToString() != "")
                userid = HttpContext.Current.Session["USER_ID"].ToString();
            else
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

            ORDER_ITEM_ID = objOrderServices.GetOrderItemNumber(Convert.ToInt32(OrdId.ToString()), Convert.ToInt32(pid.ToString()), Convert.ToInt32(userid.ToString()));
                //objOrderServices.UpdateOrderItemQtyPopUp(Convert.ToInt32(OrdId.ToString()), Convert.ToInt32(Qty.ToString()), Convert.ToInt32(pid.ToString()));


            UntPrice = objHelperDB.GetProductPrice_Exc(Convert.ToInt32(pid.ToString()), Convert.ToInt32(Qty.ToString()), userid);

            TotalAmt = UntPrice * Convert.ToInt32(Qty.ToString());
            oOrdItemInfo.ORDER_ITEM_ID = objHelperServices.CD(ORDER_ITEM_ID);
            oOrdItemInfo.UserID = objHelperServices.CI(userid);
            oOrdItemInfo.ProductID = Convert.ToInt32(pid.ToString());
            oOrdItemInfo.Quantity = Convert.ToInt32(Qty.ToString());
            oOrdItemInfo.OrderID = Convert.ToInt32(OrdId.ToString());
            oOrdItemInfo.PriceApplied = UntPrice;
            int nqty =objHelperServices.CI(Qty.ToString());
            int PrdId = objHelperServices.CI(pid.ToString());
            oOrdItemInfo.Ship_Cost = ord.CalculateShippingCost(Convert.ToInt32(OrdId.ToString()), PrdId, UntPrice, nqty);
            oOrdItemInfo.Tax_Amount = objOrderServices.CalculateTaxAmount(TotalAmt,  oOrdItemInfo.Ship_Cost ,OrdId.ToString());
            int updateqty = 0;
            if (nqty > 0)
                updateqty = objOrderServices.UpdateOrderItem(oOrdItemInfo);

            if (updateqty > 0)
            {
             // int  retVal = -1;
              //  DataSet tmpds = objOrderServices.GetOrderItemDetailSum(Convert.ToInt32(OrdId.ToString()));
                
                oOrdInfo.OrderID = Convert.ToInt32(OrdId.ToString());
                int updordprice = 0;
                updordprice = objOrderServices.UpdateOrderPrice(oOrdInfo, true);
            }
        }
        return rtn;
    }
    public string GetLeaveDuplicateProducts()
    {

        try
        {
            string LeaveDuplicateProds = "";
            if (Session["LeaveDuplicateProds"] != null && Session["LeaveDuplicateProds"].ToString() != "")
            {
                LeaveDuplicateProds = Session["LeaveDuplicateProds"].ToString();
                LeaveDuplicateProds = LeaveDuplicateProds.Replace("-", "");
                if (LeaveDuplicateProds.StartsWith(",") == true)
                    LeaveDuplicateProds = LeaveDuplicateProds.Substring(1);

                if (LeaveDuplicateProds.EndsWith(",") == true)
                    LeaveDuplicateProds = LeaveDuplicateProds.Substring(0, LeaveDuplicateProds.Length - 1);

            }
            return LeaveDuplicateProds;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

    #region "Notifications"

    public void SendQuoteNotification(int QuoteID)
    {
        //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        if (objNotificationServices.IsNotificationActive(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString()))
        {
            DataSet dsOrder = objNotificationServices.BuildNotifyInfo();
            HelperServices objHelperServices = new HelperServices();
            string sTemplate = "";
            string sEmailMessage = "";
            string sUser = "";
            sUser = objUserServices.GetUserEmailAdd(objHelperServices.CI(Session["USER_ID"]));
            try
            {
                DataRow oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.FROMCONTENT.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objHelperServices.GetOptionValues("COMPANY ADDRESS").ToString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.FULLNAME.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objUserServices.GetUserFullName(objHelperServices.CI(Session["USER_ID"]));
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.QUOTEDATE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = System.DateTime.Now.ToLongDateString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.QUOTENO.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = QuoteID.ToString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.CONSTRUCTTABLE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = ConstructQuoteDetails(QuoteID);
                dsOrder.Tables[0].Rows.Add(oRow);

                sTemplate = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString());
                sEmailMessage = objNotificationServices.ParseTemplateMessage(sTemplate, dsOrder);

                objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                objNotificationServices.NotifyTo.Add(sUser);
                objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                objNotificationServices.NotifySubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString());
                objNotificationServices.NotifyMessage = sEmailMessage;
                objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString());
                objNotificationServices.SendMessage();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    }

    public string ConstructQuoteDetails(int QuoteID)
    {
        try
        {
            int Qty = 0;
            double Price = 0.0;
            double TotalPrice = 0.0;
            string CatalogItemNo = "";
            string sQuoteDetails = "";
            DataSet dsOD = new DataSet();
            dsOD = objQuoteServices.GetQuoteDetails(QuoteID);
            string currency = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
            sQuoteDetails = @"<TABLE BORDER=1><TR><TD><STRONG>Qty</STRONG></TD><TD><STRONG>Item-Description</STRONG></TD><TD><STRONG>Price</STRONG></TD></TR>";
            foreach (DataRow row in dsOD.Tables[0].Rows)
            {
                CatalogItemNo = row["CATALOG_ITEM_NO"].ToString();
                Qty = objHelperServices.CI(row["QTY"]);
                //CUSTOM PRICE
                Price = objHelperServices.CD(row["PRICE_EXT_APPLIED"]) * Qty;
                TotalPrice = TotalPrice + Price;
                sQuoteDetails = sQuoteDetails + @"<TR><TD>" + Qty.ToString() + "</TD><TD>" + CatalogItemNo + "</TD><TD>" + currency + Price.ToString("#,#0.00") + "</TD></TR>";
            }
            sQuoteDetails = sQuoteDetails + @"<tr><td colspan=3 align=right> SubTotal : " + currency + TotalPrice.ToString("#,#0.00") + "</td></tr></TABLE>";
            return sQuoteDetails;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }

    }
    #endregion

}


