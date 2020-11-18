<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="PowerSearchPage" Title="Untitled Page" Codebehind="ps.aspx.cs" %>
<%@ Register src="UC/searchctrl.ascx" tagname="searchctrl" tagprefix="uc1" %>
<%@ Register Src="~/UC/maincategory.ascx" TagName="maincat" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">

    <%-- <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CssScriptsSDUrl"].ToString()%>scripts/jquery.sidebarFix.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript"></script>
   <script type="text/javascript">
       $(window).load(function () {
           $('.jq_sidebar_fix').sidebarFix({
               frame: $('.middle')
           });
       });
        </script>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="leftnav" Runat="Server">
<uc2:maincat ID="maincategory" runat="server" />  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<script type = "text/javascript" language ="javascript"></script><uc1:searchctrl ID="searchctrl1" runat="server" />
</asp:Content><asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>

