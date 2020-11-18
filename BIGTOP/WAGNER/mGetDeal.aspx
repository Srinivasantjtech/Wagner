<%@ Page Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" Inherits="mGetDeal" Culture ="auto:en-US" UICulture ="auto" Codebehind="mGetDeal.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server"></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">
<div class="container margin_top">
  <div class="row">
  <div class=" col-lg-12 breadcrambs">Home <span class="padding_left_right_15"> &gt; </span> Get Deals</div>
    <div class="col-lg-10 margin_top_20 col-sm-12 col-lg-push-1">
      
      <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/MicroSiteimages/get_Deals.png" class="col-lg-5 col-sm-4 for_img margin_top_20"/>
      <div class="col-lg-7 margin_top_20 margin_bottom_30 col-sm-8">
      <h2> <p><strong>Get Deal</strong></p></h2>
   <div class="clear"></div>
     <form>
     <div class="form-group">
        <label for="exampleInputEmail3" >Email address</label>
        
         <asp:TextBox  autocomplete="off"   ID="txtmail" class="form-control border_radius_none height_49"  runat="server" ></asp:TextBox>
  
 <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email" ControlToValidate="txtmail">
  </asp:RequiredFieldValidator> <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtmail" ErrorMessage="Required" Text="Enter Valid Email"
   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
   Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
      </div>
      <div class="form-group">
        <label for="exampleInputEmail3" >First Name</label>
        
        <asp:TextBox  autocomplete="off"  ID="txtFName" class="form-control border_radius_none height_49"  runat="server"  Font-Underline="False" ></asp:TextBox>
 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name" ControlToValidate="txtFName">

  </asp:RequiredFieldValidator>
      </div>
     <div class="form-group">
        <label for="exampleInputEmail3" >Last Name</label>
       <asp:TextBox  autocomplete="off"  ID="txtLName" class="form-control border_radius_none height_49"  runat="server" ></asp:TextBox>
 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name" ControlToValidate="txtLName">

  </asp:RequiredFieldValidator>
        
        
      <asp:Button ID="btnSubscribe" runat ="Server"  CausesValidation="true"   Text="Subscribe" class="btn green_bg white_color border_radius_none height_49 margin_top_20 width_100 font_weight"  OnClick="btnSubscribe_Click"  ValidationGroup="Mandatory"/>
    <asp:Label ID="lblError" class="lblErrorSkin" runat="server" Text=""></asp:Label>
      </div>
     
     </form>
      </div>
    </div>
    
    <div class="clear"></div>
    <div id="divMayus" style="visibility:hidden; color: red;" align="center"><b>Caps Lock is on.</b></div>
  </div>
</div>


</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>
