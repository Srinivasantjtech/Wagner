<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Toplog" Codebehind="Toplog.ascx.cs" %>
<%@ Register Src="CartItems.ascx" TagName="CartItems" TagPrefix="uc1" %>
<%@ Register Src="newproducts.ascx" TagName="newproducts" TagPrefix="uc1" %>

<% =ST_top()%>
<%--<%@ Import Namespace="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace="System.Diagnostics" %>--%>
<%--Script moved to all_js_master--%>
<%--<script type='text/javascript'>
    function init() {
        key_count_global = 0;
        var vartime = null;

        document.getElementById("srcfield").onkeypress = function (e) {
            if (e == undefined)
                e = event;

            if (e != null) {

                key_count_global++;
                var id = document.getElementById("PSearchDiv");
                id.innerHTML = '<div ><img src="images/Invloading.gif"/ ><span style="margin:0px;">Data Loading....</span></div>';
                id.style.display = "block";
                vartime = setTimeout("lookup(" + key_count_global + "," + e.keyCode + ")", 1000);
            }
        }
    }
    window.onload = init;

    function lookup(key_count, keyCode) {
        if (key_count == key_count_global) {
            if (keyCode != 13) {
                var SearchId = document.getElementById("srcfield")
                GetSearchProducts(SearchId)
            }
            else {
                urlredirect();
            }
        }

    }

</script>--%>
<%--<script>
    function GetSearchProducts(SearchId) {
        if (SearchId.value != "") {


            $.ajax({
                type: "POST",
                url: "GblWebMethods.aspx/GetSearchResultNew1",
                data: '{"Strvalue":"' + SearchId.value + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnajaxSuccess,
                error: OnajaxFailure


            });
        }
        else {
            var id = document.getElementById("PSearchDiv");
            toggleDiv(id, "none");
        }
    }



    function toggleDiv(Divid, dis) {
        Divid.style.display = dis;
    }

    function OnajaxSuccess(result) {

        var id = document.getElementById("PSearchDiv");
        if (result.d != "") {

            var sid = document.getElementById("srcfield");
            var s = result.d;
            id.innerHTML = result.d;

            toggleDiv(id, "block")
        }
        else {
            id.innerHTML = "No Result";
            toggleDiv(id, "block")
        }


    }
    function OnajaxFailure(result) {

        id.innerHTML = "Please try again later";
    }
    
  </script>--%>

<%--<%
    
    Stopwatch sw = new Stopwatch();
    sw.Start(); %>--%>




<%--<%
    sw.Stop();
    Security objErrorHandler = new Security();
    objErrorHandler.ExeTimelog = "ST_top_load = " + sw.Elapsed.TotalSeconds.ToString();
    objErrorHandler.createexecutiontmielog(); 
     %>--%>

<% if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("home.aspx") == true)
   { %>
<%--<div class="container-fluid zerospace">
	<div class="container">
    	<div class="main-banner animateOnScroll" 
data-animation-type="fadeInUp" data-timeout="0" data-offset-top="200">
        
             <a href="tv-brackets/ps/" >                 <img class="img-responsive" src="images/wagner-tv-brackets-banner-01.jpg"/>

             </a>

   
        </div>	
    </div>
</div>--%>
<uc1:newproducts ID="newproducts1" runat="server" />
<%} %>