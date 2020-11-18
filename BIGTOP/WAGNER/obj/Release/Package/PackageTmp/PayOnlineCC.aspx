﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="PayOnlineCC.aspx.cs" Inherits="PayOnlineCC" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language=JavaScript>
    var message = "Right click not allowed this page!";
    function clickIE4() {
        if (event.button == 2) {
            alert(message);
            return false;
        }
    }
    function clickNS4(e) {
        if (document.layers || document.getElementById && !document.all) {
            if (e.which == 2 || e.which == 3) {
                alert(message);
                return false;
            }
        }
    }

    if (document.layers) {
        document.captureEvents(Event.MOUSEDOWN);
        document.onmousedown = clickNS4;
    }
    else if (document.all && !document.getElementById) {
        document.onmousedown = clickIE4;
    }

    document.oncontextmenu = new Function("alert(message);return false")
</script>

<script language="JavaScript">
    document.onkeypress = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onmousedown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onkeydown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
</script>
   <script language="javascript" type="text/javascript">
       function Numbersonly(e) {
           var keynum
           var keychar
           var numcheck
           // For Internet Explorer
           if (window.event) {
               keynum = e.keyCode
           }
           // For Netscape/Firefox/Opera
           else if (e.which) {
               keynum = e.which
           }
           keychar = String.fromCharCode(keynum)
           //List of special characters you want to restrict
           if (keychar == "1" || keychar == "2" || keychar == "3" || keychar == "4" || keychar == "5" || keychar == "6" || keychar == "7" || keychar == "8" || keychar == "9" || keychar == "0") {

               return true;
           }
           else {
               return false;
           }
       }
</script>
<script type="text/javascript">
    (function () {

        var DEBUG = true,
	EXPOSED_NS = 'ForTheCosumer';

        var myApp = function () {

            return {
                DoSomething: function () { },
                DoSomethingElse: function () { }
            }
        } ();

        // expose my public methods
        window[EXPOSED_NS] = {
            doSomething: myApp.DoSomething,
            doSomethingElse: myApp.DoSomethingElse
        };

        if (DEBUG) {
            window.MyApp = myApp
        }
    } ());

    window.onload = func1;

    function func1() {
        document.getElementById("r1").scrollIntoView();
    }
    function Setinit() {
        var x = document.getElementById('<%= btnPay.ClientID %>');
        var y = document.getElementById('<%= BtnProgress.ClientID %>');
        var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');
        x.style.display = "none";
        y.style.display = "block";
        y.style.visibility = "visible";
        z.style.display = "block";
        z.style.visibility = "visible";
    }

</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">

</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
   <%-- <div class="box1" style="width: 955px;">--%>

    <table  width="100%" border="0" 
    align="left">
    <tr>
    <td>
    
   <div id="page-wrap">
    <%--<H4 style="TEXT-ALIGN: left" class="title3"> SHIPPING & ORDER DETAILS</H4>--%>
   <h3 class="pad10-0" style="margin:0px;">Wagner Check Out</h3>
<div class="grid12">
            <ul class="breadcrumb_wag">
            <li>
            <span class="aero">   Shipping / Delivery Details</span>
            </li>
            <li>
            <span class="aero currentpg">Payment Options</span>
            </li>
            <li>
            <span class="aero">Completed</span>
            </li>
            </ul>
</div>

<div class="grid12" runat="server" id="divCC" >
<div class="">
<div class="cl"></div>
          <span id ="r1"  > </span>  
 <div runat ="server" id="div2" class="redspan" style="font-size:12px;text-align:center;" >
            </div>

<div runat ="server" id="div1" >
          
<fieldset >
 <legend>Payment</legend>





				
                
                  
