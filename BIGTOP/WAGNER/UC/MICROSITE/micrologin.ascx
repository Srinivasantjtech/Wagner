<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="micrologin.ascx.cs" Inherits="WES.UC.MICROSITE.micrologin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>--%>
<%--<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>--%>
<%--<link href="/css/MicroSitecss/Dynamicstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" rel="stylesheet" type="text/css" />--%>
<input id="hidpwd" name="hidpwd" type="hidden" runat="server" />
<input id="hfwidth" name="hfwidth" type="hidden" runat="server" value="1000"/>
<input id="hfquerystring" name="hfquerystring" type="hidden" runat="server" />
<script  type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=myCallBack&render=explicit" async defer></script>
<%--<script type="text/javascript">
    
//    $(document).on("pageload", function () {
//        alert("pageload event fired!");
//    });
    $(document).ready(function () {
        $(window).resize(function () {

            var varwidth = $(window).width();
            $("#" + '<%= hfwidth.ClientID %>').val(varwidth);
          //  alert(varwidth);

            if (varwidth > 760) {
                window.document.href = "/mlogin.aspx";
            }
        });
    });
       
</script>--%>
<script type="text/javascript" >

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

    function keyboardup(id) {
        var value = id.value
        id.value = value.replace(/'/, '`');
        Validate(id.value);
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

        var optc = document.getElementById('<%= RBPersonal.ClientID %>')
        if (optc != null) {

            var cm = document.getElementById("tblbus");

            if (optc.checked == true) {
                if (cm != null) cm.style.display = "none";


            }
            else {
                if (cm != null) cm.style.display = "block";

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
    $(document).ready(function () {
        if ($.browser.mozilla) {
            $("#<%=txtUserName.ClientID %>").keypress(keyboardActions);
            $("#<%=txtPassword.ClientID %>").keypress(keyboardActions);
            $("#<%=chkKeepme.ClientID %>").keypress(keyboardActions);
        } else {
            $("#<%=txtUserName.ClientID %>").keydown(keyboardActions);
            $("#<%=txtPassword.ClientID %>").keydown(keyboardActions);
            $("#<%=chkKeepme.ClientID %>").keydown(keyboardActions);
        }

    });
  </script>


    <script type="text/javascript">
        function updateObjectIframe(which) {
            document.getElementById('one').innerHTML = '<' + 'object id="foo" height="370px" width="400px" name="foo" type="text/html" data="' + which.href + '"><\/object>';
        } 

</script>
    <script type="text/javascript" language="javascript">
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
    </script>
    <script language="javascript" type="text/javascript">
        function ForgotLinkPage() {
            var mUser = document.getElementById("<%=txtUserName.ClientID%>");
            window.location.href = "ForgotPassword.aspx?loginName=" + mUser.value;
        }
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
                    //document.getElementById("ctl00_maincontent_ct100_btnsubmit").style.fontWeight = "bold";
                   // document.getElementById("ctl00_maincontent_ct100_btnsubmit").style.backgroundColor = "#ff0000";

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
     <div align="center">
         <asp:Label ID="RegSucess" Visible="false" runat="server" Text ="<%$ Resources:login,lbRegSucess%>" Class="lblResultSkin"></asp:Label>
</div>
<div class="container margin_top">
  <div class="row">

<%--<% if ((Convert.ToInt32(hfwidth.Value.ToString()) > 760) || (HttpContext.Current.Request.QueryString["status"].ToString() == "Login"))
   {   %>--%>
<div id="logindiv"  runat="server" class="col-lg-6 col-md-6 cus_login padding_left_right login_height">
 <div class="customer_login">Customer Login <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/cross_blu.png" class="pull-right"></div>
 <p class="text-center padding_top40">Please Sign In using your Email ID or User Name and Password below:</p>
 <div class="col-lg-10 mar_left_30 margin_top_20">
<%-- name="helpForm"  ng-app="" ng-controller="RegCtrl"  novalidate--%>

  <div class="form-group">
    <label for="exampleInputEmail1" class="font_normal margin_bottom_15">User Name</label>
    <input type="text" runat="server" class="form-control checkout_input" id="txtUserName" placeholder="User Name"  >
  </div>
  <div class="form-group">
    <label for="exampleInputPassword1" class="font_normal margin_bottom_15">Password</label>
    <input type="password" runat="server" class="form-control checkout_input" id="txtPassword" placeholder="Password" >
   
  </div>
  <div class="form-group">
<span class="pull-left font_12">
  <asp:HyperLink ID="lnkForgotPWDPage" runat="server" class="gray_40" NavigateUrl="/MForgotPassWord.aspx">Forget Password? </asp:HyperLink>

</span>
<span class="pull-right font_12 mob_mar_top">Keep me logged in this computer  

   <input type="checkbox"  id="chkKeepme" name="chkKeepme" class="check_box" runat="server" />
</span></span>
   <input type="checkbox" name="chkShopCart" id="chkShopCart"  visible="false" style="border-style:none;" runat="server" />
   <%-- <input type="text" class="form-control checkout_input" id="vali" placeholder="User Name"  name="vali" ng-maxlength=50 ng-model="vali" required  >--%>
<div class="clear"></div>
  </div>
  <div class="form-group">
       <asp:Label ID="lblErrMsg" runat="server"  Text="" Class="lblErrorSkin" ></asp:Label>
  
  </div>
  <%--<button  runat="server" 
  class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100" 
 OnClick="cmdLogin_Click" >Submit</button>--%>
<%-- ng-click=" save(helpForm)"--%>
 <%-- OnClientClick="return passwordcheck();" --%>
  <asp:Button ID="cmdLogin" runat="server"  OnClick="cmdLogin_Click"   UseSubmitBehavior="true"
   Text="Login"  class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100"  />
   

</div>
 </div>

<%--<% }%>

<% if ((Convert.ToInt32(hfwidth.Value.ToString()) > 760) || (HttpContext.Current.Request.QueryString["status"].ToString() == "Register"))
   {   %>--%>
<div id="registerdiv"  runat="server" class="col-lg-6 col-md-6 new_cus_bg padding_left_right">

 <div class="new_customer">New Customer Sign Up<img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/cross_green.png" class="pull-right"></div>
 <p class=" padding_left_right_15 padding_top40">Create an account if you are not already registered with Wagner.
</p>

<div class="col-lg-10 margin_top_20">


  
  <div>
      <label for="exampleInputEmail1" class="font_normal margin_bottom_15 mar_right_3">User Type</label>

    <label class="radio-inline-register" style="font-weight:normal"  > 
  
    <asp:RadioButton ID="RBPersonal" class="mar_left_30"  runat="server" GroupName="X"   Checked="true" onclick="javascript:chkPB();"/>

Personal

</label>
<label class="radio-inline-register" style="font-weight:normal" >
  <asp:RadioButton ID="RBBusiness" runat="server" GroupName="X"  class="mar_left_30"  onclick="javascript:chkPB();" />
Business
</label>
  </div>
  </div>
             <div id="tblbus" style="display:none;" class="col-lg-12">
          <h4 class="blue_color_text col-lg-12 font_15"><strong>Company Details</strong></h4>  
<div class="form-group">
                         <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Company / Account Name:
                <asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
                </label>
                       
 <div class="col-sm-8">
<asp:TextBox runat="server" ID="txtcompname" Text=""  Class="form-control checkout_input"   MaxLength="50" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
</div>
 <div class="clear"></div>
</div>

<div class="form-group">
      <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">ABN / ACN / Company Number:
      <asp:Label ID="Label13" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label></label>
  
      <div class="col-sm-8">

<asp:TextBox runat="server" ID="txtcompno" Text=""   MaxLength="20" onkeyup="javascript:keyboardup(this);"  Class="form-control checkout_input"></asp:TextBox>
<div class="clear"></div>
</div>
</div>
</div>

<h4 class="blue_color_text col-lg-12 font_15"><strong>Contact Details</strong></h4>
 <div class="col-lg-12 margin_top_20">
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">First Name
     <asp:Label ID="Label9" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
    </label>
   
    <div class="col-sm-8">
  
       <asp:TextBox runat="server" ID="txtfname" Text=""  MaxLength="20" 
       class="form-control checkout_input"
                         onkeyup="javascript:keyboardup(this);"  >
                         </asp:TextBox>
     <asp:RequiredFieldValidator ID="rfvfname" runat="server"   Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name"  ControlToValidate="txtfname"></asp:RequiredFieldValidator>
   
    </div>
     <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Last Name
     <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
    </label>
    <div class="col-sm-8">
      <asp:TextBox runat="server" ID="txtlname" Text=""  MaxLength="20" onkeyup="javascript:keyboardup(this);" class="form-control checkout_input" ></asp:TextBox>
     <asp:RequiredFieldValidator ID="rfvlname" runat="server"   Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name"   ControlToValidate="txtlname"></asp:RequiredFieldValidator> 
    </div>
  
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top" >Phone
     <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
    </label>
    <div class="col-sm-8">
      <asp:TextBox runat="server" ID="txtphone" Text="" class="form-control checkout_input"
MaxLength="16" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
  <asp:RequiredFieldValidator ID="rfvphone" runat="server" Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Phone"  ControlToValidate="txtphone">
</asp:RequiredFieldValidator>
      <asp:FilteredTextBoxExtender ID="ftePhone" 
runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" 
TargetControlID="txtphone" />
    </div>
    <div class="clear"></div>
  </div>
   <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Mobile / Cell phone</label>
 
    <div class="col-sm-8">
     <asp:TextBox runat="server" ID="txtMobile" Text="" class="form-control checkout_input"
MaxLength="16" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
     
        <asp:FilteredTextBoxExtender ID="fteMobile" runat="server" 
   FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobile" />
   
    </div>
    <div class="clear"></div>
  </div>
   <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Fax</label>
    <div class="col-sm-8">
    <asp:TextBox runat="server" ID="txtfax" Text="" class="form-control checkout_input" MaxLength="16" onkeyup="javascript:keyboardup(this);"></asp:TextBox>

   <asp:FilteredTextBoxExtender ID="ftefax" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtfax" />
    </div>
    <div class="clear"></div>
  </div>
   <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Subscribe Mail</label>
    <div class="col-sm-8">
      <input ID="chkissubscribe" Checked="true"  runat="server"  type="checkbox"  value="option1">
    </div>
    <div class="clear"></div>
  </div>
  <h4 class="blue_color_text col-lg-12 font_15"><strong>Login Details</strong></h4>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">E-Mail
     <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
     </label>
    <div class="col-sm-8">
      
<asp:TextBox runat="server" ID="txtemail"  type="email"    MaxLength="55" 
    class="form-control checkout_input" onkeyup="javascript:keyboardup(this);"  />
     <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email" ControlToValidate="txtemail"></asp:RequiredFieldValidator>               
 <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtemail"   ErrorMessage="Required" 
Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  
Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
  
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Confirm E-Mail
   
    <asp:Label ID="Label4" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
   </label>
    <div class="col-sm-8">
     <asp:TextBox  runat="server" ID="txtcemail"  type="email"      MaxLength="55" 
   class="form-control checkout_input" onkeyup="javascript:keyboardup(this);"  />
   <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txtcemail" ControlToCompare="txtemail" runat="server" ErrorMessage="Confirm Email and Email should be same" ValidationGroup="Mandatory" Display="Dynamic" ></asp:CompareValidator>
     <asp:RequiredFieldValidator ID="rfvcemail" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Email"
 ControlToValidate="txtcemail"></asp:RequiredFieldValidator>                
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Password
     <asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
     </label>
    <div class="col-sm-8">
    <asp:TextBox runat="server" ID="Txt1password" Text="" class="form-control checkout_input" TextMode="Password"  MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>

<asp:RegularExpressionValidator ID="RegularExpressionValidator4" Display="Dynamic"  ControlToValidate="Txt1password"  ValidationGroup="Mandatory"  ValidationExpression="^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"  runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Password" ControlToValidate="Txt1password"></asp:RequiredFieldValidator>


    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Confirm Password
     <asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
    
    </label>
    <div class="col-sm-8">
  <asp:TextBox runat="server" ID="TxtConfirmPassword1" Text="" class="form-control checkout_input" TextMode="Password" 
 MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
 <asp:CompareValidator ID="CompareValidator3" ControlToValidate="TxtConfirmPassword1" ControlToCompare="Txt1Password"  runat="server" ErrorMessage="Confirm Password and Password should be same" ValidationGroup="Mandatory"  Display="Dynamic"></asp:CompareValidator>
   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" 
ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Password" ControlToValidate="TxtConfirmPassword1"></asp:RequiredFieldValidator>

   
    </div>
    <div class="clear"></div>
  </div>
   <h4 class="blue_color_text col-lg-12 font_15"><strong>Shipping Address Details</strong></h4>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Street Address
    
     <asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
    </label>
    <div class="col-sm-8">
  
       <asp:TextBox runat="server" ID="txtsadd" Text="" 
  MaxLength="30" 
 onkeyup="javascript:keyboardup(this);" class="form-control checkout_input" ></asp:TextBox>
     <asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Street Address"   ControlToValidate="txtsadd"></asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Address Line</label>
    <div class="col-sm-8">
     
      <asp:TextBox runat="server" ID="txtadd2" Text="" class="form-control checkout_input"   MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Suburb / Town
     <asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
   </label>
    <div class="col-sm-8">
      <asp:TextBox  class="form-control checkout_input" runat="server" ID="txttown" Text=""  MaxLength="30" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
  <asp:RequiredFieldValidator ID="rfvtown" runat="server" 
 Class="vldRequiredSkin"   ErrorMessage="Required" 
 ValidationGroup="Mandatory" Display="Dynamic" 
 Text="Enter Suburb/Town"  ControlToValidate="txttown">
 </asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">State Province
    <asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label>
    </label>
    <div class="col-sm-8">
    <asp:TextBox runat="server" ID="txtstate" Text="" class="form-control checkout_input"  MaxLength="20" Visible="false" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
<asp:DropDownList ID="drpState" runat="server" Width="250px" 
          Class="form-control" Visible="true"> </asp:DropDownList>
      <asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter State/Province" Enabled="false" ControlToValidate="txtstate"></asp:RequiredFieldValidator>
   <asp:RequiredFieldValidator ID="rfvddlstate" runat="server" Text="Select State/Province" ErrorMessage="Required" ControlToValidate="drpState" ValidationGroup="Mandatory" InitialValue=""></asp:RequiredFieldValidator>
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Postal / Zip Code
     <asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" Visible="true" Text="*" Width="1px"></asp:Label></label>
    <div class="col-sm-8">
  
      <asp:TextBox runat="server" ID="txtzip" Text="" 
 class="form-control checkout_input"  MaxLength="10" 
onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
 
 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtzip" />
 
   <asp:RequiredFieldValidator ID="rfvzip" runat="server" Class="vldRequiredSkin"
 ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" 
 Text="Enter Post/Zip Code"  ControlToValidate="txtzip"></asp:RequiredFieldValidator>
 
    </div>
    <div class="clear"></div>
  </div>
  <div class="form-group">
    <label for="inputEmail3" class="col-sm-4 control-label font_normal padding_top">Country</label>
    <div class="col-sm-8">
     <asp:DropDownList ID="drpCountry" runat="server" 
          Width="250px"  Class="form-control" 
          onselectedindexchanged="drpCountry_SelectedIndexChanged" 
          AutoPostBack="True"> </asp:DropDownList>
    </div>
    <div class="clear"></div>
  </div>
   <h4 class="blue_color_text col-lg-12 font_15"><strong>Submit Form</strong></h4>
  <%-- <h4 class="font_14 col-lg-12 margin_top">Form Verify. Please enter text shown below:</h4>--%>


   


  <div class="form-group">
<%--  <label for="inputEmail3" class="col-sm-4 col-md-5 control-label font_normal padding_top">
         <cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" 
         CaptchaBackgroundNoise="Low"  runat="server" 
         CustomValidatorErrorMessage="Invalid Verification Code" 
         CaptchaMaxTimeout="4000" CaptchaMinTimeout="4"  />
    
    </label>
    
      <div   class="col-sm-8 col-md-4">
          <asp:TextBox ID="cText" runat="server" value="" Class="form-control width_size margin_top_20"  >
          </asp:TextBox>
          
          </div>
           <div   class="col-sm-8 col-md-4">
           <asp:Label ID="cVerifyMsg" runat="server" 
          ForeColor="Red" Text="" Visible="false" />
           </div>--%>
              <div   class="col-sm-8 col-md-4">
    <div id="recaptcha1" ></div>
    </div>
    <div class="clear"></div>
      </div>



   

    <div class="clear"></div>

  <div class="form-group col-lg-12">
         <%-- onclick="javascript:checkEnableSubmit();" --%>
     <input id="chkterms" type="checkbox" class="pull-left" runat="server" onclick="javascript:validateSelection();" />
       
   <span class="padding_left"> I have read and understand</span> 
    <asp:LinkButton ID="myLink" class="blue_color_text" style="color: #01AEF0;text-decoration: none;" ToolTip=" Sales Terms and Conditions" Text="Sales Terms and Conditions*" runat="server"  />
  
    <asp:Label ID="lblCheckTerms" runat="server" ForeColor="Red" Text="*" Visible="false" />
    <div class="clear"></div>
  </div>
  <div class="form-group col-lg-12">
  
                 <asp:Button ID="btnsubmit" class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100" Text="Submit"
                 runat="server" ValidationGroup="Mandatory"   OnClick="btnsubmit_Click" 
                 OnClientClick="return checkEnableSubmit()"  CausesValidation="true"  />  
  <%--  <button class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100"
     data-toggle="modal" data-target=".bs-example-modal-lg" type="button">Submit</button>--%>
   <%-- <button  class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100" 
    type="button">Submit</button>--%>
    <div class="clear"></div>
   
  </div>
  

  
  </div>
<%--  <% } %>--%>
   
                 
                 <asp:HyperLink ID="lnkResetPassword" CssClass="HyperLinkStyle" runat="server" Visible="false">Please Reset Your Password</asp:HyperLink>
                <div class="cl"></div>
                    
           
    <asp:Label ID="lblhid1" runat="server" Text="" ></asp:Label>
      </div>
    <asp:Panel ID="PnlAdmin" runat="server" Style="display:none">

           <%--   <table width="514" border="0" cellpadding="0" cellspacing="0" style="border:2px solid #CCCCCC;background-color:#ffffff;">
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
  <tr><td width="248" height="49" valign="top" style="padding-left:20px;"><a href="#"><img src="/images/MicroSiteimages/dld_pdf_form.png" width="197" alt="" height="40" /></a></td>
    <td width="266" valign="middle"><b>
    <asp:HyperLink ID="hlink" CssClass="txt_12 blue" runat="server" NavigateUrl="~//Home.aspx">Continue Browsing Web Site</asp:HyperLink>
    </b></td></tr></table>--%>
    <div  style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6);" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">

        <div class="modal-header blue_color padding_top padding_btm">
          <button aria-label="Close" data-dismiss="modal" class="close white_bg" type="button"><span aria-hidden="true">×</span></button>
          <h4 id="H1" class=" white_color font_weight modal-title">Welcome to WES online store.</h4>
         
        </div>
        <div class="modal-body">
         <h4 class="font_weight margin_top_none font_15">New Online Account User Setup</h4>
         <p>We have detected that your account has multiple email addresses associated to it.</p>
         <p>WES online web site supports multiple user logins from your company. Your account will require at least one person in your company to be set as an Admin user so that user permission levels and adding, editing and deleting user’s can be configured by the Admin user.</p>

<p>Until this defined you will not be able to place orders, however you will be able to browse the website.</p>

<p>Please download the PDF form from below and Fax this back to us so that an Admin user from your company can be assigned. Once done, the Admin user will be able to configure user login’s for people within your own company if it is required.</p>
<div class="modal-footer">
         <a href="" class="gray_40 font_weight"> <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/pdf.png" class="margin_right"></a> <asp:HyperLink ID="hlink" class="gray_40 font_weight" runat="server" NavigateUrl="~//Home.aspx" Text="Continue Browsing Web Site"/>
         </div>
        </div>
      </div>
  </div>
</div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup"  BehaviorID="testTACpopup"    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None"   >
</asp:ModalPopupExtender>

    <asp:ModalPopupExtender ID="modalPop" PopupControlID="ModalPanelRRAE" BackgroundCssClass="modalBackground"  BehaviorID="testTACpopup"    DropShadow="true" runat="server" TargetControlID="btnHiddenTestPopupExtender" 
    RepositionMode="None"   >
</asp:ModalPopupExtender>
<%-- <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none; visibility: hidden" data-toggle="modal" data-target=".bs-example-modal-sm">
 </asp:Button>--%>
  <button ID="btnHiddenTestPopupExtender"  runat="server" Style="display: none; visibility: hidden" class="btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100" 
  data-toggle="modal" data-target=".bs-example-modal-sm" type="button">Submit</button>


   <asp:ModalPopupExtender ID="ShowAdminAlert"  PopupControlID="pnlAdmin" BackgroundCssClass="modalBackground"  DropShadow="true" TargetControlID="lblhid1" runat="server">
    </asp:ModalPopupExtender>
      <asp:HiddenField ID="ErrorStatusHiddenField" runat="server" />
     
      <div id="PopupOrderMsg" align="center" runat ="server" style="right: 50%;position: absolute;top: 600px;z-index: 999;" >
 
        <asp:Panel ID="ModalPanelRRAE" runat="server">
        <div style="left: -632px; top: -200px; position: absolute; width: 1390px; height: 1800px; background-color: rgb(0, 0, 0); opacity: 0;"></div>
        <div  tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" style="background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.6);">
  <div class="modal-dialog modal-sm">
    <div class="modal-content">
      

        <div class="modal-header red_bg padding_top padding_btm">
        <asp:Button ID="Close" runat="server" Text="×" Width="3px"
                           class="close white_bg" OnClick="btnClose_Click" OnClientClick="ForgotLinkPageClose();" />
      <%--    <button aria-label="Close" data-dismiss="modal" class="close white_bg" type="button">
          <span aria-hidden="true">×</span></button>--%>
          <h4 id="H2" class=" white_color font_weight modal-title">Account already exists ! </h4>
         
        </div>
        <div class="modal-body padding_btm_20">
        <div class="col-lg-12"><img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/oops.png"></div>
        <div class="col-lg-12">
         <p class="margin_top line_height_24">The email address you are registering your account with exists already.                       </p>
         <p class="margin_top line_height_24 margin_bottom_30"> Would you like to continue with ForgotPassword?</p>
         <%--<a href="" class="green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100 height_34 padding_btm padding_top">ForgotPassword?</a>--%>
        <asp:Button ID="ForgotPassword" UseSubmitBehavior="true"  runat="server" Text="Forgotten Password"
                            Width="205px"  class="green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 reg_sub mob_width100 height_34 padding_btm padding_top"
                             OnClick="btnForgotPassword_Click" CausesValidation="true" OnClientClick="ForgotLinkPage();" />

</div>
<div class="clear"></div>
        </div>
      </div>
  </div>
</div>
   <%--   <div class="modal fade"></div>--%>
       <%-- <div style="left: 0px; top: 0px; position: absolute; width: 100%; height: 1800px; background-color: rgb(0, 0, 0); opacity: 0.33;"></div>--%>

                    </asp:Panel>
                       </div>
                    
 
 


  
  </div>
  </div>

