<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="Login" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto"  Codebehind="Login.aspx.cs"   %>

<%@ Register src="UC/login.ascx" tagname="login" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
     
<uc1:login ID="login1" runat="server" />
</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>