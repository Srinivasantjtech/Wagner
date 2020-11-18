<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="shipping" EnableEventValidation="false"
    Title="Untitled Page"  Culture="en-US" UICulture="en-US" Codebehind="shipping.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UI/InvoiceOrder.ascx" TagName="InvoiceOrder" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function isAlphabetic() {
            var ValidChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890/\\';
            var sText = document.getElementById("ctl00_maincontent_tt1").value;
            var IsAlphabetic = true;
            var Char;
            var ErrMsg = 'Sorry, but the use of the following special characters is not allowed in the Order No field:' + '\n' + '! ` & ~ ^ * %  $ @ ’ ( “ ) ; [   ] { } ! = < >  | * , . -' + '\n' + 'Please update your order no so it longer has any of these restricted characters in it to continue order.';
            var err = document.getElementById("ctl00_maincontent_txterr");
            if (err != null) {
                err.innerHTML = '';
            }
            for (i = 0; i < sText.length; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    alert(ErrMsg);
                    document.getElementById("ctl00_maincontent_tt1").value = '';
                    document.forms[0].elements["<%=tt1.ClientID%>"].focus();
                    return false;
                }
            }

            return isAlphabetic;
        }

        function checkorderid() {
            var msgCheck = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            /* if (document.forms[0].elements["<%=tt1.ClientID%>"].value.length == 0) {
            alert('Enter Order No and then proceed');
            document.forms[0].elements["<%=tt1.ClientID%>"].focus();
            return false;
            }
            else*/
            if (document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Please Select Shipping Method") {
                alert('Please Select Shipping Method');
                document.forms[0].elements["<%=drpSM1.ClientID%>"].focus();
                return false;
            }
            else if ((document.forms[0].elements["<%=TextBox1.ClientID%>"].value == msgCheck || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == '' || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == null) && document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Courier Pickup") {
                //alert('Please Enter Comments and Submit Order');
                ShowCourierMessage();
                document.forms[0].elements["<%=TextBox1.ClientID%>"].focus();
                return false;
            }
            else {
                var numaric = document.forms[0].elements["<%=tt1.ClientID%>"].value;
                for (var j = 0; j < numaric.length; j++) {
                    var alphaa = numaric.charAt(j);
                    var hh = alphaa.charCodeAt(0);
                    //|| hh == 95 || hh == 45
                    if ((hh > 47 && hh < 58) || (hh > 64 && hh < 91) || (hh > 96 && hh < 123) || hh == 92 || hh == 47) {
                    }
                    else {
                        if (hh == 32) {
                            alert("Blank space in order no");
                        }
                        else
                            alert("Please enter valid order no, Order no should have alpha-numeric character.");
                        return false;
                    }
                }

                return (ValidationDropShipOrder()); ;
            }

            return (ValidationDropShipOrder());
        }

        $(document).ready(function () {
            var msgShipment = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter the details of Courier Company that you will be" + '\n' + "arranging to pick up your parcel from us with.";
            var msgOthers = "Type Comments Here";
            $("#<%=drpSM1.ClientID %>").change(function (event) {
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Courier Pickup") {
                    ShowCourierMessage();
                }
            });
        });

        $(document).ready(function () {
            if ($find("ShipmentModelPopupExtender") != null) {
                $find("ShipmentModelPopupExtender").hide();
            }
            $("#<%=drpSM1.ClientID %>").change(function (event) {
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Mail") {
                    ShowMailMessage();

                }

            });


        });
        $(document).ready(function () {
            if ($find("ShipmentModelPopupExtender") != null) {
                $find("ShipmentModelPopupExtender").hide();
            }
            $("#<%=drpSM1.ClientID %>").change(function (event) {
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {

                    ShowShopCounterPickupMessage();
                    showtotalamount();

                }
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Standard Shipping") {
                    showcourieramount();
                }

                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Please Select Shipping Method") {
                    intltotalamount();
                }

                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "International Shipping - TBA") {
                    intercustotalamt();
                }
            });


        });
        $(document).ready(function () {
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Standard Shipping") {
                showcourieramount();
            }
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                showtotalamount();
            }
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "International Shipping - TBA") {
                intercustotalamt();
            }
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Please Select Shipping Method") {
                intltotalamount();
            }


        });
        function showcourieramount() {
            //alert("inner");
            document.getElementById("totamtwithcouriercrge").style.display = 'block';
            document.getElementById("totalamt").style.display = 'none';
            document.getElementById("totalcoupickup").style.display = 'none';
            document.getElementById("corierhandcrgeR").style.display = '';
            document.getElementById("taxamtwithcouriercrge").style.display = 'block';
            document.getElementById("taxamtcoupickup").style.display = 'none';
            document.getElementById("TaxAmount").style.display = 'none';
            document.getElementById("ICtotalamount").style.display = 'none';
        }

        function intercustotalamt() {
            document.getElementById("totamtwithcouriercrge").style.display = 'none';
            document.getElementById("totalcoupickup").style.display = 'none';
            document.getElementById("totalamt").style.display = 'none';
            document.getElementById("corierhandcrgeR").style.display = 'none';

            document.getElementById("taxamtcoupickup").style.display = 'none';
            document.getElementById("taxamtwithcouriercrge").style.display = 'none';
            document.getElementById("TaxAmount").style.display = 'block';
            document.getElementById("ICtotalamount").style.display = 'block';
        }

        function showtotalamount() {
            document.getElementById("totamtwithcouriercrge").style.display = 'none';
            // document.getElementById("totalcoupickup").style.display = 'none'; 
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                document.getElementById("totalcoupickup").style.display = 'block';
            }
            else {
                document.getElementById("totalcoupickup").style.display = 'none';
            }
            document.getElementById("totalamt").style.display = 'none';
            document.getElementById("corierhandcrgeR").style.display = 'none';

            document.getElementById("taxamtcoupickup").style.display = 'block';
            document.getElementById("taxamtwithcouriercrge").style.display = 'none';
            document.getElementById("TaxAmount").style.display = 'none';
            document.getElementById("ICtotalamount").style.display = 'none';
        }
        function intltotalamount() {
            document.getElementById("totalamt").style.display = 'block';
            document.getElementById("totamtwithcouriercrge").style.display = 'none';
            //document.getElementById("totalcoupickup").style.display = 'none';
            document.getElementById("corierhandcrgeR").style.display = 'none';
            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                document.getElementById("totalcoupickup").style.display = 'block';
            }
            else {
                document.getElementById("totalcoupickup").style.display = 'none';
            }
            document.getElementById("taxamtcoupickup").style.display = 'none';
            document.getElementById("taxamtwithcouriercrge").style.display = 'none';
            document.getElementById("TaxAmount").style.display = 'block';
            document.getElementById("ICtotalamount").style.display = 'none';
        }

        function ShowCourierMessage() {
            var msg = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter into the Comments / Notes box the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            alert(msg);
        }
        function ShowMailMessage() {
            var msg = "**** NOTE ****" + '\n' + " Mail will be used for parcels up to 500 grams including packaging. Parcels over 500 grams will be sent by the most economical way e.g. Courier, Road, etc. ";
            alert(msg);
        }
        function ShowShopCounterPickupMessage() {
            var msg = " IMPORTANT DETAILS FOR WHEN PICKING UP GOODS FROM STORE " + '\n' + '\n' +
                      "1. Please being a printed copy of invoice when coming into store." + '\n' + '\n' +
                      "2. Proof of Identity is required. When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with. " + '\n' +
                      "The Name on the Credit Card Must Match Your Drivers License ID. Please ensure you have the Credit Card you made the purchase with and your Driver’s License ID when coming into the store to pick up goods purchased online. " + '\n' +
                      "You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Drivers License are shown OR if there if there is a mismatch in details. " + '\n' +
                      "When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with. ";
            alert(msg);
        }
        function MouseHover() {
            $find("ShipmentModelPopupExtender").show();
        }

        function MouseOut() {
            $find("ShipmentModelPopupExtender").hide();
        }

        function CheckShippment() {

            switch (document.getElementById("ctl00_maincontent_drpSM1").value) {
                //                case 'Mail':  
                //                    ShowShipmentPanel();  
                //                    break;  
                case 'Standard Shipping':
                    ShowShipmentPanel();
                    break;
                case 'Courier Pickup':
                    ShowShipmentPanel();
                    break;
                case 'Counter Pickup':
                    ShowShipmentPanel();
                    break;
                case 'Drop Shipment Order':
                    ShowDropShipmentPanel();
                    break;
                default:
                    ShowShipmentPanel();
                    break;
            }
        }

        function ShowDropShipmentPanel() {
            document.getElementById("DropShipmentRow").style.display = '';
            document.getElementById("OtherShipmentRow").style.display = 'none';

        }

        function ShowShipmentPanel() {
            document.getElementById("OtherShipmentRow").style.display = '';
            document.getElementById("DropShipmentRow").style.display = 'none';

        }

        function HidePanels() {
            if (document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') {
                document.getElementById("OtherShipmentRow").style.display = 'none';
                document.getElementById("DropShipmentRow").style.display = '';

            }
            else {
                document.getElementById("OtherShipmentRow").style.display = '';
                document.getElementById("DropShipmentRow").style.display = 'none';

            }
        }

        function ValidationDropShipOrder() {
            var isCompanyEmpty = false;
            var isStateEmpty = false;
            var isPostcodeEmpty = false;
            var isSuburbEmpty = false;
            var isadd1key = false;
            var isadd2key = false;
            var isPCkey = false;

            //            // * comment by palani

            //            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtCompany").value == '') || (document.getElementById("ctl00_maincontent_txtCompany").value == null) || (document.getElementById("ctl00_maincontent_txtCompany").value == 'null'))) {
            //                document.getElementById("ctl00_maincontent_txtCompany").style.borderColor = "red";
            //                document.getElementById("ctl00_maincontent_txtCompany").focus();
            //                isCompanyEmpty = true;
            //            }
            //            else {
            //                document.getElementById("ctl00_maincontent_txtCompany").style.borderColor = "ActiveBorder";
            //            }
            //            *//


            //            // * comment by palani

            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtAttentionTo").value == '') || (document.getElementById("ctl00_maincontent_txtAttentionTo").value == null) || (document.getElementById("ctl00_maincontent_txtAttentionTo").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtAttentionTo").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtAttentionTo").focus();
                isCompanyEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtAttentionTo").style.borderColor = "ActiveBorder";
            }
            //                       


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_drpState").value == 'Select Ship To State') || (document.getElementById("ctl00_maincontent_drpState").value == null) || (document.getElementById("ctl00_maincontent_drpState").value == 'null'))) {
                document.getElementById("ctl00_maincontent_drpState").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_drpState").focus();
                isStateEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_drpState").style.borderColor = "ActiveBorder";
            }


            //            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtState").value == '') || (document.getElementById("ctl00_maincontent_txtState").value == null) || (document.getElementById("ctl00_maincontent_txtState").value == 'null'))) {
            //                document.getElementById("ctl00_maincontent_txtState").style.borderColor = "red";
            //                document.getElementById("ctl00_maincontent_txtState").focus();
            //                isStateEmpty = true;
            //            }
            //            else {
            //                document.getElementById("ctl00_maincontent_txtState").style.borderColor = "ActiveBorder";
            //            }
            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtAddressLine1").value == '') || (document.getElementById("ctl00_maincontent_txtAddressLine1").value == null) || (document.getElementById("ctl00_maincontent_txtAddressLine1").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtAddressLine1").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtAddressLine1").focus();
                isStateEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtAddressLine1").style.borderColor = "ActiveBorder";
                //PageMethods.GetDropShipmentKeyExists(document.getElementById("ctl00_maincontent_txtAddressLine1").value,"", OnSuccess1, OnFailure1)
            }


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtPostCode").value == '') || (document.getElementById("ctl00_maincontent_txtPostCode").value == null) || (document.getElementById("ctl00_maincontent_txtPostCode").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtPostCode").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtPostCode").focus();
                isPostcodeEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtPostCode").style.borderColor = "ActiveBorder";
                // PageMethods.GetDropShipmentKeyExists(document.getElementById("ctl00_maincontent_txtPostCode").value, "PostCode", OnSuccess1, OnFailure1)
            }

            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtSuburb").value == '') || (document.getElementById("ctl00_maincontent_txtSuburb").value == null) || (document.getElementById("ctl00_maincontent_txtSuburb").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtSuburb").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtSuburb").focus();
                isSuburbEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtSuburb").style.borderColor = "ActiveBorder";
            }

            if ((isCompanyEmpty == true) || (isStateEmpty == true) || (isPostcodeEmpty == true) || (isSuburbEmpty == true)) {
                alert('Fill required fields before submit!');
                return false;
            }
            else {
                return true;
            }




        }
    </script>
  <script type="text/javascript">
      function DRPshippment() {
          alert('Non-Standard Delivery Area. We will contact you to confirm costing');
      }
