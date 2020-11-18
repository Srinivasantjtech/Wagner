<%@ Page Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" Inherits="mLogin" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto"  Codebehind="mLogin.aspx.cs"   %>

<%@ Register Src="~/UC/MICROSITE/micrologin.ascx" TagName="micrologin" TagPrefix="uc1" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>--%>

<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">

 <%-- <div class="grid12 hidden-phone">
        <div class="mar5 breadcrumb">          
          Home &gt; Login
        </div>
      </div>--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="false" EnableCdn="true" EnablePageMethods="true" ScriptMode="Release"  EnablePartialRendering="false" >
        </asp:ScriptManager>
  
     <uc1:micrologin ID="LoginMS" runat="server" />
    
   
</asp:Content> 
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>