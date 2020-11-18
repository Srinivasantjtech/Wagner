<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="NoItems.aspx.cs" Inherits="WES.NoItems" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
<div id="cart_item_holder" class="item_holder gb-scroll" tabindex="0" style="overflow: hidden; outline: none; height: 75px;">
<div class="mini-empty-cart"><h2>Shopping cart <span class="red">empty</span></h2><span class="empty-cart-icon"></span><p>
You have no items in your shopping cart<br><a onclick="/home.aspx"> Click here</a> to continue shopping</p></div></div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Popupcontent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