<div class="ccforms" style="text-align:left;">        

            
  
            <%  
                OrderServices objOrderServices = new OrderServices();
                SecurePayService objSecurePayService = new SecurePayService();

                Boolean blnsp = objSecurePayService.CheckSecurePay();
                
                
              if (objOrderServices.IsNativeCountry(OrderID) == 1)
               {
                
                
                  %>
                    <% if (blnsp==false)
                  { %>
                    <div style="padding: 16px 2px 16px 174px;background-color:#FFD52B" class="alert yellowbox icon_4">
                         <h3 style="font-size: 16px;color:Black;">Standard Credit Card Payment Option is currently unavailable. <br />Please kindly proceed to check out with Paypal payment option.</h3>
                   </div>
                   <%} %>
               <div class="form-col-2-8">
            	<span style="font-size:14px;">Payment Type</span>
            </div>
                    <%
                   
                        if (blnsp == true) 
                   {
                   %>
                       <div class="form-col-2-8">
           	  <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnSecurePayLink_Click"  CausesValidation="false" style="cursor:pointer;" ><label style="cursor:pointer;"><img style="margin-bottom:-10px" alt="cc" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/pay1uch.png" ></label>  </asp:LinkButton>            
            </div>
            <%} %>
              <div class="form-col-3-8 margbtm">
           	  <asp:LinkButton ID="LinkButton3" runat="server" OnClick="btnPayPalPayLink_Click" CausesValidation="false" style="cursor:pointer;"><label style="cursor:pointer;"><img style="margin-bottom:-13px" alt="cc" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/pay2ch.png" ></label></asp:LinkButton>
            </div>
             <%} %>

               
              <div class="cl"></div>  

            <div class="cl"></div>
            <div class="form-col-8-8">
              <img style="margin-bottom:15px" alt="cc" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/paypal.png">
              <p class="para pad10-0">Pay using your Credit Card or Paypal Account.</p>
              <p class="para pad10-0">You wil be redirected to PayPal website to complete payment transaction.<br>
                </p>
              
            </div>
            <div class="cl"></div>
           
            <div class="form-col-2-8">
           <span  Class="totalamt" style="float:left;">Total Amount
            $</span>  <asp:Label runat="server" ID="lblAmount" CssClass="totalamt"  /> 
            </div>
            <div class="form-col-2-8">   
              
              <%--   class="button normalsiz btngreen fleft" Text="Pay Now"--%>
                 <asp:Button runat="server" ID="btnPay"  style="width:100px;" class="paybtn"   OnClick="btnPay_Click" OnClientClick="Setinit()" />       


                 
                 <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="button normalsiz btngreen fleft" Enabled="false"   />       
                 
                  
         <div id="divContent" style="font-size:12px margin-left:30px;color:Red" runat="server"  >
       </div>
            </div>
            <%--<div class="form-col-8-8">
              <p class="para pad10-0">You can review this order before it’s final.</p>
            </div>--%>
            <div class="cl"></div>
          </div>
                     				
			

</fieldset>

             
     

</div>
<fieldset>
 <legend>Shipping & Order Details</legend>
 <table  width="100%" border="0" cellpadding="0" cellspacing="0"  >         
         

                    <tr>
                     <td>
                     <fieldset>
<legend>Bill To</legend>
<p class="para pad10"> <asp:Label ID="lblDeliveryTo" runat="server" Text="Delivery Address" CssClass="LabelStyle"
            Font-Bold="false" style="font-weight:normal;"   ></asp:Label></p>
<div class="cl"></div>
</fieldset>
</td>
<td>
                     <fieldset>
<legend>Ship To</legend>
<p class="para pad10"> <asp:Label ID="lblShipTo" runat="server" Text="Shipping Address" CssClass="LabelStyle"
                Font-Bold="false"  style="font-weight:normal;"></asp:Label>
</p>
<div class="cl"></div>
</fieldset>
                     </td>
                    </tr>
                  


                    <tr>
                    <td colspan="2" >
<fieldset>
<legend>Order Contents</legend>
<div class="form-col-8-8">
<table width="100%" border="0" >
    
 <%--   <tr>
      <td width="100%" colspan="2" >
        
      </td>
    </tr>--%>
