<%@ Page Language="C#" MasterPageFile="~/Mainpage.Master" AutoEventWireup="true" Inherits="CheckOut" EnableEventValidation="false"
    Title="Untitled Page"  Culture="en-US" UICulture="en-US" Codebehind="CheckOut.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UC/OrderDet.ascx" TagName="OrderDet" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
 <script type="text/javascript">
     function DRPshippment() {
         alert('Non-Standard Delivery Area. We will contact you to confirm costing');
     }
</script> 

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="Server">
 

<asp:HiddenField ID="hidSourceID" runat="server" />







 
<%-- <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/MicroSitecss/jquery-ui.css" rel="stylesheet" type="text/css" />
      <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/MicroSitecss/Dynamicstyle.css" rel="stylesheet" type="text/css" />
      <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/MicroSitecss/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/MicroSitejs/multiaccordion.jquery.js" type="text/javascript"></script>
    <link href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/MicroSitecss/multiaccordion.jquery.min.css" rel="stylesheet" type="text/css" >
    --%>
    <%-- <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/jquery-1.5.1.min.js" type="text/javascript"></script>--%>

    <script type="text/javascript">

        function isAlphabetic() {
            var ValidChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890/\\';
            var sText = document.getElementById("ctl00_MainContent_ttOrder").value;
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
                    document.getElementById("ctl00_MainContent_ttOrder").value = '';
                    document.forms[0].elements["<%=ttOrder.ClientID%>"].focus();
                    return false;
                }
            }

            return isAlphabetic;
        }

        function checkorderid() {
            var msgCheck = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            /* if (document.forms[0].elements["<%=ttOrder.ClientID%>"].value.length == 0) {
            alert('Enter Order No and then proceed');
            document.forms[0].elements["<%=ttOrder.ClientID%>"].focus();
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
                var numaric = document.forms[0].elements["<%=ttOrder.ClientID%>"].value;
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
            
            var x = document.getElementById('<%= PopDiv.ClientID %>');
            //var x = document.getElementById("PopDiv");
            if (x != null)
                x.style.display = "none";

            var x1 = document.getElementById('<%= CCPopDiv.ClientID %>');

            if (x1 != null)
                x1.style.display = "none";


            //  $find("ShipmentModelPopupExtender").hide();
            $("#<%=drpSM1.ClientID %>").change(function (event) {
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Mail") {
                    ShowMailMessage();

                }

            });


        });
        $(document).ready(function () {
            var x = document.getElementById('<%= PopDiv.ClientID %>');
            //var x = document.getElementById("PopDiv");
            if (x != null)
                x.style.display = "none";

            var x1 = document.getElementById('<%= CCPopDiv.ClientID %>');
            //var x = document.getElementById("PopDiv");
            if (x1 != null)
                x1.style.display = "none";     
          //  $find("ShipmentModelPopupExtender").hide();
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
            
            document.getElementById("totalcoupickup").style.display = 'none';
            
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
            
            document.getElementById("totalcoupickup").style.display = 'none';
            
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
//            var msg = " IMPORTANT DETAILS FOR WHEN PICKING UP GOODS FROM STORE " + '\n' + '\n' +
//                      "1. Please being a printed copy of invoice when coming into store." + '\n' + '\n' +
//                      "2. Proof of Identity is required. When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with. " + '\n' +
//                      "The Name on the Credit Card Must Match Your Drivers License ID. Please ensure you have the Credit Card you made the purchase with and your Driver’s License ID when coming into the store to pick up goods purchased online. " + '\n' +
//                      "You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Drivers License are shown OR if there if there is a mismatch in details. " + '\n' +
//                      "When picking up goods from our store you will be required to show proof of identity and the credit card you made the purchase with. ";
//                      alert(msg);
            document.getElementById("ctl00_maincontent_SCP").style.display = 'block';
            $("#ctl00_maincontent_SCP").removeClass("modal fade lgn-orderinfoscp");
            $("#ctl00_maincontent_SCP").addClass("modal fade lgn-orderinfoscp in");
        }
        function CloseSCP() {
            $("#ctl00_maincontent_SCP").removeClass("modal fade lgn-orderinfoscp in");
            $("#ctl00_maincontent_SCP").addClass("modal fade lgn-orderinfoscp");
            document.getElementById("ctl00_maincontent_SCP").style.display = 'none';
        }
        function MouseHover() {
            // $find("PopDiv").show();
            var x = document.getElementById('<%= PopDiv.ClientID %>');
            //var x = document.getElementById("PopDiv");
        if (x!=null)
            x.style.display = "block";            

        }

        function MouseOut() {
            var x = document.getElementById('<%= PopDiv.ClientID %>');
            //var x = document.getElementById("PopDiv");
            if (x != null)
                x.style.display = "none";

            $('.modal-backdrop').remove();
   $body.removeClass("modal-open");
           
           // $find("ShipmentModelPopupExtender").hide();
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
         //  document.getElementById("OtherShipmentRow").style.display = 'none';

        }

        function ShowShipmentPanel() {
          //  document.getElementById("OtherShipmentRow").style.display = '';
            document.getElementById("DropShipmentRow").style.display = 'none';

        }

        function HidePanels() {
            if (document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') {
             //   document.getElementById("OtherShipmentRow").style.display = 'none';
                document.getElementById("DropShipmentRow").style.display = '';

            }
            else {
              //  document.getElementById("OtherShipmentRow").style.display = '';
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

            //            if ((document.getElementById("ctl00_MainContent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_MainContent_txtCompany").value == '') || (document.getElementById("ctl00_MainContent_txtCompany").value == null) || (document.getElementById("ctl00_MainContent_txtCompany").value == 'null'))) {
            //                document.getElementById("ctl00_MainContent_txtCompany").style.borderColor = "red";
            //                document.getElementById("ctl00_MainContent_txtCompany").focus();
            //                isCompanyEmpty = true;
            //            }
            //            else {
            //                document.getElementById("ctl00_MainContent_txtCompany").style.borderColor = "ActiveBorder";
            //            }
            //            *//


            //            // * comment by palani

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


            //            if ((document.getElementById("ctl00_MainContent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_MainContent_txtState").value == '') || (document.getElementById("ctl00_MainContent_txtState").value == null) || (document.getElementById("ctl00_MainContent_txtState").value == 'null'))) {
            //                document.getElementById("ctl00_MainContent_txtState").style.borderColor = "red";
            //                document.getElementById("ctl00_MainContent_txtState").focus();
            //                isStateEmpty = true;
            //            }
            //            else {
            //                document.getElementById("ctl00_MainContent_txtState").style.borderColor = "ActiveBorder";
            //            }
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
            var x = document.getElementById('<%= btnSP.ClientID %>');
            var y = document.getElementById('<%= BtnProgressSP.ClientID %>');
            var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');
            x.style.display = "none";
            y.style.display = "block";
            y.style.visibility = "visible";
            z.style.display = "block";
            z.style.visibility = "visible";
        }
        else {
            // Controlvalidate('dd');
            Controlvalidate('cno');
            Controlvalidate('cn');
            Controlvalidate('cvv');
        }

    }
    </script>
