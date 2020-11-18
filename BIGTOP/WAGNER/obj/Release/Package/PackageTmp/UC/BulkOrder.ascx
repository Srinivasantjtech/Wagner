<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_BulkOrder" Codebehind="BulkOrder.ascx.cs" %>
<%--<%@ Import Namespace="TradingBell.Common" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Import Namespace ="System.Data.OleDb" %> 
<%@ Import Namespace ="System.IO" %>  
<%@ Import Namespace ="System.Data" %>
<input id="HidItemCode1" name="HidItemCode1" clientidmode="Static" type="hidden" class="autosuggest"
    runat="server" value="" />
<input id="HidQty1" name="HidQty1" clientidmode="Static" type="hidden" runat="server" />
<input id="HidTarget" name="HidTarget" clientidmode="Static" type="hidden" runat="server" />
<input id="HidtxtCnt" name="HidtxtCnt" clientidmode="Static" type="hidden" runat="server" />
<input id="Hidtxtbulk" name="Hidtxtbulk" clientidmode="Static" type="hidden" runat="server" />
<input id="HidItemCode2" name="HidItemCode2" clientidmode="Static" type="hidden" runat="server" />
<input id="HidQty2" name="HidQty2" clientidmode="Static" type="hidden" runat="server" />
     
    <style type="text/css" >
      .test
      {
          display:block;
      }
      .element.style
      {
          display:none;
      }
      .ui-helper-hidden-accessible 
      { 
          display:none; 
      }
    </style>
     <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
   <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script>  --%>

