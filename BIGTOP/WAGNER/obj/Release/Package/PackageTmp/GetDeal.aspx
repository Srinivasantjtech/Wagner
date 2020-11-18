<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="GetDeal" Title="GetDeal"  Culture ="auto:en-US" UICulture ="auto" Codebehind="GetDeal.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server"></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">
 
<asp:Panel ID ="pnlLogin" runat ="server" DefaultButton="btnSubscribe">
<div class="container-fluid">
<div class="row hidden-xs hidden-sm">
    	<div class="col-sm-20">
        	<ul id="mainbredcrumb" class="breadcrumb">
            	<li><a href="home.aspx">Home</a></li>
                <li class="active">Get Deal</li>   
            </ul>
        </div>
    </div>

    <div class="row">
<div class="categoryheading"> </div>
<div class="col-sm-20 mgntb40">
<img class="col-lg-8 col-sm-8 for_img" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/get_Deals.png">

<div class="col-lg-8 col-sm-8">
<h3><strong>Get Deal</strong></h3>
<div class="form-group">
            <label class="" for="exampleInputEmail3">Email address</label>
             <asp:TextBox  autocomplete="off"  ID="txtmail"  class="form-control border_radius_none height_49"  runat="server" ></asp:TextBox>
  
 <asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email<br/>" ControlToValidate="txtmail">
  </asp:RequiredFieldValidator> <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtmail" ErrorMessage="Required" Text="Enter Valid Email<br/>"
   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
   Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>

           
            <label class="" for="exampleInputEmail3">First Name</label>
            <asp:TextBox  autocomplete="off"  ID="txtFName" class="form-control border_radius_none height_49"  runat="server"  Font-Underline="False" ></asp:TextBox>
 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name<br/>" ControlToValidate="txtFName">

  </asp:RequiredFieldValidator>

            
            <label class="" for="exampleInputEmail3">Last Name</label>
            <asp:TextBox  autocomplete="off"  ID="txtLName" class="form-control border_radius_none height_49"  runat="server" ></asp:TextBox>
 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Class="vldRequiredSkin" ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name<br/>" ControlToValidate="txtLName">

  </asp:RequiredFieldValidator>
            
            <asp:Button ID="btnSubscribe" runat ="Server"  CausesValidation="true"   Text="Subscribe"  class="btn btn-primary margin_top" OnClick="btnSubscribe_Click"  ValidationGroup="Mandatory"/>
           
           
          </div>
          <span class="mandatory">  <asp:Label ID="lblError" class="lblErrorSkin" runat="server" Text=""></asp:Label></span>
</div>
</div>
<div class="clearfix"></div>
</div>

</div>

</asp:Panel></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server"></asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>
