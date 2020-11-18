<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="fl" Title="Untitled Page" Codebehind="fl.aspx.cs" %>
<%@ Register src="UC/family.ascx" tagname="family" tagprefix="uc1" %>
<%@ Register Src="~/UC/maincategory.ascx" TagName="maincat" TagPrefix="uc2" %>
  <asp:Content ID="Content7" ContentPlaceHolderID="metatag" Runat="Server">
         <asp:Literal runat="server" ID="litMeta" />
    </asp:Content>
<asp:Content ID="family" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
<uc2:maincat ID="maincategory" runat="server" />  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<uc1:family ID="family1" runat="server" />

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