<%-- <script type="text/javascript">
     $(document).ready(function () {
         $(".fancybox").fancybox();
     });
</script>--%>
 <%-- 
<script type="text/javascript">

  $(document).ready(function () {
        $(".accordion").multiaccordion({ defaultIcon: "ui-icon-plusthick",
            activeIcon: "ui-icon-minusthick"
        });
    });

  </script>--%>

  <script language="JavaScript" type="text/javascript">
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
      sp = document.getElementById('<%=spanship.ClientID %>');
       hp = document.getElementById('<%=hship.ClientID %>');
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
       function ValidCC(sender, args) {

           /*    var cardno = '';
           var CardType = 0;
           AEcardno = /^(?:3[47][0-9]{13})$/;
           Mcardno = /^(?:5[1-5][0-9]{14})$/;
           Vcardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;              

           var dd = document.getElementById('<%= drppaymentmethod.ClientID %>');
           if (dd != null) {
           CardType = dd.value;
           }
           if (CardType == 2) //Amercican Express	   
           cardno = /^(?:3[47][0-9]{13})$/;
           else if (CardType == 5)//Mastercard 	   
           cardno = /^(?:5[1-5][0-9]{14})$/;
           else if (CardType == 6) //Visa	   
           cardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;              
           else
           return args.IsValid = false;

           var txt = document.getElementById('<%= txtCardNumber.ClientID %>');

           if (txt.value.match(AEcardno) && mod10_check(txt.value) == true) {

           if (txt != null) {
           txt.style.border = "";
           }
           if (CardType != 2) {
           SetDropDownValue(2);
           }
           return args.IsValid = true;
           }
           else if (txt.value.match(Mcardno) && mod10_check(txt.value) == true) {

           if (txt != null) {
           txt.style.border = "";
           }
           if (CardType != 5) {
           SetDropDownValue(5);
           }
           return args.IsValid = true;
           }
           else if (txt.value.match(Vcardno) && mod10_check(txt.value) == true) {

           if (txt != null) {
           txt.style.border = "";
           }
           if (CardType != 6) {
           SetDropDownValue(6);
           }
           return args.IsValid = true;
           }
           else {
           if (txt != null) {
           txt.style.border = "1px solid #FF0000";
           }
              
           if (CardType == 2) //Amercican Express	
           {
           sender.innerHTML = "Not a valid Amercican Express credit card number!";
           }
           else if (CardType == 5)//Mastercard 	   
           {
           sender.innerHTML = "Not a valid Mastercard card number!";
           }
           else if (CardType == 6) //Visa	 
           {
           sender.innerHTML = "Not a valid Visa card number!";
           }
           else {
           sender.innerHTML = "Not a valid card number!";
           }

           return args.IsValid = false;
           } */
       }
       function SetDropDownValue(dval) {
           /*    var ddl = document.getElementById('<%= drppaymentmethod.ClientID %>');
           if (ddl != null) {

           var opts = ddl.options.length;
           for (var i = 0; i < opts; i++) {
           if (ddl.options[i].value == dval) {
           ddl.options[i].selected = true;
           break;
           }
           }
           }
           */
       }
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
<script type="text/javascript">
   
    function Controlvalidate(ctype) {
        /* if (ctype == "dd") {
        var dd = document.getElementById('<%= drppaymentmethod.ClientID %>');
        if (dd != null && dd.value == 0) {

        dd.style.border = "1px solid #FF0000";
        }
        else {
        dd.style.border = "";

        }
        }*/
        if (ctype == "cno") {
            var cno = document.getElementById('<%= txtCardNumber.ClientID %>');
            if (cno != null && cno.value == "") {
                cno.style.border = "1px solid #FF0000";
            }
            else {
                cno.style.border = "";
            }
        }
        if (ctype == "cn") {
            var cn = document.getElementById('<%= txtCardName.ClientID %>');
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
            }
            else {
                cn.style.border = "";
            }
        }
        if (ctype == "cvv") {
            var cvv = document.getElementById('<%= txtCardCVVNumber.ClientID %>');
            if (cvv != null && cvv.value == "") {
                cvv.style.border = "1px solid #FF0000";
            }
            else {
                cvv.style.border = "";
            }
        }
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
<div class="panel-group clear" id="accordion" role="tablist" aria-multiselectable="true">
            
<div class="panel panel-default">
<%--<div class="panel-heading tab_blue_color" role="tab"  >

<h4 class="panel-title" onclick="OnclickTab('ship')" style="cursor:pointer;"  id="hship" runat="server" visible="true"> 
    <span class="" id="spanship" runat="server"> Shipping / Delivery Details 