</script> 
<div class="container">
<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">Checkout</li>
            </ul>
        </div>
    </div>
<div class="row">
<div aria-multiselectable="true" role="tablist" id="accordion" class="panel-group clear">
<div class="panel panel-default">

<%--<div id="headingOne" role="tab" class="panel-heading">
                      <h4 class="panel-title"> 
                      <a aria-controls="collapseOne" aria-expanded="true" href="#collapseOne" class="" data-parent="#accordion" data-toggle="collapse"> Shipping / Delivery Details 
                      <span><img class="pull-right" src="/images/select-downarrow.png"/></span> </a> 
                      </h4>
  </div>--%>
  <a aria-controls="collapseOne" aria-expanded="true" href="#collapseOne" class="checkoutpanel" data-parent="#accordion" data-toggle="collapse">
                    <div id="headingOne" role="tab" class="panel-heading">
                      <h4 class="panel-title"> 
                       Shipping / Delivery Details 
                      <span>
                      <img class="pull-right" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/select-downarrow.png"/>
                      </span>
                        </h4>
                    </div>
                    </a>

  <div aria-labelledby="headingOne" role="tabpanel" class="panel-collapse collapse in" id="collapseOne">
  <div class="panel-body">
   <%--    <table width="100%" cellspacing="0" cellpadding="5" align="center" border="0">
        <tr>
            <td align="left" >  --%>
              
             <%-- <div  id="page-wrap">  --%>      
              <div class="grid12" style="display:none;">
              <ul class="breadcrumb_wag">
              <li>
                <span class="aero currentpg">Shipping / Delivery Details</span>
              </li>
               <li runat="server" id="liPayOption">
                <span class="aero">Payment Options</span>
              </li>
              <li runat="server" id="liFinalReview">
