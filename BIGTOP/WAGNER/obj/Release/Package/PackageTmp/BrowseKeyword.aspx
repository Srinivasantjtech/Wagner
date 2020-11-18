<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="BrowseKeyword.aspx.cs" Inherits="WES.BrowseKeyword" %>
<%@ Register src="~/UC/BrowseKeyword.ascx" tagname="BrowseKeyword" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
    <uc1:BrowseKeyword ID="BrowseKeyword1" runat="server" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
