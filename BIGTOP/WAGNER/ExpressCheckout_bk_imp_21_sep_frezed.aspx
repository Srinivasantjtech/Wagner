﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="ExpressCheckout.aspx.cs"  Inherits="ExpressCheckout" EnableEventValidation="false" Culture="en-US" UICulture="en-US" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UC/OrderDetExp.ascx" TagName="OrderDet" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
       <input type="hidden" value="true" id="chk"/>
 <script type="text/javascript">
     function DRPshippment() {
         alert('Non-Standard Delivery Area. We will contact you to confirm costing');
     }
     
</script> 
    <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CssScriptsSDUrl"].ToString()%>css/express/main.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" rel="stylesheet" type="text/css" media="screen" />
    <script type="text/javascript" src="/scripts/jquery-1.11.1.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
    <script type="text/javascript" src="/scripts/bootstrap.min.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"></script>
 <%--   <script type="text/javascript"  src="http://code.jquery.com/jquery-migrate-1.0.0.js"></script>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="Server">
   
  
<asp:HiddenField ID="hidSourceID" runat="server" />
    <script type="text/javascript">

        function isAlphabetic() {
            var ValidChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890/\\';
            var sText = document.getElementById("ctl00_maincontent_ttOrder").value;
            var IsAlphabetic = true;
            var Char;
            var ErrMsg = 'Sorry, but the use of the following special characters is not allowed in the Order No field:' + '\n' + '! ` & ~ ^ * %  $ @ ’ ( “ ) ; [   ] { } ! = < >  | * , . -' + '\n' + 'Please update your order no so it longer has any of these restricted characters in it to continue order.';
            var err = document.getElementById("ctl00_MainContent_txterr");
            if (err != null) {
                err.innerHTML = '';
            }
            for (i = 0; i < sText.length; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    alert(ErrMsg);
                    document.getElementById("ctl00_maincontent_ttOrder").value = '';
                    document.forms[0].elements["<%=ttOrder.ClientID%>"].focus();
                    return false;
                }
            }
          //  alert(IsAlphabetic);
          //  if (IsAlphabetic == true) {
                checkponum();
          //  }

            return isAlphabetic;
        }

        function checkponum() {
            
            var data = document.getElementById("ctl00_maincontent_ttOrder").value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ExpressCheckout.aspx/GetData",
                //data: JSON.stringify({ "data": data }),
                data: "{'PO':'" + data + "'}",
                datatype: "json",
                async: false,
                success: function (response) {
                    //alert("C# method calling : " + response.d);
                    if (response.d == "1") {
                        alert("Please enter valid Purchase Order No , Purchase Order No  should be more than 1 digits");
                        document.getElementById("ctl00_maincontent_ttOrder").value = '';
                        document.forms[0].elements["<%=ttOrder.ClientID%>"].focus();
                        return false;
                    }
                    else if (response.d == "2") {
                        alert("Purchase Order No  already exists, please Re-enter Purchase Order No ");
                        document.getElementById("ctl00_maincontent_ttOrder").value = '';
                        document.forms[0].elements["<%=ttOrder.ClientID%>"].focus();
                        return false;
                    }
                    else {
                        return true;
                    }
                },
                error: function (err) {
                    //alert(err.responseText);
                    return false;
                }
            });
        }

        function checkorderid() {
            var msgCheck = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            //  if (document.forms[0].elements["<%=ttOrder.ClientID%>"].value.length == 0) {
            // alert('Enter Order No and then proceed');
            // document.forms[0].elements["<%=ttOrder.ClientID%>"].focus();
            // return false;
            // }
            //else

            alert(Page_ClientValidate);

            if (document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Please Select Shipping Method") {
                alert('Please Select Shipping Method');
                document.forms[0].elements["<%=drpSM1.ClientID%>"].focus();
                return false;
            }
         //   else {
          //      if (Page_ClientValidate())
          //          {
                    //return true;
          //          var validated = Page_ClientValidate('Mandatory_Express'); if (validated) { buttonClicked_WithObj(this); return true; } else { return false; };
          //  }
            //  }

            if (Page_ClientValidate()) {
                alert("1");
                return true;
            }
            else {
                alert("2");
                return false;
            }
       
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
        
            $("#<%=drpSM1.ClientID %>").change(function (event) {
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                    
                    ShowShopCounterPickupMessage();
                    showtotalamount();
                   
                    

                }
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Standard Shipping") {
                    //alert("Standard Shipping");
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

            CRGE();
        });


        function CRGE() {
            if (document.getElementById("ctl00_maincontent_lblShippingMethod") != null) {

          
                if (document.getElementById("ctl00_maincontent_lblShippingMethod").innerHTML == "Standard Shipping") {
                    document.getElementById("shipping_charge_domestic_value").style.display = 'block';
                    document.getElementById("shipping_charge_domestic_zero").style.display = 'none';
                }
                else {
                    document.getElementById("shipping_charge_domestic_value").style.display = 'none';
                    document.getElementById("shipping_charge_domestic_zero").style.display = 'block';
                }
            }
            else {
                document.getElementById("shipping_charge_domestic_value").style.display = 'none';
                document.getElementById("shipping_charge_domestic_zero").style.display = 'block';
            }
        }

        function showcourieramount() {
            //alert("inner");
            document.getElementById("totamtwithcouriercrge").style.display = 'block';

            document.getElementById("totalamt").style.display = 'none';

            document.getElementById("totalcoupickup").style.display = 'none';

            //document.getElementById("corierhandcrgeR").style.display = '';

            document.getElementById("taxamtwithcouriercrge").style.display = 'block';

            document.getElementById("taxamtcoupickup").style.display = 'none';

            document.getElementById("TaxAmount").style.display = 'none';

            document.getElementById("ICtotalamount").style.display = 'none';

          //  document.getElementById("shipping_charge_domestic_value").style.display = 'block';
            document.getElementById("shipping_charge_domestic_zero").style.display = 'none';

        }
        //function showcourieramount1() {
        //    //alert("inner");
        //    document.getElementById("totamtwithcouriercrge1").style.display = 'block';

        //    document.getElementById("totalamt1").style.display = 'none';

        //    document.getElementById("totalcoupickup1").style.display = 'none';

        //    //document.getElementById("corierhandcrgeR1").style.display = '';

        //    document.getElementById("taxamtwithcouriercrge1").style.display = 'block';

        //    document.getElementById("taxamtcoupickup1").style.display = 'none';

        //    document.getElementById("TaxAmount1").style.display = 'none';

        //    document.getElementById("ICtotalamount1").style.display = 'none';

        //}
        function intercustotalamt() {
            if (document.getElementById("totamtwithcouriercrge") != null) {
                document.getElementById("totamtwithcouriercrge").style.display = 'none';
            }

            if (document.getElementById("totalcoupickup") != null) {
                document.getElementById("totalcoupickup").style.display = 'none';
            }
            if (document.getElementById("totalamt") != null) {
                document.getElementById("totalamt").style.display = 'none';
            }

           // if (document.getElementById("corierhandcrgeR") != null) {
           //     document.getElementById("corierhandcrgeR").style.display = 'none';
          //  }

            if (document.getElementById("taxamtcoupickup") != null) {
                document.getElementById("taxamtcoupickup").style.display = 'none';
            }

            if (document.getElementById("taxamtwithcouriercrge") != null) {
                document.getElementById("taxamtwithcouriercrge").style.display = 'none';
            }

            if (document.getElementById("TaxAmount") != null) {
                document.getElementById("TaxAmount").style.display = 'block';
            }

            if (document.getElementById("ICtotalamount") != null) {
                document.getElementById("ICtotalamount").style.display = 'block';
            }

            document.getElementById("shipping_charge_domestic_value").style.display = 'none';
            document.getElementById("shipping_charge_domestic_zero").style.display = 'none';

        }
        //function intercustotalamt1() {
        //    if (document.getElementById("totamtwithcouriercrge1") != null) {
        //        document.getElementById("totamtwithcouriercrge1").style.display = 'none';
        //    }

        //    if (document.getElementById("totalcoupickup1") != null) {
        //        document.getElementById("totalcoupickup1").style.display = 'none';
        //    }
        //    if (document.getElementById("totalamt1") != null) {
        //        document.getElementById("totalamt1").style.display = 'none';
        //    }

        //   // if (document.getElementById("corierhandcrgeR1") != null) {
        //   //     document.getElementById("corierhandcrgeR1").style.display = 'none';
        //   // }

        //    if (document.getElementById("taxamtcoupickup1") != null) {
        //        document.getElementById("taxamtcoupickup1").style.display = 'none';
        //    }

        //    if (document.getElementById("taxamtwithcouriercrge1") != null) {
        //        document.getElementById("taxamtwithcouriercrge1").style.display = 'none';
        //    }

        //    if (document.getElementById("TaxAmount1") != null) {
        //        document.getElementById("TaxAmount1").style.display = 'block';
        //    }

        //    if (document.getElementById("ICtotalamount1") != null) {
        //        document.getElementById("ICtotalamount1").style.display = 'block';
        //    }

        //}
        function showtotalamount() {


            document.getElementById("shipping_charge_domestic_value").style.display = 'none';
            document.getElementById("shipping_charge_domestic_zero").style.display = 'block';


            document.getElementById("totamtwithcouriercrge").style.display = 'none';

            document.getElementById("totalcoupickup").style.display = 'none';

            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                //alert("123");
                document.getElementById("totalcoupickup").style.display = 'block';

            }
            else {
                document.getElementById("totalcoupickup").style.display = 'none';

            }
            document.getElementById("totalamt").style.display = 'none';

            //document.getElementById("corierhandcrgeR").style.display = 'none';

            document.getElementById("taxamtcoupickup").style.display = 'block';

            document.getElementById("taxamtwithcouriercrge").style.display = 'none';

            document.getElementById("TaxAmount").style.display = 'none';

            document.getElementById("ICtotalamount").style.display = 'none';

        }
        function showtotalamount1() {
            document.getElementById("totamtwithcouriercrge1").style.display = 'none';

            document.getElementById("totalcoupickup1").style.display = 'none';

            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                document.getElementById("totalcoupickup1").style.display = 'block';

            }
            else {
                document.getElementById("totalcoupickup1").style.display = 'none';

            }
            document.getElementById("totalamt1").style.display = 'none';

            //document.getElementById("corierhandcrgeR1").style.display = 'none';

            document.getElementById("taxamtcoupickup1").style.display = 'block';

            document.getElementById("taxamtwithcouriercrge1").style.display = 'none';

            document.getElementById("TaxAmount1").style.display = 'none';

            document.getElementById("ICtotalamount1").style.display = 'none';

        }
        function intltotalamount() {
            if (document.getElementById("totalamt") != null) {
                document.getElementById("totalamt").style.display = 'block';
            }
            if (document.getElementById("totamtwithcouriercrge") != null) {
                document.getElementById("totamtwithcouriercrge").style.display = 'none';
            }

            if (document.getElementById("totalcoupickup") != null) {
                document.getElementById("totalcoupickup").style.display = 'none';
            }

          //  if (document.getElementById("corierhandcrgeR") != null) {
          ///      document.getElementById("corierhandcrgeR").style.display = 'none';
           // }

            if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup") {
                document.getElementById("totalcoupickup").style.display = 'block';

            }
            else {
                if (document.getElementById("totalcoupickup") != null) {
                    document.getElementById("totalcoupickup").style.display = 'none';
                }

            }
            if (document.getElementById("taxamtcoupickup") != null) {
                document.getElementById("taxamtcoupickup").style.display = 'none';
            }

            if (document.getElementById("taxamtwithcouriercrge") != null) {
                document.getElementById("taxamtwithcouriercrge").style.display = 'none';
            }

            if (document.getElementById("TaxAmount") != null) {
                document.getElementById("TaxAmount").style.display = 'block';
            }

            if (document.getElementById("ICtotalamount") != null) {
                document.getElementById("ICtotalamount").style.display = 'none';
            }

            document.getElementById("shipping_charge_domestic_value").style.display = 'none';
            document.getElementById("shipping_charge_domestic_zero").style.display = 'none';

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
       
            document.getElementById("ctl00_maincontent_SCP").style.display = 'block';
            $("#ctl00_maincontent_SCP").removeClass("modal fade lgn-orderinfoscp");
            $("#ctl00_maincontent_SCP").addClass("modal fade lgn-orderinfoscp in");
        }
        function CloseSCP() {
            $("#ctl00_maincontent_SCP").removeClass("modal fade lgn-orderinfoscp in");
            $("#ctl00_maincontent_SCP").addClass("modal fade lgn-orderinfoscp");
            document.getElementById("ctl00_maincontent_SCP").style.display = 'none';
        }

        function ShowShipMethodExpress() {

            document.getElementById("ctl00_maincontent_ship_method_express").style.display = 'block';
            $("#ctl00_maincontent_ship_method_express").removeClass("modal fade lgn-orderinfoscp");
            $("#ctl00_maincontent_ship_method_express").addClass("modal fade lgn-orderinfoscp in");
        }
        function CloseShipMethodExpress() {
            $("#ctl00_maincontent_ship_method_express").removeClass("modal fade lgn-orderinfoscp in");
            $("#ctl00_maincontent_ship_method_express").addClass("modal fade lgn-orderinfoscp");
            document.getElementById("ctl00_maincontent_ship_method_express").style.display = 'none';
        }

        function MouseHover() {
            var x = "";<%--document.getElementById('<%= PopDiv.ClientID %>'); --%>
            if (x != null)
                x.style.display = "block";

        }

        function MouseOut() {
            var x = "";<%--document.getElementById('<%= PopDiv.ClientID %>'); --%>
            if (x != null)
                x.style.display = "none";

            $('.modal-backdrop').remove();
            $body.removeClass("modal-open");

            
        }


        function MouseOverCCpopClick() {
            // $find("PopDiv").show();
            var x = document.getElementById('<%= CCPopDiv.ClientID %>');
            //var x = document.getElementById("PopDiv");
            if (x != null)
                x.style.display = "block";

        }

        function MouseOutCCpopClick() {
            var x = document.getElementById('<%= CCPopDiv.ClientID %>');
            //var x = document.getElementById("PopDiv");
            if (x != null)
                x.style.display = "none";

            $body.removeClass("modal-open");
            // $find("ShipmentModelPopupExtender").hide();
        }
        function CheckShippment() {

            switch (document.getElementById("ctl00_maincontent_drpSM1").value) {
 
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
          //  document.getElementById("DropShipmentRow").style.display = '';
          

        }

        function ShowShipmentPanel() {
           
           // document.getElementById("DropShipmentRow").style.display = 'none';

        }

        function HidePanels() {
         
        }

        function ValidationDropShipOrder() {
            var isCompanyEmpty = false;
            var isStateEmpty = false;
            var isPostcodeEmpty = false;
            var isSuburbEmpty = false;
            var isadd1key = false;
            var isadd2key = false;
            var isPCkey = false;

                  if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_MainContent_txtAttentionTo").value == '') || (document.getElementById("ctl00_MainContent_txtAttentionTo").value == null) || (document.getElementById("ctl00_MainContent_txtAttentionTo").value == 'null'))) {
                document.getElementById("ctl00_MainContent_txtAttentionTo").style.borderColor = "red";
                document.getElementById("ctl00_MainContent_txtAttentionTo").focus();
                isCompanyEmpty = true;
            }
            else {
                document.getElementById("ctl00_MainContent_txtAttentionTo").style.borderColor = "ActiveBorder";
            }
            //                       


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_MainContent_drpState").value == 'Select Ship To State') || (document.getElementById("ctl00_MainContent_drpState").value == null) || (document.getElementById("ctl00_MainContent_drpState").value == 'null'))) {
                document.getElementById("ctl00_MainContent_drpState").style.borderColor = "red";
                document.getElementById("ctl00_MainContent_drpState").focus();
                isStateEmpty = true;
            }
            else {
                document.getElementById("ctl00_MainContent_drpState").style.borderColor = "ActiveBorder";
            }


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_MainContent_txtAddressLine1").value == '') || (document.getElementById("ctl00_MainContent_txtAddressLine1").value == null) || (document.getElementById("ctl00_MainContent_txtAddressLine1").value == 'null'))) {
                document.getElementById("ctl00_MainContent_txtAddressLine1").style.borderColor = "red";
                document.getElementById("ctl00_MainContent_txtAddressLine1").focus();
                isStateEmpty = true;
            }
            else {
                document.getElementById("ctl00_MainContent_txtAddressLine1").style.borderColor = "ActiveBorder";
                //PageMethods.GetDropShipmentKeyExists(document.getElementById("ctl00_MainContent_txtAddressLine1").value,"", OnSuccess1, OnFailure1)
            }


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_MainContent_txtPostCode").value == '') || (document.getElementById("ctl00_MainContent_txtPostCode").value == null) || (document.getElementById("ctl00_MainContent_txtPostCode").value == 'null'))) {
                document.getElementById("ctl00_MainContent_txtPostCode").style.borderColor = "red";
                document.getElementById("ctl00_MainContent_txtPostCode").focus();
                isPostcodeEmpty = true;
            }
            else {
                document.getElementById("ctl00_MainContent_txtPostCode").style.borderColor = "ActiveBorder";
                // PageMethods.GetDropShipmentKeyExists(document.getElementById("ctl00_MainContent_txtPostCode").value, "PostCode", OnSuccess1, OnFailure1)
            }

            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_MainContent_txtSuburb").value == '') || (document.getElementById("ctl00_MainContent_txtSuburb").value == null) || (document.getElementById("ctl00_MainContent_txtSuburb").value == 'null'))) {
                document.getElementById("ctl00_MainContent_txtSuburb").style.borderColor = "red";
                document.getElementById("ctl00_MainContent_txtSuburb").focus();
                isSuburbEmpty = true;
            }
            else {
                document.getElementById("ctl00_MainContent_txtSuburb").style.borderColor = "ActiveBorder";
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
         function Setinit(SourceID) {
             var x = document.getElementById('<%= btnPay.ClientID %>');
             // var x1 = document.getElementById('<%= btnPayApi.ClientID %>');
             var y = document.getElementById('<%= BtnProgress.ClientID %>');
             var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');

             if (x != null) {
                 x.style.display = "none";
             }
             //        if (x1 != null) {
             //            x1.style.display = "none";
             //        }


             y.style.display = "block";
             y.style.visibility = "visible";
             z.style.display = "block";
             z.style.visibility = "visible";
             var hidSourceID = document.getElementById("<%=hidSourceID.ClientID%>");
             hidSourceID.value = SourceID;

         }


         function SetinitSP() {
             var res = Page_ClientValidate();
             if (res == true) {
            
             }
             else {
                 // Controlvalidate('dd');
                 Controlvalidate('cno');
                 Controlvalidate('cn');
                 Controlvalidate('cvv');
             }

         }
    </script>
<%--  <script language="JavaScript" type="text/javascript">
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
</script>--%>

<script language="JavaScript" type="text/javascript">
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
<script language="JavaScript" type="text/javascript">
    function OnclickTab(tab) {
        var sp;
        var hp;
        var dp;
        if (tab == "Pay") {
            sp = document.getElementById('<%=spanPay.ClientID %>');
            hp = document.getElementById('<%=h3Pay.ClientID %>');
            dp = document.getElementById('<%=divPay.ClientID %>');

        }
        if (tab == "paid") {
            sp = document.getElementById('<%=spanpaid.ClientID %>');
            hp = document.getElementById('<%=hpaid.ClientID %>');
            dp = document.getElementById('<%=divpaid.ClientID %>');

        }
        if (tab == "ship") {
          
            dp = document.getElementById('<%=divship.ClientID%>');

        }
        var cs = sp.getAttribute("class");
        if (cs == "collapsed") {
            sp.setAttribute("class", "")

            dp.setAttribute("class", "panel-collapse collapse in");
            //  hp.setAttribute("class", "");

            dp.setAttribute("style", "display:block;");
        }
        else {
            sp.setAttribute("class", "collapsed")

            dp.setAttribute("class", "panel-collapse collapse");
            //  hp.setAttribute("class", "check ui-accordion-header ui-state-active ui-accordion-header-active");
            dp.setAttribute("style", "display:none;");
        }
    }
</script>
 
   <script type="text/ecmascript">
      
       function mod10_check(val) {
           var nondigits = new RegExp(/[^0-9]+/g);
           var number = val.replace(nondigits, '');
           var pos, digit, i, sub_total, sum = 0;
           var strlen = number.length;
           if (strlen < 13) { return false; }
           for (i = 0; i < strlen; i++) {
               pos = strlen - i;
               digit = parseInt(number.substring(pos - 1, pos));
               if (i % 2 == 1) {
                   sub_total = digit * 2;
                   if (sub_total > 9) {
                       sub_total = 1 + (sub_total - 10);
                   }
               } else {
                   sub_total = digit;
               }
               sum += sub_total;
           }
           if (sum > 0 && sum % 10 == 0) {
               return true;
           }
           return false;
       }
</script>
   <script language="javascript" type="text/javascript">
       function Numbersonlypay(e) {
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
           //keychar = String.fromCharCode(keynum)
           keychar = keynum
           //List of special characters you want to restrict
           // if (keychar == "1" || keychar == "2" || keychar == "3" || keychar == "4" || keychar == "5" || keychar == "6" || keychar == "7" || keychar == "8" || keychar == "9" || keychar == "0") {
           if (keychar == "48" || keychar == "49" || keychar == "50" || keychar == "51" || keychar == "52" || keychar == "53" || keychar == "54" || keychar == "55" || keychar == "56" || keychar == "57" || keychar == "8") {

               return true;
           }
           else {
               return false;
           }
       }
</script>
<%--<script type="text/javascript">
    function chkPB() {

        var optc = document.getElementById('<%= RBPersonal.ClientID %>')
        if (optc != null) {

            var cm = document.getElementById("tblbus");
            if (optc.checked == true) {
                if (cm != null) cm.style.display = "none";
                document.getElementById('<%= RBBusiness.ClientID %>').checked = false;
                 document.getElementById('<%= RBPersonal.ClientID %>').checked = true;

             }
             else {
                 if (cm != null) cm.style.display = "block";
                 document.getElementById('<%= RBBusiness.ClientID %>').checked = true;
                 document.getElementById('<%= RBPersonal.ClientID %>').checked = false;
             }
         }
     }
     window.onload = chkPB();
  

</script>--%>
    <%--<script language="javascript" type="text/javascript">
        //this code handles the F5/Ctrl+F5/Ctrl+R
        document.onkeydown = checkKeycode
        function checkKeycode(e) {
            var keycode;
            if (window.event)
                keycode = window.event.keyCode;
            else if (e)
                keycode = e.which;

            // Mozilla firefox
            if ($.browser.mozilla) {
                if (keycode == 116 || (e.ctrlKey && keycode == 82)) {
                    if (e.preventDefault) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                }
            }
                // IE
            else if ($.browser.msie) {
                if (keycode == 116 || (window.event.ctrlKey && keycode == 82)) {
                    window.event.returnValue = false;
                    window.event.keyCode = 0;
                    window.status = "Refresh is disabled";
                }
            }
        }
</script>--%>
        <style>
.p20 { padding:10px 22px;}
.close {
    font-size: 26px;
    opacity: .5;
}

.checkoutleft .accordion_head_green { padding:7px 12px !important;}
.smp_btn { background:none; border:none; color:#0072bb; padding-top:4px;}
.smp_btn:hover { text-decoration:underline;}
</style>
<div class="container">
    

<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">Checkout</li>
            </ul>
        </div>
    </div>

      <asp:PlaceHolder runat="server" ID="PHOrderConfirm" Visible="false" EnableViewState ="false">
                <% 
                    HelperServices objHelperServices = new HelperServices();

                    OrderServices objOrderServices = new OrderServices();
         
                    if (Convert.ToInt16(Session["USER_ROLE"]) == 3)
                    {
                        
                        
                %>
                    <div class="accordion_head_green white_color">                          
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
                 <% 
                     
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                    {

                        Session["USER_ID"] = "";
                        Session["DUMMY_FLAG"] = "0";
                        Session["ORDER_ID"] = "0";
                        Session["USER_ROLE"] = "0";  
                             %>
                  		<%--<div class="accordion_head_green white_color">
                             <img src="images/checkout_tick.png" class="margin_right" alt=""/>
                       Your order has been successfully submitted to us for processing. Thank You!
                         
                   </div>--%>
               
                	<div class="accordion_head_green white_color clearfix">
                    
                    	<div class="col-xs-1 nopadd"><img src="images/checkout_tick.png" class="margin_right" alt=""/> </div>
                       <div class="col-xs-19">Your order has been successfully submitted to us for processing. Thank You!</div>
                         
                   </div>
                 
                   <%} %>
                   <% 
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                      {
                      %>
                       
                  <%-- <div class="accordion_head_yellow gray_40" style="background: #FFF200; padding: 12px 17px;" ><strong>Please Note :</strong> As you are purchasing from outside of Australia, We will calculate shipping charges and advise you shortly by email with further details.
                   </div>--%>

                    <div class="accordion_head_yellow gray_40" style="background: #FFF200; padding: 12px 17px;" >
                   		<strong>Please Note :</strong> 
                    	As you are purchasing from outside of Australia, We will calculate shipping charges and advise you shortly by email with further details.
                   </div>

                   <%} %>
                   
                    
                <% 
                       
                    } 
                %>
    </asp:PlaceHolder> 
<div runat="server" id="PayOkDiv">
               
                  <div id="divOk" runat="server"  class="accordion_head_green clear" style="text-align:center">
                     </div>
                       
                </div>

      <div class="row">
    	<div class="col-md-12">
        	<div class="titleblock text_black mb10">
            	<h4 class="main_heading">Order Checkout</h4>
            </div>
        </div>
    </div>

<div class="row">
<div class="" id="accordion" role="tablist" aria-multiselectable="true">
            
<div class="">
 


 <div class="row clearfix">
    		<div class="col-md-9 col-md-push-11 ml-15">
                <div class="checkoutright ml20 mb30" id="checkoutrightL1" runat="server">
                    <div class="brb">
                    	<div class="pv5 ph10" style="background:#f0f0f0;">
                    		<h4 class="heading_2 inlineblk">Order Sumary</h4>
                        	<%--<a href="/orderdetails.aspx" class="pull-right mt5" id="checkoutrightL1_EditCart">Edit Cart</a>--%>
                            <asp:Button ID="ImageButton1" runat="server" Text="Edit Cart"  OnClick="ImageButton1_Click" class="smp_btn pull-right"/>
                        </div>
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
                        <%   	  
                            String sessionId_dum;
                            // string sessionuserid_dum;
                            sessionId_dum = Session.SessionID;
                                             	     
                            //dsOItem = objOrderServices.GetOrderItems(OrderID);


                            if (Userid != 999)
                            {
                                //int ordID = 0;
                                //if (Session["ORDER_ID"] != "" && Session["ORDER_ID"] != null)
                                //{
                                //    ordID = Convert.ToInt32(Session["ORDER_ID"].ToString());
                                //    dsOItem = objOrderServices.GetOrderItems(ordID);
                                //}
                                //else
                                //{
                                    dsOItem = objOrderServices.GetOrderItems(OrderID);
                                //}
                            }
                            else
                            {
                                dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, "", sessionId_dum);
                            }

                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);

                            //objOrderServices.UpdateOrderPrice_ExpressCheckout(oOrderInfo, true, sessionId_dum);
                            
                            UserServices.UserInfo oOrdBillInfo1 = objUserServices.GetUserBillInfo(Userid);
                            UserServices.UserInfo oOrdShippInfo1 = objUserServices.GetUserShipInfo(Userid);


                            string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                            decimal ProdShippCost = 0.00M;
                            decimal TotalShipCost = 0.00M;
                            decimal ShippingValue = 0;
                            string catlogitem = "";
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
                                        decimal Amt = 0;

                                        pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                        catlogitem = rItem["CATALOG_ITEM_NO"].ToString();
                                        //ProductUnitPrice = oHelper.CDEC(oProd.GetProductBasePrice(pid));
                                        ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                        ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                                        Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice);

                                        int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                                        decimal ProdTotal = Qty * ProductUnitPrice;
                                        subtot = subtot + ProdTotal;
                                      
                        %>
                        <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-3">
                            	<%--<img class="img_h50" src="images/BANANA-SOCKETBIND.jpg">--%>
                                <% GetImage(pid); %>

                                <asp:Image ID="lblProImage" class="img_h50" Style="max-width: 60px;" runat="server" alt="Loading..."  />
                            </div>
                            <div class="col-xs-17 pr0 ">
                            	<div class="dblock pb10"><b class="f14"><% Response.Write(rItem["DESCRIPTION"].ToString()); %></b></div>
                                <div class="os_itemdetail">
                                	<div class="col-xs-6 pl0"> <span class="dblock pb5">Qty:</span> <% Response.Write(rItem["QTY"].ToString()); %> </div>
                                    <div class="col-xs-7"> <span class="dblock pb5">Item Cost:</span><% Response.Write(CurSymbol + " " + ProductUnitPrice.ToString("#,#0.00")); %> Ex GST</div>
                                    <div class="col-xs-7 text-right"> <span class="dblock pb5">Sub-Total:</span> <% Response.Write(CurSymbol + " " + Amt.ToString("#,#0.00")); %> Ex GST</div>
                                </div>
                            </div>
                        </div>

                         <%  
                               i = i + 1;
                                       
                                        
                                    } 
                                  
                                }  
                            } 
                          
                        %>
                      <%  if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                          { %>
                        <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-16">
                                <p class="mb0 mt10">
                                    Shipping Charge
                                    </p>
                                </div>
                            </div>
                        <%}
                          else if ((drpSM1.SelectedValue == "Standard Shipping") ||( lblShippingMethod.Text == "Standard Shipping"))%>
                      