<span   ><img  src="/images/tab_img.png" class="pull-right" alt=""></span>
</span></h4>
</div>--%>
  <%--<a aria-controls="collapseOne" aria-expanded="true" href="#collapseOne" class="checkoutpanel" data-parent="#accordion" data-toggle="collapse">--%>
                    <div id="headingOne" role="tab" class="panel-heading" style="background-color: #0069b2;color:#fff;">
                      <h4 class="panel-title" onclick="OnclickTab('ship')" style="cursor:pointer;"  id="hship" runat="server" visible="true"> 
                       <span class="" id="spanship" runat="server">  Shipping / Delivery Details 
                      <span>
                      <img class="pull-right" src="/images/select-downarrow_wht.png"/>
                      </span></span>
                        </h4>
                    </div>
                   <%-- </a>--%>




<%--id="collapseOne"--%>
<div  class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne"  id= "divship" runat="server">

<div class="panel-body">
 
  
       <asp:PlaceHolder runat="server" ID="PHOrderConfirm" Visible="false" EnableViewState ="false">
                <% 
                    HelperServices objHelperServices = new HelperServices();

                    OrderServices objOrderServices = new OrderServices();
                    //   int OrderIDtmp = 0;
                    //   int Useridtmp;
                    //   Useridtmp = objHelperServices.CI(Session["USER_ID"]);
                    //   if (Useridtmp <= 0)
                    //       Useridtmp = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());
                    //   if (!string.IsNullOrEmpty(Request["OrderID"]))
                    //   {
                    //       OrderID = Convert.ToInt32(Request["OrderID"].ToString());
                    //   }
                    //   else
                    //   {
                    //       OrderID = objOrderServices.GetOrderID(Useridtmp, (int)OrderServices.OrderStatus.OPEN);
                    //   }
                    if (Convert.ToInt16(Session["USER_ROLE"]) == 3)
                    {
                        
                        
                %>
                    <div class="accordion_head_green white_color">                          
                      <h3 style="font-size: 16px;">Order Now Pending Approval</h3>
                       <p class="p2">
                         Your order is now pending approval form your company supervisor/s before it can be submitted to us for processing.
                       <br>
                         The following member/s in your company will be able to authorise and submite your order:
                      <br>
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
                 <% if (objOrderServices.IsNativeCountry(OrderIDaspx) == 0)
                    { %>
                  		<div class="accordion_head_green white_color">
                             <img src="images/checkout_tick.png" class="margin_right" alt=""/>
                       Your order has been successfully submitted to us for processing. Thank You!
                         
                   </div>
                    <%--<div>
                    <img src="images/img_flash/xmas-banner-checkout-01.gif" alt="x-mas" width="100%" height="205px"  />
                    <div style="height:10px;"></div>
                   </div>--%>
                   <%} %>
                   <% if (objOrderServices.IsNativeCountry(OrderIDaspx) == 0)
                      {
                      %>
                       
                   <div class="accordion_head_yellow gray_40" style="background: #FFF200; padding: 12px 17px;" ><strong>Please Note :</strong> As you are purchasing from outside of Australia, We will calculate shipping charges and advise you shortly by email with further details.
                   </div>
                   <%} %>
                   
                    <%-- <div><img width="768" src="/images/website-hoilday-message.gif" alt="holiday message" /> </div>--%>
                <% 
                        Session["ORDER_ID"] = "0";
                        Session["Multipleitems"] = null;
                    } 
                %>
    </asp:PlaceHolder>   


      <div class="accordion_head_grey">Enter your Purchase Order No.</div>

         <div class="col-lg-10">
              <div class="form-group">
                            <label for="inputEmail3" class="col-sm-8 control-label" style="font-weight:normal;">Order No.</label>
                 <div class="col-sm-12">
                  <asp:TextBox  Width="100%" MaxLength="12"  ID="ttOrder" runat="server"  placeholder="Order No" class=" col-sm-10 form-control checkout_input"  />
                  <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
                </div>
                <div class="clear"></div>
              </div>
            </div>
            <div class="accordion_head_grey clear">Shipping</div>
             
              
              

                  <div class="cshipping_dtl">
              <div class="col-lg-10">
                <div class="form-group">
                  <label for="inputEmail3" class="col-sm-8 control-label font_normal" style="font-weight:normal;">Select Shipment Method</label>
                   <div class="col-sm-12">
                      <asp:DropDownList NAME="drpSM1" ID="drpSM1" runat="server" class="form-control checkout_input">
                                            <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method" >Please Select Shipping Method</asp:ListItem>
                                           <asp:ListItem Text="Standard Shipping" Value="Standard Shipping">Standard Shipping</asp:ListItem>
                           <%--              <asp:ListItem Text="Mail" Value="Mail">Mail</asp:ListItem>                                           
                                            <asp:ListItem Text="Courier Pickup" Value="Courier Pickup">Courier Pickup</asp:ListItem>--%>
                                            <asp:ListItem Text="Counter Pickup" Value="Counter Pickup">Shop Counter Pickup</asp:ListItem>
                     </asp:DropDownList>
                  </div>
                  <div class="clear"></div>
                </div>
              </div>
              <div class="col-lg-10">
              <div class="form-group">
                 <label for="inputEmail3" class="control-label" id="Label17">
                   <a href="#" class="checkoutlink" data-target=".lgn-orderinfo" data-toggle="modal" role="button">
                              <img class=" margin_rgt15" src="/images/info.png"/> Learn More About Shipment Method Options</a>


                   <%--<asp:HyperLink ID="HyperLink1" runat="server" onclick="MouseHover();" CssClass="checkoutlink" style="color:#333;" data-target=".bs-example-modal-lg" data-toggle="modal">
                
                  <img src="/images/micrositeimages/ex.png" alt="" class="margin_right" onclick="MouseHover();" style="cursor: pointer;" data-target=".bs-example-modal-lg" data-toggle="modal"/> <span onclick="MouseHover();" style="cursor: pointer;" data-target=".bs-example-modal-lg" data-toggle="modal"> Learn More About Shipment Method Options </span>
                       </asp:HyperLink>--%></label>
                  <div class="clear"></div>
                </div>
              </div>
            </div>

                        <div class="col-lg-20 col-xs-20" id="OtherShipmentRow">
                 <div class="col-lg-10">
                            <div class="form-group">
                   <label for="inputEmail3" class="control-label">Bill To :</label>
                  <div>                   
                     <textarea id="Ta3" cols="34" rows="5"   style="min-height:210px;overflow:hidden;"  class="width_95" readonly="readonly"  disabled="disabled" tabindex="-1" runat="server"    name="Ta3"></textarea>
                  </div>
                  <div class="clear"></div>
                </div>
              </div>
              <div class="col-lg-10">
                            <div class="form-group">
                              <label for="inputEmail3" class="control-label">Delivery To :</label>
                  <div>
         
                      <textarea id="Ta2" cols="34"  style="min-height:210px;overflow:hidden;" class="width_95"   readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="5"  name="Ta2"></textarea>
                  </div>
                  <div class="clear"></div>
                </div>
              </div>
            </div>

            
          
                 <div id="DropShipmentRow" style="display:none">
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
             
                <asp:TextBox ID="TextBox1"  runat="server" Rows="5" Columns="30" Font-Size="12px" CssClass="width_100 resize" Width="100%"  Height="72px" Font-Names="arial"  MaxLength="240"  onkeyDown="return checkMaxLength(this,event,'240');" 
                                TextMode="MultiLine">
                            </asp:TextBox>
              </div>
            </div>


               <div  id="PopDiv" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfo">
                  <div class="modal-dialog custom">
                        <div class="modal-content">
                          <div class="close-selected">
                          
                              <asp:ImageButton ID="ImageButton3" runat="server" 
                                                       src="/Images/close_btn.png"   OnClientClick="MouseOut();" CausesValidation="false" />
                        
                           
                      </div>
                          <div class="modal-header green_bg">
                            <h4 id="H2" class="text-center">
                            <img class="popsucess" alt="img" src="/images/Order__info.png"/>Shipment Method Options</h4>
                               
                              
                          </div>
                          <div class="modal-body">
                              <div class="col-lg-20 col-sm-20">
                              <div class="popuptext">
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
                            <asp:Button ID="btnOk" runat="server" Text="Close" CssClass="btn button-close"  OnClientClick="MouseOut();" CausesValidation="false"  />
                          </div>
                      </div>
                  </div>
                </div>
               <div id="SCP" runat="server" style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfoscp">
               <div class="modal-dialog custom">
                        <div class="modal-content">
                           <div class="close-selected">
                             <a  onclick="CloseSCP();"> <img src="/Images/close_btn.png" alt="" /></a>
                           
                      </div>
                          <div class="modal-header green_bg">
                            <h4 id="H4" class="text-center">
                            <img class="popsucess" alt="img" src="/images/Order__info.png"/>Shop Counter Pickup</h4>
                          </div>
                            

                      </div>
                          <div class="modal-body">
                              <div class="col-lg-20 col-sm-20">
                              <div class="popuptext">
                                  <p style="font-size:12px;">
                               <%--  <b style="font-size:16px;">Courier Service</b>
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
                             <br />--%>
                        
                          
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
              <div class="accordion_head_grey clear">Your Order Contents</div>

            <div class="col-lg-20 col-xs-20">
         <div class="table-responsive col-lg-20 col-sm-20">                                      
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
                       <thead class="checkout_thead">
                    <tr>
                      <th>Order Code</th>
                      <th>Quantity </th>
                      <th>Description </th>
                      <th>Cost(EX.GST)</th>
                      <th>Extention Amount (EX.GST)</th>
                    </tr>
                  </thead><tbody>
                    
                        <%   	                   	     
                            dsOItem = objOrderServices.GetOrderItems(OrderID);

                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);
                    

                            UserServices.UserInfo oOrdBillInfo1 = objUserServices.GetUserBillInfo(Userid);
                            UserServices.UserInfo oOrdShippInfo1 = objUserServices.GetUserShipInfo(Userid);


                            string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                            decimal ProdShippCost = 0.00M;
                            decimal TotalShipCost = 0.00M;
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
                                      
                        %>

                          <tr>
                    <td>   <% Response.Write(rItem["CATALOG_ITEM_NO"].ToString()); %></td>
                    <td>            <% Response.Write(rItem["QTY"].ToString()); %> </td>
                    <td>       <% Response.Write(rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;"));%></td>
                    <td>     <% Response.Write(CurSymbol + " " + ProductUnitPrice.ToString("#,#0.00")); %></td>
                    <td>  <% Response.Write(CurSymbol + " " + Amt.ToString("#,#0.00")); %></td>
                  </tr>
                       
                        <%  
                               i = i + 1;
                                       
                                        
                                    } 
                                  
                                }  
                            } 
                          
                        %>
                          <tr class="border_none">
                   <td colspan="3"></td>
                    <td> <strong>Sub Total </strong></td>
                    <td>        <strong>       <%    Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);        %> </strong></td>
                  </tr >
                    <tr id="corierhandcrgeR" style="display:none;" class="border_none">
                  
                   <td colspan="3"></td>
                    <td> <strong>  <%  
                           
                                Response.Write("Delivery (Ex GST)");                               
                                 %>
                                 </strong> </td>
                    <td>   <strong>           <%
                                         decimal ShippingValue = 0;
                                         if (objHelperServices.GetOptionValues("COURIER CHARGE") != "")
                                             ShippingValue = Convert.ToDecimal(objHelperServices.GetOptionValues("COURIER CHARGE").ToString());
                                         Response.Write(CurSymbol + " " + ShippingValue);      
                            
                        %> </strong> </td>
                  </tr>
                  <%
                           
                      if (objOrderServices.IsNativeCountry(OrderID) == 0)
                      {
                                %>
                        <tr class="border_none">
                  <td colspan="3"></td>
                    <td> <strong> <%  
                      if (objOrderServices.IsNativeCountry(OrderID) == 0)
                          Response.Write("Shipping Charge");
                      else
                          Response.Write("Delivery (Ex GST)");
                                 %>
                                 </strong></td>
                    <td>        <strong>      <%
                      if (objOrderServices.IsNativeCountry(OrderID) == 0)
                          Response.Write("To Be Advised");
                      else
                          Response.Write(CurSymbol + " " + oOrderInfo.ShipCost);         
                            
                        %> </strong> </td>
                  </tr>
                        <%} %>
                    <tr class="border_none">
                  <td colspan="3"></td>
                    <td>  <strong>     Total Tax Amount (GST) </strong></td>
                    <td>    <strong>          <span style="display:none;" id="TaxAmount">
                                <%       
                                  
                                    decimal ICtaxamt = 0.00M;
                                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                        Response.Write(CurSymbol + " " + ICtaxamt);
                                    else
                                    {
                                        decimal totamtanor = oOrderInfo.ProdTotalPrice;
                                        decimal taxa_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTaxa_nor = objHelperServices.CDEC(totamtanor * (taxa_nor / 100));
                                        // totamtTax = RetTax_WC + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        RetTaxa_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTaxa_nor));
                                       // Response.Write(cSymbol + " " + oOrderInfo.TaxAmount);
                                        Response.Write(CurSymbol + " " + RetTaxa_nor);
                                    }
                                    
                                    %></span>

                                  <span style="display:none;" id="taxamtcoupickup">
                                   <%   decimal totamtTaxCP =  oOrderInfo.ProdTotalPrice;
                                        decimal tax_CP = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_CP = objHelperServices.CDEC(totamtTaxCP * (tax_CP / 100));
                                       // totamtTax = RetTax_WC + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        RetTax_CP = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_CP));
                                        Response.Write(CurSymbol + " " + RetTax_CP); 
                                    %>
                                  </span>

                                  <span style="display:none;" id="taxamtwithcouriercrge">
                                       <%   decimal totamtTax = ShippingValue + oOrderInfo.ProdTotalPrice;
                                        decimal tax_WC = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_WC = objHelperServices.CDEC(totamtTax * (tax_WC / 100));
                                       // totamtTax = RetTax_WC + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        RetTax_WC = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_WC));
                                        Response.Write(CurSymbol + " " + RetTax_WC); 
                                    %></span> </strong> </td>
                  </tr>
         <tr  class="border_none blue_color_text">
                 <td colspan="3"></td>
                    <td>  <strong>      <%
                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
                        {                                        
                            %>                                        
                               Est. Total 
                            <%
                        }
                        else
                        {
                                %> Est. Total Inc GST
                       <%
                        } %>
                       
                      </strong> </td>
                    <td>          <strong id="ICtotalamount" style="display:none;">
                                  <% Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);    
                                    %>
                                </strong>
                                <strong id="totalamt" style="display:none;">
                                    <%
                                        
                                        
                                        decimal totnor = oOrderInfo.ProdTotalPrice;
                                        decimal tax_nor = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_nor = objHelperServices.CDEC(totnor * (tax_nor / 100));
                                        RetTax_nor = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_nor));
                                        //=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Total))
                                        decimal totamtnor = RetTax_nor + oOrderInfo.ProdTotalPrice;
                                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                            Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);    
                                        else
                                            Response.Write(CurSymbol + " " + totamtnor); 
                                        %>
                                </strong>
                                   <strong id="totalcoupickup" style="display:none;">
                                    <%
                                        
                                        decimal totcp = oOrderInfo.ProdTotalPrice;
                                        decimal tax_tcp = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax_tcp = objHelperServices.CDEC(totcp * (tax_CP / 100));
                                        // totamtTax = RetTax_WC + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        RetTax_tcp = objHelperServices.CDEC(objHelperServices.FixDecPlace(RetTax_tcp));


                                        decimal totcoupu = RetTax_tcp + oOrderInfo.ProdTotalPrice;
                                        totcoupu = objHelperServices.CDEC(objHelperServices.FixDecPlace(totcoupu));
                                        Response.Write(CurSymbol + " " + totcoupu);    
                                        %>
                                </strong>
                                    <strong id="totamtwithcouriercrge" style="display:none;">
                                    <%
                                       // decimal tocouamt = oOrderInfo.TaxAmount + ShippingValue + oOrderInfo.ProdTotalPrice;
                                        decimal tocouamt = 0.00M; tocouamt = ShippingValue + oOrderInfo.ProdTotalPrice;
                                        decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                                        decimal RetTax = objHelperServices.CDEC(tocouamt * (tax / 100));
                                        tocouamt = RetTax + ShippingValue + oOrderInfo.ProdTotalPrice;
                                       tocouamt = objHelperServices.CDEC(objHelperServices.FixDecPlace(tocouamt));
                                       Response.Write(CurSymbol + " " + tocouamt);    
                                        %>
                                </strong></td>
                  </tr>
 </tbody></table>




                 

                 </asp:Panel>
                  <asp:Panel ID="PnlOrderInvoice" runat="server" Visible="false">
                   <uc1:OrderDet ID="OrderDet2" runat="server" />
                   </asp:Panel>
                    <div class="col-lg-20 text-right checkout_btn">
                        <asp:Button ID="ImageButton1" runat="server" Text="Edit/Update Order"  OnClick="ImageButton1_Click" class="btn btn-primary"/>
                        <asp:Button runat="server" ID="ImageButton2" Text="Continue" class="btn primary-btn-green"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />                         
                    </div>
           
              </div>
              </div>
          


           
   
       
    <%--                                          <asp:DropDownList Visible="false"  ID="drpShipState" runat="server" Width ="230px" Class="DropdownlistSkin"  >   </asp:DropDownList>
    --%>
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
                        <td colspan="100%" background="images/17.gif" class="TableRowHead">
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
                        <td class="TableRowHead" background="images/17.gif">
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
   <%-- <div id="PopupOrderMsg" align="center" runat ="server">
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
                  
                    </td>
                    <td width="30%">
                         <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="button normalsiz btnblue" OnClick="btnForgotPassword_Click" />
                    </td>
                    <td width="35%" align="left">
                      
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>   --%>

     <div id="PopupOrderMsg" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in">
    <div class="modal-dialog modal-lg">
    <div class="modal-content">

        <div class="modal-header blue_color padding_top padding_btm">

     <%--     <button aria-label="Close" data-dismiss="modal" class="close white_bg" type="button">
          <span aria-hidden="true">×</span></button>--%>
          <h4 id="H1" class=" white_color font_weight modal-title">Your Account Has Not Been Activated! </h4>
         
        </div>
        <div class="modal-body">

     
         <p>    Account Activation Required before you can proceed to check out.
                        <br />
                        Please check your email for account activation link as this was emailed to you when you registered your account.
                        <br /> <a Href="/mConfirmMessage.aspx?Result=REMAILACTIVATION" style="color:#15c;" >Please Click Here</a></p>
        
