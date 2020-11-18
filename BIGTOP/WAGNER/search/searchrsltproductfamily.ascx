<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="search_searchrsltproductfamily" Codebehind="searchrsltproductfamily.ascx.cs" %>
<input id="HidItemPage" type="hidden" runat="server" />

       <div class="col-sm-20 col-md-20 clearfix">
<%
    string st_productList = ST_ProductListjson();
    if (st_productList != null && st_productList != string.Empty)
        Response.Write(st_productList);
    else
        if (Request.Url.OriginalString.ToString().ToUpper().Contains("PL.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("BB.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">Right now no products for sale from this category.</div>");
        else
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">No Products were found that match your selection.</div>");
%>
</div>
<script language="javascript" type="text/javascript">

    function GetSelectedIts() {
        var mySplitResult = "pppopt";
        var SelAttrStr = '';
        for (var j = 0; j < document.getElementById(mySplitResult).options.length; j++) {
            if (document.getElementById(mySplitResult).options[j].selected) {
                temp = document.getElementById(mySplitResult).options[j].value;
                document.getElementById("<%=HidItemPage.ClientID%>").value = temp;                
            }
        }       
        document.forms[0].submit();
    }
    function productbuy(buyvalue, pid) {
      
      //  alert(pid);
        //        var qtyval = document.forms[0].elements[buyvalue].value.trim();
        var qtyval = "1";
        //        var qtyavail = document.forms[0].elements[buyvalue].name;

       var qtyavail = buyvalue.toString().split('_')[1];
        var minordqty = buyvalue;
        minordqty = minordqty.toString().split('_')[2];

        var fid = buyvalue;
        fid = fid.toString().split('_')[3];

        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";
        alert(orgurl);

        if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 || qtyval.indexOf(".") != -1) {
            alert('Invalid Quantity!');
            window.document.forms[0].elements[buyvalue].style.borderColor = "red";
            document.forms[0].elements[buyvalue].focus();
            return false;
        }       
        else {
            var tOrderID = '<%=Session["ORDER_ID"]%>';
            if (tOrderID != null && parseInt(tOrderID) > 0) {
                // window.document.location = '/OrderDetails.aspx?&amp;bulkorder=1&amp;Pid=' + pid + '&amp;Qty=' + qtyval + "&amp;ORDER_ID=" + tOrderID;
                CallProductPopup(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
            }
            else {
                //window.document.location = '/OrderDetails.aspx?&amp;bulkorder=1&amp;Pid=' + pid + '&amp;Qty=' + qtyval;
                CallProductPopup(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
            }
        }
    }
    function keyct(e) {      
        var keyCode = (e.keyCode ? e.keyCode : e.which);
        if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

        }
        else {
            e.preventDefault();
        }
    }
</script>
<div style="display:none" >
<input type="text" name="eapath" id="htmleapath" runat="server" class="H1Tag"/>
<input type="text" name="bceapath" id="htmlbceapath" runat="server" class="H1Tag"/>
<input type="text" name="Totalpages" id="htmltotalpages" runat="server" class="H1Tag"/>
<input type="text" name="ViewMode" id="htmlviewmode" runat="server" class="H1Tag"/>
<input type="text" name="irecords" id="htmlirecords" runat="server" class="H1Tag"/>
</div>
<%--<asp:HiddenField ID="balrecords_cnt" runat="server" />--%>