<%--                          <% if (lblshipcost.Text != "" && lblshipcost.Text != "0")
                                          {  %>--%>
                        <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-16">
                            <%--	<b>Delivery</b>--%>
                        		<p class="mb0 mt10">
                                    <%--Click and Collect - Customer pick form store--%>
                                      <%  
                                          if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                          {
                                              Response.Write("Shipping Charge");
                                          }
                                          else
                                          {
                                              Response.Write("Delivery (Ex GST)");
                                          }
                                 %>
                        		</p>
                            </div>
                            <div class="col-xs-4 text-right ital">
                            	<%--Free--%>
                                        <%
                                        
                                            if (objHelperServices.GetOptionValues("COURIER CHARGE") != "" && (drpSM1.SelectedValue == "Standard Shipping" || lblShippingMethod.Text == "Standard Shipping"))
                                            {
                                                ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
                                            }
                                            else
                                            {
                                                ShippingValue = 0;
                                            
                                            }
                                                //Response.Write(CurSymbol + " " + ShippingValue);      
                            
                        %>

                                <%
                                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                    {

                                        Response.Write("To Be Advised");
                                        } 
                                    else
                                    {


                                        //if (lblShippingMethod.Text == "Counter Pickup")
                                        //    Response.Write(CurSymbol + " " + "0.00");
                                        //else
                                        //    Response.Write(CurSymbol + " " + ShippingValue); 
                            
                        %>
                                   <span style="display:block;" id="shipping_charge_domestic_value">
                                <%  
                                 
                                        Response.Write(CurSymbol + " " + Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString()));   
                                        //Response.Write(CurSymbol + " " + oOrderInfo.ShipCost);
                                }   %>
                                  </span>
                                 <span style="display:none;" id="shipping_charge_domestic_zero">
                                     
                                      
                                 
                                           
                                       <% if (lblshipcost.Text != "")
                                          {  %>
                                      $ <asp:Label ID="lblshipcost" runat="server" Text=""></asp:Label>
                                       <% 
                                              //oOrderInfo.ShipCost = Convert.ToDecimal( lblshipcost.Text);
                                              //ShippingValue=Convert.ToDecimal( lblshipcost.Text);
                                              ShippingValue =0;
                                          }   %>
                                 </span>
                            </div>
                        </div>
                        
                      <%--  <% } %>--%>
                        <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-16">
                            	Total Tax Amount (GST) 
                        		<p class="mb0 mt10"><%
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                        {                                        
                            %>                                        
                              <b>Est. Total </b> 
                            <%
                        }
                        else
                        {
                                %> <b>Est. Total Inc GST</b>
                       <%
                        } %> </p>
                            </div>
                            <div class="col-xs-4 text-right">
                            	
                             
                                  <span style="display:block;" id="TaxAmount">
                                       
                                <%       
                                  
                                    decimal ICtaxamt = 0.00M;
                                    if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                    //if (drpCountry.SelectedValue != "AU")
                                    {
                                        Response.Write(CurSymbol + " " + ICtaxamt);
                                    }
                                    else
                                    {
                                       // decimal totamtanor = oOrderInfo.ProdTotalPrice;
                                        decimal totamtanor = subtot + ShippingValue;
                                        decimal taxa_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTaxa_nor = objHelperServices.CDEC((totamtanor) * (taxa_nor / 100));
                                        RetTaxa_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTaxa_nor));
                                        
                                        Response.Write(CurSymbol + " " + RetTaxa_nor);
                                    }
                                    
                                    %></span>

                                  <span style="display:none;" id="taxamtcoupickup">
                                   <%  // decimal totamtTaxCP =  oOrderInfo.ProdTotalPrice;
                                       decimal totamtTaxCP = subtot;
                                        decimal tax_CP = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_CP = objHelperServices.CDEC(totamtTaxCP * (tax_CP / 100));
                                        RetTax_CP = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_CP));
                                        Response.Write(CurSymbol + " " + RetTax_CP); 
                                    %>
                                  </span>
                                    <span style="display:none;" id="taxamtwithcouriercrge">
                                       <%  
                                           //decimal totamtTax = ShippingValue + oOrderInfo.ProdTotalPrice;
                                           decimal totamtTax = ShippingValue + subtot;
                                        decimal tax_WC = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_WC = objHelperServices.CDEC(totamtTax * (tax_WC / 100));
                                        RetTax_WC = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_WC));
                                        Response.Write(CurSymbol + " " + RetTax_WC); 
                                    %></span>
                            	
                            	<p class="mb0 mt10">
                                    <strong id="ICtotalamount" style="display:none;">

                                       
                                      
                                  <% 
                                      Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);  
                                     // Response.Write(CurSymbol + " " + subtot);   
                                    %>
                                </strong>
                                <strong id="totalamt" style="display:block;">
                                    <b>
                                    <%
                                        
                                        
                                        //decimal totnor = oOrderInfo.ProdTotalPrice;
                                        decimal totnor = subtot+ShippingValue;
                                        decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
                                        RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
                                        
                                        //decimal totamtnor = RetTax_nor + oOrderInfo.ProdTotalPrice;
                                        decimal totamtnor = RetTax_nor + totnor;
                                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                        {
                                            //Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);
                                            Response.Write(CurSymbol + " " + subtot);
                                        }
                                        else
                                        {
                                            Response.Write(CurSymbol + " " + totamtnor);
                                        }
                                        %>
                                        </b>
                                </strong>
                                   <strong id="totalcoupickup" style="display:none;">
                                    <%
                                        
                                        //decimal totcp = oOrderInfo.ProdTotalPrice;
                                        decimal totcp = subtot;
                                        decimal tax_tcp = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_tcp = objHelperServices.CDEC(totcp * (tax_CP / 100));
                                        // totamtTax = RetTax_WC + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        RetTax_tcp = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_tcp));


                                       // decimal totcoupu = RetTax_tcp + oOrderInfo.ProdTotalPrice;
                                        decimal totcoupu = RetTax_tcp + subtot;
                                        totcoupu = objHelperServices.CDEC(objHelperServices.FixDecPlace(totcoupu));
                                        Response.Write(CurSymbol + " " + totcoupu);    
                                        %>
                                </strong>
                                    <strong id="totamtwithcouriercrge" style="display:none;">

                                    <%
                                        
                                        decimal tocouamt = 0.00M; 
                                        //tocouamt = ShippingValue + oOrderInfo.ProdTotalPrice;
                                        tocouamt = ShippingValue + subtot;
                                        decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax = objHelperServices.CDEC(tocouamt * (tax / 100));
                                       // tocouamt = RetTax + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        tocouamt = RetTax + ShippingValue + subtot;
                                       tocouamt = objHelperServices.CDEC(objHelperServices.FixDecPlace(tocouamt));
                                       if (lblAmount.Text != "")
                                       {
                                           Response.Write(CurSymbol + " " + lblAmount.Text);
                                       }
                                       else
                                       {
                                           Response.Write(CurSymbol + " " + tocouamt);
                                       }   
                                        %>
                                </strong>

                            	</p>
                            </div>
                        </div>
                        
                    </div>

                </div>
                <div class="checkoutright ml20 mb30" runat="server" id="checkoutrightL4">
                      <uc1:OrderDet ID="OrderDet1" runat="server" />
                </div>
            </div>
        	<div class="col-md-11 col-md-pull-9">
	          <div class="wrapleft pr40 mpr0 pb20" runat="server" id="Level1">  
            	<div class="headingwrap active clearfix">
                    <div class="no_circle">1</div>
                    <h4 class="numaric_heading inlineblk">Lets Get Started</h4>
                </div>
                <div class="checkoutleft" id="letstartdivlogin" runat="server">
                      <div id="getstartwelcome" runat="server">
                	<p class="caption mb20 mt10">Welcome Back! Please sign in to continue check out.</p>
                       </div>
                    <form>
                        <div class="form-group mb10 clearfix">
                            <label class="col-md-5 pl0">Email<span class="required">*</span></label>
                            <%--<a href="#" onclick="ShowShipMethodExpress();">test</a>--%>
                            <div class="col-md-15 mpl0">
                           <%-- <input class="form-control" value="nathat@wes.net.au" type="text">--%>
                              <asp:TextBox runat="server" ID="letstarttxtemail" Text="" class="form-control"  MaxLength="55" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                          
                                <asp:HiddenField ID="rfletstarttxtemail" runat="server" />
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Class="mandatory"   ErrorMessage="Required" ValidationGroup="Mandatory_Express1" Display="Dynamic" Text="Enter Email" ControlToValidate="letstarttxtemail" ForeColor="Red"></asp:RequiredFieldValidator>
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="letstarttxtemail"   ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory_Express1" ForeColor="Red"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="form-group mb30 clearfix" id="startdivpwd" runat="server">
                            <label class="col-md-5 pl0">Password<span class="required">*</span></label>
                            <div class="col-md-15 mpl0">
                           <%-- <input class="form-control" type="text">--%>
                                <asp:TextBox runat="server" ID="letstarttxtpwd" Text="" class="form-control" TextMode="Password"  MaxLength="55" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="mandatory"   ErrorMessage="Required" ValidationGroup="Mandatory_Express2" Display="Dynamic" Text="Enter Password" ControlToValidate="letstarttxtpwd" ForeColor="Red"></asp:RequiredFieldValidator>
                            <a class="fplink pull-right" target="_blank" href="ForgotPassword.aspx">Forgot Password</a>
                                <asp:Label ID="lblErrMsg" runat="server"  Text="" Class="mandatory" ></asp:Label>
                            </div>
                        </div>
                        
                        <p class="text-center">
                        <%--	<button class="ph30 btn primary-btn-green f16">Sign In </button>--%>
                            <asp:Button runat="server" ID="BtnGetStartContinue" Text="Continue" class="ph30 btn primary-btn-green f16"  OnClick="BtnGetStartContinue_Click"  ValidationGroup="Mandatory_Express1" UseSubmitBehavior="true"  />   
                            <asp:Button runat="server" ID="BtnGetStartLogin" Text="Sign In" class="ph30 btn primary-btn-green f16"  OnClick="BtnGetStartLogin_Click"   ValidationGroup="Mandatory_Express2" UseSubmitBehavior="true"  />   
                        </p>
                        
                    </form>
                </div>
                <div class="checkoutleft" id="letstartregister" runat="server">
                	<p class="caption mb20">Please enter details below to continue check out</p>
                    <form>
                        <div class="form-group mb10 clearfix">
                            <label class="col-md-7 pl0">Email<span class="required">*</span></label>
                            <div class="col-md-13 mpl0">
                           <%-- <input class="form-control" type="text">--%>
                                <asp:TextBox runat="server" ID="txtregemail" Text="" class="form-control"  MaxLength="55" onkeyup="javascript:keyboardup(this);"  OnTextChanged="BtnGetStartContinue_Click" AutoPostBack="true"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Class="mandatory"   ErrorMessage="Required" ValidationGroup="Mandatory_Express31" Display="Dynamic" Text="Enter Email" ControlToValidate="txtregemail" ForeColor="Red"></asp:RequiredFieldValidator>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtregemail"   ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory_Express31" ForeColor="Red"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        
                        <hr class="mv15">
                        
                        <div class="form-group mb10 clearfix">
                            <label class="col-md-7 pl0">First Name<span class="required">*</span></label>
                            <div class="col-md-13 mpl0">
                            <%--<input class="form-control" type="text">--%>
                                  <asp:TextBox runat="server" ID="txtRegFname" Text="" class="form-control" MaxLength="50"  onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Class="mandatory"  ErrorMessage="Required" ValidationGroup="Mandatory_Express31" Display="Dynamic" Text="Enter First Name"  ControlToValidate="txtRegFname"></asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class="form-group mb10 clearfix">
                            <label class="col-md-7 pl0">Last Name<span class="required">*</span></label>
                            <div class="col-md-13 mpl0">
                           <%-- <input class="form-control" type="text">--%>
                                 <asp:TextBox runat="server" ID="txtRegLname" Text="" class="form-control" MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Class="mandatory"  ErrorMessage="Required" ValidationGroup="Mandatory_Express31" Display="Dynamic" Text="Enter Last Name"   ControlToValidate="txtRegLname"></asp:RequiredFieldValidator> 
                            </div>
                        </div>
                        <div class="form-group mb30 clearfix">
                            <label class="col-md-7 pl0">Phone Number<span class="required">*</span></label>
                            <div class="col-md-13 mpl0">
                           <%-- <input class="form-control" type="text">--%>
                             <asp:TextBox runat="server" ID="txtRegphone" Text="" class="form-control" MaxLength="16" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Class="mandatory"  ErrorMessage="Required" ValidationGroup="Mandatory_Express31" Display="Dynamic" Text="Enter Phone"  ControlToValidate="txtRegphone"></asp:RequiredFieldValidator>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtRegphone" />
                            </div>
                        </div>
                        
                        <hr class="mv15">
                        
                        <div class="form-group mb30 clearfix">
                            <label class="col-md-7 pl0">Password<span class="required">*</span></label>
                            <div class="col-md-13 mpl0">
                            <%--<input class="form-control" type="text">--%>
                                 <asp:TextBox runat="server" ID="txtRegpassword" Text="" class="form-control"  TextMode="Password"  MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator4" CssClass="mandatory" Display="Dynamic"  ControlToValidate="txtRegpassword"  ValidationGroup="Mandatory_Express31"  ValidationExpression="^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"  runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory_Express31" Display="Dynamic" Text="Enter Password" ControlToValidate="txtRegpassword"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        
                        <div class="form-group mb30 clearfix">
                            <label class="col-md-7 pl0">Confirm Password<span class="required">*</span></label>
                            <div class="col-md-13 mpl0">
                            <%--<input class="form-control" type="text">--%>
                                  <asp:TextBox runat="server" ID="txtRegConfirmPassword" Text="" class="form-control" TextMode="Password" 
                                    MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                                   <asp:CompareValidator ID="CompareValidator3"  Class="mandatory" ControlToValidate="txtRegConfirmPassword" ControlToCompare="txtRegpassword"  runat="server" ErrorMessage="Confirm Password and Password should be same" ValidationGroup="Mandatory_Express31"  Display="Dynamic"></asp:CompareValidator>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory_Express31" Display="Dynamic" Text="Enter Confirm Password" ControlToValidate="txtRegConfirmPassword"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                         <asp:Label ID="lblerror_l1update" runat="server"  Text="" Class="mandatory" ></asp:Label>
                        <p class="text-center">
                    <%--    	<button class="ph20 btn primary-btn-green">Continue </button>--%>
                            <asp:Button runat="server" ID="BtnRegLetStart" OnClick="BtnRegLetStart_Click" Text="Continue" class="ph20 btn primary-btn-green"    ValidationGroup="Mandatory_Express31" UseSubmitBehavior="true"  Visible="true" />   
                              <asp:Button runat="server" ID="BtnRegLetStart_Edit" OnClick="BtnRegLetStartUpdate_Click" Text="Continue" class="ph20 btn primary-btn-green"    ValidationGroup="Mandatory_Express31" UseSubmitBehavior="true"  Visible ="false" />   
                        </p>
                        
                    </form>
                </div>
                   <div class="headingwrap clearfix mt20">
                    <div class="no_circle">2</div>
                    <h4 class="numaric_heading inlineblk">Shipping Address</h4>
                </div>
                
              <div class="headingwrap clearfix mt20">
                    <div class="no_circle">3</div>
                    <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                </div>
                
                <div class="headingwrap clearfix mt20 mb20">
                    <div class="no_circle">4</div>
                    <h4 class="numaric_heading inlineblk">Payment</h4>
                </div>
                
            </div>  <!--End Wrapleft --> 

            <div id="Level2" class="wrapleft pr40 mpr0 pb20" runat="server">
                <div class="headingwrap visited clearfix">
                    <div class="no_circle">1</div>
                    <h4 class="numaric_heading inlineblk">Contact Details</h4>
                    
                  <%--  <a href="#" class="pull-right mt5">Edit</a>--%>
                     <asp:Button ID="btnl2Editlogin" runat="server" Text="Edit"  OnClick="BtnEditLogin_Click" class="smp_btn pull-right"/>
               </div>

                    <div class="form-group row p15 text-left clearfix">
                     <div class="col-sm-20 pv15 br_dark">
                        <p class="mb0"><b>Name : </b> <asp:Label runat="server" ID="L2name" /> </p>
                        <p class="mb0"><b>Email : </b> <asp:Label runat="server" ID="L2Email" /></p>
                        <p class="mb0"><b>Phone : </b> <asp:Label runat="server" ID="L2Phone" /></p>
                    </div>
                </div>

                <div class="headingwrap active clearfix mt20">
                    <div class="no_circle">2</div>
                    <h4 class="numaric_heading inlineblk">Shipping &amp; Billing Address</h4>
                </div>    
                
                <div class="checkoutleft">
                    <form>
                    	<div class="cmn_wrap">
                            <h2 class="text-center sub_heading2">Shipping Address</h2>
                        <%--    <div class="form-group mt15 mb15 clearfix">
                                <label class="col-md-8 pl0">Shipping address type<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                  <label class=""> 
                             
                                       <asp:RadioButton ID="RBPersonal"   runat="server" GroupName="X"  Text="Personal" Checked="true" onclick="javascript:chkPB();" />
                                   </label>
                                  <label class=""> 
                                      <asp:RadioButton ID="RBBusiness" runat="server"  GroupName="X" Text="Business" onclick="javascript:chkPB();" />
                                  </label>
                                </div>
                            </div>--%>
                            <div runat="server">
                              <div class="form-group mb10 clearfix" >
                                <label class="col-md-8 pl0">Bussiness Name</label>
                                <div class="col-md-12 mpl0">
                              
                                    <asp:TextBox runat="server" ID="txtComname" Text="" class="form-control"   MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                </div>
                                  <label style="font-size:12px;font-weight:lighter;padding-top:10px" class="mb15">A business name is required if your order is being delivered to a non-residential address.</label>
                            </div>
                                  <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Receivers Name</label>
                                <div class="col-md-12 mpl0">
                             <asp:TextBox runat="server" ID="txt_attnto" Text="" class="form-control" MaxLength="250"  ></asp:TextBox>
                                </div>
                            </div>
                            
                            </div>
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Street Address<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                <asp:TextBox runat="server" ID="txtsadd" Text="" class="form-control"   MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter Street Address"   ControlToValidate="txtsadd" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Address Line 2</label>
                                <div class="col-md-12 mpl0">
                                <asp:TextBox runat="server" ID="txtadd2" Text="" class="form-control"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Suburb / Town<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                 <asp:TextBox runat="server" ID="txttown" Text="" class="form-control checkout_input"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="rfvtown" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter Suburb/Town" ForeColor="Red"  ControlToValidate="txttown"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">State Province<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                 <asp:TextBox runat="server" ID="txtstate" Text="" CssClass="form-control" MaxLength="20" Visible="false" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="vldRequiredSkin" 
           ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" 
          Text="Enter State/Province"  ControlToValidate="txtstate" ForeColor="Red"></asp:RequiredFieldValidator>
      <asp:DropDownList ID="drpstate1" runat="server" class="form-control" Visible="true"> </asp:DropDownList>

   <asp:RequiredFieldValidator ID="rfvddlstate" Display="Dynamic"  runat="server" Text="Select State/Province" ErrorMessage="Required" ControlToValidate="drpstate1" ValidationGroup="Mandatory_Express_Level2" InitialValue="" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group mb10 clearfix" id="aucust" runat="server">
                                <label class="col-md-8 pl0">Postal Zipcode<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                    <asp:TextBox runat="server" ID="txtzip" Text="" class="form-control" MaxLength="10" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvzip" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter Post/Zip Code"  ControlToValidate="txtzip" ForeColor="Red"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                    <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890"  TargetControlID="txtzip" />
                                </div>
                            </div>

                            <div class="form-group mb10 clearfix" id="intercust" runat="server">
                            <label class="col-md-8 pl0">Postal Zipcode<span class="required">*</span></label>
                               <div class="col-md-12 mpl0">
                     <asp:TextBox runat="server" ID="txtzip_inter" Text="" class="form-control" MaxLength="10" ></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter Post/Zip Code"  ControlToValidate="txtzip_inter" ForeColor="Red"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                     </div>
                
                     </div>

                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Country<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                <asp:DropDownList ID="drpCountry" runat="server" class="form-control" onselectedindexchanged="drpCountry_SelectedIndexChanged" 
          AutoPostBack="True" > </asp:DropDownList>
                                </div>
                            </div>
                           
                            
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Delivery Instructions</label>
                                <div class="col-md-12 mpl0">
                               <asp:TextBox runat="server" ID="txtDELIVERYINST" Text="" class="form-control" MaxLength="30" ></asp:TextBox>
                                </div>
                            </div>
						</div>
                        
                        
                        
                        <div class="cmn_wrap" id="L2DivBilling" runat="server">
                            <h2 class="text-center sub_heading2">Billing Address</h2>
                                  <div class="form-group mb10 clearfix" >
                                <label class="col-md-8 pl0">Bussiness Name</label>
                                <div class="col-md-12 mpl0">
                              
                                    <asp:TextBox runat="server" ID="txtbillbusname" Text="" class="form-control"   MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                </div>
                                  <label style="font-size:12px;font-weight:lighter;padding-top:10px" class="mb15">A business name is required if your order is being billed to a non-residential address.</label>
                            </div>
                                  <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Bill Name</label>
                                <div class="col-md-12 mpl0">
                             <asp:TextBox runat="server" ID="txtbillname" Text="" class="form-control" MaxLength="250"  ></asp:TextBox>
                                </div>
                            </div>
                            


                            <div class="form-group mb10 clearfix" >
                                <label class="col-md-8 pl0">Street Address<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                  <asp:TextBox runat="server" ID="txtsadd_Bill" Text="" class="form-control"   MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter Street Address"   ControlToValidate="txtsadd_Bill" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Address Line 2</label>
                                <div class="col-md-12 mpl0">
                             <asp:TextBox runat="server" ID="txtadd2_Bill" Text="" class="form-control"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Suburb / Town<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                       <asp:TextBox runat="server" ID="txttown_Bill" Text="" class="form-control"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter Suburb/Town" ForeColor="Red"  ControlToValidate="txttown_Bill"></asp:RequiredFieldValidator>
                                 </div>
                            </div>
                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">State Province<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                  

      <asp:DropDownList ID="drpstate2" runat="server" class="form-control" Visible="true"> </asp:DropDownList>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" Text="Select State/Province" ErrorMessage="Required" ControlToValidate="drpstate2" ValidationGroup="Mandatory_Express_Level2" InitialValue="" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                     <asp:TextBox runat="server" ID="txtstate_Bill" Text="" CssClass="form-control" MaxLength="20"  style="display:none " Visible="false" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter State/Province"  ControlToValidate="txtstate_bill" ForeColor="Red"></asp:RequiredFieldValidator>
   
                                </div>
                            </div>
                            <div class="form-group mb10 clearfix" id="aucust_bill" runat="server">
                                <label class="col-md-8 pl0">Postal Zipcode<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                                    <asp:TextBox runat="server" ID="txtzip_bill" Text="" class="form-control" MaxLength="10" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvzip_bill" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory_Express_Level2" Display="Dynamic" Text="Enter Post/Zip Code"  ControlToValidate="txtzip_bill" ForeColor="Red"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                   
                                </div>
                            </div>

          

                            <div class="form-group mb10 clearfix">
                                <label class="col-md-8 pl0">Country<span class="required">*</span></label>
                                <div class="col-md-12 mpl0">
                           <asp:DropDownList ID="drpcountry_bill" runat="server" class="form-control" onselectedindexchanged="drpCountry_bill_SelectedIndexChanged" 
          AutoPostBack="True" > </asp:DropDownList>
                                </div>
                            </div>
                           
						</div>
                         <asp:FilteredTextBoxExtender ID="ftezip_bill" runat="server" FilterMode="ValidChars" ValidChars="1234567890"  TargetControlID="txtzip_bill" />
                        <div class="form-group col-lg-20 center-block text-center">
                           <label class="mf12 mmt15">
                              <%-- <input class="checkbox-inline m10 mmt0 mmb0" type="checkbox">--%>
                               Billing address same as shipping address</label>
                             <asp:CheckBox ID="ChkBillingAdd" runat="server" Visible="true" AutoPostBack="true" 
                                    Class="CheckBoxSkin" Checked="false" OnCheckedChanged="ChkBillingAdd_CheckedChanged1" />
                        </div>
                        
                        <p class="text-center">
                        	<%--<button class="ph30 f16 btn primary-btn-green">Continue &gt; </button>--%>
                            <asp:Button runat="server" ID="BtnL2Continue" OnClick="BtnL2Continue_Click" Text="Continue" class="ph30 f16 btn primary-btn-green"    ValidationGroup="Mandatory_Express_Level2" UseSubmitBehavior="true"  />   
                        </p>
                        
                    </form>
                </div>
                
                <div class="headingwrap clearfix mt20">
                    <div class="no_circle">3</div>
                    <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                </div>
                
                <div class="headingwrap clearfix mt20 mb20">
                    <div class="no_circle">4</div>
                    <h4 class="numaric_heading inlineblk">Payment</h4>
                </div>
                
            </div>
         
            <div class="wrapleft pr40 mpr0 pb20" id="Level3" runat="server">  
              
               <div class="clearfix">
                    <div class="headingwrap visited clearfix">
                        <div class="no_circle">1</div>
                        <h4 class="numaric_heading inlineblk" id="hl1" runat="server">Contact Details</h4>
                        
                     <%--   <a href="#" class="pull-right mt5">Edit</a>--%>
                         <asp:Button ID="btnl3editlogin" runat="server" Text="Edit"  OnClick="BtnEditLogin_Click" class="smp_btn pull-right"/>
                   </div>
                   <div class="form-group row p15 text-left clearfix">
                         <div class="col-sm-20 pv15 br_dark">
                        <p class="mb0"><b>Name : </b> <asp:Label runat="server" ID="L3Name" /> </p>
                        <p class="mb0"><b>Email : </b> <asp:Label runat="server" ID="L3Email" /></p>
                        <p class="mb0"><b>Phone : </b> <asp:Label runat="server" ID="L3Phone" /></p>
                        </div>
                    </div>
                </div>
                
                
                
                <div class="clearfix">
                    <div class="headingwrap visited clearfix">
                        <div class="no_circle">2</div>
                        <h4 class="numaric_heading inlineblk">Shipping Address</h4>
                        
                        <a href="#" class="pull-right mt5" id="L3AEditAddress" runat="server">Edit Address</a>
                        <asp:Button ID="BtnL3EditAddress" runat="server" Text="Edit Address"  OnClick="BtnEditAddress_Click" class="smp_btn pull-right"/>
                   </div>
                   <div class="form-group row p15 text-left clearfix">
                         <div class="col-sm-20 pv15 br_dark">
                             <p class="mb0"><asp:Label runat="server" ID="L3ship_company_name" />  </p>
                              <p class="mb0"><asp:Label runat="server" ID="L3ship_attn" />  </p>
                            <p class="mb0"> <asp:Label runat="server" ID="L3Ship_Street" />  </p>
                            <p class="mb0"> <asp:Label runat="server" ID="L3Ship_Address" /> </p>
                            <p class="mb0"> <asp:Label runat="server" ID="L3Ship_Suburb" /> </p>
                             <p class="mb0"> <asp:Label runat="server" ID="L3Ship_State" /> </p>
                             <p class="mb0"> <asp:Label runat="server" ID="L3Ship_Zipcode" /> </p>
                             <p class="mb0"><asp:Label runat="server" ID="L3Ship_Country" /> </p>
                               <p class="mb0"><asp:Label runat="server" ID="L3Ship_DELIVERYINST" /> </p>
                            
                           
                        </div>
                    </div>
                </div>
                
                
                
                <div class="headingwrap active clearfix mt20">
                    <div class="no_circle">3</div>
                    <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                </div>
                
                <div class="checkoutleft">
                    <form>
                    
                        <div class="form-group pt10 mb30 clearfix">
                            <div class="col-md-14 col-xs-17 pl0 mp0">
                              <%--  <select class="form-control">
                                    <option>Counter Pickup</option>
                                    <option>Counter Pickup</option>
                                </select>--%>
                                 <asp:DropDownList NAME="drpSM1" ID="drpSM1" runat="server" class="form-control">
                                            <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method" >Please Select Shipping Method</asp:ListItem>
                                           <asp:ListItem Text="Standard Shipping" Value="Standard Shipping">Standard Shipping</asp:ListItem>
                                            <asp:ListItem Text="Shop Counter Pickup" Value="Counter Pickup">Shop Counter Pickup</asp:ListItem>
                     </asp:DropDownList>
                    <asp:RequiredFieldValidator Display="Dynamic" ID="rfvdrpSM1" InitialValue="Please Select Shipping Method" ControlToValidate="drpSM1" runat="server" ErrorMessage="Please Select Shipping Method" ValidationGroup="Mandatory_Express_Level3"  SetFocusOnError="true" ForeColor="Red" />
                            </div>
                            <div class="col-md-2 col-xs-2 mpr0 list-inline">
                            	<li>
                                    <a href="#" onclick="ShowShipMethodExpress();">
                                	<img class="mt5" src="images/info1.png"/>

                                    </a>
                                </li>
                            </div>
                        </div>
                        <div class="form-group mb30 clearfix">
                            <label>Order special notes / Comments<span class="required">*</span></label>
                                <asp:TextBox ID="TextBox1"  runat="server" Rows="5" Columns="30" Font-Size="12px" CssClass="form-control"  MaxLength="240"  onkeyDown="return checkMaxLength(this,event,'240');" 
                                TextMode="MultiLine">
                            </asp:TextBox>
                            
                        </div>
                        
                        <p class="text-center">
                        
                        <asp:Button runat="server" ID="BtnL3Continue" OnClick="BtnL3Continue_Click" Text="Continue" class="ph30 f16 btn primary-btn-green"    ValidationGroup="Mandatory_Express_Level3" UseSubmitBehavior="true"  />   
                        </p>
                        
                    </form>
                </div>
                
                
                <div class="headingwrap clearfix mt20 mb20">
                    <div class="no_circle">4</div>
                    <h4 class="numaric_heading inlineblk">Payment</h4>
                </div>
                
            </div>

           <div class="wrapleft pr40 mpr0 pb20" id="Level4" runat="server">  
                 
                            
              
               <div class="clearfix">
                    <div class="headingwrap visited clearfix">
                        <div class="no_circle">1</div>
                        <h4 class="numaric_heading inlineblk">Contact Details</h4>
                        
                       <%-- <a href="#" class="pull-right mt5">Edit</a>--%>
                             <asp:Button ID="btneditlogin4" runat="server" Text="Edit"  OnClick="BtnEditLogin_Click" class="smp_btn pull-right"/>
                   </div>
                   <div class="form-group row p15 text-left clearfix">
                         <div class="col-sm-20 pv15 br_dark">
                             <p class="mb0"><b>Name : </b> <asp:Label runat="server" ID="L4name" /> </p>
                        <p class="mb0"><b>Email : </b> <asp:Label runat="server" ID="L4Email" /></p>
                        <p class="mb0"><b>Phone : </b> <asp:Label runat="server" ID="L4Phone" /></p>
                        </div>
                    </div>
                </div>
                
                
                
                <div class="clearfix">
                    <div class="headingwrap visited clearfix">
                        <div class="no_circle">2</div>
                        <h4 class="numaric_heading inlineblk">Shipping Address</h4>
                        
                        <a href="#" class="pull-right mt5" id="L4AEditAddress" runat="server">Edit Address</a>
                         <asp:Button ID="BtnEditAddress" runat="server" Text="Edit Address"  OnClick="BtnEditAddress_Click" class="smp_btn pull-right"/>
                   </div>
                   <div class="form-group row p15 text-left clearfix">
                         <div class="col-sm-20 pv15 br_dark">
                              <p class="mb0"> <asp:Label runat="server" ID="L4Ship_Company" /> </p>
                             <p class="mb0"><asp:Label runat="server" ID="L4Ship_Attnto" /></p>
                            <p class="mb0"> <asp:Label runat="server" ID="L4Ship_Street" /> </p>
                            <p class="mb0"><asp:Label runat="server" ID="L4Ship_Address" /></p>
                            <p class="mb0"><asp:Label runat="server" ID="L4Ship_Suburb" /></p>
                            <p class="mb0"><asp:Label runat="server" ID="L4Ship_State" /></p>
                             <p class="mb0"><asp:Label runat="server" ID="L4Ship_Zipcode" /></p>
                             <p class="mb0"><asp:Label runat="server" ID="L4Ship_Country" /></p>
                                <p class="mb0"><asp:Label runat="server" ID="L4Ship_DELIVERYINST" /> </p>
                            
                        </div>
                    </div>
                </div>
             
                
                <div class="clearfix">
                    <div class="headingwrap visited clearfix">
                        <div class="no_circle">3</div>
                        <h4 class="numaric_heading inlineblk">Shipping Method</h4>
                        
                        <a href="#" class="pull-right mt5" id="L4AEditShippingMethod" runat="server">Edit Shipping Method</a>
                        <asp:Button ID="BtnL4EditShippingMethod" runat="server" Text="Edit Shipping Method"  OnClick="BtnL4EditShippingMethod_Click" class="smp_btn pull-right"/>
                   </div>
                   <div class="form-group row p15 text-left clearfix">
                         <div class="col-sm-20 pv15 br_dark">
                             <div id="divshopcounter" runat="server">
                            <p class="mb0"><b>Click and Collect </b> </p>
                            <p class="mb0">
                            	Pickup from wagnor electronic warehouse <br />
                                84-90, Paramtta Road,
                            	Summer Hill, NSW 2130
                            </p>
                            <br />
                                 </div>
                              <div id="divstandardship" runat="server">
                             <p class="mb0"><b>Shipping Method</b> </p>
                             <p class="mb0">
                             <asp:Label ID="lblShippingMethod" runat="server" Text=""  ></asp:Label>
                                 </p>
                              <br />
                                  </div>
                            <p class="mb0"><b>Order special notes / Comments </b></p>
                            <p class="mb0"><asp:Label runat="server" ID="L4Comments" /></p>
                        </div>
                    </div>
                </div>
              
                
                <div class="headingwrap active clearfix mt20 mb20">
                    <div class="no_circle">4</div>
                     <% 
                         OrderServices objOrderServices = new OrderServices();
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                      {
                      %>
                    <h4 class="numaric_heading inlineblk">Payment</h4>
                    <% } else { %>
                    <h4 class="numaric_heading inlineblk">Submit Order</h4>
                    <% } %>
                </div>
                
                <div class="checkoutleft" id="Level4_Payment" runat="server">
                    <form>
                    
                    	<div class="form-group mb10 clearfix">
                            <label class="col-md-7 pl0">Purchase Order No :</label>
                            <div class="col-md-10 mpl0">
                          <asp:TextBox  Width="100%" MaxLength="12"  ID="ttOrder" runat="server"  placeholder="Order No" class="form-control"  />
                              <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
                            </div>
                            <div class="col-md-20 mt10 pl0">
                            <p id="of_ptag" runat="server">Optional field. Enter your own order reference ID.</p>
                            </div>
                        </div>

                    
                        <%--<div class="form-group dblock">
                          <label class="checkbox-inline lspace0">
                          
                            <a href="#"><img src="images/master_check.png"/> </a></label><a href="#">
                          <label class="checkbox-inline"></label></a>
                           
                            <a href="#"><img src="images/paypal_uncheck.png"/> 
                            <label class="checkbox-inline "></label></a>
    
                        </div>
                        
                        <div class="col-sm-20 mp0 nolftpadd">
                            <div class="form-group col-lg-20 mp0 nolftpadd">
                                <div class="col-lg-20 mp0 nolftpadd">
                                 <label>Name on Card &nbsp;&nbsp;<span class="required">*</span></label>
                                    <input class="form-control checkout_input" type="text"/>
                                    </div>
                      
                             </div>
                        </div>
                        <div class="col-sm-20 mp0 nolftpadd">
                            <div class="form-group col-lg-20 mp0 nolftpadd">
                                <div class="col-lg-20 mp0 nolftpadd">
                                 <label>Card Number &nbsp;&nbsp;<span class="required">*</span></label>
                                    <input class="form-control checkout_input" type="text"/>
                                    </div>
                       
                             </div>
                        </div>
                        <div class="col-sm-20 mp0 nolftpadd clearfix">
                            <div class="form-group col-lg-10 col-sm-20 mp0 nolftpadd">
                                <div class="cvv-left">
                                 <label>CVV &nbsp;&nbsp;<span class="required">*</span></label>
                                    <input class="form-control" type="text"/>
                                    </div>
                                <div class="cvv-right">
                                <span class="mandatory checkouterror">
                                    <img src="images/question-icon.png"/>
                                </span>
                                    
                                </div>
                             </div>
                        </div>
                        
                        <div class="col-sm-20 nolftpadd clearfix margin_top15 mp0">
                            <div class="form-group col-lg-20 mp0 nolftpadd clearfix">
                                <div class="col-lg-8 col-xs-10 mp0 nolftpadd">
                                    <select class="form-control">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                    </select>
                                </div>
                                <div class="col-lg-8 col-xs-10 mp0">
                                    <select class="form-control">
                                        <option>2015</option>
                                        <option>2016</option>
                                        <option>2017</option>
                                        <option>2018</option>
                                        <option>2019</option>
                                    </select>
                                    
                                </div>
                             </div>
                        
                        </div> --%>

                         <div id="divError" runat="server" style="color:Red;" >
                                    </div>

                                    <div id="divlink" runat="server" >
                                    </div>

                        <br />

                <div runat ="server" id="div2" class="accordion_head_yellow gray_40" style="font-size:12px;text-align:center; font-weight:bold;margin-bottom:12px;background: #FFF200; padding: 12px 17px;" >
                </div>
                <h4 class="panel-title"    id= "h3Pay1"  runat="server"> <span class="collapsed"  > </span></h4>