<span class="aero">Completed</span>
</li>
              </ul>
              </div>
            <%--  <div class="grid12">--%>
              <%--  <div >    --%>
       <asp:PlaceHolder runat="server" ID="PHOrderConfirm" Visible="false" EnableViewState ="false">
                <% 
                    
                    if (Convert.ToInt16(Session["USER_ROLE"]) == 3)
                    {
                        
                        
                %>
                    <div class="alert yellowbox icon_1">
                      <h3 style="font-size: 16px;">Order Now Pending Approval</h3>
                       <p class="p2">
                         Your order is now pending approval form your company supervisor/s before it can be submitted to us for processing.
                       <br />
                         The following member/s in your company will be able to authorise and submite your order:
                      <br />
                      <asp:Label ID="lblUserRoleName" runat="server" Visible="true" Font-Names="Arial"  Font-Size="11px" ForeColor="Black" Font-Bold="true" Text=""></asp:Label>
                      </p>
                      </div>
                           

                               
                <% 
                        Session["ORDER_ID"] = "0";
                        Session["Multipleitems"] = null;
                    }
                    else if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    {
                %>
                   <div class="alert greenbox icon_3" style="padding:30px 10px 30px 60px">
                         <h3 style="font-size: 16px;">Your order has been successfully submitted to us for processing. Thank You!</h3>
                   </div>
                  
                <% 
                        Session["ORDER_ID"] = "0";
                        Session["Multipleitems"] = null;
                    } 
                %>
    </asp:PlaceHolder>   
         
       <%--     <p class=" para fright"><span class="red">*</span>Required Fields</p>
            <div class="cl"></div>--%>
           <%-- <form>--%>
            <%--<fieldset>--%>
           <%-- <legend>Enter your Purchase Order No.</legend>--%>
           <%-- <div class="form-col-2-8">
          
           <span class="formfield">Order No.</span>
          
           
             </div> --%>
             <div class="accordion_head_grey">Enter your Purchase Order No</div>
             <div class="col-lg-10">
                          <div class="form-group">
                            <label for="inputEmail3" class="col-sm-8 control-label">Order No.</label>
                            <div class="col-sm-12">
                             <%-- <input type="email" class="form-control checkout_input" id="inputEmail3" placeholder="Email">--%>
                               <asp:TextBox  MaxLength="12" ID="tt1" runat="server"  CssClass=" col-sm-10 form-control checkout_input"  />
                                <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
                            </div>
                            <div class="clear"></div>
                          </div>
                        </div>
            <%-- <div class="form-col-3-8">
                 <asp:TextBox Width="100%" MaxLength="12" ID="tt1" runat="server"  CssClass="txtboxship"  />
             </div> --%> 
            <%-- <div class="form-col-3-8">
             <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
             </div>--%>
           <%--  </fieldset>  --%>  

            <%-- <fieldset>  --%>
               <%--<legend>Shipping</legend>  --%> 
               
               <div class="accordion_head_grey clear">Shipping</div>
               <div class="cshipping_dtl">
               <div class="col-lg-10">
                            <div class="form-group">
                              <label for="inputEmail3" class="col-sm-8 control-label font_normal">Select Shipment Method</label>
                              <div class="col-sm-12">
                               <%-- <select class="form-control checkout_input">
                                  <option>Select</option>
                                </select>--%>
                                 <asp:DropDownList NAME="drpSM1" ID="drpSM1" runat="server" CssClass="form-control checkout_input">
                                            <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method" >Please Select Shipping Method</asp:ListItem>
                                           <asp:ListItem Text="Standard Shipping" Value="Standard Shipping">Standard Shipping</asp:ListItem>
                                            <asp:ListItem Text="Counter Pickup" Value="Counter Pickup">Shop Counter Pickup</asp:ListItem>
                     </asp:DropDownList>
                              </div>
                              <div class="clear"></div>
                            </div>
                          </div>
               <div class="col-lg-10">
                            <div class="form-group">
                              <label for="inputEmail3" class="control-label" id="HyperLink1">
                              <a href="#" class="checkoutlink" data-target=".lgn-orderinfo" data-toggle="modal" role="button">
                              <img class=" margin_rgt15" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/info.png"/> Learn More About Shipment Method Options</a>
                              </label>
                              <div class="clear"></div>
                            </div>
                          </div>
               </div>  
               <div class="col-lg-20 col-xs-20" id="OtherShipmentRow">
               <div class="col-lg-10">
                            <div class="form-group">
                              <label for="inputEmail3" class="control-label">Address</label>
                              <div>
                               <%-- <textarea rows="5" class="width_100 resize"></textarea>--%>
                               <textarea id="Ta3" cols="34"    class="width_95" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10"    name="Ta2"></textarea>
                              </div>
                              <div class="clear"></div>
                            </div>
                          </div>
               <div class="col-lg-10">
                            <div class="form-group">
                              <label for="inputEmail3" class="control-label">Address</label>
                              <div>
                                <%--<textarea rows="5" class="width_100 resize"></textarea>--%>
                                 <textarea id="Ta2" cols="34"  class="width_95" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10"  name="Ta3"></textarea>
                              </div>
                              <div class="clear"></div>
                            </div>
                          </div>
               </div>
               <div id="DropShipmentRow">
                    <div class="alert yellowbox">
                          <h3 style="font-size:16px;">Please Enter Shipment Delivery Details</h3>
                          <p class="p2 fright">
                            <span class="red"  style="color:#FF0000;">* </span>Required Fields
                          </p>
                          <div class="clear"></div>
                        <div class=" form-col-2-8">
                        <p class="p2">Company Name</p>
                        </div>
                        <div class=" form-col-3-8">
                          <asp:TextBox ID="txtCompany" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                           <asp:Label Width="150px" ID="Label13" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div class=" form-col-2-8">
                            <p class="p2">
                            Attn to / Receivers Code
                            <span class="red" style="color:#FF0000;">*</span>
                            </p>
                        </div>
                        <div class=" form-col-3-8">
                            <asp:TextBox ID="txtAttentionTo" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                            <asp:Label Width="150px" ID="Label27" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div class=" form-col-2-8">
                           <p class="p2">Receivers Contact Number</p>
                        </div>
                        <div class=" form-col-3-8">
                           <asp:TextBox ID="txtShipPhoneNumber" runat="server" Width="242px" MaxLength="40" CssClass="input_dr" />
                           <asp:Label Width="150px" ID="Label26" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                         <div class=" form-col-2-8">
                            <p class="p2">Address Line 1</p>
                         </div>
                          <div class=" form-col-3-8">
                           <asp:TextBox ID="txtAddressLine1" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                            <asp:Label Width="150px" ID="lbladdline1err" runat="server" ForeColor="red"></asp:Label>
                          </div>
                            <div class="clear"></div>
                             <div class=" form-col-2-8">
                               <p class="p2">Address Line 2</p>
                            </div>
                             <div class=" form-col-3-8">
                              <asp:TextBox ID="txtAddressLine2" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                              <asp:Label Width="150px" ID="lbladdline2err" runat="server" ForeColor="red"></asp:Label>
                             </div>
                             <div class="clear"></div>
                             <div class=" form-col-2-8">
                                <p class="p2">
                                Suburb
                                <span class="red" style="color:#FF0000;">*</span>
                                </p>
                             </div>
                               <div class=" form-col-3-8">
                               <asp:TextBox ID="txtSuburb" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                               <asp:Label Width="150px" ID="Label21" runat="server" ForeColor="red"></asp:Label>
                               </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                <p class="p2">
                                State
                                <span class="red" style="color:#FF0000;">*</span>
                                </p>
                                </div>
                                <div class=" form-col-3-8">
                                  <asp:DropDownList Visible="true" ID="drpState" runat="server" Width="250px"> 
                                        <asp:ListItem Text="Select Ship To State"></asp:ListItem>
                                            <asp:ListItem Text="ACT"></asp:ListItem>
                                            <asp:ListItem Text="NSW"></asp:ListItem>
                                            <asp:ListItem Text="NT"></asp:ListItem>
                                            <asp:ListItem Text="QLD"></asp:ListItem>
                                            <asp:ListItem Text="SA"></asp:ListItem>
                                            <asp:ListItem Text="TAS"></asp:ListItem>
                                            <asp:ListItem Text="VIC"></asp:ListItem>
                                            <asp:ListItem Text="WA"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label Width="150px" ID="Label22" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                    <div class="clear"></div>
                                    <div class=" form-col-2-8">
                                    <p class="p2">
                                        Post Code
                                        <span class="red" style="color:#FF0000;">*</span>
                                        </p>
                                    </div>
                                    <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtPostCode" runat="server" MaxLength="4" Width="242px" CssClass="input_dr" />
                                     <asp:Label Width="150px" ID="lblpostcode2err" runat="server" ForeColor="red"></asp:Label>
                                     <asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtPostCode" />
                                    </div>
                                    <div class="clear"></div>
                                    <div class=" form-col-2-8">
                                    <p class="p2">Country</p>
                                    </div>
                                    <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="input_dr" Width="242px" Text="Australia" ReadOnly="True" />
                                        <asp:Label Width="150px" ID="Label24" runat="server" ForeColor="red"></asp:Label>
                                    </div>
                                     <div class="clear"></div>
                                      <div class=" form-col-2-8">
                                        <p class="p2">Delivery Instructions</p>
                                      </div>
                                      <div class=" form-col-3-8">
                                      <asp:TextBox ID="txtDeliveryInstructions"  runat="server" Width="242px"    Class="textSkin" CssClass="input_dr" MaxLength="40"   /> 
                                        <asp:Label Width="150px" ID="Label25" runat="server" ForeColor="red"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtShipPhoneNumber" />
                                      </div>
                                      <div class="clear"></div>
                        </div>
                        <div class="clear padbot10"></div>
                         <div class="clear"></div>
                        <div class="form-col-8-8">
                                <textarea  id="Ta4" cols="34"   Class="textarea1"  readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10" name="Ta4"></textarea>
                        </div>
                         <div class="clear"></div>
                 </div> 
               <div class="accordion_head_grey clear">Comments / Notes</div>
               <div class="col-lg-20 col-xs-20">
                          <div class="col-lg-20 form-group">
                          <%--  <textarea rows="5" class="width_100 resize"></textarea>--%>
                           <asp:TextBox ID="TextBox1"  runat="server" Rows="5" Columns="30"  CssClass="width_100 resize"   MaxLength="240"  onkeyDown="return checkMaxLength(this,event,'240');" 
                                TextMode="MultiLine">
                            </asp:TextBox>
                          </div>
                        </div>
             <%-- <div class="form-col-2-8">
                          <span class="formfield">Select Shipment Method<span class="red"> *</span></span>
              </div>--%>
        
            <%--  <div class="form-col-3-8">
                       <asp:DropDownList NAME="drpSM1" ID="drpSM1" runat="server" CssClass="txtinput1">
                                            <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method" >Please Select Shipping Method</asp:ListItem>
                                           <asp:ListItem Text="Standard Shipping" Value="Standard Shipping">Standard Shipping</asp:ListItem>
                                            <asp:ListItem Text="Counter Pickup" Value="Counter Pickup">Shop Counter Pickup</asp:ListItem>
                     </asp:DropDownList>
             </div>--%>
           <%--   <div class="form-col-3-8">
                      
                         <a class="" href="#">
                    
                            <asp:HyperLink ID="HyperLink1" runat="server" onclick="MouseHover();" CssClass="HyperLinkStyle">
                            <img id="Img11" runat="server" src="~/images/alert.png" alt="" onclick="MouseHover();" style="cursor: pointer;" />
                             Learn More About Shipment Method Options 
                             </asp:HyperLink>
                           </a> 
                      </div>
             <div class="cl"></div>--%>
            <%-- <div class="form-col-2-8">
                         <span class="formfield">Address</span>
             </div>--%>

             <%--    <div id="OtherShipmentRow">
                     <div class="form-col-3-8">
                         <textarea id="Ta3" cols="34"   Class="textarea1" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10"  style="height:150px;"  name="Ta2"></textarea>
                    </div>
                     <div class="form-col-3-8">
                        <textarea id="Ta2" cols="34"  Class="textarea1" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10" style="height:150px;"  name="Ta3"></textarea>
                     </div>
                 </div>
                 <div class="clear"></div>--%>
                 <%--<div id="DropShipmentRow">
                    <div class="alert yellowbox">
                          <h3 style="font-size:16px;">Please Enter Shipment Delivery Details</h3>
                          <p class="p2 fright">
                            <span class="red"  style="color:#FF0000;">* </span>Required Fields
                          </p>
                          <div class="clear"></div>
                        <div class=" form-col-2-8">
                        <p class="p2">Company Name</p>
                        </div>
                        <div class=" form-col-3-8">
                          <asp:TextBox ID="txtCompany" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                           <asp:Label Width="150px" ID="Label13" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div class=" form-col-2-8">
                            <p class="p2">
                            Attn to / Receivers Code
                            <span class="red" style="color:#FF0000;">*</span>
                            </p>
                        </div>
                        <div class=" form-col-3-8">
                            <asp:TextBox ID="txtAttentionTo" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                            <asp:Label Width="150px" ID="Label27" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div class=" form-col-2-8">
                           <p class="p2">Receivers Contact Number</p>
                        </div>
                        <div class=" form-col-3-8">
                           <asp:TextBox ID="txtShipPhoneNumber" runat="server" Width="242px" MaxLength="40" CssClass="input_dr" />
                           <asp:Label Width="150px" ID="Label26" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                         <div class=" form-col-2-8">
                            <p class="p2">Address Line 1</p>
                         </div>
                          <div class=" form-col-3-8">
                           <asp:TextBox ID="txtAddressLine1" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                            <asp:Label Width="150px" ID="lbladdline1err" runat="server" ForeColor="red"></asp:Label>
                          </div>
                            <div class="clear"></div>
                             <div class=" form-col-2-8">
                               <p class="p2">Address Line 2</p>
                            </div>
                             <div class=" form-col-3-8">
                              <asp:TextBox ID="txtAddressLine2" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                              <asp:Label Width="150px" ID="lbladdline2err" runat="server" ForeColor="red"></asp:Label>
                             </div>
                             <div class="clear"></div>
                             <div class=" form-col-2-8">
                                <p class="p2">
                                Suburb
                                <span class="red" style="color:#FF0000;">*</span>
                                </p>
                             </div>
                               <div class=" form-col-3-8">
                               <asp:TextBox ID="txtSuburb" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                               <asp:Label Width="150px" ID="Label21" runat="server" ForeColor="red"></asp:Label>
                               </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                <p class="p2">
                                State
                                <span class="red" style="color:#FF0000;">*</span>
                                </p>
                                </div>
                                <div class=" form-col-3-8">
                                  <asp:DropDownList Visible="true" ID="drpState" runat="server" Width="250px"> 
                                        <asp:ListItem Text="Select Ship To State"></asp:ListItem>
                                            <asp:ListItem Text="ACT"></asp:ListItem>
                                            <asp:ListItem Text="NSW"></asp:ListItem>
                                            <asp:ListItem Text="NT"></asp:ListItem>
                                            <asp:ListItem Text="QLD"></asp:ListItem>
                                            <asp:ListItem Text="SA"></asp:ListItem>
                                            <asp:ListItem Text="TAS"></asp:ListItem>
                                            <asp:ListItem Text="VIC"></asp:ListItem>
                                            <asp:ListItem Text="WA"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label Width="150px" ID="Label22" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                    <div class="clear"></div>
                                    <div class=" form-col-2-8">
                                    <p class="p2">
                                        Post Code
                                        <span class="red" style="color:#FF0000;">*</span>
                                        </p>
                                    </div>
                                    <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtPostCode" runat="server" MaxLength="4" Width="242px" CssClass="input_dr" />
                                     <asp:Label Width="150px" ID="lblpostcode2err" runat="server" ForeColor="red"></asp:Label>
                                     <asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtPostCode" />
                                    </div>
                                    <div class="clear"></div>
                                    <div class=" form-col-2-8">
                                    <p class="p2">Country</p>
                                    </div>
                                    <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="input_dr" Width="242px" Text="Australia" ReadOnly="True" />
                                        <asp:Label Width="150px" ID="Label24" runat="server" ForeColor="red"></asp:Label>
                                    </div>
                                     <div class="clear"></div>
                                      <div class=" form-col-2-8">
                                        <p class="p2">Delivery Instructions</p>
                                      </div>
                                      <div class=" form-col-3-8">
                                      <asp:TextBox ID="txtDeliveryInstructions"  runat="server" Width="242px"    Class="textSkin" CssClass="input_dr" MaxLength="40"   /> 
                                        <asp:Label Width="150px" ID="Label25" runat="server" ForeColor="red"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtShipPhoneNumber" />
                                      </div>
                                      <div class="clear"></div>
                        </div>
                        <div class="clear padbot10"></div>
                         <div class="clear"></div>
                        <div class="form-col-8-8">
                                <textarea  id="Ta4" cols="34"   Class="textarea1"  readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10" name="Ta4"></textarea>
                        </div>
                         <div class="clear"></div>
                 </div> --%>                                          
        <%--   </fieldset>  --%>    

         <div style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfo">
                  <div class="modal-dialog">
                        <div class="modal-content">

                          <div class="modal-header green_bg">
                            <h4 id="H1" class="text-center">
                            <img class="popsucess" alt="img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Order__info.png"/>Shipment Method Options</h4>
                          </div>
                              
                          <div class="modal-body">
                              <div class="col-lg-20 text-center col-sm-10 col-md-5">
                              <div class="privacypolicy">
                             
                                  <p style="font-size:12px;">
                                 <b style="font-size:16px;">Courier Service</b>
                                <br />
                                Australia wide flat rate delivery charge of $9.90*
                                <br />
                                Goods will be sent using Courier Service if under Gross /Cubic weight of 3Kg.                                 
                                <br />
                                Parcels over 3Kg will be sent by most economical means, eg Road, E-Parcel Service etc. 
                                <br />
                                Exemptions apply to heavy / large items (e.g Server Racks) will occur higher delivery charges our sales team will contact you prior to shipping. 
                                <br />
                                Note. Dangerous goods cannot be sent by air, road service only.
                             <br />
                        
                          
                                <b style="font-size:16px;">Shop Counter Pick Up</b>
                                <br />
                                Place your order online and pick up goods from our Sydney Showroom.
                                <br>
                                Important Details For Store Pick Up:
                                <br>
                                1. Please bring a printed copy of invoice when coming into store.
                                <br />
                                2. Proof of Identity is required when picking up goods from our store. Please ensure you have the Credit Card you made the purchase with and your Driver’s 
                                <br />
                                License ID when coming into the store to pick up goods purchased online. The Name on the Credit Card Must Match Your Drivers License ID.
                                <br />
                                You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Drivers License are shown OR if there if 
                                <br />
                                there is a mismatch in details.  
                                </p> 
                                </div>
                              </div>                             
                          </div>
                          <div class="modal-footer clear border_top_none">
                           
                          </div>
                      </div>
                  </div>
    </div>
           <%--<div id="PopDiv" class="containership">
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BehaviorID="ShipmentModelPopupExtender" TargetControlID="HyperLink1"
            PopupControlID="ShipmentPopupPanel" OkControlID="btnOk" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <asp:Panel ID="ShipmentPopupPanel" runat="server" CssClass="ModalPopupStyleship">
            <div class="containership">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;
                    text-align: left;">
                    <tr>
                        <td>
                          
                            <p class="TableColumnStyleship">
                                <b>Courier Service</b>
                                <br />
                                Australia wide flat rate delivery charge of $12.95*
                                <br />
                                Goods will be sent using Courier Service if under Gross /Cubic weight of 3Kg.                                 
                                <br />
                                Parcels over 3Kg will be sent by most economical means, eg Road, E-Parcel Service etc. 
                                <br />
                                Exemptions apply to heavy / large items (e.g Server Racks) will occur higher delivery charges our sales team will contact you prior to shipping. 
                                <br />
                                Note. Dangerous goods cannot be sent by air, road service only.
                            </p>
                          
                            <p class="TableColumnStyleship">
                                <b>Shop Counter Pick Up</b>
                                <br />
                                Place your order online and pick up goods from our Sydney Showroom.
                                <br />
                                Important Details For Store Pick Up:
                                <br />
                                1. Please bring a printed copy of invoice when coming into store.
                                <br />
                                2. Proof of Identity is required when picking up goods from our store. Please ensure you have the Credit Card you made the purchase with and your Driver’s 
                                <br />
                                License ID when coming into the store to pick up goods purchased online. The Name on the Credit Card Must Match Your Drivers License ID.
                                <br />
                                You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Drivers License are shown OR if there if 
                                <br />
                                there is a mismatch in details.
                            </p>
                          
                        </td>
                    </tr>
                </table>
                <div algn="center">
                    <asp:Button ID="btnOk" runat="server" Text="Close" CssClass="ButtonStyleship"  />
                </div>
            </div>
        </asp:Panel>
    </div>--%>
           <%--<fieldset>
            <legend>Comments / Notes</legend>
            <div class="form-col-8-8">
                        <asp:TextBox ID="TextBox1"  runat="server" Rows="5" Columns="30" Font-Size="12px" CssClass="textarea1" Width="100%"  Height="72px" Font-Names="arial"  MaxLength="240"  onkeyDown="return checkMaxLength(this,event,'240');" 
                                TextMode="MultiLine">
                            </asp:TextBox>
            </div>
        
    <table id="Table3" width="100%"  runat="server" cellpadding="0" cellspacing="0" border="0" >
        <tr>
      
            <td width="100%" align="left" >
                <table width="100%" runat="server" cellpadding="1" cellspacing="0" border="0" style="border-style: none" id="colo2">
                 
                    <tr>
                        <td rowspan="2" width="75%" >
                         
                        </td>
                    
                    </tr>
                    <tr>
                  
                    </tr>
                </table>
            </td>
        
        </tr>
    </table>
  
           </fieldset>--%>
         <%--  <fieldset>
             <legend>Your Order Contents</legend>--%>
         <div class="accordion_head_grey clear">Your Order Contents</div>
         <div class="col-lg-20 col-xs-20">
         <div class="table-responsive col-lg-20 col-sm-20">
        <%--   <table id="Table4" width="100%" runat="server" cellpadding="0" cellspacing="0" border="0" style="padding-left:5px" >
        <tr>
            <td >
              
    
                       
     </td>
    </tr>
    <tr>
       <td>--%>
           <asp:Panel ID="PnlOrderContents" Visible="true" runat="server" CssClass="form-col-8-8">
                    <%
                       HelperServices objHelperServices = new HelperServices();
                       
                        OrderServices objOrderServices = new OrderServices();
                        ProductServices objProductServices = new ProductServices();
                        //ProductFamily oProdFam = new ProductFamily();
                        DataSet dsOItem = new DataSet();

                        int OrderID = 0;
                        int Userid;
                        int ProductId;
                        decimal subtot = 0.00M;
                        decimal taxamt = 0.00M;
                        decimal Total = 0.00M;

                        string SelProductId = "";
                        string OrdStatus = "";

                        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                        UserServices objUserServices = new UserServices();

                        Userid = objHelperServices.CI(Session["USER_ID"]);
                        if (Userid <= 0)
                            Userid = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());
                        if (!string.IsNullOrEmpty(Request["OrderID"]))
                        {
                            OrderID = Convert.ToInt32(Request["OrderID"].ToString());
                        }
                        else
                        {
                            OrderID = objOrderServices.GetOrderID(Userid, OpenOrdStatusID);
                        }

                       

                        OrdStatus = objOrderServices.GetOrderStatus(OrderID);
                        ProductId = objHelperServices.CI(Request.QueryString["Pid"]);
                    %>
                    
                    <table class="table table-bordered"> 
                       <thead class="view_Cart_head_color">
                        <tr>
                            <th>
                                Order Code
                            </th>
                            <th>
                                Quantity
                            </th>
                           <%-- <td colspan="2" bgcolor="#D2E2F0" align="left" width="30%" >
                                <b>Description</b>
                            </td>--%>
                            <th>
                                Description
                            </th>
                          <%--  <td style="border-style: none solid solid none;  border-color: #BCD0E2;border: 1px solid #BCD0E2;padding: 0px 0 0 9px !important;"
                                bgcolor="#D2E2F0" align="left" width="20%">
                                <b>Cost (Ex. GST)</b>
                            </td>--%>
                             <th>
                                Cost (Ex. GST)
                            </th>
                           <%-- <td style="border-style: none none solid none;  border-color: #BCD0E2;border: 1px solid #BCD0E2;padding: 0px 0 0 9px !important;"
                                bgcolor="#D2E2F0" align="left" width="19%">
                                <b>Extension Amount (Ex. GST)</b>
                            </td>--%>
                             <th>
                                Extension Amount (Ex. GST)
                            </th>
                        </tr>
                        </thead>
                        <%   	                   	     
                            dsOItem = objOrderServices.GetOrderItems(OrderID);

                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);
                    

                            UserServices.UserInfo oOrdBillInfo1 = objUserServices.GetUserBillInfo(Userid );
                            UserServices.UserInfo oOrdShippInfo1 = objUserServices.GetUserShipInfo(Userid);
                            
                             
                            string cSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                            decimal ProdShippCost = 0.00M;
                            decimal TotalShipCost = 0.00M;

                            SelProductId = "";
                            if (OrdStatus == OrderServices.OrderStatus.OPEN.ToString() || OrdStatus == "CAU_PENDING")
                            {
                                if (dsOItem != null)
                                {
                                    int i = 0;
                                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                    {
                                        decimal ProductUnitPrice;
                                        int pid;
                                        int maxqty;
                                        int minQty;
                                        int FId = 0;
                                         double OrderItemId1 = 0;
                                        string sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                        string styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                        if (rItem["PRODUCT_ID"].ToString() == dsOItem.Tables[0].Rows[dsOItem.Tables[0].Rows.Count - 1]["PRODUCT_ID"].ToString())
                                        {
                                            sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                            styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                        }
                                        pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                        OrderItemId1 = objHelperServices.CD(rItem["ORDER_ITEM_ID"].ToString());
                                        
                                        FId = objProductServices.GetFamilyID(pid);
                                        int pQty = objOrderServices.GetOrderItemQty(pid, OrderID,OrderItemId1);

                                        maxqty = objHelperServices.CI(rItem["QTY_AVAIL"].ToString());
                                        maxqty = maxqty + objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                        minQty = objHelperServices.CI(rItem["MIN_ORD_QTY"].ToString());
                                        ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                        ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                                     
                                        int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                                        decimal ProdTotal = Qty * ProductUnitPrice;
                                        subtot = subtot + ProdTotal;
                                        string Desc = rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                                        string Available = rItem["PRODUCT_STATUS"].ToString();
                                     
                                        if (Request["SelAll"] != "1")
                                        {
                                            SelProductId = "";
                                            Session["SelProduct"] = null;
                                            CheckBox chk = new CheckBox();
                        %>
                        <tr>
                            <td>
                                <%Response.Write("<a class=\"toplinkatest\"  href =pd.aspx?&Pid=" + pid + "&fid=" + FId.ToString() + ">" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                            </td>
                            <td>
                                <%Response.Write("<input class=\"qty_input\" style=\"background-color:white;\" type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\" Name =\"txtQty" + i + "\" size=\"5\"  disabled=\"disabled\" runat =\"server\" onBlur=\"javascript:return Check(" + i + "," + maxqty + "," + minQty + "," + Qty + ");\" value =\"" + Qty + "\">"); %>
                              <%--  <%Response.Write(Qty);%>--%>
                            </td>
                            <td>
                                <%Response.Write(Desc);%>
                            </td>
                           
                            <td>
                                <%Response.Write(cSymbol + " " + ProductUnitPrice.ToString("#,#0.00"));%>
                            </td>
                            <td>
                                <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                            </td>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMinQty" + i + "\" runat=\"server\" value=\"" + minQty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                            <% Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                        </tr>
                        <%  
                               i = i + 1;
                                        }
                                        else if (Request["SelAll"] == "0")
                                        {
                                            SelProductId = "";
                                            Session["SelProduct"] = null;
                        %>
                        <tr>
                            <td 
                                bgcolor="White" align="left" class="style20">
                                <%Response.Write("<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid" + pid + "&Min=" + minQty + "&Max" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                            </td>
                            <td 
                                bgcolor="White" class="shippingstyle21" align="left">
                                <%Response.Write("<input type =\"Text\" class=\"txtboxship\" Id=\"txtQtyId" + i + "_" + maxqty + "\" Name =\"txtQty" + i + "\" size=\"7\" disabled=\"disabled\" runat =\"server\" onBlur=\"javascript:Check(" + i + "," + maxqty + ");\" value =\"" + Qty + "\">"); %>
                            </td>
                            <td 
                                colspan="2" bgcolor="White" class="shippingstyle21">
                                <%Response.Write(Desc); %>AN2
                            </td>
                          
                            <td 
                                bgcolor="White" class="shippingstyle22" align="left" style="width: 130px;text-align:left;">
                                <%Response.Write(cSymbol + " " + ProductUnitPrice.ToString("#,#0.00"));%>
                            </td>
                            <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                            --%>
                            <td 
                                bgcolor="White" class="shippingstyle23" align="left" width="20%">
                                <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                            </td>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtsPrdId" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                        </tr>
                        <%
                              i = i + 1;
                                        }
                                        else
                                        { 
                        %>
                        <tr>
                            <td 
                                bgcolor="White" align="left" class="">
                                <%Response.Write("<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid=" + pid + "&Min=" + minQty + "&Max=" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                            </td>
                            <td 
                                bgcolor="White" class="Numeric" align="left">
                                <%Response.Write("<input class=\"txtboxship\" type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\" Name =\"txtQty" + i + "\" size=\"7\"  disabled=\"disabled\" runat =\"server\" onBlur=\"javascript:Check(" + i + "," + maxqty + ");\" value =\"" + Qty + "\">"); %>
                            </td>
                            <td 
                                colspan="2" bgcolor="White" class="shippingstyle21">
                                <%Response.Write(Desc); %>AN1
                            </td>
                           
                            <td 
                                bgcolor="White" class="shippingstyle23" align="left" style="width: 130px;text-align:left;" >
                                <%Response.Write(cSymbol + " " + ProductUnitPrice.ToString("#,#0.00"));%>
                            </td>
                           
                            <td 
                                bgcolor="White" class="NumericField" align="left" style="text-align:left;">
                                <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                            </td>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtsPrdId" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                            <% Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                        </tr>
                        <%   
                               SelProductId = SelProductId + "," + pid;
                               i = i + 1;
                                        } //End of SelAll
                                    } //End of for each.
                                    dsOItem.Dispose();
                                }//End of dataset empty. 
                            } // End Of Order Status Check
                            if (SelProductId != "")
                            {
                                SelProductId = SelProductId.Substring(1, SelProductId.Length - 1);
                                Session["SelProduct"] = SelProductId;
                            }
                        %>
                        <!-- End Up Here-->
                      
                        <tr>
                            <td colspan="3" rowspan="4" bgcolor="white" valign="top" align="right">
                                <font color="red"></font>
                            </td>
                          
                            <td class="NumericField" colspan="1" bgcolor="white"  align="left" style="text-align:left;">
                                Sub Total
                            </td>
                            <td class="NumericField" bgcolor="white" align="left" style="text-align:left;">
                                <%
                                   
                                    Response.Write(cSymbol + " " + oOrderInfo.ProdTotalPrice);   
                                    %>
                            </td>
                        </tr>
                        <tr id="corierhandcrgeR" style="display:none;">
                   
                           <td class="NumericField" colspan="1" bgcolor="white"  align="left" style="text-align:left;">
                               Delivery (Ex GST)
                            </td>
                              <td class="NumericField" bgcolor="white" align="left" style="text-align:left;">
                                <%
                                    decimal ShippingValue = 0;
                                    if (objHelperServices.GetOptionValues("COURIER CHARGE") != "")
                                        ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
                                    Response.Write(cSymbol + " " + ShippingValue);   
                                    %>
                            </td>
                        </tr>
                      
                            <%
                           
                                if (objOrderServices.IsNativeCountry(OrderID) == 0)   
                            {
                                %>
                                 <tr>
                          
                            <td class="NumericField" colspan="1" style="height: 21px;text-align:left;" align="left">
                                Shipping Charge <br />
                                <span style="font-size: 4"></span>
                            </td>
                            <td class="NumericField" style="height: 21px;text-align:left;" align="left">
                                To Be Advised                                
                                          </td>
                                </tr>
                       

                                <%
                            }                                                                 
                                 %>
                        <tr>
                          
                            <td class="NumericField" colspan="1" style="height: 21px;text-align:left;" align="left">
                                Tax Amount(GST)<br />
                                <span style="font-size: 4"></span>
                            </td>
                            <td class="NumericField" style="height: 21px;text-align:left;" align="left">
                                <span style="display:none;" id="TaxAmount">
                                <%       
                                  
                                    decimal ICtaxamt = 0.00M;
                                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                        Response.Write(cSymbol + " " + ICtaxamt);
                                    else
                                    {
                                        decimal totamtanor = oOrderInfo.ProdTotalPrice;
                                        decimal taxa_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTaxa_nor = objHelperServices.CDEC(totamtanor * (taxa_nor / 100));
                                       
                                        RetTaxa_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTaxa_nor));
                                      
                                        Response.Write(cSymbol + " " + RetTaxa_nor);
                                    }
                                    
                                    %></span>

                                  <span style="display:none;" id="taxamtcoupickup">
                                   <%   decimal totamtTaxCP =  oOrderInfo.ProdTotalPrice;
                                        decimal tax_CP = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_CP = objHelperServices.CDEC(totamtTaxCP * (tax_CP / 100));
                                      
                                        RetTax_CP = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_CP));
                                        Response.Write(cSymbol + " " + RetTax_CP); 
                                    %>
                                  </span>

                                  <span style="display:none;" id="taxamtwithcouriercrge">
                                       <%   decimal totamtTax = ShippingValue + oOrderInfo.ProdTotalPrice;
                                        decimal tax_WC = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_WC = objHelperServices.CDEC(totamtTax * (tax_WC / 100));
                                      
                                        RetTax_WC = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_WC));
                                        Response.Write(cSymbol + " " + RetTax_WC); 
                                    %></span>
                               
                                 
                            </td>
                        </tr>
                        <tr>
                           
                            <td class="NumericFieldship" colspan="1" style="height: 21px;border-style: none solid none none; border-width: thin; border-color: #BCD0E2" align="left">
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
                                
                                <span style="font-size: 4">(Freight not included)</span>
                            </td>
                            <td class="NumericFieldship" style="height: 21px; border-color: #BCD0E2" align="left" >

                                <strong id="ICtotalamount" style="display:none;">
                                  <% Response.Write(cSymbol + " " + oOrderInfo.ProdTotalPrice);    
                                    %>
                                </strong>
                                <strong id="totalamt" style="display:none;">
                                    <%
                                        
                                        
                                        decimal totnor = oOrderInfo.ProdTotalPrice;
                                        decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
                                        RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
                                        
                                        decimal totamtnor = RetTax_nor + oOrderInfo.ProdTotalPrice;
                                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                            Response.Write(cSymbol + " " + oOrderInfo.ProdTotalPrice);    
                                        else
                                            Response.Write(cSymbol + " " + totamtnor); 
                                        %>
                                </strong>
                                   <strong id="totalcoupickup" style="display:none;">
                                    <%
                                        
                                        decimal totcp = oOrderInfo.ProdTotalPrice;
                                        decimal tax_tcp = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_tcp = objHelperServices.CDEC(totcp * (tax_CP / 100));
                                       
                                        RetTax_tcp = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_tcp));


                                        decimal totcoupu = RetTax_tcp + oOrderInfo.ProdTotalPrice;
                                        totcoupu = objHelperServices.CDEC(objHelperServices.FixDecPlace(totcoupu));
                                        Response.Write(cSymbol + " " + totcoupu);    
                                        %>
                                </strong>
                                    <strong id="totamtwithcouriercrge" style="display:none;">
                                    <%
                                     
                                        decimal tocouamt = 0.00M; tocouamt = ShippingValue + oOrderInfo.ProdTotalPrice;
                                        decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax = objHelperServices.CDEC(tocouamt * (tax / 100));
                                        tocouamt = RetTax + ShippingValue + oOrderInfo.ProdTotalPrice;
                                       tocouamt = objHelperServices.CDEC(objHelperServices.FixDecPlace(tocouamt));
                                        Response.Write(cSymbol + " " + tocouamt);    
                                        %>
                                </strong>
                            </td>
                        </tr>
                      
                    </table>
                    <div class="col-lg-20 text-right checkout_btn">
                     <asp:Button ID="ImageButton1" runat="server" Text="Edit/Update Order" OnClick="ImageButton1_Click" class="btn btn-primary"/>
                            <asp:Button runat="server" ID="ImageButton2" Text="Continue"  class="btn btn-primary"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />       
                    </div>
                     
                </asp:Panel>
           <asp:Panel ID="PnlOrderInvoice" runat="server" Visible="false">
                    <uc1:InvoiceOrder ID="InvoiceOrder2" runat="server" />
                </asp:Panel>
      <%-- </td>
    </tr>
    </table>--%>
        </div>
        </div>
          <%-- </fieldset>--%>
           <%--</form>--%>
      <%--</div>--%>
              <%--</div>--%>
            <%-- </div>--%>
         <%-- </td>
        </tr>
      
    </table>--%>
     <%
        if (1 == 2)
       { %>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillFName" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillLName" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillMName" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbilladd1" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbilladd2" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbilladd3" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillcity" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="drpBillState" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillzip" runat="server" MaxLength="20"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:DropDownList Visible="false" ID="drpBillCountry" runat="server" Width="230px"
        AutoPostBack="true" Class="DropdownlistSkin">
    </asp:DropDownList>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillphone" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:DropDownList Visible="false" ID="cmbProvider" runat="server" Width="150px">
        <asp:ListItem Text="UPS"></asp:ListItem>
        <asp:ListItem Text="DHL"></asp:ListItem>
        <asp:ListItem Text="FedEX"></asp:ListItem>
        <asp:ListItem Text="USPS"></asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList Visible="false" ID="cmbShipMethod" runat="server" Width="150px">
        <asp:ListItem Text="Ground" Value="Ground"></asp:ListItem>
        <asp:ListItem Text="SecondDay" Value="SecondDay"></asp:ListItem>
        <asp:ListItem Text="Overnight" Value="Overnight"></asp:ListItem>
    </asp:DropDownList>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSFName" Class="textSkin"
        Width="225px" runat="server" MaxLength="50"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSMName" Class="textSkin"
        Width="225px" runat="server" MaxLength="50"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSLName" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSAdd1" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSAdd2" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSAdd3" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSCity" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="drpShipState" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSZip" runat="server" MaxLength="20"
        Class="textSkin" Width="225px" OnTextChanged="txtSZip_TextChanged"></asp:TextBox>
    <asp:DropDownList Visible="false" ID="drpShipCountry" runat="server" Width="230px"
        AutoPostBack="true" Class="DropdownlistSkin">
    </asp:DropDownList>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSPhone" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    &nbsp;<table align="center" width="558">
        <!--Site Map-->
        <tr class="tablerow">
            <td class="StaticText" align="left">
                <b>
                    <asp:Label ID="lblCheck" runat="Server" meta:resourcekey="lblCheck"></asp:Label></b>
                <asp:Label ID="lblShoppingCart" runat="Server" meta:resourcekey="lblShoppingCart"></asp:Label>
                > <b>
                    <asp:Label ID="lblShip" runat="Server" meta:resourcekey="lblShip" ForeColor="Blue"></asp:Label></b>
                >
                <asp:Label ID="lblBill" runat="Server" meta:resourcekey="lblBill"></asp:Label>
                >
                <asp:Label ID="lblReviewOrder" runat="Server" meta:resourcekey="lblReviewOrder"></asp:Label>
                >
                <asp:Label ID="lblConfirm" runat="Server" meta:resourcekey="lblConfirm"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="tbNoItems">
            <td style="height: 21px">
                <asp:LinkButton ID="ShippingLink" runat="server" Class="ErrorLinkSkin" meta:resourcekey="lbllinkcart"
                    ForeColor="Blue" PostBackUrl="~/OrderDetails.aspx" />
            </td>
        </tr>
        <!--Shipping Details-->
        <tr>
            <td align="center">
                &nbsp;<asp:Label ID="LblStar" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"
                    Width="1px"></asp:Label>&nbsp;
                <asp:Label ID="lblRequired" runat="server" meta:resourcekey="lblRequired" Class="lblNormalSkin"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <!--- Billing Informations-->
                <table id="tblBasebill" width="560" class="BaseTblBorder" align="left" border="0"
                    cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="100%" background="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/17.gif" class="TableRowHead">
                            <asp:Label ID="BillingHeader" runat="Server" meta:resourcekey="lblBillingDetails"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="ChkbillingAdd" runat="server" meta:resourcekey="ChkbillingAdd"
                                Class="CheckBoxSkin" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkbillingAdd_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px;">
                            <asp:Label ID="lblBillFName" runat="server" meta:resourcekey="lblBillFName" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="ReqFName" ControlToValidate="txtbillFName" Class="vldRequiredSkin"
                                runat="server" meta:resourcekey="rfvFName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegFname" runat="server" ControlToValidate="txtbillFName"
                                meta:resourcekey="rgExname" ValidationExpression="[a-zA-z]+([ '-][a-zA-Z]+)*"
                                Class="vldRegExSkin" Display="static" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="lblBillLName" runat="server" meta:resourcekey="lblBillLName" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ControlToValidate="txtbillLName"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvLName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegLName" runat="server" ControlToValidate="txtbillLName"
                                meta:resourcekey="rgExname" ValidationExpression="[a-zA-z]+([ '-][a-zA-Z]+)*"
                                Class="vldRegExSkin" Display="static" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillAdd1" runat="server" meta:resourcekey="lblSAdd" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="txtbilladd1"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvAdd1" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                        </td>
                        <td style="width: 144px">
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                        </td>
                        <td style="width: 144px">
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label4" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillCity" runat="Server" meta:resourcekey="lblCity" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtbillcity"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvCity" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillState" runat="Server" meta:resourcekey="lblState" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label15" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillZip" runat="Server" meta:resourcekey="lblZip" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtbillzip"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvZip" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillCountry" runat="Server" meta:resourcekey="lblCountry" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label18" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillPhone" runat="Server" meta:resourcekey="lblPhone" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="txtbillphone"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvPhone" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="ChkBillDefaultaddr" runat="server" Class="CheckBoxSkin" meta:resourcekey="ChkBillingDefaultAddr"
                                AutoPostBack="True" OnCheckedChanged="ChkDefaultBillAdd_CheckedChanged" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table id="tblBase" width="560" class="BaseTblBorder" border="0px" cellpadding="3"
                    cellspacing="0">
                    <tr>
                        <td class="TableRowHead" background="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/17.gif">
                            <asp:Label ID="lblShippingDetails" runat="Server" meta:resourcekey="lblShippingDetails"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table id="TblInner" border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="3">
                                        <asp:CheckBox ID="ChkShippingAdd" runat="server" Class="CheckBoxSkin" meta:resourcekey="ChkShippingAdd"
                                            AutoPostBack="True" Checked="True" OnCheckedChanged="ChkShippingAdd_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblProvider" runat="server" meta:resourcekey="lblProvider" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblMethod" runat="server" meta:resourcekey="lblMethod" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label20" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblSFName" runat="server" meta:resourcekey="lblSFName" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSFName"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvFName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblSLName" runat="server" meta:resourcekey="lblSLName" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtSLName"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvLastName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblSAdd" runat="server" meta:resourcekey="lblSAdd" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtSAdd1"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvAdd1" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblCity" runat="Server" meta:resourcekey="lblCity" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtSCity"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvCity" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblState" runat="Server" meta:resourcekey="lblState" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label14" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblZip" runat="Server" meta:resourcekey="lblZip" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtSZip"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvZip" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                        <asp:Label ID="lblFDMsg" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblCountry" runat="Server" meta:resourcekey="lblCountry" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblPhone" runat="Server" meta:resourcekey="lblPhone" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtSPhone"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvPhone" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:CheckBox ID="ChkShipDefaultaddr" runat="server" Class="CheckBoxSkin" meta:resourcekey="ChkShippingDefaultAddr"
                                            AutoPostBack="True" OnCheckedChanged="ChkDefaultShipAdd_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!--Shipping Details end here-->
        <!--Proceded to Next Page-->
        <tr>
            <td align="right">
                <asp:Button ID="btnShipProceed" Class="btnNormalSkin" runat="server" meta:resourcekey="btnShipProceed"
                    OnClick="btnShipProceed_Click" ValidationGroup="ShipBillGroup" />&nbsp;<table border="0">
                        <tr>
                            <td class="tablerow" align="right" style="height: 26px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
        <!-- Proceed to NExt Page end up here-->
    </table>
    <% } %>

   <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none;
        visibility: hidden"></asp:Button>
    <div id="PopupOrderMsg" align="center" runat ="server">
        <asp:Panel ID="ModalPanel" runat="server" CssClass="PopUpDisplayStyleship">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyleship">
                       Account Activation Required before you can proceed to check out.
                        <br />
                        Please check your email for account activation link as this was emailed to you when you registered your account.
                        <br />
                        If you would like us to send you the Activation Email again. <a Href="ConfirmMessage.aspx?Result=REMAILACTIVATION" class="toplinkatest">Please Click Here</a>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
            
                <tr style="height: 10px">
                    <td width="35%" align="right">
                       <%-- <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="ButtonStyle" OnClick="btnForgotPassword_Click" />--%>
                    </td>
                    <td width="30%">
                         <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="button normalsiz btnblue" OnClick="btnForgotPassword_Click" />
                    </td>
                    <td width="35%" align="left">
                        <%--<asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="ButtonStyle" OnClick="btnClose_Click" />--%>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>   

      <asp:Button ID="btnHiddenpopuploginreg" runat="server" Style="display: none;
        visibility: hidden"></asp:Button>
    <div id="Hiddenpopuploginreg" align="center" runat ="server">
        <asp:Panel ID="HiddenpopuploginregPanel" runat="server" CssClass="PopUpDisplayStyleship">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyleship">
                       Continue to Checkout Please  Login Or Register
                        <br />
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
            
                <tr style="height: 10px">
                    <td width="35%" align="right">
                     
                    </td>
                    <td width="30%">
                         <asp:Button ID="loginregrdir" runat="server" Text="Login Or Register"
                            Width="205px"  CssClass="button normalsiz btnblue" OnClick="loginregrdir_Click" />
                    </td>
                    <td width="35%" align="left">
                        
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>  


      <asp:Button ID="btnitemerrors" runat="server" Style="display: none;
        visibility: hidden"></asp:Button>

    <div id="PopupMsg">
                    <asp:Panel ID="Panelerroritems" runat="server" Style="display: none">
                    <div style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: block; padding-right: 17px;" aria-hidden="false"  role="dialog" tabindex="-1" class="modal fade lgn-orderinfo in">
                    <div class="modal-dialog">
                        <div class="modal-content">
                         <div class="modal-header green_bg">
                            <h4 id="H2" class="text-center">
                            <img class="popsucess" alt="img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Order__info.png"/>Wagner Erroe Alert</h4>
                          </div>
                           <div class="modal-body">
                              <div class="col-lg-20 text-center col-sm-10 col-md-5">
                              <p> Please review and correct Order Clarifications / Errors before proceeding to Check
                                    Out!</p>
                              </div>
                              </div>
                               <div class="modal-footer clear border_top_none">
                            <asp:Button ID="btnCancelerroritems" runat="server" Text="Close"  CssClass="btn primary-btn-blue"   OnClick="btnCancelerroritems_Click" />
                          </div>
                        <%--<table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 15px">
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td width="5%">
                                    &nbsp;
                                </td>
                                <td width="80%" align="left">
                                    Please review and correct Order Clarifications / Errors before proceeding to Check
                                    Out!
                                </td>
                                <td width="10%" align="right">
                                    <asp:Button ID="btnCancelerroritems" runat="server" Text="Close" Width="55px" Font-Bold="true"
                                        ForeColor="#1589FF" OnClick="btnCancelerroritems_Click" />
                                </td>
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>--%>
                        </div>
                        </div>
                    </div>
                    </asp:Panel>
                </div>
  </div>
  </div>
