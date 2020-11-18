<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="ct" Title="Untitled Page" Codebehind="ct.aspx.cs" %>
<%@ Register Src="UC/categorylist.ascx" TagName="ct" TagPrefix="uc1" %>

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
 
<%--<h3 id="h3_1" runat ="server" class="H3Tag" /> 
<h3 id="h3_2" runat ="server" class="H3Tag"/>
<h3 id="h3_3" runat ="server" class="H3Tag"/>
<h2 id="h2" runat ="server" class="H2Tag"/>--%>
<%--<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>scripts/jquery-1.4.1.min.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" ></script>--%>
    <uc1:ct ID="Categorylist1" runat="server" />  
    
    
</asp:Content>