<div   id= "divpaid" runat="server" ></div>

          <div class="form-group dblock" id="PayType"  runat="server">
              <% 
                 OrderServices objOrderServices = new OrderServices();

                 if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
               {
                   decimal totamt;
                   if (lblAmount.Text == "")
                   {
                       OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                       oOrderInfo = objOrderServices.GetOrder(OrderID);
                       totamt = oOrderInfo.TotalAmount;
                   }
                   else
                   {
                       totamt = Convert.ToDecimal(lblAmount.Text);
                   }
                   
                                     //SecurePayService objSecurePayService = new SecurePayService();
                                     if ( totamt <= 100)
                                     {
                              %> 
                                <label class="checkbox-inline lspace0">   
             <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnSecurePayLink_Click" CausesValidation="false" ><label>                
                                <asp:Image ID="ImagePaySP" runat="server" ImageUrl="images/master_check.png"  style="margin-top: -5px;cursor: pointer;" alt="cc"/>
                                </label></asp:LinkButton></label>
                                <%} %>
                                 <label class="checkbox-inline lspace0">
               <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnPayPalPayLink_Click" CausesValidation="false" ><label>
                                <asp:Image ID="ImagePay" runat="server" ImageUrl="images/paypal_uncheck.png"  style="margin-top: -5px;cursor: pointer;" alt="cc"/>
                                </label></asp:LinkButton></label>
         
         
                  
            <%} %>            
       
 </div>