<%--<link rel="Stylesheet" href="css/jquery-ui.css" type="text/css" />  --%>
<script src="scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
<script src="scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    var integer_only_warned = false;
    function integeronly(obj) {
        var value_entered = obj.value;
        if (!integer_only_warned) {
            if (value_entered.indexOf(".") > -1) {
                alert('Please enter an integer only. No decimal places.');
                integer_only_warned = true;
                obj.value = value_entered.replace(/[^0-9]/g, '');
            }
        }
        obj.value = value_entered.replace(/[^0-9]/g, '');
        
    }

    function dotransfer(eventTarget, eventArgument1, eventArgument2, eventArgument3) {
        document.getElementById("HidTarget").value = eventTarget;
        document.getElementById("HidItemCode1").value = eventArgument1;
        document.getElementById("HidQty1").value = eventArgument2;
        document.getElementById("HidtxtCnt").value = eventArgument3;
        //document.forms[0].submit();
    }

    function NoEnter() {
        if (window.event.keyCode == 13) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        }
    }

    function ResetDisabledControls() {
        var tb = document.getElementById("tblitembox");
        txtcnt = tb.rows.length - 1;
        for (i = 1; i <= txtcnt; i++) {
            document.forms[0].elements["txtitem" + i].disabled = false;
            document.forms[0].elements["txtqty" + i].disabled = false;
        }
        window.document.getElementById("ctl00_maincontent_ctl00_lnkbtnmore").disabled = false;
        document.getElementById("ctl00_maincontent_ctl00_txtCopyPaste").disabled = false;
    }

    function linkbtnclear() {
        //document.getElementById("HidItemCode1").value = "";
        //document.getElementById("HidQty1").value = "";
        var _isQtyAlert = true;
        var _isItemAlert = true;
        var i = 0;
        var tempqty = "";
        var tempitem = "";
        var txtcnt = 27;
        var chk = 1;
        var tb = document.getElementById("tblitembox");
        var txtcnt = "<%=intTxtCount%>";
        txtcnt = tb.rows.length - 1;
        for (i = 1; i <= txtcnt; i++) {
            if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value != "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                tempqty = tempqty + document.forms[0].elements["txtqty" + i].value + ",";
                tempitem = tempitem + document.forms[0].elements["txtitem" + i].value + ",";
            }
        }
        document.forms[0].elements["<%=HidItemCode2.ClientID%>"].value = tempitem;
        document.forms[0].elements["<%=HidQty2.ClientID%>"].value = tempqty;
  
    }

    function DisableLineEntry() {
        var tb = document.getElementById("tblitembox");
        txtcnt = tb.rows.length - 1;
        for (i = 1; i <= txtcnt; i++) {
            document.forms[0].elements["txtitem" + i].disabled = true;
            document.forms[0].elements["txtqty" + i].disabled = true;
        }

        //document.forms[0].elements["ctl00_maincontent_ctl00_lnkbtnmore"].disabled = -1;
        document.forms[0].elements["ctl00_maincontent_ctl00_lnkbtnmore"].href = "#";
        //window.document.getElementById("ctl00_maincontent_ctl00_lnkbtnmore").disabled = -1;
        //window.document.getElementById("ctl00_maincontent_ctl00_lnkbtnmore").href = "#";

    }

    function DisableBulkEntry() {
        //window.document.getElementById("ctl00_maincontent_ctl00_txtCopyPaste").disabled = true;
    }

    function dispmsg() {
        var _isQtyAlert = true;
        var _isItemAlert = true;
        var i = 0;
        var tempqty = "";
        var tempitem = "";
        var txtcnt = 27;
        var chk = 1;
        var tb = document.getElementById("tblitembox");
        var txtcnt = "<%=intTxtCount%>";
        txtcnt = tb.rows.length - 1;
        for (i = 1; i <= txtcnt; i++) {
            if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value != "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                tempqty = tempqty + document.forms[0].elements["txtqty" + i].value + ",";
                tempitem = tempitem + document.forms[0].elements["txtitem" + i].value + ",";
            }
            else if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                if (document.forms[0].elements["txtqty" + i].value.length == 0 || document.forms[0].elements["txtqty" + i].value == null || document.forms[0].elements["txtqty" + i].value == "0" || document.forms[0].elements["txtitem" + i].value.indexOf(',') != "-1") {
                    if (_isQtyAlert == true || _isQtyAlert == 'true') {
                        alert('Qty cannot be empty');
                        _isQtyAlert = false;
                    }

                    window.document.getElementById("txtitem" + i).style.borderColor = "red";
                    window.document.getElementById("txtqty" + i).style.borderColor = "red";
                    window.document.getElementById("txtqty" + i).value = "";
                    window.document.getElementById("txtqty" + i).focus();
                    return false;
                }
            }
            else if (document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value != "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                if (document.forms[0].elements["txtitem" + i].value.length == 0 || document.forms[0].elements["txtitem" + i].value == 'Item#' || document.forms[0].elements["txtitem" + i].value == null || document.forms[0].elements["txtitem" + i].value.indexOf(',') != "-1") {
                    if (_isItemAlert == true || _isItemAlert == 'true') {
                        alert('Item# cannot be empty');
                        _isItemAlert = false;
                    }

                    window.document.getElementById("txtitem" + i).style.borderColor = "red";
                    window.document.getElementById("txtqty" + i).style.borderColor = "red";
                    window.document.getElementById("txtitem" + i).value = "";
                    window.document.getElementById("txtitem" + i).focus();
                    return false;
                }
            }
            if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value == "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                chk = 0;
            }
        }

        for (i = 1; i <= txtcnt; i++) {
            if (document.forms[0].elements["txtitem" + i].value != 'Item#' && ((document.forms[0].elements["txtqty" + i].value == '') || (document.forms[0].elements["txtqty" + i].value <= 0))) {
                window.document.getElementById("txtitem" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).value = "";
                return false;
            }

            if (document.forms[0].elements["txtqty" + i].value > 0 && document.forms[0].elements["txtitem" + i].value == 'Item#') {
                window.document.getElementById("txtitem" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).style.borderColor = "red";
                return false;
            }

            if ((document.forms[0].elements["txtitem" + i].value != '' || document.forms[0].elements["txtitem" + i].value != null || document.forms[0].elements["txtitem" + i].length > 0) && (document.forms[0].elements["txtqty" + i].value > 0)) {
                window.document.getElementById("txtitem" + i).style.borderColor = "ActiveBorder";
                window.document.forms[0].elements["txtqty" + i].style.borderColor = "ActiveBorder";
            }

        }

        //    document.forms[0].elements["<%=HidItemCode1.ClientID%>"].value = tempitem;
        //    document.forms[0].elements["<%=HidQty1.ClientID%>"].value = tempqty;
        //    document.forms[0].elements["<%=HidtxtCnt.ClientID%>"].value = txtcnt * 2;
        if (chk == 0) {
            alert('Item# and Qty cannot be empty');
            return false;
        }
        else if (tempitem == "" || tempqty == "") {
            var result = checkbulk();
            if (!result) {
                alert("Item# and Qty cannot be empty !");
                document.forms[0].elements["HidItemCode1"].value = "";
                document.forms[0].elements["HidQty1"].value = "";
            }
            return result;
            //document.forms[0].elements["<%=HidtxtCnt.ClientID %>"].value = txtcnt * 2
        }
        else {
            dotransfer('BULKORDER', tempitem, tempqty, txtcnt);
            return true;

        }

    }


    function checkbulk() {
        var str = document.forms[0].elements["<%=txtCopyPaste.ClientID %>"].value;
        var i = 0;
        var chk = 0;
        var _ItemStatus = true;

        var myattr = new Array();
        if (str.length <= 0) {
            _ItemStatus = false;
            //return false;
        }

        myattr = str.split("\n");
        for (i = 0; i < myattr.length; i++) {
            if (myattr[i].length > 0) {
                myattr[i] = myattr[i].toString().replace("\t", " ");
                var myattr1 = new Array();
                if (myattr[i].toString().indexOf(",") >= 0) {
                    myattr1 = myattr[i].split(",");
                }
                else if (myattr[i].toString().indexOf(" ") >= 0) {
                    while (myattr[i].toString().indexOf("  ") > 0) {
                        myattr[i] = myattr[i].toString().replace("  ", " ");
                    }
                    myattr1 = myattr[i].split(" ");
                }
                else {
                    chk = 1;
                    break;
                }
                if (myattr1[1].length <= 0) {
                    alert("Qty cannot be empty !");
                    _ItemStatus = false;
                    //return false;
                }

                if (myattr1[0].length <= 0) {
                    alert("Item# cannot be empty !");
                    _ItemStatus = false;
                    //return false;
                }
                if (myattr1.length != 2) {
                    chk = 1;
                    break;
                }
            }
        }

        //alert(document.forms[0].elements["txtCopyPaste"].value);

        if (chk == 1) {
            alert("Qty cannot be empty !");
            _ItemStatus = false;
            //return false;
        }
        //    else
        //    {
        //        document.forms[0].elements["<%=Hidtxtbulk.ClientID%>"].value=document.forms[0].elements["<%=txtCopyPaste.ClientID %>"].value;
        //        window.document.forms[0].submit();
        //    }

        return _ItemStatus;
        //return true;
    }
    function FillValue(ctl) {

        if (ctl.value == '' || ctl.value == null) {
            ctl.value = 'Item#';
        }
    }
    function Focus(ctl) {
        SearchText(ctl);
        if (ctl.value == 'Item#') {
            ctl.value = '';
        }
        var txtbxID = ctl;
        var txtid = txtbxID.id;
        if (ctl.value != '' || ctl.value != null || ctl.value != 'Item#') {
            if (txtid == 'txtitem1') {
                $(".autosuggest").focusout(function (e) {
                    $('ul.ui-autocomplete').removeClass('autosuggest').addClass('ui-helper-hidden-accessible ');
                });

                $(".inputbackground").focus(function () {
                    $('ul.ui-autocomplete').removeAttr('style').hide();
                });
            }
        }


    }
    function Focus1(ctl) {
        var txtbxID = ctl;
        var txtid = txtbxID.id;
        if (ctl.value != '' || ctl.value != null || ctl.value != 'Item#') {
            $(".autosuggest").focusin(function (e) {
                $('ul.ui-autocomplete').removeClass('ui-helper-hidden-accessible').addClass('test');

            });
        }

        if (ctl.value != '' || ctl.value != null || ctl.value != 'Item#') {
            $(".inputbackground").focusin(function (e) {
                $('ul.ui-autocomplete').removeClass('test').addClass('ui-helper-hidden-accessible ');
                $('ul.ui-autocomplete').removeAttr('style').hide();
                $('autosuggest').removeAttr('test').hide();
                $('.autosuggest').removeClass("ui-autocomplete-loading");
                $('.autosuggest').data = "";
                $('.autosuggest').autocomplete('close');
                $('ui-autocomplete ui-menu ui-widget ui-widget-content ui-corner-all').removeAttr('style').hide();
                $('.ui-menu-item').removeAttr('style').hide();
                $('.ui-corner-all').removeAttr('style').hide();
                $('.ui-active-menuitem').removeAttr('style').hide();
                $('.ui-helper-hidden-accessible').removeAttr('style').hide();
                $('.ui-menu-item').hide();
                $('.autosuggest').autocomplete("search", "");
                $(".ui-autocomplete").css({ "display": "none" });
                if (!e) e = window.event;
                if (e.keyCode == '9') {
                    $('.autosuggest').autocomplete('close');
                    $('.autosuggest').autocomplete("search", "");
                    return false;
                }


            });
        }
        function HideAutoCompleteHack() {
            $(".ui-autocomplete").hide();
        }
    }
    function Check(Id) {

        var Qty = window.document.getElementById("txtqty" + Id).value;  //window.document.forms(0).elements("txtqty" + Id).value;
        var Code = window.document.forms(0).elements("txtitem" + Id).value;
        if ((isNaN(Qty) && Code != "Item#") || (Qty <= 0 && Code != "Item#") || (Qty.indexOf(".") != -1 && Code != "Item#") || (Qty == "" && Code != "Item#")) {
            alert('Invalid Quantity!');
            window.document.getElementById("txtitem" + Id).style.borderColor = "red";
            window.document.forms[0].elements["txtqty" + Id].style.borderColor = "red";
            window.document.getElementById("txtqty" + Id).value = "";
            window.document.getElementById("txtqty" + Id).focus();
            return false;
        }
        if (Code == '' || Code == 'Item#' || Code.length == 0 || Code == null) {
            alert('Invalid Item!');
            window.document.forms[0].elements["txtitem" + Id].style.borderColor = "red";
            window.document.forms[0].elements["txtqty" + Id].style.borderColor = "red";
            window.document.getElementById("txtitem" + Id).value = "Item#";
            window.document.getElementById("txtitem" + Id).focus();
            return false;
        }
        if ((Code != '' || Code != null || Code.lenth > 0) && (Qty > 0)) {
            window.document.getElementById("txtitem" + Id).style.borderColor = "ActiveBorder";
            window.document.forms[0].elements["txtqty" + Id].style.borderColor = "ActiveBorder";
        }
        if (document.getElementById("txtqty" + Id).focus == true) {
            alert("hshkdashk");
          
        }

    }
    function GetValues() {
        for (var i = 1; i <= 10; i++) {
            alert(document.forms[0].elements["txtQty" + i].value);
        }
    }
    function Here() {
        alert('this');
    }
