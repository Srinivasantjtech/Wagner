<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="ct" Title="Untitled Page" Codebehind="Microsite.aspx.cs" %>
<%@ Register Src="UC/categorylist.ascx" TagName="ct" TagPrefix="uc1" %>


<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<%--<h3 id="h3_1" runat ="server" class="H3Tag" /> 
<h3 id="h3_2" runat ="server" class="H3Tag"/>
<h3 id="h3_3" runat ="server" class="H3Tag"/>
<h2 id="h2" runat ="server" class="H2Tag"/>--%>
<%--<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>scripts/jquery-1.4.1.min.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" ></script>--%>
    <uc1:ct ID="Categorylist1" runat="server" />    
</asp:Content>