<div class="modal-footer">
       <%--  <asp:Button ID="btnCancel" runat="server" Text="Close"   Width="205px" class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100" OnClick="btnCancel_Click" />--%>
         <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100" OnClick="btnForgotPassword_Click" />
         </div>
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
    <%--<div id="PopupMsg">
                    <asp:Panel ID="Panelerroritems" runat="server" Style="display: none" BackColor="White"
                        Height="50px" Width="650px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#b81212">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
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
                                  
                                </td>
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>--%>
                    

                <div id="PopupMsg" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: block;" class="modal fade bs-example-modal-lg in">
  <div class="modal-dialog  margin_top_popup20 ">
    <div class="modal-content  border_radius_none">

        <div class="modal-body">
         <div class="close-selected">

                <asp:ImageButton ID="btnclose" runat="server" 
                                                       src="/Images/close_btn.png"   OnClick="btnCancelerroritems_Click" />
                        
                           
                      </div>
         <p class="alert-red">Please review and correct Order Clarification / Errors before proceeding to Check Out!  </p>
        </div>
        <div class="modal-footer">
         <%--<asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="btn-lg padding_top btn-danger border_none border_radius_none white_color semi_bold font_14 mar_right_30  mob_100 margin_left" OnClick="btnCancel_Click" />--%>
           <asp:Button ID="btnCancelerroritems" runat="server" Text="Close" CssClass="btn-lg padding_top btn-danger border_none border_radius_none white_color semi_bold font_14 mar_right_30  mob_100 margin_left" OnClick="btnCancelerroritems_Click" />
        </div>
      </div>
  </div>
