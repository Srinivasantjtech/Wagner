<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="pl" Title="Untitled Page" Codebehind="pl.aspx.cs" %>
<%@ Register Src="UC/productlist.ascx" TagName="productlist" TagPrefix="uc1" %>
<%@ Register Src="~/UC/maincategory.ascx" TagName="maincat" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
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
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
<uc2:maincat ID="maincategory" runat="server" />  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server"> 

<uc1:productlist ID="Productlist1" runat="server" /></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server"></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>