</script>
   <script type="text/javascript">
       $(document).ready(function () {
               });
       function SearchText(ctl) {
              $(".autosuggest").autocomplete({source: function (request, response) {
                   $.ajax({
                       type: "POST",
                       contentType: "application/json; charset=utf-8",
                       url: "BulkOrder.aspx/WestestAutoCompleteData",
                       data: "{'strvalue':'" + ctl.value + "'}",
                       dataType: "json",
                       success: function (data) {
                           $(".ui-autocomplete").css({ "display": "none" });
                           $('.autosuggest').removeClass("ui-autocomplete-loading");
                           $('.autosuggest').data = "";
                           $('.autosuggest').autocomplete('close');
                           $(".ui-autocomplete").hide();
                           $('.ui-autocomplete').css({ "display": "none" });
                           $("body").click(function () {
                               HideAutoCompleteHack();
                           });
                           $(document).keyup(function (e) {
                               if (e.keyCode == 9) { //esc
                                   HideAutoCompleteHack();
                                   $('.ui-autocomplete').autocomplete('close');

                               }
                           });
                           function HideAutoCompleteHack() {
                               $(".ui-autocomplete").hide();
                           }
                           $(".inputbackground").focus(function () {
                               $('ul.ui-autocomplete').removeAttr('style').hide();
                           });
                           $(document).ready(function () {
                               if ($.browser.msie) {
                                   HideAutoCompleteHack();
                                   $(".inputbackground").focus(function () {
                                       $('ul.ui-autocomplete').removeAttr('style').hide();
                                   });
                               }
                               else if ($.browser.mozilla) {
                                   $('ul.ui-autocomplete').removeAttr('style').hide();
                               }

                           });
                           response(data.d);
                           var txtbxID = ctl;
                           var txtid = txtbxID.id;
                           window.document.getElementById(txtid).setAttribute('class', 'autosuggest');
                       },
                       error: function (result) {
                           alert("Error");
                       }
                   });
               }
           });

       }
	</script>
   