</div>   

</div>

</div>

</div>
   

 <div class="panel panel-default" id="liPayOption" runat="server">
     <div id="headingtwo" role="tab" class="panel-heading" style="background-color: #0069b2;color:#fff;">
          <h4 class="panel-title"    onclick="OnclickTab('Pay')"  style="cursor:pointer;" id= "h3Pay"  runat="server"> 
              <span class="collapsed"   id="spanPay"  runat="server">
           Payment Options 
           <span > <img class="pull-right" src="/images/select-downarrow_wht.png"/></span> </span>
           </h4>
             <h4 class="panel-title"    id= "h3Pay1"  runat="server"> <span class="collapsed"  >Payment Options </span></h4>
        </div>

       <%-- id="collapseTwo" --%>
       <div runat="server" id="Paydiv" >
          <div class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo"  id= "divPay" runat="server">

            
		  <div class="panel-body">
			<div runat="server" id="divCC" >
             <%-- <div class="accordion_head margin_bottom_30">Payment Type
              <div class="clear"></div>
            </div>--%>
            <div class="accordion_head_grey clear">Payment Type</div>
                <div runat ="server" id="div2" class="accordion_head_yellow gray_40" style="font-size:12px;text-align:center; font-weight:bold;margin-bottom:12px;background: #FFF200; padding: 12px 17px;" >
                </div>
                 <div class="cl"></div>
                <div class="col-lg-20" runat ="server" id="div1"> 
             <%  
                 OrderServices objOrderServices = new OrderServices(); 
                 
                 if (objOrderServices.IsNativeCountry(OrderID) == 1)
               {                                        
                            %> 

                               <div runat="server" id="PayType">
 <div class="form-group dblock">
      <%--             <label class="checkbox-inline">   
         <asp:LinkButton ID="LinkButton3" runat="server" OnClick="btnPayPalAPIPayLink_Click" CausesValidation="false" ><label>                
                                <asp:Image ID="ImagePayApi" runat="server" ImageUrl="images/express_Checkout_select.png"  style="margin-top: -5px;cursor: pointer;" alt="cc"/>
                                </label></asp:LinkButton></label>--%>
                                <% 
                                     SecurePayService objSecurePayService = new SecurePayService();
                                     if (objSecurePayService.CheckSecurePay() == true)
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

              
              
            </div>
      

       
          
            </div>
            <%} %>            
            
                               <div runat="server" id="Div5">

                               <div runat="server" id="PaypalApiDiv">
 <div class="form-group dblock">     
                
               <%-- <label class="checkbox-inline">
              
                <img src="/images/micrositeimages/master_card.png" alt=""> </label>
                <label class="checkbox-inline margin_left_mob_none">
               
                <img src="/images/micrositeimages/american_express.png" alt=""> </label>
                <label class="checkbox-inline margin_left_mob_none">
               
                <img src="/images/micrositeimages/visa.png" alt=""> </label>
                <label class="checkbox-inline margin_left_mob_none">
               
                <img class="mob_mar_top" src="/images/micrositeimages/pay_check.png" alt=""> </label>--%>

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

                    <p class="blue_text margin_top15"><strong>Pay using your Credit Card or Paypal Account</strong></p>
              <p class="margin_bott15">You will be redirected to paypal website to complete payment transaction</p>
                   <div class="col-sm-20 nolftpadd">
                    <div class="form-group col-lg-8 nolftpadd">
                     <h3 class="green_clr">Total Amount $  <asp:Label runat="server" ID="lblpaypaltotamt" CssClass="totalamt"  /> 
                    </h3>           
                    </div>
                </div>


                   <asp:Button runat="server" ID="btnPay" Text="Pay Now" class="btn btn-primary "  OnClick="btnPay_Click" OnClientClick="Setinit(this.id)"   />       

                    <asp:Button runat="server" ID="btnPayApi" Text="Pay Now" style="width:100px;" class="btn btn-primary " Visible="false"  OnClick="btnPayApi_Click" OnClientClick="Setinit(this.id)" />       


                 
                 <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="btn btn-primary " Enabled="false"   />       
                 
                   <div class="cl"></div>
                   <%} %>
                   <div id="divContent" style="font-size:12px margin-left:30px;color:Red" runat="server"></div>
                      
            </div>
         </div>
           <div runat="server" id="PaySPDiv">
      
    
              


           <asp:DropDownList ID="drppaymentmethod" runat="server" width="200px" CssClass="cardinput" onchange="Controlvalidate('dd')"   Visible =false />     
           
            <%-- <div class="form-col-2-8">
             <asp:Label runat="server" ID="lblpaymentmethod" style="font-size:12px;" >Card Type &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>    
             </div>
             <div class="form-col-2-8">
              
             
             </div>
             <div class="form-col-2-8">
              
               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  InitialValue="0"
                    ControlToValidate="drppaymentmethod" Display="Dynamic" CssClass="error-text" ErrorMessage="Select Card Type" 
                    ></asp:RequiredFieldValidator>     
             </div>
             <div class="cl"></div>--%>
            <div class="col-sm-20 nolftpadd">
            <div class="form-group col-lg-20 nolftpadd">
                <div class="col-lg-8 nolftpadd">
                    <label>
                    Name on Card   
                    <span class="required">*</span>
                    </label>
                 <%--<asp:Label runat="server" ID="Label9" style="font-size:16px;" >Name on Card &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>--%>
                    <asp:TextBox runat="server" ID="txtCardName"  CssClass="form-control checkout_input"    MaxLength="50"  OnBlur="Controlvalidate('cn')"  />
                    </div>
                <div class="col-lg-8">
                <p class="mandatory checkouterror"><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="txtCardName" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Name on Card"></asp:RequiredFieldValidator>
                    </p>
                </div>
                </div>
                </div>
       

              
                 <div class="col-sm-20 nolftpadd">
