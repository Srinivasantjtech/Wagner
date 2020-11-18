<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="ProductDetails" Title="Untitled Page" Codebehind="pd.aspx.cs" %>
<%@ Register src="UC/Product.ascx" tagname="Product" tagprefix="uc1" %>
<%@ Register src="UC/moreproducts.ascx" tagname="moreproducts" tagprefix="uc2" %>
<%@ Register Src="~/UC/maincategory.ascx" TagName="maincat" TagPrefix="uc2" %>
<%--<%@ Register src="UC/RecentViewProducts.ascx" tagname="RecentViewProducts" tagprefix="uc3" %>--%>
    <asp:Content ID="Content7" ContentPlaceHolderID="metatag" Runat="Server">
         <asp:Literal runat="server" ID="litMeta" />
    </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%--<h3 id="h3_1" runat ="server" class="H3Tag"/> 
<h3 id="h3_2" runat ="server" class="H3Tag"/>
<h3 id="h3_3" runat ="server" class="H3Tag"/>
<h2 id="h2" runat ="server" class="H2Tag"/>
<h1 id="h1" runat ="server"  class="H1Tag"/>
<h4 id="h4" runat ="server"  class="H4Tag"/>
--%>
   
<%-- <meta id="ogprop" property="og:type" content="snapdeallog:item"  runat="server" class="metatagTag"/>
	<meta id="ogdescription" property="og:description" content="" runat="server" class="metatagTag"/>--%>
	<meta id="ogtitle" name="og_title" property="og:title" content="<%=productcode%>" runat="server" class="metatagTag"/>
<%--	<meta  name="ogsite_name" property="og:site_name" content="wagneronline.com.au" runat="server" class="metatagTag"/>
	<meta id="ogimage" name="og_image" runat="server" property="og:image" content="" class="metatagTag"/>
	<meta  name="og_url" runat="server" property="og:url" id="ogurl" content="" class="metatagTag" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
    <uc2:maincat ID="maincategory" runat="server" />  </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">

 <%--<uc3:RecentViewProducts ID="RecentViewProducts" runat="server" />--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
    <uc1:Product ID="Product1" runat="server" /></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server"></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server"></asp:Content>