<div id="SecurePayAcc" runat="server">
    <div runat="server" id="PaySPDiv" class="payment_form clearfix">
      
    
              


           <asp:DropDownList ID="drppaymentmethod" runat="server" width="200px" CssClass="cardinput" onchange="Controlvalidate('dd')"   Visible =false />     
           
          
            <div class="col-sm-20 mp0 nolftpadd">
            <div class="form-group col-lg-20 mp0 nolftpadd">
                <div class="col-lg-20 mp0 nolftpadd">
                    <label>
                    Name on Card   
                    <span class="required">*</span>
                    </label>
                
                    <asp:TextBox runat="server" ID="txtCardName"  CssClass="form-control checkout_input"    MaxLength="50"  OnBlur="Controlvalidate('cn')"  />
                    </div>
                <div class="col-lg-8">
                <p class="mandatory m0 checkouterror"><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="txtCardName" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Name on Card" ValidationGroup="sp"></asp:RequiredFieldValidator>
                    </p>
                </div>
                </div>
                </div>
       

              
                 <div class="col-sm-20 mp0 nolftpadd">
<div class="form-group col-lg-20 mp0 nolftpadd">
               <div class="col-lg-20 mp0 nolftpadd">
               <label>Card Number &nbsp;&nbsp;<span class="required">*</span></label>
               
               <asp:TextBox runat="server" ID="txtCardNumber" CssClass="form-control checkout_input"    MaxLength="19" OnBlur="Controlvalidate('cno')"  ValidationGroup="sp"/>
                </div>
              <div class="col-lg-8">
                <p class="mandatory m0 checkouterror">
               <asp:RequiredFieldValidator ID="rfCardNumber" runat="server" 
                    ControlToValidate="txtCardNumber" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Card Number"  ValidationGroup="sp"></asp:RequiredFieldValidator>
                    
                    <asp:customvalidator id="CustomValidator1"   Display="Dynamic" ValidationGroup="sp" CssClass="error-text" ClientValidationFunction="ValidCC" errormessage="Please Check Credit Card Number" controltovalidate="txtCardNumber" runat="server">
	             </asp:customvalidator>
                 </p>
              </div>
              </div>
              </div>
              
            
           
                 <div class="col-sm-20 mp0 nolftpadd clearfix">
