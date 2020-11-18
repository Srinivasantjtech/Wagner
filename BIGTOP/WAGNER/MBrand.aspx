<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="MBrand.aspx.cs" Inherits="WES.MBrand" %>
<%@ Register Src="UC/MBrand.ascx" TagName="MBrand" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="metatag" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContent" runat="server">
        <uc1:MBrand ID="MBrand1" runat="server" />    
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Popupcontent" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
