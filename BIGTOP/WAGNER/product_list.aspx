<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="product_list" Title="Untitled Page" Codebehind="product_list.aspx.cs" %>
<%@ Register Src="UC/productlist.ascx" TagName="productlist" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
  <h3 id="h3_1" runat ="server" class="H3Tag"/> <h3 id="h3_2" runat ="server" class="H3Tag"/><h3 id="h3_3" runat ="server" class="H3Tag"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server"> <uc1:productlist ID="Productlist1" runat="server" /></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server"></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>

