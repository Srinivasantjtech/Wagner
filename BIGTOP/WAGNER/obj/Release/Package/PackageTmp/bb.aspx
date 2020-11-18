<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="bb" Title="Untitled Page" Codebehind="bb.aspx.cs" %>
<%@ Register src="UC/bybrand.ascx" tagname="bb" tagprefix="uc1" %>
<%@ Register Src="~/UC/maincategory.ascx" TagName="maincat" TagPrefix="uc2" %>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">

   <%-- <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CssScriptsSDUrl"].ToString()%>scripts/jquery.sidebarFix.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript"></script>
   <script type="text/javascript">
       $(window).load(function () {
           $('.jq_sidebar_fix').sidebarFix({
               frame: $('.middle')
           });
       });
        </script>--%>
<uc2:maincat ID="maincategory" runat="server" />  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">

    <uc1:bb ID="bybrand1" runat="server" />

    </asp:Content>