<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mMyAccount.aspx.cs" Inherits="WES.mMyAccount"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>  
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--<script language="javascript" type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/jquery-ui-1.8.13.custom.min.js"></script>--%>
    <%--<link href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/progress.js" type="text/javascript"></script>--%>
   

<script language="javascript" type="text/javascript">
    function blockspecialcharacters(e) {
        var keynum
        var keychar
        var numcheck
        if (window.event) {
            keynum = e.keyCode
        }
        else if (e.which) {
            keynum = e.which
        }
        keychar = String.fromCharCode(keynum)
        if (keychar == "@" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")" || keychar == "+") {
            e.keyCode = '';
            return false;
        }
        else {
            return true;
        }
    }


    function CheckTextPassMaxLength(textBox, e, length) {

        var mLen = textBox["MaxLength"];

        var tsellen = 0;
        var text = document.getElementById(textBox.id);
        var t = text.value.substr(text.selectionStart, text.selectionEnd - text.selectionStart);




        if (t != null) {
            tsellen = t.length;
        }
        if (null == mLen)
            mLen = length;

        var maxLength = parseInt(mLen);
        if (!checkSpecialKeys(e)) {
            if (textBox.value.length - tsellen > maxLength - 1) {
                if (window.event)//IE
                {
                    alert("Password Length should not be greater than 15");
                    e.returnValue = false;
                }
                else//Firefox
                {
                    alert("Password Length should not be greater than 15");
                    e.preventDefault();
                }
            }
        }
    }

    function checkSpecialKeys(e) {
        if (e.keyCode != 9 && e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
            return false;
        else
            return true;
    }
</script>
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
             window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'none';
         }

         function setImage() {
             window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'block';
         }

         function PopupOrder(url) {
             newwindow = window.open(url, 'name', 'scrollbars=1,left=130,top=50,height=600,width=800');
             if (window.focus) {
                 newwindow.focus();
             }
             return false;
         }

         function payonlinepopup_open(url) {

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
                     if (document.getElementById("ctl00_MainContent_FromdateTextBox").value == null) {
                         document.getElementById("ctl00_MainContent_FromdateTextBox").value = Newdate;
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

             $("#<%=FromdateTextBox.ClientID %>").addClass("datepickerwidth");
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

             $("#<%=TodateTextBox.ClientID %>").addClass("datepickerwidth");
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
     

        }
        function OnFailure(error) {
            var invno = result.replace("inv", "");
            toggle(invno, "none")
            alert("Invoice PDF temopary not Available.Please try again later");
            //alert(error);

        }
</script>


   <script type="text/javascript">

       $(document).ready(function () {
           if ($("#<%=tabcontrolHiddenField.ClientID%>").val().toLowerCase() == "false") {
               //var x = $('#messages').attr('class');
               // alert(x);

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
                   $("#OH").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
                   $("#VP").removeClass();
                   $("#VP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob active");
                   $("#CP").removeClass();
                   $("#CP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
                   $("#CL").removeClass();
                   $("#CL").addClass("col-xs-12 col-lg-3 col-sm-3 border_right_none padding_left_right_mob");
               }
               // $("#messages").removeClass();
               //  $("#messages").addClass("tab-pane active");

           }

           if ($("#<%=tabcontrolprosuccessField.ClientID%>").val().toLowerCase() == "true") {
               // prosucmsg   divprofiledata  
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
               $("#OH").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
               $("#VP").removeClass();
               $("#VP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob active");
               $("#CP").removeClass();
               $("#CP").addClass("col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob");
               $("#CL").removeClass();
               $("#CL").addClass("col-xs-12 col-lg-3 col-sm-3 border_right_none padding_left_right_mob");
           }
           else {
               $("#prosucmsg").removeClass();
               $("#prosucmsg").addClass("dpynone");
               $("#divprofiledata").removeClass();
               $("#divprofiledata").addClass("col-lg-9 margin_top_20");
           }


       });
       function setActiveTab() {
           var x = $('#messages').attr('class');
           alert(x);
           var isPostback = $("#<%=tabcontrolHiddenField.ClientID%>").val().toLowerCase();
           alert(isPostback);
       }        
    </script>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
<h4 class="blue_color bolder font_15 white_color col-lg-12 panel-heading margin_bottom_none">My Account</h4>
<%--<% 
    if (Request.HttpMethod == "POST") 
    {
%>
    <script type="text/javascript">
       // var divclss = "";
       // divclss = $('#home').attr('class');
       // divclss = $('#home').attr('class');
       // alert(divclss);
        //var divclss11 = "";
       // divclss11 =  document.getElementById("profile");
        var x = $('#profile').attr('class');
        //divclss11 = $('#profile').attr('class');
        alert(x);


        
    </script>
<% 
    }
%>--%>
<div class="col-lg-12 account_bg margin_bottom_15 padding_btm_20">
      <div class="col-xs-12 col-lg-12 col-md-3 col-sm-12">
        <ul class="nav nav-tabs account_tab border_left_none border_right_none border_top_none mar_Account_top">
          <li class="col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob active"  id="OH"><a data-toggle="tab" href="#profile">
            <div class="order_history"></div>
            <strong>Order History</strong></a></li>
          <li class="col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob" id="VP"><a data-toggle="tab" href="#home">
            <div class="view_profile"></div>
            <strong>View Profile</strong></a></li>        
          <li class="col-xs-12 col-lg-3 col-sm-3 padding_left_right_mob" id="CP"><a data-toggle="tab" href="#messages">
            <div class="change_pass"></div>
            <strong>Change Password</strong></a></li>
          <li class="col-xs-12 col-lg-3 col-sm-3 border_right_none padding_left_right_mob" id="CL"> <a data-toggle="tab" href="#settings">
            <div class="login_name"></div>
            <strong>Change Login Name</strong></a></li>
        </ul>
      </div>
      <div class="col-xs-12 col-lg-12 col-md-9 col-sm-12">
        <div class="tab-content">
          <div id="home" class="tab-pane" >
          <div role="alert" class="alert alert-success border_radius_none text-center" id="prosucmsg" >
          <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/micrositeimages/green_tick.png"/>
          <strong> Your Data has been Updated Successfully....! </strong> 
          </div>
            <div class="col-lg-9 margin_top_20" id="divprofiledata">
              <h4 class="blue_color_text bolder col-lg-12 font_15">Contact Details</h4>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Company Name</label>
                <div class="col-sm-8">
              <%--    <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                 <asp:TextBox autocomplete="off" ReadOnly="false" ID="txtcompname"  CssClass="form-control checkout_input" runat="server" MaxLength="50"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">First Name</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox autocomplete="off" ID="txtFname" ReadOnly="false" runat="server" MaxLength="40" CssClass="form-control checkout_input"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
                      <asp:RequiredFieldValidator CssClass="error" ID="rfvFname" runat="server" ControlToValidate="txtFname" ErrorMessage="Enter first name"
                                                        Display="Dynamic"  ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>                                        
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Last Name</label>
                <div class="col-sm-8">
                  <%--<input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox autocomplete="off" ID="txtLname" ReadOnly="false" runat="server" MaxLength="40"
                                                            CssClass="form-control checkout_input"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rfvLname" runat="server" ControlToValidate="txtLname"
                                                        Display="Dynamic" ErrorMessage="Enter last name"  ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address1</label>
                <div class="col-sm-8">
                  <%--<input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                      <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd1" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30"   onkeyup="javascript:Validate(this);"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvAdd1" ErrorMessage="Enter address" runat="server"
                                                        ControlToValidate="txtAdd1" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address2</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd2" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30"   onkeyup="javascript:Validate(this);"></asp:TextBox>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address3</label>
                <div class="col-sm-8">
                  <%--<input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd3" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">City</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtCity" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="30"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                                             <asp:RequiredFieldValidator ID="rfvCity" ErrorMessage="Enter city" runat="server"
                                                        ControlToValidate="txtCity" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">State</label>
                <div class="col-sm-8">
                 <asp:TextBox ReadOnly="false" Visible="false" autocomplete="off" ID="drpState" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="20"   onkeyup="javascript:Validate(this);"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlstate" runat="server"  Class="form-control checkout_input"
                                                            Enabled="true">
                                                        </asp:DropDownList>
                                                               <asp:RequiredFieldValidator ID="rfvtxtstate" ErrorMessage="Enter state"
                                                        runat="server" ControlToValidate="drpState" Display="Dynamic" ValidationGroup="Mandatory"
                                                        Class="vldRequiredSkin" Enabled="false"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvddlstate" runat="server" 
                                                Text="Select State" ErrorMessage="Required" 
                                                ControlToValidate="ddlstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>
              <%--    <select class="form-control checkout_input">
                    <option>Select</option>
                  </select>--%>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Post Code / Zip</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtZip" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="10"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                                                 <asp:RequiredFieldValidator ID="rfvZip" ErrorMessage="Enter zip" runat="server"
                                                        ControlToValidate="txtZip" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                        ValidChars="1234567890" TargetControlID="txtZip" />
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Country </label>
                <div class="col-sm-8">
                 <%-- <select class="form-control checkout_input">
                    <option>Select</option>
                  </select>--%>
                        <asp:DropDownList ID="drpCountry" runat="server"  Class="form-control checkout_input"
                                                            Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Email</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAltEmail" CssClass="form-control checkout_input"
                                                            runat="server" MaxLength="55"  ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"
                                                        ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email"
                                                        ControlToValidate="txtAltEmail">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtAltEmail"
                                                        ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Subscribe mail </label>
                <div class="col-sm-8">
                <%--  <input type="checkbox" id="blankCheckbox" value="option1">--%>
                 <asp:CheckBox ID="chkissubscribe" runat="server" Style="width: 10px;" TextAlign="Right" />
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Phone</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                  <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtPhone" CssClass="form-control checkout_input"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                                                   <asp:RequiredFieldValidator ID="rfvPhone" ErrorMessage="Enter phone" runat="server"
                                        ControlToValidate="txtPhone" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtPhone" />
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Mobile</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtMobile" CssClass="form-control checkout_input"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtMobile" />
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Fax</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtFax" CssClass="form-control checkout_input"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                                               <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtFax" />
                </div>
                <div class="clear"></div>
              </div>
              <h4 class="blue_color_text bolder col-lg-12 font_15">Billing information</h4>
              <h4 class=" col-lg-12 font_12">
                  <asp:CheckBox ID="ChkBillingAdd" runat="server" Visible="true" AutoPostBack="true" 
                            Class="checkbox_text1 font_normal"   Checked="false" OnCheckedChanged="ChkBillingAdd_CheckedChanged1" />
                            <label class="checkbox_text font_normal"> Using same communication address</label>
              </h4>
              
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address1</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                  <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd1" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="rfvbAdd1" runat="server" ControlToValidate="txtbillAdd1"
                                    Display="Dynamic" ErrorMessage="Enter address"  Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address2</label>
                <div class="col-sm-8">
                  <%--<input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                       <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd2" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address3</label>
                <div class="col-sm-8">
                  <%--<input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                   <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd3" runat="server"
                                         CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">City</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                 <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillcity" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);"></asp:TextBox> 
                                             <asp:RequiredFieldValidator ID="rfvbCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtbillcity" ErrorMessage="Enter city" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">State</label>
                <div class="col-sm-8">
                 <%-- <select class="form-control checkout_input">
                    <option>Select</option>
                  </select>--%>
                   <asp:TextBox Visible="false" ReadOnly="false" autocomplete="off" ID="drpBillState"
                                        runat="server"  CssClass="form-control checkout_input" MaxLength="20"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
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
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Post Code / Zip</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillzip" runat="server" CssClass="form-control checkout_input"
                                        MaxLength="10"   onkeyup="javascript:Validate(this);"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="rfvbZip" Class="vldRequiredSkin" runat="server" ControlToValidate="txtbillzip"
                                    ErrorMessage="Enter zip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator><asp:FilteredTextBoxExtender
                                        ID="FilteredTextBoxExtender5" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                        TargetControlID="txtbillzip" />
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Country </label>
                <div class="col-sm-8">
                 <%-- <select class="form-control checkout_input">
                    <option>Select</option>
                  </select>--%>
                     <asp:DropDownList ID="drpBillCountry" runat="server" Enabled="true" 
                                        Class="form-control checkout_input" AutoPostBack="True" OnSelectedIndexChanged="drpBillCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Phone</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                  <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillphone" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="16" ></asp:TextBox>
                                          <asp:RequiredFieldValidator ID="rfvbPhone" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtbillphone" ErrorMessage="Enter phone" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtbillphone" />
                </div>
                <div class="clear"></div>
              </div>
              <h4 class="blue_color_text bolder col-lg-12 font_15">Shipping Information</h4>
              <h4 class=" col-lg-12 font_12">
            <%--    <label>
                <input type="checkbox" id="blankCheckbox" value="option1" class="pull-left">
                <p class="checkbox_text font_normal">Using same billing address</p>
              </label>--%>
               <asp:CheckBox ID="ChkShippingAdd" runat="server" Visible="true" AutoPostBack="true"
                                    Enabled="true" Class="checkbox_text1 font_normal" Checked="false" 
                                    OnCheckedChanged="ChkShippingAdd_CheckedChanged" />  <%--Text=" Using same billing address"--%>
                                      <label class="checkbox_text font_normal"> Using same billing address</label>
              </h4>
              
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address1</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                 <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd1" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);"></asp:TextBox> 
                                         <asp:RequiredFieldValidator ID="rfvsAdd1" runat="server" ControlToValidate="txtshipAdd1"
                                    Display="Dynamic" ErrorMessage="Enter address" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address2</label>
                <div class="col-sm-8">
               <%--   <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                 <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd2" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"   onkeyup="javascript:Validate(this);"></asp:TextBox> 
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address3</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                  <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd3" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);" ></asp:TextBox>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">City</label>
                <div class="col-sm-8">
                <%--  <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipcity" runat="server"
                                        CssClass="form-control checkout_input" MaxLength="30"  onkeyup="javascript:Validate(this);" ></asp:TextBox> 
                                           <asp:RequiredFieldValidator ID="rfvsCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipcity" ErrorMessage="Enter city" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">State</label>
                <div class="col-sm-8">
                 <%-- <select class="form-control checkout_input">
                    <option>Select</option>
                  </select>--%>
                       <asp:TextBox Visible="false" ReadOnly="false" autocomplete="off" ID="drpShipState"
                                        runat="server" CssClass="form-control checkout_input" MaxLength="20"   onkeyup="javascript:Validate(this);"></asp:TextBox>
                                    <asp:DropDownList ID="ddlshipstate" runat="server" Enabled="true"  Class="form-control checkout_input">
                                    </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="RVshiptxtstate" runat="server" ControlToValidate="drpShipState"
                                    Display="Dynamic" ErrorMessage="Enter State" Class="vldRequiredSkin" 
                                    ValidationGroup="Mandatory" Enabled="false"  ></asp:RequiredFieldValidator>
                              <asp:RequiredFieldValidator ID="RVshipddlstate" runat="server" 
                               Text="Select State" ErrorMessage="Required" 
                                ControlToValidate="ddlshipstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Post Code / Zip</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
                     <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipzip" runat="server" CssClass="form-control checkout_input"
                                        MaxLength="10" onkeyup="javascript:Validate(this);" ></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="rfvsZip" Class="vldRequiredSkin" runat="server" ControlToValidate="txtshipzip"
                                    ErrorMessage="Enter zip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtshipzip" />
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Country </label>
                <div class="col-sm-8">
                 <%-- <select class="form-control checkout_input">
                    <option>Select</option>
                  </select>--%>
                    <asp:DropDownList ID="drpShipCountry" Enabled="true" runat="server" 
                                        Class="form-control checkout_input" AutoPostBack="True" OnSelectedIndexChanged="drpShipCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Phone</label>
                <div class="col-sm-8">
                 <%-- <input type="text" class="form-control checkout_input" id="inputEmail3">--%>
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
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top"></label>
                <div class="col-sm-8">
                  
