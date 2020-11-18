<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="Cataloguedownload" Title="Untitled Page" Codebehind="Cataloguedownload.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UC/CatalogueDownload.ascx" TagName="CatalogueDownload" TagPrefix="uc1" %>
<%@ Register Src="UC/Newsupdate.ascx" TagName="Newsupdate" TagPrefix="uc1" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Import Namespace ="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">   
<%-- Moved to all_
    
    
    
    _master--%>
    <div>
         <uc1:CatalogueDownload ID="CatalogueDownload2" runat="server"   />       
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