</div>
<div class="panel panel-default">
<a aria-controls="collapseTwo" aria-expanded="false" href="#collapseTwo" data-parent="#accordion" data-toggle="collapse" class="collapsed checkoutpanel">
                <div id="headingTwo" role="tab" class="panel-heading">
                  <h4 class="panel-title">  Payment Options <span><img class="pull-right" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/select-downarrow.png"/></span></h4>
                </div>
  </a>
  <div aria-labelledby="headingTwo" role="tabpanel" class="panel-collapse collapse" id="collapseTwo" aria-expanded="true" style="">
                  <div class="panel-body">
                    <div class="accordion_head_grey clear">Payment Type</div>
                    <div class="col-lg-20">
                      <div class="form-group dblock">
                      <label class="checkbox-inline lspace0">                     
                        <a href="#">
                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/wagner-master.png"/> 
                        </a>
                        </label>
                        <a href="#">
                      <label class="checkbox-inline">
                      </label>
                      </a>
                       
                        <a href="#">
                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/wagner-ae.png"/> 
                        <label class="checkbox-inline ">
                        </label>
                        </a>
                       
                        <a href="#">
                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/wagner-visa.png"/> 
                        <label class="checkbox-inline">
                        </label>
                        </a>
                       
                        <a href="#">
                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/wagner-payp.png"/>
                         </a>
                        
                      <p class="blue_text margin_top15"><strong>Pay using your Credit Card or Paypal Account</strong></p>
                      <p class="margin_bott15">You will be redirected to paypal website to complete payment transaction</p>
                      <input type="submit" value="Pay Now" class="btn btn-primary">
                      
                      </div>
                    </div>
 </div>
 </div>
</div>

<div class="panel panel-default">
<a aria-controls="collapseThree" aria-expanded="false" href="#collapseThree" data-parent="#accordion" data-toggle="collapse" class="collapsed checkoutpanel">
                <div id="headingThree" role="tab" class="panel-heading">
                  <h4 class="panel-title">  Completed<span><img class="pull-right" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/select-downarrow.png"/></span></h4>
                </div>
                </a>
</div>
                </div>
                <div class="clearfix"></div>
</div>
</div>
</asp:Content>
