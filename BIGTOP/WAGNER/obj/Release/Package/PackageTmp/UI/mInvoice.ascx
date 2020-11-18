<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mInvoice.ascx.cs" Inherits="UI_mInvoice" %>
<%@ Import Namespace="System.Data" %>
<%--<%@ Import Namespace="TradingBell.Common" %>
<%@ Import Namespace="TradingBell.WebServices" %>
--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%--<style type="text/css">
    .style7InvOrd
    {
        color: black;
        font-size: 8pt;
        font-family: Arial Unicode MS, Arial;
        text-transform: capitalize;
        font-weight: bold;
    }
    .style19InvOrd
    {
        width: 169px;
    }
    .style20InvOrd
    {
        width: 249px;
    }
    .style21InvOrd
    {
        width: 368px;
    }
    .style23InvOrd
    {
        font-size: 11px;
        color: black;
        font-family: Arial Unicode MS, Arial;
        text-align: right;
        background-color: white;
        width: 36px;
    }
    .style24InvOrd
    {
        width: 53px;
    }
    .TextColumnStyleInvOrd
    {
        font-family: Arial;
        font-size: 12px;
        font-weight: bold;
        text-align: left;
    }
    .LabelStyleInvOrd
    {
        font-family: Arial;
        font-size: 12px;
        text-align: left;
    }
</style>--%>
<div class="col-lg-10">
<div class=" blue_color panel-heading white_color bolder">ORDER DETAILS</div>
<div class="col-lg-6 col-sm-6 margin_top_20">
<h4 class="table_bg panel-heading bolder font_14">SHIPPING & ORDER DETAILS</h4>
<table class="table table-bordered font_13">
<tbody class="pop_td_height">
<tr>
                    <th scope="row">SHIPPED BY</th>
                    <td>
                             <asp:Label ID="lblShippedBy" runat="server" Text="" CssClass="LabelStyleInvOrd" Font-Bold="true"
                                        ForeColor="#00aeef"></asp:Label>
                                    <asp:HyperLink ID="HlShippedBy" runat="server" CssClass="LabelStyleInvOrd" Text="" Font-Bold="true" 
                                        Target="_blank" style="text-decoration: none;"
                                        ForeColor="#00aeef"></asp:HyperLink>
                    </td>
    </tr>
    <tr>
<th scope="row">CONNOTE NO</th>
<td>
     <asp:Label ID="lblCannoteNo" runat="server" Text="" CssClass="LabelStyleInvOrd" Font-Bold="true"
                                        ForeColor="#00aeef"></asp:Label>
                                    <asp:HyperLink ID="HlCannoteNo" runat="server" CssClass="LabelStyleInvOrd" Text="" Font-Bold="true" 
                                        Target="_blank" style="text-decoration: none;"
                                        ForeColor="#00aeef"></asp:HyperLink>
</td>
</tr>
<tr>
                    <th scope="row">ORDER NO</th>
                    <td>
                      <asp:Label ID="lblOrderNo" runat="server" Text="" CssClass="LabelStyleInvOrd"></asp:Label>
                    </td>
                  </tr>
                  <tr>
                    <th scope="row">INVOICE NO</th>
                    <td>
                      <asp:Label ID="lblInvoiceNo" runat="server" Text="INVOICE NO" CssClass="LabelStyleInvOrd"></asp:Label>
                    </td>
                  </tr>
</tbody>
</table>
</div>
<div class="col-lg-6 col-sm-6 margin_top_20">
              <h4 class="table_bg panel-heading bolder font_13">USER</h4>
              <ul class=" list_style_none padding_left line_height_34">
<li class="bolder">CREATED BY USER</li>
<li class="margin_left" style="line-height:27px;">
   <asp:Label ID="lblCreateUserName" runat="server" Text="CREATED USER AND TIME" 
                                        ></asp:Label>
</li>
<li class="bolder">APPROVED BY USER</li>
<li class="margin_left" style="line-height:27px;">
      <asp:Label ID="lblApprovedUserName" runat="server" Text="APPROVED USER AND TIME"
                                        ></asp:Label>
