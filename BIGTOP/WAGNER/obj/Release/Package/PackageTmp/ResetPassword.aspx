<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="ResetPassword"  Culture="en-US"
    UICulture="en-US" Codebehind="ResetPassword.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
    <style type="text/css">
        .ButtonStyle
        {
            font-family: Arial;
            font-size: 11px;
            font-weight: bold;
            background-color: #0092c8;
            border-style: inset;
            border-color: Gray;
            color: White;
            width: 55px;
            height: 20px;
        }
        
        .ModalPopupStyle
        {
            visibility: visible;
        }
         .modalBackground
{
    background-color: #000000;
    filter: alpha(opacity=70);
    opacity: 0.7;
}
        #container
        {
            left: 200px;
            top: 50px;
            position: absolute;
        }
        
        .TextBoxStyle
        {
            font-family: Arial, Tahoma;
            font-size: 11px;
            color: Black;
            width: 180px;
            text-align: left;
        }
        .LabelStyle
        {
            font-family: Arial, Tahoma;
            font-size: 12px;
            color: Black;
            width: 180px;
            text-align: left;
        }
        
        .TableColumnStyle
        {
            font-family: Arial Unicode MS;
            font-size: 12px;
            padding-left: 10px;
            /*width: 143px;*/
            width:56px;
            text-align: left;
        }
        .HiddenButton
        {
            display: none;
            visibility: hidden;
        }
    </style>
    <script type="text/javascript">
        function CheckBeforeUpdate() {
            var retvalue = true;
            var EmptyStatus = false;

            window.document.getElementById("ctl00_maincontent_txtNewPassword").style.borderColor = "ActiveBorder";
            window.document.getElementById("ctl00_maincontent_txtConfirmPassword").style.borderColor = "ActiveBorder";
            
            if (document.getElementById("ctl00_maincontent_txtNewPassword").value == null || document.getElementById("ctl00_maincontent_txtNewPassword").value == '') {
                alert('New Password cannot be empty!');
                window.document.getElementById("ctl00_maincontent_txtNewPassword").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtNewPassword").focus();
                EmptyStatus = true;
                retvalue = false;
            }

            if (document.getElementById("ctl00_maincontent_txtConfirmPassword").value == null || document.getElementById("ctl00_maincontent_txtConfirmPassword").value == '') {
                alert('Confirm Password cannot be empty!');
                window.document.getElementById("ctl00_maincontent_txtConfirmPassword").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtConfirmPassword").focus();
                EmptyStatus = true;
                retvalue = false;
            }

            if (document.getElementById("ctl00_maincontent_txtNewPassword").value != document.getElementById("ctl00_maincontent_txtConfirmPassword").value && EmptyStatus == false) {
                alert('New Password and Confirm Password do not Match');
                document.getElementById("ctl00_maincontent_txtConfirmPassword").focus();
                retvalue = false;
            }

            return retvalue;
        }
    </script>


    <script type="text/javascript" >

        function keyboardActions(event) {
            if (event.keyCode == 13) {

                eval($("#<%=btnUpdate.ClientID %>").trigger('click'));
                return false;
            }

        }

//        $(document).ready(function () {
//            if ($.browser.mozilla) {
//                $("#<%=txtNewPassword.ClientID %>").keypress(keyboardActions);
//                $("#<%=txtConfirmPassword.ClientID %>").keypress(keyboardActions);
//                
//            } else {
//                $("#<%=txtNewPassword.ClientID %>").keydown(keyboardActions);
//                $("#<%=txtConfirmPassword.ClientID %>").keydown(keyboardActions);
//                
//            }
//        });

  </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
<div class="container">
<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="/home.aspx">Home</a></li>
                <li class="active">Reset Password</li>
            </ul>
        </div>
    </div>
