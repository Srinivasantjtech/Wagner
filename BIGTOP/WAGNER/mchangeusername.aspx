<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mchangeusername.aspx.cs" Inherits="WES.mchangeusername" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
<div  style="margin-left:10px;border-bottom: 1px solid #e5e5e5; color: #089fd6 !important;font-size: 14px;margin-top: 4px !important; padding-bottom: 16px;">          
          <a style="color:#089fd6 !important" href="<%=micrositeurl%>">Home</a> &gt; <a href="/mchangeusername.aspx" style="color:#089fd6 !important">Change UserName</a>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<div class="grid9">
      <h1 class="left-titleborder bggray blue border-rad5 bdr-none-btm contmgn">Change User Name</h1>
   
        <div class="box5 mar5 mgntop minheight">          
          <div class="mar5 ">
            <div class="login-wrapper ">     
                 <div style="text-align:center;">
                     <asp:Label ID="lblError"  style=" color: #ff0000;" runat="server" Text=""></asp:Label>
                 </div>         
              <div class="pnl">
              		<h5>Change User Name</h5>
                    <div class="grid12">
                    	<div class="grid4"><%--<span>*</span>--%>&nbsp;Current Login Name</div>
                        <div class="grid7">
                      <%--  <input type="text" class="txtfield" name="">--%>
                       <asp:TextBox  autocomplete="off"  ID="txtOldUserName" Class="txtfield"  MaxLength ="10" runat="server" ></asp:TextBox>
                       <div style="margin-top:-7px;">
                         <asp:RequiredFieldValidator ID="rfvOldPwd" class="vldRequiredSkin" ControlToValidate ="txtOldUserName" runat="server" ErrorMessage="Enter Old Login Name" ></asp:RequiredFieldValidator>
                       </div>
                        </div>
                    </div>
                    <div class="grid12">
                    	<div class="grid4"><span>*</span>&nbsp;New Login Name</div>
                        <div class="grid7">
                       <%-- <input type="text" class="txtfield" name="">--%>
                       <asp:TextBox  autocomplete="off"  ID="txtNewUserName" Class="txtfield" MaxLength ="10" runat="server"  Font-Underline="False" onkeypress="capLock(event)"></asp:TextBox>
                       <div style="margin-top:-7px;">
                       <asp:RequiredFieldValidator ID="rfvNewPwd" Class="vldRequiredSkin" ControlToValidate ="txtNewUserName" runat="server" ErrorMessage="Enter New Login name"  ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                       </div>
                        </div>
                    </div>
                    <div class="grid12">
                    	<div class="grid4"><span>*</span>&nbsp;Confirm Login Name</div>
                        <div class="grid7">
                       <%-- <input type="text" class="txtfield" name="">--%>
                       <asp:TextBox  autocomplete="off"  ID="txtConfirmUserName" Class="txtfield"  MaxLength ="10" runat="server"  onkeypress="capLock(event)"></asp:TextBox> 
                          <div style="margin-top:-7px;">
                          <asp:RequiredFieldValidator ID="rfvConPwd"  Class="vldRequiredSkin" ControlToValidate ="txtConfirmUserName" runat="server"  ErrorMessage="Enter Confirm Login name" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                          <br />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="New Login name and confirm Login name are not same"
                  ControlToCompare="txtNewUserName" ControlToValidate="txtConfirmUserName" ValueToCompare="String" ValidationGroup="Mandatory"></asp:CompareValidator>
                          </div>
                        </div>
                    </div>
                    <div class="grid12">
                    	<%--<input type="button" class="btn" value="Change Login Name" name="">--%>
                        <asp:Button ID="btnChange" runat ="Server"  Text="Change Login Name" OnClick="btnChange_Click" Class ="btn"  ValidationGroup="Mandatory"  CausesValidation="true" />
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
