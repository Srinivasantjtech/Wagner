<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="maboutus.aspx.cs" Inherits="WES.maboutus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<%--<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/thickboxAddtocart.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/thickboxaddtocart.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" ></script>--%>
<%--<script type="text/javascript" type="text/javascript">

    /*accordion for filter -End*/
    $(document).ready(function () {
        $("#footeraccordian h3").click(function () {
            //slide up all the link lists
            $("#footeraccordian ul ul").slideUp();
            //slide down the link list below the h3 clicked - only if its closed
            if (!$(this).next().is(":visible")) {
                $(this).next().slideDown();
            }
        })
    })


    function productbuy(buyvalue, pid) {
        try {


            var qtyval = 1;
            var qtyavail = 2;
            qtyavail = buyvalue.toString().split('_')[1];
            var minordqty = buyvalue.toString().split('_')[2];
            //          minordqty = minordqty.toString().split('_')[2];

            var fid = buyvalue.toString().split('_')[3];


            var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";


            if (isNaN(qtyval) || qtyval == "" || qtyval <= 0) {
                alert('Invalid Quantity!');
                //  window.document.forms[0].elements[buyvalue].style.borderColor = "red";
                //  document.forms[0].elements[buyvalue].focus();
                return false;
            }
            else {
                var tOrderID = '<%=Session["ORDER_ID"]%>';

                if (tOrderID != null && parseInt(tOrderID) > 0) {

                    CallProductPopup(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
                }
                else {

                    CallProductPopup(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
                }
            }
        }
        catch (e) {
            alert(e);
        }
    }

  </script>--%>
<% =ST_maboutus()%>
</asp:Content>
