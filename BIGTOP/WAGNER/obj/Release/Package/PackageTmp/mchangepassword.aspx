<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mchangepassword.aspx.cs" Inherits="WES.mchangepassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
<div  style="margin-left:10px;border-bottom: 1px solid #e5e5e5; color: #089fd6 !important;font-size: 14px;margin-top: 4px !important; padding-bottom: 16px;">          
          <a style="color:#089fd6 !important" href="<%=micrositeurl%>">Home</a> &gt; <a href="/mchangepassword.aspx" style="color:#089fd6 !important">ChangePassword</a>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" type="text/javascript">

    /*accordion for filter -End*/
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
    </script>
<div class="grid9" style="margin-top: 4px;">
<h1 class="left-titleborder bggray blue border-rad5 bdr-none-btm contmgn">Change Password</h1>
<div class="box5 mar5 mgntop minheight">          
          <div class="mar5 ">
            <div class="login-wrapper ">   
            <div style="text-align:center;">
               <asp:Label ID="lblError" SkinID ="lblErrorSkin" runat="server" Text="" style="color:red; font-weight:bold;"></asp:Label>          
               </div> 
              <div class="pnl">
              		<h5>Change Password</h5>
                    <div class="grid12">
                    	<div class="grid4 mrglft"><span>*</span>&nbsp;Old Password</div>
                        <div class="grid7">
                        <asp:TextBox  autocomplete="off"  ID="txtOldPassword" SkinID="textSkin" CssClass="txtfield" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                         <div style="margin-top:-7px;">
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Old Password Required"  ControlToValidate="txtOldPassword" ></asp:RequiredFieldValidator>
                         </div> 
                        <%--<input type="text" class="txtfield" name="">--%>
                        </div>
                    </div>
                    <div class="grid12">
                    	<div class="grid4 mrglft"><span>*</span>&nbsp;New Password</div>
                        <div class="grid7">
                      <%--  <input type="text" class="txtfield" name="">--%>
                      <asp:TextBox  autocomplete="off"  ID="txtNewPassword" SkinID="textSkin" CssClass="txtfield" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox>
                       <div style="margin-top:-7px;">
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
                                                        ControlToValidate="txtNewPassword"   ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
                                                        runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                                   
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="New Password Required"
                 ControlToValidate="txtNewPassword" ></asp:RequiredFieldValidator>
                 </div>
                        </div>
                    </div>
                    <div class="grid12">
                    	<div class="grid4 mrglft"><span>*</span>&nbsp;Confirm Password</div>
                        <div class="grid7">
                        <%--<input type="text" class="txtfield" name="">--%>
                         <asp:TextBox  autocomplete="off"  ID="txtNewConfirmPassword" SkinID="textSkin" CssClass="txtfield" MaxLength ="15" runat="server" TextMode ="Password" Font-Underline="False" onkeyDown="CheckTextPassMaxLength(this,event,'15');"></asp:TextBox></br>
                         <div style="margin-top:-7px;">
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required"
                 ControlToValidate="txtNewConfirmPassword" ></asp:RequiredFieldValidator><br />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match"
                  ControlToCompare="txtNewPassword" ControlToValidate="txtNewConfirmPassword" ValueToCompare="String"></asp:CompareValidator>
                  </div>
                        </div>
                    </div>
                    <div class="grid12">
                    	<%--<input type="button" class="btn" value="Change Password" name="" CausesValidation="true" UseSubmitBehavior="true" />--%>
                        <asp:Button ID="btnChange" runat ="Server"  Text="Change Password" class ="btn" OnClick="btnChange_Click"   CausesValidation="true" UseSubmitBehavior="true"/><br />
                    </div>
                    <div class="grid12">
                    	<span>*</span>&nbsp;Required Fields
                    </div>
                    <div class="cl"></div>
              </div>
            </div>	
            <div class="cl"></div>
          </div>
        </div>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