<div class="form-group col-lg-10 col-sm-20 mp0 nolftpadd">
                <div class="cvv-left" >
                <label>CVV &nbsp;&nbsp;<span class="required">*</span></label>
                <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="100px"  CssClass="form-control" MaxLength="4" OnBlur="Controlvalidate('cvv')" Text="123"/>    
             
                 </div>
                 <div class="cvv-right">
                    <span class="mandatory checkouterror">
                       <a href="#" class="checkoutlink" data-target=".bs-example-modal-lg1" data-toggle="modal" role="button">
                              <img class=" margin_rgt15" src="/images/question-icon.png"/></a>

              

                    </span>
                    </div>
                    
                
                </div>
                <div class="col-lg-8">
                <p class="mandatory m0 checkouterror">
                 <asp:RequiredFieldValidator ID="rfsecuritycode" runat="server" 
                    ControlToValidate="txtCardCVVNumber" Display="Dynamic" CssClass="error-text"  style="float: right;" ValidationGroup="sp"
                    ErrorMessage="Enter Card Security Code<Br/>"></asp:RequiredFieldValidator>                    
                          </p>
               </div>
          
              </div>
                
                
      

            <div class="col-sm-20 nolftpadd clearfix margin_top15 mp0">
<div class="form-group col-lg-20 mp0 nolftpadd clearfix">
                <div class="col-lg-8 col-xs-10 mp0 nolftpadd" >
                     <label>Card Expiry &nbsp;&nbsp;<span class="required">*</span></label>
                  
                <asp:DropDownList NAME="drpExpmonth"  ID="drpExpmonth" runat="server"  CssClass="form-control checkout_input">           
                    <asp:ListItem Selected ="true" Text ="01" Value="01"></asp:ListItem>
                    <asp:ListItem  Text ="02" Value="02"></asp:ListItem>
                    <asp:ListItem  Text ="03" Value="03"></asp:ListItem>
                    <asp:ListItem Text ="04" Value="04"></asp:ListItem>
                    <asp:ListItem  Text ="05" Value="05"></asp:ListItem>
                    <asp:ListItem  Text ="06" Value="06"></asp:ListItem>
                    <asp:ListItem  Text ="07" Value="07"></asp:ListItem>
                    <asp:ListItem Text ="08" Value="08"></asp:ListItem>
                    <asp:ListItem  Text ="09" Value="09"></asp:ListItem>
                    <asp:ListItem  Text ="10" Value="10"></asp:ListItem>
                    <asp:ListItem  Text ="11" Value="11"></asp:ListItem>
                    <asp:ListItem  Text ="12" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                    </div>
                    <div class="col-lg-8 col-xs-10 mp0" >
                     <label> &nbsp;&nbsp;<span class="required">&nbsp;&nbsp;</span></label>
                <asp:DropDownList NAME="drpExpyear"  ID="drpExpyear" runat="server" CssClass="form-control checkout_input">          
                    </asp:DropDownList>
                    </div>


              <div class="col-lg-8 col-xs-20">
              <p class="mandatory m0 checkouterror">
                 <asp:RequiredFieldValidator ID="rfmonthsp" runat="server" 
                    ControlToValidate="drpExpmonth" Display="Dynamic" CssClass="error-text" 
                    ErrorMessage="Select Month"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
                    ControlToValidate="drpExpyear" Display="Dynamic" CssClass="error-text" 
                    ErrorMessage="Select Year" ValidationGroup="sp"></asp:RequiredFieldValidator>
                    </p>
                </div>
                </div>
                </div>
               
                   <div class="col-sm-20 nolftpadd">
                    <div class="form-group col-lg-20 nolftpadd">
                        <h3 class="green_clr">Total Amount $ <asp:Label runat="server" ID="lblAmount" CssClass="totalamt"  /> </h3>                    
                    
                    </div>
                </div>
            
             

            <div class="cl"></div>
              <div class="col-sm-20 nolftpadd margin_btm_30">