<%
    if (Request.Url.ToString().ToLower().Contains("orderdetails.aspx"))
    {
%>
<%--<table align="center" width="785" border="0" cellspacing="0" cellpadding="5">
    <tr>
        <td align="left" class="style7" style="font-size: 11px; color: #0099DA;" width="100%">
            Add Items to Cart - Quick Order
        </td>
    </tr>
    <tr>
        <td class="tx_3">
        </td>
    </tr>
</table>--%>
<% 
    }
    else if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
    {
%>
<table align="center" width="785" border="0" cellspacing="0" >
    <tr>
        <td align="left" >

            <div class="breadcrumb_outer1">
             <a href="/Home.aspx" style="float: left" class="toplinkatest" style="text-decoration:none!important;" >HOME >&nbsp;</a>
             <div class="breadcrumb1"> 
  <a href="OrderTemplate.aspx"  class="breadcrumb_txt1" style="text-transform:none;"> ORDER TEMPLATE</a>
  <a href="/Home.aspx" class="breadcrumb_close1" >    
  </a>
</div>
</div>
</td>
</tr>
</table>
<%}
    else 
    {
%>
<table align="center" width="785" border="0" cellspacing="0" >
    <tr>
        <td align="left" >

            <div class="breadcrumb_outer1">
             <a href="/Home.aspx" style="float: left" class="toplinkatest" style="text-decoration:none!important;" >HOME >&nbsp;</a>
             <div class="breadcrumb1"> 
  <a href="BulkOrder.aspx?txtcnt=27"  class="breadcrumb_txt1" style="text-transform:none;"> Quick Order</a>
  <a href="/Home.aspx" class="breadcrumb_close1" >    
  </a>
</div>
</div>
</td>
</tr>
</table>
  <%}  %>