</li>
</ul>
</div>
 <div class="clearfix"></div>

 <div class="col-lg-6 col-sm-6">
              <h4 class="table_bg panel-heading bolder font_13">BILL TO</h4>
              <ul class=" list_style_none padding_left line_height_24">
                <%--<li>Created by User</li>
                <li>Test 13/12/2014 03:50 pm</li>
                <li>Approved by User</li>
                <li>Test 13/12/2014 03:50 pm</li>
                <li>Approved by User</li>
                <li>Test 13/12/2014 03:50 pm</li>--%>
                <li>
                     <asp:Label ID="lblDeliveryTo" runat="server" Text="Delivery Address" CssClass="LabelStyleInvOrd"
                            Font-Bold="false"></asp:Label>
                </li>
              </ul>
            </div>

            <div class="col-lg-6 col-sm-6">
              <h4 class="table_bg panel-heading bolder font_13">SHIP TO</h4>
              <ul class=" list_style_none padding_left line_height_24">
               <%-- <li>Created by User</li>
                <li>Test 13/12/2014 03:50 pm</li>
                <li>Approved by User</li>
                <li>Test 13/12/2014 03:50 pm</li>
                <li>Approved by User</li>
                <li>Test 13/12/2014 03:50 pm</li>--%>
                <li>
                      <asp:Label ID="lblShipTo" runat="server" Text="Shipping Address" CssClass="LabelStyleInvOrd"
                            Font-Bold="false"></asp:Label>
                </li>
              </ul>
            </div>
            <div class=" blue_color panel-heading white_color bolder clear">ORDER CONTENTS</div>
            <table class="table table-bordered font_13">
              <tbody class="pop_td_height">
                <tr>
                  <td><strong>ORDER CODE</strong></td>
                  <td><strong>QTY</strong></td>
                </tr>
                  <%
                    HelperDB objHelperDB = new HelperDB();
                    HelperServices objHelperServices = new HelperServices();
                    //ErrorHandler objErrorHandler = new ErrorHandler();
                    OrderServices objOrderServices = new OrderServices();
                    ProductServices objProductServices = new ProductServices();
                    DataSet dsOItem = new DataSet();

                    int RowNo = 0;
                    string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                    int OrderID = objHelperServices.CI(Request["Ordid"].ToString());

                    //decimal ShippingCost = oHelper.CDEC(Session["ShipCost"]);
                    decimal ProdSubTotal = 0.00M;
                    dsOItem = objOrderServices.GetOrderItems(OrderID);
                    if (dsOItem != null)
                    {
                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                        {
                            decimal ProductUnitPrice;
                            int pid;
                            decimal Amt = 0;
                            pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                            //ProductUnitPrice = oHelper.CDEC(oProd.GetProductBasePrice(pid));
                            ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                            ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                            Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice);

                            RowNo = RowNo + 1;

                            if (RowNo % 2 == 0)
                            {
                %>
                <tr>
                    <td>
                        <%Response.Write(rItem["CATALOG_ITEM_NO"].ToString());  %>
                    </td>
                    <td>
                        <% Response.Write(rItem["QTY"].ToString()); %>
                    </td>
                </tr>
                <%
                            }
                            else
                            { 
                %>
                <tr>
                    <td>
                        <%Response.Write(rItem["CATALOG_ITEM_NO"].ToString().Trim());  %>
                    </td>
                    <td>
                        <% Response.Write(rItem["QTY"].ToString()); %>
                    </td>
                </tr>
                <%
                            }

                        }//End ForEach

                    }//End dsoItem null
                %>
              </tbody>
            </table>
 </div>