<div class="form-group col-lg-8 nolftpadd">
                <asp:Button runat="server" ID="btnSP" ValidationGroup="sp" Text="Pay Now" class="btn btn-primary" OnClick="btnSecurePay_Click" OnClientClick="javascript:SetinitSP()" />       

                  <asp:Button runat="server" ID="BtnProgressSP" Text="Processing Payment. Please Wait" style="display:none;visibility:visible;float:left;" class="btn btn-primary" Enabled="false"   />     
                 
                 
                  
                    <div id="div6" style="font-size:12px;margin-left:30px;color:Red" runat="server"  ></div>
            </div>
            </div>
          
            <div class="cl"></div>
          </div>

</div>
                        <div id="PayPaypalAcc" runat="server" >

                        <%  
                            PayPalService objPayPalService = new PayPalService();
                            Boolean blnpp = objPayPalService.CheckPayPal(); 
                        %>
                        <% if (blnpp == false)
                      { %>
                    <div style="padding: 16px 2px 16px 174px;background-color:#FFD52B" class="alert yellowbox icon_4"  >
                         <h3 style="font-size: 16px;color:Black;">Paypal Payment Option is currently unavailable. <br />Please kindly proceed to check out with Standard Credit Card payment option.</h3>
                   </div>
                   <%}
                     else
                      {     %>
                        <div class="col-sm-20 mp0 nolftpadd" style="display:block; text-align:center; margin-bottom:15px; border:1px solid #ccc;">
                            <div class="form-group text-center">
                                <div id="divpaypalaccount" runat="server">
                                 <p class="blue_text margin_top15"><strong>Pay using your Paypal Account</strong></p>
                                 <p class="margin_bott15">You will be redirected to paypal website to complete payment transaction</p>
                                </div>

                                <h3 class="blue_text"><b>Total Amount $ <asp:Label runat="server" ID="lblpaypaltotamt" CssClass="totalamt"  /> </b></h3>
                             </div>
                        </div>
                         <%} %>
                        
                        <p class="text-center">
                        	<%--<button class="ph30 f16 btn primary-btn-green"> Make Payment </button>--%>
                            <asp:Button runat="server" ID="btnPay" Text="Make Payment" class="ph30 f16 btn primary-btn-green "  OnClick="btnPay_Click" OnClientClick="Setinit(this.id)"   />        <br />
                            <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait" style="display:none;visibility:visible;float:left;" class="btn btn-primary " Enabled="false"   />       
                             <br />
                        </p>
                        <br />
                         <br />
                        </div>
                    </form>
                </div> 
                <div class="checkoutleft nopadd" id="Level4_Submit" runat="server">
                    <div  class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne"  id= "divship" runat="server" >