<div class="row">
<asp:Panel ID="ResetPWDPopupPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyle" DefaultButton="btnUpdate">
<div class="modal-dialog">
                        <div class="modal-content ">
                          <div class="modal-header green_bg">
                            <h4 id="myModalLabel" class="text-center">
                            <img class="popsucess" alt="rp" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Order__info.png"/>Reset Password</h4>
                          </div>
                          <div class="modal-body">
                            
                              <div class="form-group col-sm-20">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3">New Password<span class="error"> *</span></label>
                                          <div class="col-sm-15">
                                           <%-- <input type="password" id="inputEmail3" class="form-control checkout_input"/>--%>
                                           <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control checkout_input" TextMode="Password" MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"/> 
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="New Password Required" ControlToValidate="txtNewPassword" CssClass="mandatory" >
                                             </asp:RequiredFieldValidator> 
                                             <div style="margin-bottom:0;"></div>
                                              <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic" ControlToValidate="txtNewPassword" ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$" runat="server" CssClass="mandatory" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                            </div>
                                          <div class="clear"></div>
                                        </div>                           
                              <div class="form-group col-sm-20">
                                          <label class="col-sm-5 control-label font_normal padding_top" for="inputEmail3">Confirm Password<span class="error"> *</span></label>
                                          <div class="col-sm-15">
                                            <%--<input type="password" id="Password1" class="form-control checkout_input"/>--%>
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control checkout_input" TextMode="Password"  MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"/> 
<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required" ControlToValidate="txtConfirmPassword" CssClass="mandatory" >
</asp:RequiredFieldValidator>  <div style="margin-bottom:0;"></div>
 <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" ValueToCompare="String" CssClass="mandatory" >
 </asp:CompareValidator>
                                            </div>
                                          <div class="clear"></div>
                              </div> 
                          </div>
                          <div class="modal-footer clear border_top_none">
                           <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn primary-btn-green" OnClick="btnUpdate_Click" CausesValidation="true" />
                       <%--     <button value="Yes, Continue with Previous Order" class="btn primary-btn-green" type="submit">
                            Yes, Continue with Previous Order</button>
                            <button class="btn primary-btn-blue" type="submit">No, Cancel Previous Order</button>--%>
                            
                          </div>
                           <asp:Label ID="lblPwdErrorMessage" runat="server" Class="mandatory" Visible="false"></asp:Label>
                      </div>
                  </div>
</asp:Panel>
</div>
<div class="row">
<%--  <asp:Label ID="lblErrorMessage" runat="server" Class="mandatory" Visible="false">
  </asp:Label> 
  <br />--%>

  <asp:Panel ID="pnlUser" runat="server" DefaultButton="btnSubmit">
<%--<table id="tblUserID" runat="server" class="BaseTblBorder" width="558px" border="0">
<tr style="display: none;">
<td> --%> 
<div class="categoryheading"></div>
<div class="col-sm-20 mgntb40">
<img class="col-lg-4 col-sm-4 for_img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/forgot.png.jpg"/>
<div class="col-lg-8 margin_top_20 margin_bottom_30 col-sm-8">
          <p><strong>Reset Password</strong></p>
         <form>
         <div class="form-group">

            <label class="sr-only" for="exampleInputEmail3">Email address</label>
            <%--<input  class="form-control border_radius_none height_49" id="txtEmailAddress" placeholder="Enter email"/>--%>
            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control border_radius_none height_49" />
           <%-- <input type="submit" value="Submit" class="btn btn-primary margin_top"/>--%>
              <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary margin_top" Text="Submit" OnClick="btnSubmit_Click" CausesValidation="false" /> 
              <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButton" CausesValidation="true" /> 
          </div>
           <div class="form-group" style="display:none;">
             <asp:Label ID="lblUserID" runat="server" Text="User ID" style="display:none;"></asp:Label> 
             <asp:TextBox ID="txtLoginName" runat="server" ReadOnly="true" CssClass="TextBoxStyle" style="display:none;" /> 
              <asp:Label ID="lblCompanyAccountNo" runat="server" Text="Company Account No" style="display:none;" ></asp:Label>
              <asp:TextBox ID="txtCompanyID" runat="server" MaxLength="6" CssClass="form-control checkout_input" style="display:none;" />
           </div>
       <%--  <span class="mandatory">Error Message</span>--%>
       <asp:Label ID="lblErrorMessage" runat="server" Class="mandatory" Visible="false">
        </asp:Label> 
         </form>
          </div>
</div>
  <%--<div class="form-group col-sm-10" style="display:none;">
   <asp:Label ID="lblUserID" runat="server" Text="User ID">
</asp:Label> 
   </div>--%>

<%--</td>
 <td> --%>
 <%-- <div class="form-group col-sm-10" style="display:none;">
 <asp:TextBox ID="txtLoginName" runat="server" ReadOnly="true" CssClass="TextBoxStyle" /> 
 </div>--%>
<%-- </td> 
 </tr>
<tr>
 <td align="left">--%>
<%-- <div class="form-group col-sm-10" style="display:none;">
  <asp:Label ID="lblCompanyAccountNo" runat="server" Text="Company Account No">
  </asp:Label>
  </div>--%>
<%--   </td> 
   <td align="left"> --%>
<%--  <div class="form-group col-sm-10" style="display:none;">
  <asp:TextBox ID="txtCompanyID" runat="server" MaxLength="6" CssClass="form-control checkout_input" />
  </div>--%>
