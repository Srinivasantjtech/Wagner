﻿<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="sitemap" Title="Untitled Page" Codebehind="sitemap.aspx.cs" %>
<%@ Register Src="~/UC/sitemap.ascx" TagName="sitemap" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<uc1:sitemap ID="sitemap1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server"></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>
