<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="MyAccount"  Culture="en-US"
    UICulture="en-US" Codebehind="MyAccount.aspx.cs" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
   
<style>
.dpynone
{
    display:none;
}
.dpyblk
{
    display:block;
}
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
   <%-- <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/jquery-migrate-1.0.0.js"></script>--%>
  <%--<script type="text/javascript" src="/Scripts/jquery-1.10.2.min.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>"  ></script>--%>
    <script language="javascript" type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/jquery-ui-1.8.13.custom.min.js"></script>
    <%--<link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/jquery-ui-1.8.14.custom.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" rel="stylesheet" type="text/css" />
    <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/progress.js" type="text/javascript"></script>--%>

    <script language="javascript" type="text/javascript">
        var Flag = false;

        $(document).ready(function msg() {

            $("ProcessDiv").hide();

            if (Flag == false) {

            }
            $(".HyperLink2").click(function () {

                if (Flag == true) {
                    $("ProcessDiv").show();


                }
            });
        });

        function ShowDiv(obj) {
            var dataDiv = document.getElementById(obj);
            dataDiv.style.display = "block";
            alert("h2");
        }

        function countdown_clear() {
            clearTimeout(countdown);
        }


        function ShowFileMessage() {
            alert('Invalid Specification!');
        }

        function showalert(Url) {
            Flag = confirm('Do you want to proceed the download?');
            if (Flag == true) {
                //alert(Url);
                window.location.href = "OrderHistory.aspx?Key=" + Url;
            }
            return flag;
        }

        function ShowFailureMessage() {
            alert('Invoice currently Unavailable. Please try again later');
        }

        function ShowLoader() {
            setTimeout("setImage();", 500);
            //$.hideprogress();
            window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'none';
            //document.getElementById('ctl00_maincontent_DivLoader').style.visibility = 'hidden';
            //document.getElementById('ctl00_maincontent_LoadPanel').style.visibility = 'hidden';
            alert("h2");
        }

        function setImage() {
            //$.showprogress();
            window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'block';
        }

        function PopupOrder(url) {
            newwindow = window.open(url, 'name', 'scrollbars=1,left=130,top=50,height=530,width=700');
            if (window.focus) {
                newwindow.focus();
            }
            return false;
        }

        function payonlinepopup_open(url) {

            // var hrefroot = 'payonline.aspx?OrdId=';
            // var url = hrefroot + url;
            // alert(url);


            //                var hrefroot = 'payonline.aspx?OrdId=';
            //                var hrefURL = hrefroot + url;
            //                popupWindow = window.open(hrefURL, "_blank", "directories=no, status=no, menubar=no, scrollbars=yes, resizable=no,width=700, height=530,top=50,left=130");
            //                if (window.focus) {
            //                    popupWindow.focus();
            //                }
            //                return false

        }





        function initinvoicereq() {
            var ret = confirm("Do you want to proceed")
            return ret;
        }
        $(function () {
            $("#<%=FromdateTextBox.ClientID %>").removeClass();
            $("#<%=FromdateTextBox.ClientID %>").datepicker({

                beforeShow: function (input, inst) {
                    var d1 = new Date();
                    d1 = d1.format('MM/dd/yyyy');
                    var sub = d1.split('/');
                    var mon = sub[0] - 1;
                    var Newdate = new Date(mon + "/" + sub[1] + "/" + sub[2]);
                    Newdate = Newdate.format('dd/MM/yyyy');
                    //alert(Newdate);
                    if (document.getElementById("ctl00_maincontent_FromdateTextBox").value == null) {
                        document.getElementById("ctl00_maincontent_FromdateTextBox").value = Newdate;
                    }
                },
                defaultDate: '-1m',
                maxDate: "+0M +0D",
                dateFormat: 'dd/mm/yy',
                // showOn: "button",                            
                // buttonImage: "/images/Calendar2.PNG",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true
            });
            $("#<%=TodateTextBox.ClientID %>").removeClass();
            $("#<%=FromdateTextBox.ClientID %>").addClass("form-control checkout_input");
            $("#<%=TodateTextBox.ClientID %>").removeClass();
            $("#<%=TodateTextBox.ClientID %>").datepicker({
                maxDate: "+0M +0D",
                dateFormat: 'dd/mm/yy',
                //showOn: "button",
                // buttonImage: "/images/Calendar2.PNG",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true
            });
            $("#<%=TodateTextBox.ClientID %>").removeClass();
            $("#<%=TodateTextBox.ClientID %>").addClass("form-control checkout_input");
        });
    </script>
    <script language="javascript" type="text/javascript">
        function Focus() {
            //alert('dd');
            SearchText1();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //alert("sdasdas");
            // SearchText('txtitem1');

        });
        function SearchText() {

            $('a.HyperLink2').click({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "OrderHistory.aspx/WestestAutoCompleteData",
                        //  data: "{'username':'" + document.getElementById(ctl).value + "'}",
                        data: "{'strvalue':'A'}",
                        dataType: "json",
                        success: function (data) {
                            //alert("success");
                            response(data.d);
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                }
            });
        }
        function SearchText1() {
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "OrderHistory.aspx/WestestAutoCompleteData",
                //  data: "{'username':'" + document.getElementById(ctl).value + "'}",
                data: "{'strvalue':'A'}",
                dataType: "json",
                success: function (data) {
                    //alert("success");
                    response(data.d);
                },
                error: function (result) {
                    alert("Error");
                }
            });
        }
        function asyncServerCall(userid) {
            jQuery.ajax({
                url: 'WebForm1.aspx/GetData',
                type: "POST",
                data: "{'userid':" + userid + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alert(data.d);
                }

            });
        }

	</script>
    <script type="text/javascript" language="javascript">
        function toggle(inv, dis) {
            var e = document.getElementById(inv);
            e.style.display = dis;
        }
        function GetInvoice(OrderInv, inv) {

            toggle(inv, "")
            PageMethods.SendInvoiceSignal(OrderInv, OnSuccess, OnFailure);
        }
        function OnSuccess(result) {
            if (result.indexOf("inv") != -1) {
                var invno = result.replace("inv", "");
                document.getElementById('<%=ImageButton1.ClientID %>').onclick = null;
                document.getElementById('<%=ImageButton1.ClientID %>').click();
                toggle(invno, "none")
            }
            else {
                var invno1 = result.replace("inv", "");
                toggle(invno1, "none")
                alert("Invoice PDF temopary not Available.Please try again later");
            }

        }
        function OnFailure(error) {
            var invno = result.replace("inv", "");
            toggle(invno, "none")
            alert("Invoice PDF temopary not Available.Please try again later");
            //alert(error);

        }

        $(document).ready(function () {
            $("#myTab li:first").addClass("active").show();
            $(".tab-content:first").show();

            $("#myTab li").click(function () {
                $("#myTab li").removeClass("active");
                $(".tab-content div").removeClass("active");
                $(this).addClass("active");
                var activeTab = $(this).find("a").attr("href");
                // $(activeTab).addClass("active").show();
                $(activeTab).addClass("active")
                return false;
            });
        });

