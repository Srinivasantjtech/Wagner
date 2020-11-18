<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mpl.aspx.cs" Inherits="WES.mpl" %>
<%@ Register Src="UC/MICROSITE/ProductListMS.ascx" TagName="productlistMS" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
      <div class=" col-lg-12 breadcrambs"> <%=Bread_Crumbs_MS(true) %> </div>
                  <%//=ST_Top_filter() %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/darktooltip.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/micrositecss/thickboxAddtocart_MS.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />



<%--productbuy code moved to MS_Alljs--%>

   



    

      
        <uc1:productlistMS ID="Productlist1" runat="server" />


   <%-- <script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/micrositejs/thickboxAddtocart_MS.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" />
--%>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        var clsname = $("#demo1").attr('class');
        //alert(clsname);
        $("#demo1").removeClass(clsname);
        $("#demo1").addClass("collapse in");
        $("demo1").css({ 'height': 'auto' });
    });
</script>


<script language="javascript" type="text/javascript">
	 $('#list').click(function(){$('#products .item').addClass('list-group-item');});
	 $('#grid').click(function(){$('#products .item').removeClass('list-group-item');});

	 $("#list").click(function () {
    $(this).addClass('yellow');
	$('#grid').removeClass('yellow');
    $("#ctl00_MainContent_Productlist1_htmlviewmode").val("LV");
    SetViewType("LV");
});
$('#grid').addClass('yellow');
	 $("#grid").click(function () {
    $(this).addClass('yellow');
	$('#list').removeClass('yellow');
     $("#ctl00_MainContent_Productlist1_htmlviewmode").val("GV");
     SetViewType("GV");
});

     $('.navbar .dropdown').hover(function() {
    $(this).addClass('open').find('.dropdown-menu').first().stop(true, true).show();
    }, function() {
    $(this).removeClass('open').find('.dropdown-menu').first().stop(true, true).hide();
    });
</script>
<script language="javascript" type="text/javascript">

    function SetViewType(viewtype) {

        var type = viewtype;
        $.ajax({
            type: "POST",
            url: "/mpl.aspx/SetViewType",
            data: "{'viewtype':'" + type + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: false,
            success: OncartSuccess,
            error: OncartFailure

        });

    }

    function OnFailure(result) {
        alert("failure");
    }
    function OnSuccess(result) {
    }



    function SetSortOrder(SortOrder) {
        var url = window.location.href;
        var type = SortOrder;
        $.ajax({
            type: "POST",
            url: "/mpl.aspx/SetSortOrder",
            data: "{'orderVal':'" + SortOrder + "','url':'" + url + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {
                if (result.d == "1") {
                    window.location = url;
                }
            },
            error: function (result) {
                //rtn = false;
            }


        });

    }

</script>

  
 <script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/MicroSitejs/thegoods.js" type="text/javascript" />
</asp:Content>