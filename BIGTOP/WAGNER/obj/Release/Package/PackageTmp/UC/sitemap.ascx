<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_sitemap" Codebehind="sitemap.ascx.cs" %>
<div class="container">
	<div class="row">
    	<div class="col-sm-20">
        	<ul class="breadcrumb" id="mainbredcrumb">
            	<li><a href="home.aspx">Home</a></li>
                <li class="active">Sitemap</li>
            </ul>
        </div>
    </div>
    <div class="row margin_btm_30">

  <div class="col-md-20 nolftpadd">
    <div class="row">  <% Response.Write(ST_Sitemap()); %>
    
    </div>
  </div>

</div>
</div>

<style type="text/css">
#loading {
    width: 100%;
    height: 100%;
    top: 0px;
    left: 0px;
    position: fixed;
    display: block;
    opacity: 0.7;
    background:#000 !important;
    z-index: 99;
    text-align: center;
}
#loadingimage
{
    margin-top:257px;
    /*  background: rgba( 255, 255, 255, .8 ) url('/images/ajax-loader1.gif') 50% 50% no-repeat;*/
}
#loadingtext
{
 color: #4682B4;
    font-family: Arial;
    font-size: 13px;
    font-weight: bolder;
    margin-left: 101px;
    margin-top: 46px;
}
#loading_anim {
    position:absolute;
   /* left:50%;
    top:50%;
    z-index:1010;*/
}


 .bom_container {
    /* position: absolute;*/
    /* width: 720px;
     height: 500px;*/
     
    /* border: 1px solid #000;*/
    width: 100%;
    height: 100%;
    top: 0px;
    left: 0px;
    position: fixed;
    display: block;
    opacity: 0.7;
    background:#000 !important;
    z-index: 99;
    text-align: center;
      width: 100%;
    height: 100%;
 }
</style>
<script type="text/javascript">

    $(function () {
        $(".withanimation").click(function (e) {
            e.preventDefault();
            $("#loading").show();

            var url = $(this).attr("href");

            setTimeout(function () {
                setTimeout(function () { showSpinner(); }, 30);
                window.location = url;
            }, 0);

        });
    });

    function showSpinner() {
        var opts = {
            lines: 15, // The number of lines to draw
            length: 3, // The length of each line
            width: 4, // The line thickness
            radius: 30, // The radius of the inner circle
            rotate: 0, // The rotation offset
            color: '#fff', // #rgb or #rrggbb
            speed: 2, // Rounds per second
            trail: 70, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            top: 'auto', // Top position relative to parent in px
            left: 'auto' // Left position relative to parent in px
        };
        $('#loading_anim').each(function () {
            spinner = new Spinner(opts).spin(this);
        });
    }



</script>

<%--<script type="text/javascript" src="../Scripts/1.4.4_jquery.min.js"></script>--%>
<script language="javascript" type="text/javascript">
    //    $(function () {
    //        $('#AutoSitemapxml').click(function () {
    //            $('#loading').show();
    //            $('#loading').css("visibility", "visible");
    //            $('#loading').append('<div id="loadingimage"><img src="/images/ajax-loader1.gif" alt="Loading..." /><p id="loadingtext">DATA LOADING PLEASE WAIT...</p></div>');
    //        });
    //    });

    $(function () {
        $('#AutoSitemapxml').click(function () {
            $('#bom_container').css("visibility", "visible");
            $('#bom_container1').css("visibility", "visible");
        });
    });


    //    $(function () {
    //        $('#AutoSitemapxml').click(function () {
    //            $('#loading').css("visibility", "visible");
    //        });
    //    });
</script>