<div class="panel-body">
 

         


 


           
   

    

   <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none;visibility: hidden"></asp:Button>
   

     

       <div id="divonlinesubmitordererror"  visible="false" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in" >
    <div class="modal-dialog modal-lg">
    <div class="modal-content">

        <div class="modal-header blue_color padding_top padding_btm"  style="background-color: #0069b2;color:#fff;">

          <h4 id="H5" class=" white_color font_weight modal-title">Invalid Login Details for Checkout! </h4>
         
        </div>
        <div class="modal-body">

     
         <p>   
                      
                         User Id does not matche with online submit order.
                        <br /> <a href="/logout.aspx" style="color:#15c;" >Please Click Here</a></p>
        

        </div>
      </div>
  </div> 
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
  
                    

                <div id="PopupMsg" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in">
  <div class="modal-dialog  margin_top_popup20 ">
    <div class="modal-content  border_radius_none">

        
        <div class="modal-body">
         <p class="alert-red">Please review and correct Order Clarification / Errors before proceeding to Check Out!  </p>
        </div>
        <div class="modal-footer">
       
           <asp:Button ID="btnCancelerroritems" runat="server" Text="Close" CssClass="btn-lg padding_top btn-danger border_none border_radius_none white_color semi_bold font_14 mar_right_30  mob_100 margin_left" OnClick="btnCancelerroritems_Click" />
        </div>
      </div>
  </div>