<%--<table id="tblBase" width="650px" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse"
    align="center">
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="4" cellspacing="" border="0" style="font-family: Arial;
                font-size: small; font-weight: normal; border-collapse: collapse" bgcolor="#c3d4dd">
                <tr>
                    <td width="48%">
                        SHIPPING & ORDER DETAILS
                    </td>
                    <td width="4%" style="background-color: White">
                        &nbsp;
                    </td>
                    <td width="48%">
                        USER
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
                <tr>
                    <td width="48%">
                        <table width="100%" cellpadding="2" cellspacing="0" border="0" style="border-collapse: collapse">
                            <tr>
                                <td width="40%" align="left" class="TextColumnStyleInvOrd">
                                    SHIPPED BY
                                </td>
                                <td width="60%" align="left">
                                    <asp:Label ID="lblShippedBy" runat="server" Text="" CssClass="LabelStyleInvOrd" Font-Bold="true"
                                        ForeColor="#00aeef"></asp:Label>
                                    <asp:HyperLink ID="HlShippedBy" runat="server" CssClass="LabelStyleInvOrd" Text="" Font-Bold="true" 
                                        Target="_blank" style="text-decoration: none;"
                                        ForeColor="#00aeef"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" align="left" class="TextColumnStyleInvOrd">
                                    CONNOTE NO
                                </td>
                                <td  width="60%" align="left" >
                                    <asp:Label ID="lblCannoteNo" runat="server" Text="" CssClass="LabelStyleInvOrd" Font-Bold="true"
                                        ForeColor="#00aeef"></asp:Label>
                                    <asp:HyperLink ID="HlCannoteNo" runat="server" CssClass="LabelStyleInvOrd" Text="" Font-Bold="true" 
                                        Target="_blank" style="text-decoration: none;"
                                        ForeColor="#00aeef"></asp:HyperLink>
                                    
                                </td>
                              
                            </tr>
                            <tr>
                                <td width="40%" align="left" class="TextColumnStyleInvOrd">
                                    ORDER NO
                                </td>
                                <td width="60%" align="left">
                                    <asp:Label ID="lblOrderNo" runat="server" Text="" CssClass="LabelStyleInvOrd"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" align="left" class="TextColumnStyleInvOrd">
                                    INVOICE NO 
                                </td>
                                <td width="60%" align="left">
                                    <asp:Label ID="lblInvoiceNo" runat="server" Text="INVOICE NO" CssClass="LabelStyleInvOrd"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="4%" style="background-color: White">
                        &nbsp;
                    </td>
                    <td width="48%" align="left">
                        <table width="100%" cellpadding="2" cellspacing="0" border="0" style="border-collapse: separate">
                         <asp:Label ID="lblCreatedUser" runat="server" Text="CREATED BY USER" CssClass="LabelStyleInvOrd"
                                        Font-Bold="true"></asp:Label>
                                         <asp:Label ID="lblApprovedUser" runat="server" Text="APPROVED BY USER" Font-Names="Arial"
                                        Font-Size="11px" Font-Bold="true"></asp:Label>
                            <tr valign="top">
                                <td>
                                    <asp:Label ID="lblCreatedUser" runat="server" Text="CREATED BY USER" CssClass="LabelStyleInvOrd"
                                        Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    &nbsp;
                                    <asp:Label ID="lblCreateUserName" runat="server" Text="CREATED USER AND TIME" Font-Names="Arial"
                                        Font-Size="11px"></asp:Label>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Label ID="lblApprovedUser" runat="server" Text="APPROVED BY USER" Font-Names="Arial"
                                        Font-Size="11px" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    &nbsp;
                                    <asp:Label ID="lblApprovedUserName" runat="server" Text="APPROVED USER AND TIME"
                                        Font-Names="Arial" Font-Size="11px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="font-family: Arial;
                font-size: small; font-weight: normal; border-collapse: collapse" bgcolor="#c3d4dd">
                <tr>
                    <td width="48%" align="left">
                        BILL TO:
                    </td>
                    <td width="4%" style="background-color: White">
                        &nbsp;
                    </td>
                    <td width="48%%" align="left">
                        SHIP TO:
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse"
                bgcolor="white">
                <tr>
                    <td width="48%" align="left">
                        <asp:Label ID="lblDeliveryTo" runat="server" Text="Delivery Address" CssClass="LabelStyleInvOrd"
                            Font-Bold="false"></asp:Label>
                    </td>
                    <td width="4%">
                        &nbsp;
                    </td>
                    <td width="48%" align="left">
                        <asp:Label ID="lblShipTo" runat="server" Text="Shipping Address" CssClass="LabelStyleInvOrd"
                            Font-Bold="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr valign="top">
        <td width="100%" colspan="3" valign="top">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td width="100%" bgcolor="0092c8" style="font-family: Arial; font-size: small; font-weight: bold;
            color: White;">
            ORDER CONTENTS
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" border="0" cellpadding="3" cellspacing="0" style="font-family: Arial;
                font-size: small; font-weight: normal; border-collapse: collapse; border-color: Red;"
                bgcolor="#c1d8d9">
                <tr>
                    <td align="left" width="30%">
                        ORDER CODE
                    </td>
                    <td align="left" width="70%">
                        QTY
                    </td>
                </tr>
               
                <%
                    HelperDB objHelperDB = new HelperDB();
                    HelperServices objHelperServices = new HelperServices();
                    //ErrorHandler objErrorHandler = new ErrorHandler();
                    OrderServices objOrderServices = new OrderServices();
                    ProductServices objProductServices = new ProductServices();
                    DataSet dsOItem = new DataSet();

                    int RowNo = 0;
                    string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                    int OrderID = objHelperServices.CI(Request["Ordid"].ToString());

                    //decimal ShippingCost = oHelper.CDEC(Session["ShipCost"]);
                    decimal ProdSubTotal = 0.00M;
                    dsOItem = objOrderServices.GetOrderItems(OrderID);
                    if (dsOItem != null)
                    {
                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                        {
                            decimal ProductUnitPrice;
                            int pid;
                            decimal Amt = 0;
                            pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                            //ProductUnitPrice = oHelper.CDEC(oProd.GetProductBasePrice(pid));
                            ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                            ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                            Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice);

                            RowNo = RowNo + 1;

                            if (RowNo % 2 == 0)
                            {
                %>
                <tr style="background-color: #c3d4dd; font-size: 12px;text-align:left;">
                    <td align="left" width="30%">
                        <%Response.Write(rItem["CATALOG_ITEM_NO"].ToString());  %>
                    </td>
                    <td align="left" width="70%">
                        <% Response.Write(rItem["QTY"].ToString()); %>
                    </td>
                </tr>
                <%
                            }
                            else
                            { 
                %>
                <tr style="background-color: White;font-size: 12px;text-align:left;">
                    <td width="30%">
                        <%Response.Write(rItem["CATALOG_ITEM_NO"].ToString().Trim());  %>
                    </td>
                    <td width="70%">
                        <% Response.Write(rItem["QTY"].ToString()); %>
                    </td>
                </tr>
                <%
                            }

                        }//End ForEach

                    }//End dsoItem null
                %>
            </table>
        </td>
    </tr>
</table>--%>