<div class="form-group col-lg-20 nolftpadd">
               <div class="col-lg-8 nolftpadd">
               <label>Card Number &nbsp;&nbsp;<span class="required">*</span></label>
                <%--<asp:Label runat="server" ID="lblcardnumber" style="font-size:16px;"   >Card Number &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>--%>
               <asp:TextBox runat="server" ID="txtCardNumber" CssClass="form-control checkout_input"    MaxLength="19" OnBlur="Controlvalidate('cno')" />
                </div>
              <div class="col-lg-8">
                <p class="mandatory checkouterror">
               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="txtCardNumber" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Card Number"  ></asp:RequiredFieldValidator>
                    
                    <asp:customvalidator id="CustomValidator1"   Display="Dynamic"  CssClass="error-text" ClientValidationFunction="ValidCC" errormessage="Please Check Credit Card Number" controltovalidate="txtCardNumber" runat="server">
	             </asp:customvalidator>
                 </p>
              </div>
              </div>
              </div>
              
            
           
                 <div class="col-sm-20 nolftpadd clearfix">
<div class="form-group col-lg-4 col-sm-20 nolftpadd">
                <div class="cvv-left" >
                <label>CVV &nbsp;&nbsp;<span class="required">*</span></label>
                <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="100px"  CssClass="form-control" MaxLength="4" OnBlur="Controlvalidate('cvv')"/>    
                <%--<div class="cvv-left">
                  
                 <asp:Label runat="server" ID="lblcardcvvnumber" style="font-size:16px;"  >CVV Number &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                  
                 </div>--%>
                 </div>
                 <div class="cvv-right">
                    <span class="mandatory checkouterror">
                       <a href="#" class="checkoutlink" data-target=".bs-example-modal-lg1" data-toggle="modal" role="button">
                              <img class=" margin_rgt15" src="/images/question-icon.png"/></a>

                 <%--  <asp:HyperLink ID="HyperLink2" runat="server" onclick="MouseOverCCpopClick();" CssClass="question" style="color:#333; " data-target=".bs-example-modal-lg1" data-toggle="modal">
                
                  <img src="/images/question-icon.png" alt="" class="margin_right" onclick="MouseOverCCpopClick();" style="cursor: pointer;" data-target=".bs-example-modal-lg1" data-toggle="modal"/>
                        </asp:HyperLink>--%></span>
                    </div>
                    
                
                </div>
                <div class="col-lg-8">
                <p class="mandatory checkouterror">
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                    ControlToValidate="txtCardCVVNumber" Display="Dynamic" CssClass="error-text"  style="float: right;"
                    ErrorMessage="Enter Card Security Code<Br/>"></asp:RequiredFieldValidator>                    
                          </p>

             <%--
                    <asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup"  BehaviorID="testTACpopup"
    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None">