<%--   </td> 
   </tr>
<tr>
 <td align="left">--%>
 <%--<div class="form-group col-sm-10">
  <asp:Label ID="lblEmailAddress" runat="server" Text="EMail Address" CssClass="font_normal">
  </asp:Label> 
  </div>--%>
<%--  </td>
   <td align="left"> --%>
<%--   <div class="form-group col-sm-10">
   <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control checkout_input" />
   </div>--%>
<%--    </td> 
    </tr> 
    <tr> 
    <td colspan="2" align="center">--%>
   <%-- <div class="form-group col-sm-10">
     <asp:Button ID="btnSubmit" runat="server" CssClass="btn primary-btn-green" Text="Submit" OnClick="btnSubmit_Click" CausesValidation="false" /> 
     <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButton" CausesValidation="true" /> 
     </div>--%>
 <%--    </td> 
     </tr>
</table>--%>
</asp:Panel>
</div>
</div>
 <%--<table width="558px" cellspacing="5" cellpadding="5" border="0" style="border-collapse: collapse;   text-align: left;" align="center">
   <tr> 
   <td width="558px" align="center">
    <asp:Label ID="lblErrorMessage" runat="server" Class="lblErrorSkin" Visible="false">
    </asp:Label> 
    </td> 
    </tr>
     </table>--%>
<%-- <br />
<asp:Panel ID="pnlUser" runat="server" DefaultButton="btnSubmit">
<table id="tblUserID" runat="server" class="BaseTblBorder" width="558px" border="0">
<tr style="display: none;">
<td> 
<asp:Label ID="lblUserID" runat="server" Text="User ID">
</asp:Label> 
</td>
 <td> 
 <asp:TextBox ID="txtLoginName" runat="server" ReadOnly="true" CssClass="TextBoxStyle" /> 
 </td> 
 </tr>
<tr>
 <td align="left">
  <asp:Label ID="lblCompanyAccountNo" runat="server" Text="Company Account No">
  </asp:Label> </td> <td align="left"> 
  <asp:TextBox ID="txtCompanyID" runat="server" MaxLength="6" CssClass="TextBoxStyle" />
   </td> 
   </tr>
<tr>
 <td align="left">
  <asp:Label ID="lblEmailAddress" runat="server" Text="EMail Address">
  </asp:Label> 
  </td>
   <td align="left"> 
   <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="TextBoxStyle" />
    </td> 
    </tr> 
    <tr> 
    <td colspan="2" align="center">
     <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonStyle" Text="Submit" OnClick="btnSubmit_Click" CausesValidation="false" /> 
     <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButton" CausesValidation="true" /> </td> </tr>
</table>
</asp:Panel>--%>
<%--<br />--%>
<%--<div id="PopDiv" class="container">
<asp:Panel ID="ResetPWDPopupPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyle" DefaultButton="btnUpdate">
<div class="container">
<table width="100%" cellspacing="0" cellpadding="5" border="0" style="border-collapse: collapse;   text-align: left;">
<tr>
 <td colspan="2" align="center"> 
 <asp:Label ID="lblPwdErrorMessage" runat="server" Class="lblErrorSkin" Visible="false">
 </asp:Label>
  </td> 
  </tr>
<tr>
 <td class="TableColumnStyle">
  <asp:Label ID="lblNewPassword" runat="server" Text="New Password" CssClass="LabelStyle">
  </asp:Label> 
  </td> 
  <td class="TableColumnStyle">
<asp:TextBox ID="txtNewPassword" runat="server" CssClass="TextBoxStyle" TextMode="Password" MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"/> 
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="New Password Required" ControlToValidate="txtNewPassword" >
</asp:RequiredFieldValidator> 
</td> </tr>
<tr>
<td class="TableColumnStyle">
</td>
<td colspan="2">
 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic" ControlToValidate="txtNewPassword" ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$" runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
 
  </td>
  </tr>
<tr> 
<td class="TableColumnStyle">
 <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password" CssClass="LabelStyle">
 </asp:Label>
  </td> 
  <td class="TableColumnStyle"> 
<asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="TextBoxStyle" TextMode="Password"  MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"/> 
<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required" ControlToValidate="txtConfirmPassword" >
</asp:RequiredFieldValidator>
 <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" ValueToCompare="String">
 </asp:CompareValidator>
  </td>
   </tr> 
<tr> 
<td align="center" colspan="2">
 <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="ButtonStyle" OnClick="btnUpdate_Click" CausesValidation="true" />
  </td> 
  </tr>
</table>
</div>
</asp:Panel>
</div>--%>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server"></asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server"></asp:Content>