</div>   

</div>

</div>
                </div>
                
            </div>
          </div>  
           
      </div>

             


</div>
   

 <div class="" id="liPayOption" runat="server">
     <div id="headingtwo" role="tab" class="" >
          <h4 class="panel-title"    onclick="OnclickTab('Pay')"  style="cursor:pointer;" id= "h3Pay"  runat="server"> 
              <span class="collapsed"   id="spanPay"  runat="server">
        

              </span>
           </h4>
          
        </div>

       <%-- id="collapseTwo" --%>
       <div runat="server" id="Paydiv" >
          <div class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo"  id= "divPay" runat="server">

            
		  <div class="panel-body">
			<div runat="server" id="divCC" visible="false" >
            
                <div class="col-lg-20" runat ="server" id="div1"> 
            
            
                               <div runat="server" id="Div5">

                               <div runat="server" id="PaypalApiDiv">
 <div class="form-group dblock">     
                
              

             <%  
                PayPalService objPayPalService = new PayPalService();

               // Boolean blnpp = true;          
                Boolean blnpp = objPayPalService.CheckPayPal(); 
                  %>
                   <% if (blnpp == false)
                      { %>
                    <div style="background-color:#FFD52B;display:block; height:100px; text-align:center; margin-bottom:15px; border:1px solid #ccc;" class="alert yellowbox icon_4"  >
                         <h3 style="font-size: 16px;color:Black;">Paypal Payment Option is currently unavailable. <br />Please kindly proceed to check out with Standard Credit Card payment option.</h3>
                   </div>
                   <%}
                      else
                      {     %>

                    <p class="blue_text margin_top15"><strong>Pay using your Paypal Account</strong></p>
              <p class="margin_bott15">You will be redirected to paypal website to complete payment transaction</p>
                   <div class="col-sm-20 nolftpadd">
                    <div class="form-group col-lg-20 nolftpadd">
                     <h3 class="green_clr">Total Amount $  
                         
                    </h3>           
                    </div>
                </div>


                   

                    <asp:Button runat="server" ID="btnPayApi" Text="Pay Now" style="width:100px;" class="btn btn-primary " Visible="false"  OnClick="btnPayApi_Click" OnClientClick="Setinit(this.id)" />       


                 
                 
                 
                   <div class="cl"></div>
                   <%} %>
                   <div id="divContent" style="font-size:12px ;margin-left:30px;color:Red" runat="server"></div>
                      
            </div>
         </div>
                             


       
          
            </div>
               <%-- <div class="accordion_head_grey clear">Shipping & Order Details</div>
               <div class="col-lg-10">
<div class="form-group">
<label class="col-sm-20 control-label" for="inputEmail3"><strong>Bill To</strong></label>
               <div class="col-sm-20">
<ul class="list-group">
                    <li class="list-group-item" style="background-color:#fff;">  
                        <asp:Label ID="txtpaybill" runat="server" Text="Label" ></asp:Label></li>                
                  </ul>
                </div>
                <div class="clear"></div>
              </div>
            </div>
              <div class="col-lg-10">
<div class="form-group">
<label class="col-sm-20 control-label" for="inputEmail3"><strong>Ship To</strong></label>
              <div class="col-sm-20">
<ul class="list-group">
                    <li class="list-group-item" style="background-color:#fff;">   <asp:Label ID="txtpayship" runat="server" Text="Label"   ></asp:Label></li>                
                  </ul>
                </div>
                <div class="clear"></div>
              </div>
            </div>--%>
               
                 <%--   <div class="accordion_head_grey clear">Order Contents</div>--%>
               <div class="col-lg-20 col-xs-20">
<div class="table-responsive col-lg-20 col-sm-20">                                           
                     <div class="col-lg-20 text-right">
                  
                       <asp:Button ID="ImgBtnEditShipping" runat="server" Text="Edit / Update Order"  style="width:auto !important" CssClass="btn btn-primary" OnClick="ImgBtnEditShipping_Click"  CausesValidation="false" Visible="false" />
                    </div>
                   
                  </div>
                </div>
              </div>
             <div class="grid12" runat="server" id="divTimeout" visible="false" >
                <fieldset>
                <div style="text-align:center;padding:130px;" >
                <span style="font-size:21px;"  > Your session has timed out</span><br />
                <span style="font-size:14px;"> <a href="/Login.aspx" class="para pad10-0" style="font-size:11px; color:#0033cc; font-weight:bold;">Click here</a> to log in again </span>
                </div>
                </fieldset>
            </div>
			  <div class="cl"></div>
		  </div>
          </div>
		</div>
      </div> 
       </div>
<div class="" id="liFinalReview" runat="server">
 <div class="" role="tab" >
 <h4 class="panel-title"  onclick="OnclickTab('paid')"  style="cursor:pointer;"  id= "hpaid"  runat="server" > 
    <span  class="collapsed"  >
  <%--Completed--%><span  id="spanpaid"  runat="server">
 <%-- <img src="/images/select-downarrow_wht.png" class="pull-right" alt=""/>--%></span> </span>
   </h4>

    <h4 class="panel-title"   id= "hpaid1"  runat="server" >  <%--<span  class="collapsed"  > Completed </span>--%> </h4>
  </div>
		
        <div runat="server" id="paiddiv">
           
        </div>
        </div>

</div>
</div>
</div>
    <div  id="ship_method_express" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfoscp">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header blue_clr p15">
<%--        <button type="button" class="close white_text" OnClientClick="CloseShipMethodExpress();" ><span aria-hidden="true">×</span></button>--%>
            <asp:ImageButton ID="ImageButton2" runat="server"  CssClass="close white_text"
                                                       src="/Images/close_btn.png"   OnClientClick="CloseShipMethodExpress();" />
        <h4 class="modal-title" id="shipmethod_lbl">
        	<img class="popsucess" alt="img" src="/images/Order__info.png"/>
            Shipping Method options
        </h4>
      </div>
      <div class="modal-body">
        
           <div class="popuptext">
            <p style="font-size: 12px;">
                <b style="font-size: 16px;">Courier Service</b>
                <br />
                Australia wide flat rate delivery charge of $9.90*
                <br />
                Goods will be sent using Courier Service if under Gross /Cubic weight of 3Kg.
                <br />
                Parcels over 3Kg will be sent by most economical means, eg Road, E-Parcel Service
                etc.
                <br />
                Exemptions apply to heavy / large items (e.g Server Racks) will occur higher delivery
                charges our sales team will contact you prior to shipping.
                <br />
                Note. Dangerous goods cannot be sent by air, road service only.
                <br />
                <b style="font-size: 16px;">Shop Counter Pick Up</b>
                <br />
                Place your order online and pick up goods from our Sydney Showroom.
                <br />
                Important Details For Store Pick Up:
                <br />
                1. Please bring a printed copy of invoice when coming into store.
                <br />
                2. Proof of Identity is required when picking up goods from our store. Please ensure
                you have the Credit Card you made the purchase with and your Driver's
                <br />
                License ID when coming into the store to pick up goods purchased online. The Name
                on the Credit Card Must Match Your Drivers License ID.
                <br />
                You will not be able to pick up the goods from our store unless the Credit Card
                you made the purchase with and your Drivers License are shown OR if there if
                <br />
                there is a mismatch in details.
            </p>
        </div>
      
        
      </div>
      <div class="modal-footer" style="text-align:center;">
      <%--  <button type="button" OnClientClick="CloseShipMethodExpress();" class="btn button-close">Close</button>--%>
          <asp:Button ID="Button2" runat="server" Text="Close" CssClass="btn button-close"  OnClientClick="CloseShipMethodExpress();" CausesValidation="false"  />
      </div>
    </div>
  </div>
</div>
      <div id="SCP" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfoscp">
               <div class="modal-dialog custom">
                        <div class="modal-content">
                         <div class="close-selected">
                                                       <asp:ImageButton ID="ImageButton3" runat="server" 
                                                       src="/Images/close_btn.png"   OnClientClick="CloseSCP();" />
                        
                           
                      </div>
                          <div class="modal-header green_bg">
                            <h4 id="H4" class="text-center">
                            <img class="popsucess" alt="img" src="/images/Order__info.png"/>Shop Counter Pickup</h4>
                          </div>
                          <div class="modal-body">
                              <div class="col-lg-20 col-sm-20">
                              <div class="popuptext">
                                  <p style="font-size:12px;">
                                             
                          
                                <b style="font-size:13px;">IMPORTANT DETAILS FOR WHEN PICKING UP GOODS FROM STORE</b>
                              
                                <br />   <br />
                                1. Please being a printed copy of invoice when coming into store.
                                <br />
                                2. Proof of Identity is required. When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with.
                                <br />
                               The Name on the Credit Card Must Match Your Drivers License ID. Please ensure you have the Credit Card you made the purchase with and your Driver’s License ID when coming into the store to pick up goods purchased online.
                                <br />
                                You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Drivers License are shown OR if there if there is a mismatch in details.
                                <br />
                                When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with.  
                                </p> 
                                </div>
                              </div>                             
                          </div>
                          <div class="modal-footer clear border_top_none">
                            <asp:Button ID="Button1" runat="server" Text="Close" CssClass="btn button-close"  OnClientClick="CloseSCP();" CausesValidation="false"  />
                          </div>
                      </div>
                  </div>
               </div>
 <div  id="CCPopDiv" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade bs-example-modal-lg1">
                  <div class="modal-dialog">
                        <div class="modal-content">
                          <div class="modal-header green_bg">
                            <h4 id="H3" class="text-center">
                            <img class="popsucess" alt="img" src="/images/Order__info.png"/>CVV Help</h4>
                          </div>
                          <div class="modal-body">
                                  
                    
  <h1>How to find the security code on a credit card</h1>
    <p>Find out where to locate the security code on your credit card.</p>
    <p><strong> Visa, MasterCard, Discover, JCB, and Diners Club</strong></p>
        <p> The security code is a three-digit number on the back of your credit card, immediately following your main card number.</p>
        <img src="images/Mcard.jpg"/>
        <p><strong>American Express</strong></p>
        <p>The security code is a four-digit number located on the front of your credit card, to the right above your main credit card number.</p>
        <img src="images/Acard.jpg"/>
        <p>If your security code is missing or illegible, call the bank or credit card establishment referenced on your card for assistance.</p>                              
                          </div>
                          <div class="modal-footer clear border_top_none">
                           
                          </div>
                      </div>
                  </div>
    </div>
</asp:Content>

