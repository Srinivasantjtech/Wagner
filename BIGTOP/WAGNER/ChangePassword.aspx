<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="ChangePassword" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto" Codebehind="ChangePassword.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server"></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">
<script language ="javascript"></script>
<asp:Panel ID ="pnlLogin" runat ="server" DefaultButton="btnChange">
<table align =center  width="780px" border="0" cellspacing="0" cellpadding="5">
<tr><td align="left" class="tx_1" style="font-size:12px;"><a href="/Home.aspx" style="color:#0099FF;font-size:12px;" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Change Password</td> </tr>
<tr><td class="tx_3"><hr></td></tr></table>   
<Table cellSpacing="0" cellPadding="0" width="770px" align="center"  border="0">
<tbody> <tr><td width="10"height="10"><img height="17" src="images/tbl_topLeft.gif" width="10"></td>
<td background="Images/tbl_top.gif"height="10"><img height="17" src="images/tbl_top.gif" width="10"></td>
<td width="10"height="10"><img height="17" src="images/tbl_topRight.gif" width="10"></td>
</tr>
<tr>
<td width="10"  background="Images/tbl_left.gif"> 
<img height="10" src="images/tbl_left.gif" width="10" alt="" />
</td>
<td valign="top">                   
<table id ="tblBase" align ="center" width="100%" border ="0" cellpadding="3" cellspacing="0" >
<tr> 
<td align ="center">
<asp:Label ID="lblError" SkinID ="lblErrorSkin" runat="server" Text="" style="color:red; font-weight:bold;"></asp:Label>
</td>
</tr>                  
<tr>
 <td  valign ="top" align ="center" >
 <table id="LoginTable"  align="center"  width="770px" border="0" class="BaseTblBorder" cellpadding ="3" cellspacing ="0">
<tr>
 <td class="tx_6" colspan ="3" style="height:20px;background:url(images/17.gif);">
 <asp:Label id="lblHead" runat="server" meta:resourcekey="LblChangePwd" ></asp:Label>
 </td>
 </tr>
<tr>
<td colspan="3" align="right">
<asp:Label ID="Label3" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   Width="1px"></asp:Label>  
&nbsp;<asp:Label ID="Label4" runat="server" meta:resourcekey="LblReqField" Class="lblNormalSkin"></asp:Label> 
</td>
</tr>
<tr> 
<td>
<asp:Label ID="Label20" runat="server" CssClass="lblRequiredSkin" SkinID="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
</td>
<td align="left" style="width: 123px;font-weight:bold;font-size: 12px;">
<asp:Label ID="lblOldPassword" SkinID="lblStaticSkin" runat="server" meta:resourcekey="LblOldPwd"></asp:Label> 
</td>
<td  align="left" style="height: 30px">
<asp:TextBox  autocomplete="off"  ID="txtOldPassword" SkinID="textSkin" CssClass="textSkin" TextMode ="Password" MaxLength ="15" runat="server" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
<%--<asp:RequiredFieldValidator ID="rfvOldPwd" SkinID ="vldRequiredSkin" ControlToValidate ="txtOldPassword" runat="server" meta:resourcekey="rfvOldPwd" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>--%>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Old Password Required"  ControlToValidate="txtOldPassword" ></asp:RequiredFieldValidator>
</td>
</tr>                                        <tr>
<td>
<asp:Label ID="Label1" runat="server" SkinID="lblRequiredSkin"  CssClass="lblRequiredSkin" meta:resourcekey="LblStar"  ></asp:Label>
</td>
<td align="left" style="width: 123px;font-weight:bold;font-size: 12px;" >
<asp:Label ID="lblNewPassword" SkinID="lblStaticSkin" runat="server" meta:resourcekey="LblNewPwd" ></asp:Label>
<td  align="left"> 
<asp:TextBox  autocomplete="off"  ID="txtNewPassword" SkinID="textSkin" CssClass="textSkin" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
<%--<asp:RequiredFieldValidator ID="rfvNewPwd" SkinID="vldRequiredSkin" ControlToValidate ="txtNewPassword" runat="server" meta:resourcekey="rfvNewPwd"  ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"  ControlToValidate="txtNewPassword"   ValidationExpression="^[a-zA-Z0-9\s]{6,24}$"  runat="server" ErrorMessage="Password must contain alphabet and numeric,and length should be 6 to 24"></asp:RegularExpressionValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic"  ControlToValidate="txtNewPassword"   ValidationExpression=".*[0-9].*"   runat="server" ErrorMessage="Password must contain one numeric"></asp:RegularExpressionValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic"  ControlToValidate="txtNewPassword"   ValidationExpression=".*[a-zA-Z].*"   runat="server" ErrorMessage="Password must contain one alphabet"></asp:RegularExpressionValidator>--%>
<asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
                                                        ControlToValidate="txtNewPassword"   ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
                                                        runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                                   
                                                  <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic"
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
<td style="height: 30px"><asp:Label ID="Label2" runat="server" SkinID="lblRequiredSkin"  CssClass="lblRequiredSkin" meta:resourcekey="LblStar"  ></asp:Label>
</td>
<td align="left" style="height: 30px; width: 123px;font-weight:bold;font-size: 12px;"> 
<asp:Label ID="lblConfirmPassword" SkinID="lblStaticSkin" runat="server"  meta:resourcekey="lblConfirmPwd" ></asp:Label>
</td>
<td align="left" style="height: 30px">
<asp:TextBox  autocomplete="off"  ID="txtConfirmPassword" SkinID="textSkin" CssClass="textSkin"  MaxLength ="15" runat="server" TextMode ="Password" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
<%--<asp:RequiredFieldValidator ID="rfvConPwd"  SkinID="vldRequiredSkin" ControlToValidate ="txtConfirmPassword" runat="server" meta:resourcekey="rfvConPwd" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
<asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match"  ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" ValueToCompare="String"></asp:CompareValidator>--%>
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
<asp:Button ID="btnChange" runat ="Server"  meta:resourcekey="btnChangePwd" class ="btnNormalSkin" OnClick="btnChange_Click"  CausesValidation="true" UseSubmitBehavior="true"/><br />
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
<td width="10" background="Images/tbl_right.gif" height ="350"><img height="10" src="images/tbl_right.gif" width="10"></td> </tr>
<tr>
<td width="10"height="10">
<img height="10"  src="images/tbl_bottomLeft.gif" width="10"></td><td background="Images/tbl_bottom.gif"height="10"><img height="10" src="images/tbl_bottom.gif" width="10">
</td>
<td width="10"height="10">
<img height="10" src="images/tbl_bottomRight.gif" width="10">
</td>
</tr>
</tbody>
</Table> </asp:Panel></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server"></asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>