<%--    <tr>
        <td  width="100%" colspan="2"  align="left" >
        <H3 class="title1" style="TEXT-ALIGN: left">ORDER CONTENTS</H3>
            
        </td>
    </tr>--%>
    <tr>
        <td width="100%" colspan="2" >
            <table  width="100%" id="test1" border="0" cellpadding="3" cellspacing="0"  class="orderdettable" >
         
                <tr  class="" style="background-color:#BCD0E2;">
                    <td align="left" width="20%">
                        ORDER CODE
                    </td>
                    <td align="left" width="10%">
                        QTY
                    </td>
                    <td align="left" width="25%">
                        Description
                    </td>
                    <td  align="right" width="20%">
                        Cost(Ex. GST)
                    </td>
                    <td align="left" width="30%">
                        Extension Amount (Ex. GST)
                    </td>
                </tr> 
                <asp:Repeater ID="OrderitemdetailRepeater"   runat="server"  > 
                 
                    <ItemTemplate >
                        
                        <tr id="tRow"  runat="server"  class="rowOdd">
                        <td  id="TD1" runat="server" style="text-align:left;"><%# Eval("CATALOG_ITEM_NO")%></td>
                        <td style="text-align:left;"><%# Eval("QTY")%></td>
                        <td style="text-align:left;"> <%# Eval("DESCRIPTION")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal(Eval("PRICE_EXT_APPLIED")).ToString("#,#0.00")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal (Eval("TOTAL_EXT")).ToString("#,#0.00") %></td>                                       
                       </tr>  
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="tRow"  runat="server" class="">
                        <td  id="TD1" runat="server" style="text-align:left;"><%# Eval("CATALOG_ITEM_NO")%></td>
                        <td style="text-align:left;"><%# Eval("QTY")%></td>
                        <td style="text-align:left;"> <%# Eval("DESCRIPTION")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal(Eval("PRICE_EXT_APPLIED")).ToString("#,#0.00")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal (Eval("TOTAL_EXT")).ToString("#,#0.00") %></td>
                       </tr>  
                    </AlternatingItemTemplate>
               </asp:Repeater> 
         
       
               <tr style="background-color: white; font-size: 12px;">
                  <td align="left" width="50%"  colspan="3"   rowspan="4" valign="bottom">
                        <asp:Button ID="ImgBtnEditShipping" runat="server" Text="Edit / Update Order" style="float: left !important;font-size: 12px;
    text-shadow: none;" OnClick="ImgBtnEditShipping_Click" class="btn btn-default fright" CausesValidation="false" />
                    </td>
 
                    <td  align="left" width="20%" class="">
                        <strong>Sub Total (Ex GST)</strong>
                    </td>
                  <td align="left" width="30%" class="" style="text-align:right;">
                     <strong>  $   <asp:Label runat="server" ID="Product_Total_price"/> </strong>
                  </td>
                </tr>
                <tr style="background-color: white; font-size: 12px;">
                        <%-- <td align="left" width="50%"  >
                        
                        </td>--%>

                        <td  align="left" width="20%" class="rowOdd">
                           <%-- <strong>Delivery / Handling Charge (Ex GST) </strong>--%>
                           <strong>Delivery (Ex GST)</strong>
                        </td>
                        <td align="left" width="30%" class="rowOdd" style="text-align:right;">
                            <strong> $  <asp:Label runat="server" ID="lblCourier"/></strong>
                        </td>
                    </tr>
                 <tr style="background-color: white; font-size: 12px;">
                 <%-- <td align="left" width="50%"  >
                        
                    </td>--%>

                    <td  align="left" width="20%" class="rowOdd">
                        <strong>Total Tax Amount (GST) </strong>
                    </td>
                  <td align="left" width="30%" class="rowOdd" style="text-align:right;">
                      <strong> $  <asp:Label runat="server" ID="Tax_amount"/></strong>
                  </td>
                </tr>
                 
                 <tr style="background-color: white; font-size: 12px;">
                  <%--<td align="left" width="50%"  >
                        
                    </td>--%>

                    <td  align="left" width="20%" class="Rsucess">
                        <strong> <asp:Label runat="server" ID="lblTotalCap"/></strong>
	
                    </td>
                  <td align="left" width="30%" class="Rsucess" style="text-align:right;">
                    <strong>  $  <asp:Label runat="server" ID="Total_Amount"/></strong>
	 
                  </td>
                </tr>
              
              
            </table>
        </td>
    </tr>
</table>
</div>
</fieldset>
                    </td>
                    </tr>

                    
                    </table>
                   
</fieldset>


            

   
</div>
</div>
<div class="grid12" runat="server" id="divTimeout" visible="false"    >
    <fieldset>
    <div style="text-align:center;padding:130px;" >
    <span style="font-size:21px;"  > Your session has timed out</span><br />
    <span style="font-size:14px;"> <a href="/Login.aspx" class="para pad10-0" style="font-size:11px; color:#0033cc; font-weight:bold;">Click here</a> to log in again </span>
    </div>
    </fieldset>
</div>
<div class="grid12" runat="server" id="divPayUnavailable" visible="false"    >
    <fieldset>
    <div style="text-align:center;padding:130px;" >    <span style="font-size:21px;"  > Payment Option is currently unavailable.</span><br />
    
    </div>
    </fieldset>
</div>
 </div>

 </td>
 </tr>
 
 </table>
  
</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