</asp:ModalPopupExtender>--%>
               </div>
          
              </div>
                
                
      

            <div class="col-sm-20 nolftpadd clearfix">
<div class="form-group col-lg-20 nolftpadd">
                <div class="col-lg-3 col-xs-10 nolftpadd" >
                     <label>Card Expiry &nbsp;&nbsp;<span class="required">*</span></label>
                   <%--<asp:Label runat="server" ID="lblexpirationdate"  style="font-size:16px;" >Card Expiry &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>--%>
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
                    <div class="col-lg-3 col-xs-10" >
                     <label> &nbsp;&nbsp;<span class="required">&nbsp;&nbsp;</span></label>
                <asp:DropDownList NAME="drpExpyear"  ID="drpExpyear" runat="server" CssClass="form-control checkout_input">          
                    </asp:DropDownList>
                    </div>


              <div class="col-lg-8">
              <p class="mandatory checkouterror">
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                    ControlToValidate="drpExpmonth" Display="Dynamic" CssClass="error-text" 
                    ErrorMessage="Select Month"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
                    ControlToValidate="drpExpyear" Display="Dynamic" CssClass="error-text" 
                    ErrorMessage="Select Year"></asp:RequiredFieldValidator>
                    </p>
                </div>
                </div>
                </div>
               
                   <div class="col-sm-20 nolftpadd">
                    <div class="form-group col-lg-8 nolftpadd">
                        <h3 class="green_clr">Total Amount $ <asp:Label runat="server" ID="lblAmount" CssClass="totalamt"  /> </h3>                    
                    
                    </div>
                </div>
            
             

            <div class="cl"></div>
              <div class="col-sm-20 nolftpadd margin_btm_30">
