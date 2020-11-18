<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="PopChangePwd.aspx.cs" Inherits="WES.PopChangePwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">

<asp:Panel ID ="pnlLogin" runat ="server" DefaultButton="btnChange" style="text-align :left" >
<table   width="600px" border="0" cellspacing="0" cellpadding="5">
<tr><td align="left" class="tx_1" style="font-size:12px;"><a href="/Home.aspx" style="color:#0099FF;font-size:12px;" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Change Password</td> </tr>
<tr><td class="tx_3"><hr></td></tr></table>  
<table width="600px">
<tr>
    <td colspan="4" align="center"><h2 class="col-green">Welcome to Wagner online Super store</h2></td>
  </tr>
  <tr><td style="height:10px" >
  </td></tr>
  <tr>
    <td colspan="4" align="center" >
    <h3 style="color:Black"   >
    To get started please create a new login password
    </h3>
    
    </td>
  </tr>
   
</table>  
<Table cellSpacing="0" cellPadding="0" width="600px"   border="0">
<tbody> 

<tr>

<td valign="top">                   
<table id ="tblBase" align ="center" width="100%" border ="0" cellpadding="3" cellspacing="0" >
<tr> 
<td align ="center">
<asp:Label ID="lblError" SkinID ="lblErrorSkin" runat="server" Text="" style="color:red; font-weight:bold;"></asp:Label>
</td>
</tr>                  
<tr>
 <td  valign ="top" align ="center" >
 <table id="LoginTable"  align="center"  width="600px" border="0" cellpadding ="3" cellspacing ="0">

 
<tr>
<td colspan="3" align="right">
<asp:Label ID="Label3" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   Width="1px"></asp:Label>  
&nbsp;<asp:Label ID="Label4" runat="server" meta:resourcekey="LblReqField" Class="lblNormalSkin"></asp:Label> 
</td>
</tr>
                               
<tr>
<td style="vertical-align:top;">
<asp:Label ID="Label1" runat="server" SkinID="lblRequiredSkin"  CssClass="lblRequiredSkin" meta:resourcekey="LblStar"  ></asp:Label>
</td>
<td align="left" style="width: 123px;font-weight:bold;font-size: 12px;vertical-align:top" >
<asp:Label ID="lblNewPassword" SkinID="lblStaticSkin" runat="server" meta:resourcekey="LblNewPwd" ></asp:Label>
<td  align="left"> 
<asp:TextBox  autocomplete="off"  ID="txtNewPassword" SkinID="textSkin" CssClass="textSkin" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>

<asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
ControlToValidate="txtNewPassword"   ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                                   
<%--     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic"
        ControlToValidate="txtNewPassword"   ValidationExpression=".*[0-9].*"
        runat="server" ErrorMessage="Password must contain one number"></asp:RegularExpressionValidator>
                                                   
    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic"
        ControlToValidate="txtNewPassword"   ValidationExpression=".*[a-zA-Z].*"
        runat="server" ErrorMessage="Password must contain one letter"></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="New Password Required"
                 ControlToValidate="txtNewPassword" ></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td style="height: 30px;vertical-align:top"><asp:Label ID="Label2" runat="server" SkinID="lblRequiredSkin"  CssClass="lblRequiredSkin" meta:resourcekey="LblStar"  ></asp:Label>
</td>
<td align="left" style="height: 30px; width: 123px;font-weight:bold;font-size: 12px;vertical-align:top"> 
<asp:Label ID="lblConfirmPassword" SkinID="lblStaticSkin" runat="server"  meta:resourcekey="lblConfirmPwd" ></asp:Label>
</td>
<td align="left"  style="height: 30px;vertical-align:middle ">
<asp:TextBox  autocomplete="off"  ID="txtConfirmPassword" SkinID="textSkin" CssClass="textSkin"  MaxLength ="15" runat="server" TextMode ="Password"  onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>

 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required"
                 ControlToValidate="txtConfirmPassword" ></asp:RequiredFieldValidator><br />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match" 
                  ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" ValueToCompare="String"></asp:CompareValidator>
</td>
</tr>
<tr>
<td align ="center" colspan ="4"> 
<table align="center" > 
<tr>
<td> 
<br />
<asp:Button ID="btnChange" runat ="Server"  meta:resourcekey="btnChangePwd" class="btn-updatepwd" OnClick="btnChange_Click"  CausesValidation="true" UseSubmitBehavior="true"/><br />
 </td>
 </tr>
<tr>
<td>
<div id="divMayus" style="visibility:hidden; color: red;" align="center"><b>Caps Lock is on.</b></div>
</td>
</tr>
</table>
</td>
</tr>
</table>
</td> 
</tr>
</table>  
</td>

</tbody>
</Table> </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