<%
    if (Request.Url.ToString().ToLower().Contains("orderdetails.aspx"))
    {
%>
        <div class="box1" style="width:745px;">
       <h4 class="title3" style="text-align:left;">Add Addtional Items to Cart - Quick Order</h4>
<% }
    else if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
    {        
         %>
         <div class="box1" style="width:750px;">
<h4 class="title3" style="text-align:left;">Order Template</h4>
         <%
    }         
     else
    {        
         %>
         <div class="box1" style="width:750px;">
<h4 class="title3" style="text-align:left;">Quick Order</h4>
         <%
    }
         %>
<P class=p3>Enter part numbers using either <STRONG>line entry fields</STRONG>, <STRONG>bulk entry field</STRONG>, or <STRONG>excel file upload</STRONG>.</P>
<%--<table border="0"  cellspacing="1" cellpadding="1" style="background: #000; margin-left: 25px;
    width: 770px">
    <tr>
        <td style="background: #fff">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td width="95%">--%>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td  align="left">
                                <div class="quickorder1" style="margin-right: 4%;">
                                <h3 class="title1" style="text-align:left;">Product Line Entry
                                                </h3>
                                                <p class="p2">Please TAB Key to quickly jump to next field for faster entry </p>
                                                 <div class="QOordercode">
                                                <p class="p2"><strong>Order Code</strong></p>
                                              </div>
                                              <div class="QOqty">
                                                <p class="p2"><strong>Qty</strong></p>
                                              </div>

                                <%--    <table cellpadding="0" cellspacing="0" border="0" >                                        
                                        <tr>
                                            <td >--%>
                                                <%
                                                    HelperServices objHelperServices = new HelperServices();
                                                    StringBuilder oStrCtrls = new StringBuilder();
                                                    
                                                  
                                                  
                                                   
                                                    
                                                    int i = 1;
                                                    int txtCount = 10;
                                                    if (HidtxtCnt.Value.ToString() != "")
                                                    {
                                                        txtCount = objHelperServices.CI(HidtxtCnt.Value.ToString());

                                                        if (txtCount > 50)
                                                        {
                                                            txtCount = 50;
                                                        }
                                                    }
                                                    DataSet ds = new DataSet();
                                                    if (HttpContext.Current.Session["fileData"] != null)
                                                    {                                                           
                                                          ds=(DataSet) HttpContext.Current.Session["fileData"];
                                                          txtCount = ds.Tables[0].Rows.Count;                                                  
                                                    }
                                                    DataSet tmpds = new DataSet();
                                                    
                                                    if (HttpContext.Current.Session["linkmoredata"] != null)
                                                    {
                                                        tmpds = (DataSet)HttpContext.Current.Session["linkmoredata"];
                                                        txtCount = Convert.ToInt16(HttpContext.Current.Session["linkmoredatatxtcount"]);

                                                    }
                                                    DataSet dstemplate = new DataSet();
                                                    if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
                                                    {
                                                          OrderServices  objOrderService = new OrderServices();
                                                          int userid = objHelperServices.CI(HttpContext.Current.Session["USER_ID"]);
                                                          dstemplate = objOrderService.GetOrderTemplateItem(userid);
                                                        
                                                    }
                                                    oStrCtrls.Append("<table id=\"tblitembox\" cellpadding=\"3\" cellspacing=\"10\" border=\"0\" style=\"vertical-align:top; \">");
                                                    //oStrCtrls.Append("<tr  class=\"tx_4table\"><td valign=\"top\"><strong>Order Code</strong></td><td><strong>Qty</strong></td></tr>");
                                                    
                                                    
                                                        for (i = 1; i <= txtCount; i++)
                                                        {
                                                              string _code = "Item#";
                                                              string _qty = "";    
                                                        
                                                             if (ds!=null && ds.Tables.Count>0 )
                                                            {
                                                                if (ds.Tables[0].Rows.Count>=i)
                                                                {
                                                                 _code =ds.Tables[0].Rows[i-1][0].ToString();
                                                                 _qty = ds.Tables[0].Rows[i-1][1].ToString();    
                                                                }
                                                            }

                                                             if (tmpds != null && tmpds.Tables.Count > 0)
                                                             {
                                                                 if (tmpds.Tables[0].Rows.Count >= i)
                                                                 {
                                                                     _code = tmpds.Tables[0].Rows[i - 1][0].ToString();
                                                                     _qty = tmpds.Tables[0].Rows[i - 1][1].ToString();
                                                                 }
                                                             }
                                                             if (dstemplate != null && dstemplate.Tables.Count > 0)
                                                             {
                                                                 if (dstemplate.Tables[0].Rows.Count >= i)
                                                                 {
                                                                     _code = dstemplate.Tables[0].Rows[i - 1]["Pcode"].ToString();
                                                                     _qty = dstemplate.Tables[0].Rows[i - 1]["Qty"].ToString();
                                                                 }
                                                             }
                                                             //oStrCtrls.Append(System.Environment.NewLine + "<tr><td><input type=\"text\" class=\"autosuggest\"  name=\"itembox\" id=\"txtitem" + i + "\" style=\"width:200px\"  class=\"inputbackground\" value=\"" + _code + "\" onblur=\"FillValue(txtitem" + i + ")\" onfocus=\"Focus(txtitem" + i + ")\"></td><td><input type=\"text\" value=\"" + _qty + "\" name=\"qtybox\" id=\"txtqty" + i + "\" class=\"inputbackground\" onkeyup=\"integeronly(this)\" style=\"width:50px\" runat=\"server\" onBlur=\"javascript:return Check(" + i + ");\" onfocus=\"Focus1(txtqty" + i + ")\"></td></tr>" + System.Environment.NewLine);
                                                             oStrCtrls.Append(System.Environment.NewLine + "<tr><td width=\"74%\">");
                                                             oStrCtrls.Append("<input type=\"text\" class=\"autosuggest\"  name=\"itembox\" id=\"txtitem" + i + "\" style=\"width:94%\"  class=\"inputbackground\" value=\"" + _code + "\" onblur=\"FillValue(txtitem" + i + ")\" onfocus=\"Focus(txtitem" + i + ")\">");
                                                             oStrCtrls.Append("</td>");
                                                             oStrCtrls.Append("<td>");
                                                             oStrCtrls.Append("<input type=\"text\" value=\"" + _qty + "\" name=\"qtybox\" id=\"txtqty" + i + "\" class=\"input_dr\" onkeyup=\"integeronly(this)\" style=\"width:90%\" runat=\"server\" onBlur=\"javascript:return Check(" + i + ");\" onfocus=\"Focus1(txtqty" + i + ")\">");
                                                            oStrCtrls.Append("</tr>");
                                                            
                                                        }

                                                    oStrCtrls.Append("</table>");
                                                    Response.Write(oStrCtrls.ToString());
                                                %>
                                           <%-- </td>
                                        </tr>
                                    </table>--%>
                                    <div align="left">
                                     <asp:LinkButton ID="lnkbtnmore" runat="server" Style="font-size: 11px;
                                        " OnClientClick="linkbtnclear();"  class="toplinkatest" OnClick="lnkbtnmore_Click">+ Add more fields</asp:LinkButton>
                                       <%-- <%  if (!Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
                                            { %>
                                        <br /><asp:LinkButton ID="lnkBtnLoadTemplate" runat="server" Style="font-size: 11px;
                                        " OnClientClick="linkbtnclear();"  class="toplinkatest" OnClick="lnkBtnLoadTemplate_Click">+ Load from Template</asp:LinkButton>
                                        <%} %>--%>
                                        </div>
                                    </div>
                                
                                    <div class="quickorder1" >
                                    <h3 class="title1" style="text-align:left;">Product Bulk Entry</h3>
                                     <p class="p2">Copy & paste your order from your file into the space below or type in manually with one item per line. Code No. first followed by the required Qty, seperated by a space or comma.</p>
                                     <p>
                                     <textarea id="txtCopyPaste" name="txtCopyPaste" rows="0" cols="38" runat="server" style="width:320px; height:280px" ></textarea>
                                     </p>                                  
                                    </div>
                                    
                                    
                             
                                    <br/>
                                </td>
                            </tr>
                            <tr>                            
                            <td align="left">
                            <br />
                                   <div class="quickorder3">
                                    <H3 class="title1">Order File Upload</H3>                                    
                                    <P class="p2">Upload your excel file for quick order. Enter the Order Code in column "A" and quantity in column "B".</P>
                                    <p class="pad10" style="padding:0px 0px;">  
                                      <asp:LinkButton id="LinkButton2" 
                                                Text="Click Here to Download example Excel upload order sheet" 
                                                 OnClick="LinkButton_Click"  runat="server" class="toplinkatest" > 
                                                 <div class="toplinkatestbulk"><p style="margin: 0 0 0 21px;width:282px;">Click Here to Download example Excel upload order sheet</p></div>     
                                                       <%-- <div class="toplinkatestbulk"><p style="margin: 0 0 0 21px;width:282px;">Click Here to Download example Excel upload order sheet</p></div>                     --%>
                                                </asp:LinkButton>
                                    </p>
                                    <div>
                                    <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="inputtxtfile"   /> 
                                    <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" class="inputupload" style="height:25px;margin-left:0px;border:1px solid #465C71;font-family:Arial;"/>
                                    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
                                    </div>
                                    </div>
                            </td>
                            </tr>


                           <%--  <tr>
                                <td >            
                                
                                   
                                        <p align="left" Style="font-family: Arial; font-size: 11px; margin:0; color:#00AFFF;">
                                       
                                       <img src="images/Icon_Excel.png" alt="" />
                                        <asp:LinkButton id="LinkButton1"  
                                                Text="Click Here to Download example Excel upload order sheet" 
                                                Font-Names="Arial" Font-Size="11px"  OnClick="LinkButton_Click"  runat="server" 
                                                ForeColor="#00AFFF"/>
                                         
                                       </p> 
                                       <br />                                           
                                    
                                </td>
                                                               
                            </tr>
                          --%>
                         
                            <tr>
                            <td  align="left">
                                <asp:RegularExpressionValidator  id="RegularExpressionValidator1" runat="server"  ErrorMessage="Invalid File Type.Only xls, xlsx or csv files are allowed!"  ValidationExpression="^.+(.xls|.XLS|.xlsx|.XLSX|.csv|.CSV)$"   ControlToValidate="FileUploadControl"></asp:RegularExpressionValidator>
                            
                               </td>
                            </tr>
                           

                            <tr>
                              
                                <td align="center" width="100%">
                                        <%
                                            if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
                                            { %>
                                                                                               
                                                      <asp:Button ID="Button2" Text="Save Template" runat="server" 
                                                     OnClientClick="return dispmsg();" class="button normalsiz btngreen btnmain" />
                                          <% }
                                            else
                                            { %>
                                             <asp:Button ID="btnAddCart1" Text="Add to Cart" runat="server" 
                                                     OnClientClick="return dispmsg();" class="button normalsiz btngreen btnmain" />
                                            <%} %>
                                                <asp:Button ID="btnResetCart" Text="Reset" runat="server" Style="width: 100%; height: 40px;"
                                                    OnClick="btnResetCart_Click" Visible="false" />
                                    
                                </td>
                              
                            </tr>
                        </table>
              
</div>
