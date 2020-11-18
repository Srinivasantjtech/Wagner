<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_login" EnableTheming="true" Codebehind="login.ascx.cs"  %>
<%@ Register Src="~/UC/newproductsnav.ascx" TagName="newproductsnav" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>--%>
<input id="hidpwd" name="hidpwd" type="hidden" runat="server" />
<script type="text/javascript">
  
    function txtpassword_onFocus() {
        if (document.forms[0].elements["<%=hidpwd.ClientID%>"] != null) {
            if (document.forms[0].elements["<%=hidpwd.ClientID%>"].value.length != 0) {
               // document.forms[0].elements["<%=txtPassword.ClientID%>"].value = document.forms[0].elements["<%=hidpwd.ClientID%>"].value;
            }
        }
    }
   
    
    function passwordcheck() {
        if (document.forms[0].elements["<%=txtUserName.ClientID%>"].value.length != 0) {
            if (document.forms[0].elements["<%=txtPassword.ClientID%>"] != null) {
       //  if(document.getElementById("<%=txtUserName.ClientID%>").value.length != 0){
          //   if (document.getElementById("<%=txtPassword.ClientID%>").value.length != 0) {
     
            }
            else {
                document.forms[0].elements["<%=txtPassword.ClientID%>"].value = document.forms[0].elements["<%=hidpwd.ClientID%>"].value;
               // document.getElementById("<%=txtPassword.ClientID%>").value = document.getElementById("<%=hidpwd.ClientID%>").value;
                return false;
            }
        }
    }
    function ForgotLinkPage() {
        var mUser = document.getElementById("<%=txtUserName.ClientID%>");
        window.location.href = "ForgotPassword.aspx?loginName=" + mUser.value;
    }
    function ForgotLinkPageUserID() {
        window.location.href = "ForgotUserName.aspx";
    }
    function ForgotLinkPageClose() {
        document.getElementById("ctl00_maincontent_ctl00_PopupOrderMsg").style.display = "none";
        document.getElementById("ctl00_maincontent_ctl00_popUp_backgroundElement").style.display = "none";

    }


    var recaptcha1;
    var myCallBack = function () {
        recaptcha1 = grecaptcha.render('recaptcha1', {
            // 'sitekey': '6LfMvyATAAAAAEQrA6JxZhXQYjtXHA5nvOOSkWeY', //Replace this with your Site key
            'sitekey': '<%=ConfigurationManager.AppSettings["Googlerecaptcha"] %>',
            'theme': 'light'
        });

    };
    function validateform() {
        var captcha_response = grecaptcha.getResponse(recaptcha1);
        if (captcha_response.length == 0) {
            return false;
        }
        else {
            return true;
        }
    }


</script>
   <script type="text/javascript">
       function blockspecialcharacters(e) {
           var key = window.event ? e.keyCode : e.which;
           var keychar = String.fromCharCode(key);
           var reg = new RegExp("[0-9.a-z.-]");      
           return reg.test(keychar);
       }   
    </script>
    
