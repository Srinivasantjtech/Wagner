<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UI_InvoiceOrder" Codebehind="InvoiceOrder.ascx.cs" %>
<%@ Import Namespace="System.Data" %>
<%--<%@ Import Namespace="TradingBell.Common" %>
<%@ Import Namespace="TradingBell.WebServices" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<table id="tblBase" class="" align="center" width="100%" border="0px"
    cellpadding="0" cellspacing="0">
    <tr>
        <td width="100%" colspan="6" align="left" >
            <table cellpadding="0" cellspacing="0" width="100%" class="orderdettable">
                <tr>
                    <td bgcolor="#D2E2F0" align="left" style=" border-width: thin; border-color: #BCD0E2; " width="13%" >
                        <b>Order Code</b>
                    </td>
                    <td style="border-style: none solid solid none;  border-color: #BCD0E2;border: 1px solid #BCD0E2;padding: 0px 0 0 9px !important;"
                                bgcolor="#D2E2F0" align="left" width="10%">
                        <b>Quantity</b>
                    </td>
                      <td style="border-style: none solid solid none;  border-color: #BCD0E2;border: 1px solid #BCD0E2;padding: 0px 0 0 9px !important;"
                                colspan="2" bgcolor="#D2E2F0" align="left" width="30%" >
                                <b>Description</b>
                            </td>

                      <td style="border-style: none solid solid none;  border-color: #BCD0E2;border: 1px solid #BCD0E2;padding: 0px 0 0 9px !important;"
                                bgcolor="#D2E2F0" align="left" width="20%">
                                <b>Cost</b>
                            </td>
                            <td style="border-style: none none solid none;  border-color: #BCD0E2;border: 1px solid #BCD0E2;padding: 0px 0 0 9px !important;"
                                bgcolor="#D2E2F0" align="left" width="19%">
                                <b>Extension Amount (Ex. GST)</b>
                            </td>
                </tr>
                <%-- <table class ="BaseTable1" cellpadding="3" cellspacing="1" width="558">
                        <tr class="TableRowHead">
                           <td class="tx_6" background="images/17.gif" align ="center" style="width: 210px">Item No.</td>
                           <td class="tx_6" background="images/17.gif" align ="center" width ="100px">
                               Quantity</td>
                               <td class="tx_6" background="images/17.gif" align ="center" width ="210px">
                               Description</td>
                            <td class="tx_6" background="images/17.gif" align ="center" width ="100px">Cost</td>
                            <td class="tx_6" background="images/17.gif" align ="center" width ="100px">Amount</td>
                        </tr>--%>
                <!--- Dynamic content for orderd item details  -->
                <%
                    HelperServices objHelperServices = new HelperServices();
                    
                    OrderServices objOrderServices = new OrderServices();
                    UserServices objUserServices = new UserServices();
                    ProductServices objProductServices = new ProductServices();
                    DataSet dsOItem = new DataSet();

                    string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                    int OrderID = objHelperServices.CI(Request["OrderID"].ToString());
                    string catlogitem = "";
                    //decimal ShippingCost = oHelper.CDEC(Session["ShipCost"]);
                    decimal ProdSubTotal = 0.00M;
                    int _UserrID;
                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    
                    
                    
                    dsOItem = objOrderServices.GetOrderItems(OrderID);
                   
                    _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());

                    UserServices.UserInfo oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                    UserServices.UserInfo oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);


                    //if (oOrdShippInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au                
                   
                    
                    if (dsOItem != null)
                    {
                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                        {
                            decimal ProductUnitPrice;
                            int pid;
                            decimal Amt = 0;

                            pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                            catlogitem = rItem["CATALOG_ITEM_NO"].ToString();
                            //ProductUnitPrice = oHelper.CDEC(oProd.GetProductBasePrice(pid));
                            ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                            ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                            Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice); 
                                    
                %>
                <tr style="z-index: 1">
                    <td  bgcolor="White" align="left" style="border-style: none solid solid #E7E7E7; border-width: thin;
                        border-color: #E7E7E7" width="13%" class="toplinkatest">
                        <% Response.Write(rItem["CATALOG_ITEM_NO"].ToString()); %></td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        bgcolor="White" align="left" width="10%" >
                        <% Response.Write(rItem["QTY"].ToString()); %>
                    </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        colspan="2" bgcolor="White" align="left" width="25%">
                        <% Response.Write(rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;"));%>
                    </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        bgcolor="White" align="left" width="20%">
                      <% Response.Write(CurSymbol + " " + ProductUnitPrice.ToString("#,#0.00")); %> </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        bgcolor="White" align="left" width="27%">
                      <% Response.Write(CurSymbol + " " + Amt.ToString("#,#0.00")); %> </td>
                </tr>
                <%  
                                    
                            //ProdSubTotal =ProdSubTotal + oHelper.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());                                         

                        }//End ForEach

                    }//End dsoItem null
                %>
                <tr>
                    <td  colspan="4" rowspan="4" height="" class="style21INORD"  bgcolor="white" align="right" valign="top">
                        <font color="red">
                            <%--Availability & Cost is only Estimate. Actual Invoice may vary.--%>
                         </font>
                    </td>
                    <td colspan="1" class="NumericField" style="height: 15px; border-style: none solid solid none;text-align:left;
                        border-width: thin; border-color: #E7E7E7;">
                        Sub Total (Ex GST)
                    </td>
                    <td class="NumericField" style="height: 15px; border-style: none solid solid none;text-align:left;
                        border-width: thin; border-color: #E7E7E7;">
                        <% 
                            //ProdSubTotal = objOrderServices.GetCurrentProductTotalCost(OrderID);
                            //decimal TaxCst = 0.00M;
                            //decimal Total = 0.00M;
                            //Response.Write(CurSymbol + " " + ProdSubTotal);
                            //if (ProdSubTotal > 0)
                            //{
                            //    TaxCst = Math.Round((ProdSubTotal * 10 / 100), 2, MidpointRounding.AwayFromZero);
                            //}
                            //else
                            //{
                            //    TaxCst = 0;
                            //}
                            //Total = ProdSubTotal + TaxCst;   
                            Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);                                 
                        %>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" class="NumericField" style="height: 15px;text-align:left;">
                        <%   // if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                            if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                  Response.Write("Shipping Charge");
                                else
                                //  Response.Write("Delivery / Handling Charge (Ex GST)");   <
                                Response.Write("Delivery (Ex GST)");                               
                                 %>
                    </td>
                    <td class="NumericField" style="height: 15px;text-align:left;">
                        <%
                            //Response.Write(CurSymbol + Session["TaxAmt"].ToString()); 
                            //decimal TaxCst = objOrderServices.GetTaxAmount(OrderID);

                            //Response.Write(CurSymbol + " " + TaxCst);
                            //if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                            if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                Response.Write("To Be Advised");         
                            else
                                Response.Write(CurSymbol + " " + oOrderInfo.ShipCost);         
                            
                        %>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" class="NumericField" style="height: 15px;text-align:left;">
                        Total Tax Amount (GST)
                    </td>
                    <td class="NumericField" style="height: 15px;text-align:left;">
                        <%
                            //Response.Write(CurSymbol + Session["TaxAmt"].ToString()); 
                            //decimal TaxCst = objOrderServices.GetTaxAmount(OrderID);

                            //Response.Write(CurSymbol + " " + TaxCst);
                            Response.Write(CurSymbol + " " + oOrderInfo.TaxAmount);         
                        %>
                    </td>
                </tr>
                 
                <%--     <tr>
                            <td colspan="3" class="NumericField" style="height: 25px">Shipping & Handling
                            </td>
                            <td class="NumericField" style="height: 25px">
                                <%
                                    //Response.Write(CurSymbol +  ShippingCost);
                                    Response.Write(CurSymbol + " " + objOrderServices.GetShippingCost(OrderID));
                                %>
                            </td>
                        </tr>--%>
                <tr>
                    <td colspan="1" class="NumericFieldship" style="border-style: none solid solid none;text-align:left;
                        border-width: thin; border-color: #E7E7E7;">
                        <%
                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
                        {                                        
                            %>                                        
                                <strong>Est. Total </strong><br />
                            <%
                        }
                        else
                        {
                                %>
                            <strong>Est. Total Inc GST</strong><br />
                                <%
                        } %>
                       
                        (Freight not included)
                    </td>
                    <td class="NumericFieldship" style="border-style: none solid solid none; border-width: thin;text-align:left;
                        border-color: #E7E7E7;">
                        <strong>
                            <%
                                //decimal ProductTotalCost = ProdSubTotal + oHelper.CDEC(Session["TaxAmt"].ToString()) + ShippingCost;
                                //Response.Write(CurSymbol + ProductTotalCost);
                                //Response.Write(CurSymbol + " " + objOrderServices.GetOrderTotalCost(OrderID));                                    
                              //  Response.Write(CurSymbol + " " + Total);
                                Response.Write(CurSymbol + " " + oOrderInfo.TotalAmount);     
                            %>
                        </strong>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

