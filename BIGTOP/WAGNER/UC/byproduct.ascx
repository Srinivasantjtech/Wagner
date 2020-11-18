<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_byproduct" Codebehind="byproduct.ascx.cs" %>
<link href="search/templates/byproduct/searchrsltproducts/searchrsltproducts_files/base.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
        function __doPostBack(eventTarget, eventArgument) {
            document.getElementById("__EVENTTARGET").value = eventTarget;
            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            document.forms[0].submit();
        }
      
       
          
       
       
    </script>
<link href="search/templates/type1/searchrsltproducts/searchrsltproducts_files/base.css" type="text/css" rel="Stylesheet" />
    <input type="hidden" id="hdnFamilyId" runat="server">
    <input type="hidden" name="__EVENTTARGET" value="">
    <input type="hidden" name="__EVENTARGUMENT" value="">
    <table align="center" style="width: 558">
    <tr><td align="center"><table width="558" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">
        <tr><td align="left" class="tx_1">
            <a href="/Home.aspx" style="color:#0099FF" class="tx_3">Home</a> / <%
               // Response.Write(Bread_Crumbs());
                %>
          </td></tr>
        <tr><td width=100%><hr> </td></tr></table></td> </tr>
        <tr><td><table style=" width: 100%;"> <tr><td><div style="width: 100%;  solid #D1D1D1">
                                <table style="width: 100%"><tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td><div style="width: 100%">
                                            <%--<uc6:searchbyproduct ID="searchrbyproduct1" runat="server" />--%>
                                            </div>
                                            <div style="width: 100%">
                                               <%-- <uc2:searchrsltcategory ID="searchrsltcategory1" runat="server" />--%>
                                            </div>
                                            <%--<div style="width: 100%">
                                             <uc5:searchparametricfilter ID="searchparametricfilter1" runat="server" />
                                             </div>--%>
                                            <div style="width: 100%">
                                                <%--<uc4:searchrsltproducts ID="searchrsltproducts1" runat="server" />--%>
                                            </div></td></tr></table></div></td></tr>
                    <tr><td>&nbsp;</td> </tr></table></td></tr></table>