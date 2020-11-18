<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mResetPwd.aspx.cs" Inherits="mResetPwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Banner" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
<div class="container margin_top">
  <div class="row">
 
    <div class="col-lg-8 margin_top_20 col-sm-12 col-lg-push-2">
      
      <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/reset.png" class="col-lg-4 col-sm-4 for_img">
      <div class="col-lg-8 margin_top_20 margin_bottom_30 col-sm-8">
      <p><strong>Reset Password</strong></p>
     <p> To start resetting your password, type your email</p>
    
    <div id="divemail" runat="server">
      <asp:Label ID="lblErrorMessage" runat="server" Class="lblErrorSkin"  style="color:Red" 
Visible="false"></asp:Label>
     <div class="form-group">
       <label for="exampleInputEmail3" class="exampleInputEmail3">Company Id</label>
     <asp:TextBox ID="txtCompanyID" runat="server" MaxLength="6" class="form-control border_radius_none height_49"  /> 
       </div>
       <div class="form-group">
        <label for="exampleInputEmail3" class="exampleInputEmail3">Email address</label>
       <asp:TextBox placeholder="Enter email" autocomplete="off" ID="txtEmailAddress"
        runat="server"  class="form-control border_radius_none height_49" AutoCompleteType="Disabled"></asp:TextBox> 
 <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin" 
   ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic"
    Text="Enter Email" ControlToValidate="txtEmailAddress"></asp:RequiredFieldValidator>               
 <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtEmailAddress"  
  ErrorMessage="Required" 
Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  
Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
  
<%--        <input type="email" placeholder="Enter email" id="exampleInputEmail3" class="form-control border_radius_none height_49">--%>
         <asp:Button ID="btnSubmit"  Text="Submit" runat="server" meta:resourcekey="btnSubmit" OnClick="btnSubmit_Click" ValidationGroup="Mandatory" Class="btn green_bg white_color border_radius_none height_49 margin_top_20 width_100 font_weight" />
      <asp:Label ID="lblError" runat="server" Class="lblErrorSkin" Text=""></asp:Label>
      </div>
      </div> 
             <div id="popupreset"  runat="server" visible="false"  >


         <div class="form-group">
        <label for="exampleInputEmail3" >New Password</label>
        
        <asp:TextBox  autocomplete="off"  TextMode="Password" MaxLength="15"
        ID="txtNewPassword" class="form-control border_radius_none height_49"  
        runat="server"  Font-Underline="False" ></asp:TextBox>

<asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
runat="server" ErrorMessage="New Password Required" 
ControlToValidate="txtNewPassword" ></asp:RequiredFieldValidator>
 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" 
ControlToValidate="txtNewPassword" 
ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$" 
runat="server" ErrorMessage="Password must contain letters and numbers.
 Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>       
      </div>
     <div class="form-group">
        <label for="exampleInputEmail3" >Confirm Password</label>
       <asp:TextBox  autocomplete="off"  ID="txtConfirmPassword" TextMode="Password" MaxLength="15"  class="form-control border_radius_none height_49"  runat="server" ></asp:TextBox>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
ErrorMessage="Confirm Password Required" ControlToValidate="txtConfirmPassword" >
</asp:RequiredFieldValidator> 
<asp:CompareValidator ID="CompareValidator2" runat="server" 
ErrorMessage="Passwords does not match" ControlToCompare="txtNewPassword"  
ControlToValidate="txtConfirmPassword" ValueToCompare="String"></asp:CompareValidator>

<asp:Button ID="btnUpdate" runat="server" Text="Update" 
 OnClick="btnUpdate_Click" CausesValidation="true"  Class="btn green_bg white_color border_radius_none height_49 margin_top_20 width_100 font_weight"/>

        <asp:TextBox ID="txtLoginName" runat="server" ReadOnly="true" CssClass="TextBoxStyle" Visible="false" /> 
     
 
   <asp:Label ID="lblPwdErrorMessage" runat="server" Class="lblErrorSkin" 
Visible="false"  style="color:Red" ></asp:Label>
 
      </div>
     
     <div  class="form-group">







     
     </div>
     </div>
      </div>
  
    </div>
    
    <div class="clear"></div>
  </div>
</div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Popupcontent" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