<%--                  <asp:Button runat="server" ID="btnUpdate"  text="Update" OnClick="btnUpdate_Click" CssClass="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub"  />--%>
                	  <asp:Button ID="btnUpdate" Visible="true" runat="server"   text="Update"
                                OnClick="btnUpdate_Click"   
                        class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub" 
                         ValidationGroup="Mandatory" />

                </div>
                <div class="clear"></div>
              </div>
            </div>
          </div>
          <div id="profile" class="tab-pane active">
            <div class="col-lg-12  margin_top_20">
      
                <div class="form-group col-lg-4">
                  <label class="font_normal" for="exampleInputEmail1">Order No or Invoice No </label>
               <%--   <input type="email" class="form-control checkout_input" id="exampleInputEmail1">--%>
                 <asp:TextBox runat="server" ID="OrderNo" CssClass="form-control checkout_input" ></asp:TextBox>
                </div>
                <div class="form-group col-lg-4">
                  <label class="font_normal" for="exampleInputEmail1">From Date </label>
                
                     <asp:TextBox runat="server" ID="FromdateTextBox"  CssClass="form-control checkout_input datepickerwidth" ></asp:TextBox>
               
                </div>
                <div class="form-group col-lg-4">
                  <label class="font_normal" for="exampleInputEmail1">To Date </label>
                    <asp:TextBox runat="server" ID="TodateTextBox"  CssClass="form-control checkout_input datepickerwidth" ></asp:TextBox>
             
                </div>
                <div class="form-group col-lg-4">
                  <label class="font_normal" for="exampleInputEmail1">Created User </label>
                 
                     <asp:DropDownList runat="server" ID="CreatedUserDropDownlist" BackColor="White"  CssClass="form-control checkout_input" >
                            </asp:DropDownList>
                </div>
                <div class="form-group col-lg-8">
                     <asp:Button runat="server" UseSubmitBehavior="true" CausesValidation="false" ID="SearchButton"  CssClass="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100 margin_top_20" Text="Submit" OnClick="SearchButton_Click" />
                     <asp:Button runat="server" UseSubmitBehavior="true" CausesValidation="false" ID="ResetButton"  CssClass="btn-lg blue_color border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100 margin_top_20"  Text="Reset"  OnClick="ResetButton_Click" />    
                </div>
              
              <div class="table-responsive col-lg-12">
              
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
                               
                            }
                            else
                            {
                                FromDateHiddenField.Value = null;
                            }

                            if (ToDateHiddenField.Value.Trim() != "")
                            {
                               
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
                   <th>Cust. Order Date</th>
                  <th>Cust. Order No.</th>
                  <th>Invoice Date</th>
                  <th>Invoice No</th>
                  <th>User</th>
                  <th>Order Status</th>
                  <th>Shipping Track &amp; Trace</th>
                  <th>Submitted Order</th>
                  <th>View Invoice</th>
                    </tr>
                  </thead>

                  <tbody>
                   <%  
                            string bgcolor = "";
                            bool bodrow = true;
                            int i = 1;
                            foreach (DataRow oDr in oDt.Rows)
                            {
                               // bgcolor = bodrow ? "white" : "#e7efef";
                               // bodrow = !bodrow;
                                if (i % 2 == 1)
                                    bgcolor = "tdwhite";
                                else
                                    bgcolor = "tdgrey";
                                i++;
                    %>
                          <tr>
               <td>
                            <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Order Date"])%>
                        </td>
                        <td>
                            <%=oDr["Cust.Order No"].ToString()%>
                        </td>
                        <td>
                            <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Modified Date"])%>
                        </td>
                        <td>
                            <%=oDr["Invoice No"].ToString()%>
                        </td>
                        <td>
                            <%=oDr["User"].ToString()%>
                        </td>
                        <td>
                            <%=oDr["Order Status"].ToString()%>
                        </td>
                        <td>
                            <font color="#3399FF">
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
                                else if (oDr["CC_PAY_RESPONSE"].ToString().ToLower() != "yes" && (oDr["Order Status"].ToString().Trim() == "Payment Required" || oDr["Order Status"].ToString().Trim() == "Proforma Payment Required"))
                                {                                    
                                    %>
                                      
                                    <a id="A1" style="color: #3399FF" href="mcheckout.aspx?<%= EncryptSP(oDr["ORDERID"].ToString() + "#####" + "Pay" ) %>">
                                          <img id="Img1" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/make_payment.png"  alt="make payment" />
                                     </a>
                                 <%                                 
                                }
                                    %>
                            </font>
                        </td>
                        <td>
                            <a id="HyperLink1" class="blue_color_text" href="OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>"
                                onclick="return PopupOrder('mOrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>')">
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
                          <% } %>
                  </tbody>
                </table>
                    <%
                        }
                        else
                        {
                            MsgLabel.Text = "No Records Found!";
                            MsgLabel.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgLabel.Text = ex.Message;
                        MsgLabel.Visible = true;
                    }
                %>
              </div>
               <asp:Label runat="server" ID="MsgLabel" Width="250px"> &nbsp; </asp:Label>
            </div>
          </div>
          <div id="messages" class="tab-pane">
          <div class="col-lg-9 margin_top_20">
              <asp:Label ID="lblError" SkinID ="lblErrorSkin" CssClass="error_top col-lg-12 text-center" runat="server" Text="" ></asp:Label> 
              <div class="form-group">
            
                <label class="col-sm-4 control-label font_normal padding_top" for="inputEmail3">Old Password</label>
                <div class="col-sm-8">
                  <asp:TextBox  autocomplete="off"  ID="txtOldPassword" SkinID="textSkin" CssClass="form-control checkout_input" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Old Password Required"  ControlToValidate="txtOldPassword" CssClass="error" ></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">New Password</label>
                <div class="col-sm-8">
                         <asp:TextBox  autocomplete="off"  ID="txtNewPassword" SkinID="textSkin" CssClass="form-control checkout_input" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                    
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
                                                        ControlToValidate="txtNewPassword"  CssClass="error"  ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
                                                        runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                                   
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="error" ErrorMessage="New Password Required" ControlToValidate="txtNewPassword" ></asp:RequiredFieldValidator>
                
                                       
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label class="col-sm-4 control-label font_normal padding_top" for="inputEmail3">Confirm Password</label>
                <div class="col-sm-8">
                      <asp:TextBox  autocomplete="off"  ID="txtNewConfirmPassword" SkinID="textSkin" CssClass="form-control checkout_input" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox></br>
                    
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="error" ErrorMessage="Confirm Password Required"
                 ControlToValidate="txtNewConfirmPassword" ></asp:RequiredFieldValidator><br />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match" CssClass="error"
                  ControlToCompare="txtNewPassword" ControlToValidate="txtNewConfirmPassword" ValueToCompare="String"></asp:CompareValidator>
                 </div>
                <div class="clear"></div>
           </div> 
            
              <div class="form-group">
                <label class="col-sm-4 control-label font_normal padding_top" for="inputEmail3"></label>
                <div class="col-sm-8">
                 
                     <asp:Button ID="btnChange" runat ="Server"  Text="Change Password"  CssClass="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100 margin_top_20" OnClick="btnChange_Click"    CausesValidation="true" /><br />
                </div>
                <div class="clear"></div>
              </div>
              </div>
          </div>
          <div id="settings" class="tab-pane">
          <div class="col-lg-9 margin_top_20">
            <asp:Label ID="lblErrorpwd" SkinID ="lblErrorSkin" CssClass="error_top col-lg-12 text-center" runat="server" Text="" ></asp:Label> 
              <div class="form-group">
                <label class="col-sm-4 control-label font_normal padding_top" for="inputEmail3">Current Login name</label>
                <div class="col-sm-8">
                   <asp:TextBox  autocomplete="off"  ID="txtOldUserName" CssClass="form-control checkout_input"  MaxLength ="10" runat="server" ></asp:TextBox>
                  </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">New Login name</label>
                <div class="col-sm-8">
                <asp:TextBox  autocomplete="off"  ID="txtNewUserName" MaxLength ="10" runat="server" CssClass="form-control checkout_input"  Font-Underline="False" onkeypress="capLock(event)"></asp:TextBox>
                 <asp:RequiredFieldValidator CssClass="error" ID="rfvNewPwd" Class="vldRequiredSkin" ControlToValidate ="txtNewUserName" runat="server" ErrorMessage="Enter New Login name"  ValidationGroup="MandatoryCL"  ></asp:RequiredFieldValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label class="col-sm-4 control-label font_normal padding_top" for="inputEmail3">Confirm Login name</label>
                <div class="col-sm-8">
                 <asp:TextBox  autocomplete="off"  ID="txtConfirmUserName" CssClass="form-control checkout_input"  MaxLength ="10" runat="server"  onkeypress="capLock(event)"></asp:TextBox> 
                  <asp:RequiredFieldValidator ID="rfvConPwd" CssClass="error"  Class="vldRequiredSkin" ControlToValidate ="txtConfirmUserName" runat="server"  ErrorMessage="Enter Confirm Login name" ValidationGroup="MandatoryCL"></asp:RequiredFieldValidator>
                          <br />
                            <asp:CompareValidator ID="CompareValidator2" CssClass="error" runat="server" ErrorMessage="New Login name and confirm Login name are not same"
                  ControlToCompare="txtNewUserName" ControlToValidate="txtConfirmUserName" ValueToCompare="String" ValidationGroup="MandatoryCL"></asp:CompareValidator>
                </div>
                <div class="clear"></div>
              </div>
              <div class="form-group">
                <label class="col-sm-4 control-label font_normal padding_top" for="inputEmail3"></label>
                <div class="col-sm-8">
                  
                  <asp:Button ID="btnChangeLoginName" runat ="Server"  Text="Change Login Name" OnClick="btnChangeLoginName_Click"  CssClass="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100 margin_top_20"  ValidationGroup="MandatoryCL"  CausesValidation="true" />
                </div>
                <div class="clear"></div>
              </div>
            </div>
            </div>
     
      </div>
      <div class="clear"></div>
    </div>
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