</script>
<%--<script type="text/javascript">

    $(document).ready(function () {
        if ($("#<%=tabcontrolHiddenField.ClientID%>").val().toLowerCase() == "false") {
            if ($("#ctl00_MainContent_lblError").text() == "Invalid old password") {
                $("#profile").removeClass();
                $("#profile").addClass("tab-pane");
                $("#messages").removeClass();
                $("#messages").addClass("tab-pane active");
                $("#settings").removeClass();
                $("#settings").addClass("tab-pane");
                $("#home").removeClass();
                $("#home").addClass("tab-pane");

                $("#OH").removeClass();
                $("#OH").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
                $("#VP").removeClass();
                $("#VP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
                $("#CP").removeClass();
                $("#CP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob active");
                $("#CL").removeClass();
                $("#CL").addClass("col-xs-12 col-lg-3 col-sm-3 border_right_none padding_left_right_mob");
                var dataDiv = document.getElementById("profile");

                document.getElementById("profile").className = "tab-pane";
                dataDiv.classList.add('tab-pane');

                document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
                document.getElementById("CP").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active";

            }
            if ($("#<%=tabcontrolprofileHiddenField.ClientID%>").val().toLowerCase() == "true") {
                $("#messages").removeClass();
                $("#profile").removeClass();
                $("#settings").removeClass();
                $("#home").removeClass();
                $("#profile").addClass("tab-pane");
                $("#messages").addClass("tab-pane");
                $("#settings").addClass("tab-pane");
                $("#home").addClass("tab-pane active");
                $("#OH").removeClass();
                $("#OH").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob"); 
                $("#VP").removeClass();
                $("#VP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active");
                $("#CP").removeClass();
                $("#CP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
                $("#CL").removeClass();
                $("#CL").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
                var dataDiv = document.getElementById("profile");

                document.getElementById("profile").className = "tab-pane";
                dataDiv.classList.add('tab-pane');

                document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
                document.getElementById("VP").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active";
            }
        }

        if ($("#<%=tabcontrolprosuccessField.ClientID%>").val().toLowerCase() == "true") {
            $("#prosucmsg").removeClass();
            $("#prosucmsg").addClass("alert alert-success border_radius_none text-center dpyblk");
            $("#divprofiledata").removeClass();
            $("#divprofiledata").addClass("dpynone");
            $("#messages").removeClass();
            $("#profile").removeClass();
            $("#settings").removeClass();
            $("#home").removeClass();
            $("#profile").addClass("tab-pane");
            $("#messages").addClass("tab-pane");
            $("#settings").addClass("tab-pane");
            $("#home").addClass("tab-pane active");
            $("#OH").removeClass();
            $("#OH").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
            $("#VP").removeClass();
            $("#VP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active");
            $("#CP").removeClass();
            $("#CP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
            $("#CL").removeClass();
            $("#CL").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
            var dataDiv = document.getElementById("profile");

            document.getElementById("profile").className = "tab-pane";
            dataDiv.classList.add('tab-pane');

            document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
            document.getElementById("VP").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active";
        }
        else {
            $("#prosucmsg").removeClass();
            $("#prosucmsg").addClass("dpynone");
          //  $("#divprofiledata").removeClass();
          //  $("#divprofiledata").addClass("col-lg-9 margin_top_20");
        }


    });

    function showprofiletab()
    {
        var dataDiv = document.getElementById("profile");

        document.getElementById("profile").className = "tab-pane";
        dataDiv.classList.add('tab-pane');

        document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
        document.getElementById("VP").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active";
     
    }
    </script>--%>
 
    <script type="text/javascript">    $(document).ready(function () {
        if ($("#<%=tabcontrolHiddenField.ClientID%>").val().toLowerCase() == "false") {
            //alert("before if");

            // if ($("#ctl00_MainContent_lblError").text() == "Invalid old password")
            if (document.getElementById("ctl00_maincontent_lblError").innerHTML == "Invalid old password") {
                document.getElementById("prosucmsg").style.display = "none";

                $("#prosucmsg").removeClass();
                $("#profile").removeClass();
                $("#profile").addClass("tab-pane");
                $("#messages").removeClass();
                $("#messages").addClass("tab-pane active");
                $("#settings").removeClass();
                $("#settings").addClass("tab-pane");
                $("#home").removeClass();
                $("#home").addClass("tab-pane");
                $("#OH").removeClass();
              //  $("#OH").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
                $("#VP").removeClass();
              //  $("#VP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
                $("#CP").removeClass();
      //          $("#CP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob active");               //commented due to the design issue
                $("#CL").removeClass();
              //  $("#CL").addClass("col-xs-12  col-sm-3 border_right_none padding_left_right_mob");

                //                var dataDiv = document.getElementById("profile");

                document.getElementById("profile").className = "tab-pane";
                document.getElementById("VP").className = "padding_left_right_mob";
                document.getElementById("CP").className = "padding_left_right_mob";
                document.getElementById("CL").className = "padding_left_right_mob";
                document.getElementById("OH").className = "padding_left_right_mob";
                //document.getElementById("VP").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
                //document.getElementById("CL").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
                //document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
            }

            else if (document.getElementById("ctl00_maincontent_lblErrorusername").innerHTML == "User name should not contain `WES` or `CELL` or `WAG`,try with different user name") {

                document.getElementById("profile").className = "tab-pane";
                document.getElementById("VP").className = "padding_left_right_mob";
                document.getElementById("CP").className = "padding_left_right_mob";
                document.getElementById("OH").className = "padding_left_right_mob";
                //document.getElementById("VP").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
                //document.getElementById("CP").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
                //document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";

                $("#CL").addClass("col-xs-12 col-lg-3 col-sm-3 border_right_none padding_left_right_mob active");
                $("#settings").removeClass();
                $("#settings").addClass("tab-pane active");
            }
            else if ($("#<%=tabcontrolprofileHiddenField.ClientID%>").val().toLowerCase() == "true") {
                $("#messages").removeClass();
                $("#profile").removeClass();
                $("#settings").removeClass();
                $("#home").removeClass();
                $("#profile").addClass("tab-pane");
                $("#messages").addClass("tab-pane");
                $("#settings").addClass("tab-pane");
                $("#home").addClass("tab-pane active");
                $("#OH").removeClass();
                //$("#OH").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
                //$("#VP").removeClass();
                //$("#VP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active");
                //$("#CP").removeClass();
                //$("#CP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
                //$("#CL").removeClass();
                //$("#CL").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
                $("#OH").addClass("padding_left_right_mob");
                $("#VP").removeClass();
                $("#VP").addClass("padding_left_right_mob active");
                $("#CP").removeClass();
                $("#CP").addClass("padding_left_right_mob");
                $("#CL").removeClass();
                $("#CL").addClass("padding_left_right_mob");
                var dataDiv = document.getElementById("profile");

                document.getElementById("profile").className = "tab-pane";
                dataDiv.classList.add('tab-pane');
                //document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
                document.getElementById("OH").className = "padding_left_right_mob";
            }
    }

        if ($("#<%=tabcontrolprosuccessField.ClientID%>").val().toLowerCase() == "true") {

            $("#prosucmsg").removeClass();
            $("#prosucmsg").addClass("alert alert-success border_radius_none text-center dpyblk");
            $("#divprofiledata").removeClass();

            $("#divprofiledata").addClass("dpynone");

            $("#messages").removeClass();

            $("#profile").removeClass();

            $("#settings").removeClass();
            $("#home").removeClass();
            var dataDiv = document.getElementById("profile");

            document.getElementById("profile").className = "tab-pane";
            dataDiv.classList.add('tab-pane');
            $("#profile").addClass("tab-pane");
            $("#messages").addClass("tab-pane");
            $("#settings").addClass("tab-pane");
            $("#home").addClass("tab-pane active");
            $("#OH").removeClass();
            //document.getElementById("OH").className = "col-xs-10 col-sm-10 col-md-5 padding_left_right_mob";
            //$("#OH").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
            //$("#VP").removeClass();
            //$("#VP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob active");
            //$("#CP").removeClass();
            //$("#CP").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");
            //$("#CL").removeClass();
            //$("#CL").addClass("col-xs-10 col-sm-10 col-md-5 padding_left_right_mob");


            document.getElementById("OH").className = "padding_left_right_mob";
            $("#OH").addClass("padding_left_right_mob");
            $("#VP").removeClass();
            $("#VP").addClass("padding_left_right_mob active");
            $("#CP").removeClass();
            $("#CP").addClass("padding_left_right_mob");
            $("#CL").removeClass();
            $("#CL").addClass("padding_left_right_mob");
        }
        else {
            $("#prosucmsg").removeClass();
            $("#prosucmsg").addClass("dpynone");

            //  $("#divprofiledata").removeClass();
            //  $("#divprofiledata").addClass("col-lg-9 margin_top_20");
        }
    });</script> 

     <script type="text/javascript"> 
         function Validate(txt) {

             if (!txt.value.match(/^[a-zA-Z0-9?=.,:*!@#$%^&*_\-\s]+$/)) {


                 var lastChar = txt.value[txt.value.length - 1];


                 if (!lastChar.match(/^[a-zA-Z0-9?=.,:*!@#$%^&*_\-\s]+$/)) {

                     if (!txt.value.match("/")) {
                         txt.value = txt.value.replace(lastChar, '');
                         if (!txt.value.match(/^[a-zA-Z0-9?=.,:*!@#$%^&*_\-\s]+$/)) {
                             alert("Invalid Text ': " + txt.value + " '");
                             txt.value = '';

                         }
                         else {

                             alert("Invalid Text : ' " + lastChar + " '");
                         }

                     }




                 }
                 else {

                     if (!txt.value.match("/")) {
                         alert("Invalid Text ': " + txt.value + " '");
                         txt.value = '';
                     }


                 }


             }


         }

         </script>
<div class="container">
<div class="row">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">MyAccount</li>
            </ul>
        </div>
    </div>
     <%

        if (Session["USER_ID"].ToString() != null && Session["USER_ID"].ToString() != "")
        {
    %>
    <div class="row">
      <div class="col-md-20">
        <div class="row">
        <h4>My Account</h4>
        <div class="row product-info orderdetails">
           <div class="myacount">
              <div class="tabs">
               <ul id="myTab" class="nav nav-tabs nav-stacked col-md-4 account_tab clearfix">
                                <li class="padding_left_right_mob active" id="OH"><a href="#profile">
                                <div class="order_history"></div>
                                Order History</a></li>
                                <li class="padding_left_right_mob" id="VP"><a href="#home">
                                <div class="view_profile"></div>
                                View Profile</a></li>
                                <li class="padding_left_right_mob" id="CP"><a href="#messages">
                                <div class="change_pass"></div>
                                Change Password</a></li>
                                <li class="padding_left_right_mob" id="CL"><a href="#settings">
                                 <div class="login_name"></div>
                                 Change Login Name</a></li>
                            </ul>
           <%--    <div class="clearfix"></div>--%>
               <div class="tab-content col-md-16" style="border-left:1px solid #ccc;" >
                 <div id="profile" class="tab-pane active">
                      <div class="col-lg-20  margin_top_20">
                                   <%--   <form>--%>
                                        <div class="form-group col-lg-10">
                                          <label class="font_normal" for="exampleInputEmail1">Order No or Invoice No </label>
                                        <%--  <input type="email" class="form-control checkout_input" id="exampleInputEmail1"/>--%>
                                          <asp:TextBox runat="server" ID="OrderNo"   CssClass="form-control checkout_input" ></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-10">
                                          <label class="font_normal" for="exampleInputEmail1">From Date </label>
                                          <%--<input type="email" data-beatpicker="true" class="form-control checkout_input" id=""/>--%>
                                          <asp:TextBox runat="server" ID="FromdateTextBox"  CssClass="form-control checkout_input datepickerwidth"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-10">
                                          <label class="font_normal" for="exampleInputEmail1">To Date </label>
                                         <%-- <input type="email" data-beatpicker="true" class="form-control checkout_input " id=""/>--%>
                                           <asp:TextBox runat="server" ID="TodateTextBox"  CssClass="form-control checkout_input datepickerwidth" ></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-10">
                                          <label class="font_normal" for="exampleInputEmail1">Created User </label>
                                         <%-- <select class="form-control checkout_input">
                                            <option>select</option>
                                            <option>1</option>
                                          </select>--%>
                                           <asp:DropDownList runat="server" ID="CreatedUserDropDownlist"  CssClass="form-control checkout_input">
                                           </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-lg-20">
                                          <%--<input type="submit" value="Submit" id="myaccount-btn" class="btn btn-primary"/>
                                          <input type="reset" value="Reset" class="btn btn-primary"/>--%>
                                          <asp:Button runat="server" ID="SearchButton" UseSubmitBehavior="true" CausesValidation="false"  CssClass="btn btn-primary"  Text="Search" OnClick="SearchButton_Click" />
                                           <asp:Button runat="server" ID="ResetButton" UseSubmitBehavior="true" CausesValidation="false" CssClass="btn btn-primary"  Text="Reset"  OnClick="ResetButton_Click" /> 
                                         <%--  <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButtonOrdhis" />--%>
                                            
                                        </div>

                                <%--      </form>--%>
                                      <div class="table-responsive col-lg-20 inner-table">
                                         
                                      <% 
                    try
                    {
                        HelperServices objHelperServices = new HelperServices();
                        Security objSecurity = new Security();
                        
                        OrderServices objOrderServices = new OrderServices();
                        DataTable oDt = null;
                        //int Companyid = Convert.ToInt16(Session["COMPANY_ID"]);
                        int Companyid = Convert.ToInt32(Session["COMPANY_ID"]);

                        if (OrderNoHiddenField.Value.Trim() != "" || FromDateHiddenField.Value.Trim() != "" || ToDateHiddenField.Value.Trim() != "" || UserHiddenField.Value.Trim() != "")
                        {
                            DateTime Fromdate1 = new DateTime();
                            DateTime Todate1 = new DateTime();

                            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US", true);

                            if (FromDateHiddenField.Value.Trim() != "")
                            {
                                // Fromdate1 = DateTime.Parse(FromDateHiddenField.Value.ToString(), culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            }
                            else
                            {
                                FromDateHiddenField.Value = null;
                            }

                            if (ToDateHiddenField.Value.Trim() != "")
                            {
                                //  Todate1 = DateTime.Parse(ToDateHiddenField.Value.ToString(), culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            }
                            else
                            {
                                ToDateHiddenField.Value = null;
                            }

                            if (OrderNoHiddenField.Value.Trim() == "")
                            {
                                OrderNoHiddenField.Value = null;
                            }

                            if (UserHiddenField.Value.Trim() == "")
                            {
                                UserHiddenField.Value = null;
                            }
                                                                                    
                            oDt = objOrderServices.GetFilteredOrderHistory(OrderNoHiddenField.Value, FromDateHiddenField.Value, ToDateHiddenField.Value, UserHiddenField.Value, Companyid);                            
                        }
                        else
                        {
                            oDt = objOrderServices.GetOrderHistory();
                        }

                        if (oDt != null && oDt.Rows.Count > 0)
                        {
                %>
                                        <table class="table table-bordered">
                                          <thead>
                                            <tr class="table_bg">
                                              <th>Cust. Order Date </th>
                                              <th>Cust. Order No</th>
                                              <th>Invoice Date </th>
                                              <th>Invoice No </th>
                                              <th>User </th>
                                              <th>Order Status </th>
                                              <th>Shipping Track &amp; Trace </th>
                                              <th>Submitted Order </th>
                                              <th>View Invoice </th>
                                            </tr>
                                          </thead>
                                          <%  
                            string bgcolor = "";
                            bool bodrow = true;

                            foreach (DataRow oDr in oDt.Rows)
                            {
                                bgcolor = bodrow ? "white" : "#e7efef";
                                bodrow = !bodrow;
                    %>
                                          <tbody>
                                            <tr>
                                              <td><%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Order Date"])%></td>
                                              <td><%=oDr["Cust.Order No"].ToString()%></td>
                                              <td>  <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Modified Date"])%></td>
                                              <td><%=oDr["Invoice No"].ToString()%></td>
                                              <td> <%=oDr["User"].ToString()%></td>
                                              <td> <%=oDr["Order Status"].ToString()%></td>
                                              <td>
                                                 <%-- <font color="#3399FF">--%>
                            <% 
                                if (!string.IsNullOrEmpty(oDr["SHIPTRACKURL"].ToString().Trim()))
                                { %>
                                <img id="ExternalLinkImg" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/External_Link.gif" height="12px" width="12px" alt="" /> 
                                &nbsp;
                                <a href="<% = oDr["SHIPTRACKURL"].ToString() %>" style="color: #0099da; text-decoration:none;"
                                    onclick="window.open('<% = oDr["SHIPTRACKURL"].ToString() %>','popup','width=800,height=600,scrollbars=yes,resizable=yes,toolbar=no,directories=no,location=no,menubar=no,modal=yes,status=yes,left=150,top=25'); return false">
                                    <%=oDr["Shipping Track & Trace"].ToString().Trim() %> </a>
                                 <% }
                                else if (!string.IsNullOrEmpty(oDr["Shipping Track & Trace"].ToString().Trim()))
                                {%>
                                   <%=oDr["Shipping Track & Trace"].ToString().Trim()%>
                                  <%}
                                                                  
                                    %>
                                      
                                    <a id="A1" class="submit_order" href="checkout.aspx?<%= EncryptSP(oDr["ORDERID"].ToString() + "#####" + "Pay") %>">
                                         Pay Now<%-- <img id="Img1" src="images/makepayment.png" height="25px" width="100px" alt="" />--%>
                                     </a>
                                 <%                                 
//}
                                    %>
                           <%-- </font>--%>
                                              </td>
                                              <td>
                                              <a id="HyperLink1" class="blue-color" href="OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>"
                                onclick="return PopupOrder('OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>')">
                                View Order </a>
                                              </td>
                                              <td>
                                                  <%
                              if (oDr["Invoice No"].ToString() != "" && (oDr["Order Status"].ToString().Trim() == "Payment Required" || oDr["Order Status"].ToString().Trim() == "Payment Successful" || oDr["Order Status"].ToString().Trim() == "Processing Order" || oDr["Order Status"].ToString().Trim() == "Order Shipped" || oDr["Order Status"].ToString().Trim() == "Completed" || oDr["Order Status"].ToString().Trim() == "Shipped" || oDr["Order Status"].ToString().Trim() == "Proforma Payment Required"))
                                {
                                    string InvNo= oDr["Invoice No"].ToString();
                                    string EncryptKeyString = string.Format("{0}|{1}", oDr["ORDERID"].ToString(), oDr["Invoice No"].ToString().Trim());
                                    string PdfNameValue = objSecurity.Encrypt(EncryptKeyString, Session.SessionID);
                                    string pdfn = Server.HtmlEncode(PdfNameValue).ToString();
                            %>
                                                     
                               <a class="HyperLink2" id="A2" style="color: #3399FF; cursor: pointer;" onclick="GetInvoice('<%=EncryptKeyString%>','<%=InvNo%>');" >
                                 <%
                                     if (oDr["Order Status"].ToString().Trim() != "Proforma Payment Required")
                                     {
                                     %>
                                 View Invoice 
                                 <%
                                     }
                                     else
                                     { %>
                                     View Proforma Invoice
                                     <%} %>
                                 </a>                               
                                <div id="<%=InvNo%>" style="display:none;" ><img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Invloading.gif"/ ></div>
                                   <asp:ImageButton ID="ImageButton1" runat="server" onclick="cmdUpdateField_Click"    style="display:none;" ></asp:ImageButton>
                                   
         
                            
                            <% } %>
                                              </td>
                                            </tr>
                                       
                                          </tbody>
                                            <% } %>
                                        </table>
                                        <%
                        }
                        else
                        {
                            MsgLabel.Text = "No Records Found!";
                            MsgLabel.Visible = true;
                            rcf.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgLabel.Text = ex.Message;
                        MsgLabel.Visible = true;
                        rcf.Visible = true;
                    }
                %>
                  <div class="form-group col-lg-20" id="rcf" runat="server" visible="false">
                      <asp:Label runat="server" ID="MsgLabel" Width="250px"> &nbsp; </asp:Label>
                  </div>
                                      </div>
                                    </div>


                                      <div id="PopDiv" class="containerOrdhis">
        <asp:Panel ID="SignalPopupPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyleOrdhis">
            <div class="containerOrdhis">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td width="10px">
                            
                        <asp:ImageButton ID="ImageButton2" runat="server" OnClick="btncancel_Click" ImageAlign="Right" />                        
                         </td>
                        
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis" colspan="2" style="font-size: medium; font-weight: bold">
                          &nbsp;&nbsp;Your Invoice has been generated.
                        <br/>
                        </td>
                        
                        
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis" colspan="3">
                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Click here to Download.." OnClick="btnClose_Click"  Font-Size="Medium" Font-Underline="True" ForeColor="Maroon" Font-Bold="False" />
                           
                        </td>
                        
                        
                    </tr>
           
                </table>
            </div>
        </asp:Panel>
    </div>
    <div id="PopDiv1" class="containerOrdhis">
        <asp:Panel ID="SignalProcessPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyleOrdhis">
            <div class="containerOrdhis">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr class="TableColumnStyleOrdhis">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                           Invoice PDF temopary not Available. 
                            <br />
                            Please try again later
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                            
                            <asp:Button ID="Button1" runat="server" Text="Close"  OnClick="btncancel_Click" CssClass="ButtonStyleOrdhis"/>                            
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
   
    </div>
     
     <div id="Div1" class="containerOrdhis">
        <asp:Panel ID="Panel1" runat="server" Style="display: none;" CssClass="ModalPopupStyleOrdhis">
            <div class="containerOrdhis">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">                   
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                            <asp:Button ID="ExitButton" runat="server" Text="Close"   OnClick="btncancel_Click" CssClass="ButtonStyleOrdhis"/>                            
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel> 
    </div>
    <div id="ProcessDiv" class="containerOrdhis" runat="server"> 
         <asp:Panel ID="ShowProcessPanel" runat="server" Style="display: none;"  CssClass="ModalPopupStyleOrdhis">
         <div class="containerOrdhis">
                   <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr class="TableColumnStyleOrdhis">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOrdhis">
                           Please wait for a moment.
                            <br />
                            Invoice will be available shortly
                        </td>
                    </tr>
                   </table>          
      </div>
        </asp:Panel>
       
    </div>
                            <asp:HiddenField ID="tabcontrolprosuccessField" runat="server" />
    <asp:HiddenField ID="tabcontrolHiddenField" runat="server" />
    <asp:HiddenField ID="tabcontrolprofileHiddenField" runat="server" />
                         <asp:HiddenField ID="FromDateHiddenField" runat="server"/>
                          <asp:HiddenField ID="ToDateHiddenField" runat="server" />
                         <asp:HiddenField ID="UserHiddenField" runat="server" />
                         <asp:HiddenField ID="OrderNoHiddenField" runat="server" />


                  <asp:Timer ID="Timer1" runat="server" OnTick="timer1_Tick" Interval="3000" Enabled="false" />
                  <asp:updatepanel id="updatepaneltimer" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"/>
         </Triggers>
   
    
       </asp:updatepanel>
                 </div>
                   <div id="home" class="tab-pane">
                   <div role="alert" class="alert alert-success border_radius_none text-center" id="prosucmsg" >
                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/micrositeimages/green_tick.png"/>
                     <strong> Your Data has been Updated Successfully....! </strong> 
                  </div>
                   <div id="divprofiledata">
                                        
                                          <h4 class="">Contact Details</h4>
                                          
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Company Name</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                                 <asp:TextBox autocomplete="off" ReadOnly="false" ID="txtcompname" CssClass="form-control checkout_input"
                                                            runat="server" onkeyup="javascript:Validate(this);" ></asp:TextBox>

                                            </div>
                                             <div class="clear"></div>
                                          </div>

                                            <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Mobile Phone</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                                 <asp:TextBox autocomplete="off" ReadOnly="false" ID="textMobilePhone" CssClass="form-control checkout_input"
                                                            runat="server" onkeyup="javascript:Validate(this);" MaxLength="10"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="reMobilePhone" runat="server" ControlToValidate="textMobilePhone"
                                                        ErrorMessage="Required" Text="Mobile No. must start with 04 and must be 10 digit" ValidationExpression="^(04)\d{8}$"
                                                        Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                                <asp:FilteredTextBoxExtender ID="fteMobilePhone" runat="server" FilterMode="ValidChars"
                                                        ValidChars="1234567890" TargetControlID="textMobilePhone" />

                                            </div>
                                             <div class="clear"></div>
                                          </div>

                                          <div class="clear"></div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Email <span class="error">*</span></label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAltEmail" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="55" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"
                                                        ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email"
                                                        ControlToValidate="txtAltEmail">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtAltEmail"
                                                        ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                            </div>
                                           <%-- <div class="clear"></div>--%>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Subscribe mail </label>
                                            <div class="col-sm-12">
                                              <%--<input type="checkbox" id="blankCheckbox" value="option1"/>--%>
                                               <asp:CheckBox ID="chkissubscribe" runat="server" Style="width: 10px;" TextAlign="Right" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                        
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">First Name</label>
                                            <div class="col-sm-12">
                                          <%--    <input type="text" class="form-control checkout_input" />--%>
                                                              <asp:TextBox autocomplete="off" ID="txtFname" ReadOnly="false" runat="server" MaxLength="40" CssClass="form-control checkout_input"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                      <asp:RequiredFieldValidator CssClass="error" ID="rfvFname" runat="server" ControlToValidate="txtFname" ErrorMessage="Enter first name"
                                                        Display="Dynamic"  ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>  
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Last Name</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox autocomplete="off" ID="txtLname" ReadOnly="false" runat="server" MaxLength="40"
                                                            CssClass="form-control checkout_input" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rfvLname" runat="server" ControlToValidate="txtLname"
                                                        Display="Dynamic" ErrorMessage="Enter last name"  ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                            <div style="display:none">
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address1</label>
                                            <div class="col-sm-12">
                                            <%--  <input type="text" class="form-control checkout_input" />--%>
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd1" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                                         <%--   <asp:RequiredFieldValidator ID="rfvAdd1" ErrorMessage="Enter address" runat="server"
                                                        ControlToValidate="txtAdd1" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address2</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                                  <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd2" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address3</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd3" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">City</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                               <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtCity" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                                                           <%--  <asp:RequiredFieldValidator ID="rfvCity" ErrorMessage="Enter city" runat="server"
                                                        ControlToValidate="txtCity" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">State</label>
                                            <div class="col-sm-12">
                                             <%-- <select class="form-control checkout_input">
                                                <option>Select</option>
                                              </select>--%>
                                                  <asp:TextBox ReadOnly="false" Visible="false" autocomplete="off" ID="drpState" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="20"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlstate" runat="server"  Class="form-control checkout_input"
                                                            Enabled="true">
                                                        </asp:DropDownList>
                                                            <%--   <asp:RequiredFieldValidator ID="rfvtxtstate" ErrorMessage="Enter state"
                                                        runat="server" ControlToValidate="drpState" Display="Dynamic" ValidationGroup="Mandatory"
                                                        Class="vldRequiredSkin" Enabled="false"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvddlstate" runat="server" 
                                                Text="Select State" ErrorMessage="Required" 
                                                ControlToValidate="ddlstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Post Code / Zip</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                                 <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtZip" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="10" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                                                 <asp:RequiredFieldValidator ID="rfvZip" ErrorMessage="Enter zip" runat="server"
                                                        ControlToValidate="txtZip" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                        ValidChars="1234567890" TargetControlID="txtZip" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Country </label>
                                            <div class="col-sm-12">
                                             <%-- <select class="form-control checkout_input">
                                                <option>Select</option>
                                              </select>--%>
                                               <asp:DropDownList ID="drpCountry" runat="server"  Class="form-control checkout_input"
                                                            Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Phone</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtPhone" CssClass="form-control checkout_input"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                                                 <%--  <asp:RequiredFieldValidator ID="rfvPhone" ErrorMessage="Enter phone" runat="server"
                                        ControlToValidate="txtPhone" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtPhone" />--%>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Mobile</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                             <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtMobile" CssClass="form-control checkout_input"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtMobile" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Fax</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                               <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtFax" CssClass="form-control checkout_input"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                                               <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtFax" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          </div>
                                          <div class="col-sm-20">
                                          <h4 class="">Billing information</h4>
                                          </div>

                                          <div class=" col-lg-20 font_12">
                                           <div class="billing-address" style="display:none">
                                           <div class="col-sm-2 col-xs-20">
                                        <asp:CheckBox ID="ChkBillingAdd" runat="server" Visible="false" AutoPostBack="true" OnClientClick="showprofiletab();"
                                    Class="CheckBoxSkin" meta:resourcekey="ChkBillTitle" Checked="false" OnCheckedChanged="ChkBillingAdd_CheckedChanged1" />
                                          <%--  <h4 class="checkbox_text font_normal">Using same Communication address</h4>--%>
                                            </div>
                                         <%--   <div class="col-sm-12 col-xs-20"><h4>Using same Communication address</h4></div>--%>
                                           </div>
                                            
                                          </div>
                                          
                                          
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address1</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd1" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="rfvbAdd1" runat="server" ControlToValidate="txtbillAdd1"
                                    Display="Dynamic" ErrorMessage="Enter address"  Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address2</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd2" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address3</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                               <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd3" runat="server"
                                         CssClass="form-control checkout_input" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">City</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillcity" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30" onkeyup="javascript:Validate(this);"></asp:TextBox> 
                                             <asp:RequiredFieldValidator ID="rfvbCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtbillcity" ErrorMessage="Enter city" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">State</label>
                                            <div class="col-sm-12 mb0">
                                              <%--<select class="form-control checkout_input">
                                                <option>Select</option>
                                              </select>--%>
                                               <asp:TextBox Visible="false" ReadOnly="false" autocomplete="off" ID="drpBillState"
                                        runat="server"  CssClass="form-control checkout_input" MaxLength="20" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                    <asp:DropDownList ID="ddlbillstate" runat="server" Enabled="true"  Class="form-control checkout_input">
                                    </asp:DropDownList>
                                       <asp:RequiredFieldValidator ID="RVtxtBillstate" runat="server" ControlToValidate="drpBillState"
                                    Display="Dynamic" ErrorMessage="Enter State" Class="vldRequiredSkin" ValidationGroup="Mandatory" Enabled="false">
                                    </asp:RequiredFieldValidator>
                           <asp:RequiredFieldValidator ID="RVddlBillstate" runat="server" 
                                                Text="Select State" ErrorMessage="Required" 
                                                ControlToValidate="ddlbillstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>
                                            </div>
                                            <%--<div class="clear"></div>--%>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Post Code / Zip</label>
                                            <div class="col-sm-12">
                                            <%--  <input type="text" class="form-control checkout_input" />--%>
                                               <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillzip" runat="server" CssClass="form-control checkout_input"
                                        MaxLength="10" onkeyup="javascript:Validate(this);"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="rfvbZip" Class="vldRequiredSkin" runat="server" ControlToValidate="txtbillzip"
                                    ErrorMessage="Enter zip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator><asp:FilteredTextBoxExtender
                                        ID="FilteredTextBoxExtender5" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                        TargetControlID="txtbillzip" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Country </label>
                                            <div class="col-sm-12">
                                              <%--<select class="form-control checkout_input">
                                                <option>Select</option>
                                              </select>--%>
                                              <asp:DropDownList ID="drpBillCountry" runat="server" Enabled="true" 
                                        Class="form-control checkout_input" AutoPostBack="True" OnSelectedIndexChanged="drpBillCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Phone</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillphone" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="16" ></asp:TextBox>
                                          <asp:RequiredFieldValidator ID="rfvbPhone" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtbillphone" ErrorMessage="Enter phone" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtbillphone" />
                                            </div>
                                            
                                          </div>
                                           <div class="clear"></div>
                                        <%--  <h4 class="blue_color_text bolder col-lg-12 font_15">Shipping Information</h4>--%>
                                          <div class=" col-lg-20 font_12">
                                            <div class="billing-address">
                                            <div class="col-sm-2 col-xs-20">
                                                                                         <asp:CheckBox ID="ChkShippingAdd" runat="server" Visible="true" AutoPostBack="true"
                                    Enabled="true" Class="checkbox_text1 font_normal" Checked="false" 
                                    OnCheckedChanged="ChkShippingAdd_CheckedChanged" />
                                            </div>
                                            <div class="col-sm-12 col-xs-20">
                                             <h4 class="checkbox_text font_normal">Using same billing address</h4>
                                            </div>
                                           
                                            </div>
                                          </div>
                                          
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address1</label>
                                            <div class="col-sm-12">
                                             <%-- <input type="text" class="form-control checkout_input" />--%>
                                             <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd1" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30" onkeyup="javascript:Validate(this);"></asp:TextBox> 
                                         <asp:RequiredFieldValidator ID="rfvsAdd1" runat="server" ControlToValidate="txtshipAdd1"
                                    Display="Dynamic" ErrorMessage="Enter address" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address2</label>
                                            <div class="col-sm-12">
                                           <%--   <input type="text" class="form-control checkout_input" />--%>
                                           <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd2" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox> 
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Address3</label>
                                            <div class="col-sm-12">
                                           <%--   <input type="text" class="form-control checkout_input" />--%>
                                           <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd3" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">City</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipcity" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30" onkeyup="javascript:Validate(this);" ></asp:TextBox> 
                                           <asp:RequiredFieldValidator ID="rfvsCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipcity" ErrorMessage="Enter city" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">State</label>
                                            <div class="col-sm-12 mb0">
                                             <%-- <select class="form-control checkout_input">
                                                <option>Select</option>
                                              </select>--%>
                                               <asp:TextBox Visible="false" ReadOnly="false" autocomplete="off" ID="drpShipState"
                                        runat="server" CssClass="form-control checkout_input" MaxLength="20"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                                    <asp:DropDownList ID="ddlshipstate" runat="server" Enabled="true"  Class="form-control checkout_input">
                                    </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="RVshiptxtstate" runat="server" ControlToValidate="drpShipState"
                                    Display="Dynamic" ErrorMessage="Enter State" Class="vldRequiredSkin" 
                                    ValidationGroup="Mandatory" Enabled="false"></asp:RequiredFieldValidator>
                              <asp:RequiredFieldValidator ID="RVshipddlstate" runat="server" 
                               Text="Select State" ErrorMessage="Required" 
                                ControlToValidate="ddlshipstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>
                                            </div>
                                            <%--<div class="clear"></div>--%>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Post Code / Zip</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                              <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipzip" runat="server" CssClass="form-control checkout_input"
                                        MaxLength="10"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="rfvsZip" Class="vldRequiredSkin" runat="server" ControlToValidate="txtshipzip"
                                    ErrorMessage="Enter zip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtshipzip" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Country </label>
                                            <div class="col-sm-12">
                                              <%--<select class="form-control checkout_input">
                                                <option>Select</option>
                                              </select>--%>
                                                <asp:DropDownList ID="drpShipCountry" Enabled="true" runat="server" 
                                        Class="form-control checkout_input" AutoPostBack="True" OnSelectedIndexChanged="drpShipCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-10">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top">Phone</label>
                                            <div class="col-sm-12">
                                              <%--<input type="text" class="form-control checkout_input" />--%>
                                               <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipphone" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="16" ></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="rfvsPhone" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipphone" ErrorMessage="Enter phone" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtshipphone" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          <div class="form-group col-md-20">
                                            <label for="inputEmail3" class="col-sm-6 control-label font_normal padding_top"></label>
                                            <div class="col-sm-14 text-right">
                                           <%--   <button class="btn btn-primary">Update</button>--%>
                                             <asp:Button ID="btnUpdate" Visible="true" runat="server"   text="Update"
                                OnClick="btnUpdate_Click"   
                        class="btn btn-primary" 
                         ValidationGroup="Mandatory" />
                                            </div>
                                            <div class="clear"></div>
                                          </div>
                                          </div>
                   </div>
                  <div id="messages" class="tab-pane">
                     <div class="col-sm-20 centered-block">
                     <asp:Label ID="lblError" SkinID ="lblErrorSkin" CssClass="mandatory" runat="server" Text="" ></asp:Label> 
                     </div>
                                     <%-- <form>--%>
                                        <div class="form-group col-sm-10">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3">Old Password<span class="error"> *</span></label>
                                          <div class="col-sm-12">
                                            <%--<input type="password" id="inputEmail3" class="form-control checkout_input">--%>
                                             <asp:TextBox  autocomplete="off"  ID="txtOldPassword" SkinID="textSkin" CssClass="form-control checkout_input" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Old Password Required"  ControlToValidate="txtOldPassword" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                          <div class="clear"></div>
                                        </div>
                                        <div class="form-group col-sm-10">
                                          <label for="inputEmail3" class="col-sm-5 control-label font_normal padding_top">New Password</label>
                                          <div class="col-sm-12">
                                          <%--  <input type="password" id="inputEmail3" class="form-control checkout_input">--%>
                                           <asp:TextBox  autocomplete="off"  ID="txtNewPassword" SkinID="textSkin" CssClass="form-control checkout_input" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                    
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
                                                        ControlToValidate="txtNewPassword"  CssClass="error"  ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
                                                        runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                                   
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="error" ErrorMessage="New Password Required" ControlToValidate="txtNewPassword"  Display="Dynamic"></asp:RequiredFieldValidator>
                                          </div>
                                          <div class="clear"></div>
                                        </div>
                                        <div class="form-group col-sm-10">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3">Confirm Password</label>
                                          <div class="col-sm-12">
                                          <%--  <input type="password" id="inputEmail3" class="form-control checkout_input">--%>
                                           <asp:TextBox  autocomplete="off"  ID="txtNewConfirmPassword" SkinID="textSkin" CssClass="form-control checkout_input" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox></br>
                    
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="error" ErrorMessage="Confirm Password Required"
                 ControlToValidate="txtNewConfirmPassword" ></asp:RequiredFieldValidator><br />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match" CssClass="error"
                  ControlToCompare="txtNewPassword" ControlToValidate="txtNewConfirmPassword" ValueToCompare="String"></asp:CompareValidator>
                                          </div>
                                          <div class="clear"></div>
                                        </div>
                                        <div class="form-group col-sm-10">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3"></label>
                                          <div class="col-sm-12">
                                       <%--     <input type="reset" value="Change Password" class="btn btn-primary">--%>
                                        <asp:Button ID="btnChange" runat ="Server"  Text="Change Password"  CssClass="btn btn-primary" OnClick="btnChange_Click"    CausesValidation="true" /><br />
                                          </div>
                                          <div class="clear"></div>
                                        </div>
                                      <%--</form>--%>
                                    </div>
              <div id="settings" class="tab-pane">
                 <div class="col-sm-20 centered-block">
                     <asp:Label ID="lblErrorusername" SkinID ="lblErrorSkin" CssClass="mandatory" runat="server" Text="" ></asp:Label> 
                     </div>
                                    <div class="col-lg-20 margin_top_20">
                                    <%--  <form>--%>
                                        <div class="form-group col-sm-10">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3">Current Login name<span class="error"> *</span></label>
                                          <div class="col-sm-12">
                                          <%--  <input type="password" id="inputEmail3" class="form-control checkout_input">--%>
                                          <asp:TextBox  autocomplete="off"  ID="txtOldUserName" CssClass="form-control checkout_input"  MaxLength ="10" runat="server" ></asp:TextBox>
                                          </div>
                                          <div class="clear"></div>
                                        </div>
                                        <div class="form-group col-sm-10">
                                          <label for="inputEmail3" class="col-sm-5 control-label font_normal padding_top">New Login name</label>
                                          <div class="col-sm-12">
                                           <%-- <input type="password" id="inputEmail3" class="form-control checkout_input">--%>
                                            <asp:TextBox  autocomplete="off"  ID="txtNewUserName" MaxLength ="10" runat="server" CssClass="form-control checkout_input"  Font-Underline="False" onkeypress="capLock(event)"></asp:TextBox>
                 <asp:RequiredFieldValidator CssClass="error" ID="rfvNewPwd" Class="vldRequiredSkin" ControlToValidate ="txtNewUserName" runat="server" ErrorMessage="Enter New Login name"  ValidationGroup="MandatoryCL"  ></asp:RequiredFieldValidator>
                                             </div>
                                          <div class="clear"></div>
                                        </div>
                                        <div class="form-group col-sm-10">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3">Confirm Login name</label>
                                          <div class="col-sm-12">
                                            <%--<input type="password" id="inputEmail3" class="form-control checkout_input">--%>
                                            <asp:TextBox  autocomplete="off"  ID="txtConfirmUserName" CssClass="form-control checkout_input"  MaxLength ="10" runat="server"  onkeypress="capLock(event)"></asp:TextBox> 
                  <asp:RequiredFieldValidator ID="rfvConPwd" CssClass="error"  Class="vldRequiredSkin" ControlToValidate ="txtConfirmUserName" runat="server"  ErrorMessage="Enter Confirm Login name" ValidationGroup="MandatoryCL"></asp:RequiredFieldValidator>
                          <br />
                            <asp:CompareValidator ID="CompareValidator2" CssClass="error" runat="server" ErrorMessage="New Login name and confirm Login name are not same"
                  ControlToCompare="txtNewUserName" ControlToValidate="txtConfirmUserName" ValueToCompare="String" ValidationGroup="MandatoryCL"></asp:CompareValidator>
                                          </div>
                                          <div class="clear"></div>
                                        </div>
                                        <div class="form-group col-sm-10">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3"></label>
                                          <div class="col-sm-12">
                                            <%--<input type="reset" value="Change Username" class="btn btn-primary">--%>
                                            <asp:Button ID="btnChangeLoginName" runat ="Server"  Text="Change Username" OnClick="btnChangeLoginName_Click"  CssClass="btn btn-primary"  ValidationGroup="MandatoryCL"  CausesValidation="true" />
                                          </div>
                                          <div class="clear"></div>
                                        </div>
                                     <%-- </form>--%>
                                    </div>
                                  </div>
               </div>
               </div>
            </div>
        </div>
        </div>
      </div>
     </div>
     <%  }
        else
        {
           Response.Redirect("/Login.aspx",false);
        }
    %>
    <%--<table width="812px" cellspacing="5" cellpadding="0" border="0" align="left">
     <tr>
      <td align="left">
     <a href="/Home.aspx" style="color: #0099FF" class="tx_3">Home</a> 
     <font style="font-family: Arial, Helvetica, sans-serif; font-weight: bolder; font-size: small; font-style: normal"> / 
     </font>MyAccount 
     </td>
      </tr> 
      <tr>
       <td align="left">
      <hr /> 
      </td> 
      </tr> 
      </table>--%>
  <%--  <table width="100%" cellspacing="0" cellpadding="0" border="0" height="440px" align="center">
        <tbody>
           <tr>
            <td width="10" height="10"> 
            <img alt="" height="17" src="images/tbl_topLeft.gif" width="10" />
             </td>
              <td background="Images/tbl_top.gif" height="10"> <img alt="" height="17" src="images/tbl_top.gif" width="10" />
               </td> <td width="10" height="10">
                <img alt="" height="17" src="images/tbl_topRight.gif" width="10" />
                 </td> 
                 </tr>
            <tr>
                <td width="10" background="Images/tbl_left.gif"> <img alt="" height="10" src="images/tbl_left.gif" width="10" /> </td>
                <td valign="top">
                    <table align="left" border="0" width="100%" cellpadding="1" cellspacing="0">
                        <tr>
                            <td valign="top" align="left" width="30%">--%>
                             
                               <%--     <% if (System.Convert.ToInt16(Session["USER_ROLE"]) <= 3)
                                       { %>
                                <ul>
                                         <% if (System.Convert.ToInt16(Session["USER_ROLE"]) != 3)
                                                    { %>
                                        <li>
                                            <a href="OrderHistory.aspx" class="CompanylinkSkin" >Order History</a>
                                        </li>
                                             <%} %>
                                             
                                              <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 1 && System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer") == true)
                                                    { %>
                                        <li>

                                            <a href="PaymentHistory.aspx" class="CompanylinkSkin" >Payment History</a>
                                        </li>
                                             <%} %>

                                              <% if (System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer") == false)
                                                    { %>
                                              <li>
              
                                                   <a href="PendingOrder.aspx" class="CompanylinkSkin" >Pending Orders</a>
                                            </li>
                                             <%} %>
                                      <%} %>
                               
                                    <% 
                                        HelperServices objHelperServices = new HelperServices();
                                        if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString() == "YES" && System.Convert.ToInt16(Session["USER_ROLE"]) < 4)
            {
                if (objHelperServices.GetOptionValues("ORDERPURCHASE").ToString().ToUpper() == "YES")
                {
                                    %>
                                
                                    <%      }
                                        if (objHelperServices.GetOptionValues("QUOTEPURCHASE").ToString().ToUpper() == "YES")
                { %>
                                    <%
                }
            } %>
                               
                                    <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 1 && System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer")==false) 
                                       { %>

                                    <li>
                                       
                                            <asp:LinkButton ID="lbtnCompanyUsers" Text="Company Users" runat="server" Class="CompanylinkSkin"
                                            OnClick="MultiUserEdit_Click"></asp:LinkButton>
                                             
                                            </li>
                                        <%} %>

                                        <% if (System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer") == true)
                                           {                                               
                                            
                                            %>

                                             <li>
 
                                            <a href="RetailerEditUserProfile.aspx" class="CompanylinkSkin" >View Profile</a>

                                            </li>
                                          
                                          <%}
                                           else
                                           { %>
                                    
                                              <li>
 
                                            <a href="EditUserProfile.aspx" class="CompanylinkSkin" >View Profile</a>
                                            </li>
                                          
                                          <%} %>
                                          <li>
            
                                                    <a href="changePassword.aspx" class="CompanylinkSkin" >Change Password</a>
                                                    </li>
                                         <li>
                               

                                                    <a href="changeUserName.aspx" class="CompanylinkSkin" >Change User Name</a>
                                                    </li>
                                          
                                        
                                   
                                           </ul> --%>
                                          
                                          <%--</td>
                                           </tr> 
                                           </table> 
                                          </td> 
                                          <td width="10" background="Images/tbl_right.gif">
                                           <img alt="" height="350" src="images/tbl_right.gif" width="10" /> 
                                           </td>
                                            </tr>
<tr> 
<td width="10" height="10">
 <img alt="" height="10" src="images/tbl_bottomLeft.gif" width="10" />
  </td> 
  <td background="Images/tbl_bottom.gif" height="10"> 
  <img alt="" height="10" src="images/tbl_bottom.gif" width="10" /> 
  </td>
   <td width="10" height="10"> 
  <img alt="" height="10" src="images/tbl_bottomRight.gif" width="10" /> 
  </td>
   </tr>
   </tbody>
    </table>--%>
    <%--<%  }
        else
        {
            Response.Redirect("/Login.aspx",false);
        }
    %>--%>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server"></asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
