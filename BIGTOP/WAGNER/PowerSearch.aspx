<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="PowerSearchPage" Title="Untitled Page" Codebehind="ps.aspx.cs" %>
<%@ Register src="UC/searchctrl.ascx" tagname="searchctrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<script type = "text/javascript" language ="javascript"></script><uc1:searchctrl ID="searchctrl1" runat="server" />
</asp:Content><asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>