<div class="form-group col-lg-8 nolftpadd">
                <asp:Button runat="server" ID="btnSP"  Text="Pay Now" class="btn btn-primary" OnClick="btnSecurePay_Click" OnClientClick="javascript:SetinitSP()" />       

                  <asp:Button runat="server" ID="BtnProgressSP" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="btn btn-primary" Enabled="false"   />     
                 
                 
                  
                    <div id="div6" style="font-size:12px margin-left:30px;color:Red" runat="server"  ></div>
            </div>
            </div>
          
            <div class="cl"></div>
          </div>



       
          
            </div>
                <div class="accordion_head_grey clear">Shipping & Order Details</div>
               <div class="col-lg-10">
<div class="form-group">
<label class="col-sm-20 control-label" for="inputEmail3"><strong>Bill To</strong></label>
               <div class="col-sm-20">
<ul class="list-group">
                    <li class="list-group-item" style="background-color:#fff;">  <asp:Label ID="txtpaybill" runat="server" Text="Label" ></asp:Label></li>                
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
            </div>
               
                    <div class="accordion_head_grey clear">Order Contents</div>
               <div class="col-lg-20 col-xs-20">
<div class="table-responsive col-lg-20 col-sm-20">                                           
                     <uc1:OrderDet ID="OrderDet1" runat="server" />
                     <div class="col-lg-20 text-right">
                  
                       <asp:Button ID="ImgBtnEditShipping" runat="server" Text="Edit / Update Order"  style="width:auto !important" CssClass="btn btn-primary" OnClick="ImgBtnEditShipping_Click"  CausesValidation="false" />
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
<div class="panel panel-default" id="liFinalReview" runat="server">
 <div class="panel-heading tab_blue_color" role="tab" style="background-color: #0069b2;color:#fff;">
 <h4 class="panel-title"  onclick="OnclickTab('paid')"  style="cursor:pointer;"  id= "hpaid"  runat="server" > 
    <span  class="collapsed"  >
  Completed<span  id="spanpaid"  runat="server">
  <img src="/images/select-downarrow_wht.png" class="pull-right" alt=""/></span> </span>
   </h4>

    <h4 class="panel-title"   id= "hpaid1"  runat="server" >  <span  class="collapsed"  > Completed </span> </h4>
  </div>
		<%--  id="collapseThree"--%>
        <div runat="server" id="paiddiv">
             <div  class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree"  id= "divpaid" runat="server" >
          <div class="panel-body" >
		<div runat="server" id="PayOkDiv">
                   <%-- <img src="images/checkout_tick.png" class="margin_right" style="float:left;" alt=""/>--%>
                  <div id="divOk" runat="server"  class="accordion_head_green clear">
                     </div>
                       <%--<div>
                    <img src="images/img_flash/xmas-banner-checkout-01.gif" alt="x-mas" width="100%" height="205px"  />
                    <div style="height:10px;"></div>
                   </div>--%>
                       
                </div>
                
                 <div class="col-lg-10">
<div class="form-group">
<label class="col-sm-20 control-label" for="inputEmail3"><strong>Order Details</strong></label>
               <div class="col-sm-20">
<ul class="list-group">
                   <li class="list-group-item" style="background-color:#fff;height:100px;">
                   Order No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <asp:Label ID="lblOrderNo" runat="server" Text="" CssClass="LabelStyle" style="font-weight:normal;" ></asp:Label>
                   </br>
                     Shipping Method : <asp:Label ID="lblShippingMethod" runat="server" Text="" CssClass="LabelStyle" style="font-weight:normal;" ></asp:Label>
                   </li>
                   
                  </ul>
                  
                </div>
                <div class="clear"></div>
              </div>
            </div>
              <div class="col-lg-10">
<div class="form-group">
<label class="col-sm-20 control-label" for="inputEmail3"><strong>Payment Information</strong></label>
         <div class="col-sm-20">
<ul class="list-group">
                   <li class="list-group-item" style="height:100px;">
                             <div runat ="server" id="div3">
                                <div id="div4"  runat="server" >
                                 <div id="divError" runat="server" style="color:Red;" >
                                    </div>

                                    <div id="divlink" runat="server" >
                                    </div>
                                </div>
                           
                                         </div> 
                     </li>
                   
                  </ul>
                  
                                  
                </div>
                <div class="clear"></div>
              </div>
            </div>
              
             <div class="col-lg-10">
                <div class="form-group">
                <label class="col-sm-12 control-label" for="inputEmail3"><strong>Bill To</strong></label>
               <div class="col-sm-20">
                    <ul class="list-group">
                   <li class="list-group-item" style="background-color:#fff;">
                   <asp:Label ID="txtPaidbill" runat="server" Text="Label"  ></asp:Label>
                   </li>
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
                   <li class="list-group-item" style="background-color:#fff;">
                    <asp:Label ID="txtPaidship" runat="server" Text="Label"  ></asp:Label>
                    </li>
                    </ul>
                </div>
                <div class="clear"></div>
              </div>
            </div>
            <div class="accordion_head_grey clear">Order Contents</div>
                 <div class="col-lg-20 col-xs-20">
<div class="table-responsive col-lg-20 col-sm-20">                                                     
                  <uc1:OrderDet ID="OrderDet3" runat="server" />
                   
                  </div>
		        </div>
		  </div>
		</div>
        </div>
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
 <%--  <div id="CCPopDiv" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6); display: none;" class="modal fade bs-example-modal-lg1 in">
   

  <div class="modal-dialog  margin_top_popup20 modal-lg" style="margin-top:5%;width:500px;">
    <div class="modal-content  border_radius_none">

        
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
        <div class="modal-footer">
          <asp:Button ID="Button1" runat="server" Text="Close" CssClass="btn btn-primary"  OnClientClick="MouseOutCCpopClick();"   CausesValidation="false"/>
         
        </div>
      </div>
  </div>
</div>  --%>
    
       
</asp:Content>