<script type="text/javascript" >
    
     function keyboardup(id) {
         var value = id.value
         // id.value = value.replace(/'/, '`');
         var e = e || window.event;
          if (e.keyCode == '37' || e.keyCode == '38' || e.keyCode == '39' || e.keyCode == '40' || e.keyCode == '8') {
         }
         else {
             id.value = value.replace(/'/g, "`")
         }
          //Validate(id.value);
           }

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
     function chkPB() {

         var optc =  document.getElementById('<%= RBPersonal.ClientID %>')
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
     function keyboardActions(event) {
         if (event.keyCode == 13) {
             eval($("#<%=cmdLogin.ClientID %>").trigger('click'));
             return false;
         }
     }
//     $(document).ready(function () {
//         if ($.browser.mozilla) {          
//             $("#<%=txtUserName.ClientID %>").keypress(keyboardActions);
//             $("#<%=txtPassword.ClientID %>").keypress(keyboardActions);
//             $("#<%=chkKeepme.ClientID %>").keypress(keyboardActions);
//         } else {
//             $("#<%=txtUserName.ClientID %>").keydown(keyboardActions);
//             $("#<%=txtPassword.ClientID %>").keydown(keyboardActions);
//             $("#<%=chkKeepme.ClientID %>").keydown(keyboardActions);
//         }

//     });

  </script>


    <script type="text/javascript">
        function updateObjectIframe(which) {
            document.getElementById('one').innerHTML = '<' + 'object id="foo" height="370px" width="400px" name="foo" type="text/html" data="' + which.href + '"><\/object>';
        }

       
</script>
    <%--<script type="text/javascript" language="javascript">
        function MouseHover(ID) {
            switch (parseInt(ID)) {
                case 1:
                    document.getElementById("ctl00_maincontent_ForgotPassword").style.backgroundColor = "#009F00";
                    break;
                case 2:
                    document.getElementById("ctl00_maincontent_Close").style.backgroundColor = "red";
                    break;
            }
        }

        function MouseOut(ID) {
            switch (parseInt(ID)) {
                case 1:
                    document.getElementById("ctl00_maincontent_ForgotPassword").style.backgroundColor = "#1589FF";
                    break;
                case 2:
                    document.getElementById("ctl00_maincontent_Close").style.backgroundColor = "#1589FF";
                    break;
            }
        }
    </script>--%>
    <script language="javascript" type="text/javascript">
        function checkEnableSubmit() {

            
            //var checkbox = document.getElementById('<%= chkterms.ClientID %>').checked;
            // alert(checkbox);
            if ((document.getElementById('<%= chkterms.ClientID %>').checked == false)) {
                //                var x = document.getElementById('<%= chkterms.ClientID %>').childNodes[0].nodeValue;
                //                alert(x);
                //document.getElementById('ctl00_maincontent_chkterms').childNodes[0].nodeValue = "Required";
                document.getElementById('<%= lblCheckTerms.ClientID %>').childNodes[0].nodeValue = "Required";
                alert('To Submit form you must agree to the Sales Terms and Conditions');
                return false;
            }
            validateSelection();

            var valid_g_captcha = validateform();
            if (valid_g_captcha == false) {
                alert("Invalid Captcha");
                return false;
            }
        }



        function checkEnableOthers() {
            if ((document.getElementById("ctl00_maincontent_chkother").checked == true) || (document.getElementById("ctl00_maincontent_chkother").checked == 'true') || (document.getElementById("ctl00_maincontent_chkother").checked == 1)) {
                document.getElementById("ctl00_maincontent_txtothers").disabled = false;
            }
            else {
                document.getElementById("ctl00_maincontent_txtothers").disabled = true;
            }
        }

        function checkselectedlist() {
            document.getElementById('<%= ErrorStatusHiddenField.ClientID %>').value = "1";
            return true;
        }

        function validateSelection() {
            
            if (checkselectedlist() == true) {
                if (document.getElementById('<%= chkterms.ClientID %>').checked == true) {
                    // document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                    document.getElementById('<%= lblCheckTerms.ClientID %>').style.display = '';
                    //document.getElementById("ctl00_maincontent_ctl00_btnsubmit").style.fontWeight = "bold";
                    //document.getElementById("ctl00_maincontent_ctl00_btnsubmit").style.backgroundColor = "#ff0000";

                    //document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
                    document.getElementById('<%= lblCheckTerms.ClientID %>').childNodes[0].nodeValue = "*";
                    return true;
                }
                else {
                    // document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                    document.getElementById('<%= lblCheckTerms.ClientID %>').style.display = '';
                    //document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "Required";
                    document.getElementById('<%= lblCheckTerms.ClientID %>').childNodes[0].nodeValue = "Required";
                    return false;
                }
            }
            else {
                //document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                document.getElementById('<%= lblCheckTerms.ClientID %>').style.display = '';
                //document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
                document.getElementById('<%= lblCheckTerms.ClientID %>').childNodes[0].nodeValue = "*";
                document.getElementById('<%= chkterms.ClientID %>').checked = false;

                return false;
            }
        }


        function Forgotpasswordhref() {

            window.location.href = 'Forgotpassword.aspx';
          
        }

      
    </script>
<style type="text/css">
    .HyperLinkStyle
    {
        font-family: Arial, Tahoma, Arial Unicode MS;
        font-size: 11px;
        color: #0099ff;
        cursor: pointer;
        text-decoration: none;
    }
     .modalBackground
{
    background-color: #000000;
    filter: alpha(opacity=70);
    opacity: 0.7;
}
</style>
<div class="container">
<%--<asp:UpdatePanel ID="updpnllogin" runat="server" UpdateMode="Conditional"   >
<contenttemplate>--%>
        <script language="javascript" type="text/javascript">
            Sys.Application.add_load(chkPB)
            </script>

<div class="col-sm-20">
          <ul class="breadcrumb " id="mainbredcrumb">
              <li><a href="/home.aspx">Home</a></li>
              <li class="active">Login/Register</li>
          </ul>
    </div>
    <div class="clearfix"></div>
    <div class="col-sm-20">
    <asp:Label ID="RegSucess" Visible="false" runat="server" Text ="<%$ Resources:login,lbRegSucess%>" Class="alert alert-success"></asp:Label>
    </div>

<div class="col-lg-10 col-md-10 col-sm-20 cus_login login_height">
 <asp:Panel  ID="updpnlloginPanel2" runat="server" DefaultButton="cmdLogin">
     <%
        string image1 = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/Order__info.png";
     %>
 <div class="customer_login">Customer Login <img class="pull-right" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/cross_blu.png"/></div>
 <p class="padding_top40 padding_left">Please Sign In using your Email ID or User Name and Password below:</p>
 <div class="col-lg-17 col-sm-17 col-xs-17 signup_inner">

  <div class="form-group">
    <label class="font_normal margin_bottom_15" for="exampleInputEmail1">User Name</label>
   <%-- <input type="email" placeholder="Full Name" id="txtUserName" class="form-control checkout_input"/>--%>
   <asp:TextBox ID="txtUserName"  autocomplete="off" runat="server" class="form-control checkout_input"  type="text" placeholder="Username" size="25" MaxLength="50"  onkeyup="javascript:keyboardup(this);"  ></asp:TextBox> <%--onkeyup="javascript:keyboardup(this);"--%>
   <%-- <span class="mandatory">Enter User Name</span>--%>
  </div>
  <div class="form-group">
    <label class="font_normal margin_bottom_15" for="exampleInputPassword1">Password</label>
   <%-- <input type="password" placeholder="Password" id="txtPassword" class="form-control checkout_input"/>--%>
   <asp:TextBox ID="txtPassword"   class="form-control checkout_input" runat="server" TextMode ="Password"  size="25" MaxLength="40"  onFocus="txtpassword_onFocus();"   placeholder="Password" ></asp:TextBox>
   <%-- <span class="mandatory">Enter Email</span>
    <span class="mandatory">Enter Valid Email</span>--%>
  </div>
  <div class="form-group">
<span class="pull-left font_12"><%--<a class="gray_40" id="lnkForgotPWDPage" href="">Forgot Password ?</a>--%>
<asp:HyperLink ID="lnkForgotPWDPage" runat="server" class="gray_40" onclick="javascript:ForgotLinkPage();" style="cursor:pointer;">Forgot Password ?</asp:HyperLink></span>
<span class="pull-right font_12 mob_mar_top keepmelog">Keep me logged in this computer  <input id="chkKeepme" name="chkKeepme" type="checkbox" runat="server" class="check_box"/><input type="checkbox" name="chkShopCart" id="chkShopCart"  visible="false" style="border-style:none;" runat="server" /></span>
<asp:HyperLink ID="HyperLink1" runat="server" CssClass="HyperLinkStyle" Visible="false">Password </asp:HyperLink>
</div>
<div class="clearfix"></div>
<div class="loginerror"><asp:Label ID="lblErrMsg" runat="server"  Text="" Class="mandatory" ></asp:Label> </div>
  
  </div>
  <%--<button class="btn btn-primary-green signup_inner reg_sub mob_width100" runat ="server" OnClick="cmdLogin_Click" OnClientClick="return passwordcheck();"  id="buttonlogin">Submit</button>--%>
  <asp:Button ID="cmdLogin" class="btn btn-primary-green signup_inner reg_sub mob_width100" text="Submit" runat ="server" OnClick="cmdLogin_Click" ValidationGroup="LoginValidation"/>
   <asp:HyperLink ID="lnkResetPassword" CssClass="HyperLinkStyle" runat="server" Visible="false">Please Reset Your Password</asp:HyperLink>
<%-- <asp:Button ID="btntest" runat="server" Text="test" OnClick="btntest_Click" />--%>
  </asp:Panel>
</div>
    <div class="col-lg-10 col-md-10 col-sm-20 new_cus_bg">
      <asp:Panel  ID="updpnlloginPanel1" runat="server" DefaultButton="btnsubmit">  
    <div class="new_customer ">New Customer Sign Up
        <%
            string image2 = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/cross_green.png";
        %>
    <img class="pull-right" src="<%=image2 %>" alt=""/>
    </div>
    <div class="col-sm-20">
 <p class=" padding_left_right_15 padding_top40 paddlft_20">Create an account if you are not already registered with Wagner.
 </p></div>
 <div class="col-lg-17 signup_inner">

  
  <div class="form-group">
      <label class="font_normal margin_rgt15" for="exampleInputEmail1">User Type</label>

    <label class="radio-inline padd_top0">
  	<%--<input type="radio" value="option1" class="margin_lft15" id="RBPersonal" runat="server" name="RBPersonal" Checked="true" onclick="javascript:chkPB();"> Personal--%>
    <asp:RadioButton ID="RBPersonal"   runat="server" GroupName="X"  Text="Personal" Checked="true" onclick="javascript:chkPB();" />

</label>

	<label class="radio-inline padd_top0">
 <%-- <input type="radio" class="margin_lft15" value="option2" id="RBBusiness" runat="server" name="RBBusiness" onclick="javascript:chkPB();"> Business--%>
  <asp:RadioButton ID="RBBusiness" runat="server"  GroupName="X" Text="Business" onclick="javascript:chkPB();" />
  
</label>
  </div>
  
</div>
<div id="tblbus">
<div class="col-lg-20">
 	<h4 class="regformhead">Company Details</h4>
 </div>
 <div class="col-lg-20 register-panel">
  <div class="form-group" id="Cmp">
    <label class="col-sm-6 control-label" for="inputEmail3">Company / Account Name<span></span></label>
    <div class="col-sm-14">
      <%--<input type="email" id="inputEmail3" class="form-control checkout_input">--%>
      <asp:TextBox runat="server" ID="txtcompname" Text=""  class="form-control checkout_input"  MaxLength="50" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
    </div>
     
  </div>
  <div class="form-group" id="CmpNo">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">ABN / ACN / Company Number<span></span></label>
    <div class="col-sm-14">
      <%--<input type="email" id="inputEmail3" class="form-control checkout_input">--%>
      <asp:TextBox runat="server" ID="txtcompno" Text="" class="form-control checkout_input" MaxLength="20" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
    </div>
  </div>	
</div>
</div>
<div class="col-lg-20">
 	<h4 class="regformhead">Contact Details</h4>
 </div>
 <div class="col-lg-20 register-panel">
 <div class="form-group">
    <label class="col-sm-6 control-label" for="inputEmail3">First Name<span>*</span></label>
    <div class="col-sm-14">
     <%-- <input type="email"  class="form-control checkout_input" MaxLength="20" id="txtfname" onkeyup="javascript:keyboardup(this);"/>--%>
     <asp:TextBox runat="server" ID="txtfname" Text="" class="form-control checkout_input"  MaxLength="20" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:RequiredFieldValidator ID="rfvfname" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name"  ControlToValidate="txtfname"></asp:RequiredFieldValidator>
    </div>
     
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Last Name<span>*</span></label>
    <div class="col-sm-14">
      <%--<input type="email" id="txtlname" class="form-control checkout_input" MaxLength="20" onkeyup="javascript:keyboardup(this);"/>--%>
      <asp:TextBox runat="server" ID="txtlname" Text="" class="form-control checkout_input" MaxLength="20" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:RequiredFieldValidator ID="rfvlname" runat="server" Class="mandatory"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name"   ControlToValidate="txtlname"></asp:RequiredFieldValidator> 
    </div>
   
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Phone<span>*</span></label>
    <div class="col-sm-14">
      <%--<input type="text" id="txtphone" class="form-control checkout_input" MaxLength="16" onkeyup="javascript:keyboardup(this);"/>--%>
      <asp:TextBox runat="server" ID="txtphone" Text="" class="form-control checkout_input" MaxLength="16" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvphone" runat="server" Class="mandatory"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Phone"  ControlToValidate="txtphone"></asp:RequiredFieldValidator>
<asp:FilteredTextBoxExtender ID="ftePhone" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtphone" />
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Mobile/Cell phone</label>
    <div class="col-sm-14">
      <%--<input type="text" id="txtMobile" class="form-control checkout_input" MaxLength="16" onkeyup="javascript:keyboardup(this);" />--%>
      <asp:TextBox runat="server" ID="txtMobile" Text="" class="form-control checkout_input" MaxLength="16" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobile" />
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Fax</label>
    <div class="col-sm-14">
      <%--<input type="email" id="txtfax" class="form-control checkout_input" MaxLength="16" onkeyup="javascript:keyboardup(this);"/>--%>
      <asp:TextBox runat="server" ID="txtfax" Text="" class="form-control checkout_input" MaxLength="16" onkeyup="javascript:keyboardup(this);"></asp:TextBox> 
      <asp:FilteredTextBoxExtender ID="ftefax" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtfax" />
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label martop0" for="inputEmail3">Subscribe Mail</label>
    <div class="col-sm-14">
     <%-- <input type="checkbox" value="option1" ID="chkissubscribe" Checked="true"/>--%>
     <asp:CheckBox ID="chkissubscribe" Checked="true"  runat="server"/>
    </div>
    <div class="clear"></div>
  </div>
  <div class="col-sm-20">
  <h4 class="regformhead">Login Details</h4>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">E-Mail<span>*</span></label>
    <div class="col-sm-14">
     <%-- <input type="email" id="txtemail" class="form-control checkout_input" MaxLength="55" onkeyup="javascript:keyboardup(this);"/>--%>
      <asp:TextBox runat="server" ID="txtemail" Text="" class="form-control checkout_input"  MaxLength="55" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
     <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="mandatory"   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email" ControlToValidate="txtemail"></asp:RequiredFieldValidator>
  <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtemail"   ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Confirm E-Mail<span>*</span></label>
    <div class="col-sm-14">
      <%--<input type="email" id="txtcemail" class="form-control checkout_input"  MaxLength="55" onkeyup="javascript:keyboardup(this);"/>--%>
    <asp:TextBox runat="server" ID="txtcemail" Text="" class="form-control checkout_input"  MaxLength="55" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
    <asp:CompareValidator ID="CompareValidator2" CssClass="mandatory" ControlToValidate="txtcemail" ControlToCompare="txtemail" runat="server" ErrorMessage="Confirm Email and Email should be same" ValidationGroup="Mandatory" Display="Dynamic" ></asp:CompareValidator>
    <asp:RequiredFieldValidator ID="rfvcemail" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Email" ControlToValidate="txtcemail"></asp:RequiredFieldValidator> 
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Password<span>*</span></label>
    <div class="col-sm-14">
      <%--<input type="email" id="Txt1password" class="form-control checkout_input" TextMode="Password"  MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');"/>--%>
      <asp:TextBox runat="server" ID="Txt1password" Text="" class="form-control checkout_input"  TextMode="Password"  MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
<asp:RegularExpressionValidator ID="RegularExpressionValidator4" CssClass="mandatory" Display="Dynamic"  ControlToValidate="Txt1password"  ValidationGroup="Mandatory"  ValidationExpression="^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"  runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Password" ControlToValidate="Txt1password"></asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Confirm Password<span>*</span></label>
    <div class="col-sm-14">
    <%--  <input type="email" id="TxtConfirmPassword" class="form-control checkout_input" TextMode="Password"  MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"/>--%>
    <asp:TextBox runat="server" ID="TxtConfirmPassword" Text="" class="form-control checkout_input" TextMode="Password" 
 MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
       <asp:CompareValidator ID="CompareValidator3"  Class="mandatory" ControlToValidate="TxtConfirmPassword" ControlToCompare="Txt1Password"  runat="server" ErrorMessage="Confirm Password and Password should be same" ValidationGroup="Mandatory"  Display="Dynamic"></asp:CompareValidator>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Password" ControlToValidate="TxtConfirmPassword"></asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="col-sm-20">
   <h4 class="regformhead">Shipping Address Details</h4>
   </div>
   <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Street Address<span>*</span></label>
    <div class="col-sm-14">
     <%-- <input type="email" id="txtsadd" class="form-control checkout_input" MaxLength="30" onkeyup="javascript:keyboardup(this);"/>--%>
     <asp:TextBox runat="server" ID="txtsadd" Text="" class="form-control checkout_input"   MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Street Address"   ControlToValidate="txtsadd"></asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Address Line</label>
    <div class="col-sm-14">
     <%-- <input type="email" id="txtadd2" class="form-control checkout_input" MaxLength="30" onkeyup="javascript:keyboardup(this);"/>--%>
     <asp:TextBox runat="server" ID="txtadd2" Text="" class="form-control checkout_input"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Suburb / Town<span>*</span></label>
    <div class="col-sm-14">
     <%-- <input type="email" id="txttown" class="form-control checkout_input" MaxLength="30" onkeyup="javascript:keyboardup(this);"/>--%>
     <asp:TextBox runat="server" ID="txttown" Text="" class="form-control checkout_input"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
      <asp:RequiredFieldValidator ID="rfvtown" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Suburb/Town"  ControlToValidate="txttown"></asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">State Province<span>*</span></label>
    <div class="col-sm-14">
     <%-- <select class="form-control"><option>Select</option></select>--%>
     <asp:TextBox runat="server" ID="txtstate" Text="" CssClass="form-control checkout_input" MaxLength="20" Visible="false" onkeyup="javascript:keyboardup(this);"></asp:TextBox>

      <asp:DropDownList ID="drpState" runat="server" class="form-control" Visible="true"> </asp:DropDownList>
      <asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter State/Province" Enabled="false" ControlToValidate="txtstate"></asp:RequiredFieldValidator>
   <asp:RequiredFieldValidator ID="rfvddlstate" runat="server" Text="Select State/Province" ErrorMessage="Required" ControlToValidate="drpState" ValidationGroup="Mandatory" InitialValue=""></asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group" id="aucust" runat="server">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Postal / Zip Code<span>*</span></label>
    <div class="col-sm-14">
    <asp:TextBox runat="server" ID="txtzip" Text="" class="form-control checkout_input" MaxLength="10" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvzip" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Post/Zip Code"  ControlToValidate="txtzip"></asp:RequiredFieldValidator>&nbsp;&nbsp;
    <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890"  TargetControlID="txtzip" />
    </div>
    <div class="clear"></div>
  </div>
   <div class="form-group" id="intercust" runat="server">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Postal / Zip Code<span>*</span></label>
    <div class="col-sm-14">
     <asp:TextBox runat="server" ID="txtzip_inter" Text="" class="form-control checkout_input" MaxLength="10" ></asp:TextBox>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Post/Zip Code"  ControlToValidate="txtzip_inter"></asp:RequiredFieldValidator>&nbsp;&nbsp;
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">Country</label>
    <div class="col-sm-14">
      <%--<select class="form-control"><option>Select</option></select>--%>
      <asp:DropDownList ID="drpCountry" runat="server" class="form-control" onselectedindexchanged="drpCountry_SelectedIndexChanged" 
          AutoPostBack="True"> </asp:DropDownList>
    </div>
    <div class="clear"></div>
  </div>
  <div class="col-sm-20">
   <h4 class="regformhead">Submit Form</h4>
   </div>
  <%-- <p>Form Verify. Please enter text shown below: <span style="color:red">*</span></p>--%>
   <div class="form-group reg_captcha">
    <div class="col-sm-5 martop10">
   <div id="recaptcha1" ></div>
   </div>
    <%--<label class="col-sm-6 control-label font_normal padding_top" for="inputEmail3">
    <cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" CaptchaBackgroundNoise="Low" width="144px" runat="server" CustomValidatorErrorMessage="Invalid Verification Code" CaptchaMaxTimeout="4000" CaptchaMinTimeout="4" />
    </label>
    <div class="col-sm-5 martop10">
        <asp:TextBox ID="cText" runat="server" value="" class="form-control width_size margin_top_20" ></asp:TextBox>
        <span class="mandatory">
        <asp:Label ID="cVerifyMsg" runat="server" ForeColor="Red" CssClass="mandatory" Text="" Visible="false" />
        </span>
     
    </div>--%>
    <div class="clear"></div>
  </div>
  <div class="form-group col-lg-20">     <%--onclick="javascript:checkEnableSubmit();" --%>
    <input id="chkterms" type="checkbox" runat="server"  onclick="javascript:validateSelection();" class="pull-left"/>
   <span class="padding_left"> I have read and understand</span><%--<asp:LinkButton ID="myLink" class="tandc" ToolTip="Sales Terms and Conditions" Text="Sales Terms and Conditions" runat="server" style="display:none;"  />--%>&nbsp;&nbsp; <a class="tandc"  data-target=".lgn-orderinfo" data-toggle="modal" role="button" href="#">Sales Terms and Conditions</a><asp:Label ID="lblCheckTerms" runat="server" Class="mandatory" Text="*" Visible="false" />
   
   
    <div class="clear"></div>
    
  </div>
  <div class="form-group col-lg-20">
   <%-- <button type="button" data-target=".bs-example-modal-lg" data-toggle="modal" class="btn btn-primary-green reg_sub mob_width100" >Submit</button>--%>
     <asp:Button ID="btnsubmit" data-target=".bs-example-modal-lg" data-toggle="modal" class="btn btn-primary-green reg_sub mob_width100" Text="Submit" 
                 runat="server" ValidationGroup="Mandatory"   OnClick="btnsubmit_Click" 
                 OnClientClick="return checkEnableSubmit()" UseSubmitBehavior="true" />  
                
    <div class="clear"></div>
  </div>
 </div>
 </asp:Panel>
    </div>
             <asp:Label ID="lblhid" runat="server" Text="" ></asp:Label>
             <asp:Label ID="lblhid1" runat="server" Text="" ></asp:Label>
             <asp:Panel ID="pnlChgPassword" runat="server" Width="300px" Style="display:none">
             <table border="0" width="100%" cellspacing="0" cellpadding="3" bgcolor="black" style="border:1px;border-color:Black">
             <tr><td height="28" colspan="2" style="background-color:#5CBBE9;"><table width="260" border="0" align="left" cellpadding="0" cellspacing="0">
             <tr><td align="left" height="30" class="tx_6" width="90%">&nbsp;&nbsp;&nbsp;CHANGE PASSWORD</td><td align="right">
               <%
                   string image2 = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/ico_11.gif";
                %>
             <img src="<%=image2%>" width="14" height="17"></td></tr></table></td></tr>
             <tr><td bgcolor="white" valign="top"><asp:Label ID="lblNewPassword" Class="lblStaticSkin" runat="server" Text="New Password" ></asp:Label></td><td bgcolor="white"><asp:TextBox ID="txtchgPassnew1" Class="textSkin" MaxLength="50" runat="server" TextMode="Password" /><br />
             <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic" ControlToValidate="txtchgPassnew1"   ValidationExpression="^[a-zA-Z0-9\s]{6,24}$" runat="server" ErrorMessage="Password must contain alphabet and numeric,and length should be 6 to 24"></asp:RegularExpressionValidator>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" ControlToValidate="txtchgPassnew1"   ValidationExpression=".*[0-9].*"  runat="server" ErrorMessage="Password must contain one numeric"></asp:RegularExpressionValidator>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic" ControlToValidate="txtchgPassnew1"   ValidationExpression=".*[a-zA-Z].*"  runat="server" ErrorMessage="Password must contain one alphabet"></asp:RegularExpressionValidator>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="New Password Required"  ControlToValidate="txtchgPassnew1" ></asp:RequiredFieldValidator></td></tr>
              <tr><td bgcolor="white" valign="top"><asp:Label ID="lblConfirmPassword" Class="lblStaticSkin" runat="server"  Text="Confirm Password"></asp:Label></td>
              <td bgcolor="white"><asp:TextBox ID="txtchgCPass" Class="textSkin" MaxLength="50" Textmode="Password" runat="server" /><br />
              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required" ControlToValidate="txtchgCPass" ></asp:RequiredFieldValidator><br />
             <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match"  ControlToCompare="txtchgPassnew1" ControlToValidate="txtchgCPass" ValueToCompare="String"></asp:CompareValidator>
              <asp:Label ID="lblCMsg1"  runat="server" /><br/></td></tr>
              <tr><td align="center" bgcolor="white"><asp:Button ID="btnOk" class="chg_boton" UseSubmitBehavior="true" runat="server" Text="Ok" OnClick="btnOk_Click"  CausesValidation="true"/></td>
              <td align="center" bgcolor="white"><asp:Button ID="btnCancel" class="chg_boton"  UseSubmitBehavior="false"  runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False" /></td> </tr>
              <tr><td align="center" bgcolor="white" colspan="2"><asp:Label ID="lblCMsg" runat="server" Class="lblErrorSkin"  Font-Bold="true" Text=""></asp:Label></td></tr></table>
              </asp:Panel>
 <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
              <asp:ModalPopupExtender ID="ChgPassPop" PopupControlID="pnlChgPassword" BackgroundCssClass="modalBackground"  DropShadow="true" runat="server" TargetControlID="lblhid"> 
              </asp:ModalPopupExtender>
              <asp:Panel ID="PnlAdmin" runat="server" Style="display:none">
              <table width="514" border="0" cellpadding="0" cellspacing="0" style="border:2px solid #CCCCCC;background-color:#ffffff;">
    <tr><td colspan="2" valign="top" style="padding:20px;"><strong class="txt_18 blue1">Welcome to WES online store.</strong>
      <p class="blue1 txt_14"><strong>New Online Account User Setup</strong></p>
      <p>We have detected that your account has multiple email addresses associated to it.</p>
      <p>WES online web site supports multiple user logins from your company. Your account will require at least one
      person in your company to be set as an Admin user so that user permission levels and adding, editing and
      deleting user’s can be configured by the Admin user.</p>
      <p>Until this defined you will not be able to place orders, however you will be able to browse the website.</p>
      <p>Please download the PDF form from below and Fax this back to us so that an Admin user from your company
      can be assigned. Once done, the Admin user will be able to configure user login’s for people within your own
    company if it is required.</p></td></tr>
  <tr><td width="248" height="49" valign="top" style="padding-left:20px;"><a href="#">
       <%
           string image3 = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/dld_pdf_form.png";
        %>
      <img src="<%=image3%>" width="197" alt="" height="40" /></a></td>
    <td width="266" valign="middle"><b><asp:HyperLink ID="hlink" CssClass="txt_12 blue" runat="server" NavigateUrl="~//Home.aspx">Continue Browsing Web Site</asp:HyperLink></b></td></tr></table>
    </asp:Panel>
   <asp:ModalPopupExtender ID="ShowAdminAlert"  PopupControlID="pnlAdmin" BackgroundCssClass="modalBackground"  DropShadow="true" TargetControlID="lblhid1" runat="server">
    </asp:ModalPopupExtender>
  
   <asp:Label ID="lblConfirm" Text="" runat="server"></asp:Label> 
<%--<asp:Panel ID="pnlTAC" runat="server" Width="650px"  Style="display:none; ">
<a href = "javascript:Hidepopup()" class="testbutton" >
</a>
<div class="boxfull" style="width:575px;height:500px;overflow:scroll;">
<table width="96%" border="0" cellpadding="0" cellspacing="0">
  <tr>
  <td align="center" valign="top">
   <table width="100%" border="0" cellpadding="0" cellspacing="0">

  <tr>
  <td height="85" valign="top" align="left">
  <table width="100%" border="0" cellpadding="0" cellspacing="0">
       
        <tr><td width="100%" height="85" valign="top">
            <p class="bold_txt2">Terms &amp; Conditions</p>
            <p>Find out more about Wagner. If you can't find the information you need, please use the Make an Enquiry page or phone 1100 13 933 and get help from a Customer Service representative.</p>
          </td></tr> </table>
          </td>
          </tr>
  <tr>
  <td height="100%" valign="top"><table width="100%" border="0" cellpadding="0" cellspacing="0">
    
        <tr><td height="28" colspan="3" class="tab6" align="left"><strong>GENERAL INFORMATION AND CONDITIONS </strong> </td></tr>
        <tr><td width="6" height="6" valign="top" class="cr_11"></td> <td width="708" valign="top" class="bdr_top" height="6"></td><td width="6" valign="top" height="6" class="cr_13"></td></tr>
        <tr><td height="314" valign="top" class="bdr_left">

        </td>
          <td valign="top" style="text-align:justify">
            <p><strong>WAGNER ELECTRONICS:</strong>  Is an independent Australian organisation dedicated to the Electronics Industry. Our aim is to give you the BEST possible service at COMPETITIVE prices.</p>
            <p><strong>THE PRODUCTS</strong>:  Goods offered are from reliable and internationally known manufacturers and are of the highest quality. Data and specifications are those quoted by our suppliers which we believe to be correct.</p>
            <p><strong>PRICES:</strong>  This price list supersedes all previous price lists and although correct at the time of printing, prices are subject to change without notice. All prices INCLUDE GST.</p>
            <p><strong>DELIVERY</strong>:  A delivery and handling charge applies to all orders. Please note that goods are sent at the purchasers risk. Minimum surface mail charge is &#36;6.90. Parcels over 500g attract parcel rates and in most cases the mail charges are more than courier. Therefore, for heavier or urgent orders we recommend that the courier service be used. In most cases they deliver on the next working day, thus courier can be both convenient and cost effective.</p>
            <p><strong>PICKUP</strong>:  Wagner Electronics is open 8:30 to 5.00 Mon-Fri, 9:00 to 4:00 on Sat.</p>
            <p><strong>LOCAL:</strong>  (Sydney Metropolitan Road Service) &#36;6.90. This is an overnight service.</p>
            <p><strong>LOCAL SAME DAY:</strong>  (Sydney Metro') &#36;8.50. Orders must be received before 10AM. Delivery is between 2PM and 5.30PM. Please state clearly on your order &quot;SAME DAY DELIVERY&quot;.</p>
            <p><strong>COUNTRY and INTERSTATE:</strong>  Air service is &#36;9.90 up to 3Kg.</p>
            <p><strong>HEAVY PARCELS:</strong>  (Above 3Kg) will be sent by road freight at no extra charge. Please allow 2-3 working days for delivery. Some country areas may take up to 7 days.</p>
            <p><strong>DANGEROUS GOODS:</strong>   Orders with aerosols or isopropanol products must be sent by road.</p>
            <p><strong>BACK ORDERS:</strong>   Goods not in stock will be automatically back ordered unless otherwise requested.</p>
            <p><strong>CLAIMS: </strong>   The use of our  products is totally beyond our control or supervision. We therefore  cannot accept any responsibility for losses or consequential damage to  any goods or equipment. A general Three month warranty applies to most  products from date of invoice. Manufacturers, however, may vary this  period at their discretion. PLEASE NOTE. No warranty applies to some  parts as faults not correctly diagnosed or other faults in associated  circuitry can damage newly fitted parts, e.g. semiconductors, etc. <br /><br />
              <strong>GOODS RETURNED: </strong>   Please, it is essential you obtain authorization before returning any  goods. All goods to be returned must be freight prepaid and accompanied  with a copy of the original purchase invoice. Faulty goods will be  replaced only. Not credited. Goods purchased incorrectly or no longer  required will not be accepted for credit. <br />
              <br />
              <strong>OWNERSHIP OF GOODS: </strong>   Ownership of all goods and materials supplied by WAGNER Components  remains the property of WAGNER Components, in accordance with the  retention of title clause, until all goods supplied are paid for by the  customer. <br />
              <br /> <strong>ERRORS/OMISSIONS: </strong>   Due to the size of this catalogue and the thousands of products it  contains, it is possible that mistakes exist. All care has been taken  to eliminate errors, but no responsibility can be accepted. Therefore,  we recommend that this catalogue be used as a guide only. All prices  are correct at the time of printing, but may change without notice.  Please tell us if you discover any discrepancies. We will advise you of  corrections as they become available. See WAGNER NEWS</p></td>
          <td valign="top" class="bdr_right"> <img src="Images/Cr_15.png" width="6" height="10"  alt=""/> </td></tr>
        <tr><td height="6" valign="top" class="cr_16"></td><td valign="top" class="bdr_bottom" width="10" height="6"></td>
          <td valign="top" class="cr_18" width="6" height="6"></td></tr></table></td></tr><tr><td>&nbsp;</td></tr></table>
          </td>
          </tr>
          </table>
</div> 
 </asp:Panel>--%>

 <div style="background: rgba(0, 0, 0, 0.6) none repeat scroll 0% 0%; display: none; padding-right: 17px;" aria-hidden="true" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" class="modal fade lgn-orderinfo">
                  <div class="modal-dialog">
                        <div class="modal-content" style="height:500px;overflow-y:auto;">
                          <div class="modal-header green_bg">
                            <h4 id="H1" class="text-center">
                                 <%
                                  string image1 = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/Order__info.png";
                                   %>
                            <img class="popsucess" alt="img" src="<%=image1 %>"/>Terms and Conditions</h4>
                          </div>
                          <div class="modal-body">
                              <div class="col-lg-20 text-center col-sm-10 col-md-5">
<div class="privacypolicy">
  <h2 style="font-size: 12px;font-weight: bold;">TERMS AND CONDITIONS</h2>
  <p style="font-size: 12px;">Find out more about Wagner. If you can't find the information you need, please use the Make an Enquiry page or phone 1100 13 933 and get help from a Customer Service representative.</p>
  <br />
      <div class="centered-block">
        <h3 class="bld_fnt blue_text" style="font-size: 13px;">GENERAL INFORMATION AND CONDITIONS </h3>
      </div>

      <h3 style="font-weight: bold; font-size: 12px;">WAGNER ELECTRONICS:</h3>
      <p style="font-size: 12px;">Wagner Electronics is an Independent Australian Company dedicated to the Electronics Industry. Our aim is to give you the Best possible service at Competitive Prices.</p>

      <h3 style="font-weight: bold; font-size: 12px;">THE PRODUCTS:</h3>
      <p style="font-size: 12px;"> Goods supplied are sourced from numerous sources locally and overseas. It is our policy to purchase from reputable suppliers in order to assure that the products are of the highest quality.</p>

      <h3 style="font-weight: bold; font-size: 12px;">PRICES:</h3> 
      <p style="font-size: 12px;">The prices shown are current at the time of display however there are many factors which can effect cost prices including material cost rises, shortages and currency variations, etc. The intention is to maintain the published prices however this may not be possible in all cases. Prices are therefore subject to change without notice.</p>

      <h3 style="font-weight: bold; font-size: 12px;">OWNERSHIP OF GOODS:</h3>
       <p style="font-size: 12px;">Goods and Services supplied by Wagner Electronics remain the property of Wagner Electronics, in accordance with the retention of title clause, until goods supplied are paid for by the customer.</p>

      <h3 style="font-weight: bold; font-size: 12px;">LIABILITY:</h3> 
      <p style="font-size: 12px;">The use of the products purchased is beyond our control or supervision. We therefore cannot accept any liability for losses or consequential damage to any goods or equipment. Although WES does not manufacture the goods sold, we do ensure that the products are of the highest quality however in a rare event of a product being faulty, we will make a claim to our supplier on your behalf. Each supplier have their own policy on returned goods which range from repair to replacement after inspection.</p>

      <h3 style="font-weight: bold; font-size: 12px;">DANGEROUS GOODS:</h3> 
      <p style="font-size: 12px;">Orders with goods which are restricted, such as some batteries, aerosols or isopropanol products must be sent by road.</p>

      <h3 style="font-weight: bold; font-size: 12px;">BACK ORDERS:</h3> 
      <p style="font-size: 12px;">Goods not in stock will be automatically back ordered and shipped when available with your next order unless otherwise requested..</p>

      <h3 style="font-weight: bold; font-size: 12px;">GOODS RETURNED:</h3> 
      <p style="font-size: 12px;">Please, it is essential you obtain a Return Authorisation number ( RA ) before returning any goods. All goods to be returned must be freight prepaid and accompanied with a copy of the original purchase invoice. The RA number must be clearly written on the parcel. Parcels returned without an RA will not be accepted or will be returned. Faulty goods will be returned to our supplier for their determination. Faulty goods will be replaced or repaired only, not credited. Goods purchased incorrectly or no longer required are not able to be returned. For goods that are returned, they must be sent back in the original packaging and same as new condition they were received in; that is no price tags, shipping labels, tape / text marks, physical damage to box, etc. Please ensure that all items are returned including any parts, accessories and instructions that were included with the original product.</p>

      <h3 style="font-weight: bold; font-size: 12px;">ERRORS / OMISSIONS:</h3> 
      <p style="font-size: 12px;">Due to the extensive product range, it is possible that mistakes exist. All care has been taken to eliminate errors, but no responsibility can be accepted. Please tell us if you discover any discrepancies.</p>

      <h3 style="font-weight: bold; font-size: 12px;">AUSTRALIA WIDE DELIVERY *:</h3> 
      <p style="font-size: 12px;">Australia wide flat rate delivery charge of $9.90*
      *Additional charges may occur for Heavy / Large items (such as Server Racks) and non-standard delivery areas within Australia. Our sales team will notify you if there any additional chargers.</p>

      <h3 style="font-weight: bold; font-size: 12px;">PICK UPS:</h3> 
      <p style="font-size: 12px;">Goods can be picked up from our premises from 8.30AM to 5.00PM Monday to Friday and from 9AM till 4PM on Saturday. In order to avoid delays it is recommended that you place your order 2 hours prior to pick up.
      If you are picking up on Saturday please place your order on Friday.
      Important Details For Store Pick Up:
      <li style="font-size: 12px;">1. Please bring a printed copy of invoice when coming into store.</li>
      <li style="font-size: 12px;">2. Proof of Identity is required when picking up goods from our store. Please ensure you have the Credit Card you made the purchase with and your Driver License ID.</li>
      <li style="font-size: 12px;">3. The Name on the Credit Card Must Match Your Driver's License ID.</li>
      <li style="font-size: 12px;">4. You will not be able to pick up the goods from our store unless the Credit Card you made the purchase with and your Driver's License are shown OR if there if there is a mismatch between the in the name / details on your credit card and drivers licence details.</li>
      </p>
      <h3 style="font-weight: bold; font-size: 12px;">INTERNATIONAL SHIPPING:</h3> 
      <p style="font-size: 12px;">We ship internationally, once your order is submitted to us we will advise shipping cost by email and update your update order with freight cost.</p>
      <br />
           
</div>    
                              </div>                             
                          </div>
                          <div class="modal-footer clear border_top_none">
                           
                          </div>
                      </div>
                  </div>
    </div>
<%--<asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup"  BehaviorID="testTACpopup"    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None"   >
</asp:ModalPopupExtender>--%>
 <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none; visibility: hidden">
 </asp:Button>
<div id="PopupOrderMsg" align="center" runat ="server">
        <asp:Panel ID="ModalPanelRRAE" runat="server" CssClass="PopUpDisplayStyle">
                        <div class="modal-dialog">
                        
                        <div class="modal-content ">
                          <div class="close-selected">
                            
                             <asp:ImageButton ID="Button1" runat="server"
                            OnClick="btnClose_Click" OnClientClick="ForgotLinkPageClose();" />
                      </div>
                             <div class="modal-header green_bg">
                            <h4 id="myModalLabel" class="text-center">
                                 <%
                                  string image1 = System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString() + "images/Order__info.png";
                                   %>
                            <img class="popsucess" alt="" src="<%=image1 %>"/>Email Address Exists</h4>
                          </div>
                          <div class="modal-body">
                             
                              <div class="col-lg-20 text-center col-sm-10 col-md-5 mgntb20">
                                <p>The email address you are registering your account with exists already
                                <br /> Would you like to continue with ForgotPassword?
                                </p>
                              </div>                             
                            
                          </div>
                          <div class="modal-footer clear border_top_none">
                          <asp:Button ID="ForgotPassword" UseSubmitBehavior="true"  runat="server" Text="Forgotten Password"
                            Width="205px"  CssClass="btn primary-btn-green" OnClick="btnForgotPassword_Click" CausesValidation="true" OnClientClick="ForgotLinkPage();" />
                            
                           <asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="btn primary-btn-blue" OnClick="btnClose_Click" OnClientClick="ForgotLinkPageClose();" />
                            
                          </div>
                        </div>
                        </div>
</asp:Panel>
</div>
   <asp:HiddenField ID="ErrorStatusHiddenField" runat="server" />
<%--
 </contenttemplate>
    <triggers><asp:AsyncPostBackTrigger ControlID="cmdLogin" EventName="Click"/>
   <asp:AsyncPostBackTrigger ControlID="btnOk" EventName="Click" />
   <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
   </triggers>
 
</asp:UpdatePanel>--%>
</div>


