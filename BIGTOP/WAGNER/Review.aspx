<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="Review.aspx.cs" Inherits="WES.Review" %>
<asp:Content ID="Content1" ContentPlaceHolderID="metatag" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="server">
      <div class="home_heading"  style="margin-top:15px">&nbsp;&nbsp;&nbsp;Reviews</div>
    <div class="reviewmgr-stream" data-include-empty="false" data-review-limit="15" data-url="https://reviewr.app/wagneronline/"></div><script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "//platform.reviewmgr.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "reviewmgr-wjs");</script>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Popupcontent" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
