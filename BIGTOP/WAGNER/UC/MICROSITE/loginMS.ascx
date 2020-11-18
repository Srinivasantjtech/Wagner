<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_loginMS" EnableTheming="true" Codebehind="loginMS.ascx.cs"  %>
 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<input id="hidpwd" name="hidpwd" type="hidden" runat="server" />
<script type="text/javascript">
    google.maps.event.addDomListener(window, 'load', initialize);
    var componentForm = {

        street_number: 'short_name',
        route: 'long_name',
        locality: 'long_name',
        administrative_area_level_1: 'short_name',
        country: 'long_name',
        postal_code: 'short_name'
    };
    function initialize() {
        
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById('txtAutocomplete'));
        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            // Get the place details from the autocomplete object.

            document.getElementById("ctl00_maincontent_login1_street_number").value = "";
            document.getElementById("ctl00_maincontent_login1_route").value = "";
            document.getElementById("ctl00_maincontent_login1_locality").value = "";
            document.getElementById("ctl00_maincontent_login1_administrative_area_level_1").value = "";
            document.getElementById("ctl00_maincontent_login1_country").value = "";
            document.getElementById("ctl00_maincontent_login1_postal_code").value = "";

            var place = autocomplete.getPlace();

            var getsubpremise = document.getElementById('txtAutocomplete').value.split(",");
            document.getElementById("ctl00_maincontent_login1_subpremise").value = getsubpremise[0];
            for (var i = 0; i < place.address_components.length; i++) {
                var addressType = place.address_components[i].types[0];

                if (componentForm[addressType]) {
                    var val = place.address_components[i][componentForm[addressType]];
                    addressType = "ctl00_maincontent_login1_" + addressType;

                    document.getElementById(addressType).value = val;
                }
            }
            document.getElementById("ctl00_maincontent_login1_hfdoorno").value = document.getElementById("ctl00_maincontent_login1_street_number").value + " " + document.getElementById("ctl00_maincontent_login1_route").value;
            document.getElementById("ctl00_maincontent_login1_hfpincode").value = val;
            document.getElementById("ctl00_maincontent_login1_hfcity").value = val;
            document.getElementById("ctl00_maincontent_login1_hfstate").value = val;

        });
    }

    $(document).ready(function () {
        $("#footeraccordian h3").click(function () {
            //slide up all the link lists
            $("#footeraccordian ul ul").slideUp();
            //slide down the link list below the h3 clicked - only if its closed
            if (!$(this).next().is(":visible")) {
                $(this).next().slideDown();
            }
        })
    })

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
                // if (document.forms[0].elements["<%=txtPassword.ClientID%>"].value.length == 0) {
                //   document.forms[0].elements["<%=txtPassword.ClientID%>"].value = document.forms[0].elements["<%=hidpwd.ClientID%>"].value;                 
                // }
            }
            else {
                document.forms[0].elements["<%=txtPassword.ClientID%>"].value = document.forms[0].elements["<%=hidpwd.ClientID%>"].value;
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
        id.value = value.replace(/'/, '`');
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
                    //document.getElementById("ctl00_maincontent_ct100_btnsubmit").style.backgroundColor = "#ff0000";

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
    
      
      <div class="grid4">
        <div class="mar5">
          <div class="mobilemenu visible-phone">
            <div class=""><a class="btn-menuico open" href="#nav-1">
            <img src="/images/MicroSiteimages/mobmenu.png"></a></div>
          </div>
          <ul class="sidemenu"  id="nav-1">
            <div class="mobmenu-header visible-phone"><a href="">About</a> <a href="">My Account</a><a href="">Wagner Online</a></div>
            <div class="left-title gradleft visible-phone"><a href=""><img src="/images/MicroSiteimages/homewhite-16.png">&nbsp;&nbsp;&nbsp;Home</a></div>
            <div class="signup blue border-rad5">
            	<h5>Customer Login</h5>
                <p>Please Sign In using your Email ID or User Name and Password below:</p>
                  <asp:TextBox ID="txtUserName"  
                  autocomplete="off" runat="server" class="input_code" style="width:94%; padding:2px;" type="text" 
                  placeholder="Username" size="25" MaxLength="50"  onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
                <asp:TextBox ID="txtPassword"  style="width:94%;padding:2px;" class=" input_code" runat="server" TextMode ="Password"  size="25" MaxLength="40"  onFocus="txtpassword_onFocus();"   placeholder="Password" ></asp:TextBox>
                <p>
               <input type="checkbox"  id="chkKeepme" name="chkKeepme" class="chkbox" runat="server" />
                <span class="algn">Keep me logged in this computer</span>
                	<div class="cl"></div>
                </p>
                   <asp:HyperLink ID="lnkForgotPWDPage" runat="server" CssClass="HyperLinkStyle" NavigateUrl="/MForgotPassWord.aspx">Forget Password? </asp:HyperLink></p>

               <%-- <a href="#">Forget Password?</a>--%>
              <input type="checkbox" name="chkShopCart" id="chkShopCart"  visible="false" style="border-style:none;" runat="server" />
              <asp:Button ID="cmdLogin" class="loginbtn" text="LOGIN" runat ="server"     ValidationGroup="LoginValidation" OnClick="cmdLogin_Click" OnClientClick="return passwordcheck();"  />
                <div class="cl"></div>
                <div>
                <asp:Label ID="lblErrMsg" runat="server"  Text="" Class="lblErrorSkin" ></asp:Label>
                </div>
            </div>
            

            
            
          </ul>
        </div>
        
      </div>
      <div class="grid8">                        
			<div class="form-holder">
            	<div class="form-head bgcustsign" style=" margin-top:0">New Customer Sign Up</div>
              <table>
              <tr>
              <td>
              <p>Creat an account if you are not already registered with Wagner.                                          
                </p>
              </td>
              <td align="right" width="50%">
              <div class="reqfield"> Required Fields*</div>
              </td>
              </tr>
              </table>
                
             
                <div class="form-head sgnbdrrad">User Type</div>
                <div class="signcontpnl">
                <table width="100%">
              <tr>
              <td>
                <p class="grid3">User Type:</p><p class="grid3">
                <asp:RadioButton ID="RBPersonal" runat="server" GroupName="X"  Text="Personal" Checked="true" onclick="javascript:chkPB();"/>
               <p class="grid3">
                 <asp:RadioButton ID="RBBusiness" runat="server" GroupName="X" Text="Business" onclick="javascript:chkPB();" />
              </p>
            </td>
           </tr> <tr>
           <td>
           <div id="tblbus" style="display:none;" cellpadding="0" cellspacing="0" border="0" >
             <%-- <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblbus" style="display:none;" >
                  <tr id="Cmp"  >
                        <td width="68%" align="left"   >--%>
                      <p class="grid4 title"> Company / Account Name:*
                                           </p>
                       <%--</td>--%>
<%--<td align="left" >--%><p class="grid4" ><asp:TextBox runat="server" ID="txtcompname" Text=""     MaxLength="50" onkeyup="javascript:keyboardup(this);"></asp:TextBox></p>
<%-- </td>
</tr>   --%>           
<%--<tr id="CmpNo" >
<td align="left" width="68%">--%> <p class="grid4 title" > ABN / ACN / Company Number:*</p>
<%--</td>--%>
<%--<td align="left">--%>
<p class="grid4">
<asp:TextBox runat="server" ID="txtcompno" Text=""   MaxLength="20" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
</p>
</div>

                        </td>
                        </tr>
                      </table>
                <div class="cl"></div> 
                </div>
                <div class="cl"></div>
                <div class="form-head sgnbdrrad">Contact Details</div>
                <div class="signcontpnl">
             <%--   <table style="width:100%;line-height: 0px;line-height: 0px;" >
                <tr>
                <td class="logintdsttyle">--%>
                
                  <p class="grid4 title">First Name*</p>
               <%-- </td>
                <td>--%>
                 <p class="grid7">
              <asp:TextBox runat="server" ID="txtfname" Text=""  MaxLength="20" 
                         onkeyup="javascript:keyboardup(this);"  ></asp:TextBox>
                </p>
               <%-- </td>
                </tr>--%>
                <br />
                <table style="width:100%;line-height: 0px;" >
                <tr>
                <td class="logintdsttyle">
                </td>
                <td>
              
                <asp:RequiredFieldValidator ID="rfvfname" runat="server"   Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name"  ControlToValidate="txtfname"></asp:RequiredFieldValidator>
               </td>
                </tr>
                </table>
                <%-- <table style="width:100%" >
                 <tr>
                 <td class="logintdsttyle">--%>
                   <p class="grid4 title">Last Name*</p>
                <%-- </td>
                 <td>--%>
                 <p class="grid7">
                 <asp:TextBox runat="server" ID="txtlname" Text=""  MaxLength="20" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>
                </p>
                <%-- </td>
                 </tr>--%>
                 <table style="width:100%;line-height: 0px;">
                 <tr>
                 <td class="logintdsttyle">                 
                 </td>
                 <td>                
                 <asp:RequiredFieldValidator ID="rfvlname" runat="server"   Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name"   ControlToValidate="txtlname"></asp:RequiredFieldValidator> 
                                </td>
                 </tr>
                 </table> 
          
                  <p class="grid4 title">Phone*</p>
         
                 <p class="grid7">
               <asp:TextBox runat="server" ID="txtphone" Text="" CssClass="input_dr"
MaxLength="16" onkeyup="javascript:keyboardup(this);" ></asp:TextBox>

                </p>
            
                <br />
                <table style="width:100%;line-height: 0px;">
              <tr>
              <td class="logintdsttyle">
              
              </td>
              <td>
                <p>
                 <asp:RequiredFieldValidator ID="rfvphone" runat="server" Class="vldRequiredSkin"  ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Phone"  ControlToValidate="txtphone">
</asp:RequiredFieldValidator>
      <asp:FilteredTextBoxExtender ID="ftePhone" 
runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtphone" />           
                 </p>
              </td>
              </tr>
               
                  
               
                  </table> 
        
                   <p class="grid4 title">Mobile/Cell Phone</p>
             
                   <p class="grid7">
               <asp:TextBox runat="server" ID="txtMobile" Text="" CssClass="input_dr"
MaxLength="16" onkeyup="javascript:keyboardup(this);" >
</asp:TextBox>
                </p>
               
                  <br />
                  <table style="width:100%;line-height: 0px;" >
                  <tr>
                  <td width="40%">                  
                  </td>
                  <td>
               
 <asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobile" />                
 &nbsp;&nbsp;
<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobile" />               
                  </td>
                  </tr>
                  </table> 
                <p class="grid4 title">Fax</p>
         
 <p class="grid7">
<asp:TextBox runat="server" ID="txtfax" Text="" CssClass="input_dr" MaxLength="16" onkeyup="javascript:keyboardup(this);"  ></asp:TextBox>
</p>


 <table style="width:100%">
 <tr>
 <td class="logintdsttyle">
 <asp:FilteredTextBoxExtender ID="ftefax" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtfax" /> 
  </td>
 </tr>
 </table> 

    <p class="grid4 title">Subcribe Mail</p>
 
   <p class="grid7">
 <asp:CheckBox ID="chkissubscribe" Checked="true"  runat="server" style="width:10px;"    TextAlign="Right"/>

</p>



         <div class="cl"></div>
                </div>                
                <div class="cl"></div>
                <div class="form-head sgnbdrrad">Login Details</div>
                  
                <div class="signcontpnl">
                <p class="grid4 title">Email*</p>
                <p class="grid7">
                
              <asp:TextBox runat="server" ID="txtemail" 
  Text="" CssClass="input_dr"  MaxLength="55" 
  onkeyup="javascript:keyboardup(this);" runnat="server">
  </asp:TextBox>
                
                </p>
                <br />
                 <table style="width:100%">
             <tr>
             <td class="logintdsttyle">
             </td>
             <td>
                
 <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email" ControlToValidate="txtemail"></asp:RequiredFieldValidator>               
   <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtemail"   ErrorMessage="Required" 
Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
               
                </td>
                </tr>
                </table>
                <p class="grid4 title">Confirm Email*</p>
                <p class="grid7">
               <asp:TextBox runat="server" ID="txtcemail" 
 Text="" CssClass="input_dr" 
  MaxLength="55"
   onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                </p>


              <table style="width:100%">
             <tr>
             <td class="logintdsttyle">
             </td>
             <td>
                
                <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txtcemail" ControlToCompare="txtemail" runat="server" ErrorMessage="Confirm Email and Email should be same" ValidationGroup="Mandatory" Display="Dynamic" >
                </asp:CompareValidator>

        
                 <asp:RequiredFieldValidator ID="rfvcemail" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Email"
 ControlToValidate="txtcemail"></asp:RequiredFieldValidator>                                         
     </td>
          </tr> 
          </table>
                <p class="grid4 title">Password*</p>

                <p class="grid7">
                
                
              <asp:TextBox runat="server" ID="Txt1password" Text="" CssClass="input_dr"  TextMode="Password"  MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>

                
                
                </p>
                  <table style="width:100%">
             <tr>
             <td class="logintdsttyle">
             </td>
             <td>
             
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" Display="Dynamic"  ControlToValidate="Txt1password"  ValidationGroup="Mandatory"  ValidationExpression="^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"  runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Password" ControlToValidate="Txt1password"></asp:RequiredFieldValidator>

              </td> 
              </tr> 
              </table> 
                <p class="grid4 title">Confirm Password*</p>

                <p class="grid7">
  
   <asp:TextBox runat="server" ID="TxtConfirmPassword" Text="" CssClass="input_dr" TextMode="Password" 
 MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
   
   </p>
     <table style="width:100%">
             <tr>
             <td class="logintdsttyle">
             </td>
             <td>
   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Class="vldRequiredSkin"
ErrorMessage="Required" 
ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Password" ControlToValidate="TxtConfirmPassword"></asp:RequiredFieldValidator>

   
</td>
</tr>
</table>
                <div class="cl"></div>
                </div>
                <div class="cl"></div>
                <div class="form-head sgnbdrrad">Shipping Address Details</div>
                <div class="signcontpnl">
              
                <p class="grid4 title">Street Address*</p>
                <p class="grid7">
 <asp:TextBox runat="server" ID="txtsadd" Text="" 
 CssClass="input_dr"   MaxLength="30" 
 onkeyup="javascript:keyboardup(this);"></asp:TextBox>
 
 </p>
   <table style="width:100%">
                <tr>
                <td class="logintdsttyle">
              
                </td>
                <td>
                  <asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Street Address"   ControlToValidate="txtsadd"></asp:RequiredFieldValidator>
                </td>
                </tr>
                </table>
 <p class="grid4 title">Address Line 2</p>
 <p class="grid7">
<asp:TextBox runat="server" ID="txtadd2" Text="" CssClass="input_dr"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>

 
 </p>
<p class="grid4 title">Suburb/Town*</p>
 <p class="grid7">
 <asp:TextBox runat="server" ID="txttown" Text="" CssClass="input_dr"  MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
 
 </p>

   <table style="width:100%">
                <tr>
                <td class="logintdsttyle">
              
                </td>
                <td>
 <asp:RequiredFieldValidator ID="rfvtown" runat="server" 
 Class="vldRequiredSkin"   ErrorMessage="Required" 
 ValidationGroup="Mandatory" Display="Dynamic" 
 Text="Enter Suburb/Town"  ControlToValidate="txttown">
 </asp:RequiredFieldValidator>
 </td>
</tr>
</table>
 

<p class="grid4 title">State/Province*</p>
<p class="grid7">
<asp:TextBox runat="server" ID="txtstate" Text="" CssClass="input_dr" MaxLength="20" Visible="false" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
<asp:DropDownList ID="drpState" runat="server" Width="250px" 
          Class="DropdownlistSkin" Visible="true"> </asp:DropDownList>

</p>
  <table style="width:100%">
                <tr>
                <td class="logintdsttyle">
              
                </td>
                <td>
<asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="vldRequiredSkin"
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter State/Province" Enabled="false" ControlToValidate="txtstate"></asp:RequiredFieldValidator>
   <asp:RequiredFieldValidator ID="rfvddlstate" runat="server" Text="Select State/Province" ErrorMessage="Required" ControlToValidate="drpState" ValidationGroup="Mandatory" InitialValue=""></asp:RequiredFieldValidator>
</td>
</tr>
</table>

<p class="grid4 title">Postal/Zip Code*</p>
<p class="grid7">
<asp:TextBox runat="server" ID="txtzip" Text="" 
CssClass="input_dr" MaxLength="10" 
onkeyup="javascript:keyboardup(this);" ></asp:TextBox>

</p>
  <table style="width:100%">
                <tr>
                <td class="logintdsttyle">
              
                </td>
                <td>
<asp:RequiredFieldValidator ID="rfvzip" runat="server" Class="vldRequiredSkin"
 ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" 
 Text="Enter Post/Zip Code"  ControlToValidate="txtzip"></asp:RequiredFieldValidator>
 
 &nbsp;&nbsp;
 <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890"  TargetControlID="txtzip" /> 
 
</td>
</tr> 
</table> 
 <p class="grid4 title">Country</p>
<p class="grid7">
<asp:DropDownList ID="drpCountry" runat="server" 
          Width="250px" Class="DropdownlistSkin" 
          onselectedindexchanged="drpCountry_SelectedIndexChanged" 
          AutoPostBack="True"> </asp:DropDownList>
</p>
                <div class="cl"></div>
                </div>               
                <div class="cl"></div>
                <div class="form-head sgnbdrrad">Submit Form</div>
                <div class="signcontpnl">
                <p class="signadj">Form Verify. Please enter text shown below:</p>
                <div class="loginlbl signpdng">
     <cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" 
         CaptchaBackgroundNoise="Low" Width="263px"  runat="server" 
         CustomValidatorErrorMessage="Invalid Verification Code" 
         CaptchaMaxTimeout="4000" CaptchaMinTimeout="4" /></div>
                <div class="loginlbl signpdng">Text Code*</div>
                <table>
                <tr>
                <td>
                     <asp:TextBox ID="cText" runat="server" value="" CssClass="input_code"  ></asp:TextBox>
                </td>
                <td>
                
                    </td>
                    <td>
                        <asp:Label ID="cVerifyMsg" runat="server" 
          ForeColor="Red" Text="" Visible="false" />

                    </td>
                </tr>
                </table>
             
                  <div class="cl"></div>
                <input id="chkterms" type="checkbox" runat="server" onclick="javascript:checkEnableSubmit();" />
                  
                  <label class="lbl-checkbox">I have read and understand  </label>
                   <asp:LinkButton ID="myLink" CssClass="modal" style="color: #01AEF0;text-decoration: none;" ToolTip=" Sales Terms and Conditions" Text="Sales Terms and Conditions" runat="server"  />
                    <asp:Label ID="lblCheckTerms" runat="server" ForeColor="Red" Text="*" Visible="false" />
                  <div class="cl"></div>
                
                
                 <asp:Button ID="btnsubmit" class="btn-login btn wid txtcolor lgmgnbtm" Text="Submit" style="width: 100px;" 
                 runat="server" ValidationGroup="Mandatory"   OnClick="btnsubmit_Click" 
                 OnClientClick="return checkEnableSubmit()" UseSubmitBehavior="true" />  
              
                 <div class="cl"></div>
                  </div>
                  
                  
                    <asp:Label ID="RegSucess" Visible="false" runat="server" Text ="<%$ Resources:login,lbRegSucess%>" Class="lblResultSkin"></asp:Label>
                 
                 <asp:HyperLink ID="lnkResetPassword" CssClass="HyperLinkStyle" runat="server" Visible="false">Please Reset Your Password</asp:HyperLink>
                <div class="cl"></div>
                     </div>   
            </div>
    <asp:Label ID="lblhid1" runat="server" Text="" ></asp:Label>
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
  <tr><td width="248" height="49" valign="top" style="padding-left:20px;"><a href="#"><img src="/images/MicroSiteimages/dld_pdf_form.png" width="197" alt="" height="40" /></a></td>
    <td width="266" valign="middle"><b>
    <asp:HyperLink ID="hlink" CssClass="txt_12 blue" runat="server" NavigateUrl="~//Home.aspx">Continue Browsing Web Site</asp:HyperLink>
    </b></td></tr></table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup"  BehaviorID="testTACpopup"    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None"   >
</asp:ModalPopupExtender>
 <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none; visibility: hidden">
 </asp:Button>
   <asp:ModalPopupExtender ID="ShowAdminAlert"  PopupControlID="pnlAdmin" BackgroundCssClass="modalBackground"  DropShadow="true" TargetControlID="lblhid1" runat="server">
    </asp:ModalPopupExtender>
      <asp:HiddenField ID="ErrorStatusHiddenField" runat="server" />
      <div id="PopupOrderMsg" align="center" runat ="server">
        <asp:Panel ID="ModalPanelRRAE" runat="server" CssClass="PopUpDisplayStyle">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;" align="center">
                <tr style="height: 5px"><td colspan="3">&nbsp;</td> </tr>
                <tr style="height: 10px"> <td width="100%" align="center" colspan="3">&nbsp;</td></tr>
                <tr style="height: 10px"> <td width="100%" align="center" colspan="3" class="TextContentStyle">
                       The email address you are registering your account with exists already.
                        <br /> Would you like to continue with ForgotPassword?</td></tr>
                <tr style="height: 10px"><td width="100%" align="center" colspan="3">&nbsp;</td> </tr>
                <tr style="height: 5px"><td colspan="3"> &nbsp;</td></tr>
                <tr style="height: 10px"> <td width="45%" align="right"><asp:Button ID="ForgotPassword" UseSubmitBehavior="true"  runat="server" Text="Forgotten Password"
                            Width="205px"  CssClass="ButtonStyle" OnClick="btnForgotPassword_Click" CausesValidation="true" OnClientClick="ForgotLinkPage();" /></td>
                    <td width="10%">&nbsp;</td>
                    <td width="45%" align="left"><asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="ButtonStyle" OnClick="btnClose_Click" OnClientClick="ForgotLinkPageClose();" />
                    </td></tr></table> 
                    </asp:Panel></div>
    
 




