<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="bs.aspx.cs" Inherits="WES.bs" %>

<%@ Register Src="UC/bs.ascx" TagName="bs" TagPrefix="uc1" %>
<%@ Register Src="~/UC/maincategory.ascx" TagName="maincat" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
<%--  <h3 id="h3_1" runat ="server" class="H3Tag"/> 
  <h3 id="h3_2" runat ="server" class="H3Tag"/>
  <h3 id="h3_3" runat ="server" class="H3Tag"/>--%>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
<uc2:maincat ID="maincategory" runat="server" />  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server"> 

<uc1:bs ID="bs1" runat="server" /></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server"></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>