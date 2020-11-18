<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PowerSearchAndBM" Codebehind="PowerSearchAndBM.ascx.cs" %>
<input id="HidItemPage" type="hidden" runat="server" />
<input id="Hidcat" type="hidden" runat="server" />
<%string strval=null;
    if (Request.Url.OriginalString.ToString().ToUpper().Contains("MBB.ASPX"))
    { 
        strval = ST_BrandAndModelProductListNewJson();
    }
    else
        strval = ST_ProductListJson();
    if (strval != null && strval != string.Empty)
    {
        loadloader.Value = "true";
        if (strval != "CLEAR")
        {
            Response.Write(strval);
        }
    }
    else
    {
        if (Request.Url.OriginalString.ToString().ToUpper().Contains("MPL.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("MBB.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
        {
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">Right now no products for sale from this category.</div>");
            loadloader.Value = "false";
        }
        else
        {
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">No Products were found that match your selection.</div>");
            loadloader.Value = "false";
        }
    }
%>
<script language="javascript">
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
</script>
<script type="text/javascript">
        function __doPostBack(eventTarget, eventArgument) {
            document.getElementById("__EVENTTARGET").value = eventTarget;
            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            document.forms[0].submit();
        }
       

</script>

<input type="text" name="pseapath" id="htmlpseapath" runat="server" style="display:none;"/>
<input type="text" name="psbceapath" id="htmlpsbceapath" runat="server" style="display:none;"/>
<input type="text" name="psTotalpages" id="htmlpstotalpages" runat="server" style="display:none;"/>
<input type="text" name="eapath" id="htmleapath" runat="server" style="display:none;"/>
<input type="text" name="bceapath" id="htmlbceapth" runat="server" style="display:none;"/>
<input type="text" name="Totalpages" id="htmltotalpage" runat="server" style="display:none;"/>
<input type="text" name="ViewMode" id="htmlviewmode" runat="server" style="display:none;"/>
<input type="text" name="psViewMode" id="htmlpsviewmode" runat="server" style="display:none;"/>
<input type="text" name="irecords" id="htmlirecords" runat="server" style="display:none;"/>
<input type="text" name="psirecords" id="htmlpsirecords" runat="server" style="display:none;"/>

<input type="text" name="psloader" id="loadloader" value="false" runat="server" style="display:none;"/>