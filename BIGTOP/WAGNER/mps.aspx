<%@ Page Title="" Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true" CodeBehind="mps.aspx.cs" Inherits="WES.mps" %>
<%@ Register Src="UC/MICROSITE/searchctrlMS.ascx" TagName="searchctrlMS" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
           <div class=" col-lg-12 breadcrambs"> <%=Bread_Crumbs_MS(true) %></div>
             
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<%--      <div class="grid9">--%>

<%--<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/MicroSitecss/darktooltip.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
<link rel="Stylesheet" href="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>css/micrositecss/thickboxAddtocart_MS.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />--%>

<%--removed product by function--%>
        
      
        <uc1:searchctrlMS ID="searchctrlMS1" runat="server" />



<%--        </div>--%>
<%--<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>Scripts/micrositejs/thickboxAddtocart_MS.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" />
--%>
 

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">

<%--<script type="text/javascript">


    /*jQuery time*/
    $(document).ready(function () {

        $(".acch").click(function () {
            $(this).removeClass("hsmall");
            $(this).addClass("hbold");
        });

        $("#accordian h3").click(function () {
            $("#accordian ul ul").slideUp();
            //slide down the link list below the h3 clicked - only if its closed
            if (!$(this).next().is(":visible")) {
                $(this).next().slideDown();
            }
        })
    })
</script>
<script type="text/javascript">
    /*accordion for filter -Start*/
    /*jQuery time*/
    $(document).ready(function () {
        $("#filter h3").click(function () {
            //slide up all the link lists

            $("#filter ul ul").slideUp();
            //slide down the link list below the h3 clicked - only if its closed
            if (!$(this).next().is(":visible")) {
                $(this).next().slideDown();
            }
        })
    })
</script>

<script type="text/javascript">
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

</script>

<script language="javascript">
    function func_showChild(id) {
        $('.h3_1').css({ "color": "", "background": "" });
        $('.h3_2').css({ "color": "", "background": "" });
        $('.h3_3').css({ "color": "", "background": "" });
        $('.h3_4').css({ "color": "", "background": "" });
        $('.h3_5').css({ "color": "", "background": "" });
        $('.h3_6').css({ "color": "", "background": "" });
        $('.h3_7').css({ "color": "", "background": "" });
        $('.h3_8').css({ "color": "", "background": "" });
        $('.h3_9').css({ "color": "", "background": "" });
        $('.h3_' + id).css({ "font-weight": "bold", "color": "#fff", "background": "#099dd9 url(/images/MicroSiteimages/minus.png) 95% 10px no-repeat" });
    }
</script>--%>

<script language="javascript" type="text/javascript">
	 $('#list').click(function(){$('#products .item').addClass('list-group-item');});
	 $('#grid').click(function(){$('#products .item').removeClass('list-group-item');});
	
	 $("#list").click(function () {
    $(this).addClass('yellow');
	$('#grid').removeClass('yellow');
    $("#ctl00_MainContent_searchctrlMS1_PowerSearchAndBM1_htmlpsviewmode").val("LV");
    SetViewType("LV");
});
$('#grid').addClass('yellow');
	 $("#grid").click(function () {
    $(this).addClass('yellow');
	$('#list').removeClass('yellow');
    $("#ctl00_MainContent_searchctrlMS1_PowerSearchAndBM1_htmlpsviewmode").val("GV");
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
            url: "/mps.aspx/SetViewType",
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
            url: "/mps.aspx/SetSortOrder",
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
<%--<script language="javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/MicroSitejs/thegoods.js" type="text/javascript" />--%>
</asp:Content>